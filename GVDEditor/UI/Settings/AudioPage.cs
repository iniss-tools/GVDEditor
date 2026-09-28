using System.Globalization;
using ExControls;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using ToolsCore.Tools;
using Field = GVDEditor.Domain.Rules.AudioRules.Field;

namespace GVDEditor.UI.Settings;

/// <summary>
///     Stranka Audio v okne Globalne nastavenia - zoznam audio liniek a udaje vybranej linky s upravou priamo
///     v poliach. Zmeny idu rovno do <see cref="GlobData.Audios" />, Zrusit okna ich vrati.
/// </summary>
public partial class AudioPage : UserControl, ISettingsPage
{
    private readonly GridPageSupport _grid;
    private readonly List<(Audio Item, Field Field, string Text)> _problems = [];
    // povodna farba okraja poli (podla temy) - chybne pole sa zafarbi
    private readonly Dictionary<ExTextBox, Color> _borders = [];
    private IList<GVDDirectory> _grafikony = [];
    private Audio? _current;
    private bool _loading;
    private Color _hintColor;

    // aktualny obsah ponuky stanic - prestavuje sa, len ked sa zmeni
    private (bool CustomOnly, Station? Extra)? _stationsState;

    /// <summary>
    ///     Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public AudioPage()
    {
        InitializeComponent();
        dgv.AutoGenerateColumns = false;
        _grid = new GridPageSupport(dgv, lHint);
    }

    /// <inheritdoc />
    public event EventHandler? ProblemsChanged;

    /// <inheritdoc />
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? FirstProblem => _problems.Count == 0 ? null : $"{Label(_problems[0].Item)} – {_problems[0].Text}";

    /// <inheritdoc />
    public void FocusFirstProblem()
    {
        if (_problems.Count == 0)
            return;

        var (item, field, _) = _problems[0];
        Select(item);
        FieldControl(field).Focus();
    }

    /// <summary>
    ///     Naplni stranku - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name="grafikony">grafikony v priecinku - port hlaseni grafikonu prepisuje uzol linky jeho stanice</param>
    public void LoadData(IList<GVDDirectory> grafikony)
    {
        _grafikony = grafikony;
        foreach (var header in new[] { lBasic, lTech })
            header.Font = new Font(Font, FontStyle.Bold);
        _hintColor = lHint.ForeColor;
        foreach (var box in TextBoxes)
            _borders[box] = box.BorderColor;
        if (GlobData.UsingStyle.DarkScrollBar)
            pDetail.SetTheme(WindowsTheme.DarkExplorer);
        _grid.CaptureColors();

        Fill(GlobData.Audios.FirstOrDefault());
    }

    private IEnumerable<ExTextBox> TextBoxes =>
        [tbName, tbShortName, tbQueue, tbSoundCard, tbInputLine, tbAmplifier, tbExchange, tbNode];

    private static string StationText(Station station) =>
        AudioRules.IsTest(station) ? Resources.FGlobalSettings_Testovaci_okruh : station.Name;

    private static string Label(Audio audio) => audio.Name.Trim().Length == 0 ? StationText(audio.Station) : audio.Name.Trim();

    // nazov linky podla stanice - novej linke a linke, ktorej nazov pouzivatel neprepisal
    private static string DefaultName(Station station) => AudioRules.IsTest(station) ? "Test" : station.Name;

    private static int IndexOf(Audio audio)
    {
        for (var i = 0; i < GlobData.Audios.Count; i++)
            if (ReferenceEquals(GlobData.Audios[i], audio))
                return i;
        return -1;
    }

    private void Fill(Audio? select)
    {
        _loading = true;
        dgv.Rows.Clear();
        foreach (var audio in GlobData.Audios)
        {
            var index = dgv.Rows.Add(Label(audio), StationText(audio.Station));
            dgv.Rows[index].Tag = audio;
        }
        _loading = false;

        if (select is not null)
            Select(select);
        Check();
        _grid.Defer(ShowCurrent);
    }

