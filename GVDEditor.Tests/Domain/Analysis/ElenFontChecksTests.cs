using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.UI.Settings;
using ToolsCore.Iniss.Elen;

namespace GVDEditor.Tests.Domain.Analysis;

/// <summary>
/// Analyza grafikonu: cisla pisiem, ktore by tabula ELEN dostala za ESC ako riadiaci znak (dolny bajt pod 0x20,
/// bez bitu 0x40) - zoznam pisiem, stlpce, texty vlakov a lava strana pravidiel TabTab.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class ElenFontChecksTests
{
    private static TableItem Column(string name, int font, TableDivType? div = null, TableTabTab? tab1 = null, TableTabTab? tab2 = null) => new()
    {
        Key = name, Name = name, FillSection = TableFillSection.Free, Align = TableAlign.Left,
        DivType = div ?? TableDivType.Free, Tab1 = tab1 ?? TableTabTab.Empty, Tab2 = tab2 ?? TableTabTab.Empty, Start = 0, End = 48, FontIdx = font
    };

    private static TableFont Font(string name, int id) => new() { Name = name, FontID = id, FileName = "" };

    /// <summary>
    /// Grafikon s katalogovou tabulou vyrobcu <paramref name="manufacturer" /> a fyzickou tabulou, ktora ju pouziva.
    /// </summary>
    private static (GrafikonDocument Doc, TableCatalog Catalog) Grafikon(TableManufacturer manufacturer, params TableItem[] columns)
    {
        var doc = new GrafikonDocument();
        var catalog = AddCatalog(doc, "ODJ", manufacturer, columns);
        doc.TablePhysicals.Add(new TablePhysical { Key = "T1", Name = "Tabuľa 1", TableCatalog = catalog, ID = 1 });
        return (doc, catalog);
    }

    private static TableCatalog AddCatalog(GrafikonDocument doc, string key, TableManufacturer manufacturer, params TableItem[] columns)
    {
        var catalog = new TableCatalog { Key = key, Name = key, Manufacturer = manufacturer };
        catalog.Items.AddRange(columns);
        doc.TableCatalogs.Add(catalog);
        return catalog;
    }

    private static TableText Text(string key, TableCatalog realizedOn, params int[] fonts)
    {
        var text = new TableText { Key = key, Name = key };
        text.Realizations.Add(new TableTextRealization { Table = realizedOn, Item = realizedOn.Items[0] });
        var number = 1000;
        foreach (var font in fonts)
            text.Trains.Add(new TableTrain { Train = new Train { Number = (number++).ToString(CultureInfo.InvariantCulture) }, Text = "x", FontID = font });
        return text;
    }

    [TestMethod]
    [DataRow(4, true)]
    [DataRow(5, true)]
    [DataRow(0, true)]
    [DataRow(533, true)] // 0x215 - posle sa len dolny bajt 0x15
    [DataRow(0x8604, true)] // rozsirene pismo, no dolny bajt 0x04
    [DataRow(-1, false)] // pismo sa neposiela
    [DataRow(-2, false)]
    [DataRow(32, false)]
    [DataRow(82, false)]
    [DataRow(98, false)]
    [DataRow(34370, false)] // 0x8642
    public void RiadiaciZnak_DolnyBajtPod0x20(int id, bool expected)
    {
        Assert.AreEqual(expected, new ElenFontCode(id).SendsControlChar);
    }

    [TestMethod]
    public void RiadiaciZnak_Bit0x40ZachovaVzhlad()
    {
        var code = new ElenFontCode(0x15);
        var fixedCode = code.WithKeptBit;

        Assert.AreEqual(0x55, fixedCode.Id);
        Assert.IsFalse(fixedCode.SendsControlChar);
        Assert.AreEqual(code.Face, fixedCode.Face);
        Assert.AreEqual(code.Color, fixedCode.Color);
        Assert.AreEqual(code.Blinks, fixedCode.Blinks);
    }

    [TestMethod]
    [DataRow("ELEN")]
    [DataRow("ELENOLD")]
    [DataRow("ELEN10")]
    [DataRow("ELEN16")]
    [DataRow("ELEN16Kam")]
    [DataRow("ELEKON")]
    public void ZoznamPisiem_TabuleElen_HlasiZleCisla(string manufacturer)
    {
        // zoznam pisiem ako v realnom grafikone - 4 a 5 bez bitu 0x40, ostatne platne
        var (doc, _) = Grafikon(TableManufacturer.Parse(manufacturer)!, Column("Cas", 82));
        doc.TableFonts.Add(Font("Štandartné", 4));
        doc.TableFonts.Add(Font("Štandartné + tučné", 4));
        doc.TableFonts.Add(Font("Štandartné + červené", 5));
        doc.TableFonts.Add(Font("Tenké zelené", 82));
        doc.TableFonts.Add(Font("Tučné zelené", 98));
        doc.TableFonts.Add(Font("Rozšírené 6", 34370));

        var findings = ElenFontChecks.Find(doc);

        Assert.IsTrue(findings.All(f => f.Source == ElenFontSource.FontList));
        CollectionAssert.AreEqual(new[] { "Štandartné", "Štandartné + tučné", "Štandartné + červené" }, findings.Select(f => f.Name).ToArray());
        CollectionAssert.AreEqual(new[] { 4, 4, 5 }, findings.Select(f => f.FontId).ToArray());
    }

    [TestMethod]
    public void BezTabuleElen_NicNehlasi()
    {
        // LCD1 cisla pisiem pouziva inak (16 + cislo fontu) - 17 je tam platne
        var (doc, catalog) = Grafikon(TableManufacturer.Lcd1, Column("Cas", 17));
        doc.TableFonts.Add(Font("Font 1", 17));
        doc.TableTexts.Add(Text("Ciel", catalog, 18));

        Assert.IsEmpty(ElenFontChecks.Find(doc));
    }

    [TestMethod]
    public void KatalogElenBezFyzickejTabule_NicNehlasi()
    {
        // katalogova tabula je len predloha - kym ju nepouziva fyzicka tabula, INISS jej nic neposiela
        var doc = new GrafikonDocument();
        AddCatalog(doc, "ODJ", TableManufacturer.Elen10, Column("Cas", 4));
        doc.TableFonts.Add(Font("Štandartné", 4));

        Assert.IsEmpty(ElenFontChecks.Find(doc));
    }

    [TestMethod]
    public void Stlpce_LenTabuleElenSPrvymZlymStlpcomAPoctom()
    {
        var (doc, elen) = Grafikon(TableManufacturer.Elen10, Column("Cas", 82), Column("Ciel", 4), Column("Smer", -1), Column("Druh", 5));
        var lcd = AddCatalog(doc, "LCD", TableManufacturer.Lcd1, Column("Cas", 17));
        doc.TablePhysicals.Add(new TablePhysical { Key = "T2", Name = "Tabuľa 2", TableCatalog = lcd, ID = 2 });

        var findings = ElenFontChecks.Find(doc);

        Assert.HasCount(1, findings);
        Assert.AreEqual(ElenFontSource.Column, findings[0].Source);
        Assert.AreSame(elen, findings[0].Item);
        Assert.AreEqual("„Ciel“", findings[0].Detail);
        Assert.AreEqual(4, findings[0].FontId);
        Assert.AreEqual(2, findings[0].Count);
    }

    [TestMethod]
    public void TextyVlakov_LenRealizovaneNaTabuliElen()
    {
        var (doc, elen) = Grafikon(TableManufacturer.Elen16, Column("Ciel", 82));
        var lcd = AddCatalog(doc, "LCD", TableManufacturer.Lcd1, Column("Ciel", 17));
        doc.TablePhysicals.Add(new TablePhysical { Key = "T2", Name = "Tabuľa 2", TableCatalog = lcd, ID = 2 });
        // -1 = pismo stlpca, nic sa neposiela
        var onElen = Text("Cieľová stanica", elen, -1, 82, 20);
        doc.TableTexts.Add(onElen);
        doc.TableTexts.Add(Text("Smer LCD", lcd, 18));

        var findings = ElenFontChecks.Find(doc);

        Assert.HasCount(1, findings);
        Assert.AreEqual(ElenFontSource.TrainText, findings[0].Source);
        Assert.AreSame(onElen, findings[0].Item);
        Assert.AreEqual("1002", findings[0].Detail);
        Assert.AreEqual(20, findings[0].FontId);
        Assert.AreEqual(1, findings[0].Count);
    }

    [TestMethod]
    public void TabTab_LavaStranaPravidielAPoloziek()
    {
        // prava strana sa len porovnava ({18}), {@} je predvolene pismo, {n} v uvodzovkach je text
        var druh = new TableTabTab
        {
            Key = "DRUH",
            Text = "R{81}=R\nSC{82}=SC{18}\n{@}=-{@}\n\"Ex{4}\"=Ex\nOdklon, \"ODKLON\"{5} = #SWITCH\nAutobus{0}=#VYLUKA"
        };
        var (doc, _) = Grafikon(TableManufacturer.Elen, Column("Druh", 82, TableDivType.Table, druh));
        doc.TabTabs.Add(druh);

        var findings = ElenFontChecks.Find(doc);

        Assert.HasCount(1, findings);
        Assert.AreEqual(ElenFontSource.TabTab, findings[0].Source);
        Assert.AreSame(druh, findings[0].Item);
        Assert.AreEqual("5", findings[0].Detail);
        Assert.AreEqual(5, findings[0].FontId);
        Assert.AreEqual(2, findings[0].Count);
        CollectionAssert.AreEqual(new[] { (0, 81), (1, 82), (2, -1), (4, 5), (5, 0) }, ElenFontChecks.SentFonts(druh.Text).ToArray());
    }

    [TestMethod]
    public void TabTab_SekciaNepouzitaTabulouElen_NicNehlasi()
    {
        // TAB1 pri volnom plneni a TAB2 mimo casu sa nepouziju; sekciu pouziva len tabula LCD1
        var tab1 = new TableTabTab { Key = "VOLNY", Text = "R{4}=R" };
        var tab2 = new TableTabTab { Key = "TAB2", Text = "1{4}=1" };
        var lcdTab = new TableTabTab { Key = "LCD", Text = "R{17}=R" };
        var (doc, _) = Grafikon(TableManufacturer.Elen10, Column("Druh", 82, TableDivType.Free, tab1), Column("Cas", 82, TableDivType.Table, tab2: tab2));
        var lcd = AddCatalog(doc, "LCD", TableManufacturer.Lcd1, Column("Druh", 17, TableDivType.Table, lcdTab));
        doc.TablePhysicals.Add(new TablePhysical { Key = "T2", Name = "Tabuľa 2", TableCatalog = lcd, ID = 2 });
        doc.TabTabs.Add(tab1);
        doc.TabTabs.Add(tab2);
        doc.TabTabs.Add(lcdTab);

        Assert.IsEmpty(ElenFontChecks.Find(doc));
    }

    [TestMethod]
    public void TabTab_Tab2PriCase()
    {
        var tab2 = new TableTabTab { Key = "MINUTY", Text = "0{4}=0" };
        var hodiny = new TableTabTab { Key = "HODINY", Text = "0=0" };
        var (doc, _) = Grafikon(TableManufacturer.Elen10, Column("Cas", 82, TableDivType.TableTime, hodiny, tab2));
        doc.TabTabs.Add(hodiny);
        doc.TabTabs.Add(tab2);

        var findings = ElenFontChecks.Find(doc);

        Assert.HasCount(1, findings);
        Assert.AreSame(tab2, findings[0].Item);
    }

    [TestMethod]
    public void Problem_TextRiesenieAOprava()
    {
        var (doc, _) = Grafikon(TableManufacturer.Elen10, Column("Cas", 82));
        var font = Font("Štandartné", 4);
        doc.TableFonts.Add(font);
        var host = new Host();
        var scope = new AnalysisScope(doc, new InissWorkspace(), host);

        var problem = ElenFontChecks.Problems(scope).Single();

        Assert.AreEqual(ProblemType.Warning, problem.ProblemType);
        Assert.AreEqual(FixType.Manual, problem.FixType);
        StringAssert.Contains(problem.Text, "Štandartné");
        StringAssert.Contains(problem.Text, "0x04");
        StringAssert.Contains(problem.Solution, "68");

        // pouzivatel okno zavrie bez zmeny
        Assert.AreEqual(FixResult.NotSolved, problem.FixProblem());
        Assert.AreEqual(LocalSettingsPage.Pisma, host.Page);
        Assert.AreSame(font, host.Selected);

        // pouzivatel zmeni cislo pisma
        host.OnShow = () => font.FontID = 98;
        Assert.AreEqual(FixResult.Done, problem.FixProblem());
    }

    [TestMethod]
    public void Problem_ViacZlychMiestPolozky()
    {
        var (doc, _) = Grafikon(TableManufacturer.Elen10, Column("Ciel", 4), Column("Smer", 5));
        var scope = new AnalysisScope(doc, new InissWorkspace(), new Host());

        var problem = ElenFontChecks.Problems(scope).Single();

        StringAssert.Contains(problem.Text, "ODJ");
        StringAssert.Contains(problem.Text, "„Ciel“");
        StringAssert.Contains(problem.Text, "1)");
    }

    private sealed class Host : IAnalyzerHost
    {
        public LocalSettingsPage? Page { get; private set; }

        public object? Selected { get; private set; }

        public Action? OnShow { get; set; }

        public bool ShowLocalSettings(LocalSettingsPage page = LocalSettingsPage.Grafikon, LocalSettingsAction action = LocalSettingsAction.None,
            object? select = null)
        {
            Page = page;
            Selected = select;
            OnShow?.Invoke();
            return OnShow is not null;
        }

        public void EditTabTab(TableTabTab tabTab) => OnShow?.Invoke();

        public bool EditTrain(Train train) => false;

        public void ShowInissSettings(string? section)
        {
        }
    }
}
