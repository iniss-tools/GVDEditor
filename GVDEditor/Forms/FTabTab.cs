using System.Text.RegularExpressions;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ScintillaNET;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Nastavenie TabTabs.
/// </summary>
public partial class FTabTab : Form
{
    private static readonly string[] operatory = { "AND", "OR", "NOT", "ODD" };

    private readonly TabTabLexer cSharpLexer = new(
        TabTabACItems.GetFunctionItems().Select(item => item.FunctionName),
        TabTabACItems.GetEventItems().Select(item => item.MenuText),
        TabTabACItems.GetConstantItems().Select(item => item.ConstName),
        operatory);

    internal readonly BindingList<TabTabDoc> documents = new();

    private readonly TableTabTab SelectedTab;

    private int lastCaretPos;
    private int maxLineNumberCharLength;

    private readonly Scintilla sc;


    /// <summary>
    ///     Vytvori novy formular typu <see cref="FTabTab"/>.
    /// </summary>
    public FTabTab(TableTabTab tab = null)
    {
        InitializeComponent();

        if (GlobData.UsingStyle.DarkTitleBar) ExTools.SetImmersiveDarkMode(Handle, true);

        sc = scText.scintilla;
        acMenu.TargetControlWrapper = new ScintillaWrapper(sc);

        foreach (var tabTab in GlobData.TabTabs) 
            documents.Add(new TabTabDoc { Document = CreateDocument(tabTab.Text), TabTab = tabTab });

        lbTabTabs.DataSource = documents;

        sc.EmptyUndoBuffer();

        acMenu.SetAutocompleteItems(TabTabACItems.GetItems());

        tsbUndo.Enabled = false;
        tsbRedo.Enabled = false;

        ShowNumberLines();

        SelectedTab = tab;
    }

