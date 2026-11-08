namespace GVDEditor.Domain.Analysis;

internal enum FixType
{
    /// <summary>
    /// Program problem opravi uplne sam automaticky.
    /// </summary>
    Auto,

    /// <summary>
    /// Pouzivatel musi vybrat jednu z ponukanych moznosti, aby chybu opravil.
    /// </summary>
    SemiAuto,

    /// <summary>
    /// Pouzivatel musi chybu opravit sam a program mu len ukaze, kde ma chybu opravit.
    /// </summary>
    Manual
}
