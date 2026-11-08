namespace GVDEditor.Mcp.Results;

/// <summary>Katalogova tabula.</summary>
internal sealed record CatalogTableInfo(string Key, string Name,
    [property: Description("Manufacturer.")] string? Manufacturer,
    [property: Description("Number of segments.")] int Segments);
