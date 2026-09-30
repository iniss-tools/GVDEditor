using System.Reflection;
using System.Xml.Serialization;
using GVDEditor.Properties;
using ToolsCore.XML;

namespace GVDEditor.Config;

/// <summary>
/// Obsahuje zoznam všetkých možných stĺpcov pre tabuľku na pracovnej ploche programu.
/// </summary>
public record DesktopColumns()
{
    private static readonly Type ClassType = typeof(DesktopColumns);

    [XmlIgnore]
    private static readonly Dictionary<string, (string name, int order, int minWidth, bool visible)> Props = new()
    {
        [nameof(Number)] = (Resources.Column_Number, 0, 60, true),
        [nameof(Type)] = (Resources.Column_Type, 1, 40, true),
        [nameof(Name)] = (Resources.Column_Name, 2, 100, true),
        [nameof(LinkaPrichod)] = (Resources.Column_LineArrival, 3, 50, false),
        [nameof(LinkaOdchod)] = (Resources.Column_LineDeparture, 4, 50, false),
        [nameof(Routing)] = (Resources.Column_Routing, 5, 100, true),
        [nameof(Prichod)] = (Resources.Column_Arrival, 6, 60, true),
        [nameof(Odchod)] = (Resources.Column_Departure, 7, 60, true),
        [nameof(VychodziaStanica)] = (Resources.Column_StartStation, 8, 120, true),
        [nameof(KonecnaStanica)] = (Resources.Column_EndStation, 9, 120, true),
        [nameof(DateLimit)] = (Resources.Column_DateLimit, 10, 300, true),
        [nameof(Track)] = (Resources.Column_Track, 11, 100, true),
        [nameof(Operator)] = (Resources.Column_Operator, 12, 50, true),
        [nameof(OtherBtn)] = (Resources.Column_Other, 13, 50, true)
    };

    #region Properties

