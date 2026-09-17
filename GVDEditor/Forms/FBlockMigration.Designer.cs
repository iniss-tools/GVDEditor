
namespace GVDEditor.Forms
{
    partial class FBlockMigration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FBlockMigration));
            this.lInfo = new System.Windows.Forms.Label();
            this.dgvBlocks = new System.Windows.Forms.DataGridView();
            this.colIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrains = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDir = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lNote = new System.Windows.Forms.Label();
            this.bOK = new ExControls.ExButton();
            this.bStorno = new ExControls.ExButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlocks)).BeginInit();
            this.SuspendLayout();
            //
            // lInfo
            //
            resources.ApplyResources(this.lInfo, "lInfo");
            this.lInfo.Name = "lInfo";
            //
            // dgvBlocks
            //
            this.dgvBlocks.AllowUserToAddRows = false;
            this.dgvBlocks.AllowUserToDeleteRows = false;
            this.dgvBlocks.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvBlocks, "dgvBlocks");
            this.dgvBlocks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBlocks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colIndex, this.colStation, this.colTrains, this.colFrom, this.colTo, this.colDir });
            this.dgvBlocks.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
            this.dgvBlocks.MultiSelect = false;
            this.dgvBlocks.Name = "dgvBlocks";
            this.dgvBlocks.RowHeadersVisible = false;
            this.dgvBlocks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            //
            // colIndex
            //
            resources.ApplyResources(this.colIndex, "colIndex");
            this.colIndex.Name = "colIndex";
            this.colIndex.ReadOnly = true;
            //
            // colStation
            //
            resources.ApplyResources(this.colStation, "colStation");
            this.colStation.Name = "colStation";
            this.colStation.ReadOnly = true;
            //
            // colTrains
            //
            resources.ApplyResources(this.colTrains, "colTrains");
            this.colTrains.Name = "colTrains";
            this.colTrains.ReadOnly = true;
            //
            // colFrom
            //
            resources.ApplyResources(this.colFrom, "colFrom");
            this.colFrom.Name = "colFrom";
            this.colFrom.ReadOnly = true;
            //
            // colTo
            //
            resources.ApplyResources(this.colTo, "colTo");
            this.colTo.Name = "colTo";
            this.colTo.ReadOnly = true;
            //
            // colDir
            //
            this.colDir.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.colDir, "colDir");
            this.colDir.Name = "colDir";
            //
            // lNote
            //
            resources.ApplyResources(this.lNote, "lNote");
            this.lNote.Name = "lNote";
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
            // FBlockMigration
            //
            this.AcceptButton = this.bOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.Controls.Add(this.bStorno);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.lNote);
            this.Controls.Add(this.dgvBlocks);
            this.Controls.Add(this.lInfo);
            this.MinimizeBox = false;
            this.Name = "FBlockMigration";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlocks)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lInfo;
        private System.Windows.Forms.DataGridView dgvBlocks;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrains;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDir;
        private System.Windows.Forms.Label lNote;
        private ExControls.ExButton bOK;
        private ExControls.ExButton bStorno;
    }
}
