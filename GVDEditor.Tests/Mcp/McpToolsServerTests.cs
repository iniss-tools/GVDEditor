using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using GVDEditor.Config;
using GVDEditor.Mcp;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace GVDEditor.Tests.Mcp;

/// <summary>
/// Kazdy nastroj cez server MCP nad grafikonom z <see cref="Editor(out EditorContext, bool, IAgentHost?)" />: vysledok
/// zodpoveda scheme vystupu, ktoru dostane klient (aj formaty datumu a casu - Claude Code ich kontroluje).
/// Novy nastroj treba pridat do <see cref="KazdyNastroj_VysledokPodlaSchemy" />.
/// </summary>
public sealed partial class McpToolsTests
{
    public TestContext TestContext { get; set; } = null!;

    /// <summary>Pripojeny klient a schemy vystupu nastrojov; zaznamenava volane nastroje.</summary>
    private sealed class Session(McpClient client, Dictionary<string, JsonElement?> schemas, TestContext test) : IAsyncDisposable
    {
        public HashSet<string> Called { get; } = [];

        public IReadOnlyCollection<string> Tools => schemas.Keys;

        public async Task<JsonElement> Call(string tool, Dictionary<string, object?>? arguments = null)
        {
            var result = await client.CallToolAsync(tool, arguments, cancellationToken: test.CancellationToken);
            Assert.IsFalse(result.IsError == true, $"{tool}: {Text(result)}");
            Called.Add(tool);
            // nastroj s textovym vysledkom (tabtab_reference) schemu vystupu nema
            if (schemas[tool] is not { } schema)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(Text(result)), $"{tool}: prazdny vysledok");
                return default;
            }

