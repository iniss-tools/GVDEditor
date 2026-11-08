namespace GVDEditor.Mcp.Results;

/// <summary>Riadok zostavy logickej tabule.</summary>
internal sealed record LayoutRowInfo(
    [property: Description("Physical board key.")] string Physical,
    [property: Description("First record shown there, from 1.")] int FirstRecord,
    [property: Description("Last record shown there.")] int LastRecord,
    [property: Description("Row of the physical board for the first record, from 1.")] int StartRow,
    [property: Description("View type key on that board.")] string? ViewType);
