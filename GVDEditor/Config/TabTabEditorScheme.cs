using System.Xml.Serialization;
using GVDEditor.Properties;
using ToolsCore.XML;

namespace GVDEditor.Config;

/// <summary>
/// Obsahuje zoznam všetkých štýlov pre text v editore TabTab.
/// </summary>
public record TabTabEditorScheme() : IColorScheme
{
    /// <inheritdoc />
    [XmlIgnore]
    public bool DisableFontEdit => false;

    /// <summary>
    /// Font v editore TabTab.
    /// </summary>
    [XmlIgnore] 
    public Font Font { get; set; } = new("Consolas", 10);

    /// <summary>
    /// Font v editore TabTab vo formate XML.
    /// </summary>
    [XmlElement(Type = typeof(XmlFont), ElementName = "Font")]
    public XmlFont FontXML
    {
        get => XmlFont.FromFont(Font);
        set => Font = XmlFont.ToFont(value);
    }

    /// <inheritdoc />
    [XmlIgnore]
    public string Name => Resources.Scheme_TabTabEditor;

    [XmlIgnore]
    private static readonly Dictionary<string, ColorSetting> Props = new()
    {
        [nameof(Number)] = new(Color.Purple) { Name = Resources.Scheme_Number, DisableBackColorEdit = true },
        [nameof(String)] = new(Color.Red) { Name = Resources.Scheme_String, DisableBackColorEdit = true  },
        [nameof(Comment)] = new(Color.Green) { Name = Resources.Scheme_Comment, DisableBackColorEdit = true },
        [nameof(OnNewLine)] = new(Color.OrangeRed) { Name = Resources.Scheme_OnNewLine, DisableBackColorEdit = true },
        [nameof(Operator)] = new(Color.Black) { Name = Resources.Scheme_Operator, DisableBackColorEdit = true },
        [nameof(Constant)] = new(Color.DimGray) { Name = Resources.Scheme_Constant, DisableBackColorEdit = true },
        [nameof(Default)] = new(Color.Black) { Name = Resources.Scheme_Default, DisableBackColorEdit = true },
        [nameof(Var)] = new(Color.DarkSlateGray) { Name = Resources.Scheme_Var, DisableBackColorEdit = true },
        [nameof(Event)] = new(Color.SaddleBrown) { Name = Resources.Scheme_Event, DisableBackColorEdit = true },
        [nameof(Function)] = new(Color.Blue) { Name = Resources.Scheme_Function, DisableBackColorEdit = true },
        [nameof(Identifier)] = new(Color.Teal) { Name = Resources.Scheme_Identifier, DisableBackColorEdit = true },
        [nameof(SelBraces)] = new(Color.BlueViolet,Color.LightGray) { Name = Resources.Scheme_SelBraces },
        [nameof(SelBraceBad)] = new(Color.LightGray, Color.Red) { Name = Resources.Scheme_SelBraceBad }
    };

    #region Properties

