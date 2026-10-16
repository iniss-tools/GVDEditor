namespace GVDEditor.Mcp;

/// <summary>Riadok katalogovej tabule (set_catalog_table).</summary>
internal sealed record CatalogRowInput(
    [property: Description("Row height in points (usually the minimal row height).")] int Height,
    [property: Description("Row width in points (width of the board).")] int Width,
    [property: Description("Size kept for the original configuration tool (typically 15); INISS ignores it.")] int Size = 15);

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

/// <summary>Rezim typu zobrazenia (set_catalog_table).</summary>
internal sealed record ViewModeInput(
    [property: Description("View mode key (see table_options), e.g. LVM_Vlak.")] string Mode,
    [property: Description("Column keys shown in this mode, in order.")] IReadOnlyList<string> Columns);

/// <summary>Poradie stlpcov pre typ zobrazenia (set_catalog_table).</summary>
internal sealed record ColumnOrderInput(
    [property: Description("View type key (see table_options), e.g. Tabule_Odjezdova.")] string ViewType,
    [property: Description("Column keys shown in every mode not listed in modes.")] IReadOnlyList<string>? Columns = null,
    [property: Description("Modes with their own columns; the other modes get columns.")] IReadOnlyList<ViewModeInput>? Modes = null,
    [property: Description("Board rows per record.")] int LinesPerRecord = 1);

/// <summary>Riadok zostavy logickej tabule (set_logical_table).</summary>
internal sealed record LayoutRowInput(
    [property: Description("Physical board key.")] string Physical,
    [property: Description("First record of the logical board shown there, from 1.")] int FirstRecord,
    [property: Description("Last record shown there (records first..last go to consecutive rows).")] int LastRecord,
    [property: Description("Row of the physical board for the first record, from 1.")] int StartRow = 1,
    [property: Description("View type key on that board; null = the view type of the logical board.")] string? ViewType = null);
