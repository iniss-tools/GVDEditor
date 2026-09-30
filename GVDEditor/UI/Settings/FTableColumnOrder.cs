using System.Globalization;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using ToolsCore.Tools;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Dialog - Nastavenie poradia stlpcov katalogej tabule.
/// </summary>
public partial class FTableColumnOrder : Form
{
    private readonly BindingList<TableItem> _allItems;
    private readonly bool _initialization;
    private readonly BindingList<TableItem> _orderedItems = [];

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public BindingList<TableViewTypeTab> ItemsTypeTabs { get; set; }

    private TableViewMode _selectedMode = null!;

    private TableViewType _selectedType = null!;

    /// <summary>
    /// Vytvori novy formular typu <see cref="FTableColumnOrder"/>.
    /// </summary>
    /// <param name="items">Stlpce.</param>
    /// <param name="itemsTypeTabs">Typy pohladov.</param>
    public FTableColumnOrder(IList<TableItem> items, IList<TableViewTypeTab> itemsTypeTabs)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();

        _allItems = new BindingList<TableItem>(items);
        ItemsTypeTabs = new BindingList<TableViewTypeTab>(itemsTypeTabs);

        _initialization = true;
        cbViewType.DataSource = TableViewType.GetValues();
        cbViewMode.DataSource = TableViewMode.GetValues();
        _initialization = false;

        listColumns.DataSource = _allItems;
        listOrder.DataSource = _orderedItems;

