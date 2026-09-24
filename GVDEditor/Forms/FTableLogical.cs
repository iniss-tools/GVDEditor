using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Nastavenie logickej tabule.
/// </summary>
public partial class FTableLogical : Form
{
    private readonly bool copy;

    /// <summary>
    ///     Zostava - pre kazdu fyzicku tabulu rozsah zaznamov, pociatocny riadok a typ zobrazenia.
    /// </summary>
    private readonly BindingList<TableLogicalSegment> TVybrane;

    /// <summary>
    ///     Umiestnenia zaznamov pri otvoreni okna - ulozia sa bezo zmeny, ak pouzivatel zostavu nezmenil.
    /// </summary>
    private readonly List<TableRecord> originalRecords;

    /// <summary>
    ///     Ci sa umiestnenia daju vyjadrit zostavou; ak nie, zostava sa neda upravit a ulozia sa povodne umiestnenia.
    /// </summary>
    private readonly bool expressible;

    private readonly bool loading;
    private bool zostavaChanged;
    private int lastCount;
    private TableViewType? lastTypeView;

    /// <summary>
    ///     Logicka tabula, ktoru upravuje tento dialog.
    /// </summary>
    public TableLogical ThisTable;

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FTableLogical"/>.
    /// </summary>
    /// <param name="table">Upravujuca tabula.</param>
    /// <param name="tables">Dostupne fyzicke tabule.</param>
    /// <param name="copy">Ci sa jedna o kopiu.</param>
    public FTableLogical(TableLogical table, IReadOnlyCollection<TablePhysical> tables, bool copy = false, Station? thisStation = null)
    {
        loading = true;
        InitializeComponent();
        dgvZostava.AutoGenerateColumns = false;
        this.ApplyThemeAndFonts();

        ThisTable = table;
        this.copy = copy;

        FillStations(thisStation);

        originalRecords = ThisTable.Records;
        var segments = TableLogicalLayout.FromRecords(originalRecords);
        expressible = TableLogicalLayout.IsExpressible(originalRecords, segments);
        TVybrane = new BindingList<TableLogicalSegment>(segments);

        cbTypeView.DataSource = TableViewType.GetValues();
        if (ThisTable.ViewType != null) cbTypeView.SelectedItem = ThisTable.ViewType;
        lastTypeView = cbTypeView.SelectedItem as TableViewType;

        // stlpec ma vsetky typy (aby sa dal zobrazit aj typ, ktory katalog nepodporuje), bunky len podporovane
        colTypeView.DataSource = TableViewType.GetValues().ToList();

        listFyzTab.DataSource = tables;
        dgvZostava.DataSource = TVybrane;

        tbName.Text = ThisTable.Name;
        tbKey.Text = ThisTable.Key;

        tbComment.Text = table.Comment;

        nudCountRecords.Value = ThisTable.Records.Count;
        lastCount = ThisTable.Records.Count;

        if (!expressible)
        {
            dgvZostava.ReadOnly = true;
            nudCountRecords.Enabled = false;
            bAddTab.Enabled = false;
            bRemoveTab.Enabled = false;
            listFyzTab.Enabled = false;
        }

        // IDSTATION: 0 = neuvedene (prazdne pole), inak stanica zo zoznamu alebo vlastne cislo
        if (ThisTable.IdStation != 0)
        {
            var match = cbIdStation.Items.Cast<StationItem>().FirstOrDefault(item => item.Id == ThisTable.IdStation);
            if (match != null)
                cbIdStation.SelectedItem = match;
            else
                cbIdStation.Text = ThisTable.IdStation.ToString();
        }

        loading = false;
    }

    /// <summary>
    ///     Polozka ponuky stanic pre IDSTATION - cislo a nazov.
    /// </summary>
    private sealed record StationItem(int Id, string Name)
    {
        public override string ToString() => $"{Id} – {Name}";
    }

