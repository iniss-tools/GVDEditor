namespace GVDEditor.Mcp.Results;

/// <summary>Stanica otvoreneho grafikonu (get_station).</summary>
internal sealed record StationInfo(
    [property: Description("Station of the timetable.")] string? Station,
    IReadOnlyList<TrackInfo> Tracks,
    [property: Description("Platforms: key - name.")] IReadOnlyList<string> Platforms,
    [property: Description("Train operators: id - name.")] IReadOnlyList<string> Operators,
    [property: Description("Train types: key - text on boards.")] IReadOnlyList<string> TrainTypes,
    [property: Description("Stations added by this timetable (not in the sound bank).")] IReadOnlyList<string> CustomStations,
    [property: Description("Languages of announcements in this timetable.")] IReadOnlyList<string> Languages);
