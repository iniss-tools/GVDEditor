using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Pisma tabul (zalozka Pisma v Lokalnych nastaveniach): kde sa cislo pisma pouziva a jeho prenesenie pri zmene ID.
///     Stlpce katalogovych tabul a texty vlakov sa menia, TabTab len hlasi - {n} tam moze byt aj kod znaku.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TableFontUsageTests
{
    private static TableItem Column(string key, int font) => new()
    {
        Key = key, Name = key, FillSection = TableFillSection.Free, Align = TableAlign.Left,
        DivType = TableDivType.Free, Tab1 = TableTabTab.Empty, Tab2 = TableTabTab.Empty, Start = 0, End = 8, FontIDX = font
    };

    private static TableCatalog Catalog(params TableItem[] items)
    {
        var catalog = new TableCatalog { Key = "Kat", Name = "Kat" };
        catalog.Items.AddRange(items);
        return catalog;
    }

    private static TableText Text(params int[] fonts)
    {
        var text = new TableText { Key = "Ciel", Name = "Ciel" };
        foreach (var font in fonts)
            text.Trains.Add(new TableTrain { Train = new Train(), Text = "x", FontID = font });
        return text;
    }

    [TestMethod]
    public void Pouzitie_PocitaStlpceTextyASekcieTabTab()
    {
        List<TableCatalog> catalogs = [Catalog(Column("Cas", 81), Column("Smer", 16), Column("Druh", 81))];
        List<TableText> texts = [Text(81, -1, 16)];
        List<TableTabTab> tabTabs =
        [
            TableTabTab.Empty,
            new() { Key = "Druh", Text = "R{81}=R\r\nEx{810}=Ex" },
            new() { Key = "Smer", Text = "{@}=-{@}" },
            new() { Key = "Iny", Text = "IC{081}=IC" },
        ];

        var usage = TableFontUsage.Find(81, catalogs, texts, tabTabs);

        Assert.AreEqual(2, usage.CatalogColumns);
        Assert.AreEqual(1, usage.TrainTexts);
        CollectionAssert.AreEqual(new[] { "Druh", "Iny" }, usage.TabTabSections.ToArray());
        Assert.IsTrue(usage.IsUsed);
        Assert.IsTrue(usage.HasReplaceable);
    }

    [TestMethod]
    public void Pouzitie_NepouzitePismo()
    {
        var usage = TableFontUsage.Find(99, [Catalog(Column("Cas", 81))], [Text(81)], [new() { Key = "Druh", Text = "R{81}=R" }]);

        Assert.IsFalse(usage.IsUsed);
        Assert.IsFalse(usage.HasReplaceable);
    }

    [TestMethod]
    public void Pouzitie_LenTabTabSaNedaZmenitAutomaticky()
    {
        var usage = TableFontUsage.Find(81, [Catalog(Column("Cas", 16))], [Text(16)], [new() { Key = "Druh", Text = "R{81}=R" }]);

        Assert.IsTrue(usage.IsUsed);
        Assert.IsFalse(usage.HasReplaceable);
    }

    [TestMethod]
    public void Zmena_PrepisujeStlpceATextyAleNieTabTab()
    {
        List<TableCatalog> catalogs = [Catalog(Column("Cas", 81), Column("Smer", 16))];
        List<TableText> texts = [Text(81, 16)];
        var druh = new TableTabTab { Key = "Druh", Text = "R{81}=R" };

        var changed = TableFontUsage.Replace(81, 85, catalogs, texts);

        Assert.AreEqual(2, changed);
        Assert.AreEqual(85, catalogs[0].Items[0].FontIDX);
        Assert.AreEqual(16, catalogs[0].Items[1].FontIDX);
        Assert.AreEqual(85, texts[0].Trains[0].FontID);
        Assert.AreEqual(16, texts[0].Trains[1].FontID);
        Assert.AreEqual("R{81}=R", druh.Text);
    }
}
