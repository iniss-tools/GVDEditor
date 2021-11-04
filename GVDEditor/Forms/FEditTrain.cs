using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Forms
{
    /// <summary>
    ///     Dialog - Uprava/pridanie vlaku.
    /// </summary>
    public partial class FEditTrain : Form
    {
        private readonly bool copy;

        private readonly BindingList<Dodatok> Doplnky = new();

        private readonly BindingList<Operator> Operators = new(GlobData.Operators);

        private readonly BindingList<Radenie> Radenia = new();

        private readonly List<Radenie> RemovedRadenia = new();
        private readonly BindingList<Station> StaniceDo = new();
        private readonly BindingList<Station> StaniceZo = new();

        private readonly BindingList<string> TrainNames = new();

        private DateTime _lastKeyPressZo, _lastKeyPressDo;
        private string _searchStringZo, _searchStringDo;
        private bool ignoreSelectedIndexChanged = true;

        private bool initialization;

        private string linkaPrichod = "", linkaOdchod = "";

        /// <summary>
        ///     Index riadku na pracovnej ploche.
        /// </summary>
        public int Row;

        private List<FyzZvuk> selSounds;

        private string tbCisloOldValue = "";

        /// <summary>
        ///     Vlak, s ktorym tento dialog pracuje.
        /// </summary>
        public Train ThisTrain;

        private List<ReportType> VybraneReporty;

        /// <summary>
        ///     Vytvori novy formular typu <see cref="FEditTrain"/>.
        /// </summary>
        /// <param name="train">Upravovany vlak.</param>
        /// <param name="row">Index riadku na prac. ploche.</param>
        /// <param name="gvd">Vybrane GVD.</param>
        /// <param name="copy">Ci sa jedna o kopiu vlaku.</param>
        public FEditTrain(Train train, int row, GVDInfo gvd, bool copy = false)
        {
            InitializeComponent();
            FormUtils.SetFormFont(this);

            ThisTrain = train;
            Row = row;
            this.copy = copy;

            initialization = true;

            cbTyp.DataSource = GlobData.TrainsTypes;

            foreach (var name in GlobData.TrainNames) 
                TrainNames.Add(name);

            if (GlobData.Config.AutoVariant) nudVarianta.Enabled = false;

            cbNazov.DataSource = TrainNames;

            cbDopravca.DataSource = Operators;

            cbKolajPrichod.DataSource = GlobData.Tracks;
            cbKolajOdchod.DataSource = GlobData.Tracks;

            listStaniceZo.DataSource = GlobData.Stations;
            listStaniceDo.DataSource = GlobData.Stations;

            listRadenia.DataSource = Radenia;

            StaniceZo.ListChanged += StaniceZoOnListChanged;
            StaniceDo.ListChanged += StaniceDoOnListChanged;

            dgvTrasaZo.DataSource = StaniceZo;
            dgvTrasaDo.DataSource = StaniceDo;

            dtpPlatnostOd.Value = gvd.StartValidTimeTable.Date;
            dtpPlatnostDo.Value = gvd.EndValidTimeTable.Date;

            dtpRadenieOd.Value = gvd.StartValidTimeTable.Date;
            dtpRadenieDo.Value = gvd.EndValidTimeTable.Date;

            foreach (var jazyk in GlobData.Languages)
                if (!jazyk.IsBasic)
                    clbJazyky.Items.Add(jazyk);

            var lenDoplnky = new List<FyzZvuk>();
            foreach (var snd in GlobData.Sounds)
                if (snd.Dir.Name.EqualsIgnoreCase("DODATKY"))
                    lenDoplnky.Add(snd);

            listAllDoplnky.DataSource = lenDoplnky;
            listVybrateDoplnky.DataSource = Doplnky;

            FillRadenieSet();

            cbNazov.SelectedItem = null;

            if (ThisTrain != null)
            {
                if (copy)
                {
                    Text = Resources.FEditTrain_FEditTrain_Duplikovať_vlak;
                    bSave.Text = Resources.FEditTrain_FEditTrain_Pridať;
                }

                InitializeData();
            }
            else
            {
                Text = Resources.FEditTrain_FEditTrain_Pridať_vlak;
                bSave.Text = Resources.FEditTrain_FEditTrain_Pridať;

                mtPrichod.Enabled = false;
                mtOdchod.Enabled = false;

                bRadenieEdit.Enabled = false;
                bRadenieDelete.Enabled = false;

                bDoplnkyEdit.Enabled = false;
                bDoplnkyDelete.Enabled = false;
            }

            this.ApplyTheme();

            initialization = false;
        }

        /// <summary>
        ///     Uprava textu Formu
        /// </summary>
        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        private void InitializeData()
        {
            //initialization = true;
            tbCislo.Text = ThisTrain.Number;
            //initialization = false;
            tbCisloOldValue = ThisTrain.Number;

            cbTyp.SelectedItem = ThisTrain.Type;

            if (!string.IsNullOrEmpty(ThisTrain.Name)) cbNazov.Text = ThisTrain.Name;

            cbDopravca.SelectedItem = ThisTrain.Operator;


            if (ThisTrain.Arrival != null) 
                mtPrichod.Text = ThisTrain.Arrival?.ToString("HH:mm");

            if (ThisTrain.Departure != null) 
                mtOdchod.Text = ThisTrain.Departure?.ToString("HH:mm");


            cbKolajPrichod.SelectedItem = ThisTrain.Track;

            cbKolajOdchod.SelectedItem = ThisTrain.Track;

            tDatumoveObmedzenie.Text = ThisTrain.DateLimitText;

            dtpPlatnostOd.Value = ThisTrain.ZaciatokPlatnosti;
            dtpPlatnostDo.Value = ThisTrain.KoniecPlatnosti;

            boxMedzistatny.Checked = ThisTrain.IsMedzistatny;
            boxMimoriadny.Checked = ThisTrain.IsMimoriadny;
            boxMiestenkovy.Checked = ThisTrain.IsMiestenkovy;
            boxDialkovy.Checked = ThisTrain.IsDialkovy;
            boxNizkopodlazny.Checked = ThisTrain.IsNizkopodlazny;
            boxLozkovy.Checked = ThisTrain.IsIbaLozkovy;

            tbLinkaPrichod.Text = ThisTrain.LineArrival;
            tbLinkaOdchod.Text = ThisTrain.LineDeparture;

            nudVarianta.Value = ThisTrain.Variant;

            foreach (var st in ThisTrain.StaniceZoSmeru) StaniceZo.Add(st);
            foreach (var st in ThisTrain.StaniceDoSmeru) StaniceDo.Add(st);

            StaniceZo.RaiseListChangedEvents = true;
            StaniceDo.RaiseListChangedEvents = true;

            initialization = false;
            StaniceCountChanged();
            initialization = true;

            foreach (var jazyk in ThisTrain.Languages)
                if (!jazyk.IsBasic)
                    clbJazyky.SetItemChecked(clbJazyky.Items.IndexOf(jazyk), true);

            foreach (var doplnok in ThisTrain.Doplnky) Doplnky.Add(doplnok);

            foreach (var radenie in ThisTrain.Radenia) Radenia.Add(radenie);

            if (StaniceZo.Count == 0) mtPrichod.Enabled = false;

            if (StaniceDo.Count == 0) mtOdchod.Enabled = false;

            if (Radenia.Count == 0)
            {
                bRadenieEdit.Enabled = false;
                bRadenieDelete.Enabled = false;
            }

            if (Doplnky.Count == 0)
            {
                bDoplnkyEdit.Enabled = false;
                bDoplnkyDelete.Enabled = false;
            }
        }

        private void FEditTrain_Load(object sender, EventArgs e)
        {
            if (listVybrateDoplnky.SelectedIndex != -1) DoplnkyIndexChanged(Doplnky[listVybrateDoplnky.SelectedIndex]);
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            var train = (copy ? new Train() : ThisTrain) ?? new Train();

            if (!string.IsNullOrEmpty(tbCislo.Text))
            {
                train.Number = tbCislo.Text;
            }
            else
            {
                Utils.ShowError(Resources.FEditTrain_bSave_Click_Zadajte_číslo_vlaku);
                DialogResult = DialogResult.None;
                return;
            }

            train.Type = (TrainType)cbTyp.SelectedItem;
            train.Name = cbNazov.Text;
            train.Operator = (Operator)cbDopravca.SelectedItem;

            if (StaniceZo.Count != 0 && StaniceDo.Count != 0)
            {
                if (!Utils.IsTime(mtPrichod.Text))
                {
                    Utils.ShowError(Resources.FEditTrain_bSave_Click_Nesprávny_formát_času_príchodu);
                    DialogResult = DialogResult.None;
                    return;
                }

                if (!Utils.IsTime(mtOdchod.Text))
                {
                    Utils.ShowError(Resources.FEditTrain_bSave_Click_Nesprávny_formát_času_odchodu);
                    DialogResult = DialogResult.None;
                    return;
                }

                var pr = DateTime.Parse(mtPrichod.Text);
                var od = DateTime.Parse(mtOdchod.Text);
                if (od.CompareTo(pr) < 0)
                {
                    Utils.ShowError(Resources.FEditTrain_bSave_Click_Čas_príchodu_je_neskôr_ako_čas_odchodu);
                    DialogResult = DialogResult.None;
                    return;
                }

                train.Arrival = pr;
                train.Departure = od;
                train.Routing = Routing.Prechadzajuci;
                train.StartingStation = StaniceZo.First();
                train.EndingStation = StaniceDo.Last();
            }
            else if (StaniceZo.Count != 0)
            {
                if (!Utils.IsTime(mtPrichod.Text))
                {
                    Utils.ShowError(Resources.FEditTrain_bSave_Click_Nesprávny_formát_času_príchodu);
                    DialogResult = DialogResult.None;
                    return;
                }

                train.Arrival = DateTime.Parse(mtPrichod.Text);
                train.Departure = null;
                train.Routing = Routing.Konciaci;
                train.StartingStation = StaniceZo.First();
                train.EndingStation = null;
            }
            else if (StaniceDo.Count != 0)
            {
                if (!Utils.IsTime(mtOdchod.Text))
                {
                    Utils.ShowError(Resources.FEditTrain_bSave_Click_Nesprávny_formát_času_odchodu);
                    DialogResult = DialogResult.None;
                    return;
                }

                train.Arrival = null;
                train.Departure = DateTime.Parse(mtOdchod.Text);
                train.Routing = Routing.Vychadzajuci;
                train.StartingStation = null;
                train.EndingStation = StaniceDo.Last();
            }
            else
            {
                Utils.ShowError(Resources.FEditTrain_bSave_Click_Vlak_nemá_zadanú_žiadnu_stanicu);
                DialogResult = DialogResult.None;
                return;
            }

            if (cbKolajPrichod.SelectedIndex == -1 && cbKolajOdchod.SelectedIndex == -1)
            {
                Utils.ShowError(Resources.FEditTrain_bSave_Click_Nie_je_vybratá_koľaj);
                DialogResult = DialogResult.None;
                return;
            }

            train.Track = (Track)cbKolajPrichod.SelectedItem;

            train.DateLimitText = tDatumoveObmedzenie.Text;

            train.IsMedzistatny = boxMedzistatny.Checked;
            train.IsMimoriadny = boxMimoriadny.Checked;
            train.IsMiestenkovy = boxMiestenkovy.Checked;
            train.IsDialkovy = boxDialkovy.Checked;
            train.IsNizkopodlazny = boxNizkopodlazny.Checked;
            train.IsIbaLozkovy = boxLozkovy.Checked;

            train.StaniceZoSmeru.Clear();
            train.StaniceDoSmeru.Clear();

            for (var index = 0; index < dgvTrasaZo.Rows.Count; index++)
            {
                var selectedRow = dgvTrasaZo.Rows[index];
                var st = (Station)selectedRow.DataBoundItem;

                train.StaniceZoSmeru.Add(st);
            }

            for (var index = 0; index < dgvTrasaDo.Rows.Count; index++)
            {
                var selectedRow = dgvTrasaDo.Rows[index];
                var st = (Station)selectedRow.DataBoundItem;

                train.StaniceDoSmeru.Add(st);
            }

            train.Languages.Clear();

            foreach (var item in clbJazyky.CheckedItems.OfType<Language>()) train.Languages.Add(item);

            var platnostOd = dtpPlatnostOd.Value;
            var platnostDo = dtpPlatnostDo.Value;

            if (platnostOd.CompareTo(platnostDo) > 0)
            {
                Utils.ShowError(Resources.FEditTrain_bSave_Click_Začiatok_platnosti_musí_skôr_ako_koniec_platnosti);
                DialogResult = DialogResult.None;
                return;
            }

            train.ZaciatokPlatnosti = platnostOd;
            train.KoniecPlatnosti = platnostDo;

            try
            {
                var dom = new DateLimit(platnostOd.Date, platnostDo.Date);
                dom.TextToBitArray(train.DateLimitText);
            }
            catch (Exception ex)
            {
                Utils.ShowError(ex.Message);
                DialogResult = DialogResult.None;
                return;
            }

            train.LineArrival = linkaPrichod;
            train.LineDeparture = linkaOdchod;

            train.Doplnky = Doplnky.ToList();

            var dateRem = new DateLimit(platnostOd.Date, platnostDo.Date, true, true, false, false);

            var varianta = decimal.ToInt32(nudVarianta.Value);

            var id = 0;
            var seltrains = new List<Train>();
            foreach (var vlak in GlobData.Trains)
            {
                if (Train.IsSameVariant(train, vlak) && Row != id)
                {
                    if (!GlobData.Config.AutoVariant)
                    {
                        if (varianta == -1)
                        {
                            Utils.ShowError(Resources.FEditTrain_bSave_Click_Varianta_tohto_vlaku_nemôže_byť_Minus_1);
                            DialogResult = DialogResult.None;
                            return;
                        }

                        if (varianta == vlak.Variant)
                        {
                            Utils.ShowError(Resources.FEditTrain_bSave_Click_Vybraná_varianta_vlaku_sa_už_používa_pri_inom_vlaku);
                            DialogResult = DialogResult.None;
                            return;
                        }
                    }

                    seltrains.Add(vlak);
                }

                id++;
            }

            if (!GlobData.Config.AutoVariant)
            {
                if (seltrains.Count == 0 && varianta != -1 && !GlobData.Config.DisableVariantCheck)
                {
                    Utils.ShowWarning(Resources.FEditTrain_Tento_vlak_nemá_iné_varianty_a_preto_mu_bude_varianta_nastavená_na_hodnotu_Minus_1);
                    train.Variant = -1;
                }
                else
                {
                    train.Variant = varianta;
                }
            }

            foreach (var seltrain in seltrains)
                if (seltrain.ZaciatokPlatnosti == train.ZaciatokPlatnosti && seltrain.KoniecPlatnosti == train.KoniecPlatnosti)
                    if (dateRem.Overlap(seltrain.DateLimitText, train.DateLimitText))
                    {
                        var obmand = dateRem.TextAnd(seltrain.DateLimitText, train.DateLimitText);

                        var result = Utils.ShowQuestion(string.Format(Resources.FEditTrain_DateRem_zasahuje_do_ineho_vlaku, train.Type,
                            train.Number, train.Name, obmand));

                        if (result == DialogResult.Yes)
                        {
                            var fDateRemEdit = new FDateLimitEdit(train, seltrain.DateLimitText, platnostOd, platnostDo);
                            var result2 = fDateRemEdit.ShowDialog();
                            if (result2 == DialogResult.OK)
                                seltrain.DateLimitText = fDateRemEdit.DateRemEdited;
                        }
                    }

            if (GlobData.Config.AutoVariant)
                if (copy || ThisTrain == null)
                {
                    if (seltrains.Count == 0)
                    {
                        train.Variant = -1;
                    }
                    else
                    {
                        int i;
                        for (i = 0; i < seltrains.Count; i++) seltrains[i].Variant = i + 1;

                        train.Variant = i + 1;
                    }
                }

            var newrads = new List<Radenie>();
            foreach (var rad in Radenia)
                if (!GlobData.Radenia.Contains(rad))
                {
                    rad.CisloVlaku = train.Number;
                    newrads.Add(rad);
                }
                else
                {
                    rad.CisloVlaku = train.Number;
                }

            GlobData.Radenia.AddRange(newrads);

            foreach (var vlak in GlobData.Trains)
                if (train.Number == vlak.Number)
                {
                    vlak.Radenia.AddRange(newrads);

                    foreach (var r in RemovedRadenia) vlak.Radenia.Remove(r);
                }

            foreach (var r in RemovedRadenia) GlobData.Radenia.Remove(r);

            if (copy || ThisTrain == null) ThisTrain = train;

            DialogResult = DialogResult.OK;
        }

        private void bZrusit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void cbKolajPrichod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreSelectedIndexChanged)
            {
                ignoreSelectedIndexChanged = true;
                cbKolajOdchod.SelectedIndex = cbKolajPrichod.SelectedIndex;
                ignoreSelectedIndexChanged = false;
            }
        }

        private void cbKolajOdchod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreSelectedIndexChanged)
            {
                ignoreSelectedIndexChanged = true;
                cbKolajPrichod.SelectedIndex = cbKolajOdchod.SelectedIndex;
                ignoreSelectedIndexChanged = false;
            }
        }

        private void tbCislo_TextChanged(object sender, EventArgs e)
        {
            tbCislo.Text = tbCislo.Text.Trim();
            //if (Regex.IsMatch(tbCislo.Text, "[^0-9]"))
            if (tbCislo.Text.Contains(";") || tbCislo.Text.Contains(" ") || tbCislo.Text.Contains("\t"))
            {
                tbCislo.Text = tbCisloOldValue;
            }
            else
            {
                tbCisloOldValue = tbCislo.Text;

                if (!string.IsNullOrEmpty(tbCislo.Text))
                {
                    var i = 0;
                    foreach (var train in GlobData.Trains)
                    {
                        var cislo = tbCislo.Text;
                        if (train.Number == cislo && train.Radenia.Count != 0 && Row != i && !copy && !initialization)
                        {
                            Utils.ShowWarning(Resources
                                .FEditTrain_Číslo_vlaku_sa_zhoduje_s_iným_vlakom_a_preto_bude_aj_jeho_radenie_priradené_k_tomuto_vlaku);

                            Radenia.Clear();
                            foreach (var rad in train.Radenia) Radenia.Add(rad);
                            break;
                        }

                        i++;
                    }
                }
            }
        }

        private void bSkorZo_Click(object sender, EventArgs e)
        {
            if (dgvTrasaZo.CurrentRow != null)
            {
                var sel = dgvTrasaZo.SelectedRows[0].Index;
                var stationPov = StaniceZo[sel];

                if (sel - 1 >= 0)
                {
                    StaniceZo.RemoveAt(sel);
                    StaniceZo.Insert(sel - 1, stationPov);
                    dgvTrasaZo.ClearSelection();
                    dgvTrasaZo.Rows[sel - 1].Selected = true;
                }
            }
        }

        private void bNeskorZo_Click(object sender, EventArgs e)
        {
            if (dgvTrasaZo.CurrentRow != null)
            {
                var sel = dgvTrasaZo.SelectedRows[0].Index;
                var stationPov = StaniceZo[sel];

                if (sel + 1 < StaniceZo.Count)
                {
                    StaniceZo.RemoveAt(sel);
                    StaniceZo.Insert(sel + 1, stationPov);
                    dgvTrasaZo.ClearSelection();
                    dgvTrasaZo.Rows[sel + 1].Selected = true;
                }
            }
        }

        private void bAddZo_Click(object sender, EventArgs e)
        {
            if (listStaniceZo.SelectedIndex != -1)
            {
                var st = (Station)listStaniceZo.SelectedItem;
                StaniceZo.Add(new Station(st.ID, st.Name, IsInLongReport: true));
                dgvTrasaZo.Rows[StaniceZo.Count - 1].Cells[0].Value = true;
            }
        }

        private void bDeleteZo_Click(object sender, EventArgs e)
        {
            if (dgvTrasaZo.SelectedRows.Count > 0) StaniceZo.RemoveAt(dgvTrasaZo.CurrentCell.RowIndex);
        }

        private void bSkorDo_Click(object sender, EventArgs e)
        {
            if (dgvTrasaDo.CurrentRow != null)
            {
                var sel = dgvTrasaDo.SelectedRows[0].Index;
                var stationPov = StaniceDo[sel];

                if (sel - 1 >= 0)
                {
                    StaniceDo.RemoveAt(sel);
                    StaniceDo.Insert(sel - 1, stationPov);
                    dgvTrasaDo.ClearSelection();
                    dgvTrasaDo.Rows[sel - 1].Selected = true;
                }
            }
        }

        private void bNeskorDo_Click(object sender, EventArgs e)
        {
            if (dgvTrasaDo.CurrentRow != null)
            {
                var sel = dgvTrasaDo.SelectedRows[0].Index;
                var stationPov = StaniceDo[sel];

                if (sel + 1 < StaniceDo.Count)
                {
                    StaniceDo.RemoveAt(sel);
                    StaniceDo.Insert(sel + 1, stationPov);
                    dgvTrasaDo.ClearSelection();
                    dgvTrasaDo.Rows[sel + 1].Selected = true;
                }
            }
        }

        private void bAddDo_Click(object sender, EventArgs e)
        {
            if (listStaniceDo.SelectedIndex != -1)
            {
                var st = (Station)listStaniceDo.SelectedItem;
                StaniceDo.Add(new Station(st.ID, st.Name, IsInLongReport: true));
                dgvTrasaDo.Rows[StaniceDo.Count - 1].Cells[0].Value = true;
            }
        }

        private void bDeleteDo_Click(object sender, EventArgs e)
        {
            if (dgvTrasaDo.SelectedRows.Count > 0) StaniceDo.RemoveAt(dgvTrasaDo.CurrentCell.RowIndex);
        }

        private void listStaniceZo_DoubleClick(object sender, EventArgs e)
        {
            if (listStaniceZo.SelectedIndex != -1)
            {
                var st = (Station)listStaniceZo.SelectedItem;
                StaniceZo.Add(new Station(st.ID, st.Name, IsInLongReport: true));
            }
        }

        private void listStaniceDo_DoubleClick(object sender, EventArgs e)
        {
            if (listStaniceDo.SelectedIndex != -1)
            {
                var st = (Station)listStaniceDo.SelectedItem;
                StaniceDo.Add(new Station(st.ID, st.Name, IsInLongReport: true));
            }
        }

        private void dgvTrasaZo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
                StaniceZo.RemoveAt(e.RowIndex);
            else
                StaniceZo.Clear();
        }

        private void dgvTrasaDo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
                StaniceDo.RemoveAt(e.RowIndex);
            else
                StaniceDo.Clear();
        }

        private void listAllDoplnky_DoubleClick(object sender, EventArgs e)
        {
            if (listAllDoplnky.SelectedIndex != -1)
            {
                var doplnok = new Dodatok
                {
                    Sound = listAllDoplnky.SelectedItem as FyzZvuk,
                    Name = ((FyzZvuk)listAllDoplnky.SelectedItem).Name.Replace("D", "")
                };
                Doplnky.Add(doplnok);
            }
        }

        private void bDoplnkyAdd_Click(object sender, EventArgs e)
        {
            if (listAllDoplnky.SelectedIndex != -1)
            {
                var doplnok = new Dodatok
                {
                    Sound = listAllDoplnky.SelectedItem as FyzZvuk,
                    Name = ((FyzZvuk)listAllDoplnky.SelectedItem).Name.Replace("D", "")
                };
                Doplnky.Add(doplnok);

                bDoplnkyEdit.Enabled = true;
                bDoplnkyDelete.Enabled = true;
            }
        }

        private void bDoplnkyEdit_Click(object sender, EventArgs e)
        {
            if (listVybrateDoplnky.SelectedIndex != -1)
                Doplnky[listVybrateDoplnky.SelectedIndex].ChosenReports = GetFromTable(dgvDoplnokSet, VybraneReporty);
        }

        private void bDoplnkyDelete_Click(object sender, EventArgs e)
        {
            if (listVybrateDoplnky.SelectedIndex != -1)
            {
                Doplnky.RemoveAt(listVybrateDoplnky.SelectedIndex);
                if (Doplnky.Count == 0)
                {
                    bDoplnkyEdit.Enabled = false;
                    bDoplnkyDelete.Enabled = false;
                }
            }
        }

        private void listVybrateDoplnky_DoubleClick(object sender, EventArgs e)
        {
            if (listVybrateDoplnky.SelectedIndex != -1) Doplnky.RemoveAt(listVybrateDoplnky.SelectedIndex);
        }

        private void listAllDoplnky_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listAllDoplnky.SelectedIndex != -1 && listAllDoplnky.SelectedItem != null)
                tbTextDoplnku.Text = ((FyzZvuk)listAllDoplnky.SelectedItem).Text;
        }

        private void listVybrateDoplnky_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listVybrateDoplnky.SelectedIndex != -1)
            {
                var doplnok = Doplnky[listVybrateDoplnky.SelectedIndex];
                tbTextDoplnku.Text = doplnok.Sound.Text;

                if (!initialization) DoplnkyIndexChanged(doplnok);
            }
        }

        private void DoplnkyIndexChanged(Dodatok doplnok)
        {
            TruncateTable(dgvDoplnokSet);

            for (var i = 0; i < dgvDoplnokSet.Rows.Count; i++)
            for (var j = 0; j < GlobData.ReportVariants.Count; j++)
                foreach (var reportType in doplnok.ChosenReports)
                    if (reportType.Type == VybraneReporty[i])
                    {
                        var found = false;
                        foreach (var variant in reportType.Variants)
                            if (variant == GlobData.ReportVariants[j])
                            {
                                dgvDoplnokSet.Rows[i].Cells[j + 1].Value = true;
                                found = true;
                            }

                        if (!found) dgvDoplnokSet.Rows[i].Cells[j + 1].Value = false;
                    }
        }

        private void StaniceZoOnListChanged(object sender, ListChangedEventArgs e)
        {
            if (!initialization)
            {
                if (e.ListChangedType == ListChangedType.ItemAdded)
                {
                    if (e.NewIndex == 0)
                    {
                        StaniceZoEdited();
                        StaniceCountChanged();
                    }
                }
                else if (e.ListChangedType == ListChangedType.ItemDeleted)
                {
                    if (StaniceZo.Count == 0)
                    {
                        StaniceZoEdited();
                        StaniceCountChanged();
                    }
                }
            }
        }

        private void StaniceDoOnListChanged(object sender, ListChangedEventArgs e)
        {
            if (!initialization)
            {
                if (e.ListChangedType == ListChangedType.ItemAdded)
                {
                    if (e.NewIndex == 0)
                    {
                        StaniceDoEdited();
                        StaniceCountChanged();
                    }
                }
                else if (e.ListChangedType == ListChangedType.ItemDeleted)
                {
                    if (StaniceDo.Count == 0)
                    {
                        StaniceDoEdited();
                        StaniceCountChanged();
                    }
                }
            }
        }

        private void StaniceZoEdited()
        {
            if (StaniceZo.Count > 0)
            {
                mtPrichod.Enabled = true;
            }
            else
            {
                mtPrichod.Text = "";
                mtPrichod.Enabled = false;
            }
        }

        private void StaniceDoEdited()
        {
            if (StaniceDo.Count > 0)
            {
                mtOdchod.Enabled = true;
            }
            else
            {
                mtOdchod.Text = "";
                mtOdchod.Enabled = false;
            }
        }

        private void StaniceCountChanged()
        {
            if (!initialization)
            {
                if (StaniceZo.Count > 0 && StaniceDo.Count > 0)
                    VybraneReporty = GlobData.ReportTypesP;
                else if (StaniceZo.Count > 0)
                    VybraneReporty = GlobData.ReportTypesK;
                else if (StaniceDo.Count > 0)
                    VybraneReporty = GlobData.ReportTypesV;
                else
                    VybraneReporty = new List<ReportType>(1);

                FillDoplnkySet(VybraneReporty);

                if (listVybrateDoplnky.SelectedIndex != -1)
                    DoplnkyIndexChanged(Doplnky[listVybrateDoplnky.SelectedIndex]);

                foreach (var dodatok in Doplnky)
                    for (var i = 0; i < dodatok.ChosenReports.Count; i++)
                        if (!VybraneReporty.Contains(dodatok.ChosenReports[i].Type))
                            dodatok.ChosenReports.RemoveAt(i);
            }
        }

        private void cbZoSmeruCustom_CheckedChanged(object sender, EventArgs e)
        {
            listStaniceZo.DataSource = cbZoSmeruCustom.Checked ? GlobData.CustomStations : GlobData.Stations;
        }

        private void cbDoSmeruCustom_CheckedChanged(object sender, EventArgs e)
        {
            listStaniceDo.DataSource = cbDoSmeruCustom.Checked ? GlobData.CustomStations : GlobData.Stations;
        }

        private void listRadenia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listRadenia.SelectedIndex != -1)
            {
                var radenie = Radenia[listRadenia.SelectedIndex];
                tbRadenie.Text = radenie.Text;
                dtpRadenieOd.Value = radenie.ZacPlatnosti;
                dtpRadenieDo.Value = radenie.KonPlatnosti;
                tbDateRemRadenie.Text = radenie.DatObm;
                selSounds = radenie.Sounds;

                TruncateTable(dgvRadenieSet);

                for (var i = 0; i < dgvRadenieSet.Rows.Count; i++)
                for (var j = 0; j < GlobData.ReportVariants.Count; j++)
                    foreach (var reportType in radenie.ChosenReports)
                        if (reportType.Type == GlobData.ReportTypes[i])
                        {
                            var found = false;
                            foreach (var variant in reportType.Variants)
                                if (variant == GlobData.ReportVariants[j])
                                {
                                    dgvRadenieSet.Rows[i].Cells[j + 1].Value = true;
                                    found = true;
                                }

                            if (!found) dgvRadenieSet.Rows[i].Cells[j + 1].Value = false;
                        }
            }
        }

        private void bRadenieAdd_Click(object sender, EventArgs e)
        {
            var radenie = new Radenie();
            var odP = dtpRadenieOd.Value.Date;
            var doP = dtpRadenieDo.Value.Date;

            if (doP.CompareTo(odP) <= 0)
            {
                Utils.ShowError(Resources.FEditTrain_Začiatok_platnosti_radenia_je_neskôr_ako_jeho_koniec);
                return;
            }

            try
            {
                var thisDaterem = new DateLimit(odP, doP);
                thisDaterem.TextToBitArray(tbDateRemRadenie.Text);
            }
            catch (Exception ex)
            {
                Utils.ShowError(Resources.FEditTrain_DateRem_radenia_obsahuje_chybu + ex.Message);
                return;
            }

            foreach (var rad in Radenia)
            {
                var rem = new DateLimit(rad.ZacPlatnosti, rad.KonPlatnosti, insertMarks: false);
                var and = rem.TextAnd(rad.DatObm, tbDateRemRadenie.Text);
                if (rad.ZacPlatnosti == odP && rad.KonPlatnosti == doP && and != "t.č. nejde" && and != "t.č. nejede")
                {
                    Utils.ShowError(string.Format(Resources.FEditTrain_Zadané_dátumové_obmedzenie_radenia_sa_prekrýva_s_iným_v_období, and));
                    return;
                }
            }

            if (selSounds == null)
            {
                Utils.ShowError(Resources.FEditTrain_Nebolo_zadané_radenie_vlaku);
                return;
            }

            radenie.DatObm = tbDateRemRadenie.Text;
            radenie.ZacPlatnosti = odP;
            radenie.KonPlatnosti = doP;
            radenie.Text = tbRadenie.Text;
            radenie.Sounds = selSounds;

            radenie.ChosenReports = GetFromTable(dgvRadenieSet, GlobData.ReportTypes);

            Radenia.Add(radenie);

            bRadenieEdit.Enabled = true;
            bRadenieDelete.Enabled = true;
        }

        private void bRadenieEdit_Click(object sender, EventArgs e)
        {
            if (listRadenia.SelectedIndex != -1)
            {
                var radenie = Radenia[listRadenia.SelectedIndex];
                var odP = dtpRadenieOd.Value.Date;
                var doP = dtpRadenieDo.Value.Date;

                if (doP.CompareTo(odP) <= 0)
                {
                    Utils.ShowError(Resources.FEditTrain_Začiatok_platnosti_radenia_je_neskôr_ako_jeho_koniec);
                    return;
                }

                try
                {
                    var thisDaterem = new DateLimit(odP, doP);
                    thisDaterem.TextToBitArray(tbDateRemRadenie.Text);
                }
                catch (Exception ex)
                {
                    Utils.ShowError(Resources.FEditTrain_DateRem_radenia_obsahuje_chybu + ex.Message);
                    return;
                }

                var j = 0;
                foreach (var rad in Radenia)
                {
                    var rem = new DateLimit(rad.ZacPlatnosti, rad.KonPlatnosti, insertMarks: false);
                    var and = rem.TextAnd(rad.DatObm, tbDateRemRadenie.Text);
                    if (rad.ZacPlatnosti == odP && rad.KonPlatnosti == doP && and != "t.č. nejde" && and != "t.č. nejede" &&
                        listRadenia.SelectedIndex != j)
                    {
                        Utils.ShowError(string.Format(Resources.FEditTrain_Zadané_dátumové_obmedzenie_radenia_sa_prekrýva_s_iným_v_období, and));
                        return;
                    }

                    j++;
                }

                if (selSounds == null)
                {
                    Utils.ShowError(Resources.FEditTrain_Nebolo_zadané_radenie_vlaku);
                    return;
                }

                radenie.DatObm = tbDateRemRadenie.Text;
                radenie.ZacPlatnosti = odP;
                radenie.KonPlatnosti = doP;
                radenie.Text = tbRadenie.Text;
                radenie.Sounds = selSounds;

                radenie.ChosenReports = GetFromTable(dgvRadenieSet, GlobData.ReportTypes);

                Radenia.ResetBindings();
            }
        }

        private void bRadenieDelete_Click(object sender, EventArgs e)
        {
            if (listRadenia.SelectedIndex != -1)
            {
                RemovedRadenia.Add(Radenia[listRadenia.SelectedIndex]);
                Radenia.RemoveAt(listRadenia.SelectedIndex);

                if (Radenia.Count == 0)
                {
                    bRadenieEdit.Enabled = false;
                    bRadenieDelete.Enabled = false;
                }
            }
        }

        private void bEditRadenie_Click(object sender, EventArgs e)
        {
            FRadenie fRadenie;
            if (listRadenia.SelectedIndex == -1)
            {
                fRadenie = new FRadenie(new List<FyzZvuk>());
                var result = fRadenie.ShowDialog();
                if (result == DialogResult.OK)
                {
                    selSounds = new List<FyzZvuk>(fRadenie.SelSounds);
                    tbRadenie.Text = Radenie.SoundsToString(selSounds);
                }
            }
            else
            {
                fRadenie = new FRadenie(new List<FyzZvuk>(Radenia[listRadenia.SelectedIndex].Sounds));
                var result = fRadenie.ShowDialog();
                if (result == DialogResult.OK)
                {
                    selSounds = new List<FyzZvuk>(fRadenie.SelSounds);
                    tbRadenie.Text = Radenie.SoundsToString(selSounds);
                }
            }
        }

        private void listRadenia_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is Radenie radenie)
            {
                if (radenie.ZacPlatnosti == DateTime.MinValue)
                    e.Value = "Nezadaná platnosť";
                else
                    e.Value = radenie.ZacPlatnosti.Date.ToString("dd.MM.yyyy") + " - " +
                              radenie.KonPlatnosti.Date.ToString("dd.MM.yyyy");
            }
        }

        private void bPlay_Click(object sender, EventArgs e)
        {
            var soundsS = new List<string>();
            if (selSounds != null)
                foreach (var fyzZvuk in selSounds)
                    soundsS.Add(GlobData.RAWBANKDir + "\\" + fyzZvuk.Language.RelativePath +
                                fyzZvuk.Dir.RelativePath + fyzZvuk.FileName);
            else if (listRadenia.SelectedIndex != -1)
                foreach (var fyzZvuk in Radenia[listRadenia.SelectedIndex].Sounds)
                    soundsS.Add(GlobData.RAWBANKDir + "\\" + fyzZvuk.Language.RelativePath +
                                fyzZvuk.Dir.RelativePath + fyzZvuk.FileName);

            var player = new WAVPlayer(soundsS.ToArray(), GlobData.Config.PlayerWordPause);
            player.StartPlay();
        }

        private void tbLinkaPrichod_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbLinkaPrichod.Text))
            {
                if (new Regex(@"^[a-zA-Z0-9]{1,20}$").IsMatch(tbLinkaPrichod.Text))
                    linkaPrichod = tbLinkaPrichod.Text;
                else
                    tbLinkaPrichod.Text = linkaPrichod;
            }
        }

        private void tbLinkaOdchod_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbLinkaOdchod.Text))
            {
                if (new Regex(@"^[a-zA-Z0-9]{1,20}$").IsMatch(tbLinkaOdchod.Text))
                    linkaOdchod = tbLinkaOdchod.Text;
                else
                    tbLinkaOdchod.Text = linkaOdchod;
            }
        }

        private void listStaniceZo_KeyPress(object sender, KeyPressEventArgs e)
        {
            var newDate = DateTime.Now;
            var diff = newDate - _lastKeyPressZo;
            if (diff.TotalSeconds >= 1.5)
                _searchStringZo = string.Empty;
            _searchStringZo += e.KeyChar;

            var found = listStaniceZo.Items.Cast<Station>()
                .FirstOrDefault(item => item.Name.ToLower().StartsWith(_searchStringZo));
            if (found != null)
                listStaniceZo.SelectedItem = found;

            _lastKeyPressZo = newDate;
            e.Handled = true;
        }

        private void listStaniceDo_KeyPress(object sender, KeyPressEventArgs e)
        {
            var newDate = DateTime.Now;
            var diff = newDate - _lastKeyPressDo;
            if (diff.TotalSeconds >= 1.5)
                _searchStringDo = string.Empty;
            _searchStringDo += e.KeyChar;

            var found = listStaniceDo.Items.Cast<Station>().FirstOrDefault(item => item.Name.ToLower().StartsWith(_searchStringDo));
            if (found != null) listStaniceDo.SelectedItem = found;

            _lastKeyPressDo = newDate;
            e.Handled = true;
        }

        private void FillDoplnkySet(List<ReportType> reportTypes)
        {
            var dt = new DataTable();

            var header = new DataColumn("Typ", typeof(string))
            {
                Caption = "Typ",
                ReadOnly = true
            };
            dt.Columns.Add(header);

            foreach (var variant in GlobData.ReportVariants)
            {
                var dc = new DataColumn(variant.Name, typeof(bool))
                {
                    Caption = variant.Name
                };
                dt.Columns.Add(dc);
            }

            foreach (var reportType in reportTypes)
            {
                var row = dt.NewRow();
                row[0] = reportType.Name;
                dt.Rows.Add(row);
            }

            dgvDoplnokSet.DataSource = dt;

            var cheader = dgvDoplnokSet.Columns[0];
            cheader.HeaderText = dt.Columns[0].Caption;
            cheader.SortMode = DataGridViewColumnSortMode.NotSortable;
            cheader.Width = 120;

            for (var i = 1; i < dgvDoplnokSet.Columns.Count; i++)
            {
                var column = (DataGridViewCheckBoxColumn)dgvDoplnokSet.Columns[i];
                column.HeaderText = dt.Columns[i].Caption;
                if (column.Width != 80) column.Width = 80;
                if (i == dgvDoplnokSet.Columns.Count - 1) column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void FEditTrain_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start(LinkConsts.LINK_EDIT_TRAIN);
        }

        private void FillRadenieSet()
        {
            var dt = new DataTable();

            var header = new DataColumn { DataType = typeof(string), Caption = "Typ", ReadOnly = true };
            dt.Columns.Add(header);

            foreach (var variant in GlobData.ReportVariants)
            {
                var dc = new DataColumn { DataType = typeof(bool), Caption = variant.Name };
                dt.Columns.Add(dc);
            }

            foreach (var reportType in GlobData.ReportTypes)
            {
                var row = dt.NewRow();
                row[0] = reportType.Name;
                dt.Rows.Add(row);
            }

            dgvRadenieSet.DataSource = dt;

            var cheader = dgvRadenieSet.Columns[0];
            cheader.HeaderText = dt.Columns[cheader.HeaderText].Caption;
            cheader.SortMode = DataGridViewColumnSortMode.NotSortable;
            cheader.Width = 120;

            for (var i = 1; i < dgvRadenieSet.Columns.Count; i++)
            {
                var column = (DataGridViewCheckBoxColumn)dgvRadenieSet.Columns[i];
                column.HeaderText = dt.Columns[column.HeaderText].Caption;
                column.Width = 80;
                if (i == dgvRadenieSet.Columns.Count - 1) column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private static void TruncateTable(DataGridView dgv)
        {
            for (var i = 0; i < dgv.Rows.Count; i++)
            for (var j = 1; j <= GlobData.ReportVariants.Count; j++)
                dgv.Rows[i].Cells[j].Value = false;
        }

        private static List<ChosenReportType> GetFromTable(DataGridView dgv, IReadOnlyList<ReportType> allReportTypes)
        {
            var reportTypes = new List<ChosenReportType>();

            for (var i = 0; i < dgv.Rows.Count; i++)
            for (var j = 0; j < GlobData.ReportVariants.Count; j++)
                if (dgv.Rows[i].Cells[j + 1].Value is true)
                {
                    var found = false;
                    foreach (var t in reportTypes)
                        if (t.Type == allReportTypes[i])
                        {
                            t.Variants.Add(GlobData.ReportVariants[j]);
                            found = true;
                        }

                    if (!found)
                        reportTypes.Add(new ChosenReportType
                            { Type = allReportTypes[i], Variants = new List<ReportVariant> { GlobData.ReportVariants[j] } });
                }

            return reportTypes;
        }
    }
}