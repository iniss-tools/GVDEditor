using System.Text.RegularExpressions;
using AutocompleteMenuNS;
using JetBrains.Annotations;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ScintillaNET;
using ToolsCore;
using ToolsCore.Expressions;
using ToolsCore.TabTab;
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

    private readonly TableTabTab? SelectedTab;
    private readonly int homeStationId;
    private FTabTabPreview? preview;

    private int lastCaretPos;
    private int maxLineNumberCharLength;

    private readonly Scintilla sc;

    // kontrola pravidiel a podmienok (ToolsCore.TabTab) - indikatory v editore a zoznam problemov
    private const int SCI_SETILEXER = 4033;
    private const int IndicatorError = 8;
    private const int IndicatorWarning = 9;
    private const int IndicatorInfo = 10;
    private const int IndicatorGoTo = 11;

    private readonly GvdExprSymbols _symbols = new();
    private readonly System.Windows.Forms.Timer _validateTimer = new() { Interval = 400 };
    private IReadOnlyList<TabTabDiagnostic> _diagnostics = [];
    private string _validatedText = "";
    private readonly ExBindingList<ProblemRow> _problemRows = new() { Sortable = true };
    private readonly ShellIcon _iconError = new(ShellIconType.Error, ShellIconSize.Small);
    private readonly ShellIcon _iconWarning = new(ShellIconType.Warning, ShellIconSize.Small);
    private readonly ShellIcon _iconInfo = new(ShellIconType.Info, ShellIconSize.Small);
    
    /// <summary>
    ///     Vytvori novy formular typu <see cref="FTabTab"/>.
    /// </summary>
    /// <param name="tab">Sekcia, ktora sa ma otvorit.</param>
    /// <param name="station">Stanica grafikonu - pre nahlad (ZAJMSTANICE, MISTNI); moze byt <see langword="null"/>.</param>
    public FTabTab(TableTabTab? tab = null, Station? station = null)
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
        homeStationId = station is not null && int.TryParse(station.ID, out var sid) ? sid : 0;
        tsbPreview.Text = tsbPreview.ToolTipText = Resources.FTabTab_Nahlad;

        _validateTimer.Tick += (_, _) =>
        {
            _validateTimer.Stop();
            ValidateDocument();
        };
        sc.DwellStart += sc_DwellStart;
        sc.DwellEnd += (_, _) => sc.CallTipCancel();

        tsbProbGoTo.Text = tsmiProbGoTo.Text = Resources.FTabTab_Problems_Zobrazit;
        tsbProbFix.Text = tsmiProbFix.Text = Resources.FTabTab_Problems_Opravit;
        tsbProbErrors.Image = _iconError.ToBitmap();
        tsbProbWarnings.Image = _iconWarning.ToBitmap();
        tsbProbInfos.Image = _iconInfo.ToBitmap();
        dgvProblems.DataSource = _problemRows;
        dgvProblems.Sort(cProbLine, ListSortDirection.Ascending);
        dgvProblems_SelectionChanged(this, EventArgs.Empty);
        FormClosed += (_, _) =>
        {
            _iconError.Dispose();
            _iconWarning.Dispose();
            _iconInfo.Dispose();
        };
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
        // vyber textu vo farbe zvyraznenia temy (svetla: systemova modra, tmava: podla stylu), nie farbou ramika
        var highlight = GlobData.UsingStyle.ControlsColorScheme.Highlight;
        sc.SetSelectionBackColor(true, highlight.BackColor);
        sc.SetSelectionForeColor(true, highlight.ForeColor);
        sc.SetAdditionalSelBack(highlight.BackColor);
        sc.SetAdditionalSelFore(highlight.ForeColor);

        if (!GlobData.UsingStyle.ControlsDefaultStyle)
            sc.BorderStyle = ScintillaNET.BorderStyle.None;

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

        // Scintilla 5: SCI_SETILEXER s NULL = ziadny lexer, stylovanie robi kontajner (StyleNeeded).
        // sc.Lexer = Lexer.Container v Scintilla.NET 5.3 vyhodi "No lexer name was found".
        sc.DirectMessage(SCI_SETILEXER, IntPtr.Zero, IntPtr.Zero);

        //highlight active braces
        sc.IndentationGuides = IndentView.LookBoth;

        sc.Styles[Style.BraceLight].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.SelBraces.ForeColor;
        sc.Styles[Style.BraceLight].BackColor = GlobData.UsingStyle.TabTabEditorScheme.SelBraces.BackColor;
        sc.Styles[Style.BraceLight].Bold = GlobData.UsingStyle.TabTabEditorScheme.SelBraces.Bold;

        sc.Styles[Style.BraceBad].ForeColor = GlobData.UsingStyle.TabTabEditorScheme.SelBraceBad.ForeColor;
        sc.Styles[Style.BraceBad].BackColor = GlobData.UsingStyle.TabTabEditorScheme.SelBraceBad.BackColor;
        sc.Styles[Style.BraceBad].Bold = GlobData.UsingStyle.TabTabEditorScheme.SelBraceBad.Bold;

        sc.Indicators[IndicatorError].Style = IndicatorStyle.Squiggle;
        sc.Indicators[IndicatorError].ForeColor = Color.Red;
        sc.Indicators[IndicatorWarning].Style = IndicatorStyle.Squiggle;
        sc.Indicators[IndicatorWarning].ForeColor = Color.DarkOrange;
        sc.Indicators[IndicatorInfo].Style = IndicatorStyle.Dots;
        sc.Indicators[IndicatorInfo].ForeColor = Color.Gray;
        sc.Indicators[IndicatorGoTo].Style = IndicatorStyle.RoundBox;
        sc.Indicators[IndicatorGoTo].ForeColor = Color.Gold;
        sc.Indicators[IndicatorGoTo].Alpha = 70;
        sc.Indicators[IndicatorGoTo].OutlineAlpha = 160;
        sc.Indicators[IndicatorGoTo].Under = true;
        sc.MouseDwellTime = 500;

        var box = GlobData.UsingStyle.ControlsColorScheme.Box;
        dgvProblems.BackgroundColor = box.BackColor;
        dgvProblems.DefaultCellStyle.BackColor = box.BackColor;
        dgvProblems.DefaultCellStyle.ForeColor = box.ForeColor;
        dgvProblems.EnableHeadersVisualStyles = GlobData.UsingStyle.ControlsDefaultStyle;
        if (!GlobData.UsingStyle.ControlsDefaultStyle)
        {
            dgvProblems.ColumnHeadersDefaultCellStyle.BackColor = GlobData.UsingStyle.ControlsColorScheme.Button.BackColor;
            dgvProblems.ColumnHeadersDefaultCellStyle.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Button.ForeColor;
        }
        FormUtils.ChangeColorContextMenu(GlobData.UsingStyle, conMenuProblems);

        if (SelectedTab is not null)
            for (var i = 0; i < documents.Count; i++)
                if (documents[i].TabTab == SelectedTab)
                    lbTabTabs.SelectedIndex = i;

        ValidateDocument();
    }

    /// <summary>
    ///     Riadok v zozname problemov. Vlastnosti cita <see cref="dgvProblems"/> cez data binding
    ///     (<c>DataPropertyName</c> stlpcov), nie kod.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    internal sealed class ProblemRow(TabTabDiagnostic diagnostic)
    {
        public TabTabDiagnostic Diagnostic { get; } = diagnostic;

        /// <summary>Poradie pre triedenie: chyba 0, varovanie 1, informacia 2.</summary>
        public int Severity => diagnostic.Severity switch
        {
            ExprSeverity.Error => 0,
            ExprSeverity.Warning => 1,
            _ => 2
        };

        public string Code => diagnostic.CodeName;
        public int Line => diagnostic.LineIndex + 1;
        public string Message => diagnostic.Message;
        public string Solution => diagnostic.Suggestion ?? "";
    }

    /// <summary>
    ///     Skontroluje text aktualnej sekcie, podciarkne problemy v editore a naplni zoznam problemov.
    /// </summary>
    private void ValidateDocument()
    {
        if (lbTabTabs.SelectedIndex == -1)
        {
            _diagnostics = [];
            _problemRows.Clear();
            UpdateProblemCounts(0, 0, 0);
            return;
        }

        var tab = documents[lbTabTabs.SelectedIndex].TabTab;
        var text = sc.Text;
        _validatedText = text;
        var result = TabTabValidator.Validate(text, _symbols.OptionsFor(tab));
        _diagnostics = result.Diagnostics;

        foreach (var ind in new[] { IndicatorError, IndicatorWarning, IndicatorInfo, IndicatorGoTo })
        {
            sc.IndicatorCurrent = ind;
            sc.IndicatorClearRange(0, sc.TextLength);
        }

        _problemRows.RaiseListChangedEvents = false;
        _problemRows.Clear();
        foreach (var d in _diagnostics)
        {
            var (start, end) = CharRange(text, d);
            sc.IndicatorCurrent = d.Severity switch
            {
                ExprSeverity.Error => IndicatorError,
                ExprSeverity.Warning => IndicatorWarning,
                _ => IndicatorInfo
            };
            sc.IndicatorFillRange(start, Math.Max(1, end - start));
            _problemRows.Add(new ProblemRow(d));
        }
        _problemRows.RaiseListChangedEvents = true;
        _problemRows.ResetBindings();

        UpdateProblemCounts(result.ErrorCount, result.WarningCount, _diagnostics.Count - result.ErrorCount - result.WarningCount);
        ApplyProblemFilter();

        if (preview is { IsDisposed: false })
            preview.RefreshPreview();
    }

    /// <summary>
    ///     Text sekcie podla mena - z editora (aj neulozeny), nie z GlobData.
    /// </summary>
    private string? SectionText(string name)
    {
        var doc = documents.FirstOrDefault(d => d.TabTab.Key == name);
        if (doc is null) return null;
        if (lbTabTabs.SelectedIndex != -1 && documents[lbTabTabs.SelectedIndex] == doc)
            return sc.Text;

        // text ineho dokumentu Scintilly: docasne prepnut a precitat
        var current = sc.Document;
        sc.AddRefDocument(current);
        sc.Document = doc.Document;
        var text = sc.Text;
        sc.Document = current;
        sc.ReleaseDocument(current);
        return text;
    }

    private void tsbPreview_Click(object sender, EventArgs e)
    {
        var section = lbTabTabs.SelectedIndex == -1 ? null : documents[lbTabTabs.SelectedIndex].TabTab.Key;
        if (preview is { IsDisposed: false })
        {
            preview.Close();
        }
        preview = new FTabTabPreview(SectionText, section, homeStationId) { Owner = this };
        preview.Show(this);
    }

    private void UpdateProblemCounts(int errors, int warnings, int infos)
    {
        tsbProbErrors.Text = string.Format(Resources.FTabTab_Problems_Chyby, errors);
        tsbProbWarnings.Text = string.Format(Resources.FTabTab_Problems_Varovania, warnings);
        tsbProbInfos.Text = string.Format(Resources.FTabTab_Problems_Spravy, infos);

        if (errors + warnings == 0)
        {
            tsslProblems.Image = GlobalResources.correct;
            tsslProblems.Text = Resources.FTabTab_Bez_problemov;
            tsslProblems.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Panel.ForeColor;
        }
        else
        {
            tsslProblems.Image = errors > 0 ? _iconError.ToBitmap() : _iconWarning.ToBitmap();
            tsslProblems.Text = string.Format(Resources.FTabTab_Stav_kontroly, errors, warnings);
            tsslProblems.ForeColor = errors > 0 ? Color.Red : GlobData.UsingStyle.ControlsColorScheme.Panel.ForeColor;
        }
    }

    /// <summary>
    ///     Skryje riadky podla prepinacov Chyby / Varovania / Spravy.
    /// </summary>
    private void ApplyProblemFilter()
    {
        if (dgvProblems.DataSource is null || !IsHandleCreated) return;

        var cm = (CurrencyManager?)BindingContext?[dgvProblems.DataSource];
        cm?.SuspendBinding();
        foreach (DataGridViewRow row in dgvProblems.Rows)
        {
            if (row.DataBoundItem is not ProblemRow pr) continue;
            row.Visible = pr.Diagnostic.Severity switch
            {
                ExprSeverity.Error => tsbProbErrors.Checked,
                ExprSeverity.Warning => tsbProbWarnings.Checked,
                _ => tsbProbInfos.Checked
            };
        }
        cm?.ResumeBinding();
    }

    private ProblemRow? SelectedProblem =>
        dgvProblems.SelectedRows.Count > 0 ? dgvProblems.SelectedRows[0].DataBoundItem as ProblemRow : null;

    /// <summary>
    ///     Rozsah hlasenia v znakoch (ScintillaNET pracuje so znakovymi poziciami a na bajty prevadza sam).
    ///     Bodove hlasenie zvyrazni jeden znak; na konci textu znak pred nim.
    /// </summary>
    private static (int Start, int End) CharRange(string text, TabTabDiagnostic d)
    {
        var s = Math.Clamp(d.Start, 0, text.Length);
        var e = Math.Clamp(d.End, s, text.Length);
        if (e == s)
        {
            if (s < text.Length) e = s + 1;
            else if (s > 0) s--;
        }
        return (s, e);
    }

    private void sc_DwellStart(object? sender, DwellEventArgs e)
    {
        if (e.Position < 0 || _diagnostics.Count == 0 || _validatedText != sc.Text)
            return;

        var text = _validatedText;
        var hits = new List<string>();
        foreach (var d in _diagnostics)
        {
            var (start, end) = CharRange(text, d);
            if (e.Position >= start && e.Position < end)
                hits.Add(d.Suggestion is null ? d.Message : $"{d.Message}\n→ {d.Suggestion}");
        }

        if (hits.Count > 0)
            sc.CallTipShow(e.Position, string.Join("\n", hits));
    }

    private void GoToDiagnostic(TabTabDiagnostic d)
    {
        if (_validatedText != sc.Text) ValidateDocument();
        var (start, end) = CharRange(_validatedText, d);

        // zvyraznenie miesta problemu - nie vyberom (jeho farba je v svetlej teme prilis tmava),
        // ale docasnym indikatorom, ktory zmizne pri dalsej kontrole alebo skoku
        sc.IndicatorCurrent = IndicatorGoTo;
        sc.IndicatorClearRange(0, sc.TextLength);
        sc.IndicatorFillRange(start, Math.Max(1, end - start));

        sc.GotoPosition(start);
        sc.ScrollCaret();
        sc.Focus();
    }

    /// <summary>
    ///     Pouzije navrhovanu opravu na text v editore (ako jednu akciu pre Undo) a znova skontroluje.
    /// </summary>
    private void ApplyFix(TabTabDiagnostic d)
    {
        if (d.Fix is null) return;
        if (_validatedText != sc.Text)
        {
            // text sa medzitym zmenil - pozicie opravy uz nemusia sediet
            ValidateDocument();
            return;
        }

        var text = _validatedText;
        sc.BeginUndoAction();
        foreach (var edit in d.Fix.Edits.OrderByDescending(x => x.Start))
        {
            sc.DeleteRange(edit.Start, edit.Length);
            sc.InsertText(edit.Start, edit.NewText);
        }
        sc.EndUndoAction();

        _validateTimer.Stop();
        ValidateDocument();
    }

    private void tsbProbFilter_CheckedChanged(object sender, EventArgs e) => ApplyProblemFilter();

    private void tsbProbGoTo_Click(object sender, EventArgs e)
    {
        if (SelectedProblem is { } p) GoToDiagnostic(p.Diagnostic);
    }

    private void tsbProbFix_Click(object sender, EventArgs e)
    {
        if (SelectedProblem is { } p) ApplyFix(p.Diagnostic);
    }

    private void dgvProblems_SelectionChanged(object sender, EventArgs e)
    {
        var p = SelectedProblem;
        tsbProbGoTo.Enabled = tsmiProbGoTo.Enabled = p is not null;
        tsbProbFix.Enabled = tsmiProbFix.Enabled = p?.Diagnostic.Fix is not null;
        tsbProbFix.ToolTipText = tsmiProbFix.ToolTipText = p?.Diagnostic.Fix?.Title ?? Resources.FTabTab_Problems_Opravit;
    }

    private void dgvProblems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex != cProbType.Index || e.RowIndex < 0) return;
        if (dgvProblems.Rows[e.RowIndex].DataBoundItem is not ProblemRow pr) return;

        var cell = dgvProblems.Rows[e.RowIndex].Cells[e.ColumnIndex];
        switch (pr.Diagnostic.Severity)
        {
            case ExprSeverity.Error:
                e.Value = _iconError.ToBitmap();
                cell.ToolTipText = Resources.FTabTab_Problems_Chyba;
                break;
            case ExprSeverity.Warning:
                e.Value = _iconWarning.ToBitmap();
                cell.ToolTipText = Resources.FTabTab_Problems_Varovanie;
                break;
            default:
                e.Value = _iconInfo.ToBitmap();
                cell.ToolTipText = Resources.FTabTab_Problems_Informacia;
                break;
        }
        e.FormattingApplied = true;
    }

    private void dgvProblems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && dgvProblems.Rows[e.RowIndex].DataBoundItem is ProblemRow pr)
            GoToDiagnostic(pr.Diagnostic);
    }

    private void dgvProblems_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        // klik na kod otvori dokumentaciu jazyka / TabTab
        if (e.RowIndex < 0 || e.ColumnIndex != cProbCode.Index) return;
        if (dgvProblems.Rows[e.RowIndex].DataBoundItem is not ProblemRow pr) return;

        Utils.OpenShell(pr.Diagnostic.ExprCode is not null ? LinkConsts.LINK_DOC_VYRAZY : LinkConsts.LINK_DOC_TABTAB);
    }

    private void dgvProblems_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
        {
            dgvProblems.ClearSelection();
            dgvProblems.Rows[e.RowIndex].Selected = true;
        }
    }

    private void dgvProblems_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter && SelectedProblem is { } p)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            GoToDiagnostic(p.Diagnostic);
        }
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

        _validateTimer.Stop();
        _validateTimer.Start();
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

            _validateTimer.Stop();
            ValidateDocument();
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
        public TableTabTab TabTab { get; set; } = null!;
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