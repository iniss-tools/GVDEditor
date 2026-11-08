namespace GVDEditor.Mcp.Results;

/// <summary>Rezim typu zobrazenia.</summary>
internal sealed record ViewModeInfo(
    [property: Description("View mode key.")] string Mode,
    [property: Description("Column keys shown in this mode, in order.")] IReadOnlyList<string> Columns);
