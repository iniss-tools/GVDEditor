using System.Globalization;
using ExControls;
using GVDEditor.Properties;
using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Editor akcie stavu spolu s jej tlacidlom (jeden riadok mriezky).
/// </summary>
internal sealed class SdEventEditor : SdEditorBase
{
    private readonly ExTextBox _key = TextField();
    private readonly ExTextBox _name = TextField();
    private readonly ExComboBox _class;
    private readonly ExComboBox _next = new() { DropDownStyle = ComboBoxStyle.DropDown, Width = 220 };
    private readonly ExComboBox _report = new() { DropDownStyle = ComboBoxStyle.DropDown, Width = 220 };
    private readonly LinkLabel _reportInfo = new() { AutoSize = true, MaximumSize = new Size(360, 0), Anchor = AnchorStyles.Left, Margin = new Padding(3, 0, 3, 3), Visible = false };
    private readonly ErrorProvider _errors = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
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

    public SdEventEditor(SdEditorContext context) : base(context)
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
        _errors.SetIconAlignment(_report, ErrorIconAlignment.MiddleLeft);
        // upozornenie na neznamy typ hlasenia - pod polom, s odkazom na navrhnuty typ
        var infoRow = Table.RowCount++;
        Table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Table.Controls.Add(_reportInfo, 1, infoRow);
        Table.SizeChanged += (_, _) => _reportInfo.MaximumSize = new Size(Math.Max(120, _report.Width), 0);
        _reportInfo.LinkClicked += (_, _) => UseSuggestedReport();
        _report.TextChanged += (_, _) => ValidateReport();
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
        SetRowVisible(_report, HasReport(cls));
        ValidateReport();
        _delayArr.Visible = _delayDep.Visible = cls == "SDEventVlakAttr";
        SetRowVisible(_ctrlId, _hasControl.Checked);
        SetRowVisible(_design, _hasControl.Checked);
    }

    /// <summary>
    /// Typ hlasenia, ktory nie je v Categori.txt: ikona chyby pri poli a pod nim upozornenie, pripadne s odkazom
    /// na typ s rovnakym nazvom (kluc a nazov typu sa lahko zamenia - INISS hlada podla kluca).
    /// </summary>
    private void ValidateReport()
    {
        var rep = _report.Text.Trim();
        var unknown = HasReport(SdEditorContext.Value(_class) as string) && rep != Resources.FStateDgm_Akcia_BezHlasenia && Context.IsUnknownReport(rep);
        var message = unknown ? string.Format(CultureInfo.CurrentCulture, Resources.FStateDgm_Akcia_ReportNeznamy, rep) : "";
        _errors.SetError(_report, message);
        _reportInfo.Visible = unknown;
        var suggested = unknown ? Context.SuggestReport(rep) : null;
        _reportInfo.Tag = suggested?.Key;
        if (!unknown) return;

        if (suggested == null)
        {
            _reportInfo.Text = message;
            _reportInfo.LinkArea = new LinkArea(0, 0);
            return;
        }

        var link = string.Format(CultureInfo.CurrentCulture, Resources.FStateDgm_Akcia_ReportNavrh, suggested.Key, suggested.Name.Trim());
        // oblast odkazu sa rata v texte, z ktoreho LinkLabel vynecha \r - preto len \n
        _reportInfo.Text = message + "\n" + link;
        _reportInfo.LinkArea = new LinkArea(message.Length + 1, link.Length);
    }

    /// <summary>Prepise neznamy typ hlasenia navrhnutym (odkaz pod polom); bez navrhu nerobi nic.</summary>
    internal void UseSuggestedReport()
    {
        if (_reportInfo.Tag is string key) _report.Text = key;
    }

    /// <summary>Trieda akcie, ktora moze spustit hlasenie (<c>ReportKey</c>).</summary>
    private static bool HasReport(string? cls) => cls is "SDEventUniPos" or "SDEventReportAboutState" or "SDEventVlakAttr";

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
            _report.Items.AddRange(Context.ReportKeys.Cast<object>().ToArray());
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
        // podla triedy, nie podla _report.Visible - Apply sa vola az po zatvoreni dialogu, ked je skryte cele okno
        e.ReportKey = !HasReport(e.Class) || rep.Length == 0 || rep == Resources.FStateDgm_Akcia_BezHlasenia ? null : rep;
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
