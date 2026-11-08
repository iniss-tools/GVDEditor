namespace GVDEditor.Mcp.Results;

/// <summary>Vysledny text stlpca tabule pre vlak (preview_board_text).</summary>
internal sealed record BoardColumnText(
    [property: Description("Catalog board key.")] string Catalog,
    [property: Description("Column (item) name.")] string Column,
    [property: Description("DIVTYPE 0-4.")] int DivType,
    [property: Description("TAB1 section key; null = none.")] string? Tab1,
    [property: Description("TAB2 section key; null = none.")] string? Tab2,
    [property: Description("The value the column computes itself.")] string Own,
    [property: Description("Text sent to the board; # separates alternating parts.")] string Text,
    [property: Description("Font number of the result; null = column font.")] int? Font,
    [property: Description("Evaluation error (e.g. division by zero); null = none.")] string? Error,
    IReadOnlyList<ComposeStepInfo> Steps);