    /// <summary>
    ///     Naplni ponuku stanic: najprv stanica tohto grafikonu, potom stanice ostatnych grafikonov z DirList.TXT.
    ///     Pole je editovatelne, takze sa da zadat aj ine cislo.
    /// </summary>
    private void FillStations(Station? thisStation)
    {
        var items = new List<StationItem>();

        void AddStation(Station? station)
        {
            if (station == null || !int.TryParse(station.ID, out var id) || id == 0 || items.Any(item => item.Id == id))
                return;
            items.Add(new StationItem(id, station.Name));
        }

        AddStation(thisStation);
        foreach (var dir in GlobData.GVDDirs)
        {
            try
            {
                AddStation(TxtParser.ReadInfoGVD(dir.FullPath).ThisStation);
            }
            catch (Exception)
            {
                // priecinok bez citatelneho Grafikon.txt - do ponuky sa nedostane
            }
        }

        cbIdStation.Items.Clear();
        foreach (var item in items)
            cbIdStation.Items.Add(item);
    }

    /// <summary>
    ///     Precita IDSTATION z pola - vybrata polozka alebo rucne zadane cislo (aj v tvare "5613600 – Nazov").
    /// </summary>
    /// <returns>Cislo stanice, 0 ak je pole prazdne, alebo <see langword="null" /> pri neplatnom zadani.</returns>
    private int? ReadIdStation()
    {
        if (cbIdStation.SelectedItem is StationItem selected)
            return selected.Id;

        var text = cbIdStation.Text.Trim();
        if (text.Length == 0)
            return 0;

        var digits = new string(text.TakeWhile(char.IsDigit).ToArray());
        return digits.Length > 0 && int.TryParse(digits, out var id) ? id : null;
    }

    private void FTableLogical_Shown(object? sender, EventArgs e)
    {
        RefreshTypeViewCells();
        if (!expressible)
            Utils.ShowWarning(Resources.FTableLogical_Zostava_nevyjadriteľná);
    }

    /// <summary>
    ///     Kazdej bunke typu zobrazenia ponukne len typy, ktore podporuje katalog fyzickej tabule riadku
    ///     (plus aktualny typ, ak ho katalog nepodporuje - aby sa dal zobrazit a nestratil sa).
    /// </summary>
    private void RefreshTypeViewCells()
    {
        foreach (DataGridViewRow row in dgvZostava.Rows)
        {
            if (row.DataBoundItem is not TableLogicalSegment segment || row.Cells[colTypeView.Index] is not DataGridViewComboBoxCell cell)
                continue;

            var types = TableLogicalLayout.SupportedViewTypes(segment.Table);
            if (types.Count == 0)
                types = TableViewType.GetValues().ToList();
            if (segment.TypeView != null && !types.Contains(segment.TypeView))
                types.Add(segment.TypeView);

            cell.DataSource = types;
            cell.DisplayMember = "Name";
            cell.ValueMember = "This";
        }
    }

    private void ReloadZostava()
    {
        TVybrane.ResetBindings();
        RefreshTypeViewCells();
    }

    private void bSave_Click(object sender, EventArgs e)
    {
        var table = copy ? new TableLogical() : ThisTable;

        if (!dgvZostava.EndEdit())
        {
            DialogResult = DialogResult.None;
            return;
        }

        var idStation = ReadIdStation();
        if (idStation == null)
        {
            Utils.ShowError(Resources.FTableLogical_Neplatné_číslo_stanice);
            DialogResult = DialogResult.None;
            return;
        }

        if (string.IsNullOrEmpty(tbName.Text) || string.IsNullOrEmpty(tbKey.Text))
        {
            Utils.ShowError(Resources.Tables_Nie_sú_vyplnené_všetky_povinné_polia);
            DialogResult = DialogResult.None;
            return;
        }

        foreach (var t in GlobData.TableLogicals)
            if (t.Key == tbKey.Text && table != t)
            {
                Utils.ShowError(Resources.Tables_Zadaný_kľúč_tabule_už_existuje);
                DialogResult = DialogResult.None;
                return;
            }

        var count = decimal.ToInt32(nudCountRecords.Value);
        if (count < 1)
        {
            Utils.ShowError(Resources.FTableLogical_Bez_záznamov);
            DialogResult = DialogResult.None;
            return;
        }

        List<TableRecord> records;
        if (!expressible || !zostavaChanged)
        {
            // zostavu nikto nemenil (alebo sa ju neda zobrazit) - umiestnenia ostanu presne ako boli
            records = copy ? TableLogicalLayout.CloneRecords(originalRecords) : originalRecords;
        }
        else
        {
            if (!ValidateZostava(count))
            {
                DialogResult = DialogResult.None;
                return;
            }

            records = TableLogicalLayout.ToRecords(TVybrane, count);
        }

        table.Key = tbKey.Text;
        table.Name = tbName.Text;
        table.ViewType = (TableViewType)cbTypeView.SelectedItem!;
        if (copy) table.TypeViewFlags = ThisTable.TypeViewFlags;

        table.Records = records;

        table.Comment = tbComment.Text;

        table.IdStation = idStation.Value;

        if (copy) ThisTable = table;

        DialogResult = DialogResult.OK;
    }

