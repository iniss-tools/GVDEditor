using System.Globalization;
using GVDEditor.Config;
using GVDEditor.Integration;
using GVDEditor.Properties;
using ToolsCore.Iniss.Registry;
using ToolsCore.Tools;

namespace GVDEditor.UI.InissSettings;

/// <summary>
/// Presmerovanie liniek INISSu na simulator tabul: ktore linky, kam (adresa a prvy port pre INISS) a ci v simulatore
/// zalozit linky a tabule stanice. Samotny zapis .INI a volanie simulatora robi okno Nastavenia INISSu.
/// </summary>
internal partial class FSimulatorRedirect : Form
{
    // posledny port musi zostat platny aj pre linku 100
    private const int MaxBasePort = 65535 - 100;

    private readonly IReadOnlyList<RedirectedLine> _redirected;
    private readonly CancellationTokenSource _closing = new();
    private IReadOnlyList<int>? _supported;
    private bool _checksTouched;
    private bool _filling;

    /// <summary>
    /// Vytvori okno.
    /// </summary>
    /// <param name="lines">linky konfiguracie s tabulami, ktore im INISS priradi</param>
    /// <param name="redirected">linky, ktore .INI uz presmerovava</param>
    /// <param name="settings">posledne pouzity simulator</param>
    /// <param name="iniFile">nazov suboru .INI (do uvodu)</param>
    public FSimulatorRedirect(DriverLineMap lines, IReadOnlyList<RedirectedLine> redirected, TableSimulatorSettings settings, string iniFile)
    {
        _redirected = redirected;
        InitializeComponent();
        this.ApplyThemeAndFonts();
        dgvLines.AutoGenerateColumns = false;
        lIntro.Text = string.Format(CultureInfo.CurrentCulture, Resources.InissRedirect_Intro, iniFile);
        tbHost.Text = settings.Host;
        tbBasePort.Text = settings.BasePort.ToString(CultureInfo.CurrentCulture);
        tbUrl.Text = settings.WebUrl;
        tbApiKey.Text = settings.ApiKey;
        cboxPrepare.Checked = settings.Prepare;
        bUndo.Visible = redirected.Count > 0;

        _filling = true;
        foreach (var line in lines.Lines)
        {
            var index = dgvLines.Rows.Add();
            var row = dgvLines.Rows[index];
            row.Tag = line;
            var selectable = line is { Active: true, Line: not null };
            row.Cells[cDo.Index].Value = selectable && (redirected.Count > 0 ? IsRedirected(line) : true);
            row.Cells[cDo.Index].ReadOnly = !selectable;
            row.Cells[cLine.Index].Value = line.Line?.ToString(CultureInfo.CurrentCulture) ?? "?";
            row.Cells[cSection.Index].Value = line.Section;
            row.Cells[cClass.Index].Value = DriverClasses.Find(line.Class)?.ToString() ?? line.Class.ToString(CultureInfo.CurrentCulture);
            row.Cells[cTables.Index].Value = line.Tables.Count.ToString(CultureInfo.CurrentCulture);
            row.Cells[cPort.Index].Value = line.Port;
            if (!selectable) row.DefaultCellStyle.ForeColor = SystemColors.GrayText;
        }

        _filling = false;
        UpdateRows();

        dgvLines.CurrentCellDirtyStateChanged += (_, _) =>
        {
            if (dgvLines.IsCurrentCellDirty) dgvLines.CommitEdit(DataGridViewDataErrorContexts.Commit);
        };
        dgvLines.CellValueChanged += (_, e) =>
        {
            if (_filling || e.ColumnIndex != cDo.Index) return;
            _checksTouched = true;
            UpdateRows();
        };
        tbHost.TextChanged += (_, _) => UpdateRows();
        tbBasePort.TextChanged += (_, _) => UpdateRows();
        bProbe.Click += async (_, _) => await ProbeAsync();
        bApply.Click += (_, _) => Apply();
        bUndo.Click += (_, _) =>
        {
            Undo = true;
            DialogResult = DialogResult.OK;
        };
        tlpMain.SizeChanged += (_, _) => FitIntro();
        Shown += async (_, _) => await ProbeAsync();
        FormClosed += (_, _) =>
        {
            _closing.Cancel();
            _closing.Dispose();
        };
        FitIntro();
    }

    /// <summary>Simulator, ako ho obsluha zadala (na ulozenie do nastaveni).</summary>
    public TableSimulatorSettings Settings => new()
    {
        Host = tbHost.Text.Trim(),
        BasePort = BasePort() ?? TableSimulatorSettings.DefaultBasePort,
        WebUrl = tbUrl.Text.Trim(),
        Prepare = cboxPrepare.Checked,
        ApiKey = tbApiKey.Text.Trim()
    };

    /// <summary>Zrusit cele presmerovanie (namiesto <see cref="Ports" />).</summary>
    public bool Undo { get; private set; }

    /// <summary>Oznacene linky: sekcia → port na simulatore.</summary>
    public IReadOnlyDictionary<string, int> Ports { get; private set; } = new Dictionary<string, int>();

    /// <summary>Webova adresa simulatora, ak sa v nom maju zalozit linky a tabule; inak null.</summary>
    public Uri? PrepareUrl => cboxPrepare.Checked ? TableSimulatorClient.ParseUrl(tbUrl.Text) : null;

    private bool IsRedirected(DriverLine line) => _redirected.Any(r => string.Equals(r.Section, line.Section, StringComparison.OrdinalIgnoreCase));

