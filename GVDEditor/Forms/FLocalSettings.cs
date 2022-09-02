using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Lokálne nastavenia konkrétneho GVD.
/// </summary>
public partial class FLocalSettings : Form
{
    private readonly bool _openTabTabEditor;

    /// <summary>
    ///     Tento priečinok.
    /// </summary>
    public readonly GVDDirectory ThisDir;

    /// <summary>
    ///     Priečinok s písmami pre tabule.
    /// </summary>
    public string FontDir;

    private readonly Color _defaultBorderColor;

    /// <summary>
    ///     Vytvori novy formulár typu <see cref="FLocalSettings"/>.
    /// </summary>
    /// <param name="dir">Aktualny priecinok s grafikonom.</param>
    /// <param name="openIndex">Index TabPage, ktory sa ma otvorit po otvoreni dialogu.</param>
    public FLocalSettings(GVDDirectory dir, int openIndex = -1)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();

        _defaultBorderColor = nudFontID.BorderColor;

        ThisDir = dir;

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

        listDopravcovia.DataSource = GlobData.Operators;

        listNastupistia.DataSource = GlobData.Platforms;
        listKolaje.DataSource = GlobData.Tracks;
        cbNastupistia.DataSource = GlobData.Platforms;

        foreach (var logical in GlobData.TableLogicals) clbKolajTables.Items.Add(logical);
        cbFontType.DataSource = TableFontType.GetValues();

        listFyzTabule.DataSource = GlobData.TablePhysicals;
        listLogTabule.DataSource = GlobData.TableLogicals;
        listKatTabule.DataSource = GlobData.TableCatalogs;
        listTabTabs.DataSource = GlobData.TabTabs;
        listTexty.DataSource = GlobData.TableTexts;
        listFonts.DataSource = GlobData.TableFonts;

        listCustomStations.DataSource = GlobData.CustomStations;

        FontDir = Utils.ParseStringOrDefault(GlobData.TableFontDir);
        tbFontDir.Text = Utils.ParseStringOrDefault(GlobData.TableFontDir);

        if (GlobData.Operators.Count == 0)
        {
            bDopravcaEdit.Enabled = false;
            bDopravcaDelete.Enabled = false;
        }

        if (GlobData.Platforms.Count == 0)
        {
            bNastEdit.Enabled = false;
            bNastDelete.Enabled = false;
        }

        if (GlobData.Tracks.Count == 0)
        {
            bKolajEdit.Enabled = false;
            bKolajDelete.Enabled = false;
        }

        if (GlobData.TablePhysicals.Count == 0)
        {
            bFyzTabEdit.Enabled = false;
            bFyzTabCopy.Enabled = false;
            bFyzTabDelete.Enabled = false;
        }

        if (GlobData.TableLogicals.Count == 0)
        {
            bLogTabEdit.Enabled = false;
            bLogTabCopy.Enabled = false;
            bLogTabDelete.Enabled = false;
        }

        if (GlobData.TableCatalogs.Count == 0)
        {
            bKatTabEdit.Enabled = false;
            bKatTabCopy.Enabled = false;
            bKatTabDelete.Enabled = false;
        }

        if (GlobData.TabTabs.Count == 0) 
            bTabTabDelete.Enabled = false;

        if (GlobData.TableFonts.Count == 0)
        {
            bFontEdit.Enabled = false;
            bFontDelete.Enabled = false;
        }

        if (GlobData.CustomStations.Count == 0)
        {
            bCStationEdit.Enabled = false;
            bCStationDelete.Enabled = false;
        }