            var content = result.StructuredContent!.Value;
            var errors = OutputSchema.Validate(content, schema);
            Assert.IsEmpty(errors, $"{tool}: {string.Join("; ", errors)}");
            return content;
        }

        /// <summary>Chyba nastroja - text, ktory dostane model.</summary>
        public async Task<string> Error(string tool, Dictionary<string, object?>? arguments = null)
        {
            var result = await client.CallToolAsync(tool, arguments, cancellationToken: test.CancellationToken);
            Assert.IsTrue(result.IsError, $"{tool} mal skoncit chybou: {Text(result)}");
            Called.Add(tool);
            return Text(result);
        }

        private static string Text(CallToolResult result) => string.Join(" ", result.Content.OfType<TextContentBlock>().Select(t => t.Text));

        public ValueTask DisposeAsync() => client.DisposeAsync();
    }

    private static int FreePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        return ((IPEndPoint)listener.LocalEndpoint).Port;
    }

    [TestMethod]
    public async Task KazdyNastroj_VysledokPodlaSchemy()
    {
        // analyza cita subory instalacie - grafikon z testu dostane prazdny priecinok
        var folder = Directory.CreateTempSubdirectory("GvdMcpTools").FullName;
        try
        {
            await CallEveryTool(folder);
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    private async Task CallEveryTool(string folder)
    {
        using var server = new GvdMcpServer();
        var settings = new McpServerSettings { Enabled = true, AllowWrites = true, Port = FreePort(), Key = McpServerSettings.NewKey() };
        var editor = Editor(out var context);
        context.Workspace.INISSDir = context.Workspace.DataDir = folder;
        context.Directory!.Dir.FullPath = folder;
        await server.ApplyAsync(editor, settings);
        Assert.IsNull(server.Error, server.Error);
        var transport = new HttpClientTransport(new HttpClientTransportOptions
        {
            Endpoint = new Uri(server.Address!),
            TransportMode = HttpTransportMode.StreamableHttp,
            AdditionalHeaders = new Dictionary<string, string> { ["Authorization"] = "Bearer " + settings.Key }
        });
        var client = await McpClient.CreateAsync(transport, cancellationToken: TestContext.CancellationToken);
        var tools = await client.ListToolsAsync(cancellationToken: TestContext.CancellationToken);
        await using var mcp = new Session(client, tools.ToDictionary(t => t.Name, t => t.ProtocolTool.OutputSchema), TestContext);

        // citanie
        await mcp.Call("get_grafikon");
        await mcp.Call("list_grafikons");
        await mcp.Call("get_station");
        var trains = await mcp.Call("list_trains", new() { ["day"] = "2026-12-19" });
        Assert.AreEqual(3, trains.GetProperty("total").GetInt32());
        await mcp.Call("get_train", new() { ["id"] = 2 });
        await mcp.Call("explain_validity", new() { ["text"] = "jazdí v 6" });
        await mcp.Call("list_tables");
        await mcp.Call("get_table", new() { ["kind"] = "catalog", ["key"] = "K16" });
        await mcp.Call("table_options");
        await mcp.Call("get_tabtab", new() { ["key"] = "DRUH" });
        await mcp.Call("preview_board_text", new() { ["train"] = 2, ["section"] = "druh" });
        await mcp.Call("tabtab_reference");
        await mcp.Call("analyze_grafikon");

        // zmeny
        await mcp.Call("regenerate_board_texts", new() { ["onlyMissing"] = true });
        await mcp.Call("set_font", new() { ["id"] = 81, ["name"] = "Tenké červené" });
        await mcp.Call("set_catalog_table", new()
        {
            ["key"] = "K20",
            ["copyFrom"] = "K16",
            // kopia nesmie mat chyby, ktore ma predloha z testu (bez vyrobcu, stlpec bez TAB1)
            ["manufacturer"] = "ELEN16",
            ["columns"] = new[]
            {
                new Dictionary<string, object?> { ["key"] = "Druh", ["name"] = "Druh", ["start"] = 0, ["end"] = 48, ["font"] = 0, ["content"] = 15, ["divType"] = 3, ["tab1"] = "DRUH" },
                new Dictionary<string, object?> { ["key"] = "Ciel", ["name"] = "Cieľ", ["start"] = 48, ["end"] = 200, ["font"] = 0, ["content"] = 8 }
            },
            ["columnOrder"] = new[] { new Dictionary<string, object?> { ["viewType"] = "Tabule_Odjezdova", ["columns"] = new[] { "Druh", "Ciel" } } }
        });
        await mcp.Call("set_physical_table", new() { ["key"] = "T2", ["catalog"] = "K20", ["address"] = 5, ["line"] = 2 });
        await mcp.Call("set_logical_table", new()
        {
            ["key"] = "L1",
            ["records"] = 2,
            ["layout"] = new[] { new Dictionary<string, object?> { ["physical"] = "T2", ["firstRecord"] = 1, ["lastRecord"] = 2 } }
        });
        // kolaje grafikonu z testu su zdielane - zmena len nanecisto
        var track = await mcp.Call("set_track_tables", new() { ["track"] = "2", ["tables"] = new[] { "L1" }, ["dryRun"] = true });
        Assert.IsTrue(track.GetProperty("change").GetProperty("dryRun").GetBoolean());
        await mcp.Call("get_table", new() { ["kind"] = "logical", ["key"] = "L1" });
        await mcp.Call("delete_table", new() { ["kind"] = "font", ["key"] = "81" });
        await mcp.Call("set_train", new()
        {
            ["number"] = "3005",
            ["type"] = "Os",
            ["operator"] = "ZSSK",
            ["departure"] = "14:15",
            ["stationsAfter"] = new[] { new Dictionary<string, object?> { ["station"] = "Brezová Dolina", ["short"] = true } }
        });
        await mcp.Call("set_train", new() { ["id"] = 1, ["departure"] = "10:20:00", ["track"] = "2" });
        await mcp.Call("set_tabtab", new() { ["key"] = "NOVA", ["text"] = "R=R" });
        await mcp.Call("delete_tabtab", new() { ["key"] = "NOVA" });
        await mcp.Call("delete_train", new() { ["id"] = 4 });
        // bez konfiguracie spustania sa simulator pripravit neda - nastroj musi skoncit zrozumitelnou chybou
        StringAssert.Contains(await mcp.Error("prepare_simulator", new() { ["dryRun"] = true }), "iniss.noConfiguration");

        Assert.HasCount(3, context.Document.Trains, "nove vlaky sa zmazali");
        CollectionAssert.AreEquivalent(mcp.Tools.ToArray(), mcp.Called.ToArray(), "kazdy nastroj sa v teste vola");
    }
}
