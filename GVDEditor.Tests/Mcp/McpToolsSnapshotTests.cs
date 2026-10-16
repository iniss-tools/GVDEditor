using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using GVDEditor.Config;
using GVDEditor.Mcp;
using ModelContextProtocol;
using ModelContextProtocol.Client;

namespace GVDEditor.Tests.Mcp;

/// <summary>
/// Snimka <c>mcp-tools.json</c> - zmena toho, co model vidi (nazvy, popisy, schemy, priznaky nastrojov a pokyny servera),
/// musi byt v diffe.
/// </summary>
public sealed partial class McpToolsTests
{
    /// <summary>Premenna prostredia: <c>1</c> = snimku zapisat namiesto porovnania.</summary>
    private const string UpdateVariable = "GVDEDITOR_UPDATE_MCP";

    // snimku cita clovek - apostrofy a diakritika bez \uXXXX
    private static readonly JsonSerializerOptions Indented = new() { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

    private static string SnapshotPath([CallerFilePath] string thisFile = "") => Path.Combine(Path.GetDirectoryName(thisFile)!, "mcp-tools.json");

    /// <summary>Server s povolenymi zmenami alebo bez nich: pokyny a vsetky nastroje, ktore klient vidi.</summary>
    private async Task<(string? Instructions, IList<McpClientTool> Tools)> ListTools(bool allowWrites)
    {
        using var server = new GvdMcpServer();
        var settings = new McpServerSettings { Enabled = true, AllowWrites = allowWrites, Port = FreePort(), Key = McpServerSettings.NewKey() };
        await server.ApplyAsync(new EditorAccess(new EditorContext(null!), null), settings);
        var transport = new HttpClientTransport(new HttpClientTransportOptions
        {
            Endpoint = new Uri(server.Address!),
            TransportMode = HttpTransportMode.StreamableHttp,
            AdditionalHeaders = new Dictionary<string, string> { ["Authorization"] = "Bearer " + settings.Key }
        });
        await using var client = await McpClient.CreateAsync(transport, cancellationToken: TestContext.CancellationToken);
        return (client.ServerInstructions, await client.ListToolsAsync(cancellationToken: TestContext.CancellationToken));
    }

    [TestMethod]
    public async Task Nastroje_ZodpovedajuSnimke()
    {
        var (readOnlyInstructions, _) = await ListTools(false);
        var (instructions, tools) = await ListTools(true);
        var document = new JsonObject
        {
            ["instructions"] = instructions,
            ["instructionsWithoutChanges"] = readOnlyInstructions,
            ["tools"] = new JsonArray(tools.OrderBy(t => t.Name, StringComparer.Ordinal)
                .Select(t => JsonSerializer.SerializeToNode(t.ProtocolTool, McpJsonUtilities.DefaultOptions)).ToArray())
        };

        var path = SnapshotPath();
        if (Environment.GetEnvironmentVariable(UpdateVariable) == "1")
        {
            await File.WriteAllTextAsync(path, document.ToJsonString(Indented).ReplaceLineEndings("\n") + "\n", TestContext.CancellationToken);
            return;
        }

        Assert.IsTrue(File.Exists(path), $"chyba {path}");
        var snapshot = JsonNode.Parse(await File.ReadAllTextAsync(path, TestContext.CancellationToken));
        Assert.IsTrue(JsonNode.DeepEquals(document, snapshot), $"Mcp/mcp-tools.json nezodpoveda serveru - spustite test s {UpdateVariable}=1");
    }
}
