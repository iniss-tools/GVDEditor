using ExControls;

namespace GVDEditor.UI.InissSettings
{
    partial class FCompareSettings
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
            var resources = new ComponentResourceManager(typeof(FCompareSettings));
            tlpMain = new TableLayoutPanel();
            flpOther = new FlowLayoutPanel();
            lOther = new Label();
            cbOther = new ExComboBox();
            bFile = new ExButton();
            lInfo = new Label();
            flpFilter = new FlowLayoutPanel();
            cboxDifferentOnly = new ExCheckBox();
            cboxOtherExplicit = new ExCheckBox();
            cboxHideApp = new ExCheckBox();
            dgvDiff = new DataGridView();
            cTake = new DataGridViewCheckBoxColumn();
            cKey = new DataGridViewTextBoxColumn();
            cCurrent = new DataGridViewTextBoxColumn();
            cCurrentSource = new DataGridViewTextBoxColumn();
            cOther = new DataGridViewTextBoxColumn();
            cOtherSource = new DataGridViewTextBoxColumn();
            flpButtons = new FlowLayoutPanel();
            bClose = new ExButton();
            bTakeOver = new ExButton();
            cbTarget = new ExComboBox();
            lTarget = new Label();
            lCount = new Label();
            ((ISupportInitialize)dgvDiff).BeginInit();
            tlpMain.SuspendLayout();
            flpOther.SuspendLayout();
            flpFilter.SuspendLayout();
            flpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(flpOther, 0, 0);
            tlpMain.Controls.Add(lInfo, 0, 1);
            tlpMain.Controls.Add(flpFilter, 0, 2);
            tlpMain.Controls.Add(dgvDiff, 0, 3);
            tlpMain.Controls.Add(flpButtons, 0, 4);
            tlpMain.Name = "tlpMain";
            // 
            // flpOther
            // 
            resources.ApplyResources(flpOther, "flpOther");
            flpOther.Controls.Add(lOther);
            flpOther.Controls.Add(cbOther);
            flpOther.Controls.Add(bFile);
            flpOther.Name = "flpOther";
            // 
            // lOther
            // 
            resources.ApplyResources(lOther, "lOther");
            lOther.Name = "lOther";
            // 
            // cbOther
            // 
            resources.ApplyResources(cbOther, "cbOther");
            cbOther.DropDownStyle = ComboBoxStyle.DropDownList;
            cbOther.FormattingEnabled = true;
            cbOther.Name = "cbOther";
            // 
            // bFile
            // 
            resources.ApplyResources(bFile, "bFile");
            bFile.UseVisualStyleBackColor = true;
            bFile.Name = "bFile";
            // 
            // lInfo
            // 
            resources.ApplyResources(lInfo, "lInfo");
            lInfo.Name = "lInfo";
            // 
            // flpFilter
            // 
            resources.ApplyResources(flpFilter, "flpFilter");
            flpFilter.Controls.Add(cboxDifferentOnly);
            flpFilter.Controls.Add(cboxOtherExplicit);
            flpFilter.Controls.Add(cboxHideApp);
            flpFilter.Name = "flpFilter";
            // 
            // cboxDifferentOnly
            // 
            resources.ApplyResources(cboxDifferentOnly, "cboxDifferentOnly");
            cboxDifferentOnly.UseVisualStyleBackColor = true;
            cboxDifferentOnly.Checked = true;
            cboxDifferentOnly.CheckState = CheckState.Checked;
            cboxDifferentOnly.Name = "cboxDifferentOnly";
            // 
            // cboxOtherExplicit
            // 
            resources.ApplyResources(cboxOtherExplicit, "cboxOtherExplicit");
            cboxOtherExplicit.UseVisualStyleBackColor = true;
            cboxOtherExplicit.Name = "cboxOtherExplicit";
            // 
            // cboxHideApp
            // 
            resources.ApplyResources(cboxHideApp, "cboxHideApp");
            cboxHideApp.UseVisualStyleBackColor = true;
            cboxHideApp.Checked = true;
            cboxHideApp.CheckState = CheckState.Checked;
            cboxHideApp.Name = "cboxHideApp";
            // 
            // dgvDiff
            // 
            resources.ApplyResources(dgvDiff, "dgvDiff");
            dgvDiff.Columns.AddRange(new DataGridViewColumn[] { cTake, cKey, cCurrent, cCurrentSource, cOther, cOtherSource });
            dgvDiff.AllowUserToAddRows = false;
            dgvDiff.AllowUserToDeleteRows = false;
            dgvDiff.AllowUserToResizeRows = false;
            dgvDiff.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDiff.RowHeadersVisible = false;
            dgvDiff.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDiff.Name = "dgvDiff";
            // 
            // cTake
            // 
            resources.ApplyResources(cTake, "cTake");
            cTake.SortMode = DataGridViewColumnSortMode.NotSortable;
            cTake.Name = "cTake";
            // 
            // cKey
            // 
            resources.ApplyResources(cKey, "cKey");
            cKey.ReadOnly = true;
            cKey.SortMode = DataGridViewColumnSortMode.NotSortable;
            cKey.Name = "cKey";
            // 
            // cCurrent
            // 
            resources.ApplyResources(cCurrent, "cCurrent");
            cCurrent.ReadOnly = true;
            cCurrent.SortMode = DataGridViewColumnSortMode.NotSortable;
            cCurrent.Name = "cCurrent";
            // 
            // cCurrentSource
            // 
            resources.ApplyResources(cCurrentSource, "cCurrentSource");
            cCurrentSource.ReadOnly = true;
            cCurrentSource.SortMode = DataGridViewColumnSortMode.NotSortable;
            cCurrentSource.Name = "cCurrentSource";
            // 
            // cOther
            // 
            resources.ApplyResources(cOther, "cOther");
            cOther.ReadOnly = true;
            cOther.SortMode = DataGridViewColumnSortMode.NotSortable;
            cOther.Name = "cOther";
            // 
            // cOtherSource
            // 
            resources.ApplyResources(cOtherSource, "cOtherSource");
            cOtherSource.ReadOnly = true;
            cOtherSource.SortMode = DataGridViewColumnSortMode.NotSortable;
            cOtherSource.Name = "cOtherSource";
            // 
            // flpButtons
            // 
            resources.ApplyResources(flpButtons, "flpButtons");
            flpButtons.Controls.Add(bClose);
            flpButtons.Controls.Add(bTakeOver);
            flpButtons.Controls.Add(cbTarget);
            flpButtons.Controls.Add(lTarget);
            flpButtons.Controls.Add(lCount);
            flpButtons.Name = "flpButtons";
            // 
            // bClose
            // 
            resources.ApplyResources(bClose, "bClose");
            bClose.UseVisualStyleBackColor = true;
            bClose.DialogResult = DialogResult.Cancel;
            bClose.Name = "bClose";
            // 
            // bTakeOver
            // 
            resources.ApplyResources(bTakeOver, "bTakeOver");
            bTakeOver.UseVisualStyleBackColor = true;
            bTakeOver.Name = "bTakeOver";
            // 
            // cbTarget
            // 
            resources.ApplyResources(cbTarget, "cbTarget");
            cbTarget.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTarget.FormattingEnabled = true;
            cbTarget.Name = "cbTarget";
            // 
            // lTarget
            // 
            resources.ApplyResources(lTarget, "lTarget");
            lTarget.Name = "lTarget";
            // 
            // lCount
            // 
            resources.ApplyResources(lCount, "lCount");
            lCount.Name = "lCount";
            // 
            // FCompareSettings
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            CancelButton = bClose;
            Name = "FCompareSettings";
            flpButtons.ResumeLayout(false);
            flpButtons.PerformLayout();
            flpFilter.ResumeLayout(false);
            flpFilter.PerformLayout();
            flpOther.ResumeLayout(false);
            flpOther.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ((ISupportInitialize)dgvDiff).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private FlowLayoutPanel flpOther;
        private Label lOther;
        private ExComboBox cbOther;
        private ExButton bFile;
        private Label lInfo;
        private FlowLayoutPanel flpFilter;
        private ExCheckBox cboxDifferentOnly;
        private ExCheckBox cboxOtherExplicit;
        private ExCheckBox cboxHideApp;
        private DataGridView dgvDiff;
        private DataGridViewCheckBoxColumn cTake;
        private DataGridViewTextBoxColumn cKey;
        private DataGridViewTextBoxColumn cCurrent;
        private DataGridViewTextBoxColumn cCurrentSource;
        private DataGridViewTextBoxColumn cOther;
        private DataGridViewTextBoxColumn cOtherSource;
        private FlowLayoutPanel flpButtons;
        private ExButton bClose;
        private ExButton bTakeOver;
        private ExComboBox cbTarget;
        private Label lTarget;
        private Label lCount;
    }
}
