using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Integration;
using ToolsCore.Iniss.Registry;

namespace GVDEditor.Tests.Domain.Analysis;

/// <summary>
/// Kontroly nastaveni INISSu voci grafikonu - linky tabul, vypnute tabule, nastavenia tabul mimo dat, nazvy suborov.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class InissChecksTests
{
    private static InissTable Table(int index, string grafikon, string key, int port) =>
        new(index, grafikon, new TablePhysical
        {
            Key = key, Name = key, ID = 1, CommunicationPort = port, Rem = "", SaveXML = "", ReverseArrows = "", Comment = "",
            TableCatalog = new TableCatalog { Key = "K", Name = "K", Comment = "", Manufacturer = TableManufacturer.Elen }
        });

    private static InissRegistryView View(RegBranch machine, params InissTable[] tables)
    {
        var config = RegResolver.Resolve(new InissConfigSource
        {
            AppName = "INISS", Version = new RegVersion(3, 39), Machine = machine, Tables = tables.ToDictionary(t => t.Index, t => t.ToInfo())
        });
        return new InissRegistryView("Test", config, tables, true);
    }

    // posielanie na tabule je v INISSe predvolene vypnute
    private static RegBranch Machine() => new RegBranch().Set("Environment", "OutToTableDriver", RegRawValue.Dword(1));

    private static string[] Keys(List<InissFinding> findings) => findings.Select(f => f.Key).ToArray();

    [TestMethod]
    public void BeznaKonfiguracia_BezZisteni()
    {
        var machine = Machine().Set("Driver", "TableClass", RegRawValue.Dword(4)).Set("Driver", "TablePort", RegRawValue.String("COM3"));

        Assert.IsEmpty(InissChecks.Evaluate("A", View(machine, Table(0, "A", "T1", 3))));
    }

    [TestMethod]
    public void TabulaBezLinky_LenTabuleTohtoGrafikonu()
    {
        var view = View(Machine().Set("Driver", "TableClass", RegRawValue.Dword(4)).Set("Driver", "TablePort", RegRawValue.String("COM3")),
            Table(0, "A", "T1", 5), Table(1, "B", "T2", 5));

        var findings = InissChecks.Evaluate("A", view);

        CollectionAssert.AreEqual(new[] { "unserved:0" }, Keys(findings));
        StringAssert.Contains(findings[0].Text, "T1");
    }

    [TestMethod]
    public void VypnutePosielanie_NahradiKontroluLiniek()
    {
        var view = View(new RegBranch().Set("Environment", "OutToTableDriver", RegRawValue.Dword(0)), Table(0, "A", "T1", 5));

        CollectionAssert.AreEqual(new[] { "output" }, Keys(InissChecks.Evaluate("A", view)));
    }

    [TestMethod]
    public void DuplicitnaLinka_AVypnutaTabula()
    {
        var machine = Machine()
            .Set("Driver", "TableClass", RegRawValue.Dword(4)).Set("Driver", "TablePort", RegRawValue.String("COM3"))
            .Set("Driver0", "TableClass", RegRawValue.Dword(4)).Set("Driver0", "TablePort", RegRawValue.String("3=TCP://h:1"))
            .Set("Tables", "Enabled0", RegRawValue.Dword(0));

        var findings = InissChecks.Evaluate("A", View(machine, Table(0, "A", "T1", 3)));

        Assert.IsTrue(findings.Any(f => f.Key.StartsWith("line:Driver0", StringComparison.Ordinal) && f.Type == ProblemType.Error));
        Assert.IsTrue(findings.Any(f => f.Key == "disabled:0" && f.Type == ProblemType.Hint));
    }

    [TestMethod]
    public void NastaveniaTabulMimoDat_Informacia()
    {
        var machine = Machine()
            .Set("Driver", "TableClass", RegRawValue.Dword(4)).Set("Driver", "TablePort", RegRawValue.String("COM3"))
            .Set("Tables", "Enabled5", RegRawValue.Dword(1)).Set("Tables", "DayLight7", RegRawValue.Dword(3));

        var findings = InissChecks.Evaluate("A", View(machine, Table(0, "A", "T1", 3)));

        var orphans = findings.Single();
        Assert.AreEqual("orphans", orphans.Key);
        StringAssert.Contains(orphans.Text, "5, 7");
    }

    [TestMethod]
    public void Rozsahy_SuvisleCislaSkratene() => Assert.AreEqual("1, 4–7, 9, 10", InissChecks.Ranges([1, 4, 5, 6, 7, 9, 10]));

    [TestMethod]
    public void InyNazovSuboru_VarovaniePodlaVelkostiPismenNie()
    {
        var machine = new RegBranch()
            .Set("PathNames", "GVVlaky", RegRawValue.String("VLAKY2.TXT"))
            .Set("PathNames", "GVGrafikon", RegRawValue.String("grafikon.txt"));

        var findings = InissChecks.Evaluate("A", View(machine));

        CollectionAssert.AreEqual(new[] { "file:GVVlaky" }, Keys(findings));
        Assert.AreEqual("PathNames", findings[0].Section);
    }

    [TestMethod]
    public void PresmerovanieNaSimulator_Upozornenie()
    {
        var machine = Machine().Set("Driver", "TableClass", RegRawValue.Dword(4)).Set("Driver", "TablePort", RegRawValue.String("COM3"));
        var tables = new[] { Table(0, "A", "T1", 3) };
        var before = View(machine, tables);
        var ini = SimulatorRedirect.Update(before.Config, null, new Dictionary<string, string> { ["Driver"] = "3=TCP://127.0.0.1:47003" });
        var config = RegResolver.Resolve(new InissConfigSource
        {
            AppName = "INISS", Version = new RegVersion(3, 39), Machine = machine, Ini = ini, Tables = tables.ToDictionary(t => t.Index, t => t.ToInfo())
        });

        var finding = InissChecks.Evaluate("A", before with { Config = config }).Single();

        Assert.AreEqual("redirected", finding.Key);
        Assert.AreEqual("Driver", finding.Section);
        Assert.IsEmpty(InissChecks.Evaluate("B", before with { Config = config }), "grafikon bez tabul na linke");
    }

    [TestMethod]
    public void BezNastaveni_LenInformacia()
    {
        var view = View(new RegBranch(), Table(0, "A", "T1", 5)) with { Exists = false };

        var finding = InissChecks.Evaluate("A", view).Single();

        Assert.AreEqual("missing", finding.Key);
        Assert.AreEqual(ProblemType.Hint, finding.Type);
    }
}
