using ExControls;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ToolsCore;
using ToolsCore.Expressions;
using ToolsCore.StateDgm;
using ToolsCore.Tools;

namespace GVDEditor.Controls;

/// <summary>
///     Spolocny kontext editorov stavoveho diagramu - symboly pre kontrolu vyrazov a typy hlaseni z Categori.txt.
/// </summary>
internal static class SdEditorContext
{
    /// <summary>Symboly grafikonu pre validator vyrazov (druhy vlakov, stanice…).</summary>
    public static IExprSymbolProvider? Symbols { get; set; }

    /// <summary>Kluce typov hlaseni z lokalneho Categori.txt.</summary>
    public static IReadOnlyList<string> ReportKeys { get; set; } = [];

    /// <summary>Polozka comboboxu s hodnotou.</summary>
    public sealed record Item(string Text, object? Value)
    {
        /// <inheritdoc />
        public override string ToString() => Text;
    }

    /// <summary>Vyberie polozku podla hodnoty (alebo prvu, ak sa nenajde).</summary>
    public static void Select(ComboBox cb, object? value)
    {
        for (var i = 0; i < cb.Items.Count; i++)
            if (cb.Items[i] is Item it && Equals(it.Value, value))
            {
                cb.SelectedIndex = i;
                return;
            }

        cb.SelectedIndex = cb.Items.Count > 0 ? 0 : -1;
    }

    /// <summary>Hodnota vybranej polozky.</summary>
    public static object? Value(ComboBox cb) => (cb.SelectedItem as Item)?.Value;

    /// <summary>Skontroluje vyraz; vrati text chyby/varovania alebo null.</summary>
    public static (ExprSeverity Severity, string Message)? Check(string text, ExprContext context, bool isCondition)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        var r = ExprValidator.Validate(text, new ExprValidationOptions { Context = context, Symbols = Symbols, IsCondition = isCondition, ReportContextDependent = false });
        var d = r.Diagnostics.OrderBy(x => x.Severity == ExprSeverity.Error ? 0 : x.Severity == ExprSeverity.Warning ? 1 : 2).FirstOrDefault();
        if (d == null) return null;
        var msg = d.Message + (d.Suggestion != null ? " – " + d.Suggestion : "");
        return (d.Severity, msg);
    }

    /// <summary>Citatelny posun v sekundach (<c>-20 min</c>, <c>+90 s</c>).</summary>
    public static string Seconds(int s)
    {
        var abs = Math.Abs(s);
        return abs % 60 == 0 ? string.Format(Resources.FStateDgm_Min, abs / 60) : string.Format(Resources.FStateDgm_Sek, abs);
    }
}

/// <summary>
///     Zaklad editora vlastnosti: tabulka popis | ovladaci prvok, udalost <see cref="Changed" /> pri kazdej zmene.
/// </summary>
internal abstract class SdEditorBase : UserControl
{
    private readonly ToolTip _tips = new();

    /// <summary>Tabulka s riadkami editora.</summary>
    protected readonly TableLayoutPanel Table;

    /// <summary>Prebieha plnenie z modelu - zmeny sa nehlasia.</summary>
    protected bool Loading;

    protected SdEditorBase()
    {
        AutoScroll = true;
        Table = new TableLayoutPanel
        {
            ColumnCount = 2,
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Padding = new Padding(6)
        };
        Table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        Controls.Add(Table);
    }

    /// <summary>Nastala zmena v modeli.</summary>
    public event EventHandler? Changed;

    /// <summary>Ohlasi zmenu (mimo plnenia).</summary>
    protected void RaiseChanged()
    {
        if (!Loading) Changed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    ///     Koliesko mysi nad comboboxom / ciselnym polom posuva cely editor, nie hodnotu pola (hodnota sa meni len
    ///     klavesnicou alebo klikom) - inak by sa pri rolovani panela nechtiac prepisovali hodnoty.
    /// </summary>
    private void HookWheel(Control c)
    {
        if (c is ComboBox or NumericUpDown)
            c.MouseWheel += (_, e) =>
            {
                if (c is ComboBox { DroppedDown: true }) return;
                if (e is HandledMouseEventArgs h) h.Handled = true;
                ScrollBy(-e.Delta);
            };
        foreach (Control child in c.Controls) HookWheel(child);
        c.ControlAdded += (_, e) => HookWheel(e.Control);
    }

    /// <summary>Posunie obsah editora o dany pocet bodov.</summary>
    private void ScrollBy(int delta)
    {
        if (!VerticalScroll.Visible) return;
        var y = Math.Clamp(-AutoScrollPosition.Y + delta, VerticalScroll.Minimum, Math.Max(VerticalScroll.Minimum, VerticalScroll.Maximum - VerticalScroll.LargeChange + 1));
        AutoScrollPosition = new Point(-AutoScrollPosition.X, y);
    }

    /// <summary>Prida riadok popis + prvok; vrati popis.</summary>
    protected Label AddRow(string label, Control control, string? tip = null)
    {
        var l = new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 6, 6, 3) };
        var row = Table.RowCount++;
        Table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Table.Controls.Add(l, 0, row);
        control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        HookWheel(control);
        Table.Controls.Add(control, 1, row);
        if (tip != null)
        {
            _tips.SetToolTip(control, tip);
            _tips.SetToolTip(l, tip);
        }

        return l;
    }

    /// <summary>Prida prvok cez obe stlpce.</summary>
    protected void AddFull(Control control, int topMargin = 3)
    {
        var row = Table.RowCount++;
        Table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        control.Margin = new Padding(3, topMargin, 3, 3);
        control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        HookWheel(control);
        Table.Controls.Add(control, 0, row);
        Table.SetColumnSpan(control, 2);
    }

    /// <summary>Prida nadpis casti.</summary>
    protected Label AddHeader(string text)
    {
        var l = new Label { Text = text, AutoSize = true, Font = new Font(Font, FontStyle.Bold) };
        AddFull(l, 12);
        return l;
    }

    /// <summary>Prida informacny text (zalamovany).</summary>
    protected Label AddInfo(string text)
    {
        var l = new Label { Text = text, AutoSize = true, MaximumSize = new Size(360, 0), ForeColor = SystemColors.GrayText };
        AddFull(l);
        Table.SizeChanged += (_, _) => l.MaximumSize = new Size(Math.Max(120, Table.ClientSize.Width - 20), 0);
        return l;
    }

    /// <summary>Vytvori combo so zoznamom poloziek.</summary>
    protected static ExComboBox Combo(params SdEditorContext.Item[] items)
    {
        var cb = new ExComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220 };
        cb.Items.AddRange(items.Cast<object>().ToArray());
        return cb;
    }

    /// <summary>Vytvori textove pole.</summary>
    protected static ExTextBox Text(string? hint = null) => new() { Width = 220, HintText = hint };

    /// <summary>Vytvori ciselne pole.</summary>
    protected static ExNumericUpDown Number(int min, int max, int step = 1) => new()
    {
        Minimum = min, Maximum = max, Increment = step, Width = 90, Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Right
    };

    /// <summary>Vytvori zaskrtavacie pole.</summary>
    protected static ExCheckBox Check(string text) => new() { Text = text, AutoSize = true, Anchor = AnchorStyles.Left };

    /// <summary>Zoznam prvkov riadka - zobrazi/skryje aj popis.</summary>
    protected void SetRowVisible(Control control, bool visible)
    {
        control.Visible = visible;
        var pos = Table.GetPositionFromControl(control);
        if (pos.Column == 1 && Table.GetControlFromPosition(0, pos.Row) is { } l) l.Visible = visible;
    }
}

