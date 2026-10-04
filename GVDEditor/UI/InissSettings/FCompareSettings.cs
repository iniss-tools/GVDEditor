using System.Globalization;
using GVDEditor.Properties;
using ToolsCore.Iniss.Registry;
using ToolsCore.Tools;

namespace GVDEditor.UI.InissSettings;

/// <summary>
/// Porovnanie nastaveni INISSu s inou vetvou registra alebo so suborom .reg/.INI (export, zaloha) a prevzatie
/// vybranych hodnot. Okno nic nezapisuje - vrati zmeny (<see cref="Changes" />), ktore zapise okno Nastavenia INISSu.
/// </summary>
internal partial class FCompareSettings : Form
{
    private readonly IDialogService _dialogs;
    private readonly ResolvedConfig _current;
    private readonly HashSet<string> _checked = new(StringComparer.OrdinalIgnoreCase);
    private ResolvedConfig? _other;
    private List<RegDifference> _differences = [];
    private bool _filling;

    /// <summary>
    /// Vytvori okno pre aktualnu konfiguraciu.
    /// </summary>
    /// <param name="current">zobrazena konfiguracia v okne Nastavenia INISSu</param>
    /// <param name="canWriteIni">konfiguracia ma program - prevzate hodnoty sa daju zapisat aj do jeho .INI</param>
    /// <param name="dialogs">dialogy</param>
    public FCompareSettings(ResolvedConfig current, bool canWriteIni, IDialogService dialogs)
    {
        _current = current;
        _dialogs = dialogs;
        InitializeComponent();
        dgvDiff.AutoGenerateColumns = false;
        this.ApplyThemeAndFonts();
        dgvDiff.BackgroundColor = dgvDiff.DefaultCellStyle.BackColor.IsEmpty ? SystemColors.Window : dgvDiff.DefaultCellStyle.BackColor;

        cbOther.Items.AddRange(InissRegistry.AppNames().Where(a => !string.Equals(a, current.Source.AppName, StringComparison.OrdinalIgnoreCase))
            .Cast<object>().ToArray());
        cbTarget.Items.Add(Resources.InissCompare_TargetRegistry);
        if (canWriteIni) cbTarget.Items.Add(Resources.InissSettings_Target_Ini);
        cbTarget.SelectedIndex = 0;

        cbOther.SelectedIndexChanged += (_, _) =>
        {
            if (_filling || cbOther.SelectedItem is not string app) return;
            ShowOther(LoadBranch(app), string.Format(CultureInfo.CurrentCulture, Resources.InissCompare_InfoBranch, app), false);
        };
        bFile.Click += (_, _) => ChooseFile();
        cboxDifferentOnly.CheckedChanged += (_, _) => FillGrid();
        cboxOtherExplicit.CheckedChanged += (_, _) => FillGrid();
        cboxHideApp.CheckedChanged += (_, _) => FillGrid();
        dgvDiff.CurrentCellDirtyStateChanged += (_, _) =>
        {
            if (dgvDiff.IsCurrentCellDirty) dgvDiff.CommitEdit(DataGridViewDataErrorContexts.Commit);
        };
        dgvDiff.CellValueChanged += (_, e) =>
        {
            if (_filling || e.RowIndex < 0 || e.ColumnIndex != cTake.Index || dgvDiff.Rows[e.RowIndex].Tag is not RegDifference d) return;
            if (dgvDiff.Rows[e.RowIndex].Cells[cTake.Index].Value is true) _checked.Add(Key(d));
            else _checked.Remove(Key(d));
            UpdateCount();
        };
        bTakeOver.Click += (_, _) => TakeOver();
        lInfo.Text = Resources.InissCompare_InfoNone;
        UpdateCount();
    }

    /// <summary>Zmeny, ktore prevezmu oznacene hodnoty (po OK).</summary>
    public List<RegChange> Changes { get; private set; } = [];

    /// <summary>Opis druhej konfiguracie (do suhrnu pred zapisom).</summary>
    public string OtherDescription { get; private set; } = "";

    private static string Key(RegDifference d) => d.Section + "\\" + d.Name;

