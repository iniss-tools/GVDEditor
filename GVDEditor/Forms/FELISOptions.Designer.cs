
namespace GVDEditor.Forms
{
    partial class FELISOptions
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
            this.cbOperators = new ExControls.ExCheckBox();
            this.cbDateRems = new ExControls.ExCheckBox();
            this.cbSkipPassingTrains = new ExControls.ExCheckBox();
            this.bImport = new ExControls.ExButton();
            this.bStorno = new ExControls.ExButton();
            this.tbPath = new ExControls.ExTextBox();
            this.exGroupBox1 = new ExControls.ExGroupBox();
            this.cbNewFormat = new ExControls.ExCheckBox();
            this.cbReorder = new ExControls.ExCheckBox();
            this.exLineSeparator1 = new ExControls.ExLineSeparator();
            this.bBrowse = new ExControls.ExButton();
            this.label1 = new System.Windows.Forms.Label();
            this.dOpenELIS = new System.Windows.Forms.OpenFileDialog();
            this.exGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbOperators
            // 
            this.cbOperators.AutoSize = true;
            this.cbOperators.BoxBackColor = System.Drawing.Color.White;
            this.cbOperators.Checked = true;
            this.cbOperators.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOperators.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbOperators.Location = new System.Drawing.Point(4, 17);
            this.cbOperators.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbOperators.Name = "cbOperators";
            this.cbOperators.Size = new System.Drawing.Size(198, 17);
            this.cbOperators.TabIndex = 0;
            this.cbOperators.Text = "V súbore sú definovaní dopravcovia";
            this.cbOperators.UseVisualStyleBackColor = true;
            // 
            // cbDateRems
            // 
            this.cbDateRems.AutoSize = true;
            this.cbDateRems.BoxBackColor = System.Drawing.Color.White;
            this.cbDateRems.Checked = true;
            this.cbDateRems.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDateRems.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbDateRems.Location = new System.Drawing.Point(4, 37);
            this.cbDateRems.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbDateRems.Name = "cbDateRems";
            this.cbDateRems.Size = new System.Drawing.Size(248, 17);
            this.cbDateRems.TabIndex = 2;
            this.cbDateRems.Text = "V súbore sú definované dátumové obmedzenia";
            this.cbDateRems.UseVisualStyleBackColor = true;
            // 
            // cbSkipPassingTrains
            // 
            this.cbSkipPassingTrains.AutoSize = true;
            this.cbSkipPassingTrains.BoxBackColor = System.Drawing.Color.White;
            this.cbSkipPassingTrains.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbSkipPassingTrains.Location = new System.Drawing.Point(4, 67);
            this.cbSkipPassingTrains.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbSkipPassingTrains.Name = "cbSkipPassingTrains";
            this.cbSkipPassingTrains.Size = new System.Drawing.Size(188, 17);
            this.cbSkipPassingTrains.TabIndex = 4;
            this.cbSkipPassingTrains.Text = "Preskakovať prechádzajúce vlaky";
            this.cbSkipPassingTrains.UseVisualStyleBackColor = true;
            // 
            // bImport
            // 
            this.bImport.AutoSize = true;
            this.bImport.Location = new System.Drawing.Point(149, 161);
            this.bImport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bImport.Name = "bImport";
            this.bImport.Size = new System.Drawing.Size(56, 25);
            this.bImport.TabIndex = 4;
            this.bImport.Text = "Import";
            this.bImport.UseVisualStyleBackColor = true;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // bStorno
            // 
            this.bStorno.AutoSize = true;
            this.bStorno.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bStorno.Location = new System.Drawing.Point(251, 161);
            this.bStorno.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bStorno.Name = "bStorno";
            this.bStorno.Size = new System.Drawing.Size(56, 25);
            this.bStorno.TabIndex = 5;
            this.bStorno.Text = "Zrušiť";
            this.bStorno.UseVisualStyleBackColor = true;
            this.bStorno.Click += new System.EventHandler(this.bStorno_Click);
            // 
            // tbPath
            // 
            this.tbPath.BorderColor = System.Drawing.Color.DimGray;
            this.tbPath.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbPath.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbPath.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbPath.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbPath.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbPath.HintText = null;
            this.tbPath.Location = new System.Drawing.Point(9, 25);
            this.tbPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(348, 20);
            this.tbPath.TabIndex = 1;
            // 
            // exGroupBox1
            // 
            this.exGroupBox1.Controls.Add(this.cbNewFormat);
            this.exGroupBox1.Controls.Add(this.cbReorder);
            this.exGroupBox1.Controls.Add(this.cbSkipPassingTrains);
            this.exGroupBox1.Controls.Add(this.exLineSeparator1);
            this.exGroupBox1.Controls.Add(this.cbOperators);
            this.exGroupBox1.Controls.Add(this.cbDateRems);
            this.exGroupBox1.Location = new System.Drawing.Point(9, 55);
            this.exGroupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.exGroupBox1.Name = "exGroupBox1";
            this.exGroupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.exGroupBox1.Size = new System.Drawing.Size(441, 93);
            this.exGroupBox1.TabIndex = 3;
            this.exGroupBox1.TabStop = false;
            this.exGroupBox1.Text = "Možnosti";
            // 
            // cbNewFormat
            // 
            this.cbNewFormat.AutoSize = true;
            this.cbNewFormat.BoxBackColor = System.Drawing.Color.White;
            this.cbNewFormat.Checked = true;
            this.cbNewFormat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNewFormat.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbNewFormat.Location = new System.Drawing.Point(242, 17);
            this.cbNewFormat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbNewFormat.Name = "cbNewFormat";
            this.cbNewFormat.Size = new System.Drawing.Size(114, 17);
            this.cbNewFormat.TabIndex = 1;
            this.cbNewFormat.Text = "Použiť nový formát";
            this.cbNewFormat.UseVisualStyleBackColor = true;
            // 
            // cbReorder
            // 
            this.cbReorder.AutoSize = true;
            this.cbReorder.BoxBackColor = System.Drawing.Color.White;
            this.cbReorder.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbReorder.Location = new System.Drawing.Point(242, 67);
            this.cbReorder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbReorder.Name = "cbReorder";
            this.cbReorder.Size = new System.Drawing.Size(190, 17);
            this.cbReorder.TabIndex = 5;
            this.cbReorder.Text = "Zoradiť vlaky a prepočítať varianty";
            this.cbReorder.UseVisualStyleBackColor = true;
            // 
            // exLineSeparator1
            // 
            this.exLineSeparator1.Location = new System.Drawing.Point(4, 59);
            this.exLineSeparator1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.exLineSeparator1.Name = "exLineSeparator1";
            this.exLineSeparator1.Size = new System.Drawing.Size(432, 8);
            this.exLineSeparator1.TabIndex = 3;
            this.exLineSeparator1.Text = null;
            // 
            // bBrowse
            // 
            this.bBrowse.Location = new System.Drawing.Point(362, 21);
            this.bBrowse.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(88, 26);
            this.bBrowse.TabIndex = 2;
            this.bBrowse.Text = "Prehľadávať...";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cesta k súboru:";
            // 
            // dOpenELIS
            // 
            this.dOpenELIS.Filter = "Textové súbory (*.txt)|*.txt";
            // 
            // FELISOptions
            // 
            this.AcceptButton = this.bImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.ClientSize = new System.Drawing.Size(459, 196);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bBrowse);
            this.Controls.Add(this.exGroupBox1);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.bStorno);
            this.Controls.Add(this.bImport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FELISOptions";
            this.ShowInTaskbar = false;
            this.Text = "Možnosti importu z ELIS súboru";
            this.exGroupBox1.ResumeLayout(false);
            this.exGroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExControls.ExCheckBox cbOperators;
        private ExControls.ExCheckBox cbDateRems;
        private ExControls.ExCheckBox cbSkipPassingTrains;
        private ExControls.ExButton bImport;
        private ExControls.ExButton bStorno;
        private ExControls.ExTextBox tbPath;
        private ExControls.ExGroupBox exGroupBox1;
        private ExControls.ExButton bBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog dOpenELIS;
        private ExControls.ExLineSeparator exLineSeparator1;
        private ExControls.ExCheckBox cbReorder;
        private ExControls.ExCheckBox cbNewFormat;
    }
}