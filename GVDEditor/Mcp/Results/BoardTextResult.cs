namespace GVDEditor.Mcp.Results;

/// <summary>Texty jedneho typu textu na tabuliach po generovani.</summary>
internal sealed record BoardTextResult(
    [property: Description("Board text type key.")] string Key,
    [property: Description("Board text type name.")] string Name,
    [property: Description("Catalog board and column the texts are generated for (catalog/column); null = no realization in a column showing the destination "
                           + "or stations on the way - the type was skipped.")] string? Column,
    [property: Description("Trains that got a text.")] int Added,
    [property: Description("Trains whose text or font changed (the font is reset to the default).")] int Changed,
    [property: Description("Texts removed (the train is no longer in the timetable).")] int Removed,
    [property: Description("First changes in train order (at most 20).")] IReadOnlyList<TrainTextChange> Changes);
