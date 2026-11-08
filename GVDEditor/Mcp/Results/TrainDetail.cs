namespace GVDEditor.Mcp.Results;

/// <summary>Vlak so vsetkymi udajmi (get_train).</summary>
internal sealed record TrainDetail(
    TrainRow Train,
    [property: Description("First day of validity.")] DateOnly ValidFrom,
    [property: Description("Last day of validity.")] DateOnly ValidTo,
    [property: Description("Stations before this station (where the train comes from), in order.")] IReadOnlyList<RouteStop> StationsBefore,
    [property: Description("Stations after this station (where the train goes), in order.")] IReadOnlyList<RouteStop> StationsAfter,
    [property: Description("Departure track when different from the arrival track.")] string? TrackDeparture,
    [property: Description("Line of arrival.")] string? LineArrival,
    [property: Description("Line of departure.")] string? LineDeparture,
    [property: Description("Languages of announcements for this train besides the basic one (keys).")] IReadOnlyList<string> Languages,
    [property: Description("Set flags: international, longDistance, extraordinary, reservation, sleeperOnly, lowFloor, transfer, flagO, diesel.")]
    IReadOnlyList<string> Flags,
    [property: Description("Lockout number; 0 = none.")] int Lockout,
    IReadOnlyList<TrainSupplement> Supplements,
    IReadOnlyList<TrainFormation> Formations);
