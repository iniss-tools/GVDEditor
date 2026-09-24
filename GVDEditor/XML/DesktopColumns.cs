using System.Reflection;
using System.Xml.Serialization;
using ToolsCore.XML;

namespace GVDEditor.XML;

/// <summary>
///     Obsahuje zoznam všetkých možných stĺpcov pre tabuľku na pracovnej ploche programu.
/// </summary>
public record DesktopColumns()
{
    private static readonly Type ClassType = typeof(DesktopColumns);

    [XmlIgnore]
    private static readonly Dictionary<string, (string name, int order, int minWidth, bool visible)> props = new()
    {
        [nameof(Number)] = ("Číslo", 0, 60, true),
        [nameof(Type)] = ("Typ", 1, 40, true),
        [nameof(Name)] = ("Názov", 2, 100, true),
        [nameof(LinkaPrichod)] = ("Linka príchod", 3, 50, false),
        [nameof(LinkaOdchod)] = ("Linka odchod", 4, 50, false),
        [nameof(Routing)] = ("Smerovanie", 5, 100, true),
        [nameof(Prichod)] = ("Príchod", 6, 60, true),
        [nameof(Odchod)] = ("Odchod", 7, 60, true),
        [nameof(VychodziaStanica)] = ("Východzia stanica", 8, 120, true),
        [nameof(KonecnaStanica)] = ("Konečná stanica", 9, 120, true),
        [nameof(DateLimit)] = ("Dátumové obmedzenie", 10, 300, true),
        [nameof(Track)] = ("Koľaj", 11, 100, true),
        [nameof(Operator)] = ("Dopravca", 12, 50, true),
        [nameof(OtherBtn)] = ("Ostatné", 13, 50, true),
    };

    #region Properties

    /// <summary>
    ///     Stĺpec Číslo.
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
    ///     Stĺpec Typ.
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
    ///     Stĺpec Názov.
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
    ///     Stĺpec Linka-Príchod.
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
    ///     Stĺpec Linka-Odchod.
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
    ///     Stĺpec Smerovanie.
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
    ///     Stĺpec Príchod.
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
    ///     Stĺpec Odchod.
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
    ///     Stĺpec Východzia stanica.
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
    ///     Stĺpec Konečná stanica.
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
    ///     Stĺpec Dátumové obmedzenie.
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
    ///     Stĺpec Koľaj.
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
    ///     Stĺpec Dopravca.
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
    ///     Stĺpec Ostatné.
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
    ///     Vráti zoradený zoznam všetkých možných stĺpcov pre tabuľku na pracovnej ploche programu
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
        => new(props[propname].name, propname, props[propname].order, props[propname].minWidth, props[propname].visible);

    private static void AssignColumnProps(ref DesktopColumn obj, string propname)
    {
        if (obj is null)
        {
            obj = InitColumn(propname);
        }
        else
        {
            obj.Name = props[propname].name;
            obj.PropertyName = propname;
        }
    }
    
    // Every property setter below unconditionally assigns its backing field before this constructor
    // exits (see the "set" accessors above), but Roslyn's per-constructor flow analysis doesn't credit
    // assignment performed indirectly through a property setter call - it only sees `this` escaping into
    // a method call and forgets the field's null-state. All backing fields are genuinely never null here.
#pragma warning disable CS8618
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
#pragma warning restore CS8618
}