/// <summary>
///     Hodnota, ktoru INISS cita ako cislo aj ako vyraz: vyber zo zoznamu / cislo, alebo po prepnuti [ƒ] volny vyraz.
/// </summary>
internal sealed class SdDynamicField : UserControl
{
    private readonly ExComboBox? _combo;
    private readonly ExNumericUpDown? _number;
    private readonly ExTextBox _expr;
    private readonly ExButton _fx;
    private bool _isExpr;
    private readonly ErrorProvider _errors = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
    private readonly ExprContext _context;
    private readonly bool _nullWhenZero;
    private bool _loading;

    private SdDynamicField(ExComboBox? combo, ExNumericUpDown? number, ExprContext context, bool nullWhenZero)
    {
        _combo = combo;
        _number = number;
        _context = context;
        _nullWhenZero = nullWhenZero;
        Margin = new Padding(0);

        // [ zoznam / cislo / vyraz (jeden z nich viditelny) ][ ƒ ]
        var basic = (Control?)combo ?? number!;
        _expr = new ExTextBox { Visible = false, HintText = Resources.FStateDgm_VyrazTip, Font = GlobData.UsingStyle.TabTabEditorScheme.Font };
        _fx = new ExButton { Text = Resources.FStateDgm_Vyraz, Width = 28, Margin = new Padding(3), Font = new Font(Font.FontFamily, Font.Size, FontStyle.Italic | FontStyle.Bold) };
        new ToolTip().SetToolTip(_fx, Resources.FStateDgm_VyrazTip);

        var flow = new TableLayoutPanel { ColumnCount = 2, RowCount = 1, Dock = DockStyle.Fill, Margin = new Padding(0) };
        flow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        flow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        flow.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var host = new Panel { Dock = DockStyle.Fill, Margin = new Padding(3) };
        basic.Dock = DockStyle.Top;
        _expr.Dock = DockStyle.Top;
        host.Controls.Add(_expr);
        host.Controls.Add(basic);
        _fx.Anchor = AnchorStyles.Left;
        flow.Controls.Add(host, 0, 0);
        flow.Controls.Add(_fx, 1, 0);
        Controls.Add(flow);
        basic.SizeChanged += (_, _) => UpdateHeight();
        _expr.SizeChanged += (_, _) => UpdateHeight();
        UpdateHeight();

        _fx.Click += (_, _) => SetExpressionMode(!_isExpr);
        SetExpressionMode(false);
        if (_combo != null) _combo.SelectedIndexChanged += (_, _) => Fire();
        if (_number != null) _number.ValueChanged += (_, _) => Fire();
        _expr.TextChanged += (_, _) =>
        {
            Validate();
            Fire();
        };
        _errors.SetIconAlignment(_expr, ErrorIconAlignment.MiddleLeft);
    }

    /// <summary>Hodnota sa zmenila.</summary>
    public event EventHandler? ValueChanged;

    private void UpdateHeight()
    {
        var h = Math.Max(((Control?)_combo ?? _number!).Height, _expr.Height);
        _fx.Height = h;
        Height = h + 8;
    }

