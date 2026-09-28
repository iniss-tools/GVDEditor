using System.Globalization;
using ExControls;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Snapshots;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Dialog - Lokálne nastavenia konkrétneho GVD. Stránky sú v strome vľavo; každá je samostatný prvok
/// v <c>Forms/Settings</c>, okno ich len hostí, zbiera ich chyby a pri OK zapíše grafikon.
/// </summary>
public partial class FLocalSettings : Form
{
    private readonly bool _openTabTabEditor;
    private readonly bool _openStateDgmEditor;

    /// <summary>
    /// Tento priečinok.
    /// </summary>
    public readonly GVDDirectory ThisDir;

    /// <summary>
    /// Priečinok s písmami pre tabule.
    /// </summary>
    public string FontDir => fontsPage.FontDir;

    /// <summary>
    /// Stav dat pred otvorenim okna; ak sa okno nezavrie tlacidlom OK, data sa don vratia.
    /// </summary>
    private readonly LocalSettingsSnapshot _snapshot;

    /// <summary>
    /// Stranka, ktora sa ma vybrat po otvoreni okna.
    /// </summary>
    private readonly LocalSettingsPage _startPage;

    /// <summary>
    /// Stranky, ktore samy kontroluju svoje udaje, s panelom, v ktorom su.
    /// </summary>
    private readonly (ExOptionsPanel Panel, ISettingsPage Page)[] _checkedPages;

    /// <summary>
    /// Polozka, ktora sa ma po otvoreni okna vybrat.
    /// </summary>
    private readonly object? _select;

    /// <summary>
    /// Plnenie stranok - az pri prvom zobrazeni, zvysne postupne po otvoreni okna.
    /// </summary>
    private readonly PageLoader _pages;

    /// <summary>
    /// Clanok dokumentacie ku kazdej stranke okna.
    /// </summary>
    private readonly Dictionary<ExOptionsPanel, string> _helpLinks;

    /// <summary>
    /// Vytvori novy formulár typu <see cref="FLocalSettings"/>.
    /// </summary>
    /// <param name="dir">Aktualny priecinok s grafikonom.</param>
    /// <param name="page">Stranka, ktora sa ma otvorit po otvoreni dialogu.</param>
    /// <param name="action">Editor, ktory sa ma otvorit hned po otvoreni dialogu.</param>
    /// <param name="select">Polozka, ktora sa ma na stranke vybrat (napr. text na tabuli z analyzy grafikonu).</param>
    public FLocalSettings(GVDDirectory dir, LocalSettingsPage page = LocalSettingsPage.Grafikon,
        LocalSettingsAction action = LocalSettingsAction.None, object? select = null)
    {
        // stranky menia data priamo v GlobData - Zrusit ich vracia z tejto snimky
        _snapshot = LocalSettingsSnapshot.Capture();

        InitializeComponent();
        this.ApplyThemeAndFonts();
        // koliesko posuva stranku, nie hodnotu zoznamu alebo pola pod kurzorom
        WheelScroll.Attach(this);
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
        _select = select;

        _helpLinks = new Dictionary<ExOptionsPanel, string>
        {
            [pGrafikon] = GvdLinkConsts.LINK_LOCAL_GRAFIKON,
            [pJazyky] = GvdLinkConsts.LINK_LOCAL_JAZYKY,
            [pStanice] = GvdLinkConsts.LINK_LOCAL_STANICE,
            [pDopravcovia] = GvdLinkConsts.LINK_LOCAL_DOPRAVCOVIA,
            [pNastupistia] = GvdLinkConsts.LINK_LOCAL_NASTUPISTIA_KOLAJE,
            [pFonts] = GvdLinkConsts.LINK_TFONTS,
            [pTabTab] = GvdLinkConsts.LINK_TABTAB_EDITOR,
            [pKatTab] = GvdLinkConsts.LINK_TCATALOG,
            [pFyzTab] = GvdLinkConsts.LINK_TPHYSICAL,
            [pLogTab] = GvdLinkConsts.LINK_TLOGICAL,
            [pTTexts] = GvdLinkConsts.LINK_TTEXTS,
            [pStateDgm] = GvdLinkConsts.LINK_LOCAL_STATEDGM
        };
        optionsView.SelectedPanelChanged += (_, _) => UpdateHelpLink();

        _checkedPages =
        [
            (pGrafikon, grafikonPage), (pJazyky, languagesPage), (pStanice, customStationsPage), (pDopravcovia, operatorsPage),
            (pNastupistia, platformsTracksPage), (pFonts, fontsPage), (pFyzTab, physicalTablesPage), (pTTexts, textsPage),
            (pLogTab, logicalTablesPage), (pKatTab, catalogTablesPage)
        ];
        foreach (var (_, checkedPage) in _checkedPages)
            checkedPage.ProblemsChanged += (_, _) => UpdateProblems();

        // stranky s kontrolou chyb idu prve, aby sa chyby v strome ukazali co najskor
        var station = dir.GVD.ThisStation;
        _pages = new PageLoader(this, optionsView);
        _pages.Add(pGrafikon, () => grafikonPage.LoadData(dir));
        _pages.Add(pJazyky, languagesPage.LoadData);
        _pages.Add(pStanice, () => customStationsPage.LoadData(station.Name));
        _pages.Add(pDopravcovia, operatorsPage.LoadData);
        _pages.Add(pNastupistia, platformsTracksPage.LoadData);
        _pages.Add(pFonts, () => fontsPage.LoadData(Utils.ParseStringOrDefault(GlobData.TableFontDir)));
        _pages.Add(pFyzTab, physicalTablesPage.LoadData);
        _pages.Add(pTTexts, () => textsPage.LoadData(dir.GVD));
        _pages.Add(pLogTab, () => logicalTablesPage.LoadData(station));
        _pages.Add(pKatTab, catalogTablesPage.LoadData);
        _pages.Add(pTabTab, () => tabTabPage.LoadData(station));
        _pages.Add(pStateDgm, () => stateDgmPage.LoadData(dir));
        _pages.Load(PanelOf(page));
        UpdateProblems();
    }

    private ExOptionsPanel PanelOf(LocalSettingsPage page) => page switch
    {
        LocalSettingsPage.JazykyHlaseni => pJazyky,
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

    /// <summary>
    /// Ak niektora stranka hlasi chybu, prepne na nu a oznaci chybne pole.
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
        languagesPage.Apply();
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
        if (_select is TableText text)
            textsPage.SelectText(text);
        else if (_select is TableCatalog catalog)
            catalogTablesPage.SelectTable(catalog);
        UpdateHelpLink();

        if (_openTabTabEditor)
        {
            _pages.Load(pTabTab);
            BeginInvoke(tabTabPage.OpenEditor);
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
