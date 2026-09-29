using System.Globalization;
using ExControls;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Snapshots;
using GVDEditor.Properties;
using ToolsCore.Iniss.Tools;
using ToolsCore.Tools;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Dialog - Globalne nastavenia vsetkych GVD v priecinku.
/// </summary>
public partial class FGlobalSettings : Form
{
    /// <summary>
    /// Vsetky grafikony.
    /// </summary>
    public BindingList<GVDDirectory> Grafikony { get; }


    /// <summary>
    /// Odstranene grafikony - ich priecinky sa po OK presunu do Kosa.
    /// </summary>
    public List<GVDDirectory> RemovedGVDs { get; } = new();


    private readonly GVDDirectory? _openGrafikon;

    // porty a farby grafikonov pred upravou - stranka Grafikony ich meni priamo, zatvorenie bez OK ich musi vratit
    private readonly List<(DirList dir, int? tablePort, int? reportPort, Color? color)> _dirSnapshot;

    // jazyky, meskania, typy vlakov a audio linky pred upravou - stranky ich menia priamo v GlobData
    private readonly GlobalSettingsSnapshot _globalSnapshot;

    // stranka, ktora sa ma vybrat po otvoreni okna
    private readonly GlobalSettingsPage _startPage;

    // plnenie stranok - az pri prvom zobrazeni, zvysne postupne po otvoreni okna
    private readonly PageLoader _pages;

    // stranky, ktore samy kontroluju svoje udaje, s panelom, v ktorom su
    private readonly (ExOptionsPanel Panel, ISettingsPage Page)[] _checkedPages;

    // clanok dokumentacie ku kazdej stranke okna
    private readonly Dictionary<ExOptionsPanel, string> _helpLinks;

    /// <summary>
    /// Vytvori novy formular typu <see cref="FGlobalSettings"/>.
    /// </summary>
    /// <param name="gvds">Vsetky grafikony v priecinku.</param>
    /// <param name="page">Stranka, ktora sa ma otvorit po otvoreni dialogu.</param>
    /// <param name="openGrafikon">Otvoreny grafikon - jeho vlaky sa pri kontrole pouzitia typu vlaku beru z pamate.</param>
    public FGlobalSettings(IList<GVDDirectory> gvds, GlobalSettingsPage page = GlobalSettingsPage.Grafikony,
        GVDDirectory? openGrafikon = null)
    {
        _openGrafikon = openGrafikon;
        InitializeComponent();
        this.ApplyThemeAndFonts();
        // koliesko posuva stranku, nie hodnotu zoznamu alebo pola pod kurzorom
        WheelScroll.Attach(this);
        // SetFormFont zapina AutoSize - okno s menitelnou velkostou by sa nedalo zmensit
        AutoSize = false;
        // nazov stranky nad nou tucne ako v nastaveniach programu
        optionsView.HeaderNodeNameFont = new Font(optionsView.HeaderNodeNameFont, FontStyle.Bold);
        SettingsWindow.ApplyPlacement(this, GlobData.Config.GlobalSettingsWindow);
        _startPage = page;
        // zobrazi sa len stranka, ktorou sa okno otvara - ostatne sa vytvoria az pri prvom zobrazeni
        optionsView.SelectedPanel = PanelOf(page);

        _helpLinks = new Dictionary<ExOptionsPanel, string>
        {
            [pGrafikony] = GvdLinkConsts.LINK_GLOBAL_GRAFIKONY,
            [pJazyky] = GvdLinkConsts.LINK_GLOBAL_JAZYKY,
            [pMeskania] = GvdLinkConsts.LINK_GLOBAL_MESKANIA,
            [pTrainTypes] = GvdLinkConsts.LINK_GLOBAL_TYPY_VLAKOV,
            [pAudio] = GvdLinkConsts.LINK_GLOBAL_AUDIO
        };
        optionsView.SelectedPanelChanged += (_, _) => UpdateHelpLink();

        Grafikony = new BindingList<GVDDirectory>(gvds);
        _dirSnapshot = gvds.Select(g => (g.Dir, g.Dir.TablePort, g.Dir.ReportPort, g.Dir.BackColor)).ToList();
        _globalSnapshot = GlobalSettingsSnapshot.Capture();

        _checkedPages =
        [
            (pGrafikony, grafikonyPage), (pJazyky, languagesPage), (pMeskania, delaysPage), (pTrainTypes, trainTypesPage),
            (pAudio, audioPage)
        ];
        foreach (var (_, checkedPage) in _checkedPages)
            checkedPage.ProblemsChanged += (_, _) => UpdateProblems();

        // typy vlakov citaju vlaky vsetkych grafikonov - pri mnohych grafikonoch by otvorenie okna trvalo
        _pages = new PageLoader(this, optionsView);
        _pages.Add(pGrafikony, () => grafikonyPage.LoadData(Grafikony, RemovedGVDs));
        _pages.Add(pJazyky, () => languagesPage.LoadData(RawBankParser.ReadFyzBankFile(GlobData.RawBankDir, out _)));
        _pages.Add(pMeskania, delaysPage.LoadData);
        _pages.Add(pAudio, () => audioPage.LoadData(Grafikony));
        _pages.Add(pTrainTypes, () => trainTypesPage.LoadData(Grafikony, _openGrafikon));
        _pages.Load(PanelOf(page));
        UpdateProblems();
    }

