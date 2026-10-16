using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using GVDEditor.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Mcp;

/// <summary>
/// Server MCP vnutri GVDEditora (Streamable HTTP bez relacie) - agent cita otvoreny grafikon aj s neulozenymi zmenami.
/// Pocuva len na tomto pocitaci, vyzaduje pristupovy kluc a odmieta poziadavky z cudzej stranky v prehliadaci.
/// </summary>
internal sealed class GvdMcpServer : IDisposable
{
    public const string Pattern = "/mcp";

    private const string Bearer = "Bearer ";

    /// <summary>Pokyny pre agenta pri pripojeni.</summary>
    private const string Instructions =
        "GVDEditor edits timetables (grafikon) of INISS, the Slovak and Czech railway station information system (boards and announcements). "
        + "Tools read the timetable open in the running editor including unsaved changes; list_grafikons shows the other timetables of the "
        + "installation. Train data use Slovak terms: routing originates/passes/terminates, validity is a calendar restriction text "
        + "(explain_validity expands it to days), TabTab is the INISS language deciding what boards show - read tabtab_reference before explaining it and do not guess its "
        + "semantics.";

    /// <summary>Pokyny podla povolenia zmien.</summary>
    private const string ReadOnlyInstructions = " Changes are made by the user in GVDEditor.";

    private const string WriteInstructions =
        " Tools that change the timetable work in the editor's memory only: each call is one undo step the user can revert in GVDEditor, nothing is "
        + "saved - the user reviews the changes and saves the timetable. Call a change with dryRun first when its effect is not obvious.";

    /// <summary>Vysledky nastrojov aj s null - schema vlastnosti vyzaduje (klient by vysledok bez nich odmietol).</summary>
    private static readonly JsonSerializerOptions ToolJson = new(McpJsonUtilities.DefaultOptions) { DefaultIgnoreCondition = JsonIgnoreCondition.Never };

    /// <summary>
    /// Schemy nastrojov: <see cref="TimeOnly" /> a <see cref="DateTime" /> sa zapisu bez casoveho pasma, co formaty
    /// <c>time</c> a <c>date-time</c> (RFC 3339) nepripustaju - klient by vysledok odmietol, preto bez formatu.
    /// </summary>
    private static readonly AIJsonSchemaCreateOptions ToolSchemas = new()
    {
        TransformSchemaNode = (context, node) =>
        {
            var type = Nullable.GetUnderlyingType(context.TypeInfo.Type) ?? context.TypeInfo.Type;
            if ((type == typeof(TimeOnly) || type == typeof(DateTime)) && node is JsonObject schema)
                schema.Remove("format");
            return node;
        }
    };

    private WebApplication? _app;

    /// <summary>Adresa beziaceho servera; null, ak nebezi.</summary>
    public string? Address { get; private set; }

    /// <summary>Preco sa server posledny raz nespustil (napr. obsadeny port); null = bez chyby.</summary>
    public string? Error { get; private set; }

    /// <summary>Nastavenie, s ktorym server bezi (alebo sa ho pokusil spustit).</summary>
    public McpServerSettings? Settings { get; private set; }

    /// <summary>
    /// Server podla <paramref name="settings" />: zastavi beziaci a pri zapnutom nastaveni spusti novy. Chybu nehadze -
    /// je v <see cref="Error" /> (GVDEditor bez servera funguje dalej).
    /// </summary>
    public async Task ApplyAsync(EditorAccess editor, McpServerSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        await StopAsync().ConfigureAwait(false);
        Settings = settings.Clone();
        Error = null;
        if (!settings.Enabled)
            return;
        var key = settings.Key;
        if (key.Length == 0)
        {
            Error = "no access key";
            return;
        }

        try
        {
            // zostavenie a start servera mimo vlakna okna - pri starte GVDEditora by ho inak na chvilu zablokovali
            _app = await Task.Run(() => StartAsync(editor, settings.Port, key, settings.AllowWrites)).ConfigureAwait(false);
            Address = settings.Address;
            Log.Info("MCP: server bezi na " + Address);
        }
        catch (Exception e) when (e is IOException or InvalidOperationException or SocketException)
        {
            Error = e.Message;
            Log.Exception(e, "MCP: server sa nespustil");
        }
    }

