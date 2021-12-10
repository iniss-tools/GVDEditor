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
    public Font Font = new("Segoe UI", 9);

    /// <summary>
    ///     Pracovná plocha, stĺpec Typ vlaku - Osobný vlak.
    /// </summary>
    [XmlElement("Os")] 
    public ColorSetting Os = new() { ForeColor = Color.Black, BackColor = Color.Transparent, Bold = false };

    /// <summary>
    ///     Pracovná plocha, stĺpec Typ vlaku - Rýchlik.
    /// </summary>
    [XmlElement("R")] 
    public ColorSetting R = new() { ForeColor = Color.Red, BackColor = Color.Transparent, Bold = true };

    /// <summary>
    ///     Pracovná plocha, stĺpec Typ vlaku - Vlak vyššej kvality.
    /// </summary>
    [XmlElement("X")] 
    public ColorSetting X = new() { ForeColor = Color.Green, BackColor = Color.Transparent, Bold = true };

    /// <summary>
    ///     Pracovná plocha, stĺpec Typ vlaku - Služobný vlak.
    /// </summary>
    [XmlElement("Sl")]
    public ColorSetting Sl = new() { ForeColor = Color.OrangeRed, BackColor = Color.Transparent, Bold = false };


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