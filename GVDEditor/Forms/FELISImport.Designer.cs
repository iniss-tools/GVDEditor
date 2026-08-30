
namespace GVDEditor.Forms
{
    partial class FELISImport
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
            this.cbSkipPassingTrains = new ExControls.ExCheckBox();
            this.cbReorder = new ExControls.ExCheckBox();
            this.cbReplace = new ExControls.ExCheckBox();
            this.bImport = new ExControls.ExButton();
            this.bStorno = new ExControls.ExButton();
            this.tbAppPath = new ExControls.ExTextBox();
            this.exGroupBox1 = new ExControls.ExGroupBox();
            this.bBrowse = new ExControls.ExButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lStation = new System.Windows.Forms.Label();
            this.lReg = new System.Windows.Forms.Label();
            this.tbReg = new ExControls.ExTextBox();
            this.exGroupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // cbSkipPassingTrains
            //
            this.cbSkipPassingTrains.AutoSize = true;
            this.cbSkipPassingTrains.BoxBackColor = System.Drawing.Color.White;
            this.cbSkipPassingTrains.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbSkipPassingTrains.Location = new System.Drawing.Point(4, 17);
            this.cbSkipPassingTrains.Margin = new System.Windows.Forms.Padding(2);
            this.cbSkipPassingTrains.Name = "cbSkipPassingTrains";
            this.cbSkipPassingTrains.Size = new System.Drawing.Size(188, 17);
            this.cbSkipPassingTrains.TabIndex = 0;
            this.cbSkipPassingTrains.Text = "Preskakovať prechádzajúce vlaky";
            this.cbSkipPassingTrains.UseVisualStyleBackColor = true;
            //
            // cbReorder
            //
            this.cbReorder.AutoSize = true;
            this.cbReorder.BoxBackColor = System.Drawing.Color.White;
            this.cbReorder.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbReorder.Location = new System.Drawing.Point(242, 17);
            this.cbReorder.Margin = new System.Windows.Forms.Padding(2);
            this.cbReorder.Name = "cbReorder";
            this.cbReorder.Size = new System.Drawing.Size(190, 17);
            this.cbReorder.TabIndex = 1;
            this.cbReorder.Text = "Zoradiť vlaky a prepočítať varianty";
            this.cbReorder.UseVisualStyleBackColor = true;
            //
            // cbReplace
            //
            this.cbReplace.AutoSize = true;
            this.cbReplace.BoxBackColor = System.Drawing.Color.White;
            this.cbReplace.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbReplace.Location = new System.Drawing.Point(4, 40);
            this.cbReplace.Margin = new System.Windows.Forms.Padding(2);
            this.cbReplace.Name = "cbReplace";
            this.cbReplace.Size = new System.Drawing.Size(232, 17);
            this.cbReplace.TabIndex = 2;
            this.cbReplace.Text = "Nahradiť všetky vlaky v grafikone";
            this.cbReplace.UseVisualStyleBackColor = true;
            //
            // bImport
            //
            this.bImport.AutoSize = true;
            this.bImport.Location = new System.Drawing.Point(149, 205);
            this.bImport.Margin = new System.Windows.Forms.Padding(2);
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
            this.bStorno.Location = new System.Drawing.Point(251, 205);
            this.bStorno.Margin = new System.Windows.Forms.Padding(2);
            this.bStorno.Name = "bStorno";
            this.bStorno.Size = new System.Drawing.Size(56, 25);
            this.bStorno.TabIndex = 5;
            this.bStorno.Text = "Zrušiť";
            this.bStorno.UseVisualStyleBackColor = true;
            this.bStorno.Click += new System.EventHandler(this.bStorno_Click);
            //
            // tbAppPath
            //
            this.tbAppPath.BorderColor = System.Drawing.Color.DimGray;
            this.tbAppPath.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbAppPath.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbAppPath.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbAppPath.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbAppPath.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbAppPath.HintText = null;
            this.tbAppPath.Location = new System.Drawing.Point(9, 25);
            this.tbAppPath.Margin = new System.Windows.Forms.Padding(2);
            this.tbAppPath.Name = "tbAppPath";
            this.tbAppPath.Size = new System.Drawing.Size(348, 20);
            this.tbAppPath.TabIndex = 1;
            //
            // exGroupBox1
            //
            this.exGroupBox1.Controls.Add(this.cbReplace);
            this.exGroupBox1.Controls.Add(this.cbReorder);
            this.exGroupBox1.Controls.Add(this.cbSkipPassingTrains);
            this.exGroupBox1.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.exGroupBox1.Location = new System.Drawing.Point(9, 121);
            this.exGroupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.exGroupBox1.Name = "exGroupBox1";
            this.exGroupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.exGroupBox1.Size = new System.Drawing.Size(441, 68);
            this.exGroupBox1.TabIndex = 3;
            this.exGroupBox1.TabStop = false;
            this.exGroupBox1.Text = "Možnosti";
            //
            // bBrowse
            //
            this.bBrowse.Location = new System.Drawing.Point(362, 21);
            this.bBrowse.Margin = new System.Windows.Forms.Padding(2);
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
            this.label1.Size = new System.Drawing.Size(163, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Priečinok s aplikáciou Cestovné poriadky:";
            //
            // lReg
            //
            this.lReg.AutoSize = true;
            this.lReg.Location = new System.Drawing.Point(9, 54);
            this.lReg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lReg.Name = "lReg";
            this.lReg.Size = new System.Drawing.Size(258, 13);
            this.lReg.TabIndex = 7;
            this.lReg.Text = "Registračné číslo (voľne dostupné dáta ho nepotrebujú):";
            //
            // tbReg
            //
            this.tbReg.BorderColor = System.Drawing.Color.DimGray;
            this.tbReg.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbReg.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbReg.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbReg.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbReg.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbReg.HintText = null;
            this.tbReg.Location = new System.Drawing.Point(9, 70);
            this.tbReg.Margin = new System.Windows.Forms.Padding(2);
            this.tbReg.Name = "tbReg";
            this.tbReg.Size = new System.Drawing.Size(441, 20);
            this.tbReg.TabIndex = 3;
            //
            // lStation
            //
            this.lStation.AutoSize = true;
            this.lStation.Location = new System.Drawing.Point(9, 99);
            this.lStation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lStation.Name = "lStation";
            this.lStation.Size = new System.Drawing.Size(163, 13);
            this.lStation.TabIndex = 6;
            this.lStation.Text = "Vlaky sa načítajú pre stanicu:";
            //
            // FELISImport
            //
            this.AcceptButton = this.bImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.ClientSize = new System.Drawing.Size(459, 240);
            this.Controls.Add(this.tbReg);
            this.Controls.Add(this.lReg);
            this.Controls.Add(this.lStation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bBrowse);
            this.Controls.Add(this.exGroupBox1);
            this.Controls.Add(this.tbAppPath);
            this.Controls.Add(this.bStorno);
            this.Controls.Add(this.bImport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FELISImport";
            this.ShowInTaskbar = false;
            this.Text = "Import vlakov z programu ELIS";
            this.exGroupBox1.ResumeLayout(false);
            this.exGroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExControls.ExCheckBox cbSkipPassingTrains;
        private ExControls.ExCheckBox cbReorder;
        private ExControls.ExCheckBox cbReplace;
        private ExControls.ExButton bImport;
        private ExControls.ExButton bStorno;
        private ExControls.ExTextBox tbAppPath;
        private ExControls.ExGroupBox exGroupBox1;
        private ExControls.ExButton bBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lStation;
        private System.Windows.Forms.Label lReg;
        private ExControls.ExTextBox tbReg;
    }
}
