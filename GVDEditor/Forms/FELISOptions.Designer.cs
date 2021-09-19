
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
            this.components = new System.ComponentModel.Container();
            this.cbOperators = new ExControls.ExCheckBox();
            this.cbDateRems = new ExControls.ExCheckBox();
            this.cbSkipPassingTrains = new ExControls.ExCheckBox();
            this.bImport = new System.Windows.Forms.Button();
            this.bStorno = new System.Windows.Forms.Button();
            this.tbPath = new ExControls.ExTextBox();
            this.exGroupBox1 = new ExControls.ExGroupBox();
            this.cbReorder = new ExControls.ExCheckBox();
            this.exLineSeparator1 = new ExControls.ExLineSeparator();
            this.bBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dOpenELIS = new System.Windows.Forms.OpenFileDialog();
            this.cbNewFormat = new ExControls.ExCheckBox();
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
            this.cbOperators.Location = new System.Drawing.Point(6, 21);
            this.cbOperators.Name = "cbOperators";
            this.cbOperators.Size = new System.Drawing.Size(256, 21);
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
            this.cbDateRems.Location = new System.Drawing.Point(6, 46);
            this.cbDateRems.Name = "cbDateRems";
            this.cbDateRems.Size = new System.Drawing.Size(327, 21);
            this.cbDateRems.TabIndex = 1;
            this.cbDateRems.Text = "V súbore sú definované dátumové obmedzenia";
            this.cbDateRems.UseVisualStyleBackColor = true;
            // 
            // cbSkipPassingTrains
            // 
            this.cbSkipPassingTrains.AutoSize = true;
            this.cbSkipPassingTrains.BoxBackColor = System.Drawing.Color.White;
            this.cbSkipPassingTrains.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbSkipPassingTrains.Location = new System.Drawing.Point(6, 83);
            this.cbSkipPassingTrains.Name = "cbSkipPassingTrains";
            this.cbSkipPassingTrains.Size = new System.Drawing.Size(242, 21);
            this.cbSkipPassingTrains.TabIndex = 2;
            this.cbSkipPassingTrains.Text = "Preskakovať prechádzajúce vlaky";
            this.cbSkipPassingTrains.UseVisualStyleBackColor = true;
            // 
            // bImport
            // 
            this.bImport.AutoSize = true;
            this.bImport.Location = new System.Drawing.Point(199, 198);
            this.bImport.Name = "bImport";
            this.bImport.Size = new System.Drawing.Size(75, 31);
            this.bImport.TabIndex = 3;
            this.bImport.Text = "Import";
            this.bImport.UseVisualStyleBackColor = true;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // bStorno
            // 
            this.bStorno.AutoSize = true;
            this.bStorno.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bStorno.Location = new System.Drawing.Point(335, 198);
            this.bStorno.Name = "bStorno";
            this.bStorno.Size = new System.Drawing.Size(75, 31);
            this.bStorno.TabIndex = 4;
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
            this.tbPath.Location = new System.Drawing.Point(12, 31);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(463, 22);
            this.tbPath.TabIndex = 5;
            // 
            // exGroupBox1
            // 
            this.exGroupBox1.Controls.Add(this.cbNewFormat);
            this.exGroupBox1.Controls.Add(this.cbReorder);
            this.exGroupBox1.Controls.Add(this.cbSkipPassingTrains);
            this.exGroupBox1.Controls.Add(this.exLineSeparator1);
            this.exGroupBox1.Controls.Add(this.cbOperators);
            this.exGroupBox1.Controls.Add(this.cbDateRems);
            this.exGroupBox1.Location = new System.Drawing.Point(12, 68);
            this.exGroupBox1.Name = "exGroupBox1";
            this.exGroupBox1.Size = new System.Drawing.Size(588, 114);
            this.exGroupBox1.TabIndex = 6;
            this.exGroupBox1.TabStop = false;
            this.exGroupBox1.Text = "Možnosti";
            // 
            // cbReorder
            // 
            this.cbReorder.AutoSize = true;
            this.cbReorder.BoxBackColor = System.Drawing.Color.White;
            this.cbReorder.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbReorder.Location = new System.Drawing.Point(323, 83);
            this.cbReorder.Name = "cbReorder";
            this.cbReorder.Size = new System.Drawing.Size(246, 21);
            this.cbReorder.TabIndex = 4;
            this.cbReorder.Text = "Zoradiť vlaky a prepočítať varianty";
            this.cbReorder.UseVisualStyleBackColor = true;
            // 
            // exLineSeparator1
            // 
            this.exLineSeparator1.Location = new System.Drawing.Point(6, 73);
            this.exLineSeparator1.Name = "exLineSeparator1";
            this.exLineSeparator1.Size = new System.Drawing.Size(576, 10);
            this.exLineSeparator1.TabIndex = 3;
            this.exLineSeparator1.Text = null;
            // 
            // bBrowse
            // 
            this.bBrowse.Location = new System.Drawing.Point(482, 26);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(118, 32);
            this.bBrowse.TabIndex = 7;
            this.bBrowse.Text = "Prehľadávať...";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Cesta k súboru:";
            // 
            // dOpenELIS
            // 
            this.dOpenELIS.Filter = "Textové súbory (*.txt)|*.txt";
            // 
            // cbNewFormat
            // 
            this.cbNewFormat.AutoSize = true;
            this.cbNewFormat.BoxBackColor = System.Drawing.Color.White;
            this.cbNewFormat.Checked = true;
            this.cbNewFormat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNewFormat.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbNewFormat.Location = new System.Drawing.Point(323, 21);
            this.cbNewFormat.Name = "cbNewFormat";
            this.cbNewFormat.Size = new System.Drawing.Size(148, 21);
            this.cbNewFormat.TabIndex = 5;
            this.cbNewFormat.Text = "Použiť nový formát";
            this.cbNewFormat.UseVisualStyleBackColor = true;
            // 
            // FELISOptions
            // 
            this.AcceptButton = this.bImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.ClientSize = new System.Drawing.Size(612, 241);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bBrowse);
            this.Controls.Add(this.exGroupBox1);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.bStorno);
            this.Controls.Add(this.bImport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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
        private System.Windows.Forms.Button bImport;
        private System.Windows.Forms.Button bStorno;
        private ExControls.ExTextBox tbPath;
        private ExControls.ExGroupBox exGroupBox1;
        private System.Windows.Forms.Button bBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog dOpenELIS;
        private ExControls.ExLineSeparator exLineSeparator1;
        private ExControls.ExCheckBox cbReorder;
        private ExControls.ExCheckBox cbNewFormat;
    }
}