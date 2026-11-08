namespace GVDEditor.Mcp.Results;

/// <summary>Radenie vlaku.</summary>
internal sealed record TrainFormation(
    [property: Description("Text of the formation.")] string Text,
    [property: Description("Destination station of the coaches.")] string? Destination,
    [property: Description("Train number of the coaches.")] string Number,
    [property: Description("Validity of the formation; null = whole validity of the train.")] string? Validity,
    [property: Description("Announcement types it is used in.")] IReadOnlyList<string> Reports);