    /// <summary>
    /// Textový editor TabTab - Číslo.
    /// </summary>
    [XmlElement("Number")]
    public ColorSetting Number
    {
        get => field ??= InitProperty(nameof(Number));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(Number));
        }
    } = InitProperty(nameof(Number));

    /// <summary>
    /// Textový editor TabTab - Reťazec.
    /// </summary>
    [XmlElement("String")]
    public ColorSetting String
    {
        get => field ??= InitProperty(nameof(String));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(String));
        }
    } = InitProperty(nameof(String));

    /// <summary>
    /// Textový editor TabTab - Komentár.
    /// </summary>
    [XmlElement("Comment")]
    public ColorSetting Comment
    {
        get => field ??= InitProperty(nameof(Comment));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(Comment));
        }
    } = InitProperty(nameof(Comment));

    /// <summary>
    /// Textový editor TabTab - Znak konca riadku a prechod do ďalšieho.
    /// </summary>
    [XmlElement("OnNewLine")]
    public ColorSetting OnNewLine
    {
        get => field ??= InitProperty(nameof(OnNewLine));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(OnNewLine));
        }
    } = InitProperty(nameof(OnNewLine));

    /// <summary>
    /// Textový editor TabTab - Operátor.
    /// </summary>
    [XmlElement("Operator")]
    public ColorSetting Operator
    {
        get => field ??= InitProperty(nameof(Operator));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(Operator));
        }
    } = InitProperty(nameof(Operator));

    /// <summary>
    /// Textový editor TabTab - Konštanta.
    /// </summary>
    [XmlElement("Constant")]
    public ColorSetting Constant
    {
        get => field ??= InitProperty(nameof(Constant));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(Constant));
        }
    } = InitProperty(nameof(Constant));

    /// <summary>
    /// Textový editor TabTab - Normálny text.
    /// </summary>
    [XmlElement("Default")]
    public ColorSetting Default
    {
        get => field ??= InitProperty(nameof(Default));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(Default));
        }
    } = InitProperty(nameof(Default));

    /// <summary>
    /// Textový editor TabTab - Označenie premennej.
    /// </summary>
    [XmlElement("Var")]
    public ColorSetting Var
    {
        get => field ??= InitProperty(nameof(Var));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(Var));
        }
    } = InitProperty(nameof(Var));

    /// <summary>
    /// Textový editor TabTab - Udalosť.
    /// </summary>
    [XmlElement("Event")]
    public ColorSetting Event
    {
        get => field ??= InitProperty(nameof(Event));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(Event));
        }
    } = InitProperty(nameof(Event));

    /// <summary>
    /// Textový editor TabTab - Funkcia.
    /// </summary>
    [XmlElement("Function")]
    public ColorSetting Function
    {
        get => field ??= InitProperty(nameof(Function));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(Function));
        }
    } = InitProperty(nameof(Function));

    /// <summary>
    /// Textový editor TabTab - Identifikátor.
    /// </summary>
    [XmlElement("Identifier")]
    public ColorSetting Identifier
    {
        get => field ??= InitProperty(nameof(Identifier));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(Identifier));
        }
    } = InitProperty(nameof(Identifier));

    /// <summary>
    /// Textový editor TabTab - Označenenie aktívnych zátvoriek.
    /// </summary>
    [XmlElement("SelBraces")]
    public ColorSetting SelBraces
    {
        get => field ??= InitProperty(nameof(SelBraces));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(SelBraces));
        }
    } = InitProperty(nameof(SelBraces));

    /// <summary>
    /// Textový editor TabTab - Označenenie aktívnej zátvorky, ktorá nemá páru.
    /// </summary>
    [XmlElement("SelBraceBad")]
    public ColorSetting SelBraceBad
    {
        get => field ??= InitProperty(nameof(SelBraceBad));
        set
        {
            field = value;
            AssignProperty(ref field, nameof(SelBraceBad));
        }
    } = InitProperty(nameof(SelBraceBad));

    #endregion

    private static ColorSetting InitProperty(string propname) => Props[propname] with { };

    private static void AssignProperty(ref ColorSetting? prop, string propname)
    {
        if (prop is null)
            prop = InitProperty(propname);
        else
        {
            prop.Name = Props[propname].Name;
            prop.DisableBackColorEdit = Props[propname].DisableBackColorEdit;
            prop.DisableFontBoldEdit = Props[propname].DisableFontBoldEdit;
        }
    }
    
    protected TabTabEditorScheme(TabTabEditorScheme original)
    {
        if (original.Number != null) Number = original.Number with { };
        if (original.String != null) String = original.String with { };
        if (original.Comment != null) Comment = original.Comment with { };
        if (original.OnNewLine != null) OnNewLine = original.OnNewLine with { };
        if (original.Operator != null) Operator = original.Operator with { };
        if (original.Constant != null) Constant = original.Constant with { };
        if (original.Default != null) Default = original.Default with { };
        if (original.Var != null) Var = original.Var with { };
        if (original.Event != null) Event = original.Event with { };
        if (original.Function != null) Function = original.Function with { };
        if (original.Identifier != null) Identifier = original.Identifier with { };
        if (original.SelBraces != null) SelBraces = original.SelBraces with { };
        if (original.SelBraceBad != null) SelBraceBad = original.SelBraceBad with { };
        Font = (Font)original.Font.Clone();
    }
}