using System.Globalization;
using ExControls;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.UI.Settings;

/// <summary>
///     Stranka Grafikon v okne Lokalne nastavenia - obdobia platnosti, stanica a priecinok grafikonu. Udaje sa do
///     grafikonu zapisu az po OK (<see cref="Apply" />), priecinok sa vtedy aj premenuje (<see cref="RenamePendingDir" />).
/// </summary>
public partial class GrafikonPage : UserControl, ISettingsPage
{
    private readonly List<(Control Control, string Text)> _problems = [];
    private GVDDirectory _dir = null!;
    private Color _hintColor;
    private bool _loading;

    /// <summary>
    ///     Novy nazov priecinka grafikonu, ktory tlacidlo Premenovat overilo; priecinok sa premenuje az pri OK.
    /// </summary>
    private string? _pendingDirName;

    /// <summary>
    ///     Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public GrafikonPage()
    {
        InitializeComponent();
    }

    /// <inheritdoc />
    public event EventHandler? ProblemsChanged;

    /// <inheritdoc />
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? FirstProblem => _problems.Count == 0 ? null : _problems[0].Text;

    /// <inheritdoc />
    public void FocusFirstProblem()
    {
        if (_problems.Count > 0)
            _problems[0].Control.Focus();
    }

    /// <summary>
    ///     Naplni stranku udajmi grafikonu - volat az po nastaveni temy okna.
    /// </summary>
    public void LoadData(GVDDirectory dir)
    {
        _dir = dir;
        foreach (var header in new[] { lPeriod, lStation, lFiles })
            header.Font = new Font(Font, FontStyle.Bold);
        lDirNote.ForeColor = SystemColors.GrayText;
        _hintColor = lHint.ForeColor;
        if (GlobData.UsingStyle.DarkScrollBar)
            pScroll.SetTheme(WindowsTheme.DarkExplorer);

        _loading = true;
        tbDir.Text = dir.Dir.FullPath;
        dtpGVDOd.Value = dir.GVD.StartValidTimeTable;
        dtpGVDDo.Value = dir.GVD.EndValidTimeTable;
        dtpDataOd.Value = dir.GVD.StartValidData;
        dtpDataDo.Value = dir.GVD.EndValidData;

        cbStationName.DataSource = GlobData.Stations;
        cbCustomStation.Checked = dir.GVD.ThisStation.IsCustom;
        if (cbCustomStation.Checked)
        {
            nudIDStation.Value = Math.Clamp(int.Parse(dir.GVD.ThisStation.ID, CultureInfo.InvariantCulture), nudIDStation.Minimum,
                nudIDStation.Maximum);
            tbGVDStationName.Text = dir.GVD.ThisStation.Name;
        }
        else
        {
            cbStationName.SelectedItem = dir.GVD.ThisStation;
        }

        tbDirName.Text = dir.Dir.DirName;
        _loading = false;

        UpdateStationFields();
        Check();
    }

    private void UpdateStationFields()
    {
        cbStationName.Enabled = !cbCustomStation.Checked;
        nudIDStation.Enabled = tbGVDStationName.Enabled = cbCustomStation.Checked;
    }

    /// <summary>
    ///     Navrhovany nazov priecinka podla stanice a roku zaciatku platnosti dat (priecinok sa premenuje len tlacidlom).
    /// </summary>
    private void SuggestDirName()
    {
        if (_loading)
            return;

        var station = cbCustomStation.Checked ? tbGVDStationName.Text : cbStationName.SelectedItem?.ToString();
        tbDirName.Text = station + @"." + dtpDataOd.Value.Year;
    }

    private void Period_Changed(object? sender, EventArgs e)
    {
        if (!_loading)
            Check();
    }

    private void dtpDataOd_ValueChanged(object? sender, EventArgs e)
    {
        SuggestDirName();
        Period_Changed(sender, e);
    }