    private void FTabTab_Load(object sender, EventArgs e)
    {
        lbTabTabs.Font = GlobData.Config.Fonts.Menu; //GlobData.UsingStyle.TabTabEditorScheme.Font;

        sc.StyleResetDefault();
        sc.Styles[Style.Default].Font = GlobData.UsingStyle.TabTabEditorScheme.Font.Name;
        sc.Styles[Style.Default].SizeF = GlobData.UsingStyle.TabTabEditorScheme.Font.Size;
        sc.Styles[Style.Default].BackColor = GlobData.UsingStyle.ControlsColorScheme.Box.BackColor;
        sc.StyleClearAll();
        sc.Styles[Style.LineNumber].BackColor = GlobData.UsingStyle.ControlsColorScheme.Button.BackColor;
        sc.Styles[Style.LineNumber].ForeColor = GlobData.UsingStyle.ControlsColorScheme.Button.ForeColor;
        sc.CaretForeColor = GlobData.UsingStyle.ControlsColorScheme.Box.ForeColor;
        sc.SetSelectionBackColor(true, GlobData.UsingStyle.ControlsColorScheme.Border.ForeColor);

        if (!GlobData.UsingStyle.ControlsDefaultStyle)
            sc.BorderStyle = BorderStyle.None;

        if (GlobData.UsingStyle.DarkScrollBar)
        {
            scText.VScrollBarControl.SetTheme(WindowsTheme.DarkExplorer);
            scText.HScrollBarControl.SetTheme(WindowsTheme.DarkExplorer);
        }

        this.ApplyThemeAndFonts();

        FormUtils.ChangeColorContextMenu(GlobData.UsingStyle, conMenuScText);

        acMenu.Colors.BackColor = GlobData.UsingStyle.ControlsColorScheme.Panel.BackColor;
        acMenu.Colors.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Panel.ForeColor;
        acMenu.Colors.SelectedForeColor = GlobData.UsingStyle.ControlsColorScheme.Panel.ForeColor;

        sc.Styles[TabTabStyle.Default].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.Default.ForeColor;
        sc.Styles[TabTabStyle.Default].Bold = GlobData.UsingStyle.TabTabEditorScheme.Default.Bold;

        sc.Styles[TabTabStyle.Function].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.Function.ForeColor;
        sc.Styles[TabTabStyle.Function].Bold = GlobData.UsingStyle.TabTabEditorScheme.Function.Bold;

        sc.Styles[TabTabStyle.Identifier].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.Identifier.ForeColor;
        sc.Styles[TabTabStyle.Identifier].Bold = GlobData.UsingStyle.TabTabEditorScheme.Identifier.Bold;

        sc.Styles[TabTabStyle.Number].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.Number.ForeColor;
        sc.Styles[TabTabStyle.Number].Bold = GlobData.UsingStyle.TabTabEditorScheme.Number.Bold;

        sc.Styles[TabTabStyle.String].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.String.ForeColor;
        sc.Styles[TabTabStyle.String].Bold = GlobData.UsingStyle.TabTabEditorScheme.String.Bold;

        sc.Styles[TabTabStyle.Comment].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.Comment.ForeColor;
        sc.Styles[TabTabStyle.Comment].Bold = GlobData.UsingStyle.TabTabEditorScheme.Comment.Bold;

        sc.Styles[TabTabStyle.Var].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.Var.ForeColor;
        sc.Styles[TabTabStyle.Var].Bold = GlobData.UsingStyle.TabTabEditorScheme.Var.Bold;

        sc.Styles[TabTabStyle.Event].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.Event.ForeColor;
        sc.Styles[TabTabStyle.Event].Bold = GlobData.UsingStyle.TabTabEditorScheme.Event.Bold;

        sc.Styles[TabTabStyle.OnNewLine].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.OnNewLine.ForeColor;
        sc.Styles[TabTabStyle.OnNewLine].Bold = GlobData.UsingStyle.TabTabEditorScheme.OnNewLine.Bold;

        sc.Styles[TabTabStyle.Operator].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.Operator.ForeColor;
        sc.Styles[TabTabStyle.Operator].Bold = GlobData.UsingStyle.TabTabEditorScheme.Operator.Bold;

        sc.Styles[TabTabStyle.Constant].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.Constant.ForeColor;
        sc.Styles[TabTabStyle.Constant].Bold = GlobData.UsingStyle.TabTabEditorScheme.Constant.Bold;

        sc.Lexer = Lexer.Container;

        //highlight active braces
        sc.IndentationGuides = IndentView.LookBoth;

        sc.Styles[Style.BraceLight].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.SelBraces.ForeColor;
        sc.Styles[Style.BraceLight].BackColor = GlobData.UsingStyle.TabTabEditorScheme.SelBraces.BackColor;
        sc.Styles[Style.BraceLight].Bold = GlobData.UsingStyle.TabTabEditorScheme.SelBraces.Bold;

        sc.Styles[Style.BraceBad].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.SelBraceBad.ForeColor;
        sc.Styles[Style.BraceBad].BackColor = GlobData.UsingStyle.TabTabEditorScheme.SelBraceBad.BackColor;
        sc.Styles[Style.BraceBad].Bold = GlobData.UsingStyle.TabTabEditorScheme.SelBraceBad.Bold;

        if (SelectedTab is not null)
            for (var i = 0; i < documents.Count; i++)
                if (documents[i].TabTab == SelectedTab)
                    lbTabTabs.SelectedIndex = i;
    }

    private void FTabTab_FormClosing(object sender, FormClosingEventArgs e)
    {
        var unsaved = documents.Any(doc => doc.Unsaved);

        if (unsaved)
        {
            var result = Utils.ShowQuestion(Resources.FMain_Save_Changes, MessageBoxButtons.YesNoCancel);
            switch (result)
            {
                case DialogResult.Yes:
                    DoSaveAll();
                    e.Cancel = false;
                    DialogResult = DialogResult.OK;
                    break;
                case DialogResult.No:
                    e.Cancel = false;
                    DialogResult = DialogResult.Cancel;
                    break;
                default:
                    e.Cancel = true;
                    break;
            }
        }
    }

    private void DoSave()
    {
        if (lbTabTabs.SelectedIndex != -1)
        {
            documents[lbTabTabs.SelectedIndex].TabTab.Text = sc.Text;
            documents[lbTabTabs.SelectedIndex].Unsaved = false;
            documents.ResetBindings();
        }
    }

    private void DoSaveAll()
    {
        var current = sc.Document;

        foreach (var doc in documents)
        {
            SwitchDocument(doc.Document);
            doc.TabTab.Text = sc.Text;
            doc.Unsaved = false;
        }

        sc.Document = current;
        documents.ResetBindings();
    }

    private void DoUndo()
    {
        sc.Undo();

        if (!sc.CanUndo && lbTabTabs.SelectedIndex != -1)
        {
            documents[lbTabTabs.SelectedIndex].Unsaved = false;
            documents.ResetBindings();
        }
    }

