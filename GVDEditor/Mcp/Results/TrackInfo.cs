namespace GVDEditor.Mcp.Results;

/// <summary>Koľaj stanice.</summary>
internal sealed record TrackInfo(
    [property: Description("Track key used in train records.")] string Key,
    [property: Description("Track name.")] string Name,
    [property: Description("Platform key.")] string? Platform,
    [property: Description("Logical tables (keys) showing trains on this track.")] IReadOnlyList<string> Tables);
