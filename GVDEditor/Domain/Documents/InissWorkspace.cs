using ExControls;
using GVDEditor.Domain.Entities;
using ToolsCore.Entities;

namespace GVDEditor.Domain.Documents;

/// <summary>
/// Zvolena instalacia INISS - cesty, zoznam grafikonov, zvukova banka a globalne nastavenia stanice.
/// Plati od zvolenia priecinka INISS po zvolenie ineho; otvoreny grafikon je zvlast v <see cref="GrafikonDocument" />.
/// </summary>
internal sealed class InissWorkspace
{
    public string INISSDir { get; set; } = null!;
    public string DataDir { get; set; } = null!;
    public string RawBankDir { get; set; } = null!;

    /// <summary>
    /// Spustitelne subory v priecinku INISS (ponuka pri spustani INISS).
    /// </summary>
    public List<string> INISSExeFiles { get; set; } = null!;

    /// <summary>
    /// Zoznam grafikonov (DirList.txt).
    /// </summary>
    public List<DirList> GVDDirs { get; set; } = null!;

    public ExBindingList<Audio> Audios { get; set; } = null!;

    /// <summary>
    /// Zvuky hlavneho jazyka banky.
    /// </summary>
    public List<FyzSound> Sounds { get; set; } = null!;

    /// <summary>
    /// Texty vyluk, odklonov a dodatkov zalozene obsluhou v INISSe (RAWBANK\LogZvuk.usr).
    /// </summary>
    public List<LogZvukText> LogZvukTexts { get; set; } = [];

    /// <summary>
    /// Jazyky stanice (globalny Categori.txt).
    /// </summary>
    public ExBindingList<FyzLanguage> Languages { get; set; } = null!;

    /// <summary>
    /// Stanice zo zvukovej banky (skupina R1).
    /// </summary>
    public List<Station> Stations { get; set; } = null!;

    public ExBindingList<string> Delays { get; set; } = null!;
    public ExBindingList<TrainType> TrainsTypes { get; set; } = null!;

    /// <summary>
    /// Nazvy vlakov zo zvukovej banky (skupina V8).
    /// </summary>
    public List<TrainName> TrainNames { get; set; } = null!;
}