    private void DoRedo() => sc.Redo();

    private void DoAddTab()
    {
        var frtt = new FTabTabRename();
        if (frtt.ShowDialog() == DialogResult.OK)
        {
            documents.Add(new TabTabDoc
                { Unsaved = true, Document = CreateDocument(""), TabTab = new TableTabTab { Key = frtt.NewTabName, Text = "" } });
            lbTabTabs.SelectedIndex = documents.Count - 1;
        }
    }

    private void DoRemoveTab()
    {
        if (lbTabTabs.SelectedIndex != -1)
        {
            var delete = true;
            var where = " ";
            var index = lbTabTabs.SelectedIndex;
            var tab = GlobData.TabTabs[index];

            foreach (var tc in GlobData.TableCatalogs)
            {
                foreach (var ti in tc.Items)
                {
                    if (ti.Tab1 == tab)
                    {
                        delete = false;
                        where += $"Katalógová tabuľa {tc.Name}, položka {ti.Name}, TAB1";
                        break;
                    }

                    if (ti.Tab2 == tab)
                    {
                        delete = false;
                        where += $"Katalógová tabuľa {tc.Name}, položka {ti.Name}, TAB2";
                        break;
                    }
                }

                if (!delete)
                    break;
            }

            if (delete)
                documents.RemoveAt(index);
            else
                Utils.ShowError(Resources.SelectedItemRemoveCancel + where);
        }
    }

    private void DoRenameTab()
    {
        var frtt = new FTabTabRename();
        if (frtt.ShowDialog() == DialogResult.OK && lbTabTabs.SelectedIndex != -1)
        {
            documents[lbTabTabs.SelectedIndex].TabTab.Key = frtt.NewTabName;
            documents.ResetBindings();
        }
    }

    private void DoFindReplace(bool showReplace = false)
    {
        var ffar = new FTabTabFindReplace(sc, showReplace);
        ffar.Show();
    }

    private void DoReformat()
    {
        sc.BeginUndoAction();

        foreach (var f in TabTabACItems.GetFunctionItems().Select(item => item.FunctionName))
            sc.Text = sc.Text.Replace(f, f, StringComparison.CurrentCultureIgnoreCase);

        foreach (var c in TabTabACItems.GetConstantItems().Select(item => item.ConstName))
            sc.Text = sc.Text.Replace(c, c, StringComparison.CurrentCultureIgnoreCase);

        AddSpaces("=");
        AddSpaces(@"\|\|");
        AddSpaces("&&");

        sc.EndUndoAction();
    }

    private void AddSpaces(string replc)
    {
        var fc = replc[0];
        var lc = replc[replc.Length - 1];

        sc.Text = Regex.Replace(sc.Text, $@"[^ ]{replc}[^ \r\n]|[^ ]{replc}.?|.?{replc}[^ \r\n]", delegate(Match match)
        {
            var fmc = match.Value[0];
            var lmc = match.Value[match.Value.Length - 1];

            var first = fmc != fc && fmc != ' ' ? fmc.ToString() : "";
            var last = lmc != lc && lmc != ' ' ? lmc.ToString() : "";

            return $"{first} {replc} {last}".Replace("\\", "");
        });
    }

    private void tsbSave_Click(object sender, EventArgs e) => DoSave();

    private void tsbSaveAll_Click(object sender, EventArgs e) => DoSaveAll();

    private void tsbStorno_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

    private void tsbUndo_Click(object sender, EventArgs e) => DoUndo();

    private void tsbRedo_Click(object sender, EventArgs e) => DoRedo();

    private void tsbAddTab_Click(object sender, EventArgs e) => DoAddTab();

    private void tsbRemoveTab_Click(object sender, EventArgs e) => DoRemoveTab();

    private void tsbRename_Click(object sender, EventArgs e) => DoRenameTab();

    private void tsbFindReplace_Click(object sender, EventArgs e) => DoFindReplace();

    private void tsbReformat_Click(object sender, EventArgs e) => DoReformat();

    private void tsmiSave_Click(object sender, EventArgs e) => DoSave();

    private void tsmiSaveAll_Click(object sender, EventArgs e) => DoSaveAll();

    private void tsmiUndo_Click(object sender, EventArgs e) => DoUndo();

    private void tsmiRedo_Click(object sender, EventArgs e) => DoRedo();

