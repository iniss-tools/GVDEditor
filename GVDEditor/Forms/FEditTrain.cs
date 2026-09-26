using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ToolsCore.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Uprava/pridanie vlaku.
/// </summary>
public partial class FEditTrain : Form
{
    private readonly bool copy;

    private readonly BindingList<Dodatok> Doplnky = new();

    private readonly BindingList<Operator> Operators = new(GlobData.Operators);

    // kopie radeni - Upravit ich meni len v okne, do grafikonu sa zapisu az v bSave_Click
    private readonly RadeniaEditing RadeniaEditing = new();

    private BindingList<Radenie> Radenia => RadeniaEditing.Items;
    private readonly BindingList<Station> StaniceDo = new();
    private readonly BindingList<Station> StaniceZo = new();

    private readonly BindingList<TrainName> TrainNames = new();

    private DateTime _lastKeyPressZo, _lastKeyPressDo;
    private string _searchStringZo = "", _searchStringDo = "";
    private bool ignoreSelectedIndexChanged = true;
    private readonly ToolTip _toolTip = new();

    private bool initialization;

    private string linkaPrichod = "", linkaOdchod = "";

    /// <summary>
    ///     Index riadku na pracovnej ploche.
    /// </summary>
    public int Row;

    private List<FyzSound>? selSounds;

    // obdobie grafikonu - ukaze sa pri radeni bez obdobia platnosti (DateTimePicker DateTime.MinValue nevie zobrazit)
    private readonly DateTime _gvdStart, _gvdEnd;

    private string tbCisloOldValue = "";

    /// <summary>
    ///     Vlak, s ktorym tento dialog pracuje.
    /// </summary>
    public Train? ThisTrain;

