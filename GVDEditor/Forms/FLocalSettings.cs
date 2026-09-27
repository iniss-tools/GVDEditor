using System.Globalization;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Forms.Settings;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Lokálne nastavenia konkrétneho GVD. Stránky sú v strome vľavo; každá je samostatný prvok
///     v <c>Forms/Settings</c>, okno ich len hostí, zbiera ich chyby a pri OK zapíše grafikon.
/// </summary>
public partial class FLocalSettings : Form
{
    private readonly bool _openTabTabEditor;
    private readonly bool _openStateDgmEditor;

    /// <summary>
    ///     Tento priečinok.
    /// </summary>
    public readonly GVDDirectory ThisDir;

    /// <summary>
    ///     Priečinok s písmami pre tabule.
    /// </summary>
    public string FontDir => fontsPage.FontDir;

    /// <summary>
    ///     Stav dat pred otvorenim okna; ak sa okno nezavrie tlacidlom OK, data sa don vratia.
    /// </summary>
    private readonly LocalSettingsSnapshot _snapshot;

    /// <summary>
    ///     Stranka, ktora sa ma vybrat po otvoreni okna.
    /// </summary>
    private readonly LocalSettingsPage _startPage;

    /// <summary>
    ///     Stranky, ktore samy kontroluju svoje udaje, s panelom, v ktorom su.
    /// </summary>
    private readonly (ExOptionsPanel Panel, ISettingsPage Page)[] _checkedPages;

    /// <summary>
    ///     Plnenie stranok - az pri prvom zobrazeni, zvysne postupne po otvoreni okna.
    /// </summary>
    private readonly PageLoader _pages;

    /// <summary>
    ///     Clanok dokumentacie ku kazdej stranke okna.
    /// </summary>
    private readonly Dictionary<ExOptionsPanel, string> _helpLinks;

    /// <summary>
    ///     Vytvori novy formulár typu <see cref="FLocalSettings"/>.
    /// </summary>
    /// <param name="dir">Aktualny priecinok s grafikonom.</param>
    /// <param name="page">Stranka, ktora sa ma otvorit po otvoreni dialogu.</param>
    /// <param name="action">Editor, ktory sa ma otvorit hned po otvoreni dialogu.</param>
    public FLocalSettings(GVDDirectory dir, LocalSettingsPage page = LocalSettingsPage.Grafikon,
        LocalSettingsAction action = LocalSettingsAction.None)
    {
        // stranky menia data priamo v GlobData - Zrusit ich vracia z tejto snimky
        _snapshot = LocalSettingsSnapshot.Capture();

        InitializeComponent();
        this.ApplyThemeAndFonts();
        // SetFormFont zapina AutoSize - okno s menitelnou velkostou by sa nedalo zmensit
        AutoSize = false;
        // nazov stranky nad nou tucne ako v nastaveniach programu
        optionsView.HeaderNodeNameFont = new Font(optionsView.HeaderNodeNameFont, FontStyle.Bold);
        SettingsWindow.ApplyPlacement(this, GlobData.Config.LocalSettingsWindow);
        pGroupStanica.GenerateLinksToChildren = true;
        pGroupTabule.GenerateLinksToChildren = true;

        ThisDir = dir;
        _startPage = page;
        // zobrazi sa len stranka, ktorou sa okno otvara - ostatne sa vytvoria az pri prvom zobrazeni
        optionsView.SelectedPanel = PanelOf(page);
        _openTabTabEditor = action == LocalSettingsAction.OpenTabTabEditor;
        _openStateDgmEditor = action == LocalSettingsAction.OpenStateDgmEditor;

        _helpLinks = new Dictionary<ExOptionsPanel, string>
        {
            [pGrafikon] = LinkConsts.LINK_LOCAL_GRAFIKON,
            [pStanice] = LinkConsts.LINK_LOCAL_STANICE,
            [pDopravcovia] = LinkConsts.LINK_LOCAL_DOPRAVCOVIA,
            [pNastupistia] = LinkConsts.LINK_LOCAL_NASTUPISTIA_KOLAJE,
            [pFonts] = LinkConsts.LINK_TFONTS,
            [pTabTab] = LinkConsts.LINK_TABTAB_EDITOR,
            [pKatTab] = LinkConsts.LINK_TCATALOG,
            [pFyzTab] = LinkConsts.LINK_TPHYSICAL,
            [pLogTab] = LinkConsts.LINK_TLOGICAL,
            [pTTexts] = LinkConsts.LINK_TTEXTS,
            [pStateDgm] = LinkConsts.LINK_LOCAL_STATEDGM
        };
        optionsView.SelectedPanelChanged += (_, _) => UpdateHelpLink();

        _checkedPages =
        [
            (pGrafikon, grafikonPage), (pStanice, customStationsPage), (pDopravcovia, operatorsPage),
            (pNastupistia, platformsTracksPage), (pFonts, fontsPage)
        ];
        foreach (var (_, checkedPage) in _checkedPages)
            checkedPage.ProblemsChanged += (_, _) => UpdateProblems();

        // stranky s kontrolou chyb idu prve, aby sa chyby v strome ukazali co najskor
        var station = dir.GVD.ThisStation;
        _pages = new PageLoader(this, optionsView);
        _pages.Add(pGrafikon, () => grafikonPage.LoadData(dir));
        _pages.Add(pStanice, () => customStationsPage.LoadData(station.Name));
        _pages.Add(pDopravcovia, operatorsPage.LoadData);
        _pages.Add(pNastupistia, platformsTracksPage.LoadData);
        _pages.Add(pFonts, () => fontsPage.LoadData(Utils.ParseStringOrDefault(GlobData.TableFontDir)));
        _pages.Add(pTabTab, () => tabTabPage.LoadData(new TabTabKind(station)));
        _pages.Add(pKatTab, () => catalogTablesPage.LoadData(new CatalogTablesKind()));
        _pages.Add(pFyzTab, () => physicalTablesPage.LoadData(new PhysicalTablesKind()));
        _pages.Add(pLogTab, () => logicalTablesPage.LoadData(new LogicalTablesKind(station)));
        _pages.Add(pTTexts, () => textsPage.LoadData(new TableTextsKind(dir.GVD)));
        _pages.Add(pStateDgm, () => stateDgmPage.LoadData(dir));
        _pages.Load(PanelOf(page));
        UpdateProblems();
    }

