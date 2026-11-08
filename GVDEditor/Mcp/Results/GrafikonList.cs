namespace GVDEditor.Mcp.Results;

/// <summary>Grafikony instalacie.</summary>
internal sealed record GrafikonList(
    [property: Description("INISS installation folder; empty = none chosen.")] string InissFolder,
    IReadOnlyList<GrafikonFolder> Grafikons);
