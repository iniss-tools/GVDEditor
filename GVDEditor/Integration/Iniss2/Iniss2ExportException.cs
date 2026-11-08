namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Export sa nedal vykonat.
/// </summary>
/// <param name="issue">dovod</param>
/// <param name="detail">nazov stanice alebo grafikonu</param>
internal sealed class Iniss2ExportException(Iniss2ExportIssue issue, string detail) : Exception(detail)
{
    /// <summary>
    /// Dovod.
    /// </summary>
    public Iniss2ExportIssue Issue { get; } = issue;
}
