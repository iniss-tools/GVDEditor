using System.Reflection;
using System.Xml.Serialization;
using ToolsCore.XML;

namespace GVDEditor.XML;

/// <summary>
///     Obsahuje zoznam všetkých možných stĺpcov pre tabuľku na pracovnej ploche programu.
/// </summary>
public record DesktopColumns()
{
    private static readonly Type classType = typeof(DesktopColumns);

    [XmlIgnore]
    private static readonly Dictionary<string, (string name, int order, int minWidth, bool visible)> props = new()
    {
        [nameof(Number)] = ("Číslo", 0, 60, true),
        [nameof(Type)] = ("Typ", 1, 40, true),
        [nameof(Name)] = ("Názov", 2, 100, true),
        [nameof(LinkaPrichod)] = ("Linka príchod", 3, 50, false),
        [nameof(LinkaOdchod)] = ("Linka odchod", 4, 50, false),
        [nameof(Routing)] = ("Linka odchod", 5, 100, true),
        [nameof(Prichod)] = ("Linka odchod", 6, 60, true),
        [nameof(Odchod)] = ("Odchod", 7, 60, true),
        [nameof(VychodziaStanica)] = ("Východzia stanica", 8, 120, true),
        [nameof(KonecnaStanica)] = ("Konečná stanica", 9, 120, true),
        [nameof(DateLimit)] = ("Dátumové obmedzenie", 10, 300, true),
        [nameof(Track)] = ("Koľaj", 11, 100, true),
        [nameof(Operator)] = ("Operátor", 12, 50, true),
        [nameof(OtherBtn)] = ("Ostatné", 13, 50, true),
    };

    #region Fields

    private DesktopColumn _number = InitColumn(nameof(Number));
    private DesktopColumn _type = InitColumn(nameof(Type));
    private DesktopColumn _name = InitColumn(nameof(Name));
    private DesktopColumn _linkaPrichod = InitColumn(nameof(LinkaPrichod));
    private DesktopColumn _linkaOdchod = InitColumn(nameof(LinkaOdchod));
    private DesktopColumn _routing = InitColumn(nameof(Routing));
    private DesktopColumn _prichod = InitColumn(nameof(Prichod));
    private DesktopColumn _odchod = InitColumn(nameof(Odchod));
    private DesktopColumn _vychodziaStanica = InitColumn(nameof(VychodziaStanica));
    private DesktopColumn _konecnaStanica = InitColumn(nameof(KonecnaStanica));
    private DesktopColumn _dateLimit = InitColumn(nameof(DateLimit));
    private DesktopColumn _track = InitColumn(nameof(Track));
    private DesktopColumn _operator = InitColumn(nameof(Operator));
    private DesktopColumn _otherBtn = InitColumn(nameof(OtherBtn));

    #endregion

    #region Properties

    /// <summary>
    ///     Stĺpec Číslo.
    /// </summary>
    [XmlElement("Number")]
    public DesktopColumn Number
    {
        get => _number ??= InitColumn(nameof(Number));
        set
        {
            _number = value;
            AssignColumnProps(ref _number, nameof(Number));
        }
    }

    /// <summary>
    ///     Stĺpec Typ.
    /// </summary>
    [XmlElement("Type")]
    public DesktopColumn Type
    {
        get => _type ??= InitColumn(nameof(Type));
        set
        {
            _type = value;
            AssignColumnProps(ref _type, nameof(Type));
        }
    }

    /// <summary>
    ///     Stĺpec Názov.
    /// </summary>
    [XmlElement("Name")]
    public DesktopColumn Name
    {
        get => _name ??= InitColumn(nameof(Name));
        set
        {
            _name = value;
            AssignColumnProps(ref _name, nameof(Name));
        }
    }

    /// <summary>
    ///     Stĺpec Linka-Príchod.
    /// </summary>
    [XmlElement("LinkaPrichod")]
    public DesktopColumn LinkaPrichod
    {
        get => _linkaPrichod ??= InitColumn(nameof(LinkaPrichod));
        set
        {
            _linkaPrichod = value;
            AssignColumnProps(ref _linkaPrichod, nameof(LinkaPrichod));
        }
    }

    /// <summary>
    ///     Stĺpec Linka-Príchod.
    /// </summary>
    [XmlElement("LinkaOdchod")]
    public DesktopColumn LinkaOdchod
    {
        get => _linkaOdchod ??= InitColumn(nameof(LinkaOdchod));
        set
        {
            _linkaOdchod = value;
            AssignColumnProps(ref _linkaOdchod, nameof(LinkaOdchod));
        }
    }

    /// <summary>
    ///     Stĺpec Smerovanie.
    /// </summary>
    [XmlElement("Routing")]
    public DesktopColumn Routing
    {
        get => _routing ??= InitColumn(nameof(Routing));
        set
        {
            _routing = value;
            AssignColumnProps(ref _routing, nameof(Routing));
        }
    }

