namespace GVDEditor.Mcp.Inputs;

/// <summary>Rezim typu zobrazenia (set_catalog_table).</summary>
internal sealed record ViewModeInput(
    [property: Description("View mode key (see table_options), e.g. LVM_Vlak.")] string Mode,
    [property: Description("Column keys shown in this mode, in order.")] IReadOnlyList<string> Columns);
