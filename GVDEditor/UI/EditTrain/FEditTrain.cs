using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ExControls;
using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Formats;
using GVDEditor.UI.Settings;
using GVDEditor.UI.StateDgm;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.UI.EditTrain;

/// <summary>
///     Dialog - Uprava/pridanie vlaku. Stranky v strome menia koncept vlaku (<see cref="TrainDraft" />), okno ho
///     po kazdej zmene skontroluje; vlak a radenia v grafikone sa zmenia az tlacidlom OK.
/// </summary>
public partial class FEditTrain : Form
{
    private readonly bool copy;

    // koncept vlaku - okno meni len jeho kopie, vlak a radenia v grafikone sa zmenia az v bSave_Click
    private readonly TrainDraft _draft;

    // grafikon, voci ktoremu sa vlak kontroluje
    private readonly TrainContext _context;

    // stranky, ktore menia koncept a samy zobrazuju svoje chyby
    private readonly ITrainPage[] _pages;

    // clanok dokumentacie ku kazdej stranke okna
    private readonly Dictionary<ExOptionsPanel, string> _helpLinks;

    // stranka, na ktorej sa okno otvara
    private readonly ExOptionsPanel _startPanel;

    private readonly string? _gvdDir;
    private readonly int _homeStationId;

    // chyby a upozornenia konceptu po poslednej zmene
    private List<TrainRules.Problem> _problems = [];

    // smerovanie, podla ktoreho su upravene hlasenia dodatkov
    private Routing? _routing;

    private bool initialization;

    /// <summary>
    ///     Index riadku na pracovnej ploche.
    /// </summary>
    public int Row;