    /// <summary>
    ///     Stĺpec Príchod.
    /// </summary>
    [XmlElement("Prichod")]
    public DesktopColumn Prichod
    {
        get => _prichod ??= InitColumn(nameof(Prichod));
        set
        {
            _prichod = value;
            AssignColumnProps(ref _prichod, nameof(Prichod));
        }
    }

    /// <summary>
    ///     Stĺpec Odchod.
    /// </summary>
    [XmlElement("Odchod")]
    public DesktopColumn Odchod
    {
        get => _odchod ??= InitColumn(nameof(Odchod));
        set
        {
            _odchod = value;
            AssignColumnProps(ref _odchod, nameof(Odchod));
        }
    }

    /// <summary>
    ///     Stĺpec Východzia stanica.
    /// </summary>
    [XmlElement("StartStation")]
    public DesktopColumn VychodziaStanica
    {
        get => _vychodziaStanica ??= InitColumn(nameof(VychodziaStanica));
        set
        {
            _vychodziaStanica = value;
            AssignColumnProps(ref _vychodziaStanica, nameof(VychodziaStanica));
        }
    }

    /// <summary>
    ///     Stĺpec Konečná stanica.
    /// </summary>
    [XmlElement("EndStation")]
    public DesktopColumn KonecnaStanica
    {
        get => _konecnaStanica ??= InitColumn(nameof(KonecnaStanica));
        set
        {
            _konecnaStanica = value;
            AssignColumnProps(ref _konecnaStanica, nameof(KonecnaStanica));
        }
    }

    /// <summary>
    ///     Stĺpec Dátumové obmedzenie.
    /// </summary>
    [XmlElement("DateLimit")]
    public DesktopColumn DateLimit
    {
        get => _dateLimit ??= InitColumn(nameof(DateLimit));
        set
        {
            _dateLimit = value;
            AssignColumnProps(ref _dateLimit, nameof(DateLimit));
        }
    }

    /// <summary>
    ///     Stĺpec Koľaj.
    /// </summary>
    [XmlElement("Track")]
    public DesktopColumn Track
    {
        get => _track ??= InitColumn(nameof(Track));
        set
        {
            _track = value;
            AssignColumnProps(ref _track, nameof(Track));
        }
    }

    /// <summary>
    ///     Stĺpec Linka-Príchod.
    /// </summary>
    [XmlElement("Operator")]
    public DesktopColumn Operator
    {
        get => _operator ??= InitColumn(nameof(Operator));
        set
        {
            _operator = value;
            AssignColumnProps(ref _operator, nameof(Operator));
        }
    }

    /// <summary>
    ///     Stĺpec Ostatné.
    /// </summary>
    [XmlElement("OtherBtn")]
    public DesktopColumn OtherBtn
    {
        get => _otherBtn ??= InitColumn(nameof(OtherBtn));
        set
        {
            _otherBtn = value;
            AssignColumnProps(ref _otherBtn, nameof(OtherBtn));
        }
    }

    #endregion

    /// <summary>
    ///     Vráti zoradený zoznam všetkých možných stĺpcov pre tabuľku na pracovnej ploche programu
    /// </summary>
    /// <returns></returns>
    public IList<DesktopColumn> GetValues()
    {
        var properties = classType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var ordered = properties.Select(prop => prop.GetValue(this) as DesktopColumn).ToList();
        return ordered.OrderBy(i => i.Order).ToList();
    }

    public void SetValues(IEnumerable<DesktopColumn> columns)
    {
        var index = 0;
        foreach (var column in columns)
        {
            column.Order = index++;
            classType.GetProperty(column.PropertyName)?.SetValue(this, column);
        }
    }

    private static DesktopColumn InitColumn(string propname)
        => new(props[propname].name, propname, props[propname].order, props[propname].minWidth, props[propname].visible);

    private static void AssignColumnProps(ref DesktopColumn obj, string propname)
    {
        if (obj is null)
        {
            InitColumn(propname);
        }
        else
        {
            obj.Name = props[propname].name;
            obj.PropertyName = propname;
        }
    }
    
    protected DesktopColumns(DesktopColumns original)
    {
        Number = original.Number with { };
        Type = original.Type with { };
        Name = original.Name with { };
        LinkaPrichod = original.LinkaPrichod with { };
        LinkaOdchod = original.LinkaOdchod with { };
        Routing = original.Routing with { };
        Prichod = original.Prichod with { };
        Odchod = original.Odchod with { };
        VychodziaStanica = original.VychodziaStanica with { };
        KonecnaStanica = original.KonecnaStanica with { };
        DateLimit = original.DateLimit with { };
        Track = original.Track with { };
        Operator = original.Operator with { };
        OtherBtn = original.OtherBtn with { };
    }
}