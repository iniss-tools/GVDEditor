using System.Xml.Serialization;

namespace GVDEditor.Config;

/// <summary>
/// Velkost okna, ktoru si program pamata medzi spusteniami. Rozmery su v bodoch pri 96 DPI, aby sa okno
/// na monitore s inym zvacsenim otvorilo rovnako velke.
/// </summary>
public sealed class WindowPlacement
{
    /// <summary>
    /// Sirka okna (pri 96 DPI).
    /// </summary>
    [XmlAttribute("Width")]
    public int Width { get; set; }

    /// <summary>
    /// Vyska okna (pri 96 DPI).
    /// </summary>
    [XmlAttribute("Height")]
    public int Height { get; set; }

    /// <summary>
    /// Okno bolo maximalizovane.
    /// </summary>
    [XmlAttribute("Maximized")]
    public bool Maximized { get; set; }

    /// <summary>
    /// Kopia.
    /// </summary>
    public WindowPlacement Clone() => new() { Width = Width, Height = Height, Maximized = Maximized };
}

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
