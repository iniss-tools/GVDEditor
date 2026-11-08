using System.Xml.Serialization;

namespace GVDEditor.Config;

/// <summary>
/// Zapamatane velkosti okien programu (prvok <c>Windows</c> v konfiguracii); <see langword="null" /> = predvolena
/// velkost z navrhu.
/// </summary>
public sealed class WindowPlacements
{
    /// <summary>
    /// Okno Lokalne nastavenia.
    /// </summary>
    [XmlElement("LocalSettings")]
    public WindowPlacement? LocalSettings { get; set; }

    /// <summary>
    /// Okno Globalne nastavenia.
    /// </summary>
    [XmlElement("GlobalSettings")]
    public WindowPlacement? GlobalSettings { get; set; }

    /// <summary>
    /// Okno Nastavenia INISSu.
    /// </summary>
    [XmlElement("InissSettings")]
    public WindowPlacement? InissSettings { get; set; }

    /// <summary>
    /// Okno Upravit vlak.
    /// </summary>
    [XmlElement("EditTrain")]
    public WindowPlacement? EditTrain { get; set; }

    /// <summary>
    /// Kopia.
    /// </summary>
    public WindowPlacements Clone() => new()
    {
        LocalSettings = LocalSettings?.Clone(),
        GlobalSettings = GlobalSettings?.Clone(),
        InissSettings = InissSettings?.Clone(),
        EditTrain = EditTrain?.Clone()
    };
}
