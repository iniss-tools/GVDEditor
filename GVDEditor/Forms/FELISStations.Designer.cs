
namespace GVDEditor.Forms
{
    partial class FELISStations
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FELISStations));
            this.dgvStations = new System.Windows.Forms.DataGridView();
            this.colElis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStation = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lInfo = new System.Windows.Forms.Label();
            this.bOK = new ExControls.ExButton();
            this.bStorno = new ExControls.ExButton();
            this.bCreate = new ExControls.ExButton();
            this.bSkipAll = new ExControls.ExButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStations)).BeginInit();
            this.SuspendLayout();
            //
            // lInfo
            //
            resources.ApplyResources(this.lInfo, "lInfo");
            this.lInfo.Name = "lInfo";
            //
            // dgvStations
            //
            resources.ApplyResources(this.dgvStations, "dgvStations");
            this.dgvStations.AllowUserToAddRows = false;
            this.dgvStations.AllowUserToDeleteRows = false;
            this.dgvStations.AllowUserToResizeRows = false;
            this.dgvStations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvStations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colElis,
            this.colStation});
            this.dgvStations.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvStations.MultiSelect = true;
            this.dgvStations.Name = "dgvStations";
            this.dgvStations.RowHeadersVisible = false;
            this.dgvStations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            //
            // colElis
            //
            resources.ApplyResources(this.colElis, "colElis");
            this.colElis.Name = "colElis";
            this.colElis.ReadOnly = true;
            //
            // colStation
            //
            resources.ApplyResources(this.colStation, "colStation");
            this.colStation.Name = "colStation";
            //
            // bCreate
            //
            resources.ApplyResources(this.bCreate, "bCreate");
            this.bCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCreate.Name = "bCreate";
            this.bCreate.UseVisualStyleBackColor = true;
            this.bCreate.Click += new System.EventHandler(this.bCreate_Click);
            //
            // bSkipAll
            //
            resources.ApplyResources(this.bSkipAll, "bSkipAll");
            this.bSkipAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bSkipAll.Name = "bSkipAll";
            this.bSkipAll.UseVisualStyleBackColor = true;
            this.bSkipAll.Click += new System.EventHandler(this.bSkipAll_Click);
            //
            // bOK
            //
            resources.ApplyResources(this.bOK, "bOK");
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Name = "bOK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            //
            // bStorno
            //
            resources.ApplyResources(this.bStorno, "bStorno");
            this.bStorno.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bStorno.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bStorno.Name = "bStorno";
            this.bStorno.UseVisualStyleBackColor = true;
            this.bStorno.Click += new System.EventHandler(this.bStorno_Click);
            //
            // FELISStations
            //
            resources.ApplyResources(this, "$this");
            this.AcceptButton = this.bOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.Controls.Add(this.bStorno);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bSkipAll);
            this.Controls.Add(this.bCreate);
            this.Controls.Add(this.dgvStations);
            this.Controls.Add(this.lInfo);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(560, 340);
            this.Name = "FELISStations";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.dgvStations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvStations;
        private System.Windows.Forms.DataGridViewTextBoxColumn colElis;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStation;
        private System.Windows.Forms.Label lInfo;
        private ExControls.ExButton bOK;
        private ExControls.ExButton bStorno;
        private ExControls.ExButton bCreate;
        private ExControls.ExButton bSkipAll;
    }
}