    /// <summary>Prepne medzi vyberom/cislom a volnym vyrazom; tlacidlo [ƒ] je v rezime vyrazu zvyraznene.</summary>
    private void SetExpressionMode(bool expr)
    {
        var changed = _isExpr != expr;
        _isExpr = expr;
        var basic = (Control?)_combo ?? _number!;
        basic.Visible = !expr;
        _expr.Visible = expr;
        var scheme = GlobSettings.UsingStyle.ControlsColorScheme;
        _fx.BackColor = expr ? scheme.Highlight.BackColor : scheme.Button.BackColor;
        _fx.ForeColor = expr ? scheme.Highlight.ForeColor : scheme.Button.ForeColor;
        if (!changed || _loading) return;

        if (expr) _expr.Text = BasicText();
        else if (_combo != null) SdEditorContext.Select(_combo, StateDgmReader.TryStrtol(_expr.Text.Trim(), out var n) ? n : null);
        else if (_number != null && StateDgmReader.TryStrtol(_expr.Text.Trim(), out var m)) _number.Value = Math.Clamp(m, _number.Minimum, _number.Maximum);
        Validate();
        ValueChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Pole s vyberom z ciselnych hodnot (Value = null znamena kluc nenastaveny).</summary>
    public static SdDynamicField Choice(ExprContext context, params SdEditorContext.Item[] items)
    {
        var cb = new ExComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        cb.Items.AddRange(items.Cast<object>().ToArray());
        cb.SelectedIndex = 0;
        return new SdDynamicField(cb, null, context, false);
    }

    /// <summary>Ciselne pole; <paramref name="nullWhenZero" /> - nula sa do suboru nezapisuje.</summary>
    public static SdDynamicField Number(int min, int max, int step, bool nullWhenZero)
    {
        var n = new ExNumericUpDown { Minimum = min, Maximum = max, Increment = step, TextAlign = HorizontalAlignment.Right };
        return new SdDynamicField(null, n, ExprContext.Condition, nullWhenZero);
    }

    /// <summary>Aktualna hodnota (null = kluc sa nezapise).</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public StateDgmDynamic? Value
    {
        get
        {
            if (_isExpr)
                return string.IsNullOrWhiteSpace(_expr.Text) ? null : StateDgmDynamic.FromExpression(_expr.Text.Trim());
            if (_combo != null)
                return SdEditorContext.Value(_combo) is int n ? StateDgmDynamic.FromNumber(n) : null;
            var v = (int)_number!.Value;
            return v == 0 && _nullWhenZero ? null : StateDgmDynamic.FromNumber(v);
        }
        set
        {
            _loading = true;
            try
            {
                if (value is { IsExpression: true })
                {
                    SetExpressionMode(true);
                    _expr.Text = value.Expression;
                }
                else
                {
                    SetExpressionMode(false);
                    _expr.Text = "";
                    if (_combo != null) SdEditorContext.Select(_combo, value?.Number);
                    else _number!.Value = Math.Clamp(value?.Number ?? 0, _number.Minimum, _number.Maximum);
                }

                Validate();
            }
            finally
            {
                _loading = false;
            }
        }
    }

    private string BasicText()
    {
        if (_combo != null) return SdEditorContext.Value(_combo) is int n ? n.ToString() : "";
        return ((int)_number!.Value).ToString();
    }

    private void Fire()
    {
        if (!_loading) ValueChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Validate()
    {
        var r = _isExpr ? SdEditorContext.Check(_expr.Text, _context, false) : null;
        _errors.SetError(_expr, r?.Message ?? "");
    }
}

/// <summary>
///     Editor hlavicky diagramu - popis suboru a vyraz IndCat.
/// </summary>
internal sealed class SdHeaderEditor : SdEditorBase
{
    private readonly ExTextBox _comments;
    private readonly ExComboBox _indCatMode;
    private readonly ExTextBox _indCat;
    private readonly Label _info;
    private readonly ErrorProvider _errors = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
    private StateDgmDiagram? _d;

    public SdHeaderEditor()
    {
        AddHeader(Resources.FStateDgm_Diagram);
        _comments = new ExTextBox { Multiline = true, Height = 70, ScrollBars = ScrollBars.Vertical, Width = 300 };
        AddRow(Resources.FStateDgm_HlavickoveKomentare, _comments);
        _indCatMode = Combo(
            new SdEditorContext.Item(Resources.FStateDgm_IndCatPredvoleny, null),
            new SdEditorContext.Item("INDCAT6", "INDCAT6"),
            new SdEditorContext.Item("INDCAT8", "INDCAT8"),
            new SdEditorContext.Item(Resources.FStateDgm_IndCatVlastny, ""));
        AddRow(Resources.FStateDgm_IndCat, _indCatMode);
        _indCat = Text();
        AddRow("", _indCat);
        _info = AddInfo("");
        _errors.SetIconAlignment(_indCat, ErrorIconAlignment.MiddleLeft);

        _comments.TextChanged += (_, _) =>
        {
            if (Loading || _d == null) return;
            _d.HeaderComments.Clear();
            _d.HeaderComments.AddRange(_comments.Lines.Where(l => l.Length > 0));
            RaiseChanged();
        };
        _indCatMode.SelectedIndexChanged += (_, _) =>
        {
            var v = SdEditorContext.Value(_indCatMode) as string;
            _indCat.Enabled = v == "";
            if (Loading || _d == null) return;
            if (v == "") _indCat.Text = _d.EffectiveIndCat;
            else
            {
                _d.IndCat = v;
                _indCat.Text = v ?? "";
                RaiseChanged();
            }

            Validate();
        };
        _indCat.TextChanged += (_, _) =>
        {
            Validate();
            if (Loading || _d == null || !_indCat.Enabled) return;
            _d.IndCat = _indCat.Text.Trim();
            RaiseChanged();
        };
    }

    public void Bind(StateDgmDiagram d)
    {
        Loading = true;
        try
        {
            _d = d;
            _comments.Lines = d.HeaderComments.ToArray();
            var ic = d.IndCat?.Trim();
            var mode = ic == null ? null : ic.Equals("INDCAT6", StringComparison.OrdinalIgnoreCase) ? "INDCAT6" : ic.Equals("INDCAT8", StringComparison.OrdinalIgnoreCase) ? "INDCAT8" : "";
            SdEditorContext.Select(_indCatMode, mode);
            _indCat.Text = ic ?? "";
            _indCat.Enabled = mode == "";
            _info.Text = string.Format(Resources.FStateDgm_IndCatInfo, d.Categories.Count);
            Validate();
        }
        finally
        {
            Loading = false;
        }
    }

    private void Validate()
    {
        var r = _indCat.Enabled ? SdEditorContext.Check(_indCat.Text, ExprContext.Condition, false) : null;
        _errors.SetError(_indCat, r?.Message ?? "");
    }
}

/// <summary>
///     Editor kategorie vlaku.
/// </summary>
internal sealed class SdCategoryEditor : SdEditorBase
{
    private readonly ExTextBox _key = Text();
    private readonly ExTextBox _name = Text();
    private readonly ExTextBox _comment = Text();
    private readonly ExComboBox _icon;
    private StateDgmCategory? _c;

    public SdCategoryEditor()
    {
        AddHeader(Resources.FStateDgm_Kategoria);
        AddRow(Resources.FStateDgm_Kluc, _key);
        AddRow(Resources.FStateDgm_Nazov, _name);
        AddRow(Resources.FStateDgm_Komentar, _comment);
        _icon = Combo(
            new SdEditorContext.Item(Resources.FStateDgm_IkonaKat0, 0),
            new SdEditorContext.Item(Resources.FStateDgm_IkonaKat1, 1),
            new SdEditorContext.Item(Resources.FStateDgm_IkonaKat2, 2));
        AddRow(Resources.FStateDgm_Ikona, _icon);
        AddInfo(Resources.FStateDgm_KatInfo);

        _key.TextChanged += (_, _) => Set(c => c.Key = _key.Text.Trim());
        _name.TextChanged += (_, _) => Set(c => c.Name = _name.Text.Trim());
        _comment.TextChanged += (_, _) => Set(c => c.Comment = _comment.Text.Trim());
        _icon.SelectedIndexChanged += (_, _) => Set(c => c.Icon = SdEditorContext.Value(_icon) as int? ?? 0);
    }

    private void Set(Action<StateDgmCategory> a)
    {
        if (Loading || _c == null) return;
        a(_c);
        RaiseChanged();
    }

    public void Bind(StateDgmCategory c)
    {
        Loading = true;
        try
        {
            _c = c;
            _key.Text = c.Key;
            _name.Text = c.Name;
            _comment.Text = c.Comment;
            if (c.Icon is >= 0 and <= 2) SdEditorContext.Select(_icon, c.Icon);
            else
            {
                _icon.Items.Add(new SdEditorContext.Item(c.Icon.ToString(), c.Icon));
                _icon.SelectedIndex = _icon.Items.Count - 1;
            }
        }
        finally
        {
            Loading = false;
        }
    }
}

/// <summary>
///     Editor stavu - kluc, ikona, priznaky, automatika, tabule.
/// </summary>
internal sealed class SdStateEditor : SdEditorBase
{
    private readonly ExTextBox _key = Text();
    private readonly ExTextBox _name = Text();
    private readonly ExComboBox _icon;
    private readonly Dictionary<StateDgmAttr, ExCheckBox> _attr = new();
    private readonly ExComboBox _defaultControl;
    private readonly SdDynamicField _autoMode;
    private readonly SdDynamicField _autoTimePoint;
    private readonly SdDynamicField _autoAdd;
    private readonly SdDynamicField _autoModif;
    private readonly SdDynamicField _wait;
    private readonly ExTextBox _condition = Text();
    private readonly ErrorProvider _errors = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
    private readonly ExCheckBox _doOn;
    private readonly TableSetBox _do;
    private readonly ExCheckBox _advanced;
    private readonly ExCheckBox _undoOn;
    private readonly TableSetBox _undo;
    private StateDgmState? _s;

    public SdStateEditor()
    {
        AddHeader(Resources.FStateDgm_Stav);
        AddRow(Resources.FStateDgm_Kluc, _key);
        AddRow(Resources.FStateDgm_Nazov, _name);
        _icon = Combo(
            new SdEditorContext.Item(Resources.FStateDgm_IkonaStav0, 0), new SdEditorContext.Item(Resources.FStateDgm_IkonaStav1, 1),
            new SdEditorContext.Item(Resources.FStateDgm_IkonaStav2, 2), new SdEditorContext.Item(Resources.FStateDgm_IkonaStav3, 3),
            new SdEditorContext.Item(Resources.FStateDgm_IkonaStav4, 4), new SdEditorContext.Item(Resources.FStateDgm_IkonaStav5, 5));
        AddRow(Resources.FStateDgm_Ikona, _icon);
        _defaultControl = Combo();
        AddRow(Resources.FStateDgm_PredvoleneTlacidlo, _defaultControl);

        AddHeader(Resources.FStateDgm_Priznaky);
        var flags = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, WrapContents = false };
        foreach (var (a, text) in new[]
                 {
                     (StateDgmAttr.Stoji, Resources.FStateDgm_Attr_Stoji), (StateDgmAttr.PotvrzenaKolej, Resources.FStateDgm_Attr_PotvrzenaKolej),
                     (StateDgmAttr.Odbaven, Resources.FStateDgm_Attr_Odbaven), (StateDgmAttr.Shadow, Resources.FStateDgm_Attr_Shadow),
                     (StateDgmAttr.NyniStoji, Resources.FStateDgm_Attr_NyniStoji)
                 })
        {
            var cb = Check($"{text}  (0x{(int)a:X2})");
            cb.CheckedChanged += (_, _) => Set(s => s.Attr = _attr.Where(x => x.Value.Checked).Aggregate(StateDgmAttr.None, (acc, x) => acc | x.Key));
            _attr[a] = cb;
            flags.Controls.Add(cb);
        }

        AddFull(flags);

        AddHeader(Resources.FStateDgm_Automatika);
        _autoMode = SdDynamicField.Choice(ExprContext.Condition,
            new SdEditorContext.Item(Resources.FStateDgm_Nenastavene, null), new SdEditorContext.Item(Resources.FStateDgm_AutoMode0, 0),
            new SdEditorContext.Item(Resources.FStateDgm_AutoMode1, 1), new SdEditorContext.Item(Resources.FStateDgm_AutoMode2, 2));
        AddRow(Resources.FStateDgm_AutoMode, _autoMode);
        _autoTimePoint = SdDynamicField.Choice(ExprContext.Condition,
            new SdEditorContext.Item(Resources.FStateDgm_Nenastavene, null), new SdEditorContext.Item(Resources.FStateDgm_AutoTimePoint1, 1),
            new SdEditorContext.Item(Resources.FStateDgm_AutoTimePoint2, 2));
        AddRow(Resources.FStateDgm_AutoTimePoint, _autoTimePoint);
        _autoAdd = SdDynamicField.Number(-86400, 86400, 60, true);
        AddRow(Resources.FStateDgm_AutoTimePointAdd, _autoAdd, Resources.FStateDgm_AutoTimePointAddTip);
        _autoModif = SdDynamicField.Choice(ExprContext.Condition,
            new SdEditorContext.Item(Resources.FStateDgm_Nenastavene, null), new SdEditorContext.Item(Resources.FStateDgm_AutoModif1, 1),
            new SdEditorContext.Item(Resources.FStateDgm_AutoModif2, 2));
        AddRow(Resources.FStateDgm_AutoModif, _autoModif);
        _wait = SdDynamicField.Choice(ExprContext.StateDgmWait,
            new SdEditorContext.Item(Resources.FStateDgm_Nenastavene, null),
            new SdEditorContext.Item(Resources.FStateDgm_Wait_VVC, unchecked((int)StateDgmWaitEvent.VVC)),
            new SdEditorContext.Item(Resources.FStateDgm_Wait_OVC, (int)StateDgmWaitEvent.OVC),
            new SdEditorContext.Item(Resources.FStateDgm_Wait_Vj, (int)StateDgmWaitEvent.Vj),
            new SdEditorContext.Item(Resources.FStateDgm_Wait_Odj, (int)StateDgmWaitEvent.Odj),
            new SdEditorContext.Item(Resources.FStateDgm_Wait_ZCV, (int)StateDgmWaitEvent.ZCV));
        AddRow(Resources.FStateDgm_Wait, _wait);
        _condition.Font = GlobData.UsingStyle.TabTabEditorScheme.Font;
        AddRow(Resources.FStateDgm_AutoCondition, _condition, Resources.FStateDgm_AutoConditionTip);
        _errors.SetIconAlignment(_condition, ErrorIconAlignment.MiddleLeft);

        AddHeader(Resources.FStateDgm_Tabule);
        _doOn = Check(Resources.FStateDgm_TabuleZapnut);
        AddFull(_doOn);
        _do = new TableSetBox();
        AddFull(_do);

        _advanced = Check(Resources.FStateDgm_Rozsirene);
        _advanced.Font = new Font(Font, FontStyle.Bold);
        AddFull(_advanced, 12);
        _undoOn = Check(Resources.FStateDgm_UndoStateZapnut);
        AddFull(_undoOn);
        _undo = new TableSetBox();
        AddFull(_undo);
        _undoOn.Visible = _undo.Visible = false;

        _key.TextChanged += (_, _) => Set(s => s.Key = _key.Text.Trim());
        _name.TextChanged += (_, _) => Set(s => s.Name = _name.Text.Trim());
        _icon.SelectedIndexChanged += (_, _) => Set(s => s.Icon = SdEditorContext.Value(_icon) as int? ?? 0);
        _defaultControl.SelectedIndexChanged += (_, _) => Set(s => s.DefaultControl = SdEditorContext.Value(_defaultControl) as int? ?? 0);
        _autoMode.ValueChanged += (_, _) => Set(s => s.AutoMode = _autoMode.Value);
        _autoTimePoint.ValueChanged += (_, _) => Set(s => s.AutoTimePoint = _autoTimePoint.Value);
        _autoAdd.ValueChanged += (_, _) => Set(s => s.AutoTimePointAdd = _autoAdd.Value);
        _autoModif.ValueChanged += (_, _) => Set(s => s.AutoModif = _autoModif.Value);
        _wait.ValueChanged += (_, _) => Set(s => s.Wait = WaitValue());
        _condition.TextChanged += (_, _) =>
        {
            ValidateCondition();
            Set(s => s.AutoCondition = string.IsNullOrWhiteSpace(_condition.Text) ? null : _condition.Text.Trim());
        };
        _doOn.CheckedChanged += (_, _) =>
        {
            _do.Enabled = _doOn.Checked;
            Set(s => s.DoState = _doOn.Checked ? s.DoState ?? _do.ToModel() : null);
        };
        _do.Changed += (_, _) => Set(s =>
        {
            if (s.DoState != null) _do.Apply(s.DoState);
        });
        _advanced.CheckedChanged += (_, _) => _undoOn.Visible = _undo.Visible = _advanced.Checked;
        _undoOn.CheckedChanged += (_, _) =>
        {
            _undo.Enabled = _undoOn.Checked;
            Set(s => s.UndoState = _undoOn.Checked ? s.UndoState ?? _undo.ToModel() : null);
        };
        _undo.Changed += (_, _) => Set(s =>
        {
            if (s.UndoState != null) _undo.Apply(s.UndoState);
        });
    }

    private StateDgmDynamic? WaitValue()
    {
        var v = _wait.Value;
        // vyber zo zoznamu drzi ciselnu hodnotu konstanty - do suboru patri jej meno
        return v is { IsExpression: false, Number: { } n } ? StateDgmDynamic.FromWait((StateDgmWaitEvent)unchecked((uint)n)) : v;
    }

    private void Set(Action<StateDgmState> a)
    {
        if (Loading || _s == null) return;
        a(_s);
        RaiseChanged();
    }

    private void ValidateCondition()
    {
        var r = SdEditorContext.Check(_condition.Text, ExprContext.Condition, true);
        _errors.SetError(_condition, r?.Message ?? "");
    }

    /// <summary>Naplni zoznam predvolenych tlacidiel (po zmene ovladacov stavu).</summary>
    public void RefreshControls()
    {
        if (_s == null) return;
        var was = Loading;
        Loading = true;
        try
        {
            _defaultControl.Items.Clear();
            _defaultControl.Items.Add(new SdEditorContext.Item(Resources.FStateDgm_BezPredvoleneho, 0));
            for (var i = 0; i < _s.Controls.Count; i++)
            {
                var c = _s.Controls[i];
                _defaultControl.Items.Add(new SdEditorContext.Item($"{i + 1}: {c.DesignKey}{(c.EventKey.Length > 0 ? " → " + c.EventKey : "")}", i + 1));
            }

            if (_s.DefaultControl > _s.Controls.Count)
                _defaultControl.Items.Add(new SdEditorContext.Item(_s.DefaultControl.ToString(), _s.DefaultControl));
            SdEditorContext.Select(_defaultControl, _s.DefaultControl);
        }
        finally
        {
            Loading = was;
        }
    }

    public void Bind(StateDgmState s)
    {
        Loading = true;
        try
        {
            _s = s;
            _key.Text = s.Key;
            _name.Text = s.Name;
            if (s.Icon is >= 0 and <= StateDgmKeys.MAX_ICON) SdEditorContext.Select(_icon, s.Icon);
            else
            {
                _icon.Items.Add(new SdEditorContext.Item(s.Icon.ToString(), s.Icon));
                _icon.SelectedIndex = _icon.Items.Count - 1;
            }

            foreach (var (a, cb) in _attr) cb.Checked = s.Attr.HasFlag(a);
            RefreshControls();
            _autoMode.Value = s.AutoMode;
            _autoTimePoint.Value = s.AutoTimePoint;
            _autoAdd.Value = s.AutoTimePointAdd;
            _autoModif.Value = s.AutoModif;
            _wait.Value = WaitToChoice(s.Wait);
            _condition.Text = s.AutoCondition ?? "";
            ValidateCondition();
            _doOn.Checked = s.DoState != null;
            _do.Enabled = s.DoState != null;
            _do.Bind(s.DoState);
            _undoOn.Checked = s.UndoState != null;
            _undo.Enabled = s.UndoState != null;
            _undo.Bind(s.UndoState);
            if (s.UndoState != null) _advanced.Checked = true;
        }
        finally
        {
            Loading = false;
        }
    }

    private static StateDgmDynamic? WaitToChoice(StateDgmDynamic? w)
    {
        if (w == null) return null;
        if (w.IsExpression && Enum.TryParse<StateDgmWaitEvent>(w.Expression!.Trim(), out var e) && e != StateDgmWaitEvent.None)
            return StateDgmDynamic.FromNumber(unchecked((int)e));
        return w;
    }

    /// <summary>Pat prepinacov SVFTableSet.</summary>
    private sealed class TableSetBox : FlowLayoutPanel
    {
        private readonly ExCheckBox _dep = Check(Resources.FStateDgm_JeNaOdjezdove);
        private readonly ExCheckBox _arr = Check(Resources.FStateDgm_JeNaPrijezdove);
        private readonly ExCheckBox _plat = Check(Resources.FStateDgm_JeNaSmerovych);
        private readonly ExCheckBox _pos = Check(Resources.FStateDgm_JeZobrazenaPozice);
        private readonly ExCheckBox _track = Check(Resources.FStateDgm_JeZobrazenaKolej);
        private bool _loading;

        public TableSetBox()
        {
            FlowDirection = FlowDirection.TopDown;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            WrapContents = false;
            Padding = new Padding(16, 0, 0, 0);
            foreach (var cb in new[] { _dep, _arr, _plat, _pos, _track })
            {
                Controls.Add(cb);
                cb.CheckedChanged += (_, _) =>
                {
                    if (!_loading) Changed?.Invoke(this, EventArgs.Empty);
                };
            }
        }

        public event EventHandler? Changed;

        public void Bind(StateDgmTableSet? t)
        {
            _loading = true;
            _dep.Checked = t?.OnDepartureTable ?? false;
            _arr.Checked = t?.OnArrivalTable ?? false;
            _plat.Checked = t?.OnPlatformTables ?? false;
            _pos.Checked = t?.ShowPosition ?? false;
            _track.Checked = t?.ShowTrack ?? false;
            _loading = false;
        }

        public void Apply(StateDgmTableSet t)
        {
            t.OnDepartureTable = _dep.Checked;
            t.OnArrivalTable = _arr.Checked;
            t.OnPlatformTables = _plat.Checked;
            t.ShowPosition = _pos.Checked;
            t.ShowTrack = _track.Checked;
        }

        public StateDgmTableSet ToModel()
        {
            var t = new StateDgmTableSet();
            Apply(t);
            return t;
        }
    }
}

/// <summary>
///     Editor vzhladu tlacidla.
/// </summary>
internal sealed class SdDesignEditor : SdEditorBase
{
    private readonly ExTextBox _key = Text();
    private readonly ExTextBox _bitmaps = Text("6-7,8,9");
    private readonly ExCheckBox _def = Check(Resources.FStateDgm_DefPushBtn);
    private readonly ExTextBox _class = Text();
    private readonly ErrorProvider _errors = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
    private StateDgmDesign? _d;

