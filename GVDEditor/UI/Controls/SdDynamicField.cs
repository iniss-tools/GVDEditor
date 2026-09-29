using ExControls;
using GVDEditor.Properties;
using ToolsCore;
using ToolsCore.Expressions;
using ToolsCore.StateDgm;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Hodnota, ktoru INISS cita ako cislo aj ako vyraz: vyber zo zoznamu / cislo, alebo po prepnuti [ƒ] volny vyraz.
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
            ValidateInput();
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
        ValidateInput();
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

                ValidateInput();
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

    private void ValidateInput()
    {
        var r = _isExpr ? SdEditorContext.Check(_expr.Text, _context, false) : null;
        _errors.SetError(_expr, r?.Message ?? "");
    }
}
