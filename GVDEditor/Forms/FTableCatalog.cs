using GVDEditor.Entities;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Nastavenie katalogej tabule.
/// </summary>
public partial class FTableCatalog : Form
{
    private readonly BindingList<TableItem> Columns;

    private readonly bool copy;
    private readonly Color defaultBorderColor;
    private readonly BindingList<TableSegment> Rows;

    private readonly BindingList<TableTabTab> TabTabs1;
    private readonly BindingList<TableTabTab> TabTabs2;

    private readonly BindingList<TableViewTypeTab> ViewTypeTabs;

    /// <summary>
    ///     Tato katalogova tabula.
    /// </summary>
    public TableCatalog ThisTable;

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FTableCatalog"/>.
    /// </summary>
    /// <param name="table">Tato tabula.</param>
    /// <param name="tabtabs">Tabtabs.</param>
    /// <param name="copy">Ci sa jedna o kopiu.</param>
    public FTableCatalog(TableCatalog table, IList<TableTabTab> tabtabs, bool copy = false)
    {
        InitializeComponent();
        FormUtils.SetFormFont(this);
        this.ApplyTheme();

        ThisTable = table;
        this.copy = copy;

        defaultBorderColor = nudFont.BorderColor;

        ViewTypeTabs = new BindingList<TableViewTypeTab>(ThisTable.ViewTypeTabs);

        tabtabs.Insert(0, TableTabTab.Empty);

        TabTabs1 = new BindingList<TableTabTab>(tabtabs);

        TabTabs2 = new BindingList<TableTabTab>(tabtabs);

        cbManufacturer.DataSource = TableManufacturer.GetValues();
        cbColumnFill.DataSource = TableFillSection.GetValues();
        cbDivType.DataSource = TableDivType.GetValues();

        cbTab1.DataSource = TabTabs1;
        cbTab2.DataSource = TabTabs2;

        Columns = new BindingList<TableItem>(table.Items);
        listColumns.DataSource = Columns;

        Rows = new BindingList<TableSegment>(table.Segments);
        listRows.DataSource = Rows;

        ThisTable = table;
        tbKey.Text = table.Key;
        tbName.Text = table.Name;

        tbComment.Text = table.Comment;

        if (table.Manufacturer == null)
            cbManufacturer.SelectedIndex = 0;
        else
            cbManufacturer.SelectedItem = table.Manufacturer;

        nudMaxRecCount.Value = table.MaxRecCount;
        nudMinHeight.Value = table.MinHeight;

        if (Columns.Count == 0)
        {
            cbColumnFill.SelectedIndex = 0;
            cbDivType.SelectedIndex = 0;
        }
    }