    /// <summary>
    ///     Skontroluje zostavu: neplatne riadky su chyba, nedostatky, ktore INISS znesie (nepodporovany typ, riadky mimo
    ///     fyzickej tabule, viac zaznamov na jednom riadku), sa len oznamia s moznostou ulozit aj tak.
    /// </summary>
    private bool ValidateZostava(int count)
    {
        for (var i = 0; i < TVybrane.Count; i++)
        {
            var s = TVybrane[i];
            if (s.FirstRecord < 1 || s.LastRecord > count || s.FirstRecord > s.LastRecord || s.StartRow < 1 || s.TypeView == null)
            {
                Utils.ShowError(string.Format(Resources.FTableLogical_Neplatný_riadok_zostavy, i + 1, s.Table, count));
                return false;
            }
        }

        var warnings = new List<string>();
        foreach (var s in TVybrane)
        {
            var supported = TableLogicalLayout.SupportedViewTypes(s.Table);
            if (supported.Count != 0 && !supported.Contains(s.TypeView))
                warnings.Add(string.Format(Resources.FTableLogical_Typ_nepodporovaný, s.Table, s.TypeView.Name));
            if (s.Table.RecCount > 0 && s.EndRow > s.Table.RecCount)
                warnings.Add(string.Format(Resources.FTableLogical_Mimo_tabule, s.Table, s.StartRow, s.EndRow, s.Table.RecCount));
        }

        // rovnaky riadok tej istej fyzickej tabule pre rozne zaznamy
        var rows = new Dictionary<(TablePhysical, int), SortedSet<int>>();
        foreach (var s in TVybrane)
            for (var record = s.FirstRecord; record <= s.LastRecord; record++)
            {
                var key = (s.Table, s.StartRow + record - s.FirstRecord);
                if (!rows.TryGetValue(key, out var set))
                    rows[key] = set = new SortedSet<int>();
                set.Add(record);
            }

        foreach (var ((physical, row), set) in rows)
            if (set.Count > 1)
                warnings.Add(string.Format(Resources.FTableLogical_Kolízia_riadku, physical, row, string.Join(", ", set)));

        return warnings.Count == 0 ||
               Utils.ShowQuestion(string.Format(Resources.FTableLogical_Upozornenia_zostavy, string.Join(Environment.NewLine, warnings))) ==
               DialogResult.Yes;
    }

    private void bStorno_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

    private void bAddTab_Click(object sender, EventArgs e) => AddSelectedTable();

    private void listFyzTab_DoubleClick(object sender, EventArgs e) => AddSelectedTable();

    /// <summary>
    ///     Prida do zostavy vybratu fyzicku tabulu - vsetky zaznamy od 1. riadku, s typom logickej tabule, ak ho
    ///     katalog fyzickej tabule podporuje, inak s prvym podporovanym.
    /// </summary>
    private void AddSelectedTable()
    {
        if (!expressible || listFyzTab.SelectedItem is not TablePhysical fyztab)
            return;

        var count = decimal.ToInt32(nudCountRecords.Value);
        if (count < 1)
        {
            Utils.ShowError(Resources.FTableLogical_Najprv_počet_záznamov);
            return;
        }

        if (TVybrane.Any(s => ReferenceEquals(s.Table, fyztab)) &&
            Utils.ShowQuestion(string.Format(Resources.FTableLogical_Tabuľa_už_v_zostave, fyztab)) != DialogResult.Yes)
            return;

        var supported = TableLogicalLayout.SupportedViewTypes(fyztab);
        var tableType = cbTypeView.SelectedItem as TableViewType;
        var typeView = tableType != null && (supported.Count == 0 || supported.Contains(tableType))
            ? tableType
            : supported.FirstOrDefault() ?? tableType ?? TableViewType.Odchodova;

        TVybrane.Add(new TableLogicalSegment
        {
            Table = fyztab, FirstRecord = 1, LastRecord = count, StartRow = 1, TypeView = typeView
        });
        zostavaChanged = true;
        RefreshTypeViewCells();
    }

