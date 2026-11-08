using System.Globalization;
using GVDEditor.Config;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Integration.Iniss2;
using GVDEditor.Properties;
using GVDEditor.UI.Settings;
using Serilog;
using ToolsCore.Iniss.Registry;
using ToolsCore.Tools;

namespace GVDEditor.UI.Export;

/// <summary>
/// Sprievodca jednorazovym exportom grafikonov INISS 3.39 do INISS2: grafikony, ciel, sablona hlaseni, linky a banky pisem
/// tabul ELEN a suhrn upozorneni. Prevod a zapis robi <see cref="Iniss2ExportService" />.
/// </summary>
internal partial class FIniss2Export : Form
{
    private const int SummaryPage = 4;

    private readonly EditorContext _ctx;
    private readonly Func<string, string?, InissRunMode, IReadOnlyDictionary<int, RegTableInfo>?, InissConfigSource>? _loadSource;
    private readonly TableLayoutPanel[] _pages;
    private readonly string[] _titles;
    private readonly string[] _hints;
    private readonly string _fontsText;
    private readonly List<string> _addedFonts = [];
    private List<string> _fontFiles = [];
    private List<GVDDirectory> _loaded = [];
    private List<GrafikonDocument> _documents = [];
    private string? _dataDir;
    private Iniss2ExportPlan? _plan;
    private Iniss2ExportData? _data;
    private int _page;
    private bool _busy;

    /// <summary>
    /// Vytvori sprievodcu.
    /// </summary>
    /// <param name="ctx">kontext editora (instalacia, otvoreny grafikon, konfiguracie spustania)</param>
    /// <param name="grafikons">grafikony instalacie</param>
    /// <param name="loadSource">nacitanie nastaveni INISSu pre linky tabul; null = register a .INI tohto pocitaca - inak napr. ukazkova
    /// konfiguracia pre snimky dokumentacie</param>
    public FIniss2Export(EditorContext ctx, IReadOnlyList<GVDDirectory> grafikons,
        Func<string, string?, InissRunMode, IReadOnlyDictionary<int, RegTableInfo>?, InissConfigSource>? loadSource = null)
    {
        _ctx = ctx;
        _loadSource = loadSource;
        InitializeComponent();
        this.ApplyThemeAndFonts();
        lStep.Font = new Font(lStep.Font, FontStyle.Bold);
        foreach (var grid in new[] { dgvStations, dgvReports, dgvFonts, dgvWarnings })
        {
            grid.AutoGenerateColumns = false;
            grid.DataError += (_, e) => e.ThrowException = false;
        }

        colWarning.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvWarnings.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        _pages = [tlpGrafikons, tlpTarget, tlpReports, tlpLines, tlpSummary];
        _titles = [Resources.Iniss2Export_Step1, Resources.Iniss2Export_Step2, Resources.Iniss2Export_Step3, Resources.Iniss2Export_Step4,
            Resources.Iniss2Export_Step5];
        _hints = [Resources.Iniss2Export_Step1Hint, Resources.Iniss2Export_Step2Hint, Resources.Iniss2Export_Step3Hint, Resources.Iniss2Export_Step4Hint,
            Resources.Iniss2Export_Step5Hint];
        _fontsText = lFonts.Text;

        foreach (var dir in grafikons)
            clbGrafikons.Items.Add(new GrafikonItem(dir), ctx.Directory is { } open && SameDirectory(dir, open));

        tbInstallation.Text = ctx.Config.Iniss2ExportDir;
        cbConfiguration.Items.Add(Resources.Iniss2Export_NoLines);
        foreach (var configuration in ctx.RunConfigurations.Items)
            cbConfiguration.Items.Add(new ConfigurationItem(configuration));
        var selected = ctx.RunConfigurations.Selected;
        cbConfiguration.SelectedIndex = selected is null ? 0 : ctx.RunConfigurations.Items.IndexOf(selected) + 1;
        UpdateInstallationInfo();

        bBack.Click += (_, _) => ShowPage(_page - 1);
        bNext.Click += async (_, _) => await NextAsync();
        bBrowse.Click += (_, _) => BrowseInstallation();
        bAddFont.Click += (_, _) => AddFont();
        tbInstallation.TextChanged += (_, _) => UpdateInstallationInfo();
        cbTemplate.SelectedIndexChanged += (_, _) => FillReports();
        cbConfiguration.SelectedIndexChanged += (_, _) => UpdateLinesInfo();
        FormClosing += (_, e) => e.Cancel = _busy;
        ShowPage(0);
    }