    public SdDesignEditor()
    {
        AddHeader(Resources.FStateDgm_Vzhlad);
        AddRow(Resources.FStateDgm_Kluc, _key);
        AddRow(Resources.FStateDgm_Bitmaps, _bitmaps);
        AddFull(_def);
        AddRow(Resources.FStateDgm_Trieda, _class);
        AddInfo(Resources.FStateDgm_BitmapsInfo);
        _errors.SetIconAlignment(_bitmaps, ErrorIconAlignment.MiddleLeft);

        _key.TextChanged += (_, _) => Set(d => d.Key = _key.Text.Trim());
        _bitmaps.TextChanged += (_, _) =>
        {
            _errors.SetError(_bitmaps, StateDgmBitmaps.TryParse(_bitmaps.Text, out _) ? "" : Resources.FStateDgm_Bitmaps);
            Set(d => d.Bitmaps = _bitmaps.Text.Trim());
        };
        _def.CheckedChanged += (_, _) => Set(d => d.DefaultPushButton = _def.Checked);
        _class.TextChanged += (_, _) => Set(d => d.Class = _class.Text.Trim());
    }

    private void Set(Action<StateDgmDesign> a)
    {
        if (Loading || _d == null) return;
        a(_d);
        RaiseChanged();
    }

    public void Bind(StateDgmDesign d)
    {
        Loading = true;
        try
        {
            _d = d;
            _key.Text = d.Key;
            _bitmaps.Text = d.Bitmaps;
            _def.Checked = d.DefaultPushButton;
            _class.Text = d.Class;
        }
        finally
        {
            Loading = false;
        }
    }
}

/// <summary>
///     Editor casoveho bodu (vlastneho v hlavicke alebo v stave).
/// </summary>
internal sealed class SdTimePointEditor : SdEditorBase
{
    private readonly ExTextBox _key = Text();
    private readonly ExTextBox _name = Text();
    private readonly ExComboBox _key1 = new() { DropDownStyle = ComboBoxStyle.DropDown, Width = 220 };
    private readonly ExNumericUpDown _off1 = Number(-86400, 86400, 60);
    private readonly ExComboBox _key2 = new() { DropDownStyle = ComboBoxStyle.DropDown, Width = 220 };
    private readonly ExNumericUpDown _off2 = Number(-86400, 86400, 60);
    private readonly ExComboBox _op;
    private StateDgmTimePoint? _t;