    private List<ReportType> VybraneReporty = new(1);

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FEditTrain"/>.
    /// </summary>
    /// <param name="train">Upravovany vlak.</param>
    /// <param name="row">Index riadku na prac. ploche.</param>
    /// <param name="gvd">Vybrane GVD.</param>
    /// <param name="copy">Ci sa jedna o kopiu vlaku.</param>
    public FEditTrain(Train? train, int row, GVDInfo gvd, bool copy = false, string? gvdDir = null)
    {
        InitializeComponent();
        _gvdDir = gvdDir;
        _homeStationId = int.TryParse(gvd.ThisStation?.ID, out var stationId) ? stationId : 0;
        llCalendar.Enabled = gvdDir != null;

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

        // rovnaky zoznam v jednom BindingContext zdiela poziciu - vyber v jednom poli by prepisal druhe
        // (kolaj odchodu by sa nedala nastavit inak nez kolaj prichodu)
        cbKolajOdchod.BindingContext = new BindingContext();
        listStaniceDo.BindingContext = new BindingContext();

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
        _gvdStart = gvd.StartValidTimeTable.Date;
        _gvdEnd = gvd.EndValidTimeTable.Date;

        foreach (var jazyk in GlobData.Languages)
            if (!jazyk.IsBasic)
                clbJazyky.Items.Add(jazyk);

        var lenDoplnky = new List<FyzSound>();
        foreach (var snd in GlobData.Sounds)
            if (snd.Group.Key.EqualsIgnoreCase("DODATKY"))
                lenDoplnky.Add(snd);

        listAllDoplnky.DataSource = lenDoplnky;
        listVybrateDoplnky.DataSource = Doplnky;

        FillRadenieSet();
        FillRadenieEndStations();
        FillLockouts(ThisTrain?.LockoutNumber ?? 0);

        cbNazov.SelectedItem = null;

        if (ThisTrain != null)
        {
            if (copy)
            {
                Text = Resources.FEditTrain_FEditTrain_Duplikovať_vlak;
                bSave.Text = Resources.FEditTrain_FEditTrain_Pridať;
            }

            InitializeData(ThisTrain);
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

        this.ApplyThemeAndFonts();

        initialization = false;
        // az po zobrazeni (vytvorenie handle comboboxov tiez vyvola SelectedIndexChanged) predvyplni
        // zmena kolaje prichodu aj kolaj odchodu
        Shown += (_, _) => ignoreSelectedIndexChanged = false;
    }

    /// <summary>
    ///     Uprava textu Formu
    /// </summary>
    [AllowNull]
    public sealed override string Text
    {
        get => base.Text;
        set => base.Text = value;
    }

    protected override CreateParams CreateParams
    {
        get
        {
            var handleParam = base.CreateParams;
            handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
            return handleParam;
        }
    }

    private void InitializeData(Train train)
    {
        //initialization = true;
        tbCislo.Text = train.Number;
        //initialization = false;
        tbCisloOldValue = train.Number;

        cbTyp.SelectedItem = train.Type;

        // v grafikone je kluc zvuku, v zozname sa zobrazuje jeho nazov
        if (!string.IsNullOrEmpty(train.Name)) cbNazov.Text = TrainName.ToDisplay(TrainNames, train.Name);

        cbDopravca.SelectedItem = train.Operator;


        if (train.Arrival != null)
            mtPrichod.Text = train.Arrival?.ToString("HH:mm");

        if (train.Departure != null)
            mtOdchod.Text = train.Departure?.ToString("HH:mm");


        cbKolajPrichod.SelectedItem = train.Track;

        // kolaj pri odchode sa lisi len pri vlaku, ktory v stanici prechadza na inu kolaj (tretie pole Pozice.txt)
        cbKolajOdchod.SelectedItem = train.TrackDeparture ?? train.Track;

        tDatumoveObmedzenie.Text = train.DateLimitText;

        dtpPlatnostOd.Value = train.ZaciatokPlatnosti;
        dtpPlatnostDo.Value = train.KoniecPlatnosti;

        boxMedzistatny.Checked = train.IsMedzistatny;
        boxMimoriadny.Checked = train.IsMimoriadny;
        boxMiestenkovy.Checked = train.IsMiestenkovy;
        boxDialkovy.Checked = train.IsDialkovy;
        boxNizkopodlazny.Checked = train.IsNizkopodlazny;
        boxPrestup.Checked = train.IsPrestupovy;
        boxMotorovy.Checked = train.IsMotorovy;
        SelectLockout(train.LockoutNumber);
        boxLozkovy.Checked = train.IsIbaLozkovy;

        tbLinkaPrichod.Text = train.LineArrival;
        tbLinkaOdchod.Text = train.LineDeparture;

        nudVarianta.Value = train.Variant;

        foreach (var st in train.StaniceZoSmeru) StaniceZo.Add(st);
        foreach (var st in train.StaniceDoSmeru) StaniceDo.Add(st);

        StaniceZo.RaiseListChangedEvents = true;
        StaniceDo.RaiseListChangedEvents = true;

        initialization = false;
        StaniceCountChanged();
        initialization = true;

        foreach (var jazyk in train.Languages)
            if (!jazyk.IsBasic)
                clbJazyky.SetItemChecked(clbJazyky.Items.IndexOf(jazyk), true);

        // kopie - Upravit v zalozke Dodatky meni ChosenReports a Zrusit ich nesmie nechat vo vlaku
        foreach (var doplnok in train.Doplnky)
            Doplnky.Add(new Dodatok
            {
                Sound = doplnok.Sound,
                Name = doplnok.Name,
                ChosenReports = doplnok.ChosenReports
                    .Select(chosen => new ChosenReportType { Type = chosen.Type, Variants = [.. chosen.Variants] }).ToList()
            });

        RadeniaEditing.LoadOwn(train.Radenia);

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
        // poistka, ak by tbCislo_Validated neprebehol - prevzate radenie si musi pouzivatel pred ulozenim pozriet
        if (SyncRadeniaWithNumber() || !TryReadForm(out var values))
        {
            DialogResult = DialogResult.None;
            return;
        }

        var train = (copy ? new Train() : ThisTrain) ?? new Train();

        train.Number = values.Number;
        train.Type = values.Type;
        train.Name = values.Name;
        train.Operator = values.Operator;
        train.Arrival = values.Arrival;
        train.Departure = values.Departure;
        train.Routing = values.Routing;
        train.StartingStation = values.StartingStation;
        train.EndingStation = values.EndingStation;

        train.Track = values.Track;
        train.TrackDeparture = values.TrackDeparture;

        train.DateLimitText = tDatumoveObmedzenie.Text;

        train.IsMedzistatny = boxMedzistatny.Checked;
        train.IsMimoriadny = boxMimoriadny.Checked;
        train.IsMiestenkovy = boxMiestenkovy.Checked;
        train.IsDialkovy = boxDialkovy.Checked;
        train.IsNizkopodlazny = boxNizkopodlazny.Checked;
        train.IsPrestupovy = boxPrestup.Checked;
        train.IsMotorovy = boxMotorovy.Checked;
        train.LockoutNumber = cbVyluka.SelectedItem is LockoutItem lockout ? lockout.Code : 0;
        train.IsIbaLozkovy = boxLozkovy.Checked;

        train.StaniceZoSmeru.Clear();
        train.StaniceDoSmeru.Clear();

        for (var index = 0; index < dgvTrasaZo.Rows.Count; index++)
        {
            var selectedRow = dgvTrasaZo.Rows[index];
            var st = (Station)selectedRow.DataBoundItem!;

            train.StaniceZoSmeru.Add(st);
        }

        for (var index = 0; index < dgvTrasaDo.Rows.Count; index++)
        {
            var selectedRow = dgvTrasaDo.Rows[index];
            var st = (Station)selectedRow.DataBoundItem!;

            train.StaniceDoSmeru.Add(st);
        }

        train.Languages.Clear();

        foreach (var item in clbJazyky.CheckedItems.OfType<FyzLanguage>()) train.Languages.Add(item);

        var platnostOd = values.ValidFrom;
        var platnostDo = values.ValidTo;

        train.ZaciatokPlatnosti = platnostOd;
        train.KoniecPlatnosti = platnostDo;

        train.LineArrival = linkaPrichod;
        train.LineDeparture = linkaOdchod;

        train.Doplnky = Doplnky.ToList();

        var dateRem = new DateLimit(platnostOd.Date, platnostDo.Date, true, true, false, false);

        var varianta = values.Variant;
        var seltrains = values.OtherVariants;

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
        {
            if (seltrain.ZaciatokPlatnosti == train.ZaciatokPlatnosti &&
                seltrain.KoniecPlatnosti == train.KoniecPlatnosti &&
                dateRem.Overlap(seltrain.DateLimitText, train.DateLimitText))
            {
                var obmand = dateRem.TextAnd(seltrain.DateLimitText, train.DateLimitText);

                var result = Utils.ShowQuestion(string.Format(Resources.FEditTrain_DateRem_zasahuje_do_ineho_vlaku, train.Type,
                    train.Number, train.Name, obmand));

                if (result == DialogResult.Yes)
                {
                    var result2 = FDateLimitEdit.SetDateLimit(this, platnostOd, platnostDo, train, true, seltrain.DateLimitText);
                    if (result2 == DialogResult.OK)
                        seltrain.DateLimitText = FDateLimitEdit.Result;
                }
            }
        }

        if (GlobData.Config.AutoVariant && (copy || ThisTrain == null))
        {
            if (seltrains.Count == 0)
            {
                train.Variant = -1;
            }
            else
            {
                int i;
                for (i = 0; i < seltrains.Count; i++)
                    seltrains[i].Variant = i + 1;

                train.Variant = i + 1;
            }
        }

        RadeniaEditing.Commit(train, GlobData.Radenia, GlobData.Trains);

        if (copy || ThisTrain == null) ThisTrain = train;

        DialogResult = DialogResult.OK;
    }

    /// <summary>
    ///     Hodnoty formulara overene pred zapisom do vlaku.
    /// </summary>
    private sealed record FormValues(
        string Number,
        TrainType Type,
        string Name,
        Operator Operator,
        DateTime? Arrival,
        DateTime? Departure,
        Routing Routing,
        Station? StartingStation,
        Station? EndingStation,
        Track Track,
        Track? TrackDeparture,
        DateTime ValidFrom,
        DateTime ValidTo,
        int Variant,
        List<Train> OtherVariants);

    /// <summary>
    ///     Overi formular bez zmeny vlaku; pri chybe ju ukaze a vrati false.
    /// </summary>
    private bool TryReadForm([NotNullWhen(true)] out FormValues? values)
    {
        values = null;

        var number = tbCislo.Text;
        if (string.IsNullOrEmpty(number))
        {
            Utils.ShowError(Resources.FEditTrain_bSave_Click_Zadajte_číslo_vlaku);
            return false;
        }

        var type = (TrainType)cbTyp.SelectedItem!;
        var name = TrainName.ToStored(TrainNames, cbNazov.Text);

        DateTime? arrival = null, departure = null;
        Routing routing;
        Station? starting = null, ending = null;

        if (StaniceZo.Count != 0)
        {
            if (!Utils.IsTime(mtPrichod.Text))
            {
                Utils.ShowError(Resources.FEditTrain_bSave_Click_Nesprávny_formát_času_príchodu);
                return false;
            }

            arrival = DateTime.Parse(mtPrichod.Text);
            starting = StaniceZo.First();
        }

        if (StaniceDo.Count != 0)
        {
            if (!Utils.IsTime(mtOdchod.Text))
            {
                Utils.ShowError(Resources.FEditTrain_bSave_Click_Nesprávny_formát_času_odchodu);
                return false;
            }

            departure = DateTime.Parse(mtOdchod.Text);
            ending = StaniceDo.Last();
        }

        // odchod skor ako prichod = vlak stoji v stanici cez polnoc (odchod je nasledujuci den)
        if (arrival != null && departure != null)
            routing = Routing.Prechadzajuci;
        else if (arrival != null)
        {
            routing = Routing.Konciaci;
        }
        else if (departure != null)
        {
            routing = Routing.Vychadzajuci;
        }
        else
        {
            Utils.ShowError(Resources.FEditTrain_bSave_Click_Vlak_nemá_zadanú_žiadnu_stanicu);
            return false;
        }

        if (cbKolajPrichod.SelectedItem is not Track track)
        {
            Utils.ShowError(Resources.FEditTrain_bSave_Click_Nie_je_vybratá_koľaj);
            return false;
        }

        var trackDeparture = cbKolajOdchod.SelectedItem as Track;

        var platnostOd = dtpPlatnostOd.Value;
        var platnostDo = dtpPlatnostDo.Value;

        if (platnostOd.CompareTo(platnostDo) > 0)
        {
            Utils.ShowError(Resources.FEditTrain_bSave_Click_Začiatok_platnosti_musí_skôr_ako_koniec_platnosti);
            return false;
        }

        try
        {
            var dom = new DateLimit(platnostOd.Date, platnostDo.Date);
            dom.TextToBitArray(tDatumoveObmedzenie.Text);
        }
        catch (Exception ex)
        {
            Utils.ShowError(ex.Message);
            return false;
        }

        var varianta = decimal.ToInt32(nudVarianta.Value);
        var probe = new Train { Number = number, Name = name, Type = type };

        var id = 0;
        var seltrains = new List<Train>();
        foreach (var vlak in GlobData.Trains)
        {
            if (Train.IsSameVariant(probe, vlak) && Row != id)
            {
                if (!GlobData.Config.AutoVariant)
                {
                    if (varianta == -1)
                    {
                        Utils.ShowError(Resources.FEditTrain_bSave_Click_Varianta_tohto_vlaku_nemôže_byť_Minus_1);
                        return false;
                    }

                    if (varianta == vlak.Variant)
                    {
                        Utils.ShowError(Resources.FEditTrain_bSave_Click_Vybraná_varianta_vlaku_sa_už_používa_pri_inom_vlaku);
                        return false;
                    }
                }

                seltrains.Add(vlak);
            }

            id++;
        }

        values = new FormValues(number, type, name, (Operator)cbDopravca.SelectedItem!, arrival, departure, routing, starting, ending,
            track, trackDeparture != null && !trackDeparture.EqualsKeys(track) ? trackDeparture : null,
            platnostOd, platnostDo, varianta, seltrains);
        return true;
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
        // zmena kolaje odchodu kolaj prichodu nemeni - vlak moze v stanici prejst na inu kolaj;
        // opacny smer (prichod -> odchod) ostava ako pohodlna predvolba
    }

    /// <summary>
    ///     Polozka ponuky vyluk: kod zapisovany do Vyluka.TXT a text zobrazeny v ponuke.
    ///     <paramref name="Source"/> je polozka z LogZvuk.usr, ak ide o vyluku zalozenu obsluhou.
    /// </summary>
    private sealed record LockoutItem(int Code, string Text, LogZvukText? Source = null)
    {
        public override string ToString() => Text;
    }

    /// <summary>
    ///     Naplni ponuku vyluk: ziadna (0), zabudovana obecna vyluka (1) a vyluky zalozene obsluhou v INISSe (RAWBANK\LogZvuk.usr).
    ///     Ak vlak odkazuje na kod, ktory v banke nie je, prida sa ako neznama polozka, aby sa hodnota pri ulozeni nestratila.
    /// </summary>
    private void FillLockouts(int currentCode)
    {
        var items = new List<LockoutItem>
        {
            new(0, Resources.FEditTrain_Vyluka_None),
            new(1, $"1 – {Resources.FEditTrain_Vyluka_BuiltIn}")
        };

        foreach (var text in GlobData.LogZvukTexts)
            if (text.IsLockout && text.Code > 1 && items.All(item => item.Code != text.Code))
                items.Add(new LockoutItem(text.Code, text.ToString(), text));

        if (currentCode != 0 && items.All(item => item.Code != currentCode))
            items.Add(new LockoutItem(currentCode, $"{currentCode} – {Resources.FEditTrain_Vyluka_Unknown}"));

        cbVyluka.Items.Clear();
        foreach (var item in items)
            cbVyluka.Items.Add(item);

        cbVyluka.DropDownWidth = Math.Max(cbVyluka.Width, 320);
        SelectLockout(currentCode);
    }

    private void SelectLockout(int code)
    {
        var match = cbVyluka.Items.Cast<LockoutItem>().FirstOrDefault(item => item.Code == code);
        cbVyluka.SelectedItem = match ?? cbVyluka.Items[0];
    }

    private void cbVyluka_SelectedIndexChanged(object sender, EventArgs e)
    {
        // tooltip ukaze predlohu a stanice vybranej vyluky z LogZvuk.usr
        if (cbVyluka.SelectedItem is LockoutItem { Source: { } source })
            _toolTip.SetToolTip(cbVyluka, string.Format(Resources.FEditTrain_Vyluka_ItemHint, source.Template, string.Join(", ", source.StationNames)));
        else
            _toolTip.SetToolTip(cbVyluka, Resources.FEditTrain_Vyluka_Hint);
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
        }
    }

