namespace GVDEditor.Domain.Analysis;

internal enum ProblemType
{
    /// <summary>
    /// Len informacia pre pouzivatela. Grafikon je uplne funkcny.
    /// </summary>
    Hint,

    /// <summary>
    /// Grafikon nemusi fungovat uplne spravne, ale je spustitelny.
    /// </summary>
    Warning,

    /// <summary>
    /// Zavazna chyba v grafikone. INISS pravdepodobne nespusti tento grafikon.
    /// </summary>
    Error
}
