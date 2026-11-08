namespace GVDEditor.Mcp.Inputs;

/// <summary>Poradie stlpcov pre typ zobrazenia (set_catalog_table).</summary>
internal sealed record ColumnOrderInput(
    [property: Description("View type key (see table_options), e.g. Tabule_Odjezdova.")] string ViewType,
    [property: Description("Column keys shown in every mode not listed in modes.")] IReadOnlyList<string>? Columns = null,
    [property: Description("Modes with their own columns; the other modes get columns.")] IReadOnlyList<ViewModeInput>? Modes = null,
    [property: Description("Board rows per record.")] int LinesPerRecord = 1);
