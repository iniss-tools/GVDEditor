using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Rules;
using ToolsCore.Entities;

namespace GVDEditor.UI.EditTrain;

/// <summary>
///     Stranka Jazyky v okne vlaku - dalsie jazyky, v ktorych INISS vlak hlasi.
/// </summary>
public partial class TrainLanguagesPage : UserControl, ITrainPage
{
    private TrainDraft _draft = null!;
    private bool _loading;

    /// <summary>
    ///     Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public TrainLanguagesPage()
    {
        InitializeComponent();
    }

    /// <inheritdoc />
    public event EventHandler? Changed;

    /// <summary>
    ///     Naplni zoznam jazykmi (bez zakladneho) a zaskrtne jazyky vlaku.
    /// </summary>
    internal void LoadData(TrainDraft draft, IEnumerable<FyzLanguage> languages)
    {
        _draft = draft;
        _loading = true;
        clbLanguages.Items.Clear();
        foreach (var language in languages.Where(language => !language.IsBasic))
            clbLanguages.Items.Add(language, draft.Languages.Contains(language));
        _loading = false;
    }

    /// <inheritdoc />
    bool ITrainPage.Handles(TrainRules.Field field) => false;

    /// <inheritdoc />
    void ITrainPage.ShowProblems(IReadOnlyList<TrainRules.Problem> problems)
    {
    }

    /// <inheritdoc />
    void ITrainPage.FocusField(TrainRules.Problem problem) => clbLanguages.Focus();

    private void clbLanguages_ItemCheck(object? sender, ItemCheckEventArgs e)
    {
        if (_loading)
            return;

        // ItemCheck prichadza pred zmenou - zaskrtnutie meneneho jazyka sa berie z udalosti
        _draft.Languages.Clear();
        for (var i = 0; i < clbLanguages.Items.Count; i++)
            if (i == e.Index ? e.NewValue == CheckState.Checked : clbLanguages.GetItemChecked(i))
                _draft.Languages.Add((FyzLanguage)clbLanguages.Items[i]);
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
