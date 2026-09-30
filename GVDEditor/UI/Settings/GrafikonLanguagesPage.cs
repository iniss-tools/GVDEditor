using System.Globalization;
using GVDEditor.Properties;
using GVDEditor.Domain.Rules;
using ToolsCore.Iniss.Entities;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranka Jazyky hlaseni v okne Lokalne nastavenia - ktore jazyky stanice grafikon pouziva (LANGUAGE_nn
/// lokalneho Categori.txt). Vyber sa do <see cref="_ctx.Document.LocalLanguages" /> zapise az pri OK okna.
/// </summary>
public partial class GrafikonLanguagesPage : UserControl, ISettingsPage
{
    /// <summary>
    /// Kontext editora - nastavi ho <c>LoadData</c>.
    /// </summary>
    private EditorContext _ctx = null!;

    // ItemCheck plati este pred zmenou zaskrtnutia - vyber sa pocita s novou hodnotou menenej polozky
    private bool _loading;
    private string? _problem;
    private Color _usageColor;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public GrafikonLanguagesPage()
    {
        InitializeComponent();
    }

    /// <inheritdoc />
    public event EventHandler? ProblemsChanged;

    /// <inheritdoc />
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? FirstProblem => _problem;

    /// <inheritdoc />
    public void FocusFirstProblem() => clbLanguages.Focus();

    /// <summary>
    /// Naplni zoznam jazykmi stanice a zaskrtne jazyky grafikonu.
    /// </summary>
    /// <param name=\"context\">kontext editora</param>
    internal void LoadData(EditorContext context)
    {
        _ctx = context;
        // farba popisu podla temy okna - chyba ho prefarbi, upozornenie nie
        _usageColor = lUsage.ForeColor;
        _loading = true;
        clbLanguages.Items.Clear();
        foreach (var language in _ctx.Workspace.Languages)
            clbLanguages.Items.Add(new Item(language), language.IsBasic || _ctx.Document.LocalLanguages.Contains(language));
        _loading = false;

        Check(Selected());
    }

    /// <summary>
    /// Zapise vyber do jazykov grafikonu - volat pri OK okna, ked stranka nehlasi chybu.
    /// </summary>
    public void Apply() => _ctx.Document.LocalLanguages = Selected();

    private List<FyzLanguage> Selected(int changedIndex = -1, bool changedChecked = false)
    {
        var selected = new List<FyzLanguage>();
        for (var i = 0; i < clbLanguages.Items.Count; i++)
            if (i == changedIndex ? changedChecked : clbLanguages.GetItemChecked(i))
                selected.Add(((Item)clbLanguages.Items[i]).Language);
        return selected;
    }

    private void Check(List<FyzLanguage> selected)
    {
        _problem = GrafikonLanguageRules.Check(selected, _ctx.Workspace.Languages, _ctx.Document.Radenia);
        var lines = GrafikonLanguageRules.Warnings(selected, _ctx.Workspace.Languages, _ctx.Document.Trains);
        if (_problem is not null)
            lines.Insert(0, _problem);

        lUsage.Text = string.Join(Environment.NewLine, lines);
        lUsage.ForeColor = _problem is not null ? SettingsWindow.ProblemColor(lUsage) : _usageColor;
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void clbLanguages_ItemCheck(object? sender, ItemCheckEventArgs e)
    {
        if (_loading)
            return;

        // hlavny jazyk stanice grafikon pouziva vzdy
        if (((Item)clbLanguages.Items[e.Index]).Language.IsBasic)
        {
            e.NewValue = CheckState.Checked;
            return;
        }

        Check(Selected(e.Index, e.NewValue == CheckState.Checked));
    }

    /// <summary>
    /// Polozka zoznamu - hlavny jazyk je oznaceny v texte.
    /// </summary>
    private sealed record Item(FyzLanguage Language)
    {
        public override string ToString() => Language.IsBasic
            ? string.Format(CultureInfo.CurrentCulture, Resources.GrafikonLanguages_Hlavny_Polozka, Language.Name)
            : Language.Name;
    }
}
