using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using GVDEditor.Domain.Entities;
using GVDEditor.Integration;
using TableSimulator.Contracts.Station;
using ToolsCore.Iniss.Registry;

namespace GVDEditor.Tests.Integration;

/// <summary>
/// Linky a tabule stanice pre simulator tabul pri presmerovani liniek INISSu.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TableSimulatorClientTests
{
    private static readonly JsonSerializerOptions Web = new(JsonSerializerDefaults.Web);

    private static InissTable Table(int index, string key, TableManufacturer manufacturer, int port, int id) =>
        new(index, "Test.2025", new TablePhysical
        {
            Key = key, Name = key, ID = id, CommunicationPort = port, Rem = "", SaveXML = "", ReverseArrows = "", Comment = "",
            TableCatalog = new TableCatalog { Key = "K", Name = "K", Comment = "", Manufacturer = manufacturer }
        });

    private static DriverLineMap Map()
    {
        var machine = new RegBranch()
            .Set("Driver", "TableClass", RegRawValue.Dword(4)).Set("Driver", "TablePort", RegRawValue.String("COM3"))
            .Set("Driver0", "TableClass", RegRawValue.Dword(5)).Set("Driver0", "TablePort", RegRawValue.String("4=TCP://10.0.0.5:4001"));
        var config = RegResolver.Resolve(new InissConfigSource { AppName = "T", Version = new RegVersion(3, 39), Machine = machine });
        return DriverLines.Build(config, [Table(0, "ODCH1", TableManufacturer.Elen16, 3, 5), Table(1, "NAST1", TableManufacturer.Lcd1, 4, 12)]);
    }

    [TestMethod]
    public void Stanica_LenPresmerovaneLinkySTabulami()
    {
        var station = SimulatorStation.From(Map(), new Dictionary<string, int> { ["Driver0"] = 47004 });

        Assert.AreEqual(new StationLineDto(4, 5, 47004, "Driver0"), station.Lines.Single());
        Assert.AreEqual(new StationBoardDto(4, 12, "LCD1", "NAST1"), station.Boards.Single());
    }

    [TestMethod]
    public void Stanica_KluceJsonPodlaApiSimulatora()
    {
        // ako PostAsJsonAsync (JsonSerializerDefaults.Web) - kluce cita server simulatora
        var json = JsonSerializer.Serialize(SimulatorStation.From(Map(), new Dictionary<string, int> { ["Driver"] = 47003 }), Web);

        Assert.AreEqual("""{"lines":[{"number":3,"tableClass":4,"port":47003,"name":"Driver"}],"boards":[{"line":3,"address":5,"manufacturer":"ELEN16","name":"ODCH1","flaps":null,"flapRecords":1}],"logFolder":null,"xmlTables":[]}""", json);
    }

    [TestMethod]
    public void Stanica_ListovaTabulaSoZoznamamiListov()
    {
        var machine = new RegBranch().Set("Driver", "TableClass", RegRawValue.Dword(3)).Set("Driver", "TablePort", RegRawValue.String("COM2"));
        var config = RegResolver.Resolve(new InissConfigSource { AppName = "T", Version = new RegVersion(3, 39), Machine = machine });
        var ciel = new TableTabTab { Key = "Ciel", Text = "A=BRATISLAVA\r\nB=*BRATISLAVA\r\n" };
        var catalog = new TableCatalog
        {
            Key = "F", Name = "F", Comment = "", Manufacturer = TableManufacturer.Fers, MaxRecCount = 6,
            Items =
            [
                new TableItem
                {
                    Key = "C", Name = "C", FillSection = TableFillSection.Free, Line = 0, Start = 0, End = 8, Align = TableAlign.Left,
                    DivType = TableDivType.Table, Tab1 = ciel, Tab2 = TableTabTab.Empty
                }
            ]
        };
        var table = new InissTable(0, "Test.2025", new TablePhysical
        {
            Key = "ODCH", Name = "ODCH", ID = 20, CommunicationPort = 2, Rem = "", SaveXML = "", ReverseArrows = "", Comment = "", TableCatalog = catalog
        });

        var board = SimulatorStation.From(DriverLines.Build(config, [table]), new Dictionary<string, int> { ["Driver"] = 47002 }).Boards.Single();

        Assert.AreEqual(6, board.FlapRecords);
        Assert.AreEqual(new TableSimulator.Contracts.Boards.FlapModuleDto(0, 0, 1, "Ciel"), board.Flaps!.Modules.Single());
        Assert.AreEqual("*BRATISLAVA", board.Flaps.Lists["Ciel"]["B"]);
    }

    [TestMethod]
    public void Stanica_TabuleSExportomDoXmlAPriecinokLogov()
    {
        InissTable Xml(int index, string key, string saveXml)
        {
            var table = Table(index, key, TableManufacturer.Elen, 0, -1);
            table.Table.SaveXML = saveXml;
            return table;
        }

        var tables = new[] { Xml(0, "Odchody web", "StanicaO"), Xml(1, "ODCH1", ""), Xml(2, "Odchody iny GVD", "stanicao"), Xml(3, "Príchody web", " StanicaP ") };

        var station = SimulatorStation.From(Map(), new Dictionary<string, int>(), tables, @"C:\INISS\Logy");

        Assert.AreEqual(@"C:\INISS\Logy", station.LogFolder);
        CollectionAssert.AreEqual(new[] { new StationXmlTableDto("Odchody web", "StanicaO"), new StationXmlTableDto("Príchody web", "StanicaP") },
            station.XmlTables.ToArray());
    }

    [TestMethod]
    public void Poziadavka_SKlucomHlavickaBearer()
    {
        var uri = new Uri("http://localhost:5470/api/state");
        using var withKey = TableSimulatorClient.Request(HttpMethod.Get, uri, " ts_kluc ");
        using var withoutKey = TableSimulatorClient.Request(HttpMethod.Get, uri, "");

        Assert.AreEqual("Bearer ts_kluc", withKey.Headers.Authorization?.ToString());
        Assert.IsNull(withoutKey.Headers.Authorization);
    }

    /// <summary>
    /// Server na jedno spojenie: precita hlavicky poziadavky a posle <paramref name="response" />; s certifikatom
    /// <paramref name="certificate" /> cez TLS. Vrati adresu servera.
    /// </summary>
    private static (Uri Url, Task Served) Serve(string response, X509Certificate2? certificate = null)
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        var served = Task.Run(async () =>
        {
            try
            {
                using var client = await listener.AcceptTcpClientAsync();
                Stream stream = client.GetStream();
                if (certificate is not null)
                {
                    var ssl = new SslStream(stream);
                    stream = ssl;
                    try
                    {
                        await ssl.AuthenticateAsServerAsync(certificate);
                    }
                    catch (Exception e) when (e is IOException or System.Security.Authentication.AuthenticationException)
                    {
                        // klient certifikat odmietol
                        return;
                    }
                }

                var buffer = new byte[4096];
                var request = "";
                while (!request.Contains("\r\n\r\n", StringComparison.Ordinal))
                {
                    var read = await stream.ReadAsync(buffer);
                    if (read == 0) return;
                    request += Encoding.ASCII.GetString(buffer, 0, read);
                }

                await stream.WriteAsync(Encoding.ASCII.GetBytes(response));
                await stream.FlushAsync();
            }
            catch (IOException)
            {
                // klient spojenie zrusil (pri TLS 1.3 odmietne certifikat az po handshaku)
            }
            finally
            {
                listener.Stop();
            }
        });
        return (new Uri($"{(certificate is null ? "http" : "https")}://localhost:{port}/"), served);
    }

    [TestMethod]
    public async Task Sonda_PresmerovanieNaHttps_AdresaHttps()
    {
        var (url, served) = Serve("HTTP/1.1 308 Permanent Redirect\r\nLocation: https://simulator:5471/api/auth/me\r\nContent-Length: 0\r\nConnection: close\r\n\r\n");

        var probe = await TableSimulatorClient.ProbeAsync(url, "kluc", TimeSpan.FromSeconds(5), CancellationToken.None);
        await served;

        Assert.AreEqual(SimulatorAccess.HttpsRequired, probe.Access);
        Assert.AreEqual(new Uri("https://simulator:5471/"), probe.HttpsUrl);
    }

    [TestMethod]
    public async Task Sonda_NedoveryhodnyCertifikat()
    {
        using var key = RSA.Create(2048);
        var request = new CertificateRequest("CN=localhost", key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        using var created = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1));
        // kluc z pamate Windows pri TLS nepouzije - cez PKCS#12
        using var certificate = X509CertificateLoader.LoadPkcs12(created.Export(X509ContentType.Pkcs12), null);
        var (url, served) = Serve("HTTP/1.1 200 OK\r\nContent-Length: 0\r\n\r\n", certificate);

        var probe = await TableSimulatorClient.ProbeAsync(url, null, TimeSpan.FromSeconds(5), CancellationToken.None);
        await served;

        Assert.AreEqual(SimulatorAccess.UntrustedCertificate, probe.Access);
    }

    [TestMethod]
    public void AdresaSimulatora_LenHttp()
    {
        Assert.IsNotNull(TableSimulatorClient.ParseUrl("http://localhost:5470"));
        Assert.IsNull(TableSimulatorClient.ParseUrl("localhost:5470"));
        Assert.IsNull(TableSimulatorClient.ParseUrl("ftp://h"));
    }
}
