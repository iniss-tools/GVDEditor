using System.Xml.Serialization;
using ToolsCore.XML;

namespace GVDEditor.XML;

/// <summary>
///     Obsahuje zoznam všetkých štýlov pre text v editore TabTab.
/// </summary>
public record TabTabEditorScheme() : IColorScheme
{
    /// <inheritdoc />
    [XmlIgnore]
    public bool DisableFontEdit => false;

    /// <summary>
    ///     Font v editore TabTab.
    /// </summary>
    [XmlIgnore] 
    public Font Font { get; set; } = new("Consolas", 10);

    /// <summary>
    ///     Font v editore TabTab vo formate XML.
    /// </summary>
    [XmlElement(Type = typeof(XmlFont), ElementName = "Font")]
    public XmlFont FontXML
    {
        get => XmlFont.FromFont(Font);
        set => Font = XmlFont.ToFont(value);
    }

    /// <inheritdoc />
    [XmlIgnore]
    public string Name => "TabTab editor";

    [XmlIgnore]
    private static readonly Dictionary<string, ColorSetting> props = new()
    {
        [nameof(Number)] = new(Color.Purple) { Name = "Číslo", DisableBackColorEdit = true },
        [nameof(String)] = new(Color.Red) { Name = "Reťazec", DisableBackColorEdit = true  },
        [nameof(Comment)] = new(Color.Green) { Name = "Komentár", DisableBackColorEdit = true },
        [nameof(OnNewLine)] = new(Color.OrangeRed) { Name = "Zápis do ďalšieho riadka", DisableBackColorEdit = true },
        [nameof(Operator)] = new(Color.Black) { Name = "Operátor", DisableBackColorEdit = true },
        [nameof(Constant)] = new(Color.DimGray) { Name = "Konštanta", DisableBackColorEdit = true },
        [nameof(Default)] = new(Color.Black) { Name = "Predvolené", DisableBackColorEdit = true },
        [nameof(Var)] = new(Color.DarkSlateGray) { Name = "Premenná", DisableBackColorEdit = true },
        [nameof(Event)] = new(Color.SaddleBrown) { Name = "Udalosť", DisableBackColorEdit = true },
        [nameof(Function)] = new(Color.Blue) { Name = "Funkcia", DisableBackColorEdit = true },
        [nameof(Identifier)] = new(Color.Teal) { Name = "Identifikátor", DisableBackColorEdit = true },
        [nameof(SelBraces)] = new(Color.BlueViolet,Color.LightGray) { Name = "Zvýraznenie zátvoriek" },
        [nameof(SelBraceBad)] = new(Color.LightGray, Color.Red) { Name = "Zvýraznie zátvorky, ktorej chýba pár" },
    };

    #region Fields

    private ColorSetting _number = InitProperty(nameof(Number));
    private ColorSetting _string = InitProperty(nameof(String));
    private ColorSetting _comment = InitProperty(nameof(Comment));
    private ColorSetting _onNewLine = InitProperty(nameof(OnNewLine));
    private ColorSetting _operator = InitProperty(nameof(Operator));
    private ColorSetting _constant = InitProperty(nameof(Constant));
    private ColorSetting _default = InitProperty(nameof(Default));
    private ColorSetting _var = InitProperty(nameof(Var));
    private ColorSetting _event = InitProperty(nameof(Event));
    private ColorSetting _function = InitProperty(nameof(Function));
    private ColorSetting _identifier = InitProperty(nameof(Identifier));
    private ColorSetting _selBraces = InitProperty(nameof(SelBraces));
    private ColorSetting _selBraceBad = InitProperty(nameof(SelBraceBad));

    #endregion

    #region Properties

    /// <summary>
    ///     Textový editor TabTab - Číslo.
    /// </summary>
    [XmlElement("Number")]
    public ColorSetting Number
    {
        get => _number ??= InitProperty(nameof(Number));
        set
        {
            _number = value;
            AssignProperty(ref _number, nameof(Number));
        }
    }

    /// <summary>
    ///     Textový editor TabTab - Reťazec.
    /// </summary>
    [XmlElement("String")]
    public ColorSetting String
    {
        get => _string ??= InitProperty(nameof(String));
        set
        {
            _string = value;
            AssignProperty(ref _string, nameof(String));
        }
    }