    private Iniss2TemplateStation? Template => cbTemplate.SelectedItem as Iniss2TemplateStation;

    private static bool SameDirectory(GVDDirectory a, GVDDirectory b) => string.Equals(a.Dir.FullPath, b.Dir.FullPath, StringComparison.OrdinalIgnoreCase);

    private void ShowPage(int page)
    {
        _page = page;
        for (var i = 0; i < _pages.Length; i++)
            _pages[i].Visible = i == page;

        lStep.Text = string.Format(CultureInfo.CurrentCulture, Resources.Iniss2Export_Step, page + 1, _pages.Length, _titles[page]);
        lHint.Text = _hints[page];
        bBack.Enabled = page > 0;
        bNext.Text = page == SummaryPage ? Resources.Iniss2Export_Export : Resources.Iniss2Export_Next;
    }

    private async Task NextAsync()
    {
        if (_busy)
            return;

        if (_page == SummaryPage)
        {
            await ExportAsync();
            return;
        }

        var ok = _page switch
        {
            0 => await LeaveGrafikonsAsync(),
            1 => LeaveTarget(),
            3 => await BuildAsync(),
            _ => true
        };
        if (ok)
            ShowPage(_page + 1);
    }

    private void SetBusy(bool busy)
    {
        _busy = busy;
        UseWaitCursor = busy;
        pnlPages.Enabled = bNext.Enabled = !busy;
        bBack.Enabled = !busy && _page > 0;
    }

    // --- grafikony ---

    private async Task<bool> LeaveGrafikonsAsync()
    {
        var selected = clbGrafikons.CheckedItems.Cast<GrafikonItem>().Select(item => item.Directory).ToList();
        if (selected.Count == 0)
        {
            Utils.ShowWarning(Resources.Iniss2Export_NoGrafikon);
            return false;
        }

        if (selected.Count == _loaded.Count && selected.Zip(_loaded).All(pair => SameDirectory(pair.First, pair.Second)))
            return true;

        SetBusy(true);
        try
        {
            var workspace = _ctx.Workspace;
            var open = _ctx.Directory;
            var document = _ctx.Document;
            _documents = await Task.Run(() => Iniss2ExportService.LoadDocuments(workspace, selected, open, document));
            _loaded = selected;
        }
        catch (Exception e) when (ErrorReport.CatchErrors)
        {
            Log.Error(e, "Grafikony {Grafikons} sa pre export do INISS2 nepodarilo nacitat", selected.Select(dir => dir.Dir.FullPath));
            Utils.ShowError(string.Format(CultureInfo.CurrentCulture, Resources.Iniss2Export_LoadFailed,
                string.Join(", ", selected.Select(dir => dir.PeriodFormatted))) + Environment.NewLine + e.Message);
            return false;
        }
        finally
        {
            SetBusy(false);
        }

        FillStations();
        FillReports();
        FillFonts();
        return true;
    }

    // --- ciel ---

    private void FillStations()
    {
        dgvStations.Rows.Clear();
        for (var i = 0; i < _loaded.Count; i++)
        {
            var dir = _loaded[i];
            var index = dgvStations.Rows.Add(new GrafikonItem(dir).ToString(), Iniss2ExportService.DefaultStationDirectory(i, dir.GVD.ThisStation.Name));
            dgvStations.Rows[index].Tag = dir;
        }

        if (tbGroup.Text.Trim().Length == 0 && _loaded.Count > 0)
            tbGroup.Text = Iniss2ExportService.DefaultGroup(_loaded[0].GVD);
    }

