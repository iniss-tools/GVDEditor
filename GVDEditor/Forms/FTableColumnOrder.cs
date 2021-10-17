using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GVDEditor.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Forms
{
    /// <summary>
    ///     Dialog - Nastavenie poradia stlpcov katalogej tabule.
    /// </summary>
    public partial class FTableColumnOrder : Form
    {
        private readonly BindingList<TableItem> AllItems;
        private readonly bool initialization;
        private readonly BindingList<TableItem> OrderedItems = new();

        /// <summary>
        /// </summary>
        public BindingList<TableViewTypeTab> ItemsTypeTabs;

        private TableViewMode selectedMode;

        private TableViewType selectedType;

        /// <summary>
        ///     Vytvori novy formular typu <see cref="FTableColumnOrder"/>.
        /// </summary>
        /// <param name="items">Stlpce.</param>
        /// <param name="itemsTypeTabs">Typy pohladov.</param>
        public FTableColumnOrder(IList<TableItem> items, IList<TableViewTypeTab> itemsTypeTabs)
        {
            InitializeComponent();
            FormUtils.SetFormFont(this);
            this.ApplyTheme();

            AllItems = new BindingList<TableItem>(items);
            ItemsTypeTabs = new BindingList<TableViewTypeTab>(itemsTypeTabs);

            initialization = true;
            cbViewType.DataSource = TableViewType.GetValues();
            cbViewMode.DataSource = TableViewMode.GetValues();
            initialization = false;

            listColumns.DataSource = AllItems;
            listOrder.DataSource = OrderedItems;

            if (ItemsTypeTabs.Count != 0)
                cbViewType.SelectedItem = ItemsTypeTabs[0].ViewType;
            else
                cbViewType.SelectedIndex = 0;
            cbViewMode.SelectedIndex = 0;
        }

        private void cbViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!initialization) SaveTypeModeItems();

            OrderedItems.Clear();

            foreach (var tab in ItemsTypeTabs)
                if (tab.ViewType == cbViewType.SelectedItem as TableViewType)
                {
                    nudTypeCountLines.Value = int.Parse(tab.CountLinesRecord);
                    foreach (TableTypeModeItem item in tab)
                        if (item.ViewMode == cbViewMode.SelectedItem as TableViewMode)
                            foreach (string s in item)
                            foreach (var i in AllItems)
                                if (i.Key == s)
                                    OrderedItems.Add(i);
                }

            selectedType = cbViewType.SelectedItem as TableViewType;
            selectedMode = cbViewMode.SelectedItem as TableViewMode;
        }

        private void cbViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!initialization) SaveTypeModeItems();

            OrderedItems.Clear();

            foreach (var tab in ItemsTypeTabs)
                if (tab.ViewType == cbViewType.SelectedItem as TableViewType)
                {
                    nudTypeCountLines.Value = int.Parse(tab.CountLinesRecord);
                    foreach (TableTypeModeItem item in tab)
                        if (item.ViewMode == cbViewMode.SelectedItem as TableViewMode)
                            foreach (string s in item)
                            foreach (var i in AllItems)
                                if (i.Key == s)
                                    OrderedItems.Add(i);
                }

            selectedType = cbViewType.SelectedItem as TableViewType;
            selectedMode = cbViewMode.SelectedItem as TableViewMode;
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            if (listColumns.SelectedIndex != -1) 
                OrderedItems.Add((TableItem)listColumns.SelectedItem);
        }

        private void listColumns_DoubleClick(object sender, EventArgs e)
        {
            if (listColumns.SelectedIndex != -1) 
                OrderedItems.Add((TableItem)listColumns.SelectedItem);
        }

        private void listOrder_DoubleClick(object sender, EventArgs e)
        {
            if (listOrder.SelectedIndex != -1) 
                OrderedItems.RemoveAt(listOrder.SelectedIndex);
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            if (listOrder.SelectedIndex != -1) 
                OrderedItems.RemoveAt(listOrder.SelectedIndex);
        }

        private void bSetForAll_Click(object sender, EventArgs e)
        {
            var items = new List<string>();
            foreach (var i in OrderedItems) items.Add(i.Key);

            var types = ItemsTypeTabs.Select(tt => tt.ViewType).ToList();

            if (types.Contains(selectedType))
            {
                var indexT = types.IndexOf(selectedType);
                ItemsTypeTabs[indexT].TypeModeItems.Clear();

                foreach (var tableViewMode in TableViewMode.GetValues())
                {
                    var ti = new TableTypeModeItem { ViewMode = tableViewMode, ItemsKeys = items };
                    ItemsTypeTabs[indexT].TypeModeItems.Add(ti);
                }
            }
            else
            {
                var tmi = new TableTypeModeItem { ItemsKeys = items, ViewMode = selectedMode };

                var tvtt = new TableViewTypeTab
                {
                    ViewType = selectedType,
                    CountLinesRecord = decimal.ToInt32(nudTypeCountLines.Value).ToString()
                };
                tvtt.TypeModeItems.Add(tmi);

                ItemsTypeTabs.Add(tvtt);
            }
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
            var data = (TableItem)e.Data.GetData(typeof(TableItem));
            OrderedItems.Remove(data);
            OrderedItems.Insert(index, data);
            listOrder.SelectedItem = data;
        }

        private void SaveTypeModeItems()
        {
            var types = ItemsTypeTabs.Select(tt => tt.ViewType).ToList();

            if (types.Contains(selectedType))
            {
                var indexT = types.IndexOf(selectedType);
                var modes = ItemsTypeTabs[indexT].TypeModeItems.Select(tm => tm.ViewMode).ToList();

                if (modes.Contains(selectedMode))
                {
                    var indexM = modes.IndexOf(selectedMode);
                    ItemsTypeTabs[indexT].CountLinesRecord = decimal.ToInt32(nudTypeCountLines.Value).ToString();
                    ItemsTypeTabs[indexT].TypeModeItems[indexM].ItemsKeys.Clear();

                    foreach (var i in OrderedItems)
                        ItemsTypeTabs[indexT].TypeModeItems[indexM].ItemsKeys.Add(i.Key);
                }
                else
                {
                    var keys = new List<string>();
                    foreach (var i in OrderedItems) keys.Add(i.Key);

                    ItemsTypeTabs[indexT].CountLinesRecord = decimal.ToInt32(nudTypeCountLines.Value).ToString();

                    var tmi = new TableTypeModeItem { ItemsKeys = keys, ViewMode = selectedMode };
                    ItemsTypeTabs[indexT].TypeModeItems.Add(tmi);
                }
            }
            else
            {
                var keys = new List<string>();
                foreach (var i in OrderedItems) keys.Add(i.Key);

                var tmi = new TableTypeModeItem { ItemsKeys = keys, ViewMode = selectedMode };

                var tvtt = new TableViewTypeTab
                {
                    ViewType = selectedType,
                    CountLinesRecord = decimal.ToInt32(nudTypeCountLines.Value).ToString()
                };
                tvtt.TypeModeItems.Add(tmi);

                ItemsTypeTabs.Add(tvtt);
            }
        }

        private void FTableColumnOrder_HelpButtonClicked(object sender, CancelEventArgs e) => 
            Process.Start(LinkConsts.LINK_TCOLUMN_ORDER);
    }
}