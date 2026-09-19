namespace GVDEditor.Forms
{
    partial class FStateDgmCalendar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FStateDgmCalendar));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.flpTop = new System.Windows.Forms.FlowLayoutPanel();
            this.lTrain = new System.Windows.Forms.Label();
            this.cbTrain = new ExControls.ExComboBox();
            this.lDelayArr = new System.Windows.Forms.Label();
            this.nudDelayArr = new ExControls.ExNumericUpDown();
            this.lDelayDep = new System.Windows.Forms.Label();
            this.nudDelayDep = new ExControls.ExNumericUpDown();
            this.chkStanding = new ExControls.ExCheckBox();
            this.chkLockArr = new ExControls.ExCheckBox();
            this.chkLockDep = new ExControls.ExCheckBox();
            this.chkDeflected = new ExControls.ExCheckBox();
            this.lCategory = new System.Windows.Forms.Label();
            this.dgvCalendar = new System.Windows.Forms.DataGridView();
            this.cEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPlanned = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cTimePoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAdd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDelay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cWait = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cShort = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lInfo = new System.Windows.Forms.Label();
            this.tlpMain.SuspendLayout();
            this.flpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelayArr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelayDep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCalendar)).BeginInit();
            this.SuspendLayout();
            //
            // tlpMain
            //
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.flpTop, 0, 0);
            this.tlpMain.Controls.Add(this.lCategory, 0, 1);
            this.tlpMain.Controls.Add(this.dgvCalendar, 0, 2);
            this.tlpMain.Controls.Add(this.lInfo, 0, 3);
            this.tlpMain.Name = "tlpMain";
            //
            // flpTop
            //
            resources.ApplyResources(this.flpTop, "flpTop");
            this.flpTop.Controls.Add(this.lTrain);
            this.flpTop.Controls.Add(this.cbTrain);
            this.flpTop.Controls.Add(this.lDelayArr);
            this.flpTop.Controls.Add(this.nudDelayArr);
            this.flpTop.Controls.Add(this.lDelayDep);
            this.flpTop.Controls.Add(this.nudDelayDep);
            this.flpTop.Controls.Add(this.chkStanding);
            this.flpTop.Controls.Add(this.chkLockArr);
            this.flpTop.Controls.Add(this.chkLockDep);
            this.flpTop.Controls.Add(this.chkDeflected);
            this.flpTop.Name = "flpTop";
            //
            // lTrain
            //
            resources.ApplyResources(this.lTrain, "lTrain");
            this.lTrain.Name = "lTrain";
            //
            // cbTrain
            //
            resources.ApplyResources(this.cbTrain, "cbTrain");
            this.cbTrain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTrain.Name = "cbTrain";
            this.cbTrain.SelectedIndexChanged += new System.EventHandler(this.Input_Changed);
            //
            // lDelayArr
            //
            resources.ApplyResources(this.lDelayArr, "lDelayArr");
            this.lDelayArr.Name = "lDelayArr";
            //
            // nudDelayArr
            //
            resources.ApplyResources(this.nudDelayArr, "nudDelayArr");
            this.nudDelayArr.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            this.nudDelayArr.Name = "nudDelayArr";
            this.nudDelayArr.ValueChanged += new System.EventHandler(this.Input_Changed);
            //
            // lDelayDep
            //
            resources.ApplyResources(this.lDelayDep, "lDelayDep");
            this.lDelayDep.Name = "lDelayDep";
            //
            // nudDelayDep
            //
            resources.ApplyResources(this.nudDelayDep, "nudDelayDep");
            this.nudDelayDep.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            this.nudDelayDep.Name = "nudDelayDep";
            this.nudDelayDep.ValueChanged += new System.EventHandler(this.Input_Changed);
            //
            // chkStanding
            //
            resources.ApplyResources(this.chkStanding, "chkStanding");
            this.chkStanding.Name = "chkStanding";
            this.chkStanding.CheckedChanged += new System.EventHandler(this.Input_Changed);
            //
            // chkLockArr
            //
            resources.ApplyResources(this.chkLockArr, "chkLockArr");
            this.chkLockArr.Name = "chkLockArr";
            this.chkLockArr.CheckedChanged += new System.EventHandler(this.Input_Changed);
            //
            // chkLockDep
            //
            resources.ApplyResources(this.chkLockDep, "chkLockDep");
            this.chkLockDep.Name = "chkLockDep";
            this.chkLockDep.CheckedChanged += new System.EventHandler(this.Input_Changed);
            //
            // chkDeflected
            //
            resources.ApplyResources(this.chkDeflected, "chkDeflected");
            this.chkDeflected.Name = "chkDeflected";
            this.chkDeflected.CheckedChanged += new System.EventHandler(this.Input_Changed);
            //
            // lCategory
            //
            resources.ApplyResources(this.lCategory, "lCategory");
            this.lCategory.Name = "lCategory";
            //
            // dgvCalendar
            //
            resources.ApplyResources(this.dgvCalendar, "dgvCalendar");
            this.dgvCalendar.AllowUserToAddRows = false;
            this.dgvCalendar.AllowUserToDeleteRows = false;
            this.dgvCalendar.AllowUserToResizeRows = false;
            this.dgvCalendar.AutoGenerateColumns = false;
            this.dgvCalendar.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvCalendar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCalendar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCalendar.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cEvent,
            this.cMode,
            this.cPlanned,
            this.cTimePoint,
            this.cAdd,
            this.cDelay,
            this.cResult,
            this.cWait,
            this.cShort,
            this.cNote});
            this.dgvCalendar.MultiSelect = false;
            this.dgvCalendar.Name = "dgvCalendar";
            this.dgvCalendar.ReadOnly = true;
            this.dgvCalendar.RowHeadersVisible = false;
            this.dgvCalendar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCalendar.SelectionChanged += new System.EventHandler(this.dgvCalendar_SelectionChanged);
            //
            // cEvent
            //
            this.cEvent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cEvent.DataPropertyName = "Event";
            resources.ApplyResources(this.cEvent, "cEvent");
            this.cEvent.Name = "cEvent";
            this.cEvent.ReadOnly = true;
            //
            // cMode
            //
            this.cMode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cMode.DataPropertyName = "Mode";
            resources.ApplyResources(this.cMode, "cMode");
            this.cMode.Name = "cMode";
            this.cMode.ReadOnly = true;
            //
            // cPlanned
            //
            this.cPlanned.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cPlanned.DataPropertyName = "Planned";
            resources.ApplyResources(this.cPlanned, "cPlanned");
            this.cPlanned.Name = "cPlanned";
            this.cPlanned.ReadOnly = true;
            //
            // cTimePoint
            //
            this.cTimePoint.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cTimePoint.DataPropertyName = "TimePoint";
            resources.ApplyResources(this.cTimePoint, "cTimePoint");
            this.cTimePoint.Name = "cTimePoint";
            this.cTimePoint.ReadOnly = true;
            //
            // cAdd
            //
            this.cAdd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cAdd.DataPropertyName = "Add";
            resources.ApplyResources(this.cAdd, "cAdd");
            this.cAdd.Name = "cAdd";
            this.cAdd.ReadOnly = true;
            //
            // cDelay
            //
            this.cDelay.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cDelay.DataPropertyName = "Delay";
            resources.ApplyResources(this.cDelay, "cDelay");
            this.cDelay.Name = "cDelay";
            this.cDelay.ReadOnly = true;
            //
            // cResult
            //
            this.cResult.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cResult.DataPropertyName = "Result";
            resources.ApplyResources(this.cResult, "cResult");
            this.cResult.Name = "cResult";
            this.cResult.ReadOnly = true;
            //
            // cWait
            //
            this.cWait.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cWait.DataPropertyName = "Wait";
            resources.ApplyResources(this.cWait, "cWait");
            this.cWait.Name = "cWait";
            this.cWait.ReadOnly = true;
            //
            // cShort
            //
            this.cShort.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cShort.DataPropertyName = "Short";
            resources.ApplyResources(this.cShort, "cShort");
            this.cShort.Name = "cShort";
            this.cShort.ReadOnly = true;
            //
            // cNote
            //
            this.cNote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cNote.DataPropertyName = "Note";
            resources.ApplyResources(this.cNote, "cNote");
            this.cNote.Name = "cNote";
            this.cNote.ReadOnly = true;
            //
            // lInfo
            //
            resources.ApplyResources(this.lInfo, "lInfo");
            this.lInfo.Name = "lInfo";
            //
            // FStateDgmCalendar
            //
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.MinimizeBox = false;
            this.Name = "FStateDgmCalendar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.flpTop.ResumeLayout(false);
            this.flpTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelayArr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelayDep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCalendar)).EndInit();
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
        private ExControls.ExCheckBox chkLockArr;
        private ExControls.ExCheckBox chkLockDep;
        private ExControls.ExCheckBox chkDeflected;
        private System.Windows.Forms.Label lCategory;
        private System.Windows.Forms.DataGridView dgvCalendar;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn cMode;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cPlanned;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTimePoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDelay;
        private System.Windows.Forms.DataGridViewTextBoxColumn cResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn cWait;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cShort;
        private System.Windows.Forms.DataGridViewTextBoxColumn cNote;
        private System.Windows.Forms.Label lInfo;
    }
}