        if (openIndex != -1)
        {
            if (openIndex == -2)
            {
                tabControl.SelectTab(8);
                _openTabTabEditor = true;
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
        var fullpath = GlobData.DataDir + Path.DirectorySeparatorChar + tbDirName.Text;

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
        TxtParser.WriteDirList(dirlist);

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
        var dopravca = new Operator(GlobData.Operators.Count, tbDopravca.Text);
        foreach (var dop in GlobData.Operators)
            if (dop.Name == dopravca.Name)
            {
                Utils.ShowError(Resources.FLocalSettings_Zadaný_dopravca_už_existuje);
                return;
            }

        GlobData.Operators.Add(dopravca);

        bDopravcaEdit.Enabled = true;
        bDopravcaDelete.Enabled = true;
    }

    private void bDopravcaEdit_Click(object sender, EventArgs e)
    {
        if (listDopravcovia.SelectedIndex != -1)
        {
            var dopravca = GlobData.Operators[listDopravcovia.SelectedIndex];
            dopravca.Name = tbDopravca.Text;
            var i = 0;
            foreach (var test in GlobData.Operators)
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
            var op = GlobData.Operators[index];
            foreach (var train in GlobData.Trains)
                if (train.Operator == op)
                    train.Operator = Operator.None;

            GlobData.Operators.RemoveAt(index);

            if (GlobData.Operators.Count == 0)
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
        foreach (var test in GlobData.Platforms)
            if (nastupiste.EqualsKeys(test))
            {
                Utils.ShowError(Resources.FLocalSettings_Zadaný_kľúč_nástupišťa_už_existuje);
                return;
            }

        GlobData.Platforms.Add(nastupiste);

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

            for (var i = 0; i < GlobData.Platforms.Count; i++)
                if (tbNastOznacenie.Text == GlobData.Platforms[i].Key && i != listNastupistia.SelectedIndex)
                {
                    Utils.ShowError(Resources.FLocalSettings_Zadaný_kľúč_nástupišťa_už_existuje);
                    return;
                }

            var platform = GlobData.Platforms[listNastupistia.SelectedIndex];
            platform.Key = tbNastOznacenie.Text;
            platform.FullName = tbNastFullName.Text;
            platform.SoundName = tbNastSound.Text;

            GlobData.Platforms.ResetBindings();
        }
    }

    private void bNastDelete_Click(object sender, EventArgs e)
    {
        if (listNastupistia.SelectedIndex != -1)
        {
            var delete = true;
            var where = " ";
            var index = listNastupistia.SelectedIndex;
            var nast = GlobData.Platforms[index];

            foreach (var tr in GlobData.Tracks)
                if (tr.Platform == nast)
                {
                    delete = false;
                    where += $"Koľaj {tr.Name}";
                    break;
                }

            if (delete)
            {
                GlobData.Platforms.RemoveAt(index);

                if (GlobData.Platforms.Count == 0)
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
                for (var i = 0; i < GlobData.TableLogicals.Count; i++)
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

        foreach (var test in GlobData.Tracks)
            if (track.EqualsKeys(test))
            {
                Utils.ShowError(Resources.FLocalSettings_Zadaný_kľúč_koľaje_už_existuje);
                return;
            }

        foreach (var item in clbKolajTables.CheckedItems) 
            track.Tables.Add(item as TableLogical);

        GlobData.Tracks.Add(track);

        bKolajEdit.Enabled = true;
        bKolajDelete.Enabled = true;
    }

    private void bKolajEdit_Click(object sender, EventArgs e)
    {
        if (listKolaje.SelectedIndex != -1)
        {
            if (string.IsNullOrEmpty(tbKolajOznacenie.Text) || string.IsNullOrEmpty(tbKolajFullName.Text) || string.IsNullOrEmpty(tbKolajSound.Text))
            {
                Utils.ShowError(Resources.FLocalSettings_Všetky_parametre_sú_povinné);
                return;
            }

            for (var i = 0; i < GlobData.Tracks.Count; i++)
                if (tbKolajOznacenie.Text == GlobData.Tracks[i].Key && i != listKolaje.SelectedIndex)
                {
                    Utils.ShowError(Resources.FLocalSettings_Zadaný_kľúč_koľaje_už_existuje);
                    return;
                }

            var track = GlobData.Tracks[listKolaje.SelectedIndex];
            track.Key = tbKolajOznacenie.Text;
            track.FullName = tbKolajFullName.Text;
            track.SoundName = tbKolajSound.Text;
            track.Platform = (Platform)cbNastupistia.SelectedItem;

            track.Tables.Clear();
            foreach (var item in clbKolajTables.CheckedItems) 
                track.Tables.Add(item as TableLogical);

            GlobData.Tracks.ResetBindings();
        }
    }

    private void bKolajDelete_Click(object sender, EventArgs e)
    {
        if (listKolaje.SelectedIndex != -1)
        {
            var index = listKolaje.SelectedIndex;
            var kolaj = GlobData.Tracks[index];
            foreach (var train in GlobData.Trains)
                if (train.Track == kolaj)
                    train.Track = Track.None;

            GlobData.Tracks.RemoveAt(index);

            if (GlobData.Tracks.Count == 0)
            {
                bKolajEdit.Enabled = false;
                bKolajDelete.Enabled = false;
            }
        }
    }

    private void bFyzTabAdd_Click(object sender, EventArgs e)
    {
        var eptf = new FTablePhysical(new TablePhysical(), GlobData.TableCatalogs);
        var result = eptf.ShowDialog();
        if (result == DialogResult.OK)
        {
            GlobData.TablePhysicals.Add(eptf.ThisTable);

            bFyzTabEdit.Enabled = true;
            bFyzTabCopy.Enabled = true;
            bFyzTabDelete.Enabled = true;
        }
    }

    private void bFyzTabCopy_Click(object sender, EventArgs e)
    {
        if (listFyzTabule.SelectedIndex != -1)
        {
            var eptf = new FTablePhysical(GlobData.TablePhysicals[listFyzTabule.SelectedIndex], GlobData.TableCatalogs, true);
            var result = eptf.ShowDialog();
            if (result == DialogResult.OK) GlobData.TablePhysicals.Add(eptf.ThisTable);
        }
    }

    private void bFyzTabEdit_Click(object sender, EventArgs e)
    {
        if (listFyzTabule.SelectedIndex != -1)
        {
            var eptf = new FTablePhysical(GlobData.TablePhysicals[listFyzTabule.SelectedIndex], GlobData.TableCatalogs);
            var result = eptf.ShowDialog();
            if (result == DialogResult.OK) GlobData.TablePhysicals.ResetBindings();
        }
    }

    private void bFyzTabDelete_Click(object sender, EventArgs e)
    {
        if (listFyzTabule.SelectedIndex != -1)
        {
            var delete = true;
            var where = " ";
            var index = listFyzTabule.SelectedIndex;
            var tp = GlobData.TablePhysicals[index];

            foreach (var tl in GlobData.TableLogicals)
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
                GlobData.TablePhysicals.RemoveAt(index);

                if (GlobData.TablePhysicals.Count == 0)
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
        var eltf = new FTableLogical(new TableLogical(), GlobData.TablePhysicals);
        var result = eltf.ShowDialog();
        if (result == DialogResult.OK)
        {
            GlobData.TableLogicals.Add(eltf.ThisTable);

            bLogTabEdit.Enabled = true;
            bLogTabCopy.Enabled = true;
            bLogTabDelete.Enabled = true;
        }
    }

    private void bLogTabCopy_Click(object sender, EventArgs e)
    {
        if (listLogTabule.SelectedIndex != -1)
        {
            var eltf = new FTableLogical(GlobData.TableLogicals[listLogTabule.SelectedIndex], GlobData.TablePhysicals, true);
            var result = eltf.ShowDialog();
            if (result == DialogResult.OK) GlobData.TableLogicals.Add(eltf.ThisTable);
        }
    }

    private void bLogTabEdit_Click(object sender, EventArgs e)
    {
        if (listLogTabule.SelectedIndex != -1)
        {
            var eltf = new FTableLogical(GlobData.TableLogicals[listLogTabule.SelectedIndex], GlobData.TablePhysicals);
            var result = eltf.ShowDialog();
            if (result == DialogResult.OK) GlobData.TableLogicals.ResetBindings();
        }
    }

    private void bLogTabDelete_Click(object sender, EventArgs e)
    {
        if (listLogTabule.SelectedIndex != -1)
        {
            var delete = true;
            var where = " ";
            var index = listLogTabule.SelectedIndex;
            var tlog = GlobData.TableLogicals[index];

            foreach (var tr in GlobData.Tracks)
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
                GlobData.TableLogicals.RemoveAt(index);

                if (GlobData.TableLogicals.Count == 0)
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
        var ectf = new FTableCatalog(new TableCatalog(), GlobData.TabTabs.ToList());
        var result = ectf.ShowDialog();
        if (result == DialogResult.OK)
        {
            GlobData.TableCatalogs.Add(ectf.ThisTable);

            bKatTabEdit.Enabled = true;
            bKatTabCopy.Enabled = true;
            bKatTabDelete.Enabled = true;
        }
    }

    private void bKatTabCopy_Click(object sender, EventArgs e)
    {
        if (listKatTabule.SelectedIndex != -1)
        {
            var ectf = new FTableCatalog(GlobData.TableCatalogs[listKatTabule.SelectedIndex], GlobData.TabTabs, true);
            var result = ectf.ShowDialog();
            if (result == DialogResult.OK) GlobData.TableCatalogs.Add(ectf.ThisTable);
        }
    }

    private void bKatTabEdit_Click(object sender, EventArgs e)
    {
        if (listKatTabule.SelectedIndex != -1)
        {
            var ectf = new FTableCatalog(GlobData.TableCatalogs[listKatTabule.SelectedIndex], GlobData.TabTabs);
            var result = ectf.ShowDialog();
            if (result == DialogResult.OK) GlobData.TableCatalogs.ResetBindings();
        }
    }

    private void bKatTabDelete_Click(object sender, EventArgs e)
    {
        if (listKatTabule.SelectedIndex != -1)
        {
            var delete = true;
            var where = " ";
            var index = listKatTabule.SelectedIndex;
            var tcat = GlobData.TableCatalogs[index];

            foreach (var ph in GlobData.TablePhysicals)
                if (ph.TableCatalog == tcat)
                {
                    delete = false;
                    where += $"Fyzická tabuľa {ph.Name}";
                    break;
                }

            if (delete)
                foreach (var tt in GlobData.TableTexts)
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
                GlobData.TableCatalogs.RemoveAt(listKatTabule.SelectedIndex);

                if (GlobData.TableCatalogs.Count == 0)
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
            GlobData.TabTabs.Clear();
            foreach (var doc in ettf.documents) GlobData.TabTabs.Add(doc.TabTab);
            GlobData.TabTabs.ResetBindings();
        }

        if (GlobData.TabTabs.Count == 0) bTabTabDelete.Enabled = false;
    }

    private void bTabTabDelete_Click(object sender, EventArgs e)
    {
        if (listTabTabs.SelectedIndex != -1)
        {
            var delete = true;
            var where = " ";
            var index = listLogTabule.SelectedIndex;
            var tab = GlobData.TabTabs[index];

            foreach (var tc in GlobData.TableCatalogs)
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
                GlobData.TabTabs.RemoveAt(listTabTabs.SelectedIndex);

                if (GlobData.TabTabs.Count == 0)
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
        var ettf = new FTableText(new TableText(), GlobData.TableCatalogs, ThisDir.GVD, -1);
        var result = ettf.ShowDialog();
        if (result == DialogResult.OK)
        {
            GlobData.TableTexts.Add(ettf.ThisTableText);

            bTextEdit.Enabled = true;
            bTextDelete.Enabled = true;
        }
    }

    private void bTextEdit_Click(object sender, EventArgs e)
    {
        if (listTexty.SelectedIndex != -1)
        {
            var index = listTexty.SelectedIndex;
            var ettf = new FTableText(GlobData.TableTexts[index], GlobData.TableCatalogs, ThisDir.GVD, index);
            var result = ettf.ShowDialog();
            if (result == DialogResult.OK)
            {
                GlobData.TableTexts.RemoveAt(index);
                GlobData.TableTexts.Insert(index, ettf.ThisTableText);
                listTexty.SelectedIndex = index;
            }
        }
    }

    private void bTextDelete_Click(object sender, EventArgs e)
    {
        if (listTexty.SelectedIndex != -1) GlobData.TableTexts.RemoveAt(listTexty.SelectedIndex);

        if (GlobData.TableTexts.Count == 0)
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

        GlobData.TableFonts.Add(tfont);

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

        var tfont = GlobData.TableFonts[index];
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

        GlobData.TableFonts.ResetBindings();
    }

    private void bFontDelete_Click(object sender, EventArgs e)
    {
        if (listFonts.SelectedIndex != -1) GlobData.TableFonts.RemoveAt(listFonts.SelectedIndex);

        if (GlobData.TableFonts.Count == 0)
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
                nudFontID.BorderColor = _defaultBorderColor;
                nudFontID.ArrowsColor = Color.White;
                nudFontID.BackColor = Color.Red;
                nudFontID.ForeColor = Color.White;
                break;
            case 2:
                nudFontID.BorderColor = _defaultBorderColor;
                nudFontID.ArrowsColor = Color.White;
                nudFontID.BackColor = Color.Green;
                nudFontID.ForeColor = Color.White;
                break;
            case 3:
                nudFontID.BorderColor = _defaultBorderColor;
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
            var st = GlobData.CustomStations[listCustomStations.SelectedIndex];
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

        foreach (var station in GlobData.CustomStations)
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
        GlobData.CustomStations.Add(st);

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

            foreach (var station in GlobData.CustomStations)
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

            var st = GlobData.CustomStations[listCustomStations.SelectedIndex];
            st.ID = id.ToString();
            st.Name = name;
            st.IsCustom = true;
            GlobData.CustomStations.ResetBindings();
        }
    }

    private void bCStationDelete_Click(object sender, EventArgs e)
    {
        if (listCustomStations.SelectedIndex != -1) GlobData.CustomStations.RemoveAt(listCustomStations.SelectedIndex);

        if (GlobData.CustomStations.Count == 0)
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
        if (_openTabTabEditor) bOpenEditorTab.PerformClick();
        EnableEvents(true);
    }

    private void listKatTabule_SelectedIndexChanged(object sender, EventArgs e)
    {
        tbCommentKat.Text = listKatTabule.SelectedIndex != -1 ? GlobData.TableCatalogs[listKatTabule.SelectedIndex].Comment : "";
    }

    private void listTexty_SelectedIndexChanged(object sender, EventArgs e)
    {
        tbCommentTText.Text = listTexty.SelectedIndex != -1 ? GlobData.TableTexts[listTexty.SelectedIndex].Comment : "";
    }

    private void listFyzTabule_SelectedIndexChanged(object sender, EventArgs e)
    {
        tbCommentFyz.Text = listFyzTabule.SelectedIndex != -1 ? GlobData.TablePhysicals[listFyzTabule.SelectedIndex].Comment : "";
    }

    private void listLogTabule_SelectedIndexChanged(object sender, EventArgs e)
    {
        tbCommentLog.Text = listLogTabule.SelectedIndex != -1 ? GlobData.TableLogicals[listLogTabule.SelectedIndex].Comment : "";
    }

    private void FLocalSettings_FormClosed(object sender, FormClosedEventArgs e)
    {
       EnableEvents(false);
    }

    private void EnableEvents(bool enable)
    {
        GlobData.CustomStations.FireEventOnSort = enable;
        GlobData.Platforms.FireEventOnSort = enable;
        GlobData.TablePhysicals.FireEventOnSort = enable;
        GlobData.TableCatalogs.FireEventOnSort = enable;
        GlobData.TableLogicals.FireEventOnSort = enable;
        GlobData.TabTabs.FireEventOnSort = enable;
        GlobData.TableTexts.FireEventOnSort = enable;
        GlobData.TableFonts.FireEventOnSort = enable;
    }
}