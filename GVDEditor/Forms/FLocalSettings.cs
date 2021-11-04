using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Forms
{
    /// <summary>
    ///     Dialog - Lokalne nastavenia konkretneho GVD.
    /// </summary>
    public partial class FLocalSettings : Form
    {
        /// <summary>
        ///     Fyzicke tabule v GVD.
        /// </summary>
        public static BindingList<TablePhysical> TPhysicals;

        /// <summary>
        ///     Katalogove tabule v GVD.
        /// </summary>
        public static BindingList<TableCatalog> TCatalogs;

        /// <summary>
        ///     Logicke tabule v GVD.
        /// </summary>
        public static BindingList<TableLogical> TLogicals;

        /// <summary>
        ///     TabTabs v GVD.
        /// </summary>
        public static BindingList<TableTabTab> TabTabs;

        /// <summary>
        ///     Texty do tabul.
        /// </summary>
        public static BindingList<TableText> TTexts;

        /// <summary>
        ///     Pisma do tabul.
        /// </summary>
        public static BindingList<TableFont> TFonts;

        /// <summary>
        ///     Typy fontov.
        /// </summary>
        public static readonly BindingList<TableFontType> TFontsTypes = new(TableFontType.GetValues());

        /// <summary>
        ///     Stanice dostupne zo suboru STANICE.TXT.
        /// </summary>
        public readonly BindingList<Station> CustomStations;

        private readonly Color defaultBorderColor;

        /// <summary>
        ///     Dopravcovia v GVD.
        /// </summary>
        public readonly BindingList<Operator> Dopravcovia;

        /// <summary>
        ///     Kolaje v GVD.
        /// </summary>
        public readonly BindingList<Track> Kolaje;

        /// <summary>
        ///     Nastupistia v GVD.
        /// </summary>
        public readonly BindingList<Platform> Nastupistia;

        private readonly bool openTabTabEditor;

        /// <summary>
        ///     Tento priecinok.
        /// </summary>
        public readonly GVDDirectory ThisDir;

        /// <summary>
        ///     Priečinok s písmami pre tabule.
        /// </summary>
        public string FontDir;

        /// <summary>
        ///     Vytvori novy formular typu <see cref="FLocalSettings"/>.
        /// </summary>
        /// <param name="dir">Aktualny priecinok s grafikonom.</param>
        /// <param name="openIndex">Index TabPage, ktory sa ma otvorit po otvoreni dialogu.</param>
        public FLocalSettings(GVDDirectory dir, int openIndex = -1)
        {
            InitializeComponent();
            FormUtils.SetFormFont(this);
            this.ApplyTheme();

            defaultBorderColor = nudFontID.BorderColor;

            ThisDir = dir;

            Dopravcovia = new BindingList<Operator>(GlobData.Operators);
            CustomStations = new BindingList<Station>(GlobData.CustomStations);

            Nastupistia = new BindingList<Platform>(GlobData.Platforms);
            Kolaje = new BindingList<Track>(GlobData.Tracks);

            TPhysicals = new BindingList<TablePhysical>(GlobData.TablePhysicals);
            TCatalogs = new BindingList<TableCatalog>(GlobData.TableCatalogs);
            TLogicals = new BindingList<TableLogical>(GlobData.TableLogicals);
            TabTabs = new BindingList<TableTabTab>(GlobData.TabTabs);
            TTexts = new BindingList<TableText>(GlobData.TableTexts);
            TFonts = new BindingList<TableFont>(GlobData.TableFonts);

            tbDir.Text = dir.Dir.FullPath;

            dtpGVDOd.Value = dir.GVD.StartValidTimeTable;
            dtpGVDDo.Value = dir.GVD.EndValidTimeTable;

            dtpDataOd.Value = dir.GVD.StartValidData;
            dtpDataDo.Value = dir.GVD.EndValidData;

            cbStationName.DataSource = GlobData.Stations;

            cbCustomStation.Checked = dir.GVD.ThisStation.IsCustom;

            if (cbCustomStation.Checked)
            {
                nudIDStation.Value = int.Parse(dir.GVD.ThisStation.ID);
                tbGVDStationName.Text = dir.GVD.ThisStation.Name;
            }
            else
            {
                cbStationName.SelectedItem = dir.GVD.ThisStation;
            }

            tbDirName.Text = dir.Dir.DirName;

            listDopravcovia.DataSource = Dopravcovia;

            listNastupistia.DataSource = Nastupistia;
            listKolaje.DataSource = Kolaje;
            cbNastupistia.DataSource = Nastupistia;

            foreach (var logical in TLogicals) clbKolajTables.Items.Add(logical);
            cbFontType.DataSource = TFontsTypes;

            listFyzTabule.DataSource = TPhysicals;
            listLogTabule.DataSource = TLogicals;
            listKatTabule.DataSource = TCatalogs;
            listTabTabs.DataSource = TabTabs;
            listTexty.DataSource = TTexts;
            listFonts.DataSource = TFonts;

            listCustomStations.DataSource = CustomStations;

            FontDir = Utils.ParseStringOrDefault(GlobData.TableFontDir);
            tbFontDir.Text = Utils.ParseStringOrDefault(GlobData.TableFontDir);

            if (Dopravcovia.Count == 0)
            {
                bDopravcaEdit.Enabled = false;
                bDopravcaDelete.Enabled = false;
            }

            if (Nastupistia.Count == 0)
            {
                bNastEdit.Enabled = false;
                bNastDelete.Enabled = false;
            }

            if (Kolaje.Count == 0)
            {
                bKolajEdit.Enabled = false;
                bKolajDelete.Enabled = false;
            }

            if (TPhysicals.Count == 0)
            {
                bFyzTabEdit.Enabled = false;
                bFyzTabCopy.Enabled = false;
                bFyzTabDelete.Enabled = false;
            }

            if (TLogicals.Count == 0)
            {
                bLogTabEdit.Enabled = false;
                bLogTabCopy.Enabled = false;
                bLogTabDelete.Enabled = false;
            }

            if (TCatalogs.Count == 0)
            {
                bKatTabEdit.Enabled = false;
                bKatTabCopy.Enabled = false;
                bKatTabDelete.Enabled = false;
            }

            if (TabTabs.Count == 0) bTabTabDelete.Enabled = false;

            if (TFonts.Count == 0)
            {
                bFontEdit.Enabled = false;
                bFontDelete.Enabled = false;
            }

            if (CustomStations.Count == 0)
            {
                bCStationEdit.Enabled = false;
                bCStationDelete.Enabled = false;
            }

            if (openIndex != -1)
            {
                if (openIndex == -2)
                {
                    tabControl.SelectTab(8);
                    openTabTabEditor = true;
                }
                else
                {
                    tabControl.SelectTab(openIndex);
                }
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            var gvdInfo = ThisDir.GVD;
            gvdInfo.StartValidData = dtpDataOd.Value.Date;
            gvdInfo.EndValidData = dtpDataDo.Value.Date;
            gvdInfo.StartValidTimeTable = dtpGVDOd.Value.Date;
            gvdInfo.EndValidTimeTable = dtpGVDDo.Value.Date;

            if (cbCustomStation.Checked)
            {
                var id = decimal.ToInt32(nudIDStation.Value).ToString();
                var name = tbGVDStationName.Text;
                gvdInfo.ThisStation = new Station(id, name, IsCustom: true);
            }
            else
            {
                gvdInfo.ThisStation = (Station)cbStationName.SelectedItem;
            }

            DialogResult = DialogResult.OK;
        }

        private void bOpenDir_Click(object sender, EventArgs e) => Process.Start(ThisDir.Dir.FullPath);

        private void cbCustomStation_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCustomStation.Checked)
            {
                cbStationName.Enabled = false;
                nudIDStation.Enabled = true;
                tbGVDStationName.Enabled = true;
            }
            else
            {
                cbStationName.Enabled = true;
                nudIDStation.Enabled = false;
                tbGVDStationName.Enabled = false;
                tbDirName.Text = cbStationName.SelectedItem + @"." + dtpDataOd.Value.Year;
            }
        }

        private void dtpDataOd_ValueChanged(object sender, EventArgs e)
        {
            if (cbCustomStation.Checked)
                tbDirName.Text = tbGVDStationName.Text + @"." + dtpDataOd.Value.Year;
            else
                tbDirName.Text = cbStationName.SelectedItem + @"." + dtpDataOd.Value.Year;
        }

        private void tbGVDStationName_TextChanged(object sender, EventArgs e)
        {
            tbDirName.Text = tbGVDStationName.Text + @"." + dtpDataOd.Value.Year;
        }

        private void bDirChange_Click(object sender, EventArgs e)
        {
            var dirname = tbDirName.Text;
            var oldFullPath = ThisDir.Dir.FullPath;
            var fullpath = GlobData.DATADir + Path.DirectorySeparatorChar + tbDirName.Text;

            if (string.IsNullOrEmpty(dirname))
            {
                Utils.ShowError(Resources.FLocalSettings_Názov_priečinka_grafikonu_je_prázdny);
                DialogResult = DialogResult.None;
                return;
            }

            if (Directory.Exists(fullpath))
            {
                Utils.ShowError(Resources.Priečinok_s_týmto_názvom_už_existuje__Zmeňte_jeho_názov);
                DialogResult = DialogResult.None;
                return;
            }

            ThisDir.Dir.DirName = dirname;
            ThisDir.Dir.FullPath = fullpath;

            Directory.CreateDirectory(fullpath);

            var files = Directory.GetFiles(oldFullPath, "*.*", SearchOption.AllDirectories).ToList();

            foreach (var file in files)
            {
                var mFile = new FileInfo(file);
                mFile.MoveTo(Utils.CombinePath(fullpath, mFile.Name));
            }

            Directory.Delete(oldFullPath);

            var dirlist = FMain.ObdobiaList.Select(gvd => gvd.Dir).ToList();
            TXTParser.WriteDirList(dirlist);

            tbDir.Text = fullpath;
        }

        private void listDopravcovia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listDopravcovia.SelectedIndex != -1)
            {
                var dopravca = (Operator)listDopravcovia.SelectedItem;
                if (dopravca == Operator.None)
                {
                    bDopravcaEdit.Enabled = false;
                    bDopravcaDelete.Enabled = false;
                }
                else
                {
                    bDopravcaEdit.Enabled = true;
                    bDopravcaDelete.Enabled = true;
                }

                tbDopravca.Text = dopravca.Name;
            }
        }

        private void bDopravcaAdd_Click(object sender, EventArgs e)
        {
            var dopravca = new Operator(Dopravcovia.Count, tbDopravca.Text);
            foreach (var dop in Dopravcovia)
                if (dop.Name == dopravca.Name)
                {
                    Utils.ShowError(Resources.FLocalSettings_Zadaný_dopravca_už_existuje);
                    return;
                }

            Dopravcovia.Add(dopravca);

            bDopravcaEdit.Enabled = true;
            bDopravcaDelete.Enabled = true;
        }

        private void bDopravcaEdit_Click(object sender, EventArgs e)
        {
            if (listDopravcovia.SelectedIndex != -1)
            {
                var dopravca = Dopravcovia[listDopravcovia.SelectedIndex];
                dopravca.Name = tbDopravca.Text;
                var i = 0;
                foreach (var test in Dopravcovia)
                {
                    if (dopravca.Name == test.Name && i != listDopravcovia.SelectedIndex)
                    {
                        Utils.ShowError(Resources.FLocalSettings_Zadaný_dopravca_už_existuje);
                        return;
                    }

                    i++;
                }
            }
        }

        private void bDopravcaDelete_Click(object sender, EventArgs e)
        {
            if (listDopravcovia.SelectedIndex != -1)
            {
                var index = listDopravcovia.SelectedIndex;
                var op = Dopravcovia[index];
                foreach (var train in GlobData.Trains)
                    if (train.Operator == op)
                        train.Operator = Operator.None;

                Dopravcovia.RemoveAt(index);

                if (Dopravcovia.Count == 0)
                {
                    bDopravcaEdit.Enabled = false;
                    bDopravcaDelete.Enabled = false;
                }
            }
        }

        private void listNastupistia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listNastupistia.SelectedIndex != -1)
            {
                var nastupiste = (Platform)listNastupistia.SelectedItem;

                if (nastupiste == Platform.None)
                {
                    bNastEdit.Enabled = false;
                    bNastDelete.Enabled = false;
                }
                else
                {
                    bNastEdit.Enabled = true;
                    bNastDelete.Enabled = true;
                }

                tbNastOznacenie.Text = nastupiste.Key;
                tbNastFullName.Text = nastupiste.FullName;
                tbNastSound.Text = nastupiste.SoundName;
            }
        }

        private void tbNastOznacenie_TextChanged(object sender, EventArgs e)
        {
            tbNastFullName.Text = Resources.FLocalSettings_Nástupište_ + tbNastOznacenie.Text;
        }

        private void bNastAdd_Click(object sender, EventArgs e)
        {
            var nastupiste = new Platform(tbNastOznacenie.Text, tbNastFullName.Text, tbNastSound.Text);
            foreach (var test in Nastupistia)
                if (nastupiste.EqualsKeys(test))
                {
                    Utils.ShowError(Resources.FLocalSettings_Zadaný_kľúč_nástupišťa_už_existuje);
                    return;
                }

            Nastupistia.Add(nastupiste);

            bNastEdit.Enabled = true;
            bNastDelete.Enabled = true;
        }

        private void bNastEdit_Click(object sender, EventArgs e)
        {
            if (listNastupistia.SelectedIndex != -1)
            {
                if (string.IsNullOrEmpty(tbNastOznacenie.Text) || string.IsNullOrEmpty(tbNastFullName.Text) ||
                    string.IsNullOrEmpty(tbNastSound.Text))
                {
                    Utils.ShowError(Resources.FLocalSettings_Všetky_parametre_sú_povinné);
                    return;
                }

                for (var i = 0; i < Nastupistia.Count; i++)
                    if (tbNastOznacenie.Text == Nastupistia[i].Key && i != listNastupistia.SelectedIndex)
                    {
                        Utils.ShowError(Resources.FLocalSettings_Zadaný_kľúč_nástupišťa_už_existuje);
                        return;
                    }

                var platform = Nastupistia[listNastupistia.SelectedIndex];
                platform.Key = tbNastOznacenie.Text;
                platform.FullName = tbNastFullName.Text;
                platform.SoundName = tbNastSound.Text;

                Nastupistia.ResetBindings();
            }
        }

        private void bNastDelete_Click(object sender, EventArgs e)
        {
            if (listNastupistia.SelectedIndex != -1)
            {
                var delete = true;
                var where = " ";
                var index = listNastupistia.SelectedIndex;
                var nast = Nastupistia[index];

                foreach (var tr in Kolaje)
                    if (tr.Platform == nast)
                    {
                        delete = false;
                        where += $"Koľaj {tr.Name}";
                        break;
                    }

                if (delete)
                {
                    Nastupistia.RemoveAt(index);

                    if (Nastupistia.Count == 0)
                    {
                        bNastEdit.Enabled = false;
                        bNastDelete.Enabled = false;
                    }
                }
                else
                {
                    Utils.ShowError(Resources.SelectedItemRemoveCancel + where);
                }
            }
        }

        private void listKolaje_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listKolaje.SelectedIndex != -1)
            {
                var kolaj = (Track)listKolaje.SelectedItem;

                if (kolaj == Track.None)
                {
                    bKolajEdit.Enabled = false;
                    bKolajDelete.Enabled = false;
                }
                else
                {
                    bKolajEdit.Enabled = true;
                    bKolajDelete.Enabled = true;
                }

                tbKolajOznacenie.Text = kolaj.Key;
                tbKolajFullName.Text = kolaj.FullName;
                tbKolajSound.Text = kolaj.SoundName;
                cbNastupistia.SelectedItem = kolaj.Platform;

                if (clbKolajTables.Items.Count != 0)
                    for (var i = 0; i < TLogicals.Count; i++)
                        clbKolajTables.SetItemChecked(i, false);

                foreach (var table in kolaj.Tables)
                    clbKolajTables.SetItemChecked(clbKolajTables.Items.IndexOf(table), true);
            }
        }

        private void tbKolajOznacenie_TextChanged(object sender, EventArgs e)
        {
            tbKolajFullName.Text = Resources.FLocalSettings_Koľaj_ + tbKolajOznacenie.Text;
        }

        private void bKolajAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbKolajOznacenie.Text) || string.IsNullOrEmpty(tbKolajFullName.Text) ||
                string.IsNullOrEmpty(tbKolajSound.Text))
            {
                Utils.ShowError(Resources.FLocalSettings_Všetky_parametre_sú_povinné);
                return;
            }

            var track = new Track
            {
                Key = tbKolajOznacenie.Text,
                FullName = tbKolajFullName.Text,
                SoundName = tbKolajSound.Text,
                Platform = (Platform)cbNastupistia.SelectedItem
            };

            foreach (var test in Kolaje)
                if (track.EqualsKeys(test))
                {
                    Utils.ShowError(Resources.FLocalSettings_Zadaný_kľúč_koľaje_už_existuje);
                    return;
                }

            foreach (var item in clbKolajTables.CheckedItems) 
                track.Tables.Add(item as TableLogical);

            Kolaje.Add(track);

            bKolajEdit.Enabled = true;
            bKolajDelete.Enabled = true;
        }

        private void bKolajEdit_Click(object sender, EventArgs e)
        {
            if (listKolaje.SelectedIndex != -1)
            {
                if (string.IsNullOrEmpty(tbKolajOznacenie.Text) || string.IsNullOrEmpty(tbKolajFullName.Text) ||
                    string.IsNullOrEmpty(tbKolajSound.Text))
                {
                    Utils.ShowError(Resources.FLocalSettings_Všetky_parametre_sú_povinné);
                    return;
                }

                for (var i = 0; i < Kolaje.Count; i++)
                    if (tbKolajOznacenie.Text == Kolaje[i].Key && i != listKolaje.SelectedIndex)
                    {
                        Utils.ShowError(Resources.FLocalSettings_Zadaný_kľúč_koľaje_už_existuje);
                        return;
                    }

                var track = Kolaje[listKolaje.SelectedIndex];
                track.Key = tbKolajOznacenie.Text;
                track.FullName = tbKolajFullName.Text;
                track.SoundName = tbKolajSound.Text;
                track.Platform = (Platform)cbNastupistia.SelectedItem;

                track.Tables.Clear();
                foreach (var item in clbKolajTables.CheckedItems) 
                    track.Tables.Add(item as TableLogical);

                Kolaje.ResetBindings();
            }
        }

        private void bKolajDelete_Click(object sender, EventArgs e)
        {
            if (listKolaje.SelectedIndex != -1)
            {
                var index = listKolaje.SelectedIndex;
                var kolaj = Kolaje[index];
                foreach (var train in GlobData.Trains)
                    if (train.Track == kolaj)
                        train.Track = Track.None;

                Kolaje.RemoveAt(index);

                if (Kolaje.Count == 0)
                {
                    bKolajEdit.Enabled = false;
                    bKolajDelete.Enabled = false;
                }
            }
        }

        private void bFyzTabAdd_Click(object sender, EventArgs e)
        {
            var eptf = new FTablePhysical(new TablePhysical(), TCatalogs);
            var result = eptf.ShowDialog();
            if (result == DialogResult.OK)
            {
                TPhysicals.Add(eptf.ThisTable);

                bFyzTabEdit.Enabled = true;
                bFyzTabCopy.Enabled = true;
                bFyzTabDelete.Enabled = true;
            }
        }

        private void bFyzTabCopy_Click(object sender, EventArgs e)
        {
            if (listFyzTabule.SelectedIndex != -1)
            {
                var eptf = new FTablePhysical(TPhysicals[listFyzTabule.SelectedIndex], TCatalogs, true);
                var result = eptf.ShowDialog();
                if (result == DialogResult.OK) TPhysicals.Add(eptf.ThisTable);
            }
        }

        private void bFyzTabEdit_Click(object sender, EventArgs e)
        {
            if (listFyzTabule.SelectedIndex != -1)
            {
                var eptf = new FTablePhysical(TPhysicals[listFyzTabule.SelectedIndex], TCatalogs);
                var result = eptf.ShowDialog();
                if (result == DialogResult.OK) TPhysicals.ResetBindings();
            }
        }

        private void bFyzTabDelete_Click(object sender, EventArgs e)
        {
            if (listFyzTabule.SelectedIndex != -1)
            {
                var delete = true;
                var where = " ";
                var index = listFyzTabule.SelectedIndex;
                var tp = TPhysicals[index];

                foreach (var tl in TLogicals)
                {
                    foreach (var trecord in tl.Records)
                    {
                        foreach (TablePosition position in trecord)
                            if (position.Table == tp)
                            {
                                delete = false;
                                where += $"Logická tabuľa {tl.Name}, pozícia {position.Position}.";
                                break;
                            }

                        if (!delete)
                            break;
                    }

                    if (!delete)
                        break;
                }

                if (delete)
                {
                    TPhysicals.RemoveAt(index);

                    if (TPhysicals.Count == 0)
                    {
                        bFyzTabEdit.Enabled = false;
                        bFyzTabCopy.Enabled = false;
                        bFyzTabDelete.Enabled = false;
                    }
                }
                else
                {
                    Utils.ShowError(Resources.SelectedItemRemoveCancel + where);
                }
            }
        }

        private void bLogTabAdd_Click(object sender, EventArgs e)
        {
            var eltf = new FTableLogical(new TableLogical(), TPhysicals.ToList());
            var result = eltf.ShowDialog();
            if (result == DialogResult.OK)
            {
                TLogicals.Add(eltf.ThisTable);

                bLogTabEdit.Enabled = true;
                bLogTabCopy.Enabled = true;
                bLogTabDelete.Enabled = true;
            }
        }

        private void bLogTabCopy_Click(object sender, EventArgs e)
        {
            if (listLogTabule.SelectedIndex != -1)
            {
                var eltf = new FTableLogical(TLogicals[listLogTabule.SelectedIndex], TPhysicals.ToList(), true);
                var result = eltf.ShowDialog();
                if (result == DialogResult.OK) TLogicals.Add(eltf.ThisTable);
            }
        }

        private void bLogTabEdit_Click(object sender, EventArgs e)
        {
            if (listLogTabule.SelectedIndex != -1)
            {
                var eltf = new FTableLogical(TLogicals[listLogTabule.SelectedIndex], TPhysicals.ToList());
                var result = eltf.ShowDialog();
                if (result == DialogResult.OK) TLogicals.ResetBindings();
            }
        }

        private void bLogTabDelete_Click(object sender, EventArgs e)
        {
            if (listLogTabule.SelectedIndex != -1)
            {
                var delete = true;
                var where = " ";
                var index = listLogTabule.SelectedIndex;
                var tlog = TLogicals[index];

                foreach (var tr in Kolaje)
                {
                    foreach (var logical in tr.Tables)
                        if (logical == tlog)
                        {
                            delete = false; 
                            where += $"Koľaj {tr.Name}";
                            break;
                        }

                    if (!delete)
                        break;
                }

                if (delete)
                {
                    TLogicals.RemoveAt(index);

                    if (TLogicals.Count == 0)
                    {
                        bLogTabEdit.Enabled = false;
                        bLogTabCopy.Enabled = false;
                        bLogTabDelete.Enabled = false;
                    }
                }
                else
                {
                    Utils.ShowError(Resources.SelectedItemRemoveCancel + where);
                }
            }
        }

        private void bKatTabAdd_Click(object sender, EventArgs e)
        {
            var ectf = new FTableCatalog(new TableCatalog(), TabTabs.ToList());
            var result = ectf.ShowDialog();
            if (result == DialogResult.OK)
            {
                TCatalogs.Add(ectf.ThisTable);

                bKatTabEdit.Enabled = true;
                bKatTabCopy.Enabled = true;
                bKatTabDelete.Enabled = true;
            }
        }

        private void bKatTabCopy_Click(object sender, EventArgs e)
        {
            if (listKatTabule.SelectedIndex != -1)
            {
                var ectf = new FTableCatalog(TCatalogs[listKatTabule.SelectedIndex], TabTabs.ToList(), true);
                var result = ectf.ShowDialog();
                if (result == DialogResult.OK) TCatalogs.Add(ectf.ThisTable);
            }
        }

        private void bKatTabEdit_Click(object sender, EventArgs e)
        {
            if (listKatTabule.SelectedIndex != -1)
            {
                var ectf = new FTableCatalog(TCatalogs[listKatTabule.SelectedIndex], TabTabs.ToList());
                var result = ectf.ShowDialog();
                if (result == DialogResult.OK) TCatalogs.ResetBindings();
            }
        }

        private void bKatTabDelete_Click(object sender, EventArgs e)
        {
            if (listKatTabule.SelectedIndex != -1)
            {
                var delete = true;
                var where = " ";
                var index = listKatTabule.SelectedIndex;
                var tcat = TCatalogs[index];

                foreach (var ph in TPhysicals)
                    if (ph.TableCatalog == tcat)
                    {
                        delete = false;
                        where += $"Fyzická tabuľa {ph.Name}";
                        break;
                    }

                if (delete)
                    foreach (var tt in TTexts)
                    {
                        foreach (var realization in tt.Realizations)
                            if (realization.Table == tcat)
                            {
                                delete = false;
                                where += $"Text do tabule {tt.Name}, realizácia {realization.Item.Name}";
                                break;
                            }

                        if (!delete)
                            break;
                    }

                if (delete)
                {
                    TCatalogs.RemoveAt(listKatTabule.SelectedIndex);

                    if (TCatalogs.Count == 0)
                    {
                        bKatTabEdit.Enabled = false;
                        bKatTabCopy.Enabled = false;
                        bKatTabDelete.Enabled = false;
                    }
                }
                else
                {
                    Utils.ShowError(Resources.SelectedItemRemoveCancel + where);
                }
            }
        }

        private void bOpenEditorTab_Click(object sender, EventArgs e)
        {
            var ettf = new FTabTab();
            var result = ettf.ShowDialog();
            if (result == DialogResult.OK)
            {
                TabTabs.Clear();
                foreach (var doc in ettf.documents) TabTabs.Add(doc.TabTab);
                TabTabs.ResetBindings();
            }

            if (TabTabs.Count == 0) bTabTabDelete.Enabled = false;
        }

        private void bTabTabDelete_Click(object sender, EventArgs e)
        {
            if (listTabTabs.SelectedIndex != -1)
            {
                var delete = true;
                var where = " ";
                var index = listLogTabule.SelectedIndex;
                var tab = TabTabs[index];

                foreach (var tc in TCatalogs)
                {
                    foreach (var ti in tc.Items)
                    {
                        if (ti.Tab1 == tab)
                        {
                            delete = false;
                            where += $"Katalógová tabuľa {tc.Name}, položka {ti.Name}, TAB1";
                            break;
                        }

                        if (ti.Tab2 == tab)
                        {
                            delete = false;
                            where += $"Katalógová tabuľa {tc.Name}, položka {ti.Name}, TAB2";
                            break;
                        }
                    }

                    if (!delete)
                        break;
                }

                if (delete)
                {
                    TabTabs.RemoveAt(listTabTabs.SelectedIndex);

                    if (TabTabs.Count == 0)
                        //bTabTabEdit.Enabled = false;
                        bTabTabDelete.Enabled = false;
                }
                else
                {
                    Utils.ShowError(Resources.SelectedItemRemoveCancel + where);
                }
            }
        }

        private void bTextAdd_Click(object sender, EventArgs e)
        {
            var ettf = new FTableText(new TableText(), TCatalogs.ToList(), ThisDir.GVD, -1);
            var result = ettf.ShowDialog();
            if (result == DialogResult.OK)
            {
                TTexts.Add(ettf.ThisTableText);

                bTextEdit.Enabled = true;
                bTextDelete.Enabled = true;
            }
        }

        private void bTextEdit_Click(object sender, EventArgs e)
        {
            if (listTexty.SelectedIndex != -1)
            {
                var index = listTexty.SelectedIndex;
                var ettf = new FTableText(TTexts[index], TCatalogs.ToList(), ThisDir.GVD, index);
                var result = ettf.ShowDialog();
                if (result == DialogResult.OK)
                {
                    TTexts.RemoveAt(index);
                    TTexts.Insert(index, ettf.ThisTableText);
                    listTexty.SelectedIndex = index;
                }
            }
        }

        private void bTextDelete_Click(object sender, EventArgs e)
        {
            if (listTexty.SelectedIndex != -1) TTexts.RemoveAt(listTexty.SelectedIndex);

            if (TTexts.Count == 0)
            {
                bTextEdit.Enabled = false;
                bTextDelete.Enabled = false;
            }
        }

        private void listFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listFonts.SelectedIndex != -1)
            {
                var tfont = (TableFont)listFonts.SelectedItem;
                tbFontName.Text = tfont.Name;
                nudFontID.Value = tfont.FontID;
                tbFontFile.Text = tfont.FileName;
                cbFontType.SelectedItem = tfont.Type;
                cbFontDia.Checked = tfont.IsDia;
                cbFontProportional.Checked = tfont.IsProportional;
                cbFontLower.Checked = tfont.IsLower;
                cbFontUpper.Checked = tfont.IsUpper;
                cbFontIsNumber.Checked = tfont.IsNumber;
                cbFontSpecChar.Checked = tfont.IsSpecChars;
                cbFontSpecAssigments.Checked = tfont.IsSpecAssigment;
            }
        }

        private void bFontAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbFontName.Text))
            {
                Utils.ShowError(Resources.FLocalSettings_Nezadaný_názov_písma);
                return;
            }

            var tfont = new TableFont
            {
                Name = tbFontName.Text,
                FontID = decimal.ToInt32(nudFontID.Value),
                Type = (TableFontType)cbFontType.SelectedItem,
                Width = decimal.ToInt32(nudFontWidth.Value),
                Size = decimal.ToInt32(nudFontSize.Value),
                FileName = tbFontFile.Text,
                IsDia = cbFontDia.Checked,
                IsProportional = cbFontProportional.Checked,
                IsLower = cbFontLower.Checked,
                IsUpper = cbFontUpper.Checked,
                IsNumber = cbFontIsNumber.Checked,
                IsSpecChars = cbFontSpecChar.Checked,
                IsSpecAssigment = cbFontSpecAssigments.Checked
            };

            TFonts.Add(tfont);

            bFontEdit.Enabled = true;
            bFontDelete.Enabled = true;
        }

        private void bFontEdit_Click(object sender, EventArgs e)
        {
            var index = listFonts.SelectedIndex;
            if (index == -1) return;

            if (string.IsNullOrEmpty(tbFontName.Text))
            {
                Utils.ShowError(Resources.FLocalSettings_Nezadaný_názov_písma);
                return;
            }

            var tfont = TFonts[index];
            tfont.Name = tbFontName.Text;
            tfont.FontID = decimal.ToInt32(nudFontID.Value);
            tfont.Type = (TableFontType)cbFontType.SelectedItem;
            tfont.Width = decimal.ToInt32(nudFontWidth.Value);
            tfont.Size = decimal.ToInt32(nudFontSize.Value);
            tfont.FileName = tbFontFile.Text;
            tfont.IsDia = cbFontDia.Checked;
            tfont.IsProportional = cbFontProportional.Checked;
            tfont.IsLower = cbFontLower.Checked;
            tfont.IsUpper = cbFontUpper.Checked;
            tfont.IsNumber = cbFontIsNumber.Checked;
            tfont.IsSpecChars = cbFontSpecChar.Checked;
            tfont.IsSpecAssigment = cbFontSpecAssigments.Checked;

            TFonts.ResetBindings();
        }

        private void bFontDelete_Click(object sender, EventArgs e)
        {
            if (listFonts.SelectedIndex != -1) TFonts.RemoveAt(listFonts.SelectedIndex);

            if (TFonts.Count == 0)
            {
                bFontEdit.Enabled = false;
                bFontDelete.Enabled = false;
            }
        }

        private void nudFontID_ValueChanged(object sender, EventArgs e)
        {
            switch (nudFontID.Value % 4)
            {
                case 1:
                    nudFontID.BorderColor = defaultBorderColor;
                    nudFontID.ArrowsColor = Color.White;
                    nudFontID.BackColor = Color.Red;
                    nudFontID.ForeColor = Color.White;
                    break;
                case 2:
                    nudFontID.BorderColor = defaultBorderColor;
                    nudFontID.ArrowsColor = Color.White;
                    nudFontID.BackColor = Color.Green;
                    nudFontID.ForeColor = Color.White;
                    break;
                case 3:
                    nudFontID.BorderColor = defaultBorderColor;
                    nudFontID.ArrowsColor = Color.Black;
                    nudFontID.BackColor = Color.Yellow;
                    nudFontID.ForeColor = Color.Black;
                    break;
                default:
                    nudFontID.BorderColor = Color.DimGray;
                    nudFontID.ArrowsColor = Color.Black;
                    nudFontID.BackColor = Color.White;
                    nudFontID.ForeColor = Color.Black;
                    break;
            }
        }

        private void bOpenFontDir_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbFontDir.Text)) fdbFontsDir.SelectedPath = fdbFontsDir.SelectedPath;
            var result = fdbFontsDir.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbFontDir.Text = fdbFontsDir.SelectedPath;
                FontDir = fdbFontsDir.SelectedPath;
            }
        }

        private void listCustomStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listCustomStations.SelectedIndex != -1)
            {
                var st = CustomStations[listCustomStations.SelectedIndex];
                nudIDStanice.Value = int.Parse(st.ID);
                tbStationName.Text = st.Name;
            }
        }

        private void bCStationAdd_Click(object sender, EventArgs e)
        {
            var err = false;

            var id = decimal.ToInt32(nudIDStanice.Value);

            foreach (var station in GlobData.Stations)
                if (station.ID == id.ToString())
                    err = true;

            foreach (var station in CustomStations)
                if (station.ID == id.ToString())
                    err = true;

            if (err)
            {
                Utils.ShowError(Resources.FLocalSettings_Zadané_ID_stanice_už_má_iná_stanica);
                return;
            }

            if (string.IsNullOrEmpty(tbStationName.Text))
            {
                Utils.ShowError(Resources.FLocalSettings_Nezadaný_názov_stanice);
                return;
            }

            var name = tbStationName.Text;

            var st = new Station(id.ToString(), name) { IsCustom = true };
            CustomStations.Add(st);

            bCStationEdit.Enabled = true;
            bCStationDelete.Enabled = true;
        }

        private void bCStationEdit_Click(object sender, EventArgs e)
        {
            if (listCustomStations.SelectedIndex != -1)
            {
                var err = false;

                var id = decimal.ToInt32(nudFontID.Value);

                foreach (var station in GlobData.Stations)
                    if (station.ID == id.ToString())
                        err = true;

                foreach (var station in CustomStations)
                    if (station.ID == id.ToString())
                        err = true;

                if (err)
                {
                    Utils.ShowError(Resources.FLocalSettings_Zadané_ID_stanice_už_má_iná_stanica);
                    return;
                }

                if (string.IsNullOrEmpty(tbStationName.Text))
                {
                    Utils.ShowError(Resources.FLocalSettings_Nezadaný_názov_stanice);
                    return;
                }

                var name = tbStationName.Text;

                var st = CustomStations[listCustomStations.SelectedIndex];
                st.ID = id.ToString();
                st.Name = name;
                st.IsCustom = true;
                CustomStations.ResetBindings();
            }
        }

        private void bCStationDelete_Click(object sender, EventArgs e)
        {
            if (listCustomStations.SelectedIndex != -1) CustomStations.RemoveAt(listCustomStations.SelectedIndex);

            if (CustomStations.Count == 0)
            {
                bCStationEdit.Enabled = false;
                bCStationDelete.Enabled = false;
            }
        }

        private void FLocalSettings_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start(LinkConsts.LINK_LOCAL_SETTINGS);
        }

        private void FLocalSettings_Load(object sender, EventArgs e)
        {
            if (openTabTabEditor) bOpenEditorTab.PerformClick();
        }

        private void listKatTabule_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbCommentKat.Text = listKatTabule.SelectedIndex != -1 ? TCatalogs[listKatTabule.SelectedIndex].Comment : "";
        }

        private void listTexty_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbCommentTText.Text = listTexty.SelectedIndex != -1 ? TTexts[listTexty.SelectedIndex].Comment : "";
        }

        private void listFyzTabule_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbCommentFyz.Text = listFyzTabule.SelectedIndex != -1 ? TPhysicals[listFyzTabule.SelectedIndex].Comment : "";
        }

        private void listLogTabule_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbCommentLog.Text = listLogTabule.SelectedIndex != -1 ? TLogicals[listLogTabule.SelectedIndex].Comment : "";
        }
    }
}