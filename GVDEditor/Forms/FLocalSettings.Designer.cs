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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FLocalSettings));
            ExComboBoxStyle exComboBoxStyle1 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle2 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle3 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle4 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle5 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle6 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle7 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle8 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle9 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle10 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle11 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle12 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle13 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle14 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle15 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle16 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle17 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle18 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle19 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle20 = new ExComboBoxStyle();
            bSave = new ExButton();
            label16 = new Label();
            button3 = new ExButton();
            button2 = new ExButton();
            button1 = new ExButton();
            textBox1 = new ExTextBox();
            label17 = new Label();
            textBox2 = new ExTextBox();
            label6 = new Label();
            label5 = new Label();
            groupBox11 = new ExGroupBox();
            label42 = new Label();
            tbCustomTrainTypText = new ExTextBox();
            tbCustomTrainTypSkratka = new ExTextBox();
            label41 = new Label();
            label40 = new Label();
            bCustomTrainTypDelete = new ExButton();
            bCustomTrainTypAdd = new ExButton();
            bCustomTrainTypEdit = new ExButton();
            listTrainTypes = new ListBox();
            label38 = new Label();
            groupBox10 = new ExGroupBox();
            bDefTrainTypDelete = new ExButton();
            bDefTrainTypEdit = new ExButton();
            bDefTrainTypAdd = new ExButton();
            label39 = new Label();
            fdbFontsDir = new FolderBrowserDialog();
            tabControl = new ExTabControl();
            tpGrafikon = new TabPage();
            groupBox15 = new ExGroupBox();
            bDirChange = new ExButton();
            tbDir = new ExTextBox();
            tbDirName = new ExTextBox();
            label9 = new Label();
            label4 = new Label();
            bOpenDir = new ExButton();
            groupBox14 = new ExGroupBox();
            dynamicLineSeparator1 = new ExLineSeparator();
            tbGVDStationName = new ExTextBox();
            cbCustomStation = new ExCheckBox();
            label47 = new Label();
            label48 = new Label();
            nudIDStation = new ExNumericUpDown();
            label49 = new Label();
            cbStationName = new ExComboBox();
            groupBox13 = new ExGroupBox();
            label2 = new Label();
            label3 = new Label();
            dtpGVDOd = new ExControls.ExDateTimePicker();
            dtpGVDDo = new ExControls.ExDateTimePicker();
            label13 = new Label();
            dtpDataDo = new ExControls.ExDateTimePicker();
            label11 = new Label();
            dtpDataOd = new ExControls.ExDateTimePicker();
            tpStanice = new TabPage();
            listCustomStations = new ListBox();
            label43 = new Label();
            groupBox12 = new ExGroupBox();
            label45 = new Label();
            nudIDStanice = new ExNumericUpDown();
            bCStationDelete = new ExButton();
            bCStationEdit = new ExButton();
            bCStationAdd = new ExButton();
            tbStationName = new ExTextBox();
            label44 = new Label();
            tpDopravcovia = new TabPage();
            listDopravcovia = new ListBox();
            label7 = new Label();
            groupBox1 = new ExGroupBox();
            bDopravcaDelete = new ExButton();
            bDopravcaEdit = new ExButton();
            bDopravcaAdd = new ExButton();
            tbDopravca = new ExTextBox();
            label1 = new Label();
            tpNastupistia = new TabPage();
            listNastupistia = new ListBox();
            label8 = new Label();
            groupBox2 = new ExGroupBox();
            label26 = new Label();
            tbNastSound = new ExTextBox();
            tbNastFullName = new ExTextBox();
            label12 = new Label();
            bNastDelete = new ExButton();
            bNastEdit = new ExButton();
            bNastAdd = new ExButton();
            tbNastOznacenie = new ExTextBox();
            label10 = new Label();
            tpKolaje = new TabPage();
            listKolaje = new ListBox();
            label15 = new Label();
            groupBox3 = new ExGroupBox();
            tbKolajAlt = new ExTextBox();
            label53 = new Label();
            tbNastupisteKolaj = new ExTextBox();
            label52 = new Label();
            tbKolajSound = new ExTextBox();
            label27 = new Label();
            cbNastupistia = new ExComboBox();
            label21 = new Label();
            label20 = new Label();
            clbKolajTables = new ExCheckedListBox();
            tbKolajFullName = new ExTextBox();
            label18 = new Label();
            bKolajDelete = new ExButton();
            bKolajEdit = new ExButton();
            bKolajAdd = new ExButton();
            tbKolajOznacenie = new ExTextBox();
            label19 = new Label();
            tpFyzTab = new TabPage();
            label14 = new Label();
            tbCommentFyz = new ExTextBox();
            listFyzTabule = new ListBox();
            label22 = new Label();
            groupBox4 = new ExGroupBox();
            bFyzTabCopy = new ExButton();
            bFyzTabDelete = new ExButton();
            bFyzTabEdit = new ExButton();
            bFyzTabAdd = new ExButton();
            tpLogTab = new TabPage();
            label46 = new Label();
            tbCommentLog = new ExTextBox();
            listLogTabule = new ListBox();
            label23 = new Label();
            groupBox5 = new ExGroupBox();
            bLogTabCopy = new ExButton();
            bLogTabDelete = new ExButton();
            bLogTabEdit = new ExButton();
            bLogTabAdd = new ExButton();
            tpKatTab = new TabPage();
            label51 = new Label();
            tbCommentKat = new ExTextBox();
            listKatTabule = new ListBox();
            label24 = new Label();
            groupBox6 = new ExGroupBox();
            bKatTabCopy = new ExButton();
            bKatTabDelete = new ExButton();
            bKatTabEdit = new ExButton();
            bKatTabAdd = new ExButton();
            tpTabTab = new TabPage();
            listTabTabs = new ListBox();
            label25 = new Label();
            groupBox7 = new ExGroupBox();
            bOpenEditorTab = new ExButton();
            bTabTabDelete = new ExButton();
            tpTTexts = new TabPage();
            label50 = new Label();
            tbCommentTText = new ExTextBox();
            listTexty = new ListBox();
            label28 = new Label();
            groupBox8 = new ExGroupBox();
            bTextDelete = new ExButton();
            bTextEdit = new ExButton();
            bTextAdd = new ExButton();
            tpFonts = new TabPage();
            bOpenFontDir = new ExButton();
            tbFontDir = new ExTextBox();
            label37 = new Label();
            listFonts = new ListBox();
            label29 = new Label();
            groupBox9 = new ExGroupBox();
            label32 = new Label();
            cbFontIsNumber = new ExCheckBox();
            tbFontFile = new ExTextBox();
            cbFontSpecAssigments = new ExCheckBox();
            cbFontSpecChar = new ExCheckBox();
            cbFontUpper = new ExCheckBox();
            cbFontLower = new ExCheckBox();
            cbFontDia = new ExCheckBox();
            cbFontProportional = new ExCheckBox();
            label36 = new Label();
            cbFontType = new ExComboBox();
            label35 = new Label();
            nudFontWidth = new ExNumericUpDown();
            label34 = new Label();
            nudFontSize = new ExNumericUpDown();
            label33 = new Label();
            tbFontName = new ExTextBox();
            label31 = new Label();
            nudFontID = new ExNumericUpDown();
            label30 = new Label();
            bFontDelete = new ExButton();
            bFontEdit = new ExButton();
            bFontAdd = new ExButton();
            tpStateDgm = new TabPage();
            flpStateDgm = new FlowLayoutPanel();
            lStateDgmInfo = new Label();
            lStateDgmStatus = new Label();
            bStateDgmOpen = new ExButton();
            checkBox1 = new ExCheckBox();
            cbCustomTrainTypDruh = new ExComboBox();
            cbDefTrainTypSkratka = new ExComboBox();
            tabControl.SuspendLayout();
            tpGrafikon.SuspendLayout();
            groupBox15.SuspendLayout();
            groupBox14.SuspendLayout();
            ((ISupportInitialize)nudIDStation).BeginInit();
            groupBox13.SuspendLayout();
            tpStanice.SuspendLayout();
            groupBox12.SuspendLayout();
            ((ISupportInitialize)nudIDStanice).BeginInit();
            tpDopravcovia.SuspendLayout();
            groupBox1.SuspendLayout();
            tpNastupistia.SuspendLayout();
            groupBox2.SuspendLayout();
            tpKolaje.SuspendLayout();
            groupBox3.SuspendLayout();
            tpFyzTab.SuspendLayout();
            groupBox4.SuspendLayout();
            tpLogTab.SuspendLayout();
            groupBox5.SuspendLayout();
            tpKatTab.SuspendLayout();
            groupBox6.SuspendLayout();
            tpTabTab.SuspendLayout();
            groupBox7.SuspendLayout();
            tpTTexts.SuspendLayout();
            groupBox8.SuspendLayout();
            tpFonts.SuspendLayout();
            groupBox9.SuspendLayout();
            ((ISupportInitialize)nudFontWidth).BeginInit();
            ((ISupportInitialize)nudFontSize).BeginInit();
            ((ISupportInitialize)nudFontID).BeginInit();
            tpStateDgm.SuspendLayout();
            flpStateDgm.SuspendLayout();
            SuspendLayout();
            // 
            // bSave
            // 
            resources.ApplyResources(bSave, "bSave");
            bSave.DefaultStyle = true;
            bSave.Name = "bSave";
            bSave.UseVisualStyleBackColor = true;
            bSave.Click += bSave_Click;
            // 
            // label16
            // 
            resources.ApplyResources(label16, "label16");
            label16.Name = "label16";
            // 
            // button3
            // 
            resources.ApplyResources(button3, "button3");
            button3.DefaultStyle = true;
            button3.Name = "button3";
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            resources.ApplyResources(button2, "button2");
            button2.DefaultStyle = true;
            button2.Name = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            resources.ApplyResources(button1, "button1");
            button1.DefaultStyle = true;
            button1.Name = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.BorderColor = Color.DimGray;
            textBox1.BorderThickness = 1;
            textBox1.DefaultStyle = true;
            textBox1.DisabledBackColor = SystemColors.Control;
            textBox1.DisabledBorderColor = SystemColors.InactiveBorder;
            textBox1.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(textBox1, "textBox1");
            textBox1.HighlightColor = SystemColors.Highlight;
            textBox1.HintForeColor = SystemColors.GrayText;
            textBox1.HintText = null;
            textBox1.Name = "textBox1";
            textBox1.UseDarkScrollBar = false;
            // 
            // label17
            // 
            resources.ApplyResources(label17, "label17");
            label17.Name = "label17";
            // 
            // textBox2
            // 
            textBox2.BorderColor = Color.DimGray;
            textBox2.BorderThickness = 1;
            textBox2.DefaultStyle = true;
            textBox2.DisabledBackColor = SystemColors.Control;
            textBox2.DisabledBorderColor = SystemColors.InactiveBorder;
            textBox2.DisabledForeColor = SystemColors.GrayText;
            textBox2.HighlightColor = SystemColors.Highlight;
            textBox2.HintForeColor = SystemColors.GrayText;
            textBox2.HintText = null;
            resources.ApplyResources(textBox2, "textBox2");
            textBox2.Name = "textBox2";
            textBox2.UseDarkScrollBar = false;
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // groupBox11
            // 
            groupBox11.BorderColor = Color.LightGray;
            groupBox11.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox11.BorderThickness = 1;
            groupBox11.DefaultStyle = true;
            groupBox11.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox11, "groupBox11");
            groupBox11.Name = "groupBox11";
            groupBox11.TabStop = false;
            // 
            // label42
            // 
            resources.ApplyResources(label42, "label42");
            label42.Name = "label42";
            // 
            // tbCustomTrainTypText
            // 
            tbCustomTrainTypText.BorderColor = Color.DimGray;
            tbCustomTrainTypText.BorderThickness = 1;
            tbCustomTrainTypText.DefaultStyle = true;
            tbCustomTrainTypText.DisabledBackColor = SystemColors.Control;
            tbCustomTrainTypText.DisabledBorderColor = SystemColors.InactiveBorder;
            tbCustomTrainTypText.DisabledForeColor = SystemColors.GrayText;
            tbCustomTrainTypText.HighlightColor = SystemColors.Highlight;
            tbCustomTrainTypText.HintForeColor = SystemColors.GrayText;
            tbCustomTrainTypText.HintText = null;
            resources.ApplyResources(tbCustomTrainTypText, "tbCustomTrainTypText");
            tbCustomTrainTypText.Name = "tbCustomTrainTypText";
            tbCustomTrainTypText.UseDarkScrollBar = false;
            // 
            // tbCustomTrainTypSkratka
            // 
            tbCustomTrainTypSkratka.BorderColor = Color.DimGray;
            tbCustomTrainTypSkratka.BorderThickness = 1;
            tbCustomTrainTypSkratka.DefaultStyle = true;
            tbCustomTrainTypSkratka.DisabledBackColor = SystemColors.Control;
            tbCustomTrainTypSkratka.DisabledBorderColor = SystemColors.InactiveBorder;
            tbCustomTrainTypSkratka.DisabledForeColor = SystemColors.GrayText;
            tbCustomTrainTypSkratka.HighlightColor = SystemColors.Highlight;
            tbCustomTrainTypSkratka.HintForeColor = SystemColors.GrayText;
            tbCustomTrainTypSkratka.HintText = null;
            resources.ApplyResources(tbCustomTrainTypSkratka, "tbCustomTrainTypSkratka");
            tbCustomTrainTypSkratka.Name = "tbCustomTrainTypSkratka";
            tbCustomTrainTypSkratka.UseDarkScrollBar = false;
            // 
            // label41
            // 
            resources.ApplyResources(label41, "label41");
            label41.Name = "label41";
            // 
            // label40
            // 
            resources.ApplyResources(label40, "label40");
            label40.Name = "label40";
            // 
            // bCustomTrainTypDelete
            // 
            bCustomTrainTypDelete.DefaultStyle = true;
            resources.ApplyResources(bCustomTrainTypDelete, "bCustomTrainTypDelete");
            bCustomTrainTypDelete.Name = "bCustomTrainTypDelete";
            // 
            // bCustomTrainTypAdd
            // 
            bCustomTrainTypAdd.DefaultStyle = true;
            resources.ApplyResources(bCustomTrainTypAdd, "bCustomTrainTypAdd");
            bCustomTrainTypAdd.Name = "bCustomTrainTypAdd";
            // 
            // bCustomTrainTypEdit
            // 
            bCustomTrainTypEdit.DefaultStyle = true;
            resources.ApplyResources(bCustomTrainTypEdit, "bCustomTrainTypEdit");
            bCustomTrainTypEdit.Name = "bCustomTrainTypEdit";
            // 
            // listTrainTypes
            // 
            resources.ApplyResources(listTrainTypes, "listTrainTypes");
            listTrainTypes.Name = "listTrainTypes";
            // 
            // label38
            // 
            resources.ApplyResources(label38, "label38");
            label38.Name = "label38";
            // 
            // groupBox10
            // 
            groupBox10.BorderColor = Color.LightGray;
            groupBox10.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox10.BorderThickness = 1;
            groupBox10.DefaultStyle = true;
            groupBox10.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox10, "groupBox10");
            groupBox10.Name = "groupBox10";
            groupBox10.TabStop = false;
            // 
            // bDefTrainTypDelete
            // 
            bDefTrainTypDelete.DefaultStyle = true;
            resources.ApplyResources(bDefTrainTypDelete, "bDefTrainTypDelete");
            bDefTrainTypDelete.Name = "bDefTrainTypDelete";
            // 
            // bDefTrainTypEdit
            // 
            bDefTrainTypEdit.DefaultStyle = true;
            resources.ApplyResources(bDefTrainTypEdit, "bDefTrainTypEdit");
            bDefTrainTypEdit.Name = "bDefTrainTypEdit";
            // 
            // bDefTrainTypAdd
            // 
            bDefTrainTypAdd.DefaultStyle = true;
            resources.ApplyResources(bDefTrainTypAdd, "bDefTrainTypAdd");
            bDefTrainTypAdd.Name = "bDefTrainTypAdd";
            // 
            // label39
            // 
            resources.ApplyResources(label39, "label39");
            label39.Name = "label39";
            // 
            // fdbFontsDir
            // 
            resources.ApplyResources(fdbFontsDir, "fdbFontsDir");
            // 
            // tabControl
            // 
            tabControl.ActiveHeaderBackColor = Color.White;
            tabControl.ActiveHeaderForeColor = Color.Black;
            tabControl.BorderColor = Color.LightGray;
            tabControl.BorderThickness = 1;
            tabControl.Controls.Add(tpGrafikon);
            tabControl.Controls.Add(tpStanice);
            tabControl.Controls.Add(tpDopravcovia);
            tabControl.Controls.Add(tpNastupistia);
            tabControl.Controls.Add(tpKolaje);
            tabControl.Controls.Add(tpFyzTab);
            tabControl.Controls.Add(tpLogTab);
            tabControl.Controls.Add(tpKatTab);
            tabControl.Controls.Add(tpTabTab);
            tabControl.Controls.Add(tpTTexts);
            tabControl.Controls.Add(tpFonts);
            tabControl.Controls.Add(tpStateDgm);
            tabControl.DefaultStyle = true;
            tabControl.HeaderBackColor = SystemColors.Control;
            tabControl.HeaderForeColor = Color.Black;
            tabControl.HighlightBackColor = SystemColors.GradientInactiveCaption;
            tabControl.HighlightForeColor = Color.Black;
            resources.ApplyResources(tabControl, "tabControl");
            tabControl.Multiline = true;
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.SizeMode = TabSizeMode.FillToRight;
            // 
            // tpGrafikon
            // 
            tpGrafikon.BackColor = Color.Transparent;
            tpGrafikon.Controls.Add(groupBox15);
            tpGrafikon.Controls.Add(groupBox14);
            tpGrafikon.Controls.Add(groupBox13);
            resources.ApplyResources(tpGrafikon, "tpGrafikon");
            tpGrafikon.Name = "tpGrafikon";
            // 
            // groupBox15
            // 
            groupBox15.BorderColor = Color.LightGray;
            groupBox15.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox15.BorderThickness = 1;
            groupBox15.Controls.Add(bDirChange);
            groupBox15.Controls.Add(tbDir);
            groupBox15.Controls.Add(tbDirName);
            groupBox15.Controls.Add(label9);
            groupBox15.Controls.Add(label4);
            groupBox15.Controls.Add(bOpenDir);
            groupBox15.DefaultStyle = true;
            groupBox15.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox15, "groupBox15");
            groupBox15.Name = "groupBox15";
            groupBox15.TabStop = false;
            // 
            // bDirChange
            // 
            resources.ApplyResources(bDirChange, "bDirChange");
            bDirChange.DefaultStyle = true;
            bDirChange.Name = "bDirChange";
            bDirChange.UseVisualStyleBackColor = true;
            bDirChange.Click += bDirChange_Click;
            // 
            // tbDir
            // 
            tbDir.BorderColor = Color.DimGray;
            tbDir.BorderStyle = BorderStyle.FixedSingle;
            tbDir.BorderThickness = 1;
            tbDir.DefaultStyle = true;
            tbDir.DisabledBackColor = SystemColors.Control;
            tbDir.DisabledBorderColor = SystemColors.InactiveBorder;
            tbDir.DisabledForeColor = SystemColors.GrayText;
            tbDir.HighlightColor = SystemColors.Highlight;
            tbDir.HintForeColor = SystemColors.GrayText;
            tbDir.HintText = null;
            resources.ApplyResources(tbDir, "tbDir");
            tbDir.Name = "tbDir";
            tbDir.ReadOnly = true;
            tbDir.UseDarkScrollBar = false;
            // 
            // tbDirName
            // 
            tbDirName.BorderColor = Color.DimGray;
            tbDirName.BorderThickness = 1;
            tbDirName.DefaultStyle = true;
            tbDirName.DisabledBackColor = SystemColors.Control;
            tbDirName.DisabledBorderColor = SystemColors.InactiveBorder;
            tbDirName.DisabledForeColor = SystemColors.GrayText;
            tbDirName.HighlightColor = SystemColors.Highlight;
            tbDirName.HintForeColor = SystemColors.GrayText;
            tbDirName.HintText = null;
            resources.ApplyResources(tbDirName, "tbDirName");
            tbDirName.Name = "tbDirName";
            tbDirName.UseDarkScrollBar = false;
            // 
            // label9
            // 
            resources.ApplyResources(label9, "label9");
            label9.Name = "label9";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // bOpenDir
            // 
            resources.ApplyResources(bOpenDir, "bOpenDir");
            bOpenDir.DefaultStyle = true;
            bOpenDir.Name = "bOpenDir";
            bOpenDir.UseVisualStyleBackColor = true;
            bOpenDir.Click += bOpenDir_Click;
            // 
            // groupBox14
            // 
            groupBox14.BorderColor = Color.LightGray;
            groupBox14.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox14.BorderThickness = 1;
            groupBox14.Controls.Add(dynamicLineSeparator1);
            groupBox14.Controls.Add(tbGVDStationName);
            groupBox14.Controls.Add(cbCustomStation);
            groupBox14.Controls.Add(label47);
            groupBox14.Controls.Add(label48);
            groupBox14.Controls.Add(nudIDStation);
            groupBox14.Controls.Add(label49);
            groupBox14.Controls.Add(cbStationName);
            groupBox14.DefaultStyle = true;
            groupBox14.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox14, "groupBox14");
            groupBox14.Name = "groupBox14";
            groupBox14.TabStop = false;
            // 
            // dynamicLineSeparator1
            // 
            dynamicLineSeparator1.LineColor = Color.LightGray;
            dynamicLineSeparator1.LineOrientation = LineOrientation.Vertical;
            dynamicLineSeparator1.LineStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            dynamicLineSeparator1.LineThickness = 1;
            resources.ApplyResources(dynamicLineSeparator1, "dynamicLineSeparator1");
            dynamicLineSeparator1.Name = "dynamicLineSeparator1";
            // 
            // tbGVDStationName
            // 
            tbGVDStationName.BorderColor = Color.DimGray;
            tbGVDStationName.BorderThickness = 1;
            tbGVDStationName.DefaultStyle = true;
            tbGVDStationName.DisabledBackColor = SystemColors.Control;
            tbGVDStationName.DisabledBorderColor = SystemColors.InactiveBorder;
            tbGVDStationName.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(tbGVDStationName, "tbGVDStationName");
            tbGVDStationName.HighlightColor = SystemColors.Highlight;
            tbGVDStationName.HintForeColor = SystemColors.GrayText;
            tbGVDStationName.HintText = null;
            tbGVDStationName.Name = "tbGVDStationName";
            tbGVDStationName.UseDarkScrollBar = false;
            tbGVDStationName.TextChanged += tbGVDStationName_TextChanged;
            // 
            // cbCustomStation
            // 
            resources.ApplyResources(cbCustomStation, "cbCustomStation");
            cbCustomStation.BorderColor = Color.Black;
            cbCustomStation.BoxBackColor = Color.White;
            cbCustomStation.DefaultStyle = true;
            cbCustomStation.DisabledForeColor = Color.DimGray;
            cbCustomStation.HighlightColor = SystemColors.Highlight;
            cbCustomStation.MarkColor = Color.Black;
            cbCustomStation.Name = "cbCustomStation";
            cbCustomStation.UseVisualStyleBackColor = true;
            cbCustomStation.CheckedChanged += cbCustomStation_CheckedChanged;
            // 
            // label47
            // 
            resources.ApplyResources(label47, "label47");
            label47.Name = "label47";
            // 
            // label48
            // 
            resources.ApplyResources(label48, "label48");
            label48.Name = "label48";
            // 
            // nudIDStation
            // 
            nudIDStation.ArrowsColor = Color.Black;
            nudIDStation.BorderColor = Color.Gainsboro;
            nudIDStation.DefaultStyle = true;
            resources.ApplyResources(nudIDStation, "nudIDStation");
            nudIDStation.HighlightColor = SystemColors.Highlight;
            nudIDStation.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            nudIDStation.Minimum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nudIDStation.Name = "nudIDStation";
            nudIDStation.SelectedButtonColor = Color.Empty;
            nudIDStation.Value = new decimal(new int[] { 1000000, 0, 0, 0 });
            // 
            // label49
            // 
            resources.ApplyResources(label49, "label49");
            label49.Name = "label49";
            // 
            // cbStationName
            // 
            cbStationName.DefaultStyle = true;
            cbStationName.DisplayMember = "Name";
            cbStationName.DropDownBackColor = Color.White;
            cbStationName.DropDownSelectedRowBackColor = Color.Empty;
            cbStationName.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStationName.FormattingEnabled = true;
            resources.ApplyResources(cbStationName, "cbStationName");
            cbStationName.Name = "cbStationName";
            exComboBoxStyle1.ArrowColor = null;
            exComboBoxStyle1.BackColor = null;
            exComboBoxStyle1.BorderColor = null;
            exComboBoxStyle1.ButtonBackColor = null;
            exComboBoxStyle1.ButtonBorderColor = null;
            exComboBoxStyle1.ButtonRenderFirst = null;
            exComboBoxStyle1.ForeColor = null;
            cbStationName.StyleDisabled = exComboBoxStyle1;
            exComboBoxStyle2.ArrowColor = null;
            exComboBoxStyle2.BackColor = null;
            exComboBoxStyle2.BorderColor = null;
            exComboBoxStyle2.ButtonBackColor = null;
            exComboBoxStyle2.ButtonBorderColor = null;
            exComboBoxStyle2.ButtonRenderFirst = null;
            exComboBoxStyle2.ForeColor = null;
            cbStationName.StyleHighlight = exComboBoxStyle2;
            exComboBoxStyle3.ArrowColor = null;
            exComboBoxStyle3.BackColor = null;
            exComboBoxStyle3.BorderColor = null;
            exComboBoxStyle3.ButtonBackColor = null;
            exComboBoxStyle3.ButtonBorderColor = null;
            exComboBoxStyle3.ButtonRenderFirst = null;
            exComboBoxStyle3.ForeColor = null;
            cbStationName.StyleNormal = exComboBoxStyle3;
            exComboBoxStyle4.ArrowColor = null;
            exComboBoxStyle4.BackColor = null;
            exComboBoxStyle4.BorderColor = null;
            exComboBoxStyle4.ButtonBackColor = null;
            exComboBoxStyle4.ButtonBorderColor = null;
            exComboBoxStyle4.ButtonRenderFirst = null;
            exComboBoxStyle4.ForeColor = null;
            cbStationName.StyleSelected = exComboBoxStyle4;
            cbStationName.UseDarkScrollBar = false;
            // 
            // groupBox13
            // 
            groupBox13.BorderColor = Color.LightGray;
            groupBox13.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox13.BorderThickness = 1;
            groupBox13.Controls.Add(label2);
            groupBox13.Controls.Add(label3);
            groupBox13.Controls.Add(dtpGVDOd);
            groupBox13.Controls.Add(dtpGVDDo);
            groupBox13.Controls.Add(label13);
            groupBox13.Controls.Add(dtpDataDo);
            groupBox13.Controls.Add(label11);
            groupBox13.Controls.Add(dtpDataOd);
            groupBox13.DefaultStyle = true;
            groupBox13.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox13, "groupBox13");
            groupBox13.Name = "groupBox13";
            groupBox13.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // dtpGVDOd
            // 
            dtpGVDOd.Format = DateTimePickerFormat.Short;
            resources.ApplyResources(dtpGVDOd, "dtpGVDOd");
            dtpGVDOd.Name = "dtpGVDOd";
            // 
            // dtpGVDDo
            // 
            dtpGVDDo.Format = DateTimePickerFormat.Short;
            resources.ApplyResources(dtpGVDDo, "dtpGVDDo");
            dtpGVDDo.Name = "dtpGVDDo";
            // 
            // label13
            // 
            resources.ApplyResources(label13, "label13");
            label13.Name = "label13";
            // 
            // dtpDataDo
            // 
            dtpDataDo.Format = DateTimePickerFormat.Short;
            resources.ApplyResources(dtpDataDo, "dtpDataDo");
            dtpDataDo.Name = "dtpDataDo";
            // 
            // label11
            // 
            resources.ApplyResources(label11, "label11");
            label11.Name = "label11";
            // 
            // dtpDataOd
            // 
            dtpDataOd.Format = DateTimePickerFormat.Short;
            resources.ApplyResources(dtpDataOd, "dtpDataOd");
            dtpDataOd.Name = "dtpDataOd";
            dtpDataOd.ValueChanged += dtpDataOd_ValueChanged;
            // 
            // tpStanice
            // 
            tpStanice.BackColor = Color.Transparent;
            tpStanice.Controls.Add(listCustomStations);
            tpStanice.Controls.Add(label43);
            tpStanice.Controls.Add(groupBox12);
            resources.ApplyResources(tpStanice, "tpStanice");
            tpStanice.Name = "tpStanice";
            // 
            // listCustomStations
            // 
            listCustomStations.FormattingEnabled = true;
            resources.ApplyResources(listCustomStations, "listCustomStations");
            listCustomStations.Name = "listCustomStations";
            listCustomStations.SelectedIndexChanged += listCustomStations_SelectedIndexChanged;
            // 
            // label43
            // 
            resources.ApplyResources(label43, "label43");
            label43.Name = "label43";
            // 
            // groupBox12
            // 
            groupBox12.BorderColor = Color.LightGray;
            groupBox12.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox12.BorderThickness = 1;
            groupBox12.Controls.Add(label45);
            groupBox12.Controls.Add(nudIDStanice);
            groupBox12.Controls.Add(bCStationDelete);
            groupBox12.Controls.Add(bCStationEdit);
            groupBox12.Controls.Add(bCStationAdd);
            groupBox12.Controls.Add(tbStationName);
            groupBox12.Controls.Add(label44);
            groupBox12.DefaultStyle = true;
            groupBox12.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox12, "groupBox12");
            groupBox12.Name = "groupBox12";
            groupBox12.TabStop = false;
            // 
            // label45
            // 
            resources.ApplyResources(label45, "label45");
            label45.Name = "label45";
            // 
            // nudIDStanice
            // 
            nudIDStanice.ArrowsColor = Color.Black;
            nudIDStanice.BorderColor = Color.Gainsboro;
            nudIDStanice.DefaultStyle = true;
            nudIDStanice.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(nudIDStanice, "nudIDStanice");
            nudIDStanice.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            nudIDStanice.Minimum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nudIDStanice.Name = "nudIDStanice";
            nudIDStanice.SelectedButtonColor = Color.Empty;
            nudIDStanice.Value = new decimal(new int[] { 1000000, 0, 0, 0 });
            // 
            // bCStationDelete
            // 
            resources.ApplyResources(bCStationDelete, "bCStationDelete");
            bCStationDelete.DefaultStyle = true;
            bCStationDelete.Name = "bCStationDelete";
            bCStationDelete.UseVisualStyleBackColor = true;
            bCStationDelete.Click += bCStationDelete_Click;
            // 
            // bCStationEdit
            // 
            resources.ApplyResources(bCStationEdit, "bCStationEdit");
            bCStationEdit.DefaultStyle = true;
            bCStationEdit.Name = "bCStationEdit";
            bCStationEdit.UseVisualStyleBackColor = true;
            bCStationEdit.Click += bCStationEdit_Click;
            // 
            // bCStationAdd
            // 
            resources.ApplyResources(bCStationAdd, "bCStationAdd");
            bCStationAdd.DefaultStyle = true;
            bCStationAdd.Name = "bCStationAdd";
            bCStationAdd.UseVisualStyleBackColor = true;
            bCStationAdd.Click += bCStationAdd_Click;
            // 
            // tbStationName
            // 
            tbStationName.BorderColor = Color.DimGray;
            tbStationName.BorderThickness = 1;
            tbStationName.DefaultStyle = true;
            tbStationName.DisabledBackColor = SystemColors.Control;
            tbStationName.DisabledBorderColor = SystemColors.InactiveBorder;
            tbStationName.DisabledForeColor = SystemColors.GrayText;
            tbStationName.HighlightColor = SystemColors.Highlight;
            tbStationName.HintForeColor = SystemColors.GrayText;
            tbStationName.HintText = null;
            resources.ApplyResources(tbStationName, "tbStationName");
            tbStationName.Name = "tbStationName";
            tbStationName.UseDarkScrollBar = false;
            // 
            // label44
            // 
            resources.ApplyResources(label44, "label44");
            label44.Name = "label44";
            // 
            // tpDopravcovia
            // 
            tpDopravcovia.BackColor = Color.Transparent;
            tpDopravcovia.Controls.Add(listDopravcovia);
            tpDopravcovia.Controls.Add(label7);
            tpDopravcovia.Controls.Add(groupBox1);
            resources.ApplyResources(tpDopravcovia, "tpDopravcovia");
            tpDopravcovia.Name = "tpDopravcovia";
            // 
            // listDopravcovia
            // 
            listDopravcovia.FormattingEnabled = true;
            resources.ApplyResources(listDopravcovia, "listDopravcovia");
            listDopravcovia.Name = "listDopravcovia";
            listDopravcovia.SelectedIndexChanged += listDopravcovia_SelectedIndexChanged;
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // groupBox1
            // 
            groupBox1.BorderColor = Color.LightGray;
            groupBox1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox1.BorderThickness = 1;
            groupBox1.Controls.Add(bDopravcaDelete);
            groupBox1.Controls.Add(bDopravcaEdit);
            groupBox1.Controls.Add(bDopravcaAdd);
            groupBox1.Controls.Add(tbDopravca);
            groupBox1.Controls.Add(label1);
            groupBox1.DefaultStyle = true;
            groupBox1.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // bDopravcaDelete
            // 
            resources.ApplyResources(bDopravcaDelete, "bDopravcaDelete");
            bDopravcaDelete.DefaultStyle = true;
            bDopravcaDelete.Name = "bDopravcaDelete";
            bDopravcaDelete.UseVisualStyleBackColor = true;
            bDopravcaDelete.Click += bDopravcaDelete_Click;
            // 
            // bDopravcaEdit
            // 
            resources.ApplyResources(bDopravcaEdit, "bDopravcaEdit");
            bDopravcaEdit.DefaultStyle = true;
            bDopravcaEdit.Name = "bDopravcaEdit";
            bDopravcaEdit.UseVisualStyleBackColor = true;
            bDopravcaEdit.Click += bDopravcaEdit_Click;
            // 
            // bDopravcaAdd
            // 
            resources.ApplyResources(bDopravcaAdd, "bDopravcaAdd");
            bDopravcaAdd.DefaultStyle = true;
            bDopravcaAdd.Name = "bDopravcaAdd";
            bDopravcaAdd.UseVisualStyleBackColor = true;
            bDopravcaAdd.Click += bDopravcaAdd_Click;
            // 
            // tbDopravca
            // 
            tbDopravca.BorderColor = Color.DimGray;
            tbDopravca.BorderThickness = 1;
            tbDopravca.DefaultStyle = true;
            tbDopravca.DisabledBackColor = SystemColors.Control;
            tbDopravca.DisabledBorderColor = SystemColors.InactiveBorder;
            tbDopravca.DisabledForeColor = SystemColors.GrayText;
            tbDopravca.HighlightColor = SystemColors.Highlight;
            tbDopravca.HintForeColor = SystemColors.GrayText;
            tbDopravca.HintText = null;
            resources.ApplyResources(tbDopravca, "tbDopravca");
            tbDopravca.Name = "tbDopravca";
            tbDopravca.UseDarkScrollBar = false;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // tpNastupistia
            // 
            tpNastupistia.BackColor = Color.Transparent;
            tpNastupistia.Controls.Add(listNastupistia);
            tpNastupistia.Controls.Add(label8);
            tpNastupistia.Controls.Add(groupBox2);
            resources.ApplyResources(tpNastupistia, "tpNastupistia");
            tpNastupistia.Name = "tpNastupistia";
            // 
            // listNastupistia
            // 
            listNastupistia.FormattingEnabled = true;
            resources.ApplyResources(listNastupistia, "listNastupistia");
            listNastupistia.Name = "listNastupistia";
            listNastupistia.SelectedIndexChanged += listNastupistia_SelectedIndexChanged;
            // 
            // label8
            // 
            resources.ApplyResources(label8, "label8");
            label8.Name = "label8";
            // 
            // groupBox2
            // 
            groupBox2.BorderColor = Color.LightGray;
            groupBox2.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox2.BorderThickness = 1;
            groupBox2.Controls.Add(label26);
            groupBox2.Controls.Add(tbNastSound);
            groupBox2.Controls.Add(tbNastFullName);
            groupBox2.Controls.Add(label12);
            groupBox2.Controls.Add(bNastDelete);
            groupBox2.Controls.Add(bNastEdit);
            groupBox2.Controls.Add(bNastAdd);
            groupBox2.Controls.Add(tbNastOznacenie);
            groupBox2.Controls.Add(label10);
            groupBox2.DefaultStyle = true;
            groupBox2.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox2, "groupBox2");
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            // 
            // label26
            // 
            resources.ApplyResources(label26, "label26");
            label26.Name = "label26";
            // 
            // tbNastSound
            // 
            tbNastSound.BorderColor = Color.DimGray;
            tbNastSound.BorderThickness = 1;
            tbNastSound.DefaultStyle = true;
            tbNastSound.DisabledBackColor = SystemColors.Control;
            tbNastSound.DisabledBorderColor = SystemColors.InactiveBorder;
            tbNastSound.DisabledForeColor = SystemColors.GrayText;
            tbNastSound.HighlightColor = SystemColors.Highlight;
            tbNastSound.HintForeColor = SystemColors.GrayText;
            tbNastSound.HintText = null;
            resources.ApplyResources(tbNastSound, "tbNastSound");
            tbNastSound.Name = "tbNastSound";
            tbNastSound.UseDarkScrollBar = false;
            // 
            // tbNastFullName
            // 
            tbNastFullName.BorderColor = Color.DimGray;
            tbNastFullName.BorderThickness = 1;
            tbNastFullName.DefaultStyle = true;
            tbNastFullName.DisabledBackColor = SystemColors.Control;
            tbNastFullName.DisabledBorderColor = SystemColors.InactiveBorder;
            tbNastFullName.DisabledForeColor = SystemColors.GrayText;
            tbNastFullName.HighlightColor = SystemColors.Highlight;
            tbNastFullName.HintForeColor = SystemColors.GrayText;
            tbNastFullName.HintText = null;
            resources.ApplyResources(tbNastFullName, "tbNastFullName");
            tbNastFullName.Name = "tbNastFullName";
            tbNastFullName.UseDarkScrollBar = false;
            // 
            // label12
            // 
            resources.ApplyResources(label12, "label12");
            label12.Name = "label12";
            // 
            // bNastDelete
            // 
            resources.ApplyResources(bNastDelete, "bNastDelete");
            bNastDelete.DefaultStyle = true;
            bNastDelete.Name = "bNastDelete";
            bNastDelete.UseVisualStyleBackColor = true;
            bNastDelete.Click += bNastDelete_Click;
            // 
            // bNastEdit
            // 
            resources.ApplyResources(bNastEdit, "bNastEdit");
            bNastEdit.DefaultStyle = true;
            bNastEdit.Name = "bNastEdit";
            bNastEdit.UseVisualStyleBackColor = true;
            bNastEdit.Click += bNastEdit_Click;
            // 
            // bNastAdd
            // 
            resources.ApplyResources(bNastAdd, "bNastAdd");
            bNastAdd.DefaultStyle = true;
            bNastAdd.Name = "bNastAdd";
            bNastAdd.UseVisualStyleBackColor = true;
            bNastAdd.Click += bNastAdd_Click;
            // 
            // tbNastOznacenie
            // 
            tbNastOznacenie.BorderColor = Color.DimGray;
            tbNastOznacenie.BorderThickness = 1;
            tbNastOznacenie.DefaultStyle = true;
            tbNastOznacenie.DisabledBackColor = SystemColors.Control;
            tbNastOznacenie.DisabledBorderColor = SystemColors.InactiveBorder;
            tbNastOznacenie.DisabledForeColor = SystemColors.GrayText;
            tbNastOznacenie.HighlightColor = SystemColors.Highlight;
            tbNastOznacenie.HintForeColor = SystemColors.GrayText;
            tbNastOznacenie.HintText = null;
            resources.ApplyResources(tbNastOznacenie, "tbNastOznacenie");
            tbNastOznacenie.Name = "tbNastOznacenie";
            tbNastOznacenie.UseDarkScrollBar = false;
            tbNastOznacenie.TextChanged += tbNastOznacenie_TextChanged;
            // 
            // label10
            // 
            resources.ApplyResources(label10, "label10");
            label10.Name = "label10";
            // 
            // tpKolaje
            // 
            tpKolaje.BackColor = Color.Transparent;
            tpKolaje.Controls.Add(listKolaje);
            tpKolaje.Controls.Add(label15);
            tpKolaje.Controls.Add(groupBox3);
            resources.ApplyResources(tpKolaje, "tpKolaje");
            tpKolaje.Name = "tpKolaje";
            // 
            // listKolaje
            // 
            listKolaje.FormattingEnabled = true;
            resources.ApplyResources(listKolaje, "listKolaje");
            listKolaje.Name = "listKolaje";
            listKolaje.SelectedIndexChanged += listKolaje_SelectedIndexChanged;
            // 
            // label15
            // 
            resources.ApplyResources(label15, "label15");
            label15.Name = "label15";
            // 
            // groupBox3
            // 
            groupBox3.BorderColor = Color.LightGray;
            groupBox3.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox3.BorderThickness = 1;
            groupBox3.Controls.Add(tbKolajAlt);
            groupBox3.Controls.Add(label53);
            groupBox3.Controls.Add(tbNastupisteKolaj);
            groupBox3.Controls.Add(label52);
            groupBox3.Controls.Add(tbKolajSound);
            groupBox3.Controls.Add(label27);
            groupBox3.Controls.Add(cbNastupistia);
            groupBox3.Controls.Add(label21);
            groupBox3.Controls.Add(label20);
            groupBox3.Controls.Add(clbKolajTables);
            groupBox3.Controls.Add(tbKolajFullName);
            groupBox3.Controls.Add(label18);
            groupBox3.Controls.Add(bKolajDelete);
            groupBox3.Controls.Add(bKolajEdit);
            groupBox3.Controls.Add(bKolajAdd);
            groupBox3.Controls.Add(tbKolajOznacenie);
            groupBox3.Controls.Add(label19);
            groupBox3.DefaultStyle = true;
            groupBox3.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox3, "groupBox3");
            groupBox3.Name = "groupBox3";
            groupBox3.TabStop = false;
            // 
            // tbKolajAlt
            // 
            tbKolajAlt.BorderColor = Color.DimGray;
            tbKolajAlt.BorderThickness = 1;
            tbKolajAlt.DefaultStyle = true;
            tbKolajAlt.DisabledBackColor = SystemColors.Control;
            tbKolajAlt.DisabledBorderColor = SystemColors.InactiveBorder;
            tbKolajAlt.DisabledForeColor = SystemColors.GrayText;
            tbKolajAlt.HighlightColor = SystemColors.Highlight;
            tbKolajAlt.HintForeColor = SystemColors.GrayText;
            tbKolajAlt.HintText = null;
            resources.ApplyResources(tbKolajAlt, "tbKolajAlt");
            tbKolajAlt.Name = "tbKolajAlt";
            tbKolajAlt.UseDarkScrollBar = false;
            // 
            // label53
            // 
            resources.ApplyResources(label53, "label53");
            label53.Name = "label53";
            // 
            // tbNastupisteKolaj
            // 
            tbNastupisteKolaj.BorderColor = Color.DimGray;
            tbNastupisteKolaj.BorderThickness = 1;
            tbNastupisteKolaj.DefaultStyle = true;
            tbNastupisteKolaj.DisabledBackColor = SystemColors.Control;
            tbNastupisteKolaj.DisabledBorderColor = SystemColors.InactiveBorder;
            tbNastupisteKolaj.DisabledForeColor = SystemColors.GrayText;
            tbNastupisteKolaj.HighlightColor = SystemColors.Highlight;
            tbNastupisteKolaj.HintForeColor = SystemColors.GrayText;
            tbNastupisteKolaj.HintText = null;
            resources.ApplyResources(tbNastupisteKolaj, "tbNastupisteKolaj");
            tbNastupisteKolaj.Name = "tbNastupisteKolaj";
            tbNastupisteKolaj.UseDarkScrollBar = false;
            // 
            // label52
            // 
            resources.ApplyResources(label52, "label52");
            label52.Name = "label52";
            // 
            // tbKolajSound
            // 
            tbKolajSound.BorderColor = Color.DimGray;
            tbKolajSound.BorderThickness = 1;
            tbKolajSound.DefaultStyle = true;
            tbKolajSound.DisabledBackColor = SystemColors.Control;
            tbKolajSound.DisabledBorderColor = SystemColors.InactiveBorder;
            tbKolajSound.DisabledForeColor = SystemColors.GrayText;
            tbKolajSound.HighlightColor = SystemColors.Highlight;
            tbKolajSound.HintForeColor = SystemColors.GrayText;
            tbKolajSound.HintText = null;
            resources.ApplyResources(tbKolajSound, "tbKolajSound");
            tbKolajSound.Name = "tbKolajSound";
            tbKolajSound.UseDarkScrollBar = false;
            // 
            // label27
            // 
            resources.ApplyResources(label27, "label27");
            label27.Name = "label27";
            // 
            // cbNastupistia
            // 
            cbNastupistia.DefaultStyle = true;
            cbNastupistia.DropDownBackColor = Color.White;
            cbNastupistia.DropDownSelectedRowBackColor = Color.Empty;
            cbNastupistia.DropDownStyle = ComboBoxStyle.DropDownList;
            cbNastupistia.FormattingEnabled = true;
            resources.ApplyResources(cbNastupistia, "cbNastupistia");
            cbNastupistia.Name = "cbNastupistia";
            exComboBoxStyle5.ArrowColor = null;
            exComboBoxStyle5.BackColor = null;
            exComboBoxStyle5.BorderColor = null;
            exComboBoxStyle5.ButtonBackColor = null;
            exComboBoxStyle5.ButtonBorderColor = null;
            exComboBoxStyle5.ButtonRenderFirst = null;
            exComboBoxStyle5.ForeColor = null;
            cbNastupistia.StyleDisabled = exComboBoxStyle5;
            exComboBoxStyle6.ArrowColor = null;
            exComboBoxStyle6.BackColor = null;
            exComboBoxStyle6.BorderColor = null;
            exComboBoxStyle6.ButtonBackColor = null;
            exComboBoxStyle6.ButtonBorderColor = null;
            exComboBoxStyle6.ButtonRenderFirst = null;
            exComboBoxStyle6.ForeColor = null;
            cbNastupistia.StyleHighlight = exComboBoxStyle6;
            exComboBoxStyle7.ArrowColor = null;
            exComboBoxStyle7.BackColor = null;
            exComboBoxStyle7.BorderColor = null;
            exComboBoxStyle7.ButtonBackColor = null;
            exComboBoxStyle7.ButtonBorderColor = null;
            exComboBoxStyle7.ButtonRenderFirst = null;
            exComboBoxStyle7.ForeColor = null;
            cbNastupistia.StyleNormal = exComboBoxStyle7;
            exComboBoxStyle8.ArrowColor = null;
            exComboBoxStyle8.BackColor = null;
            exComboBoxStyle8.BorderColor = null;
            exComboBoxStyle8.ButtonBackColor = null;
            exComboBoxStyle8.ButtonBorderColor = null;
            exComboBoxStyle8.ButtonRenderFirst = null;
            exComboBoxStyle8.ForeColor = null;
            cbNastupistia.StyleSelected = exComboBoxStyle8;
            cbNastupistia.UseDarkScrollBar = false;
            // 
            // label21
            // 
            resources.ApplyResources(label21, "label21");
            label21.Name = "label21";
            // 
            // label20
            // 
            resources.ApplyResources(label20, "label20");
            label20.Name = "label20";
            // 
            // clbKolajTables
            // 
            clbKolajTables.BorderColor = Color.Black;
            clbKolajTables.DefaultStyle = true;
            clbKolajTables.DisabledForeColor = Color.DimGray;
            clbKolajTables.DisplayMember = "This";
            clbKolajTables.FocusedBackColor = Color.Gray;
            clbKolajTables.FormattingEnabled = true;
            clbKolajTables.HighlightColor = Color.FromArgb(0, 120, 215);
            resources.ApplyResources(clbKolajTables, "clbKolajTables");
            clbKolajTables.MarkColor = Color.Black;
            clbKolajTables.Name = "clbKolajTables";
            clbKolajTables.SquareBackColor = Color.White;
            clbKolajTables.ValueMember = "This";
            // 
            // tbKolajFullName
            // 
            tbKolajFullName.BorderColor = Color.DimGray;
            tbKolajFullName.BorderThickness = 1;
            tbKolajFullName.DefaultStyle = true;
            tbKolajFullName.DisabledBackColor = SystemColors.Control;
            tbKolajFullName.DisabledBorderColor = SystemColors.InactiveBorder;
            tbKolajFullName.DisabledForeColor = SystemColors.GrayText;
            tbKolajFullName.HighlightColor = SystemColors.Highlight;
            tbKolajFullName.HintForeColor = SystemColors.GrayText;
            tbKolajFullName.HintText = null;
            resources.ApplyResources(tbKolajFullName, "tbKolajFullName");
            tbKolajFullName.Name = "tbKolajFullName";
            tbKolajFullName.UseDarkScrollBar = false;
            // 
            // label18
            // 
            resources.ApplyResources(label18, "label18");
            label18.Name = "label18";
            // 
            // bKolajDelete
            // 
            resources.ApplyResources(bKolajDelete, "bKolajDelete");
            bKolajDelete.DefaultStyle = true;
            bKolajDelete.Name = "bKolajDelete";
            bKolajDelete.UseVisualStyleBackColor = true;
            bKolajDelete.Click += bKolajDelete_Click;
            // 
            // bKolajEdit
            // 
            resources.ApplyResources(bKolajEdit, "bKolajEdit");
            bKolajEdit.DefaultStyle = true;
            bKolajEdit.Name = "bKolajEdit";
            bKolajEdit.UseVisualStyleBackColor = true;
            bKolajEdit.Click += bKolajEdit_Click;
            // 
            // bKolajAdd
            // 
            resources.ApplyResources(bKolajAdd, "bKolajAdd");
            bKolajAdd.DefaultStyle = true;
            bKolajAdd.Name = "bKolajAdd";
            bKolajAdd.UseVisualStyleBackColor = true;
            bKolajAdd.Click += bKolajAdd_Click;
            // 
            // tbKolajOznacenie
            // 
            tbKolajOznacenie.BorderColor = Color.DimGray;
            tbKolajOznacenie.BorderThickness = 1;
            tbKolajOznacenie.DefaultStyle = true;
            tbKolajOznacenie.DisabledBackColor = SystemColors.Control;
            tbKolajOznacenie.DisabledBorderColor = SystemColors.InactiveBorder;
            tbKolajOznacenie.DisabledForeColor = SystemColors.GrayText;
            tbKolajOznacenie.HighlightColor = SystemColors.Highlight;
            tbKolajOznacenie.HintForeColor = SystemColors.GrayText;
            tbKolajOznacenie.HintText = null;
            resources.ApplyResources(tbKolajOznacenie, "tbKolajOznacenie");
            tbKolajOznacenie.Name = "tbKolajOznacenie";
            tbKolajOznacenie.UseDarkScrollBar = false;
            tbKolajOznacenie.TextChanged += tbKolajOznacenie_TextChanged;
            // 
            // label19
            // 
            resources.ApplyResources(label19, "label19");
            label19.Name = "label19";
            // 
            // tpFyzTab
            // 
            tpFyzTab.BackColor = Color.Transparent;
            tpFyzTab.Controls.Add(label14);
            tpFyzTab.Controls.Add(tbCommentFyz);
            tpFyzTab.Controls.Add(listFyzTabule);
            tpFyzTab.Controls.Add(label22);
            tpFyzTab.Controls.Add(groupBox4);
            resources.ApplyResources(tpFyzTab, "tpFyzTab");
            tpFyzTab.Name = "tpFyzTab";
            // 
            // label14
            // 
            resources.ApplyResources(label14, "label14");
            label14.Name = "label14";
            // 
            // tbCommentFyz
            // 
            tbCommentFyz.BorderColor = Color.DimGray;
            tbCommentFyz.BorderThickness = 1;
            tbCommentFyz.DefaultStyle = true;
            tbCommentFyz.DisabledBackColor = SystemColors.Control;
            tbCommentFyz.DisabledBorderColor = SystemColors.InactiveBorder;
            tbCommentFyz.DisabledForeColor = SystemColors.GrayText;
            tbCommentFyz.HighlightColor = SystemColors.Highlight;
            tbCommentFyz.HintForeColor = SystemColors.GrayText;
            tbCommentFyz.HintText = null;
            resources.ApplyResources(tbCommentFyz, "tbCommentFyz");
            tbCommentFyz.Name = "tbCommentFyz";
            tbCommentFyz.ReadOnly = true;
            tbCommentFyz.UseDarkScrollBar = false;
            // 
            // listFyzTabule
            // 
            listFyzTabule.FormattingEnabled = true;
            resources.ApplyResources(listFyzTabule, "listFyzTabule");
            listFyzTabule.Name = "listFyzTabule";
            listFyzTabule.SelectedIndexChanged += listFyzTabule_SelectedIndexChanged;
            // 
            // label22
            // 
            resources.ApplyResources(label22, "label22");
            label22.Name = "label22";
            // 
            // groupBox4
            // 
            groupBox4.BorderColor = Color.LightGray;
            groupBox4.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox4.BorderThickness = 1;
            groupBox4.Controls.Add(bFyzTabCopy);
            groupBox4.Controls.Add(bFyzTabDelete);
            groupBox4.Controls.Add(bFyzTabEdit);
            groupBox4.Controls.Add(bFyzTabAdd);
            groupBox4.DefaultStyle = true;
            groupBox4.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox4, "groupBox4");
            groupBox4.Name = "groupBox4";
            groupBox4.TabStop = false;
            // 
            // bFyzTabCopy
            // 
            resources.ApplyResources(bFyzTabCopy, "bFyzTabCopy");
            bFyzTabCopy.DefaultStyle = true;
            bFyzTabCopy.Name = "bFyzTabCopy";
            bFyzTabCopy.UseVisualStyleBackColor = true;
            bFyzTabCopy.Click += bFyzTabCopy_Click;
            // 
            // bFyzTabDelete
            // 
            resources.ApplyResources(bFyzTabDelete, "bFyzTabDelete");
            bFyzTabDelete.DefaultStyle = true;
            bFyzTabDelete.Name = "bFyzTabDelete";
            bFyzTabDelete.UseVisualStyleBackColor = true;
            bFyzTabDelete.Click += bFyzTabDelete_Click;
            // 
            // bFyzTabEdit
            // 
            resources.ApplyResources(bFyzTabEdit, "bFyzTabEdit");
            bFyzTabEdit.DefaultStyle = true;
            bFyzTabEdit.Name = "bFyzTabEdit";
            bFyzTabEdit.UseVisualStyleBackColor = true;
            bFyzTabEdit.Click += bFyzTabEdit_Click;
            // 
            // bFyzTabAdd
            // 
            resources.ApplyResources(bFyzTabAdd, "bFyzTabAdd");
            bFyzTabAdd.DefaultStyle = true;
            bFyzTabAdd.Name = "bFyzTabAdd";
            bFyzTabAdd.UseVisualStyleBackColor = true;
            bFyzTabAdd.Click += bFyzTabAdd_Click;
            // 
            // tpLogTab
            // 
            tpLogTab.BackColor = Color.Transparent;
            tpLogTab.Controls.Add(label46);
            tpLogTab.Controls.Add(tbCommentLog);
            tpLogTab.Controls.Add(listLogTabule);
            tpLogTab.Controls.Add(label23);
            tpLogTab.Controls.Add(groupBox5);
            resources.ApplyResources(tpLogTab, "tpLogTab");
            tpLogTab.Name = "tpLogTab";
            // 
            // label46
            // 
            resources.ApplyResources(label46, "label46");
            label46.Name = "label46";
            // 
            // tbCommentLog
            // 
            tbCommentLog.BorderColor = Color.DimGray;
            tbCommentLog.BorderThickness = 1;
            tbCommentLog.DefaultStyle = true;
            tbCommentLog.DisabledBackColor = SystemColors.Control;
            tbCommentLog.DisabledBorderColor = SystemColors.InactiveBorder;
            tbCommentLog.DisabledForeColor = SystemColors.GrayText;
            tbCommentLog.HighlightColor = SystemColors.Highlight;
            tbCommentLog.HintForeColor = SystemColors.GrayText;
            tbCommentLog.HintText = null;
            resources.ApplyResources(tbCommentLog, "tbCommentLog");
            tbCommentLog.Name = "tbCommentLog";
            tbCommentLog.ReadOnly = true;
            tbCommentLog.UseDarkScrollBar = false;
            // 
            // listLogTabule
            // 
            listLogTabule.FormattingEnabled = true;
            resources.ApplyResources(listLogTabule, "listLogTabule");
            listLogTabule.Name = "listLogTabule";
            listLogTabule.SelectedIndexChanged += listLogTabule_SelectedIndexChanged;
            // 
            // label23
            // 
            resources.ApplyResources(label23, "label23");
            label23.Name = "label23";
            // 
            // groupBox5
            // 
            groupBox5.BorderColor = Color.LightGray;
            groupBox5.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox5.BorderThickness = 1;
            groupBox5.Controls.Add(bLogTabCopy);
            groupBox5.Controls.Add(bLogTabDelete);
            groupBox5.Controls.Add(bLogTabEdit);
            groupBox5.Controls.Add(bLogTabAdd);
            groupBox5.DefaultStyle = true;
            groupBox5.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox5, "groupBox5");
            groupBox5.Name = "groupBox5";
            groupBox5.TabStop = false;
            // 
            // bLogTabCopy
            // 
            resources.ApplyResources(bLogTabCopy, "bLogTabCopy");
            bLogTabCopy.DefaultStyle = true;
            bLogTabCopy.Name = "bLogTabCopy";
            bLogTabCopy.UseVisualStyleBackColor = true;
            bLogTabCopy.Click += bLogTabCopy_Click;
            // 
            // bLogTabDelete
            // 
            resources.ApplyResources(bLogTabDelete, "bLogTabDelete");
            bLogTabDelete.DefaultStyle = true;
            bLogTabDelete.Name = "bLogTabDelete";
            bLogTabDelete.UseVisualStyleBackColor = true;
            bLogTabDelete.Click += bLogTabDelete_Click;
            // 
            // bLogTabEdit
            // 
            resources.ApplyResources(bLogTabEdit, "bLogTabEdit");
            bLogTabEdit.DefaultStyle = true;
            bLogTabEdit.Name = "bLogTabEdit";
            bLogTabEdit.UseVisualStyleBackColor = true;
            bLogTabEdit.Click += bLogTabEdit_Click;
            // 
            // bLogTabAdd
            // 
            resources.ApplyResources(bLogTabAdd, "bLogTabAdd");
            bLogTabAdd.DefaultStyle = true;
            bLogTabAdd.Name = "bLogTabAdd";
            bLogTabAdd.UseVisualStyleBackColor = true;
            bLogTabAdd.Click += bLogTabAdd_Click;
            // 
            // tpKatTab
            // 
            tpKatTab.BackColor = Color.Transparent;
            tpKatTab.Controls.Add(label51);
            tpKatTab.Controls.Add(tbCommentKat);
            tpKatTab.Controls.Add(listKatTabule);
            tpKatTab.Controls.Add(label24);
            tpKatTab.Controls.Add(groupBox6);
            resources.ApplyResources(tpKatTab, "tpKatTab");
            tpKatTab.Name = "tpKatTab";
            // 
            // label51
            // 
            resources.ApplyResources(label51, "label51");
            label51.Name = "label51";
            // 
            // tbCommentKat
            // 
            tbCommentKat.BorderColor = Color.DimGray;
            tbCommentKat.BorderThickness = 1;
            tbCommentKat.DefaultStyle = true;
            tbCommentKat.DisabledBackColor = SystemColors.Control;
            tbCommentKat.DisabledBorderColor = SystemColors.InactiveBorder;
            tbCommentKat.DisabledForeColor = SystemColors.GrayText;
            tbCommentKat.HighlightColor = SystemColors.Highlight;
            tbCommentKat.HintForeColor = SystemColors.GrayText;
            tbCommentKat.HintText = null;
            resources.ApplyResources(tbCommentKat, "tbCommentKat");
            tbCommentKat.Name = "tbCommentKat";
            tbCommentKat.ReadOnly = true;
            tbCommentKat.UseDarkScrollBar = false;
            // 
            // listKatTabule
            // 
            listKatTabule.FormattingEnabled = true;
            resources.ApplyResources(listKatTabule, "listKatTabule");
            listKatTabule.Name = "listKatTabule";
            listKatTabule.SelectedIndexChanged += listKatTabule_SelectedIndexChanged;
            // 
            // label24
            // 
            resources.ApplyResources(label24, "label24");
            label24.Name = "label24";
            // 
            // groupBox6
            // 
            groupBox6.BorderColor = Color.LightGray;
            groupBox6.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox6.BorderThickness = 1;
            groupBox6.Controls.Add(bKatTabCopy);
            groupBox6.Controls.Add(bKatTabDelete);
            groupBox6.Controls.Add(bKatTabEdit);
            groupBox6.Controls.Add(bKatTabAdd);
            groupBox6.DefaultStyle = true;
            groupBox6.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox6, "groupBox6");
            groupBox6.Name = "groupBox6";
            groupBox6.TabStop = false;
            // 
            // bKatTabCopy
            // 
            resources.ApplyResources(bKatTabCopy, "bKatTabCopy");
            bKatTabCopy.DefaultStyle = true;
            bKatTabCopy.Name = "bKatTabCopy";
            bKatTabCopy.UseVisualStyleBackColor = true;
            bKatTabCopy.Click += bKatTabCopy_Click;
            // 
            // bKatTabDelete
            // 
            resources.ApplyResources(bKatTabDelete, "bKatTabDelete");
            bKatTabDelete.DefaultStyle = true;
            bKatTabDelete.Name = "bKatTabDelete";
            bKatTabDelete.UseVisualStyleBackColor = true;
            bKatTabDelete.Click += bKatTabDelete_Click;
            // 
            // bKatTabEdit
            // 
            resources.ApplyResources(bKatTabEdit, "bKatTabEdit");
            bKatTabEdit.DefaultStyle = true;
            bKatTabEdit.Name = "bKatTabEdit";
            bKatTabEdit.UseVisualStyleBackColor = true;
            bKatTabEdit.Click += bKatTabEdit_Click;
            // 
            // bKatTabAdd
            // 
            resources.ApplyResources(bKatTabAdd, "bKatTabAdd");
            bKatTabAdd.DefaultStyle = true;
            bKatTabAdd.Name = "bKatTabAdd";
            bKatTabAdd.UseVisualStyleBackColor = true;
            bKatTabAdd.Click += bKatTabAdd_Click;
            // 
            // tpTabTab
            // 
            tpTabTab.BackColor = Color.Transparent;
            tpTabTab.Controls.Add(listTabTabs);
            tpTabTab.Controls.Add(label25);
            tpTabTab.Controls.Add(groupBox7);
            resources.ApplyResources(tpTabTab, "tpTabTab");
            tpTabTab.Name = "tpTabTab";
            // 
            // listTabTabs
            // 
            listTabTabs.FormattingEnabled = true;
            resources.ApplyResources(listTabTabs, "listTabTabs");
            listTabTabs.Name = "listTabTabs";
            // 
            // label25
            // 
            resources.ApplyResources(label25, "label25");
            label25.Name = "label25";
            // 
            // groupBox7
            // 
            groupBox7.BorderColor = Color.LightGray;
            groupBox7.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox7.BorderThickness = 1;
            groupBox7.Controls.Add(bOpenEditorTab);
            groupBox7.Controls.Add(bTabTabDelete);
            groupBox7.DefaultStyle = true;
            groupBox7.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox7, "groupBox7");
            groupBox7.Name = "groupBox7";
            groupBox7.TabStop = false;
            // 
            // bOpenEditorTab
            // 
            resources.ApplyResources(bOpenEditorTab, "bOpenEditorTab");
            bOpenEditorTab.DefaultStyle = true;
            bOpenEditorTab.Name = "bOpenEditorTab";
            bOpenEditorTab.UseVisualStyleBackColor = true;
            bOpenEditorTab.Click += bOpenEditorTab_Click;
            // 
            // bTabTabDelete
            // 
            resources.ApplyResources(bTabTabDelete, "bTabTabDelete");
            bTabTabDelete.DefaultStyle = true;
            bTabTabDelete.Name = "bTabTabDelete";
            bTabTabDelete.UseVisualStyleBackColor = true;
            bTabTabDelete.Click += bTabTabDelete_Click;
            // 
            // tpTTexts
            // 
            tpTTexts.BackColor = Color.Transparent;
            tpTTexts.Controls.Add(label50);
            tpTTexts.Controls.Add(tbCommentTText);
            tpTTexts.Controls.Add(listTexty);
            tpTTexts.Controls.Add(label28);
            tpTTexts.Controls.Add(groupBox8);
            resources.ApplyResources(tpTTexts, "tpTTexts");
            tpTTexts.Name = "tpTTexts";
            // 
            // label50
            // 
            resources.ApplyResources(label50, "label50");
            label50.Name = "label50";
            // 
            // tbCommentTText
            // 
            tbCommentTText.BorderColor = Color.DimGray;
            tbCommentTText.BorderThickness = 1;
            tbCommentTText.DefaultStyle = true;
            tbCommentTText.DisabledBackColor = SystemColors.Control;
            tbCommentTText.DisabledBorderColor = SystemColors.InactiveBorder;
            tbCommentTText.DisabledForeColor = SystemColors.GrayText;
            tbCommentTText.HighlightColor = SystemColors.Highlight;
            tbCommentTText.HintForeColor = SystemColors.GrayText;
            tbCommentTText.HintText = null;
            resources.ApplyResources(tbCommentTText, "tbCommentTText");
            tbCommentTText.Name = "tbCommentTText";
            tbCommentTText.ReadOnly = true;
            tbCommentTText.UseDarkScrollBar = false;
            // 
            // listTexty
            // 
            listTexty.FormattingEnabled = true;
            resources.ApplyResources(listTexty, "listTexty");
            listTexty.Name = "listTexty";
            listTexty.SelectedIndexChanged += listTexty_SelectedIndexChanged;
            // 
            // label28
            // 
            resources.ApplyResources(label28, "label28");
            label28.Name = "label28";
            // 
            // groupBox8
            // 
            groupBox8.BorderColor = Color.LightGray;
            groupBox8.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox8.BorderThickness = 1;
            groupBox8.Controls.Add(bTextDelete);
            groupBox8.Controls.Add(bTextEdit);
            groupBox8.Controls.Add(bTextAdd);
            groupBox8.DefaultStyle = true;
            groupBox8.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox8, "groupBox8");
            groupBox8.Name = "groupBox8";
            groupBox8.TabStop = false;
            // 
            // bTextDelete
            // 
            resources.ApplyResources(bTextDelete, "bTextDelete");
            bTextDelete.DefaultStyle = true;
            bTextDelete.Name = "bTextDelete";
            bTextDelete.UseVisualStyleBackColor = true;
            bTextDelete.Click += bTextDelete_Click;
            // 
            // bTextEdit
            // 
            resources.ApplyResources(bTextEdit, "bTextEdit");
            bTextEdit.DefaultStyle = true;
            bTextEdit.Name = "bTextEdit";
            bTextEdit.UseVisualStyleBackColor = true;
            bTextEdit.Click += bTextEdit_Click;
            // 
            // bTextAdd
            // 
            resources.ApplyResources(bTextAdd, "bTextAdd");
            bTextAdd.DefaultStyle = true;
            bTextAdd.Name = "bTextAdd";
            bTextAdd.UseVisualStyleBackColor = true;
            bTextAdd.Click += bTextAdd_Click;
            // 
            // tpFonts
            // 
            tpFonts.BackColor = Color.Transparent;
            tpFonts.Controls.Add(bOpenFontDir);
            tpFonts.Controls.Add(tbFontDir);
            tpFonts.Controls.Add(label37);
            tpFonts.Controls.Add(listFonts);
            tpFonts.Controls.Add(label29);
            tpFonts.Controls.Add(groupBox9);
            resources.ApplyResources(tpFonts, "tpFonts");
            tpFonts.Name = "tpFonts";
            // 
            // bOpenFontDir
            // 
            resources.ApplyResources(bOpenFontDir, "bOpenFontDir");
            bOpenFontDir.DefaultStyle = true;
            bOpenFontDir.Name = "bOpenFontDir";
            bOpenFontDir.UseVisualStyleBackColor = true;
            bOpenFontDir.Click += bOpenFontDir_Click;
            // 
            // tbFontDir
            // 
            tbFontDir.BorderColor = Color.DimGray;
            tbFontDir.BorderThickness = 1;
            tbFontDir.DefaultStyle = true;
            tbFontDir.DisabledBackColor = SystemColors.Control;
            tbFontDir.DisabledBorderColor = SystemColors.InactiveBorder;
            tbFontDir.DisabledForeColor = SystemColors.GrayText;
            tbFontDir.HighlightColor = SystemColors.Highlight;
            tbFontDir.HintForeColor = SystemColors.GrayText;
            tbFontDir.HintText = null;
            resources.ApplyResources(tbFontDir, "tbFontDir");
            tbFontDir.Name = "tbFontDir";
            tbFontDir.ReadOnly = true;
            tbFontDir.UseDarkScrollBar = false;
            // 
            // label37
            // 
            resources.ApplyResources(label37, "label37");
            label37.Name = "label37";
            // 
            // listFonts
            // 
            listFonts.FormattingEnabled = true;
            resources.ApplyResources(listFonts, "listFonts");
            listFonts.Name = "listFonts";
            listFonts.SelectedIndexChanged += listFonts_SelectedIndexChanged;
            // 
            // label29
            // 
            resources.ApplyResources(label29, "label29");
            label29.Name = "label29";
            // 
            // groupBox9
            // 
            groupBox9.BorderColor = Color.LightGray;
            groupBox9.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox9.BorderThickness = 1;
            groupBox9.Controls.Add(label32);
            groupBox9.Controls.Add(cbFontIsNumber);
            groupBox9.Controls.Add(tbFontFile);
            groupBox9.Controls.Add(cbFontSpecAssigments);
            groupBox9.Controls.Add(cbFontSpecChar);
            groupBox9.Controls.Add(cbFontUpper);
            groupBox9.Controls.Add(cbFontLower);
            groupBox9.Controls.Add(cbFontDia);
            groupBox9.Controls.Add(cbFontProportional);
            groupBox9.Controls.Add(label36);
            groupBox9.Controls.Add(cbFontType);
            groupBox9.Controls.Add(label35);
            groupBox9.Controls.Add(nudFontWidth);
            groupBox9.Controls.Add(label34);
            groupBox9.Controls.Add(nudFontSize);
            groupBox9.Controls.Add(label33);
            groupBox9.Controls.Add(tbFontName);
            groupBox9.Controls.Add(label31);
            groupBox9.Controls.Add(nudFontID);
            groupBox9.Controls.Add(label30);
            groupBox9.Controls.Add(bFontDelete);
            groupBox9.Controls.Add(bFontEdit);
            groupBox9.Controls.Add(bFontAdd);
            groupBox9.DefaultStyle = true;
            groupBox9.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox9, "groupBox9");
            groupBox9.Name = "groupBox9";
            groupBox9.TabStop = false;
            // 
            // label32
            // 
            resources.ApplyResources(label32, "label32");
            label32.Name = "label32";
            // 
            // cbFontIsNumber
            // 
            resources.ApplyResources(cbFontIsNumber, "cbFontIsNumber");
            cbFontIsNumber.BorderColor = Color.Black;
            cbFontIsNumber.BoxBackColor = Color.White;
            cbFontIsNumber.DefaultStyle = true;
            cbFontIsNumber.DisabledForeColor = Color.DimGray;
            cbFontIsNumber.HighlightColor = SystemColors.Highlight;
            cbFontIsNumber.MarkColor = Color.Black;
            cbFontIsNumber.Name = "cbFontIsNumber";
            cbFontIsNumber.UseVisualStyleBackColor = true;
            // 
            // tbFontFile
            // 
            tbFontFile.BorderColor = Color.DimGray;
            tbFontFile.BorderThickness = 1;
            tbFontFile.DefaultStyle = true;
            tbFontFile.DisabledBackColor = SystemColors.Control;
            tbFontFile.DisabledBorderColor = SystemColors.InactiveBorder;
            tbFontFile.DisabledForeColor = SystemColors.GrayText;
            tbFontFile.HighlightColor = SystemColors.Highlight;
            tbFontFile.HintForeColor = SystemColors.GrayText;
            tbFontFile.HintText = null;
            resources.ApplyResources(tbFontFile, "tbFontFile");
            tbFontFile.Name = "tbFontFile";
            tbFontFile.UseDarkScrollBar = false;
            // 
            // cbFontSpecAssigments
            // 
            resources.ApplyResources(cbFontSpecAssigments, "cbFontSpecAssigments");
            cbFontSpecAssigments.BorderColor = Color.Black;
            cbFontSpecAssigments.BoxBackColor = Color.White;
            cbFontSpecAssigments.DefaultStyle = true;
            cbFontSpecAssigments.DisabledForeColor = Color.DimGray;
            cbFontSpecAssigments.HighlightColor = SystemColors.Highlight;
            cbFontSpecAssigments.MarkColor = Color.Black;
            cbFontSpecAssigments.Name = "cbFontSpecAssigments";
            cbFontSpecAssigments.UseVisualStyleBackColor = true;
            // 
            // cbFontSpecChar
            // 
            resources.ApplyResources(cbFontSpecChar, "cbFontSpecChar");
            cbFontSpecChar.BorderColor = Color.Black;
            cbFontSpecChar.BoxBackColor = Color.White;
            cbFontSpecChar.DefaultStyle = true;
            cbFontSpecChar.DisabledForeColor = Color.DimGray;
            cbFontSpecChar.HighlightColor = SystemColors.Highlight;
            cbFontSpecChar.MarkColor = Color.Black;
            cbFontSpecChar.Name = "cbFontSpecChar";
            cbFontSpecChar.UseVisualStyleBackColor = true;
            // 
            // cbFontUpper
            // 
            resources.ApplyResources(cbFontUpper, "cbFontUpper");
            cbFontUpper.BorderColor = Color.Black;
            cbFontUpper.BoxBackColor = Color.White;
            cbFontUpper.DefaultStyle = true;
            cbFontUpper.DisabledForeColor = Color.DimGray;
            cbFontUpper.HighlightColor = SystemColors.Highlight;
            cbFontUpper.MarkColor = Color.Black;
            cbFontUpper.Name = "cbFontUpper";
            cbFontUpper.UseVisualStyleBackColor = true;
            // 
            // cbFontLower
            // 
            resources.ApplyResources(cbFontLower, "cbFontLower");
            cbFontLower.BorderColor = Color.Black;
            cbFontLower.BoxBackColor = Color.White;
            cbFontLower.DefaultStyle = true;
            cbFontLower.DisabledForeColor = Color.DimGray;
            cbFontLower.HighlightColor = SystemColors.Highlight;
            cbFontLower.MarkColor = Color.Black;
            cbFontLower.Name = "cbFontLower";
            cbFontLower.UseVisualStyleBackColor = true;
            // 
            // cbFontDia
            // 
            resources.ApplyResources(cbFontDia, "cbFontDia");
            cbFontDia.BorderColor = Color.Black;
            cbFontDia.BoxBackColor = Color.White;
            cbFontDia.DefaultStyle = true;
            cbFontDia.DisabledForeColor = Color.DimGray;
            cbFontDia.HighlightColor = SystemColors.Highlight;
            cbFontDia.MarkColor = Color.Black;
            cbFontDia.Name = "cbFontDia";
            cbFontDia.UseVisualStyleBackColor = true;
            // 
            // cbFontProportional
            // 
            resources.ApplyResources(cbFontProportional, "cbFontProportional");
            cbFontProportional.BorderColor = Color.Black;
            cbFontProportional.BoxBackColor = Color.White;
            cbFontProportional.DefaultStyle = true;
            cbFontProportional.DisabledForeColor = Color.DimGray;
            cbFontProportional.HighlightColor = SystemColors.Highlight;
            cbFontProportional.MarkColor = Color.Black;
            cbFontProportional.Name = "cbFontProportional";
            cbFontProportional.UseVisualStyleBackColor = true;
            // 
            // label36
            // 
            resources.ApplyResources(label36, "label36");
            label36.Name = "label36";
            // 
            // cbFontType
            // 
            cbFontType.DefaultStyle = true;
            cbFontType.DropDownBackColor = Color.White;
            cbFontType.DropDownSelectedRowBackColor = Color.Empty;
            cbFontType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFontType.FormattingEnabled = true;
            resources.ApplyResources(cbFontType, "cbFontType");
            cbFontType.Name = "cbFontType";
            exComboBoxStyle9.ArrowColor = null;
            exComboBoxStyle9.BackColor = null;
            exComboBoxStyle9.BorderColor = null;
            exComboBoxStyle9.ButtonBackColor = null;
            exComboBoxStyle9.ButtonBorderColor = null;
            exComboBoxStyle9.ButtonRenderFirst = null;
            exComboBoxStyle9.ForeColor = null;
            cbFontType.StyleDisabled = exComboBoxStyle9;
            exComboBoxStyle10.ArrowColor = null;
            exComboBoxStyle10.BackColor = null;
            exComboBoxStyle10.BorderColor = null;
            exComboBoxStyle10.ButtonBackColor = null;
            exComboBoxStyle10.ButtonBorderColor = null;
            exComboBoxStyle10.ButtonRenderFirst = null;
            exComboBoxStyle10.ForeColor = null;
            cbFontType.StyleHighlight = exComboBoxStyle10;
            exComboBoxStyle11.ArrowColor = null;
            exComboBoxStyle11.BackColor = null;
            exComboBoxStyle11.BorderColor = null;
            exComboBoxStyle11.ButtonBackColor = null;
            exComboBoxStyle11.ButtonBorderColor = null;
            exComboBoxStyle11.ButtonRenderFirst = null;
            exComboBoxStyle11.ForeColor = null;
            cbFontType.StyleNormal = exComboBoxStyle11;
            exComboBoxStyle12.ArrowColor = null;
            exComboBoxStyle12.BackColor = null;
            exComboBoxStyle12.BorderColor = null;
            exComboBoxStyle12.ButtonBackColor = null;
            exComboBoxStyle12.ButtonBorderColor = null;
            exComboBoxStyle12.ButtonRenderFirst = null;
            exComboBoxStyle12.ForeColor = null;
            cbFontType.StyleSelected = exComboBoxStyle12;
            cbFontType.UseDarkScrollBar = false;
            // 
            // label35
            // 
            resources.ApplyResources(label35, "label35");
            label35.Name = "label35";
            // 
            // nudFontWidth
            // 
            nudFontWidth.ArrowsColor = Color.Black;
            nudFontWidth.BorderColor = Color.Gainsboro;
            nudFontWidth.DefaultStyle = true;
            nudFontWidth.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(nudFontWidth, "nudFontWidth");
            nudFontWidth.Name = "nudFontWidth";
            nudFontWidth.SelectedButtonColor = Color.Empty;
            // 
            // label34
            // 
            resources.ApplyResources(label34, "label34");
            label34.Name = "label34";
            // 
            // nudFontSize
            // 
            nudFontSize.ArrowsColor = Color.Black;
            nudFontSize.BorderColor = Color.Gainsboro;
            nudFontSize.DefaultStyle = true;
            nudFontSize.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(nudFontSize, "nudFontSize");
            nudFontSize.Name = "nudFontSize";
            nudFontSize.SelectedButtonColor = Color.Empty;
            // 
            // label33
            // 
            resources.ApplyResources(label33, "label33");
            label33.Name = "label33";
            // 
            // tbFontName
            // 
            tbFontName.BorderColor = Color.DimGray;
            tbFontName.BorderThickness = 1;
            tbFontName.DefaultStyle = true;
            tbFontName.DisabledBackColor = SystemColors.Control;
            tbFontName.DisabledBorderColor = SystemColors.InactiveBorder;
            tbFontName.DisabledForeColor = SystemColors.GrayText;
            tbFontName.HighlightColor = SystemColors.Highlight;
            tbFontName.HintForeColor = SystemColors.GrayText;
            tbFontName.HintText = null;
            resources.ApplyResources(tbFontName, "tbFontName");
            tbFontName.Name = "tbFontName";
            tbFontName.UseDarkScrollBar = false;
            // 
            // label31
            // 
            resources.ApplyResources(label31, "label31");
            label31.Name = "label31";
            // 
            // nudFontID
            // 
            nudFontID.ArrowsColor = Color.Black;
            nudFontID.BorderColor = Color.Gainsboro;
            nudFontID.DefaultStyle = true;
            nudFontID.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(nudFontID, "nudFontID");
            nudFontID.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            nudFontID.Name = "nudFontID";
            nudFontID.SelectedButtonColor = Color.Empty;
            nudFontID.ValueChanged += nudFontID_ValueChanged;
            // 
            // label30
            // 
            resources.ApplyResources(label30, "label30");
            label30.Name = "label30";
            // 
            // bFontDelete
            // 
            resources.ApplyResources(bFontDelete, "bFontDelete");
            bFontDelete.DefaultStyle = true;
            bFontDelete.Name = "bFontDelete";
            bFontDelete.UseVisualStyleBackColor = true;
            bFontDelete.Click += bFontDelete_Click;
            // 
            // bFontEdit
            // 
            resources.ApplyResources(bFontEdit, "bFontEdit");
            bFontEdit.DefaultStyle = true;
            bFontEdit.Name = "bFontEdit";
            bFontEdit.UseVisualStyleBackColor = true;
            bFontEdit.Click += bFontEdit_Click;
            // 
            // bFontAdd
            // 
            resources.ApplyResources(bFontAdd, "bFontAdd");
            bFontAdd.DefaultStyle = true;
            bFontAdd.Name = "bFontAdd";
            bFontAdd.UseVisualStyleBackColor = true;
            bFontAdd.Click += bFontAdd_Click;
            // 
            // tpStateDgm
            // 
            tpStateDgm.BackColor = Color.Transparent;
            tpStateDgm.Controls.Add(flpStateDgm);
            resources.ApplyResources(tpStateDgm, "tpStateDgm");
            tpStateDgm.Name = "tpStateDgm";
            // 
            // flpStateDgm
            // 
            flpStateDgm.Controls.Add(lStateDgmInfo);
            flpStateDgm.Controls.Add(lStateDgmStatus);
            flpStateDgm.Controls.Add(bStateDgmOpen);
            resources.ApplyResources(flpStateDgm, "flpStateDgm");
            flpStateDgm.Name = "flpStateDgm";
            flpStateDgm.SizeChanged += flpStateDgm_SizeChanged;
            // 
            // lStateDgmInfo
            // 
            resources.ApplyResources(lStateDgmInfo, "lStateDgmInfo");
            lStateDgmInfo.Name = "lStateDgmInfo";
            // 
            // lStateDgmStatus
            // 
            resources.ApplyResources(lStateDgmStatus, "lStateDgmStatus");
            lStateDgmStatus.Name = "lStateDgmStatus";
            // 
            // bStateDgmOpen
            // 
            resources.ApplyResources(bStateDgmOpen, "bStateDgmOpen");
            bStateDgmOpen.DefaultStyle = true;
            bStateDgmOpen.Name = "bStateDgmOpen";
            bStateDgmOpen.UseVisualStyleBackColor = true;
            bStateDgmOpen.Click += bStateDgmOpen_Click;
            // 
            // checkBox1
            // 
            resources.ApplyResources(checkBox1, "checkBox1");
            checkBox1.BorderColor = Color.Black;
            checkBox1.BoxBackColor = Color.White;
            checkBox1.DefaultStyle = true;
            checkBox1.DisabledForeColor = Color.DimGray;
            checkBox1.HighlightColor = SystemColors.Highlight;
            checkBox1.MarkColor = Color.Black;
            checkBox1.Name = "checkBox1";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // cbCustomTrainTypDruh
            // 
            cbCustomTrainTypDruh.DefaultStyle = true;
            cbCustomTrainTypDruh.DropDownBackColor = Color.White;
            cbCustomTrainTypDruh.DropDownSelectedRowBackColor = Color.Empty;
            resources.ApplyResources(cbCustomTrainTypDruh, "cbCustomTrainTypDruh");
            cbCustomTrainTypDruh.Name = "cbCustomTrainTypDruh";
            exComboBoxStyle13.ArrowColor = null;
            exComboBoxStyle13.BackColor = null;
            exComboBoxStyle13.BorderColor = null;
            exComboBoxStyle13.ButtonBackColor = null;
            exComboBoxStyle13.ButtonBorderColor = null;
            exComboBoxStyle13.ButtonRenderFirst = null;
            exComboBoxStyle13.ForeColor = null;
            cbCustomTrainTypDruh.StyleDisabled = exComboBoxStyle13;
            exComboBoxStyle14.ArrowColor = null;
            exComboBoxStyle14.BackColor = null;
            exComboBoxStyle14.BorderColor = null;
            exComboBoxStyle14.ButtonBackColor = null;
            exComboBoxStyle14.ButtonBorderColor = null;
            exComboBoxStyle14.ButtonRenderFirst = null;
            exComboBoxStyle14.ForeColor = null;
            cbCustomTrainTypDruh.StyleHighlight = exComboBoxStyle14;
            exComboBoxStyle15.ArrowColor = null;
            exComboBoxStyle15.BackColor = null;
            exComboBoxStyle15.BorderColor = null;
            exComboBoxStyle15.ButtonBackColor = null;
            exComboBoxStyle15.ButtonBorderColor = null;
            exComboBoxStyle15.ButtonRenderFirst = null;
            exComboBoxStyle15.ForeColor = null;
            cbCustomTrainTypDruh.StyleNormal = exComboBoxStyle15;
            exComboBoxStyle16.ArrowColor = null;
            exComboBoxStyle16.BackColor = null;
            exComboBoxStyle16.BorderColor = null;
            exComboBoxStyle16.ButtonBackColor = null;
            exComboBoxStyle16.ButtonBorderColor = null;
            exComboBoxStyle16.ButtonRenderFirst = null;
            exComboBoxStyle16.ForeColor = null;
            cbCustomTrainTypDruh.StyleSelected = exComboBoxStyle16;
            cbCustomTrainTypDruh.UseDarkScrollBar = false;
            // 
            // cbDefTrainTypSkratka
            // 
            cbDefTrainTypSkratka.DefaultStyle = true;
            cbDefTrainTypSkratka.DropDownBackColor = Color.White;
            cbDefTrainTypSkratka.DropDownSelectedRowBackColor = Color.Empty;
            resources.ApplyResources(cbDefTrainTypSkratka, "cbDefTrainTypSkratka");
            cbDefTrainTypSkratka.Name = "cbDefTrainTypSkratka";
            exComboBoxStyle17.ArrowColor = null;
            exComboBoxStyle17.BackColor = null;
            exComboBoxStyle17.BorderColor = null;
            exComboBoxStyle17.ButtonBackColor = null;
            exComboBoxStyle17.ButtonBorderColor = null;
            exComboBoxStyle17.ButtonRenderFirst = null;
            exComboBoxStyle17.ForeColor = null;
            cbDefTrainTypSkratka.StyleDisabled = exComboBoxStyle17;
            exComboBoxStyle18.ArrowColor = null;
            exComboBoxStyle18.BackColor = null;
            exComboBoxStyle18.BorderColor = null;
            exComboBoxStyle18.ButtonBackColor = null;
            exComboBoxStyle18.ButtonBorderColor = null;
            exComboBoxStyle18.ButtonRenderFirst = null;
            exComboBoxStyle18.ForeColor = null;
            cbDefTrainTypSkratka.StyleHighlight = exComboBoxStyle18;
            exComboBoxStyle19.ArrowColor = null;
            exComboBoxStyle19.BackColor = null;
            exComboBoxStyle19.BorderColor = null;
            exComboBoxStyle19.ButtonBackColor = null;
            exComboBoxStyle19.ButtonBorderColor = null;
            exComboBoxStyle19.ButtonRenderFirst = null;
            exComboBoxStyle19.ForeColor = null;
            cbDefTrainTypSkratka.StyleNormal = exComboBoxStyle19;
            exComboBoxStyle20.ArrowColor = null;
            exComboBoxStyle20.BackColor = null;
            exComboBoxStyle20.BorderColor = null;
            exComboBoxStyle20.ButtonBackColor = null;
            exComboBoxStyle20.ButtonBorderColor = null;
            exComboBoxStyle20.ButtonRenderFirst = null;
            exComboBoxStyle20.ForeColor = null;
            cbDefTrainTypSkratka.StyleSelected = exComboBoxStyle20;
            cbDefTrainTypSkratka.UseDarkScrollBar = false;
            // 
            // FLocalSettings
            // 
            AcceptButton = bSave;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(bSave);
            Controls.Add(tabControl);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            HelpButton = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FLocalSettings";
            ShowInTaskbar = false;
            HelpButtonClicked += FLocalSettings_HelpButtonClicked;
            FormClosed += FLocalSettings_FormClosed;
            Load += FLocalSettings_Load;
            tabControl.ResumeLayout(false);
            tpGrafikon.ResumeLayout(false);
            groupBox15.ResumeLayout(false);
            groupBox15.PerformLayout();
            groupBox14.ResumeLayout(false);
            groupBox14.PerformLayout();
            ((ISupportInitialize)nudIDStation).EndInit();
            groupBox13.ResumeLayout(false);
            groupBox13.PerformLayout();
            tpStanice.ResumeLayout(false);
            tpStanice.PerformLayout();
            groupBox12.ResumeLayout(false);
            groupBox12.PerformLayout();
            ((ISupportInitialize)nudIDStanice).EndInit();
            tpDopravcovia.ResumeLayout(false);
            tpDopravcovia.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tpNastupistia.ResumeLayout(false);
            tpNastupistia.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            tpKolaje.ResumeLayout(false);
            tpKolaje.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            tpFyzTab.ResumeLayout(false);
            tpFyzTab.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            tpLogTab.ResumeLayout(false);
            tpLogTab.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            tpKatTab.ResumeLayout(false);
            tpKatTab.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            tpTabTab.ResumeLayout(false);
            tpTabTab.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            tpTTexts.ResumeLayout(false);
            tpTTexts.PerformLayout();
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            tpFonts.ResumeLayout(false);
            tpFonts.PerformLayout();
            groupBox9.ResumeLayout(false);
            groupBox9.PerformLayout();
            ((ISupportInitialize)nudFontWidth).EndInit();
            ((ISupportInitialize)nudFontSize).EndInit();
            ((ISupportInitialize)nudFontID).EndInit();
            tpStateDgm.ResumeLayout(false);
            flpStateDgm.ResumeLayout(false);
            flpStateDgm.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

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
        private ExControls.ExDateTimePicker dtpDataDo;
        private ExControls.ExDateTimePicker dtpDataOd;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private ExTextBox tbDir;
        private ExControls.ExButton bOpenDir;
        private System.Windows.Forms.Label label9;
        private ExControls.ExDateTimePicker dtpGVDDo;
        private ExControls.ExDateTimePicker dtpGVDOd;
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
        private System.Windows.Forms.TabPage tpStateDgm;
        private System.Windows.Forms.FlowLayoutPanel flpStateDgm;
        private System.Windows.Forms.Label lStateDgmInfo;
        private System.Windows.Forms.Label lStateDgmStatus;
        private ExControls.ExButton bStateDgmOpen;
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
        private Label label52;
        private ExTextBox tbKolajAlt;
        private Label label53;
        private ExTextBox tbNastupisteKolaj;
    }
}