    private static async Task<WebApplication> StartAsync(EditorAccess editor, int port, string key, bool allowWrites)
    {
        var builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions { ContentRootPath = AppContext.BaseDirectory });
        builder.WebHost.ConfigureKestrel(options => options.Listen(IPAddress.Loopback, port));
        builder.Logging.ClearProviders();
        builder.Services.AddSingleton(editor);
        builder.Services.AddMcpServer(options =>
            {
                options.ServerInfo = new Implementation { Name = "GVDEditor", Version = Application.ProductVersion };
                options.ServerInstructions = Instructions + (allowWrites ? WriteInstructions : ReadOnlyInstructions);
            })
            .WithHttpTransport(options => options.Stateless = true);
        // nastroje su internal - SDK ich zo zostavy samo nenajde; nastroje so zmenami len s povolenim (klient ich bez neho nevidi)
        foreach (var method in ToolMethods().Where(m => allowWrites || IsReadOnly(m)))
            builder.Services.AddSingleton(provider => McpServerTool.Create(method, (object?)null, new McpServerToolCreateOptions { Services = provider, SerializerOptions = ToolJson, SchemaCreateOptions = ToolSchemas }));

        var app = builder.Build();
        var expected = Encoding.UTF8.GetBytes(key);
        app.MapMcp(Pattern).Add(endpoint =>
        {
            var inner = endpoint.RequestDelegate ?? throw new InvalidOperationException("MCP endpoint has no request delegate.");
            endpoint.RequestDelegate = context =>
            {
                if (!IsSameOrigin(context.Request))
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                else if (!HasKey(context.Request, expected))
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                else
                    return inner(context);
                return Task.CompletedTask;
            };
        });
        try
        {
            await app.StartAsync().ConfigureAwait(false);
        }
        catch
        {
            await app.DisposeAsync().ConfigureAwait(false);
            throw;
        }
        return app;
    }

    /// <summary>Staticke metody nastrojov z tried <see cref="McpServerToolTypeAttribute" /> v GVDEditore.</summary>
    public static IReadOnlyList<MethodInfo> ToolMethods() =>
        typeof(GvdMcpServer).Assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<McpServerToolTypeAttribute>() is not null)
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .Where(m => m.GetCustomAttribute<McpServerToolAttribute>() is not null)
            .ToList();

    /// <summary>Nastroj grafikon nemeni.</summary>
    public static bool IsReadOnly(MethodInfo method) => method.GetCustomAttribute<McpServerToolAttribute>()?.ReadOnly == true;

    /// <summary>Hlavicka <c>Authorization: Bearer</c> s klucom - porovnanie v konstantnom case.</summary>
    private static bool HasKey(HttpRequest request, byte[] expected)
    {
        var header = request.Headers.Authorization.ToString();
        return header.StartsWith(Bearer, StringComparison.OrdinalIgnoreCase)
               && CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(header[Bearer.Length..].Trim()), expected);
    }

    /// <summary>Bez hlavicky Origin (nie prehliadac) alebo z rovnakeho servera - ochrana pred DNS rebinding.</summary>
    private static bool IsSameOrigin(HttpRequest request)
    {
        var origin = request.Headers.Origin.ToString();
        return origin.Length == 0
               || Uri.TryCreate(origin, UriKind.Absolute, out var uri) && string.Equals(uri.Authority, request.Host.Value, StringComparison.OrdinalIgnoreCase);
    }

    private async Task StopAsync()
    {
        if (_app is not { } app)
            return;
        _app = null;
        Address = null;
        await app.DisposeAsync().ConfigureAwait(false);
    }

    public void Dispose() =>
        // pri ukonceni programu - na vlakne okna sa na zastavenie neda cakat cez await
        Task.Run(StopAsync).Wait(TimeSpan.FromSeconds(2));
}