    public SdTimePointEditor()
    {
        AddHeader(Resources.FStateDgm_CasovyBod);
        AddRow(Resources.FStateDgm_Kluc, _key);
        AddRow(Resources.FStateDgm_Nazov, _name);
        AddRow(Resources.FStateDgm_TP_Key1, _key1);
        AddRow(Resources.FStateDgm_TP_Offset1, _off1);
        AddRow(Resources.FStateDgm_TP_Key2, _key2);
        AddRow(Resources.FStateDgm_TP_Offset2, _off2);
        _op = Combo(new SdEditorContext.Item(Resources.FStateDgm_TP_Min, StateDgmKeys.OPERATOR_MIN), new SdEditorContext.Item(Resources.FStateDgm_TP_Max, StateDgmKeys.OPERATOR_MAX));
        AddRow(Resources.FStateDgm_TP_Operator, _op);
        AddInfo(Resources.FStateDgm_TP_Info);
        AddInfo(string.Format(Resources.FStateDgm_TP_Zabudovane, string.Join(", ", StateDgmKeys.BuiltInTimePoints)));

        _key.TextChanged += (_, _) => Set(t => t.Key = _key.Text.Trim());
        _name.TextChanged += (_, _) => Set(t => t.Name = _name.Text.Trim());
        _key1.TextChanged += (_, _) => Set(t => t.TimePointKey1 = _key1.Text.Trim());
        _key2.TextChanged += (_, _) => Set(t => t.TimePointKey2 = _key2.Text.Trim());
        _off1.ValueChanged += (_, _) => Set(t => t.Offset1 = (int)_off1.Value);
        _off2.ValueChanged += (_, _) => Set(t => t.Offset2 = (int)_off2.Value);
        _op.SelectedIndexChanged += (_, _) => Set(t => t.Operator = SdEditorContext.Value(_op) as string ?? StateDgmKeys.OPERATOR_MIN);
    }

