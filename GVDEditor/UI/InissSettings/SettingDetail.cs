using System.Globalization;
using ExControls;
using GVDEditor.Properties;
using ToolsCore.Iniss.Registry;

namespace GVDEditor.UI.InissSettings;

/// <summary>Zmena v detaile - ciel zapisu alebo obnovenie predvolenej.</summary>
/// <param name="Target">kam zapisat</param>
/// <param name="Reset">obnovit predvolenu (zmazat zo vsetkych vrstiev)</param>
internal sealed record SettingEdit(RegWriteTarget Target, bool Reset);

/// <summary>
/// Detail vybranej hodnoty: popis, ciel zapisu, obnovenie predvolenej, vrstvy (kde hodnota lezi) a zistenia.
/// Samotna hodnota sa upravuje priamo v tabulke.
/// </summary>
internal sealed class SettingDetail : UserControl
{
    private readonly TableLayoutPanel _table;
    private readonly Label _title = new() { AutoSize = true, Margin = new Padding(3, 3, 3, 6) };
    private readonly Label _description = WrapLabel();
    private readonly Label _targetLabel = new() { AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 7, 6, 3), Text = Resources.InissSettings_Target };
    private readonly ExComboBox _target = new() { DropDownStyle = ComboBoxStyle.DropDownList, Anchor = AnchorStyles.Left | AnchorStyles.Right, MaximumSize = new Size(700, 0) };
    private readonly Label _targetNote = WrapLabel();
    private readonly ExButton _reset = new() { AutoSize = true, Text = Resources.InissSettings_Reset };
    private readonly ExButton _revert = new() { AutoSize = true, Text = Resources.InissSettings_Revert };
    private readonly FlowLayoutPanel _actions = new() { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, WrapContents = false, Margin = new Padding(0, 6, 0, 0) };
    private readonly Label _layersHeader = Header(Resources.InissSettings_Layers);
    private readonly Label _layers = WrapLabel();
    private readonly Label _diagnosticsHeader = Header(Resources.InissSettings_Findings);
    private readonly Label _diagnostics = WrapLabel();
    private SettingRow? _row;
    private InissSettingsModel? _model;
    private bool _loading;

    public SettingDetail()
    {
        AutoScroll = true;
        _title.Font = new Font(Font, FontStyle.Bold);
        _table = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 2, Padding = new Padding(6) };
        _table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        AddFull(_title);
        AddFull(_description);
        AddRow(_targetLabel, _target);
        AddFull(_targetNote);
        _actions.Controls.Add(_reset);
        _actions.Controls.Add(_revert);
        AddFull(_actions);
        AddFull(_layersHeader);
        AddFull(_layers);
        AddFull(_diagnosticsHeader);
        AddFull(_diagnostics);
        Controls.Add(_table);

        _target.SelectedIndexChanged += (_, _) =>
        {
            if (_loading || _model is null) return;
            UpdateTargetNote(_model);
            Edited?.Invoke(this, new SettingEdit(SelectedTarget, false));
        };
        _reset.Click += (_, _) => Edited?.Invoke(this, new SettingEdit(SelectedTarget, true));
        _revert.Click += (_, _) => Reverted?.Invoke(this, EventArgs.Empty);
        _table.SizeChanged += (_, _) => UpdateWrap();
        Clear();
    }

    /// <summary>Pouzivatel zmenil ciel zapisu alebo poziadal o obnovenie predvolenej.</summary>
    public event EventHandler<SettingEdit>? Edited;

    /// <summary>Pouzivatel vratil neulozenu zmenu.</summary>
    public event EventHandler? Reverted;

    /// <summary>Vybrany ciel zapisu.</summary>
    public RegWriteTarget SelectedTarget => _target.SelectedItem is TargetItem t ? t.Target : RegWriteTarget.Registry;

    /// <summary>Prazdny detail (nic nie je vybrate).</summary>
    public void Clear()
    {
        _row = null;
        _title.Text = Resources.InissSettings_NoSelection;
        foreach (var c in new Control[] { _description, _targetLabel, _target, _targetNote, _actions, _layersHeader, _layers, _diagnosticsHeader, _diagnostics })
            c.Visible = false;
    }

    /// <summary>Zobrazi riadok a jeho neulozenu zmenu.</summary>
    public void Bind(SettingRow row, PendingChange? pending, InissSettingsModel model)
    {
        _loading = true;
        try
        {
            _row = row;
            _model = model;
            var setting = row.Setting;
            _title.Text = $"{row.Section}\\{row.Name}";
            _description.Text = Description(row);
            _description.Visible = true;

            var editable = setting is not null && setting.Setting.Type != RegValueType.Binary;
            _targetLabel.Visible = _target.Visible = editable;
            _target.Items.Clear();
            if (editable)
            {
                _target.Items.Add(new TargetItem(RegWriteTarget.Registry, RegistryTargetText(setting!, model)));
                if (model.IniPath is not null) _target.Items.Add(new TargetItem(RegWriteTarget.Ini, Resources.InissSettings_Target_Ini));
                var target = pending?.Target ?? InissSettingsModel.DefaultTarget(row);
                _target.SelectedIndex = target == RegWriteTarget.Ini && _target.Items.Count > 1 ? 1 : 0;
            }

            UpdateTargetNote(model);
            _actions.Visible = true;
            _reset.Text = setting is null ? Resources.InissSettings_Delete : Resources.InissSettings_Reset;
            _reset.Enabled = row.Extra.Count > 0 || (setting?.Layers.Count ?? 0) > 0;
            _revert.Visible = pending is not null;

            _layersHeader.Visible = _layers.Visible = true;
            _layers.Text = LayersText(row, pending);
            _diagnosticsHeader.Visible = _diagnostics.Visible = row.Diagnostics.Count > 0;
            _diagnostics.Text = string.Join(Environment.NewLine, row.Diagnostics.Select(d => $"• {InissSettingsModel.SeverityText(d.Severity)}: {d.Message}"));
            UpdateWrap();
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>Obnovi casti detailu zavisle od neulozenej zmeny.</summary>
    public void UpdatePending(SettingRow row, PendingChange? pending)
    {
        if (!ReferenceEquals(_row, row)) return;
        _layers.Text = LayersText(row, pending);
        _revert.Visible = pending is not null;
    }

    private void UpdateTargetNote(InissSettingsModel model)
    {
        var notes = new List<string>();
        if (_row?.Setting is { } s)
        {
            if (s.Setting.Type == RegValueType.Color) notes.Add(Resources.InissSettings_Note_Color);
            var section = model.Config.FindSection(s.Section);
            if (SelectedTarget == RegWriteTarget.Registry)
            {
                if (section?.FromIni == true) notes.Add(Resources.InissSettings_Note_SectionFromIni);
                if (s.Source == RegSource.Ini) notes.Add(Resources.InissSettings_Note_IniRemoved);
                if (s.RegistryLocation == RegLocation.Machine && !model.CanWriteMachine) notes.Add(Resources.InissSettings_Note_Uac);
            }
            else
            {
                notes.Add(Resources.InissSettings_Note_Ini);
                if (s.Setting.Write is RegWriteMode.AutoAndApp or RegWriteMode.App) notes.Add(Resources.InissSettings_Note_IniAppWrites);
            }

            if (!s.IsRead) notes.Add(Resources.InissSettings_Note_NotRead);
        }

        _targetNote.Text = string.Join(Environment.NewLine, notes);
        _targetNote.Visible = notes.Count > 0;
    }

    private static string RegistryTargetText(ResolvedSetting setting, InissSettingsModel model)
    {
        if (setting.RegistryLocation == RegLocation.User) return Resources.InissSettings_Target_User;
        return model.CanWriteMachine ? Resources.InissSettings_Target_Machine : Resources.InissSettings_Target_MachineUac;
    }

    private static string Description(SettingRow row)
    {
        if (row.Setting is not { } s) return Resources.InissSettings_ExtraDescription;
        var text = s.Setting.Description;
        var choices = s.Setting.Choices.Select(c => $"• {c.Value} – {c.Text}").ToList();
        if (choices.Count > 0) text += Environment.NewLine + string.Join(Environment.NewLine, choices);
        return text;
    }

    private static string LayersText(SettingRow row, PendingChange? pending)
    {
        var setting = row.Setting?.Setting;
        var lines = row.Extra.Select(l => $"{InissSettingsModel.LocationText(l.Location)}: {InissSettingsModel.Format(l.Raw, setting)}{StateSuffix(l.State)}").ToList();
        if (lines.Count == 0) lines.Add(Resources.InissSettings_NoLayers);
        if (row.Setting is { } s)
        {
            var def = s.DefaultValue is null ? s.Setting.DynamicDefaultText ?? "—" : InissSettingsModel.Format(s.DefaultValue, setting);
            lines.Add(string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_DefaultLine, def));
            lines.Add(string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_EffectiveLine, InissSettingsModel.Format(s.Value, setting),
                InissSettingsModel.SourceText(s.Source)));
        }

        if (pending is not null)
            lines.Add(string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_PendingLine,
                pending.Value is null ? Resources.InissSettings_PendingReset : InissSettingsModel.Format(pending.Value, setting)));
        return string.Join(Environment.NewLine, lines);
    }

    private static string StateSuffix(RegLayerState state) => state switch
    {
        RegLayerState.Used => " ← " + Resources.InissSettings_Layer_Used,
        RegLayerState.Shadowed => " (" + Resources.InissSettings_Layer_Shadowed + ")",
        RegLayerState.WrongType => " (" + Resources.InissSettings_Layer_WrongType + ")",
        _ => " (" + Resources.InissSettings_Layer_NotRead + ")"
    };

    // --- rozlozenie ---

    private void AddRow(Control label, Control control)
    {
        var row = _table.RowCount++;
        _table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _table.Controls.Add(label, 0, row);
        _table.Controls.Add(control, 1, row);
    }

    private void AddFull(Control control)
    {
        var row = _table.RowCount++;
        _table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _table.Controls.Add(control, 0, row);
        _table.SetColumnSpan(control, 2);
    }

    private void UpdateWrap()
    {
        var width = Math.Max(200, _table.ClientSize.Width - 24);
        foreach (var l in new[] { _description, _targetNote, _layers, _diagnostics })
            l.MaximumSize = new Size(width, 0);
    }

    private static Label WrapLabel() => new() { AutoSize = true, Margin = new Padding(3, 3, 3, 6) };

    private static Label Header(string text)
    {
        var l = new Label { AutoSize = true, Text = text, Margin = new Padding(3, 12, 3, 3) };
        l.Font = new Font(l.Font, FontStyle.Bold);
        return l;
    }

    private sealed record TargetItem(RegWriteTarget Target, string Text)
    {
        public override string ToString() => Text;
    }
}
