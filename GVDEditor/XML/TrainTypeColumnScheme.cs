using System.Xml.Serialization;
using ToolsCore.XML;

namespace GVDEditor.XML;

/// <summary>
///     Spracovanie fontu a jeho farby pre zobrazenie na pracovnej ploche programu.
/// </summary>
public class TrainTypeColumnScheme
{
    /// <summary>
    ///     Font typu vlaku na pracovnej ploche v časti Typ vlaku.
    /// </summary>
    [XmlIgnore] 
    public Font Font { get; set; } = new("Segoe UI", 9);

    /// <summary>
    ///     Pracovná plocha, stĺpec Typ vlaku - Osobný vlak.
    /// </summary>
    [XmlElement("Os")] 
    public ColorSetting Os { get; set; } = new() { ForeColor = Color.Black, BackColor = Color.Transparent, Bold = false };

    /// <summary>
    ///     Pracovná plocha, stĺpec Typ vlaku - Rýchlik.
    /// </summary>
    [XmlElement("R")] 
    public ColorSetting R { get; set; } = new() { ForeColor = Color.Red, BackColor = Color.Transparent, Bold = true };

    /// <summary>
    ///     Pracovná plocha, stĺpec Typ vlaku - Vlak vyššej kvality.
    /// </summary>
    [XmlElement("X")] 
    public ColorSetting X { get; set; } = new() { ForeColor = Color.Green, BackColor = Color.Transparent, Bold = true };

    /// <summary>
    ///     Pracovná plocha, stĺpec Typ vlaku - Služobný vlak.
    /// </summary>
    [XmlElement("Sl")]
    public ColorSetting Sl { get; set; } = new() { ForeColor = Color.OrangeRed, BackColor = Color.Transparent, Bold = false };


    /// <summary>
    ///     Font typu vlaku na pracovnej ploche v časti Typ vlaku vo formate XML.
    /// </summary>
    [XmlElement(Type = typeof(XMLFont), ElementName = "Font")]
    public XMLFont FontXML
    {
        get => XMLFont.FromFont(Font);
        set => Font = XMLFont.ToFont(value);
    }
}