using ExControls;
using GVDEditor.Properties;
using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Editor casoveho bodu (vlastneho v hlavicke alebo v stave).
/// </summary>
internal sealed class SdTimePointEditor : SdEditorBase
{
    private readonly ExTextBox _key = TextField();
    private readonly ExTextBox _name = TextField();
    private readonly ExComboBox _key1 = new() { DropDownStyle = ComboBoxStyle.DropDown, Width = 220 };
    private readonly ExNumericUpDown _off1 = Number(-86400, 86400, 60);
    private readonly ExComboBox _key2 = new() { DropDownStyle = ComboBoxStyle.DropDown, Width = 220 };
    private readonly ExNumericUpDown _off2 = Number(-86400, 86400, 60);
    private readonly ExComboBox _op;
    private StateDgmTimePoint? _t;

    public SdTimePointEditor(SdEditorContext context) : base(context)
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

        _key.TextChanged += (_, _) => Set(t =>
        {
            t.Key = _key.Text.Trim();
            RenameReferences(t, t.Key);
        });
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

    public void Bind(StateDgmDiagram d, StateDgmTimePoint t, IEnumerable<string> availableKeys)
    {
        Loading = true;
        try
        {
            _t = t;
            BindKey(d, t.Key);
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
