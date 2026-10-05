using System.Diagnostics.CodeAnalysis;
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

        Assert.AreEqual("""{"lines":[{"number":3,"tableClass":4,"port":47003,"name":"Driver"}],"boards":[{"line":3,"address":5,"manufacturer":"ELEN16","name":"ODCH1"}]}""", json);
    }

    [TestMethod]
    public void AdresaSimulatora_LenHttp()
    {
        Assert.IsNotNull(TableSimulatorClient.ParseUrl("http://localhost:5470"));
        Assert.IsNull(TableSimulatorClient.ParseUrl("localhost:5470"));
        Assert.IsNull(TableSimulatorClient.ParseUrl("ftp://h"));
    }
}
