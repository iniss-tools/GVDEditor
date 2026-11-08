namespace GVDEditor.Mcp.Inputs;

/// <summary>Riadok zostavy logickej tabule (set_logical_table).</summary>
internal sealed record LayoutRowInput(
    [property: Description("Physical board key.")] string Physical,
    [property: Description("First record of the logical board shown there, from 1.")] int FirstRecord,
    [property: Description("Last record shown there (records first..last go to consecutive rows).")] int LastRecord,
    [property: Description("Row of the physical board for the first record, from 1.")] int StartRow = 1,
    [property: Description("View type key on that board; null = the view type of the logical board.")] string? ViewType = null);
