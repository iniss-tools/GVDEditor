using ExControls;
using GVDEditor.Properties;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Editor stavu - kluc, ikona, priznaky, automatika, tabule.
/// </summary>
internal sealed class SdStateEditor : SdEditorBase
{
    private readonly ExTextBox _key = TextField();
    private readonly ExTextBox _name = TextField();
    private readonly ExComboBox _icon;
    private readonly Dictionary<StateDgmAttr, ExCheckBox> _attr = new();
    private readonly ExComboBox _defaultControl;
    private readonly SdDynamicField _autoMode;
    private readonly SdDynamicField _autoTimePoint;
    private readonly SdDynamicField _autoAdd;
    private readonly SdDynamicField _autoModif;
    private readonly SdDynamicField _wait;
    private readonly ExTextBox _condition = TextField();
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

        _key.TextChanged += (_, _) => Set(s =>
        {
            s.Key = _key.Text.Trim();
            RenameReferences(s, s.Key);
        });
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

    public void Bind(StateDgmDiagram d, StateDgmState s)
    {
        Loading = true;
        try
        {
            _s = s;
            BindKey(d, s.Key);
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
