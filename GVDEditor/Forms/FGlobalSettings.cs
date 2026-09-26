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
    private readonly GVDDirectory? _openGrafikon;

    // porty a farby grafikonov pred upravou - Upravit ich meni priamo, zatvorenie bez OK ich musi vratit
    private readonly List<(DirList dir, int? tablePort, int? reportPort, Color? color)> _dirSnapshot;

    // jazyky, meskania, typy vlakov a audio linky pred upravou - zalozky ich menia priamo v GlobData
    private readonly GlobalSettingsSnapshot _globalSnapshot;

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FGlobalSettings"/>.
    /// </summary>
    /// <param name="gvds">Vsetky grafikony v priecinku.</param>
    /// <param name="openIndex">Index TabPage, ktory sa ma otvorit po otvoreni dialogu.</param>
    /// <param name="openGrafikon">Otvoreny grafikon - jeho vlaky sa pri kontrole pouzitia typu vlaku beru z pamate.</param>
    public FGlobalSettings(IList<GVDDirectory> gvds, int openIndex = -1, GVDDirectory? openGrafikon = null)
    {
        _openGrafikon = openGrafikon;
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
        // vyber druhu predvyplni skratku a text na tabuli - inak by ostali z predchadzajuceho typu
        cbDefTrainTypSkratka.SelectionChangeCommitted += (_, _) =>
        {
            if (cbDefTrainTypSkratka.SelectedItem is not TrainType template) return;
            tbDefaultTrainTypSkratka.Text = template.CategoryTrain;
            tbDefaultTrainTypText.Text = template.CategoryTrain;
        };
        listTrainTypes.DataSource = GlobData.TrainsTypes;

        FillAudioStations(null);
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
        if (cbDefTrainTypSkratka.SelectedItem is not TrainType template)
            return;

        // novy objekt - polozka zoznamu predvolenych typov je len vzor a nesmie sa dostat do zoznamu
        var typ = new TrainType(template.CategoryTrain)
        {
            Key = tbDefaultTrainTypSkratka.Text.Trim(),
            TextInTable = tbDefaultTrainTypText.Text.Trim()
        };
        if (!CheckTrainType(typ, null))
            return;

        GlobData.TrainsTypes.Add(typ);
        listTrainTypes.SelectedItem = typ;
    }

    private void bDefTrainTypEdit_Click(object sender, EventArgs e)
    {
        if (listTrainTypes.SelectedItem is not TrainType typ || typ.IsCustom || cbDefTrainTypSkratka.SelectedItem is not TrainType template)
            return;

        var edited = new TrainType(template.CategoryTrain)
        {
            Key = tbDefaultTrainTypSkratka.Text.Trim(),
            TextInTable = tbDefaultTrainTypText.Text.Trim()
        };
        if (!CheckTrainType(edited, typ))
            return;

        // uprava na mieste - vlaky otvoreneho grafikonu sa odkazuju na tento objekt
        typ.CategoryTrain = edited.CategoryTrain;
        typ.Key = edited.Key;
        typ.TextInTable = edited.TextInTable;
        GlobData.TrainsTypes.ResetBindings();
    }

    private void bDefTrainTypDelete_Click(object sender, EventArgs e) => DeleteTrainType();

    private void bCustomTrainTypAdd_Click(object sender, EventArgs e)
    {
        var typ = new TrainType("", tbCustomTrainTypSkratka.Text.Trim(), tbCustomTrainTypText.Text.Trim());
        var category = FreeCustomCategory(CustomPrefix, null);
        if (category == null)
            return;

        typ.CategoryTrain = category;
        if (!CheckTrainType(typ, null))
            return;

        GlobData.TrainsTypes.Add(typ);
        listTrainTypes.SelectedItem = typ;
    }

    private void bCustomTrainTypEdit_Click(object sender, EventArgs e)
    {
        if (listTrainTypes.SelectedItem is not TrainType typ || !typ.IsCustom)
            return;

        // v tej istej skupine ostava miesto (napr. R2); pri zmene skupiny sa hlada volne miesto v novej
        var category = typ.CategoryTrain.StartsWith(CustomPrefix, StringComparison.Ordinal) &&
                       Regex.IsMatch(typ.CategoryTrain, "^" + CustomPrefix + "[1-9]$")
            ? typ.CategoryTrain
            : FreeCustomCategory(CustomPrefix, typ);
        if (category == null)
            return;

        var edited = new TrainType(category, tbCustomTrainTypSkratka.Text.Trim(), tbCustomTrainTypText.Text.Trim());
        if (!CheckTrainType(edited, typ))
            return;

        // uprava na mieste - vlaky otvoreneho grafikonu sa odkazuju na tento objekt
        typ.CategoryTrain = edited.CategoryTrain;
        typ.Key = edited.Key;
        typ.TextInTable = edited.TextInTable;
        GlobData.TrainsTypes.ResetBindings();
    }

    private void bCustomTrainTypDelete_Click(object sender, EventArgs e) => DeleteTrainType();

    /// <summary>
    ///     Skupina vlastnych typov vybrata v poli Farba / druh (Os, R, X, Sl).
    /// </summary>
    private string CustomPrefix => cbCustomTrainTypDruh.SelectedIndex switch
    {
        1 => "R",
        2 => "X",
        3 => "Sl",
        _ => "Os"
    };

    /// <summary>
    ///     Volne miesto vlastneho typu v skupine <paramref name="prefix" /> (napr. R3). Obsadene miesta sa
    ///     precisluju od 1 - vlaky sa odkazuju na kluc, nie na miesto, takze sa ich to netyka.
    /// </summary>
    /// <returns><see langword="null" />, ak je skupina plna (9 miest).</returns>
    private static string? FreeCustomCategory(string prefix, TrainType? exclude)
    {
        var num = 1;
        foreach (var trainType in GlobData.TrainsTypes)
            if (trainType.IsCustom && !ReferenceEquals(trainType, exclude) && Regex.IsMatch(trainType.CategoryTrain, "^" + prefix + "[1-9]$"))
                trainType.CategoryTrain = prefix + num++;

        if (num <= 9)
            return prefix + num;

        Utils.ShowError(Resources.FGlobalSettings_Maximálny_počet_typov_vlakov_tohto_druhu_je_9);
        return null;
    }

    /// <summary>
    ///     Skontroluje pridavany alebo upraveny typ vlaku (<paramref name="original" /> = upravovany typ).
    /// </summary>
    private bool CheckTrainType(TrainType typ, TrainType? original)
    {
        if (string.IsNullOrEmpty(typ.Key))
        {
            Utils.ShowError(Resources.FGlobalSettings_Nebola_zadaná_skratka_typu_vlaku);
            return false;
        }

        var others = GlobData.TrainsTypes.Where(t => !ReferenceEquals(t, original)).ToList();
        if (others.Any(t => t.Key == typ.Key))
        {
            Utils.ShowError(Resources.FGlobalSettings_Vybraný_typ_vlaku_sa_už_v_zozname_nachádza);
            return false;
        }

        // INISS uklada typ na miesto jeho kategorie - druhy riadok tej istej kategorie by prvy prepisal
        if (others.Any(t => t.CategoryTrain == typ.CategoryTrain))
        {
            Utils.ShowError(string.Format(Resources.FGlobalSettings_Kategoria_typu_obsadena, typ.CategoryTrain));
            return false;
        }

        if (original != null && original.Key != typ.Key)
            return CheckTrainTypeUnused(original, Resources.FGlobalSettings_Typ_vlaku_premenovanie);

        return true;
    }

    private void DeleteTrainType()
    {
        if (listTrainTypes.SelectedItem is not TrainType typ)
            return;

        if (!CheckTrainTypeUnused(typ, Resources.FGlobalSettings_Typ_vlaku_odstranenie))
            return;

        GlobData.TrainsTypes.Remove(typ);
        if (GlobData.TrainsTypes.Count == 0)
        {
            bDefTrainTypEdit.Enabled = false;
            bDefTrainTypDelete.Enabled = false;
            bCustomTrainTypEdit.Enabled = false;
            bCustomTrainTypDelete.Enabled = false;
        }
    }

    /// <summary>
    ///     Typ, ktory pouzivaju vlaky niektoreho grafikonu, sa nesmie odstranit ani premenovat - grafikon by sa
    ///     potom nedal otvorit (neznamy typ vlaku v Export3A.TXT).
    /// </summary>
    private bool CheckTrainTypeUnused(TrainType typ, string action)
    {
        var used = TrainTypeUsage.Find(typ.Key, Grafikony, _openGrafikon, GlobData.Trains);
        if (used.Count == 0)
            return true;

        Utils.ShowError(string.Format(Resources.FGlobalSettings_Typ_vlaku_pouzivaju, typ.Key, string.Join(", ", used), action));
        return false;
    }

    private void listAudio_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listAudio.SelectedIndex != -1)
        {
            var audio = GlobData.Audios[listAudio.SelectedIndex];
            cbCustomOnly.Checked = GlobData.CustomStations.Any(s => s.ID == audio.Station.ID);
            FillAudioStations(audio.Station);
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

    private void cbCustomOnly_CheckedChanged(object sender, EventArgs e) => FillAudioStations(cbAudioStanica.SelectedItem as Station);

    /// <summary>
    ///     Kluc testovacieho okruhu v stlpci 1 Audio.txt.
    /// </summary>
    private const string AudioTestKey = "TEST";

    /// <summary>
    ///     Naplni zoznam stanic audio linky: testovaci okruh, potom stanice zvukovej banky alebo vlastne stanice.
    ///     Stanica <paramref name="select" /> sa vyberie podla cisla - ak v zozname nie je (napr. neznama stanica
    ///     zo suboru), prida sa, aby ju Upravit potichu nezmenilo na inu.
    /// </summary>
    private void FillAudioStations(Station? select)
    {
        var st = cbCustomOnly.Checked
            ? new List<Station>(GlobData.CustomStations)
            : new List<Station>(GlobData.Stations);
        st.Sort();
        st.Insert(0, new Station(AudioTestKey, Resources.FGlobalSettings_Testovaci_okruh));

        var index = select == null ? -1 : st.FindIndex(s => string.Equals(s.ID, select.ID, StringComparison.OrdinalIgnoreCase));
        if (select != null && index == -1)
        {
            st.Add(select);
            index = st.Count - 1;
        }

        cbAudioStanica.DataSource = st;
        if (index != -1) cbAudioStanica.SelectedIndex = index;
    }

    /// <summary>
    ///     Skontroluje, ci je okruh TEST najviac jeden (<paramref name="index" /> = upravovana linka).
    /// </summary>
    private bool CheckAudioTest(Station station, int index)
    {
        if (!string.Equals(station.ID, AudioTestKey, StringComparison.OrdinalIgnoreCase))
            return true;

        if (!GlobData.Audios.Where((a, i) => i != index && string.Equals(a.Station.ID, AudioTestKey, StringComparison.OrdinalIgnoreCase)).Any())
            return true;

        Utils.ShowError(Resources.FGlobalSettings_Testovaci_okruh_uz_je);
        return false;
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

        if (!CheckAudioTest((Station)cbAudioStanica.SelectedItem!, -1))
            return;

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

            if (!CheckAudioTest((Station)cbAudioStanica.SelectedItem!, listAudio.SelectedIndex))
                return;

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

    // posledny nazov linky - protokolovy nazov a fronta ho nasleduju, len kym sa s nim zhoduju
    private string _lastAudioName = "";

    private void tbAudioName_TextChanged(object sender, EventArgs e)
    {
        if (tbAudioNazovSkratka.Text.Length == 0 || tbAudioNazovSkratka.Text == _lastAudioName)
            tbAudioNazovSkratka.Text = tbAudioName.Text;
        if (tbAudioNazovFronta.Text.Length == 0 || tbAudioNazovFronta.Text == _lastAudioName)
            tbAudioNazovFronta.Text = tbAudioName.Text;
        _lastAudioName = tbAudioName.Text;
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