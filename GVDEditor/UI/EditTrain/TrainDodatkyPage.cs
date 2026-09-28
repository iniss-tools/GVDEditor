using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using ToolsCore.Entities;

namespace GVDEditor.UI.EditTrain;

/// <summary>
/// Stranka Dodatky v okne vlaku - dodatky hlasenia a pri kazdom tabulka Kedy hlasit, ktora sa upravuje priamo.
/// Typy hlaseni sa riadia smerovanim vlaku, preto sa tabulka pri zmene trasy prestavi.
/// </summary>
public partial class TrainDodatkyPage : UserControl, ITrainPage
{
    private TrainDraft _draft = null!;
    private BindingList<Dodatok> _doplnky = [];
    private List<ReportType> _types = [];
    private Routing? _routing;
    private Color _hintColor;
    private bool _loading;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public TrainDodatkyPage()
    {
        InitializeComponent();
    }

    /// <inheritdoc />
    public event EventHandler? Changed;

    /// <summary>
    /// Naplni stranku dodatkami konceptu - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name="draft">koncept vlaku</param>
    /// <param name="sounds">nahravky dodatkov zo zvukovej banky</param>
    internal void LoadData(TrainDraft draft, IEnumerable<FyzSound> sounds)
    {
        _draft = draft;
        foreach (var header in new[] { lListHeader, lDetailHeader })
            header.Font = new Font(Font, FontStyle.Bold);
        _hintColor = lHint.ForeColor;
        lInfo.ForeColor = SystemColors.GrayText;

        _loading = true;
        cbAdd.Items.Clear();
        foreach (var sound in sounds)
            cbAdd.Items.Add(sound);
        if (cbAdd.Items.Count != 0)
            cbAdd.SelectedIndex = 0;

        _doplnky = new BindingList<Dodatok>(draft.Doplnky);
        listDodatky.DataSource = _doplnky;
        _loading = false;

        UpdateTypes();
        ShowSelected();
    }

    /// <inheritdoc />
    bool ITrainPage.Handles(TrainRules.Field field) => field == TrainRules.Field.Dodatok;

    /// <inheritdoc />
    void ITrainPage.ShowProblems(IReadOnlyList<TrainRules.Problem> problems)
    {
        // smerovanie sa meni na stranke Trasa a casy - typy hlaseni idu s nim
        if (_draft.Routing != _routing)
        {
            UpdateTypes();
            ShowSelected();
        }

        TrainPageHint.Show(lHint, problems.Where(problem => problem.Field == TrainRules.Field.Dodatok).ToList(), _hintColor);
    }

    /// <inheritdoc />
    void ITrainPage.FocusField(TrainRules.Problem problem)
    {
        if (problem.Row >= 0 && problem.Row < listDodatky.Items.Count)
            listDodatky.SelectedIndex = problem.Row;
        listDodatky.Focus();
    }

    private void UpdateTypes()
    {
        _routing = _draft.Routing;
        _types = TrainRules.ReportTypesFor(_routing, GlobData.ReportTypes);
        lInfo.Text = _routing == null ? Resources.TrainDodatkyPage_BezTrasy : Resources.TrainDodatkyPage_Info;
    }

    private Dodatok? Selected => listDodatky.SelectedItem as Dodatok;

    private void ShowSelected()
    {
        var dodatok = Selected;
        lText.Text = dodatok?.Sound?.Text ?? "";
        matrix.Bind(dodatok?.ChosenReports, _types, GlobData.ReportVariants);
        bRemove.Enabled = dodatok != null;
    }

    private void OnChanged()
    {
        if (!_loading)
            Changed?.Invoke(this, EventArgs.Empty);
    }

    private void listDodatky_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (!_loading)
            ShowSelected();
    }

    // kod dodatku a jeho text, napr. „1002 – Vlak ide cez…“
    private static string Describe(string code, string? text) => string.IsNullOrEmpty(text) ? code : $"{code} – {text}";

    private void listDodatky_Format(object? sender, ListControlConvertEventArgs e)
    {
        if (e.ListItem is Dodatok dodatok)
            e.Value = Describe(dodatok.Name, dodatok.Sound?.Text);
    }

    private void cbAdd_Format(object? sender, ListControlConvertEventArgs e)
    {
        if (e.ListItem is FyzSound sound)
            e.Value = Describe(Dodatok.CodeFromKey(sound.Key), sound.Text);
    }

    private void cbAdd_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        Add();
        e.Handled = e.SuppressKeyPress = true;
    }

    private void bAdd_Click(object? sender, EventArgs e) => Add();

    /// <summary>
    /// Prida dodatok vybrany v ponuke. Dostane rovnake hlasenia ako vybrany dodatok (dodatky vlaku sa zvycajne
    /// hlasia spolu), inak ziadne - upozornenie pripomenie, ze ich treba zaskrtnut.
    /// </summary>
    private void Add()
    {
        if (cbAdd.SelectedItem is not FyzSound sound)
            return;

        var template = Selected;
        var dodatok = new Dodatok
        {
            Sound = sound,
            Name = Dodatok.CodeFromKey(sound.Key),
            ChosenReports = template == null ? [] : TrainDraft.CopyDodatok(template).ChosenReports
        };
        _doplnky.Add(dodatok);
        listDodatky.SelectedItem = dodatok;
        ShowSelected();
        OnChanged();
    }

    private void bRemove_Click(object? sender, EventArgs e) => Remove();

    private void Remove()
    {
        var index = listDodatky.SelectedIndex;
        if (index < 0)
            return;

        _doplnky.RemoveAt(index);
        if (_doplnky.Count != 0)
            listDodatky.SelectedIndex = Math.Min(index, _doplnky.Count - 1);
        ShowSelected();
        OnChanged();
    }

    private void listDodatky_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Delete)
            return;

        Remove();
        e.Handled = true;
    }

    private void matrix_Changed(object? sender, EventArgs e) => OnChanged();
}