    /// <summary>
    ///     Vlak, s ktorym tento dialog pracuje.
    /// </summary>
    public Train? ThisTrain;

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FEditTrain"/>.
    /// </summary>
    /// <param name="train">Upravovany vlak.</param>
    /// <param name="row">Index riadku na prac. ploche.</param>
    /// <param name="gvd">Vybrane GVD.</param>
    /// <param name="copy">Ci sa jedna o kopiu vlaku.</param>
    /// <param name="gvdDir">Priecinok grafikonu (pre Kalendar akcii); <see langword="null" />, ak nie je.</param>
    /// <param name="startPage">Stranka, na ktorej sa okno otvori.</param>
    public FEditTrain(Train? train, int row, GVDInfo gvd, bool copy = false, string? gvdDir = null,
        EditTrainPage startPage = EditTrainPage.Vlak)
    {
        InitializeComponent();
        _draft = train != null ? TrainDraft.From(train) : NewDraft(gvd);
        _routing = _draft.Routing;
        _context = new TrainContext(GlobData.Trains, row, gvd.ThisStation?.ID);
        // ostatne varianty upraveneho vlaku - pri zmene cisla, nazvu alebo typu sa mozu zmenit s nim
        if (train != null && !copy)
            _draft.LoadSiblings(GlobData.Trains, row);
        _gvdDir = gvdDir;
        _homeStationId = int.TryParse(gvd.ThisStation?.ID, out var stationId) ? stationId : 0;

        ThisTrain = train;
        Row = row;
        this.copy = copy;

        initialization = true;

        if (train == null || copy)
        {
            Text = copy ? Resources.FEditTrain_FEditTrain_Duplikovať_vlak : Resources.FEditTrain_FEditTrain_Pridať_vlak;
            bSave.Text = Resources.FEditTrain_FEditTrain_Pridať;
        }

        this.ApplyThemeAndFonts();
        // koliesko posuva stranku, nie hodnotu zoznamu alebo pola pod kurzorom
        WheelScroll.Attach(this);
        // SetFormFont zapina AutoSize - okno s menitelnou velkostou by sa nedalo zmensit
        AutoSize = false;
        // nazov stranky nad nou tucne ako v nastaveniach programu
        optionsView.HeaderNodeNameFont = new Font(optionsView.HeaderNodeNameFont, FontStyle.Bold);
        // v Designeri by odkazy vytvorili handle priskoro
        pGroupHlasenia.GenerateLinksToChildren = true;
        SettingsWindow.ApplyPlacement(this, GlobData.Config.EditTrainWindow);
        // zobrazi sa len stranka, ktorou sa okno otvara - ostatne sa vytvoria az pri prvom zobrazeni
        _startPanel = startPage switch
        {
            EditTrainPage.Trasa => pTrasa,
            EditTrainPage.Platnost => pPlatnost,
            EditTrainPage.Jazyky => pJazyky,
            EditTrainPage.Dodatky => pDodatky,
            EditTrainPage.Radenie => pRadenie,
            _ => pVlak
        };
        optionsView.SelectedPanel = _startPanel;

        // stranky plnit az po teme okna (nastavuju si pisma a farby)
        trainPage.LoadData(_draft, GlobData.TrainNames);
        routePage.LoadData(_draft, gvd.ThisStation);
        validityPage.LoadData(_draft, _context, gvd.ThisStation?.Name, gvdDir != null ? OpenCalendar : null);
        // INISS jazyk, ktory grafikon nepouziva, u vlaku preskoci - ponukaju sa len jazyky grafikonu
        languagesPage.LoadData(_draft, GrafikonLanguageRules.Offered(GlobData.Languages, GlobData.LocalLanguages, _draft.Languages));
        dodatkyPage.LoadData(_draft, GlobData.Sounds.Where(sound => sound.Group.Key.EqualsIgnoreCase("DODATKY")));
        radeniePage.LoadData(_draft, gvd.StartValidTimeTable, gvd.EndValidTimeTable);
        _pages = [trainPage, routePage, validityPage, languagesPage, dodatkyPage, radeniePage];
        foreach (var page in _pages)
            page.Changed += (_, _) => Recheck();
        // radenia sa zosuladia s cislom az po dopisani - pri kazdom znaku by prevzali radenie vlaku s medzicislom
        trainPage.NumberCommitted += (_, _) =>
        {
            SyncRadeniaWithNumber();
            Recheck();
        };

        _helpLinks = new Dictionary<ExOptionsPanel, string>
        {
            [pVlak] = GvdLinkConsts.LINK_EDIT_TRAIN,
            [pTrasa] = GvdLinkConsts.LINK_EDIT_TRAIN,
            [pPlatnost] = GvdLinkConsts.LINK_EDIT_TRAIN,
            [pGroupHlasenia] = GvdLinkConsts.LINK_EDIT_TRAIN_HLASENIA,
            [pJazyky] = GvdLinkConsts.LINK_EDIT_TRAIN_HLASENIA,
            [pDodatky] = GvdLinkConsts.LINK_EDIT_TRAIN_HLASENIA,
            [pRadenie] = GvdLinkConsts.LINK_EDIT_TRAIN_RADENIE
        };
        optionsView.SelectedPanelChanged += (_, _) => UpdateHelpLink();

        initialization = false;
        Recheck();
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

    // novy vlak dostane prvy typ, dopravcu a kolaj z ponuk ako doteraz
    private static TrainDraft NewDraft(GVDInfo gvd)
    {
        var draft = TrainDraft.New(gvd.StartValidTimeTable, gvd.EndValidTimeTable);
        draft.Type = GlobData.TrainsTypes.FirstOrDefault();
        draft.Operator = GlobData.Operators.FirstOrDefault();
        draft.Track = draft.TrackDeparture = GlobData.Tracks.FirstOrDefault();
        return draft;
    }

    private void FEditTrain_Load(object sender, EventArgs e)
    {
        optionsView.TreeView.ExpandAll();
        optionsView.SelectedPanel = _startPanel;
        UpdateHelpLink();
    }

    private void FEditTrain_FormClosed(object? sender, FormClosedEventArgs e)
    {
        GlobData.Config.EditTrainWindow = SettingsWindow.CapturePlacement(this);
        SettingsWindow.SaveConfig();
    }

    private void bSave_Click(object sender, EventArgs e)
    {
        // cislo mohlo byt dopisane bez opustenia pola - radenie patri cislu, s ktorym sa vlak ulozi
        SyncRadeniaWithNumber();
        Recheck();

        // stranka s chybou sa zobrazi a okno ostane otvorene
        if (_problems.FirstOrDefault(problem => !problem.IsWarning) is { } error)
        {
            FocusProblem(error);
            DialogResult = DialogResult.None;
            return;
        }

        var isNew = copy || ThisTrain == null;
        var train = isNew ? new Train() : ThisTrain!;
        // vlak, ktory odide do inej skupiny variant, dostane v nej nove cislo (TrainVariants.Normalize po ulozeni) -
        // vlakom, ktore uz v skupine su, cisla ostanu
        if (!isNew && _draft.KeyChanged && !_draft.RenamesSiblings)
            train.Variant = -1;

        _draft.ApplyTo(train);
        _draft.ApplyToSiblings();
        _draft.ApplyVariantLimits();
        _draft.Radenia.Commit(train, GlobData.Radenia, GlobData.Trains);

        if (isNew) ThisTrain = train;

        DialogResult = DialogResult.OK;
    }

    private void bZrusit_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    /// <summary>
    ///     Skontroluje koncept a chyby ukaze na strankach, v strome a v spodnom riadku okna. Po zmene smerovania
    ///     najprv z dodatkov odstrani hlasenia, ktore vlak uz nema.
    /// </summary>
    private void Recheck()
    {
        if (initialization)
            return;

        if (_draft.Routing != _routing)
        {
            _routing = _draft.Routing;
            TrainRules.PruneReports(_draft.Doplnky, TrainRules.ReportTypesFor(_routing, GlobData.ReportTypes));
        }

        _problems = TrainRules.Check(_draft, _context);
        foreach (var page in _pages)
            page.ShowProblems(_problems);

        var errorColor = SettingsWindow.ProblemColor(optionsView.TreeView);
        foreach (var panel in optionsView.Panels.Cast<ExOptionsPanel>())
        {
            var error = _problems.FirstOrDefault(problem => !problem.IsWarning && PanelOf(problem.Field) == panel);
            panel.Node.ForeColor = error is null ? Color.Empty : errorColor;
            panel.Node.ToolTipText = error?.Message ?? "";
        }

        var first = _problems.FirstOrDefault(problem => !problem.IsWarning);
        lProblem.Text = first is null ? "" : $"{PanelOf(first.Field).NodeText}: {first.Message}";
        lProblem.ForeColor = SettingsWindow.ProblemColor(lProblem);
    }

    /// <summary>
    ///     Stranka, na ktorej je pole vlaku.
    /// </summary>
    private ExOptionsPanel PanelOf(TrainRules.Field field) => field switch
    {
        TrainRules.Field.Arrival or TrainRules.Field.Departure or TrainRules.Field.Track or TrainRules.Field.LineArrival
            or TrainRules.Field.LineDeparture or TrainRules.Field.Route => pTrasa,
        TrainRules.Field.Validity or TrainRules.Field.DateLimit => pPlatnost,
        TrainRules.Field.Dodatok => pDodatky,
        TrainRules.Field.Radenie => pRadenie,
        _ => pVlak
    };

    /// <summary>
    ///     Prepne na stranku s polom, ku ktoremu sa chyba viaze, a da mu fokus.
    /// </summary>
    private void FocusProblem(TrainRules.Problem problem)
    {
        optionsView.SelectedPanel = PanelOf(problem.Field);
        _pages.FirstOrDefault(page => page.Handles(problem.Field))?.FocusField(problem);
    }

    private void UpdateHelpLink()
    {
        var panel = optionsView.SelectedPanel;
        llHelp.Text = string.Format(CultureInfo.CurrentCulture, Resources.SettingsForm_Napoveda, panel?.NodeText);
        llHelp.Enabled = panel is not null && _helpLinks.ContainsKey(panel);
    }

    private void llHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenHelp();

    private void FEditTrain_HelpRequested(object sender, HelpEventArgs hlpevent)
    {
        hlpevent.Handled = true;
        OpenHelp();
    }

    private void OpenHelp()
    {
        if (optionsView.SelectedPanel is { } panel && _helpLinks.TryGetValue(panel, out var link))
            Utils.OpenShell(link);
    }

    /// <summary>
    ///     Radenie patri cislu vlaku: ak ma rovnake cislo iny vlak s radenim, okno prevezme jeho radenie a na strankach
    ///     Vlak a Radenie to oznami pruhom; ak uz nie, vrati radenie vlaku, s ktorym sa okno otvorilo.
    /// </summary>
    private void SyncRadeniaWithNumber()
    {
        var cislo = _draft.Number;
        if (initialization || string.IsNullOrEmpty(cislo))
            return;

        // pri kopii je Row novy riadok, takze sem patri aj zdrojovy vlak - Shows ho vsak vynecha
        var other = GlobData.Trains.Where((train, i) => train.Number == cislo && train.Radenia.Count != 0 && Row != i).FirstOrDefault();
        if (other == null)
        {
            _draft.Radenia.RestoreOwn();
            ShowRadeniaNotice(null);
            return;
        }

        // okno uz zobrazuje radenia tohto cisla (varianta, zdroj kopie) - netreba ich preberat a zahodit upravy
        if (_draft.Radenia.Shows(other.Radenia))
            return;

        _draft.Radenia.Load(other.Radenia);
        ShowRadeniaNotice(string.Format(CultureInfo.CurrentCulture, Resources.TrainBasicsPage_Radenie_Prevzate, cislo,
            TrainRules.Label(other)));
    }

    private void ShowRadeniaNotice(string? text)
    {
        trainPage.ShowRadeniaNotice(text);
        radeniePage.ShowRadeniaNotice(text);
    }

    /// <summary>
    ///     Nahlad Kalendara akcii vlaku pre upravovany vlak podla stavoveho diagramu grafikonu (subor sa cita z disku).
    /// </summary>
    private void OpenCalendar()
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
