using GVDEditor.Entities;
using GVDEditor.Forms.Settings;
using GVDEditor.Properties;
using GVDEditor.Tools;

namespace GVDEditor.Forms.EditTrain;

/// <summary>
///     Stranka Radenie v okne vlaku - radenia cisla vlaku; vybrane radenie sa upravuje priamo v poliach (kopie,
///     do grafikonu sa zapisu az po OK). Radenie bez obdobia platnosti plati stale.
/// </summary>
public partial class TrainRadeniePage : UserControl, ITrainPage
{
    private readonly FieldMarks _marks = new();
    private TrainDraft _draft = null!;
    private DateTime _gvdStart, _gvdEnd;
    private Color _hintColor;
    private bool _loading;

    /// <summary>
    ///     Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public TrainRadeniePage()
    {
        InitializeComponent();
    }

    /// <inheritdoc />
    public event EventHandler? Changed;

    private BindingList<Radenie> Radenia => _draft.Radenia.Items;

    /// <summary>
    ///     Naplni stranku radeniami konceptu - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name="draft">koncept vlaku</param>
    /// <param name="gvdStart">zaciatok platnosti grafikonu (predvolene obdobie noveho radenia)</param>
    /// <param name="gvdEnd">koniec platnosti grafikonu</param>
    internal void LoadData(TrainDraft draft, DateTime gvdStart, DateTime gvdEnd)
    {
        _draft = draft;
        _gvdStart = gvdStart.Date;
        _gvdEnd = gvdEnd.Date;
        foreach (var header in new[] { lListHeader, lDetailHeader, lWhen })
            header.Font = new Font(Font, FontStyle.Bold);
        _hintColor = lHint.ForeColor;
        lInfo.ForeColor = SystemColors.GrayText;
        lInfo.Text = Resources.TrainRadeniePage_Info;
        lBanner.Padding = new Padding(6);
        lBanner.Visible = false;
        _marks.Capture(tbLimit);

        _loading = true;
        FillEndStations();
        listRadenia.DataSource = Radenia;
        _loading = false;
        ShowSelected();
    }

    /// <inheritdoc />
    bool ITrainPage.Handles(TrainRules.Field field) => field == TrainRules.Field.Radenie;

    /// <inheritdoc />
    void ITrainPage.ShowProblems(IReadOnlyList<TrainRules.Problem> problems)
    {
        var mine = problems.Where(problem => problem.Field == TrainRules.Field.Radenie).ToList();
        var index = listRadenia.SelectedIndex;
        var fields = Selected is { } radenie ? RadenieRules.Check(radenie, Radenia, index).Select(p => p.Field).ToList() : [];
        _marks.Mark(fields.Contains(RadenieRules.Field.DateLimit) ? [tbLimit] : []);
        TrainPageHint.Show(lHint, mine, _hintColor);
    }

    /// <inheritdoc />
    void ITrainPage.FocusField(TrainRules.Problem problem)
    {
        if (problem.Row >= 0 && problem.Row < Radenia.Count)
            listRadenia.SelectedIndex = problem.Row;
        if (Selected is not { } radenie)
            return;

        var field = RadenieRules.Check(radenie, Radenia, listRadenia.SelectedIndex).Select(p => p.Field).FirstOrDefault();
        Control control = field switch
        {
            RadenieRules.Field.Validity => dtpTo,
            RadenieRules.Field.DateLimit => tbLimit,
            _ => bCompose
        };
        control.Focus();
    }

    /// <summary>
    ///     Ukaze alebo skryje pruh s oznamenim o radeni prevzatom od vlaku s rovnakym cislom.
    /// </summary>
    internal void ShowRadeniaNotice(string? text)
    {
        lBanner.Text = text ?? "";
        lBanner.Visible = text != null;
        if (text != null)
        {
            var dark = BackColor.GetBrightness() < 0.5f;
            lBanner.BackColor = dark ? Color.FromArgb(78, 66, 28) : Color.FromArgb(255, 243, 205);
            lBanner.ForeColor = dark ? Color.FromArgb(255, 236, 179) : Color.FromArgb(102, 77, 3);
        }

        // radenia sa mohli nahradit radeniami ineho vlaku
        ShowSelected();
    }

