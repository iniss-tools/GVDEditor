using System.Xml.Serialization;

namespace GVDEditor.XML;

/// <summary>
///     Velkost okna, ktoru si program pamata medzi spusteniami. Rozmery su v bodoch pri 96 DPI, aby sa okno
///     na monitore s inym zvacsenim otvorilo rovnako velke.
/// </summary>
public sealed class WindowPlacement
{
    /// <summary>
    ///     Sirka okna (pri 96 DPI).
    /// </summary>
    [XmlAttribute("Width")]
    public int Width { get; set; }

    /// <summary>
    ///     Vyska okna (pri 96 DPI).
    /// </summary>
    [XmlAttribute("Height")]
    public int Height { get; set; }

    /// <summary>
    ///     Okno bolo maximalizovane.
    /// </summary>
    [XmlAttribute("Maximized")]
    public bool Maximized { get; set; }

    /// <summary>
    ///     Kopia.
    /// </summary>
    public WindowPlacement Clone() => new() { Width = Width, Height = Height, Maximized = Maximized };
}
