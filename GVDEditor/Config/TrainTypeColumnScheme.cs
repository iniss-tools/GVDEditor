using System.Xml.Serialization;
using GVDEditor.Properties;
using ToolsCore.XML;

namespace GVDEditor.Config;

/// <summary>
/// Spracovanie fontu a jeho farby pre zobrazenie na pracovnej ploche programu.
/// </summary>
public record TrainTypeColumnScheme() : IColorScheme
{
    /// <inheritdoc />
    [XmlIgnore]
    public string Name => Resources.Scheme_TrainTypeColumn;

    [XmlIgnore]
    private static readonly Dictionary<string, ColorSetting> Props = new()
    {
        [nameof(Os)] = new(Color.Transparent, Color.Black, true) { NameSource = () => "Os", Bold = false },
        [nameof(R)] = new(Color.Transparent, Color.Red, true) { NameSource = () => "R", Bold = true },
        [nameof(X)] = new(Color.Transparent, Color.Green, true) { NameSource = () => "X", Bold = true },
        [nameof(Sl)] = new(Color.Transparent, Color.OrangeRed, true) { NameSource = () => "Sl", Bold = false },
    };

    #region Properties

    /// <summary>
    /// Pracovná plocha, stĺpec Typ vlaku - Osobný vlak.
    /// </summary>
    [XmlElement("Os")]
    public ColorSetting Os
    {
        get => field ??= InitProperty(nameof(Os));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(Os));
        }
    } = InitProperty(nameof(Os));

    /// <summary>
    /// Pracovná plocha, stĺpec Typ vlaku - Rýchlik.
    /// </summary>
    [XmlElement("R")]
    public ColorSetting R
    {
        get => field ??= InitProperty(nameof(R));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(R));
        }
    } = InitProperty(nameof(R));

    /// <summary>
    /// Pracovná plocha, stĺpec Typ vlaku - Vlak vyššej kvality.
    /// </summary>
    [XmlElement("X")]
    public ColorSetting X
    {
        get => field ??= InitProperty(nameof(X));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(X));
        }
    } = InitProperty(nameof(X));

    /// <summary>
    /// Pracovná plocha, stĺpec Typ vlaku - Služobný vlak.
    /// </summary>
    [XmlElement("Sl")]
    public ColorSetting Sl
    {
        get => field ??= InitProperty(nameof(Sl));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(Sl));
        }
    } = InitProperty(nameof(Sl));

    #endregion

    private static ColorSetting InitProperty(string propname) => Props[propname] with { };

    private static void AssignProperty(ref ColorSetting prop, string propname)
    {
        if (prop is null)
            prop = InitProperty(propname);
        else
        {
            prop.NameSource = Props[propname].NameSource;
            prop.DisableBackColorEdit = Props[propname].DisableBackColorEdit;
            prop.DisableFontBoldEdit = Props[propname].DisableFontBoldEdit;
        }
    }
    
    protected TrainTypeColumnScheme(TrainTypeColumnScheme original)
    {
        if (original.Os != null) Os = original.Os with { };
        if (original.R != null) R = original.R with { };
        if (original.X != null) X = original.X with { };
        if (original.Sl != null) Sl = original.Sl with { };
    }
}