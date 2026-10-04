using ExControls;

namespace GVDEditor.UI.InissSettings
{
    partial class FRunConfigurations
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
            var resources = new ComponentResourceManager(typeof(FRunConfigurations));
            tlpMain = new TableLayoutPanel();
            scMain = new SplitContainer();
            tlpLeft = new TableLayoutPanel();
            flpListButtons = new FlowLayoutPanel();
            bAdd = new ExButton();
            bCopy = new ExButton();
            bRemove = new ExButton();
            tvConfigs = new ExTreeView();
            pDetail = new Panel();
            tlpDetail = new TableLayoutPanel();
            lName = new Label();
            tbName = new ExTextBox();
            lProgram = new Label();
            cbProgram = new ExComboBox();
            lRegistry = new Label();
            cbRegistry = new ExComboBox();
            lBranchInfo = new Label();
            cboxAdmin = new ExCheckBox();
            cboxShared = new ExCheckBox();
            lSwitches = new Label();
            tlpSwitches = new TableLayoutPanel();
            cboxMinimize = new ExCheckBox();
            cboxMultiuse = new ExCheckBox();
            cboxRemote = new ExCheckBox();
            cboxExport = new ExCheckBox();
            cboxExportHlas = new ExCheckBox();
            cboxImport = new ExCheckBox();
            cboxImportDat = new ExCheckBox();
            cboxNoRestore = new ExCheckBox();
            lActivators = new Label();
            flpActivators = new FlowLayoutPanel();
            lExtra = new Label();
            tbExtra = new ExTextBox();
            lSaveBefore = new Label();
            cbSaveBefore = new ExComboBox();
            cboxAnalyze = new ExCheckBox();
            lWhenRunning = new Label();
            cbWhenRunning = new ExComboBox();
            lCommandLine = new Label();
            tbCommandLine = new ExTextBox();
            flpChecks = new FlowLayoutPanel();
            flpActions = new FlowLayoutPanel();
            bInissSettings = new ExButton();
            bLogs = new ExButton();
            flpButtons = new FlowLayoutPanel();
            bCancel = new ExButton();
            bOK = new ExButton();
            bRun = new ExButton();
            ((ISupportInitialize)scMain).BeginInit();
            scMain.Panel1.SuspendLayout();
            scMain.Panel2.SuspendLayout();
            tlpMain.SuspendLayout();
            scMain.SuspendLayout();
            tlpLeft.SuspendLayout();
            flpListButtons.SuspendLayout();
            pDetail.SuspendLayout();
            tlpDetail.SuspendLayout();
            tlpSwitches.SuspendLayout();
            flpActivators.SuspendLayout();
            flpChecks.SuspendLayout();
            flpActions.SuspendLayout();
            flpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(scMain, 0, 0);
            tlpMain.Controls.Add(flpButtons, 0, 1);
            tlpMain.Name = "tlpMain";
            // 
            // scMain
            // 
            resources.ApplyResources(scMain, "scMain");
            scMain.Panel1.Controls.Add(tlpLeft);
            scMain.Panel2.Controls.Add(pDetail);
            scMain.Name = "scMain";
            // 
            // tlpLeft
            // 
            resources.ApplyResources(tlpLeft, "tlpLeft");
            tlpLeft.Controls.Add(flpListButtons, 0, 0);
            tlpLeft.Controls.Add(tvConfigs, 0, 1);
            tlpLeft.Name = "tlpLeft";
            // 
            // flpListButtons
            // 
            resources.ApplyResources(flpListButtons, "flpListButtons");
            flpListButtons.Controls.Add(bAdd);
            flpListButtons.Controls.Add(bCopy);
            flpListButtons.Controls.Add(bRemove);
            flpListButtons.Name = "flpListButtons";
            // 
            // bAdd
            // 
            resources.ApplyResources(bAdd, "bAdd");
            bAdd.UseVisualStyleBackColor = true;
            bAdd.Name = "bAdd";
            // 
            // bCopy
            // 
            resources.ApplyResources(bCopy, "bCopy");
            bCopy.UseVisualStyleBackColor = true;
            bCopy.Name = "bCopy";
            // 
            // bRemove
            // 
            resources.ApplyResources(bRemove, "bRemove");
            bRemove.UseVisualStyleBackColor = true;
            bRemove.Name = "bRemove";
            // 
            // tvConfigs
            // 
            resources.ApplyResources(tvConfigs, "tvConfigs");
            tvConfigs.FullRowSelect = true;
            tvConfigs.HideSelection = false;
            tvConfigs.ShowLines = false;
            tvConfigs.ShowRootLines = false;
            tvConfigs.Style = ExTreeViewStyle.Light;
            tvConfigs.Name = "tvConfigs";
            // 
            // pDetail
            // 
            resources.ApplyResources(pDetail, "pDetail");
            pDetail.Controls.Add(tlpDetail);
            pDetail.Name = "pDetail";
            // 
            // tlpDetail
            // 
            resources.ApplyResources(tlpDetail, "tlpDetail");
            tlpDetail.Controls.Add(lName, 0, 0);
            tlpDetail.Controls.Add(tbName, 1, 0);
            tlpDetail.Controls.Add(lProgram, 0, 1);
            tlpDetail.Controls.Add(cbProgram, 1, 1);
            tlpDetail.Controls.Add(lRegistry, 0, 2);
            tlpDetail.Controls.Add(cbRegistry, 1, 2);
            tlpDetail.Controls.Add(lBranchInfo, 1, 3);
            tlpDetail.Controls.Add(cboxAdmin, 1, 4);
            tlpDetail.Controls.Add(cboxShared, 1, 5);
            tlpDetail.Controls.Add(lSwitches, 0, 6);
            tlpDetail.Controls.Add(tlpSwitches, 1, 6);
            tlpDetail.Controls.Add(lActivators, 0, 7);
            tlpDetail.Controls.Add(flpActivators, 1, 7);
            tlpDetail.Controls.Add(lExtra, 0, 8);
            tlpDetail.Controls.Add(tbExtra, 1, 8);
            tlpDetail.Controls.Add(lSaveBefore, 0, 9);
            tlpDetail.Controls.Add(cbSaveBefore, 1, 9);
            tlpDetail.Controls.Add(cboxAnalyze, 1, 10);
            tlpDetail.Controls.Add(lWhenRunning, 0, 11);
            tlpDetail.Controls.Add(cbWhenRunning, 1, 11);
            tlpDetail.Controls.Add(lCommandLine, 0, 12);
            tlpDetail.Controls.Add(tbCommandLine, 1, 12);
            tlpDetail.Controls.Add(flpChecks, 0, 13);
            tlpDetail.SetColumnSpan(flpChecks, 2);
            tlpDetail.Controls.Add(flpActions, 1, 14);
            tlpDetail.Name = "tlpDetail";
            // 
            // lName
            // 
            resources.ApplyResources(lName, "lName");
            lName.Name = "lName";
            // 
            // tbName
            // 
            resources.ApplyResources(tbName, "tbName");
            tbName.Name = "tbName";
            // 
            // lProgram
            // 
            resources.ApplyResources(lProgram, "lProgram");
            lProgram.Name = "lProgram";
            // 
            // cbProgram
            // 
            resources.ApplyResources(cbProgram, "cbProgram");
            cbProgram.DropDownStyle = ComboBoxStyle.DropDownList;
            cbProgram.FormattingEnabled = true;
            cbProgram.Name = "cbProgram";
            // 
            // lRegistry
            // 
            resources.ApplyResources(lRegistry, "lRegistry");
            lRegistry.Name = "lRegistry";
            // 
            // cbRegistry
            // 
            resources.ApplyResources(cbRegistry, "cbRegistry");
            cbRegistry.FormattingEnabled = true;
            cbRegistry.Name = "cbRegistry";
            // 
            // lBranchInfo
            // 
            resources.ApplyResources(lBranchInfo, "lBranchInfo");
            lBranchInfo.Name = "lBranchInfo";
            // 
            // cboxAdmin
            // 
            resources.ApplyResources(cboxAdmin, "cboxAdmin");
            cboxAdmin.UseVisualStyleBackColor = true;
            cboxAdmin.Name = "cboxAdmin";
            // 
            // cboxShared
            // 
            resources.ApplyResources(cboxShared, "cboxShared");
            cboxShared.UseVisualStyleBackColor = true;
            cboxShared.Name = "cboxShared";
            // 
            // lSwitches
            // 
            resources.ApplyResources(lSwitches, "lSwitches");
            lSwitches.Name = "lSwitches";
            // 
            // tlpSwitches
            // 
            resources.ApplyResources(tlpSwitches, "tlpSwitches");
            tlpSwitches.Controls.Add(cboxMinimize, 0, 0);
            tlpSwitches.Controls.Add(cboxMultiuse, 0, 1);
            tlpSwitches.Controls.Add(cboxRemote, 0, 2);
            tlpSwitches.Controls.Add(cboxExport, 0, 3);
            tlpSwitches.Controls.Add(cboxExportHlas, 0, 4);
            tlpSwitches.Controls.Add(cboxImport, 0, 5);
            tlpSwitches.Controls.Add(cboxImportDat, 0, 6);
            tlpSwitches.Controls.Add(cboxNoRestore, 0, 7);
            tlpSwitches.Name = "tlpSwitches";
            // 
            // cboxMinimize
            // 
            resources.ApplyResources(cboxMinimize, "cboxMinimize");
            cboxMinimize.UseVisualStyleBackColor = true;
            cboxMinimize.Name = "cboxMinimize";
            // 
            // cboxMultiuse
            // 
            resources.ApplyResources(cboxMultiuse, "cboxMultiuse");
            cboxMultiuse.UseVisualStyleBackColor = true;
            cboxMultiuse.Name = "cboxMultiuse";
            // 
            // cboxRemote
            // 
            resources.ApplyResources(cboxRemote, "cboxRemote");
            cboxRemote.UseVisualStyleBackColor = true;
            cboxRemote.Name = "cboxRemote";
            // 
            // cboxExport
            // 
            resources.ApplyResources(cboxExport, "cboxExport");
            cboxExport.UseVisualStyleBackColor = true;
            cboxExport.Name = "cboxExport";
            // 
            // cboxExportHlas
            // 
            resources.ApplyResources(cboxExportHlas, "cboxExportHlas");
            cboxExportHlas.UseVisualStyleBackColor = true;
            cboxExportHlas.Name = "cboxExportHlas";
            // 
            // cboxImport
            // 
            resources.ApplyResources(cboxImport, "cboxImport");
            cboxImport.UseVisualStyleBackColor = true;
            cboxImport.Name = "cboxImport";
            // 
            // cboxImportDat
            // 
            resources.ApplyResources(cboxImportDat, "cboxImportDat");
            cboxImportDat.UseVisualStyleBackColor = true;
            cboxImportDat.Name = "cboxImportDat";
            // 
            // cboxNoRestore
            // 
            resources.ApplyResources(cboxNoRestore, "cboxNoRestore");
            cboxNoRestore.UseVisualStyleBackColor = true;
            cboxNoRestore.Name = "cboxNoRestore";
            // 
            // lActivators
            // 
            resources.ApplyResources(lActivators, "lActivators");
            lActivators.Name = "lActivators";
            // 
            // flpActivators
            // 
            resources.ApplyResources(flpActivators, "flpActivators");
            flpActivators.Name = "flpActivators";
            // 
            // lExtra
            // 
            resources.ApplyResources(lExtra, "lExtra");
            lExtra.Name = "lExtra";
            // 
            // tbExtra
            // 
            resources.ApplyResources(tbExtra, "tbExtra");
            tbExtra.Name = "tbExtra";
            // 
            // lSaveBefore
            // 
            resources.ApplyResources(lSaveBefore, "lSaveBefore");
            lSaveBefore.Name = "lSaveBefore";
            // 
            // cbSaveBefore
            // 
            resources.ApplyResources(cbSaveBefore, "cbSaveBefore");
            cbSaveBefore.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSaveBefore.FormattingEnabled = true;
            cbSaveBefore.Name = "cbSaveBefore";
            // 
            // cboxAnalyze
            // 
            resources.ApplyResources(cboxAnalyze, "cboxAnalyze");
            cboxAnalyze.UseVisualStyleBackColor = true;
            cboxAnalyze.Name = "cboxAnalyze";
            // 
            // lWhenRunning
            // 
            resources.ApplyResources(lWhenRunning, "lWhenRunning");
            lWhenRunning.Name = "lWhenRunning";
            // 
            // cbWhenRunning
            // 
            resources.ApplyResources(cbWhenRunning, "cbWhenRunning");
            cbWhenRunning.DropDownStyle = ComboBoxStyle.DropDownList;
            cbWhenRunning.FormattingEnabled = true;
            cbWhenRunning.Name = "cbWhenRunning";
            // 
            // lCommandLine
            // 
            resources.ApplyResources(lCommandLine, "lCommandLine");
            lCommandLine.Name = "lCommandLine";
            // 
            // tbCommandLine
            // 
            resources.ApplyResources(tbCommandLine, "tbCommandLine");
            tbCommandLine.ReadOnly = true;
            tbCommandLine.Name = "tbCommandLine";
            // 
            // flpChecks
            // 
            resources.ApplyResources(flpChecks, "flpChecks");
            flpChecks.WrapContents = false;
            flpChecks.Name = "flpChecks";
            // 
            // flpActions
            // 
            resources.ApplyResources(flpActions, "flpActions");
            flpActions.Controls.Add(bInissSettings);
            flpActions.Controls.Add(bLogs);
            flpActions.Name = "flpActions";
            // 
            // bInissSettings
            // 
            resources.ApplyResources(bInissSettings, "bInissSettings");
            bInissSettings.UseVisualStyleBackColor = true;
            bInissSettings.Name = "bInissSettings";
            // 
            // bLogs
            // 
            resources.ApplyResources(bLogs, "bLogs");
            bLogs.UseVisualStyleBackColor = true;
            bLogs.Name = "bLogs";
            // 
            // flpButtons
            // 
            resources.ApplyResources(flpButtons, "flpButtons");
            flpButtons.Controls.Add(bCancel);
            flpButtons.Controls.Add(bOK);
            flpButtons.Controls.Add(bRun);
            flpButtons.Name = "flpButtons";
            // 
            // bCancel
            // 
            resources.ApplyResources(bCancel, "bCancel");
            bCancel.DialogResult = DialogResult.Cancel;
            bCancel.UseVisualStyleBackColor = true;
            bCancel.Name = "bCancel";
            // 
            // bOK
            // 
            resources.ApplyResources(bOK, "bOK");
            bOK.UseVisualStyleBackColor = true;
            bOK.Name = "bOK";
            // 
            // bRun
            // 
            resources.ApplyResources(bRun, "bRun");
            bRun.UseVisualStyleBackColor = true;
            bRun.Name = "bRun";
            // 
            // FRunConfigurations
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            AcceptButton = bOK;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            Name = "FRunConfigurations";
            flpButtons.ResumeLayout(false);
            flpButtons.PerformLayout();
            flpActions.ResumeLayout(false);
            flpActions.PerformLayout();
            flpChecks.ResumeLayout(false);
            flpChecks.PerformLayout();
            flpActivators.ResumeLayout(false);
            flpActivators.PerformLayout();
            tlpSwitches.ResumeLayout(false);
            tlpSwitches.PerformLayout();
            tlpDetail.ResumeLayout(false);
            tlpDetail.PerformLayout();
            pDetail.ResumeLayout(false);
            pDetail.PerformLayout();
            flpListButtons.ResumeLayout(false);
            flpListButtons.PerformLayout();
            tlpLeft.ResumeLayout(false);
            tlpLeft.PerformLayout();
            scMain.Panel1.ResumeLayout(false);
            scMain.Panel1.PerformLayout();
            scMain.Panel2.ResumeLayout(false);
            scMain.Panel2.PerformLayout();
            ((ISupportInitialize)scMain).EndInit();
            scMain.ResumeLayout(false);
            scMain.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private SplitContainer scMain;
        private TableLayoutPanel tlpLeft;
        private FlowLayoutPanel flpListButtons;
        private ExButton bAdd;
        private ExButton bCopy;
        private ExButton bRemove;
        private ExTreeView tvConfigs;
        private Panel pDetail;
        private TableLayoutPanel tlpDetail;
        private Label lName;
        private ExTextBox tbName;
        private Label lProgram;
        private ExComboBox cbProgram;
        private Label lRegistry;
        private ExComboBox cbRegistry;
        private Label lBranchInfo;
        private ExCheckBox cboxAdmin;
        private ExCheckBox cboxShared;
        private Label lSwitches;
        private TableLayoutPanel tlpSwitches;
        private ExCheckBox cboxMinimize;
        private ExCheckBox cboxMultiuse;
        private ExCheckBox cboxRemote;
        private ExCheckBox cboxExport;
        private ExCheckBox cboxExportHlas;
        private ExCheckBox cboxImport;
        private ExCheckBox cboxImportDat;
        private ExCheckBox cboxNoRestore;
        private Label lActivators;
        private FlowLayoutPanel flpActivators;
        private Label lExtra;
        private ExTextBox tbExtra;
        private Label lSaveBefore;
        private ExComboBox cbSaveBefore;
        private ExCheckBox cboxAnalyze;
        private Label lWhenRunning;
        private ExComboBox cbWhenRunning;
        private Label lCommandLine;
        private ExTextBox tbCommandLine;
        private FlowLayoutPanel flpChecks;
        private FlowLayoutPanel flpActions;
        private ExButton bInissSettings;
        private ExButton bLogs;
        private FlowLayoutPanel flpButtons;
        private ExButton bCancel;
        private ExButton bOK;
        private ExButton bRun;
    }
}
