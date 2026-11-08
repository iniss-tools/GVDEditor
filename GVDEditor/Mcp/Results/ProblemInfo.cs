namespace GVDEditor.Mcp.Results;

/// <summary>Problem z analyzy.</summary>
internal sealed record ProblemInfo(
    [property: Description("hint, warning or error.")] string Severity,
    [property: Description("What is wrong.")] string Text,
    [property: Description("How to fix it.")] string Solution,
    [property: Description("auto, semiAuto or manual - how the Analysis window in GVDEditor can fix it.")] string Fix);
