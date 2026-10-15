using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using GVDEditor.Config;
using GVDEditor.Mcp;
using ModelContextProtocol.Client;

namespace GVDEditor.Tests.Mcp;

/// <summary>Server MCP v GVDEditore: len s klucom, len z tohto pocitaca, podla nastavenia.</summary>
[TestClass]
public sealed class GvdMcpServerTests
{
    public TestContext TestContext { get; set; } = null!;

    private static int FreePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        return ((IPEndPoint)listener.LocalEndpoint).Port;
    }

    private static McpServerSettings Enabled(int port) => new() { Enabled = true, Port = port, Key = McpServerSettings.NewKey() };

    private static EditorAccess Editor() => new(new EditorContext(null!), null);

    private async Task<HttpStatusCode> Initialize(string address, string? key, string? origin = null)
    {
        using var http = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, address)
        {
            Content = new StringContent(
                """{"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2025-11-25","capabilities":{},"clientInfo":{"name":"test","version":"1"}}}""",
                Encoding.UTF8, "application/json")
        };
        request.Headers.Accept.ParseAdd("application/json");
        request.Headers.Accept.ParseAdd("text/event-stream");
        if (key is not null)
            request.Headers.Authorization = new("Bearer", key);
        if (origin is not null)
            request.Headers.Add("Origin", origin);
        using var response = await http.SendAsync(request, TestContext.CancellationToken);
        return response.StatusCode;
    }

    [TestMethod]
    public async Task Server_LenSKlucomAZTohtoPocitaca()
    {
        using var server = new GvdMcpServer();
        var settings = Enabled(FreePort());
        await server.ApplyAsync(Editor(), settings);
        var address = server.Address!;

        Assert.AreEqual(settings.Address, address);
        Assert.AreEqual(HttpStatusCode.Unauthorized, await Initialize(address, null));
        Assert.AreEqual(HttpStatusCode.Unauthorized, await Initialize(address, "gvd_zly"));
        Assert.AreEqual(HttpStatusCode.Forbidden, await Initialize(address, settings.Key, "http://evil.example"));
        Assert.AreEqual(HttpStatusCode.OK, await Initialize(address, settings.Key));
    }

    [TestMethod]
    public async Task Server_NastrojGetGrafikonBezOtvorenehoGrafikonu()
    {
        using var server = new GvdMcpServer();
        var settings = Enabled(FreePort());
        await server.ApplyAsync(Editor(), settings);
        var transport = new HttpClientTransport(new HttpClientTransportOptions
        {
            Endpoint = new Uri(server.Address!),
            TransportMode = HttpTransportMode.StreamableHttp,
            AdditionalHeaders = new Dictionary<string, string> { ["Authorization"] = "Bearer " + settings.Key }
        });
        await using var client = await McpClient.CreateAsync(transport, cancellationToken: TestContext.CancellationToken);

        var result = await client.CallToolAsync("get_grafikon", cancellationToken: TestContext.CancellationToken);
        var grafikon = (JsonElement)result.StructuredContent!;

        Assert.AreEqual(JsonValueKind.Null, grafikon.GetProperty("folder").ValueKind);
        Assert.AreEqual(0, grafikon.GetProperty("trains").GetInt32());
    }

    [TestMethod]
    public async Task Server_NastrojeSoZmenamiLenSPovolenim()
    {
        async Task<List<string>> Tools(bool allowWrites)
        {
            using var server = new GvdMcpServer();
            var settings = Enabled(FreePort());
            settings.AllowWrites = allowWrites;
            await server.ApplyAsync(Editor(), settings);
            var transport = new HttpClientTransport(new HttpClientTransportOptions
            {
                Endpoint = new Uri(server.Address!),
                TransportMode = HttpTransportMode.StreamableHttp,
                AdditionalHeaders = new Dictionary<string, string> { ["Authorization"] = "Bearer " + settings.Key }
            });
            await using var client = await McpClient.CreateAsync(transport, cancellationToken: TestContext.CancellationToken);
            return (await client.ListToolsAsync(cancellationToken: TestContext.CancellationToken)).Select(t => t.Name).ToList();
        }

        var readOnly = await Tools(false);
        var writes = await Tools(true);

        CollectionAssert.Contains(readOnly, "get_grafikon");
        CollectionAssert.DoesNotContain(readOnly, "regenerate_board_texts");
        CollectionAssert.Contains(writes, "regenerate_board_texts");
        Assert.AreEqual(GvdMcpServer.ToolMethods().Count, writes.Count);
    }

    [TestMethod]
    public async Task Server_VypnutyAleboObsadenyPortNebezi()
    {
        using var server = new GvdMcpServer();
        await server.ApplyAsync(Editor(), new McpServerSettings { Enabled = false, Port = FreePort(), Key = McpServerSettings.NewKey() });
        Assert.IsNull(server.Address);
        Assert.IsNull(server.Error);

        using var busy = new TcpListener(IPAddress.Loopback, 0);
        busy.Start();
        await server.ApplyAsync(Editor(), Enabled(((IPEndPoint)busy.LocalEndpoint).Port));

        // GVDEditor bezi dalej - chyba je v stave servera
        Assert.IsNull(server.Address);
        Assert.IsNotNull(server.Error);
    }

    [TestMethod]
    public void Nastavenie_KlucSifrovanyVConfigu()
    {
        var settings = new McpServerSettings { Enabled = true, AllowWrites = true, Port = 5491 };
        var key = settings.EnsureKey();
        var serializer = new XmlSerializer(typeof(McpServerSettings));
        using var writer = new StringWriter();
        serializer.Serialize(writer, settings);
        using var reader = XmlReader.Create(new StringReader(writer.ToString()), new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null });
        var read = (McpServerSettings)serializer.Deserialize(reader)!;

        StringAssert.StartsWith(key, McpServerSettings.KeyPrefix);
        Assert.AreEqual(key, settings.EnsureKey(), "existujuci kluc sa nemeni");
        Assert.DoesNotContain(key, writer.ToString(), "kluc nie je v configu citatelny");
        Assert.AreEqual(key, read.Key);
        Assert.IsTrue(read.SameAs(settings));
        Assert.IsTrue(read.AllowWrites);
        Assert.IsFalse(read.SameAs(new McpServerSettings { Enabled = true, Port = 5491, ProtectedKey = settings.ProtectedKey }), "povolenie zmien = iny server");
        Assert.AreEqual("http://127.0.0.1:5491/mcp", read.Address);
        Assert.AreNotEqual(McpServerSettings.NewKey(), McpServerSettings.NewKey());
    }
}