    private void cbStationName_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        SuggestDirName();
        Period_Changed(sender, e);
    }

    private void cbCustomStation_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateStationFields();
        SuggestDirName();
        Period_Changed(sender, e);
    }

    private void tbGVDStationName_TextChanged(object? sender, EventArgs e)
    {
        SuggestDirName();
        Period_Changed(sender, e);
    }

    private void Station_Changed(object? sender, EventArgs e) => Period_Changed(sender, e);

    private void Check()
    {
        _problems.Clear();
        if (dtpGVDDo.Value.Date <= dtpGVDOd.Value.Date)
            _problems.Add((dtpGVDDo, Resources.FNewGrafikon_Čas_konca_platnosti_grafikonu_má_byť_neskôr_ako_začiatok_platnosti));
        if (dtpDataDo.Value.Date <= dtpDataOd.Value.Date)
            _problems.Add((dtpDataDo, Resources.FNewGrafikon_Čas_konca_platnosti_dát_má_byť_neskôr_ako_začiatok_platnosti));

        if (!cbCustomStation.Checked)
        {
            if (cbStationName.SelectedItem is not Station)
                _problems.Add((cbStationName, Resources.FNewGrafikon_Nie_je_vybratá_stanica));
        }
        else if (string.IsNullOrWhiteSpace(tbGVDStationName.Text))
        {
            _problems.Add((tbGVDStationName, Resources.FNewGrafikon_Nie_je_zadaná_žiadna_stanica));
        }
        else
        {
            var id = decimal.ToInt32(nudIDStation.Value).ToString(CultureInfo.InvariantCulture);
            if (GlobData.Stations.Concat(GlobData.CustomStations).Any(station => station.ID == id))
                _problems.Add((nudIDStation, Resources.FNewGrafikon_Zadané_ID_vlastnej_stanice_už_patrí_inej_stanici));
        }

        lHint.Text = string.Join(Environment.NewLine, _problems.Select(p => p.Text));
        lHint.ForeColor = _problems.Count > 0 ? SettingsWindow.ProblemColor(lHint) : _hintColor;
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void bOpenDir_Click(object? sender, EventArgs e) => Utils.OpenShell(_dir.Dir.FullPath);

    private void bDirChange_Click(object? sender, EventArgs e)
    {
        var dirname = tbDirName.Text.Trim();
        tbDirName.Text = dirname;

        var error = GVDDirRename.Validate(dirname, _dir.Dir.FullPath, GlobData.DataDir, out var fullpath);
        if (error != null)
        {
            Utils.ShowError(error);
            return;
        }

        // nezmeneny nazov - nie je co presuvat, pripadne skorsie naplanovanie sa rusi
        _pendingDirName = string.Equals(fullpath, _dir.Dir.FullPath, StringComparison.Ordinal) ? null : dirname;
        tbDir.Text = fullpath;
        lDirNote.Text = _pendingDirName is null
            ? Resources.GrafikonPage_Priecinok_bez_zmeny
            : string.Format(CultureInfo.CurrentCulture, Resources.GrafikonPage_Priecinok_po_OK, dirname);
    }

    /// <summary>
    ///     Premenuje priecinok grafikonu na nazov naplanovany tlacidlom Premenovat a zapise <c>DirList.TXT</c>.
    /// </summary>
    /// <returns><see langword="false" />, ak sa premenovanie nepodarilo a dialog ma ostat otvoreny.</returns>
    public bool RenamePendingDir()
    {
        if (_pendingDirName == null) return true;

        var dirname = _pendingDirName;
        var oldFullPath = _dir.Dir.FullPath;

        // od kliknutia na Premenovat mohol na disku vzniknut priecinok s rovnakym nazvom
        var error = GVDDirRename.Validate(dirname, oldFullPath, GlobData.DataDir, out var fullpath);
        if (error != null)
        {
            Utils.ShowError(error);
            return false;
        }

        if (!string.Equals(fullpath, oldFullPath, StringComparison.Ordinal))
        {
            try
            {
                Directory.Move(oldFullPath, fullpath);
            }
            catch (Exception exception)
            {
                Log.Exception(exception);
                Utils.ShowError(string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_Priečinok_grafikonu_sa_nepodarilo_premenovať,
                    dirname, exception.Message));
                return false;
            }

            // GlobData.GVDDirs obsahuje vsetky zaznamy DirList.TXT (aj grafikony inych stanic a necitatelne),
            // FMain.ObdobiaList len obdobia prave vybratej stanice
            GVDDirRename.UpdateEntries(_dir.Dir, GlobData.GVDDirs, dirname, fullpath);
            TxtParser.WriteDirList(GlobData.GVDDirs);
        }

        _pendingDirName = null;
        return true;
    }

    /// <summary>
    ///     Zapise obdobia platnosti a stanicu do grafikonu (po OK).
    /// </summary>
    public void Apply()
    {
        var gvdInfo = _dir.GVD;
        gvdInfo.StartValidData = dtpDataOd.Value.Date;
        gvdInfo.EndValidData = dtpDataDo.Value.Date;
        gvdInfo.StartValidTimeTable = dtpGVDOd.Value.Date;
        gvdInfo.EndValidTimeTable = dtpGVDDo.Value.Date;

        gvdInfo.ThisStation = cbCustomStation.Checked
            ? new Station(decimal.ToInt32(nudIDStation.Value).ToString(CultureInfo.InvariantCulture), tbGVDStationName.Text, IsCustom: true)
            : (Station)cbStationName.SelectedItem!;
    }
}
