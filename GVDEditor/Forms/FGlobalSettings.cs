using System.Text.RegularExpressions;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ToolsCore.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Globalne nastavenia vsetkych GVD v priecinku.
/// </summary>
public partial class FGlobalSettings : Form
{
    /// <summary>
    ///     Vsetky grafikony.
    /// </summary>
    public readonly BindingList<GVDDirectory> Grafikony;


    private readonly List<FyzLanguage> RBLangs;

    /// <summary>
    ///     Odstranene grafikony.
    /// </summary>
    public readonly List<GVDDirectory> RemovedGVDs = new();


    // null = grafikon nema vlastnu farbu, INISS pouzije farbu zo svojej palety
    private Color? _selectedColor;
    private readonly List<TrainType> _predefinedTrainTypes;

    // porty a farby grafikonov pred upravou - Upravit ich meni priamo, zatvorenie bez OK ich musi vratit
    private readonly List<(DirList dir, int? tablePort, int? reportPort, Color? color)> _dirSnapshot;

    // jazyky, meskania, typy vlakov a audio linky pred upravou - zalozky ich menia priamo v GlobData
    private readonly GlobalSettingsSnapshot _globalSnapshot;

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FGlobalSettings"/>.
    /// </summary>
    /// <param name="gvds">Vsetky grafikony v priecinku.</param>
    /// <param name="openIndex">Index TabPage, ktory sa ma otvorit po otvoreni dialogu.</param>
    public FGlobalSettings(IList<GVDDirectory> gvds, int openIndex = -1)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();

        Grafikony = new BindingList<GVDDirectory>(gvds);
        _dirSnapshot = gvds.Select(g => (g.Dir, g.Dir.TablePort, g.Dir.ReportPort, g.Dir.BackColor)).ToList();
        _globalSnapshot = GlobalSettingsSnapshot.Capture();

        listLanguages.DataSource = GlobData.Languages;
        listGrafikony.DataSource = Grafikony;
        listMeskania.DataSource = GlobData.Delays;
        listGrafikony.DisplayMember = "PeriodFormatted";

        _predefinedTrainTypes = TrainType.GetDefaultValues();
        cbDefTrainTypSkratka.DataSource = _predefinedTrainTypes;
        listTrainTypes.DataSource = GlobData.TrainsTypes;

        var st = new List<Station>(GlobData.Stations);
        st.Sort();
        cbAudioStanica.DataSource = st;
        listAudio.DataSource = GlobData.Audios;

        if (GlobData.Delays.Count == 0)
        {
            bMeskanieEdit.Enabled = false;
            bMeskanieDelete.Enabled = false;
        }

        if (GlobData.TrainsTypes.Count == 0)
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

        RBLangs = RawBankParser.ReadFyzBankFile(GlobData.RawBankDir, out _);
        dgvLanguagesRawBank.DataSource = RBLangs;