    private Radenie? Selected => listRadenia.SelectedIndex >= 0 && listRadenia.SelectedIndex < Radenia.Count
        ? Radenia[listRadenia.SelectedIndex]
        : null;

    /// <summary>
    ///     Zobrazi vybrane radenie v poliach; bez vyberu su polia nedostupne.
    /// </summary>
    private void ShowSelected()
    {
        var radenie = Selected;
        _loading = true;
        tlpDetail.Enabled = radenie != null;
        bDuplicate.Enabled = bRemove.Enabled = radenie != null;

        tbText.Text = radenie?.Text ?? "";
        cbValidity.Checked = radenie?.HasValidity ?? true;
        dtpFrom.Value = radenie is { HasValidity: true } ? radenie.ZacPlatnosti.Date : _gvdStart;
        dtpTo.Value = radenie is { HasValidity: true } ? radenie.KonPlatnosti.Date : _gvdEnd;
        tbLimit.Text = radenie?.DatObm ?? "";
        SelectEndStation(radenie?.DestStation);
        matrix.Bind(radenie?.ChosenReports, GlobData.ReportTypes, GlobData.ReportVariants);
        UpdateValidityFields();
        _loading = false;
    }

    private void UpdateValidityFields()
    {
        dtpFrom.Enabled = dtpTo.Enabled = tbLimit.Enabled = bLimit.Enabled = cbValidity.Checked;
        bPlay.Enabled = Selected is { Sounds.Count: > 0 };
    }

    private void OnChanged()
    {
        if (_loading)
            return;

        // popis v zozname sa obnovi bez znovunacitania poli (kurzor v poli dni ostane na mieste)
        if (listRadenia.SelectedIndex >= 0)
        {
            _loading = true;
            Radenia.ResetItem(listRadenia.SelectedIndex);
            _loading = false;
        }

        Changed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    ///     Zapise polia do vybraneho radenia. Bez obdobia platnosti ma radenie prazdne obdobie aj dni (tak ho cita INISS).
    /// </summary>
    private void Detail_Changed(object? sender, EventArgs e)
    {
        if (_loading || Selected is not { } radenie)
            return;

        if (cbValidity.Checked)
        {
            radenie.ZacPlatnosti = dtpFrom.Value.Date;
            radenie.KonPlatnosti = dtpTo.Value.Date;
            radenie.DatObm = tbLimit.Text;
        }
        else
        {
            radenie.ZacPlatnosti = radenie.KonPlatnosti = DateTime.MinValue;
            radenie.DatObm = "";
        }

        radenie.DestStation = (cbDest.SelectedItem as EndStationItem)?.Station!;
        UpdateValidityFields();
        OnChanged();
    }

    private void matrix_Changed(object? sender, EventArgs e) => OnChanged();

    private void listRadenia_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (!_loading)
            ShowSelected();
    }

    private void listRadenia_Format(object? sender, ListControlConvertEventArgs e)
    {
        if (e.ListItem is not Radenie radenie)
            return;

        var text = !radenie.HasValidity
            ? Resources.FEditTrain_Radenie_BezPlatnosti
            : DateLimit.FormatDate(radenie.ZacPlatnosti) + " – " + DateLimit.FormatDate(radenie.KonPlatnosti);

        // radenia s rovnakym obdobim sa lisia len datumovym obmedzenim
        if (!string.IsNullOrWhiteSpace(radenie.DatObm)) text += $" ({radenie.DatObm})";
        if (radenie.DestStation != null) text += " → " + radenie.DestStation.Name;
        if (radenie.Sounds.Count == 0) text += " " + Resources.TrainRadeniePage_Nove;
        e.Value = text;
    }

