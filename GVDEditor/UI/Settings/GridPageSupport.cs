namespace GVDEditor.UI.Settings;

/// <summary>
///     Spolocne spravanie stranok nastaveni s tabulkou: chyby oznacene priamo v bunkach, text pod tabulkou
///     (chyba vybraneho riadka alebo napoveda k nemu) a skok na prvu chybu.
/// </summary>
internal sealed class GridPageSupport
{
    private readonly DataGridView _dgv;
    private readonly Label _hint;
    private readonly List<(DataGridViewCell? Cell, string Text)> _problems = [];
    private Color _hintColor;

    /// <summary>
    ///     Vytvori podporu pre tabulku <paramref name="dgv" /> s textom pod nou <paramref name="hint" />.
    /// </summary>
    public GridPageSupport(DataGridView dgv, Label hint)
    {
        _dgv = dgv;
        _hint = hint;
        _hintColor = hint.ForeColor;
    }

    /// <summary>
    ///     Prva chyba tabulky; <see langword="null" />, ak ziadna nie je.
    /// </summary>
    public string? FirstProblem => _problems.Count == 0 ? null : _problems[0].Text;

    /// <summary>
    ///     Zapamata farbu textu pod tabulkou - volat po nastaveni temy okna. Prazdna plocha tabulky dostane farbu
    ///     buniek (predvolena sivá plocha vyzera ako nedostupny prvok).
    /// </summary>
    public void CaptureColors()
    {
        _hintColor = _hint.ForeColor;
        var cells = _dgv.DefaultCellStyle.BackColor;
        _dgv.BackgroundColor = cells.IsEmpty ? SystemColors.Window : cells;
    }

    /// <summary>
    ///     Bunka, ktora sa neda upravit (napr. cislo pouzivanej stanice), vyzera ako nedostupna.
    /// </summary>
    public static void MarkLocked(DataGridViewCell cell) => cell.Style.ForeColor = SystemColors.GrayText;

    /// <summary>
    ///     Zmaze chyby vo vsetkych bunkach pred novou kontrolou.
    /// </summary>
    public void BeginCheck()
    {
        _problems.Clear();
        foreach (DataGridViewRow row in _dgv.Rows)
            foreach (DataGridViewCell cell in row.Cells)
                cell.ErrorText = "";
    }

    /// <summary>
    ///     Oznaci chybu v bunke; <see langword="null" /> znamena, ze bunka je v poriadku. Chyba celeho zoznamu
    ///     (napr. chyba hlavny jazyk) nema bunku - <paramref name="cell" /> je <see langword="null" />.
    /// </summary>
    public void Report(DataGridViewCell? cell, string? problem)
    {
        if (problem is null)
            return;

        if (cell is not null)
            cell.ErrorText = problem;
        _problems.Add((cell, problem));
    }

    /// <summary>
    ///     Vyberie bunku s prvou chybou.
    /// </summary>
    public void FocusFirstProblem()
    {
        if (_problems.Count == 0)
            return;

        _dgv.Focus();
        if (_problems[0].Cell is { } cell)
            _dgv.CurrentCell = cell;
    }

    /// <summary>
    ///     Pod tabulkou ukaze chybu vybraneho riadka, potom chybu celeho zoznamu, inak napovedu <paramref name="neutral" />.
    /// </summary>
    public void ShowHint(string? neutral)
    {
        var row = _dgv.CurrentCell?.RowIndex ?? -1;
        var problem = _problems.FirstOrDefault(p => p.Cell is not null && p.Cell.RowIndex == row).Text ??
                      _problems.FirstOrDefault(p => p.Cell is null).Text;
        _hint.Text = problem ?? neutral ?? "";
        _hint.ForeColor = problem is not null ? SettingsWindow.ProblemColor(_hint) : _hintColor;
    }

    /// <summary>
    ///     Spusti obnovenie stavu stranky az po skonceni udalosti tabulky. Zmena aktualnej bunky (napr. po odstraneni
    ///     riadka) nesmie hned vypnut tlacidlo, ktore ma fokus - fokus by presiel do tabulky a ta by pocas zmeny
    ///     bunky menila bunku znova (InvalidOperationException: reentrant call to SetCurrentCellAddressCore).
    /// </summary>
    public void Defer(Action update)
    {
        if (!_dgv.IsHandleCreated)
        {
            update();
            return;
        }

        // viac poziadaviek pred spustenim sa zluci do jednej
        if (_deferred)
            return;

        _deferred = true;
        _dgv.BeginInvoke(() =>
        {
            _deferred = false;
            update();
        });
    }

    private bool _deferred;

    /// <summary>
    ///     Vyberie bunku a zacne ju upravovat (novy riadok).
    /// </summary>
    public void Edit(int row, DataGridViewColumn column)
    {
        _dgv.Focus();
        _dgv.CurrentCell = _dgv.Rows[row].Cells[column.Index];
        _dgv.BeginEdit(true);
    }

    /// <summary>
    ///     Klavesy tabulky: Insert prida polozku, Delete odstrani vybranu (ak sa bunka prave neupravuje).
    /// </summary>
    public static void HandleKeys(KeyEventArgs e, Action add, Action delete)
    {
        switch (e.KeyCode)
        {
            case Keys.Insert:
                add();
                e.Handled = true;
                break;
            case Keys.Delete:
                delete();
                e.Handled = true;
                break;
        }
    }
}
