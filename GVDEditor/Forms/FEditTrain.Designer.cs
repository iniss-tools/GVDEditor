using System.Windows.Forms;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Forms.EditTrain;

namespace GVDEditor.Forms
{
    partial class FEditTrain
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
            components = new Container();
            OptionsNode optionsNode1 = new OptionsNode();
            OptionsNode optionsNode2 = new OptionsNode();
            OptionsNode optionsNode5 = new OptionsNode();
            OptionsNode optionsNode6 = new OptionsNode();
            OptionsNode optionsNode7 = new OptionsNode();
            OptionsNode optionsNode8 = new OptionsNode();
            OptionsNode optionsNode9 = new OptionsNode();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FEditTrain));
            optionsView = new ExOptionsView();
            pVlak = new ExOptionsPanel(optionsView);
            dodatkyPage = new TrainDodatkyPage();
            radeniePage = new TrainRadeniePage();
            pTrasa = new ExOptionsPanel(optionsView);
            routePage = new TrainRoutePage();
            trainPage = new TrainBasicsPage();
            pPlatnost = new ExOptionsPanel(optionsView);
            validityPage = new TrainValidityPage();
            pGroupHlasenia = new ExOptionsPanel(optionsView);
            pJazyky = new ExOptionsPanel(optionsView);
            languagesPage = new TrainLanguagesPage();
            tlpBottom = new TableLayoutPanel();
            llHelp = new LinkLabel();
            lProblem = new Label();
            bSave = new ExButton();
            bZrusit = new ExButton();
            pDodatky = new ExOptionsPanel(optionsView);
            pRadenie = new ExOptionsPanel(optionsView);
            ((ISupportInitialize)optionsView).BeginInit();
            optionsView.SuspendLayout();
            pVlak.SuspendLayout();
            pTrasa.SuspendLayout();
            pPlatnost.SuspendLayout();
            pJazyky.SuspendLayout();
            tlpBottom.SuspendLayout();
            pDodatky.SuspendLayout();
            pRadenie.SuspendLayout();
            SuspendLayout();
            // 
            // optionsView
            // 
            resources.ApplyResources(optionsView, "optionsView");
            optionsView.HeaderNodeNameVisible = true;
            optionsView.Name = "optionsView";
            optionsView.Panels.Add(pVlak);
            optionsView.Panels.Add(pTrasa);
            optionsView.Panels.Add(pPlatnost);
            optionsView.Panels.Add(pGroupHlasenia);
            optionsView.Panels.Add(pJazyky);
            optionsView.Panels.Add(pDodatky);
            optionsView.Panels.Add(pRadenie);
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
            // pVlak
            // 
            pVlak.Node = optionsNode1;
            resources.ApplyResources(pVlak, "pVlak");
            pVlak.Controls.Add(trainPage);
            pVlak.Name = "pVlak";
            pVlak.ParentNode = null;
            // 
            // trainPage
            // 
            resources.ApplyResources(trainPage, "trainPage");
            trainPage.Name = "trainPage";
            // 
            // pTrasa
            // 
            pTrasa.Node = optionsNode2;
            resources.ApplyResources(pTrasa, "pTrasa");
            pTrasa.Controls.Add(routePage);
            pTrasa.Name = "pTrasa";
            pTrasa.ParentNode = null;
            // 
            // routePage
            // 
            resources.ApplyResources(routePage, "routePage");
            routePage.Name = "routePage";
            // 
            // pPlatnost
            // 
            pPlatnost.Node = optionsNode5;
            resources.ApplyResources(pPlatnost, "pPlatnost");
            pPlatnost.Controls.Add(validityPage);
            pPlatnost.Name = "pPlatnost";
            pPlatnost.ParentNode = null;
            // 
            // validityPage
            // 
            resources.ApplyResources(validityPage, "validityPage");
            validityPage.Name = "validityPage";
            // 
            // pGroupHlasenia
            // 
            pGroupHlasenia.Node = optionsNode6;
            resources.ApplyResources(pGroupHlasenia, "pGroupHlasenia");
            pGroupHlasenia.Name = "pGroupHlasenia";
            pGroupHlasenia.ParentNode = null;
            // 
            // pJazyky
            // 
            pJazyky.Node = optionsNode7;
            resources.ApplyResources(pJazyky, "pJazyky");
            pJazyky.Controls.Add(languagesPage);
            pJazyky.Name = "pJazyky";
            pJazyky.ParentNode = optionsNode6;
            // 
            // languagesPage
            // 
            resources.ApplyResources(languagesPage, "languagesPage");
            languagesPage.Name = "languagesPage";
            // 
            // tlpBottom
            // 
            resources.ApplyResources(tlpBottom, "tlpBottom");
            tlpBottom.Controls.Add(llHelp, 0, 0);
            tlpBottom.Controls.Add(lProblem, 1, 0);
            tlpBottom.Controls.Add(bSave, 2, 0);
            tlpBottom.Controls.Add(bZrusit, 3, 0);
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
            bSave.DefaultStyle = true;
            bSave.Name = "bSave";
            bSave.UseVisualStyleBackColor = true;
            bSave.Click += bSave_Click;
            // 
            // bZrusit
            // 
            resources.ApplyResources(bZrusit, "bZrusit");
            bZrusit.DialogResult = DialogResult.Cancel;
            bZrusit.Name = "bZrusit";
            bZrusit.UseVisualStyleBackColor = true;
            bZrusit.Click += bZrusit_Click;
            // 
            // pDodatky
            // 
            pDodatky.Node = optionsNode8;
            resources.ApplyResources(pDodatky, "pDodatky");
            pDodatky.Controls.Add(dodatkyPage);
            pDodatky.Name = "pDodatky";
            pDodatky.ParentNode = optionsNode6;
            // 
            // dodatkyPage
            // 
            resources.ApplyResources(dodatkyPage, "dodatkyPage");
            dodatkyPage.Name = "dodatkyPage";
            // 
            // pRadenie
            // 
            pRadenie.Node = optionsNode9;
            resources.ApplyResources(pRadenie, "pRadenie");
            pRadenie.Controls.Add(radeniePage);
            pRadenie.Name = "pRadenie";
            pRadenie.ParentNode = optionsNode6;
            // 
            // radeniePage
            // 
            resources.ApplyResources(radeniePage, "radeniePage");
            radeniePage.Name = "radeniePage";
            // 
            // FEditTrain
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = bZrusit;
            Controls.Add(optionsView);
            Controls.Add(tlpBottom);
            MinimizeBox = false;
            Name = "FEditTrain";
            ShowIcon = false;
            ShowInTaskbar = false;
            FormClosed += FEditTrain_FormClosed;
            HelpRequested += FEditTrain_HelpRequested;
            Load += FEditTrain_Load;
            ((ISupportInitialize)optionsView).EndInit();
            optionsView.ResumeLayout(false);
            pVlak.ResumeLayout(false);
            pTrasa.ResumeLayout(false);
            pPlatnost.ResumeLayout(false);
            pJazyky.ResumeLayout(false);
            tlpBottom.ResumeLayout(false);
            tlpBottom.PerformLayout();
            pDodatky.ResumeLayout(false);
            pDodatky.PerformLayout();
            pRadenie.ResumeLayout(false);
            pRadenie.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private ExOptionsPanel pRadenie;
        private ExControls.ExButton bSave;
        private ExControls.ExButton bZrusit;
        private ExOptionsPanel pDodatky;
        private ExOptionsView optionsView;
        private ExOptionsPanel pVlak;
        private EditTrain.TrainDodatkyPage dodatkyPage;
        private EditTrain.TrainRadeniePage radeniePage;
        private ExOptionsPanel pTrasa;
        private EditTrain.TrainRoutePage routePage;
        private EditTrain.TrainBasicsPage trainPage;
        private ExOptionsPanel pPlatnost;
        private EditTrain.TrainValidityPage validityPage;
        private ExOptionsPanel pGroupHlasenia;
        private ExOptionsPanel pJazyky;
        private EditTrain.TrainLanguagesPage languagesPage;
        private TableLayoutPanel tlpBottom;
        private LinkLabel llHelp;
        private Label lProblem;
    }
}