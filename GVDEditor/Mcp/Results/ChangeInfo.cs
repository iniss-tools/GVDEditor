namespace GVDEditor.Mcp.Results;

/// <summary>Zmena grafikonu agentom - spolocna cast vysledku nastrojov so zmenami.</summary>
internal sealed record ChangeInfo(
    [property: Description("Dry run only - the timetable stays as it was.")] bool DryRun,
    [property: Description("The timetable is (or would be) different; false = it already was in that state.")] bool Changed,
    [property: Description("Undo step the user sees in GVDEditor (Edit > Undo); null = no step. The change is not saved - the user saves the timetable.")]
    string? UndoStep);