    private void Set(Action<StateDgmTimePoint> a)
    {
        if (Loading || _t == null) return;
        a(_t);
        RaiseChanged();
    }

    public void Bind(StateDgmTimePoint t, IEnumerable<string> availableKeys)
    {
        Loading = true;
        try
        {
            _t = t;
            var keys = availableKeys.Where(k => k != t.Key).Cast<object>().ToArray();
            _key1.Items.Clear();
            _key1.Items.AddRange(keys);
            _key2.Items.Clear();
            _key2.Items.AddRange(keys);
            _key.Text = t.Key;
            _name.Text = t.Name;
            _key1.Text = t.TimePointKey1;
            _key2.Text = t.TimePointKey2;
            _off1.Value = Math.Clamp(t.Offset1, _off1.Minimum, _off1.Maximum);
            _off2.Value = Math.Clamp(t.Offset2, _off2.Minimum, _off2.Maximum);
            SdEditorContext.Select(_op, t.Operator == StateDgmKeys.OPERATOR_MAX ? StateDgmKeys.OPERATOR_MAX : StateDgmKeys.OPERATOR_MIN);
        }
        finally
        {
            Loading = false;
        }
    }
}

/// <summary>
///     Editor akcie stavu spolu s jej tlacidlom (jeden riadok mriezky).
/// </summary>
internal sealed class SdEventEditor : SdEditorBase
{
    private readonly ExTextBox _key = Text();
    private readonly ExTextBox _name = Text();
    private readonly ExComboBox _class;
    private readonly ExComboBox _next = new() { DropDownStyle = ComboBoxStyle.DropDown, Width = 220 };
    private readonly ExComboBox _report = new() { DropDownStyle = ComboBoxStyle.DropDown, Width = 220 };
    private readonly ExComboBox _dialog;
    private readonly ExCheckBox _hasControl = Check(Resources.FStateDgm_Akcia_MaTlacidlo);
    private readonly ExNumericUpDown _ctrlId = Number(0, 99);
    private readonly ExComboBox _design = new() { DropDownStyle = ComboBoxStyle.DropDown, Width = 220 };
    private readonly ExCheckBox _adv = Check(Resources.FStateDgm_Akcia_Rozsirene);
    private readonly ExCheckBox _posArr = Check(Resources.FStateDgm_Akcia_PosArr);
    private readonly ExCheckBox _posDep = Check(Resources.FStateDgm_Akcia_PosDep);
    private readonly ExCheckBox _copyPos = Check(Resources.FStateDgm_Akcia_CopyPos);
    private readonly ExCheckBox _modif = Check(Resources.FStateDgm_Akcia_ModifReport);
    private readonly ExCheckBox _ask = Check(Resources.FStateDgm_Akcia_AskReport);
    private readonly ExCheckBox _hide = Check(Resources.FStateDgm_Akcia_HideShow);
    private readonly ExCheckBox _delayArr = Check(Resources.FStateDgm_Akcia_DelayArr);
    private readonly ExCheckBox _delayDep = Check(Resources.FStateDgm_Akcia_DelayDep);
    private readonly FlowLayoutPanel _advPanel;

