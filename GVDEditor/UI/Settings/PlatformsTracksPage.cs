using System.Globalization;
using ExControls;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using ToolsCore.Tools;
using Field = GVDEditor.Domain.Rules.PlatformTrackRules.Field;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranka Nastupistia a kolaje v okne Lokalne nastavenia - strom nastupiste → kolaje a udaje vybranej polozky
/// s upravou priamo v poliach. Zmeny idu rovno do <see cref="_ctx.Document.Platforms" /> a <see cref="_ctx.Document.Tracks" />,
/// Zrusit okna ich vrati.
/// </summary>
public partial class PlatformsTracksPage : UserControl, ISettingsPage
{
    /// <summary>
    /// Kontext editora - nastavi ho <c>LoadData</c>.
    /// </summary>
    private EditorContext _ctx = null!;

    private readonly List<(object Item, Field Field, string Text)> _problems = [];
    // povodna farba okraja poli (podla temy) - chybne pole sa zafarbi
    private readonly Dictionary<ExTextBox, Color> _borders = [];
    private object? _current;
    private bool _loading;
    private Color _hintColor;
    // stranka sa plni az pri prvom zobrazeni - vyber prvej kolaje pred naplnenim sa urobi po nom
    private bool _loaded;
    private bool _selectFirstTrack;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public PlatformsTracksPage()
    {
        InitializeComponent();
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
        SelectItem(item);
        FieldBox(item, field)?.Focus();
    }

    /// <summary>
    /// Naplni stranku - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name=\"context\">kontext editora</param>
    internal void LoadData(EditorContext context)
    {
        _ctx = context;
        foreach (var header in new[] { lPlatHeader, lIdent, lBoards, lSound })
            header.Font = new Font(Font, FontStyle.Bold);
        _hintColor = lHint.ForeColor;
        foreach (var box in new[] { tbPlatKey, tbPlatName, tbPlatSound, tbTrKey, tbTrName, tbTrFull, tbTrText, tbTrSound })
            _borders[box] = box.BorderColor;
        if (_ctx.UsingStyle.DarkScrollBar)
            pDetail.SetTheme(WindowsTheme.DarkExplorer);

        RefreshTables();
        _ctx.Document.TableLogicals.ListChanged += TableLogicals_ListChanged;
        Disposed += (_, _) => _ctx.Document.TableLogicals.ListChanged -= TableLogicals_ListChanged;

        BuildTree(null);
        Check();
        _loaded = true;
        if (_selectFirstTrack)
            SelectFirstTrack();
    }

    /// <summary>
    /// Vyberie prvu kolaj (stranka otvorena cez Vlastnosti → Kolaje).
    /// </summary>
    public void SelectFirstTrack()
    {
        _selectFirstTrack = !_loaded;
        if (!_loaded)
            return;

        var track = _ctx.Document.Tracks.FirstOrDefault(t => t != Track.None);
        if (track is not null)
            SelectItem(track);
    }

    private static bool IsReal(Platform platform) => platform != Platform.None;
    private static bool IsReal(Track track) => track != Track.None;

    // Platform a Track su record - dva rovnake by IndexOf/Remove podla hodnoty zamenili
    private static int IndexOf<T>(IList<T> list, T item) where T : class
    {
        for (var i = 0; i < list.Count; i++)
            if (ReferenceEquals(list[i], item))
                return i;
        return -1;
    }

    private static string Label(object item) => item switch
    {
        Platform p => string.IsNullOrWhiteSpace(p.FullName) ? p.Key : p.FullName,
        Track t => string.IsNullOrWhiteSpace(t.FullName) ? t.Key : t.FullName,
        _ => ""
    };

    /// <summary>
    /// Nastupiste, pod ktorym kolaj lezi v strome - rovnaky objekt, inak podla oznacenia.
    /// </summary>
    private Platform? PlatformOf(Track track)
    {
        if (track.Platform is null || !IsReal(track.Platform))
            return null;

        return _ctx.Document.Platforms.FirstOrDefault(p => ReferenceEquals(p, track.Platform))
               ?? _ctx.Document.Platforms.FirstOrDefault(p => IsReal(p) && p.EqualsKeys(track.Platform));
    }

