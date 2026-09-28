using GVDEditor.Domain.Entities;

namespace GVDEditor.Domain.Analysis;

/// <summary>
///     Kde sa tabule pouzivaju - podla toho sa rozhoduje, ci sa tabula da odstranit.
/// </summary>
internal static class TableUsage
{
    /// <summary>
    ///     Pozicie logickych tabul, na ktore sa posiela obsah fyzickej tabule <paramref name="table" />.
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
}
