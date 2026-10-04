using ExControls;

namespace GVDEditor.UI.InissSettings
{
    partial class FInissSettings
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
            var resources = new ComponentResourceManager(typeof(FInissSettings));
            tlpMain = new TableLayoutPanel();
            flpTop = new FlowLayoutPanel();
            lRunConfig = new Label();
            cbRunConfig = new ExComboBox();
            lProgram = new Label();
            cbProgram = new ExComboBox();
            lConfig = new Label();
            cbConfig = new ExComboBox();
            lRunMode = new Label();
            cbRunMode = new ExComboBox();
            bReload = new ExButton();
            lInfo = new Label();
            scMain = new SplitContainer();
            tlpLeft = new TableLayoutPanel();
            tbSearch = new ExTextBox();
            tvSections = new ExTreeView();
            scRight = new SplitContainer();
            tlpGrid = new TableLayoutPanel();
            lSection = new Label();
            flpFilter = new FlowLayoutPanel();
            cboxChangedOnly = new ExCheckBox();
            cboxNotRead = new ExCheckBox();
            bAddLine = new ExButton();
            bRemoveLine = new ExButton();
            dgvValues = new DataGridView();
            cState = new DataGridViewImageColumn();
            cName = new DataGridViewTextBoxColumn();
            cValue = new DataGridViewTextBoxColumn();
            cSource = new DataGridViewTextBoxColumn();
            cDefault = new DataGridViewTextBoxColumn();
            pDetail = new Panel();
            tlpBottom = new TableLayoutPanel();
            bTools = new ExButton();
            flpButtons = new FlowLayoutPanel();
            bClose = new ExButton();
            bSaveRestart = new ExButton();
            bSave = new ExButton();
            bDiscard = new ExButton();
            lChanges = new Label();
            ((ISupportInitialize)scMain).BeginInit();
            scMain.Panel1.SuspendLayout();
            scMain.Panel2.SuspendLayout();
            ((ISupportInitialize)scRight).BeginInit();
            scRight.Panel1.SuspendLayout();
            scRight.Panel2.SuspendLayout();
            ((ISupportInitialize)dgvValues).BeginInit();
            tlpMain.SuspendLayout();
            flpTop.SuspendLayout();
            scMain.SuspendLayout();
            tlpLeft.SuspendLayout();
            scRight.SuspendLayout();
            tlpGrid.SuspendLayout();
            flpFilter.SuspendLayout();
            pDetail.SuspendLayout();
            tlpBottom.SuspendLayout();
            flpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(flpTop, 0, 0);
            tlpMain.Controls.Add(lInfo, 0, 1);
            tlpMain.Controls.Add(scMain, 0, 2);
            tlpMain.Controls.Add(tlpBottom, 0, 3);
            tlpMain.Name = "tlpMain";
            // 
            // flpTop
            // 
            resources.ApplyResources(flpTop, "flpTop");
            flpTop.Controls.Add(lRunConfig);
            flpTop.Controls.Add(cbRunConfig);
            flpTop.Controls.Add(lProgram);
            flpTop.Controls.Add(cbProgram);
            flpTop.Controls.Add(lConfig);
            flpTop.Controls.Add(cbConfig);
            flpTop.Controls.Add(lRunMode);
            flpTop.Controls.Add(cbRunMode);
            flpTop.Controls.Add(bReload);
            flpTop.Name = "flpTop";
            // 
            // lRunConfig
            // 
            resources.ApplyResources(lRunConfig, "lRunConfig");
            lRunConfig.Name = "lRunConfig";
            // 
            // cbRunConfig
            // 
            resources.ApplyResources(cbRunConfig, "cbRunConfig");
            cbRunConfig.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRunConfig.FormattingEnabled = true;
            cbRunConfig.Name = "cbRunConfig";
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
            // lConfig
            // 
            resources.ApplyResources(lConfig, "lConfig");
            lConfig.Name = "lConfig";
            // 
            // cbConfig
            // 
            resources.ApplyResources(cbConfig, "cbConfig");
            cbConfig.DropDownStyle = ComboBoxStyle.DropDownList;
            cbConfig.FormattingEnabled = true;
            cbConfig.Name = "cbConfig";
            // 
            // lRunMode
            // 
            resources.ApplyResources(lRunMode, "lRunMode");
            lRunMode.Name = "lRunMode";
            // 
            // cbRunMode
            // 
            resources.ApplyResources(cbRunMode, "cbRunMode");
            cbRunMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRunMode.FormattingEnabled = true;
            cbRunMode.Name = "cbRunMode";
            // 
            // bReload
            // 
            resources.ApplyResources(bReload, "bReload");
            bReload.UseVisualStyleBackColor = true;
            bReload.Name = "bReload";
            // 
            // lInfo
            // 
            resources.ApplyResources(lInfo, "lInfo");
            lInfo.Name = "lInfo";
            // 
            // scMain
            // 
            resources.ApplyResources(scMain, "scMain");
            scMain.Panel1.Controls.Add(tlpLeft);
            scMain.Panel2.Controls.Add(scRight);
            scMain.Name = "scMain";
            // 
            // tlpLeft
            // 
            resources.ApplyResources(tlpLeft, "tlpLeft");
            tlpLeft.Controls.Add(tbSearch, 0, 0);
            tlpLeft.Controls.Add(tvSections, 0, 1);
            tlpLeft.Name = "tlpLeft";
            // 
            // tbSearch
            // 
            resources.ApplyResources(tbSearch, "tbSearch");
            tbSearch.Name = "tbSearch";
            // 
            // tvSections
            // 
            resources.ApplyResources(tvSections, "tvSections");
            tvSections.FullRowSelect = true;
            tvSections.HideSelection = false;
            tvSections.ShowLines = false;
            tvSections.Style = ExTreeViewStyle.Light;
            tvSections.Name = "tvSections";
            // 
            // scRight
            // 
            resources.ApplyResources(scRight, "scRight");
            scRight.Panel1.Controls.Add(tlpGrid);
            scRight.Panel2.Controls.Add(pDetail);
            scRight.Name = "scRight";
            // 
            // tlpGrid
            // 
            resources.ApplyResources(tlpGrid, "tlpGrid");
            tlpGrid.Controls.Add(lSection, 0, 0);
            tlpGrid.Controls.Add(flpFilter, 0, 1);
            tlpGrid.Controls.Add(dgvValues, 0, 2);
            tlpGrid.Name = "tlpGrid";
            // 
            // lSection
            // 
            resources.ApplyResources(lSection, "lSection");
            lSection.Name = "lSection";
            // 
            // flpFilter
            // 
            resources.ApplyResources(flpFilter, "flpFilter");
            flpFilter.Controls.Add(cboxChangedOnly);
            flpFilter.Controls.Add(cboxNotRead);
            flpFilter.Controls.Add(bAddLine);
            flpFilter.Controls.Add(bRemoveLine);
            flpFilter.Name = "flpFilter";
            // 
            // cboxChangedOnly
            // 
            resources.ApplyResources(cboxChangedOnly, "cboxChangedOnly");
            cboxChangedOnly.UseVisualStyleBackColor = true;
            cboxChangedOnly.Name = "cboxChangedOnly";
            // 
            // cboxNotRead
            // 
            resources.ApplyResources(cboxNotRead, "cboxNotRead");
            cboxNotRead.UseVisualStyleBackColor = true;
            cboxNotRead.Name = "cboxNotRead";
            // 
            // bAddLine
            // 
            resources.ApplyResources(bAddLine, "bAddLine");
            bAddLine.UseVisualStyleBackColor = true;
            bAddLine.Name = "bAddLine";
            // 
            // bRemoveLine
            // 
            resources.ApplyResources(bRemoveLine, "bRemoveLine");
            bRemoveLine.UseVisualStyleBackColor = true;
            bRemoveLine.Name = "bRemoveLine";
            // 
            // dgvValues
            // 
            resources.ApplyResources(dgvValues, "dgvValues");
            dgvValues.Columns.AddRange(new DataGridViewColumn[] { cState, cName, cValue, cSource, cDefault });
            dgvValues.AllowUserToAddRows = false;
            dgvValues.AllowUserToDeleteRows = false;
            dgvValues.AllowUserToResizeRows = false;
            dgvValues.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvValues.MultiSelect = false;
            dgvValues.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgvValues.RowHeadersVisible = false;
            dgvValues.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvValues.Name = "dgvValues";
            // 
            // cState
            // 
            resources.ApplyResources(cState, "cState");
            cState.ImageLayout = DataGridViewImageCellLayout.Zoom;
            cState.ReadOnly = true;
            cState.Resizable = DataGridViewTriState.False;
            cState.SortMode = DataGridViewColumnSortMode.NotSortable;
            cState.Name = "cState";
            // 
            // cName
            // 
            resources.ApplyResources(cName, "cName");
            cName.ReadOnly = true;
            cName.SortMode = DataGridViewColumnSortMode.NotSortable;
            cName.Name = "cName";
            // 
            // cValue
            // 
            resources.ApplyResources(cValue, "cValue");
            cValue.SortMode = DataGridViewColumnSortMode.NotSortable;
            cValue.Name = "cValue";
            // 
            // cSource
            // 
            resources.ApplyResources(cSource, "cSource");
            cSource.ReadOnly = true;
            cSource.SortMode = DataGridViewColumnSortMode.NotSortable;
            cSource.Name = "cSource";
            // 
            // cDefault
            // 
            resources.ApplyResources(cDefault, "cDefault");
            cDefault.ReadOnly = true;
            cDefault.SortMode = DataGridViewColumnSortMode.NotSortable;
            cDefault.Name = "cDefault";
            // 
            // pDetail
            // 
            resources.ApplyResources(pDetail, "pDetail");
            pDetail.Name = "pDetail";
            // 
            // tlpBottom
            // 
            resources.ApplyResources(tlpBottom, "tlpBottom");
            tlpBottom.Controls.Add(bTools, 0, 0);
            tlpBottom.Controls.Add(flpButtons, 1, 0);
            tlpBottom.Name = "tlpBottom";
            // 
            // bTools
            // 
            resources.ApplyResources(bTools, "bTools");
            bTools.UseVisualStyleBackColor = true;
            bTools.Name = "bTools";
            // 
            // flpButtons
            // 
            resources.ApplyResources(flpButtons, "flpButtons");
            flpButtons.Controls.Add(bClose);
            flpButtons.Controls.Add(bSaveRestart);
            flpButtons.Controls.Add(bSave);
            flpButtons.Controls.Add(bDiscard);
            flpButtons.Controls.Add(lChanges);
            flpButtons.WrapContents = false;
            flpButtons.Name = "flpButtons";
            // 
            // bClose
            // 
            resources.ApplyResources(bClose, "bClose");
            bClose.UseVisualStyleBackColor = true;
            bClose.Name = "bClose";
            // 
            // bSaveRestart
            // 
            resources.ApplyResources(bSaveRestart, "bSaveRestart");
            bSaveRestart.UseVisualStyleBackColor = true;
            bSaveRestart.Name = "bSaveRestart";
            // 
            // bSave
            // 
            resources.ApplyResources(bSave, "bSave");
            bSave.UseVisualStyleBackColor = true;
            bSave.Name = "bSave";
            // 
            // bDiscard
            // 
            resources.ApplyResources(bDiscard, "bDiscard");
            bDiscard.UseVisualStyleBackColor = true;
            bDiscard.Name = "bDiscard";
            // 
            // lChanges
            // 
            resources.ApplyResources(lChanges, "lChanges");
            lChanges.Name = "lChanges";
            // 
            // FInissSettings
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            Name = "FInissSettings";
            flpButtons.ResumeLayout(false);
            flpButtons.PerformLayout();
            tlpBottom.ResumeLayout(false);
            tlpBottom.PerformLayout();
            pDetail.ResumeLayout(false);
            pDetail.PerformLayout();
            flpFilter.ResumeLayout(false);
            flpFilter.PerformLayout();
            tlpGrid.ResumeLayout(false);
            tlpGrid.PerformLayout();
            scRight.Panel1.ResumeLayout(false);
            scRight.Panel1.PerformLayout();
            scRight.Panel2.ResumeLayout(false);
            scRight.Panel2.PerformLayout();
            ((ISupportInitialize)scRight).EndInit();
            scRight.ResumeLayout(false);
            scRight.PerformLayout();
            tlpLeft.ResumeLayout(false);
            tlpLeft.PerformLayout();
            scMain.Panel1.ResumeLayout(false);
            scMain.Panel1.PerformLayout();
            scMain.Panel2.ResumeLayout(false);
            scMain.Panel2.PerformLayout();
            ((ISupportInitialize)scMain).EndInit();
            scMain.ResumeLayout(false);
            scMain.PerformLayout();
            flpTop.ResumeLayout(false);
            flpTop.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ((ISupportInitialize)dgvValues).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private FlowLayoutPanel flpTop;
        private Label lRunConfig;
        private ExComboBox cbRunConfig;
        private Label lProgram;
        private ExComboBox cbProgram;
        private Label lConfig;
        private ExComboBox cbConfig;
        private Label lRunMode;
        private ExComboBox cbRunMode;
        private ExButton bReload;
        private Label lInfo;
        private SplitContainer scMain;
        private TableLayoutPanel tlpLeft;
        private ExTextBox tbSearch;
        private ExTreeView tvSections;
        private SplitContainer scRight;
        private TableLayoutPanel tlpGrid;
        private Label lSection;
        private FlowLayoutPanel flpFilter;
        private ExCheckBox cboxChangedOnly;
        private ExCheckBox cboxNotRead;
        private ExButton bAddLine;
        private ExButton bRemoveLine;
        private DataGridView dgvValues;
        private DataGridViewImageColumn cState;
        private DataGridViewTextBoxColumn cName;
        private DataGridViewTextBoxColumn cValue;
        private DataGridViewTextBoxColumn cSource;
        private DataGridViewTextBoxColumn cDefault;
        private Panel pDetail;
        private TableLayoutPanel tlpBottom;
        private ExButton bTools;
        private FlowLayoutPanel flpButtons;
        private ExButton bClose;
        private ExButton bSaveRestart;
        private ExButton bSave;
        private ExButton bDiscard;
        private Label lChanges;
    }
}
