namespace GVDEditor.Mcp.Results;

/// <summary>Vysledok list_trains.</summary>
internal sealed record TrainList(
    [property: Description("Number of trains matching the filter.")] int Total,
    IReadOnlyList<TrainRow> Trains);
