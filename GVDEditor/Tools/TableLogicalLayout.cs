using GVDEditor.Entities;

namespace GVDEditor.Tools;

/// <summary>
///     Jeden riadok zostavy logickej tabule: suvisly rozsah zaznamov logickej tabule, ktory ide na jednu fyzicku
///     tabulu od urceneho riadku s jednym typom zobrazenia. Cisla su od 1 ako v okne (zaznam 1 = <c>nnn</c> 001,
///     riadok 1 = <c>POSITION</c> 0).
/// </summary>
public sealed class TableLogicalSegment
{
    /// <summary>
    ///     Fyzicka tabula, na ktoru sa zaznamy posielaju.
    /// </summary>
    public TablePhysical Table { get; set; } = null!;

    /// <summary>
    ///     Prvy zaznam logickej tabule v rozsahu (od 1).
    /// </summary>
    public int FirstRecord { get; set; }

    /// <summary>
    ///     Posledny zaznam logickej tabule v rozsahu (od 1).
    /// </summary>
    public int LastRecord { get; set; }

    /// <summary>
    ///     Riadok fyzickej tabule, na ktory ide <see cref="FirstRecord" /> (od 1); dalsie zaznamy idu na nasledujuce riadky.
    /// </summary>
    public int StartRow { get; set; }

    /// <summary>
    ///     Typ zobrazenia umiestneni (<c>TYPE_VIEW_KEY_nnn_mmm</c>).
    /// </summary>
    public TableViewType TypeView { get; set; } = null!;

    /// <summary>
    ///     Posledny riadok fyzickej tabule, na ktory rozsah siaha (od 1).
    /// </summary>
    public int EndRow => StartRow + LastRecord - FirstRecord;

    /// <summary>
    ///     Kopia riadku.
    /// </summary>
    public TableLogicalSegment Clone() => (TableLogicalSegment)MemberwiseClone();
}

/// <summary>
///     Prevod medzi umiestneniami zaznamov logickej tabule (<see cref="TableRecord" />) a zostavou
///     (<see cref="TableLogicalSegment" />), s ktorou pracuje okno logickej tabule.
/// </summary>
public static class TableLogicalLayout
{
    /// <summary>
    ///     Rozlozi umiestnenia zaznamov na co najmenej riadkov zostavy tak, aby <see cref="ToRecords" /> vratil
    ///     presne tie iste umiestnenia v tom istom poradi. Susedne zaznamy sa spoja do jedneho riadku, ak idu na tu istu
    ///     fyzicku tabulu na susedne riadky s tym istym typom zobrazenia.
    /// </summary>
    /// <param name="records">Zaznamy logickej tabule.</param>
    /// <returns>Riadky zostavy v poradi, ktore zachova poradie umiestneni v kazdom zazname.</returns>
    public static List<TableLogicalSegment> FromRecords(IReadOnlyList<TableRecord> records)
    {
        var segments = new List<TableLogicalSegment>();

        for (var i = 0; i < records.Count; i++)
        {
            var record = i + 1;
            // ToRecords sklada umiestnenia zaznamu v poradi riadkov zostavy - predlzit sa teda da len riadok za
            // poslednym pouzitym a po zalozeni noveho riadku (ide na koniec) uz ziadny starsi
            var lastUsed = -1;
            var created = false;

            foreach (var position in records[i].Positions)
            {
                var found = -1;
                if (!created)
                    for (var s = lastUsed + 1; s < segments.Count; s++)
                    {
                        var segment = segments[s];
                        if (segment.LastRecord == record - 1 && ReferenceEquals(segment.Table, position.Table) &&
                            segment.TypeView == position.TypeView &&
                            segment.StartRow - 1 + (record - segment.FirstRecord) == position.Position)
                        {
                            found = s;
                            break;
                        }
                    }

                if (found != -1)
                {
                    segments[found].LastRecord = record;
                    lastUsed = found;
                }
                else
                {
                    segments.Add(new TableLogicalSegment
                    {
                        Table = position.Table,
                        FirstRecord = record,
                        LastRecord = record,
                        StartRow = position.Position + 1,
                        TypeView = position.TypeView
                    });
                    lastUsed = segments.Count - 1;
                    created = true;
                }
            }
        }

        return segments;
    }