    // radenia sa zosuladia s cislom az po dopisani - pri kazdom znaku by prevzali radenie vlaku s medzicislom (1, 10, ...)
    private void tbCislo_Validated(object sender, EventArgs e)
    {
        SyncRadeniaWithNumber();
    }

    /// <summary>
    ///     Radenie patri cislu vlaku: ak ma rovnake cislo iny vlak s radenim, okno prevezme jeho radenie; ak uz nie,
    ///     vrati radenie vlaku, s ktorym sa okno otvorilo.
    /// </summary>
    /// <returns>true, ak okno prevzalo radenie ineho vlaku</returns>
    private bool SyncRadeniaWithNumber()
    {
        var cislo = tbCislo.Text;
        if (initialization || string.IsNullOrEmpty(cislo))
            return false;

        // pri kopii je Row novy riadok, takze sem patri aj zdrojovy vlak - Shows ho vsak vynecha
        var other = GlobData.Trains.Where((train, i) => train.Number == cislo && train.Radenia.Count != 0 && Row != i).FirstOrDefault();
        if (other == null)
        {
            RadeniaEditing.RestoreOwn();
            return false;
        }

        // okno uz zobrazuje radenia tohto cisla (varianta, zdroj kopie) - netreba ich preberat a zahodit upravy
        if (RadeniaEditing.Shows(other.Radenia))
            return false;

        Utils.ShowWarning(copy
            ? Resources.FEditTrain_Cislo_kopie_sa_zhoduje_s_inym_vlakom
            : Resources.FEditTrain_Číslo_vlaku_sa_zhoduje_s_iným_vlakom_a_preto_bude_aj_jeho_radenie_priradené_k_tomuto_vlaku);

        RadeniaEditing.Load(other.Radenia);
        return true;
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
            var st = (Station)listStaniceZo.SelectedItem!;
            StaniceZo.Add(new Station(st.ID, st.Name, IsInLongReport: true));
            dgvTrasaZo.Rows[StaniceZo.Count - 1].Cells[0].Value = true;
        }
    }

    private void bDeleteZo_Click(object sender, EventArgs e)
    {
        if (dgvTrasaZo.SelectedRows.Count > 0) StaniceZo.RemoveAt(dgvTrasaZo.CurrentCell!.RowIndex);
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
            var st = (Station)listStaniceDo.SelectedItem!;
            StaniceDo.Add(new Station(st.ID, st.Name, IsInLongReport: true));
            dgvTrasaDo.Rows[StaniceDo.Count - 1].Cells[0].Value = true;
        }
    }

    private void bDeleteDo_Click(object sender, EventArgs e)
    {
        if (dgvTrasaDo.SelectedRows.Count > 0) StaniceDo.RemoveAt(dgvTrasaDo.CurrentCell!.RowIndex);
    }

    private void listStaniceZo_DoubleClick(object sender, EventArgs e)
    {
        if (listStaniceZo.SelectedIndex != -1)
        {
            var st = (Station)listStaniceZo.SelectedItem!;
            StaniceZo.Add(new Station(st.ID, st.Name, IsInLongReport: true));
        }
    }

    private void listStaniceDo_DoubleClick(object sender, EventArgs e)
    {
        if (listStaniceDo.SelectedIndex != -1)
        {
            var st = (Station)listStaniceDo.SelectedItem!;
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
            var selSound = (FyzSound)listAllDoplnky.SelectedItem!;
            AddDodatok(selSound);
        }
    }

    /// <summary>
    ///     Prida dodatok s hlaseniami zaskrtnutymi v tabulke Kedy hlasit - inak by mal prazdnu mapu a nikdy by nezaznel.
    /// </summary>
    private void AddDodatok(FyzSound sound)
    {
        var doplnok = new Dodatok
        {
            Sound = sound,
            Name = Dodatok.CodeFromKey(sound.Key),
            ChosenReports = GetFromTable(dgvDoplnokSet, VybraneReporty)
        };
        Doplnky.Add(doplnok);
        listVybrateDoplnky.SelectedItem = doplnok;

        bDoplnkyEdit.Enabled = true;
        bDoplnkyDelete.Enabled = true;
    }

    private void bDoplnkyAdd_Click(object sender, EventArgs e)
    {
        if (listAllDoplnky.SelectedIndex != -1)
        {
            AddDodatok((FyzSound)listAllDoplnky.SelectedItem!);
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
            tbTextDoplnku.Text = ((FyzSound)listAllDoplnky.SelectedItem).Text;
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

    private void StaniceZoOnListChanged(object? sender, ListChangedEventArgs e)
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

    private void StaniceDoOnListChanged(object? sender, ListChangedEventArgs e)
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

            // RemoveAt v cykle dopredu by preskocil hlasenie hned za odstranenym
            foreach (var dodatok in Doplnky)
                dodatok.ChosenReports.RemoveAll(chosen => !VybraneReporty.Contains(chosen.Type));
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
            dtpRadenieOd.Value = radenie.HasValidity ? radenie.ZacPlatnosti : _gvdStart;
            dtpRadenieDo.Value = radenie.HasValidity ? radenie.KonPlatnosti : _gvdEnd;
            tbDateRemRadenie.Text = radenie.DatObm;
            SelectRadenieEndStation(radenie.DestStation);
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
            if (rad.ZacPlatnosti == odP && rad.KonPlatnosti == doP && SameEndStation(rad.DestStation, SelectedRadenieEndStation) &&
                and != "t.č. nejde" && and != "t.č. nejede")
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
        radenie.DestStation = SelectedRadenieEndStation!;

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

            // radenie bez obdobia platnosti ho nedostane, kym pouzivatel nezmeni zobrazene obdobie ani nezada dni
            if (!radenie.HasValidity && odP == _gvdStart && doP == _gvdEnd && string.IsNullOrWhiteSpace(tbDateRemRadenie.Text))
            {
                if (selSounds == null)
                {
                    Utils.ShowError(Resources.FEditTrain_Nebolo_zadané_radenie_vlaku);
                    return;
                }

                radenie.DatObm = "";
                radenie.Text = tbRadenie.Text;
                radenie.Sounds = selSounds;
                radenie.ChosenReports = GetFromTable(dgvRadenieSet, GlobData.ReportTypes);
                radenie.DestStation = SelectedRadenieEndStation!;
                Radenia.ResetBindings();
                return;
            }

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
                if (rad.ZacPlatnosti == odP && rad.KonPlatnosti == doP && SameEndStation(rad.DestStation, SelectedRadenieEndStation) &&
                    and != "t.č. nejde" && and != "t.č. nejede" &&
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
            radenie.DestStation = SelectedRadenieEndStation!;

            Radenia.ResetBindings();
        }
    }

    private void bRadenieDelete_Click(object sender, EventArgs e)
    {
        if (listRadenia.SelectedIndex != -1)
        {
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
            fRadenie = new FRadenie(new List<FyzSound>());
            var result = fRadenie.ShowDialog();
            if (result == DialogResult.OK)
            {
                selSounds = new List<FyzSound>(fRadenie.SelSounds);
                tbRadenie.Text = Radenie.SoundsToString(selSounds);
            }
        }
        else
        {
            fRadenie = new FRadenie(new List<FyzSound>(Radenia[listRadenia.SelectedIndex].Sounds));
            var result = fRadenie.ShowDialog();
            if (result == DialogResult.OK)
            {
                selSounds = new List<FyzSound>(fRadenie.SelSounds);
                tbRadenie.Text = Radenie.SoundsToString(selSounds);
            }
        }
    }

    /// <summary>
    ///     Polozka ponuky cielovej stanice radenia; <see cref="Station" /> null = radenie pre vlak do akejkolvek stanice.
    /// </summary>
    private sealed record EndStationItem(Station? Station, string Text)
    {
        public override string ToString() => Text;
    }

    /// <summary>
    ///     Naplni ponuku cielovej stanice radenia: ziadne obmedzenie, stanice zo zvukovej banky a vlastne stanice.
    ///     Radenie patri cislu vlaku - obmedzenie na ciel rozlisi varianty toho isteho vlaku do roznych stanic.
    /// </summary>
    private void FillRadenieEndStations()
    {
        cbRadenieEndStation.DropDownStyle = ComboBoxStyle.DropDownList;
        cbRadenieEndStation.Items.Clear();
        cbRadenieEndStation.Items.Add(new EndStationItem(null, Resources.FEditTrain_Radenie_LubovolnyCiel));
        foreach (var station in GlobData.Stations.Concat(GlobData.CustomStations).DistinctBy(s => s.ID).OrderBy(s => s.Name))
            cbRadenieEndStation.Items.Add(new EndStationItem(station, station.Name));
        cbRadenieEndStation.SelectedIndex = 0;
    }

    private void SelectRadenieEndStation(Station? station)
    {
        if (station == null)
        {
            cbRadenieEndStation.SelectedIndex = 0;
            return;
        }

        var item = cbRadenieEndStation.Items.Cast<EndStationItem>().FirstOrDefault(i => i.Station?.ID == station.ID);
        if (item == null)
        {
            // stanica zo suboru, ktora nie je v banke ani medzi vlastnymi stanicami - ponecha sa
            item = new EndStationItem(station, station.Name == station.ID ? station.ID : $"{station.Name} ({station.ID})");
            cbRadenieEndStation.Items.Add(item);
        }

        cbRadenieEndStation.SelectedItem = item;
    }

    private Station? SelectedRadenieEndStation => (cbRadenieEndStation.SelectedItem as EndStationItem)?.Station;

    private static bool SameEndStation(Station? a, Station? b) => a?.ID == b?.ID;

    private void listRadenia_Format(object sender, ListControlConvertEventArgs e)
    {
        if (e.ListItem is Radenie radenie)
        {
            var text = !radenie.HasValidity
                ? Resources.FEditTrain_Radenie_BezPlatnosti
                : radenie.ZacPlatnosti.Date.ToString("dd.MM.yyyy") + " - " + radenie.KonPlatnosti.Date.ToString("dd.MM.yyyy");

            // radenia s rovnakym obdobim sa lisia len datumovym obmedzenim
            if (!string.IsNullOrWhiteSpace(radenie.DatObm)) text += $" ({radenie.DatObm})";
            if (radenie.DestStation != null) text += " → " + radenie.DestStation.Name;
            e.Value = text;
        }
    }

    private void bPlay_Click(object sender, EventArgs e)
    {
        var soundsS = new List<string>();
        if (selSounds != null)
            foreach (var fyzZvuk in selSounds)
                soundsS.Add(GlobData.RawBankDir + "\\" + fyzZvuk.Language.RelativePath +
                            fyzZvuk.Group.RelativePath + fyzZvuk.FileName);
        else if (listRadenia.SelectedIndex != -1)
            foreach (var fyzZvuk in Radenia[listRadenia.SelectedIndex].Sounds)
                soundsS.Add(GlobData.RawBankDir + "\\" + fyzZvuk.Language.RelativePath +
                            fyzZvuk.Group.RelativePath + fyzZvuk.FileName);

        var player = new WavPlayer(soundsS.ToArray(), GlobData.Config.PlayerSoundsOffset);
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
        else
        {
            // vymazane pole = vlak bez linky
            linkaPrichod = "";
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
        else
        {
            linkaOdchod = "";
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
        cheader.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

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
        Utils.OpenShell(LinkConsts.LINK_EDIT_TRAIN);
    }

    private void BEditLimit_Click(object sender, EventArgs e)
    {
        var result = FDateLimitEdit.SetDateLimit(this, dtpPlatnostOd.Value, dtpPlatnostDo.Value, defaultValue: tDatumoveObmedzenie.Text);
        if (result is DialogResult.OK)
        {
            tDatumoveObmedzenie.Text = FDateLimitEdit.Result;
        }
    }

    private void BEditLimitRadenie_Click(object sender, EventArgs e)
    {
        var result = FDateLimitEdit.SetDateLimit(this, dtpRadenieOd.Value, dtpRadenieDo.Value, defaultValue: tbDateRemRadenie.Text);
        if (result is DialogResult.OK)
        {
            tbDateRemRadenie.Text = FDateLimitEdit.Result;
        }
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
        cheader.HeaderText = dt.Columns[cheader.HeaderText]!.Caption;
        cheader.SortMode = DataGridViewColumnSortMode.NotSortable;
        cheader.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

        for (var i = 1; i < dgvRadenieSet.Columns.Count; i++)
        {
            var column = (DataGridViewCheckBoxColumn)dgvRadenieSet.Columns[i];
            column.HeaderText = dt.Columns[column.HeaderText]!.Caption;
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

    private readonly string? _gvdDir;
    private readonly int _homeStationId;

    /// <summary>
    ///     Nahlad Kalendara akcii vlaku pre upravovany vlak podla stavoveho diagramu grafikonu (subor sa cita z disku).
    /// </summary>
    private void llCalendar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        if (_gvdDir == null) return;
        var dir = _gvdDir;
        var f = new FStateDgmCalendar(() =>
        {
            try
            {
                return TxtParser.ReadStateDgm(dir);
            }
            catch (ToolsCore.StateDgm.StateDgmParseException)
            {
                return null;
            }
        }, _homeStationId, ThisTrain);
        f.Show(this);
    }
}