        if (ItemsTypeTabs.Count != 0)
            cbViewType.SelectedItem = ItemsTypeTabs[0].ViewType;
        else
            cbViewType.SelectedIndex = 0;
        cbViewMode.SelectedIndex = 0;
    }

    private void cbViewType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!_initialization) SaveTypeModeItems();

        _orderedItems.Clear();

        foreach (var tab in ItemsTypeTabs)
            if (tab.ViewType == cbViewType.SelectedItem as TableViewType)
            {
                nudTypeCountLines.Value = int.Parse(tab.CountLinesRecord, CultureInfo.CurrentCulture);
                foreach (TableTypeModeItem item in tab)
                    if (item.ViewMode == cbViewMode.SelectedItem as TableViewMode)
                        foreach (string s in item)
                        foreach (var i in _allItems)
                            if (i.Key == s)
                                _orderedItems.Add(i);
            }

        _selectedType = (TableViewType)cbViewType.SelectedItem!;
        _selectedMode = (TableViewMode)cbViewMode.SelectedItem!;
    }

    private void cbViewMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!_initialization) SaveTypeModeItems();

        _orderedItems.Clear();

        foreach (var tab in ItemsTypeTabs)
            if (tab.ViewType == cbViewType.SelectedItem as TableViewType)
            {
                nudTypeCountLines.Value = int.Parse(tab.CountLinesRecord, CultureInfo.CurrentCulture);
                foreach (TableTypeModeItem item in tab)
                    if (item.ViewMode == cbViewMode.SelectedItem as TableViewMode)
                        foreach (string s in item)
                        foreach (var i in _allItems)
                            if (i.Key == s)
                                _orderedItems.Add(i);
            }

        _selectedType = (TableViewType)cbViewType.SelectedItem!;
        _selectedMode = (TableViewMode)cbViewMode.SelectedItem!;
    }

    private void bAdd_Click(object sender, EventArgs e)
    {
        if (listColumns.SelectedIndex != -1) 
            _orderedItems.Add((TableItem)listColumns.SelectedItem!);
    }

    private void listColumns_DoubleClick(object sender, EventArgs e)
    {
        if (listColumns.SelectedIndex != -1) 
            _orderedItems.Add((TableItem)listColumns.SelectedItem!);
    }

    private void listOrder_DoubleClick(object sender, EventArgs e)
    {
        if (listOrder.SelectedIndex != -1) 
            _orderedItems.RemoveAt(listOrder.SelectedIndex);
    }

    private void bDelete_Click(object sender, EventArgs e)
    {
        if (listOrder.SelectedIndex != -1) 
            _orderedItems.RemoveAt(listOrder.SelectedIndex);
    }

    private void bSetForAll_Click(object sender, EventArgs e)
    {
        var keys = _orderedItems.Select(i => i.Key);
        var tab = ItemsTypeTabs.FirstOrDefault(tt => tt.ViewType == _selectedType);

        if (tab == null)
        {
            tab = new TableViewTypeTab { ViewType = _selectedType };
            ItemsTypeTabs.Add(tab);
        }

        tab.CountLinesRecord = decimal.ToInt32(nudTypeCountLines.Value).ToString(CultureInfo.CurrentCulture);
        // vsetky mody (kazdy s vlastnou kopiou zoznamu klucov)
        TableCatalogEditing.SetAllModes(tab, keys);
    }

    private void bSave_Click(object sender, EventArgs e)
    {
        SaveTypeModeItems();
        DialogResult = DialogResult.OK;
    }

    private void listOrder_MouseDown(object sender, MouseEventArgs e)
    {
        if (listOrder.SelectedItem == null) return;
        listOrder.DoDragDrop(listOrder.SelectedItem, DragDropEffects.Move);
    }

    private void listOrder_DragOver(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Move;

    private void listOrder_DragDrop(object sender, DragEventArgs e)
    {
        var point = listOrder.PointToClient(new Point(e.X, e.Y));
        var index = listOrder.IndexFromPoint(point);
        if (index < 0) index = listOrder.Items.Count - 1;
        var data = (TableItem)e.Data!.GetData(typeof(TableItem))!;
        _orderedItems.Remove(data);
        _orderedItems.Insert(index, data);
        listOrder.SelectedItem = data;
    }

    private void SaveTypeModeItems()
    {
        var types = ItemsTypeTabs.Select(tt => tt.ViewType).ToList();

        if (types.Contains(_selectedType))
        {
            var indexT = types.IndexOf(_selectedType);
            var modes = ItemsTypeTabs[indexT].TypeModeItems.Select(tm => tm.ViewMode).ToList();

            if (modes.Contains(_selectedMode))
            {
                var indexM = modes.IndexOf(_selectedMode);
                ItemsTypeTabs[indexT].CountLinesRecord = decimal.ToInt32(nudTypeCountLines.Value).ToString(CultureInfo.CurrentCulture);
                ItemsTypeTabs[indexT].TypeModeItems[indexM].ItemsKeys.Clear();

                foreach (var i in _orderedItems)
                    ItemsTypeTabs[indexT].TypeModeItems[indexM].ItemsKeys.Add(i.Key);
            }
            else
            {
                var keys = new List<string>();
                foreach (var i in _orderedItems) keys.Add(i.Key);

                ItemsTypeTabs[indexT].CountLinesRecord = decimal.ToInt32(nudTypeCountLines.Value).ToString(CultureInfo.CurrentCulture);

                var tmi = new TableTypeModeItem { ItemsKeys = keys, ViewMode = _selectedMode };
                ItemsTypeTabs[indexT].TypeModeItems.Add(tmi);
            }
        }
        else
        {
            var keys = new List<string>();
            foreach (var i in _orderedItems) keys.Add(i.Key);

            var tmi = new TableTypeModeItem { ItemsKeys = keys, ViewMode = _selectedMode };

            var tvtt = new TableViewTypeTab
            {
                ViewType = _selectedType,
                CountLinesRecord = decimal.ToInt32(nudTypeCountLines.Value).ToString(CultureInfo.CurrentCulture)
            };
            tvtt.TypeModeItems.Add(tmi);

            ItemsTypeTabs.Add(tvtt);
        }
    }

    private void FTableColumnOrder_HelpButtonClicked(object sender, CancelEventArgs e) => 
        Utils.OpenShell(GvdLinkConsts.LinkTcolumnOrder);
}