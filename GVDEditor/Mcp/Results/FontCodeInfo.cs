namespace GVDEditor.Mcp.Results;

/// <summary>Cislo pisma pouzite v sekcii.</summary>
internal sealed record FontCodeInfo(
    [property: Description("Font number as written in {n}.")] int Code,
    [property: Description("Meaning on ELEN boards: colour, blinking, tall digits, face, extended font.")] string Elen,
    [property: Description("Font of the timetable font list with this number (other manufacturers); null = none.")] string? FontList);