    public SdEventEditor()
    {
        AddHeader(Resources.FStateDgm_Akcia_Titul);
        AddRow(Resources.FStateDgm_Kluc, _key);
        AddRow(Resources.FStateDgm_Nazov, _name);
        _class = Combo(
            new SdEditorContext.Item(Resources.FStateDgm_Akcia_Class_UniPos, "SDEventUniPos"),
            new SdEditorContext.Item(Resources.FStateDgm_Akcia_Class_Dialog, "SDEventWithDialog"),
            new SdEditorContext.Item(Resources.FStateDgm_Akcia_Class_Change, "SDEventChangeState"),
            new SdEditorContext.Item(Resources.FStateDgm_Akcia_Class_Report, "SDEventReportAboutState"),
            new SdEditorContext.Item(Resources.FStateDgm_Akcia_Class_VlakAttr, "SDEventVlakAttr"));
        AddRow(Resources.FStateDgm_Trieda, _class);
        AddRow(Resources.FStateDgm_Akcia_NextState, _next);
        AddRow(Resources.FStateDgm_Akcia_ReportKey, _report);
        _dialog = Combo(
            new SdEditorContext.Item(Resources.FStateDgm_Akcia_Dlg_Kolej, "SDDlgKolej"),
            new SdEditorContext.Item(Resources.FStateDgm_Akcia_Dlg_Zpozdeni, "SDDlgZpozdeni"),
            new SdEditorContext.Item(Resources.FStateDgm_Akcia_Dlg_ZpozdeniG, "SDDlgZpozdeniG"));
        AddRow(Resources.FStateDgm_Akcia_Dialog, _dialog);

        AddHeader(Resources.FStateDgm_Akcia_Tlacidlo);
        AddFull(_hasControl);
        AddRow(Resources.FStateDgm_Akcia_CtrlID, _ctrlId);
        AddRow(Resources.FStateDgm_Akcia_Vzhlad, _design);

        _adv.Font = new Font(Font, FontStyle.Bold);
        AddFull(_adv, 12);
        _advPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, WrapContents = false, Visible = false, Padding = new Padding(16, 0, 0, 0) };
        foreach (var cb in new[] { _posArr, _posDep, _copyPos, _modif, _ask, _hide, _delayArr, _delayDep }) _advPanel.Controls.Add(cb);
        AddFull(_advPanel);

        _class.SelectedIndexChanged += (_, _) => UpdateEnabled();
        _hasControl.CheckedChanged += (_, _) => UpdateEnabled();
        _adv.CheckedChanged += (_, _) => _advPanel.Visible = _adv.Checked;
        foreach (var c in new Control[] { _key, _name, _class, _next, _report, _dialog, _hasControl, _ctrlId, _design, _posArr, _posDep, _copyPos, _modif, _ask, _hide, _delayArr, _delayDep })
        {
            switch (c)
            {
                case TextBox t: t.TextChanged += (_, _) => RaiseChanged(); break;
                case ComboBox cb: cb.TextChanged += (_, _) => RaiseChanged(); cb.SelectedIndexChanged += (_, _) => RaiseChanged(); break;
                case CheckBox ch: ch.CheckedChanged += (_, _) => RaiseChanged(); break;
                case NumericUpDown n: n.ValueChanged += (_, _) => RaiseChanged(); break;
            }
        }
    }

    private void UpdateEnabled()
    {
        var cls = SdEditorContext.Value(_class) as string;
        SetRowVisible(_dialog, cls == "SDEventWithDialog");
        SetRowVisible(_next, cls != "SDEventWithDialog");
        SetRowVisible(_report, cls is "SDEventUniPos" or "SDEventReportAboutState" or "SDEventVlakAttr");
        _delayArr.Visible = _delayDep.Visible = cls == "SDEventVlakAttr";
        SetRowVisible(_ctrlId, _hasControl.Checked);
        SetRowVisible(_design, _hasControl.Checked);
    }

    /// <summary>Naplni editor akciou a jej tlacidlom (null = bez tlacidla).</summary>
    public void Bind(StateDgmEvent e, StateDgmControl? control, IEnumerable<string> stateKeys, IEnumerable<string> designKeys, int nextCtrlId)
    {
        Loading = true;
        try
        {
            _next.Items.Clear();
            _next.Items.Add(Resources.FStateDgm_Akcia_BezZmeny);
            _next.Items.AddRange(stateKeys.Cast<object>().ToArray());
            _report.Items.Clear();
            _report.Items.Add(Resources.FStateDgm_Akcia_BezHlasenia);
            _report.Items.AddRange(SdEditorContext.ReportKeys.Cast<object>().ToArray());
            _design.Items.Clear();
            _design.Items.AddRange(designKeys.Cast<object>().ToArray());

            _key.Text = e.Key;
            _name.Text = e.Name ?? "";
            if (!StateDgmKeys.EventClasses.Contains(e.Class) || _class.Items.Cast<SdEditorContext.Item>().All(i => (string)i.Value! != e.Class))
                _class.Items.Add(new SdEditorContext.Item(e.Class, e.Class));
            SdEditorContext.Select(_class, e.Class);
            _next.Text = string.IsNullOrEmpty(e.NextState) ? Resources.FStateDgm_Akcia_BezZmeny : e.NextState;
            _report.Text = string.IsNullOrEmpty(e.ReportKey) ? Resources.FStateDgm_Akcia_BezHlasenia : e.ReportKey;
            if (e.Dialog != null && !StateDgmKeys.Dialogs.Contains(e.Dialog)) _dialog.Items.Add(new SdEditorContext.Item(e.Dialog, e.Dialog));
            SdEditorContext.Select(_dialog, e.Dialog ?? "SDDlgKolej");
            _hasControl.Checked = control != null;
            _ctrlId.Value = control?.CtrlId ?? nextCtrlId;
            _design.Text = control?.DesignKey ?? "";
            _posArr.Checked = e.PositionForArrival is > 0;
            _posDep.Checked = e.PositionForDeparture is > 0;
            _copyPos.Checked = e.CopyPosition is not 0;
            _modif.Checked = e.ModifyReport is > 0 || (e.ModifyReport == null && !string.IsNullOrEmpty(e.ReportKey));
            _ask.Checked = e.AskBeforeReport is > 0;
            _hide.Checked = e.HideShow is > 0;
            _delayArr.Checked = e.DelayArrival is > 0;
            _delayDep.Checked = e.DelayDeparture is > 0;
            _adv.Checked = e.PositionForArrival != null || e.PositionForDeparture != null || e.CopyPosition != null || e.ModifyReport != null
                           || e.AskBeforeReport != null || e.HideShow != null || e.DelayArrival != null || e.DelayDeparture != null;
            UpdateEnabled();
        }
        finally
        {
            Loading = false;
        }
    }

    /// <summary>Zapise hodnoty do akcie a vrati tlacidlo (null = bez tlacidla).</summary>
    public StateDgmControl? Apply(StateDgmEvent e, StateDgmControl? existing)
    {
        e.Key = _key.Text.Trim();
        e.Name = string.IsNullOrWhiteSpace(_name.Text) ? null : _name.Text.Trim();
        e.Class = SdEditorContext.Value(_class) as string ?? "SDEventUniPos";
        var next = _next.Text.Trim();
        e.NextState = e.Class == "SDEventWithDialog" || next.Length == 0 || next == Resources.FStateDgm_Akcia_BezZmeny ? null : next;
        var rep = _report.Text.Trim();
        e.ReportKey = !_report.Visible || rep.Length == 0 || rep == Resources.FStateDgm_Akcia_BezHlasenia ? null : rep;
        e.Dialog = e.Class == "SDEventWithDialog" ? SdEditorContext.Value(_dialog) as string : null;
        // rozsirene volby: zapisu sa len tie, ktore sa lisia od predvolenych INISSu
        e.PositionForArrival = _posArr.Checked ? 1 : null;
        e.PositionForDeparture = _posDep.Checked ? 1 : null;
        e.CopyPosition = _copyPos.Checked ? null : 0;
        var defaultModif = !string.IsNullOrEmpty(e.ReportKey);
        e.ModifyReport = _modif.Checked == defaultModif ? null : _modif.Checked ? 1 : 0;
        e.AskBeforeReport = _ask.Checked ? 1 : null;
        e.HideShow = _hide.Checked ? 1 : null;
        e.DelayArrival = e.Class == "SDEventVlakAttr" && _delayArr.Checked ? 1 : null;
        e.DelayDeparture = e.Class == "SDEventVlakAttr" && _delayDep.Checked ? 1 : null;

        if (!_hasControl.Checked) return null;
        var c = existing ?? new StateDgmControl();
        c.CtrlId = (int)_ctrlId.Value;
        c.DesignKey = _design.Text.Trim();
        c.EventKey = e.Key;
        return c;
    }
}

