using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;

namespace GVDEditor.Tests.Domain.Rules;

/// <summary>
///     Kontrola fyzickych tabul a textov na tabuliach (Lokalne nastavenia → Tabule).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TableRulesTests
{
    private static TableCatalog Catalog(TableManufacturer manufacturer, params string[] items) => new()
    {
        Key = "K", Name = "Katalóg", Comment = "", Manufacturer = manufacturer,
        Items = items.Select(i => new TableItem { Key = i, Name = i }).ToList()
    };

    private static TablePhysical Physical(string key, TableCatalog catalog, int id = 1) => new()
    {
        Key = key, Name = key, TableCatalog = catalog, ID = id, SaveXML = "", Rem = "", ReverseArrows = "", Comment = ""
    };

    [TestMethod]
    [DataRow("Tabuľa", "Tabuľa 3")]
    [DataRow("Tabuľa 2", "Tabuľa 3")]
    [DataRow("Iná", "Iná")]
    public void Kopia_DostaneVolnyNazov(string name, string expected)
    {
        Assert.AreEqual(expected, TableRules.Unique(["Tabuľa", "Tabuľa 2"], name));
    }

    [TestMethod]
    public void Kluc_PrazdnyAleboRovnaky_JeChyba()
    {
        Assert.IsNotNull(TableRules.CheckKey(["A", " "], 1));
        Assert.IsNotNull(TableRules.CheckKey(["A", "A"], 1));
        // INISS kluce porovnava presne - iny tvar pismen je iny kluc
        Assert.IsNull(TableRules.CheckKey(["A", "a"], 1));
    }

    [TestMethod]
    [DataRow(1, true)]
    [DataRow(127, true)]
    [DataRow(128, false)]
    [DataRow(0, false)]
    [DataRow(-1, true)]
    public void Fyzicka_AdresaPodlaVyrobcu(int id, bool valid)
    {
        Assert.AreEqual(valid, TablePhysicalRules.CheckId(id, TableManufacturer.ELEN16) is null);
    }

    [TestMethod]
    public void Fyzicka_VyrobcaBezKontroly_AdresaVzdyPlati()
    {
        Assert.IsNull(TablePhysicalRules.CheckId(999, TableManufacturer.AdonBuse));
        Assert.IsNull(TablePhysicalRules.CheckId(999, TableManufacturer.Pragotron));
    }

    [TestMethod]
    public void Fyzicka_OdstranenyKatalog_JeChyba()
    {
        var catalog = Catalog(TableManufacturer.LCD1);
        TablePhysical[] tables = [Physical("A", catalog)];

        Assert.AreEqual(0, TablePhysicalRules.Check(tables, 0, [catalog]).Count);
        Assert.AreEqual(TablePhysicalRules.Field.Catalog, TablePhysicalRules.Check(tables, 0, []).Single().Field);
    }

    [TestMethod]
    public void Fyzicka_HlasiVsetkyChyby()
    {
        var catalog = Catalog(TableManufacturer.ELEN16);
        TablePhysical[] tables = [Physical("A", catalog), Physical("A", catalog, 500)];
        tables[1].Name = "";

        var fields = TablePhysicalRules.Check(tables, 1, [catalog]).Select(p => p.Field).ToList();

        CollectionAssert.AreEquivalent(new[] { TablePhysicalRules.Field.Name, TablePhysicalRules.Field.Key, TablePhysicalRules.Field.Id },
            fields);
    }

    [TestMethod]
    public void Text_RealizaciaNaNeexistujuciStlpec_JeChyba()
    {
        var catalog = Catalog(TableManufacturer.LCD1, "Ciel", "Cas");
        var text = new TableText { Key = "T", Name = "Text", Comment = "" };
        text.Realizations.Add(new TableTextRealization { Table = catalog, Item = catalog.Items[0] });
        text.Realizations.Add(new TableTextRealization { Table = catalog, Item = new TableItem { Key = "X", Name = "X" } });

        var problems = TableTextRules.Check([text], 0, [catalog]);

        Assert.AreEqual(1, problems.Count);
        Assert.AreEqual(TableTextRules.Field.Realization, problems[0].Field);
        Assert.AreEqual(1, problems[0].Row);
    }

    [TestMethod]
    public void Text_RealizaciaOdstranenejTabule_JeChyba()
    {
        var catalog = Catalog(TableManufacturer.LCD1, "Ciel");
        var realization = new TableTextRealization { Table = catalog, Item = catalog.Items[0] };

        Assert.IsNull(TableTextRules.CheckRealization(realization, [catalog]));
        Assert.IsNotNull(TableTextRules.CheckRealization(realization, []));
    }
}
