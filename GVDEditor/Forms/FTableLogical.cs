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
    private readonly BindingList<TableLogicalZostava> TVybrane;

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
        InitializeComponent();
        this.ApplyThemeAndFonts();

        ThisTable = table;
        this.copy = copy;

        FillStations(thisStation);

        var vybrane = new HashSet<TablePhysical>();
        var zostava = new List<TableLogicalZostava>();

        foreach (var record in ThisTable.Records)
        foreach (TablePosition position in record)
            if (vybrane.Contains(position.Table))
            {
                foreach (var logicalZostava in zostava.Where(logicalZostava =>
                             logicalZostava.Table.Equals(position.Table)))
                    logicalZostava.EndRow = position.Position + 1;
            }
            else
            {
                vybrane.Add(position.Table);
                zostava.Add(new TableLogicalZostava
                {
                    Table = position.Table, StartRow = position.Position + 1, EndRow = position.Position + 1
                });
            }

        TVybrane = new BindingList<TableLogicalZostava>(zostava);

        cbTypeView.DataSource = TableViewType.GetValues();
        if (ThisTable.ViewType != null) cbTypeView.SelectedItem = ThisTable.ViewType;

        listFyzTab.DataSource = tables;
        dgvZostava.DataSource = TVybrane;

        tbName.Text = ThisTable.Name;
        tbKey.Text = ThisTable.Key;

        tbComment.Text = table.Comment;

        nudCountRecords.Value = ThisTable.Records.Count;

        // IDSTATION: 0 = neuvedene (prazdne pole), inak stanica zo zoznamu alebo vlastne cislo
        if (ThisTable.IdStation != 0)
        {
            var match = cbIdStation.Items.Cast<StationItem>().FirstOrDefault(item => item.Id == ThisTable.IdStation);
            if (match != null)
                cbIdStation.SelectedItem = match;
            else
                cbIdStation.Text = ThisTable.IdStation.ToString();
        }
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

    private void bSave_Click(object sender, EventArgs e)
    {
        var table = copy ? new TableLogical() : ThisTable;

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

        table.Key = tbKey.Text;
        table.Name = tbName.Text;
        table.ViewType = (TableViewType)cbTypeView.SelectedItem!;

        var records = new List<TableRecord>();

        for (var i = 0; i < nudCountRecords.Value; i++)
        {
            var positions = new List<TablePosition>();
            foreach (var zostava in TVybrane)
                if (i >= zostava.StartRow - 1 && i <= zostava.EndRow - 1)
                    positions.Add(new TablePosition
                    {
                        Table = zostava.Table, Position = i,
                        TypeView = (TableViewType)cbTypeView.SelectedItem!
                    });

            records.Add(new TableRecord { Positions = positions });
        }

        table.Records = records;

        table.Comment = tbComment.Text;

        table.IdStation = idStation.Value;

        if (copy) ThisTable = table;

        DialogResult = DialogResult.OK;
    }

    private void bStorno_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

    private void bAddTab_Click(object sender, EventArgs e)
    {
        if (listFyzTab.SelectedIndex != -1)
        {
            var fyztab = (TablePhysical)listFyzTab.SelectedItem!;
            var found = false;
            foreach (var zostava in TVybrane)
                if (zostava.Table.Equals(fyztab))
                    found = true;

            if (!found)
                TVybrane.Add(new TableLogicalZostava
                    { Table = fyztab, StartRow = 0, EndRow = decimal.ToInt32(nudCountRecords.Value) });
        }
    }

    private void bRemoveTab_Click(object sender, EventArgs e)
    {
        if (dgvZostava.SelectedRows.Count != 0) TVybrane.RemoveAt(dgvZostava.SelectedRows[0].Index);
    }

    private void listFyzTab_DoubleClick(object sender, EventArgs e)
    {
        if (listFyzTab.SelectedIndex != -1)
        {
            var fyztab = (TablePhysical)listFyzTab.SelectedItem!;
            var found = false;
            foreach (var zostava in TVybrane)
                if (zostava.Table.Equals(fyztab))
                    found = true;

            if (!found)
                TVybrane.Add(new TableLogicalZostava
                    { Table = fyztab, StartRow = 0, EndRow = decimal.ToInt32(nudCountRecords.Value) });
        }
    }

    private void dgvZostava_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
        if (e.RowIndex != -1)
            switch (e.ColumnIndex)
            {
                case 0:
                {
                    var num = int.Parse((string)e.FormattedValue!);
                    if (num < 1) e.Cancel = true;

                    break;
                }
                case 1:
                {
                    var num = int.Parse((string)e.FormattedValue!);
                    if (num > nudCountRecords.Value) e.Cancel = true;

                    break;
                }
            }
    }

    private void FTableLogical_HelpButtonClicked(object sender, CancelEventArgs e)
    {
        Utils.OpenShell(LinkConsts.LINK_TLOGICAL);
    }

    private class TableLogicalZostava
    {
        public TablePhysical Table { get; set; } = null!;
        public int StartRow { get; set; }
        public int EndRow { get; set; }
    }
}