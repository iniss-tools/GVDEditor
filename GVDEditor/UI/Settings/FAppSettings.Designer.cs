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
            ExControls.OptionsNode optionsNode2 = new ExControls.OptionsNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAppSettings));
            this.exGroupBox2 = new ExControls.ExGroupBox();
            this.cboxTabTextAutoGenerate = new ExControls.ExCheckBox();
            this.pStartupIniss = new ExControls.ExOptionsPanel(this.optionsView);
            this.ppStartupIniss = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.cboxManualCmdArgs = new ExControls.ExCheckBox();
            this.cboxRunAsAdmin = new ExControls.ExCheckBox();
            this.tbCmdArguments = new ExControls.ExTextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.groupArguments = new ExControls.ExGroupBox();
            this.label4 = new ExControls.ExLabel();
            this.label19 = new System.Windows.Forms.Label();
            this.cbArgRegister = new ExControls.ExComboBox();
            this.cboxArgExportTableTexts = new ExControls.ExCheckBox();
            this.cboxArgAsClient = new ExControls.ExCheckBox();
            this.cboxArgExportHlasTexts = new ExControls.ExCheckBox();
            this.cboxArgMinimize = new ExControls.ExCheckBox();
            this.cboxArgMoreInstances = new ExControls.ExCheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbDateLimitLanguage = new ExControls.ExComboBox();
            this.exGroupBox4 = new ExControls.ExGroupBox();
            this.nudPlayerWordPause = new ExControls.ExNumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.pGeneral.SuspendLayout();
            this.pConcreteGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsView)).BeginInit();
            this.pDesktopComponents.SuspendLayout();
            this.pLocalization.SuspendLayout();
            this.pConcreteLocalization.SuspendLayout();
            this.exGroupBox2.SuspendLayout();
            this.pStartupIniss.SuspendLayout();
            this.ppStartupIniss.SuspendLayout();
            this.groupArguments.SuspendLayout();
            this.exGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlayerWordPause)).BeginInit();
            this.SuspendLayout();
            // 
            // pConcreteGeneral
            // 
            this.pConcreteGeneral.Controls.Add(this.exGroupBox4);
            this.pConcreteGeneral.Controls.Add(this.exGroupBox2);
            resources.ApplyResources(this.pConcreteGeneral, "pConcreteGeneral");
            // 
            // optionsView
            // 
            this.optionsView.HeaderNodeNameVisible = true;
            this.optionsView.Panels.Add(this.pStartupIniss);
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
            // pStartupIniss
            // 
            this.pStartupIniss.Controls.Add(this.ppStartupIniss);
            this.pStartupIniss.Name = "pStartupIniss";
            optionsNode2.ImageKey = "start.png";
            optionsNode2.Name = "";
            optionsNode2.SelectedImageKey = "start.png";
            optionsNode2.Text = resources.GetString("optionsNode2.Text");
            this.pStartupIniss.Node = optionsNode2;
            this.pStartupIniss.NodeText = resources.GetString("pStartupIniss.NodeText");
            this.pStartupIniss.ParentNode = null;
            // 
            // ppStartupIniss
            // 
            this.ppStartupIniss.Controls.Add(this.label6);
            this.ppStartupIniss.Controls.Add(this.cboxManualCmdArgs);
            this.ppStartupIniss.Controls.Add(this.cboxRunAsAdmin);
            this.ppStartupIniss.Controls.Add(this.tbCmdArguments);
            this.ppStartupIniss.Controls.Add(this.label18);
            this.ppStartupIniss.Controls.Add(this.groupArguments);
            resources.ApplyResources(this.ppStartupIniss, "ppStartupIniss");
            this.ppStartupIniss.Name = "ppStartupIniss";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cboxManualCmdArgs
            // 
            resources.ApplyResources(this.cboxManualCmdArgs, "cboxManualCmdArgs");
            this.cboxManualCmdArgs.BoxBackColor = System.Drawing.Color.White;
            this.cboxManualCmdArgs.Checked = true;
            this.cboxManualCmdArgs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxManualCmdArgs.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxManualCmdArgs.Name = "cboxManualCmdArgs";
            this.cboxManualCmdArgs.CheckedChanged += new System.EventHandler(this.CboxManualCmdArgs_CheckedChanged);
            // 
            // cboxRunAsAdmin
            // 
            resources.ApplyResources(this.cboxRunAsAdmin, "cboxRunAsAdmin");
            this.cboxRunAsAdmin.BoxBackColor = System.Drawing.Color.White;
            this.cboxRunAsAdmin.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxRunAsAdmin.Margin = new System.Windows.Forms.Padding(18, 3, 18, 3);
            this.cboxRunAsAdmin.Name = "cboxRunAsAdmin";
            // 
            // tbCmdArguments
            // 
            resources.ApplyResources(this.tbCmdArguments, "tbCmdArguments");
            this.tbCmdArguments.BorderColor = System.Drawing.Color.DimGray;
            this.tbCmdArguments.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbCmdArguments.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbCmdArguments.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCmdArguments.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbCmdArguments.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCmdArguments.HintText = null;
            this.tbCmdArguments.Margin = new System.Windows.Forms.Padding(0);
            this.tbCmdArguments.Name = "tbCmdArguments";
            // 
            // label18
            // 
            this.label18.AutoEllipsis = true;
            resources.ApplyResources(this.label18, "label18");
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Margin = new System.Windows.Forms.Padding(0);
            this.label18.Name = "label18";
            this.label18.Text = "Pozn.: Ak zvolíte pri spustení iný program ako INISS, zadané argumenty nebudú fun" +
    "govať.";
            // 
            // groupArguments
            // 
            resources.ApplyResources(this.groupArguments, "groupArguments");
            this.groupArguments.Controls.Add(this.label4);
            this.groupArguments.Controls.Add(this.label19);
            this.groupArguments.Controls.Add(this.cbArgRegister);
            this.groupArguments.Controls.Add(this.cboxArgExportTableTexts);
            this.groupArguments.Controls.Add(this.cboxArgAsClient);
            this.groupArguments.Controls.Add(this.cboxArgExportHlasTexts);
            this.groupArguments.Controls.Add(this.cboxArgMinimize);
            this.groupArguments.Controls.Add(this.cboxArgMoreInstances);
            this.groupArguments.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupArguments.Enabled = false;
            this.groupArguments.Margin = new System.Windows.Forms.Padding(0);
            this.groupArguments.Name = "groupArguments";
            this.groupArguments.Padding = new System.Windows.Forms.Padding(0);
            this.groupArguments.TabStop = false;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            // 
            // cbArgRegister
            // 
            resources.ApplyResources(this.cbArgRegister, "cbArgRegister");
            this.cbArgRegister.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbArgRegister.FormattingEnabled = true;
            this.cbArgRegister.Margin = new System.Windows.Forms.Padding(0);
            this.cbArgRegister.Name = "cbArgRegister";
            this.cbArgRegister.StyleDisabled.ArrowColor = null;
            this.cbArgRegister.StyleDisabled.BackColor = null;
            this.cbArgRegister.StyleDisabled.BorderColor = null;
            this.cbArgRegister.StyleDisabled.ButtonBackColor = null;
            this.cbArgRegister.StyleDisabled.ButtonBorderColor = null;
            this.cbArgRegister.StyleDisabled.ButtonRenderFirst = null;
            this.cbArgRegister.StyleDisabled.ForeColor = null;
            this.cbArgRegister.StyleHighlight.ArrowColor = null;
            this.cbArgRegister.StyleHighlight.BackColor = null;
            this.cbArgRegister.StyleHighlight.BorderColor = null;
            this.cbArgRegister.StyleHighlight.ButtonBackColor = null;
            this.cbArgRegister.StyleHighlight.ButtonBorderColor = null;
            this.cbArgRegister.StyleHighlight.ButtonRenderFirst = null;
            this.cbArgRegister.StyleHighlight.ForeColor = null;
            this.cbArgRegister.StyleNormal.ArrowColor = null;
            this.cbArgRegister.StyleNormal.BackColor = null;
            this.cbArgRegister.StyleNormal.BorderColor = null;
            this.cbArgRegister.StyleNormal.ButtonBackColor = null;
            this.cbArgRegister.StyleNormal.ButtonBorderColor = null;
            this.cbArgRegister.StyleNormal.ButtonRenderFirst = null;
            this.cbArgRegister.StyleNormal.ForeColor = null;
            this.cbArgRegister.StyleSelected.ArrowColor = null;
            this.cbArgRegister.StyleSelected.BackColor = null;
            this.cbArgRegister.StyleSelected.BorderColor = null;
            this.cbArgRegister.StyleSelected.ButtonBackColor = null;
            this.cbArgRegister.StyleSelected.ButtonBorderColor = null;
            this.cbArgRegister.StyleSelected.ButtonRenderFirst = null;
            this.cbArgRegister.StyleSelected.ForeColor = null;
            this.cbArgRegister.UseDarkScrollBar = false;
            this.cbArgRegister.SelectedIndexChanged += new System.EventHandler(this.ArgsUpdate);
            this.cbArgRegister.TextUpdate += new System.EventHandler(this.ArgsUpdate);
            // 
            // cboxArgExportTableTexts
            // 
            resources.ApplyResources(this.cboxArgExportTableTexts, "cboxArgExportTableTexts");
            this.cboxArgExportTableTexts.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgExportTableTexts.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgExportTableTexts.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboxArgExportTableTexts.Margin = new System.Windows.Forms.Padding(2);
            this.cboxArgExportTableTexts.Name = "cboxArgExportTableTexts";
            this.cboxArgExportTableTexts.UseVisualStyleBackColor = true;
            this.cboxArgExportTableTexts.CheckedChanged += new System.EventHandler(this.ArgsUpdate);
            // 
            // cboxArgAsClient
            // 
            resources.ApplyResources(this.cboxArgAsClient, "cboxArgAsClient");
            this.cboxArgAsClient.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgAsClient.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgAsClient.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboxArgAsClient.Margin = new System.Windows.Forms.Padding(2);
            this.cboxArgAsClient.Name = "cboxArgAsClient";
            this.cboxArgAsClient.UseVisualStyleBackColor = true;
            this.cboxArgAsClient.CheckedChanged += new System.EventHandler(this.ArgsUpdate);
            // 
            // cboxArgExportHlasTexts
            // 
            resources.ApplyResources(this.cboxArgExportHlasTexts, "cboxArgExportHlasTexts");
            this.cboxArgExportHlasTexts.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgExportHlasTexts.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgExportHlasTexts.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboxArgExportHlasTexts.Margin = new System.Windows.Forms.Padding(2);
            this.cboxArgExportHlasTexts.Name = "cboxArgExportHlasTexts";
            this.cboxArgExportHlasTexts.UseVisualStyleBackColor = true;
            this.cboxArgExportHlasTexts.CheckedChanged += new System.EventHandler(this.ArgsUpdate);
            // 
            // cboxArgMinimize
            // 
            resources.ApplyResources(this.cboxArgMinimize, "cboxArgMinimize");
            this.cboxArgMinimize.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgMinimize.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgMinimize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboxArgMinimize.Margin = new System.Windows.Forms.Padding(2);
            this.cboxArgMinimize.Name = "cboxArgMinimize";
            this.cboxArgMinimize.UseVisualStyleBackColor = true;
            this.cboxArgMinimize.CheckedChanged += new System.EventHandler(this.ArgsUpdate);
            // 
            // cboxArgMoreInstances
            // 
            resources.ApplyResources(this.cboxArgMoreInstances, "cboxArgMoreInstances");
            this.cboxArgMoreInstances.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgMoreInstances.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgMoreInstances.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboxArgMoreInstances.Margin = new System.Windows.Forms.Padding(2);
            this.cboxArgMoreInstances.Name = "cboxArgMoreInstances";
            this.cboxArgMoreInstances.UseVisualStyleBackColor = true;
            this.cboxArgMoreInstances.CheckedChanged += new System.EventHandler(this.ArgsUpdate);
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
            this.pStartupIniss.ResumeLayout(false);
            this.ppStartupIniss.ResumeLayout(false);
            this.ppStartupIniss.PerformLayout();
            this.groupArguments.ResumeLayout(false);
            this.groupArguments.PerformLayout();
            this.exGroupBox4.ResumeLayout(false);
            this.exGroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlayerWordPause)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ExControls.ExGroupBox exGroupBox2;
        private ExControls.ExOptionsPanel pStartupIniss;
        private ExControls.ExCheckBox cboxTabTextAutoGenerate;
        private Label label18;
        private ExControls.ExCheckBox cboxRunAsAdmin;
        private ExControls.ExGroupBox groupArguments;
        private ExLabel label4;
        private ExControls.ExTextBox tbCmdArguments;
        private ExControls.ExComboBox cbArgRegister;
        private Label label19;
        private ExControls.ExCheckBox cboxArgExportTableTexts;
        private ExControls.ExCheckBox cboxArgAsClient;
        private ExControls.ExCheckBox cboxArgExportHlasTexts;
        private ExControls.ExCheckBox cboxArgMinimize;
        private ExControls.ExCheckBox cboxArgMoreInstances;
        private Panel ppStartupIniss;
        private ExControls.ExComboBox cbDateLimitLanguage;
        private Label label5;
        private ExControls.ExCheckBox cboxManualCmdArgs;
        private Label label6;
        private ExControls.ExGroupBox exGroupBox4;
        private ExControls.ExNumericUpDown nudPlayerWordPause;
        private Label label7;
    }
}