using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using GVDEditor.Integration;
using GVDEditor.Properties;
using ToolsCore.Iniss.Registry;
using ToolsCore.Tools;

namespace GVDEditor.UI.InissSettings;

/// <summary>
/// Sprievodca novou linkou k tabuliam: sekcia Driver*, protokol (TableClass) a pripojenie (seriovy port, TCP/UDP
/// s cislom linky, pomenovana rura). Ostatne hodnoty linky ostanu predvolene podla protokolu - zapise sa len
/// TableClass, TablePort a pri seriovom porte TablePortParam, ak sa lisi od predvolby.
/// </summary>
internal partial class FDriverWizard : Form
{
    private enum Connection
    {
        Serial,
        Tcp,
        Udp,
        Pipe
    }

    private readonly DriverLineMap _lines;
    private readonly IReadOnlyList<InissTable> _tables;
    private string _paramsDefault = "";

    /// <summary>
    /// Vytvori sprievodcu.
    /// </summary>
    /// <param name="config">vyhodnotena konfiguracia (obsadene sekcie, vetva zapisu)</param>
    /// <param name="lines">existujuce linky</param>
    /// <param name="tables">fyzicke tabule dat</param>
    /// <param name="serialPorts">seriove porty pocitaca</param>
    /// <param name="registryTarget">text cielu zapisu do registra (HKLM/HKCU)</param>
    /// <param name="iniAvailable">da sa zapisat do suboru .INI</param>
    public FDriverWizard(ResolvedConfig config, DriverLineMap lines, IReadOnlyList<InissTable> tables, IReadOnlyList<string> serialPorts,
        string registryTarget, bool iniAvailable)
    {
        _lines = lines;
        _tables = tables;
        InitializeComponent();
        this.ApplyThemeAndFonts();
        lIntro.MaximumSize = new Size(LogicalToDeviceUnits(540), 0);
        lPreview.MaximumSize = new Size(LogicalToDeviceUnits(540), 0);

        // volne sekcie: Driver0, Driver1… a nakoniec Driver (dvojciferne tvary 00-09 sa neponukaju)
        var used = config.Sections.Select(s => s.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var max = RegCatalog.FindSection("Driver")!.MaxIndex;
        foreach (var name in Enumerable.Range(0, max + 1).Select(i => "Driver" + i.ToString(CultureInfo.InvariantCulture)).Append("Driver"))
            if (!used.Contains(name) && !(name.Length == 7 && used.Contains("Driver0" + name[6])))
                cbSection.Items.Add(name);
        cbSection.SelectedIndex = cbSection.Items.Count > 0 ? 0 : -1;

        foreach (var c in DriverClasses.All) cbClass.Items.Add(c);
        // protokol podla tabul, ktorym zatial INISS nic neposle
        var orphan = lines.Unserved.Select(u => u.Table).FirstOrDefault(t => t.Manufacturer is not null);
        var suggested = orphan?.Manufacturer is { } m ? DriverClasses.All.FirstOrDefault(c => c.Manufacturers.Contains(m)) : null;
        cbClass.SelectedItem = suggested ?? DriverClasses.Find(4);

        cbConnection.Items.AddRange([Resources.InissSettings_Conn_Serial, Resources.InissSettings_Conn_Tcp, Resources.InissSettings_Conn_Udp, Resources.InissSettings_Conn_Pipe]);
        cbConnection.SelectedIndex = 0;

        var usedLines = lines.Lines.Select(l => l.Line).OfType<int>().ToHashSet();
        foreach (var port in serialPorts.Concat(Enumerable.Range(1, 16).Select(i => "COM" + i.ToString(CultureInfo.InvariantCulture)))
                     .Distinct(StringComparer.OrdinalIgnoreCase))
            cbComPort.Items.Add(port);
        var freeLine = DriverLines.FreeLine(lines, tables);
        cbComPort.Text = cbComPort.Items.Cast<string>().FirstOrDefault(p => DriverClasses.LineNumber(p) == freeLine)
                         ?? cbComPort.Items.Cast<string>().FirstOrDefault(p => DriverClasses.LineNumber(p) is { } n && !usedLines.Contains(n))
                         ?? "COM1";

        nudLine.Minimum = 1;
        nudLine.Maximum = 100;
        nudLine.Value = freeLine;
        nudNetPort.Minimum = 1;
        nudNetPort.Maximum = 65535;
        nudNetPort.Value = 4001;
        tbPipe.Text = @"\\PIPE\";

        cbTarget.Items.Add(registryTarget);
        if (iniAvailable) cbTarget.Items.Add(Resources.InissSettings_Target_Ini);
        cbTarget.SelectedIndex = 0;

        cbClass.SelectedIndexChanged += (_, _) => ClassChanged();
        cbConnection.SelectedIndexChanged += (_, _) => UpdateView();
        foreach (var c in new Control[] { cbComPort, tbParams, tbHost, tbPipe, cbSection })
            c.TextChanged += (_, _) => UpdatePreview();
        nudLine.ValueChanged += (_, _) => UpdatePreview();
        nudNetPort.ValueChanged += (_, _) => UpdatePreview();
        bOK.Click += (_, _) => Accept();
        ClassChanged();
        UpdateView();
    }

    [AllowNull]
    public sealed override string Text
    {
        get => base.Text;
        set => base.Text = value;
    }

    /// <summary>Vybrana sekcia (Driver3).</summary>
    public string Section => cbSection.SelectedItem as string ?? "";

    /// <summary>Zapisat do suboru .INI namiesto registra.</summary>
    public bool ToIni => cbTarget.SelectedIndex == 1;

    private Connection SelectedConnection => (Connection)Math.Max(0, cbConnection.SelectedIndex);

    private int SelectedClass => (cbClass.SelectedItem as DriverClass)?.Class ?? DriverDefaults.DefaultClass;

    /// <summary>Hodnota TablePort podla pripojenia.</summary>
    public string Port => SelectedConnection switch
    {
        Connection.Serial => cbComPort.Text.Trim().ToUpperInvariant(),
        Connection.Tcp => string.Create(CultureInfo.InvariantCulture, $"{nudLine.Value}=TCP://{tbHost.Text.Trim()}:{nudNetPort.Value}"),
        Connection.Udp => string.Create(CultureInfo.InvariantCulture, $"{nudLine.Value}=UDP://{tbHost.Text.Trim()}:{nudNetPort.Value}"),
        _ => string.Create(CultureInfo.InvariantCulture, $"{nudLine.Value}={tbPipe.Text.Trim()}")
    };

    /// <summary>Zmeny, ktore linku zalozia.</summary>
    public IReadOnlyList<RegChange> Changes()
    {
        var target = ToIni ? RegWriteTarget.Ini : RegWriteTarget.Registry;
        var changes = new List<RegChange>
        {
            new(Section, "TableClass", RegValueType.Dword, SelectedClass, target),
            new(Section, "TablePort", RegValueType.String, Port, target)
        };
        if (SelectedConnection == Connection.Serial && tbParams.Text.Trim() != _paramsDefault)
            changes.Add(new RegChange(Section, "TablePortParam", RegValueType.String, tbParams.Text.Trim(), target));
        return changes;
    }

    private void ClassChanged()
    {
        // parametre linky nasleduju predvolbu triedy, kym ich pouzivatel nezmeni
        var keep = tbParams.Text.Trim().Length > 0 && tbParams.Text.Trim() != _paramsDefault;
        _paramsDefault = DriverDefaults.For(SelectedClass, "COM1")["TablePortParam"] as string ?? "";
        if (!keep) tbParams.Text = _paramsDefault;
        UpdatePreview();
    }

    private void UpdateView()
    {
        var c = SelectedConnection;
        lComPort.Visible = cbComPort.Visible = lParams.Visible = tbParams.Visible = c == Connection.Serial;
        lHost.Visible = tbHost.Visible = lNetPort.Visible = nudNetPort.Visible = c is Connection.Tcp or Connection.Udp;
        lPipe.Visible = tbPipe.Visible = c == Connection.Pipe;
        lLine.Visible = nudLine.Visible = c != Connection.Serial;
        UpdatePreview();
    }

    /// <summary>Cislo linky podla pripojenia (seriovy port: cislo COM).</summary>
    private int? Line => SelectedConnection == Connection.Serial ? DriverClasses.LineNumber(cbComPort.Text) : (int)nudLine.Value;

    private void UpdatePreview()
    {
        var lines = new List<string> { string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_Wizard_Port, Port) };
        if (Line is { } line)
        {
            var onLine = _tables.Where(t => t.Table.CommunicationPort == line && t.Table.ID != -1).ToList();
            lines.Add(onLine.Count == 0
                ? string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_Wizard_NoTables, line)
                : string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_Wizard_Tables, line, string.Join(", ", onLine.Select(t => t.Table.Key))));
            // tabule bez cisla linky, ktore zatial nic nedostavaju a na tuto linku by sa priradili automaticky
            var auto = _lines.Unserved.Select(u => u.Table)
                .Where(t => t.Table.CommunicationPort == 0 && t.Manufacturer is { } am && DriverClasses.AcceptsAutomatically(SelectedClass, am)).ToList();
            if (auto.Count > 0)
                lines.Add(string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_Wizard_AutoTables, string.Join(", ", auto.Select(t => t.Table.Key))));
            foreach (var t in onLine.Where(t => t.Manufacturer is { } m && !DriverClasses.Accepts(SelectedClass, m)))
                lines.Add(string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_LineWrongFamily, t.Table.Key, t.Table.TableCatalog?.Manufacturer?.Name,
                    DriverClasses.Find(SelectedClass)?.Name));
            if (_lines.Lines.FirstOrDefault(l => l.Line == line) is { } other)
                lines.Add(string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_Wizard_LineUsed, line, other.Section));
        }

        lPreview.Text = string.Join(Environment.NewLine, lines);
    }

    private void Accept()
    {
        string? error = null;
        if (Section.Length == 0) error = Resources.InissSettings_Wizard_NoSection;
        else if (SelectedConnection == Connection.Serial && DriverClasses.LineNumber(cbComPort.Text) is null) error = Resources.InissSettings_Wizard_ComInvalid;
        else if (SelectedConnection is Connection.Tcp or Connection.Udp && tbHost.Text.Trim().Length == 0) error = Resources.InissSettings_Wizard_HostMissing;
        else if (Line is { } line && _lines.Lines.FirstOrDefault(l => l.Line == line) is { } other)
            error = string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_Wizard_LineUsed, line, other.Section);
        if (error is not null)
        {
            Utils.ShowWarning(error);
            return;
        }

        DialogResult = DialogResult.OK;
    }
}