    private void listRadenia_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Delete)
            return;

        Remove();
        e.Handled = true;
    }

    /// <summary>
    ///     Nove radenie s obdobim grafikonu; nahravky sa mu zlozia tlacidlom Zlozit radenie.
    /// </summary>
    private void bNew_Click(object? sender, EventArgs e) =>
        Add(new Radenie { ZacPlatnosti = _gvdStart, KonPlatnosti = _gvdEnd, DatObm = "", Text = "", DestStation = null! });

    private void bDuplicate_Click(object? sender, EventArgs e)
    {
        if (Selected is { } radenie)
            Add(RadeniaEditing.Clone(radenie));
    }

    private void Add(Radenie radenie)
    {
        Radenia.Add(radenie);
        listRadenia.SelectedIndex = Radenia.Count - 1;
        ShowSelected();
        OnChanged();
    }

    private void bRemove_Click(object? sender, EventArgs e) => Remove();

    private void Remove()
    {
        var index = listRadenia.SelectedIndex;
        if (index < 0 || index >= Radenia.Count)
            return;

        _loading = true;
        Radenia.RemoveAt(index);
        if (Radenia.Count != 0)
            listRadenia.SelectedIndex = Math.Min(index, Radenia.Count - 1);
        _loading = false;
        ShowSelected();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    private void bCompose_Click(object? sender, EventArgs e)
    {
        if (Selected is not { } radenie)
            return;

        using var dialog = new FRadenie([.. radenie.Sounds]);
        if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
            return;

        radenie.Sounds = [.. dialog.SelSounds];
        radenie.Text = Radenie.SoundsToString(radenie.Sounds);
        tbText.Text = radenie.Text;
        UpdateValidityFields();
        OnChanged();
    }

    private void bPlay_Click(object? sender, EventArgs e)
    {
        if (Selected is not { Sounds.Count: > 0 } radenie)
            return;

        var files = radenie.Sounds.Select(sound =>
            GlobData.RawBankDir + "\\" + sound.Language.RelativePath + sound.Group.RelativePath + sound.FileName).ToArray();
        new WavPlayer(files, GlobData.Config.PlayerSoundsOffset).StartPlay();
    }

    private void bLimit_Click(object? sender, EventArgs e)
    {
        if (FindForm() is not { } form || dtpTo.Value.Date < dtpFrom.Value.Date)
            return;

        if (FDateLimitEdit.SetDateLimit(form, dtpFrom.Value, dtpTo.Value, defaultValue: tbLimit.Text) is { } limit)
            tbLimit.Text = limit;
    }

    // ---------------------------------------------------------------- cielova stanica

    /// <summary>
    ///     Polozka ponuky cielovej stanice radenia; <see cref="Station" /> null = radenie pre vlak do akejkolvek stanice.
    /// </summary>
    private sealed record EndStationItem(Station? Station, string Text)
    {
        public override string ToString() => Text;
    }

    /// <summary>
    ///     Naplni ponuku cielovej stanice radenia: ziadne obmedzenie, stanice zo zvukovej banky a vlastne stanice.
    ///     Radenie patri cislu vlaku - obmedzenie na ciel rozlisi varianty toho isteho vlaku do roznych stanic.
    /// </summary>
    private void FillEndStations()
    {
        cbDest.Items.Clear();
        cbDest.Items.Add(new EndStationItem(null, Resources.FEditTrain_Radenie_LubovolnyCiel));
        foreach (var station in GlobData.Stations.Concat(GlobData.CustomStations).DistinctBy(s => s.ID).OrderBy(s => s.Name))
            cbDest.Items.Add(new EndStationItem(station, station.Name));
        cbDest.SelectedIndex = 0;
    }

    private void SelectEndStation(Station? station)
    {
        if (station == null)
        {
            cbDest.SelectedIndex = 0;
            return;
        }

        var item = cbDest.Items.Cast<EndStationItem>().FirstOrDefault(i => i.Station?.ID == station.ID);
        if (item == null)
        {
            // stanica zo suboru, ktora nie je v banke ani medzi vlastnymi stanicami - ponecha sa
            item = new EndStationItem(station, station.Name == station.ID ? station.ID : $"{station.Name} ({station.ID})");
            cbDest.Items.Add(item);
        }

        cbDest.SelectedItem = item;
    }
}
