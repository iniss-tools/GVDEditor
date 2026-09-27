using GVDEditor.Properties;
using GVDEditor.Tools;
using ToolsCore.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Forms.Settings;

/// <summary>
///     Stranka Jazyky v okne Globalne nastavenia - jazyky, v ktorych INISS hlasi (Categori.txt), s upravou priamo
///     v tabulke. Zmeny idu rovno do <see cref="GlobData.Languages" />, Zrusit okna ich vrati.
/// </summary>
public partial class LanguagesPage : UserControl, ISettingsPage
{
    private readonly GridPageSupport _grid;
    private List<FyzLanguage> _bank = [];
    private bool _loading;

    /// <summary>
    ///     Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public LanguagesPage()
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
    public string? FirstProblem => _grid.FirstProblem;

    /// <inheritdoc />
    public void FocusFirstProblem() => _grid.FocusFirstProblem();

    /// <summary>
    ///     Naplni tabulku jazykmi - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name="bank">jazyky zvukovej banky</param>
    public void LoadData(List<FyzLanguage> bank)
    {
        _bank = bank;
        _grid.CaptureColors();

        // ponuka klucov: co INISS pozna a je v banke; kluc zo suboru mimo ponuky sa prida, aby ho bunka vedela zobrazit
        colKey.Items.Clear();
        var keys = LanguageRules.InissKeys.Where(key => _bank.Any(l => l.Key == key))
            .Concat(GlobData.Languages.Select(l => l.Key)).Distinct().ToArray<object>();
        colKey.Items.AddRange(keys);

        _loading = true;
        dgv.Rows.Clear();
        foreach (var language in GlobData.Languages)
        {
            var index = dgv.Rows.Add(language.Key, language.Name, language.IsBasic, BankName(language.Key));
            dgv.Rows[index].Tag = language;
        }
        _loading = false;

        Check();
    }

    private string BankName(string key) => _bank.FirstOrDefault(l => l.Key == key)?.Name ?? Resources.LanguagesPage_Nie_v_banke;

    private FyzLanguage? CurrentLanguage => dgv.CurrentRow?.Tag as FyzLanguage;

    private void Check()
    {
        _grid.BeginCheck();
        var languages = GlobData.Languages.ToList();
        var bankKeys = _bank.Select(l => l.Key).ToList();
        for (var i = 0; i < dgv.Rows.Count; i++)
        {
            var cell = dgv.Rows[i].Cells[colKey.Index];
            _grid.Report(cell, i >= LanguageRules.MaxLanguages
                ? string.Format(Resources.LanguageRules_Najviac_jazykov, LanguageRules.MaxLanguages)
                : LanguageRules.CheckLanguage(languages, i, bankKeys));
        }

        _grid.Report(null, LanguageRules.CheckBasic(languages));

        bAdd.Enabled = GlobData.Languages.Count < LanguageRules.MaxLanguages;
        _grid.Defer(UpdateSelection);
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private string BankHint() =>
        string.Format(Resources.LanguagesPage_Banka, string.Join(", ", _bank.Select(l => $"{l.Key} – {l.Name}")));

    private void bAdd_Click(object sender, EventArgs e)
    {
        if (GlobData.Languages.Count >= LanguageRules.MaxLanguages)
            return;

        // prvy jazyk banky, ktory INISS pozna a v zozname este nie je
        var used = GlobData.Languages.Select(l => l.Key).ToHashSet();
        var key = LanguageRules.InissKeys.FirstOrDefault(k => !used.Contains(k) && _bank.Any(l => l.Key == k))
                  ?? LanguageRules.InissKeys.FirstOrDefault(k => !used.Contains(k))
                  ?? LanguageRules.InissKeys[0];
        var bankLanguage = _bank.FirstOrDefault(l => l.Key == key);
        var language = new FyzLanguage(key, bankLanguage?.Name ?? key) { IsBasic = GlobData.Languages.Count == 0 };
        GlobData.Languages.Add(language);

        _loading = true;
        var index = dgv.Rows.Add(language.Key, language.Name, language.IsBasic, BankName(language.Key));
        dgv.Rows[index].Tag = language;
        _loading = false;

        Check();
        _grid.Edit(index, colKey);
    }

    private void bDelete_Click(object sender, EventArgs e)
    {
        if (CurrentLanguage is not { } language)
            return;

        GlobData.Languages.Remove(language);
        dgv.Rows.RemoveAt(dgv.CurrentRow!.Index);
        Check();
    }

    private void dgv_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_loading || e.RowIndex < 0)
            return;

        var row = dgv.Rows[e.RowIndex];
        var language = (FyzLanguage)row.Tag!;
        var value = row.Cells[e.ColumnIndex].Value;

        _loading = true;
        if (e.ColumnIndex == colKey.Index)
        {
            var key = value as string ?? "";
            // nazov prevzaty z banky ide s klucom; vlastny nazov ostava
            var nameFromBank = language.Name.Length == 0 || language.Name == BankName(language.Key) || language.Name == language.Key;
            language.Key = key;
            if (nameFromBank)
            {
                language.Name = _bank.FirstOrDefault(l => l.Key == key)?.Name ?? key;
                row.Cells[colName.Index].Value = language.Name;
            }

            row.Cells[colBank.Index].Value = BankName(key);
        }
        else if (e.ColumnIndex == colName.Index)
        {
            language.Name = (value as string ?? "").Trim();
        }
        else if (e.ColumnIndex == colBasic.Index)
        {
            language.IsBasic = value is true;
            // hlavny moze byt len jeden - zaskrtnutie presuva oznacenie
            if (language.IsBasic)
                foreach (DataGridViewRow other in dgv.Rows)
                    if (other != row && other.Tag is FyzLanguage l && l.IsBasic)
                    {
                        l.IsBasic = false;
                        other.Cells[colBasic.Index].Value = false;
                    }
        }
        _loading = false;

        GlobData.Languages.ResetBindings();
        Check();
    }

    private void dgv_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        // zaskrtnutie a vyber z ponuky sa prejavia hned, nie az po opusteni bunky
        if (dgv.IsCurrentCellDirty && dgv.CurrentCell is DataGridViewCheckBoxCell or DataGridViewComboBoxCell)
            dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void dgv_DataError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        // kluc mimo ponuky sa zobrazi tak, ako je, chybu ukaze kontrola
        e.ThrowException = false;
    }

    private void dgv_CurrentCellChanged(object? sender, EventArgs e) => _grid.Defer(UpdateSelection);

    private void UpdateSelection()
    {
        bDelete.Enabled = CurrentLanguage is not null;
        _grid.ShowHint(BankHint());
    }

    private void dgv_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && !dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
            dgv.BeginEdit(true);
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e) =>
        GridPageSupport.HandleKeys(e, () => bAdd_Click(this, EventArgs.Empty), () => bDelete_Click(this, EventArgs.Empty));
}
