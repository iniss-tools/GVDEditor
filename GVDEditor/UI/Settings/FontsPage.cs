using System.Globalization;
using ExControls;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.UI.Controls;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranka Pisma v okne Lokalne nastavenia - zoznam pisiem tabul (ModeTabs.txt [FONT]) s upravou priamo
/// vo vybranom pisme. Vzhlad sa vybera rezom, farbou a efektmi, cislo sklada <see cref="TableFontPicker" />.
/// Stlpce katalogovych tabul a texty vlakov, ktore pismo pouzivaju, sa pri zmene vzhladu precisluju s nim.
/// </summary>
public partial class FontsPage : UserControl, ISettingsPage
{
    /// <summary>
    /// Kontext editora - nastavi ho <c>LoadData</c>.
    /// </summary>
    private EditorContext _ctx = null!;

    /// <summary>
    /// Stav pisma pocas upravy: ktore udaje sa este riadia rezom a kde sa pismo pouziva.
    /// </summary>
    private sealed class FontState
    {
        public bool AutoName;
        public bool AutoType;
        public bool AutoWidth;
        public bool AutoProp;
        public List<TableItem> Columns = [];
        public List<TableTrain> Trains = [];
        public IReadOnlyList<string> TabTabSections = [];
        public int TabTabId;
    }

    private readonly Dictionary<TableFont, FontState> _states = new(ReferenceEqualityComparer.Instance);
    private readonly List<(TableFont Font, string Text)> _problems = [];
    private TableFont? _current;
    private bool _loading;

    // zmena zoznamu pisiem touto strankou - nie je treba nanovo citat zoznam
    private bool _selfChange;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public FontsPage()
    {
        InitializeComponent();
    }

    /// <inheritdoc />
    public event EventHandler? ProblemsChanged;

    /// <inheritdoc />
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? FirstProblem => _problems.Count == 0 ? null : ProblemText(_problems[0]);

    /// <summary>
    /// Priecinok s pismami (subory .fnt).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string FontDir { get; private set; } = "";

    /// <inheritdoc />
    public void FocusFirstProblem()
    {
        if (_problems.Count == 0)
            return;

        var (font, text) = _problems[0];
        SelectFont(font);
        if (text == FontRules.CheckName(font.Name))
            tbName.Focus();
        else
            picker.Focus();
    }

    /// <summary>
    /// Naplni stranku pismami - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name=\"context\">kontext editora</param>
    /// <param name="fontDir">priecinok s pismami</param>
    internal void LoadData(EditorContext context, string fontDir)
    {
        _ctx = context;
        FontDir = fontDir;
        tbDir.Text = fontDir;

        foreach (var header in new[] { lLook, lData, lUseHeader })
            header.Font = new Font(Font, FontStyle.Bold);
        foreach (var auto in new[] { lTypeAuto, lWidthAuto, lPropAuto, lDataNote })
            auto.ForeColor = SystemColors.GrayText;
        listFonts.ItemHeight = Font.Height + 6;
        // tema nastavuje tmave posuvniky len niektorym prvkom - panel s posuvanim ich ma inak svetle
        if (_ctx.UsingStyle.DarkScrollBar)
            pDetail.SetTheme(WindowsTheme.DarkExplorer);

        cbType.Items.Clear();
        cbType.Items.AddRange(TableFontType.GetValues().ToArray<object>());

        foreach (var font in _ctx.Document.TableFonts)
            _states[font] = NewState(font, false);
        RecountUsage();

        _ctx.Document.TableFonts.ListChanged += TableFonts_ListChanged;
        Disposed += (_, _) => _ctx.Document.TableFonts.ListChanged -= TableFonts_ListChanged;

        FillList(_ctx.Document.TableFonts.FirstOrDefault());
    }

    private static FontState NewState(TableFont font, bool autoName)
    {
        var code = new ElenFontCode(font.FontID);
        return new FontState
        {
            AutoName = autoName,
            // udaj sa riadi rezom, kym zodpoveda odporucaniu - napr. pismo LCD s inou sirkou sa nemeni
            AutoType = font.Type == code.SuggestedType,
            AutoWidth = font.Width == code.SuggestedWidth,
            AutoProp = font.IsProportional == code.SuggestedProportional,
            TabTabId = font.FontID
        };
    }

    private FontState State(TableFont font)
    {
        if (!_states.TryGetValue(font, out var state))
            _states[font] = state = NewState(font, false);
        return state;
    }

