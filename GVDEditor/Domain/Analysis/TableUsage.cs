using GVDEditor.Domain.Entities;

namespace GVDEditor.Domain.Analysis;

/// <summary>
/// Kde sa tabule pouzivaju - podla toho sa rozhoduje, ci sa tabula da odstranit.
/// </summary>
internal static class TableUsage
{
    /// <summary>
    /// Pozicie logickych tabul, na ktore sa posiela obsah fyzickej tabule <paramref name="table" />.
    /// </summary>
    public static IEnumerable<(TableLogical Logical, int Position)> LogicalPositions(TablePhysical table,
        IEnumerable<TableLogical> logicals)
    {
        foreach (var logical in logicals)
        {
            foreach (var record in logical.Records)
            {
                foreach (TablePosition position in record)
                {
                    if (ReferenceEquals(position.Table, table))
                        yield return (logical, position.Position);
                }
            }
        }
    }

    /// <summary>
    /// Fyzicke tabule s predlohou <paramref name="catalog" /> a texty zobrazovane v jej stlpcoch.
    /// </summary>
    public static (List<TablePhysical> Physicals, List<(TableText Text, TableItem Column)> Texts) CatalogUsage(TableCatalog catalog,
        IEnumerable<TablePhysical> physicals, IEnumerable<TableText> texts) =>
        (physicals.Where(p => ReferenceEquals(p.TableCatalog, catalog)).ToList(),
            texts.SelectMany(text => text.Realizations.Where(r => ReferenceEquals(r.Table, catalog)).Select(r => (text, r.Item))).ToList());

    /// <summary>
    /// Kolaje, ktorych vlaky sa zobrazuju na logickej tabuli <paramref name="logical" />.
    /// </summary>
    public static List<Track> Tracks(TableLogical logical, IEnumerable<Track> tracks) =>
        tracks.Where(track => track.Tables.Any(t => ReferenceEquals(t, logical))).ToList();
}