    private void BrowseInstallation()
    {
        using var dialog = new FolderBrowserDialog { Description = Resources.Iniss2Export_SelectInstallation, UseDescriptionForTitle = true };
        if (Directory.Exists(tbInstallation.Text.Trim()))
            dialog.InitialDirectory = tbInstallation.Text.Trim();
        if (dialog.ShowDialog(this) == DialogResult.OK)
            tbInstallation.Text = dialog.SelectedPath;
    }

    private void UpdateInstallationInfo()
    {
        var path = tbInstallation.Text.Trim();
        var dataDir = Iniss2ExportService.DataDirectory(path);
        lInstallationInfo.Text = path.Length == 0 ? ""
            : dataDir is null ? Resources.Iniss2Export_NotInstallation
            : string.Format(CultureInfo.CurrentCulture, Resources.Iniss2Export_Installation, dataDir);
    }

    private List<string> StationDirectories() =>
        dgvStations.Rows.Cast<DataGridViewRow>().Select(row => (row.Cells[colStationDirectory.Index].Value as string ?? "").Trim()).ToList();

    private static bool ValidName(string name) => name.Length > 0 && name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 && name.Trim('.').Length > 0;

    private bool LeaveTarget()
    {
        dgvStations.EndEdit();
        var dataDir = Iniss2ExportService.DataDirectory(tbInstallation.Text.Trim());
        var group = tbGroup.Text.Trim();
        string? error = null;
        if (dataDir is null)
            error = Resources.Iniss2Export_NotInstallation;
        else if (!ValidName(group))
            error = Resources.Iniss2Export_GroupInvalid;
        else if (Iniss2ExportService.GroupExists(dataDir, group))
            error = string.Format(CultureInfo.CurrentCulture, Resources.Iniss2Export_IssueDetail, group, Iniss2ExportTexts.Issue(Iniss2ExportIssue.GroupExists));
        else
            error = StationDirectoriesError();

        if (error is not null)
        {
            Utils.ShowWarning(error);
            return false;
        }

        if (!string.Equals(dataDir, _dataDir, StringComparison.OrdinalIgnoreCase))
        {
            _dataDir = dataDir;
            FillTemplates();
            FillFonts();
        }

        return true;
    }

    private string? StationDirectoriesError()
    {
        var directories = StationDirectories();
        for (var i = 0; i < directories.Count; i++)
        {
            if (!ValidName(directories[i]))
                return string.Format(CultureInfo.CurrentCulture, Resources.Iniss2Export_DirectoryInvalid, _loaded[i].GVD.ThisStation.Name);
        }

        var twice = directories.GroupBy(directory => directory, StringComparer.OrdinalIgnoreCase).FirstOrDefault(group => group.Count() > 1);
        return twice is null ? null : string.Format(CultureInfo.CurrentCulture, Resources.Iniss2Export_DirectoryTwice, twice.Key);
    }

    // --- hlasenia ---

    private void FillTemplates()
    {
        List<Iniss2TemplateStation> templates;
        try
        {
            templates = Iniss2ExportService.TemplateStations(_dataDir!);
        }
        catch (Exception e) when (ErrorReport.CatchErrors)
        {
            // instalacia bez citatelnych stanic jednoducho nema sablonu
            Log.Warning(e, "Stanice instalacie INISS2 {DataDir} sa nepodarilo precitat", _dataDir);
            templates = [];
        }

        cbTemplate.Items.Clear();
        cbTemplate.Items.Add(Resources.Iniss2Export_NoTemplate);
        cbTemplate.Items.AddRange([.. templates]);
        cbTemplate.SelectedIndex = templates.Count > 0 ? 1 : 0;
    }

