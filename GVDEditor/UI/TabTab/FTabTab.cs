using System.Globalization;
using AutocompleteMenuNS;
using JetBrains.Annotations;
using ExControls;
using GVDEditor.Domain.Entities;
using GVDEditor.TabTabEditor;
using GVDEditor.Properties;
using ScintillaNET;
using ToolsCore;
using ToolsCore.Commands;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.TabTab;
using ToolsCore.Tools;

namespace GVDEditor.UI.TabTab;

/// <summary>
/// Dialog - Nastavenie TabTabs.
/// </summary>
internal partial class FTabTab : Form
{
    /// <summary>
    /// Kontext editora - nastavenia programu, instalacia INISS a otvoreny grafikon.
    /// </summary>
    private readonly EditorContext _ctx;

    private static readonly string[] Operatory = ["AND", "OR", "NOT", "ODD"];

    private readonly TabTabLexer _cSharpLexer = new(
        TabTabACItems.GetFunctionItems().Select(item => item.FunctionName),
        TabTabACItems.GetEventItems().Select(item => item.MenuText),
        TabTabACItems.GetConstantItems().Select(item => item.ConstName),
        Operatory);

    private readonly BindingList<TabTabDoc> _documents = [];

    private readonly TableTabTab? _selectedTab;
    private readonly int _homeStationId;
    private FTabTabPreview? _preview;

    private int _lastCaretPos;
    private int _maxLineNumberCharLength;

    private readonly Scintilla _sc;

    // kontrola pravidiel a podmienok (ToolsCore.TabTab) - indikatory v editore a zoznam problemov
    private const int SCI_SETILEXER = 4033;
    private const int IndicatorError = 8;
    private const int IndicatorWarning = 9;
    private const int IndicatorInfo = 10;
    private const int IndicatorGoTo = 11;

    private readonly GvdExprSymbols _symbols;
    private readonly System.Windows.Forms.Timer _validateTimer = new() { Interval = 400 };
    private IReadOnlyList<TabTabDiagnostic> _diagnostics = [];
    private string _validatedText = "";
    private readonly ExBindingList<ProblemRow> _problemRows = new() { Sortable = true };
    private readonly ShellIcon _iconError = new(ShellIconType.Error, ShellIconSize.Small);
    private readonly ShellIcon _iconWarning = new(ShellIconType.Warning, ShellIconSize.Small);
    private readonly ShellIcon _iconInfo = new(ShellIconType.Info, ShellIconSize.Small);
    
