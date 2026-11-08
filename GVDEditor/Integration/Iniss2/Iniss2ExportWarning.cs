namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Upozornenie exportu.
/// </summary>
/// <param name="Issue">druh</param>
/// <param name="Detail">cislo vlaku alebo nazov stanice</param>
internal sealed record Iniss2ExportWarning(Iniss2ExportIssue Issue, string Detail);
