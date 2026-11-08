namespace GVDEditor.Mcp.Results;

/// <summary>Hlasenie (dodatok) vlaku.</summary>
internal sealed record TrainSupplement(
    [property: Description("Supplement name.")] string Name,
    [property: Description("Sound of the supplement.")] string? Sound,
    [property: Description("Announcement types it is used in.")] IReadOnlyList<string> Reports);
