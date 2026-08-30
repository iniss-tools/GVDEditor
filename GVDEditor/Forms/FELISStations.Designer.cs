
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
            this.lInfo.Location = new System.Drawing.Point(12, 9);
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new System.Drawing.Size(600, 46);
            this.lInfo.TabIndex = 0;
            this.lInfo.Text = "Stanice z programu ELIS";
            //
            // dgvStations
            //
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
            this.dgvStations.Location = new System.Drawing.Point(12, 58);
            this.dgvStations.MultiSelect = true;
            this.dgvStations.Name = "dgvStations";
            this.dgvStations.RowHeadersVisible = false;
            this.dgvStations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStations.Size = new System.Drawing.Size(600, 320);
            this.dgvStations.TabIndex = 1;
            //
            // colElis
            //
            this.colElis.HeaderText = "Názov v programe ELIS";
            this.colElis.Name = "colElis";
            this.colElis.ReadOnly = true;
            this.colElis.Width = 250;
            //
            // colStation
            //
            this.colStation.HeaderText = "Priradiť k stanici grafikonu";
            this.colStation.Name = "colStation";
            this.colStation.Width = 320;
            //
            // bCreate
            //
            this.bCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCreate.AutoSize = true;
            this.bCreate.Location = new System.Drawing.Point(12, 388);
            this.bCreate.Name = "bCreate";
            this.bCreate.Size = new System.Drawing.Size(190, 25);
            this.bCreate.TabIndex = 2;
            this.bCreate.Text = "Založiť nové stanice z označených";
            this.bCreate.UseVisualStyleBackColor = true;
            this.bCreate.Click += new System.EventHandler(this.bCreate_Click);
            //
            // bSkipAll
            //
            this.bSkipAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bSkipAll.AutoSize = true;
            this.bSkipAll.Location = new System.Drawing.Point(208, 388);
            this.bSkipAll.Name = "bSkipAll";
            this.bSkipAll.Size = new System.Drawing.Size(110, 25);
            this.bSkipAll.TabIndex = 3;
            this.bSkipAll.Text = "Vynechať všetky";
            this.bSkipAll.UseVisualStyleBackColor = true;
            this.bSkipAll.Click += new System.EventHandler(this.bSkipAll_Click);
            //
            // bOK
            //
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.AutoSize = true;
            this.bOK.Location = new System.Drawing.Point(454, 388);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 25);
            this.bOK.TabIndex = 4;
            this.bOK.Text = "Pokračovať";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            //
            // bStorno
            //
            this.bStorno.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bStorno.AutoSize = true;
            this.bStorno.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bStorno.Location = new System.Drawing.Point(537, 388);
            this.bStorno.Name = "bStorno";
            this.bStorno.Size = new System.Drawing.Size(75, 25);
            this.bStorno.TabIndex = 5;
            this.bStorno.Text = "Zrušiť import";
            this.bStorno.UseVisualStyleBackColor = true;
            this.bStorno.Click += new System.EventHandler(this.bStorno_Click);
            //
            // FELISStations
            //
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.ClientSize = new System.Drawing.Size(624, 425);
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
            this.Text = "Priradenie staníc z programu ELIS";
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
