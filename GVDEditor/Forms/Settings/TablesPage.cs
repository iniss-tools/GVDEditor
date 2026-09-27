namespace GVDEditor.Forms.Settings;

/// <summary>
///     Stranka so zoznamom tabul, textov alebo sekcii TabTab v okne Lokalne nastavenia. Polozky sa upravuju
///     v ich dialogoch (dvojklik, Enter); pod zoznamom je vidno, kde sa vybrana polozka pouziva. Co zoznam obsahuje,
///     urcuje <see cref="TableListKind" />.
/// </summary>
public partial class TablesPage : UserControl
{
    private TableListKind? _kind;

    /// <summary>
    ///     Vytvori stranku; obsah urci az <see cref="LoadData" />.
    /// </summary>
    public TablesPage()
    {
        InitializeComponent();
        dgv.AutoGenerateColumns = false;
    }

    /// <summary>
    ///     Naplni stranku podla druhu zoznamu - volat az po nastaveni temy okna.
    /// </summary>
    internal void LoadData(TableListKind kind)
    {
        _kind = kind;
        lInfo.Text = kind.Info;
        bAdd.Text = kind.AddText;
        bEdit.Text = kind.EditText;
        bDuplicate.Visible = kind.CanDuplicate;
        bEdit.Visible = !kind.SingleButton;
        colDetail.HeaderText = kind.DetailHeader;
        colKey.Visible = kind.ShowKey;
        colComment.Visible = kind.ShowComment;
        // bez komentara vyplni zvysok sirky stlpec s podrobnostou
        if (!kind.ShowComment)
            colDetail.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        lUseHeader.Font = new Font(Font, FontStyle.Bold);
        dgv.BackgroundColor = dgv.DefaultCellStyle.BackColor.IsEmpty ? SystemColors.Window : dgv.DefaultCellStyle.BackColor;
        Fill(null);
    }

    /// <summary>
    ///     Otvori dialog na pridanie (napr. editor TabTab hned po otvoreni okna).
    /// </summary>
    public void OpenAdd() => bAdd_Click(this, EventArgs.Empty);

    private object? Current => dgv.CurrentRow?.Tag;

    /// <inheritdoc />
    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        // odkazy medzi tabulami sa mohli zmenit na inej stranke okna
        if (Visible && _kind is not null)
            Fill(Current);
    }

    private void Fill(object? select)
    {
        if (_kind is null)
            return;

        dgv.Rows.Clear();
        foreach (var item in _kind.Items)
        {
            var index = dgv.Rows.Add(_kind.Name(item), _kind.Key(item), _kind.Detail(item), _kind.Comment(item));
            dgv.Rows[index].Tag = item;
        }

        ApplyFilter();
        var row = dgv.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.Visible && ReferenceEquals(r.Tag, select))
                  ?? dgv.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.Visible);
        if (row is not null)
            dgv.CurrentCell = row.Cells[colName.Index];
        UpdateSelection();
    }

    private void UpdateSelection()
    {
        if (_kind is null)
            return;

        var item = Current;
        bEdit.Enabled = bDuplicate.Enabled = item is not null;
        if (item is null)
        {
            bDelete.Enabled = false;
            lUse.Text = "";
            return;
        }

        var usage = _kind.Usage(item);
        bDelete.Enabled = usage.Count == 0 || !_kind.UsageBlocksDelete;
        // dlhy zoznam by stlacil tabulku - zvysok sa zhrnie poctom
        const int shown = 6;
        var lines = usage.Take(usage.Count > shown ? shown - 1 : shown).Select(u => "– " + u).ToList();
        if (usage.Count > shown)
            lines.Add(string.Format(System.Globalization.CultureInfo.CurrentCulture, Properties.Resources.TablesPage_Dalsie,
                usage.Count - lines.Count));
        if (usage.Count > 0 && _kind.UsageBlocksDelete)
            lines.Add(Properties.Resources.TablesPage_Pouzivana_neodstranit);
        lUse.Text = usage.Count == 0 ? _kind.NoUsage : string.Join(Environment.NewLine, lines);
    }

    private void bAdd_Click(object? sender, EventArgs e)
    {
        if (_kind is null)
            return;

        if (_kind.SingleButton && Current is { } item)
        {
            Fill(_kind.Edit(FindForm()!, item) ?? item);
            return;
        }

        var added = _kind.Add(FindForm()!);
        Fill(added ?? Current);
    }

    private void bDuplicate_Click(object? sender, EventArgs e)
    {
        if (_kind is null || Current is not { } item)
            return;

        Fill(_kind.Duplicate(FindForm()!, item) ?? item);
    }

    private void bEdit_Click(object? sender, EventArgs e)
    {
        if (_kind is null || Current is not { } item)
            return;

        Fill(_kind.Edit(FindForm()!, item) ?? item);
    }

    private void bDelete_Click(object? sender, EventArgs e)
    {
        if (_kind is null || Current is not { } item || !bDelete.Enabled)
            return;

        var index = dgv.CurrentRow!.Index;
        _kind.Remove(item);
        Fill(null);
        if (dgv.Rows.Count > 0)
            dgv.CurrentCell = dgv.Rows[Math.Min(index, dgv.Rows.Count - 1)].Cells[colName.Index];
    }

    private void tbFilter_TextChanged(object? sender, EventArgs e)
    {
        ApplyFilter();
        UpdateSelection();
    }

    private void ApplyFilter()
    {
        var filter = tbFilter.Text.Trim();
        foreach (DataGridViewRow row in dgv.Rows)
        {
            var visible = filter.Length == 0 || row.Cells.Cast<DataGridViewCell>()
                .Any(c => c.OwningColumn!.Visible && (c.Value as string ?? "").Contains(filter, StringComparison.CurrentCultureIgnoreCase));
            if (!visible && dgv.CurrentRow == row)
                dgv.CurrentCell = null;
            row.Visible = visible;
        }
    }

    // obnovenie tlacidiel az po skonceni zmeny bunky - vypnutie tlacidla s fokusom by presunulo fokus do tabulky
    // uprostred zmeny bunky (reentrant call to SetCurrentCellAddressCore)
    private bool _updatePending;

    private void dgv_CurrentCellChanged(object? sender, EventArgs e)
    {
        if (!IsHandleCreated)
        {
            UpdateSelection();
            return;
        }

        if (_updatePending)
            return;

        _updatePending = true;
        BeginInvoke(() =>
        {
            _updatePending = false;
            UpdateSelection();
        });
    }

    private void dgv_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            bEdit_Click(this, EventArgs.Empty);
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Enter:
                bEdit_Click(this, EventArgs.Empty);
                e.Handled = true;
                break;
            case Keys.Insert:
                bAdd_Click(this, EventArgs.Empty);
                e.Handled = true;
                break;
            case Keys.Delete:
                bDelete_Click(this, EventArgs.Empty);
                e.Handled = true;
                break;
        }
    }
}