    private void Select(Audio audio)
    {
        var row = dgv.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => ReferenceEquals(r.Tag, audio));
        if (row is not null)
            dgv.CurrentCell = row.Cells[colName.Index];
        ShowCurrent();
    }

    private void UpdateRow(Audio audio)
    {
        var row = dgv.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => ReferenceEquals(r.Tag, audio));
        if (row is null)
            return;

        row.Cells[colName.Index].Value = Label(audio);
        row.Cells[colStation.Index].Value = StationText(audio.Station);
    }

    private void dgv_CurrentCellChanged(object? sender, EventArgs e)
    {
        if (!_loading)
            _grid.Defer(ShowCurrent);
    }

    /// <summary>
    ///     Zobrazi udaje linky vybranej v zozname.
    /// </summary>
    private void ShowCurrent()
    {
        var audio = dgv.CurrentRow?.Tag as Audio;
        _current = audio;
        _loading = true;
        pDetail.SuspendLayout();
        try
        {
            tlpDetail.Enabled = audio is not null;
            tbName.Text = audio?.Name ?? "";
            tbShortName.Text = audio?.ShortName ?? "";
            tbQueue.Text = audio?.QueueName ?? "";
            tbSoundCard.Text = audio?.SoundCard ?? "";
            tbInputLine.Text = audio?.InputLine ?? "";
            tbAmplifier.Text = audio?.AmplifierPort ?? "";
            tbExchange.Text = audio?.ExchangeParameter ?? "";
            tbNode.Text = audio?.Node ?? "";

            if (audio is not null)
            {
                cbCustomOnly.Checked = GlobData.CustomStations.Any(s => s.ID == audio.Station.ID);
                FillStations(audio.Station);
            }
        }
        finally
        {
            pDetail.ResumeLayout(true);
            _loading = false;
        }

        bDuplicate.Enabled = audio is not null;
        bDelete.Enabled = audio is not null;
        MarkProblems();
        ShowHint();
    }

    /// <summary>
    ///     Naplni ponuku stanic: testovaci okruh, potom stanice zvukovej banky alebo vlastne stanice. Stanica
    ///     <paramref name="select" /> sa vyberie podla cisla - ak v ponuke nie je (neznama stanica zo suboru),
    ///     prida sa, aby sa linka potichu nezmenila na inu.
    /// </summary>
    private void FillStations(Station select)
    {
        var customOnly = cbCustomOnly.Checked;
        var stations = customOnly ? GlobData.CustomStations.ToList() : GlobData.Stations.ToList();
        var known = AudioRules.IsTest(select) || stations.Any(s => SameId(s, select));
        var state = (customOnly, known ? null : select);

        if (_stationsState != state)
        {
            _stationsState = state;
            stations.Sort();
            stations.Insert(0, new Station(AudioRules.TestKey, Resources.FGlobalSettings_Testovaci_okruh));
            if (!known)
                stations.Add(select);

            cbStation.BeginUpdate();
            cbStation.Items.Clear();
            cbStation.Items.AddRange(stations.ToArray<object>());
            cbStation.EndUpdate();
        }

        var wasLoading = _loading;
        _loading = true;
        cbStation.SelectedItem = cbStation.Items.Cast<Station>().FirstOrDefault(s => SameId(s, select));
        _loading = wasLoading;
    }

    private static bool SameId(Station a, Station b) => string.Equals(a.ID, b.ID, StringComparison.OrdinalIgnoreCase);

    private void cbCustomOnly_CheckedChanged(object? sender, EventArgs e)
    {
        if (!_loading && _current is not null)
            FillStations(_current.Station);
    }

    private void cbStation_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        if (_current is not { } audio || cbStation.SelectedItem is not Station station)
            return;

        // nazov, ktory pouzivatel neprepisal, ide so stanicou
        var oldStation = audio.Station;
        // kopia - stanice ponuky su spolocne s ostatnymi oknami
        audio.Station = AudioRules.IsTest(station)
            ? new Station(AudioRules.TestKey, AudioRules.TestKey)
            : new Station(station.ID, station.Name);
        if (audio.Name.Trim().Length == 0 || audio.Name == DefaultName(oldStation))
            SetName(DefaultName(audio.Station), true);

        Changed(audio);
    }

    /// <summary>
    ///     Nastavi nazov linky; protokolovy nazov a fronta ho nasleduju, kym sa s nim zhoduju.
    /// </summary>
    private void SetName(string name, bool updateBox)
    {
        var audio = _current!;
        var old = audio.Name;
        audio.Name = name;
        if (updateBox)
            SetText(tbName, name);
        if (audio.ShortName.Length == 0 || audio.ShortName == old)
            SetText(tbShortName, audio.ShortName = name);
        if (audio.QueueName.Length == 0 || audio.QueueName == old)
            SetText(tbQueue, audio.QueueName = name);
    }

    private void SetText(Control box, string value)
    {
        var wasLoading = _loading;
        _loading = true;
        box.Text = value;
        _loading = wasLoading;
    }

    private void Field_Changed(object? sender, EventArgs e)
    {
        if (_loading || _current is not { } audio || sender is not Control box)
            return;

        var value = box.Text.Trim();
        if (box == tbName)
            SetName(value, false);
        else if (box == tbShortName)
            audio.ShortName = value;
        else if (box == tbQueue)
            audio.QueueName = value;
        else if (box == tbSoundCard)
            audio.SoundCard = value;
        else if (box == tbInputLine)
            audio.InputLine = value;
        else if (box == tbAmplifier)
            audio.AmplifierPort = value;
        else if (box == tbExchange)
            audio.ExchangeParameter = value;
        else if (box == tbNode)
            audio.Node = value;

        Changed(audio);
    }

    private void Changed(Audio audio)
    {
        UpdateRow(audio);
        Check();
    }

    private Control FieldControl(Field field) => field switch
    {
        Field.Station => cbStation,
        Field.ShortName => tbShortName,
        Field.Queue => tbQueue,
        Field.Amplifier => tbAmplifier,
        Field.Exchange => tbExchange,
        Field.Node => tbNode,
        _ => tbName
    };

    private void Check()
    {
        _problems.Clear();
        var audios = GlobData.Audios.ToList();
        for (var i = 0; i < audios.Count; i++)
            if (AudioRules.Check(audios, i) is { } problem)
                _problems.Add((audios[i], problem.Field, problem.Message));

        MarkProblems();
        ShowHint();
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void MarkProblems()
    {
        foreach (DataGridViewRow row in dgv.Rows)
            row.Cells[colName.Index].ErrorText = _problems.FirstOrDefault(p => ReferenceEquals(p.Item, row.Tag)).Text ?? "";

        foreach (var box in TextBoxes)
        {
            var bad = _current is not null &&
                      _problems.Any(p => ReferenceEquals(p.Item, _current) && FieldControl(p.Field) == box);
            box.BorderColor = bad ? SettingsWindow.ProblemColor(box.Parent ?? box) : _borders.GetValueOrDefault(box, box.BorderColor);
        }
    }

    private void ShowHint()
    {
        var problem = _problems.FirstOrDefault(p => ReferenceEquals(p.Item, _current)).Text;
        if (problem is not null)
        {
            lHint.Text = problem;
            lHint.ForeColor = SettingsWindow.ProblemColor(lHint);
            return;
        }

        lHint.ForeColor = _hintColor;
        lHint.Text = NeutralHint(_current) ?? "";
    }

    private string? NeutralHint(Audio? audio)
    {
        if (audio is null)
            return null;

        if (AudioRules.IsTest(audio.Station))
            return Resources.AudioPage_Test;

        // port hlaseni grafikonu stanice INISS pouzije ako uzol linky
        var grafikon = _grafikony.FirstOrDefault(g => g.GVD.ThisStation.ID == audio.Station.ID && g.Dir.ReportPort > 0);
        return grafikon is null
            ? null
            : string.Format(CultureInfo.CurrentCulture, Resources.AudioPage_Uzol_Z_Grafikonu, grafikon.GVD.ThisStation.Name,
                grafikon.Dir.ReportPort);
    }

    private void bAdd_Click(object? sender, EventArgs e)
    {
        // prva stanica grafikonu, ktora este linku nema
        var station = _grafikony.Select(g => g.GVD.ThisStation)
                          .FirstOrDefault(s => !GlobData.Audios.Any(a => a.Station.ID == s.ID))
                      ?? _grafikony.Select(g => g.GVD.ThisStation).FirstOrDefault()
                      ?? new Station(AudioRules.TestKey, AudioRules.TestKey);
        var name = AudioRules.UniqueName(GlobData.Audios, DefaultName(station));
        var audio = new Audio
        {
            Station = new Station(station.ID, station.Name),
            Name = name,
            ShortName = name,
            QueueName = name,
            Mixer = "",
            SoundCard = ""
        };

        GlobData.Audios.Add(audio);
        Fill(audio);
        FocusName();
    }

    private void bDuplicate_Click(object? sender, EventArgs e)
    {
        if (_current is not { } source)
            return;

        var name = AudioRules.UniqueName(GlobData.Audios, source.Name.Trim());
        var audio = new Audio
        {
            Station = source.Station,
            Name = name,
            ShortName = source.ShortName == source.Name ? name : source.ShortName,
            QueueName = source.QueueName == source.Name ? name : source.QueueName,
            Mixer = source.Mixer,
            SoundCard = source.SoundCard,
            InputLine = source.InputLine,
            AmplifierPort = source.AmplifierPort,
            ExchangeParameter = source.ExchangeParameter,
            Node = source.Node
        };

        GlobData.Audios.Insert(IndexOf(source) + 1, audio);
        Fill(audio);
        FocusName();
    }

    private void FocusName()
    {
        tbName.Focus();
        tbName.SelectAll();
    }

    private void bDelete_Click(object? sender, EventArgs e)
    {
        if (_current is not { } audio)
            return;

        var index = IndexOf(audio);
        GlobData.Audios.RemoveAt(index);
        Fill(GlobData.Audios.Count == 0 ? null : GlobData.Audios[Math.Min(index, GlobData.Audios.Count - 1)]);
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e) =>
        GridPageSupport.HandleKeys(e, () => bAdd_Click(this, EventArgs.Empty), () => bDelete_Click(this, EventArgs.Empty));
}
