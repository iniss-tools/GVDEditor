using ExControls;

namespace GVDEditor.Forms
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
            ExControls.OptionsNode optionsNode1 = new ExControls.OptionsNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAppSettings));
            this.exGroupBox2 = new ExControls.ExGroupBox();
            this.cboxDontCheckTrainIndex = new ExControls.ExCheckBox();
            this.cboxAutoVariant = new ExControls.ExCheckBox();
            this.cboxTabTextAutoGenerate = new ExControls.ExCheckBox();
            this.pStartupIniss = new ExControls.ExOptionsPanel(this.optionsView);
            this.ppStartupIniss = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.cboxManualCmdArgs = new ExControls.ExCheckBox();
            this.cboxRunAsAdmin = new ExControls.ExCheckBox();
            this.tbCmdArguments = new ExControls.ExTextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.groupArguments = new ExControls.ExGroupBox();
            this.label4 = new ExLabel();
            this.label19 = new System.Windows.Forms.Label();
            this.cbArgRegister = new ExControls.ExComboBox();
            this.cboxArgExportTableTexts = new ExControls.ExCheckBox();
            this.cboxArgAsClient = new ExControls.ExCheckBox();
            this.cboxArgExportHlasTexts = new ExControls.ExCheckBox();
            this.cboxArgMinimize = new ExControls.ExCheckBox();
            this.cboxArgMoreInstances = new ExControls.ExCheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbDateLimitLanguage = new ExControls.ExComboBox();
            this.cboxShowDateTimeInStateRow = new ExControls.ExCheckBox();
            this.exGroupBox4 = new ExControls.ExGroupBox();
            this.nudPlayerWordPause = new ExControls.ExNumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.pGeneral.SuspendLayout();
            this.pConcreteGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsView)).BeginInit();
            this.pDesktopComponents.SuspendLayout();
            this.pLocalization.SuspendLayout();
            this.pConcreteLocalization.SuspendLayout();
            this.pConcreteDesktopComponents.SuspendLayout();
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
            this.pConcreteGeneral.Size = new System.Drawing.Size(521, 270);
            // 
            // optionsView
            // 
            this.optionsView.HeaderNodeNameVisible = true;
            this.optionsView.Panels.Add(this.pStartupIniss);
            this.optionsView.SearchBoxVisible = true;
            this.optionsView.Size = new System.Drawing.Size(800, 440);
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
            // pConcreteDesktopComponents
            // 
            this.pConcreteDesktopComponents.Controls.Add(this.cboxShowDateTimeInStateRow);
            // 
            // exGroupBox2
            // 
            this.exGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exGroupBox2.AutoSize = true;
            this.exGroupBox2.Controls.Add(this.cboxDontCheckTrainIndex);
            this.exGroupBox2.Controls.Add(this.cboxAutoVariant);
            this.exGroupBox2.Controls.Add(this.cboxTabTextAutoGenerate);
            this.exGroupBox2.Location = new System.Drawing.Point(0, 3);
            this.exGroupBox2.Name = "exGroupBox2";
            this.exGroupBox2.Size = new System.Drawing.Size(521, 107);
            this.exGroupBox2.TabIndex = 0;
            this.exGroupBox2.TabStop = false;
            this.exGroupBox2.Text = "Grafikon";
            // 
            // cboxDontCheckTrainIndex
            // 
            this.cboxDontCheckTrainIndex.AutoSize = true;
            this.cboxDontCheckTrainIndex.BoxBackColor = System.Drawing.Color.White;
            this.cboxDontCheckTrainIndex.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxDontCheckTrainIndex.Location = new System.Drawing.Point(7, 71);
            this.cboxDontCheckTrainIndex.Margin = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.cboxDontCheckTrainIndex.Name = "cboxDontCheckTrainIndex";
            this.cboxDontCheckTrainIndex.Size = new System.Drawing.Size(212, 17);
            this.cboxDontCheckTrainIndex.TabIndex = 2;
            this.cboxDontCheckTrainIndex.Text = "Nekontrolovať indexy vlakov pri úprave";
            // 
            // cboxAutoVariant
            // 
            this.cboxAutoVariant.AutoSize = true;
            this.cboxAutoVariant.BoxBackColor = System.Drawing.Color.White;
            this.cboxAutoVariant.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxAutoVariant.Location = new System.Drawing.Point(7, 45);
            this.cboxAutoVariant.Name = "cboxAutoVariant";
            this.cboxAutoVariant.Size = new System.Drawing.Size(190, 17);
            this.cboxAutoVariant.TabIndex = 1;
            this.cboxAutoVariant.Text = "Automatická správa variant vlakov";
            // 
            // cboxTabTextAutoGenerate
            // 
            this.cboxTabTextAutoGenerate.AutoSize = true;
            this.cboxTabTextAutoGenerate.BoxBackColor = System.Drawing.Color.White;
            this.cboxTabTextAutoGenerate.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxTabTextAutoGenerate.Location = new System.Drawing.Point(7, 19);
            this.cboxTabTextAutoGenerate.Name = "cboxTabTextAutoGenerate";
            this.cboxTabTextAutoGenerate.Size = new System.Drawing.Size(204, 17);
            this.cboxTabTextAutoGenerate.TabIndex = 0;
            this.cboxTabTextAutoGenerate.Text = "Automaticky generovať texty do tabúľ";
            // 
            // pStartupIniss
            // 
            this.pStartupIniss.Controls.Add(this.ppStartupIniss);
            this.pStartupIniss.Name = "pStartupIniss";
            optionsNode1.ImageKey = "start.png";
            optionsNode1.Name = "";
            optionsNode1.SelectedImageKey = "start.png";
            optionsNode1.Text = "Spúšťanie INISS";
            this.pStartupIniss.Node = optionsNode1;
            this.pStartupIniss.NodeText = "Spúšťanie INISS";
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
            this.ppStartupIniss.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppStartupIniss.Location = new System.Drawing.Point(0, 0);
            this.ppStartupIniss.Name = "ppStartupIniss";
            this.ppStartupIniss.Size = new System.Drawing.Size(521, 399);
            this.ppStartupIniss.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Argumenty:";
            // 
            // cboxManualCmdArgs
            // 
            this.cboxManualCmdArgs.AutoSize = true;
            this.cboxManualCmdArgs.BoxBackColor = System.Drawing.Color.White;
            this.cboxManualCmdArgs.Checked = true;
            this.cboxManualCmdArgs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxManualCmdArgs.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxManualCmdArgs.Location = new System.Drawing.Point(9, 36);
            this.cboxManualCmdArgs.Name = "cboxManualCmdArgs";
            this.cboxManualCmdArgs.Size = new System.Drawing.Size(156, 17);
            this.cboxManualCmdArgs.TabIndex = 10;
            this.cboxManualCmdArgs.Text = "Zadať argumenty manuálne";
            this.cboxManualCmdArgs.CheckedChanged += new System.EventHandler(this.CboxManualCmdArgs_CheckedChanged);
            // 
            // cboxRunAsAdmin
            // 
            this.cboxRunAsAdmin.AutoSize = true;
            this.cboxRunAsAdmin.BoxBackColor = System.Drawing.Color.White;
            this.cboxRunAsAdmin.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxRunAsAdmin.Location = new System.Drawing.Point(9, 13);
            this.cboxRunAsAdmin.Margin = new System.Windows.Forms.Padding(18, 3, 18, 3);
            this.cboxRunAsAdmin.Name = "cboxRunAsAdmin";
            this.cboxRunAsAdmin.Size = new System.Drawing.Size(142, 17);
            this.cboxRunAsAdmin.TabIndex = 7;
            this.cboxRunAsAdmin.Text = "Spustiť ako administrátor";
            // 
            // tbCmdArguments
            // 
            this.tbCmdArguments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCmdArguments.BorderColor = System.Drawing.Color.DimGray;
            this.tbCmdArguments.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbCmdArguments.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbCmdArguments.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCmdArguments.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbCmdArguments.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCmdArguments.HintText = null;
            this.tbCmdArguments.Location = new System.Drawing.Point(86, 56);
            this.tbCmdArguments.Margin = new System.Windows.Forms.Padding(0);
            this.tbCmdArguments.Name = "tbCmdArguments";
            this.tbCmdArguments.Size = new System.Drawing.Size(422, 20);
            this.tbCmdArguments.TabIndex = 1;
            // 
            // label18
            // 
            this.label18.AutoEllipsis = true;
            this.label18.AutoSize = true;
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Location = new System.Drawing.Point(6, 275);
            this.label18.Margin = new System.Windows.Forms.Padding(0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(436, 13);
            this.label18.TabIndex = 9;
            this.label18.Text = "Pozn.: Ak zvolíte pri spustení iný program ako INISS, zadané argumenty nebudú fun" +
    "govať.";
            // 
            // groupArguments
            // 
            this.groupArguments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupArguments.Controls.Add(this.label4);
            this.groupArguments.Controls.Add(this.label19);
            this.groupArguments.Controls.Add(this.cbArgRegister);
            this.groupArguments.Controls.Add(this.cboxArgExportTableTexts);
            this.groupArguments.Controls.Add(this.cboxArgAsClient);
            this.groupArguments.Controls.Add(this.cboxArgExportHlasTexts);
            this.groupArguments.Controls.Add(this.cboxArgMinimize);
            this.groupArguments.Controls.Add(this.cboxArgMoreInstances);
            this.groupArguments.Enabled = false;
            this.groupArguments.Location = new System.Drawing.Point(9, 88);
            this.groupArguments.Margin = new System.Windows.Forms.Padding(0);
            this.groupArguments.Name = "groupArguments";
            this.groupArguments.Padding = new System.Windows.Forms.Padding(0);
            this.groupArguments.Size = new System.Drawing.Size(499, 175);
            this.groupArguments.TabIndex = 8;
            this.groupArguments.TabStop = false;
            this.groupArguments.Text = "Grafické zadanie argumentov";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Spustiť s registrom:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label19.Location = new System.Drawing.Point(1, 439);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(96, 13);
            this.label19.TabIndex = 12;
            this.label19.Text = "Spustiť s registrom:";
            // 
            // cbArgRegister
            // 
            this.cbArgRegister.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbArgRegister.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbArgRegister.FormattingEnabled = true;
            this.cbArgRegister.Location = new System.Drawing.Point(123, 142);
            this.cbArgRegister.Margin = new System.Windows.Forms.Padding(0);
            this.cbArgRegister.Name = "cbArgRegister";
            this.cbArgRegister.Size = new System.Drawing.Size(364, 21);
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
            this.cbArgRegister.TabIndex = 13;
            this.cbArgRegister.UseDarkScrollBar = false;
            this.cbArgRegister.SelectedIndexChanged += new System.EventHandler(this.ArgsUpdate);
            this.cbArgRegister.TextUpdate += new System.EventHandler(this.ArgsUpdate);
            // 
            // cboxArgExportTableTexts
            // 
            this.cboxArgExportTableTexts.AutoSize = true;
            this.cboxArgExportTableTexts.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgExportTableTexts.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgExportTableTexts.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboxArgExportTableTexts.Location = new System.Drawing.Point(16, 119);
            this.cboxArgExportTableTexts.Margin = new System.Windows.Forms.Padding(2);
            this.cboxArgExportTableTexts.Name = "cboxArgExportTableTexts";
            this.cboxArgExportTableTexts.Size = new System.Drawing.Size(315, 17);
            this.cboxArgExportTableTexts.TabIndex = 11;
            this.cboxArgExportTableTexts.Text = "Exportovať texty do tabúľ pri úprave šablón vlakov do súboru";
            this.cboxArgExportTableTexts.UseVisualStyleBackColor = true;
            this.cboxArgExportTableTexts.CheckedChanged += new System.EventHandler(this.ArgsUpdate);
            // 
            // cboxArgAsClient
            // 
            this.cboxArgAsClient.AutoSize = true;
            this.cboxArgAsClient.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgAsClient.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgAsClient.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboxArgAsClient.Location = new System.Drawing.Point(16, 96);
            this.cboxArgAsClient.Margin = new System.Windows.Forms.Padding(2);
            this.cboxArgAsClient.Name = "cboxArgAsClient";
            this.cboxArgAsClient.Size = new System.Drawing.Size(149, 17);
            this.cboxArgAsClient.TabIndex = 10;
            this.cboxArgAsClient.Text = "Spustiť program ako klient";
            this.cboxArgAsClient.UseVisualStyleBackColor = true;
            this.cboxArgAsClient.CheckedChanged += new System.EventHandler(this.ArgsUpdate);
            // 
            // cboxArgExportHlasTexts
            // 
            this.cboxArgExportHlasTexts.AutoSize = true;
            this.cboxArgExportHlasTexts.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgExportHlasTexts.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgExportHlasTexts.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboxArgExportHlasTexts.Location = new System.Drawing.Point(16, 73);
            this.cboxArgExportHlasTexts.Margin = new System.Windows.Forms.Padding(2);
            this.cboxArgExportHlasTexts.Name = "cboxArgExportHlasTexts";
            this.cboxArgExportHlasTexts.Size = new System.Drawing.Size(285, 17);
            this.cboxArgExportHlasTexts.TabIndex = 9;
            this.cboxArgExportHlasTexts.Text = "Exportovať text všetkých hlásení do súboru TxtHlas.txt";
            this.cboxArgExportHlasTexts.UseVisualStyleBackColor = true;
            this.cboxArgExportHlasTexts.CheckedChanged += new System.EventHandler(this.ArgsUpdate);
            // 
            // cboxArgMinimize
            // 
            this.cboxArgMinimize.AutoSize = true;
            this.cboxArgMinimize.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgMinimize.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgMinimize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboxArgMinimize.Location = new System.Drawing.Point(16, 52);
            this.cboxArgMinimize.Margin = new System.Windows.Forms.Padding(2);
            this.cboxArgMinimize.Name = "cboxArgMinimize";
            this.cboxArgMinimize.Size = new System.Drawing.Size(222, 17);
            this.cboxArgMinimize.TabIndex = 8;
            this.cboxArgMinimize.Text = "Minimalizovať okno programu pri spustení";
            this.cboxArgMinimize.UseVisualStyleBackColor = true;
            this.cboxArgMinimize.CheckedChanged += new System.EventHandler(this.ArgsUpdate);
            // 
            // cboxArgMoreInstances
            // 
            this.cboxArgMoreInstances.AutoSize = true;
            this.cboxArgMoreInstances.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgMoreInstances.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgMoreInstances.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboxArgMoreInstances.Location = new System.Drawing.Point(16, 27);
            this.cboxArgMoreInstances.Margin = new System.Windows.Forms.Padding(2);
            this.cboxArgMoreInstances.Name = "cboxArgMoreInstances";
            this.cboxArgMoreInstances.Size = new System.Drawing.Size(199, 17);
            this.cboxArgMoreInstances.TabIndex = 7;
            this.cboxArgMoreInstances.Text = "Povoliť duplicitné inštancie programu";
            this.cboxArgMoreInstances.UseVisualStyleBackColor = true;
            this.cboxArgMoreInstances.CheckedChanged += new System.EventHandler(this.ArgsUpdate);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(159, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Jazyk dátumového obmedzenia:";
            // 
            // cbDateLimitLanguage
            // 
            this.cbDateLimitLanguage.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbDateLimitLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDateLimitLanguage.FormattingEnabled = true;
            this.cbDateLimitLanguage.Location = new System.Drawing.Point(6, 19);
            this.cbDateLimitLanguage.Name = "cbDateLimitLanguage";
            this.cbDateLimitLanguage.Size = new System.Drawing.Size(202, 21);
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
            this.cbDateLimitLanguage.TabIndex = 1;
            this.cbDateLimitLanguage.UseDarkScrollBar = false;
            // 
            // cboxShowDateTimeInStateRow
            // 
            this.cboxShowDateTimeInStateRow.AutoSize = true;
            this.cboxShowDateTimeInStateRow.BoxBackColor = System.Drawing.Color.White;
            this.cboxShowDateTimeInStateRow.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxShowDateTimeInStateRow.Location = new System.Drawing.Point(16, 3);
            this.cboxShowDateTimeInStateRow.Name = "cboxShowDateTimeInStateRow";
            this.cboxShowDateTimeInStateRow.Size = new System.Drawing.Size(191, 17);
            this.cboxShowDateTimeInStateRow.TabIndex = 0;
            this.cboxShowDateTimeInStateRow.Text = "Zobrazovať čas v stavovom riadku";
            // 
            // exGroupBox4
            // 
            this.exGroupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exGroupBox4.AutoSize = true;
            this.exGroupBox4.Controls.Add(this.nudPlayerWordPause);
            this.exGroupBox4.Controls.Add(this.label7);
            this.exGroupBox4.Location = new System.Drawing.Point(0, 116);
            this.exGroupBox4.Name = "exGroupBox4";
            this.exGroupBox4.Size = new System.Drawing.Size(521, 67);
            this.exGroupBox4.TabIndex = 1;
            this.exGroupBox4.TabStop = false;
            this.exGroupBox4.Text = "Prehrávanie";
            // 
            // nudPlayerWordPause
            // 
            this.nudPlayerWordPause.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.nudPlayerWordPause.Location = new System.Drawing.Point(218, 28);
            this.nudPlayerWordPause.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudPlayerWordPause.Name = "nudPlayerWordPause";
            this.nudPlayerWordPause.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            this.nudPlayerWordPause.Size = new System.Drawing.Size(82, 20);
            this.nudPlayerWordPause.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(205, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Medzera medzi prehrávanými zvukmi (ms):";
            // 
            // FAppSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 475);
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
            this.pConcreteDesktopComponents.ResumeLayout(false);
            this.pConcreteDesktopComponents.PerformLayout();
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
        private ExControls.ExCheckBox cboxDontCheckTrainIndex;
        private ExControls.ExCheckBox cboxAutoVariant;
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
        private ExControls.ExCheckBox cboxShowDateTimeInStateRow;
        private ExControls.ExGroupBox exGroupBox4;
        private ExControls.ExNumericUpDown nudPlayerWordPause;
        private Label label7;
    }
}