    /// <summary>
    /// Vytvori novy formular typu <see cref="FTabTab"/>.
    /// </summary>
    /// <param name="tab">Sekcia, ktora sa ma otvorit.</param>
    /// <param name="station">Stanica grafikonu - pre nahlad (ZAJMSTANICE, MISTNI); moze byt <see langword="null"/>.</param>
    public FTabTab(EditorContext context, TableTabTab? tab = null, Station? station = null)
    {
        _ctx = context;
        _symbols = new GvdExprSymbols(context.Workspace, context.Document);
        InitializeComponent();

        if (_ctx.UsingStyle.DarkTitleBar) ExTools.SetImmersiveDarkMode(Handle, true);

        _sc = scText.Scintilla;
        acMenu.TargetControlWrapper = new ScintillaWrapper(_sc);

        foreach (var tabTab in _ctx.Document.TabTabs) 
            _documents.Add(new TabTabDoc { Document = CreateDocument(tabTab.Text), TabTab = tabTab, Key = tabTab.Key });

        lbTabTabs.DataSource = _documents;

        _sc.EmptyUndoBuffer();

        acMenu.SetAutocompleteItems(TabTabACItems.GetItems());

        tsbUndo.Enabled = false;
        tsbRedo.Enabled = false;
        CreateCommands();

        ShowNumberLines();

        _selectedTab = tab;
        _homeStationId = station is not null && int.TryParse(station.ID, out var sid) ? sid : 0;
        tsbPreview.Text = tsbPreview.ToolTipText = Resources.FTabTab_Nahlad;

        _validateTimer.Tick += (_, _) =>
        {
            _validateTimer.Stop();
            // kontrola naplanovana tesne pred zatvorenim okna by siahla na zruseny editor Scintilla (pad programu)
            if (IsDisposed || _sc.IsDisposed || !_sc.IsHandleCreated)
                return;
            ValidateDocument();
        };
        Disposed += (_, _) => _validateTimer.Dispose();
        _sc.DwellStart += sc_DwellStart;
        _sc.DwellEnd += (_, _) => _sc.CallTipCancel();

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
            _validateTimer.Stop();
            _iconError.Dispose();
            _iconWarning.Dispose();
            _iconInfo.Dispose();
        };
    }

    private void FTabTab_Load(object sender, EventArgs e)
    {
        lbTabTabs.Font = _ctx.Config.Fonts.Menu; //_ctx.UsingStyle.TabTabEditorScheme.Font;

        _sc.StyleResetDefault();
        _sc.Styles[Style.Default].Font = _ctx.UsingStyle.TabTabEditorScheme.Font.Name;
        _sc.Styles[Style.Default].SizeF = _ctx.UsingStyle.TabTabEditorScheme.Font.Size;
        _sc.Styles[Style.Default].BackColor = _ctx.UsingStyle.ControlsColorScheme.Box.BackColor;
        _sc.StyleClearAll();
        _sc.Styles[Style.LineNumber].BackColor = _ctx.UsingStyle.ControlsColorScheme.Button.BackColor;
        _sc.Styles[Style.LineNumber].ForeColor = _ctx.UsingStyle.ControlsColorScheme.Button.ForeColor;
        _sc.CaretForeColor = _ctx.UsingStyle.ControlsColorScheme.Box.ForeColor;
        // vyber textu vo farbe zvyraznenia temy (svetla: systemova modra, tmava: podla stylu), nie farbou ramika
        var highlight = _ctx.UsingStyle.ControlsColorScheme.Highlight;
        _sc.SetSelectionBackColor(true, highlight.BackColor);
        _sc.SetSelectionForeColor(true, highlight.ForeColor);
        _sc.SetAdditionalSelBack(highlight.BackColor);
        _sc.SetAdditionalSelFore(highlight.ForeColor);

        if (!_ctx.UsingStyle.ControlsDefaultStyle)
            _sc.BorderStyle = ScintillaNET.BorderStyle.None;

        if (_ctx.UsingStyle.DarkScrollBar)
        {
            scText.VScrollBarControl.SetTheme(WindowsTheme.DarkExplorer);
            scText.HScrollBarControl.SetTheme(WindowsTheme.DarkExplorer);
        }

        this.ApplyThemeAndFonts();

        FormUtils.ChangeColorContextMenu(_ctx.UsingStyle, conMenuScText);

        acMenu.Colors.BackColor = _ctx.UsingStyle.ControlsColorScheme.Panel.BackColor;
        acMenu.Colors.ForeColor = _ctx.UsingStyle.ControlsColorScheme.Panel.ForeColor;
        acMenu.Colors.SelectedForeColor = _ctx.UsingStyle.ControlsColorScheme.Panel.ForeColor;

        _sc.Styles[TabTabStyle.Default].ForeColor = _ctx.UsingStyle.TabTabEditorScheme.Default.ForeColor;
        _sc.Styles[TabTabStyle.Default].Bold = _ctx.UsingStyle.TabTabEditorScheme.Default.Bold;

        _sc.Styles[TabTabStyle.Function].ForeColor = _ctx.UsingStyle.TabTabEditorScheme.Function.ForeColor;
        _sc.Styles[TabTabStyle.Function].Bold = _ctx.UsingStyle.TabTabEditorScheme.Function.Bold;

        _sc.Styles[TabTabStyle.Identifier].ForeColor = _ctx.UsingStyle.TabTabEditorScheme.Identifier.ForeColor;
        _sc.Styles[TabTabStyle.Identifier].Bold = _ctx.UsingStyle.TabTabEditorScheme.Identifier.Bold;

        _sc.Styles[TabTabStyle.Number].ForeColor = _ctx.UsingStyle.TabTabEditorScheme.Number.ForeColor;
        _sc.Styles[TabTabStyle.Number].Bold = _ctx.UsingStyle.TabTabEditorScheme.Number.Bold;

        _sc.Styles[TabTabStyle.String].ForeColor = _ctx.UsingStyle.TabTabEditorScheme.String.ForeColor;
        _sc.Styles[TabTabStyle.String].Bold = _ctx.UsingStyle.TabTabEditorScheme.String.Bold;

        _sc.Styles[TabTabStyle.Comment].ForeColor = _ctx.UsingStyle.TabTabEditorScheme.Comment.ForeColor;
        _sc.Styles[TabTabStyle.Comment].Bold = _ctx.UsingStyle.TabTabEditorScheme.Comment.Bold;

        _sc.Styles[TabTabStyle.Var].ForeColor = _ctx.UsingStyle.TabTabEditorScheme.Var.ForeColor;
        _sc.Styles[TabTabStyle.Var].Bold = _ctx.UsingStyle.TabTabEditorScheme.Var.Bold;

        _sc.Styles[TabTabStyle.Event].ForeColor = _ctx.UsingStyle.TabTabEditorScheme.Event.ForeColor;
        _sc.Styles[TabTabStyle.Event].Bold = _ctx.UsingStyle.TabTabEditorScheme.Event.Bold;

        _sc.Styles[TabTabStyle.OnNewLine].ForeColor = _ctx.UsingStyle.TabTabEditorScheme.OnNewLine.ForeColor;
        _sc.Styles[TabTabStyle.OnNewLine].Bold = _ctx.UsingStyle.TabTabEditorScheme.OnNewLine.Bold;

        _sc.Styles[TabTabStyle.Operator].ForeColor = _ctx.UsingStyle.TabTabEditorScheme.Operator.ForeColor;
        _sc.Styles[TabTabStyle.Operator].Bold = _ctx.UsingStyle.TabTabEditorScheme.Operator.Bold;

        _sc.Styles[TabTabStyle.Constant].ForeColor = _ctx.UsingStyle.TabTabEditorScheme.Constant.ForeColor;
        _sc.Styles[TabTabStyle.Constant].Bold = _ctx.UsingStyle.TabTabEditorScheme.Constant.Bold;

        // Scintilla 5: SCI_SETILEXER s NULL = ziadny lexer, stylovanie robi kontajner (StyleNeeded).
        // sc.Lexer = Lexer.Container v Scintilla.NET 5.3 vyhodi "No lexer name was found".
        _sc.DirectMessage(SCI_SETILEXER, IntPtr.Zero, IntPtr.Zero);

        //highlight active braces
        _sc.IndentationGuides = IndentView.LookBoth;

        _sc.Styles[Style.BraceLight].ForeColor = _ctx.UsingStyle.TabTabEditorScheme.SelBraces.ForeColor;
        _sc.Styles[Style.BraceLight].BackColor = _ctx.UsingStyle.TabTabEditorScheme.SelBraces.BackColor;
        _sc.Styles[Style.BraceLight].Bold = _ctx.UsingStyle.TabTabEditorScheme.SelBraces.Bold;

        _sc.Styles[Style.BraceBad].ForeColor = _ctx.UsingStyle.TabTabEditorScheme.SelBraceBad.ForeColor;
        _sc.Styles[Style.BraceBad].BackColor = _ctx.UsingStyle.TabTabEditorScheme.SelBraceBad.BackColor;
        _sc.Styles[Style.BraceBad].Bold = _ctx.UsingStyle.TabTabEditorScheme.SelBraceBad.Bold;

        _sc.Indicators[IndicatorError].Style = IndicatorStyle.Squiggle;
        _sc.Indicators[IndicatorError].ForeColor = Color.Red;
        _sc.Indicators[IndicatorWarning].Style = IndicatorStyle.Squiggle;
        _sc.Indicators[IndicatorWarning].ForeColor = Color.DarkOrange;
        _sc.Indicators[IndicatorInfo].Style = IndicatorStyle.Dots;
        _sc.Indicators[IndicatorInfo].ForeColor = Color.Gray;
        _sc.Indicators[IndicatorGoTo].Style = IndicatorStyle.RoundBox;
        _sc.Indicators[IndicatorGoTo].ForeColor = Color.Gold;
        _sc.Indicators[IndicatorGoTo].Alpha = 70;
        _sc.Indicators[IndicatorGoTo].OutlineAlpha = 160;
        _sc.Indicators[IndicatorGoTo].Under = true;
        _sc.MouseDwellTime = 500;

        var box = _ctx.UsingStyle.ControlsColorScheme.Box;
        dgvProblems.BackgroundColor = box.BackColor;
        dgvProblems.DefaultCellStyle.BackColor = box.BackColor;
        dgvProblems.DefaultCellStyle.ForeColor = box.ForeColor;
        dgvProblems.EnableHeadersVisualStyles = _ctx.UsingStyle.ControlsDefaultStyle;
        if (!_ctx.UsingStyle.ControlsDefaultStyle)
        {
            dgvProblems.ColumnHeadersDefaultCellStyle.BackColor = _ctx.UsingStyle.ControlsColorScheme.Button.BackColor;
            dgvProblems.ColumnHeadersDefaultCellStyle.ForeColor = _ctx.UsingStyle.ControlsColorScheme.Button.ForeColor;
        }
        FormUtils.ChangeColorContextMenu(_ctx.UsingStyle, conMenuProblems);

        if (_selectedTab is not null)
            for (var i = 0; i < _documents.Count; i++)
                if (_documents[i].TabTab == _selectedTab)
                    lbTabTabs.SelectedIndex = i;

        ValidateDocument();
    }

    /// <summary>
    /// Riadok v zozname problemov. Vlastnosti cita <see cref="dgvProblems"/> cez data binding
    /// (<c>DataPropertyName</c> stlpcov), nie kod.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    internal sealed class ProblemRow(TabTabDiagnostic diagnostic)
    {
        public TabTabDiagnostic Diagnostic { get; } = diagnostic;

        /// <summary>Poradie pre triedenie: chyba 0, varovanie 1, informacia 2.</summary>
        public int Severity => Diagnostic.Severity switch
        {
            ExprSeverity.Error => 0,
            ExprSeverity.Warning => 1,
            _ => 2
        };

        public string Code => Diagnostic.CodeName;
        public int Line => Diagnostic.LineIndex + 1;
        public string Message => Diagnostic.Message;
        public string Solution => Diagnostic.Suggestion ?? "";
    }

    /// <summary>
    /// Skontroluje text aktualnej sekcie, podciarkne problemy v editore a naplni zoznam problemov.
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

        var tab = _documents[lbTabTabs.SelectedIndex].TabTab;
        var text = _sc.Text;
        _validatedText = text;
        var result = TabTabValidator.Validate(text, _symbols.OptionsFor(tab));
        _diagnostics = result.Diagnostics;

        foreach (var ind in new[] { IndicatorError, IndicatorWarning, IndicatorInfo, IndicatorGoTo })
        {
            _sc.IndicatorCurrent = ind;
            _sc.IndicatorClearRange(0, _sc.TextLength);
        }

        _problemRows.RaiseListChangedEvents = false;
        _problemRows.Clear();
        foreach (var d in _diagnostics)
        {
            var (start, end) = CharRange(text, d);
            _sc.IndicatorCurrent = d.Severity switch
            {
                ExprSeverity.Error => IndicatorError,
                ExprSeverity.Warning => IndicatorWarning,
                _ => IndicatorInfo
            };
            _sc.IndicatorFillRange(start, Math.Max(1, end - start));
            _problemRows.Add(new ProblemRow(d));
        }
        _problemRows.RaiseListChangedEvents = true;
        _problemRows.ResetBindings();

        UpdateProblemCounts(result.ErrorCount, result.WarningCount, _diagnostics.Count - result.ErrorCount - result.WarningCount);
        ApplyProblemFilter();

        if (_preview is { IsDisposed: false })
            _preview.RefreshPreview();
    }

    /// <summary>
    /// Text sekcie podla mena - z editora (aj neulozeny), nie z grafikonu.
    /// </summary>
    private string? SectionText(string name)
    {
        var doc = _documents.FirstOrDefault(d => d.TabTab.Key == name);
        if (doc is null) return null;
        if (lbTabTabs.SelectedIndex != -1 && _documents[lbTabTabs.SelectedIndex] == doc)
            return _sc.Text;

        // text ineho dokumentu Scintilly: docasne prepnut a precitat
        var current = _sc.Document;
        _sc.AddRefDocument(current);
        _sc.Document = doc.Document;
        var text = _sc.Text;
        _sc.Document = current;
        _sc.ReleaseDocument(current);
        return text;
    }

    private void tsbPreview_Click(object sender, EventArgs e)
    {
        var section = lbTabTabs.SelectedIndex == -1 ? null : _documents[lbTabTabs.SelectedIndex].TabTab.Key;
        if (_preview is { IsDisposed: false })
        {
            _preview.Close();
        }
        _preview = new FTabTabPreview(_ctx, SectionText, section, _homeStationId) { Owner = this };
        _preview.Show(this);
    }

    private void UpdateProblemCounts(int errors, int warnings, int infos)
    {
        tsbProbErrors.Text = string.Format(CultureInfo.CurrentCulture, Resources.FTabTab_Problems_Chyby, errors);
        tsbProbWarnings.Text = string.Format(CultureInfo.CurrentCulture, Resources.FTabTab_Problems_Varovania, warnings);
        tsbProbInfos.Text = string.Format(CultureInfo.CurrentCulture, Resources.FTabTab_Problems_Spravy, infos);

        if (errors + warnings == 0)
        {
            tsslProblems.Image = GlobalResources.correct;
            tsslProblems.Text = Resources.FTabTab_Bez_problemov;
            tsslProblems.ForeColor = _ctx.UsingStyle.ControlsColorScheme.Panel.ForeColor;
        }
        else
        {
            tsslProblems.Image = errors > 0 ? _iconError.ToBitmap() : _iconWarning.ToBitmap();
            tsslProblems.Text = string.Format(CultureInfo.CurrentCulture, Resources.FTabTab_Stav_kontroly, errors, warnings);
            tsslProblems.ForeColor = errors > 0 ? Color.Red : _ctx.UsingStyle.ControlsColorScheme.Panel.ForeColor;
        }
    }

    /// <summary>
    /// Skryje riadky podla prepinacov Chyby / Varovania / Spravy.
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
    /// Rozsah hlasenia v znakoch (ScintillaNET pracuje so znakovymi poziciami a na bajty prevadza sam).
    /// Bodove hlasenie zvyrazni jeden znak; na konci textu znak pred nim.
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
        if (e.Position < 0 || _diagnostics.Count == 0 || _validatedText != _sc.Text)
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
            _sc.CallTipShow(e.Position, string.Join("\n", hits));
    }

    private void GoToDiagnostic(TabTabDiagnostic d)
    {
        if (_validatedText != _sc.Text) ValidateDocument();
        var (start, end) = CharRange(_validatedText, d);

        // zvyraznenie miesta problemu - nie vyberom (jeho farba je v svetlej teme prilis tmava),
        // ale docasnym indikatorom, ktory zmizne pri dalsej kontrole alebo skoku
        _sc.IndicatorCurrent = IndicatorGoTo;
        _sc.IndicatorClearRange(0, _sc.TextLength);
        _sc.IndicatorFillRange(start, Math.Max(1, end - start));

        _sc.GotoPosition(start);
        _sc.ScrollCaret();
        _sc.Focus();
    }

    /// <summary>
    /// Pouzije navrhovanu opravu na text v editore (ako jednu akciu pre Undo) a znova skontroluje.
    /// </summary>
    private void ApplyFix(TabTabDiagnostic d)
    {
        if (d.Fix is null) return;
        if (_validatedText != _sc.Text)
        {
            // text sa medzitym zmenil - pozicie opravy uz nemusia sediet
            ValidateDocument();
            return;
        }

        var text = _validatedText;
        _sc.BeginUndoAction();
        foreach (var edit in d.Fix.Edits.OrderByDescending(x => x.Start))
        {
            _sc.DeleteRange(edit.Start, edit.Length);
            _sc.InsertText(edit.Start, edit.NewText);
        }
        _sc.EndUndoAction();

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

        Utils.OpenShell(pr.Diagnostic.ExprCode is not null ? GvdLinkConsts.LinkDocVyrazy : GvdLinkConsts.LinkDocTabtab);
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

    /// <summary>
    /// Zoznam sekcii v editore sa lisi od ulozeneho (pridana, odstranena alebo premenovana sekcia).
    /// </summary>
    private bool SectionsUnsaved =>
        _documents.Any(doc => doc.KeyUnsaved) || !_documents.Select(doc => doc.TabTab).SequenceEqual(_ctx.Document.TabTabs);

    /// <summary>
    /// Prenesie zoznam sekcii (pridane, odstranene, premenovane) do <see cref="_ctx.Document.TabTabs"/>.
    /// Vola sa pri kazdom ulozeni - text sekcii sa uklada zvlast (<see cref="DoSave"/>, <see cref="DoSaveAll"/>).
    /// Objekty sekcii ostavaju tie iste, aby odkazy TAB1/TAB2 katalogovych tabul ostali platne.
    /// </summary>
    private void SaveSections()
    {
        TabTabSections.Apply(_documents.Select(doc => (doc.TabTab, doc.Key)).ToList(), _ctx.Document.TabTabs);
        foreach (var doc in _documents)
            doc.KeyUnsaved = false;

        _ctx.Document.TabTabs.ResetBindings();
    }

    private void FTabTab_FormClosing(object sender, FormClosingEventArgs e)
    {
        // pri zatvoreni bez ulozenia sa neulozene texty aj zmeny zoznamu sekcii zahodia - grafikon drzi posledny ulozeny stav
        var unsaved = _documents.Any(doc => doc.Unsaved) || SectionsUnsaved;

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
            _documents[lbTabTabs.SelectedIndex].TabTab.Text = _sc.Text;
            _documents[lbTabTabs.SelectedIndex].Unsaved = false;
        }

        SaveSections();
        _documents.ResetBindings();
        tsbSave.Enabled = false;
    }

    private void DoSaveAll()
    {
        var current = _sc.Document;

        foreach (var doc in _documents)
        {
            SwitchDocument(doc.Document);
            doc.TabTab.Text = _sc.Text;
            doc.Unsaved = false;
        }

        _sc.Document = current;
        SaveSections();
        _documents.ResetBindings();
        tsbSave.Enabled = false;
    }

    private void DoUndo()
    {
        _sc.Undo();

        if (!_sc.CanUndo && lbTabTabs.SelectedIndex != -1)
        {
            _documents[lbTabTabs.SelectedIndex].Unsaved = false;
            _documents.ResetBindings();
        }
    }

    private void DoRedo() => _sc.Redo();

    private void DoAddTab()
    {
        using var frtt = new FTabTabRename(null, _documents.Select(doc => doc.Key));
        if (frtt.ShowDialog(this) == DialogResult.OK)
        {
            // do _ctx.Document.TabTabs sa sekcia dostane az pri ulozeni (SaveSections)
            _documents.Add(new TabTabDoc
            {
                KeyUnsaved = true, Key = frtt.NewTabName, Document = CreateDocument(""),
                TabTab = new TableTabTab { Key = frtt.NewTabName, Text = "" }
            });
            lbTabTabs.SelectedIndex = _documents.Count - 1;
            tsbSave.Enabled = true;
        }
    }

    private void DoRemoveTab()
    {
        if (lbTabTabs.SelectedIndex == -1)
            return;

        // index v editore sa po pridani/odstraneni sekcie nezhoduje s _ctx.Document.TabTabs - kontroluje sa objekt dokumentu
        var index = lbTabTabs.SelectedIndex;
        if (TabTabSections.RemoveBlockedMessage(_documents[index].TabTab, _ctx.Document.TableCatalogs) is { } blocked)
        {
            Utils.ShowError(blocked);
            return;
        }

        _documents.RemoveAt(index);
        tsbSave.Enabled = true;
    }

    private void DoRenameTab()
    {
        if (lbTabTabs.SelectedIndex == -1)
            return;

        var doc = _documents[lbTabTabs.SelectedIndex];
        using var frtt = new FTabTabRename(doc.Key, _documents.Where(d => d != doc).Select(d => d.Key));
        if (frtt.ShowDialog(this) == DialogResult.OK && frtt.NewTabName != doc.Key)
        {
            // TabTab.Key sa zmeni az pri ulozeni (SaveSections), aby Odist bez ulozenia vratilo povodny nazov
            doc.Key = frtt.NewTabName;
            doc.KeyUnsaved = true;
            tsslTabTabName.Text = doc.Key;
            tsbSave.Enabled = true;
            _documents.ResetBindings();
        }
    }

    private void DoFindReplace(bool showReplace = false)
    {
        var ffar = new FTabTabFindReplace(_sc, showReplace);
        ffar.Show();
    }

    private void DoReformat()
    {
        // formatuju sa len podmienky pravidiel #SWITCH/#MERGE - texty pre tabulu (aj v uvodzovkach) ostanu, ako su
        var text = _sc.Text;
        var formatted = TabTabFormatter.Format(text);
        if (formatted == text)
            return;

        var pos = _sc.CurrentPosition;
        var firstLine = _sc.FirstVisibleLine;

        _sc.BeginUndoAction();
        _sc.Text = formatted;
        _sc.EndUndoAction();

        _sc.GotoPosition(Math.Min(pos, _sc.TextLength));
        _sc.FirstVisibleLine = firstLine;
    }

    /// <summary>
    /// Prikazy panela nastrojov a kontextovych ponuk - tlacidlo a polozka ponuky robia to iste. Skratky su pevne
    /// (zobrazene pri polozkach ponuky); Ctrl+S, Ctrl+F a Ctrl+H spracuva <see cref="FTabTab_KeyDown" />.
    /// </summary>
    private void CreateCommands()
    {
        var commands = new CommandSet();
        void Add(string id, Shortcut shortcut, Action execute, params ToolStripItem[] items) =>
            commands.Add(new CommandInfo(id, id, shortcut), execute).Bind(items);

        Add("Save", Shortcut.None, DoSave, tsbSave, tsmiSave);
        Add("SaveAll", Shortcut.CtrlShiftS, DoSaveAll, tsbSaveAll, tsmiSaveAll);
        Add("Undo", Shortcut.CtrlZ, DoUndo, tsbUndo, tsmiUndo);
        Add("Redo", Shortcut.CtrlY, DoRedo, tsbRedo, tsmiRedo);
        Add("AddTab", Shortcut.None, DoAddTab, tsbAddTab, tsmiAddTabTab);
        Add("RemoveTab", Shortcut.None, DoRemoveTab, tsbRemoveTab, tsmiDeleteTabTab);
        Add("RenameTab", Shortcut.F2, DoRenameTab, tsbRename, tsmiRenameTabTab);
        Add("FindReplace", Shortcut.None, () => DoFindReplace(), tsbFindReplace);
        Add("Reformat", Shortcut.None, DoReformat, tsbReformat);
    }

    private void tsbStorno_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

    private void tsmiCut_Click(object sender, EventArgs e) => _sc.Cut();

    private void tsmiCopy_Click(object sender, EventArgs e) => _sc.Copy();

    private void tsmiPaste_Click(object sender, EventArgs e) => _sc.Paste();

    private void tsmiDelete_Click(object sender, EventArgs e) => _sc.ReplaceSelection("");

    private void tsmiSelectAll_Click(object sender, EventArgs e) => _sc.SelectAll();

    private void scText_StyleNeeded(object sender, StyleNeededEventArgs e)
    {
        var startPos = _sc.GetEndStyled();
        var endPos = e.Position;

        _cSharpLexer.Style(_sc, startPos, endPos);
    }

    private void scText_TextChanged(object sender, EventArgs e)
    {
        tsbUndo.Enabled = _sc.CanUndo;
        tsbRedo.Enabled = _sc.CanRedo;

        if (lbTabTabs.SelectedIndex != -1 && !_documents[lbTabTabs.SelectedIndex].Unsaved)
        {
            _documents[lbTabTabs.SelectedIndex].Unsaved = true;
            tsbSave.Enabled = true;
            _documents.ResetBindings();
        }

        ShowNumberLines();

        _validateTimer.Stop();
        _validateTimer.Start();
    }

    private void ShowNumberLines()
    {
        var maxLength = _sc.Lines.Count.ToString(CultureInfo.CurrentCulture).Length;
        if (maxLength == _maxLineNumberCharLength)
            return;

        const int padding = 2;
        _sc.Margins[0].Width = _sc.TextWidth(Style.LineNumber, new string('9', maxLength + 1)) + padding;
        _maxLineNumberCharLength = maxLength;
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
        var caretPos = _sc.CurrentPosition;
        var docStart = caretPos == 1;
        var docEnd = caretPos == _sc.Text.Length;

        var charPrev = docStart ? _sc.GetCharAt(caretPos) : _sc.GetCharAt(caretPos - 2);
        var charNext = _sc.GetCharAt(caretPos);

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
                _sc.InsertText(caretPos, ")");
                break;
            case '{':
                if (charNextIsCharOrString) return;
                _sc.InsertText(caretPos, "}");
                break;
            case '[':
                if (charNextIsCharOrString) return;
                _sc.InsertText(caretPos, "]");
                break;
            case '"':
                // 0x22 = "
                if (charPrev == 0x22 && charNext == 0x22)
                {
                    _sc.DeleteRange(caretPos, 1);
                    _sc.GotoPosition(caretPos);
                    return;
                }

                if (isCharOrString)
                    _sc.InsertText(caretPos, "\"");
                break;
            case '\'':
                // 0x27 = '
                if (charPrev == 0x27 && charNext == 0x27)
                {
                    _sc.DeleteRange(caretPos, 1);
                    _sc.GotoPosition(caretPos);
                    return;
                }

                if (isCharOrString)
                    _sc.InsertText(caretPos, "'");
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
        var caretPos = _sc.CurrentPosition;
        if (_lastCaretPos != caretPos)
        {
            _lastCaretPos = caretPos;
            var bracePos1 = -1;

            // Is there a brace to the left or right?
            if (caretPos > 0 && IsBrace(_sc.GetCharAt(caretPos - 1)))
                bracePos1 = caretPos - 1;
            else if (IsBrace(_sc.GetCharAt(caretPos)))
                bracePos1 = caretPos;

            if (bracePos1 >= 0)
            {
                // Find the matching brace
                var bracePos2 = _sc.BraceMatch(bracePos1);
                if (bracePos2 == Scintilla.InvalidPosition)
                {
                    _sc.BraceBadLight(bracePos1);
                    _sc.HighlightGuide = 0;
                }
                else
                {
                    _sc.BraceHighlight(bracePos1, bracePos2);
                    _sc.HighlightGuide = _sc.GetColumn(bracePos1);
                }
            }
            else
            {
                // Turn off brace matching
                _sc.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                _sc.HighlightGuide = 0;
            }
        }

        if ((e.Change & UpdateChange.Selection) > 0)
        {
            var currentPos = _sc.CurrentPosition;
            var anchorPos = _sc.AnchorPosition;
            if (anchorPos - currentPos == 0)
            {
                tsslPosText.Text = @"Pos:";
                tsslPos.Text = currentPos.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                tsslPosText.Text = @"Sel:";
                tsslPos.Text = Math.Abs(anchorPos - currentPos).ToString(CultureInfo.CurrentCulture);
            }

            tsslRow.Text = (_sc.LineFromPosition(currentPos) + 1).ToString(CultureInfo.CurrentCulture);
            tsslCol.Text = (_sc.GetColumn(currentPos) + 1).ToString(CultureInfo.CurrentCulture);
        }
    }

    private void SwitchDocument(Document nextDocument)
    {
        var prevDocument = _sc.Document;
        _sc.AddRefDocument(prevDocument);

        _sc.Document = nextDocument;
        _sc.ReleaseDocument(nextDocument);

        scText.SwitchedDocument();
    }

    private Document CreateDocument(string text)
    {
        var l = _sc.CreateLoader(256);
        var chars = text.ToCharArray();
        if (chars.Length != 0) l.AddData(chars, text.Length);
        return l.ConvertToDocument();
    }

    private void lbTabTabs_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lbTabTabs.SelectedIndex != -1)
        {
            SwitchDocument(_documents[lbTabTabs.SelectedIndex].Document);

            tsbSave.Enabled = _documents[lbTabTabs.SelectedIndex].Unsaved;
            tsbUndo.Enabled = _sc.CanUndo;
            tsbRedo.Enabled = _sc.CanRedo;
            tsslTabTabName.Text = _documents[lbTabTabs.SelectedIndex].Key;
            tsslLen.Text = _sc.Text.Length.ToString(CultureInfo.CurrentCulture);
            tsslLines.Text = _sc.Lines.Count.ToString(CultureInfo.CurrentCulture);

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
            tsmiUndo.Enabled = _sc.CanUndo;
            tsmiRedo.Enabled = _sc.CanRedo;
            tsmiCut.Enabled = _sc.SelectedText.Length > 0;
            tsmiCopy.Enabled = _sc.SelectedText.Length > 0;
            tsmiPaste.Enabled = _sc.CanPaste;
            tsmiDelete.Enabled = _sc.SelectedText.Length > 0;
            tsmiSelectAll.Enabled = _sc.Text.Length > 0 && _sc.Text.Length != _sc.SelectedText.Length;
        }
    }

    internal class TabTabDoc
    {
        /// <summary>Sekcia v grafikone (pri novej sekcii objekt, ktory sa tam prida pri ulozeni).</summary>
        public TableTabTab TabTab { get; init; } = null!;

        /// <summary>Nazov sekcie v editore; do <see cref="TableTabTab.Key"/> sa zapise pri ulozeni.</summary>
        public string Key { get; set; } = "";

        public Document Document { get; set; }

        /// <summary>Text sekcie nie je ulozeny.</summary>
        public bool Unsaved { get; set; }

        /// <summary>Sekcia je nova alebo premenovana a zoznam sekcii este nebol ulozeny.</summary>
        public bool KeyUnsaved { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Unsaved || KeyUnsaved ? "* " + Key : Key;
        }
    }
}