    private ExOptionsPanel PanelOf(GlobalSettingsPage page) => page switch
    {
        GlobalSettingsPage.Jazyky => pJazyky,
        GlobalSettingsPage.Meskania => pMeskania,
        GlobalSettingsPage.TypyVlakov => pTrainTypes,
        GlobalSettingsPage.Audio => pAudio,
        _ => pGrafikony
    };

    private void UpdateHelpLink()
    {
        var panel = optionsView.SelectedPanel;
        llHelp.Text = string.Format(CultureInfo.CurrentCulture, Resources.SettingsForm_Napoveda, panel?.NodeText);
        llHelp.Enabled = panel is not null && _helpLinks.ContainsKey(panel);
    }

    private void llHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenHelp();

    private void FGlobalSettings_HelpRequested(object sender, HelpEventArgs hlpevent)
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
    /// V spodnom riadku ukaze prvu chybu stranok a stranky s chybou oznaci v strome.
    /// </summary>
    private void UpdateProblems()
    {
        string? first = null;
        foreach (var (panel, page) in _checkedPages)
        {
            var problem = page.FirstProblem;
            panel.Node.ForeColor = problem is null ? Color.Empty : SettingsWindow.ProblemColor(optionsView.TreeView);
            panel.Node.ToolTipText = problem ?? "";
            if (problem is not null && first is null)
                first = $"{panel.NodeText}: {problem}";
        }

        lProblem.Text = first ?? "";
        lProblem.ForeColor = SettingsWindow.ProblemColor(lProblem);
    }

    private void bSave_Click(object sender, EventArgs e)
    {
        _pages.LoadAll();

        // stranka s chybou sa zobrazi a okno ostane otvorene
        foreach (var (panel, page) in _checkedPages)
        {
            if (page.FirstProblem is null)
                continue;

            optionsView.SelectedPanel = panel;
            page.FocusFirstProblem();
            DialogResult = DialogResult.None;
            return;
        }

        DialogResult = DialogResult.OK;
    }

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

    private static void EnableEvents(bool enable)
    {
        GlobData.Audios.FireEventOnSort = enable;
        GlobData.TrainsTypes.FireEventOnSort = enable;
        GlobData.Languages.FireEventOnSort = enable;
        GlobData.Delays.FireEventOnSort = enable;
    }

    private void FGlobalSettings_Load(object sender, EventArgs e)
    {
        optionsView.TreeView.ExpandAll();
        optionsView.SelectedPanel = PanelOf(_startPage);
        UpdateHelpLink();
        EnableEvents(true);
    }

    private void FGlobalSettings_FormClosed(object sender, FormClosedEventArgs e)
    {
        EnableEvents(false);
        GlobData.Config.GlobalSettingsWindow = SettingsWindow.CapturePlacement(this);
        SettingsWindow.SaveConfig();
    }

}