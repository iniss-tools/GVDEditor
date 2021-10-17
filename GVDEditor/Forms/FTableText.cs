using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GVDEditor.Entities;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.Forms
{
    /// <summary>
    ///     Dialog - Nastavenie textov do tabul.
    /// </summary>
    public partial class FTableText : Form
    {
        private readonly GVDInfo gvd;
        private readonly int row;
        private readonly BindingList<TableItem> TableItems;
        private readonly BindingList<TableTrain> TextTrains;

        private readonly BindingList<TableTextRealization> TRealizations;

        /// <summary>
        ///     Tieto texty do tabul.
        /// </summary>
        public TableText ThisTableText;

        /// <summary>
        ///     Vytvori novy formular typu <see cref="FTableText"/>.
        /// </summary>
        /// <param name="tableText">Tieto texty do tabul.</param>
        /// <param name="catalogs">Dostupne katalogove tabule.</param>
        /// <param name="gvd">Aktualny grafikon.</param>
        /// <param name="row">ID riadku v liste.</param>
        public FTableText(TableText tableText, IReadOnlyCollection<TableCatalog> catalogs, GVDInfo gvd, int row)
        {
            InitializeComponent();
            FormUtils.SetFormFont(this);
            this.ApplyTheme();

            ThisTableText = tableText;
            this.gvd = gvd;
            this.row = row;

            TRealizations = new BindingList<TableTextRealization>(tableText.Realizations);
            TextTrains = new BindingList<TableTrain>(tableText.Trains);
            TableItems = new BindingList<TableItem>();

            cbCatalogItem.DataSource = TableItems;
            cbCatalogTable.DataSource = catalogs;

            listRealisations.DataSource = TRealizations;
            listTrains.DataSource = TextTrains;

            tbKey.Text = tableText.Key;
            tbName.Text = tableText.Name;
            tbComment.Text = tableText.Comment;

            if (TRealizations.Count != 0)
            {
                listRealisations.SelectedItem = null;
                listRealisations.SelectedIndex = 0;
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            var ttext = new TableText();

            if (string.IsNullOrEmpty(tbKey.Text) || string.IsNullOrEmpty(tbName.Text))
            {
                Utils.ShowError(Resources.Tables_Nie_sú_vyplnené_všetky_povinné_polia);
                DialogResult = DialogResult.None;
                return;
            }

            if (row != -1)
            {
                var i = 0;
                foreach (var t in FLocalSettings.TTexts)
                {
                    if (t.Key == tbKey.Text && row != i)
                    {
                        Utils.ShowError(Resources.Tables_Zadaný_kľúč_tabule_už_existuje);
                        DialogResult = DialogResult.None;
                        return;
                    }

                    i++;
                }
            }
            else
            {
                foreach (var t in FLocalSettings.TTexts)
                    if (t.Key == tbKey.Text)
                    {
                        Utils.ShowError(Resources.Tables_Zadaný_kľúč_tabule_už_existuje);
                        DialogResult = DialogResult.None;
                        return;
                    }
            }


            foreach (var tt in TextTrains) tt.Text = Regex.Replace(tt.Text, @"\t|\n|\r", "");

            ttext.Key = tbKey.Text;
            ttext.Name = tbName.Text;
            ttext.Comment = tbComment.Text;
            ttext.Realizations = TRealizations.ToList();
            ttext.Trains = TextTrains.ToList();

            ThisTableText = ttext;

            DialogResult = DialogResult.OK;
        }

        private void bStorno_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

        private void listRealisations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listRealisations.SelectedItem is TableTextRealization realization)
            {
                cbCatalogTable.SelectedItem = realization.Table;
                cbCatalogItem.SelectedItem = realization.Item;
            }
        }

        private void listTrains_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listTrains.SelectedIndex != -1)
                if (listTrains.SelectedItem is TableTrain tableTrain)
                {
                    tbTrainText.Text = tableTrain.Text;
                    nudFont.Value = tableTrain.FontID;
                }
        }

        private void listRealisations_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is TableTextRealization realization)
                e.Value = realization.Table.Name + " - " + realization.Item.Name;
        }

        private void listTrains_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is TableTrain train)
                e.Value = train.Train.ID + ". " + train.Train.Number + " " + train.Train.Type + " " +
                          (!string.IsNullOrEmpty(train.Train.Name) ? train.Train.Name : "");
        }

        private void cbCatalogTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            TableItems.Clear();
            if (listRealisations.SelectedItem is TableTextRealization realization &&
                cbCatalogTable.SelectedItem is TableCatalog catalog)
            {
                foreach (var t in catalog.Items) TableItems.Add(t);

                cbCatalogItem.SelectedItem = realization.Item;
            }
            else
            {
                var tableItems = (cbCatalogTable.SelectedItem as TableCatalog)?.Items;
                if (tableItems != null)
                    foreach (var t in tableItems)
                        TableItems.Add(t);

                cbCatalogItem.SelectedIndex = 0;
            }
        }

        private void bReAdd_Click(object sender, EventArgs e)
        {
            var realization = new TableTextRealization
            {
                Table = (TableCatalog)cbCatalogTable.SelectedItem, Item = (TableItem)cbCatalogItem.SelectedItem
            };

            TRealizations.Add(realization);
        }

        private void bReEdit_Click(object sender, EventArgs e)
        {
            if (listRealisations.SelectedIndex != -1)
            {
                var realization = TRealizations[listRealisations.SelectedIndex];
                realization.Table = (TableCatalog)cbCatalogTable.SelectedItem;
                realization.Item = (TableItem)cbCatalogItem.SelectedItem;

                TRealizations.ResetBindings();
            }
        }

        private void bReDelete_Click(object sender, EventArgs e)
        {
            if (listRealisations.SelectedIndex != -1) TRealizations.RemoveAt(listRealisations.SelectedIndex);
        }

        private void bTextEdit_Click(object sender, EventArgs e)
        {
            if (listTrains.SelectedIndex != -1)
            {
                var tableTrain = (TableTrain)listTrains.SelectedItem;
                tableTrain.Text = tbTrainText.Text;
                tableTrain.FontID = decimal.ToInt32(nudFont.Value);
            }
        }

        private void bGenerate_Click(object sender, EventArgs e)
        {
            var result = Utils.ShowQuestion(Resources.FTableText_Generate_TTexts_Info);
            if (result == DialogResult.Yes)
            {
                TableItem item;
                if (cbCatalogItem.SelectedItem != null)
                {
                    item = (TableItem)cbCatalogItem.SelectedItem;

                    if (item.FillSection != TableFillSection.CielovaStanica
                        && item.FillSection != TableFillSection.CielovaStanicaNastupiste
                        && item.FillSection != TableFillSection.CielovaStanicaPodchod
                        && item.FillSection != TableFillSection.VychadzajucaStanica
                        && item.FillSection != TableFillSection.StaniceDoSmeru
                        && item.FillSection != TableFillSection.StaniceDoSmeruNastupiste
                        && item.FillSection != TableFillSection.StaniceZoSmeru)
                    {
                        Utils.ShowError(Resources.FTableText_Generate_TTexts_Wrong_Item);
                        return;
                    }
                }
                else
                {
                    return; //TODO
                }

                TextTrains.Clear();

                foreach (var vlak in GlobData.Trains)
                {
                    var tableTrain = new TableTrain { FontID = -1, Train = vlak };

                    if (item.FillSection == TableFillSection.CielovaStanica || item.FillSection == TableFillSection.CielovaStanicaNastupiste ||
                        item.FillSection == TableFillSection.CielovaStanicaPodchod)
                    {
                        if (vlak.Routing == Routing.Prechadzajuci || vlak.Routing == Routing.Vychadzajuci)
                            tableTrain.Text = vlak.StaniceDoSmeru.Last().Name;
                        else
                            tableTrain.Text = gvd.ThisStation.Name;
                    }
                    else if (item.FillSection == TableFillSection.VychadzajucaStanica)
                    {
                        if (vlak.Routing == Routing.Prechadzajuci || vlak.Routing == Routing.Konciaci)
                            tableTrain.Text = vlak.StaniceZoSmeru.First().Name;
                        else
                            tableTrain.Text = gvd.ThisStation.Name;
                    }
                    else if (item.FillSection == TableFillSection.StaniceDoSmeru || item.FillSection == TableFillSection.StaniceDoSmeruNastupiste)
                    {
                        var sb = new StringBuilder();

                        if (vlak.Routing == Routing.Prechadzajuci || vlak.Routing == Routing.Vychadzajuci)
                        {
                            var staniceDo = new List<Station>();

                            foreach (var t in vlak.StaniceDoSmeru)
                                if (t.IsInShortReport)
                                    staniceDo.Add(t);

                            if (staniceDo.Count != 0) staniceDo.RemoveAt(staniceDo.Count - 1);

                            for (var i = 0; i < staniceDo.Count; i++)
                                if (i < staniceDo.Count - 1)
                                {
                                    sb.AppendLine(staniceDo[i].Name);
                                    sb.Append('#');
                                }
                                else if (i == staniceDo.Count - 1)
                                {
                                    sb.AppendLine(staniceDo[i].Name);
                                }

                            tableTrain.Text = sb.ToString();
                        }
                        else
                        {
                            tableTrain.Text = "";
                        }
                    }
                    else if (item.FillSection == TableFillSection.StaniceZoSmeru)
                    {
                        var sb = new StringBuilder();

                        if (vlak.Routing == Routing.Prechadzajuci || vlak.Routing == Routing.Konciaci)
                        {
                            var staniceZo = new List<Station>();

                            foreach (var t in vlak.StaniceZoSmeru)
                                if (t.IsInShortReport)
                                    staniceZo.Add(t);

                            for (var i = 1; i < staniceZo.Count; i++)
                                if (i < staniceZo.Count - 1)
                                {
                                    sb.AppendLine(staniceZo[i].Name);
                                    sb.Append('#');
                                }
                                else if (i == staniceZo.Count - 1)
                                {
                                    sb.AppendLine(staniceZo[i].Name);
                                }

                            tableTrain.Text = sb.ToString();
                        }
                        else
                        {
                            tableTrain.Text = "";
                        }
                    }
                    else
                    {
                        return;
                    }

                    TextTrains.Add(tableTrain);
                }
            }
        }

        private void nudFont_ValueChanged(object sender, EventArgs e)
        {
            switch (nudFont.Value % 4)
            {
                case 1:
                    nudFont.BackColor = Color.Red;
                    nudFont.ForeColor = Color.White;
                    break;
                case 2:
                    nudFont.BackColor = Color.Green;
                    nudFont.ForeColor = Color.White;
                    break;
                case 3:
                    nudFont.BackColor = Color.Yellow;
                    nudFont.ForeColor = Color.Black;
                    break;
                default:
                    nudFont.BackColor = Color.White;
                    nudFont.ForeColor = Color.Black;
                    break;
            }
        }

        private void FTableText_HelpButtonClicked(object sender, CancelEventArgs e) => Process.Start(LinkConsts.LINK_TTEXTS);
    }
}