    /// <summary>
    ///     Textový editor TabTab - Komentár.
    /// </summary>
    [XmlElement("Comment")]
    public ColorSetting Comment
    {
        get => _comment ??= InitProperty(nameof(Comment));
        set
        {
            _comment = value;
            AssignProperty(ref _comment, nameof(Comment));
        }
    }

    /// <summary>
    ///     Textový editor TabTab - Znak konca riadku a prechod do ďalšieho.
    /// </summary>
    [XmlElement("OnNewLine")]
    public ColorSetting OnNewLine
    {
        get => _onNewLine ??= InitProperty(nameof(OnNewLine));
        set
        {
            _onNewLine = value;
            AssignProperty(ref _onNewLine, nameof(OnNewLine));
        }
    }

    /// <summary>
    ///     Textový editor TabTab - Operátor.
    /// </summary>
    [XmlElement("Operator")]
    public ColorSetting Operator
    {
        get => _operator ??= InitProperty(nameof(Operator));
        set
        {
            _operator = value;
            AssignProperty(ref _operator, nameof(Operator));
        }
    }

    /// <summary>
    ///     Textový editor TabTab - Konštanta.
    /// </summary>
    [XmlElement("Constant")]
    public ColorSetting Constant
    {
        get => _constant ??= InitProperty(nameof(Constant));
        set
        {
            _constant = value;
            AssignProperty(ref _constant, nameof(Constant));
        }
    }

    /// <summary>
    ///     Textový editor TabTab - Normálny text.
    /// </summary>
    [XmlElement("Default")]
    public ColorSetting Default
    {
        get => _default ??= InitProperty(nameof(Default));
        set
        {
            _default = value;
            AssignProperty(ref _default, nameof(Default));
        }
    }

    /// <summary>
    ///     Textový editor TabTab - Označenie premennej.
    /// </summary>
    [XmlElement("Var")]
    public ColorSetting Var
    {
        get => _var ??= InitProperty(nameof(Var));
        set
        {
            _var = value;
            AssignProperty(ref _var, nameof(Var));
        }
    }

    /// <summary>
    ///     Textový editor TabTab - Udalosť.
    /// </summary>
    [XmlElement("Event")]
    public ColorSetting Event
    {
        get => _event ??= InitProperty(nameof(Event));
        set
        {
            _event = value;
            AssignProperty(ref _event, nameof(Event));
        }
    }

    /// <summary>
    ///     Textový editor TabTab - Funkcia.
    /// </summary>
    [XmlElement("Function")]
    public ColorSetting Function
    {
        get => _function ??= InitProperty(nameof(Function));
        set
        {
            _function = value;
            AssignProperty(ref _function, nameof(Function));
        }
    }

    /// <summary>
    ///     Textový editor TabTab - Identifikátor.
    /// </summary>
    [XmlElement("Identifier")]
    public ColorSetting Identifier
    {
        get => _identifier ??= InitProperty(nameof(Identifier));
        set
        {
            _identifier = value;
            AssignProperty(ref _identifier, nameof(Identifier));
        }
    }

    /// <summary>
    ///     Textový editor TabTab - Označenenie aktívnych zátvoriek.
    /// </summary>
    [XmlElement("SelBraces")]
    public ColorSetting SelBraces
    {
        get => _selBraces ??= InitProperty(nameof(SelBraces));
        set
        {
            _selBraces = value;
            AssignProperty(ref _selBraces, nameof(SelBraces));
        }
    }

    /// <summary>
    ///     Textový editor TabTab - Označenenie aktívnej zátvorky, ktorá nemá páru.
    /// </summary>
    [XmlElement("SelBraceBad")]
    public ColorSetting SelBraceBad
    {
        get => _selBraceBad ??= InitProperty(nameof(SelBraceBad));
        set
        {
            _selBraceBad = value;
            AssignProperty(ref _selBraceBad, nameof(SelBraceBad));
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

    protected TabTabEditorScheme(TabTabEditorScheme original)
    {
        Number = original.Number with { };
        String = original.String with { };
        Comment = original.Comment with { };
        OnNewLine = original.OnNewLine with { };
        Operator = original.Operator with { };
        Constant = original.Constant with { };
        Default = original.Default with { };
        Var = original.Var with { };
        Event = original.Event with { };
        Function = original.Function with { };
        Identifier = original.Identifier with { };
        SelBraces = original.SelBraces with { };
        SelBraceBad = original.SelBraceBad with { };
    }
}