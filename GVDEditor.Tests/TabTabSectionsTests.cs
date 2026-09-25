using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using GVDEditor.Tools;
using ToolsCore.TabTab;
using ToolsCore.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Sprava sekcii TabTab: kontrola nazvu pri pridani/premenovani, hladanie pouzitia pred odstranenim
///     a formatovanie textu sekcie (nesmie menit text posielany na tabulu).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TabTabSectionsTests
{
    private static TableItem Column(string key, TableTabTab tab1, TableTabTab tab2) => new()
    {
        Key = key, Name = key, FillSection = TableFillSection.Free, Align = TableAlign.Left,
        DivType = TableDivType.Free, Tab1 = tab1, Tab2 = tab2, Start = 0, End = 8, FontIDX = 81
    };

    // ---- nazov sekcie ----

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("Smer]")]
    [DataRow("Sm\ter")]
    [DataRow("Сме")]
    [DataRow("Žiadny")]
    [DataRow("ŽIADNY")]
    public void Nazov_NeplatnyJeOdmietnuty(string name)
    {
        Assert.IsNotNull(TabTabSections.ValidateName(name, ["Druh"], out _));
    }

    [TestMethod]
    [DataRow("Druh")]
    [DataRow("druh")]
    [DataRow(" Druh ")]
    public void Nazov_DuplicitnyJeOdmietnutyBezOhladuNaVelkostPismen(string name)
    {
        var error = TabTabSections.ValidateName(name, ["Smer", "Druh"], out _);

        Assert.IsNotNull(error);
        StringAssert.Contains(error, "Druh");
    }

    [TestMethod]
    public void Nazov_PlatnyJeOrezany()
    {
        Assert.IsNull(TabTabSections.ValidateName("  Směr2 ", ["Druh", "Smer"], out var normalized));
        Assert.AreEqual("Směr2", normalized);
    }

    // ---- pouzitie sekcie ----

    [TestMethod]
    public void Pouzitie_NajdeVsetkyStlpceTab1ITab2()
    {
        var druh = new TableTabTab { Key = "Druh", Text = "R{81}=R" };
        var smer = new TableTabTab { Key = "Smer", Text = "" };
        var catalog = new TableCatalog { Key = "Kat", Name = "Odjezdy" };
        catalog.Items.AddRange([Column("Druh", druh, TableTabTab.Empty), Column("Smer", smer, druh), Column("Cas", TableTabTab.Empty, TableTabTab.Empty)]);

        var usage = TabTabSections.FindUsage(druh, [catalog]);

        Assert.HasCount(2, usage);
        Assert.IsTrue(usage.All(u => u.Contains("Odjezdy", StringComparison.Ordinal)));
        Assert.IsTrue(usage[0].Contains("TAB1", StringComparison.Ordinal));
        Assert.IsTrue(usage[1].Contains("TAB2", StringComparison.Ordinal));
        Assert.IsEmpty(TabTabSections.FindUsage(new TableTabTab { Key = "Iny" }, [catalog]));
        Assert.IsEmpty(TabTabSections.FindUsage(TableTabTab.Empty, [catalog]));
    }

    // ---- formatovanie ----

    [TestMethod]
    public void Formatovanie_NemeniTextyVUvodzovkach()
    {
        Assert.AreEqual("ODKLON, \"ODKLON\" = #SWITCH", TabTabFormatter.Format("Odklon, \"ODKLON\" = #SWITCH"));
        Assert.AreEqual("ODKLON, \"Odklon\" = #SWITCH", TabTabFormatter.Format("odklon, \"Odklon\"=#SWITCH"));
        Assert.AreEqual("1,\"a=b||c&&d\" = #SWITCH", TabTabFormatter.Format("1,\"a=b||c&&d\"=#SWITCH"));
        Assert.AreEqual("OPERATOR(\"cd\"),\"x\" = #SWITCH", TabTabFormatter.Format("operator(\"cd\"),\"x\"=#SWITCH"));
    }

    [TestMethod]
    public void Formatovanie_UpraviPodmienky()
    {
        Assert.AreEqual(
            "TYP(Typ_R) || PRIZNAK(Prizn_M) && ODKLON,\"x\",1,\"y\" = #SWITCH",
            TabTabFormatter.Format("typ(Typ_R)||priznak(prizn_m)  &&odklon,\"x\",1,\"y\"=#SWITCH"));

        // #MERGE: oddelovac aj text ostanu; Typ_ sa nemeni (kluc TrTypes.txt rozlisuje velkost pismen)
        Assert.AreEqual(
            "(TYP(Typ_ic)),\"#\",\"IC\"{34369} = #MERGE",
            TabTabFormatter.Format("(Typ(Typ_ic)),\"#\",\"IC\"{34369}=#MERGE"));
    }

    [TestMethod]
    public void Formatovanie_NemeniJednoduchePravidlaKomentareAUdalosti()
    {
        const string text = "; typ(x)||odklon=#SWITCH\r\nR{81}=R\r\n{@}=-{@}\r\nOdklon =#ODKLON\r\nAutobus=#VYLUKA\r\nIgnoreCase\r\n";
        Assert.AreEqual(text, TabTabFormatter.Format(text));
    }

    [TestMethod]
    public void Formatovanie_ViacriadkovePravidlo()
    {
        const string text = "(typ(Typ_R)||typ(Typ_Sp)), \"(cervene pole)\", \\\r\n1,\"-\"=#SWITCH\r\n";
        const string expected = "(TYP(Typ_R) || TYP(Typ_Sp)), \"(cervene pole)\", \\\r\n1,\"-\" = #SWITCH\r\n";

        var formatted = TabTabFormatter.Format(text);

        Assert.AreEqual(expected, formatted);
        Assert.AreEqual(formatted, TabTabFormatter.Format(formatted));
    }
}
