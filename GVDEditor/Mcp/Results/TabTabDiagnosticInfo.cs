namespace GVDEditor.Mcp.Results;

/// <summary>Hlasenie kontroly sekcie TabTab.</summary>
internal sealed record TabTabDiagnosticInfo(
    [property: Description("error (INISS rejects the rule or logs it), warning or info.")] string Severity,
    [property: Description("Line in the section, from 1.")] int Line,
    [property: Description("Check code.")] string Code,
    string Message,
    [property: Description("Suggested fix; null = none.")] string? Suggestion);
