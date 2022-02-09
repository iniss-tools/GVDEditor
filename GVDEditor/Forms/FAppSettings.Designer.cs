using ExControls;
using GVDEditor.Properties;
using GVDEditor.XML;
using ToolsCore.XML;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAppSettings));
            ExControls.ExComboBoxStyle exComboBoxStyle1 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle2 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle3 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle4 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle5 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle6 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle7 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle8 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle9 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle10 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle11 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle12 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle13 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle14 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle15 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle16 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle17 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle18 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle19 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle20 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle21 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle22 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle23 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle24 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle25 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle26 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle27 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle28 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle29 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle30 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle31 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle32 = new ExControls.ExComboBoxStyle();
            this.bSave = new ExControls.ExButton();
            this.bStorno = new ExControls.ExButton();
            this.cbDateRemLocate = new ExControls.ExComboBox();
            this.cbAppLanguage = new ExControls.ExComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbLoggingError = new ExControls.ExCheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbDebugModeGUI = new ExControls.ExComboBox();
            this.cbLoggingInfo = new ExControls.ExCheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudWordPause = new ExControls.ExNumericUpDown();
            this.cbAutoTableText = new ExControls.ExCheckBox();
            this.cbClassicGUI = new ExControls.ExCheckBox();
            this.cbMoreInstance = new ExControls.ExCheckBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.treeMenu = new System.Windows.Forms.TreeView();
            this.imagesMenu = new System.Windows.Forms.ImageList(this.components);
            this.pObecne = new System.Windows.Forms.Panel();
            this.cbCheckUpdate = new ExControls.ExCheckBox();
            this.cbAutoVariant = new ExControls.ExCheckBox();
            this.cbDisableVariantCheck = new ExControls.ExCheckBox();
            this.pEnvironment = new System.Windows.Forms.Panel();
            this.dgvAppFonts = new System.Windows.Forms.DataGridView();
            this.FontDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Example = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bAppFontDefault = new ExControls.ExButton();
            this.label5 = new System.Windows.Forms.Label();
            this.pLocalization = new System.Windows.Forms.Panel();
            this.pFontsColors = new System.Windows.Forms.Panel();
            this.bUseStyle = new ExControls.ExButton();
            this.bColorsUseDefault = new ExControls.ExButton();
            this.exGroupBox2 = new ExControls.ExGroupBox();
            this.cboxDarkScrollBars = new ExControls.ExCheckBox();
            this.cboxDarkTitleBar = new ExControls.ExCheckBox();
            this.cboxDefaultStyle = new ExControls.ExCheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbColorSettings = new ExControls.ExComboBox();
            this.bRemoveStyle = new ExControls.ExButton();
            this.bRenameStyle = new ExControls.ExButton();
            this.bAddStyle = new ExControls.ExButton();
            this.label17 = new System.Windows.Forms.Label();
            this.cbStyles = new ExControls.ExComboBox();
            this.exGroupBox3 = new ExControls.ExGroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbFontSize = new ExControls.ExComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.exGroupBox1 = new ExControls.ExGroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.pbBackgroundColor = new System.Windows.Forms.PictureBox();
            this.bSetBackgroundColor = new ExControls.ExButton();
            this.label16 = new System.Windows.Forms.Label();
            this.pbForegroundColor = new System.Windows.Forms.PictureBox();
            this.bSetForegroundColor = new ExControls.ExButton();
            this.cbBold = new ExControls.ExCheckBox();
            this.cbFont = new ExControls.ExComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.listSettings = new System.Windows.Forms.ListBox();
            this.label13 = new System.Windows.Forms.Label();
            this.labelExample = new System.Windows.Forms.Label();
            this.pLogging = new System.Windows.Forms.Panel();
            this.pPlaying = new System.Windows.Forms.Panel();
            this.deviceInformationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.appFontDialog = new System.Windows.Forms.FontDialog();
            this.pDesktop = new System.Windows.Forms.Panel();
            this.cbFitLastColumn = new ExControls.ExCheckBox();
            this.bColumnDown = new ExControls.ExButton();
            this.bColumnUp = new ExControls.ExButton();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvDesktopColums = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MinWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new ExControls.ExGroupBox();
            this.cboxDateTimeInStateRow = new ExControls.ExCheckBox();
            this.cbShowRowsHeader = new ExControls.ExCheckBox();
            this.rbTS = new ExControls.ExRadioButton();
            this.rbMS = new ExControls.ExRadioButton();
            this.rbMSandTS = new ExControls.ExRadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.cbShowStateRow = new ExControls.ExCheckBox();
            this.pShortcuts = new System.Windows.Forms.Panel();
            this.lShortcutHelp = new System.Windows.Forms.Label();
            this.dgvShortcuts = new System.Windows.Forms.DataGridView();
            this.AssignShortcut = new System.Windows.Forms.DataGridViewButtonColumn();
            this.RemoveShortcut = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Default = new System.Windows.Forms.DataGridViewButtonColumn();
            this.bAllShortcutsSetDefault = new ExControls.ExButton();
            this.label8 = new System.Windows.Forms.Label();
            this.pStartup = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.groupArguments = new ExControls.ExGroupBox();
            this.cbArgRegister = new ExControls.ExComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.cboxArgExportTableTexts = new ExControls.ExCheckBox();
            this.cboxArgAsClient = new ExControls.ExCheckBox();
            this.cboxArgExportHlasTexts = new ExControls.ExCheckBox();
            this.cboxArgMinimize = new ExControls.ExCheckBox();
            this.cboxArgMoreInstances = new ExControls.ExCheckBox();
            this.cboxManualCmdArgs = new ExControls.ExCheckBox();
            this.tbCmdArguments = new ExControls.ExTextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cbStartINISSAdmin = new ExControls.ExCheckBox();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.visibleDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.nameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shortcutDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commandShortcutBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.desktopColumnBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.appFontBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nudWordPause)).BeginInit();
            this.pObecne.SuspendLayout();
            this.pEnvironment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppFonts)).BeginInit();
            this.pLocalization.SuspendLayout();
            this.pFontsColors.SuspendLayout();
            this.exGroupBox2.SuspendLayout();
            this.exGroupBox3.SuspendLayout();
            this.exGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackgroundColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForegroundColor)).BeginInit();
            this.pLogging.SuspendLayout();
            this.pPlaying.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deviceInformationBindingSource)).BeginInit();
            this.pDesktop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDesktopColums)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.pShortcuts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShortcuts)).BeginInit();
            this.pStartup.SuspendLayout();
            this.groupArguments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.commandShortcutBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.desktopColumnBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.appFontBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // bSave
            // 
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.Name = "bSave";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bStorno
            // 
            resources.ApplyResources(this.bStorno, "bStorno");
            this.bStorno.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bStorno.Name = "bStorno";
            this.bStorno.UseVisualStyleBackColor = true;
            this.bStorno.Click += new System.EventHandler(this.bStorno_Click);
            // 
            // cbDateRemLocate
            // 
            this.cbDateRemLocate.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbDateRemLocate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDateRemLocate.FormattingEnabled = true;
            this.cbDateRemLocate.Items.AddRange(new object[] {
            resources.GetString("cbDateRemLocate.Items"),
            resources.GetString("cbDateRemLocate.Items1")});
            resources.ApplyResources(this.cbDateRemLocate, "cbDateRemLocate");
            this.cbDateRemLocate.Name = "cbDateRemLocate";
            exComboBoxStyle1.ArrowColor = null;
            exComboBoxStyle1.BackColor = null;
            exComboBoxStyle1.BorderColor = null;
            exComboBoxStyle1.ButtonBackColor = null;
            exComboBoxStyle1.ButtonBorderColor = null;
            exComboBoxStyle1.ButtonRenderFirst = null;
            exComboBoxStyle1.ForeColor = null;
            this.cbDateRemLocate.StyleDisabled = exComboBoxStyle1;
            exComboBoxStyle2.ArrowColor = null;
            exComboBoxStyle2.BackColor = null;
            exComboBoxStyle2.BorderColor = null;
            exComboBoxStyle2.ButtonBackColor = null;
            exComboBoxStyle2.ButtonBorderColor = null;
            exComboBoxStyle2.ButtonRenderFirst = null;
            exComboBoxStyle2.ForeColor = null;
            this.cbDateRemLocate.StyleHighlight = exComboBoxStyle2;
            exComboBoxStyle3.ArrowColor = null;
            exComboBoxStyle3.BackColor = null;
            exComboBoxStyle3.BorderColor = null;
            exComboBoxStyle3.ButtonBackColor = null;
            exComboBoxStyle3.ButtonBorderColor = null;
            exComboBoxStyle3.ButtonRenderFirst = null;
            exComboBoxStyle3.ForeColor = null;
            this.cbDateRemLocate.StyleNormal = exComboBoxStyle3;
            exComboBoxStyle4.ArrowColor = null;
            exComboBoxStyle4.BackColor = null;
            exComboBoxStyle4.BorderColor = null;
            exComboBoxStyle4.ButtonBackColor = null;
            exComboBoxStyle4.ButtonBorderColor = null;
            exComboBoxStyle4.ButtonRenderFirst = null;
            exComboBoxStyle4.ForeColor = null;
            this.cbDateRemLocate.StyleSelected = exComboBoxStyle4;
            this.cbDateRemLocate.UseDarkScrollBar = false;
            // 
            // cbAppLanguage
            // 
            this.cbAppLanguage.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbAppLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAppLanguage.FormattingEnabled = true;
            this.cbAppLanguage.Items.AddRange(new object[] {
            resources.GetString("cbAppLanguage.Items"),
            resources.GetString("cbAppLanguage.Items1")});
            resources.ApplyResources(this.cbAppLanguage, "cbAppLanguage");
            this.cbAppLanguage.Name = "cbAppLanguage";
            exComboBoxStyle5.ArrowColor = null;
            exComboBoxStyle5.BackColor = null;
            exComboBoxStyle5.BorderColor = null;
            exComboBoxStyle5.ButtonBackColor = null;
            exComboBoxStyle5.ButtonBorderColor = null;
            exComboBoxStyle5.ButtonRenderFirst = null;
            exComboBoxStyle5.ForeColor = null;
            this.cbAppLanguage.StyleDisabled = exComboBoxStyle5;
            exComboBoxStyle6.ArrowColor = null;
            exComboBoxStyle6.BackColor = null;
            exComboBoxStyle6.BorderColor = null;
            exComboBoxStyle6.ButtonBackColor = null;
            exComboBoxStyle6.ButtonBorderColor = null;
            exComboBoxStyle6.ButtonRenderFirst = null;
            exComboBoxStyle6.ForeColor = null;
            this.cbAppLanguage.StyleHighlight = exComboBoxStyle6;
            exComboBoxStyle7.ArrowColor = null;
            exComboBoxStyle7.BackColor = null;
            exComboBoxStyle7.BorderColor = null;
            exComboBoxStyle7.ButtonBackColor = null;
            exComboBoxStyle7.ButtonBorderColor = null;
            exComboBoxStyle7.ButtonRenderFirst = null;
            exComboBoxStyle7.ForeColor = null;
            this.cbAppLanguage.StyleNormal = exComboBoxStyle7;
            exComboBoxStyle8.ArrowColor = null;
            exComboBoxStyle8.BackColor = null;
            exComboBoxStyle8.BorderColor = null;
            exComboBoxStyle8.ButtonBackColor = null;
            exComboBoxStyle8.ButtonBorderColor = null;
            exComboBoxStyle8.ButtonRenderFirst = null;
            exComboBoxStyle8.ForeColor = null;
            this.cbAppLanguage.StyleSelected = exComboBoxStyle8;
            this.cbAppLanguage.UseDarkScrollBar = false;
            this.cbAppLanguage.SelectedIndexChanged += new System.EventHandler(this.cbAppLanguage_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cbLoggingError
            // 
            resources.ApplyResources(this.cbLoggingError, "cbLoggingError");
            this.cbLoggingError.BoxBackColor = System.Drawing.Color.White;
            this.cbLoggingError.Checked = true;
            this.cbLoggingError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLoggingError.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbLoggingError.Name = "cbLoggingError";
            this.cbLoggingError.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cbDebugModeGUI
            // 
            this.cbDebugModeGUI.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbDebugModeGUI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDebugModeGUI.FormattingEnabled = true;
            this.cbDebugModeGUI.Items.AddRange(new object[] {
            resources.GetString("cbDebugModeGUI.Items"),
            resources.GetString("cbDebugModeGUI.Items1"),
            resources.GetString("cbDebugModeGUI.Items2")});
            resources.ApplyResources(this.cbDebugModeGUI, "cbDebugModeGUI");
            this.cbDebugModeGUI.Name = "cbDebugModeGUI";
            exComboBoxStyle9.ArrowColor = null;
            exComboBoxStyle9.BackColor = null;
            exComboBoxStyle9.BorderColor = null;
            exComboBoxStyle9.ButtonBackColor = null;
            exComboBoxStyle9.ButtonBorderColor = null;
            exComboBoxStyle9.ButtonRenderFirst = null;
            exComboBoxStyle9.ForeColor = null;
            this.cbDebugModeGUI.StyleDisabled = exComboBoxStyle9;
            exComboBoxStyle10.ArrowColor = null;
            exComboBoxStyle10.BackColor = null;
            exComboBoxStyle10.BorderColor = null;
            exComboBoxStyle10.ButtonBackColor = null;
            exComboBoxStyle10.ButtonBorderColor = null;
            exComboBoxStyle10.ButtonRenderFirst = null;
            exComboBoxStyle10.ForeColor = null;
            this.cbDebugModeGUI.StyleHighlight = exComboBoxStyle10;
            exComboBoxStyle11.ArrowColor = null;
            exComboBoxStyle11.BackColor = null;
            exComboBoxStyle11.BorderColor = null;
            exComboBoxStyle11.ButtonBackColor = null;
            exComboBoxStyle11.ButtonBorderColor = null;
            exComboBoxStyle11.ButtonRenderFirst = null;
            exComboBoxStyle11.ForeColor = null;
            this.cbDebugModeGUI.StyleNormal = exComboBoxStyle11;
            exComboBoxStyle12.ArrowColor = null;
            exComboBoxStyle12.BackColor = null;
            exComboBoxStyle12.BorderColor = null;
            exComboBoxStyle12.ButtonBackColor = null;
            exComboBoxStyle12.ButtonBorderColor = null;
            exComboBoxStyle12.ButtonRenderFirst = null;
            exComboBoxStyle12.ForeColor = null;
            this.cbDebugModeGUI.StyleSelected = exComboBoxStyle12;
            this.cbDebugModeGUI.UseDarkScrollBar = false;
            this.cbDebugModeGUI.SelectedIndexChanged += new System.EventHandler(this.cbDebugModeGUI_SelectedIndexChanged);
            // 
            // cbLoggingInfo
            // 
            resources.ApplyResources(this.cbLoggingInfo, "cbLoggingInfo");
            this.cbLoggingInfo.BoxBackColor = System.Drawing.Color.White;
            this.cbLoggingInfo.Checked = true;
            this.cbLoggingInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLoggingInfo.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbLoggingInfo.Name = "cbLoggingInfo";
            this.cbLoggingInfo.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // nudWordPause
            // 
            this.nudWordPause.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudWordPause, "nudWordPause");
            this.nudWordPause.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudWordPause.Name = "nudWordPause";
            this.nudWordPause.SelectedButtonColor = System.Drawing.Color.Empty;
            // 
            // cbAutoTableText
            // 
            resources.ApplyResources(this.cbAutoTableText, "cbAutoTableText");
            this.cbAutoTableText.BoxBackColor = System.Drawing.Color.White;
            this.cbAutoTableText.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbAutoTableText.Name = "cbAutoTableText";
            this.cbAutoTableText.UseVisualStyleBackColor = true;
            // 
            // cbClassicGUI
            // 
            resources.ApplyResources(this.cbClassicGUI, "cbClassicGUI");
            this.cbClassicGUI.BoxBackColor = System.Drawing.Color.White;
            this.cbClassicGUI.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbClassicGUI.Name = "cbClassicGUI";
            this.cbClassicGUI.UseVisualStyleBackColor = true;
            this.cbClassicGUI.CheckedChanged += new System.EventHandler(this.cbClassicGUI_CheckedChanged);
            // 
            // cbMoreInstance
            // 
            resources.ApplyResources(this.cbMoreInstance, "cbMoreInstance");
            this.cbMoreInstance.BoxBackColor = System.Drawing.Color.White;
            this.cbMoreInstance.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbMoreInstance.Name = "cbMoreInstance";
            this.cbMoreInstance.UseVisualStyleBackColor = true;
            // 
            // colorDialog
            // 
            this.colorDialog.SolidColorOnly = true;
            // 
            // treeMenu
            // 
            this.treeMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeMenu.FullRowSelect = true;
            this.treeMenu.HideSelection = false;
            resources.ApplyResources(this.treeMenu, "treeMenu");
            this.treeMenu.ImageList = this.imagesMenu;
            this.treeMenu.Name = "treeMenu";
            this.treeMenu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeMenu.Nodes"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeMenu.Nodes1"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeMenu.Nodes2"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeMenu.Nodes3"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeMenu.Nodes4")))});
            this.treeMenu.ShowLines = false;
            this.treeMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeMenu_AfterSelect);
            // 
            // imagesMenu
            // 
            this.imagesMenu.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagesMenu.ImageStream")));
            this.imagesMenu.TransparentColor = System.Drawing.Color.Transparent;
            this.imagesMenu.Images.SetKeyName(0, "colors.png");
            this.imagesMenu.Images.SetKeyName(1, "debugging.png");
            this.imagesMenu.Images.SetKeyName(2, "desktop.png");
            this.imagesMenu.Images.SetKeyName(3, "environment.png");
            this.imagesMenu.Images.SetKeyName(4, "general.png");
            this.imagesMenu.Images.SetKeyName(5, "iniss-settings.png");
            this.imagesMenu.Images.SetKeyName(6, "localization.png");
            this.imagesMenu.Images.SetKeyName(7, "shortcut.png");
            this.imagesMenu.Images.SetKeyName(8, "start.png");
            // 
            // pObecne
            // 
            this.pObecne.Controls.Add(this.cbCheckUpdate);
            this.pObecne.Controls.Add(this.cbAutoVariant);
            this.pObecne.Controls.Add(this.cbDisableVariantCheck);
            this.pObecne.Controls.Add(this.cbClassicGUI);
            this.pObecne.Controls.Add(this.cbMoreInstance);
            this.pObecne.Controls.Add(this.cbAutoTableText);
            resources.ApplyResources(this.pObecne, "pObecne");
            this.pObecne.Name = "pObecne";
            // 
            // cbCheckUpdate
            // 
            resources.ApplyResources(this.cbCheckUpdate, "cbCheckUpdate");
            this.cbCheckUpdate.BoxBackColor = System.Drawing.Color.White;
            this.cbCheckUpdate.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbCheckUpdate.Name = "cbCheckUpdate";
            this.cbCheckUpdate.UseVisualStyleBackColor = true;
            // 
            // cbAutoVariant
            // 
            resources.ApplyResources(this.cbAutoVariant, "cbAutoVariant");
            this.cbAutoVariant.BoxBackColor = System.Drawing.Color.White;
            this.cbAutoVariant.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbAutoVariant.Name = "cbAutoVariant";
            this.cbAutoVariant.UseVisualStyleBackColor = true;
            // 
            // cbDisableVariantCheck
            // 
            resources.ApplyResources(this.cbDisableVariantCheck, "cbDisableVariantCheck");
            this.cbDisableVariantCheck.BoxBackColor = System.Drawing.Color.White;
            this.cbDisableVariantCheck.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbDisableVariantCheck.Name = "cbDisableVariantCheck";
            this.cbDisableVariantCheck.UseVisualStyleBackColor = true;
            // 
            // pEnvironment
            // 
            this.pEnvironment.Controls.Add(this.dgvAppFonts);
            this.pEnvironment.Controls.Add(this.bAppFontDefault);
            this.pEnvironment.Controls.Add(this.label5);
            resources.ApplyResources(this.pEnvironment, "pEnvironment");
            this.pEnvironment.Name = "pEnvironment";
            // 
            // dgvAppFonts
            // 
            this.dgvAppFonts.AllowUserToAddRows = false;
            this.dgvAppFonts.AllowUserToDeleteRows = false;
            this.dgvAppFonts.AllowUserToResizeRows = false;
            this.dgvAppFonts.AutoGenerateColumns = false;
            this.dgvAppFonts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAppFonts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAppFonts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.FontDescription,
            this.Example});
            this.dgvAppFonts.DataSource = this.appFontBindingSource;
            resources.ApplyResources(this.dgvAppFonts, "dgvAppFonts");
            this.dgvAppFonts.Name = "dgvAppFonts";
            this.dgvAppFonts.ReadOnly = true;
            this.dgvAppFonts.RowHeadersVisible = false;
            this.dgvAppFonts.RowTemplate.Height = 24;
            this.dgvAppFonts.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAppFonts_CellDoubleClick);
            // 
            // FontDescription
            // 
            this.FontDescription.DataPropertyName = "FontDescription";
            resources.ApplyResources(this.FontDescription, "FontDescription");
            this.FontDescription.Name = "FontDescription";
            this.FontDescription.ReadOnly = true;
            // 
            // Example
            // 
            this.Example.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Example.DataPropertyName = "Example";
            resources.ApplyResources(this.Example, "Example");
            this.Example.Name = "Example";
            this.Example.ReadOnly = true;
            // 
            // bAppFontDefault
            // 
            resources.ApplyResources(this.bAppFontDefault, "bAppFontDefault");
            this.bAppFontDefault.Name = "bAppFontDefault";
            this.bAppFontDefault.UseVisualStyleBackColor = true;
            this.bAppFontDefault.Click += new System.EventHandler(this.bAppFontDefault_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // pLocalization
            // 
            this.pLocalization.Controls.Add(this.cbDateRemLocate);
            this.pLocalization.Controls.Add(this.label1);
            this.pLocalization.Controls.Add(this.label2);
            this.pLocalization.Controls.Add(this.cbAppLanguage);
            resources.ApplyResources(this.pLocalization, "pLocalization");
            this.pLocalization.Name = "pLocalization";
            // 
            // pFontsColors
            // 
            this.pFontsColors.Controls.Add(this.bUseStyle);
            this.pFontsColors.Controls.Add(this.bColorsUseDefault);
            this.pFontsColors.Controls.Add(this.exGroupBox2);
            this.pFontsColors.Controls.Add(this.label9);
            this.pFontsColors.Controls.Add(this.cbColorSettings);
            this.pFontsColors.Controls.Add(this.bRemoveStyle);
            this.pFontsColors.Controls.Add(this.bRenameStyle);
            this.pFontsColors.Controls.Add(this.bAddStyle);
            this.pFontsColors.Controls.Add(this.label17);
            this.pFontsColors.Controls.Add(this.cbStyles);
            this.pFontsColors.Controls.Add(this.exGroupBox3);
            resources.ApplyResources(this.pFontsColors, "pFontsColors");
            this.pFontsColors.Name = "pFontsColors";
            // 
            // bUseStyle
            // 
            resources.ApplyResources(this.bUseStyle, "bUseStyle");
            this.bUseStyle.Name = "bUseStyle";
            this.bUseStyle.UseVisualStyleBackColor = true;
            this.bUseStyle.Click += new System.EventHandler(this.bUseStyle_Click);
            // 
            // bColorsUseDefault
            // 
            resources.ApplyResources(this.bColorsUseDefault, "bColorsUseDefault");
            this.bColorsUseDefault.Name = "bColorsUseDefault";
            this.bColorsUseDefault.UseVisualStyleBackColor = true;
            this.bColorsUseDefault.Click += new System.EventHandler(this.bColorsUseDefault_Click);
            // 
            // exGroupBox2
            // 
            this.exGroupBox2.Controls.Add(this.cboxDarkScrollBars);
            this.exGroupBox2.Controls.Add(this.cboxDarkTitleBar);
            this.exGroupBox2.Controls.Add(this.cboxDefaultStyle);
            resources.ApplyResources(this.exGroupBox2, "exGroupBox2");
            this.exGroupBox2.Name = "exGroupBox2";
            this.exGroupBox2.TabStop = false;
            // 
            // cboxDarkScrollBars
            // 
            resources.ApplyResources(this.cboxDarkScrollBars, "cboxDarkScrollBars");
            this.cboxDarkScrollBars.BoxBackColor = System.Drawing.Color.White;
            this.cboxDarkScrollBars.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxDarkScrollBars.Name = "cboxDarkScrollBars";
            this.cboxDarkScrollBars.UseVisualStyleBackColor = true;
            this.cboxDarkScrollBars.CheckedChanged += new System.EventHandler(this.cboxDarkScrollBars_CheckedChanged);
            // 
            // cboxDarkTitleBar
            // 
            resources.ApplyResources(this.cboxDarkTitleBar, "cboxDarkTitleBar");
            this.cboxDarkTitleBar.BoxBackColor = System.Drawing.Color.White;
            this.cboxDarkTitleBar.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxDarkTitleBar.Name = "cboxDarkTitleBar";
            this.cboxDarkTitleBar.UseVisualStyleBackColor = true;
            this.cboxDarkTitleBar.CheckedChanged += new System.EventHandler(this.cboxDarkTitleBar_CheckedChanged);
            // 
            // cboxDefaultStyle
            // 
            resources.ApplyResources(this.cboxDefaultStyle, "cboxDefaultStyle");
            this.cboxDefaultStyle.BoxBackColor = System.Drawing.Color.White;
            this.cboxDefaultStyle.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxDefaultStyle.Name = "cboxDefaultStyle";
            this.cboxDefaultStyle.UseVisualStyleBackColor = true;
            this.cboxDefaultStyle.CheckedChanged += new System.EventHandler(this.cboxDefaultStyle_CheckedChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // cbColorSettings
            // 
            this.cbColorSettings.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbColorSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColorSettings.FormattingEnabled = true;
            this.cbColorSettings.Items.AddRange(new object[] {
            resources.GetString("cbColorSettings.Items")});
            resources.ApplyResources(this.cbColorSettings, "cbColorSettings");
            this.cbColorSettings.Name = "cbColorSettings";
            exComboBoxStyle13.ArrowColor = null;
            exComboBoxStyle13.BackColor = null;
            exComboBoxStyle13.BorderColor = null;
            exComboBoxStyle13.ButtonBackColor = null;
            exComboBoxStyle13.ButtonBorderColor = null;
            exComboBoxStyle13.ButtonRenderFirst = null;
            exComboBoxStyle13.ForeColor = null;
            this.cbColorSettings.StyleDisabled = exComboBoxStyle13;
            exComboBoxStyle14.ArrowColor = null;
            exComboBoxStyle14.BackColor = null;
            exComboBoxStyle14.BorderColor = null;
            exComboBoxStyle14.ButtonBackColor = null;
            exComboBoxStyle14.ButtonBorderColor = null;
            exComboBoxStyle14.ButtonRenderFirst = null;
            exComboBoxStyle14.ForeColor = null;
            this.cbColorSettings.StyleHighlight = exComboBoxStyle14;
            exComboBoxStyle15.ArrowColor = null;
            exComboBoxStyle15.BackColor = null;
            exComboBoxStyle15.BorderColor = null;
            exComboBoxStyle15.ButtonBackColor = null;
            exComboBoxStyle15.ButtonBorderColor = null;
            exComboBoxStyle15.ButtonRenderFirst = null;
            exComboBoxStyle15.ForeColor = null;
            this.cbColorSettings.StyleNormal = exComboBoxStyle15;
            exComboBoxStyle16.ArrowColor = null;
            exComboBoxStyle16.BackColor = null;
            exComboBoxStyle16.BorderColor = null;
            exComboBoxStyle16.ButtonBackColor = null;
            exComboBoxStyle16.ButtonBorderColor = null;
            exComboBoxStyle16.ButtonRenderFirst = null;
            exComboBoxStyle16.ForeColor = null;
            this.cbColorSettings.StyleSelected = exComboBoxStyle16;
            this.cbColorSettings.UseDarkScrollBar = false;
            this.cbColorSettings.SelectedIndexChanged += new System.EventHandler(this.cbColorSettings_SelectedIndexChanged);
            // 
            // bRemoveStyle
            // 
            resources.ApplyResources(this.bRemoveStyle, "bRemoveStyle");
            this.bRemoveStyle.Name = "bRemoveStyle";
            this.bRemoveStyle.UseVisualStyleBackColor = true;
            this.bRemoveStyle.Click += new System.EventHandler(this.bRemoveStyle_Click);
            // 
            // bRenameStyle
            // 
            resources.ApplyResources(this.bRenameStyle, "bRenameStyle");
            this.bRenameStyle.Name = "bRenameStyle";
            this.bRenameStyle.UseVisualStyleBackColor = true;
            this.bRenameStyle.Click += new System.EventHandler(this.bRenameStyle_Click);
            // 
            // bAddStyle
            // 
            resources.ApplyResources(this.bAddStyle, "bAddStyle");
            this.bAddStyle.Name = "bAddStyle";
            this.bAddStyle.UseVisualStyleBackColor = true;
            this.bAddStyle.Click += new System.EventHandler(this.bAddStyle_Click);
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // cbStyles
            // 
            this.cbStyles.BackColor = System.Drawing.Color.White;
            this.cbStyles.DefaultStyle = false;
            this.cbStyles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbStyles.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbStyles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStyles.ForeColor = System.Drawing.Color.Black;
            this.cbStyles.FormattingEnabled = true;
            resources.ApplyResources(this.cbStyles, "cbStyles");
            this.cbStyles.Name = "cbStyles";
            exComboBoxStyle17.ArrowColor = null;
            exComboBoxStyle17.BackColor = null;
            exComboBoxStyle17.BorderColor = null;
            exComboBoxStyle17.ButtonBackColor = null;
            exComboBoxStyle17.ButtonBorderColor = null;
            exComboBoxStyle17.ButtonRenderFirst = null;
            exComboBoxStyle17.ForeColor = null;
            this.cbStyles.StyleDisabled = exComboBoxStyle17;
            exComboBoxStyle18.ArrowColor = null;
            exComboBoxStyle18.BackColor = null;
            exComboBoxStyle18.BorderColor = null;
            exComboBoxStyle18.ButtonBackColor = null;
            exComboBoxStyle18.ButtonBorderColor = null;
            exComboBoxStyle18.ButtonRenderFirst = null;
            exComboBoxStyle18.ForeColor = null;
            this.cbStyles.StyleHighlight = exComboBoxStyle18;
            exComboBoxStyle19.ButtonBorderColor = System.Drawing.Color.White;
            exComboBoxStyle19.ButtonRenderFirst = true;
            this.cbStyles.StyleNormal = exComboBoxStyle19;
            exComboBoxStyle20.ArrowColor = null;
            exComboBoxStyle20.BackColor = null;
            exComboBoxStyle20.BorderColor = null;
            exComboBoxStyle20.ButtonBackColor = null;
            exComboBoxStyle20.ButtonBorderColor = null;
            exComboBoxStyle20.ButtonRenderFirst = null;
            exComboBoxStyle20.ForeColor = null;
            this.cbStyles.StyleSelected = exComboBoxStyle20;
            this.cbStyles.UseDarkScrollBar = false;
            this.cbStyles.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbStyles_DrawItem);
            this.cbStyles.SelectedIndexChanged += new System.EventHandler(this.cbStyles_SelectedIndexChanged);
            // 
            // exGroupBox3
            // 
            this.exGroupBox3.Controls.Add(this.label10);
            this.exGroupBox3.Controls.Add(this.cbFontSize);
            this.exGroupBox3.Controls.Add(this.label12);
            this.exGroupBox3.Controls.Add(this.exGroupBox1);
            this.exGroupBox3.Controls.Add(this.cbFont);
            this.exGroupBox3.Controls.Add(this.label11);
            this.exGroupBox3.Controls.Add(this.listSettings);
            this.exGroupBox3.Controls.Add(this.label13);
            this.exGroupBox3.Controls.Add(this.labelExample);
            resources.ApplyResources(this.exGroupBox3, "exGroupBox3");
            this.exGroupBox3.Name = "exGroupBox3";
            this.exGroupBox3.TabStop = false;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // cbFontSize
            // 
            this.cbFontSize.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbFontSize.FormattingEnabled = true;
            resources.ApplyResources(this.cbFontSize, "cbFontSize");
            this.cbFontSize.Name = "cbFontSize";
            exComboBoxStyle21.ArrowColor = null;
            exComboBoxStyle21.BackColor = null;
            exComboBoxStyle21.BorderColor = null;
            exComboBoxStyle21.ButtonBackColor = null;
            exComboBoxStyle21.ButtonBorderColor = null;
            exComboBoxStyle21.ButtonRenderFirst = null;
            exComboBoxStyle21.ForeColor = null;
            this.cbFontSize.StyleDisabled = exComboBoxStyle21;
            exComboBoxStyle22.ArrowColor = null;
            exComboBoxStyle22.BackColor = null;
            exComboBoxStyle22.BorderColor = null;
            exComboBoxStyle22.ButtonBackColor = null;
            exComboBoxStyle22.ButtonBorderColor = null;
            exComboBoxStyle22.ButtonRenderFirst = null;
            exComboBoxStyle22.ForeColor = null;
            this.cbFontSize.StyleHighlight = exComboBoxStyle22;
            exComboBoxStyle23.ArrowColor = null;
            exComboBoxStyle23.BackColor = null;
            exComboBoxStyle23.BorderColor = null;
            exComboBoxStyle23.ButtonBackColor = null;
            exComboBoxStyle23.ButtonBorderColor = null;
            exComboBoxStyle23.ButtonRenderFirst = null;
            exComboBoxStyle23.ForeColor = null;
            this.cbFontSize.StyleNormal = exComboBoxStyle23;
            exComboBoxStyle24.ArrowColor = null;
            exComboBoxStyle24.BackColor = null;
            exComboBoxStyle24.BorderColor = null;
            exComboBoxStyle24.ButtonBackColor = null;
            exComboBoxStyle24.ButtonBorderColor = null;
            exComboBoxStyle24.ButtonRenderFirst = null;
            exComboBoxStyle24.ForeColor = null;
            this.cbFontSize.StyleSelected = exComboBoxStyle24;
            this.cbFontSize.UseDarkScrollBar = false;
            this.cbFontSize.SelectedIndexChanged += new System.EventHandler(this.cbFontSize_SelectedIndexChanged);
            this.cbFontSize.TextChanged += new System.EventHandler(this.cbFontSize_TextChanged);
            this.cbFontSize.Validating += new System.ComponentModel.CancelEventHandler(this.cbFontSize_Validating);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // exGroupBox1
            // 
            this.exGroupBox1.Controls.Add(this.label15);
            this.exGroupBox1.Controls.Add(this.pbBackgroundColor);
            this.exGroupBox1.Controls.Add(this.bSetBackgroundColor);
            this.exGroupBox1.Controls.Add(this.label16);
            this.exGroupBox1.Controls.Add(this.pbForegroundColor);
            this.exGroupBox1.Controls.Add(this.bSetForegroundColor);
            this.exGroupBox1.Controls.Add(this.cbBold);
            resources.ApplyResources(this.exGroupBox1, "exGroupBox1");
            this.exGroupBox1.Name = "exGroupBox1";
            this.exGroupBox1.TabStop = false;
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // pbBackgroundColor
            // 
            this.pbBackgroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pbBackgroundColor, "pbBackgroundColor");
            this.pbBackgroundColor.Name = "pbBackgroundColor";
            this.pbBackgroundColor.TabStop = false;
            // 
            // bSetBackgroundColor
            // 
            resources.ApplyResources(this.bSetBackgroundColor, "bSetBackgroundColor");
            this.bSetBackgroundColor.Name = "bSetBackgroundColor";
            this.bSetBackgroundColor.UseVisualStyleBackColor = true;
            this.bSetBackgroundColor.Click += new System.EventHandler(this.bSetBackgroundColor_Click);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // pbForegroundColor
            // 
            this.pbForegroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pbForegroundColor, "pbForegroundColor");
            this.pbForegroundColor.Name = "pbForegroundColor";
            this.pbForegroundColor.TabStop = false;
            // 
            // bSetForegroundColor
            // 
            resources.ApplyResources(this.bSetForegroundColor, "bSetForegroundColor");
            this.bSetForegroundColor.Name = "bSetForegroundColor";
            this.bSetForegroundColor.UseVisualStyleBackColor = true;
            this.bSetForegroundColor.Click += new System.EventHandler(this.bSetForegroundColor_Click);
            // 
            // cbBold
            // 
            resources.ApplyResources(this.cbBold, "cbBold");
            this.cbBold.BoxBackColor = System.Drawing.Color.White;
            this.cbBold.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbBold.Name = "cbBold";
            this.cbBold.UseVisualStyleBackColor = true;
            this.cbBold.CheckedChanged += new System.EventHandler(this.cbBold_CheckedChanged);
            // 
            // cbFont
            // 
            this.cbFont.BackColor = System.Drawing.Color.White;
            this.cbFont.DefaultStyle = false;
            this.cbFont.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbFont.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbFont.ForeColor = System.Drawing.Color.Black;
            this.cbFont.FormattingEnabled = true;
            resources.ApplyResources(this.cbFont, "cbFont");
            this.cbFont.Name = "cbFont";
            exComboBoxStyle25.ArrowColor = null;
            exComboBoxStyle25.BackColor = null;
            exComboBoxStyle25.BorderColor = null;
            exComboBoxStyle25.ButtonBackColor = null;
            exComboBoxStyle25.ButtonBorderColor = null;
            exComboBoxStyle25.ButtonRenderFirst = null;
            exComboBoxStyle25.ForeColor = null;
            this.cbFont.StyleDisabled = exComboBoxStyle25;
            exComboBoxStyle26.ArrowColor = null;
            exComboBoxStyle26.BackColor = null;
            exComboBoxStyle26.BorderColor = null;
            exComboBoxStyle26.ButtonBackColor = null;
            exComboBoxStyle26.ButtonBorderColor = null;
            exComboBoxStyle26.ButtonRenderFirst = null;
            exComboBoxStyle26.ForeColor = null;
            this.cbFont.StyleHighlight = exComboBoxStyle26;
            exComboBoxStyle27.ButtonBorderColor = System.Drawing.Color.White;
            exComboBoxStyle27.ButtonRenderFirst = true;
            this.cbFont.StyleNormal = exComboBoxStyle27;
            exComboBoxStyle28.ArrowColor = null;
            exComboBoxStyle28.BackColor = null;
            exComboBoxStyle28.BorderColor = null;
            exComboBoxStyle28.ButtonBackColor = null;
            exComboBoxStyle28.ButtonBorderColor = null;
            exComboBoxStyle28.ButtonRenderFirst = null;
            exComboBoxStyle28.ForeColor = null;
            this.cbFont.StyleSelected = exComboBoxStyle28;
            this.cbFont.UseDarkScrollBar = false;
            this.cbFont.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbFont_DrawItem);
            this.cbFont.SelectedIndexChanged += new System.EventHandler(this.cbFont_SelectedIndexChanged);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // listSettings
            // 
            this.listSettings.FormattingEnabled = true;
            resources.ApplyResources(this.listSettings, "listSettings");
            this.listSettings.Name = "listSettings";
            this.listSettings.SelectedIndexChanged += new System.EventHandler(this.listSettings_SelectedIndexChanged);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // labelExample
            // 
            this.labelExample.BackColor = System.Drawing.Color.White;
            this.labelExample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.labelExample, "labelExample");
            this.labelExample.Name = "labelExample";
            // 
            // pLogging
            // 
            this.pLogging.Controls.Add(this.cbDebugModeGUI);
            this.pLogging.Controls.Add(this.label4);
            this.pLogging.Controls.Add(this.cbLoggingError);
            this.pLogging.Controls.Add(this.cbLoggingInfo);
            resources.ApplyResources(this.pLogging, "pLogging");
            this.pLogging.Name = "pLogging";
            // 
            // pPlaying
            // 
            this.pPlaying.Controls.Add(this.nudWordPause);
            this.pPlaying.Controls.Add(this.label3);
            resources.ApplyResources(this.pPlaying, "pPlaying");
            this.pPlaying.Name = "pPlaying";
            // 
            // pDesktop
            // 
            this.pDesktop.Controls.Add(this.cbFitLastColumn);
            this.pDesktop.Controls.Add(this.bColumnDown);
            this.pDesktop.Controls.Add(this.bColumnUp);
            this.pDesktop.Controls.Add(this.label6);
            this.pDesktop.Controls.Add(this.dgvDesktopColums);
            this.pDesktop.Controls.Add(this.groupBox2);
            resources.ApplyResources(this.pDesktop, "pDesktop");
            this.pDesktop.Name = "pDesktop";
            // 
            // cbFitLastColumn
            // 
            resources.ApplyResources(this.cbFitLastColumn, "cbFitLastColumn");
            this.cbFitLastColumn.BoxBackColor = System.Drawing.Color.White;
            this.cbFitLastColumn.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbFitLastColumn.Name = "cbFitLastColumn";
            this.cbFitLastColumn.UseVisualStyleBackColor = true;
            // 
            // bColumnDown
            // 
            resources.ApplyResources(this.bColumnDown, "bColumnDown");
            this.bColumnDown.Name = "bColumnDown";
            this.bColumnDown.UseVisualStyleBackColor = true;
            this.bColumnDown.Click += new System.EventHandler(this.bColumnDown_Click);
            // 
            // bColumnUp
            // 
            resources.ApplyResources(this.bColumnUp, "bColumnUp");
            this.bColumnUp.Name = "bColumnUp";
            this.bColumnUp.UseVisualStyleBackColor = true;
            this.bColumnUp.Click += new System.EventHandler(this.bColumnUp_Click);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // dgvDesktopColums
            // 
            this.dgvDesktopColums.AllowUserToAddRows = false;
            this.dgvDesktopColums.AllowUserToDeleteRows = false;
            this.dgvDesktopColums.AllowUserToResizeRows = false;
            this.dgvDesktopColums.AutoGenerateColumns = false;
            this.dgvDesktopColums.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDesktopColums.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDesktopColums.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewCheckBoxColumn,
            this.visibleDataGridViewCheckBoxColumn,
            this.MinWidth});
            this.dgvDesktopColums.DataSource = this.desktopColumnBindingSource;
            resources.ApplyResources(this.dgvDesktopColums, "dgvDesktopColums");
            this.dgvDesktopColums.Name = "dgvDesktopColums";
            this.dgvDesktopColums.RowHeadersVisible = false;
            this.dgvDesktopColums.RowTemplate.Height = 24;
            this.dgvDesktopColums.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDesktopColums.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvDesktopColums_CellValidating);
            this.dgvDesktopColums.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDesktopColums_DataError);
            // 
            // nameDataGridViewCheckBoxColumn
            // 
            this.nameDataGridViewCheckBoxColumn.DataPropertyName = "Name";
            resources.ApplyResources(this.nameDataGridViewCheckBoxColumn, "nameDataGridViewCheckBoxColumn");
            this.nameDataGridViewCheckBoxColumn.Name = "nameDataGridViewCheckBoxColumn";
            this.nameDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // MinWidth
            // 
            this.MinWidth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MinWidth.DataPropertyName = "MinWidth";
            resources.ApplyResources(this.MinWidth, "MinWidth");
            this.MinWidth.Name = "MinWidth";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboxDateTimeInStateRow);
            this.groupBox2.Controls.Add(this.cbShowRowsHeader);
            this.groupBox2.Controls.Add(this.rbTS);
            this.groupBox2.Controls.Add(this.rbMS);
            this.groupBox2.Controls.Add(this.rbMSandTS);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.cbShowStateRow);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cboxDateTimeInStateRow
            // 
            resources.ApplyResources(this.cboxDateTimeInStateRow, "cboxDateTimeInStateRow");
            this.cboxDateTimeInStateRow.BoxBackColor = System.Drawing.Color.White;
            this.cboxDateTimeInStateRow.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxDateTimeInStateRow.Name = "cboxDateTimeInStateRow";
            this.cboxDateTimeInStateRow.UseVisualStyleBackColor = true;
            // 
            // cbShowRowsHeader
            // 
            resources.ApplyResources(this.cbShowRowsHeader, "cbShowRowsHeader");
            this.cbShowRowsHeader.BoxBackColor = System.Drawing.Color.White;
            this.cbShowRowsHeader.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbShowRowsHeader.Name = "cbShowRowsHeader";
            this.cbShowRowsHeader.UseVisualStyleBackColor = true;
            // 
            // rbTS
            // 
            resources.ApplyResources(this.rbTS, "rbTS");
            this.rbTS.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbTS.Name = "rbTS";
            this.rbTS.TabStop = true;
            this.rbTS.UseVisualStyleBackColor = true;
            // 
            // rbMS
            // 
            resources.ApplyResources(this.rbMS, "rbMS");
            this.rbMS.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbMS.Name = "rbMS";
            this.rbMS.TabStop = true;
            this.rbMS.UseVisualStyleBackColor = true;
            // 
            // rbMSandTS
            // 
            resources.ApplyResources(this.rbMSandTS, "rbMSandTS");
            this.rbMSandTS.Checked = true;
            this.rbMSandTS.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbMSandTS.Name = "rbMSandTS";
            this.rbMSandTS.TabStop = true;
            this.rbMSandTS.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cbShowStateRow
            // 
            resources.ApplyResources(this.cbShowStateRow, "cbShowStateRow");
            this.cbShowStateRow.BoxBackColor = System.Drawing.Color.White;
            this.cbShowStateRow.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbShowStateRow.Name = "cbShowStateRow";
            this.cbShowStateRow.UseVisualStyleBackColor = true;
            // 
            // pShortcuts
            // 
            this.pShortcuts.Controls.Add(this.lShortcutHelp);
            this.pShortcuts.Controls.Add(this.dgvShortcuts);
            this.pShortcuts.Controls.Add(this.bAllShortcutsSetDefault);
            this.pShortcuts.Controls.Add(this.label8);
            resources.ApplyResources(this.pShortcuts, "pShortcuts");
            this.pShortcuts.Name = "pShortcuts";
            // 
            // lShortcutHelp
            // 
            resources.ApplyResources(this.lShortcutHelp, "lShortcutHelp");
            this.lShortcutHelp.Name = "lShortcutHelp";
            // 
            // dgvShortcuts
            // 
            this.dgvShortcuts.AllowUserToAddRows = false;
            this.dgvShortcuts.AllowUserToDeleteRows = false;
            this.dgvShortcuts.AllowUserToResizeRows = false;
            this.dgvShortcuts.AutoGenerateColumns = false;
            this.dgvShortcuts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvShortcuts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn1,
            this.shortcutDataGridViewTextBoxColumn,
            this.AssignShortcut,
            this.RemoveShortcut,
            this.Default});
            this.dgvShortcuts.DataSource = this.commandShortcutBindingSource;
            resources.ApplyResources(this.dgvShortcuts, "dgvShortcuts");
            this.dgvShortcuts.Name = "dgvShortcuts";
            this.dgvShortcuts.ReadOnly = true;
            this.dgvShortcuts.RowHeadersVisible = false;
            this.dgvShortcuts.RowTemplate.Height = 24;
            this.dgvShortcuts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvShortcuts_CellClick);
            // 
            // AssignShortcut
            // 
            resources.ApplyResources(this.AssignShortcut, "AssignShortcut");
            this.AssignShortcut.Name = "AssignShortcut";
            this.AssignShortcut.ReadOnly = true;
            this.AssignShortcut.Text = "Priradiť";
            this.AssignShortcut.UseColumnTextForButtonValue = true;
            // 
            // RemoveShortcut
            // 
            resources.ApplyResources(this.RemoveShortcut, "RemoveShortcut");
            this.RemoveShortcut.Name = "RemoveShortcut";
            this.RemoveShortcut.ReadOnly = true;
            this.RemoveShortcut.Text = "Odobrať";
            this.RemoveShortcut.UseColumnTextForButtonValue = true;
            // 
            // Default
            // 
            this.Default.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.Default, "Default");
            this.Default.Name = "Default";
            this.Default.ReadOnly = true;
            this.Default.Text = "Pôvodné";
            this.Default.UseColumnTextForButtonValue = true;
            // 
            // bAllShortcutsSetDefault
            // 
            resources.ApplyResources(this.bAllShortcutsSetDefault, "bAllShortcutsSetDefault");
            this.bAllShortcutsSetDefault.Name = "bAllShortcutsSetDefault";
            this.bAllShortcutsSetDefault.UseVisualStyleBackColor = true;
            this.bAllShortcutsSetDefault.Click += new System.EventHandler(this.bAllShortcutsSetDefault_Click);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // pStartup
            // 
            this.pStartup.Controls.Add(this.label18);
            this.pStartup.Controls.Add(this.groupArguments);
            this.pStartup.Controls.Add(this.cboxManualCmdArgs);
            this.pStartup.Controls.Add(this.tbCmdArguments);
            this.pStartup.Controls.Add(this.label14);
            this.pStartup.Controls.Add(this.cbStartINISSAdmin);
            resources.ApplyResources(this.pStartup, "pStartup");
            this.pStartup.Name = "pStartup";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // groupArguments
            // 
            this.groupArguments.Controls.Add(this.cbArgRegister);
            this.groupArguments.Controls.Add(this.label19);
            this.groupArguments.Controls.Add(this.cboxArgExportTableTexts);
            this.groupArguments.Controls.Add(this.cboxArgAsClient);
            this.groupArguments.Controls.Add(this.cboxArgExportHlasTexts);
            this.groupArguments.Controls.Add(this.cboxArgMinimize);
            this.groupArguments.Controls.Add(this.cboxArgMoreInstances);
            resources.ApplyResources(this.groupArguments, "groupArguments");
            this.groupArguments.Name = "groupArguments";
            this.groupArguments.TabStop = false;
            // 
            // cbArgRegister
            // 
            this.cbArgRegister.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbArgRegister.FormattingEnabled = true;
            resources.ApplyResources(this.cbArgRegister, "cbArgRegister");
            this.cbArgRegister.Name = "cbArgRegister";
            exComboBoxStyle29.ArrowColor = null;
            exComboBoxStyle29.BackColor = null;
            exComboBoxStyle29.BorderColor = null;
            exComboBoxStyle29.ButtonBackColor = null;
            exComboBoxStyle29.ButtonBorderColor = null;
            exComboBoxStyle29.ButtonRenderFirst = null;
            exComboBoxStyle29.ForeColor = null;
            this.cbArgRegister.StyleDisabled = exComboBoxStyle29;
            exComboBoxStyle30.ArrowColor = null;
            exComboBoxStyle30.BackColor = null;
            exComboBoxStyle30.BorderColor = null;
            exComboBoxStyle30.ButtonBackColor = null;
            exComboBoxStyle30.ButtonBorderColor = null;
            exComboBoxStyle30.ButtonRenderFirst = null;
            exComboBoxStyle30.ForeColor = null;
            this.cbArgRegister.StyleHighlight = exComboBoxStyle30;
            exComboBoxStyle31.ArrowColor = null;
            exComboBoxStyle31.BackColor = null;
            exComboBoxStyle31.BorderColor = null;
            exComboBoxStyle31.ButtonBackColor = null;
            exComboBoxStyle31.ButtonBorderColor = null;
            exComboBoxStyle31.ButtonRenderFirst = null;
            exComboBoxStyle31.ForeColor = null;
            this.cbArgRegister.StyleNormal = exComboBoxStyle31;
            exComboBoxStyle32.ArrowColor = null;
            exComboBoxStyle32.BackColor = null;
            exComboBoxStyle32.BorderColor = null;
            exComboBoxStyle32.ButtonBackColor = null;
            exComboBoxStyle32.ButtonBorderColor = null;
            exComboBoxStyle32.ButtonRenderFirst = null;
            exComboBoxStyle32.ForeColor = null;
            this.cbArgRegister.StyleSelected = exComboBoxStyle32;
            this.cbArgRegister.UseDarkScrollBar = false;
            this.cbArgRegister.SelectedIndexChanged += new System.EventHandler(this.cbArgRegister_SelectedIndexChanged);
            this.cbArgRegister.TextUpdate += new System.EventHandler(this.cbArgRegister_TextUpdate);
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // cboxArgExportTableTexts
            // 
            resources.ApplyResources(this.cboxArgExportTableTexts, "cboxArgExportTableTexts");
            this.cboxArgExportTableTexts.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgExportTableTexts.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgExportTableTexts.Name = "cboxArgExportTableTexts";
            this.cboxArgExportTableTexts.UseVisualStyleBackColor = true;
            this.cboxArgExportTableTexts.CheckedChanged += new System.EventHandler(this.cboxArgExportTableTexts_CheckedChanged);
            // 
            // cboxArgAsClient
            // 
            resources.ApplyResources(this.cboxArgAsClient, "cboxArgAsClient");
            this.cboxArgAsClient.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgAsClient.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgAsClient.Name = "cboxArgAsClient";
            this.cboxArgAsClient.UseVisualStyleBackColor = true;
            this.cboxArgAsClient.CheckedChanged += new System.EventHandler(this.cboxArgAsClient_CheckedChanged);
            // 
            // cboxArgExportHlasTexts
            // 
            resources.ApplyResources(this.cboxArgExportHlasTexts, "cboxArgExportHlasTexts");
            this.cboxArgExportHlasTexts.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgExportHlasTexts.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgExportHlasTexts.Name = "cboxArgExportHlasTexts";
            this.cboxArgExportHlasTexts.UseVisualStyleBackColor = true;
            this.cboxArgExportHlasTexts.CheckedChanged += new System.EventHandler(this.cboxArgExportHlasTexts_CheckedChanged);
            // 
            // cboxArgMinimize
            // 
            resources.ApplyResources(this.cboxArgMinimize, "cboxArgMinimize");
            this.cboxArgMinimize.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgMinimize.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgMinimize.Name = "cboxArgMinimize";
            this.cboxArgMinimize.UseVisualStyleBackColor = true;
            this.cboxArgMinimize.CheckedChanged += new System.EventHandler(this.cboxArgMinimize_CheckedChanged);
            // 
            // cboxArgMoreInstances
            // 
            resources.ApplyResources(this.cboxArgMoreInstances, "cboxArgMoreInstances");
            this.cboxArgMoreInstances.BoxBackColor = System.Drawing.Color.White;
            this.cboxArgMoreInstances.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxArgMoreInstances.Name = "cboxArgMoreInstances";
            this.cboxArgMoreInstances.UseVisualStyleBackColor = true;
            this.cboxArgMoreInstances.CheckedChanged += new System.EventHandler(this.cboxArgMoreInstances_CheckedChanged);
            // 
            // cboxManualCmdArgs
            // 
            resources.ApplyResources(this.cboxManualCmdArgs, "cboxManualCmdArgs");
            this.cboxManualCmdArgs.BoxBackColor = System.Drawing.Color.White;
            this.cboxManualCmdArgs.Checked = true;
            this.cboxManualCmdArgs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxManualCmdArgs.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxManualCmdArgs.Name = "cboxManualCmdArgs";
            this.cboxManualCmdArgs.UseVisualStyleBackColor = true;
            this.cboxManualCmdArgs.CheckedChanged += new System.EventHandler(this.cboxManualCmdArgs_CheckedChanged);
            // 
            // tbCmdArguments
            // 
            this.tbCmdArguments.BorderColor = System.Drawing.Color.DimGray;
            this.tbCmdArguments.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbCmdArguments.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbCmdArguments.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCmdArguments.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbCmdArguments.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCmdArguments.HintText = null;
            resources.ApplyResources(this.tbCmdArguments, "tbCmdArguments");
            this.tbCmdArguments.Name = "tbCmdArguments";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // cbStartINISSAdmin
            // 
            resources.ApplyResources(this.cbStartINISSAdmin, "cbStartINISSAdmin");
            this.cbStartINISSAdmin.BoxBackColor = System.Drawing.Color.White;
            this.cbStartINISSAdmin.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbStartINISSAdmin.Name = "cbStartINISSAdmin";
            this.cbStartINISSAdmin.UseVisualStyleBackColor = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.Frozen = true;
            resources.ApplyResources(this.nameDataGridViewTextBoxColumn, "nameDataGridViewTextBoxColumn");
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // visibleDataGridViewCheckBoxColumn
            // 
            this.visibleDataGridViewCheckBoxColumn.DataPropertyName = "Visible";
            resources.ApplyResources(this.visibleDataGridViewCheckBoxColumn, "visibleDataGridViewCheckBoxColumn");
            this.visibleDataGridViewCheckBoxColumn.Name = "visibleDataGridViewCheckBoxColumn";
            // 
            // nameDataGridViewTextBoxColumn1
            // 
            this.nameDataGridViewTextBoxColumn1.DataPropertyName = "Name";
            resources.ApplyResources(this.nameDataGridViewTextBoxColumn1, "nameDataGridViewTextBoxColumn1");
            this.nameDataGridViewTextBoxColumn1.Name = "nameDataGridViewTextBoxColumn1";
            this.nameDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // shortcutDataGridViewTextBoxColumn
            // 
            this.shortcutDataGridViewTextBoxColumn.DataPropertyName = "Shortcut";
            resources.ApplyResources(this.shortcutDataGridViewTextBoxColumn, "shortcutDataGridViewTextBoxColumn");
            this.shortcutDataGridViewTextBoxColumn.Name = "shortcutDataGridViewTextBoxColumn";
            this.shortcutDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // commandShortcutBindingSource
            // 
            this.commandShortcutBindingSource.DataSource = typeof(ToolsCore.XML.CommandShortcut);
            // 
            // desktopColumnBindingSource
            // 
            this.desktopColumnBindingSource.DataSource = typeof(ToolsCore.XML.DesktopColumn);
            // 
            // appFontBindingSource
            // 
            this.appFontBindingSource.DataSource = typeof(ToolsCore.XML.AppFont);
            // 
            // FAppSettings
            // 
            this.AcceptButton = this.bSave;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.Controls.Add(this.treeMenu);
            this.Controls.Add(this.bStorno);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.pFontsColors);
            this.Controls.Add(this.pLogging);
            this.Controls.Add(this.pStartup);
            this.Controls.Add(this.pObecne);
            this.Controls.Add(this.pPlaying);
            this.Controls.Add(this.pLocalization);
            this.Controls.Add(this.pShortcuts);
            this.Controls.Add(this.pDesktop);
            this.Controls.Add(this.pEnvironment);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FAppSettings";
            this.ShowInTaskbar = false;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FAppSettings_HelpButtonClicked);
            this.Load += new System.EventHandler(this.FAppSettings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FAppSettings_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.nudWordPause)).EndInit();
            this.pObecne.ResumeLayout(false);
            this.pObecne.PerformLayout();
            this.pEnvironment.ResumeLayout(false);
            this.pEnvironment.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppFonts)).EndInit();
            this.pLocalization.ResumeLayout(false);
            this.pLocalization.PerformLayout();
            this.pFontsColors.ResumeLayout(false);
            this.pFontsColors.PerformLayout();
            this.exGroupBox2.ResumeLayout(false);
            this.exGroupBox2.PerformLayout();
            this.exGroupBox3.ResumeLayout(false);
            this.exGroupBox3.PerformLayout();
            this.exGroupBox1.ResumeLayout(false);
            this.exGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackgroundColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForegroundColor)).EndInit();
            this.pLogging.ResumeLayout(false);
            this.pLogging.PerformLayout();
            this.pPlaying.ResumeLayout(false);
            this.pPlaying.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deviceInformationBindingSource)).EndInit();
            this.pDesktop.ResumeLayout(false);
            this.pDesktop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDesktopColums)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pShortcuts.ResumeLayout(false);
            this.pShortcuts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShortcuts)).EndInit();
            this.pStartup.ResumeLayout(false);
            this.pStartup.PerformLayout();
            this.groupArguments.ResumeLayout(false);
            this.groupArguments.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.commandShortcutBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.desktopColumnBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.appFontBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExControls.ExButton bSave;
        private ExControls.ExButton bStorno;
        private ExComboBox cbDateRemLocate;
        private ExComboBox cbAppLanguage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private ExNumericUpDown nudWordPause;
        private System.Windows.Forms.Label label4;
        private ExComboBox cbDebugModeGUI;
        private ExCheckBox cbLoggingInfo;
        private ExCheckBox cbMoreInstance;
        private ExCheckBox cbLoggingError;
        private ExCheckBox cbClassicGUI;
        private ExCheckBox cbAutoTableText;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.TreeView treeMenu;
        private System.Windows.Forms.Panel pObecne;
        private System.Windows.Forms.Panel pEnvironment;
        private System.Windows.Forms.Panel pLocalization;
        private System.Windows.Forms.Panel pFontsColors;
        private System.Windows.Forms.Panel pLogging;
        private System.Windows.Forms.Panel pPlaying;
        private System.Windows.Forms.ImageList imagesMenu;
        private ExCheckBox cbBold;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label labelExample;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ListBox listSettings;
        private ExComboBox cbFontSize;
        private System.Windows.Forms.Label label12;
        private ExComboBox cbFont;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private ExControls.ExButton bColorsUseDefault;
        private ExComboBox cbColorSettings;
        private System.Windows.Forms.Label label9;
        private ExControls.ExButton bSetForegroundColor;
        private ExControls.ExButton bSetBackgroundColor;
        private System.Windows.Forms.PictureBox pbForegroundColor;
        private System.Windows.Forms.PictureBox pbBackgroundColor;
        private ExControls.ExButton bAppFontDefault;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FontDialog appFontDialog;
        private System.Windows.Forms.Panel pDesktop;
        private ExGroupBox groupBox2;
        private ExCheckBox cbShowRowsHeader;
        private ExRadioButton rbTS;
        private ExRadioButton rbMS;
        private ExRadioButton rbMSandTS;
        private System.Windows.Forms.Label label7;
        private ExCheckBox cbShowStateRow;
        private System.Windows.Forms.DataGridView dgvDesktopColums;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.BindingSource desktopColumnBindingSource;
        private System.Windows.Forms.DataGridView dgvAppFonts;
        private System.Windows.Forms.BindingSource appFontBindingSource;
        private ExControls.ExButton bColumnUp;
        private ExControls.ExButton bColumnDown;
        private ExCheckBox cbFitLastColumn;
        private System.Windows.Forms.Panel pShortcuts;
        private System.Windows.Forms.Label label8;
        private ExControls.ExButton bAllShortcutsSetDefault;
        private System.Windows.Forms.DataGridView dgvShortcuts;
        private System.Windows.Forms.Label lShortcutHelp;
        private System.Windows.Forms.BindingSource commandShortcutBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn visibleDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MinWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FontDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn Example;
        private ExCheckBox cbDisableVariantCheck;
        private ExCheckBox cbAutoVariant;
        private System.Windows.Forms.Panel pStartup;
        private ExCheckBox cbStartINISSAdmin;
        private System.Windows.Forms.Label label14;
        private ExTextBox tbCmdArguments;
        private ExCheckBox cboxDateTimeInStateRow;
        private ExComboBox cbStyles;
        private System.Windows.Forms.Label label17;
        private ExControls.ExButton bAddStyle;
        private ExControls.ExButton bRenameStyle;
        private ExControls.ExButton bRemoveStyle;
        private ExCheckBox cboxDefaultStyle;
        private ExGroupBox exGroupBox1;
        private ExGroupBox exGroupBox2;
        private ExGroupBox exGroupBox3;
        private ExCheckBox cboxDarkTitleBar;
        private ExCheckBox cboxDarkScrollBars;
        private ExControls.ExButton bUseStyle;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn shortcutDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn AssignShortcut;
        private System.Windows.Forms.DataGridViewButtonColumn RemoveShortcut;
        private System.Windows.Forms.DataGridViewButtonColumn Default;
        private ExCheckBox cboxManualCmdArgs;
        private ExGroupBox groupArguments;
        private ExCheckBox cboxArgMoreInstances;
        private System.Windows.Forms.Label label18;
        private ExCheckBox cboxArgMinimize;
        private ExCheckBox cboxArgExportHlasTexts;
        private ExCheckBox cboxArgAsClient;
        private ExCheckBox cboxArgExportTableTexts;
        private System.Windows.Forms.Label label19;
        private ExComboBox cbArgRegister;
        private System.Windows.Forms.BindingSource deviceInformationBindingSource;
        private ExCheckBox cbCheckUpdate;
    }
}