/// <summary>
///     Editor startera s generovanou vetou.
/// </summary>
internal sealed class SdStarterEditor : SdEditorBase
{
    private readonly ExTextBox _key = Text();
    private readonly ExComboBox _event = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220 };
    private readonly ExComboBox _tp = new() { DropDownStyle = ComboBoxStyle.DropDown, Width = 220 };
    private readonly ExNumericUpDown _offset = Number(-86400, 86400, 60);
    private readonly ExCheckBox _repeat = Check(Resources.FStateDgm_Starter_Opakovat);
    private readonly ExNumericUpDown _step = Number(1, 86400, 60);
    private readonly ExComboBox _tpLast = new() { DropDownStyle = ComboBoxStyle.DropDown, Width = 220 };
    private readonly ExNumericUpDown _offsetLast = Number(-86400, 86400, 60);
    private readonly ExCheckBox _later = Check(Resources.FStateDgm_Starter_Later);
    private readonly Label _sentence;

    public SdStarterEditor()
    {
        AddHeader(Resources.FStateDgm_Starter_Titul);
        AddRow(Resources.FStateDgm_Kluc, _key);
        AddRow(Resources.FStateDgm_Starter_Akcia, _event);
        AddRow(Resources.FStateDgm_Starter_TimePoint, _tp);
        AddRow(Resources.FStateDgm_Starter_Offset, _offset);
        AddFull(_repeat);
        AddRow(Resources.FStateDgm_Starter_Step, _step);
        AddRow(Resources.FStateDgm_Starter_Last, _tpLast);
        AddRow(Resources.FStateDgm_Starter_OffsetLast, _offsetLast);
        AddFull(_later);
        _sentence = AddInfo("");
        _sentence.ForeColor = SystemColors.ControlText;

        _repeat.CheckedChanged += (_, _) =>
        {
            SetRowVisible(_step, _repeat.Checked);
            SetRowVisible(_tpLast, _repeat.Checked);
            SetRowVisible(_offsetLast, _repeat.Checked);
        };
        foreach (var c in new Control[] { _key, _event, _tp, _offset, _repeat, _step, _tpLast, _offsetLast, _later })
        {
            switch (c)
            {
                case TextBox t: t.TextChanged += (_, _) => Touch(); break;
                case ComboBox cb: cb.TextChanged += (_, _) => Touch(); cb.SelectedIndexChanged += (_, _) => Touch(); break;
                case CheckBox ch: ch.CheckedChanged += (_, _) => Touch(); break;
                case NumericUpDown n: n.ValueChanged += (_, _) => Touch(); break;
            }
        }
    }

    private void Touch()
    {
        _sentence.Text = Sentence(Preview());
        RaiseChanged();
    }

    private StateDgmStarter Preview()
    {
        var s = new StateDgmStarter();
        Apply(s);
        return s;
    }

    /// <summary>Veta popisujuca starter.</summary>
    public static string Sentence(StateDgmStarter s)
    {
        var first = s.TimeOffset == 0 ? Resources.FStateDgm_V_case : $"{SdEditorContext.Seconds(s.TimeOffset)} {(s.TimeOffset < 0 ? Resources.FStateDgm_Pred : Resources.FStateDgm_Po)}";
        var step = s.TimeOffsetStep is > 0 ? string.Format(Resources.FStateDgm_Starter_VetaKrok, SdEditorContext.Seconds(s.TimeOffsetStep.Value)) : "";
        var last = s.TimeOffsetLast != null || s.TimePointKeyLast != null
            ? string.Format(Resources.FStateDgm_Starter_VetaLast,
                s.TimeOffsetLast is { } l && l != 0 ? $"{SdEditorContext.Seconds(l)} {(l < 0 ? Resources.FStateDgm_Pred : Resources.FStateDgm_Po)}" : Resources.FStateDgm_V_case,
                s.TimePointKeyLast ?? s.TimePointKey)
            : "";
        return string.Format(Resources.FStateDgm_Starter_Veta, s.EventKey, first, s.TimePointKey, step, last);
    }

    public void Bind(StateDgmStarter s, IEnumerable<string> eventKeys, IEnumerable<string> timePointKeys)
    {
        Loading = true;
        try
        {
            _event.Items.Clear();
            _event.Items.AddRange(eventKeys.Cast<object>().ToArray());
            var tps = timePointKeys.Cast<object>().ToArray();
            _tp.Items.Clear();
            _tp.Items.AddRange(tps);
            _tpLast.Items.Clear();
            _tpLast.Items.AddRange(tps);

            _key.Text = s.Key;
            if (s.EventKey.Length > 0 && !_event.Items.Contains(s.EventKey)) _event.Items.Add(s.EventKey);
            _event.SelectedItem = s.EventKey.Length > 0 ? s.EventKey : null;
            _tp.Text = s.TimePointKey;
            _offset.Value = Math.Clamp(s.TimeOffset, _offset.Minimum, _offset.Maximum);
            _repeat.Checked = s.TimeOffsetStep is > 0;
            _step.Value = Math.Clamp(s.TimeOffsetStep ?? 600, _step.Minimum, _step.Maximum);
            _tpLast.Text = s.TimePointKeyLast ?? "";
            _offsetLast.Value = Math.Clamp(s.TimeOffsetLast ?? 0, _offsetLast.Minimum, _offsetLast.Maximum);
            _later.Checked = s.StartLaterToo;
            SetRowVisible(_step, _repeat.Checked);
            SetRowVisible(_tpLast, _repeat.Checked);
            SetRowVisible(_offsetLast, _repeat.Checked);
            _sentence.Text = Sentence(s);
        }
        finally
        {
            Loading = false;
        }
    }

    public void Apply(StateDgmStarter s)
    {
        s.Key = _key.Text.Trim();
        s.EventKey = _event.SelectedItem as string ?? "";
        s.TimePointKey = _tp.Text.Trim();
        s.TimeOffset = (int)_offset.Value;
        s.TimeOffsetStep = _repeat.Checked ? (int)_step.Value : null;
        s.TimePointKeyLast = _repeat.Checked && _tpLast.Text.Trim().Length > 0 ? _tpLast.Text.Trim() : null;
        s.TimeOffsetLast = _repeat.Checked && (int)_offsetLast.Value != 0 ? (int)_offsetLast.Value : null;
        s.StartLaterToo = _later.Checked;
    }
}
