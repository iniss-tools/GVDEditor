namespace GVDEditor.Forms
{
    partial class FTabTabPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTabTabPreview));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.flpTop = new System.Windows.Forms.FlowLayoutPanel();
            this.lTrain = new System.Windows.Forms.Label();
            this.cbTrain = new ExControls.ExComboBox();
            this.lDelayArr = new System.Windows.Forms.Label();
            this.nudDelayArr = new ExControls.ExNumericUpDown();
            this.lDelayDep = new System.Windows.Forms.Label();
            this.nudDelayDep = new ExControls.ExNumericUpDown();
            this.chkStanding = new ExControls.ExCheckBox();
            this.chkDispatched = new ExControls.ExCheckBox();
            this.chkLockArr = new ExControls.ExCheckBox();
            this.chkLockDep = new ExControls.ExCheckBox();
            this.chkDeflected = new ExControls.ExCheckBox();
            this.chkOnlySection = new ExControls.ExCheckBox();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.cTable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cKind = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDivType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTab1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTab2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOwn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFont = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbSteps = new ExControls.ExTextBox();
            this.tlpMain.SuspendLayout();
            this.flpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelayArr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelayDep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.SuspendLayout();
            //
            // tlpMain
            //
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.flpTop, 0, 0);
            this.tlpMain.Controls.Add(this.splitMain, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //
            // flpTop
            //
            this.flpTop.AutoSize = true;
            this.flpTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpTop.Controls.Add(this.lTrain);
            this.flpTop.Controls.Add(this.cbTrain);
            this.flpTop.Controls.Add(this.lDelayArr);
            this.flpTop.Controls.Add(this.nudDelayArr);
            this.flpTop.Controls.Add(this.lDelayDep);
            this.flpTop.Controls.Add(this.nudDelayDep);
            this.flpTop.Controls.Add(this.chkStanding);
            this.flpTop.Controls.Add(this.chkDispatched);
            this.flpTop.Controls.Add(this.chkLockArr);
            this.flpTop.Controls.Add(this.chkLockDep);
            this.flpTop.Controls.Add(this.chkDeflected);
            this.flpTop.Controls.Add(this.chkOnlySection);
            this.flpTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTop.Name = "flpTop";
            this.flpTop.Padding = new System.Windows.Forms.Padding(4);
            //
            // lTrain
            //
            this.lTrain.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lTrain.AutoSize = true;
            resources.ApplyResources(this.lTrain, "lTrain");
            this.lTrain.Name = "lTrain";
            //
            // cbTrain
            //
            this.cbTrain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTrain.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.cbTrain.Name = "cbTrain";
            this.cbTrain.Size = new System.Drawing.Size(260, 21);
            this.cbTrain.SelectedIndexChanged += new System.EventHandler(this.Input_Changed);
            //
            // lDelayArr
            //
            this.lDelayArr.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lDelayArr.AutoSize = true;
            resources.ApplyResources(this.lDelayArr, "lDelayArr");
            this.lDelayArr.Name = "lDelayArr";
            //
            // nudDelayArr
            //
            this.nudDelayArr.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.nudDelayArr.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            this.nudDelayArr.Name = "nudDelayArr";
            this.nudDelayArr.Size = new System.Drawing.Size(52, 20);
            this.nudDelayArr.ValueChanged += new System.EventHandler(this.Input_Changed);
            //
            // lDelayDep
            //
            this.lDelayDep.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lDelayDep.AutoSize = true;
            resources.ApplyResources(this.lDelayDep, "lDelayDep");
            this.lDelayDep.Name = "lDelayDep";
            //
            // nudDelayDep
            //
            this.nudDelayDep.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.nudDelayDep.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            this.nudDelayDep.Name = "nudDelayDep";
            this.nudDelayDep.Size = new System.Drawing.Size(52, 20);
            this.nudDelayDep.ValueChanged += new System.EventHandler(this.Input_Changed);
            //
            // chkStanding
            //
            this.chkStanding.AutoSize = true;
            resources.ApplyResources(this.chkStanding, "chkStanding");
            this.chkStanding.Name = "chkStanding";
            this.chkStanding.CheckedChanged += new System.EventHandler(this.Input_Changed);
            //
            // chkDispatched
            //
            this.chkDispatched.AutoSize = true;
            resources.ApplyResources(this.chkDispatched, "chkDispatched");
            this.chkDispatched.Name = "chkDispatched";
            this.chkDispatched.CheckedChanged += new System.EventHandler(this.Input_Changed);
            //
            // chkLockArr
            //
            this.chkLockArr.AutoSize = true;
            resources.ApplyResources(this.chkLockArr, "chkLockArr");
            this.chkLockArr.Name = "chkLockArr";
            this.chkLockArr.CheckedChanged += new System.EventHandler(this.Input_Changed);
            //
            // chkLockDep
            //
            this.chkLockDep.AutoSize = true;
            resources.ApplyResources(this.chkLockDep, "chkLockDep");
            this.chkLockDep.Name = "chkLockDep";
            this.chkLockDep.CheckedChanged += new System.EventHandler(this.Input_Changed);
            //
            // chkDeflected
            //
            this.chkDeflected.AutoSize = true;
            resources.ApplyResources(this.chkDeflected, "chkDeflected");
            this.chkDeflected.Name = "chkDeflected";
            this.chkDeflected.CheckedChanged += new System.EventHandler(this.Input_Changed);
            //
            // chkOnlySection
            //
            this.chkOnlySection.AutoSize = true;
            this.chkOnlySection.Checked = true;
            this.chkOnlySection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOnlySection.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            resources.ApplyResources(this.chkOnlySection, "chkOnlySection");
            this.chkOnlySection.Name = "chkOnlySection";
            this.chkOnlySection.CheckedChanged += new System.EventHandler(this.Input_Changed);
            //
            // splitMain
            //
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // splitMain.Panel1
            //
            this.splitMain.Panel1.Controls.Add(this.dgvResult);
            //
            // splitMain.Panel2
            //
            this.splitMain.Panel2.Controls.Add(this.tbSteps);
            this.splitMain.SplitterDistance = 300;
            //
            // dgvResult
            //
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.AllowUserToResizeRows = false;
            this.dgvResult.AutoGenerateColumns = false;
            this.dgvResult.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cTable,
            this.cColumn,
            this.cKind,
            this.cDivType,
            this.cTab1,
            this.cTab2,
            this.cOwn,
            this.cResult,
            this.cFont});
            this.dgvResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResult.MultiSelect = false;
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResult.SelectionChanged += new System.EventHandler(this.dgvResult_SelectionChanged);
            //
            // columns
            //
            this.cTable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cTable.DataPropertyName = "Table";
            this.cTable.Name = "cTable";
            this.cTable.ReadOnly = true;
            resources.ApplyResources(this.cTable, "cTable");
            this.cColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cColumn.DataPropertyName = "Column";
            this.cColumn.Name = "cColumn";
            this.cColumn.ReadOnly = true;
            resources.ApplyResources(this.cColumn, "cColumn");
            this.cKind.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cKind.DataPropertyName = "Kind";
            this.cKind.Name = "cKind";
            this.cKind.ReadOnly = true;
            resources.ApplyResources(this.cKind, "cKind");
            this.cDivType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cDivType.DataPropertyName = "DivType";
            this.cDivType.Name = "cDivType";
            this.cDivType.ReadOnly = true;
            resources.ApplyResources(this.cDivType, "cDivType");
            this.cTab1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cTab1.DataPropertyName = "Tab1";
            this.cTab1.Name = "cTab1";
            this.cTab1.ReadOnly = true;
            resources.ApplyResources(this.cTab1, "cTab1");
            this.cTab2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cTab2.DataPropertyName = "Tab2";
            this.cTab2.Name = "cTab2";
            this.cTab2.ReadOnly = true;
            resources.ApplyResources(this.cTab2, "cTab2");
            this.cOwn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cOwn.DataPropertyName = "Own";
            this.cOwn.Name = "cOwn";
            this.cOwn.ReadOnly = true;
            resources.ApplyResources(this.cOwn, "cOwn");
            this.cResult.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cResult.DataPropertyName = "Result";
            this.cResult.MinimumWidth = 150;
            this.cResult.Name = "cResult";
            this.cResult.ReadOnly = true;
            resources.ApplyResources(this.cResult, "cResult");
            this.cFont.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cFont.DataPropertyName = "Font";
            this.cFont.Name = "cFont";
            this.cFont.ReadOnly = true;
            resources.ApplyResources(this.cFont, "cFont");
            //
            // tbSteps
            //
            this.tbSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSteps.Multiline = true;
            this.tbSteps.Name = "tbSteps";
            this.tbSteps.ReadOnly = true;
            this.tbSteps.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbSteps.WordWrap = false;
            //
            // FTabTabPreview
            //
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.MinimizeBox = false;
            this.Name = "FTabTabPreview";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.flpTop.ResumeLayout(false);
            this.flpTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelayArr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelayDep)).EndInit();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.FlowLayoutPanel flpTop;
        private System.Windows.Forms.Label lTrain;
        private ExControls.ExComboBox cbTrain;
        private System.Windows.Forms.Label lDelayArr;
        private ExControls.ExNumericUpDown nudDelayArr;
        private System.Windows.Forms.Label lDelayDep;
        private ExControls.ExNumericUpDown nudDelayDep;
        private ExControls.ExCheckBox chkStanding;
        private ExControls.ExCheckBox chkDispatched;
        private ExControls.ExCheckBox chkLockArr;
        private ExControls.ExCheckBox chkLockDep;
        private ExControls.ExCheckBox chkDeflected;
        private ExControls.ExCheckBox chkOnlySection;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn cColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cKind;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDivType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTab1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTab2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOwn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFont;
        private ExControls.ExTextBox tbSteps;
    }
}
