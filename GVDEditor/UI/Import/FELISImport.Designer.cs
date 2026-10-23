namespace GVDEditor.UI.Import
{
    partial class FelisImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FelisImport));
            this.cbSkipPassingTrains = new ExControls.ExCheckBox();
            this.cbReorder = new ExControls.ExCheckBox();
            this.cbTableTexts = new ExControls.ExCheckBox();
            this.rbAdd = new ExControls.ExRadioButton();
            this.rbUpdate = new ExControls.ExRadioButton();
            this.rbReplace = new ExControls.ExRadioButton();
            this.bImport = new ExControls.ExButton();
            this.bStorno = new ExControls.ExButton();
            this.tbAppPath = new ExControls.ExTextBox();
            this.gbOptions = new ExControls.ExGroupBox();
            this.gbTrains = new ExControls.ExGroupBox();
            this.bBrowse = new ExControls.ExButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lStation = new System.Windows.Forms.Label();
            this.lLearn = new System.Windows.Forms.Label();
            this.cbLearn = new ExControls.ExComboBox();
            this.lPlan = new System.Windows.Forms.Label();
            this.tbPlan = new ExControls.ExTextBox();
            this.bPlanBrowse = new ExControls.ExButton();
            this.gbOptions.SuspendLayout();
            this.gbTrains.SuspendLayout();
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
            // cbTableTexts
            //
            resources.ApplyResources(this.cbTableTexts, "cbTableTexts");
            this.cbTableTexts.BoxBackColor = System.Drawing.Color.White;
            this.cbTableTexts.Checked = true;
            this.cbTableTexts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTableTexts.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbTableTexts.Name = "cbTableTexts";
            this.cbTableTexts.UseVisualStyleBackColor = true;
            //
            // rbAdd
            //
            resources.ApplyResources(this.rbAdd, "rbAdd");
            this.rbAdd.Checked = true;
            this.rbAdd.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbAdd.Name = "rbAdd";
            this.rbAdd.TabStop = true;
            this.rbAdd.UseVisualStyleBackColor = true;
            //
            // rbUpdate
            //
            resources.ApplyResources(this.rbUpdate, "rbUpdate");
            this.rbUpdate.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbUpdate.Name = "rbUpdate";
            this.rbUpdate.UseVisualStyleBackColor = true;
            //
            // rbReplace
            //
            resources.ApplyResources(this.rbReplace, "rbReplace");
            this.rbReplace.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbReplace.Name = "rbReplace";
            this.rbReplace.UseVisualStyleBackColor = true;
            this.rbReplace.CheckedChanged += new System.EventHandler(this.rbReplace_CheckedChanged);
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
            // gbOptions
            //
            this.gbOptions.Controls.Add(this.cbTableTexts);
            this.gbOptions.Controls.Add(this.cbReorder);
            this.gbOptions.Controls.Add(this.cbSkipPassingTrains);
            this.gbOptions.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.gbOptions, "gbOptions");
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.TabStop = false;
            //
            // gbTrains
            //
            this.gbTrains.Controls.Add(this.rbReplace);
            this.gbTrains.Controls.Add(this.rbUpdate);
            this.gbTrains.Controls.Add(this.rbAdd);
            this.gbTrains.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.gbTrains, "gbTrains");
            this.gbTrains.Name = "gbTrains";
            this.gbTrains.TabStop = false;
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
            // lLearn
            //
            resources.ApplyResources(this.lLearn, "lLearn");
            this.lLearn.Name = "lLearn";
            //
            // cbLearn
            //
            this.cbLearn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbLearn, "cbLearn");
            this.cbLearn.Name = "cbLearn";
            //
            // lPlan
            //
            resources.ApplyResources(this.lPlan, "lPlan");
            this.lPlan.Name = "lPlan";
            //
            // tbPlan
            //
            this.tbPlan.BorderColor = System.Drawing.Color.DimGray;
            this.tbPlan.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbPlan.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbPlan.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbPlan.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbPlan.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbPlan.HintText = null;
            resources.ApplyResources(this.tbPlan, "tbPlan");
            this.tbPlan.Name = "tbPlan";
            //
            // bPlanBrowse
            //
            resources.ApplyResources(this.bPlanBrowse, "bPlanBrowse");
            this.bPlanBrowse.Name = "bPlanBrowse";
            this.bPlanBrowse.UseVisualStyleBackColor = true;
            this.bPlanBrowse.Click += new System.EventHandler(this.bPlanBrowse_Click);
            //
            // FELISImport
            //
            this.AcceptButton = this.bImport;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.Controls.Add(this.bPlanBrowse);
            this.Controls.Add(this.tbPlan);
            this.Controls.Add(this.lPlan);
            this.Controls.Add(this.cbLearn);
            this.Controls.Add(this.lLearn);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.gbTrains);
            this.Controls.Add(this.lStation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bBrowse);
            this.Controls.Add(this.tbAppPath);
            this.Controls.Add(this.bStorno);
            this.Controls.Add(this.bImport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FelisImport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.gbTrains.ResumeLayout(false);
            this.gbTrains.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ExControls.ExCheckBox cbSkipPassingTrains;
        private ExControls.ExCheckBox cbReorder;
        private ExControls.ExCheckBox cbTableTexts;
        private ExControls.ExRadioButton rbAdd;
        private ExControls.ExRadioButton rbUpdate;
        private ExControls.ExRadioButton rbReplace;
        private ExControls.ExButton bImport;
        private ExControls.ExButton bStorno;
        private ExControls.ExTextBox tbAppPath;
        private ExControls.ExGroupBox gbOptions;
        private ExControls.ExGroupBox gbTrains;
        private ExControls.ExButton bBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lStation;
        private System.Windows.Forms.Label lLearn;
        private ExControls.ExComboBox cbLearn;
        private System.Windows.Forms.Label lPlan;
        private ExControls.ExTextBox tbPlan;
        private ExControls.ExButton bPlanBrowse;
    }
}
