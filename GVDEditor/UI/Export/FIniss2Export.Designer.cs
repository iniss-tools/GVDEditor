using ExControls;

namespace GVDEditor.UI.Export
{
    partial class FIniss2Export
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
            var resources = new ComponentResourceManager(typeof(FIniss2Export));
            tlpMain = new TableLayoutPanel();
            lStep = new Label();
            lHint = new Label();
            pnlPages = new Panel();
            tlpGrafikons = new TableLayoutPanel();
            lGrafikons = new Label();
            clbGrafikons = new ExCheckedListBox();
            tlpTarget = new TableLayoutPanel();
            lInstallation = new Label();
            tbInstallation = new ExTextBox();
            bBrowse = new ExButton();
            lInstallationInfo = new Label();
            lGroup = new Label();
            tbGroup = new ExTextBox();
            lStations = new Label();
            dgvStations = new DataGridView();
            colStationGrafikon = new DataGridViewTextBoxColumn();
            colStationDirectory = new DataGridViewTextBoxColumn();
            tlpReports = new TableLayoutPanel();
            lTemplate = new Label();
            cbTemplate = new ExComboBox();
            lTemplateInfo = new Label();
            lReports = new Label();
            dgvReports = new DataGridView();
            colReportType = new DataGridViewTextBoxColumn();
            colReport = new DataGridViewExComboBoxColumn();
            tlpLines = new TableLayoutPanel();
            lConfiguration = new Label();
            cbConfiguration = new ExComboBox();
            lLinesInfo = new Label();
            lFonts = new Label();
            dgvFonts = new DataGridView();
            colFontCatalog = new DataGridViewTextBoxColumn();
            colFontFile = new DataGridViewExComboBoxColumn();
            bAddFont = new ExButton();
            tlpSummary = new TableLayoutPanel();
            lSummary = new Label();
            dgvWarnings = new DataGridView();
            colWarning = new DataGridViewTextBoxColumn();
            colDetail = new DataGridViewTextBoxColumn();
            flpButtons = new FlowLayoutPanel();
            bCancel = new ExButton();
            bNext = new ExButton();
            bBack = new ExButton();
            ((ISupportInitialize)dgvStations).BeginInit();
            ((ISupportInitialize)dgvReports).BeginInit();
            ((ISupportInitialize)dgvFonts).BeginInit();
            ((ISupportInitialize)dgvWarnings).BeginInit();
            tlpMain.SuspendLayout();
            pnlPages.SuspendLayout();
            tlpGrafikons.SuspendLayout();
            tlpTarget.SuspendLayout();
            tlpReports.SuspendLayout();
            tlpLines.SuspendLayout();
            tlpSummary.SuspendLayout();
            flpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lStep, 0, 0);
            tlpMain.Controls.Add(lHint, 0, 1);
            tlpMain.Controls.Add(pnlPages, 0, 2);
            tlpMain.Controls.Add(flpButtons, 0, 3);
            tlpMain.Name = "tlpMain";
            // 
            // lStep
            // 
            resources.ApplyResources(lStep, "lStep");
            lStep.Name = "lStep";
            // 
            // lHint
            // 
            resources.ApplyResources(lHint, "lHint");
            lHint.Name = "lHint";
            // 
            // pnlPages
            // 
            resources.ApplyResources(pnlPages, "pnlPages");
            pnlPages.Controls.Add(tlpGrafikons);
            pnlPages.Controls.Add(tlpTarget);
            pnlPages.Controls.Add(tlpReports);
            pnlPages.Controls.Add(tlpLines);
            pnlPages.Controls.Add(tlpSummary);
            pnlPages.Name = "pnlPages";
            // 
            // tlpGrafikons
            // 
            resources.ApplyResources(tlpGrafikons, "tlpGrafikons");
            tlpGrafikons.Controls.Add(lGrafikons, 0, 0);
            tlpGrafikons.Controls.Add(clbGrafikons, 0, 1);
            tlpGrafikons.Name = "tlpGrafikons";
            // 
            // lGrafikons
            // 
            resources.ApplyResources(lGrafikons, "lGrafikons");
            lGrafikons.Name = "lGrafikons";
            // 
            // clbGrafikons
            // 
            resources.ApplyResources(clbGrafikons, "clbGrafikons");
            clbGrafikons.CheckOnClick = true;
            clbGrafikons.FormattingEnabled = true;
            clbGrafikons.Name = "clbGrafikons";
            // 
            // tlpTarget
            // 
            resources.ApplyResources(tlpTarget, "tlpTarget");
            tlpTarget.Controls.Add(lInstallation, 0, 0);
            tlpTarget.Controls.Add(tbInstallation, 1, 0);
            tlpTarget.Controls.Add(bBrowse, 2, 0);
            tlpTarget.Controls.Add(lInstallationInfo, 1, 1);
            tlpTarget.SetColumnSpan(lInstallationInfo, 2);
            tlpTarget.Controls.Add(lGroup, 0, 2);
            tlpTarget.Controls.Add(tbGroup, 1, 2);
            tlpTarget.Controls.Add(lStations, 0, 3);
            tlpTarget.SetColumnSpan(lStations, 3);
            tlpTarget.Controls.Add(dgvStations, 0, 4);
            tlpTarget.SetColumnSpan(dgvStations, 3);
            tlpTarget.Name = "tlpTarget";
            // 
            // lInstallation
            // 
            resources.ApplyResources(lInstallation, "lInstallation");
            lInstallation.Name = "lInstallation";
            // 
            // tbInstallation
            // 
            resources.ApplyResources(tbInstallation, "tbInstallation");
            tbInstallation.Name = "tbInstallation";
            // 
            // bBrowse
            // 
            resources.ApplyResources(bBrowse, "bBrowse");
            bBrowse.UseVisualStyleBackColor = true;
            bBrowse.Name = "bBrowse";
            // 
            // lInstallationInfo
            // 
            resources.ApplyResources(lInstallationInfo, "lInstallationInfo");
            lInstallationInfo.Name = "lInstallationInfo";
            // 
            // lGroup
            // 
            resources.ApplyResources(lGroup, "lGroup");
            lGroup.Name = "lGroup";
            // 
            // tbGroup
            // 
            resources.ApplyResources(tbGroup, "tbGroup");
            tbGroup.Name = "tbGroup";
            // 
            // lStations
            // 
            resources.ApplyResources(lStations, "lStations");
            lStations.Name = "lStations";
            // 
            // dgvStations
            // 
            resources.ApplyResources(dgvStations, "dgvStations");
            dgvStations.Columns.AddRange(new DataGridViewColumn[] { colStationGrafikon, colStationDirectory });
            dgvStations.AllowUserToAddRows = false;
            dgvStations.AllowUserToDeleteRows = false;
            dgvStations.AllowUserToResizeRows = false;
            dgvStations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStations.RowHeadersVisible = false;
            dgvStations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStations.Name = "dgvStations";
            // 
            // colStationGrafikon
            // 
            resources.ApplyResources(colStationGrafikon, "colStationGrafikon");
            colStationGrafikon.ReadOnly = true;
            colStationGrafikon.SortMode = DataGridViewColumnSortMode.NotSortable;
            colStationGrafikon.Name = "colStationGrafikon";
            // 
            // colStationDirectory
            // 
            resources.ApplyResources(colStationDirectory, "colStationDirectory");
            colStationDirectory.SortMode = DataGridViewColumnSortMode.NotSortable;
            colStationDirectory.Name = "colStationDirectory";
            // 
            // tlpReports
            // 
            resources.ApplyResources(tlpReports, "tlpReports");
            tlpReports.Controls.Add(lTemplate, 0, 0);
            tlpReports.Controls.Add(cbTemplate, 1, 0);
            tlpReports.Controls.Add(lTemplateInfo, 1, 1);
            tlpReports.Controls.Add(lReports, 0, 2);
            tlpReports.SetColumnSpan(lReports, 2);
            tlpReports.Controls.Add(dgvReports, 0, 3);
            tlpReports.SetColumnSpan(dgvReports, 2);
            tlpReports.Name = "tlpReports";
            // 
            // lTemplate
            // 
            resources.ApplyResources(lTemplate, "lTemplate");
            lTemplate.Name = "lTemplate";
            // 
            // cbTemplate
            // 
            resources.ApplyResources(cbTemplate, "cbTemplate");
            cbTemplate.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTemplate.FormattingEnabled = true;
            cbTemplate.Name = "cbTemplate";
            // 
            // lTemplateInfo
            // 
            resources.ApplyResources(lTemplateInfo, "lTemplateInfo");
            lTemplateInfo.Name = "lTemplateInfo";
            // 
            // lReports
            // 
            resources.ApplyResources(lReports, "lReports");
            lReports.Name = "lReports";
            // 
            // dgvReports
            // 
            resources.ApplyResources(dgvReports, "dgvReports");
            dgvReports.Columns.AddRange(new DataGridViewColumn[] { colReportType, colReport });
            dgvReports.AllowUserToAddRows = false;
            dgvReports.AllowUserToDeleteRows = false;
            dgvReports.AllowUserToResizeRows = false;
            dgvReports.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReports.RowHeadersVisible = false;
            dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReports.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvReports.Name = "dgvReports";
            // 
            // colReportType
            // 
            resources.ApplyResources(colReportType, "colReportType");
            colReportType.ReadOnly = true;
            colReportType.SortMode = DataGridViewColumnSortMode.NotSortable;
            colReportType.Name = "colReportType";
            // 
            // colReport
            // 
            resources.ApplyResources(colReport, "colReport");
            colReport.SortMode = DataGridViewColumnSortMode.NotSortable;
            colReport.Name = "colReport";
            // 
            // tlpLines
            // 
            resources.ApplyResources(tlpLines, "tlpLines");
            tlpLines.Controls.Add(lConfiguration, 0, 0);
            tlpLines.Controls.Add(cbConfiguration, 1, 0);
            tlpLines.Controls.Add(lLinesInfo, 1, 1);
            tlpLines.Controls.Add(lFonts, 0, 2);
            tlpLines.SetColumnSpan(lFonts, 2);
            tlpLines.Controls.Add(dgvFonts, 0, 3);
            tlpLines.SetColumnSpan(dgvFonts, 2);
            tlpLines.Controls.Add(bAddFont, 0, 4);
            tlpLines.SetColumnSpan(bAddFont, 2);
            tlpLines.Name = "tlpLines";
            // 
            // lConfiguration
            // 
            resources.ApplyResources(lConfiguration, "lConfiguration");
            lConfiguration.Name = "lConfiguration";
            // 
            // cbConfiguration
            // 
            resources.ApplyResources(cbConfiguration, "cbConfiguration");
            cbConfiguration.DropDownStyle = ComboBoxStyle.DropDownList;
            cbConfiguration.FormattingEnabled = true;
            cbConfiguration.Name = "cbConfiguration";
            // 
            // lLinesInfo
            // 
            resources.ApplyResources(lLinesInfo, "lLinesInfo");
            lLinesInfo.Name = "lLinesInfo";
            // 
            // lFonts
            // 
            resources.ApplyResources(lFonts, "lFonts");
            lFonts.Name = "lFonts";
            // 
            // dgvFonts
            // 
            resources.ApplyResources(dgvFonts, "dgvFonts");
            dgvFonts.Columns.AddRange(new DataGridViewColumn[] { colFontCatalog, colFontFile });
            dgvFonts.AllowUserToAddRows = false;
            dgvFonts.AllowUserToDeleteRows = false;
            dgvFonts.AllowUserToResizeRows = false;
            dgvFonts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFonts.RowHeadersVisible = false;
            dgvFonts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFonts.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvFonts.Name = "dgvFonts";
            // 
            // colFontCatalog
            // 
            resources.ApplyResources(colFontCatalog, "colFontCatalog");
            colFontCatalog.ReadOnly = true;
            colFontCatalog.SortMode = DataGridViewColumnSortMode.NotSortable;
            colFontCatalog.Name = "colFontCatalog";
            // 
            // colFontFile
            // 
            resources.ApplyResources(colFontFile, "colFontFile");
            colFontFile.SortMode = DataGridViewColumnSortMode.NotSortable;
            colFontFile.Name = "colFontFile";
            // 
            // bAddFont
            // 
            resources.ApplyResources(bAddFont, "bAddFont");
            bAddFont.UseVisualStyleBackColor = true;
            bAddFont.Name = "bAddFont";
            // 
            // tlpSummary
            // 
            resources.ApplyResources(tlpSummary, "tlpSummary");
            tlpSummary.Controls.Add(lSummary, 0, 0);
            tlpSummary.Controls.Add(dgvWarnings, 0, 1);
            tlpSummary.Name = "tlpSummary";
            // 
            // lSummary
            // 
            resources.ApplyResources(lSummary, "lSummary");
            lSummary.Name = "lSummary";
            // 
            // dgvWarnings
            // 
            resources.ApplyResources(dgvWarnings, "dgvWarnings");
            dgvWarnings.Columns.AddRange(new DataGridViewColumn[] { colWarning, colDetail });
            dgvWarnings.AllowUserToAddRows = false;
            dgvWarnings.AllowUserToDeleteRows = false;
            dgvWarnings.AllowUserToResizeRows = false;
            dgvWarnings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvWarnings.RowHeadersVisible = false;
            dgvWarnings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvWarnings.Name = "dgvWarnings";
            // 
            // colWarning
            // 
            resources.ApplyResources(colWarning, "colWarning");
            colWarning.ReadOnly = true;
            colWarning.SortMode = DataGridViewColumnSortMode.NotSortable;
            colWarning.Name = "colWarning";
            // 
            // colDetail
            // 
            resources.ApplyResources(colDetail, "colDetail");
            colDetail.ReadOnly = true;
            colDetail.SortMode = DataGridViewColumnSortMode.NotSortable;
            colDetail.Name = "colDetail";
            // 
            // flpButtons
            // 
            resources.ApplyResources(flpButtons, "flpButtons");
            flpButtons.Controls.Add(bCancel);
            flpButtons.Controls.Add(bNext);
            flpButtons.Controls.Add(bBack);
            flpButtons.Name = "flpButtons";
            // 
            // bCancel
            // 
            resources.ApplyResources(bCancel, "bCancel");
            bCancel.DialogResult = DialogResult.Cancel;
            bCancel.UseVisualStyleBackColor = true;
            bCancel.Name = "bCancel";
            // 
            // bNext
            // 
            resources.ApplyResources(bNext, "bNext");
            bNext.UseVisualStyleBackColor = true;
            bNext.Name = "bNext";
            // 
            // bBack
            // 
            resources.ApplyResources(bBack, "bBack");
            bBack.UseVisualStyleBackColor = true;
            bBack.Name = "bBack";
            // 
            // FIniss2Export
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            CancelButton = bCancel;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            Name = "FIniss2Export";
            flpButtons.ResumeLayout(false);
            flpButtons.PerformLayout();
            tlpSummary.ResumeLayout(false);
            tlpSummary.PerformLayout();
            tlpLines.ResumeLayout(false);
            tlpLines.PerformLayout();
            tlpReports.ResumeLayout(false);
            tlpReports.PerformLayout();
            tlpTarget.ResumeLayout(false);
            tlpTarget.PerformLayout();
            tlpGrafikons.ResumeLayout(false);
            tlpGrafikons.PerformLayout();
            pnlPages.ResumeLayout(false);
            pnlPages.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ((ISupportInitialize)dgvStations).EndInit();
            ((ISupportInitialize)dgvReports).EndInit();
            ((ISupportInitialize)dgvFonts).EndInit();
            ((ISupportInitialize)dgvWarnings).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lStep;
        private Label lHint;
        private Panel pnlPages;
        private TableLayoutPanel tlpGrafikons;
        private Label lGrafikons;
        private ExCheckedListBox clbGrafikons;
        private TableLayoutPanel tlpTarget;
        private Label lInstallation;
        private ExTextBox tbInstallation;
        private ExButton bBrowse;
        private Label lInstallationInfo;
        private Label lGroup;
        private ExTextBox tbGroup;
        private Label lStations;
        private DataGridView dgvStations;
        private DataGridViewTextBoxColumn colStationGrafikon;
        private DataGridViewTextBoxColumn colStationDirectory;
        private TableLayoutPanel tlpReports;
        private Label lTemplate;
        private ExComboBox cbTemplate;
        private Label lTemplateInfo;
        private Label lReports;
        private DataGridView dgvReports;
        private DataGridViewTextBoxColumn colReportType;
        private DataGridViewExComboBoxColumn colReport;
        private TableLayoutPanel tlpLines;
        private Label lConfiguration;
        private ExComboBox cbConfiguration;
        private Label lLinesInfo;
        private Label lFonts;
        private DataGridView dgvFonts;
        private DataGridViewTextBoxColumn colFontCatalog;
        private DataGridViewExComboBoxColumn colFontFile;
        private ExButton bAddFont;
        private TableLayoutPanel tlpSummary;
        private Label lSummary;
        private DataGridView dgvWarnings;
        private DataGridViewTextBoxColumn colWarning;
        private DataGridViewTextBoxColumn colDetail;
        private FlowLayoutPanel flpButtons;
        private ExButton bCancel;
        private ExButton bNext;
        private ExButton bBack;
    }
}