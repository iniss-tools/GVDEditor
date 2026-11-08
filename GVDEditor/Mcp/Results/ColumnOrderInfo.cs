namespace GVDEditor.Mcp.Results;

/// <summary>Poradie stlpcov pre typ zobrazenia.</summary>
internal sealed record ColumnOrderInfo(
    [property: Description("View type key.")] string ViewType,
    [property: Description("Board rows per record.")] int LinesPerRecord,
    IReadOnlyList<ViewModeInfo> Modes);
