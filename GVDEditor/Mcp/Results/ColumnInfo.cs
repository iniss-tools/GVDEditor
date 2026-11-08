namespace GVDEditor.Mcp.Results;

/// <summary>Stlpec katalogovej tabule (get_table).</summary>
internal sealed record ColumnInfo(
    [property: Description("Column key; board texts and the column order refer to it.")] string Key,
    string Name,
    [property: Description("Content number (FILL_SECTION, see table_options).")] int Content,
    [property: Description("Content name.")] string ContentName,
    [property: Description("Row of the board, from 0.")] int Line,
    [property: Description("Start in points.")] int Start,
    [property: Description("End in points.")] int End,
    [property: Description("Font number.")] int Font,
    [property: Description("Alignment number: 0 left, 1 right, 2 centre.")] int Align,
    [property: Description("DIVTYPE 0-4 - how TAB1/TAB2 apply.")] int DivType,
    [property: Description("TAB1 section key; null = none.")] string? Tab1,
    [property: Description("TAB2 section key; null = none.")] string? Tab2,
    [property: Description("Board texts (keys) shown in this column; such a column cannot be removed.")] IReadOnlyList<string> Texts);
