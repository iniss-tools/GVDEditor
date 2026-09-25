using System.Text.RegularExpressions;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Nastavenie textov do tabul.
/// </summary>
public partial class FTableText : Form
{
    private readonly GVDInfo gvd;
    private readonly int row;
    private readonly BindingList<TableItem> TableItems;
    private readonly BindingList<TableTrain> TextTrains;
    private readonly BindingList<Train> TrainsWithoutText;

    private readonly BindingList<TableTextRealization> TRealizations;

    /// <summary>
    ///     Tieto texty do tabul. Po <see cref="DialogResult.OK"/> novy objekt s upravenymi hodnotami; povodny objekt
    ///     (vratane zoznamov realizacii a vlakov) okno nemeni.
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
        this.ApplyThemeAndFonts();

        ThisTableText = tableText;
        this.gvd = gvd;
        this.row = row;

        //okno pracuje nad kopiami – Zrusit nesmie zmenit povodny text
        TRealizations = new BindingList<TableTextRealization>(tableText.Realizations.Select(TableTextGenerating.Clone).ToList());
        TextTrains = new BindingList<TableTrain>(tableText.Trains.Select(TableTextGenerating.Clone).ToList());
        TrainsWithoutText = new BindingList<Train>();
        TableItems = new BindingList<TableItem>();

        cbCatalogItem.DataSource = TableItems;
        cbCatalogTable.DataSource = catalogs;

        listRealisations.DataSource = TRealizations;
        listTrains.DataSource = TextTrains;
        cbAddTrain.DataSource = TrainsWithoutText;
        RefreshTrainsWithoutText();
        UpdateTrainButtons();

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
            foreach (var t in GlobData.TableTexts)
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
            foreach (var t in GlobData.TableTexts)
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

    private void listTrains_SelectedIndexChanged(object sender, EventArgs e) => ShowSelectedTrain();

    //po pridani/odobrati sa index vyberu nemusi zmenit (SelectedIndexChanged nepride) – polia sa obnovia rucne
    private void ShowSelectedTrain()
    {
        if (listTrains.SelectedItem is TableTrain tableTrain)
        {
            tbTrainText.Text = tableTrain.Text;
            nudFont.Value = tableTrain.FontID;
        }
        else
        {
            tbTrainText.Text = "";
            nudFont.Value = -1;
        }

        UpdateTrainButtons();
    }

    private void listRealisations_Format(object sender, ListControlConvertEventArgs e)
    {
        if (e.ListItem is TableTextRealization realization)
            e.Value = realization.Table.Name + " - " + realization.Item.Name;
    }

    private void listTrains_Format(object sender, ListControlConvertEventArgs e)
    {
        if (e.ListItem is TableTrain train)
            e.Value = FormatTrain(train.Train);
    }

    private void cbAddTrain_Format(object sender, ListControlConvertEventArgs e)
    {
        if (e.ListItem is Train train)
            e.Value = FormatTrain(train);
    }

    private static string FormatTrain(Train train) =>
        train.ID + ". " + train.Number + " " + train.Type + " " + (!string.IsNullOrEmpty(train.Name) ? train.Name : "");

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
        if (cbCatalogTable.SelectedItem == null || cbCatalogItem.SelectedItem == null) return;

        var realization = new TableTextRealization
        {
            Table = (TableCatalog)cbCatalogTable.SelectedItem!, Item = (TableItem)cbCatalogItem.SelectedItem!
        };

