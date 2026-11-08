namespace GVDEditor.Mcp.Results;

/// <summary>Vyklad textu platnosti (explain_validity).</summary>
internal sealed record ValidityDays(
    [property: Description("The text as GVDEditor writes it.")] string Normalized,
    [property: Description("Number of days the train runs.")] int Days,
    [property: Description("Days the train runs as continuous ranges.")] IReadOnlyList<DayRange> Ranges);
