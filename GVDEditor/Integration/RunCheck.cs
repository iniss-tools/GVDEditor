namespace GVDEditor.Integration;

/// <summary>
/// Zistenie pri konfiguracii spustania.
/// </summary>
internal sealed record RunCheck(RunCheckSeverity Severity, string Text);