    /// <summary>
    /// Nacita subor .reg alebo .INI ako druhu konfiguraciu. Pri <paramref name="import" /> zobrazi len hodnoty zapisane
    /// v subore a vsetky oznaci (import zo suboru).
    /// </summary>
    /// <returns><see langword="false" />, ak sa subor neda precitat alebo v nom nie su nastavenia INISSu.</returns>
    public bool LoadFile(string path, bool import)
    {
        ResolvedConfig other;
        string info;
        try
        {
            if (string.Equals(Path.GetExtension(path), ".reg", StringComparison.OrdinalIgnoreCase))
            {
                var branches = RegFile.Load(path);
                if (branches.Count == 0)
                {
                    _dialogs.ShowWarning(Resources.InissCompare_NoSettingsInFile);
                    return false;
                }

                var source = RegTools.SourceFromRegFile(branches, _current.Source);
                other = RegResolver.Resolve(source);
                info = string.Format(CultureInfo.CurrentCulture, Resources.InissCompare_InfoRegFile, Path.GetFileName(path), source.AppName);
            }
            else
            {
                var ini = InissIniFile.Load(path) ?? InissIniFile.Empty();
                if (ini.SectionNames.Count == 0)
                {
                    _dialogs.ShowWarning(Resources.InissCompare_NoSettingsInFile);
                    return false;
                }

                other = RegResolver.Resolve(RegTools.SourceFromIni(ini, _current.Source));
                info = string.Format(CultureInfo.CurrentCulture, Resources.InissCompare_InfoIniFile, Path.GetFileName(path));
            }
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException or FormatException)
        {
            _dialogs.ShowError(string.Format(CultureInfo.CurrentCulture, Resources.InissCompare_FileError, e.Message));
            return false;
        }

        _filling = true;
        cbOther.SelectedIndex = -1;
        _filling = false;
        if (import) cboxOtherExplicit.Checked = true;
        ShowOther(other, info, import);
        return true;
    }

    private ResolvedConfig LoadBranch(string app)
    {
        var t = _current.Source;
        return RegResolver.Resolve(new InissConfigSource
        {
            AppName = app, Version = t.Version, RunMode = t.RunMode, ColorNames = t.ColorNames, Tables = t.Tables,
            User = InissRegistry.Load(RegLocation.User, app),
            Machine = InissRegistry.Load(RegLocation.Machine, app),
            VirtualStore = InissRegistry.Load(RegLocation.VirtualStore, app)
        });
    }

    private void ChooseFile()
    {
        using var dialog = new OpenFileDialog { Filter = Resources.InissCompare_FileFilter, Title = Resources.InissCompare_FileTitle };
        if (dialog.ShowDialog(this) == DialogResult.OK)
            LoadFile(dialog.FileName, false);
    }

    private void ShowOther(ResolvedConfig other, string info, bool checkAll)
    {
        _other = other;
        OtherDescription = info;
        lInfo.Text = info;
        _differences = RegTools.Compare(_current, other);
        _checked.Clear();
        if (checkAll)
            foreach (var d in _differences.Where(d => !d.IsEqual && d.OtherExplicit && IsShown(d)))
                _checked.Add(Key(d));
        FillGrid();
    }

    private void FillGrid()
    {
        _filling = true;
        dgvDiff.SuspendLayout();
        try
        {
            dgvDiff.Rows.Clear();
            foreach (var d in _differences)
            {
                if (cboxDifferentOnly.Checked && d.IsEqual) continue;
                if (cboxOtherExplicit.Checked && !d.OtherExplicit) continue;
                var setting = (d.Current ?? d.Other)!.Setting;
                // rozlozenie okna a pocitadla si INISS prepisuje sam - pri porovnani len prekazaju
                if (cboxHideApp.Checked && setting.Write is RegWriteMode.App or RegWriteMode.AutoAndApp) continue;
                var index = dgvDiff.Rows.Add(_checked.Contains(Key(d)) && !d.IsEqual, Key(d),
                    d.Current is null ? Resources.InissCompare_Missing : InissSettingsModel.Format(d.Current.Value, setting),
                    d.Current is null ? "" : InissSettingsModel.SourceText(d.Current.Source, true),
                    d.Other is null ? Resources.InissCompare_Missing : InissSettingsModel.Format(d.Other.Value, setting),
                    d.Other is null ? "" : InissSettingsModel.SourceText(d.Other.Source, true));
                var row = dgvDiff.Rows[index];
                row.Tag = d;
                // rovnaku hodnotu nie je co prevziat
                row.Cells[cTake.Index].ReadOnly = d.IsEqual;
                if (d.IsEqual) row.DefaultCellStyle.ForeColor = SystemColors.GrayText;
            }
        }
        finally
        {
            dgvDiff.ResumeLayout();
            _filling = false;
        }

        UpdateCount();
    }

    private List<RegDifference> Selected() => _differences.Where(d => !d.IsEqual && _checked.Contains(Key(d)) && IsShown(d)).ToList();

    private bool IsShown(RegDifference d) =>
        !cboxHideApp.Checked || (d.Current ?? d.Other)!.Setting.Write is not (RegWriteMode.App or RegWriteMode.AutoAndApp);

    private void UpdateCount()
    {
        var differences = _differences.Count(d => !d.IsEqual && IsShown(d));
        var selected = Selected().Count;
        lCount.Text = _other is null ? "" : string.Format(CultureInfo.CurrentCulture, Resources.InissCompare_Count, differences, selected);
        bTakeOver.Enabled = selected > 0;
    }

    private void TakeOver()
    {
        var selected = Selected();
        if (selected.Count == 0) return;
        var target = cbTarget.SelectedIndex == 1 ? RegWriteTarget.Ini : RegWriteTarget.Registry;
        Changes = RegTools.TakeOver(selected, target);
        DialogResult = DialogResult.OK;
    }
}