        if (openIndex != -1) tabControl.SelectTab(openIndex);
    }

    private void bSave_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;

    /// <inheritdoc />
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        if (DialogResult != DialogResult.OK)
        {
            foreach (var (dir, tablePort, reportPort, color) in _dirSnapshot)
            {
                dir.TablePort = tablePort;
                dir.ReportPort = reportPort;
                dir.BackColor = color;
            }

            _globalSnapshot.Restore();
        }

        base.OnFormClosed(e);
    }

    private void listLanguages_SelectedIndexChanged(object sender, EventArgs e)
    {
        var jazyk = (FyzLanguage)listLanguages.SelectedItem!;
        tbLanguageName.Text = jazyk.Name;
        tbLanguageSkratka.Text = jazyk.Key;
        cbIsBasic.Checked = jazyk.IsBasic;
    }

    private void bLanguageAdd_Click(object sender, EventArgs e)
    {
        var language = new FyzLanguage(tbLanguageSkratka.Text.Trim(), tbLanguageName.Text.Trim()) { IsBasic = cbIsBasic.Checked };

        if (!CheckLanguages(AfterChange(GlobData.Languages.Count, language)))
            return;

        if (language.IsBasic) ClearOtherBasic(-1);
        GlobData.Languages.Add(language);
    }

    private void bLanguageEdit_Click(object sender, EventArgs e)
    {
        var pos = listLanguages.SelectedIndex;
        if (pos == -1) return;

        // kontrola nad kopiou - jazyk sa zmeni az ked je vysledok v poriadku
        var edited = new FyzLanguage(tbLanguageSkratka.Text.Trim(), tbLanguageName.Text.Trim()) { IsBasic = cbIsBasic.Checked };
        if (!CheckLanguages(AfterChange(pos, edited)))
            return;

        if (edited.IsBasic) ClearOtherBasic(pos);
        var language = GlobData.Languages[pos];
        language.Name = edited.Name;
        language.Key = edited.Key;
        language.IsBasic = edited.IsBasic;

        GlobData.Languages.ResetBindings();
    }

    private void bLanguageRemove_Click(object sender, EventArgs e)
    {
        var pos = listLanguages.SelectedIndex;
        if (pos == -1) return;

        if (!CheckLanguages(GlobData.Languages.Where((_, i) => i != pos).ToList()))
            return;

        GlobData.Languages.RemoveAt(pos);
    }

    /// <summary>
    ///     Jazyky tak, ako budu po pridani (<paramref name="pos" /> = pocet) alebo uprave jazyka na pozicii
    ///     <paramref name="pos" />. Zaskrtnuty hlavny jazyk presuva oznacenie - ostatne jazyky prestanu byt hlavne.
    /// </summary>
    private static List<FyzLanguage> AfterChange(int pos, FyzLanguage changed)
    {
        var after = GlobData.Languages
            .Select(l => changed.IsBasic ? new FyzLanguage(l.Key, l.Name) { IsBasic = false } : l)
            .ToList();

        if (pos < after.Count) after[pos] = changed;
        else after.Add(changed);
        return after;
    }

    private static void ClearOtherBasic(int pos)
    {
        for (var i = 0; i < GlobData.Languages.Count; i++)
            if (i != pos)
                GlobData.Languages[i].IsBasic = false;
    }

    /// <summary>
    ///     Skontroluje jazyky po zmene podla pravidiel INISSu; pri chybe ju ohlasi.
    /// </summary>
    private bool CheckLanguages(IReadOnlyList<FyzLanguage> languages)
    {
        var error = LanguageRules.Check(languages, RBLangs.Select(l => l.Key));
        if (error == null)
            return true;

        Utils.ShowError(error);
        return false;
    }

    private void listGrafikony_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listGrafikony.SelectedIndex != -1)
        {
            var dir = (GVDDirectory)listGrafikony.SelectedItem!;
            tbGrafikonStanica.Text = dir.GVD.ThisStation.Name;
            tbGrafikonObdobie.Text = dir.GVD.StartValidTimeTable.ToString("dd.MM.yyyy") + @" - " +
                                     dir.GVD.EndValidTimeTable.ToString("dd.MM.yyyy");
            tbGrafikonPath.Text = dir.Dir.FullPath;

            nudTabPort.Value = dir.Dir.TablePort ?? 0;

            nudHlaseniePort.Value = dir.Dir.ReportPort ?? 0;

            _selectedColor = dir.Dir.BackColor;
            pbColor.BackColor = _selectedColor ?? Color.Transparent;
        }
    }

    private void bEditColor_Click(object sender, EventArgs e)
    {
        var result = colorDialogFarba.ShowDialog();
        if (result == DialogResult.OK)
        {
            _selectedColor = colorDialogFarba.Color;
            pbColor.BackColor = colorDialogFarba.Color;
        }
    }

    private void bGrafikonEdit_Click(object sender, EventArgs e)
    {
        if (listGrafikony.SelectedIndex != -1)
        {
            var dir = (GVDDirectory)listGrafikony.SelectedItem!;
            var portTab = decimal.ToInt32(nudTabPort.Value);
            var portHlas = decimal.ToInt32(nudHlaseniePort.Value);
            dir.Dir.TablePort = portTab == 0 ? null : portTab;
            dir.Dir.ReportPort = portHlas == 0 ? null : portHlas;
            dir.Dir.BackColor = _selectedColor;
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
                RemovedGVDs.Add((GVDDirectory)listGrafikony.SelectedItem!);
                Grafikony.RemoveAt(listGrafikony.SelectedIndex);
            }
        }
    }

    private void bMeskanieAdd_Click(object sender, EventArgs e) => SetDelay(-1);

    private void bMeskanieEdit_Click(object sender, EventArgs e)
    {
        var index = listMeskania.SelectedIndex;
        if (index != -1) SetDelay(index);
    }

    /// <summary>
    ///     Prida cas meskania (<paramref name="index" /> = -1) alebo nahradi cas na pozicii <paramref name="index" />.
    ///     Cas sa zaradi podla velkosti - INISS ponuka casy v poradi zo suboru.
    /// </summary>
    private void SetDelay(int index)
    {
        var value = tbMeskanie.Text.Trim();
        if (value.Length == 0) return;

        if (GlobData.Delays.Where((t, i) => t == value && i != index).Any())
        {
            Utils.ShowError(Resources.FGlobalSettings_Táto_hodnota_sa_už_v_zozname_nachádza);
            return;
        }

        // INISS necislenu hodnotu preskoci - ponechat ju je volba pouzivatela (napr. "VICE480" zo starsich dat)
        if (!DelayRules.IsAcceptedByIniss(value) &&
            Utils.ShowQuestion(string.Format(Resources.FGlobalSettings_Cas_meskania_necislo, value)) != DialogResult.Yes)
            return;

        if (index != -1) GlobData.Delays.RemoveAt(index);
        var position = DelayRules.InsertIndex(GlobData.Delays, value);
        GlobData.Delays.Insert(position, value);
        listMeskania.SelectedIndex = position;

        bMeskanieEdit.Enabled = true;
        bMeskanieDelete.Enabled = true;
    }

    private void bMeskanieDelete_Click(object sender, EventArgs e)
    {
        if (listMeskania.SelectedIndex != -1) GlobData.Delays.RemoveAt(listMeskania.SelectedIndex);

        if (GlobData.Delays.Count == 0)
        {
            bMeskanieEdit.Enabled = false;
            bMeskanieDelete.Enabled = false;
        }
    }

    private void listMeskania_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listMeskania.SelectedItem != null) 
            tbMeskanie.Text = listMeskania.SelectedItem.ToString();
    }

    private void listTrainTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listTrainTypes.SelectedIndex == -1)
            return;

        var typ = (TrainType)listTrainTypes.SelectedItem!;

        if (!typ.IsCustom)
        {
            cbDefTrainTypSkratka.SelectedItem = _predefinedTrainTypes.FirstOrDefault(t => t.CategoryTrain == typ.CategoryTrain);

            tbDefaultTrainTypSkratka.Text = typ.Key;
            tbDefaultTrainTypText.Text = typ.TextInTable;

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

    private void bDefTrainTypAdd_Click(object sender, EventArgs e)
    {
        var typ = (TrainType)cbDefTrainTypSkratka.SelectedItem!;

        if (GlobData.TrainsTypes.Any(trainType => tbDefaultTrainTypSkratka.Text == trainType.Key))
        {
            Utils.ShowError(Resources.FGlobalSettings_Vybraný_typ_vlaku_sa_už_v_zozname_nachádza);
            return;
        }

        typ.CategoryTrain = ((TrainType)cbDefTrainTypSkratka.SelectedItem!).CategoryTrain;
        typ.Key = tbDefaultTrainTypSkratka.Text;
        typ.TextInTable = tbDefaultTrainTypText.Text;

        GlobData.TrainsTypes.Add(typ);

        bDefTrainTypEdit.Enabled = true;
        bDefTrainTypDelete.Enabled = true;
    }

    private void bDefTrainTypEdit_Click(object sender, EventArgs e)
    {
        if (listTrainTypes.SelectedIndex == -1)
            return;

        var typ = GlobData.TrainsTypes[listTrainTypes.SelectedIndex];
        if (GlobData.TrainsTypes.Where((t, i) => tbDefaultTrainTypSkratka.Text == t.Key && listTrainTypes.SelectedIndex != i).Any())
        {
            Utils.ShowError(Resources.FGlobalSettings_Vybraný_typ_vlaku_sa_už_v_zozname_nachádza);
            return;
        }

        typ.CategoryTrain = ((TrainType)cbDefTrainTypSkratka.SelectedItem!).CategoryTrain;
        typ.Key = tbDefaultTrainTypSkratka.Text;
        typ.TextInTable = tbDefaultTrainTypText.Text;
        GlobData.TrainsTypes.ResetBindings();
    }

    private void bDefTrainTypDelete_Click(object sender, EventArgs e)
    {
        if (listTrainTypes.SelectedIndex != -1)
            GlobData.TrainsTypes.RemoveAt(listTrainTypes.SelectedIndex);

        if (GlobData.TrainsTypes.Count == 0)
        {
            bDefTrainTypEdit.Enabled = false;
            bDefTrainTypDelete.Enabled = false;
            bCustomTrainTypEdit.Enabled = false;
            bCustomTrainTypDelete.Enabled = false;
        }
    }

    private void bCustomTrainTypAdd_Click(object sender, EventArgs e)
    {
        foreach (var trainType in GlobData.TrainsTypes)
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
            foreach (var trainType in GlobData.TrainsTypes)
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
            foreach (var trainType in GlobData.TrainsTypes)
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
            foreach (var trainType in GlobData.TrainsTypes)
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
            foreach (var trainType in GlobData.TrainsTypes)
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
        GlobData.TrainsTypes.Add(typ);

        bCustomTrainTypEdit.Enabled = false;
        bCustomTrainTypDelete.Enabled = false;
    }

    private void bCustomTrainTypEdit_Click(object sender, EventArgs e)
    {
        if (listTrainTypes.SelectedIndex != -1)
        {
            foreach (var trainType in GlobData.TrainsTypes)
                if (tbCustomTrainTypSkratka.Text == trainType.Key && ((TrainType)listTrainTypes.SelectedItem!).Key != tbCustomTrainTypSkratka.Text)
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
                        foreach (var trainType in GlobData.TrainsTypes)
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
                        foreach (var trainType in GlobData.TrainsTypes)
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
                        foreach (var trainType in GlobData.TrainsTypes)
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
                        foreach (var trainType in GlobData.TrainsTypes)
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

            GlobData.TrainsTypes[listTrainTypes.SelectedIndex] = new TrainType(category, key, table);
        }
    }

    private void bCustomTrainTypDelete_Click(object sender, EventArgs e)
    {
        if (listTrainTypes.SelectedIndex != -1) GlobData.TrainsTypes.RemoveAt(listTrainTypes.SelectedIndex);

        if (GlobData.TrainsTypes.Count == 0)
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
            var audio = GlobData.Audios[listAudio.SelectedIndex];
            cbCustomOnly.Checked = audio.Station.IsCustom;
            cbAudioStanica.SelectedItem = audio.Station;
            tbAudioName.Text = audio.Name;
            tbAudioNazovSkratka.Text = audio.ShortName;
            tbAudioNazovFronta.Text = audio.QueueName;
            tbSoundCard.Text = audio.SoundCard;
            tbInputLine.Text = audio.InputLine;
            tbAmplifier.Text = audio.AmplifierPort;
            tbExchange.Text = audio.ExchangeParameter;
            tbNode.Text = audio.Node;
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

        foreach (var a in GlobData.Audios)
            if (a.Name == tbAudioName.Text)
            {
                Utils.ShowError(Resources.FGlobalSettings_Názov_tejto_audio_linky_už_existuje);
                return;
            }

        var audio = new Audio
        {
            Station = (Station)cbAudioStanica.SelectedItem!,
            Name = tbAudioName.Text,
            ShortName = tbAudioNazovSkratka.Text,
            QueueName = tbAudioNazovFronta.Text,
            Mixer = "",
            SoundCard = tbSoundCard.Text.Trim(),
            InputLine = tbInputLine.Text.Trim(),
            AmplifierPort = tbAmplifier.Text.Trim(),
            ExchangeParameter = tbExchange.Text.Trim(),
            Node = tbNode.Text.Trim()
        };

        GlobData.Audios.Add(audio);
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
            foreach (var a in GlobData.Audios)
            {
                if (a.Name == tbAudioName.Text && i != listAudio.SelectedIndex)
                {
                    Utils.ShowError(Resources.FGlobalSettings_Názov_tejto_audio_linky_už_existuje);
                    return;
                }

                i++;
            }

            var audio = GlobData.Audios[listAudio.SelectedIndex];
            audio.Station = (Station)cbAudioStanica.SelectedItem!;
            audio.Name = tbAudioName.Text;
            audio.ShortName = tbAudioNazovSkratka.Text;
            audio.QueueName = tbAudioNazovFronta.Text;
            audio.SoundCard = tbSoundCard.Text.Trim();
            audio.InputLine = tbInputLine.Text.Trim();
            audio.AmplifierPort = tbAmplifier.Text.Trim();
            audio.ExchangeParameter = tbExchange.Text.Trim();
            audio.Node = tbNode.Text.Trim();

            GlobData.Audios.ResetBindings();
        }
    }

    private void bAudioDelete_Click(object sender, EventArgs e)
    {
        if (listAudio.SelectedIndex != -1) GlobData.Audios.RemoveAt(listAudio.SelectedIndex);
    }

    private void tbAudioName_TextChanged(object sender, EventArgs e)
    {
        tbAudioNazovSkratka.Text = tbAudioName.Text;
        tbAudioNazovFronta.Text = tbAudioName.Text;
    }

    private void FGlobalSettings_HelpButtonClicked(object sender, CancelEventArgs e)
    {
        Utils.OpenShell(LinkConsts.LINK_GLOBAL_SETTINGS);
    }

    private void EnableEvents(bool enable)
    {
        GlobData.Audios.FireEventOnSort = enable;
        GlobData.TrainsTypes.FireEventOnSort = enable;
        GlobData.Languages.FireEventOnSort = enable;
        GlobData.Delays.FireEventOnSort = enable;
    }

    private void FGlobalSettings_Load(object sender, EventArgs e)
    {
        EnableEvents(true);
    }

    private void FGlobalSettings_FormClosed(object sender, FormClosedEventArgs e)
    {
        EnableEvents(false);
    }

    private void tbMeskanie_TextChanged(object sender, EventArgs e)
    {
        bMeskanieAdd.Enabled = !string.IsNullOrEmpty(tbMeskanie.Text);
        bMeskanieEdit.Enabled = !string.IsNullOrEmpty(tbMeskanie.Text) && listMeskania.SelectedIndex != -1;
    }
}