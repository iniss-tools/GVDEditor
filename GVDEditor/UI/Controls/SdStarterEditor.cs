using ExControls;
using GVDEditor.Properties;
using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Editor startera s generovanou vetou.
/// </summary>
internal sealed class SdStarterEditor : SdEditorBase
{
    private readonly ExTextBox _key = TextField();
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
