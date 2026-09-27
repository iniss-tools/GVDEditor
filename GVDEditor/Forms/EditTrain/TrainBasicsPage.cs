using System.Globalization;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Forms.Settings;
using GVDEditor.Properties;
using GVDEditor.Tools;

namespace GVDEditor.Forms.EditTrain;

/// <summary>
///     Stranka Vlak v okne vlaku - cislo, typ, nazov, dopravca, priznaky a vyluka.
/// </summary>
public partial class TrainBasicsPage : UserControl, ITrainPage
{
    private readonly FieldMarks _marks = new();
    private TrainDraft _draft = null!;
    private List<TrainName> _names = [];
    private Color _hintColor;
    private bool _loading;

    /// <summary>
    ///     Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public TrainBasicsPage()
    {
        InitializeComponent();
    }

    /// <inheritdoc />
    public event EventHandler? Changed;

    /// <summary>
    ///     Cislo vlaku je dopisane (pole stratilo fokus) - okno podla neho zosuladi radenia.
    /// </summary>
    public event EventHandler? NumberCommitted;

    /// <summary>
    ///     Naplni stranku udajmi konceptu - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name="draft">koncept vlaku</param>
    /// <param name="names">mena vlakov zo zvukovej banky</param>
    internal void LoadData(TrainDraft draft, IEnumerable<TrainName> names)
    {
        _draft = draft;
        _names = [.. names];
        foreach (var header in new[] { lTrainHeader, lFlagsHeader, lLockoutHeader })
            header.Font = new Font(Font, FontStyle.Bold);
        _hintColor = lHint.ForeColor;
        lLockoutInfo.ForeColor = SystemColors.GrayText;
        lBanner.Padding = new Padding(6);
        lBanner.Visible = false;
        if (GlobData.UsingStyle.DarkScrollBar)
            pScroll.SetTheme(WindowsTheme.DarkExplorer);
        _marks.Capture(tbNumber);

        _loading = true;
        tbNumber.Text = draft.Number;

        cbType.DataSource = GlobData.TrainsTypes;
        cbType.SelectedItem = draft.Type;

        cbName.DataSource = _names;
        cbName.SelectedItem = null;
        // v grafikone je kluc zvuku, v zozname sa zobrazuje jeho nazov
        cbName.Text = TrainName.ToDisplay(_names, draft.Name);

        cbOperator.DataSource = GlobData.Operators.ToList();
        cbOperator.SelectedItem = draft.Operator;

        boxMiestenkovy.Checked = draft.IsMiestenkovy;
        boxMedzistatny.Checked = draft.IsMedzistatny;
        boxDialkovy.Checked = draft.IsDialkovy;
        boxMimoriadny.Checked = draft.IsMimoriadny;
        boxNizkopodlazny.Checked = draft.IsNizkopodlazny;
        boxLozkovy.Checked = draft.IsIbaLozkovy;
        boxPrestup.Checked = draft.IsPrestupovy;
        boxMotorovy.Checked = draft.IsMotorovy;

        FillLockouts(draft.LockoutNumber);
        _loading = false;
    }

    /// <inheritdoc />
    bool ITrainPage.Handles(TrainRules.Field field) => IsMine(field);

    private static bool IsMine(TrainRules.Field field) =>
        field is TrainRules.Field.Number or TrainRules.Field.Type or TrainRules.Field.Operator;

    /// <inheritdoc />
    void ITrainPage.ShowProblems(IReadOnlyList<TrainRules.Problem> problems)
    {
        var mine = problems.Where(problem => IsMine(problem.Field)).ToList();
        _marks.Mark(mine.Where(problem => !problem.IsWarning && problem.Field == TrainRules.Field.Number).Select(_ => (Control)tbNumber));
        TrainPageHint.Show(lHint, mine, _hintColor);
    }