    private ExOptionsPanel PanelOf(LocalSettingsPage page) => page switch
    {
        LocalSettingsPage.VlastneStanice => pStanice,
        LocalSettingsPage.Dopravcovia => pDopravcovia,
        LocalSettingsPage.Nastupistia or LocalSettingsPage.Kolaje => pNastupistia,
        LocalSettingsPage.FyzickeTabule => pFyzTab,
        LocalSettingsPage.LogickeTabule => pLogTab,
        LocalSettingsPage.KatalogoveTabule => pKatTab,
        LocalSettingsPage.TabTab => pTabTab,
        LocalSettingsPage.Texty => pTTexts,
        LocalSettingsPage.Pisma => pFonts,
        LocalSettingsPage.StavovyDiagram => pStateDgm,
        _ => pGrafikon
    };

    private void UpdateHelpLink()
    {
        var panel = optionsView.SelectedPanel;
        llHelp.Text = string.Format(CultureInfo.CurrentCulture, Resources.SettingsForm_Napoveda, panel?.NodeText);
        llHelp.Enabled = panel is not null && _helpLinks.ContainsKey(panel);
    }

    private void llHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenHelp();

    private void FLocalSettings_HelpRequested(object sender, HelpEventArgs hlpevent)
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
    ///     V spodnom riadku ukaze prvu chybu stranok a stranky s chybou oznaci v strome.
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

    /// <summary>
    ///     Ak niektora stranka hlasi chybu, prepne na nu a oznaci chybne pole.
    /// </summary>
    private bool CheckPages()
    {
        foreach (var (panel, page) in _checkedPages)
        {
            if (page.FirstProblem is null)
                continue;

            optionsView.SelectedPanel = panel;
            page.FocusFirstProblem();
            return false;
        }

        return true;
    }

    private void bSave_Click(object sender, EventArgs e)
    {
        // kontrola a zapis potrebuju vsetky stranky
        _pages.LoadAll();
        if (!CheckPages())
        {
            DialogResult = DialogResult.None;
            return;
        }

        // Pozice_A.txt nema riadky nastupist - nastupiste bez kolaje sa nezapise a po opatovnom otvoreni zmizne
        var withoutTracks = TrackEditing.PlatformsWithoutTracks(GlobData.Platforms.Where(p => p != Platform.None), GlobData.Tracks);
        if (withoutTracks.Count > 0 &&
            Utils.ShowQuestion(string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_Nastupistia_Bez_Kolaje,
                string.Join(", ", withoutTracks.Select(platform => platform.Key)))) != DialogResult.Yes)
        {
            DialogResult = DialogResult.None;
            optionsView.SelectedPanel = pNastupistia;
            return;
        }

        if (!grafikonPage.RenamePendingDir())
        {
            DialogResult = DialogResult.None;
            optionsView.SelectedPanel = pGrafikon;
            return;
        }

        grafikonPage.Apply();
        DialogResult = DialogResult.OK;
    }

    private void FLocalSettings_Load(object sender, EventArgs e)
    {
        optionsView.TreeView.ExpandAll();
        optionsView.SelectedPanel = PanelOf(_startPage);
        if (_startPage == LocalSettingsPage.Kolaje)
        {
            _pages.Load(pNastupistia);
            platformsTracksPage.SelectFirstTrack();
        }
        UpdateHelpLink();

        if (_openTabTabEditor)
        {
            _pages.Load(pTabTab);
            BeginInvoke(tabTabPage.OpenAdd);
        }

        if (_openStateDgmEditor)
        {
            _pages.Load(pStateDgm);
            BeginInvoke(stateDgmPage.OpenEditor);
        }
        EnableEvents(true);
    }

    private void FLocalSettings_FormClosed(object sender, FormClosedEventArgs e)
    {
        EnableEvents(false);

        GlobData.Config.LocalSettingsWindow = SettingsWindow.CapturePlacement(this);
        SettingsWindow.SaveConfig();

        // Zrusit, krizik aj Esc - vratia sa zmeny na vsetkych strankach
        if (DialogResult != DialogResult.OK)
            _snapshot.Restore();
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
