namespace GVDEditor.Mcp.Results;

/// <summary>Fyzicka tabula.</summary>
internal sealed record PhysicalTableInfo(int Id, string Key, string Name,
    [property: Description("Communication port (line) of the board.")] int Port,
    [property: Description("Catalog board (type) key.")] string? Catalog);