        TRealizations.Add(realization);
    }

    private void bReEdit_Click(object sender, EventArgs e)
    {
        if (listRealisations.SelectedIndex != -1 && cbCatalogTable.SelectedItem != null && cbCatalogItem.SelectedItem != null)
        {
            var realization = TRealizations[listRealisations.SelectedIndex];
            realization.Table = (TableCatalog)cbCatalogTable.SelectedItem!;
            realization.Item = (TableItem)cbCatalogItem.SelectedItem!;

            TRealizations.ResetBindings();
        }
    }

    private void bReDelete_Click(object sender, EventArgs e)
    {
        if (listRealisations.SelectedIndex != -1) TRealizations.RemoveAt(listRealisations.SelectedIndex);
    }

    private void bTextEdit_Click(object sender, EventArgs e)
    {
        if (listTrains.SelectedItem is TableTrain tableTrain)
        {
            tableTrain.Text = tbTrainText.Text;
            tableTrain.FontID = decimal.ToInt32(nudFont.Value);
            TextTrains.ResetItem(listTrains.SelectedIndex);
        }
    }

    private void bTrainAdd_Click(object sender, EventArgs e)
    {
        if (cbAddTrain.SelectedItem is not Train train) return;

        //text sa predvyplni podla stlpca vybraneho v casti Realizacia (ak ho generovanie podporuje)
        var tableTrain = TableTextGenerating.CreateFor(train, (cbCatalogItem.SelectedItem as TableItem)?.FillSection, gvd.ThisStation);

        //zoznam drzi poradie vlakov v grafikone
        var index = 0;
        while (index < TextTrains.Count && TextTrains[index].Train.ID <= train.ID) index++;
        TextTrains.Insert(index, tableTrain);

        RefreshTrainsWithoutText();
        listTrains.SelectedIndex = index;
        ShowSelectedTrain();
    }

    private void bTrainRemove_Click(object sender, EventArgs e)
    {
        var index = listTrains.SelectedIndex;
        if (index == -1) return;

        TextTrains.RemoveAt(index);
        RefreshTrainsWithoutText();
        if (TextTrains.Count != 0) listTrains.SelectedIndex = Math.Min(index, TextTrains.Count - 1);
        ShowSelectedTrain();
    }

    private void bGenerate_Click(object sender, EventArgs e)
    {
        if (cbCatalogTable.SelectedItem is not TableCatalog table || cbCatalogItem.SelectedItem is not TableItem item)
        {
            Utils.ShowError(Resources.FTableText_Generate_TTexts_No_Item);
            return;
        }

        if (!TableTextGenerating.IsSupported(item.FillSection))
        {
            Utils.ShowError(Resources.FTableText_Generate_TTexts_Wrong_Item);
            return;
        }

        var result = Utils.ShowQuestion(string.Format(Resources.FTableText_Generate_TTexts_Info, table.Name, item.Name));
        if (result != DialogResult.Yes) return;

        var generated = TableTextGenerating.Generate(GlobData.Trains, item.FillSection, gvd.ThisStation);

        TextTrains.RaiseListChangedEvents = false;
        TextTrains.Clear();
        foreach (var tableTrain in generated) TextTrains.Add(tableTrain);
        TextTrains.RaiseListChangedEvents = true;
        TextTrains.ResetBindings();

        RefreshTrainsWithoutText();
        ShowSelectedTrain();
    }

    private void RefreshTrainsWithoutText()
    {
        TrainsWithoutText.RaiseListChangedEvents = false;
        TrainsWithoutText.Clear();
        foreach (var train in TableTextGenerating.TrainsWithoutText(GlobData.Trains, TextTrains)) TrainsWithoutText.Add(train);
        TrainsWithoutText.RaiseListChangedEvents = true;
        TrainsWithoutText.ResetBindings();
    }

    private void UpdateTrainButtons()
    {
        var selected = listTrains.SelectedIndex != -1;
        bTextEdit.Enabled = selected;
        bTrainRemove.Enabled = selected;
        bTrainAdd.Enabled = TrainsWithoutText.Count != 0;
        cbAddTrain.Enabled = TrainsWithoutText.Count != 0;
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

    private void FTableText_HelpButtonClicked(object sender, CancelEventArgs e) => Utils.OpenShell(LinkConsts.LINK_TTEXTS);
}