    private void FillReports()
    {
        var template = Template;
        lReports.Visible = dgvReports.Visible = template is not null;
        dgvReports.Rows.Clear();
        if (template is null)
        {
            lTemplateInfo.Text = Resources.Iniss2Export_NoTemplateInfo;
            return;
        }

        lTemplateInfo.Text = string.Format(CultureInfo.CurrentCulture, Resources.Iniss2Export_TemplateInfo,
            string.Join(", ", template.Reports.Select(report => report.Key)));
        colReport.DataSource = new List<Choice> { new("", Resources.Iniss2Export_NoReport) }
            .Concat(template.Reports.Select(report => new Choice(report.Key, $"{report.Key}  {report.Name}"))).ToList();
        colReport.ValueMember = nameof(Choice.Value);
        colReport.DisplayMember = nameof(Choice.Text);

        var types = _documents.SelectMany(document => document.ReportTypes).DistinctBy(type => type.Key).ToList();
        var letters = Iniss2ExportService.DefaultLetters(types, template.Reports);
        foreach (var type in types)
        {
            var index = dgvReports.Rows.Add($"{type.Char}  {type.Name}", letters[type.Key]);
            dgvReports.Rows[index].Tag = type.Key;
        }
    }

    // --- tabule ---

    private void UpdateLinesInfo() =>
        lLinesInfo.Text = cbConfiguration.SelectedItem is ConfigurationItem ? Resources.Iniss2Export_LinesInfo : Resources.Iniss2Export_NoLinesInfo;

    private void FillFonts()
    {
        var previous = dgvFonts.Rows.Cast<DataGridViewRow>()
            .ToDictionary(row => (string)row.Tag!, row => row.Cells[colFontFile.Index].Value as string ?? "");
        _fontFiles = [.. Iniss2ExportService.FontCandidates(_ctx.Workspace.DataDir, _dataDir).Union(_addedFonts, StringComparer.OrdinalIgnoreCase)];
        SetFontChoices();

        var catalogs = Iniss2ExportService.BankCatalogs(_documents);
        lFonts.Text = catalogs.Count > 0 ? _fontsText : Resources.Iniss2Export_NoFonts;
        dgvFonts.Visible = bAddFont.Visible = catalogs.Count > 0;
        dgvFonts.Rows.Clear();
        foreach (var catalog in catalogs)
        {
            var key = catalog.Key ?? "";
            var file = previous.TryGetValue(key, out var chosen) && _fontFiles.Contains(chosen, StringComparer.OrdinalIgnoreCase) ? chosen
                : _fontFiles.Count == 1 ? _fontFiles[0] : "";
            var index = dgvFonts.Rows.Add(key, file);
            dgvFonts.Rows[index].Tag = key;
        }

        UpdateLinesInfo();
    }

    private void SetFontChoices()
    {
        colFontFile.DataSource = new List<Choice> { new("", Resources.Iniss2Export_NoFont) }
            .Concat(_fontFiles.Select(file => new Choice(file, $"{Path.GetFileName(file)}  ({Path.GetDirectoryName(file)})"))).ToList();
        colFontFile.ValueMember = nameof(Choice.Value);
        colFontFile.DisplayMember = nameof(Choice.Text);
    }

    private void AddFont()
    {
        using var dialog = new OpenFileDialog { Filter = Resources.Iniss2Export_FontFilter };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        var file = dialog.FileName;
        if (!_fontFiles.Contains(file, StringComparer.OrdinalIgnoreCase))
        {
            _addedFonts.Add(file);
            _fontFiles.Add(file);
            dgvFonts.EndEdit();
            SetFontChoices();
        }

        if (dgvFonts.CurrentRow is { } row)
            row.Cells[colFontFile.Index].Value = _fontFiles.First(candidate => string.Equals(candidate, file, StringComparison.OrdinalIgnoreCase));
    }

    // --- suhrn a zapis ---

    private Iniss2ExportPlan Plan()
    {
        dgvReports.EndEdit();
        dgvFonts.EndEdit();
        var letters = dgvReports.Rows.Cast<DataGridViewRow>()
            .ToDictionary(row => (string)row.Tag!, row => row.Cells[colReport.Index].Value as string ?? "");
        var fonts = dgvFonts.Rows.Cast<DataGridViewRow>()
            .Where(row => row.Cells[colFontFile.Index].Value is string { Length: > 0 })
            .ToDictionary(row => (string)row.Tag!, row => row.Cells[colFontFile.Index].Value as string ?? "");
        return new Iniss2ExportPlan(_loaded, _dataDir!, tbGroup.Text.Trim(), StationDirectories(), Template, letters,
            (cbConfiguration.SelectedItem as ConfigurationItem)?.Configuration, fonts);
    }

