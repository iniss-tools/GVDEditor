namespace GVDEditor.Mcp.Results;

/// <summary>Otvoreny grafikon (get_grafikon).</summary>
internal sealed record GrafikonSummary(
    [property: Description("INISS installation folder; empty = none chosen.")] string InissFolder,
    [property: Description("Folder of the open timetable; null = no timetable open.")] string? Folder,
    [property: Description("Station of the timetable.")] string? Station,
    [property: Description("First day the timetable is valid.")] DateOnly? ValidFrom,
    [property: Description("Last day the timetable is valid.")] DateOnly? ValidTo,
    [property: Description("Number of trains (with variants).")] int Trains);
