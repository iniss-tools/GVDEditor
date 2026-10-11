namespace GVDEditor.UI.Import
{
    partial class FelisReview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FelisReview));
            this.tcReview = new ExControls.ExTabControl();
            this.tpShort = new System.Windows.Forms.TabPage();
            this.lShortInfo = new System.Windows.Forms.Label();
            this.cbKeepTrainShort = new ExControls.ExCheckBox();
            this.dgvStations = new System.Windows.Forms.DataGridView();
            this.colStName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStTrains = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStShort = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colStHint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpRemarks = new System.Windows.Forms.TabPage();
            this.lRemarksInfo = new System.Windows.Forms.Label();
            this.dgvRemarks = new System.Windows.Forms.DataGridView();
            this.colRemText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemTrains = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemFlag = new ExControls.DataGridViewExComboBoxColumn();
            this.colRemAddition = new ExControls.DataGridViewExComboBoxColumn();
            this.colRemLockout = new ExControls.DataGridViewExComboBoxColumn();
            this.bOK = new ExControls.ExButton();
            this.bStorno = new ExControls.ExButton();
            this.tcReview.SuspendLayout();
            this.tpShort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStations)).BeginInit();
            this.tpRemarks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRemarks)).BeginInit();
            this.SuspendLayout();
            //
            // tcReview
            //
            resources.ApplyResources(this.tcReview, "tcReview");
            this.tcReview.Controls.Add(this.tpShort);
            this.tcReview.Controls.Add(this.tpRemarks);
            this.tcReview.Name = "tcReview";
            this.tcReview.SelectedIndex = 0;
            //
            // tpShort
            //
            this.tpShort.Controls.Add(this.dgvStations);
            this.tpShort.Controls.Add(this.cbKeepTrainShort);
            this.tpShort.Controls.Add(this.lShortInfo);
            resources.ApplyResources(this.tpShort, "tpShort");
            this.tpShort.Name = "tpShort";
            this.tpShort.UseVisualStyleBackColor = true;
            //
            // lShortInfo
            //
            resources.ApplyResources(this.lShortInfo, "lShortInfo");
            this.lShortInfo.Name = "lShortInfo";
            //
            // cbKeepTrainShort
            //
            resources.ApplyResources(this.cbKeepTrainShort, "cbKeepTrainShort");
            this.cbKeepTrainShort.BoxBackColor = System.Drawing.Color.White;
            this.cbKeepTrainShort.Checked = true;
            this.cbKeepTrainShort.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbKeepTrainShort.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbKeepTrainShort.Name = "cbKeepTrainShort";
            this.cbKeepTrainShort.UseVisualStyleBackColor = true;
            //
            // dgvStations
            //
            this.dgvStations.AllowUserToAddRows = false;
            this.dgvStations.AllowUserToDeleteRows = false;
            this.dgvStations.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvStations, "dgvStations");
            this.dgvStations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colStName, this.colStTrains, this.colStShort, this.colStHint });
            this.dgvStations.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvStations.Name = "dgvStations";
            this.dgvStations.RowHeadersVisible = false;
            this.dgvStations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            //
            // tpRemarks
            //
            this.tpRemarks.Controls.Add(this.dgvRemarks);
            this.tpRemarks.Controls.Add(this.lRemarksInfo);
            resources.ApplyResources(this.tpRemarks, "tpRemarks");
            this.tpRemarks.Name = "tpRemarks";
            this.tpRemarks.UseVisualStyleBackColor = true;
            //
            // lRemarksInfo
            //
            resources.ApplyResources(this.lRemarksInfo, "lRemarksInfo");
            this.lRemarksInfo.Name = "lRemarksInfo";
            //
            // dgvRemarks
            //
            this.dgvRemarks.AllowUserToAddRows = false;
            this.dgvRemarks.AllowUserToDeleteRows = false;
            this.dgvRemarks.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvRemarks, "dgvRemarks");
            this.dgvRemarks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRemarks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colRemText, this.colRemTrains, this.colRemFlag, this.colRemAddition, this.colRemLockout });
            this.dgvRemarks.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvRemarks.Name = "dgvRemarks";
            this.dgvRemarks.RowHeadersVisible = false;
            this.dgvRemarks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            //
            // bOK
            //
            resources.ApplyResources(this.bOK, "bOK");
            this.bOK.Name = "bOK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            //
            // bStorno
            //
            resources.ApplyResources(this.bStorno, "bStorno");
            this.bStorno.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bStorno.Name = "bStorno";
            this.bStorno.UseVisualStyleBackColor = true;
            //
            // FELISReview
            //
            this.AcceptButton = this.bOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.Controls.Add(this.bStorno);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.tcReview);
            this.MinimizeBox = false;
            this.Name = "FelisReview";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            //
            // colStName
            //
            resources.ApplyResources(this.colStName, "colStName");
            this.colStName.Name = "colStName";
            this.colStName.ReadOnly = true;
            //
            // colStTrains
            //
            resources.ApplyResources(this.colStTrains, "colStTrains");
            this.colStTrains.Name = "colStTrains";
            this.colStTrains.ReadOnly = true;
            //
            // colStShort
            //
            resources.ApplyResources(this.colStShort, "colStShort");
            this.colStShort.Name = "colStShort";
            //
            // colStHint
            //
            resources.ApplyResources(this.colStHint, "colStHint");
            this.colStHint.Name = "colStHint";
            this.colStHint.ReadOnly = true;
            //
            // colRemText
            //
            resources.ApplyResources(this.colRemText, "colRemText");
            this.colRemText.Name = "colRemText";
            this.colRemText.ReadOnly = true;
            //
            // colRemTrains
            //
            resources.ApplyResources(this.colRemTrains, "colRemTrains");
            this.colRemTrains.Name = "colRemTrains";
            this.colRemTrains.ReadOnly = true;
            //
            // colRemFlag
            //
            resources.ApplyResources(this.colRemFlag, "colRemFlag");
            this.colRemFlag.Name = "colRemFlag";
            //
            // colRemAddition
            //
            resources.ApplyResources(this.colRemAddition, "colRemAddition");
            this.colRemAddition.Name = "colRemAddition";
            //
            // colRemLockout
            //
            resources.ApplyResources(this.colRemLockout, "colRemLockout");
            this.colRemLockout.Name = "colRemLockout";
            this.tcReview.ResumeLayout(false);
            this.tpShort.ResumeLayout(false);
            this.tpShort.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStations)).EndInit();
            this.tpRemarks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRemarks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ExControls.ExTabControl tcReview;
        private System.Windows.Forms.TabPage tpShort;
        private System.Windows.Forms.Label lShortInfo;
        private ExControls.ExCheckBox cbKeepTrainShort;
        private System.Windows.Forms.DataGridView dgvStations;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStTrains;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colStShort;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStHint;
        private System.Windows.Forms.TabPage tpRemarks;
        private System.Windows.Forms.Label lRemarksInfo;
        private System.Windows.Forms.DataGridView dgvRemarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemText;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemTrains;
        private ExControls.DataGridViewExComboBoxColumn colRemFlag;
        private ExControls.DataGridViewExComboBoxColumn colRemAddition;
        private ExControls.DataGridViewExComboBoxColumn colRemLockout;
        private ExControls.ExButton bOK;
        private ExControls.ExButton bStorno;
    }
}
