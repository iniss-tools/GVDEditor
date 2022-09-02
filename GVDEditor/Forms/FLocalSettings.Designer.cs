using System.Windows.Forms;
using ExControls;
using GVDEditor.Entities;

namespace GVDEditor.Forms
{
    partial class FLocalSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLocalSettings));
            this.bSave = new ExControls.ExButton();
            this.label16 = new System.Windows.Forms.Label();
            this.button3 = new ExControls.ExButton();
            this.button2 = new ExControls.ExButton();
            this.button1 = new ExControls.ExButton();
            this.textBox1 = new ExControls.ExTextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox2 = new ExControls.ExTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox11 = new ExControls.ExGroupBox();
            this.label42 = new System.Windows.Forms.Label();
            this.tbCustomTrainTypText = new ExControls.ExTextBox();
            this.tbCustomTrainTypSkratka = new ExControls.ExTextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.bCustomTrainTypDelete = new ExControls.ExButton();
            this.bCustomTrainTypAdd = new ExControls.ExButton();
            this.bCustomTrainTypEdit = new ExControls.ExButton();
            this.listTrainTypes = new System.Windows.Forms.ListBox();
            this.label38 = new System.Windows.Forms.Label();
            this.groupBox10 = new ExControls.ExGroupBox();
            this.bDefTrainTypDelete = new ExControls.ExButton();
            this.bDefTrainTypEdit = new ExControls.ExButton();
            this.bDefTrainTypAdd = new ExControls.ExButton();
            this.label39 = new System.Windows.Forms.Label();
            this.fdbFontsDir = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl = new ExControls.ExTabControl();
            this.tpGrafikon = new System.Windows.Forms.TabPage();
            this.groupBox15 = new ExControls.ExGroupBox();
            this.bDirChange = new ExControls.ExButton();
            this.tbDir = new ExControls.ExTextBox();
            this.tbDirName = new ExControls.ExTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.bOpenDir = new ExControls.ExButton();
            this.groupBox14 = new ExControls.ExGroupBox();
            this.dynamicLineSeparator1 = new ExControls.ExLineSeparator();
            this.tbGVDStationName = new ExControls.ExTextBox();
            this.cbCustomStation = new ExControls.ExCheckBox();
            this.label47 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.nudIDStation = new ExControls.ExNumericUpDown();
            this.label49 = new System.Windows.Forms.Label();
            this.cbStationName = new ExControls.ExComboBox();
            this.groupBox13 = new ExControls.ExGroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpGVDOd = new System.Windows.Forms.DateTimePicker();
            this.dtpGVDDo = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.dtpDataDo = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.dtpDataOd = new System.Windows.Forms.DateTimePicker();
            this.tpStanice = new System.Windows.Forms.TabPage();
            this.listCustomStations = new System.Windows.Forms.ListBox();
            this.label43 = new System.Windows.Forms.Label();
            this.groupBox12 = new ExControls.ExGroupBox();
            this.label45 = new System.Windows.Forms.Label();
            this.nudIDStanice = new ExControls.ExNumericUpDown();
            this.bCStationDelete = new ExControls.ExButton();
            this.bCStationEdit = new ExControls.ExButton();
            this.bCStationAdd = new ExControls.ExButton();
            this.tbStationName = new ExControls.ExTextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.tpDopravcovia = new System.Windows.Forms.TabPage();
            this.listDopravcovia = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new ExControls.ExGroupBox();
            this.bDopravcaDelete = new ExControls.ExButton();
            this.bDopravcaEdit = new ExControls.ExButton();
            this.bDopravcaAdd = new ExControls.ExButton();
            this.tbDopravca = new ExControls.ExTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tpNastupistia = new System.Windows.Forms.TabPage();
            this.listNastupistia = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new ExControls.ExGroupBox();
            this.label26 = new System.Windows.Forms.Label();
            this.tbNastSound = new ExControls.ExTextBox();
            this.tbNastFullName = new ExControls.ExTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.bNastDelete = new ExControls.ExButton();
            this.bNastEdit = new ExControls.ExButton();
            this.bNastAdd = new ExControls.ExButton();
            this.tbNastOznacenie = new ExControls.ExTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tpKolaje = new System.Windows.Forms.TabPage();
            this.listKolaje = new System.Windows.Forms.ListBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox3 = new ExControls.ExGroupBox();
            this.tbKolajSound = new ExControls.ExTextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.cbNastupistia = new ExControls.ExComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.clbKolajTables = new ExControls.ExCheckedListBox();
            this.tbKolajFullName = new ExControls.ExTextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.bKolajDelete = new ExControls.ExButton();
            this.bKolajEdit = new ExControls.ExButton();
            this.bKolajAdd = new ExControls.ExButton();
            this.tbKolajOznacenie = new ExControls.ExTextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.tpFyzTab = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.tbCommentFyz = new ExControls.ExTextBox();
            this.listFyzTabule = new System.Windows.Forms.ListBox();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox4 = new ExControls.ExGroupBox();
            this.bFyzTabCopy = new ExControls.ExButton();
            this.bFyzTabDelete = new ExControls.ExButton();
            this.bFyzTabEdit = new ExControls.ExButton();
            this.bFyzTabAdd = new ExControls.ExButton();
            this.tpLogTab = new System.Windows.Forms.TabPage();
            this.label46 = new System.Windows.Forms.Label();
            this.tbCommentLog = new ExControls.ExTextBox();
            this.listLogTabule = new System.Windows.Forms.ListBox();
            this.label23 = new System.Windows.Forms.Label();
            this.groupBox5 = new ExControls.ExGroupBox();
            this.bLogTabCopy = new ExControls.ExButton();
            this.bLogTabDelete = new ExControls.ExButton();
            this.bLogTabEdit = new ExControls.ExButton();
            this.bLogTabAdd = new ExControls.ExButton();
            this.tpKatTab = new System.Windows.Forms.TabPage();
            this.label51 = new System.Windows.Forms.Label();
            this.tbCommentKat = new ExControls.ExTextBox();
            this.listKatTabule = new System.Windows.Forms.ListBox();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox6 = new ExControls.ExGroupBox();
            this.bKatTabCopy = new ExControls.ExButton();
            this.bKatTabDelete = new ExControls.ExButton();
            this.bKatTabEdit = new ExControls.ExButton();
            this.bKatTabAdd = new ExControls.ExButton();
            this.tpTabTab = new System.Windows.Forms.TabPage();
            this.listTabTabs = new System.Windows.Forms.ListBox();
            this.label25 = new System.Windows.Forms.Label();
            this.groupBox7 = new ExControls.ExGroupBox();
            this.bOpenEditorTab = new ExControls.ExButton();
            this.bTabTabDelete = new ExControls.ExButton();
            this.tpTTexts = new System.Windows.Forms.TabPage();
            this.label50 = new System.Windows.Forms.Label();
            this.tbCommentTText = new ExControls.ExTextBox();
            this.listTexty = new System.Windows.Forms.ListBox();
            this.label28 = new System.Windows.Forms.Label();
            this.groupBox8 = new ExControls.ExGroupBox();
            this.bTextDelete = new ExControls.ExButton();
            this.bTextEdit = new ExControls.ExButton();
            this.bTextAdd = new ExControls.ExButton();
            this.tpFonts = new System.Windows.Forms.TabPage();
            this.bOpenFontDir = new ExControls.ExButton();
            this.tbFontDir = new ExControls.ExTextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.listFonts = new System.Windows.Forms.ListBox();
            this.label29 = new System.Windows.Forms.Label();
            this.groupBox9 = new ExControls.ExGroupBox();
            this.label32 = new System.Windows.Forms.Label();
            this.cbFontIsNumber = new ExControls.ExCheckBox();
            this.tbFontFile = new ExControls.ExTextBox();
            this.cbFontSpecAssigments = new ExControls.ExCheckBox();
            this.cbFontSpecChar = new ExControls.ExCheckBox();
            this.cbFontUpper = new ExControls.ExCheckBox();
            this.cbFontLower = new ExControls.ExCheckBox();
            this.cbFontDia = new ExControls.ExCheckBox();
            this.cbFontProportional = new ExControls.ExCheckBox();
            this.label36 = new System.Windows.Forms.Label();
            this.cbFontType = new ExControls.ExComboBox();
            this.label35 = new System.Windows.Forms.Label();
            this.nudFontWidth = new ExControls.ExNumericUpDown();
            this.label34 = new System.Windows.Forms.Label();
            this.nudFontSize = new ExControls.ExNumericUpDown();
            this.label33 = new System.Windows.Forms.Label();
            this.tbFontName = new ExControls.ExTextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.nudFontID = new ExControls.ExNumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.bFontDelete = new ExControls.ExButton();
            this.bFontEdit = new ExControls.ExButton();
            this.bFontAdd = new ExControls.ExButton();
            this.checkBox1 = new ExControls.ExCheckBox();
            this.cbCustomTrainTypDruh = new ExControls.ExComboBox();
            this.cbDefTrainTypSkratka = new ExControls.ExComboBox();
            this.tabControl.SuspendLayout();
            this.tpGrafikon.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudIDStation)).BeginInit();
            this.groupBox13.SuspendLayout();
            this.tpStanice.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudIDStanice)).BeginInit();
            this.tpDopravcovia.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tpNastupistia.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tpKolaje.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tpFyzTab.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tpLogTab.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tpKatTab.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tpTabTab.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tpTTexts.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tpFonts.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontID)).BeginInit();
            this.SuspendLayout();
            // 
            // bSave
            // 
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.Name = "bSave";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BorderColor = System.Drawing.Color.DimGray;
            this.textBox1.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.textBox1.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.textBox1.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.textBox1.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.textBox1.HintText = null;
            this.textBox1.Name = "textBox1";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // textBox2
            // 
            this.textBox2.BorderColor = System.Drawing.Color.DimGray;
            this.textBox2.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.textBox2.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.textBox2.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.textBox2.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.textBox2.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.textBox2.HintText = null;
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox11
            // 
            resources.ApplyResources(this.groupBox11, "groupBox11");
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.TabStop = false;
            // 
            // label42
            // 
            resources.ApplyResources(this.label42, "label42");
            this.label42.Name = "label42";
            // 
            // tbCustomTrainTypText
            // 
            this.tbCustomTrainTypText.BorderColor = System.Drawing.Color.DimGray;
            this.tbCustomTrainTypText.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbCustomTrainTypText.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbCustomTrainTypText.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCustomTrainTypText.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbCustomTrainTypText.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCustomTrainTypText.HintText = null;
            resources.ApplyResources(this.tbCustomTrainTypText, "tbCustomTrainTypText");
            this.tbCustomTrainTypText.Name = "tbCustomTrainTypText";
            // 
            // tbCustomTrainTypSkratka
            // 
            this.tbCustomTrainTypSkratka.BorderColor = System.Drawing.Color.DimGray;
            this.tbCustomTrainTypSkratka.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbCustomTrainTypSkratka.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbCustomTrainTypSkratka.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCustomTrainTypSkratka.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbCustomTrainTypSkratka.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCustomTrainTypSkratka.HintText = null;
            resources.ApplyResources(this.tbCustomTrainTypSkratka, "tbCustomTrainTypSkratka");
            this.tbCustomTrainTypSkratka.Name = "tbCustomTrainTypSkratka";
            // 
            // label41
            // 
            resources.ApplyResources(this.label41, "label41");
            this.label41.Name = "label41";
            // 
            // label40
            // 
            resources.ApplyResources(this.label40, "label40");
            this.label40.Name = "label40";
            // 
            // bCustomTrainTypDelete
            // 
            resources.ApplyResources(this.bCustomTrainTypDelete, "bCustomTrainTypDelete");
            this.bCustomTrainTypDelete.Name = "bCustomTrainTypDelete";
            // 
            // bCustomTrainTypAdd
            // 
            resources.ApplyResources(this.bCustomTrainTypAdd, "bCustomTrainTypAdd");
            this.bCustomTrainTypAdd.Name = "bCustomTrainTypAdd";
            // 
            // bCustomTrainTypEdit
            // 
            resources.ApplyResources(this.bCustomTrainTypEdit, "bCustomTrainTypEdit");
            this.bCustomTrainTypEdit.Name = "bCustomTrainTypEdit";
            // 
            // listTrainTypes
            // 
            resources.ApplyResources(this.listTrainTypes, "listTrainTypes");
            this.listTrainTypes.Name = "listTrainTypes";
            // 
            // label38
            // 
            resources.ApplyResources(this.label38, "label38");
            this.label38.Name = "label38";
            // 
            // groupBox10
            // 
            resources.ApplyResources(this.groupBox10, "groupBox10");
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.TabStop = false;
            // 
            // bDefTrainTypDelete
            // 
            resources.ApplyResources(this.bDefTrainTypDelete, "bDefTrainTypDelete");
            this.bDefTrainTypDelete.Name = "bDefTrainTypDelete";
            // 
            // bDefTrainTypEdit
            // 
            resources.ApplyResources(this.bDefTrainTypEdit, "bDefTrainTypEdit");
            this.bDefTrainTypEdit.Name = "bDefTrainTypEdit";
            // 
            // bDefTrainTypAdd
            // 
            resources.ApplyResources(this.bDefTrainTypAdd, "bDefTrainTypAdd");
            this.bDefTrainTypAdd.Name = "bDefTrainTypAdd";
            // 
            // label39
            // 
            resources.ApplyResources(this.label39, "label39");
            this.label39.Name = "label39";
            // 
            // fdbFontsDir
            // 
            resources.ApplyResources(this.fdbFontsDir, "fdbFontsDir");
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpGrafikon);
            this.tabControl.Controls.Add(this.tpStanice);
            this.tabControl.Controls.Add(this.tpDopravcovia);
            this.tabControl.Controls.Add(this.tpNastupistia);
            this.tabControl.Controls.Add(this.tpKolaje);
            this.tabControl.Controls.Add(this.tpFyzTab);
            this.tabControl.Controls.Add(this.tpLogTab);
            this.tabControl.Controls.Add(this.tpKatTab);
            this.tabControl.Controls.Add(this.tpTabTab);
            this.tabControl.Controls.Add(this.tpTTexts);
            this.tabControl.Controls.Add(this.tpFonts);
            this.tabControl.HeaderBackColor = System.Drawing.SystemColors.Control;
            this.tabControl.HighlightBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            // 
            // tpGrafikon
            // 
            this.tpGrafikon.BackColor = System.Drawing.Color.Transparent;
            this.tpGrafikon.Controls.Add(this.groupBox15);
            this.tpGrafikon.Controls.Add(this.groupBox14);
            this.tpGrafikon.Controls.Add(this.groupBox13);
            resources.ApplyResources(this.tpGrafikon, "tpGrafikon");
            this.tpGrafikon.Name = "tpGrafikon";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.bDirChange);
            this.groupBox15.Controls.Add(this.tbDir);
            this.groupBox15.Controls.Add(this.tbDirName);
            this.groupBox15.Controls.Add(this.label9);
            this.groupBox15.Controls.Add(this.label4);
            this.groupBox15.Controls.Add(this.bOpenDir);
            resources.ApplyResources(this.groupBox15, "groupBox15");
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.TabStop = false;
            // 
            // bDirChange
            // 
            resources.ApplyResources(this.bDirChange, "bDirChange");
            this.bDirChange.Name = "bDirChange";
            this.bDirChange.UseVisualStyleBackColor = true;
            this.bDirChange.Click += new System.EventHandler(this.bDirChange_Click);
            // 
            // tbDir
            // 
            this.tbDir.BorderColor = System.Drawing.Color.DimGray;
            this.tbDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDir.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbDir.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbDir.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDir.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbDir.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDir.HintText = null;
            resources.ApplyResources(this.tbDir, "tbDir");
            this.tbDir.Name = "tbDir";
            this.tbDir.ReadOnly = true;
            // 
            // tbDirName
            // 
            this.tbDirName.BorderColor = System.Drawing.Color.DimGray;
            this.tbDirName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbDirName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbDirName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDirName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbDirName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDirName.HintText = null;
            resources.ApplyResources(this.tbDirName, "tbDirName");
            this.tbDirName.Name = "tbDirName";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // bOpenDir
            // 
            resources.ApplyResources(this.bOpenDir, "bOpenDir");
            this.bOpenDir.Name = "bOpenDir";
            this.bOpenDir.UseVisualStyleBackColor = true;
            this.bOpenDir.Click += new System.EventHandler(this.bOpenDir_Click);
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.dynamicLineSeparator1);
            this.groupBox14.Controls.Add(this.tbGVDStationName);
            this.groupBox14.Controls.Add(this.cbCustomStation);
            this.groupBox14.Controls.Add(this.label47);
            this.groupBox14.Controls.Add(this.label48);
            this.groupBox14.Controls.Add(this.nudIDStation);
            this.groupBox14.Controls.Add(this.label49);
            this.groupBox14.Controls.Add(this.cbStationName);
            resources.ApplyResources(this.groupBox14, "groupBox14");
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.TabStop = false;
            // 
            // dynamicLineSeparator1
            // 
            this.dynamicLineSeparator1.LineOrientation = ExControls.LineOrientation.Vertical;
            resources.ApplyResources(this.dynamicLineSeparator1, "dynamicLineSeparator1");
            this.dynamicLineSeparator1.Name = "dynamicLineSeparator1";
            // 
            // tbGVDStationName
            // 
            this.tbGVDStationName.BorderColor = System.Drawing.Color.DimGray;
            this.tbGVDStationName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbGVDStationName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbGVDStationName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.tbGVDStationName, "tbGVDStationName");
            this.tbGVDStationName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbGVDStationName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbGVDStationName.HintText = null;
            this.tbGVDStationName.Name = "tbGVDStationName";
            this.tbGVDStationName.TextChanged += new System.EventHandler(this.tbGVDStationName_TextChanged);
            // 
            // cbCustomStation
            // 
            resources.ApplyResources(this.cbCustomStation, "cbCustomStation");
            this.cbCustomStation.BoxBackColor = System.Drawing.Color.White;
            this.cbCustomStation.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbCustomStation.Name = "cbCustomStation";
            this.cbCustomStation.UseVisualStyleBackColor = true;
            this.cbCustomStation.CheckedChanged += new System.EventHandler(this.cbCustomStation_CheckedChanged);
            // 
            // label47
            // 
            resources.ApplyResources(this.label47, "label47");
            this.label47.Name = "label47";
            // 
            // label48
            // 
            resources.ApplyResources(this.label48, "label48");
            this.label48.Name = "label48";
            // 
            // nudIDStation
            // 
            resources.ApplyResources(this.nudIDStation, "nudIDStation");
            this.nudIDStation.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.nudIDStation.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudIDStation.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudIDStation.Name = "nudIDStation";
            this.nudIDStation.SelectedButtonColor = System.Drawing.Color.Empty;
            this.nudIDStation.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // label49
            // 
            resources.ApplyResources(this.label49, "label49");
            this.label49.Name = "label49";
            // 
            // cbStationName
            // 
            this.cbStationName.DisplayMember = "Name";
            this.cbStationName.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbStationName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStationName.FormattingEnabled = true;
            resources.ApplyResources(this.cbStationName, "cbStationName");
            this.cbStationName.Name = "cbStationName";
            this.cbStationName.StyleDisabled.ArrowColor = null;
            this.cbStationName.StyleDisabled.BackColor = null;
            this.cbStationName.StyleDisabled.BorderColor = null;
            this.cbStationName.StyleDisabled.ButtonBackColor = null;
            this.cbStationName.StyleDisabled.ButtonBorderColor = null;
            this.cbStationName.StyleDisabled.ButtonRenderFirst = null;
            this.cbStationName.StyleDisabled.ForeColor = null;
            this.cbStationName.StyleHighlight.ArrowColor = null;
            this.cbStationName.StyleHighlight.BackColor = null;
            this.cbStationName.StyleHighlight.BorderColor = null;
            this.cbStationName.StyleHighlight.ButtonBackColor = null;
            this.cbStationName.StyleHighlight.ButtonBorderColor = null;
            this.cbStationName.StyleHighlight.ButtonRenderFirst = null;
            this.cbStationName.StyleHighlight.ForeColor = null;
            this.cbStationName.StyleNormal.ArrowColor = null;
            this.cbStationName.StyleNormal.BackColor = null;
            this.cbStationName.StyleNormal.BorderColor = null;
            this.cbStationName.StyleNormal.ButtonBackColor = null;
            this.cbStationName.StyleNormal.ButtonBorderColor = null;
            this.cbStationName.StyleNormal.ButtonRenderFirst = null;
            this.cbStationName.StyleNormal.ForeColor = null;
            this.cbStationName.StyleSelected.ArrowColor = null;
            this.cbStationName.StyleSelected.BackColor = null;
            this.cbStationName.StyleSelected.BorderColor = null;
            this.cbStationName.StyleSelected.ButtonBackColor = null;
            this.cbStationName.StyleSelected.ButtonBorderColor = null;
            this.cbStationName.StyleSelected.ButtonRenderFirst = null;
            this.cbStationName.StyleSelected.ForeColor = null;
            this.cbStationName.UseDarkScrollBar = false;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.label2);
            this.groupBox13.Controls.Add(this.label3);
            this.groupBox13.Controls.Add(this.dtpGVDOd);
            this.groupBox13.Controls.Add(this.dtpGVDDo);
            this.groupBox13.Controls.Add(this.label13);
            this.groupBox13.Controls.Add(this.dtpDataDo);
            this.groupBox13.Controls.Add(this.label11);
            this.groupBox13.Controls.Add(this.dtpDataOd);
            resources.ApplyResources(this.groupBox13, "groupBox13");
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // dtpGVDOd
            // 
            this.dtpGVDOd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpGVDOd, "dtpGVDOd");
            this.dtpGVDOd.Name = "dtpGVDOd";
            // 
            // dtpGVDDo
            // 
            this.dtpGVDDo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpGVDDo, "dtpGVDDo");
            this.dtpGVDDo.Name = "dtpGVDDo";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // dtpDataDo
            // 
            this.dtpDataDo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpDataDo, "dtpDataDo");
            this.dtpDataDo.Name = "dtpDataDo";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // dtpDataOd
            // 
            this.dtpDataOd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpDataOd, "dtpDataOd");
            this.dtpDataOd.Name = "dtpDataOd";
            this.dtpDataOd.ValueChanged += new System.EventHandler(this.dtpDataOd_ValueChanged);
            // 
            // tpStanice
            // 
            this.tpStanice.BackColor = System.Drawing.Color.Transparent;
            this.tpStanice.Controls.Add(this.listCustomStations);
            this.tpStanice.Controls.Add(this.label43);
            this.tpStanice.Controls.Add(this.groupBox12);
            resources.ApplyResources(this.tpStanice, "tpStanice");
            this.tpStanice.Name = "tpStanice";
            // 
            // listCustomStations
            // 
            this.listCustomStations.FormattingEnabled = true;
            resources.ApplyResources(this.listCustomStations, "listCustomStations");
            this.listCustomStations.Name = "listCustomStations";
            this.listCustomStations.SelectedIndexChanged += new System.EventHandler(this.listCustomStations_SelectedIndexChanged);
            // 
            // label43
            // 
            resources.ApplyResources(this.label43, "label43");
            this.label43.Name = "label43";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.label45);
            this.groupBox12.Controls.Add(this.nudIDStanice);
            this.groupBox12.Controls.Add(this.bCStationDelete);
            this.groupBox12.Controls.Add(this.bCStationEdit);
            this.groupBox12.Controls.Add(this.bCStationAdd);
            this.groupBox12.Controls.Add(this.tbStationName);
            this.groupBox12.Controls.Add(this.label44);
            resources.ApplyResources(this.groupBox12, "groupBox12");
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.TabStop = false;
            // 
            // label45
            // 
            resources.ApplyResources(this.label45, "label45");
            this.label45.Name = "label45";
            // 
            // nudIDStanice
            // 
            this.nudIDStanice.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudIDStanice, "nudIDStanice");
            this.nudIDStanice.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudIDStanice.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudIDStanice.Name = "nudIDStanice";
            this.nudIDStanice.SelectedButtonColor = System.Drawing.Color.Empty;
            this.nudIDStanice.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // bCStationDelete
            // 
            resources.ApplyResources(this.bCStationDelete, "bCStationDelete");
            this.bCStationDelete.Name = "bCStationDelete";
            this.bCStationDelete.UseVisualStyleBackColor = true;
            this.bCStationDelete.Click += new System.EventHandler(this.bCStationDelete_Click);
            // 
            // bCStationEdit
            // 
            resources.ApplyResources(this.bCStationEdit, "bCStationEdit");
            this.bCStationEdit.Name = "bCStationEdit";
            this.bCStationEdit.UseVisualStyleBackColor = true;
            this.bCStationEdit.Click += new System.EventHandler(this.bCStationEdit_Click);
            // 
            // bCStationAdd
            // 
            resources.ApplyResources(this.bCStationAdd, "bCStationAdd");
            this.bCStationAdd.Name = "bCStationAdd";
            this.bCStationAdd.UseVisualStyleBackColor = true;
            this.bCStationAdd.Click += new System.EventHandler(this.bCStationAdd_Click);
            // 
            // tbStationName
            // 
            this.tbStationName.BorderColor = System.Drawing.Color.DimGray;
            this.tbStationName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbStationName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbStationName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbStationName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbStationName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbStationName.HintText = null;
            resources.ApplyResources(this.tbStationName, "tbStationName");
            this.tbStationName.Name = "tbStationName";
            // 
            // label44
            // 
            resources.ApplyResources(this.label44, "label44");
            this.label44.Name = "label44";
            // 
            // tpDopravcovia
            // 
            this.tpDopravcovia.BackColor = System.Drawing.Color.Transparent;
            this.tpDopravcovia.Controls.Add(this.listDopravcovia);
            this.tpDopravcovia.Controls.Add(this.label7);
            this.tpDopravcovia.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tpDopravcovia, "tpDopravcovia");
            this.tpDopravcovia.Name = "tpDopravcovia";
            // 
            // listDopravcovia
            // 
            this.listDopravcovia.FormattingEnabled = true;
            resources.ApplyResources(this.listDopravcovia, "listDopravcovia");
            this.listDopravcovia.Name = "listDopravcovia";
            this.listDopravcovia.SelectedIndexChanged += new System.EventHandler(this.listDopravcovia_SelectedIndexChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bDopravcaDelete);
            this.groupBox1.Controls.Add(this.bDopravcaEdit);
            this.groupBox1.Controls.Add(this.bDopravcaAdd);
            this.groupBox1.Controls.Add(this.tbDopravca);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // bDopravcaDelete
            // 
            resources.ApplyResources(this.bDopravcaDelete, "bDopravcaDelete");
            this.bDopravcaDelete.Name = "bDopravcaDelete";
            this.bDopravcaDelete.UseVisualStyleBackColor = true;
            this.bDopravcaDelete.Click += new System.EventHandler(this.bDopravcaDelete_Click);
            // 
            // bDopravcaEdit
            // 
            resources.ApplyResources(this.bDopravcaEdit, "bDopravcaEdit");
            this.bDopravcaEdit.Name = "bDopravcaEdit";
            this.bDopravcaEdit.UseVisualStyleBackColor = true;
            this.bDopravcaEdit.Click += new System.EventHandler(this.bDopravcaEdit_Click);
            // 
            // bDopravcaAdd
            // 
            resources.ApplyResources(this.bDopravcaAdd, "bDopravcaAdd");
            this.bDopravcaAdd.Name = "bDopravcaAdd";
            this.bDopravcaAdd.UseVisualStyleBackColor = true;
            this.bDopravcaAdd.Click += new System.EventHandler(this.bDopravcaAdd_Click);
            // 
            // tbDopravca
            // 
            this.tbDopravca.BorderColor = System.Drawing.Color.DimGray;
            this.tbDopravca.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbDopravca.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbDopravca.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDopravca.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbDopravca.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDopravca.HintText = null;
            resources.ApplyResources(this.tbDopravca, "tbDopravca");
            this.tbDopravca.Name = "tbDopravca";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tpNastupistia
            // 
            this.tpNastupistia.BackColor = System.Drawing.Color.Transparent;
            this.tpNastupistia.Controls.Add(this.listNastupistia);
            this.tpNastupistia.Controls.Add(this.label8);
            this.tpNastupistia.Controls.Add(this.groupBox2);
            resources.ApplyResources(this.tpNastupistia, "tpNastupistia");
            this.tpNastupistia.Name = "tpNastupistia";
            // 
            // listNastupistia
            // 
            this.listNastupistia.FormattingEnabled = true;
            resources.ApplyResources(this.listNastupistia, "listNastupistia");
            this.listNastupistia.Name = "listNastupistia";
            this.listNastupistia.SelectedIndexChanged += new System.EventHandler(this.listNastupistia_SelectedIndexChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label26);
            this.groupBox2.Controls.Add(this.tbNastSound);
            this.groupBox2.Controls.Add(this.tbNastFullName);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.bNastDelete);
            this.groupBox2.Controls.Add(this.bNastEdit);
            this.groupBox2.Controls.Add(this.bNastAdd);
            this.groupBox2.Controls.Add(this.tbNastOznacenie);
            this.groupBox2.Controls.Add(this.label10);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // tbNastSound
            // 
            this.tbNastSound.BorderColor = System.Drawing.Color.DimGray;
            this.tbNastSound.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbNastSound.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbNastSound.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbNastSound.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbNastSound.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbNastSound.HintText = null;
            resources.ApplyResources(this.tbNastSound, "tbNastSound");
            this.tbNastSound.Name = "tbNastSound";
            // 
            // tbNastFullName
            // 
            this.tbNastFullName.BorderColor = System.Drawing.Color.DimGray;
            this.tbNastFullName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbNastFullName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbNastFullName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbNastFullName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbNastFullName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbNastFullName.HintText = null;
            resources.ApplyResources(this.tbNastFullName, "tbNastFullName");
            this.tbNastFullName.Name = "tbNastFullName";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // bNastDelete
            // 
            resources.ApplyResources(this.bNastDelete, "bNastDelete");
            this.bNastDelete.Name = "bNastDelete";
            this.bNastDelete.UseVisualStyleBackColor = true;
            this.bNastDelete.Click += new System.EventHandler(this.bNastDelete_Click);
            // 
            // bNastEdit
            // 
            resources.ApplyResources(this.bNastEdit, "bNastEdit");
            this.bNastEdit.Name = "bNastEdit";
            this.bNastEdit.UseVisualStyleBackColor = true;
            this.bNastEdit.Click += new System.EventHandler(this.bNastEdit_Click);
            // 
            // bNastAdd
            // 
            resources.ApplyResources(this.bNastAdd, "bNastAdd");
            this.bNastAdd.Name = "bNastAdd";
            this.bNastAdd.UseVisualStyleBackColor = true;
            this.bNastAdd.Click += new System.EventHandler(this.bNastAdd_Click);
            // 
            // tbNastOznacenie
            // 
            this.tbNastOznacenie.BorderColor = System.Drawing.Color.DimGray;
            this.tbNastOznacenie.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbNastOznacenie.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbNastOznacenie.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbNastOznacenie.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbNastOznacenie.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbNastOznacenie.HintText = null;
            resources.ApplyResources(this.tbNastOznacenie, "tbNastOznacenie");
            this.tbNastOznacenie.Name = "tbNastOznacenie";
            this.tbNastOznacenie.TextChanged += new System.EventHandler(this.tbNastOznacenie_TextChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // tpKolaje
            // 
            this.tpKolaje.BackColor = System.Drawing.Color.Transparent;
            this.tpKolaje.Controls.Add(this.listKolaje);
            this.tpKolaje.Controls.Add(this.label15);
            this.tpKolaje.Controls.Add(this.groupBox3);
            resources.ApplyResources(this.tpKolaje, "tpKolaje");
            this.tpKolaje.Name = "tpKolaje";
            // 
            // listKolaje
            // 
            this.listKolaje.FormattingEnabled = true;
            resources.ApplyResources(this.listKolaje, "listKolaje");
            this.listKolaje.Name = "listKolaje";
            this.listKolaje.SelectedIndexChanged += new System.EventHandler(this.listKolaje_SelectedIndexChanged);
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbKolajSound);
            this.groupBox3.Controls.Add(this.label27);
            this.groupBox3.Controls.Add(this.cbNastupistia);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.clbKolajTables);
            this.groupBox3.Controls.Add(this.tbKolajFullName);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.bKolajDelete);
            this.groupBox3.Controls.Add(this.bKolajEdit);
            this.groupBox3.Controls.Add(this.bKolajAdd);
            this.groupBox3.Controls.Add(this.tbKolajOznacenie);
            this.groupBox3.Controls.Add(this.label19);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // tbKolajSound
            // 
            this.tbKolajSound.BorderColor = System.Drawing.Color.DimGray;
            this.tbKolajSound.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbKolajSound.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbKolajSound.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbKolajSound.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbKolajSound.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbKolajSound.HintText = null;
            resources.ApplyResources(this.tbKolajSound, "tbKolajSound");
            this.tbKolajSound.Name = "tbKolajSound";
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.Name = "label27";
            // 
            // cbNastupistia
            // 
            this.cbNastupistia.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbNastupistia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNastupistia.FormattingEnabled = true;
            resources.ApplyResources(this.cbNastupistia, "cbNastupistia");
            this.cbNastupistia.Name = "cbNastupistia";
            this.cbNastupistia.StyleDisabled.ArrowColor = null;
            this.cbNastupistia.StyleDisabled.BackColor = null;
            this.cbNastupistia.StyleDisabled.BorderColor = null;
            this.cbNastupistia.StyleDisabled.ButtonBackColor = null;
            this.cbNastupistia.StyleDisabled.ButtonBorderColor = null;
            this.cbNastupistia.StyleDisabled.ButtonRenderFirst = null;
            this.cbNastupistia.StyleDisabled.ForeColor = null;
            this.cbNastupistia.StyleHighlight.ArrowColor = null;
            this.cbNastupistia.StyleHighlight.BackColor = null;
            this.cbNastupistia.StyleHighlight.BorderColor = null;
            this.cbNastupistia.StyleHighlight.ButtonBackColor = null;
            this.cbNastupistia.StyleHighlight.ButtonBorderColor = null;
            this.cbNastupistia.StyleHighlight.ButtonRenderFirst = null;
            this.cbNastupistia.StyleHighlight.ForeColor = null;
            this.cbNastupistia.StyleNormal.ArrowColor = null;
            this.cbNastupistia.StyleNormal.BackColor = null;
            this.cbNastupistia.StyleNormal.BorderColor = null;
            this.cbNastupistia.StyleNormal.ButtonBackColor = null;
            this.cbNastupistia.StyleNormal.ButtonBorderColor = null;
            this.cbNastupistia.StyleNormal.ButtonRenderFirst = null;
            this.cbNastupistia.StyleNormal.ForeColor = null;
            this.cbNastupistia.StyleSelected.ArrowColor = null;
            this.cbNastupistia.StyleSelected.BackColor = null;
            this.cbNastupistia.StyleSelected.BorderColor = null;
            this.cbNastupistia.StyleSelected.ButtonBackColor = null;
            this.cbNastupistia.StyleSelected.ButtonBorderColor = null;
            this.cbNastupistia.StyleSelected.ButtonRenderFirst = null;
            this.cbNastupistia.StyleSelected.ForeColor = null;
            this.cbNastupistia.UseDarkScrollBar = false;
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // clbKolajTables
            // 
            this.clbKolajTables.DisplayMember = "This";
            this.clbKolajTables.FormattingEnabled = true;
            this.clbKolajTables.HighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            resources.ApplyResources(this.clbKolajTables, "clbKolajTables");
            this.clbKolajTables.Name = "clbKolajTables";
            this.clbKolajTables.SquareBackColor = System.Drawing.Color.White;
            this.clbKolajTables.ValueMember = "This";
            // 
            // tbKolajFullName
            // 
            this.tbKolajFullName.BorderColor = System.Drawing.Color.DimGray;
            this.tbKolajFullName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbKolajFullName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbKolajFullName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbKolajFullName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbKolajFullName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbKolajFullName.HintText = null;
            resources.ApplyResources(this.tbKolajFullName, "tbKolajFullName");
            this.tbKolajFullName.Name = "tbKolajFullName";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // bKolajDelete
            // 
            resources.ApplyResources(this.bKolajDelete, "bKolajDelete");
            this.bKolajDelete.Name = "bKolajDelete";
            this.bKolajDelete.UseVisualStyleBackColor = true;
            this.bKolajDelete.Click += new System.EventHandler(this.bKolajDelete_Click);
            // 
            // bKolajEdit
            // 
            resources.ApplyResources(this.bKolajEdit, "bKolajEdit");
            this.bKolajEdit.Name = "bKolajEdit";
            this.bKolajEdit.UseVisualStyleBackColor = true;
            this.bKolajEdit.Click += new System.EventHandler(this.bKolajEdit_Click);
            // 
            // bKolajAdd
            // 
            resources.ApplyResources(this.bKolajAdd, "bKolajAdd");
            this.bKolajAdd.Name = "bKolajAdd";
            this.bKolajAdd.UseVisualStyleBackColor = true;
            this.bKolajAdd.Click += new System.EventHandler(this.bKolajAdd_Click);
            // 
            // tbKolajOznacenie
            // 
            this.tbKolajOznacenie.BorderColor = System.Drawing.Color.DimGray;
            this.tbKolajOznacenie.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbKolajOznacenie.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbKolajOznacenie.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbKolajOznacenie.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbKolajOznacenie.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbKolajOznacenie.HintText = null;
            resources.ApplyResources(this.tbKolajOznacenie, "tbKolajOznacenie");
            this.tbKolajOznacenie.Name = "tbKolajOznacenie";
            this.tbKolajOznacenie.TextChanged += new System.EventHandler(this.tbKolajOznacenie_TextChanged);
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // tpFyzTab
            // 
            this.tpFyzTab.BackColor = System.Drawing.Color.Transparent;
            this.tpFyzTab.Controls.Add(this.label14);
            this.tpFyzTab.Controls.Add(this.tbCommentFyz);
            this.tpFyzTab.Controls.Add(this.listFyzTabule);
            this.tpFyzTab.Controls.Add(this.label22);
            this.tpFyzTab.Controls.Add(this.groupBox4);
            resources.ApplyResources(this.tpFyzTab, "tpFyzTab");
            this.tpFyzTab.Name = "tpFyzTab";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // tbCommentFyz
            // 
            this.tbCommentFyz.BorderColor = System.Drawing.Color.DimGray;
            this.tbCommentFyz.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbCommentFyz.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbCommentFyz.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCommentFyz.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbCommentFyz.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCommentFyz.HintText = null;
            resources.ApplyResources(this.tbCommentFyz, "tbCommentFyz");
            this.tbCommentFyz.Name = "tbCommentFyz";
            this.tbCommentFyz.ReadOnly = true;
            // 
            // listFyzTabule
            // 
            this.listFyzTabule.FormattingEnabled = true;
            resources.ApplyResources(this.listFyzTabule, "listFyzTabule");
            this.listFyzTabule.Name = "listFyzTabule";
            this.listFyzTabule.SelectedIndexChanged += new System.EventHandler(this.listFyzTabule_SelectedIndexChanged);
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.bFyzTabCopy);
            this.groupBox4.Controls.Add(this.bFyzTabDelete);
            this.groupBox4.Controls.Add(this.bFyzTabEdit);
            this.groupBox4.Controls.Add(this.bFyzTabAdd);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // bFyzTabCopy
            // 
            resources.ApplyResources(this.bFyzTabCopy, "bFyzTabCopy");
            this.bFyzTabCopy.Name = "bFyzTabCopy";
            this.bFyzTabCopy.UseVisualStyleBackColor = true;
            this.bFyzTabCopy.Click += new System.EventHandler(this.bFyzTabCopy_Click);
            // 
            // bFyzTabDelete
            // 
            resources.ApplyResources(this.bFyzTabDelete, "bFyzTabDelete");
            this.bFyzTabDelete.Name = "bFyzTabDelete";
            this.bFyzTabDelete.UseVisualStyleBackColor = true;
            this.bFyzTabDelete.Click += new System.EventHandler(this.bFyzTabDelete_Click);
            // 
            // bFyzTabEdit
            // 
            resources.ApplyResources(this.bFyzTabEdit, "bFyzTabEdit");
            this.bFyzTabEdit.Name = "bFyzTabEdit";
            this.bFyzTabEdit.UseVisualStyleBackColor = true;
            this.bFyzTabEdit.Click += new System.EventHandler(this.bFyzTabEdit_Click);
            // 
            // bFyzTabAdd
            // 
            resources.ApplyResources(this.bFyzTabAdd, "bFyzTabAdd");
            this.bFyzTabAdd.Name = "bFyzTabAdd";
            this.bFyzTabAdd.UseVisualStyleBackColor = true;
            this.bFyzTabAdd.Click += new System.EventHandler(this.bFyzTabAdd_Click);
            // 
            // tpLogTab
            // 
            this.tpLogTab.BackColor = System.Drawing.Color.Transparent;
            this.tpLogTab.Controls.Add(this.label46);
            this.tpLogTab.Controls.Add(this.tbCommentLog);
            this.tpLogTab.Controls.Add(this.listLogTabule);
            this.tpLogTab.Controls.Add(this.label23);
            this.tpLogTab.Controls.Add(this.groupBox5);
            resources.ApplyResources(this.tpLogTab, "tpLogTab");
            this.tpLogTab.Name = "tpLogTab";
            // 
            // label46
            // 
            resources.ApplyResources(this.label46, "label46");
            this.label46.Name = "label46";
            // 
            // tbCommentLog
            // 
            this.tbCommentLog.BorderColor = System.Drawing.Color.DimGray;
            this.tbCommentLog.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbCommentLog.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbCommentLog.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCommentLog.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbCommentLog.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCommentLog.HintText = null;
            resources.ApplyResources(this.tbCommentLog, "tbCommentLog");
            this.tbCommentLog.Name = "tbCommentLog";
            this.tbCommentLog.ReadOnly = true;
            // 
            // listLogTabule
            // 
            this.listLogTabule.FormattingEnabled = true;
            resources.ApplyResources(this.listLogTabule, "listLogTabule");
            this.listLogTabule.Name = "listLogTabule";
            this.listLogTabule.SelectedIndexChanged += new System.EventHandler(this.listLogTabule_SelectedIndexChanged);
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.bLogTabCopy);
            this.groupBox5.Controls.Add(this.bLogTabDelete);
            this.groupBox5.Controls.Add(this.bLogTabEdit);
            this.groupBox5.Controls.Add(this.bLogTabAdd);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // bLogTabCopy
            // 
            resources.ApplyResources(this.bLogTabCopy, "bLogTabCopy");
            this.bLogTabCopy.Name = "bLogTabCopy";
            this.bLogTabCopy.UseVisualStyleBackColor = true;
            this.bLogTabCopy.Click += new System.EventHandler(this.bLogTabCopy_Click);
            // 
            // bLogTabDelete
            // 
            resources.ApplyResources(this.bLogTabDelete, "bLogTabDelete");
            this.bLogTabDelete.Name = "bLogTabDelete";
            this.bLogTabDelete.UseVisualStyleBackColor = true;
            this.bLogTabDelete.Click += new System.EventHandler(this.bLogTabDelete_Click);
            // 
            // bLogTabEdit
            // 
            resources.ApplyResources(this.bLogTabEdit, "bLogTabEdit");
            this.bLogTabEdit.Name = "bLogTabEdit";
            this.bLogTabEdit.UseVisualStyleBackColor = true;
            this.bLogTabEdit.Click += new System.EventHandler(this.bLogTabEdit_Click);
            // 
            // bLogTabAdd
            // 
            resources.ApplyResources(this.bLogTabAdd, "bLogTabAdd");
            this.bLogTabAdd.Name = "bLogTabAdd";
            this.bLogTabAdd.UseVisualStyleBackColor = true;
            this.bLogTabAdd.Click += new System.EventHandler(this.bLogTabAdd_Click);
            // 
            // tpKatTab
            // 
            this.tpKatTab.BackColor = System.Drawing.Color.Transparent;
            this.tpKatTab.Controls.Add(this.label51);
            this.tpKatTab.Controls.Add(this.tbCommentKat);
            this.tpKatTab.Controls.Add(this.listKatTabule);
            this.tpKatTab.Controls.Add(this.label24);
            this.tpKatTab.Controls.Add(this.groupBox6);
            resources.ApplyResources(this.tpKatTab, "tpKatTab");
            this.tpKatTab.Name = "tpKatTab";
            // 
            // label51
            // 
            resources.ApplyResources(this.label51, "label51");
            this.label51.Name = "label51";
            // 
            // tbCommentKat
            // 
            this.tbCommentKat.BorderColor = System.Drawing.Color.DimGray;
            this.tbCommentKat.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbCommentKat.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbCommentKat.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCommentKat.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbCommentKat.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCommentKat.HintText = null;
            resources.ApplyResources(this.tbCommentKat, "tbCommentKat");
            this.tbCommentKat.Name = "tbCommentKat";
            this.tbCommentKat.ReadOnly = true;
            // 
            // listKatTabule
            // 
            this.listKatTabule.FormattingEnabled = true;
            resources.ApplyResources(this.listKatTabule, "listKatTabule");
            this.listKatTabule.Name = "listKatTabule";
            this.listKatTabule.SelectedIndexChanged += new System.EventHandler(this.listKatTabule_SelectedIndexChanged);
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.bKatTabCopy);
            this.groupBox6.Controls.Add(this.bKatTabDelete);
            this.groupBox6.Controls.Add(this.bKatTabEdit);
            this.groupBox6.Controls.Add(this.bKatTabAdd);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // bKatTabCopy
            // 
            resources.ApplyResources(this.bKatTabCopy, "bKatTabCopy");
            this.bKatTabCopy.Name = "bKatTabCopy";
            this.bKatTabCopy.UseVisualStyleBackColor = true;
            this.bKatTabCopy.Click += new System.EventHandler(this.bKatTabCopy_Click);
            // 
            // bKatTabDelete
            // 
            resources.ApplyResources(this.bKatTabDelete, "bKatTabDelete");
            this.bKatTabDelete.Name = "bKatTabDelete";
            this.bKatTabDelete.UseVisualStyleBackColor = true;
            this.bKatTabDelete.Click += new System.EventHandler(this.bKatTabDelete_Click);
            // 
            // bKatTabEdit
            // 
            resources.ApplyResources(this.bKatTabEdit, "bKatTabEdit");
            this.bKatTabEdit.Name = "bKatTabEdit";
            this.bKatTabEdit.UseVisualStyleBackColor = true;
            this.bKatTabEdit.Click += new System.EventHandler(this.bKatTabEdit_Click);
            // 
            // bKatTabAdd
            // 
            resources.ApplyResources(this.bKatTabAdd, "bKatTabAdd");
            this.bKatTabAdd.Name = "bKatTabAdd";
            this.bKatTabAdd.UseVisualStyleBackColor = true;
            this.bKatTabAdd.Click += new System.EventHandler(this.bKatTabAdd_Click);
            // 
            // tpTabTab
            // 
            this.tpTabTab.BackColor = System.Drawing.Color.Transparent;
            this.tpTabTab.Controls.Add(this.listTabTabs);
            this.tpTabTab.Controls.Add(this.label25);
            this.tpTabTab.Controls.Add(this.groupBox7);
            resources.ApplyResources(this.tpTabTab, "tpTabTab");
            this.tpTabTab.Name = "tpTabTab";
            // 
            // listTabTabs
            // 
            this.listTabTabs.FormattingEnabled = true;
            resources.ApplyResources(this.listTabTabs, "listTabTabs");
            this.listTabTabs.Name = "listTabTabs";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.bOpenEditorTab);
            this.groupBox7.Controls.Add(this.bTabTabDelete);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // bOpenEditorTab
            // 
            resources.ApplyResources(this.bOpenEditorTab, "bOpenEditorTab");
            this.bOpenEditorTab.Name = "bOpenEditorTab";
            this.bOpenEditorTab.UseVisualStyleBackColor = true;
            this.bOpenEditorTab.Click += new System.EventHandler(this.bOpenEditorTab_Click);
            // 
            // bTabTabDelete
            // 
            resources.ApplyResources(this.bTabTabDelete, "bTabTabDelete");
            this.bTabTabDelete.Name = "bTabTabDelete";
            this.bTabTabDelete.UseVisualStyleBackColor = true;
            this.bTabTabDelete.Click += new System.EventHandler(this.bTabTabDelete_Click);
            // 
            // tpTTexts
            // 
            this.tpTTexts.BackColor = System.Drawing.Color.Transparent;
            this.tpTTexts.Controls.Add(this.label50);
            this.tpTTexts.Controls.Add(this.tbCommentTText);
            this.tpTTexts.Controls.Add(this.listTexty);
            this.tpTTexts.Controls.Add(this.label28);
            this.tpTTexts.Controls.Add(this.groupBox8);
            resources.ApplyResources(this.tpTTexts, "tpTTexts");
            this.tpTTexts.Name = "tpTTexts";
            // 
            // label50
            // 
            resources.ApplyResources(this.label50, "label50");
            this.label50.Name = "label50";
            // 
            // tbCommentTText
            // 
            this.tbCommentTText.BorderColor = System.Drawing.Color.DimGray;
            this.tbCommentTText.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbCommentTText.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbCommentTText.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCommentTText.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbCommentTText.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCommentTText.HintText = null;
            resources.ApplyResources(this.tbCommentTText, "tbCommentTText");
            this.tbCommentTText.Name = "tbCommentTText";
            this.tbCommentTText.ReadOnly = true;
            // 
            // listTexty
            // 
            this.listTexty.FormattingEnabled = true;
            resources.ApplyResources(this.listTexty, "listTexty");
            this.listTexty.Name = "listTexty";
            this.listTexty.SelectedIndexChanged += new System.EventHandler(this.listTexty_SelectedIndexChanged);
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.bTextDelete);
            this.groupBox8.Controls.Add(this.bTextEdit);
            this.groupBox8.Controls.Add(this.bTextAdd);
            resources.ApplyResources(this.groupBox8, "groupBox8");
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.TabStop = false;
            // 
            // bTextDelete
            // 
            resources.ApplyResources(this.bTextDelete, "bTextDelete");
            this.bTextDelete.Name = "bTextDelete";
            this.bTextDelete.UseVisualStyleBackColor = true;
            this.bTextDelete.Click += new System.EventHandler(this.bTextDelete_Click);
            // 
            // bTextEdit
            // 
            resources.ApplyResources(this.bTextEdit, "bTextEdit");
            this.bTextEdit.Name = "bTextEdit";
            this.bTextEdit.UseVisualStyleBackColor = true;
            this.bTextEdit.Click += new System.EventHandler(this.bTextEdit_Click);
            // 
            // bTextAdd
            // 
            resources.ApplyResources(this.bTextAdd, "bTextAdd");
            this.bTextAdd.Name = "bTextAdd";
            this.bTextAdd.UseVisualStyleBackColor = true;
            this.bTextAdd.Click += new System.EventHandler(this.bTextAdd_Click);
            // 
            // tpFonts
            // 
            this.tpFonts.BackColor = System.Drawing.Color.Transparent;
            this.tpFonts.Controls.Add(this.bOpenFontDir);
            this.tpFonts.Controls.Add(this.tbFontDir);
            this.tpFonts.Controls.Add(this.label37);
            this.tpFonts.Controls.Add(this.listFonts);
            this.tpFonts.Controls.Add(this.label29);
            this.tpFonts.Controls.Add(this.groupBox9);
            resources.ApplyResources(this.tpFonts, "tpFonts");
            this.tpFonts.Name = "tpFonts";
            // 
            // bOpenFontDir
            // 
            resources.ApplyResources(this.bOpenFontDir, "bOpenFontDir");
            this.bOpenFontDir.Name = "bOpenFontDir";
            this.bOpenFontDir.UseVisualStyleBackColor = true;
            this.bOpenFontDir.Click += new System.EventHandler(this.bOpenFontDir_Click);
            // 
            // tbFontDir
            // 
            this.tbFontDir.BorderColor = System.Drawing.Color.DimGray;
            this.tbFontDir.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbFontDir.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbFontDir.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbFontDir.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbFontDir.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbFontDir.HintText = null;
            resources.ApplyResources(this.tbFontDir, "tbFontDir");
            this.tbFontDir.Name = "tbFontDir";
            this.tbFontDir.ReadOnly = true;
            // 
            // label37
            // 
            resources.ApplyResources(this.label37, "label37");
            this.label37.Name = "label37";
            // 
            // listFonts
            // 
            this.listFonts.FormattingEnabled = true;
            resources.ApplyResources(this.listFonts, "listFonts");
            this.listFonts.Name = "listFonts";
            this.listFonts.SelectedIndexChanged += new System.EventHandler(this.listFonts_SelectedIndexChanged);
            // 
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label32);
            this.groupBox9.Controls.Add(this.cbFontIsNumber);
            this.groupBox9.Controls.Add(this.tbFontFile);
            this.groupBox9.Controls.Add(this.cbFontSpecAssigments);
            this.groupBox9.Controls.Add(this.cbFontSpecChar);
            this.groupBox9.Controls.Add(this.cbFontUpper);
            this.groupBox9.Controls.Add(this.cbFontLower);
            this.groupBox9.Controls.Add(this.cbFontDia);
            this.groupBox9.Controls.Add(this.cbFontProportional);
            this.groupBox9.Controls.Add(this.label36);
            this.groupBox9.Controls.Add(this.cbFontType);
            this.groupBox9.Controls.Add(this.label35);
            this.groupBox9.Controls.Add(this.nudFontWidth);
            this.groupBox9.Controls.Add(this.label34);
            this.groupBox9.Controls.Add(this.nudFontSize);
            this.groupBox9.Controls.Add(this.label33);
            this.groupBox9.Controls.Add(this.tbFontName);
            this.groupBox9.Controls.Add(this.label31);
            this.groupBox9.Controls.Add(this.nudFontID);
            this.groupBox9.Controls.Add(this.label30);
            this.groupBox9.Controls.Add(this.bFontDelete);
            this.groupBox9.Controls.Add(this.bFontEdit);
            this.groupBox9.Controls.Add(this.bFontAdd);
            resources.ApplyResources(this.groupBox9, "groupBox9");
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.TabStop = false;
            // 
            // label32
            // 
            resources.ApplyResources(this.label32, "label32");
            this.label32.Name = "label32";
            // 
            // cbFontIsNumber
            // 
            resources.ApplyResources(this.cbFontIsNumber, "cbFontIsNumber");
            this.cbFontIsNumber.BoxBackColor = System.Drawing.Color.White;
            this.cbFontIsNumber.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbFontIsNumber.Name = "cbFontIsNumber";
            this.cbFontIsNumber.UseVisualStyleBackColor = true;
            // 
            // tbFontFile
            // 
            this.tbFontFile.BorderColor = System.Drawing.Color.DimGray;
            this.tbFontFile.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbFontFile.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbFontFile.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbFontFile.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbFontFile.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbFontFile.HintText = null;
            resources.ApplyResources(this.tbFontFile, "tbFontFile");
            this.tbFontFile.Name = "tbFontFile";
            // 
            // cbFontSpecAssigments
            // 
            resources.ApplyResources(this.cbFontSpecAssigments, "cbFontSpecAssigments");
            this.cbFontSpecAssigments.BoxBackColor = System.Drawing.Color.White;
            this.cbFontSpecAssigments.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbFontSpecAssigments.Name = "cbFontSpecAssigments";
            this.cbFontSpecAssigments.UseVisualStyleBackColor = true;
            // 
            // cbFontSpecChar
            // 
            resources.ApplyResources(this.cbFontSpecChar, "cbFontSpecChar");
            this.cbFontSpecChar.BoxBackColor = System.Drawing.Color.White;
            this.cbFontSpecChar.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbFontSpecChar.Name = "cbFontSpecChar";
            this.cbFontSpecChar.UseVisualStyleBackColor = true;
            // 
            // cbFontUpper
            // 
            resources.ApplyResources(this.cbFontUpper, "cbFontUpper");
            this.cbFontUpper.BoxBackColor = System.Drawing.Color.White;
            this.cbFontUpper.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbFontUpper.Name = "cbFontUpper";
            this.cbFontUpper.UseVisualStyleBackColor = true;
            // 
            // cbFontLower
            // 
            resources.ApplyResources(this.cbFontLower, "cbFontLower");
            this.cbFontLower.BoxBackColor = System.Drawing.Color.White;
            this.cbFontLower.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbFontLower.Name = "cbFontLower";
            this.cbFontLower.UseVisualStyleBackColor = true;
            // 
            // cbFontDia
            // 
            resources.ApplyResources(this.cbFontDia, "cbFontDia");
            this.cbFontDia.BoxBackColor = System.Drawing.Color.White;
            this.cbFontDia.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbFontDia.Name = "cbFontDia";
            this.cbFontDia.UseVisualStyleBackColor = true;
            // 
            // cbFontProportional
            // 
            resources.ApplyResources(this.cbFontProportional, "cbFontProportional");
            this.cbFontProportional.BoxBackColor = System.Drawing.Color.White;
            this.cbFontProportional.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbFontProportional.Name = "cbFontProportional";
            this.cbFontProportional.UseVisualStyleBackColor = true;
            // 
            // label36
            // 
            resources.ApplyResources(this.label36, "label36");
            this.label36.Name = "label36";
            // 
            // cbFontType
            // 
            this.cbFontType.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbFontType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFontType.FormattingEnabled = true;
            resources.ApplyResources(this.cbFontType, "cbFontType");
            this.cbFontType.Name = "cbFontType";
            this.cbFontType.StyleDisabled.ArrowColor = null;
            this.cbFontType.StyleDisabled.BackColor = null;
            this.cbFontType.StyleDisabled.BorderColor = null;
            this.cbFontType.StyleDisabled.ButtonBackColor = null;
            this.cbFontType.StyleDisabled.ButtonBorderColor = null;
            this.cbFontType.StyleDisabled.ButtonRenderFirst = null;
            this.cbFontType.StyleDisabled.ForeColor = null;
            this.cbFontType.StyleHighlight.ArrowColor = null;
            this.cbFontType.StyleHighlight.BackColor = null;
            this.cbFontType.StyleHighlight.BorderColor = null;
            this.cbFontType.StyleHighlight.ButtonBackColor = null;
            this.cbFontType.StyleHighlight.ButtonBorderColor = null;
            this.cbFontType.StyleHighlight.ButtonRenderFirst = null;
            this.cbFontType.StyleHighlight.ForeColor = null;
            this.cbFontType.StyleNormal.ArrowColor = null;
            this.cbFontType.StyleNormal.BackColor = null;
            this.cbFontType.StyleNormal.BorderColor = null;
            this.cbFontType.StyleNormal.ButtonBackColor = null;
            this.cbFontType.StyleNormal.ButtonBorderColor = null;
            this.cbFontType.StyleNormal.ButtonRenderFirst = null;
            this.cbFontType.StyleNormal.ForeColor = null;
            this.cbFontType.StyleSelected.ArrowColor = null;
            this.cbFontType.StyleSelected.BackColor = null;
            this.cbFontType.StyleSelected.BorderColor = null;
            this.cbFontType.StyleSelected.ButtonBackColor = null;
            this.cbFontType.StyleSelected.ButtonBorderColor = null;
            this.cbFontType.StyleSelected.ButtonRenderFirst = null;
            this.cbFontType.StyleSelected.ForeColor = null;
            this.cbFontType.UseDarkScrollBar = false;
            // 
            // label35
            // 
            resources.ApplyResources(this.label35, "label35");
            this.label35.Name = "label35";
            // 
            // nudFontWidth
            // 
            this.nudFontWidth.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudFontWidth, "nudFontWidth");
            this.nudFontWidth.Name = "nudFontWidth";
            this.nudFontWidth.SelectedButtonColor = System.Drawing.Color.Empty;
            // 
            // label34
            // 
            resources.ApplyResources(this.label34, "label34");
            this.label34.Name = "label34";
            // 
            // nudFontSize
            // 
            this.nudFontSize.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudFontSize, "nudFontSize");
            this.nudFontSize.Name = "nudFontSize";
            this.nudFontSize.SelectedButtonColor = System.Drawing.Color.Empty;
            // 
            // label33
            // 
            resources.ApplyResources(this.label33, "label33");
            this.label33.Name = "label33";
            // 
            // tbFontName
            // 
            this.tbFontName.BorderColor = System.Drawing.Color.DimGray;
            this.tbFontName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbFontName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbFontName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbFontName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbFontName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbFontName.HintText = null;
            resources.ApplyResources(this.tbFontName, "tbFontName");
            this.tbFontName.Name = "tbFontName";
            // 
            // label31
            // 
            resources.ApplyResources(this.label31, "label31");
            this.label31.Name = "label31";
            // 
            // nudFontID
            // 
            this.nudFontID.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudFontID, "nudFontID");
            this.nudFontID.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudFontID.Name = "nudFontID";
            this.nudFontID.SelectedButtonColor = System.Drawing.Color.Empty;
            this.nudFontID.ValueChanged += new System.EventHandler(this.nudFontID_ValueChanged);
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            // 
            // bFontDelete
            // 
            resources.ApplyResources(this.bFontDelete, "bFontDelete");
            this.bFontDelete.Name = "bFontDelete";
            this.bFontDelete.UseVisualStyleBackColor = true;
            this.bFontDelete.Click += new System.EventHandler(this.bFontDelete_Click);
            // 
            // bFontEdit
            // 
            resources.ApplyResources(this.bFontEdit, "bFontEdit");
            this.bFontEdit.Name = "bFontEdit";
            this.bFontEdit.UseVisualStyleBackColor = true;
            this.bFontEdit.Click += new System.EventHandler(this.bFontEdit_Click);
            // 
            // bFontAdd
            // 
            resources.ApplyResources(this.bFontAdd, "bFontAdd");
            this.bFontAdd.Name = "bFontAdd";
            this.bFontAdd.UseVisualStyleBackColor = true;
            this.bFontAdd.Click += new System.EventHandler(this.bFontAdd_Click);
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.BoxBackColor = System.Drawing.Color.White;
            this.checkBox1.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // cbCustomTrainTypDruh
            // 
            this.cbCustomTrainTypDruh.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.cbCustomTrainTypDruh, "cbCustomTrainTypDruh");
            this.cbCustomTrainTypDruh.Name = "cbCustomTrainTypDruh";
            this.cbCustomTrainTypDruh.StyleDisabled.ArrowColor = null;
            this.cbCustomTrainTypDruh.StyleDisabled.BackColor = null;
            this.cbCustomTrainTypDruh.StyleDisabled.BorderColor = null;
            this.cbCustomTrainTypDruh.StyleDisabled.ButtonBackColor = null;
            this.cbCustomTrainTypDruh.StyleDisabled.ButtonBorderColor = null;
            this.cbCustomTrainTypDruh.StyleDisabled.ButtonRenderFirst = null;
            this.cbCustomTrainTypDruh.StyleDisabled.ForeColor = null;
            this.cbCustomTrainTypDruh.StyleHighlight.ArrowColor = null;
            this.cbCustomTrainTypDruh.StyleHighlight.BackColor = null;
            this.cbCustomTrainTypDruh.StyleHighlight.BorderColor = null;
            this.cbCustomTrainTypDruh.StyleHighlight.ButtonBackColor = null;
            this.cbCustomTrainTypDruh.StyleHighlight.ButtonBorderColor = null;
            this.cbCustomTrainTypDruh.StyleHighlight.ButtonRenderFirst = null;
            this.cbCustomTrainTypDruh.StyleHighlight.ForeColor = null;
            this.cbCustomTrainTypDruh.StyleNormal.ArrowColor = null;
            this.cbCustomTrainTypDruh.StyleNormal.BackColor = null;
            this.cbCustomTrainTypDruh.StyleNormal.BorderColor = null;
            this.cbCustomTrainTypDruh.StyleNormal.ButtonBackColor = null;
            this.cbCustomTrainTypDruh.StyleNormal.ButtonBorderColor = null;
            this.cbCustomTrainTypDruh.StyleNormal.ButtonRenderFirst = null;
            this.cbCustomTrainTypDruh.StyleNormal.ForeColor = null;
            this.cbCustomTrainTypDruh.StyleSelected.ArrowColor = null;
            this.cbCustomTrainTypDruh.StyleSelected.BackColor = null;
            this.cbCustomTrainTypDruh.StyleSelected.BorderColor = null;
            this.cbCustomTrainTypDruh.StyleSelected.ButtonBackColor = null;
            this.cbCustomTrainTypDruh.StyleSelected.ButtonBorderColor = null;
            this.cbCustomTrainTypDruh.StyleSelected.ButtonRenderFirst = null;
            this.cbCustomTrainTypDruh.StyleSelected.ForeColor = null;
            this.cbCustomTrainTypDruh.UseDarkScrollBar = false;
            // 
            // cbDefTrainTypSkratka
            // 
            this.cbDefTrainTypSkratka.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.cbDefTrainTypSkratka, "cbDefTrainTypSkratka");
            this.cbDefTrainTypSkratka.Name = "cbDefTrainTypSkratka";
            this.cbDefTrainTypSkratka.StyleDisabled.ArrowColor = null;
            this.cbDefTrainTypSkratka.StyleDisabled.BackColor = null;
            this.cbDefTrainTypSkratka.StyleDisabled.BorderColor = null;
            this.cbDefTrainTypSkratka.StyleDisabled.ButtonBackColor = null;
            this.cbDefTrainTypSkratka.StyleDisabled.ButtonBorderColor = null;
            this.cbDefTrainTypSkratka.StyleDisabled.ButtonRenderFirst = null;
            this.cbDefTrainTypSkratka.StyleDisabled.ForeColor = null;
            this.cbDefTrainTypSkratka.StyleHighlight.ArrowColor = null;
            this.cbDefTrainTypSkratka.StyleHighlight.BackColor = null;
            this.cbDefTrainTypSkratka.StyleHighlight.BorderColor = null;
            this.cbDefTrainTypSkratka.StyleHighlight.ButtonBackColor = null;
            this.cbDefTrainTypSkratka.StyleHighlight.ButtonBorderColor = null;
            this.cbDefTrainTypSkratka.StyleHighlight.ButtonRenderFirst = null;
            this.cbDefTrainTypSkratka.StyleHighlight.ForeColor = null;
            this.cbDefTrainTypSkratka.StyleNormal.ArrowColor = null;
            this.cbDefTrainTypSkratka.StyleNormal.BackColor = null;
            this.cbDefTrainTypSkratka.StyleNormal.BorderColor = null;
            this.cbDefTrainTypSkratka.StyleNormal.ButtonBackColor = null;
            this.cbDefTrainTypSkratka.StyleNormal.ButtonBorderColor = null;
            this.cbDefTrainTypSkratka.StyleNormal.ButtonRenderFirst = null;
            this.cbDefTrainTypSkratka.StyleNormal.ForeColor = null;
            this.cbDefTrainTypSkratka.StyleSelected.ArrowColor = null;
            this.cbDefTrainTypSkratka.StyleSelected.BackColor = null;
            this.cbDefTrainTypSkratka.StyleSelected.BorderColor = null;
            this.cbDefTrainTypSkratka.StyleSelected.ButtonBackColor = null;
            this.cbDefTrainTypSkratka.StyleSelected.ButtonBorderColor = null;
            this.cbDefTrainTypSkratka.StyleSelected.ButtonRenderFirst = null;
            this.cbDefTrainTypSkratka.StyleSelected.ForeColor = null;
            this.cbDefTrainTypSkratka.UseDarkScrollBar = false;
            // 
            // FLocalSettings
            // 
            this.AcceptButton = this.bSave;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FLocalSettings";
            this.ShowInTaskbar = false;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FLocalSettings_HelpButtonClicked);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FLocalSettings_FormClosed);
            this.Load += new System.EventHandler(this.FLocalSettings_Load);
            this.tabControl.ResumeLayout(false);
            this.tpGrafikon.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudIDStation)).EndInit();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.tpStanice.ResumeLayout(false);
            this.tpStanice.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudIDStanice)).EndInit();
            this.tpDopravcovia.ResumeLayout(false);
            this.tpDopravcovia.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tpNastupistia.ResumeLayout(false);
            this.tpNastupistia.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tpKolaje.ResumeLayout(false);
            this.tpKolaje.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tpFyzTab.ResumeLayout(false);
            this.tpFyzTab.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tpLogTab.ResumeLayout(false);
            this.tpLogTab.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tpKatTab.ResumeLayout(false);
            this.tpKatTab.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tpTabTab.ResumeLayout(false);
            this.tpTabTab.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tpTTexts.ResumeLayout(false);
            this.tpTTexts.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tpFonts.ResumeLayout(false);
            this.tpFonts.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExControls.ExButton bSave;
        private System.Windows.Forms.Label label16;
        private ExControls.ExButton button3;
        private ExControls.ExButton button2;
        private ExControls.ExButton button1;
        private ExTextBox textBox1;
        private ExCheckBox checkBox1;
        private System.Windows.Forms.Label label17;
        private ExTextBox textBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tpDopravcovia;
        private System.Windows.Forms.ListBox listDopravcovia;
        private System.Windows.Forms.Label label7;
        private ExGroupBox groupBox1;
        private ExControls.ExButton bDopravcaDelete;
        private ExControls.ExButton bDopravcaEdit;
        private ExControls.ExButton bDopravcaAdd;
        private ExTextBox tbDopravca;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tpGrafikon;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpDataDo;
        private System.Windows.Forms.DateTimePicker dtpDataOd;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private ExTextBox tbDir;
        private ExControls.ExButton bOpenDir;
        private System.Windows.Forms.Label label9;
        private DateTimePicker dtpGVDDo;
        private System.Windows.Forms.DateTimePicker dtpGVDOd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private ExTabControl tabControl;
        private System.Windows.Forms.TabPage tpNastupistia;
        private System.Windows.Forms.ListBox listNastupistia;
        private System.Windows.Forms.Label label8;
        private ExGroupBox groupBox2;
        private ExTextBox tbNastFullName;
        private System.Windows.Forms.Label label12;
        private ExControls.ExButton bNastDelete;
        private ExControls.ExButton bNastEdit;
        private ExControls.ExButton bNastAdd;
        private ExTextBox tbNastOznacenie;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tpKolaje;
        private System.Windows.Forms.ListBox listKolaje;
        private System.Windows.Forms.Label label15;
        private ExGroupBox groupBox3;
        private System.Windows.Forms.Label label20;
        private ExCheckedListBox clbKolajTables;
        private ExTextBox tbKolajFullName;
        private System.Windows.Forms.Label label18;
        private ExControls.ExButton bKolajDelete;
        private ExControls.ExButton bKolajEdit;
        private ExControls.ExButton bKolajAdd;
        private ExTextBox tbKolajOznacenie;
        private System.Windows.Forms.Label label19;
        private ExComboBox cbNastupistia;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TabPage tpFyzTab;
        private System.Windows.Forms.TabPage tpKatTab;
        private System.Windows.Forms.TabPage tpTabTab;
        private System.Windows.Forms.ListBox listFyzTabule;
        private System.Windows.Forms.Label label22;
        private ExGroupBox groupBox4;
        private ExControls.ExButton bFyzTabDelete;
        private ExControls.ExButton bFyzTabEdit;
        private ExControls.ExButton bFyzTabAdd;
        private System.Windows.Forms.TabPage tpLogTab;
        private System.Windows.Forms.ListBox listLogTabule;
        private System.Windows.Forms.Label label23;
        private ExGroupBox groupBox5;
        private ExControls.ExButton bLogTabDelete;
        private ExControls.ExButton bLogTabEdit;
        private ExControls.ExButton bLogTabAdd;
        private System.Windows.Forms.ListBox listKatTabule;
        private System.Windows.Forms.Label label24;
        private ExGroupBox groupBox6;
        private ExControls.ExButton bKatTabDelete;
        private ExControls.ExButton bKatTabEdit;
        private ExControls.ExButton bKatTabAdd;
        private System.Windows.Forms.ListBox listTabTabs;
        private System.Windows.Forms.Label label25;
        private ExGroupBox groupBox7;
        private ExControls.ExButton bTabTabDelete;
        private System.Windows.Forms.Label label26;
        private ExTextBox tbNastSound;
        private ExTextBox tbKolajSound;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TabPage tpTTexts;
        private System.Windows.Forms.ListBox listTexty;
        private System.Windows.Forms.Label label28;
        private ExGroupBox groupBox8;
        private ExControls.ExButton bTextDelete;
        private ExControls.ExButton bTextEdit;
        private ExControls.ExButton bTextAdd;
        private System.Windows.Forms.TabPage tpFonts;
        private System.Windows.Forms.ListBox listFonts;
        private System.Windows.Forms.Label label29;
        private ExGroupBox groupBox9;
        private ExNumericUpDown nudFontWidth;
        private System.Windows.Forms.Label label34;
        private ExNumericUpDown nudFontSize;
        private System.Windows.Forms.Label label33;
        private ExTextBox tbFontName;
        private System.Windows.Forms.Label label31;
        private ExNumericUpDown nudFontID;
        private System.Windows.Forms.Label label30;
        private ExControls.ExButton bFontDelete;
        private ExControls.ExButton bFontEdit;
        private ExControls.ExButton bFontAdd;
        private System.Windows.Forms.Label label32;
        private ExTextBox tbFontFile;
        private ExCheckBox cbFontSpecAssigments;
        private ExCheckBox cbFontSpecChar;
        private ExCheckBox cbFontUpper;
        private ExCheckBox cbFontLower;
        private ExCheckBox cbFontDia;
        private ExCheckBox cbFontProportional;
        private System.Windows.Forms.Label label36;
        private ExComboBox cbFontType;
        private System.Windows.Forms.Label label35;
        private ExCheckBox cbFontIsNumber;
        private ExControls.ExButton bOpenFontDir;
        private ExTextBox tbFontDir;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.FolderBrowserDialog fdbFontsDir;
        private ExControls.ExButton bFyzTabCopy;
        private ExControls.ExButton bLogTabCopy;
        private ExControls.ExButton bKatTabCopy;
        private ExGroupBox groupBox11;
        private System.Windows.Forms.Label label42;
        private ExTextBox tbCustomTrainTypText;
        private ExTextBox tbCustomTrainTypSkratka;
        private System.Windows.Forms.Label label41;
        private ExComboBox cbCustomTrainTypDruh;
        private System.Windows.Forms.Label label40;
        private ExControls.ExButton bCustomTrainTypDelete;
        private ExControls.ExButton bCustomTrainTypAdd;
        private ExControls.ExButton bCustomTrainTypEdit;
        private System.Windows.Forms.ListBox listTrainTypes;
        private System.Windows.Forms.Label label38;
        private ExGroupBox groupBox10;
        private ExComboBox cbDefTrainTypSkratka;
        private ExControls.ExButton bDefTrainTypDelete;
        private ExControls.ExButton bDefTrainTypEdit;
        private ExControls.ExButton bDefTrainTypAdd;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TabPage tpStanice;
        private System.Windows.Forms.ListBox listCustomStations;
        private System.Windows.Forms.Label label43;
        private ExGroupBox groupBox12;
        private System.Windows.Forms.Label label45;
        private ExNumericUpDown nudIDStanice;
        private ExControls.ExButton bCStationDelete;
        private ExControls.ExButton bCStationEdit;
        private ExControls.ExButton bCStationAdd;
        private ExTextBox tbStationName;
        private System.Windows.Forms.Label label44;
        private ExGroupBox groupBox13;
        private ExGroupBox groupBox14;
        private ExTextBox tbGVDStationName;
        private ExCheckBox cbCustomStation;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label48;
        private ExNumericUpDown nudIDStation;
        private System.Windows.Forms.Label label49;
        private ExComboBox cbStationName;
        private ExGroupBox groupBox15;
        private ExTextBox tbDirName;
        private ExControls.ExButton bDirChange;
        private ExLineSeparator dynamicLineSeparator1;
        private ExControls.ExButton bOpenEditorTab;
        private Label label14;
        private ExTextBox tbCommentFyz;
        private Label label46;
        private ExTextBox tbCommentLog;
        private Label label50;
        private ExTextBox tbCommentTText;
        private Label label51;
        private ExTextBox tbCommentKat;
    }
}