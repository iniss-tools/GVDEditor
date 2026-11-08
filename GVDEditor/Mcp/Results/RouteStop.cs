namespace GVDEditor.Mcp.Results;

/// <summary>Stanica na trase vlaku.</summary>
internal sealed record RouteStop(
    [property: Description("Station name.")] string Station,
    [property: Description("Station number.")] string Id,
    [property: Description("Announced in the short announcement.")] bool Short,
    [property: Description("Announced in the long announcement.")] bool Long);
