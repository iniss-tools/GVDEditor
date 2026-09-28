using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;

namespace GVDEditor.Tests.Domain.Rules;

/// <summary>
/// Stranka Katalogove tabule: kontrola stlpcov podla toho, co INISS prijme, a upravy tabule (novy stlpec, kopia).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TableCatalogRulesTests
{
    private static readonly TableTabTab Druh = new() { Key = "Druh", Text = "" };

    private static TableItem Column(string key, int line, int start, int end, TableDivType? div = null, TableTabTab? tab1 = null,
        TableTabTab? tab2 = null) => new()
    {
        Key = key, Name = key, Line = line, Start = start, End = end, FillSection = TableFillSection.Free, Align = TableAlign.Left,
        DivType = div ?? TableDivType.Free, Tab1 = tab1 ?? TableTabTab.Empty, Tab2 = tab2 ?? TableTabTab.Empty
    };

    private static TableCatalog Catalog(TableManufacturer manufacturer, params TableItem[] items)
    {
        var table = new TableCatalog { Key = "K", Name = "Katalóg", Comment = "", Manufacturer = manufacturer };
        table.Items.AddRange(items);
        return table;
    }

    [TestMethod]
    public void Stlpce_PrekryvANeusporiadanaPozicia_NieSuChyba()
    {
        // alternativy pre rozne rezimy sa na riadku prekryvaju a nemusia ist za sebou - bezne v realnych datach
        var table = Catalog(TableManufacturer.ELEN16, Column("Cas", 0, 0, 30), Column("Text", 0, 0, 200), Column("Mesk", 0, 168, 216),
            Column("Smer", 0, 40, 160));

        Assert.AreEqual(0, TableCatalogRules.Check([table], 0).Count);
    }

    [TestMethod]
    public void Stlpec_BezSirky_JeChyba()
    {
        var table = Catalog(TableManufacturer.ELEN16, Column("A", 0, 40, 40));

        Assert.IsNotNull(TableCatalogRules.CheckColumn(table, 0));
    }

    [TestMethod]
    [DataRow(0, false, false, true)]
    [DataRow(1, false, false, false)]
    [DataRow(1, true, false, true)]
    [DataRow(2, true, false, false)]
    [DataRow(2, true, true, true)]
    [DataRow(3, true, false, true)]
    [DataRow(4, false, false, false)]
    public void Stlpec_TabPodlaSposobuPlnenia(int div, bool tab1, bool tab2, bool valid)
    {
        var divType = TableDivType.GetValues().Single(d => d.Id == div);
        var table = Catalog(TableManufacturer.ELEN16, Column("A", 0, 0, 40, divType, tab1 ? Druh : null, tab2 ? Druh : null));

        Assert.AreEqual(valid, TableCatalogRules.CheckColumn(table, 0) is null);
    }

    [TestMethod]
    public void Stlpec_ElenNad512_JeChyba()
    {
        Assert.IsNotNull(TableCatalogRules.CheckColumn(Catalog(TableManufacturer.ELEN, Column("A", 0, 480, 520)), 0));
        Assert.IsNull(TableCatalogRules.CheckColumn(Catalog(TableManufacturer.ELEN16, Column("A", 0, 480, 520)), 0));
    }

    [TestMethod]
    public void Stlpec_ZdvojenyKluc_JeChyba()
    {
        var table = Catalog(TableManufacturer.ELEN16, Column("A", 0, 0, 40), Column("A", 0, 40, 80));

        Assert.IsNotNull(TableCatalogRules.CheckColumn(table, 1));
    }

    [TestMethod]
    public void Upozornenia_NasobokZnakuANepouzitaTab()
    {
        var table = Catalog(TableManufacturer.LCD1, Column("A", 0, 0, 22), Column("B", 0, 24, 48, TableDivType.Free, Druh));

        var warnings = TableCatalogRules.Warnings(table);

        CollectionAssert.AreEqual(new[] { 0, 1 }, warnings.Select(w => w.Column).ToArray());
        Assert.AreEqual(0, TableCatalogRules.Check([table], 0).Count);
    }

    [TestMethod]
    public void NovyStlpec_ZaPoslednymZarovnanyNaZnak()
    {
        var table = Catalog(TableManufacturer.LCD1, Column("Stĺpec", 1, 0, 30));

        var item = TableCatalogEditing.NewColumn(table, "Stĺpec", TableCatalogRules.CellWidth(table.Manufacturer));

        Assert.AreEqual("Stĺpec 2", item.Name);
        Assert.AreEqual((1, 32, 96), (item.Line, item.Start, item.End));
    }

    [TestMethod]
    public void Kopia_MaVlastneStlpceARiadky()
    {
        var table = Catalog(TableManufacturer.LCD1, Column("A", 0, 0, 32));
        table.Segments.Add(new TableSegment { Height = 10 });

        var copy = TableCatalogEditing.Clone(table);
        copy.Items[0].Name = "B";
        copy.Segments[0].Height = 20;

        Assert.AreEqual("A", table.Items[0].Name);
        Assert.AreEqual(10, table.Segments[0].Height);
    }

    [TestMethod]
    public void TextyVStlpci()
    {
        var table = Catalog(TableManufacturer.LCD1, Column("A", 0, 0, 32), Column("B", 0, 32, 64));
        var text = new TableText { Key = "T", Name = "Cieľ", Comment = "" };
        text.Realizations.Add(new TableTextRealization { Table = table, Item = table.Items[1] });

        Assert.AreEqual(0, TableCatalogEditing.TextsUsing(table.Items[0], [text]).Count);
        Assert.AreEqual(1, TableCatalogEditing.TextsUsing(table.Items[1], [text]).Count);
    }
}
