namespace GVDEditor.Mcp.Results;

/// <summary>Grafikon instalacie (list_grafikons).</summary>
internal sealed record GrafikonFolder(
    [property: Description("Timetable folder name.")] string Folder,
    [property: Description("Station; null = the folder has no readable timetable header.")] string? Station,
    [property: Description("First day the timetable is valid.")] DateOnly? ValidFrom,
    [property: Description("Last day the timetable is valid.")] DateOnly? ValidTo,
    [property: Description("Number of trains in the saved timetable.")] int? Trains,
    [property: Description("The timetable is open in GVDEditor.")] bool Open);
