using GVDEditor.Config;
using GVDEditor.TabTabEditor;
using ScintillaNET;
using ToolsCore.XML;
using Style = ToolsCore.XML.Style;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Nahlad farieb editora TabTab na stranke farebnej schemy: ukazka pravidiel zvyraznena ako v editore. Klik na
/// slovo vyberie jeho polozku schemy.
/// </summary>
internal sealed class TabTabColorPreview : Scintilla, IColorPreview
{
    private const int SciSetILexer = 4033;

    // ukazka pravidiel TabTab (data INISS, nie text rozhrania)
    private const string Sample = "[Zpozdeni2]\r\n" +
                                  "zpozdeni > 5 AND Typ(Typ_R), \"@min\" = #SWITCH ; 34369\r\n" +
                                  "(Typ(Typ_EC)),,\"EC\"{34369},\\\r\n" +
                                  "(!LinkaOdj(\"\")),\"#\",%Linka% = #MERGE";

    private static readonly Dictionary<int, string> StyleKeys = new()
    {
        [TabTabStyle.Default] = "TabTab.Default",
        [TabTabStyle.Function] = "TabTab.Function",
        [TabTabStyle.Identifier] = "TabTab.Identifier",
        [TabTabStyle.Number] = "TabTab.Number",
        [TabTabStyle.String] = "TabTab.String",
        [TabTabStyle.Comment] = "TabTab.Comment",
        [TabTabStyle.Var] = "TabTab.Var",
        [TabTabStyle.Event] = "TabTab.Event",
        [TabTabStyle.OnNewLine] = "TabTab.OnNewLine",
        [TabTabStyle.Operator] = "TabTab.Operator",
        [TabTabStyle.Constant] = "TabTab.Constant"
    };

    private readonly TabTabLexer _lexer = new(
        TabTabACItems.GetFunctionItems().Select(item => item.FunctionName),
        TabTabACItems.GetEventItems().Select(item => item.MenuText),
        TabTabACItems.GetConstantItems().Select(item => item.ConstName),
        ["AND", "OR", "NOT", "ODD"]);

    public TabTabColorPreview()
    {
        BorderStyle = ScintillaNET.BorderStyle.None;
        Margins[0].Width = 0;
        Margins[1].Width = 0;
        HScrollBar = false;
        VScrollBar = false;
        StyleNeeded += (_, e) => _lexer.Style(this, GetEndStyled(), e.Position);
    }

    /// <inheritdoc />
    public Control Control => this;

    /// <inheritdoc />
    public event EventHandler<string>? KeyClicked;

    /// <inheritdoc />
    public void ShowScheme(Style style)
    {
        if (style is not GVDEditorStyle gvd)
            return;

        var scheme = gvd.TabTabEditorScheme;
        var font = GlobSettingsEditorFont();
        StyleResetDefault();
        Styles[ScintillaNET.Style.Default].Font = font.Name;
        Styles[ScintillaNET.Style.Default].SizeF = font.SizeInPoints;
        Styles[ScintillaNET.Style.Default].BackColor = gvd.ControlsColorScheme.Box.BackColor;
        StyleClearAll();
        Set(TabTabStyle.Default, scheme.Default);
        Set(TabTabStyle.Function, scheme.Function);
        Set(TabTabStyle.Identifier, scheme.Identifier);
        Set(TabTabStyle.Number, scheme.Number);
        Set(TabTabStyle.String, scheme.String);
        Set(TabTabStyle.Comment, scheme.Comment);
        Set(TabTabStyle.Var, scheme.Var);
        Set(TabTabStyle.Event, scheme.Event);
        Set(TabTabStyle.OnNewLine, scheme.OnNewLine);
        Set(TabTabStyle.Operator, scheme.Operator);
        Set(TabTabStyle.Constant, scheme.Constant);

        // bez lexera Scintilla - stylovanie robi kontajner (StyleNeeded)
        DirectMessage(SciSetILexer, IntPtr.Zero, IntPtr.Zero);
        ReadOnly = false;
        Text = Sample;
        ReadOnly = true;
        Colorize(0, -1);
    }

    private void Set(int index, ColorSetting setting)
    {
        Styles[index].ForeColor = setting.ForeColor;
        Styles[index].Bold = setting.Bold;
    }

    private static Font GlobSettingsEditorFont() => ToolsCore.GlobSettings.Fonts.EditorFont;

    /// <inheritdoc />
    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        var position = CharPositionFromPointClose(e.X, e.Y);
        if (position >= 0 && StyleKeys.TryGetValue(GetStyleAt(position), out var id))
            KeyClicked?.Invoke(this, id);
    }
}
