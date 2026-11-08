namespace GVDEditor.Mcp.Results;

/// <summary>Logicka tabula.</summary>
internal sealed record LogicalTableInfo(string Key, string Name,
    [property: Description("View type.")] string? View,
    [property: Description("Physical boards (keys) by record and position.")] IReadOnlyList<string> Physicals);