    /// <summary>
    ///     Zostavi zaznamy logickej tabule zo zostavy. Umiestnenia kazdeho zaznamu su v poradi riadkov zostavy;
    ///     casti rozsahov za <paramref name="recordCount" /> sa ignoruju.
    /// </summary>
    /// <param name="segments">Riadky zostavy.</param>
    /// <param name="recordCount">Pocet zaznamov logickej tabule.</param>
    public static List<TableRecord> ToRecords(IEnumerable<TableLogicalSegment> segments, int recordCount)
    {
        var records = new List<TableRecord>(recordCount);
        for (var i = 0; i < recordCount; i++)
            records.Add(new TableRecord());

        foreach (var segment in segments)
            for (var record = Math.Max(1, segment.FirstRecord); record <= Math.Min(recordCount, segment.LastRecord); record++)
                records[record - 1].Positions.Add(new TablePosition
                {
                    Table = segment.Table,
                    Position = segment.StartRow - 1 + (record - segment.FirstRecord),
                    TypeView = segment.TypeView
                });

        return records;
    }

    /// <summary>
    ///     Porovna umiestnenia dvoch zoznamov zaznamov (pocet zaznamov, poradie umiestneni, fyzicka tabula, riadok a typ).
    /// </summary>
    /// <returns>Popis prvej odlisnosti alebo <see langword="null" />, ak su zhodne.</returns>
    public static string? FindDifference(IReadOnlyList<TableRecord> expected, IReadOnlyList<TableRecord> actual)
    {
        if (expected.Count != actual.Count)
            return $"pocet zaznamov {actual.Count}, ocakavany {expected.Count}";

        for (var i = 0; i < expected.Count; i++)
        {
            var e = expected[i].Positions;
            var a = actual[i].Positions;
            if (e.Count != a.Count)
                return $"zaznam {i + 1}: pocet umiestneni {a.Count}, ocakavany {e.Count}";

            for (var k = 0; k < e.Count; k++)
                if (!ReferenceEquals(e[k].Table, a[k].Table) || e[k].Position != a[k].Position || e[k].TypeView != a[k].TypeView)
                    return $"zaznam {i + 1}, umiestnenie {k + 1}: {Describe(a[k])}, ocakavane {Describe(e[k])}";
        }

        return null;
    }

    /// <summary>
    ///     Ci sa umiestnenia daju vyjadrit zostavou - zostava z nich musi dat presne tie iste umiestnenia a riadky
    ///     fyzickych tabul musia byt od 1 (zaporny <c>POSITION</c> okno nevie zobrazit).
    /// </summary>
    public static bool IsExpressible(IReadOnlyList<TableRecord> records, IReadOnlyList<TableLogicalSegment> segments) =>
        segments.All(segment => segment.StartRow >= 1) &&
        FindDifference(records, ToRecords(segments, records.Count)) == null;

    /// <summary>
    ///     Hlboka kopia zaznamov (nove <see cref="TableRecord" /> a <see cref="TablePosition" />, tie iste fyzicke tabule).
    /// </summary>
    public static List<TableRecord> CloneRecords(IEnumerable<TableRecord> records) =>
        records.Select(record => new TableRecord
        {
            Positions = record.Positions.Select(position => new TablePosition
            {
                Table = position.Table, Position = position.Position, TypeView = position.TypeView
            }).ToList()
        }).ToList();

    /// <summary>
    ///     Typy zobrazenia, ktore fyzicka tabula podporuje (<c>TYPE_VIEW_TAB_KEY</c> jej katalogovej tabule).
    /// </summary>
    public static List<TableViewType> SupportedViewTypes(TablePhysical table) =>
        table.TableCatalog?.ViewTypeTabs.Select(tab => tab.ViewType).Distinct().ToList() ?? new List<TableViewType>();

    private static string Describe(TablePosition position) =>
        $"{position.Table?.Key}/{position.Position}/{position.TypeView?.Key}";
}