    /// <summary>
    /// Zisti, ktore stlpce a texty pouzivaju ktore pismo. Pismu s cislom, ktore ma aj ine pismo, sa pouzitie
    /// nemeni - nedalo by sa rozlisit, ktoremu z nich patri.
    /// </summary>
    private void RecountUsage()
    {
        var fonts = _ctx.Document.TableFonts.ToList();
        foreach (var font in fonts)
        {
            if (fonts.Count(f => f.FontID == font.FontID) > 1)
                continue;

            var state = State(font);
            state.Columns = _ctx.Document.TableCatalogs.SelectMany(c => c.Items).Where(item => item.FontIdx == font.FontID).ToList();
            state.Trains = _ctx.Document.TableTexts.SelectMany(t => t.Trains).Where(train => train.FontID == font.FontID).ToList();
            state.TabTabSections = TableFontUsage.Find(font.FontID, [], [], _ctx.Document.TabTabs).TabTabSections;
            state.TabTabId = font.FontID;
        }
    }

    /// <inheritdoc />
    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        // katalogove tabule a texty sa mohli medzitym zmenit na inej stranke okna
        if (Visible && _current is not null)
        {
            RecountUsage();
            ShowUsage();
        }
    }

    private void TableFonts_ListChanged(object? sender, ListChangedEventArgs e)
    {
        // pismo pridane inde (napr. z vyberu pisma v katalogovej tabuli)
        if (_selfChange || _loading)
            return;

        FillList(_current is not null && _ctx.Document.TableFonts.Contains(_current) ? _current : _ctx.Document.TableFonts.FirstOrDefault());
    }

    private void FillList(TableFont? select)
    {
        _loading = true;
        listFonts.BeginUpdate();
        listFonts.Items.Clear();
        listFonts.Items.AddRange(_ctx.Document.TableFonts.ToArray<object>());
        listFonts.EndUpdate();
        _loading = false;

        SelectFont(select);
        Check();
    }

    private void SelectFont(TableFont? font)
    {
        if (font is null)
        {
            _current = null;
            ShowCurrent();
            return;
        }

        if (!ReferenceEquals(listFonts.SelectedItem, font))
            listFonts.SelectedItem = font;
        else
            ShowCurrent();
    }

    private void listFonts_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_loading)
            return;

        _current = listFonts.SelectedItem as TableFont;
        ShowCurrent();
    }

    /// <summary>
    /// Zobrazi udaje vybraneho pisma.
    /// </summary>
    private void ShowCurrent()
    {
        var font = _current;
        pDetail.Enabled = font is not null;
        bDuplicate.Enabled = bDelete.Enabled = font is not null;

        _loading = true;
        try
        {
            tbName.Text = font?.Name ?? "";
            picker.Value = font?.FontID ?? FontRules.SuggestId([]);
            tbFile.Text = font?.FileName ?? "";
            ShowData();
        }
        finally
        {
            _loading = false;
        }

        ShowUsage();
        ShowProblem();
    }

    /// <summary>
    /// Zobrazi udaje pre obsluhu (typ, rozmery, znaky) a ktore sa riadia rezom.
    /// </summary>
    private void ShowData()
    {
        var font = _current;
        var loading = _loading;
        _loading = true;
        try
        {
            cbType.SelectedItem = font?.Type;
            nudSize.Value = Math.Clamp(font?.Size ?? 7, nudSize.Minimum, nudSize.Maximum);
            nudWidth.Value = Math.Clamp(font?.Width ?? 6, nudWidth.Minimum, nudWidth.Maximum);
            cbProp.Checked = font?.IsProportional ?? false;
            cbDia.Checked = font?.IsDia ?? false;
            cbLower.Checked = font?.IsLower ?? false;
            cbUpper.Checked = font?.IsUpper ?? false;
            cbNum.Checked = font?.IsNumber ?? false;
            cbSpec.Checked = font?.IsSpecChars ?? false;
            cbSpecAssign.Checked = font?.IsSpecAssigment ?? false;

            var state = font is null ? null : State(font);
            lTypeAuto.Visible = state?.AutoType ?? false;
            lWidthAuto.Visible = state?.AutoWidth ?? false;
            lPropAuto.Visible = state?.AutoProp ?? false;
        }
        finally
        {
            _loading = loading;
        }
    }

    private void ShowUsage()
    {
        if (_current is not { } font)
        {
            lUse.Text = "";
            return;
        }

        var state = State(font);
        var lines = new List<string>();
        if (state.Columns.Count > 0)
            lines.Add("– " + string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_Pismo_Stlpce, state.Columns.Count));
        if (state.Trains.Count > 0)
            lines.Add("– " + string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_Pismo_Texty, state.Trains.Count));
        if (state.TabTabSections.Count > 0)
            lines.Add("– " + string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_Pismo_TabTab,
                string.Join(", ", state.TabTabSections), state.TabTabId));

        if (lines.Count == 0)
            lUse.Text = Resources.FontsPage_Nepouziva;
        else
        {
            if (state.Columns.Count + state.Trains.Count > 0)
                lines.Add(Resources.FontsPage_Precisluju);
            lUse.Text = string.Join(Environment.NewLine, lines);
        }
    }

    private void ShowProblem()
    {
        var problems = _problems.Where(p => ReferenceEquals(p.Font, _current)).Select(p => p.Text).ToList();
        lProblem.Text = string.Join(" ", problems);
        lProblem.Visible = problems.Count > 0;
        lProblem.ForeColor = SettingsWindow.ProblemColor(lProblem);
    }

    private void Check()
    {
        _problems.Clear();
        var fonts = _ctx.Document.TableFonts.ToList();
        for (var i = 0; i < fonts.Count; i++)
        {
            if (FontRules.CheckName(fonts[i].Name) is { } name)
                _problems.Add((fonts[i], name));
            if (FontRules.CheckId(fonts, i) is { } id)
                _problems.Add((fonts[i], id));
        }

        listFonts.Invalidate();
        ShowProblem();
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private static string ProblemText((TableFont Font, string Text) problem) =>
        string.IsNullOrWhiteSpace(problem.Font.Name) ? problem.Text : $"{problem.Font.Name} – {problem.Text}";

    /// <summary>
    /// Po zmene pisma obnovi jeho riadok v zozname a oznami zmenu zoznamu (napr. vyberu pisma v inych oknach).
    /// </summary>
    private void Changed(TableFont font)
    {
        _selfChange = true;
        try
        {
            var index = _ctx.Document.TableFonts.IndexOf(font);
            if (index >= 0)
                _ctx.Document.TableFonts.ResetItem(index);
        }
        finally
        {
            _selfChange = false;
        }

        Check();
    }

    private void picker_ValueChanged(object? sender, EventArgs e)
    {
        if (_loading || _current is not { } font)
            return;

        var state = State(font);
        var id = picker.Value;

        // stlpce a texty, ktore pismo pouzivaju, idu s nim (TabTab sa nemeni - {n} moze byt aj kod znaku)
        foreach (var column in state.Columns)
            column.FontIdx = id;
        foreach (var train in state.Trains)
            train.FontID = id;
        font.FontID = id;

        var code = new ElenFontCode(id);
        if (state.AutoType)
            font.Type = code.SuggestedType;
        if (state.AutoWidth)
            font.Width = code.SuggestedWidth;
        if (state.AutoProp)
            font.IsProportional = code.SuggestedProportional;
        if (state.AutoName)
        {
            font.Name = code.SuggestedName();
            _loading = true;
            tbName.Text = font.Name;
            _loading = false;
        }

        ShowData();
        Changed(font);
    }

    private void tbName_TextChanged(object? sender, EventArgs e)
    {
        if (_loading || _current is not { } font)
            return;

        font.Name = tbName.Text;
        State(font).AutoName = false;
        Changed(font);
    }

    private void cbType_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        if (_current is not { } font)
            return;

        font.Type = cbType.SelectedItem as TableFontType;
        State(font).AutoType = false;
        ShowData();
    }

    private void nudSize_ValueChanged(object? sender, EventArgs e)
    {
        if (!_loading && _current is { } font)
            font.Size = decimal.ToInt32(nudSize.Value);
    }

    private void nudWidth_ValueChanged(object? sender, EventArgs e)
    {
        if (_loading || _current is not { } font)
            return;

        font.Width = decimal.ToInt32(nudWidth.Value);
        State(font).AutoWidth = false;
        ShowData();
    }

    private void cbProp_CheckedChanged(object? sender, EventArgs e)
    {
        if (_loading || _current is not { } font)
            return;

        font.IsProportional = cbProp.Checked;
        State(font).AutoProp = false;
        ShowData();
    }

    private void Flag_CheckedChanged(object? sender, EventArgs e)
    {
        if (_loading || _current is not { } font)
            return;

        font.IsDia = cbDia.Checked;
        font.IsLower = cbLower.Checked;
        font.IsUpper = cbUpper.Checked;
        font.IsNumber = cbNum.Checked;
        font.IsSpecChars = cbSpec.Checked;
        font.IsSpecAssigment = cbSpecAssign.Checked;
    }

    private void tbFile_TextChanged(object? sender, EventArgs e)
    {
        if (!_loading && _current is { } font)
            font.FileName = tbFile.Text.Trim();
    }

    private void bAdd_Click(object? sender, EventArgs e)
    {
        var code = new ElenFontCode(FontRules.SuggestId(_ctx.Document.TableFonts.Select(f => f.FontID)));
        var font = new TableFont
        {
            Name = code.SuggestedName(),
            FontID = code.Id,
            Type = code.SuggestedType,
            Width = code.SuggestedWidth,
            IsProportional = code.SuggestedProportional,
            Size = 7,
            FileName = "",
            IsDia = true,
            IsLower = true,
            IsUpper = true,
            IsNumber = true
        };
        Add(font, NewState(font, true));
        picker.Focus();
    }

    private void bDuplicate_Click(object? sender, EventArgs e)
    {
        if (_current is not { } source)
            return;

        var font = new TableFont
        {
            Name = string.Format(CultureInfo.CurrentCulture, Resources.FontsPage_Kopia, source.Name),
            FontID = source.FontID,
            Type = source.Type,
            Size = source.Size,
            Width = source.Width,
            FileName = source.FileName,
            IsDia = source.IsDia,
            IsProportional = source.IsProportional,
            IsLower = source.IsLower,
            IsUpper = source.IsUpper,
            IsNumber = source.IsNumber,
            IsSpecChars = source.IsSpecChars,
            IsSpecAssigment = source.IsSpecAssigment
        };
        var state = State(source);
        // kopia nema ziadne pouzitie; kym ma rovnake cislo, hlasi sa chyba - pouzivatel zmeni vzhlad
        Add(font, new FontState
        {
            AutoType = state.AutoType, AutoWidth = state.AutoWidth, AutoProp = state.AutoProp, TabTabId = font.FontID
        });
    }

    private void Add(TableFont font, FontState state)
    {
        _states[font] = state;
        _selfChange = true;
        try
        {
            _ctx.Document.TableFonts.Insert(_current is null ? _ctx.Document.TableFonts.Count : _ctx.Document.TableFonts.IndexOf(_current) + 1, font);
        }
        finally
        {
            _selfChange = false;
        }

        FillList(font);
    }

    private void bDelete_Click(object? sender, EventArgs e)
    {
        if (_current is not { } font)
            return;

        var state = State(font);
        if (state.Columns.Count + state.Trains.Count + state.TabTabSections.Count > 0)
        {
            var usage = lUse.Text.Replace(Environment.NewLine + Resources.FontsPage_Precisluju, "", StringComparison.Ordinal);
            if (Utils.ShowQuestion(string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_Pismo_Odstranit,
                    font.FontID, usage)) != DialogResult.Yes)
                return;
        }

        var index = _ctx.Document.TableFonts.IndexOf(font);
        _states.Remove(font);
        _selfChange = true;
        try
        {
            _ctx.Document.TableFonts.Remove(font);
        }
        finally
        {
            _selfChange = false;
        }

        FillList(_ctx.Document.TableFonts.Count == 0 ? null : _ctx.Document.TableFonts[Math.Min(index, _ctx.Document.TableFonts.Count - 1)]);
    }

    private void listFonts_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Insert:
                bAdd_Click(this, EventArgs.Empty);
                e.Handled = true;
                break;
            case Keys.Delete:
                bDelete_Click(this, EventArgs.Empty);
                e.Handled = true;
                break;
        }
    }

    private void bDir_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog();
        if (Directory.Exists(FontDir))
            dialog.SelectedPath = FontDir;
        if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
            return;

        FontDir = dialog.SelectedPath;
        tbDir.Text = FontDir;
    }

    /// <summary>
    /// Riadok zoznamu: farba pisma, nazov, cislo a vykricnik pri chybe.
    /// </summary>
    private void listFonts_DrawItem(object? sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        if (e.Index < 0 || listFonts.Items[e.Index] is not TableFont font)
            return;

        var g = e.Graphics;
        var bounds = e.Bounds;
        var dot = Math.Max(6, bounds.Height - 12);
        var code = new ElenFontCode(font.FontID);
        using (var brush = new SolidBrush(LedPreview.LitColor(code.Color)))
        using (var pen = new Pen(Color.FromArgb(120, 0, 0, 0)))
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var r = new Rectangle(bounds.Left + 6, bounds.Top + (bounds.Height - dot) / 2, dot, dot);
            g.FillEllipse(brush, r);
            g.DrawEllipse(pen, r);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
        }

        var id = font.FontID.ToString(CultureInfo.InvariantCulture);
        var idSize = TextRenderer.MeasureText(id, e.Font);
        var textLeft = bounds.Left + 12 + dot;
        var hasProblem = _problems.Any(p => ReferenceEquals(p.Font, font));
        var name = (string.IsNullOrWhiteSpace(font.Name) ? Resources.FontsPage_Bez_nazvu : font.Name) + (hasProblem ? "  !" : "");
        var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;
        TextRenderer.DrawText(g, name, e.Font, new Rectangle(textLeft, bounds.Top, bounds.Right - textLeft - idSize.Width - 8,
            bounds.Height), hasProblem && (e.State & DrawItemState.Selected) == 0 ? SettingsWindow.ProblemColor(listFonts) : e.ForeColor, flags);
        TextRenderer.DrawText(g, id, e.Font, new Rectangle(bounds.Right - idSize.Width - 6, bounds.Top, idSize.Width + 4, bounds.Height),
            e.ForeColor, flags);
        e.DrawFocusRectangle();
    }
}