    private void tsmiCut_Click(object sender, EventArgs e) => sc.Cut();

    private void tsmiCopy_Click(object sender, EventArgs e) => sc.Copy();

    private void tsmiPaste_Click(object sender, EventArgs e) => sc.Paste();

    private void tsmiDelete_Click(object sender, EventArgs e) => sc.ReplaceSelection("");

    private void tsmiSelectAll_Click(object sender, EventArgs e) => sc.SelectAll();

    private void tsmiAddTabTab_Click(object sender, EventArgs e) => DoAddTab();

    private void tsmiDeleteTabTab_Click(object sender, EventArgs e) => DoRemoveTab();

    private void tsmiRenameTabTab_Click(object sender, EventArgs e) => DoRenameTab();

    private void scText_StyleNeeded(object sender, StyleNeededEventArgs e)
    {
        var startPos = sc.GetEndStyled();
        var endPos = e.Position;

        cSharpLexer.Style(sc, startPos, endPos);
    }

    private void scText_TextChanged(object sender, EventArgs e)
    {
        tsbUndo.Enabled = sc.CanUndo;
        tsbRedo.Enabled = sc.CanRedo;

        if (lbTabTabs.SelectedIndex != -1 && !documents[lbTabTabs.SelectedIndex].Unsaved)
        {
            documents[lbTabTabs.SelectedIndex].Unsaved = true;
            tsbSave.Enabled = true;
            documents.ResetBindings();
        }

        ShowNumberLines();
    }

    private void ShowNumberLines()
    {
        var maxLength = sc.Lines.Count.ToString().Length;
        if (maxLength == maxLineNumberCharLength)
            return;

        const int padding = 2;
        sc.Margins[0].Width = sc.TextWidth(Style.LineNumber, new string('9', maxLength + 1)) + padding;
        maxLineNumberCharLength = maxLength;
    }

