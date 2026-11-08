namespace GVDEditor.Mcp.Results;

/// <summary>Vysledok regenerate_board_texts.</summary>
internal sealed record RegenerateTextsResult(
    ChangeInfo Change,
    IReadOnlyList<BoardTextResult> Texts);
