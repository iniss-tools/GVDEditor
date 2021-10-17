using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
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
    ///     Dialog - Globalne nastavenia vsetkych GVD v priecinku.
    /// </summary>
    public partial class FGlobalSettings : Form
    {
        /// <summary>
        ///     Vsetky casy meskani.
        /// </summary>
        public static readonly BindingList<int> Meskania = new(GlobData.Delays);

        /// <summary>
        ///     Vsetky audio oblasti.
        /// </summary>
        public readonly BindingList<Audio> Audios = new(GlobData.Audios);

        /// <summary>
        ///     Vsetky grafikony.
        /// </summary>
        public readonly BindingList<GVDDirectory> Grafikony;

        private readonly BindingList<Language> Languages = new(GlobData.Languages);
        private readonly List<Language> RBLangs;

        /// <summary>
        ///     Odstranene grafikony.
        /// </summary>
        public readonly List<GVDDirectory> RemovedGVDs = new();

        /// <summary>
        ///     Vsetky typy vlakov.
        /// </summary>
        public readonly BindingList<TrainType> TrainTypes = new(GlobData.TrainsTypes);

        private Color selectedColor = Color.White;

        /// <summary>
        ///     Vytvori novy formular typu <see cref="FGlobalSettings"/>.
        /// </summary>
        /// <param name="gvds">Vsetky grafikony v priecinku.</param>
        /// <param name="openIndex">Index TabPage, ktory sa ma otvorit po otvoreni dialogu.</param>
        public FGlobalSettings(IList<GVDDirectory> gvds, int openIndex = -1)
        {
            InitializeComponent();
            FormUtils.SetFormFont(this);
            this.ApplyTheme();

            Grafikony = new BindingList<GVDDirectory>(gvds);

            listLanguages.DataSource = Languages;
            listGrafikony.DataSource = Grafikony;
            listMeskania.DataSource = Meskania;
            listGrafikony.DisplayMember = "PeriodFormatted";

            cbDefTrainTypSkratka.DataSource = TrainType.GetDefaultValues();
            listTrainTypes.DataSource = TrainTypes;

            var st = new List<Station>(GlobData.Stations);
            st.Sort();
            cbAudioStanica.DataSource = st;
            listAudio.DataSource = Audios;

            if (Meskania.Count == 0)
            {
                bMeskanieEdit.Enabled = false;
                bMeskanieDelete.Enabled = false;
            }

            if (TrainTypes.Count == 0)
            {
                bDefTrainTypEdit.Enabled = false;
                bDefTrainTypDelete.Enabled = false;

                bCustomTrainTypEdit.Enabled = false;
                bCustomTrainTypDelete.Enabled = false;
            }
            else
            {
                listTrainTypes.SelectedIndex = 1;
                listTrainTypes.SelectedIndex = 0;
            }

            cbCustomTrainTypDruh.SelectedIndex = 0;

            RBLangs = RawBankReader.ReadFyzBankFile(GlobData.RAWBANKDir, out _);
            dgvLanguagesRawBank.DataSource = RBLangs;

            if (openIndex != -1) tabControl.SelectTab(openIndex);
        }

        private void bSave_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;

        private void listLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            var jazyk = (Language)listLanguages.SelectedItem;
            tbLanguageName.Text = jazyk.Name;
            tbLanguageSkratka.Text = jazyk.Key;
            cbIsBasic.Checked = jazyk.IsBasic;
        }

        private void bLanguageAdd_Click(object sender, EventArgs e)
        {
            var basic = false;
            foreach (var language in Languages)
                if (language.IsBasic)
                    basic = true;

            if (cbIsBasic.Checked && basic)
            {
                Utils.ShowError(Resources.FGlobalSettings_bLanguageAdd_Click_Iba_1_jazyk_môže_byť_hlavný);
                return;
            }

            var jazyk = new Language
            {
                Name = tbLanguageName.Text, Key = tbLanguageSkratka.Text, IsBasic = cbIsBasic.Checked
            };

            foreach (var lang in Languages)
                if (lang.Key == jazyk.Key)
                {
                    Utils.ShowError(Resources.FGlobalSettings_Zadaný_jazyk_sa_sa_už_v_zozname_nachádza);
                    return;
                }

            var keys = RBLangs.Select(lang => lang.Key).ToList();

            if (!keys.Contains(jazyk.Key))
            {
                Utils.ShowError(Resources.FGlobalSettings_Kľúč_jazyka_sa_nezhoduje_so_žiadnym_jazykom_nacházajúci_sa_v_zvukovej_banke);
                return;
            }

            Languages.Add(jazyk);
        }

        private void bLanguageEdit_Click(object sender, EventArgs e)
        {
            var pos = listLanguages.SelectedIndex;

            if (pos == -1) return;

            var j = 0;
            var basic = false;
            foreach (var l in Languages)
            {
                if (l.IsBasic)
                {
                    basic = true;
                    break;
                }

                j++;
            }

            if (cbIsBasic.Checked && basic && pos != j)
            {
                Utils.ShowError(Resources.FGlobalSettings_bLanguageAdd_Click_Iba_1_jazyk_môže_byť_hlavný);
                return;
            }

            var keys = RBLangs.Select(lang => lang.Key).ToList();

            if (!keys.Contains(tbLanguageSkratka.Text))
            {
                Utils.ShowError(Resources.FGlobalSettings_Kľúč_jazyka_sa_nezhoduje_so_žiadnym_jazykom_nacházajúci_sa_v_zvukovej_banke);
                return;
            }

            var i = 0;
            foreach (var lang in Languages)
            {
                if (lang.Key == tbLanguageSkratka.Text && pos != i)
                {
                    Utils.ShowError(Resources.FGlobalSettings_Zadaný_jazyk_sa_sa_už_v_zozname_nachádza);
                    return;
                }

                i++;
            }

            var language = Languages[pos];
            language.Name = tbLanguageName.Text;
            language.Key = tbLanguageSkratka.Text;
            language.IsBasic = cbIsBasic.Checked;

            Languages.ResetBindings();
        }

        private void bLanguageRemove_Click(object sender, EventArgs e)
        {
            Languages.RemoveAt(listLanguages.SelectedIndex);
        }

        private void listGrafikony_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listGrafikony.SelectedIndex != -1)
            {
                var dir = (GVDDirectory)listGrafikony.SelectedItem;
                tbGrafikonStanica.Text = dir.GVD.ThisStation.Name;
                tbGrafikonObdobie.Text = dir.GVD.StartValidTimeTable.ToString("dd.MM.yyyy") + @" - " +
                                         dir.GVD.EndValidTimeTable.ToString("dd.MM.yyyy");
                tbGrafikonPath.Text = dir.Dir.FullPath;

                nudTabPort.Value = dir.Dir.TablePort ?? 0;

                nudHlaseniePort.Value = dir.Dir.ReportPort ?? 0;

                if (dir.Dir.BackColor.HasValue)
                {
                    selectedColor = dir.Dir.BackColor.Value;
                    pbColor.BackColor = selectedColor;
                }
                else
                {
                    selectedColor = Color.White;
                    pbColor.BackColor = Color.White;
                }
            }
        }

        private void bEditColor_Click(object sender, EventArgs e)
        {
            var result = colorDialogFarba.ShowDialog();
            if (result == DialogResult.OK)
            {
                selectedColor = colorDialogFarba.Color;
                pbColor.BackColor = selectedColor;
            }
        }

        private void bGrafikonEdit_Click(object sender, EventArgs e)
        {
            if (listGrafikony.SelectedIndex != -1)
            {
                var dir = (GVDDirectory)listGrafikony.SelectedItem;
                var portTab = decimal.ToInt32(nudTabPort.Value);
                var portHlas = decimal.ToInt32(nudHlaseniePort.Value);
                dir.Dir.TablePort = portTab == 0 ? null : portTab;
                dir.Dir.ReportPort = portHlas == 0 ? null : portHlas;
                dir.Dir.BackColor = selectedColor;
            }
        }

        private void bGrafikonDelete_Click(object sender, EventArgs e)
        {
            if (listGrafikony.SelectedIndex != -1)
            {
                var result =
                    Utils.ShowQuestion(Resources.FGlobalSettings_Naozaj_chcete_odstrániť_tento_grafikon);

                if (result == DialogResult.Yes)
                {
                    RemovedGVDs.Add(listGrafikony.SelectedItem as GVDDirectory);
                    Grafikony.RemoveAt(listGrafikony.SelectedIndex);
                }
            }
        }

        private void bMeskanieAdd_Click(object sender, EventArgs e)
        {
            var num = decimal.ToInt32(nudMeskanie.Value);
            foreach (var meskanie in Meskania)
                if (meskanie == num)
                {
                    Utils.ShowError(Resources.FGlobalSettings_Táto_hodnota_sa_už_v_zozname_nachádza);
                    return;
                }

            Meskania.Add(num);
        }

        private void bMeskanieEdit_Click(object sender, EventArgs e)
        {
            var index = listMeskania.SelectedIndex;
            if (index == -1) return;

            var num = decimal.ToInt32(nudMeskanie.Value);
            foreach (var meskanie in Meskania)
                if (meskanie == num)
                {
                    Utils.ShowError(Resources.FGlobalSettings_Táto_hodnota_sa_už_v_zozname_nachádza);
                    return;
                }

            Meskania.RemoveAt(index);
            Meskania.Insert(index, decimal.ToInt32(nudMeskanie.Value));
        }

        private void bMeskanieDelete_Click(object sender, EventArgs e)
        {
            if (listMeskania.SelectedIndex != -1) Meskania.RemoveAt(listMeskania.SelectedIndex);

            if (Meskania.Count == 0)
            {
                bMeskanieEdit.Enabled = false;
                bMeskanieDelete.Enabled = false;
            }
        }

        private void listMeskania_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listMeskania.SelectedItem != null) nudMeskanie.Value = (int)listMeskania.SelectedItem;
        }

        private void listTrainTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listTrainTypes.SelectedIndex != 0)
            {
                var typ = (TrainType)listTrainTypes.SelectedItem;

                if (!typ.IsCustom)
                {
                    cbDefTrainTypSkratka.SelectedItem = typ;

                    bDefTrainTypEdit.Enabled = true;
                    bDefTrainTypDelete.Enabled = true;
                    bCustomTrainTypEdit.Enabled = false;
                    bCustomTrainTypDelete.Enabled = false;
                }
                else
                {
                    if (Regex.IsMatch(typ.CategoryTrain, "^Os[1-9]$"))
                        cbCustomTrainTypDruh.SelectedIndex = 0;
                    else if (Regex.IsMatch(typ.CategoryTrain, "^R[1-9]$"))
                        cbCustomTrainTypDruh.SelectedIndex = 1;
                    else if (Regex.IsMatch(typ.CategoryTrain, "^X[1-9]$"))
                        cbCustomTrainTypDruh.SelectedIndex = 2;
                    else if (Regex.IsMatch(typ.CategoryTrain, "^Sl[1-9]$")) cbCustomTrainTypDruh.SelectedIndex = 3;

                    tbCustomTrainTypSkratka.Text = typ.Key;
                    tbCustomTrainTypText.Text = typ.TextInTable;

                    bDefTrainTypEdit.Enabled = false;
                    bDefTrainTypDelete.Enabled = false;
                    bCustomTrainTypEdit.Enabled = true;
                    bCustomTrainTypDelete.Enabled = true;
                }
            }
        }

        private void bDefTrainTypAdd_Click(object sender, EventArgs e)
        {
            var typ = (TrainType)cbDefTrainTypSkratka.SelectedItem;
            foreach (var trainType in TrainTypes)
                if (typ == trainType)
                {
                    Utils.ShowError(Resources.FGlobalSettings_Vybraný_typ_vlaku_sa_už_v_zozname_nachádza);
                    return;
                }

            TrainTypes.Add(typ);

            bDefTrainTypEdit.Enabled = true;
            bDefTrainTypDelete.Enabled = true;
        }

        private void bDefTrainTypEdit_Click(object sender, EventArgs e)
        {
            if (listTrainTypes.SelectedIndex != 0)
            {
                var typ = (TrainType)cbDefTrainTypSkratka.SelectedItem;
                foreach (var trainType in TrainTypes)
                    if (typ == trainType && ((TrainType)listTrainTypes.SelectedItem).Key != typ.Key)
                    {
                        Utils.ShowError(Resources.FGlobalSettings_Vybraný_typ_vlaku_sa_už_v_zozname_nachádza);
                        return;
                    }

                TrainTypes[listTrainTypes.SelectedIndex] = typ;
            }
        }

        private void bDefTrainTypDelete_Click(object sender, EventArgs e)
        {
            if (listTrainTypes.SelectedIndex != 0) TrainTypes.RemoveAt(listTrainTypes.SelectedIndex);

            if (TrainTypes.Count == 0)
            {
                bDefTrainTypEdit.Enabled = false;
                bDefTrainTypDelete.Enabled = false;
                bCustomTrainTypEdit.Enabled = false;
                bCustomTrainTypDelete.Enabled = false;
            }
        }

        private void bCustomTrainTypAdd_Click(object sender, EventArgs e)
        {
            foreach (var trainType in TrainTypes)
                if (tbCustomTrainTypSkratka.Text == trainType.Key)
                {
                    Utils.ShowError(Resources.FGlobalSettings_Click_Zadaný_typ_vlaku_sa_už_v_zozname_nachádza);
                    return;
                }

            if (string.IsNullOrEmpty(tbCustomTrainTypSkratka.Text))
            {
                Utils.ShowError(Resources.FGlobalSettings_Nebola_zadaná_skratka_typu_vlaku);
                return;
            }

            var key = tbCustomTrainTypSkratka.Text;
            var table = tbCustomTrainTypText.Text;
            var category = "";
            var num = 1;

            if (cbCustomTrainTypDruh.SelectedIndex == 0)
            {
                foreach (var trainType in TrainTypes)
                    if (trainType.IsCustom)
                        if (Regex.IsMatch(trainType.CategoryTrain, "^Os[1-9]$"))
                        {
                            trainType.CategoryTrain = "Os" + num;
                            num++;
                        }

                if (num > 9)
                {
                    Utils.ShowError(Resources.FGlobalSettings_Maximálny_počet_typov_vlakov_tohto_druhu_je_9);
                    return;
                }

                category = "Os" + num;
            }
            else if (cbCustomTrainTypDruh.SelectedIndex == 1)
            {
                foreach (var trainType in TrainTypes)
                    if (trainType.IsCustom)
                        if (Regex.IsMatch(trainType.CategoryTrain, "^R[1-9]$"))
                        {
                            trainType.CategoryTrain = "R" + num;
                            num++;
                        }

                if (num > 9)
                {
                    Utils.ShowError(Resources.FGlobalSettings_Maximálny_počet_typov_vlakov_tohto_druhu_je_9);
                    return;
                }

                category = "R" + num;
            }
            else if (cbCustomTrainTypDruh.SelectedIndex == 2)
            {
                foreach (var trainType in TrainTypes)
                    if (trainType.IsCustom)
                        if (Regex.IsMatch(trainType.CategoryTrain, "^X[1-9]$"))
                        {
                            trainType.CategoryTrain = "X" + num;
                            num++;
                        }

                if (num > 9)
                {
                    Utils.ShowError(Resources.FGlobalSettings_Maximálny_počet_typov_vlakov_tohto_druhu_je_9);
                    return;
                }

                category = "X" + num;
            }
            else if (cbCustomTrainTypDruh.SelectedIndex == 3)
            {
                foreach (var trainType in TrainTypes)
                    if (trainType.IsCustom)
                        if (Regex.IsMatch(trainType.CategoryTrain, "^Sl[1-9]$"))
                        {
                            trainType.CategoryTrain = "Sl" + num;
                            num++;
                        }

                if (num > 9)
                {
                    Utils.ShowError(Resources.FGlobalSettings_Maximálny_počet_typov_vlakov_tohto_druhu_je_9);
                    return;
                }

                category = "Sl" + num;
            }

            var typ = new TrainType(category, key, table);
            TrainTypes.Add(typ);

            bCustomTrainTypEdit.Enabled = false;
            bCustomTrainTypDelete.Enabled = false;
        }

        private void bCustomTrainTypEdit_Click(object sender, EventArgs e)
        {
            if (listTrainTypes.SelectedIndex != 0)
            {
                foreach (var trainType in TrainTypes)
                    if (tbCustomTrainTypSkratka.Text == trainType.Key && ((TrainType)listTrainTypes.SelectedItem).Key != tbCustomTrainTypSkratka.Text)
                    {
                        Utils.ShowError(Resources.FGlobalSettings_Click_Zadaný_typ_vlaku_sa_už_v_zozname_nachádza);
                        return;
                    }

                if (string.IsNullOrEmpty(tbCustomTrainTypSkratka.Text))
                {
                    Utils.ShowError(Resources.FGlobalSettings_Nebola_zadaná_skratka_typu_vlaku);
                    return;
                }

                var key = tbCustomTrainTypSkratka.Text;
                var table = tbCustomTrainTypText.Text;
                var category = "";
                var num = 1;

                switch (cbCustomTrainTypDruh.SelectedIndex)
                {
                    case 0:
                    {
                        foreach (var trainType in TrainTypes)
                            if (trainType.IsCustom)
                                if (Regex.IsMatch(trainType.CategoryTrain, "^Os[1-9]$"))
                                {
                                    trainType.CategoryTrain = "Os" + num;
                                    num++;
                                }

                        if (num > 9)
                        {
                            Utils.ShowError(Resources.FGlobalSettings_Maximálny_počet_typov_vlakov_tohto_druhu_je_9);
                            return;
                        }

                        category = "Os" + num;
                        break;
                    }
                    case 1:
                    {
                        foreach (var trainType in TrainTypes)
                            if (trainType.IsCustom)
                                if (Regex.IsMatch(trainType.CategoryTrain, "^R[1-9]$"))
                                {
                                    trainType.CategoryTrain = "R" + num;
                                    num++;
                                }

                        if (num > 9)
                        {
                            Utils.ShowError(Resources.FGlobalSettings_Maximálny_počet_typov_vlakov_tohto_druhu_je_9);
                            return;
                        }

                        category = "R" + num;
                        break;
                    }
                    case 2:
                    {
                        foreach (var trainType in TrainTypes)
                            if (trainType.IsCustom)
                                if (Regex.IsMatch(trainType.CategoryTrain, "^X[1-9]$"))
                                {
                                    trainType.CategoryTrain = "X" + num;
                                    num++;
                                }

                        if (num > 9)
                        {
                            Utils.ShowError(Resources.FGlobalSettings_Maximálny_počet_typov_vlakov_tohto_druhu_je_9);
                            return;
                        }

                        category = "X" + num;
                        break;
                    }
                    case 3:
                    {
                        foreach (var trainType in TrainTypes)
                            if (trainType.IsCustom)
                                if (Regex.IsMatch(trainType.CategoryTrain, "^Sl[1-9]$"))
                                {
                                    trainType.CategoryTrain = "Sl" + num;
                                    num++;
                                }

                        if (num > 9)
                        {
                            Utils.ShowError(Resources.FGlobalSettings_Maximálny_počet_typov_vlakov_tohto_druhu_je_9);
                            return;
                        }

                        category = "Sl" + num;
                        break;
                    }
                }

                TrainTypes[listTrainTypes.SelectedIndex] = new TrainType(category, key, table);
            }
        }

        private void bCustomTrainTypDelete_Click(object sender, EventArgs e)
        {
            if (listTrainTypes.SelectedIndex != 0) TrainTypes.RemoveAt(listTrainTypes.SelectedIndex);

            if (TrainTypes.Count == 0)
            {
                bDefTrainTypEdit.Enabled = false;
                bDefTrainTypDelete.Enabled = false;
                bCustomTrainTypEdit.Enabled = false;
                bCustomTrainTypDelete.Enabled = false;
            }
        }

        private void listAudio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listAudio.SelectedIndex != -1)
            {
                var audio = Audios[listAudio.SelectedIndex];
                cbCustomOnly.Checked = audio.Station.IsCustom;
                cbAudioStanica.SelectedItem = audio.Station;
                tbAudioName.Text = audio.Name;
                tbAudioNazovSkratka.Text = audio.ShortName;
                tbAudioNazovFronta.Text = audio.QueueName;
            }
        }

        private void cbCustomOnly_CheckedChanged(object sender, EventArgs e)
        {
            var st = cbCustomOnly.Checked
                ? new List<Station>(GlobData.CustomStations)
                : new List<Station>(GlobData.Stations);

            st.Sort();
            cbAudioStanica.DataSource = st;
        }

        private void bAudioAdd_Click(object sender, EventArgs e)
        {
            if (cbAudioStanica.SelectedItem == null || string.IsNullOrEmpty(tbAudioName.Text) ||
                string.IsNullOrEmpty(tbAudioNazovSkratka.Text) || string.IsNullOrEmpty(tbAudioNazovFronta.Text))
            {
                Utils.ShowError(Resources.FGlobalSettings_Neboli_vyplnené_všetky_polia);
                return;
            }

            foreach (var a in Audios)
                if (a.Name == tbAudioName.Text)
                {
                    Utils.ShowError(Resources.FGlobalSettings_Názov_tejto_audio_linky_už_existuje);
                    return;
                }

            var audio = new Audio
            {
                Station = cbAudioStanica.SelectedItem as Station,
                Name = tbAudioName.Text,
                ShortName = tbAudioNazovSkratka.Text,
                QueueName = tbAudioNazovFronta.Text
            };

            Audios.Add(audio);
        }

        private void bAudioEdit_Click(object sender, EventArgs e)
        {
            if (listAudio.SelectedIndex != -1)
            {
                if (cbAudioStanica.SelectedItem == null || string.IsNullOrEmpty(tbAudioName.Text) ||
                    string.IsNullOrEmpty(tbAudioNazovSkratka.Text) || string.IsNullOrEmpty(tbAudioNazovFronta.Text))
                {
                    Utils.ShowError(Resources.FGlobalSettings_Neboli_vyplnené_všetky_polia);
                    return;
                }

                var i = 0;
                foreach (var a in Audios)
                {
                    if (a.Name == tbAudioName.Text && i != listAudio.SelectedIndex)
                    {
                        Utils.ShowError(Resources.FGlobalSettings_Názov_tejto_audio_linky_už_existuje);
                        return;
                    }

                    i++;
                }

                var audio = Audios[listAudio.SelectedIndex];
                audio.Station = cbAudioStanica.SelectedItem as Station;
                audio.Name = tbAudioName.Text;
                audio.ShortName = tbAudioNazovSkratka.Text;
                audio.QueueName = tbAudioNazovFronta.Text;

                Audios.ResetBindings();
            }
        }

        private void bAudioDelete_Click(object sender, EventArgs e)
        {
            if (listAudio.SelectedIndex != -1) Audios.RemoveAt(listAudio.SelectedIndex);
        }

        private void tbAudioName_TextChanged(object sender, EventArgs e)
        {
            tbAudioNazovSkratka.Text = tbAudioName.Text;
            tbAudioNazovFronta.Text = tbAudioName.Text;
        }

        private void FGlobalSettings_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start(LinkConsts.LINK_GLOBAL_SETTINGS);
        }
    }
}