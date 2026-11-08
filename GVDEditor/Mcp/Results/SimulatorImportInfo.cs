namespace GVDEditor.Mcp.Results;

/// <summary>Co simulator pri priprave urobil.</summary>
internal sealed record SimulatorImportInfo(
    int LinesAdded,
    int LinesUpdated,
    int BoardsAdded,
    [property: Description("Boards the simulator already had - they stay as they are.")] int BoardsKept,
    int VirtualBoardsAdded,
    int VirtualBoardsKept,
    [property: Description("Lines whose class the simulator does not know.")] IReadOnlyList<int> UnsupportedLines,
    [property: Description("Boards the simulator skipped.")] IReadOnlyList<string> SkippedBoards,
    [property: Description("INISS log folder for virtual boards: none, added, watched (already), missing (does not exist), full (no more folders).")]
    string LogFolder);
