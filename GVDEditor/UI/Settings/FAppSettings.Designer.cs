using ExControls;

namespace GVDEditor.UI.Settings
{
    partial class FAppSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAppSettings));
            this.exGroupBox2 = new ExControls.ExGroupBox();
            this.cboxTabTextAutoGenerate = new ExControls.ExCheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbDateLimitLanguage = new ExControls.ExComboBox();
            this.exGroupBox4 = new ExControls.ExGroupBox();
            this.nudPlayerWordPause = new ExControls.ExNumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.exGroupBox5 = new ExControls.ExGroupBox();
            this.cboxInissEvaluation = new ExControls.ExCheckBox();
            ExControls.OptionsNode optionsNodeMcp = new ExControls.OptionsNode();
            this.pMcp = new ExControls.ExOptionsPanel(this.optionsView);
            this.mcpPage = new ToolsCore.Mcp.McpSettingsPage();
            this.pGeneral.SuspendLayout();
            this.pConcreteGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsView)).BeginInit();
            this.exGroupBox2.SuspendLayout();
            this.exGroupBox4.SuspendLayout();
            this.exGroupBox5.SuspendLayout();
            this.pMcp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlayerWordPause)).BeginInit();
            this.SuspendLayout();
            // 
            // pConcreteGeneral
            // 
            this.pConcreteGeneral.Controls.Add(this.exGroupBox5);
            this.pConcreteGeneral.Controls.Add(this.exGroupBox4);
            this.pConcreteGeneral.Controls.Add(this.exGroupBox2);
            resources.ApplyResources(this.pConcreteGeneral, "pConcreteGeneral");
            // 
            // optionsView
            // 
            this.optionsView.HeaderNodeNameVisible = true;
            this.optionsView.Panels.Add(this.pMcp);
            this.optionsView.SearchBoxVisible = true;
            resources.ApplyResources(this.optionsView, "optionsView");
            // 
            // 
            // 
            this.optionsView.TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionsView.TreeView.FullRowSelect = true;
            this.optionsView.TreeView.HideSelection = false;
            this.optionsView.TreeView.ItemHeight = 20;
            this.optionsView.TreeView.Name = "treeView";
            this.optionsView.TreeView.PathSeparator = " / ";
            this.optionsView.TreeView.ShowLines = false;
            this.optionsView.TreeView.ShowNodeToolTips = true;
            this.optionsView.TreeView.Style = ExControls.ExTreeViewStyle.Light;
            this.optionsView.TreeView.TabIndex = 0;
            // 
            // exGroupBox2
            // 
            resources.ApplyResources(this.exGroupBox2, "exGroupBox2");
            this.exGroupBox2.Controls.Add(this.cbDateLimitLanguage);
            this.exGroupBox2.Controls.Add(this.label5);
            this.exGroupBox2.Controls.Add(this.cboxTabTextAutoGenerate);
            this.exGroupBox2.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.exGroupBox2.Name = "exGroupBox2";
            this.exGroupBox2.TabStop = false;
            // 
            // cboxTabTextAutoGenerate
            // 
            resources.ApplyResources(this.cboxTabTextAutoGenerate, "cboxTabTextAutoGenerate");
            this.cboxTabTextAutoGenerate.BoxBackColor = System.Drawing.Color.White;
            this.cboxTabTextAutoGenerate.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxTabTextAutoGenerate.Name = "cboxTabTextAutoGenerate";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cbDateLimitLanguage
            // 
            this.cbDateLimitLanguage.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbDateLimitLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDateLimitLanguage.FormattingEnabled = true;
            resources.ApplyResources(this.cbDateLimitLanguage, "cbDateLimitLanguage");
            this.cbDateLimitLanguage.Name = "cbDateLimitLanguage";
            this.cbDateLimitLanguage.StyleDisabled.ArrowColor = null;
            this.cbDateLimitLanguage.StyleDisabled.BackColor = null;
            this.cbDateLimitLanguage.StyleDisabled.BorderColor = null;
            this.cbDateLimitLanguage.StyleDisabled.ButtonBackColor = null;
            this.cbDateLimitLanguage.StyleDisabled.ButtonBorderColor = null;
            this.cbDateLimitLanguage.StyleDisabled.ButtonRenderFirst = null;
            this.cbDateLimitLanguage.StyleDisabled.ForeColor = null;
            this.cbDateLimitLanguage.StyleHighlight.ArrowColor = null;
            this.cbDateLimitLanguage.StyleHighlight.BackColor = null;
            this.cbDateLimitLanguage.StyleHighlight.BorderColor = null;
            this.cbDateLimitLanguage.StyleHighlight.ButtonBackColor = null;
            this.cbDateLimitLanguage.StyleHighlight.ButtonBorderColor = null;
            this.cbDateLimitLanguage.StyleHighlight.ButtonRenderFirst = null;
            this.cbDateLimitLanguage.StyleHighlight.ForeColor = null;
            this.cbDateLimitLanguage.StyleNormal.ArrowColor = null;
            this.cbDateLimitLanguage.StyleNormal.BackColor = null;
            this.cbDateLimitLanguage.StyleNormal.BorderColor = null;
            this.cbDateLimitLanguage.StyleNormal.ButtonBackColor = null;
            this.cbDateLimitLanguage.StyleNormal.ButtonBorderColor = null;
            this.cbDateLimitLanguage.StyleNormal.ButtonRenderFirst = null;
            this.cbDateLimitLanguage.StyleNormal.ForeColor = null;
            this.cbDateLimitLanguage.StyleSelected.ArrowColor = null;
            this.cbDateLimitLanguage.StyleSelected.BackColor = null;
            this.cbDateLimitLanguage.StyleSelected.BorderColor = null;
            this.cbDateLimitLanguage.StyleSelected.ButtonBackColor = null;
            this.cbDateLimitLanguage.StyleSelected.ButtonBorderColor = null;
            this.cbDateLimitLanguage.StyleSelected.ButtonRenderFirst = null;
            this.cbDateLimitLanguage.StyleSelected.ForeColor = null;
            this.cbDateLimitLanguage.UseDarkScrollBar = false;
            // 
            // exGroupBox4
            // 
            resources.ApplyResources(this.exGroupBox4, "exGroupBox4");
            this.exGroupBox4.Controls.Add(this.nudPlayerWordPause);
            this.exGroupBox4.Controls.Add(this.label7);
            this.exGroupBox4.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.exGroupBox4.Name = "exGroupBox4";
            this.exGroupBox4.TabStop = false;
            // 
            // nudPlayerWordPause
            // 
            this.nudPlayerWordPause.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.nudPlayerWordPause.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            resources.ApplyResources(this.nudPlayerWordPause, "nudPlayerWordPause");
            this.nudPlayerWordPause.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudPlayerWordPause.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nudPlayerWordPause.Name = "nudPlayerWordPause";
            this.nudPlayerWordPause.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // exGroupBox5
            // 
            resources.ApplyResources(this.exGroupBox5, "exGroupBox5");
            this.exGroupBox5.Controls.Add(this.cboxInissEvaluation);
            this.exGroupBox5.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.exGroupBox5.Name = "exGroupBox5";
            this.exGroupBox5.TabStop = false;
            // 
            // cboxInissEvaluation
            // 
            resources.ApplyResources(this.cboxInissEvaluation, "cboxInissEvaluation");
            this.cboxInissEvaluation.BoxBackColor = System.Drawing.Color.White;
            this.cboxInissEvaluation.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxInissEvaluation.Name = "cboxInissEvaluation";
            // 
            // pMcp
            // 
            this.pMcp.Controls.Add(this.mcpPage);
            this.pMcp.Name = "pMcp";
            optionsNodeMcp.Name = "";
            optionsNodeMcp.Text = resources.GetString("optionsNodeMcp.Text");
            this.pMcp.Node = optionsNodeMcp;
            this.pMcp.NodeText = resources.GetString("pMcp.NodeText");
            this.pMcp.ParentNode = null;
            // 
            // mcpPage
            // 
            resources.ApplyResources(this.mcpPage, "mcpPage");
            this.mcpPage.Name = "mcpPage";
            // 
            // FAppSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            resources.ApplyResources(this, "$this");
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FAppSettings";
            this.pGeneral.ResumeLayout(false);
            this.pGeneral.PerformLayout();
            this.pConcreteGeneral.ResumeLayout(false);
            this.pConcreteGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsView)).EndInit();
            this.exGroupBox2.ResumeLayout(false);
            this.exGroupBox2.PerformLayout();
            this.exGroupBox4.ResumeLayout(false);
            this.exGroupBox4.PerformLayout();
            this.exGroupBox5.ResumeLayout(false);
            this.exGroupBox5.PerformLayout();
            this.pMcp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudPlayerWordPause)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ExControls.ExGroupBox exGroupBox2;
        private ExControls.ExCheckBox cboxTabTextAutoGenerate;
        private ExControls.ExComboBox cbDateLimitLanguage;
        private Label label5;
        private ExControls.ExGroupBox exGroupBox4;
        private ExControls.ExNumericUpDown nudPlayerWordPause;
        private Label label7;
        private ExControls.ExGroupBox exGroupBox5;
        private ExControls.ExCheckBox cboxInissEvaluation;
        private ExControls.ExOptionsPanel pMcp;
        private ToolsCore.Mcp.McpSettingsPage mcpPage;
    }
}