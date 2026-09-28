using ExControls;
using ToolsCore.Entities;

namespace GVDEditor.UI.Settings
{
    partial class FGlobalSettings
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
            OptionsNode optionsNode3 = new OptionsNode();
            OptionsNode optionsNode4 = new OptionsNode();
            OptionsNode optionsNode5 = new OptionsNode();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FGlobalSettings));
            optionsView = new ExOptionsView();
            grafikonyPage = new GrafikonyPage();
            audioPage = new AudioPage();
            trainTypesPage = new TrainTypesPage();
            languagesPage = new LanguagesPage();
            delaysPage = new DelaysPage();
            tlpBottom = new TableLayoutPanel();
            llHelp = new LinkLabel();
            lProblem = new Label();
            pGrafikony = new ExOptionsPanel(optionsView);
            pJazyky = new ExOptionsPanel(optionsView);
            pMeskania = new ExOptionsPanel(optionsView);
            pTrainTypes = new ExOptionsPanel(optionsView);
            pAudio = new ExOptionsPanel(optionsView);
            bSave = new ExButton();
            bStorno = new ExButton();
            ((ISupportInitialize)optionsView).BeginInit();
            optionsView.SuspendLayout();
            tlpBottom.SuspendLayout();
            pGrafikony.SuspendLayout();
            pJazyky.SuspendLayout();
            pMeskania.SuspendLayout();
            pTrainTypes.SuspendLayout();
            pAudio.SuspendLayout();
            SuspendLayout();
            // 
            // optionsView
            // 
            resources.ApplyResources(optionsView, "optionsView");
            optionsView.HeaderNodeNameVisible = true;
            optionsView.Name = "optionsView";
            optionsView.Panels.Add(pGrafikony);
            optionsView.Panels.Add(pJazyky);
            optionsView.Panels.Add(pMeskania);
            optionsView.Panels.Add(pTrainTypes);
            optionsView.Panels.Add(pAudio);
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
            // pGrafikony
            // 
            pGrafikony.Node = optionsNode1;
            resources.ApplyResources(pGrafikony, "pGrafikony");
            pGrafikony.Controls.Add(grafikonyPage);
            pGrafikony.Name = "pGrafikony";
            pGrafikony.ParentNode = null;
            // 
            // grafikonyPage
            // 
            resources.ApplyResources(grafikonyPage, "grafikonyPage");
            grafikonyPage.Name = "grafikonyPage";
            // 
            // pJazyky
            // 
            pJazyky.Node = optionsNode2;
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
            // pMeskania
            // 
            pMeskania.Node = optionsNode3;
            resources.ApplyResources(pMeskania, "pMeskania");
            pMeskania.Controls.Add(delaysPage);
            pMeskania.Name = "pMeskania";
            pMeskania.ParentNode = null;
            // 
            // delaysPage
            // 
            resources.ApplyResources(delaysPage, "delaysPage");
            delaysPage.Name = "delaysPage";
            // 
            // pTrainTypes
            // 
            pTrainTypes.Node = optionsNode4;
            resources.ApplyResources(pTrainTypes, "pTrainTypes");
            pTrainTypes.Controls.Add(trainTypesPage);
            pTrainTypes.Name = "pTrainTypes";
            pTrainTypes.ParentNode = null;
            // 
            // trainTypesPage
            // 
            resources.ApplyResources(trainTypesPage, "trainTypesPage");
            trainTypesPage.Name = "trainTypesPage";
            // 
            // pAudio
            // 
            pAudio.Node = optionsNode5;
            resources.ApplyResources(pAudio, "pAudio");
            pAudio.Controls.Add(audioPage);
            pAudio.Name = "pAudio";
            pAudio.ParentNode = null;
            // 
            // audioPage
            // 
            resources.ApplyResources(audioPage, "audioPage");
            audioPage.Name = "audioPage";
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
            bSave.DefaultStyle = true;
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
            // FGlobalSettings
            // 
            AcceptButton = bSave;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = bStorno;
            Controls.Add(optionsView);
            Controls.Add(tlpBottom);
            MinimizeBox = false;
            Name = "FGlobalSettings";
            ShowIcon = false;
            ShowInTaskbar = false;
            HelpRequested += FGlobalSettings_HelpRequested;
            FormClosed += FGlobalSettings_FormClosed;
            Load += FGlobalSettings_Load;
            ((ISupportInitialize)optionsView).EndInit();
            optionsView.ResumeLayout(false);
            tlpBottom.ResumeLayout(false);
            tlpBottom.PerformLayout();
            pGrafikony.ResumeLayout(false);
            pGrafikony.PerformLayout();
            pJazyky.ResumeLayout(false);
            pJazyky.PerformLayout();
            pMeskania.ResumeLayout(false);
            pMeskania.PerformLayout();
            pTrainTypes.ResumeLayout(false);
            pTrainTypes.PerformLayout();
            pAudio.ResumeLayout(false);
            pAudio.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private ExOptionsView optionsView;
        private ExOptionsPanel pJazyky;
        private ExControls.ExButton bSave;
        private ExControls.ExButton bStorno;
        private ExOptionsPanel pGrafikony;
        private ExOptionsPanel pMeskania;
        private ExOptionsPanel pTrainTypes;
        private ExOptionsPanel pAudio;
        private LanguagesPage languagesPage;
        private DelaysPage delaysPage;
        private TableLayoutPanel tlpBottom;
        private LinkLabel llHelp;
        private Label lProblem;
        private TrainTypesPage trainTypesPage;
        private GrafikonyPage grafikonyPage;
        private AudioPage audioPage;
    }
}