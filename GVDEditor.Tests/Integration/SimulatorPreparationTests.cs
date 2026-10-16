using System.Diagnostics.CodeAnalysis;
using GVDEditor.Integration;
using TableSimulator.Contracts.Station;
using ToolsCore.Iniss.Registry;

namespace GVDEditor.Tests.Integration;

/// <summary>
/// Linky stanice pre simulator tabul bez zmeny .INI: presmerovana linka ide na port z .INI, ostatne na prvy port + cislo.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class SimulatorPreparationTests
{
    private static InissTable Table(int index, string key, TableManufacturer manufacturer, int port, int id) =>
        new(index, "Test.2025", new TablePhysical
        {
            Key = key, Name = key, ID = id, CommunicationPort = port, Rem = "", SaveXML = "", ReverseArrows = "", Comment = "",
            TableCatalog = new TableCatalog { Key = "K", Name = "K", Comment = "", Manufacturer = manufacturer }
        });

    private static readonly InissTable[] Tables = [Table(0, "ODCH1", TableManufacturer.Elen16, 3, 5), Table(1, "NAST1", TableManufacturer.Lcd1, 4, 12)];

    /// <summary>Linka 3 (ELEN) na COM a linka 4 (LCD), ktoru .INI presmerovava na simulator s portom 47104.</summary>
    private static (ResolvedConfig Config, InissIniFile Ini) Installation()
    {
        var machine = new RegBranch()
            .Set("Driver", "TableClass", RegRawValue.Dword(4)).Set("Driver", "TablePort", RegRawValue.String("3=COM3"))
            .Set("Driver0", "TableClass", RegRawValue.Dword(5)).Set("Driver0", "TablePort", RegRawValue.String("4=TCP://10.0.0.5:4001"));
        var original = RegResolver.Resolve(new InissConfigSource { AppName = "T", Version = new RegVersion(3, 39), Machine = machine });
        var ini = SimulatorRedirect.Update(original, null, new Dictionary<string, string> { ["Driver0"] = SimulatorRedirect.TablePort(4, "127.0.0.1", 47104) });
        var redirected = RegResolver.Resolve(new InissConfigSource { AppName = "T", Version = new RegVersion(3, 39), Machine = machine, Ini = ini });
        return (redirected, ini);
    }

    [TestMethod]
    public void Priprava_BezVyberuLenPresmerovaneLinkySPortomZIni()
    {
        var (config, ini) = Installation();

        var plan = SimulatorPreparation.Plan(config, ini, Tables, @"C:\INISS\Logy", [], 47000, null);

        var line = plan.Lines.Single();
        Assert.AreEqual((4, 47104, true), (line.Line.Line, line.Port, line.Redirected));
        Assert.AreEqual(new StationLineDto(4, 5, 47104, "Driver0"), plan.Request.Lines.Single());
        Assert.AreEqual("NAST1", plan.Request.Boards.Single().Name);
        Assert.AreEqual(@"C:\INISS\Logy", plan.Request.LogFolder);
        Assert.IsEmpty(plan.Missing);
    }

    [TestMethod]
    public void Priprava_VybraneLinkyANepresmerovaneNaPrvyPortPlusCislo()
    {
        var (config, ini) = Installation();

        var chosen = SimulatorPreparation.Plan(config, ini, Tables, null, [], 47000, [3, 4]);
        var withoutIni = SimulatorPreparation.Plan(config, null, Tables, null, [], 47000, null);
        var missing = SimulatorPreparation.Plan(config, ini, Tables, null, [], 47000, [3, 9]);

        CollectionAssert.AreEqual(new[] { (3, 47003, false), (4, 47104, true) },
            chosen.Lines.Select(l => (l.Line.Line!.Value, l.Port, l.Redirected)).ToArray());
        Assert.HasCount(2, chosen.Request.Boards);
        // bez znaciek presmerovania v .INI sa linka neberie ako presmerovana - vsetky aktivne linky na prvy port + cislo
        CollectionAssert.AreEqual(new[] { 47003, 47004 }, withoutIni.Lines.Select(l => l.Port).ToArray());
        CollectionAssert.AreEqual(new[] { 9 }, missing.Missing.ToArray());
        Assert.IsEmpty(missing.Lines);
    }
}
