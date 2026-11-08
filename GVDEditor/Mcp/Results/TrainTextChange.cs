namespace GVDEditor.Mcp.Results;

/// <summary>Zmena textu vlaku na tabuli.</summary>
internal sealed record TrainTextChange(
    [property: Description("Train id.")] int Train,
    [property: Description("Train type, number and name.")] string Label,
    [property: Description("Text before; null = the train had no text.")] string? Before,
    [property: Description("Text after (# = line break); null = the text was removed.")] string? After);
