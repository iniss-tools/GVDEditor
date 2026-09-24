using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Zostava logickej tabule (okno FTableLogical): umiestnenia zaznamov sa po prevode na zostavu a spat nesmu zmenit.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TableLogicalLayoutTests
{
    private static readonly TablePhysical TabA = new() { Key = "A", Name = "A" };
    private static readonly TablePhysical TabB = new() { Key = "B", Name = "B" };

    private static TablePosition Pos(TablePhysical table, int position, TableViewType? type = null) =>
        new() { Table = table, Position = position, TypeView = type ?? TableViewType.Odchodova };

    private static List<TableRecord> Records(params TablePosition[][] records) =>
        records.Select(positions => new TableRecord { Positions = positions.ToList() }).ToList();

    private static void AssertRoundTrip(List<TableRecord> records, int expectedSegments)
    {
        var segments = TableLogicalLayout.FromRecords(records);
        Assert.IsNull(TableLogicalLayout.FindDifference(records, TableLogicalLayout.ToRecords(segments, records.Count)));
        Assert.IsTrue(TableLogicalLayout.IsExpressible(records, segments));
        Assert.HasCount(expectedSegments, segments);
    }

    [TestMethod]
    public void Zostava_JedenZaznamNaDruhomRiadku()
    {
        // napr. podchodova tabula: jediny zaznam na 2. riadku fyzickej tabule (POSITION_001_001=1)
        var records = Records([Pos(TabA, 1)]);
        var segments = TableLogicalLayout.FromRecords(records);

        Assert.HasCount(1, segments);
        Assert.AreEqual(1, segments[0].FirstRecord);
        Assert.AreEqual(1, segments[0].LastRecord);
        Assert.AreEqual(2, segments[0].StartRow);
        AssertRoundTrip(records, 1);
    }

    [TestMethod]
    public void Zostava_ZaznamyRozdeleneNaDveTabule()
    {
        // zaznamy 1-8 na tabuli A od riadku 1, zaznamy 9-16 na tabuli B tiez od riadku 1
        var records = new List<TableRecord>();
        for (var i = 0; i < 16; i++)
            records.Add(new TableRecord { Positions = [i < 8 ? Pos(TabA, i) : Pos(TabB, i - 8)] });
        var segments = TableLogicalLayout.FromRecords(records);

        Assert.HasCount(2, segments);
        Assert.AreEqual((9, 16, 1), (segments[1].FirstRecord, segments[1].LastRecord, segments[1].StartRow));
        AssertRoundTrip(records, 2);
    }

    [TestMethod]
    public void Zostava_InyTypZobrazeniaNaJednejTabuli()
    {
        var records = Records(
            [Pos(TabA, 0), Pos(TabB, 0, TableViewType.Nastupistna)],
            [Pos(TabA, 1), Pos(TabB, 1, TableViewType.Nastupistna)]);
        var segments = TableLogicalLayout.FromRecords(records);

        Assert.AreEqual(TableViewType.Nastupistna, segments[1].TypeView);
        AssertRoundTrip(records, 2);
    }

    [TestMethod]
    public void Zostava_ZachovaPoradieUmiestneniVZazname()
    {
        // zaznam 1 ide najprv na A, potom na B; zaznam 2 naopak - poradie COUNT_POS sa musi zachovat
        var records = Records([Pos(TabA, 0), Pos(TabB, 0)], [Pos(TabB, 1), Pos(TabA, 1)]);
        AssertRoundTrip(records, 3);
    }

    [TestMethod]
    public void Zostava_ZapornaPoziciaNieJeVyjadritelna()
    {
        var records = Records([Pos(TabA, -1)]);
        var segments = TableLogicalLayout.FromRecords(records);

        Assert.IsFalse(TableLogicalLayout.IsExpressible(records, segments));
    }

    [TestMethod]
    public void Zostava_ToRecordsIgnorujeRozsahZaPoctomZaznamov()
    {
        var segments = new List<TableLogicalSegment>
        {
            new() { Table = TabA, FirstRecord = 2, LastRecord = 5, StartRow = 3, TypeView = TableViewType.Odchodova }
        };
        var records = TableLogicalLayout.ToRecords(segments, 3);

        Assert.HasCount(3, records);
        Assert.IsEmpty(records[0].Positions);
        Assert.AreEqual(2, records[1].Positions[0].Position);
        Assert.AreEqual(3, records[2].Positions[0].Position);
    }
}
