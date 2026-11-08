namespace GVDEditor.Mcp.Results;

/// <summary>Vysledok preview_board_text.</summary>
internal sealed record BoardPreview(
    [property: Description("The train the texts are composed for.")] TrainRow Train,
    IReadOnlyList<BoardColumnText> Columns);
