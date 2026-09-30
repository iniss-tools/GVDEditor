using System.Windows.Forms;
using ExControls;

namespace GVDEditor.UI.Settings
{
    partial class FLocalSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            OptionsNode optionsNode1 = new OptionsNode();
            OptionsNode optionsNode2 = new OptionsNode();
            OptionsNode optionsNode3 = new OptionsNode();
            OptionsNode optionsNode4 = new OptionsNode();
            OptionsNode optionsNode5 = new OptionsNode();
            OptionsNode optionsNode6 = new OptionsNode();
            OptionsNode optionsNode7 = new OptionsNode();
            OptionsNode optionsNode8 = new OptionsNode();
            OptionsNode optionsNode9 = new OptionsNode();
            OptionsNode optionsNode10 = new OptionsNode();
            OptionsNode optionsNode11 = new OptionsNode();
            OptionsNode optionsNode12 = new OptionsNode();
            OptionsNode optionsNode13 = new OptionsNode();
            OptionsNode optionsNode14 = new OptionsNode();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FLocalSettings));
            optionsView = new ExOptionsView();
            grafikonPage = new GrafikonPage();
            languagesPage = new GrafikonLanguagesPage();
            platformsTracksPage = new PlatformsTracksPage();
            physicalTablesPage = new PhysicalTablesPage();
            logicalTablesPage = new LogicalTablesPage();
            catalogTablesPage = new CatalogTablesPage();
            tabTabPage = new TabTabPage();
            textsPage = new TableTextsPage();
            stateDgmPage = new StateDgmPage();
            fontsPage = new FontsPage();
            pGroupStanica = new ExOptionsPanel(optionsView);
            pGroupTabule = new ExOptionsPanel(optionsView);
            customStationsPage = new CustomStationsPage();
            operatorsPage = new OperatorsPage();
            tlpBottom = new TableLayoutPanel();
            llHelp = new LinkLabel();
            lProblem = new Label();
            bSave = new ExButton();
            bStorno = new ExButton();
            pGrafikon = new ExOptionsPanel(optionsView);
            pJazyky = new ExOptionsPanel(optionsView);
            pStanice = new ExOptionsPanel(optionsView);
            pDopravcovia = new ExOptionsPanel(optionsView);
            pNastupistia = new ExOptionsPanel(optionsView);
            pFyzTab = new ExOptionsPanel(optionsView);
            pLogTab = new ExOptionsPanel(optionsView);
            pKatTab = new ExOptionsPanel(optionsView);
            pTabTab = new ExOptionsPanel(optionsView);
            pTTexts = new ExOptionsPanel(optionsView);
            pFonts = new ExOptionsPanel(optionsView);
            pStateDgm = new ExOptionsPanel(optionsView);
            ((ISupportInitialize)optionsView).BeginInit();
            optionsView.SuspendLayout();
            tlpBottom.SuspendLayout();
            pGrafikon.SuspendLayout();
            pJazyky.SuspendLayout();
            pStanice.SuspendLayout();
            pDopravcovia.SuspendLayout();
            pNastupistia.SuspendLayout();
            pFyzTab.SuspendLayout();
            pLogTab.SuspendLayout();
            pKatTab.SuspendLayout();
            pTabTab.SuspendLayout();
            pTTexts.SuspendLayout();
            pFonts.SuspendLayout();
            pStateDgm.SuspendLayout();
            SuspendLayout();
            // 
            // tlpBottom
            // 
            resources.ApplyResources(tlpBottom, "tlpBottom");
            tlpBottom.Controls.Add(llHelp, 0, 0);
            tlpBottom.Controls.Add(lProblem, 1, 0);
            tlpBottom.Controls.Add(bSave, 2, 0);
            tlpBottom.Controls.Add(bStorno, 3, 0);
            tlpBottom.Name = "tlpBottom";
            // 
            // llHelp
            // 
            resources.ApplyResources(llHelp, "llHelp");
            llHelp.Name = "llHelp";
            llHelp.TabStop = true;
            llHelp.LinkClicked += llHelp_LinkClicked;
            // 
            // lProblem
            // 
            resources.ApplyResources(lProblem, "lProblem");
            lProblem.AutoEllipsis = true;
            lProblem.Name = "lProblem";
            lProblem.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // bSave
            // 
            resources.ApplyResources(bSave, "bSave");
            bSave.Name = "bSave";
            bSave.UseVisualStyleBackColor = true;
            bSave.Click += bSave_Click;
            // 
            // bStorno
            // 
            resources.ApplyResources(bStorno, "bStorno");
            bStorno.DialogResult = DialogResult.Cancel;
            bStorno.Name = "bStorno";
            bStorno.UseVisualStyleBackColor = true;
            // 
            // optionsView
            // 
            resources.ApplyResources(optionsView, "optionsView");
            optionsView.HeaderNodeNameVisible = true;
            optionsView.Name = "optionsView";
            optionsView.Panels.Add(pGrafikon);
            optionsView.Panels.Add(pJazyky);
            optionsView.Panels.Add(pGroupStanica);
            optionsView.Panels.Add(pStanice);
            optionsView.Panels.Add(pDopravcovia);
            optionsView.Panels.Add(pNastupistia);
            optionsView.Panels.Add(pGroupTabule);
            optionsView.Panels.Add(pFonts);
            optionsView.Panels.Add(pTabTab);
            optionsView.Panels.Add(pKatTab);
            optionsView.Panels.Add(pFyzTab);
            optionsView.Panels.Add(pLogTab);
            optionsView.Panels.Add(pTTexts);
            optionsView.Panels.Add(pStateDgm);
            optionsView.SearchBoxVisible = false;
            optionsView.TreeView.Dock = DockStyle.Fill;
            optionsView.TreeView.FullRowSelect = true;
            optionsView.TreeView.HideSelection = false;
            optionsView.TreeView.ItemHeight = 22;
            optionsView.TreeView.Name = "treeView";
            optionsView.TreeView.PathSeparator = " / ";
            optionsView.TreeView.ShowLines = false;
            optionsView.TreeView.ShowNodeToolTips = true;
            optionsView.TreeView.Style = ExTreeViewStyle.Light;
            optionsView.TreeView.TabIndex = 0;
            // 
            // pGroupStanica
            // 
            pGroupStanica.Node = optionsNode2;
            resources.ApplyResources(pGroupStanica, "pGroupStanica");
            pGroupStanica.Name = "pGroupStanica";
            pGroupStanica.ParentNode = null;
            // 
            // pGroupTabule
            // 
            pGroupTabule.Node = optionsNode7;
            resources.ApplyResources(pGroupTabule, "pGroupTabule");
            pGroupTabule.Name = "pGroupTabule";
            pGroupTabule.ParentNode = null;
            // 
            // pGrafikon
            // 
            pGrafikon.Node = optionsNode1;
            resources.ApplyResources(pGrafikon, "pGrafikon");
            pGrafikon.Controls.Add(grafikonPage);
            pGrafikon.Name = "pGrafikon";
            pGrafikon.ParentNode = null;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // grafikonPage
            // 
            resources.ApplyResources(grafikonPage, "grafikonPage");
            grafikonPage.Name = "grafikonPage";
            // 
            // pJazyky
            // 
            pJazyky.Node = optionsNode6;
            resources.ApplyResources(pJazyky, "pJazyky");
            pJazyky.Controls.Add(languagesPage);
            pJazyky.Name = "pJazyky";
            pJazyky.ParentNode = null;
            // 
            // languagesPage
            // 
            resources.ApplyResources(languagesPage, "languagesPage");
            languagesPage.Name = "languagesPage";
            // 
            // pStanice
            // 
            pStanice.Node = optionsNode3;
            resources.ApplyResources(pStanice, "pStanice");
            pStanice.Controls.Add(customStationsPage);
            pStanice.Name = "pStanice";
            pStanice.ParentNode = optionsNode2;
            // 
            // customStationsPage
            // 
            resources.ApplyResources(customStationsPage, "customStationsPage");
            customStationsPage.Name = "customStationsPage";
            // 
            // pDopravcovia
            // 
            pDopravcovia.Node = optionsNode4;
            resources.ApplyResources(pDopravcovia, "pDopravcovia");
            pDopravcovia.Controls.Add(operatorsPage);
            pDopravcovia.Name = "pDopravcovia";
            pDopravcovia.ParentNode = optionsNode2;
            // 
            // operatorsPage
            // 
            resources.ApplyResources(operatorsPage, "operatorsPage");
            operatorsPage.Name = "operatorsPage";
            // 
            // pNastupistia
            // 
            pNastupistia.Node = optionsNode5;
            resources.ApplyResources(pNastupistia, "pNastupistia");
            pNastupistia.Controls.Add(platformsTracksPage);
            pNastupistia.Name = "pNastupistia";
            pNastupistia.ParentNode = optionsNode2;
            // 
            // platformsTracksPage
            // 
            resources.ApplyResources(platformsTracksPage, "platformsTracksPage");
            platformsTracksPage.Name = "platformsTracksPage";
            // 
            // pFyzTab
            // 
            pFyzTab.Node = optionsNode11;
            resources.ApplyResources(pFyzTab, "pFyzTab");
            pFyzTab.Controls.Add(physicalTablesPage);
            pFyzTab.Name = "pFyzTab";
            pFyzTab.ParentNode = optionsNode7;
            // 
            // physicalTablesPage
            // 
            resources.ApplyResources(physicalTablesPage, "physicalTablesPage");
            physicalTablesPage.Name = "physicalTablesPage";
            // 
            // pLogTab
            // 
            pLogTab.Node = optionsNode12;
            resources.ApplyResources(pLogTab, "pLogTab");
            pLogTab.Controls.Add(logicalTablesPage);
            pLogTab.Name = "pLogTab";
            pLogTab.ParentNode = optionsNode7;
            // 
            // logicalTablesPage
            // 
            resources.ApplyResources(logicalTablesPage, "logicalTablesPage");
            logicalTablesPage.Name = "logicalTablesPage";
            // 
            // pKatTab
            // 
            pKatTab.Node = optionsNode10;
            resources.ApplyResources(pKatTab, "pKatTab");
            pKatTab.Controls.Add(catalogTablesPage);
            pKatTab.Name = "pKatTab";
            pKatTab.ParentNode = optionsNode7;
            // 
            // catalogTablesPage
            // 
            resources.ApplyResources(catalogTablesPage, "catalogTablesPage");
            catalogTablesPage.Name = "catalogTablesPage";
            // 
            // pTabTab
            // 
            pTabTab.Node = optionsNode9;
            resources.ApplyResources(pTabTab, "pTabTab");
            pTabTab.Controls.Add(tabTabPage);
            pTabTab.Name = "pTabTab";
            pTabTab.ParentNode = optionsNode7;
            // 
            // tabTabPage
            // 
            resources.ApplyResources(tabTabPage, "tabTabPage");
            tabTabPage.Name = "tabTabPage";
            // 
            // pTTexts
            // 
            pTTexts.Node = optionsNode13;
            resources.ApplyResources(pTTexts, "pTTexts");
            pTTexts.Controls.Add(textsPage);
            pTTexts.Name = "pTTexts";
            pTTexts.ParentNode = optionsNode7;
            // 
            // textsPage
            // 
            resources.ApplyResources(textsPage, "textsPage");
            textsPage.Name = "textsPage";
            // 
            // pFonts
            // 
            pFonts.Node = optionsNode8;
            resources.ApplyResources(pFonts, "pFonts");
            pFonts.Controls.Add(fontsPage);
            pFonts.Name = "pFonts";
            pFonts.ParentNode = optionsNode7;
            // 
            // fontsPage
            // 
            resources.ApplyResources(fontsPage, "fontsPage");
            fontsPage.Name = "fontsPage";
            // 
            // pStateDgm
            // 
            pStateDgm.Node = optionsNode14;
            resources.ApplyResources(pStateDgm, "pStateDgm");
            pStateDgm.Controls.Add(stateDgmPage);
            pStateDgm.Name = "pStateDgm";
            pStateDgm.ParentNode = null;
            // 
            // stateDgmPage
            // 
            resources.ApplyResources(stateDgmPage, "stateDgmPage");
            stateDgmPage.Name = "stateDgmPage";
            // 
            // FLocalSettings
            // 
            AcceptButton = bSave;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = bStorno;
            Controls.Add(optionsView);
            Controls.Add(tlpBottom);
            MinimizeBox = false;
            Name = "FLocalSettings";
            ShowIcon = false;
            ShowInTaskbar = false;
            HelpRequested += FLocalSettings_HelpRequested;
            FormClosed += FLocalSettings_FormClosed;
            Load += FLocalSettings_Load;
            ((ISupportInitialize)optionsView).EndInit();
            optionsView.ResumeLayout(false);
            tlpBottom.ResumeLayout(false);
            tlpBottom.PerformLayout();
            pGrafikon.ResumeLayout(false);
            pJazyky.ResumeLayout(false);
            pStanice.ResumeLayout(false);
            pStanice.PerformLayout();
            pDopravcovia.ResumeLayout(false);
            pDopravcovia.PerformLayout();
            pNastupistia.ResumeLayout(false);
            pNastupistia.PerformLayout();
            pFyzTab.ResumeLayout(false);
            pFyzTab.PerformLayout();
            pLogTab.ResumeLayout(false);
            pLogTab.PerformLayout();
            pKatTab.ResumeLayout(false);
            pKatTab.PerformLayout();
            pTabTab.ResumeLayout(false);
            pTabTab.PerformLayout();
            pTTexts.ResumeLayout(false);
            pTTexts.PerformLayout();
            pFonts.ResumeLayout(false);
            pFonts.PerformLayout();
            pStateDgm.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private ExControls.ExButton bSave;
        private ExControls.ExButton bStorno;
        private ExOptionsPanel pDopravcovia;
        private ExOptionsPanel pGrafikon;
        private ExOptionsPanel pJazyky;
        private ExOptionsView optionsView;
        private ExOptionsPanel pNastupistia;
        private ExOptionsPanel pFyzTab;
        private ExOptionsPanel pKatTab;
        private ExOptionsPanel pTabTab;
        private ExOptionsPanel pLogTab;
        private ExOptionsPanel pTTexts;
        private ExOptionsPanel pFonts;
        private ExOptionsPanel pStateDgm;
        private ExOptionsPanel pStanice;
        private ExOptionsPanel pGroupStanica;
        private ExOptionsPanel pGroupTabule;
        private CustomStationsPage customStationsPage;
        private OperatorsPage operatorsPage;
        private TableLayoutPanel tlpBottom;
        private LinkLabel llHelp;
        private Label lProblem;
        private FontsPage fontsPage;
        private GrafikonPage grafikonPage;
        private GrafikonLanguagesPage languagesPage;
        private PlatformsTracksPage platformsTracksPage;
        private PhysicalTablesPage physicalTablesPage;
        private LogicalTablesPage logicalTablesPage;
        private CatalogTablesPage catalogTablesPage;
        private TabTabPage tabTabPage;
        private TableTextsPage textsPage;
        private StateDgmPage stateDgmPage;
    }
}