    private async Task<bool> BuildAsync()
    {
        var plan = Plan();
        var workspace = _ctx.Workspace;
        var documents = _documents;
        var loadSource = _loadSource;
        SetBusy(true);
        try
        {
            var (data, warnings) = await Task.Run(() =>
            {
                var built = Iniss2ExportService.Build(workspace, documents, plan, loadSource);
                return (built, (List<Iniss2ExportWarning>)[.. built.Warnings, .. Iniss2ExportService.Check(plan.DataDir, built)]);
            });
            _plan = plan;
            _data = data;
            FillSummary(plan, warnings);
            return true;
        }
        catch (Iniss2ExportException e)
        {
            Log.Warning("Export do INISS2 sa neda zostavit: {Issue} {Detail}", e.Issue, e.Message);
            Utils.ShowWarning(Iniss2ExportTexts.Error(e));
            return false;
        }
        catch (Exception e) when (ErrorReport.CatchErrors)
        {
            Log.Error(e, "Export grafikonov {Grafikons} do INISS2 sa nepodarilo zostavit", plan.Grafikons.Select(dir => dir.Dir.FullPath));
            Utils.ShowError(Resources.Iniss2Export_Failed + Environment.NewLine + e.Message);
            return false;
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void FillSummary(Iniss2ExportPlan plan, List<Iniss2ExportWarning> warnings)
    {
        var stations = string.Join(", ", plan.Grafikons.Select(dir => dir.GVD.ThisStation.Name));
        lSummary.Text = string.Format(CultureInfo.CurrentCulture, Resources.Iniss2Export_Summary, plan.Group, stations, plan.DataDir) + Environment.NewLine
                        + (warnings.Count == 0 ? Resources.Iniss2Export_NoWarnings
                            : string.Format(CultureInfo.CurrentCulture, Resources.Iniss2Export_Warnings, warnings.Count));
        dgvWarnings.Rows.Clear();
        foreach (var warning in warnings.OrderBy(warning => warning.Issue))
            dgvWarnings.Rows.Add(Iniss2ExportTexts.Issue(warning.Issue), warning.Detail);
    }

    private async Task ExportAsync()
    {
        if (_data is null || _plan is null)
            return;

        var data = _data;
        var plan = _plan;
        SetBusy(true);
        try
        {
            await Task.Run(() => Iniss2ExportService.Write(data, plan));
        }
        catch (Iniss2ExportException e)
        {
            Log.Warning("Export do INISS2 sa neda zapisat: {Issue} {Detail}", e.Issue, e.Message);
            Utils.ShowWarning(Iniss2ExportTexts.Error(e));
            return;
        }
        catch (Exception e) when (ErrorReport.CatchErrors)
        {
            Log.Error(e, "Export grafikonov do INISS2 {DataDir} grafikon {Group} zlyhal", plan.DataDir, plan.Group);
            Utils.ShowError(Resources.Iniss2Export_Failed + Environment.NewLine + e.Message);
            return;
        }
        finally
        {
            SetBusy(false);
        }

        _ctx.Config.Iniss2ExportDir = tbInstallation.Text.Trim();
        SettingsWindow.SaveConfig(_ctx.Config);
        Utils.ShowInfo(string.Format(CultureInfo.CurrentCulture, Resources.Iniss2Export_Done, plan.Group));
        DialogResult = DialogResult.OK;
    }

    // polozka zoznamu grafikonov: stanica, obdobie a priecinok
    private sealed record GrafikonItem(GVDDirectory Directory)
    {
        public override string ToString() => $"{Directory.PeriodFormatted}  ({Directory.Dir.DirName})";
    }

    // konfiguracia spustania v ponuke liniek
    private sealed record ConfigurationItem(RunConfiguration Configuration)
    {
        public override string ToString() => Configuration.Name;
    }

    /// <summary>
    /// Volba stlpca s rozbalovacim zoznamom: hodnota a zobrazeny text.
    /// </summary>
    internal sealed record Choice(string Value, string Text);
}