    private void BuildTree(object? select)
    {
        _loading = true;
        tvTracks.BeginUpdate();
        tvTracks.Nodes.Clear();
        TreeNode? selected = null;

        foreach (var platform in _ctx.Document.Platforms.Where(IsReal))
        {
            var node = new TreeNode(Label(platform)) { Tag = platform };
            if (ReferenceEquals(platform, select)) selected = node;
            foreach (var track in _ctx.Document.Tracks.Where(t => IsReal(t) && ReferenceEquals(PlatformOf(t), platform)))
            {
                var child = new TreeNode(Label(track)) { Tag = track };
                if (ReferenceEquals(track, select)) selected = child;
                node.Nodes.Add(child);
            }

            tvTracks.Nodes.Add(node);
        }

        // kolaje, ktore nelezia na ziadnom nastupisti
        var orphans = _ctx.Document.Tracks.Where(t => IsReal(t) && PlatformOf(t) is null).ToList();
        if (orphans.Count > 0)
        {
            var node = new TreeNode(Resources.PlatformsTracksPage_Bez_nastupista);
            foreach (var track in orphans)
            {
                var child = new TreeNode(Label(track)) { Tag = track };
                if (ReferenceEquals(track, select)) selected = child;
                node.Nodes.Add(child);
            }

            tvTracks.Nodes.Add(node);
        }

        tvTracks.ExpandAll();
        tvTracks.EndUpdate();
        _loading = false;

        tvTracks.SelectedNode = selected ?? (tvTracks.Nodes.Count > 0 ? tvTracks.Nodes[0] : null);
        if (tvTracks.SelectedNode is null)
            Show(null);
        MarkProblems();
    }

    private void SelectItem(object item)
    {
        var node = AllNodes(tvTracks.Nodes).FirstOrDefault(n => ReferenceEquals(n.Tag, item));
        if (node is not null)
            tvTracks.SelectedNode = node;
    }

    private static IEnumerable<TreeNode> AllNodes(System.Collections.IEnumerable nodes)
    {
        foreach (TreeNode node in nodes)
        {
            yield return node;
            foreach (var child in AllNodes(node.Nodes))
                yield return child;
        }
    }

