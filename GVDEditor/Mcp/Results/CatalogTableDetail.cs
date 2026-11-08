namespace GVDEditor.Mcp.Results;

/// <summary>Katalogova tabula - predloha vzhladu tabule (get_table).</summary>
internal sealed record CatalogTableDetail(string Key, string Name, string Comment,
    [property: Description("Manufacturer key - protocol of the board.")] string? Manufacturer,
    [property: Description("Most records the board can show.")] int MaxRecords,
    [property: Description("Minimal row height in points.")] int MinHeight,
    [property: Description("Number of segments (NUM_SEGMENTS).")] int Segments,
    [property: Description("Row sizes; INISS reads but does not use them.")] IReadOnlyList<RowInfo> Rows,
    IReadOnlyList<ColumnInfo> Columns,
    [property: Description("Columns shown per view type and mode.")] IReadOnlyList<ColumnOrderInfo> ColumnOrder);