    private void bRemoveTab_Click(object sender, EventArgs e)
    {
        if (!expressible || dgvZostava.SelectedRows.Count == 0)
            return;

        TVybrane.RemoveAt(dgvZostava.SelectedRows[0].Index);
        zostavaChanged = true;
    }

    /// <summary>
    ///     Zmena poctu zaznamov: rozsahy za novym koncom sa skratia alebo odstrania, rozsahy konciace na
    ///     povodnom poslednom zazname sa predlzia na novy posledny.
    /// </summary>
    private void nudCountRecords_ValueChanged(object? sender, EventArgs e)
    {
        if (loading || !expressible)
            return;

        var count = decimal.ToInt32(nudCountRecords.Value);
        if (count == lastCount)
            return;

        for (var i = TVybrane.Count - 1; i >= 0; i--)
        {
            var s = TVybrane[i];
            if (count < lastCount)
            {
                if (s.FirstRecord > count)
                    TVybrane.RemoveAt(i);
                else if (s.LastRecord > count)
                    s.LastRecord = count;
            }
            else if (s.LastRecord == lastCount)
            {
                s.LastRecord = count;
            }
        }

        lastCount = count;
        zostavaChanged = true;
        ReloadZostava();
    }

    /// <summary>
    ///     Zmena typu logickej tabule prenesie novy typ na riadky zostavy, ktore mali doterajsi typ a ktorych
    ///     fyzicka tabula novy typ podporuje.
    /// </summary>
    private void cbTypeView_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (loading || cbTypeView.SelectedItem is not TableViewType newType)
            return;

        var oldType = lastTypeView;
        lastTypeView = newType;
        if (!expressible || oldType == null || oldType == newType)
            return;

        var changed = false;
        foreach (var s in TVybrane)
        {
            var supported = TableLogicalLayout.SupportedViewTypes(s.Table);
            if (s.TypeView == oldType && (supported.Count == 0 || supported.Contains(newType)))
            {
                s.TypeView = newType;
                changed = true;
            }
        }

        if (!changed)
            return;

        zostavaChanged = true;
        ReloadZostava();
    }

    private void dgvZostava_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
        if (e.RowIndex == -1 || dgvZostava.ReadOnly)
            return;

        if (e.ColumnIndex == colFirstRecord.Index || e.ColumnIndex == colLastRecord.Index)
        {
            if (!int.TryParse(Convert.ToString(e.FormattedValue), out var num) || num < 1 || num > nudCountRecords.Value)
                e.Cancel = true;
        }
        else if (e.ColumnIndex == colStartRow.Index)
        {
            if (!int.TryParse(Convert.ToString(e.FormattedValue), out var num) || num < 1)
                e.Cancel = true;
        }
    }

    private void dgvZostava_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (!loading && e.RowIndex != -1)
            zostavaChanged = true;
    }

    private void dgvZostava_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        // vyber v combo bunke sa ma prejavit hned, nie az po opusteni bunky
        if (dgvZostava.IsCurrentCellDirty && dgvZostava.CurrentCell is DataGridViewComboBoxCell)
            dgvZostava.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void dgvZostava_DataError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        // neplatny vstup - bunka ostane v editacii (hodnota sa kontroluje v CellValidating a pri ulozeni)
        e.ThrowException = false;
        e.Cancel = true;
    }

    private void FTableLogical_HelpButtonClicked(object sender, CancelEventArgs e)
    {
        Utils.OpenShell(LinkConsts.LINK_TLOGICAL);
    }
}
