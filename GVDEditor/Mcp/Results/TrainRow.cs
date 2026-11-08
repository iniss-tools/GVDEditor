namespace GVDEditor.Mcp.Results;

/// <summary>Vlak v zozname (list_trains).</summary>
internal sealed record TrainRow(
    [property: Description("Train id used by get_train.")] int Id,
    [property: Description("Train number.")] string Number,
    [property: Description("Variant number; 0 = the only or base variant.")] int Variant,
    [property: Description("Train type key, e.g. Os, R, Ex.")] string? Type,
    [property: Description("Train name.")] string Name,
    [property: Description("Routing: originates, passes through or terminates here.")] string? Routing,
    [property: Description("Origin station.")] string? From,
    [property: Description("Destination station.")] string? To,
    [property: Description("Arrival time; null = originates here.")] TimeOnly? Arrival,
    [property: Description("Departure time; null = terminates here.")] TimeOnly? Departure,
    [property: Description("Track key.")] string? Track,
    [property: Description("Operator name.")] string? Operator,
    [property: Description("Calendar restriction text; empty = runs daily in its validity.")] string Validity);