    private void FTabTab_KeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;
        if (e.Control && e.Shift && e.KeyCode == Keys.S)
            DoSaveAll();
        else if (e.Control && e.KeyCode == Keys.S)
            DoSave();
        else if (e.Control && e.KeyCode is Keys.F)
            DoFindReplace();
        else if (e.Control && e.KeyCode is Keys.H)
            DoFindReplace(true);
        else
            e.Handled = false;
    }

    private void scText_CharAdded(object sender, CharAddedEventArgs e) => InsertMatchedChars(e);

    private void InsertMatchedChars(CharAddedEventArgs e)
    {
        var caretPos = sc.CurrentPosition;
        var docStart = caretPos == 1;
        var docEnd = caretPos == sc.Text.Length;

        var charPrev = docStart ? sc.GetCharAt(caretPos) : sc.GetCharAt(caretPos - 2);
        var charNext = sc.GetCharAt(caretPos);

        var isCharPrevBlank = charPrev is ' ' or '\t' or '\n' or '\r';

        var isCharNextBlank = charNext is ' ' or '\t' or '\n' or '\r' || docEnd;

        var isEnclosed = charPrev == '(' && charNext == ')' || charPrev == '{' && charNext == '}' || charPrev == '[' && charNext == ']';

        var isSpaceEnclosed = charPrev == '(' && isCharNextBlank || isCharPrevBlank && charNext == ')' ||
                              charPrev == '{' && isCharNextBlank || isCharPrevBlank && charNext == '}' ||
                              charPrev == '[' && isCharNextBlank || isCharPrevBlank && charNext == ']';

        var isCharOrString = isCharPrevBlank && isCharNextBlank || isEnclosed || isSpaceEnclosed;

        var charNextIsCharOrString = charNext is '"' or '\'';

        switch (e.Char)
        {
            case '(':
                if (charNextIsCharOrString) return;
                sc.InsertText(caretPos, ")");
                break;
            case '{':
                if (charNextIsCharOrString) return;
                sc.InsertText(caretPos, "}");
                break;
            case '[':
                if (charNextIsCharOrString) return;
                sc.InsertText(caretPos, "]");
                break;
            case '"':
                // 0x22 = "
                if (charPrev == 0x22 && charNext == 0x22)
                {
                    sc.DeleteRange(caretPos, 1);
                    sc.GotoPosition(caretPos);
                    return;
                }

                if (isCharOrString)
                    sc.InsertText(caretPos, "\"");
                break;
            case '\'':
                // 0x27 = '
                if (charPrev == 0x27 && charNext == 0x27)
                {
                    sc.DeleteRange(caretPos, 1);
                    sc.GotoPosition(caretPos);
                    return;
                }

                if (isCharOrString)
                    sc.InsertText(caretPos, "'");
                break;
        }
    }

    private static bool IsBrace(int c)
    {
        return c switch
        {
            '(' => true,
            ')' => true,
            '[' => true,
            ']' => true,
            '{' => true,
            '}' => true,
            _ => false
        };
    }

    private void scText_UpdateUI(object sender, UpdateUIEventArgs e)
    {
        // Has the caret changed position?
        var caretPos = sc.CurrentPosition;
        if (lastCaretPos != caretPos)
        {
            lastCaretPos = caretPos;
            var bracePos1 = -1;

            // Is there a brace to the left or right?
            if (caretPos > 0 && IsBrace(sc.GetCharAt(caretPos - 1)))
                bracePos1 = caretPos - 1;
            else if (IsBrace(sc.GetCharAt(caretPos)))
                bracePos1 = caretPos;

            if (bracePos1 >= 0)
            {
                // Find the matching brace
                var bracePos2 = sc.BraceMatch(bracePos1);
                if (bracePos2 == Scintilla.InvalidPosition)
                {
                    sc.BraceBadLight(bracePos1);
                    sc.HighlightGuide = 0;
                }
                else
                {
                    sc.BraceHighlight(bracePos1, bracePos2);
                    sc.HighlightGuide = sc.GetColumn(bracePos1);
                }
            }
            else
            {
                // Turn off brace matching
                sc.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                sc.HighlightGuide = 0;
            }
        }

        if ((e.Change & UpdateChange.Selection) > 0)
        {
            var currentPos = sc.CurrentPosition;
            var anchorPos = sc.AnchorPosition;
            if (anchorPos - currentPos == 0)
            {
                tsslPosText.Text = @"Pos:";
                tsslPos.Text = currentPos.ToString();
            }
            else
            {
                tsslPosText.Text = @"Sel:";
                tsslPos.Text = Math.Abs(anchorPos - currentPos).ToString();
            }

            tsslRow.Text = (sc.LineFromPosition(currentPos) + 1).ToString();
            tsslCol.Text = (sc.GetColumn(currentPos) + 1).ToString();
        }
    }

    private void SwitchDocument(Document nextDocument)
    {
        var prevDocument = sc.Document;
        sc.AddRefDocument(prevDocument);

        sc.Document = nextDocument;
        sc.ReleaseDocument(nextDocument);

        scText.SwitchedDocument();
    }

    private Document CreateDocument(string text)
    {
        var l = sc.CreateLoader(256);
        var chars = text.ToCharArray();
        if (chars.Length != 0) l.AddData(chars, text.Length);
        return l.ConvertToDocument();
    }

    private void lbTabTabs_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lbTabTabs.SelectedIndex != -1)
        {
            SwitchDocument(documents[lbTabTabs.SelectedIndex].Document);

            tsbSave.Enabled = documents[lbTabTabs.SelectedIndex].Unsaved;
            tsbUndo.Enabled = sc.CanUndo;
            tsbRedo.Enabled = sc.CanRedo;
            tsslTabTabName.Text = documents[lbTabTabs.SelectedIndex].TabTab.Key;
            tsslLen.Text = sc.Text.Length.ToString();
            tsslLines.Text = sc.Lines.Count.ToString();
        }
    }

    private void scText_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (char.IsControl(e.KeyChar))
            e.Handled = true;
    }

    private void scText_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            tsmiSave.Enabled = tsbSave.Enabled;
            tsmiUndo.Enabled = sc.CanUndo;
            tsmiRedo.Enabled = sc.CanRedo;
            tsmiCut.Enabled = sc.SelectedText.Length > 0;
            tsmiCopy.Enabled = sc.SelectedText.Length > 0;
            tsmiPaste.Enabled = sc.CanPaste;
            tsmiDelete.Enabled = sc.SelectedText.Length > 0;
            tsmiSelectAll.Enabled = sc.Text.Length > 0 && sc.Text.Length != sc.SelectedText.Length;
        }
    }

    internal class TabTabDoc
    {
        public TableTabTab TabTab { get; set; }
        public Document Document { get; set; }
        public bool Unsaved { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Unsaved ? "* " + TabTab.Key : TabTab.Key;
        }
    }
}