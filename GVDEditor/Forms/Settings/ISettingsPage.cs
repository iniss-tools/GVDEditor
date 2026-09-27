namespace GVDEditor.Forms.Settings;

/// <summary>
///     Stranka okna nastaveni, ktora pri kazdej zmene sama skontroluje svoje udaje a chyby oznaci priamo pri poliach.
///     Okno podla nej ukaze chybu v spodnom riadku a pri OK prepne na stranku s chybou.
/// </summary>
internal interface ISettingsPage
{
    /// <summary>
    ///     Zmenili sa chyby stranky.
    /// </summary>
    event EventHandler? ProblemsChanged;

    /// <summary>
    ///     Prva chyba, ktora brani ulozeniu; <see langword="null" />, ak je stranka v poriadku.
    /// </summary>
    string? FirstProblem { get; }

    /// <summary>
    ///     Presunie fokus na pole s prvou chybou.
    /// </summary>
    void FocusFirstProblem();
}
