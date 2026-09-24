using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Okno katalogovej tabule (FTableCatalog, FTableColumnOrder): poradie stlpcov musi po premenovani ci zmazani
///     stlpca ostat citatelne, polozka „Ziadny“ sa nesmie dostat do TabTab.txt.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TableCatalogEditingTests
{
    private static TableItem Column(string key, TableAlign? align = null) => new()
    {
        Key = key, Name = key, FillSection = TableFillSection.Free, Align = align ?? TableAlign.Left,
        DivType = TableDivType.Free, Tab1 = TableTabTab.Empty, Tab2 = TableTabTab.Empty, Start = 0, End = 8
    };

    private static TableViewTypeTab TypeTab(params string[] keys)
    {
        var tab = new TableViewTypeTab { ViewType = TableViewType.Odchodova, CountLinesRecord = "1" };
        TableCatalogEditing.SetAllModes(tab, keys);
        return tab;
    }

    [TestMethod]
    public void PoradieStlpcov_PremenovanieKlucaVoVsetkychModoch()
    {
        List<TableViewTypeTab> tabs = [TypeTab("Cas", "Smer")];

        TableCatalogEditing.RenameKey(tabs, "Smer", "Ciel");

        Assert.IsTrue(tabs[0].TypeModeItems.All(m => m.ItemsKeys.SequenceEqual(["Cas", "Ciel"])));
        Assert.IsNull(TableCatalogEditing.FindUnknownKey(tabs, [Column("Cas"), Column("Ciel")]));
    }

    [TestMethod]
    public void PoradieStlpcov_ZmazanyStlpecSaOdoberieZoVsetkychModov()
    {
        List<TableViewTypeTab> tabs = [TypeTab("Cas", "Smer", "Smer")];
        Assert.AreEqual(TableViewMode.GetValues().Count * 2, TableCatalogEditing.CountKeyUsages(tabs, "Smer"));

        TableCatalogEditing.RemoveKey(tabs, "Smer");

        Assert.AreEqual(0, TableCatalogEditing.CountKeyUsages(tabs, "Smer"));
        Assert.IsTrue(tabs[0].TypeModeItems.All(m => m.ItemsKeys.SequenceEqual(["Cas"])));
    }

    [TestMethod]
    public void PoradieStlpcov_NeznamyKlucSaNajde()
    {
        List<TableViewTypeTab> tabs = [TypeTab("Cas", "Smer")];

        var unknown = TableCatalogEditing.FindUnknownKey(tabs, [Column("Cas")]);

        Assert.IsNotNull(unknown);
        Assert.AreEqual("Smer", unknown.Value.Key);
        Assert.AreSame(tabs[0], unknown.Value.Tab);
    }

    [TestMethod]
    public void PoradieStlpcov_PouzitPreVsetkyModyVytvoriVsetkySestModov()
    {
        var tab = new TableViewTypeTab { ViewType = TableViewType.Odchodova, CountLinesRecord = "1" };

        TableCatalogEditing.SetAllModes(tab, ["Cas"]);

        CollectionAssert.AreEqual(TableViewMode.GetValues().ToList(), tab.TypeModeItems.Select(m => m.ViewMode).ToList());
        // kazdy mod ma vlastny zoznam – uprava jedneho nesmie zmenit ostatne
        tab.TypeModeItems[0].ItemsKeys.Clear();
        Assert.HasCount(1, tab.TypeModeItems[1].ItemsKeys);
    }

    [TestMethod]
    public void PoradieStlpcov_KopiaJeNezavisla()
    {
        var tab = TypeTab("Cas");

        var copy = TableCatalogEditing.Clone(tab);
        copy.TypeModeItems[0].ItemsKeys.Add("Smer");
        copy.TypeModeItems.RemoveAt(1);

        Assert.HasCount(TableViewMode.GetValues().Count, tab.TypeModeItems);
        Assert.HasCount(1, tab.TypeModeItems[0].ItemsKeys);
    }

    [TestMethod]
    public void Riadky_ZmenaPoctuNechaPresnePozadovanyPocet()
    {
        // Enumerable.Range nejde – CS0433 (Enumerable aj v ExControls)
        var rows = new List<TableSegment>();
        for (var i = 0; i < 10; i++) rows.Add(new TableSegment { Height = i });

        TableCatalogEditing.ResizeRows(rows, 3, () => new TableSegment());
        CollectionAssert.AreEqual(new[] { 0, 1, 2 }, rows.Select(r => r.Height).ToArray());

        TableCatalogEditing.ResizeRows(rows, 5, () => new TableSegment { Height = 7 });
        CollectionAssert.AreEqual(new[] { 0, 1, 2, 7, 7 }, rows.Select(r => r.Height).ToArray());
        Assert.AreNotSame(rows[3], rows[4]);

        TableCatalogEditing.ResizeRows(rows, 0, () => new TableSegment());
        Assert.IsEmpty(rows);
    }

    [TestMethod]
    public void TabTab_ZiadnySaPridaLenDoKopie()
    {
        var tab = new TableTabTab { Key = "Smer", Text = "\"A\"=\"B\"" };
        List<TableTabTab> global = [tab, TableTabTab.Empty with { }];

        var list = TableCatalogEditing.WithEmptyTabTab(global);

        CollectionAssert.AreEqual(new[] { TableTabTab.Empty, tab }, list);
        Assert.HasCount(2, global);
    }

    [TestMethod]
    public void Stlpec_KopiaAZapisSpatZachovaInstanciu()
    {
        var original = Column("Smer");
        var clone = TableCatalogEditing.Clone(original);
        clone.Key = "Ciel";
        clone.Align = TableAlign.Right;

        Assert.AreEqual("Smer", original.Key);
        TableCatalogEditing.CopyTo(clone, original);
        Assert.AreEqual("Ciel", original.Key);
        Assert.AreSame(TableAlign.Right, original.Align);
    }

    [TestMethod]
    public void Subory_ZapisACitanieBezPolozkyZiadnyAZarovnanimVpravo()
    {
        var dir = Directory.CreateTempSubdirectory("gvdtables");
        try
        {
            var tabTab = new TableTabTab { Key = "Smer", Text = "\"A\"=\"B\"" };
            var catalog = new TableCatalog
            {
                Key = "Odch", Name = "Odchodova", Comment = "", Manufacturer = TableManufacturer.GetValues()[0], MaxRecCount = 1,
                Items = [Column("Cas", TableAlign.Right), Column("Smer")],
                Segments = [new TableSegment { Height = 10, Width = 128, Size = 15 }],
                ViewTypeTabs = [TypeTab("Cas", "Smer")]
            };

            // FTableCatalog kedysi vkladal Ziadny do GlobData.TabTabs – do suboru sa nesmie dostat
            TxtParser.WriteTables(dir.FullName, [TableTabTab.Empty, tabTab], [catalog], [], []);
            var text = File.ReadAllText(Path.Combine(dir.FullName, FileConsts.FILE_TABTAB), Encodings.Win1250);
            Assert.DoesNotContain("[" + TableTabTab.Empty.Key + "]", text);

            var (tabTabs, catalogs, _, _) = TxtParser.ReadTables(dir.FullName);
            CollectionAssert.AreEqual(new[] { tabTab }, tabTabs);
            Assert.AreSame(TableAlign.Right, catalogs[0].Items[0].Align);
            Assert.AreSame(TableAlign.Left, catalogs[0].Items[1].Align);
            CollectionAssert.AreEqual(new[] { "Cas", "Smer" }, catalogs[0].ViewTypeTabs[0].TypeModeItems[0].ItemsKeys);
        }
        finally
        {
            dir.Delete(true);
        }
    }

    [TestMethod]
    public void Subory_PrazdnaSekciaZiadnySaPriCitaniVynecha()
    {
        var dir = Directory.CreateTempSubdirectory("gvdtables");
        try
        {
            TxtParser.WriteTables(dir.FullName, [new TableTabTab { Key = "Smer", Text = "\"A\"=\"B\"" }], [], [], []);
            // subor po starsej verzii GVDEditora: prazdna sekcia [Ziadny]
            // (za nou dalsia sekcia – dokazuje, ze sa pripisane sekcie naozaj citaju)
            File.AppendAllText(Path.Combine(dir.FullName, FileConsts.FILE_TABTAB),
                "\r\n[" + TableTabTab.Empty.Key + "]\r\n\r\n[Druh]\r\n\"R\"=\"{2}R\"\r\n", Encodings.Win1250);

            var (tabTabs, _, _, _) = TxtParser.ReadTables(dir.FullName);

            CollectionAssert.AreEqual(new[] { "Smer", "Druh" }, tabTabs.Select(t => t.Key).ToArray());
        }
        finally
        {
            dir.Delete(true);
        }
    }
}
