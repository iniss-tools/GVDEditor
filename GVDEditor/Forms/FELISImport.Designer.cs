
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FELISImport));
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
            resources.ApplyResources(this.cbSkipPassingTrains, "cbSkipPassingTrains");
            this.cbSkipPassingTrains.BoxBackColor = System.Drawing.Color.White;
            this.cbSkipPassingTrains.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbSkipPassingTrains.Name = "cbSkipPassingTrains";
            this.cbSkipPassingTrains.UseVisualStyleBackColor = true;
            // 
            // cbReorder
            // 
            resources.ApplyResources(this.cbReorder, "cbReorder");
            this.cbReorder.BoxBackColor = System.Drawing.Color.White;
            this.cbReorder.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbReorder.Name = "cbReorder";
            this.cbReorder.UseVisualStyleBackColor = true;
            // 
            // cbReplace
            // 
            resources.ApplyResources(this.cbReplace, "cbReplace");
            this.cbReplace.BoxBackColor = System.Drawing.Color.White;
            this.cbReplace.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbReplace.Name = "cbReplace";
            this.cbReplace.UseVisualStyleBackColor = true;
            // 
            // bImport
            // 
            resources.ApplyResources(this.bImport, "bImport");
            this.bImport.Name = "bImport";
            this.bImport.UseVisualStyleBackColor = true;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // bStorno
            // 
            resources.ApplyResources(this.bStorno, "bStorno");
            this.bStorno.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bStorno.Name = "bStorno";
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
            resources.ApplyResources(this.tbAppPath, "tbAppPath");
            this.tbAppPath.Name = "tbAppPath";
            // 
            // exGroupBox1
            // 
            this.exGroupBox1.Controls.Add(this.cbReplace);
            this.exGroupBox1.Controls.Add(this.cbReorder);
            this.exGroupBox1.Controls.Add(this.cbSkipPassingTrains);
            this.exGroupBox1.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.exGroupBox1, "exGroupBox1");
            this.exGroupBox1.Name = "exGroupBox1";
            this.exGroupBox1.TabStop = false;
            // 
            // bBrowse
            // 
            resources.ApplyResources(this.bBrowse, "bBrowse");
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lStation
            // 
            resources.ApplyResources(this.lStation, "lStation");
            this.lStation.Name = "lStation";
            // 
            // lReg
            // 
            resources.ApplyResources(this.lReg, "lReg");
            this.lReg.Name = "lReg";
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
            resources.ApplyResources(this.tbReg, "tbReg");
            this.tbReg.Name = "tbReg";
            // 
            // FELISImport
            // 
            this.AcceptButton = this.bImport;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
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
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FELISImport";
            this.ShowInTaskbar = false;
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
