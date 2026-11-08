namespace GVDEditor.Mcp.Inputs;

/// <summary>Stlpec katalogovej tabule (set_catalog_table).</summary>
internal sealed record CatalogColumnInput(
    [property: Description("Column key; board texts and the column order refer to it.")] string Key,
    [property: Description("Column name for the operator.")] string Name,
    [property: Description("Start of the column in points.")] int Start,
    [property: Description("End of the column in points (greater than start).")] int End,
    [property: Description("Font number of the column.")] int Font,
    [property: Description("Content number (FILL_SECTION, see table_options); 1 = free text.")] int Content = 1,
    [property: Description("Row of the board the column is on, from 0.")] int Line = 0,
    [property: Description("Alignment number (see table_options); 0 = left.")] int Align = 0,
    [property: Description("DIVTYPE 0-4 - how TAB1/TAB2 apply (see tabtab_reference); 0 = text as it is.")] int DivType = 0,
    [property: Description("TabTab section key used as TAB1; null = none.")] string? Tab1 = null,
    [property: Description("TabTab section key used as TAB2 (DIVTYPE 2); null = none.")] string? Tab2 = null,
    [property: Description("Current key of an existing column when the key changes; null = the column is matched by key.")] string? PreviousKey = null);
