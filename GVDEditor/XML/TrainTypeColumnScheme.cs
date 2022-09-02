using System.Xml.Serialization;
using ToolsCore.XML;

namespace GVDEditor.XML;

/// <summary>
///     Spracovanie fontu a jeho farby pre zobrazenie na pracovnej ploche programu.
/// </summary>
public record TrainTypeColumnScheme() : IColorScheme
{
    /// <inheritdoc />
    [XmlIgnore]
    public bool DisableFontEdit => false;

    /// <summary>
    ///     Font typu vlaku na pracovnej ploche v časti Typ vlaku.
    /// </summary>
    [XmlIgnore] 
    public Font Font { get; set; } = new("Segoe UI", 9);

    /// <summary>
    ///     Font typu vlaku na pracovnej ploche v časti Typ vlaku vo formate XML.
    /// </summary>
    [XmlElement(Type = typeof(XmlFont), ElementName = "Font")]
    public XmlFont FontXML
    {
        get => XmlFont.FromFont(Font);
        set => Font = XmlFont.ToFont(value);
    }

    /// <inheritdoc />
    [XmlIgnore]
    public string Name => "Stĺpec typ vlaku";

    [XmlIgnore]
    private static readonly Dictionary<string, ColorSetting> props = new()
    {
        [nameof(Os)] = new(Color.Transparent, Color.Black, true) { Name = "Os", Bold = false },
        [nameof(R)] = new(Color.Transparent, Color.Red, true) { Name = "R", Bold = true },
        [nameof(X)] = new(Color.Transparent, Color.Green, true) { Name = "X", Bold = true },
        [nameof(Sl)] = new(Color.Transparent, Color.OrangeRed, true) { Name = "Sl", Bold = false },
    };

    #region Fields

    private ColorSetting _os = InitProperty(nameof(Os));
    private ColorSetting _r = InitProperty(nameof(R));
    private ColorSetting _x = InitProperty(nameof(X));
    private ColorSetting _sl = InitProperty(nameof(Sl));

    #endregion

    #region Properties

    /// <summary>
    ///     Pracovná plocha, stĺpec Typ vlaku - Osobný vlak.
    /// </summary>
    [XmlElement("Os")]
    public ColorSetting Os
    {
        get => _os ??= InitProperty(nameof(Os));
        set
        {
            _os = value;
            AssignProperty(ref _os, nameof(Os));
        }
    }

    /// <summary>
    ///     Pracovná plocha, stĺpec Typ vlaku - Rýchlik.
    /// </summary>
    [XmlElement("R")]
    public ColorSetting R
    {
        get => _r ??= InitProperty(nameof(R));
        set
        {
            _r = value;
            AssignProperty(ref _r, nameof(R));
        }
    }

    /// <summary>
    ///     Pracovná plocha, stĺpec Typ vlaku - Vlak vyššej kvality.
    /// </summary>
    [XmlElement("X")]
    public ColorSetting X
    {
        get => _x ??= InitProperty(nameof(X));
        set
        {
            _x = value;
            AssignProperty(ref _x, nameof(X));
        }
    }

    /// <summary>
    ///     Pracovná plocha, stĺpec Typ vlaku - Služobný vlak.
    /// </summary>
    [XmlElement("Sl")]
    public ColorSetting Sl
    {
        get => _sl ??= InitProperty(nameof(Sl));
        set
        {
            _sl = value;
            AssignProperty(ref _sl, nameof(Sl));
        }
    }

    #endregion

    private static ColorSetting InitProperty(string propname) => props[propname] with { };

    private static void AssignProperty(ref ColorSetting prop, string propname)
    {
        if (prop is null)
            InitProperty(propname);
        else
        {
            prop.Name = props[propname].Name;
            prop.DisableBackColorEdit = props[propname].DisableBackColorEdit;
            prop.DisableFontBoldEdit = props[propname].DisableFontBoldEdit;
        }
    }

    protected TrainTypeColumnScheme(TrainTypeColumnScheme original)
    {
        Os = original.Os with { };
        R = original.R with { };
        X = original.X with { };
        Sl = original.Sl with { };
        Font = (Font) original.Font.Clone();
    }
}