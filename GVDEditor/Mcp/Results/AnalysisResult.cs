namespace GVDEditor.Mcp.Results;

/// <summary>Vysledok analyzy (analyze_grafikon).</summary>
internal sealed record AnalysisResult(
    [property: Description("Number of errors.")] int Errors,
    [property: Description("Number of warnings.")] int Warnings,
    IReadOnlyList<ProblemInfo> Problems);
