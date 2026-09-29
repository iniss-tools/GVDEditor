namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranky okna Lokalne nastavenia, ktore sa daju otvorit priamo (menu Vlastnosti, analyza grafikonu).
/// </summary>
public enum LocalSettingsPage
{
    Grafikon,
    JazykyHlaseni,
    VlastneStanice,
    Dopravcovia,
    Nastupistia,
    Kolaje,
    FyzickeTabule,
    LogickeTabule,
    KatalogoveTabule,
    TabTab,
    Texty,
    Pisma,
    StavovyDiagram
}

/// <summary>
/// Editor, ktory sa ma otvorit hned po otvoreni okna Lokalne nastavenia.
/// </summary>
public enum LocalSettingsAction
{
    None,
    OpenTabTabEditor,
    OpenStateDgmEditor
}

/// <summary>
/// Stranky okna Globalne nastavenia.
/// </summary>
public enum GlobalSettingsPage
{
    Grafikony,
    Jazyky,
    Meskania,
    TypyVlakov,
    Audio
}
