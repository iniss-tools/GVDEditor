using GVDEditor.Domain.Entities;
using GVDEditor.UI.Settings;

namespace GVDEditor.Domain.Analysis;

/// <summary>
/// Hlavne okno z pohladu analyzy - opravy, ktore potrebuju pouzivatela (nastavenia, editor TabTab).
/// </summary>
internal interface IAnalyzerHost
{
    /// <summary>
    /// Otvori lokalne nastavenia na danej stranke a vyberie polozku.
    /// </summary>
    /// <returns><see langword="true" />, ak pouzivatel nastavenia ulozil.</returns>
    bool ShowLocalSettings(LocalSettingsPage page = LocalSettingsPage.Grafikon, LocalSettingsAction action = LocalSettingsAction.None,
        object? select = null);

    /// <summary>
    /// Otvori editor TabTab so sekciou <paramref name="tabTab" />.
    /// </summary>
    void EditTabTab(TableTabTab tabTab);

    /// <summary>
    /// Otvori okno vlaku.
    /// </summary>
    /// <returns><see langword="true" />, ak pouzivatel vlak ulozil.</returns>
    bool EditTrain(Train train);

    /// <summary>
    /// Otvori okno Nastavenia INISSu (vybrana konfiguracia spustania) na sekcii <paramref name="section" />.
    /// </summary>
    void ShowInissSettings(string? section);
}