    private void listColumns_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Columns.Count != 0)
        {
            var item = (TableItem)listColumns.SelectedItem;
            tbColumnKey.Text = item.Key;
            tbColumnName.Text = item.Name;
            cbColumnFill.SelectedItem = item.FillSection;
            cbDivType.SelectedItem = item.DivType;

            if (item.Align == TableAlign.Left)
                rbLeft.Checked = true;
            else if (item.Align == TableAlign.Center)
                rbCenter.Checked = true;
            else if (item.Align == TableAlign.Right)
                rbRight.Checked = true;

            cbTab1.SelectedIndex = TabTabs1.IndexOf(item.Tab1);
            cbTab2.SelectedIndex = TabTabs2.IndexOf(item.Tab2);

            nudStart.Value = item.Start;
            nudEnd.Value = item.End;
            nudLine.Value = item.Line;
            nudFont.Value = item.FontIDX;
        }
    }

    private void bColumnAdd_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tbColumnKey.Text) || string.IsNullOrEmpty(tbColumnName.Text))
        {
            Utils.ShowError(Resources.FTableCatalog_Nie_sú_vyplnené_všetky_povinné_parametre);
            return;
        }

        foreach (var col in Columns)
            if (col.Key == tbColumnKey.Text)
            {
                Utils.ShowError(Resources.FTableCatalog_Zadaný_kľúč_stĺpca_už_existuje);
                return;
            }

        var start = decimal.ToInt32(nudStart.Value);
        var end = decimal.ToInt32(nudEnd.Value);
        var line = decimal.ToInt32(nudLine.Value);

        if (start >= end)
        {
            Utils.ShowError(Resources.FTableCatalog_Posledný_pixel_stĺpca_je_menší_alebo_rovný_ako_počiatočný);
            return;
        }

        var item = new TableItem { Key = tbColumnKey.Text, Name = tbColumnName.Text };

        if (rbCenter.Checked)
            item.Align = TableAlign.Center;
        else if (rbLeft.Checked)
            item.Align = TableAlign.Left;
        else if (rbLeft.Checked)
            item.Align = TableAlign.Right;

        item.Start = start;
        item.End = end;
        item.Line = line;

        item.DivType = (TableDivType)cbDivType.SelectedItem;

        item.FillSection = (TableFillSection)cbColumnFill.SelectedItem;

        item.FontIDX = decimal.ToInt32(nudFont.Value);

        item.Tab1 = (TableTabTab)cbTab1.SelectedItem;
        item.Tab2 = (TableTabTab)cbTab2.SelectedItem;

        if (CheckDivType(item.DivType, item.Tab1, item.Tab2)) Columns.Add(item);
    }

    private void bColumnEdit_Click(object sender, EventArgs e)
    {
        if (listColumns.SelectedIndex != -1)
        {
            if (string.IsNullOrEmpty(tbColumnKey.Text) || string.IsNullOrEmpty(tbColumnName.Text))
            {
                Utils.ShowError(Resources.FTableCatalog_Nie_sú_vyplnené_všetky_povinné_parametre);
                return;
            }

            for (var i = 0; i < Columns.Count; i++)
                if (Columns[i].Key == tbColumnKey.Text && i != listColumns.SelectedIndex)
                {
                    Utils.ShowError(Resources.FTableCatalog_Zadaný_kľúč_stĺpca_už_existuje);
                    return;
                }

            var div = (TableDivType)cbDivType.SelectedItem;
            var tab1 = (TableTabTab)cbTab1.SelectedItem;
            var tab2 = (TableTabTab)cbTab2.SelectedItem;

            if (!CheckDivType(div, tab1, tab2))
                return;

            var start = decimal.ToInt32(nudStart.Value);
            var end = decimal.ToInt32(nudEnd.Value);
            var line = decimal.ToInt32(nudLine.Value);

            if (start >= end)
            {
                Utils.ShowError(Resources.FTableCatalog_Posledný_pixel_stĺpca_je_menší_alebo_rovný_ako_počiatočný);
                return;
            }

            var item = Columns[listColumns.SelectedIndex];
            item.Key = tbColumnKey.Text;
            item.Name = tbColumnName.Text;

            if (rbCenter.Checked)
                item.Align = TableAlign.Center;
            else if (rbLeft.Checked)
                item.Align = TableAlign.Left;
            else if (rbLeft.Checked)
                item.Align = TableAlign.Right;

            item.Start = start;
            item.End = end;
            item.Line = line;

            item.DivType = div;

            item.FillSection = (TableFillSection)cbColumnFill.SelectedItem;

            item.FontIDX = decimal.ToInt32(nudFont.Value);

            item.Tab1 = tab1;
            item.Tab2 = tab2;

            Columns.ResetBindings();
        }
    }

    private static bool CheckDivType(TableDivType div, TableTabTab tab1, TableTabTab tab2)
    {
        if (div == TableDivType.Free)
        {
            if (tab1 != TableTabTab.Empty) return ShowWarningDefTab(true);
            if (tab2 != TableTabTab.Empty) return ShowWarningDefTab(false);
        }
        else if (div == TableDivType.Table || div == TableDivType.Translate || div == TableDivType.Char)
        {
            if (tab1 == TableTabTab.Empty)
            {
                ShowErrorUnDefTab(true);
                return false;
            }

            if (tab2 != TableTabTab.Empty) return ShowWarningDefTab(false);
        }
        else if (div == TableDivType.TableTime)
        {
            if (tab1 == TableTabTab.Empty)
            {
                ShowErrorUnDefTab(true);
                return false;
            }

            if (tab2 == TableTabTab.Empty)
            {
                ShowErrorUnDefTab(false);
                return false;
            }
        }

        return true;
    }

    private static bool ShowWarningDefTab(bool tab1)
    {
        var result = Utils.ShowWarning(string.Format(Resources.FTableCatalog_ShowWarningDefTab, tab1 ? "TAB1" : "TAB2"), MessageBoxButtons.YesNo);
        return result != DialogResult.No;
    }

    private static void ShowErrorUnDefTab(bool tab1)
    {
        Utils.ShowError(string.Format(Resources.FTableCatalog_ShowErrorUnDefTab, tab1 ? "TAB1" : "TAB2"));
    }

    private void bColumnDelete_Click(object sender, EventArgs e)
    {
        if (listColumns.SelectedIndex != -1) Columns.RemoveAt(listColumns.SelectedIndex);
    }

    private void listRows_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Rows.Count != 0 && listRows.SelectedItem != null)
        {
            var segment = (TableSegment)listRows.SelectedItem;
            nudHeight.Value = segment.Height;
            nudWidth.Value = segment.Width;
            nudSize.Value = segment.Size;
        }
    }

    private void bRowEdit_Click(object sender, EventArgs e)
    {
        if (listRows.SelectedIndex != -1)
        {
            var segment = Rows[listRows.SelectedIndex];
            segment.Width = decimal.ToInt32(nudWidth.Value);
            segment.Height = decimal.ToInt32(nudHeight.Value);
            segment.Size = decimal.ToInt32(nudSize.Value);

            Rows.ResetBindings();
        }
    }

    private void listRows_Format(object sender, ListControlConvertEventArgs e)
    {
        var segment = (TableSegment)e.ListItem;
        e.Value = "W:" + segment.Width + "; H:" + segment.Height + "; S:" + segment.Size;
    }

    private void nudMaxRecCount_ValueChanged(object sender, EventArgs e)
    {
        if (Rows.Count > nudMaxRecCount.Value)
        {
            for (var i = 0; i < Rows.Count; i++)
                if (i > nudMaxRecCount.Value)
                    Rows.RemoveAt(i);
        }
        else if (Rows.Count < nudMaxRecCount.Value)
        {
            for (var i = Rows.Count; i < nudMaxRecCount.Value; i++)
                Rows.Add(new TableSegment { Height = 0, Size = 0, Width = 0 });
        }
    }

    private void bSetAll_Click(object sender, EventArgs e)
    {
        var segment = new TableSegment
        {
            Size = decimal.ToInt32(nudSize.Value),
            Width = decimal.ToInt32(nudWidth.Value),
            Height = decimal.ToInt32(nudHeight.Value)
        };

        for (var i = 0; i < Rows.Count; i++) Rows[i] = segment;
    }

    private void bSetColumnOrder_Click(object sender, EventArgs e)
    {
        var etcof = new FTableColumnOrder(Columns.ToList(), ViewTypeTabs.ToList());
        var result = etcof.ShowDialog();
        if (result.Equals(DialogResult.OK))
        {
            ViewTypeTabs.Clear();
            foreach (var tab in etcof.ItemsTypeTabs) ViewTypeTabs.Add(tab);
        }
    }

    private void bSave_Click(object sender, EventArgs e)
    {
        var table = copy ? new TableCatalog() : ThisTable;

        if (string.IsNullOrEmpty(tbKey.Text) || string.IsNullOrEmpty(tbName.Text))
        {
            Utils.ShowError(Resources.Tables_Nie_sú_vyplnené_všetky_povinné_polia);
            DialogResult = DialogResult.None;
            return;
        }

        foreach (var t in FLocalSettings.Catalogs)
            if (t.Key == tbName.Text && !table.Equals(t))
            {
                Utils.ShowError(Resources.Tables_Zadaný_kľúč_tabule_už_existuje);
                DialogResult = DialogResult.None;
                return;
            }

        var end = 0;
        var line = 0;
        for (var i = 0; i < Columns.Count; i++)
        {
            if (Columns[i].Line != line)
            {
                if (Columns[i].Line != line + 1)
                {
                    Utils.ShowError(string.Format(Resources.FTableCatalog_Definované_stĺpce_nie_sú_zotriedené_podľa_zobrazovaného_riadka_záznamu,
                        Columns[i - 1].Name, Columns[i - 1].Line, Columns[i].Name, Columns[i].Line));
                    DialogResult = DialogResult.None;
                    return;
                }

                end = 0;
                line = Columns[i].Line;
            }

            if (Columns[i].End < end)
            {
                Utils.ShowError(string.Format(Resources.FTableCatalog_Definované_stĺpce_nie_sú_zotriedené_podľa_pozície, Columns[i - 1].Name,
                    Columns[i - 1].End, Columns[i].Name, Columns[i].End));
                DialogResult = DialogResult.None;
                return;
            }

            end = Columns[i].End;
        }

        var manufacturer = cbManufacturer.SelectedItem as TableManufacturer;
        if (manufacturer == TableManufacturer.ELEN && end > 512)
        {
            Utils.ShowError(Resources.FTableCatalog_Tabula_ELEN_moze_mat_max_poziciu_512);
            DialogResult = DialogResult.None;
            return;
        }

        var modesCount = TableViewMode.GetValues().Count;
        foreach (var tab in ViewTypeTabs)
        {
            var c = tab.TypeModeItems.Count;
            if (c != modesCount)
            {
                Utils.ShowError(string.Format(Resources.FTableCatalog_bSave_NespravnyPocetModov, tab.ViewType, c, modesCount));
                DialogResult = DialogResult.None;
                return;
            }
        }

        table.Key = tbKey.Text;
        table.Name = tbName.Text;
        table.Items = Columns.ToList();
        table.Manufacturer = manufacturer;
        table.MaxRecCount = decimal.ToInt32(nudMaxRecCount.Value);
        table.Segments = Rows.ToList();
        table.ViewTypeTabs = ViewTypeTabs.ToList();
        table.MinHeight = decimal.ToInt32(nudMinHeight.Value);

        table.Comment = tbComment.Text;

        if (copy) ThisTable = table;

        DialogResult = DialogResult.OK;
    }

    private void bStorno_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

    private void nudHeight_Validating(object sender, CancelEventArgs e)
    {
        if (nudHeight.Value > nudMinHeight.Value) 
            e.Cancel = true;
    }

    private void nudFont_ValueChanged(object sender, EventArgs e)
    {
        switch (nudFont.Value % 4)
        {
            case 1:
                nudFont.BorderColor = defaultBorderColor;
                nudFont.ArrowsColor = Color.White;
                nudFont.BackColor = Color.Red;
                nudFont.ForeColor = Color.White;
                break;
            case 2:
                nudFont.BorderColor = defaultBorderColor;
                nudFont.ArrowsColor = Color.White;
                nudFont.BackColor = Color.Green;
                nudFont.ForeColor = Color.White;
                break;
            case 3:
                nudFont.BorderColor = defaultBorderColor;
                nudFont.ArrowsColor = Color.Black;
                nudFont.BackColor = Color.Yellow;
                nudFont.ForeColor = Color.Black;
                break;
            default:
                nudFont.BorderColor = Color.DimGray;
                nudFont.ArrowsColor = Color.Black;
                nudFont.BackColor = Color.White;
                nudFont.ForeColor = Color.Black;
                break;
        }
    }

    private void FTableCatalog_HelpButtonClicked(object sender, CancelEventArgs e)
    {
        Process.Start(LinkConsts.LINK_TCATALOG);
    }

    private void bUp_Click(object sender, EventArgs e)
    {
        if (listColumns.SelectedIndex >= 0)
        {
            var sel = listColumns.SelectedIndex;
            var item = Columns[sel];

            if (sel - 1 >= 0)
            {
                Columns.RemoveAt(sel);
                Columns.Insert(sel - 1, item);
                listColumns.SelectedIndex = sel - 1;
            }
        }
    }

    private void bDown_Click(object sender, EventArgs e)
    {
        if (listColumns.SelectedIndex >= 0)
        {
            var sel = listColumns.SelectedIndex;
            var item = Columns[sel];

            if (sel + 1 < Columns.Count)
            {
                Columns.RemoveAt(sel);
                Columns.Insert(sel + 1, item);
                listColumns.SelectedIndex = sel + 1;
            }
        }
    }
}