    private int? BasePort() =>
        int.TryParse(tbBasePort.Text.Trim(), NumberStyles.None, CultureInfo.CurrentCulture, out var port) && port is >= 1 and <= MaxBasePort ? port : null;

    private void FitIntro() =>
        lIntro.MaximumSize = new Size(Math.Max(LogicalToDeviceUnits(200), tlpMain.ClientSize.Width - tlpMain.Padding.Horizontal - LogicalToDeviceUnits(8)), 0);

    /// <summary>Novy TablePort a poznamky podla oznacenia, adresy a tried, ktore simulator pozna.</summary>
    private void UpdateRows()
    {
        var host = tbHost.Text.Trim();
        var basePort = BasePort();
        foreach (DataGridViewRow row in dgvLines.Rows)
        {
            if (row.Tag is not DriverLine line) continue;
            var on = row.Cells[cDo.Index].Value is true;
            row.Cells[cNew.Index].Value = on && line.Line is { } n && basePort is { } b && host.Length > 0
                ? SimulatorRedirect.TablePort(n, host, b + n)
                : "";
            var notes = new List<string>();
            if (!line.Active) notes.Add(Resources.InissRedirect_NoteInactive);
            if (IsRedirected(line)) notes.Add(Resources.InissRedirect_NoteRedirected);
            if (_supported is not null && !_supported.Contains(line.Class)) notes.Add(Resources.InissRedirect_NoteUnsupported);
            row.Cells[cNote.Index].Value = string.Join(", ", notes);
        }
    }

    /// <summary>Zisti, ci simulator bezi a ake triedy liniek pozna; bez zasahu obsluhy odznaci linky, ktore by nezobrazil.</summary>
    private async Task ProbeAsync()
    {
        if (TableSimulatorClient.ParseUrl(tbUrl.Text) is not { } url)
        {
            lStatus.ForeColor = GVDEditor.UI.Settings.SettingsWindow.ProblemColor(this);
            lStatus.Text = Resources.InissRedirect_UrlInvalid;
            return;
        }

        bProbe.Enabled = false;
        lStatus.ResetForeColor();
        lStatus.Text = Resources.InissRedirect_StatusChecking;
        try
        {
            var probe = await TableSimulatorClient.ProbeAsync(url, tbApiKey.Text, TimeSpan.FromSeconds(2), _closing.Token);
            if (IsDisposed) return;
            var state = probe.State;
            _supported = state?.SupportedClasses;
            var running = state is null
                ? ""
                : string.Format(CultureInfo.CurrentCulture, Resources.InissRedirect_StatusRunning,
                    string.Join(", ", state.SupportedClasses.Select(c => c.ToString(CultureInfo.CurrentCulture))));
            lStatus.Text = probe.Access switch
            {
                SimulatorAccess.Offline => string.Format(CultureInfo.CurrentCulture, Resources.InissRedirect_StatusOffline, url),
                SimulatorAccess.Unauthorized => string.IsNullOrWhiteSpace(tbApiKey.Text)
                    ? Resources.InissRedirect_StatusNeedsKey
                    : Resources.InissRedirect_StatusKeyInvalid,
                SimulatorAccess.ReadOnly => running + " " + Resources.InissRedirect_StatusReadOnly,
                SimulatorAccess.HttpsRequired => string.Format(CultureInfo.CurrentCulture, Resources.InissRedirect_StatusHttpsRequired, probe.HttpsUrl),
                SimulatorAccess.UntrustedCertificate => string.Format(CultureInfo.CurrentCulture, Resources.InissRedirect_StatusUntrusted,
                    new Uri(url, "api/https/certificate")),
                _ => running
            };
            if (probe.Access is not (SimulatorAccess.Ok or SimulatorAccess.Offline))
                lStatus.ForeColor = GVDEditor.UI.Settings.SettingsWindow.ProblemColor(this);
            if (_supported is not null && !_checksTouched && _redirected.Count == 0)
            {
                _filling = true;
                foreach (DataGridViewRow row in dgvLines.Rows)
                    if (row.Tag is DriverLine line && !_supported.Contains(line.Class))
                        row.Cells[cDo.Index].Value = false;
                _filling = false;
            }

            UpdateRows();
        }
        catch (OperationCanceledException)
        {
            // okno sa zatvorilo
        }
        finally
        {
            if (!IsDisposed) bProbe.Enabled = true;
        }
    }

    private void Apply()
    {
        var host = tbHost.Text.Trim();
        string? error = null;
        if (host.Length == 0 || host.Any(c => char.IsWhiteSpace(c) || c is '/' or '\\'))
            error = Resources.InissRedirect_HostInvalid;
        else if (BasePort() is null)
            error = string.Format(CultureInfo.CurrentCulture, Resources.InissRedirect_PortInvalid, MaxBasePort);
        else if (cboxPrepare.Checked && PrepareUrl is null)
            error = Resources.InissRedirect_UrlInvalid;

        var ports = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (DataGridViewRow row in dgvLines.Rows)
            if (row.Tag is DriverLine { Line: { } n } line && row.Cells[cDo.Index].Value is true && BasePort() is { } b)
                ports[line.Section] = b + n;
        if (error is null && ports.Count == 0)
            error = Resources.InissRedirect_NothingSelected;

        if (error is not null)
        {
            lStatus.Text = error;
            lStatus.ForeColor = GVDEditor.UI.Settings.SettingsWindow.ProblemColor(this);
            return;
        }

        Ports = ports;
        DialogResult = DialogResult.OK;
    }
}