    /// <summary>
    /// Stĺpec Číslo.
    /// </summary>
    [XmlElement("Number")]
    public DesktopColumn Number
    {
        get => field ??= InitColumn(nameof(Number));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(Number));
        }
    } = InitColumn(nameof(Number));

    /// <summary>
    /// Stĺpec Typ.
    /// </summary>
    [XmlElement("Type")]
    public DesktopColumn Type
    {
        get => field ??= InitColumn(nameof(Type));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(Type));
        }
    } = InitColumn(nameof(Type));

    /// <summary>
    /// Stĺpec Názov.
    /// </summary>
    [XmlElement("Name")]
    public DesktopColumn Name
    {
        get => field ??= InitColumn(nameof(Name));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(Name));
        }
    } = InitColumn(nameof(Name));

    /// <summary>
    /// Stĺpec Linka-Príchod.
    /// </summary>
    [XmlElement("LinkaPrichod")]
    public DesktopColumn LinkaPrichod
    {
        get => field ??= InitColumn(nameof(LinkaPrichod));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(LinkaPrichod));
        }
    } = InitColumn(nameof(LinkaPrichod));

    /// <summary>
    /// Stĺpec Linka-Odchod.
    /// </summary>
    [XmlElement("LinkaOdchod")]
    public DesktopColumn LinkaOdchod
    {
        get => field ??= InitColumn(nameof(LinkaOdchod));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(LinkaOdchod));
        }
    } = InitColumn(nameof(LinkaOdchod));

    /// <summary>
    /// Stĺpec Smerovanie.
    /// </summary>
    [XmlElement("Routing")]
    public DesktopColumn Routing
    {
        get => field ??= InitColumn(nameof(Routing));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(Routing));
        }
    } = InitColumn(nameof(Routing));

    /// <summary>
    /// Stĺpec Príchod.
    /// </summary>
    [XmlElement("Prichod")]
    public DesktopColumn Prichod
    {
        get => field ??= InitColumn(nameof(Prichod));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(Prichod));
        }
    } = InitColumn(nameof(Prichod));

    /// <summary>
    /// Stĺpec Odchod.
    /// </summary>
    [XmlElement("Odchod")]
    public DesktopColumn Odchod
    {
        get => field ??= InitColumn(nameof(Odchod));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(Odchod));
        }
    } = InitColumn(nameof(Odchod));

    /// <summary>
    /// Stĺpec Východzia stanica.
    /// </summary>
    [XmlElement("StartStation")]
    public DesktopColumn VychodziaStanica
    {
        get => field ??= InitColumn(nameof(VychodziaStanica));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(VychodziaStanica));
        }
    } = InitColumn(nameof(VychodziaStanica));

    /// <summary>
    /// Stĺpec Konečná stanica.
    /// </summary>
    [XmlElement("EndStation")]
    public DesktopColumn KonecnaStanica
    {
        get => field ??= InitColumn(nameof(KonecnaStanica));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(KonecnaStanica));
        }
    } = InitColumn(nameof(KonecnaStanica));

    /// <summary>
    /// Stĺpec Dátumové obmedzenie.
    /// </summary>
    [XmlElement("DateLimit")]
    public DesktopColumn DateLimit
    {
        get => field ??= InitColumn(nameof(DateLimit));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(DateLimit));
        }
    } = InitColumn(nameof(DateLimit));

    /// <summary>
    /// Stĺpec Koľaj.
    /// </summary>
    [XmlElement("Track")]
    public DesktopColumn Track
    {
        get => field ??= InitColumn(nameof(Track));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(Track));
        }
    } = InitColumn(nameof(Track));

    /// <summary>
    /// Stĺpec Dopravca.
    /// </summary>
    [XmlElement("Operator")]
    public DesktopColumn Operator
    {
        get => field ??= InitColumn(nameof(Operator));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(Operator));
        }
    } = InitColumn(nameof(Operator));

    /// <summary>
    /// Stĺpec Ostatné.
    /// </summary>
    [XmlElement("OtherBtn")]
    public DesktopColumn OtherBtn
    {
        get => field ??= InitColumn(nameof(OtherBtn));
        set
        {
            field = value;
            AssignColumnProps(ref field, nameof(OtherBtn));
        }
    } = InitColumn(nameof(OtherBtn));

    #endregion

    /// <summary>
    /// Vráti zoradený zoznam všetkých možných stĺpcov pre tabuľku na pracovnej ploche programu
    /// </summary>
    /// <returns></returns>
    public IList<DesktopColumn> GetValues()
    {
        var properties = ClassType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var ordered = properties.Select(prop => (DesktopColumn)prop.GetValue(this)!).ToList();
        return ordered.OrderBy(i => i.Order).ToList();
    }

    public void SetValues(IEnumerable<DesktopColumn> columns)
    {
        var index = 0;
        foreach (var column in columns)
        {
            column.Order = index++;
            ClassType.GetProperty(column.PropertyName)?.SetValue(this, column);
        }
    }

    private static DesktopColumn InitColumn(string propname)
        => new(Props[propname].name, propname, Props[propname].order, Props[propname].minWidth, Props[propname].visible);

    private static void AssignColumnProps(ref DesktopColumn? obj, string propname)
    {
        if (obj is null)
        {
            obj = InitColumn(propname);
        }
        else
        {
            obj.Name = Props[propname].name;
            obj.PropertyName = propname;
        }
    }
    
    protected DesktopColumns(DesktopColumns original)
    {
        if (original.Number != null) Number = original.Number with { };
        if (original.Type != null) Type = original.Type with { };
        if (original.Name != null) Name = original.Name with { };
        if (original.LinkaPrichod != null) LinkaPrichod = original.LinkaPrichod with { };
        if (original.LinkaOdchod != null) LinkaOdchod = original.LinkaOdchod with { };
        if (original.Routing != null) Routing = original.Routing with { };
        if (original.Prichod != null) Prichod = original.Prichod with { };
        if (original.Odchod != null) Odchod = original.Odchod with { };
        if (original.VychodziaStanica != null) VychodziaStanica = original.VychodziaStanica with { };
        if (original.KonecnaStanica != null) KonecnaStanica = original.KonecnaStanica with { };
        if (original.DateLimit != null) DateLimit = original.DateLimit with { };
        if (original.Track != null) Track = original.Track with { };
        if (original.Operator != null) Operator = original.Operator with { };
        if (original.OtherBtn != null) OtherBtn = original.OtherBtn with { };
    }
}