    /// <inheritdoc />
    void ITrainPage.FocusField(TrainRules.Problem problem)
    {
        Control control = problem.Field switch
        {
            TrainRules.Field.Type => cbType,
            TrainRules.Field.Operator => cbOperator,
            _ => tbNumber
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
    }

    private void OnChanged()
    {
        if (!_loading)
            Changed?.Invoke(this, EventArgs.Empty);
    }

    private void tbNumber_TextChanged(object? sender, EventArgs e)
    {
        if (_loading)
            return;

        _draft.Number = tbNumber.Text;
        OnChanged();
    }

    private void tbNumber_Validated(object? sender, EventArgs e) => NumberCommitted?.Invoke(this, EventArgs.Empty);

    private void cbType_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        _draft.Type = cbType.SelectedItem as TrainType;
        OnChanged();
    }

    private void cbName_TextChanged(object? sender, EventArgs e)
    {
        if (_loading)
            return;

        _draft.Name = TrainName.ToStored(_names, cbName.Text);
        OnChanged();
    }

    private void cbOperator_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        _draft.Operator = cbOperator.SelectedItem as Operator;
        OnChanged();
    }

    private void Flag_CheckedChanged(object? sender, EventArgs e)
    {
        if (_loading)
            return;

        _draft.IsMiestenkovy = boxMiestenkovy.Checked;
        _draft.IsMedzistatny = boxMedzistatny.Checked;
        _draft.IsDialkovy = boxDialkovy.Checked;
        _draft.IsMimoriadny = boxMimoriadny.Checked;
        _draft.IsNizkopodlazny = boxNizkopodlazny.Checked;
        _draft.IsIbaLozkovy = boxLozkovy.Checked;
        _draft.IsPrestupovy = boxPrestup.Checked;
        _draft.IsMotorovy = boxMotorovy.Checked;
        OnChanged();
    }

    /// <summary>
    ///     Polozka ponuky vyluk: kod zapisovany do grafikonu a text zobrazeny v ponuke.
    ///     <paramref name="Source" /> je vyluka zalozena obsluhou v INISSe.
    /// </summary>
    private sealed record LockoutItem(int Code, string Text, LogZvukText? Source = null)
    {
        public override string ToString() => Text;
    }

    /// <summary>
    ///     Naplni ponuku vyluk: ziadna (0), zabudovana obecna vyluka (1) a vyluky zalozene obsluhou v INISSe.
    ///     Ak vlak odkazuje na kod, ktory INISS nepozna, prida sa ako neznama polozka, aby sa hodnota pri ulozeni nestratila.
    /// </summary>
    private void FillLockouts(int currentCode)
    {
        var items = new List<LockoutItem>
        {
            new(0, Resources.FEditTrain_Vyluka_None),
            new(1, $"1 – {Resources.FEditTrain_Vyluka_BuiltIn}")
        };

        foreach (var text in GlobData.LogZvukTexts)
            if (text.IsLockout && text.Code > 1 && items.All(item => item.Code != text.Code))
                items.Add(new LockoutItem(text.Code, text.ToString(), text));

        if (currentCode != 0 && items.All(item => item.Code != currentCode))
            items.Add(new LockoutItem(currentCode, $"{currentCode} – {Resources.FEditTrain_Vyluka_Unknown}"));

        cbLockout.Items.Clear();
        foreach (var item in items)
            cbLockout.Items.Add(item);

        cbLockout.DropDownWidth = Math.Max(cbLockout.Width, 320);
        cbLockout.SelectedItem = items.FirstOrDefault(item => item.Code == currentCode) ?? items[0];
        UpdateLockoutInfo();
    }

    // predloha a stanice vybranej vyluky zalozenej obsluhou
    private void UpdateLockoutInfo()
    {
        lLockoutInfo.Text = cbLockout.SelectedItem is LockoutItem { Source: { } source }
            ? string.Format(CultureInfo.CurrentCulture, Resources.FEditTrain_Vyluka_ItemHint, source.Template,
                string.Join(", ", source.StationNames))
            : Resources.TrainBasicsPage_Vyluka_Info;
    }

    private void cbLockout_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        UpdateLockoutInfo();
        _draft.LockoutNumber = cbLockout.SelectedItem is LockoutItem lockout ? lockout.Code : 0;
        OnChanged();
    }
}
