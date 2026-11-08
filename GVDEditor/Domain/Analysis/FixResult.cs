namespace GVDEditor.Domain.Analysis;

internal enum FixResult
{
    /// <summary>
    /// Ak bola chyba opravena.
    /// </summary>
    Done,

    /// <summary>
    /// Ak pouzivatel chybu neopravil.
    /// </summary>
    NotSolved,

    /// <summary>
    /// Ak pocas opravy doslo k chybe.
    /// </summary>
    Error
}
