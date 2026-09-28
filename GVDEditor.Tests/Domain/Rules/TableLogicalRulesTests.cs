using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;

namespace GVDEditor.Tests.Domain.Rules;

/// <summary>
///     Stranka Logicke tabule: upravy zostavy (pocet zaznamov, typ, novy riadok) a kontrola tabule.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TableLogicalRulesTests
{
    private static TablePhysical Physical(string key, int recCount, params TableViewType[] supported)
    {
        var catalog = new TableCatalog { Key = "K" + key, Name = "K" + key, Comment = "", Manufacturer = TableManufacturer.LCD1 };
        foreach (var type in supported)
            catalog.ViewTypeTabs.Add(new TableViewTypeTab { ViewType = type, CountLinesRecord = "1" });
        return new TablePhysical { Key = key, Name = key, RecCount = recCount, TableCatalog = catalog };
    }

    private static TableLogicalSegment Segment(TablePhysical table, int first, int last, int row = 1, TableViewType? type = null) =>
        new() { Table = table, FirstRecord = first, LastRecord = last, StartRow = row, TypeView = type ?? TableViewType.Odchodova };

    [TestMethod]
    public void Pocet_ZmenseniSkratiAleboOdstraniRiadky()
    {
        var a = Physical("A", 0);
        List<TableLogicalSegment> segments = [Segment(a, 1, 4), Segment(a, 5, 8, 5)];

        TableLogicalLayout.Resize(segments, 8, 3);

        Assert.HasCount(1, segments);
        Assert.AreEqual(3, segments[0].LastRecord);
    }

    [TestMethod]
    public void Pocet_ZvacseniPredliziRiadkyDoKonca()
    {
        var a = Physical("A", 0);
        List<TableLogicalSegment> segments = [Segment(a, 1, 4), Segment(a, 1, 8)];

        TableLogicalLayout.Resize(segments, 8, 10);

        Assert.AreEqual(4, segments[0].LastRecord);
        Assert.AreEqual(10, segments[1].LastRecord);
    }

    [TestMethod]
    public void Typ_PrejdeLenNaPodporovaneRiadky()
    {
        var both = Physical("A", 0, TableViewType.Odchodova, TableViewType.Prichodova);
        var only = Physical("B", 0, TableViewType.Odchodova);
        List<TableLogicalSegment> segments = [Segment(both, 1, 4), Segment(only, 1, 4)];

        Assert.IsTrue(TableLogicalLayout.ChangeViewType(segments, TableViewType.Odchodova, TableViewType.Prichodova));
        Assert.AreEqual(TableViewType.Prichodova, segments[0].TypeView);
        Assert.AreEqual(TableViewType.Odchodova, segments[1].TypeView);
    }

    [TestMethod]
    public void NovyRiadok_BeriePodporovanyTyp()
    {
        var table = Physical("A", 0, TableViewType.Nastupistna);

        var segment = TableLogicalLayout.NewSegment(table, 6, TableViewType.Odchodova);

        Assert.AreEqual((1, 6, 1), (segment.FirstRecord, segment.LastRecord, segment.StartRow));
        Assert.AreEqual(TableViewType.Nastupistna, segment.TypeView);
    }

    [TestMethod]
    public void Kontrola_NeplatnyRiadokZostavy()
    {
        var a = Physical("A", 0);
        var table = new TableLogical { Key = "L", Name = "L", Comment = "", Records = TableLogicalLayout.ToRecords([], 4) };

        var problems = TableLogicalRules.Check([table], 0, [Segment(a, 1, 4), Segment(a, 3, 6), Segment(a, 1, 1, 0)]);

        CollectionAssert.AreEqual(new[] { 1, 2 }, problems.Where(p => p.Field == TableLogicalRules.Field.Segment).Select(p => p.Row).ToArray());
    }

    [TestMethod]
    public void Kontrola_TabulaBezZaznamov()
    {
        var table = new TableLogical { Key = "L", Name = "L", Comment = "" };

        Assert.AreEqual(TableLogicalRules.Field.Count, TableLogicalRules.Check([table], 0, null).Single().Field);
    }

    [TestMethod]
    public void Upozornenia_TypMimoTabuleAKolizia()
    {
        var a = Physical("A", 4, TableViewType.Odchodova);
        var warnings = TableLogicalRules.Warnings([Segment(a, 1, 6, 1, TableViewType.Prichodova), Segment(a, 1, 1, 2)]);

        // nepodporovany typ a riadky 1-6 na 4-riadkovej tabuli patria prvemu riadku zostavy, kolizia riadku 2 obom
        Assert.AreEqual(2, warnings.Count(w => w.Row == 0));
        Assert.AreEqual(1, warnings.Count(w => w.Row == -1));
    }

    [TestMethod]
    [DataRow("", 0)]
    [DataRow("5613600", 5613600)]
    [DataRow("5613600 – Dolné Mesto", 5613600)]
    [DataRow("abc", null)]
    [DataRow("56x", null)]
    public void Stanica_CisloZPola(string text, int? expected)
    {
        Assert.AreEqual(expected, TableLogicalRules.ParseStation(text));
    }
}