    private void tvTracks_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (!_loading)
            Show(e.Node?.Tag);
    }

    /// <summary>
    /// Zobrazi udaje nastupista alebo kolaje.
    /// </summary>
    private void Show(object? item)
    {
        _current = item;
        _loading = true;
        pDetail.SuspendLayout();
        try
        {
            tlpPlatform.Visible = item is Platform;
            tlpTrack.Visible = item is Track;

            if (item is Platform platform)
            {
                tbPlatKey.Text = platform.Key;
                tbPlatName.Text = platform.FullName;
                tbPlatSound.Text = platform.SoundName;
                var tracks = _ctx.Document.Tracks.Count(t => IsReal(t) && ReferenceEquals(PlatformOf(t), platform));
                lPlatNote.Text = tracks == 0
                    ? Resources.PlatformsTracksPage_Bez_kolaje
                    : string.Format(CultureInfo.CurrentCulture, Resources.PlatformsTracksPage_Kolaji, tracks);
            }
            else if (item is Track track)
            {
                tbTrKey.Text = track.Key;
                tbTrName.Text = track.Name;
                tbTrFull.Text = track.FullName;
                tbTrText.Text = track.TrackName;
                tbTrPlatText.Text = track.PlatformTrackText;
                tbTrAlt.Text = track.AltTrackText;
                tbTrSound.Text = track.SoundName;

                cbTrPlatform.Items.Clear();
                cbTrPlatform.Items.AddRange(_ctx.Document.Platforms.ToArray<object>());
                cbTrPlatform.SelectedItem = PlatformOf(track) ?? (object)Platform.None;

                for (var i = 0; i < clbTables.Items.Count; i++)
                    clbTables.SetItemChecked(i, track.Tables.Any(t => ReferenceEquals(t, clbTables.Items[i])));

                var (arrival, departure) = TrackEditing.CountUsage(track, _ctx.Document.Trains);
                lTrUse.Text = string.Format(CultureInfo.CurrentCulture, Resources.PlatformsTracksPage_Vlaky, arrival, departure);
            }
        }
        finally
        {
            pDetail.ResumeLayout(true);
            _loading = false;
        }

        UpdateButtons();
        ShowHint();
    }

    private void UpdateButtons()
    {
        var hasTracks = _current is Platform p && _ctx.Document.Tracks.Any(t => IsReal(t) && ReferenceEquals(PlatformOf(t), p));
        bDelete.Enabled = _current is Track || (_current is Platform && !hasTracks);
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
        lHint.Text = _current is Platform && !bDelete.Enabled ? Resources.PlatformsTracksPage_Najprv_kolaje : "";
    }

    private Control? FieldBox(object item, Field field) => item switch
    {
        Platform => field switch
        {
            Field.Key => tbPlatKey,
            Field.FullName => tbPlatName,
            Field.SoundName => tbPlatSound,
            _ => null
        },
        Track => field switch
        {
            Field.Key => tbTrKey,
            Field.Name => tbTrName,
            Field.FullName => tbTrFull,
            Field.TrackName => tbTrText,
            Field.SoundName => tbTrSound,
            _ => null
        },
        _ => null
    };

    private void Check()
    {
        _problems.Clear();
        var platforms = _ctx.Document.Platforms.Where(IsReal).ToList();
        for (var i = 0; i < platforms.Count; i++)
            if (PlatformTrackRules.CheckPlatform(platforms, i) is { } p)
                _problems.Add((platforms[i], p.Field, p.Message));

        var tracks = _ctx.Document.Tracks.Where(IsReal).ToList();
        for (var i = 0; i < tracks.Count; i++)
            if (PlatformTrackRules.CheckTrack(tracks, i) is { } t)
                _problems.Add((tracks[i], t.Field, t.Message));

        MarkProblems();
        ShowHint();
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void MarkProblems()
    {
        var error = SettingsWindow.ProblemColor(tvTracks);
        foreach (var node in AllNodes(tvTracks.Nodes))
        {
            var problem = _problems.FirstOrDefault(p => ReferenceEquals(p.Item, node.Tag)).Text;
            node.ForeColor = problem is null ? Color.Empty : error;
            node.ToolTipText = problem ?? "";
            if (node.Tag is not null)
                node.Text = Label(node.Tag);
        }

        foreach (var (field, box) in new (Field, Control)[] { (Field.Key, tbPlatKey), (Field.FullName, tbPlatName), (Field.SoundName, tbPlatSound) })
            MarkBox(box, _current is Platform && _problems.Any(p => ReferenceEquals(p.Item, _current) && p.Field == field));
        foreach (var (field, box) in new (Field, Control)[]
                 {
                     (Field.Key, tbTrKey), (Field.Name, tbTrName), (Field.FullName, tbTrFull), (Field.TrackName, tbTrText),
                     (Field.SoundName, tbTrSound)
                 })
            MarkBox(box, _current is Track && _problems.Any(p => ReferenceEquals(p.Item, _current) && p.Field == field));
    }

    private void MarkBox(Control box, bool bad)
    {
        if (box is ExTextBox tb && _borders.TryGetValue(tb, out var normal))
            tb.BorderColor = bad ? SettingsWindow.ProblemColor(tb.Parent ?? tb) : normal;
    }

    private void Platform_Changed(object? sender, EventArgs e)
    {
        if (_loading || _current is not Platform platform)
            return;

        if (sender == tbPlatKey)
        {
            // cely nazov ide s oznacenim, kym ho pouzivatel neprepise
            var auto = Resources.FLocalSettings_Nástupište_ + platform.Key;
            platform.Key = tbPlatKey.Text.Trim();
            if (platform.FullName.Length == 0 || platform.FullName == auto)
            {
                platform.FullName = Resources.FLocalSettings_Nástupište_ + platform.Key;
                SetText(tbPlatName, platform.FullName);
            }
        }
        else if (sender == tbPlatName)
            platform.FullName = tbPlatName.Text;
        else if (sender == tbPlatSound)
            platform.SoundName = tbPlatSound.Text.Trim();

        Changed(_ctx.Document.Platforms, platform);
    }

    private void Track_Changed(object? sender, EventArgs e)
    {
        if (_loading || _current is not Track track)
            return;

        if (sender == tbTrKey)
        {
            // kratky nazov, text na tabuli a cely nazov idu s oznacenim, kym ich pouzivatel neprepise
            var oldKey = track.Key ?? "";
            var auto = Resources.FLocalSettings_Koľaj_ + oldKey;
            track.Key = tbTrKey.Text.Trim();
            if (string.IsNullOrEmpty(track.Name) || track.Name == oldKey)
                SetText(tbTrName, track.Name = track.Key);
            if (string.IsNullOrEmpty(track.TrackName) || track.TrackName == oldKey)
                SetText(tbTrText, track.TrackName = track.Key);
            if (string.IsNullOrEmpty(track.FullName) || track.FullName == auto)
                SetText(tbTrFull, track.FullName = Resources.FLocalSettings_Koľaj_ + track.Key);
        }
        else if (sender == tbTrName)
            track.Name = tbTrName.Text.Trim();
        else if (sender == tbTrFull)
            track.FullName = tbTrFull.Text;
        else if (sender == tbTrText)
            track.TrackName = tbTrText.Text.Trim();
        else if (sender == tbTrPlatText)
            track.PlatformTrackText = tbTrPlatText.Text.Trim();
        else if (sender == tbTrAlt)
            track.AltTrackText = tbTrAlt.Text.Trim();
        else if (sender == tbTrSound)
            track.SoundName = tbTrSound.Text.Trim();

        Changed(_ctx.Document.Tracks, track);
    }

    private void SetText(Control box, string value)
    {
        _loading = true;
        box.Text = value;
        _loading = false;
    }

    private void Changed<T>(BindingList<T> list, T item) where T : class
    {
        var index = IndexOf(list, item);
        if (index >= 0)
            list.ResetItem(index);
        Check();
    }

    private void cbTrPlatform_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        if (_current is not Track track || cbTrPlatform.SelectedItem is not Platform platform)
            return;

        track.Platform = platform;
        Changed(_ctx.Document.Tracks, track);
        BuildTree(track);
    }

    private void clbTables_ItemCheck(object? sender, ItemCheckEventArgs e)
    {
        if (_loading || _current is not Track track || clbTables.Items[e.Index] is not TableLogical table)
            return;

        var has = track.Tables.Any(t => ReferenceEquals(t, table));
        if (e.NewValue == CheckState.Checked && !has)
            track.Tables.Add(table);
        else if (e.NewValue != CheckState.Checked && has)
            track.Tables.RemoveAll(t => ReferenceEquals(t, table));
    }

    /// <summary>
    /// Zoznam logickych tabul - meni sa aj na stranke Logicke tabule toho isteho okna.
    /// </summary>
    private void RefreshTables()
    {
        _loading = true;
        clbTables.BeginUpdate();
        clbTables.Items.Clear();
        foreach (var logical in _ctx.Document.TableLogicals)
            clbTables.Items.Add(logical, _current is Track track && track.Tables.Any(t => ReferenceEquals(t, logical)));
        clbTables.EndUpdate();
        _loading = false;
    }

    private void TableLogicals_ListChanged(object? sender, ListChangedEventArgs e) => RefreshTables();

    private void bAddPlatform_Click(object? sender, EventArgs e)
    {
        var key = PlatformTrackRules.SuggestKey(_ctx.Document.Platforms.Select(p => p.Key));
        var platform = new Platform(key, Resources.FLocalSettings_Nástupište_ + key, "");
        _ctx.Document.Platforms.Add(platform);
        BuildTree(platform);
        Check();
        tbPlatSound.Focus();
    }

    private void bAddTrack_Click(object? sender, EventArgs e)
    {
        var platform = _current switch
        {
            Platform p => p,
            Track t => PlatformOf(t),
            _ => null
        } ?? _ctx.Document.Platforms.FirstOrDefault(IsReal) ?? Platform.None;

        var key = PlatformTrackRules.SuggestKey(_ctx.Document.Tracks.Select(t => t.Key));
        var track = new Track
        {
            Key = key,
            Name = key,
            FullName = Resources.FLocalSettings_Koľaj_ + key,
            TrackName = key,
            SoundName = "",
            Platform = platform
        };
        _ctx.Document.Tracks.Add(track);
        BuildTree(track);
        Check();
        tbTrSound.Focus();
    }

    private void bDelete_Click(object? sender, EventArgs e)
    {
        if (!bDelete.Enabled)
            return;

        switch (_current)
        {
            case Track track:
            {
                var (arrival, departure) = TrackEditing.CountUsage(track, _ctx.Document.Trains);
                var question = arrival + departure == 0
                    ? string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_Kolaj_Odstranit_Nepouzita, track.Key)
                    : string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_Kolaj_Odstranit, track.Key, arrival, departure);
                if (Utils.ShowQuestion(question) != DialogResult.Yes)
                    return;

                var platform = PlatformOf(track);
                TrackEditing.Remove(track, _ctx.Document.Tracks, _ctx.Document.Trains);
                BuildTree(platform);
                break;
            }
            case Platform platform:
                _ctx.Document.Platforms.RemoveAt(IndexOf(_ctx.Document.Platforms, platform));
                BuildTree(null);
                break;
        }

        Check();
    }

    private void tvTracks_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete)
        {
            bDelete_Click(this, EventArgs.Empty);
            e.Handled = true;
        }
    }
}
