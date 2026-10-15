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
            this.ppMcp = new System.Windows.Forms.Panel();
            this.lMcpHint = new System.Windows.Forms.Label();
            this.exGroupBox6 = new ExControls.ExGroupBox();
            this.cboxMcpEnabled = new ExControls.ExCheckBox();
            this.lMcpPort = new System.Windows.Forms.Label();
            this.nudMcpPort = new ExControls.ExNumericUpDown();
            this.lMcpStatus = new System.Windows.Forms.Label();
            this.flpMcpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.bMcpClaude = new ExControls.ExButton();
            this.bMcpVsCode = new ExControls.ExButton();
            this.bMcpNewKey = new ExControls.ExButton();
            this.pGeneral.SuspendLayout();
            this.pConcreteGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsView)).BeginInit();
            this.pDesktopComponents.SuspendLayout();
            this.pLocalization.SuspendLayout();
            this.pConcreteLocalization.SuspendLayout();
            this.exGroupBox2.SuspendLayout();
            this.exGroupBox4.SuspendLayout();
            this.exGroupBox5.SuspendLayout();
            this.pMcp.SuspendLayout();
            this.ppMcp.SuspendLayout();
            this.exGroupBox6.SuspendLayout();
            this.flpMcpButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlayerWordPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMcpPort)).BeginInit();
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
            this.optionsView.TreeView.ImageIndex = 0;
            this.optionsView.TreeView.ItemHeight = 20;
            this.optionsView.TreeView.Name = "treeView";
            this.optionsView.TreeView.PathSeparator = " / ";
            this.optionsView.TreeView.SelectedImageIndex = 0;
            this.optionsView.TreeView.ShowLines = false;
            this.optionsView.TreeView.ShowNodeToolTips = true;
            this.optionsView.TreeView.Style = ExControls.ExTreeViewStyle.Light;
            this.optionsView.TreeView.TabIndex = 0;
            // 
            // pConcreteLocalization
            // 
            this.pConcreteLocalization.Controls.Add(this.cbDateLimitLanguage);
            this.pConcreteLocalization.Controls.Add(this.label5);
            // 
            // exGroupBox2
            // 
            resources.ApplyResources(this.exGroupBox2, "exGroupBox2");
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
            this.pMcp.Controls.Add(this.ppMcp);
            this.pMcp.Name = "pMcp";
            optionsNodeMcp.ImageKey = "artificial-intelligence.png";
            optionsNodeMcp.Name = "";
            optionsNodeMcp.SelectedImageKey = "artificial-intelligence.png";
            optionsNodeMcp.Text = resources.GetString("optionsNodeMcp.Text");
            this.pMcp.Node = optionsNodeMcp;
            this.pMcp.NodeText = resources.GetString("pMcp.NodeText");
            this.pMcp.ParentNode = null;
            // 
            // ppMcp
            // 
            this.ppMcp.Controls.Add(this.exGroupBox6);
            this.ppMcp.Controls.Add(this.lMcpHint);
            resources.ApplyResources(this.ppMcp, "ppMcp");
            this.ppMcp.Name = "ppMcp";
            // 
            // lMcpHint
            // 
            resources.ApplyResources(this.lMcpHint, "lMcpHint");
            this.lMcpHint.Name = "lMcpHint";
            // 
            // exGroupBox6
            // 
            resources.ApplyResources(this.exGroupBox6, "exGroupBox6");
            this.exGroupBox6.Controls.Add(this.flpMcpButtons);
            this.exGroupBox6.Controls.Add(this.lMcpStatus);
            this.exGroupBox6.Controls.Add(this.nudMcpPort);
            this.exGroupBox6.Controls.Add(this.lMcpPort);
            this.exGroupBox6.Controls.Add(this.cboxMcpEnabled);
            this.exGroupBox6.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.exGroupBox6.Name = "exGroupBox6";
            this.exGroupBox6.TabStop = false;
            // 
            // cboxMcpEnabled
            // 
            resources.ApplyResources(this.cboxMcpEnabled, "cboxMcpEnabled");
            this.cboxMcpEnabled.BoxBackColor = System.Drawing.Color.White;
            this.cboxMcpEnabled.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxMcpEnabled.Name = "cboxMcpEnabled";
            this.cboxMcpEnabled.CheckedChanged += new System.EventHandler(this.McpSettingChanged);
            // 
            // lMcpPort
            // 
            resources.ApplyResources(this.lMcpPort, "lMcpPort");
            this.lMcpPort.Name = "lMcpPort";
            // 
            // nudMcpPort
            // 
            this.nudMcpPort.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudMcpPort, "nudMcpPort");
            this.nudMcpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudMcpPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nudMcpPort.Name = "nudMcpPort";
            this.nudMcpPort.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            this.nudMcpPort.Value = new decimal(new int[] {
            5490,
            0,
            0,
            0});
            this.nudMcpPort.ValueChanged += new System.EventHandler(this.McpSettingChanged);
            // 
            // lMcpStatus
            // 
            resources.ApplyResources(this.lMcpStatus, "lMcpStatus");
            this.lMcpStatus.Name = "lMcpStatus";
            // 
            // flpMcpButtons
            // 
            resources.ApplyResources(this.flpMcpButtons, "flpMcpButtons");
            this.flpMcpButtons.Controls.Add(this.bMcpClaude);
            this.flpMcpButtons.Controls.Add(this.bMcpVsCode);
            this.flpMcpButtons.Controls.Add(this.bMcpNewKey);
            this.flpMcpButtons.Name = "flpMcpButtons";
            // 
            // bMcpClaude
            // 
            resources.ApplyResources(this.bMcpClaude, "bMcpClaude");
            this.bMcpClaude.Name = "bMcpClaude";
            this.bMcpClaude.UseVisualStyleBackColor = true;
            this.bMcpClaude.Click += new System.EventHandler(this.bMcpClaude_Click);
            // 
            // bMcpVsCode
            // 
            resources.ApplyResources(this.bMcpVsCode, "bMcpVsCode");
            this.bMcpVsCode.Name = "bMcpVsCode";
            this.bMcpVsCode.UseVisualStyleBackColor = true;
            this.bMcpVsCode.Click += new System.EventHandler(this.bMcpVsCode_Click);
            // 
            // bMcpNewKey
            // 
            resources.ApplyResources(this.bMcpNewKey, "bMcpNewKey");
            this.bMcpNewKey.Name = "bMcpNewKey";
            this.bMcpNewKey.UseVisualStyleBackColor = true;
            this.bMcpNewKey.Click += new System.EventHandler(this.bMcpNewKey_Click);
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
            this.pDesktopComponents.ResumeLayout(false);
            this.pDesktopComponents.PerformLayout();
            this.pLocalization.ResumeLayout(false);
            this.pLocalization.PerformLayout();
            this.pConcreteLocalization.ResumeLayout(false);
            this.pConcreteLocalization.PerformLayout();
            this.exGroupBox2.ResumeLayout(false);
            this.exGroupBox2.PerformLayout();
            this.exGroupBox4.ResumeLayout(false);
            this.exGroupBox4.PerformLayout();
            this.exGroupBox5.ResumeLayout(false);
            this.exGroupBox5.PerformLayout();
            this.pMcp.ResumeLayout(false);
            this.ppMcp.ResumeLayout(false);
            this.flpMcpButtons.ResumeLayout(false);
            this.flpMcpButtons.PerformLayout();
            this.exGroupBox6.ResumeLayout(false);
            this.exGroupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlayerWordPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMcpPort)).EndInit();
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
        private Panel ppMcp;
        private Label lMcpHint;
        private ExControls.ExGroupBox exGroupBox6;
        private ExControls.ExCheckBox cboxMcpEnabled;
        private Label lMcpPort;
        private ExControls.ExNumericUpDown nudMcpPort;
        private Label lMcpStatus;
        private FlowLayoutPanel flpMcpButtons;
        private ExControls.ExButton bMcpClaude;
        private ExControls.ExButton bMcpVsCode;
        private ExControls.ExButton bMcpNewKey;
    }
}