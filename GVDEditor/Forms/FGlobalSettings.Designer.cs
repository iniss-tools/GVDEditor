using ExControls;
using ToolsCore.Entities;

namespace GVDEditor.Forms
{
    partial class FGlobalSettings
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
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FGlobalSettings));
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
            tabControl = new ExTabControl();
            tpGrafikony = new TabPage();
            label13 = new Label();
            listGrafikony = new ListBox();
            label3 = new Label();
            groupBox1 = new ExGroupBox();
            bGrafikonEdit = new ExButton();
            bEditColor = new ExButton();
            pbColor = new PictureBox();
            label12 = new Label();
            nudHlaseniePort = new ExNumericUpDown();
            nudTabPort = new ExNumericUpDown();
            label11 = new Label();
            label10 = new Label();
            tbGrafikonPath = new ExTextBox();
            label9 = new Label();
            tbGrafikonObdobie = new ExTextBox();
            label4 = new Label();
            tbGrafikonStanica = new ExTextBox();
            bGrafikonDelete = new ExButton();
            label7 = new Label();
            tpJazyky = new TabPage();
            dgvLanguagesRawBank = new DataGridView();
            keyDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            fyzBankNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            languageBindingSource = new BindingSource(components);
            label19 = new Label();
            listLanguages = new ListBox();
            label15 = new Label();
            groupBox4 = new ExGroupBox();
            tbLanguageSkratka = new ExTextBox();
            label17 = new Label();
            cbIsBasic = new ExCheckBox();
            tbLanguageName = new ExTextBox();
            bLanguageRemove = new ExButton();
            bLanguageEdit = new ExButton();
            bLanguageAdd = new ExButton();
            label16 = new Label();
            tpMeskania = new TabPage();
            listMeskania = new ListBox();
            label38 = new Label();
            groupBox10 = new ExGroupBox();
            nudMeskanie = new ExNumericUpDown();
            bMeskanieDelete = new ExButton();
            bMeskanieEdit = new ExButton();
            bMeskanieAdd = new ExButton();
            label39 = new Label();
            tpTrainTypes = new TabPage();
            groupBox11 = new ExGroupBox();
            label42 = new Label();
            tbCustomTrainTypText = new ExTextBox();
            tbCustomTrainTypSkratka = new ExTextBox();
            label41 = new Label();
            cbCustomTrainTypDruh = new ExComboBox();
            label40 = new Label();
            bCustomTrainTypDelete = new ExButton();
            bCustomTrainTypAdd = new ExButton();
            bCustomTrainTypEdit = new ExButton();
            listTrainTypes = new ListBox();
            label14 = new Label();
            groupBox3 = new ExGroupBox();
            label20 = new Label();
            tbDefaultTrainTypText = new ExTextBox();
            tbDefaultTrainTypSkratka = new ExTextBox();
            label21 = new Label();
            cbDefTrainTypSkratka = new ExComboBox();
            bDefTrainTypDelete = new ExButton();
            bDefTrainTypEdit = new ExButton();
            bDefTrainTypAdd = new ExButton();
            label18 = new Label();
            tpAudio = new TabPage();
            listAudio = new ListBox();
            label1 = new Label();
            groupBox2 = new ExGroupBox();
            label26 = new Label();
            label25 = new Label();
            tbNode = new ExTextBox();
            tbExchange = new ExTextBox();
            tbAmplifier = new ExTextBox();
            tbInputLine = new ExTextBox();
            label24 = new Label();
            label23 = new Label();
            label22 = new Label();
            tbSoundCard = new ExTextBox();
            tbAudioName = new ExTextBox();
            label8 = new Label();
            label6 = new Label();
            tbAudioNazovFronta = new ExTextBox();
            tbAudioNazovSkratka = new ExTextBox();
            label5 = new Label();
            cbCustomOnly = new ExCheckBox();
            cbAudioStanica = new ExComboBox();
            bAudioDelete = new ExButton();
            bAudioEdit = new ExButton();
            bAudioAdd = new ExButton();
            label2 = new Label();
            bSave = new ExButton();
            colorDialogFarba = new ColorDialog();
            tabControl.SuspendLayout();
            tpGrafikony.SuspendLayout();
            groupBox1.SuspendLayout();
            ((ISupportInitialize)pbColor).BeginInit();
            ((ISupportInitialize)nudHlaseniePort).BeginInit();
            ((ISupportInitialize)nudTabPort).BeginInit();
            tpJazyky.SuspendLayout();
            ((ISupportInitialize)dgvLanguagesRawBank).BeginInit();
            ((ISupportInitialize)languageBindingSource).BeginInit();
            groupBox4.SuspendLayout();
            tpMeskania.SuspendLayout();
            groupBox10.SuspendLayout();
            ((ISupportInitialize)nudMeskanie).BeginInit();
            tpTrainTypes.SuspendLayout();
            groupBox11.SuspendLayout();
            groupBox3.SuspendLayout();
            tpAudio.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.ActiveHeaderBackColor = Color.White;
            tabControl.ActiveHeaderForeColor = Color.Black;
            tabControl.BorderColor = Color.LightGray;
            tabControl.BorderThickness = 1;
            tabControl.Controls.Add(tpGrafikony);
            tabControl.Controls.Add(tpJazyky);
            tabControl.Controls.Add(tpMeskania);
            tabControl.Controls.Add(tpTrainTypes);
            tabControl.Controls.Add(tpAudio);
            tabControl.DefaultStyle = true;
            tabControl.HeaderBackColor = Color.LightGray;
            tabControl.HeaderForeColor = Color.Black;
            tabControl.HighlightBackColor = SystemColors.GradientInactiveCaption;
            tabControl.HighlightForeColor = Color.Black;
            resources.ApplyResources(tabControl, "tabControl");
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            // 
            // tpGrafikony
            // 
            tpGrafikony.BackColor = Color.Transparent;
            tpGrafikony.Controls.Add(label13);
            tpGrafikony.Controls.Add(listGrafikony);
            tpGrafikony.Controls.Add(label3);
            tpGrafikony.Controls.Add(groupBox1);
            resources.ApplyResources(tpGrafikony, "tpGrafikony");
            tpGrafikony.Name = "tpGrafikony";
            // 
            // label13
            // 
            resources.ApplyResources(label13, "label13");
            label13.Name = "label13";
            // 
            // listGrafikony
            // 
            listGrafikony.FormattingEnabled = true;
            resources.ApplyResources(listGrafikony, "listGrafikony");
            listGrafikony.Name = "listGrafikony";
            listGrafikony.SelectedIndexChanged += listGrafikony_SelectedIndexChanged;
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // groupBox1
            // 
            groupBox1.BorderColor = Color.LightGray;
            groupBox1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox1.BorderThickness = 1;
            groupBox1.Controls.Add(bGrafikonEdit);
            groupBox1.Controls.Add(bEditColor);
            groupBox1.Controls.Add(pbColor);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(nudHlaseniePort);
            groupBox1.Controls.Add(nudTabPort);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(tbGrafikonPath);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(tbGrafikonObdobie);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(tbGrafikonStanica);
            groupBox1.Controls.Add(bGrafikonDelete);
            groupBox1.Controls.Add(label7);
            groupBox1.DefaultStyle = true;
            groupBox1.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // bGrafikonEdit
            // 
            resources.ApplyResources(bGrafikonEdit, "bGrafikonEdit");
            bGrafikonEdit.DefaultStyle = true;
            bGrafikonEdit.Name = "bGrafikonEdit";
            bGrafikonEdit.UseVisualStyleBackColor = true;
            bGrafikonEdit.Click += bGrafikonEdit_Click;
            // 
            // bEditColor
            // 
            resources.ApplyResources(bEditColor, "bEditColor");
            bEditColor.DefaultStyle = true;
            bEditColor.Name = "bEditColor";
            bEditColor.UseVisualStyleBackColor = true;
            bEditColor.Click += bEditColor_Click;
            // 
            // pbColor
            // 
            pbColor.BackColor = Color.White;
            pbColor.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(pbColor, "pbColor");
            pbColor.Name = "pbColor";
            pbColor.TabStop = false;
            // 
            // label12
            // 
            resources.ApplyResources(label12, "label12");
            label12.Name = "label12";
            // 
            // nudHlaseniePort
            // 
            nudHlaseniePort.ArrowsColor = Color.Black;
            nudHlaseniePort.BorderColor = Color.Gainsboro;
            nudHlaseniePort.DefaultStyle = true;
            nudHlaseniePort.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(nudHlaseniePort, "nudHlaseniePort");
            nudHlaseniePort.Name = "nudHlaseniePort";
            nudHlaseniePort.SelectedButtonColor = SystemColors.Highlight;
            // 
            // nudTabPort
            // 
            nudTabPort.ArrowsColor = Color.Black;
            nudTabPort.BorderColor = Color.Gainsboro;
            nudTabPort.DefaultStyle = true;
            nudTabPort.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(nudTabPort, "nudTabPort");
            nudTabPort.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudTabPort.Name = "nudTabPort";
            nudTabPort.SelectedButtonColor = SystemColors.Highlight;
            // 
            // label11
            // 
            resources.ApplyResources(label11, "label11");
            label11.Name = "label11";
            // 
            // label10
            // 
            resources.ApplyResources(label10, "label10");
            label10.Name = "label10";
            // 
            // tbGrafikonPath
            // 
            tbGrafikonPath.BorderColor = Color.DimGray;
            tbGrafikonPath.BorderStyle = BorderStyle.FixedSingle;
            tbGrafikonPath.BorderThickness = 1;
            tbGrafikonPath.DefaultStyle = true;
            tbGrafikonPath.DisabledBackColor = SystemColors.Control;
            tbGrafikonPath.DisabledBorderColor = SystemColors.InactiveBorder;
            tbGrafikonPath.DisabledForeColor = SystemColors.GrayText;
            tbGrafikonPath.HighlightColor = SystemColors.Highlight;
            tbGrafikonPath.HintForeColor = SystemColors.GrayText;
            tbGrafikonPath.HintText = null;
            resources.ApplyResources(tbGrafikonPath, "tbGrafikonPath");
            tbGrafikonPath.Name = "tbGrafikonPath";
            tbGrafikonPath.ReadOnly = true;
            // 
            // label9
            // 
            resources.ApplyResources(label9, "label9");
            label9.Name = "label9";
            // 
            // tbGrafikonObdobie
            // 
            tbGrafikonObdobie.BorderColor = Color.DimGray;
            tbGrafikonObdobie.BorderStyle = BorderStyle.FixedSingle;
            tbGrafikonObdobie.BorderThickness = 1;
            tbGrafikonObdobie.DefaultStyle = true;
            tbGrafikonObdobie.DisabledBackColor = SystemColors.Control;
            tbGrafikonObdobie.DisabledBorderColor = SystemColors.InactiveBorder;
            tbGrafikonObdobie.DisabledForeColor = SystemColors.GrayText;
            tbGrafikonObdobie.HighlightColor = SystemColors.Highlight;
            tbGrafikonObdobie.HintForeColor = SystemColors.GrayText;
            tbGrafikonObdobie.HintText = null;
            resources.ApplyResources(tbGrafikonObdobie, "tbGrafikonObdobie");
            tbGrafikonObdobie.Name = "tbGrafikonObdobie";
            tbGrafikonObdobie.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // tbGrafikonStanica
            // 
            tbGrafikonStanica.BorderColor = Color.DimGray;
            tbGrafikonStanica.BorderStyle = BorderStyle.FixedSingle;
            tbGrafikonStanica.BorderThickness = 1;
            tbGrafikonStanica.DefaultStyle = true;
            tbGrafikonStanica.DisabledBackColor = SystemColors.Control;
            tbGrafikonStanica.DisabledBorderColor = SystemColors.InactiveBorder;
            tbGrafikonStanica.DisabledForeColor = SystemColors.GrayText;
            tbGrafikonStanica.HighlightColor = SystemColors.Highlight;
            tbGrafikonStanica.HintForeColor = SystemColors.GrayText;
            tbGrafikonStanica.HintText = null;
            resources.ApplyResources(tbGrafikonStanica, "tbGrafikonStanica");
            tbGrafikonStanica.Name = "tbGrafikonStanica";
            tbGrafikonStanica.ReadOnly = true;
            // 
            // bGrafikonDelete
            // 
            resources.ApplyResources(bGrafikonDelete, "bGrafikonDelete");
            bGrafikonDelete.DefaultStyle = true;
            bGrafikonDelete.Name = "bGrafikonDelete";
            bGrafikonDelete.UseVisualStyleBackColor = true;
            bGrafikonDelete.Click += bGrafikonDelete_Click;
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // tpJazyky
            // 
            tpJazyky.BackColor = Color.Transparent;
            tpJazyky.Controls.Add(dgvLanguagesRawBank);
            tpJazyky.Controls.Add(label19);
            tpJazyky.Controls.Add(listLanguages);
            tpJazyky.Controls.Add(label15);
            tpJazyky.Controls.Add(groupBox4);
            resources.ApplyResources(tpJazyky, "tpJazyky");
            tpJazyky.Name = "tpJazyky";
            // 
            // dgvLanguagesRawBank
            // 
            dgvLanguagesRawBank.AllowUserToAddRows = false;
            dgvLanguagesRawBank.AllowUserToDeleteRows = false;
            dgvLanguagesRawBank.AllowUserToResizeRows = false;
            dgvLanguagesRawBank.AutoGenerateColumns = false;
            dgvLanguagesRawBank.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLanguagesRawBank.Columns.AddRange(new DataGridViewColumn[] { keyDataGridViewTextBoxColumn, fyzBankNameDataGridViewTextBoxColumn });
            dgvLanguagesRawBank.DataSource = languageBindingSource;
            resources.ApplyResources(dgvLanguagesRawBank, "dgvLanguagesRawBank");
            dgvLanguagesRawBank.Name = "dgvLanguagesRawBank";
            dgvLanguagesRawBank.ReadOnly = true;
            dgvLanguagesRawBank.RowHeadersVisible = false;
            dgvLanguagesRawBank.RowTemplate.Height = 24;
            // 
            // keyDataGridViewTextBoxColumn
            // 
            keyDataGridViewTextBoxColumn.DataPropertyName = "Key";
            resources.ApplyResources(keyDataGridViewTextBoxColumn, "keyDataGridViewTextBoxColumn");
            keyDataGridViewTextBoxColumn.Name = "keyDataGridViewTextBoxColumn";
            keyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fyzBankNameDataGridViewTextBoxColumn
            // 
            fyzBankNameDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            fyzBankNameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            resources.ApplyResources(fyzBankNameDataGridViewTextBoxColumn, "fyzBankNameDataGridViewTextBoxColumn");
            fyzBankNameDataGridViewTextBoxColumn.Name = "fyzBankNameDataGridViewTextBoxColumn";
            fyzBankNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // languageBindingSource
            // 
            languageBindingSource.DataSource = typeof(FyzLanguage);
            // 
            // label19
            // 
            resources.ApplyResources(label19, "label19");
            label19.Name = "label19";
            // 
            // listLanguages
            // 
            listLanguages.FormattingEnabled = true;
            resources.ApplyResources(listLanguages, "listLanguages");
            listLanguages.Name = "listLanguages";
            listLanguages.SelectedIndexChanged += listLanguages_SelectedIndexChanged;
            // 
            // label15
            // 
            resources.ApplyResources(label15, "label15");
            label15.Name = "label15";
            // 
            // groupBox4
            // 
            groupBox4.BorderColor = Color.LightGray;
            groupBox4.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox4.BorderThickness = 1;
            groupBox4.Controls.Add(tbLanguageSkratka);
            groupBox4.Controls.Add(label17);
            groupBox4.Controls.Add(cbIsBasic);
            groupBox4.Controls.Add(tbLanguageName);
            groupBox4.Controls.Add(bLanguageRemove);
            groupBox4.Controls.Add(bLanguageEdit);
            groupBox4.Controls.Add(bLanguageAdd);
            groupBox4.Controls.Add(label16);
            groupBox4.DefaultStyle = true;
            groupBox4.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox4, "groupBox4");
            groupBox4.Name = "groupBox4";
            groupBox4.TabStop = false;
            // 
            // tbLanguageSkratka
            // 
            tbLanguageSkratka.BorderColor = Color.DimGray;
            tbLanguageSkratka.BorderThickness = 1;
            tbLanguageSkratka.DefaultStyle = true;
            tbLanguageSkratka.DisabledBackColor = SystemColors.Control;
            tbLanguageSkratka.DisabledBorderColor = SystemColors.InactiveBorder;
            tbLanguageSkratka.DisabledForeColor = SystemColors.GrayText;
            tbLanguageSkratka.HighlightColor = SystemColors.Highlight;
            tbLanguageSkratka.HintForeColor = SystemColors.GrayText;
            tbLanguageSkratka.HintText = null;
            resources.ApplyResources(tbLanguageSkratka, "tbLanguageSkratka");
            tbLanguageSkratka.Name = "tbLanguageSkratka";
            // 
            // label17
            // 
            resources.ApplyResources(label17, "label17");
            label17.Name = "label17";
            // 
            // cbIsBasic
            // 
            resources.ApplyResources(cbIsBasic, "cbIsBasic");
            cbIsBasic.BorderColor = Color.Black;
            cbIsBasic.BoxBackColor = Color.White;
            cbIsBasic.DefaultStyle = true;
            cbIsBasic.DisabledForeColor = Color.DimGray;
            cbIsBasic.HighlightColor = SystemColors.Highlight;
            cbIsBasic.MarkColor = Color.Black;
            cbIsBasic.Name = "cbIsBasic";
            cbIsBasic.UseVisualStyleBackColor = true;
            // 
            // tbLanguageName
            // 
            tbLanguageName.BorderColor = Color.DimGray;
            tbLanguageName.BorderThickness = 1;
            tbLanguageName.DefaultStyle = true;
            tbLanguageName.DisabledBackColor = SystemColors.Control;
            tbLanguageName.DisabledBorderColor = SystemColors.InactiveBorder;
            tbLanguageName.DisabledForeColor = SystemColors.GrayText;
            tbLanguageName.HighlightColor = SystemColors.Highlight;
            tbLanguageName.HintForeColor = SystemColors.GrayText;
            tbLanguageName.HintText = null;
            resources.ApplyResources(tbLanguageName, "tbLanguageName");
            tbLanguageName.Name = "tbLanguageName";
            // 
            // bLanguageRemove
            // 
            resources.ApplyResources(bLanguageRemove, "bLanguageRemove");
            bLanguageRemove.DefaultStyle = true;
            bLanguageRemove.Name = "bLanguageRemove";
            bLanguageRemove.UseVisualStyleBackColor = true;
            bLanguageRemove.Click += bLanguageRemove_Click;
            // 
            // bLanguageEdit
            // 
            resources.ApplyResources(bLanguageEdit, "bLanguageEdit");
            bLanguageEdit.DefaultStyle = true;
            bLanguageEdit.Name = "bLanguageEdit";
            bLanguageEdit.UseVisualStyleBackColor = true;
            bLanguageEdit.Click += bLanguageEdit_Click;
            // 
            // bLanguageAdd
            // 
            resources.ApplyResources(bLanguageAdd, "bLanguageAdd");
            bLanguageAdd.DefaultStyle = true;
            bLanguageAdd.Name = "bLanguageAdd";
            bLanguageAdd.UseVisualStyleBackColor = true;
            bLanguageAdd.Click += bLanguageAdd_Click;
            // 
            // label16
            // 
            resources.ApplyResources(label16, "label16");
            label16.Name = "label16";
            // 
            // tpMeskania
            // 
            tpMeskania.BackColor = Color.Transparent;
            tpMeskania.Controls.Add(listMeskania);
            tpMeskania.Controls.Add(label38);
            tpMeskania.Controls.Add(groupBox10);
            resources.ApplyResources(tpMeskania, "tpMeskania");
            tpMeskania.Name = "tpMeskania";
            // 
            // listMeskania
            // 
            listMeskania.FormattingEnabled = true;
            resources.ApplyResources(listMeskania, "listMeskania");
            listMeskania.Name = "listMeskania";
            listMeskania.SelectedIndexChanged += listMeskania_SelectedIndexChanged;
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
            groupBox10.Controls.Add(nudMeskanie);
            groupBox10.Controls.Add(bMeskanieDelete);
            groupBox10.Controls.Add(bMeskanieEdit);
            groupBox10.Controls.Add(bMeskanieAdd);
            groupBox10.Controls.Add(label39);
            groupBox10.DefaultStyle = true;
            groupBox10.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox10, "groupBox10");
            groupBox10.Name = "groupBox10";
            groupBox10.TabStop = false;
            // 
            // nudMeskanie
            // 
            nudMeskanie.ArrowsColor = Color.Black;
            nudMeskanie.BorderColor = Color.Gainsboro;
            nudMeskanie.DefaultStyle = true;
            nudMeskanie.HighlightColor = SystemColors.Highlight;
            nudMeskanie.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            resources.ApplyResources(nudMeskanie, "nudMeskanie");
            nudMeskanie.Maximum = new decimal(new int[] { 480, 0, 0, 0 });
            nudMeskanie.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            nudMeskanie.Name = "nudMeskanie";
            nudMeskanie.SelectedButtonColor = SystemColors.Highlight;
            nudMeskanie.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // bMeskanieDelete
            // 
            resources.ApplyResources(bMeskanieDelete, "bMeskanieDelete");
            bMeskanieDelete.DefaultStyle = true;
            bMeskanieDelete.Name = "bMeskanieDelete";
            bMeskanieDelete.UseVisualStyleBackColor = true;
            bMeskanieDelete.Click += bMeskanieDelete_Click;
            // 
            // bMeskanieEdit
            // 
            resources.ApplyResources(bMeskanieEdit, "bMeskanieEdit");
            bMeskanieEdit.DefaultStyle = true;
            bMeskanieEdit.Name = "bMeskanieEdit";
            bMeskanieEdit.UseVisualStyleBackColor = true;
            bMeskanieEdit.Click += bMeskanieEdit_Click;
            // 
            // bMeskanieAdd
            // 
            resources.ApplyResources(bMeskanieAdd, "bMeskanieAdd");
            bMeskanieAdd.DefaultStyle = true;
            bMeskanieAdd.Name = "bMeskanieAdd";
            bMeskanieAdd.UseVisualStyleBackColor = true;
            bMeskanieAdd.Click += bMeskanieAdd_Click;
            // 
            // label39
            // 
            resources.ApplyResources(label39, "label39");
            label39.Name = "label39";
            // 
            // tpTrainTypes
            // 
            tpTrainTypes.BackColor = Color.Transparent;
            tpTrainTypes.Controls.Add(groupBox11);
            tpTrainTypes.Controls.Add(listTrainTypes);
            tpTrainTypes.Controls.Add(label14);
            tpTrainTypes.Controls.Add(groupBox3);
            resources.ApplyResources(tpTrainTypes, "tpTrainTypes");
            tpTrainTypes.Name = "tpTrainTypes";
            // 
            // groupBox11
            // 
            groupBox11.BorderColor = Color.LightGray;
            groupBox11.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox11.BorderThickness = 1;
            groupBox11.Controls.Add(label42);
            groupBox11.Controls.Add(tbCustomTrainTypText);
            groupBox11.Controls.Add(tbCustomTrainTypSkratka);
            groupBox11.Controls.Add(label41);
            groupBox11.Controls.Add(cbCustomTrainTypDruh);
            groupBox11.Controls.Add(label40);
            groupBox11.Controls.Add(bCustomTrainTypDelete);
            groupBox11.Controls.Add(bCustomTrainTypAdd);
            groupBox11.Controls.Add(bCustomTrainTypEdit);
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
            // 
            // label41
            // 
            resources.ApplyResources(label41, "label41");
            label41.Name = "label41";
            // 
            // cbCustomTrainTypDruh
            // 
            cbCustomTrainTypDruh.DefaultStyle = true;
            cbCustomTrainTypDruh.DropDownBackColor = Color.White;
            cbCustomTrainTypDruh.DropDownSelectedRowBackColor = SystemColors.Highlight;
            cbCustomTrainTypDruh.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCustomTrainTypDruh.FormattingEnabled = true;
            cbCustomTrainTypDruh.Items.AddRange(new object[] { resources.GetString("cbCustomTrainTypDruh.Items"), resources.GetString("cbCustomTrainTypDruh.Items1"), resources.GetString("cbCustomTrainTypDruh.Items2"), resources.GetString("cbCustomTrainTypDruh.Items3") });
            resources.ApplyResources(cbCustomTrainTypDruh, "cbCustomTrainTypDruh");
            cbCustomTrainTypDruh.Name = "cbCustomTrainTypDruh";
            exComboBoxStyle1.ArrowColor = null;
            exComboBoxStyle1.BackColor = null;
            exComboBoxStyle1.BorderColor = null;
            exComboBoxStyle1.ButtonBackColor = null;
            exComboBoxStyle1.ButtonBorderColor = null;
            exComboBoxStyle1.ButtonRenderFirst = null;
            exComboBoxStyle1.ForeColor = null;
            cbCustomTrainTypDruh.StyleDisabled = exComboBoxStyle1;
            exComboBoxStyle2.ArrowColor = null;
            exComboBoxStyle2.BackColor = null;
            exComboBoxStyle2.BorderColor = null;
            exComboBoxStyle2.ButtonBackColor = null;
            exComboBoxStyle2.ButtonBorderColor = null;
            exComboBoxStyle2.ButtonRenderFirst = null;
            exComboBoxStyle2.ForeColor = null;
            cbCustomTrainTypDruh.StyleHighlight = exComboBoxStyle2;
            exComboBoxStyle3.ArrowColor = null;
            exComboBoxStyle3.BackColor = null;
            exComboBoxStyle3.BorderColor = null;
            exComboBoxStyle3.ButtonBackColor = null;
            exComboBoxStyle3.ButtonBorderColor = null;
            exComboBoxStyle3.ButtonRenderFirst = null;
            exComboBoxStyle3.ForeColor = null;
            cbCustomTrainTypDruh.StyleNormal = exComboBoxStyle3;
            exComboBoxStyle4.ArrowColor = null;
            exComboBoxStyle4.BackColor = null;
            exComboBoxStyle4.BorderColor = null;
            exComboBoxStyle4.ButtonBackColor = null;
            exComboBoxStyle4.ButtonBorderColor = null;
            exComboBoxStyle4.ButtonRenderFirst = null;
            exComboBoxStyle4.ForeColor = null;
            cbCustomTrainTypDruh.StyleSelected = exComboBoxStyle4;
            cbCustomTrainTypDruh.UseDarkScrollBar = false;
            // 
            // label40
            // 
            resources.ApplyResources(label40, "label40");
            label40.Name = "label40";
            // 
            // bCustomTrainTypDelete
            // 
            resources.ApplyResources(bCustomTrainTypDelete, "bCustomTrainTypDelete");
            bCustomTrainTypDelete.DefaultStyle = true;
            bCustomTrainTypDelete.Name = "bCustomTrainTypDelete";
            bCustomTrainTypDelete.UseVisualStyleBackColor = true;
            bCustomTrainTypDelete.Click += bCustomTrainTypDelete_Click;
            // 
            // bCustomTrainTypAdd
            // 
            resources.ApplyResources(bCustomTrainTypAdd, "bCustomTrainTypAdd");
            bCustomTrainTypAdd.DefaultStyle = true;
            bCustomTrainTypAdd.Name = "bCustomTrainTypAdd";
            bCustomTrainTypAdd.UseVisualStyleBackColor = true;
            bCustomTrainTypAdd.Click += bCustomTrainTypAdd_Click;
            // 
            // bCustomTrainTypEdit
            // 
            resources.ApplyResources(bCustomTrainTypEdit, "bCustomTrainTypEdit");
            bCustomTrainTypEdit.DefaultStyle = true;
            bCustomTrainTypEdit.Name = "bCustomTrainTypEdit";
            bCustomTrainTypEdit.UseVisualStyleBackColor = true;
            bCustomTrainTypEdit.Click += bCustomTrainTypEdit_Click;
            // 
            // listTrainTypes
            // 
            listTrainTypes.DisplayMember = "Name";
            listTrainTypes.FormattingEnabled = true;
            resources.ApplyResources(listTrainTypes, "listTrainTypes");
            listTrainTypes.Name = "listTrainTypes";
            listTrainTypes.SelectedIndexChanged += listTrainTypes_SelectedIndexChanged;
            // 
            // label14
            // 
            resources.ApplyResources(label14, "label14");
            label14.Name = "label14";
            // 
            // groupBox3
            // 
            groupBox3.BorderColor = Color.LightGray;
            groupBox3.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox3.BorderThickness = 1;
            groupBox3.Controls.Add(label20);
            groupBox3.Controls.Add(tbDefaultTrainTypText);
            groupBox3.Controls.Add(tbDefaultTrainTypSkratka);
            groupBox3.Controls.Add(label21);
            groupBox3.Controls.Add(cbDefTrainTypSkratka);
            groupBox3.Controls.Add(bDefTrainTypDelete);
            groupBox3.Controls.Add(bDefTrainTypEdit);
            groupBox3.Controls.Add(bDefTrainTypAdd);
            groupBox3.Controls.Add(label18);
            groupBox3.DefaultStyle = true;
            groupBox3.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox3, "groupBox3");
            groupBox3.Name = "groupBox3";
            groupBox3.TabStop = false;
            // 
            // label20
            // 
            resources.ApplyResources(label20, "label20");
            label20.Name = "label20";
            // 
            // tbDefaultTrainTypText
            // 
            tbDefaultTrainTypText.BorderColor = Color.DimGray;
            tbDefaultTrainTypText.BorderThickness = 1;
            tbDefaultTrainTypText.DefaultStyle = true;
            tbDefaultTrainTypText.DisabledBackColor = SystemColors.Control;
            tbDefaultTrainTypText.DisabledBorderColor = SystemColors.InactiveBorder;
            tbDefaultTrainTypText.DisabledForeColor = SystemColors.GrayText;
            tbDefaultTrainTypText.HighlightColor = SystemColors.Highlight;
            tbDefaultTrainTypText.HintForeColor = SystemColors.GrayText;
            tbDefaultTrainTypText.HintText = null;
            resources.ApplyResources(tbDefaultTrainTypText, "tbDefaultTrainTypText");
            tbDefaultTrainTypText.Name = "tbDefaultTrainTypText";
            // 
            // tbDefaultTrainTypSkratka
            // 
            tbDefaultTrainTypSkratka.BorderColor = Color.DimGray;
            tbDefaultTrainTypSkratka.BorderThickness = 1;
            tbDefaultTrainTypSkratka.DefaultStyle = true;
            tbDefaultTrainTypSkratka.DisabledBackColor = SystemColors.Control;
            tbDefaultTrainTypSkratka.DisabledBorderColor = SystemColors.InactiveBorder;
            tbDefaultTrainTypSkratka.DisabledForeColor = SystemColors.GrayText;
            tbDefaultTrainTypSkratka.HighlightColor = SystemColors.Highlight;
            tbDefaultTrainTypSkratka.HintForeColor = SystemColors.GrayText;
            tbDefaultTrainTypSkratka.HintText = null;
            resources.ApplyResources(tbDefaultTrainTypSkratka, "tbDefaultTrainTypSkratka");
            tbDefaultTrainTypSkratka.Name = "tbDefaultTrainTypSkratka";
            // 
            // label21
            // 
            resources.ApplyResources(label21, "label21");
            label21.Name = "label21";
            // 
            // cbDefTrainTypSkratka
            // 
            cbDefTrainTypSkratka.DefaultStyle = true;
            cbDefTrainTypSkratka.DropDownBackColor = Color.White;
            cbDefTrainTypSkratka.DropDownSelectedRowBackColor = SystemColors.Highlight;
            cbDefTrainTypSkratka.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDefTrainTypSkratka.FormattingEnabled = true;
            resources.ApplyResources(cbDefTrainTypSkratka, "cbDefTrainTypSkratka");
            cbDefTrainTypSkratka.Name = "cbDefTrainTypSkratka";
            exComboBoxStyle5.ArrowColor = null;
            exComboBoxStyle5.BackColor = null;
            exComboBoxStyle5.BorderColor = null;
            exComboBoxStyle5.ButtonBackColor = null;
            exComboBoxStyle5.ButtonBorderColor = null;
            exComboBoxStyle5.ButtonRenderFirst = null;
            exComboBoxStyle5.ForeColor = null;
            cbDefTrainTypSkratka.StyleDisabled = exComboBoxStyle5;
            exComboBoxStyle6.ArrowColor = null;
            exComboBoxStyle6.BackColor = null;
            exComboBoxStyle6.BorderColor = null;
            exComboBoxStyle6.ButtonBackColor = null;
            exComboBoxStyle6.ButtonBorderColor = null;
            exComboBoxStyle6.ButtonRenderFirst = null;
            exComboBoxStyle6.ForeColor = null;
            cbDefTrainTypSkratka.StyleHighlight = exComboBoxStyle6;
            exComboBoxStyle7.ArrowColor = null;
            exComboBoxStyle7.BackColor = null;
            exComboBoxStyle7.BorderColor = null;
            exComboBoxStyle7.ButtonBackColor = null;
            exComboBoxStyle7.ButtonBorderColor = null;
            exComboBoxStyle7.ButtonRenderFirst = null;
            exComboBoxStyle7.ForeColor = null;
            cbDefTrainTypSkratka.StyleNormal = exComboBoxStyle7;
            exComboBoxStyle8.ArrowColor = null;
            exComboBoxStyle8.BackColor = null;
            exComboBoxStyle8.BorderColor = null;
            exComboBoxStyle8.ButtonBackColor = null;
            exComboBoxStyle8.ButtonBorderColor = null;
            exComboBoxStyle8.ButtonRenderFirst = null;
            exComboBoxStyle8.ForeColor = null;
            cbDefTrainTypSkratka.StyleSelected = exComboBoxStyle8;
            cbDefTrainTypSkratka.UseDarkScrollBar = false;
            // 
            // bDefTrainTypDelete
            // 
            resources.ApplyResources(bDefTrainTypDelete, "bDefTrainTypDelete");
            bDefTrainTypDelete.DefaultStyle = true;
            bDefTrainTypDelete.Name = "bDefTrainTypDelete";
            bDefTrainTypDelete.UseVisualStyleBackColor = true;
            bDefTrainTypDelete.Click += bDefTrainTypDelete_Click;
            // 
            // bDefTrainTypEdit
            // 
            resources.ApplyResources(bDefTrainTypEdit, "bDefTrainTypEdit");
            bDefTrainTypEdit.DefaultStyle = true;
            bDefTrainTypEdit.Name = "bDefTrainTypEdit";
            bDefTrainTypEdit.UseVisualStyleBackColor = true;
            bDefTrainTypEdit.Click += bDefTrainTypEdit_Click;
            // 
            // bDefTrainTypAdd
            // 
            resources.ApplyResources(bDefTrainTypAdd, "bDefTrainTypAdd");
            bDefTrainTypAdd.DefaultStyle = true;
            bDefTrainTypAdd.Name = "bDefTrainTypAdd";
            bDefTrainTypAdd.UseVisualStyleBackColor = true;
            bDefTrainTypAdd.Click += bDefTrainTypAdd_Click;
            // 
            // label18
            // 
            resources.ApplyResources(label18, "label18");
            label18.Name = "label18";
            // 
            // tpAudio
            // 
            tpAudio.BackColor = Color.Transparent;
            tpAudio.Controls.Add(listAudio);
            tpAudio.Controls.Add(label1);
            tpAudio.Controls.Add(groupBox2);
            resources.ApplyResources(tpAudio, "tpAudio");
            tpAudio.Name = "tpAudio";
            // 
            // listAudio
            // 
            listAudio.DisplayMember = "Name";
            listAudio.FormattingEnabled = true;
            resources.ApplyResources(listAudio, "listAudio");
            listAudio.Name = "listAudio";
            listAudio.SelectedIndexChanged += listAudio_SelectedIndexChanged;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // groupBox2
            // 
            groupBox2.BorderColor = Color.LightGray;
            groupBox2.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox2.BorderThickness = 1;
            groupBox2.Controls.Add(label26);
            groupBox2.Controls.Add(label25);
            groupBox2.Controls.Add(tbNode);
            groupBox2.Controls.Add(tbExchange);
            groupBox2.Controls.Add(tbAmplifier);
            groupBox2.Controls.Add(tbInputLine);
            groupBox2.Controls.Add(label24);
            groupBox2.Controls.Add(label23);
            groupBox2.Controls.Add(label22);
            groupBox2.Controls.Add(tbSoundCard);
            groupBox2.Controls.Add(tbAudioName);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(tbAudioNazovFronta);
            groupBox2.Controls.Add(tbAudioNazovSkratka);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(cbCustomOnly);
            groupBox2.Controls.Add(cbAudioStanica);
            groupBox2.Controls.Add(bAudioDelete);
            groupBox2.Controls.Add(bAudioEdit);
            groupBox2.Controls.Add(bAudioAdd);
            groupBox2.Controls.Add(label2);
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
            // label25
            // 
            resources.ApplyResources(label25, "label25");
            label25.Name = "label25";
            // 
            // tbNode
            // 
            tbNode.BorderColor = Color.DimGray;
            tbNode.BorderThickness = 1;
            tbNode.DefaultStyle = true;
            tbNode.DisabledBackColor = SystemColors.Control;
            tbNode.DisabledBorderColor = SystemColors.InactiveBorder;
            tbNode.DisabledForeColor = SystemColors.GrayText;
            tbNode.HighlightColor = SystemColors.Highlight;
            tbNode.HintForeColor = SystemColors.GrayText;
            tbNode.HintText = null;
            resources.ApplyResources(tbNode, "tbNode");
            tbNode.Name = "tbNode";
            // 
            // tbExchange
            // 
            tbExchange.BorderColor = Color.DimGray;
            tbExchange.BorderThickness = 1;
            tbExchange.DefaultStyle = true;
            tbExchange.DisabledBackColor = SystemColors.Control;
            tbExchange.DisabledBorderColor = SystemColors.InactiveBorder;
            tbExchange.DisabledForeColor = SystemColors.GrayText;
            tbExchange.HighlightColor = SystemColors.Highlight;
            tbExchange.HintForeColor = SystemColors.GrayText;
            tbExchange.HintText = "číslo posielané ovládaču TORNZ";
            resources.ApplyResources(tbExchange, "tbExchange");
            tbExchange.Name = "tbExchange";
            // 
            // tbAmplifier
            // 
            tbAmplifier.BorderColor = Color.DimGray;
            tbAmplifier.BorderThickness = 1;
            tbAmplifier.DefaultStyle = true;
            tbAmplifier.DisabledBackColor = SystemColors.Control;
            tbAmplifier.DisabledBorderColor = SystemColors.InactiveBorder;
            tbAmplifier.DisabledForeColor = SystemColors.GrayText;
            tbAmplifier.HighlightColor = SystemColors.Highlight;
            tbAmplifier.HintForeColor = SystemColors.GrayText;
            tbAmplifier.HintText = null;
            resources.ApplyResources(tbAmplifier, "tbAmplifier");
            tbAmplifier.Name = "tbAmplifier";
            // 
            // tbInputLine
            // 
            tbInputLine.BorderColor = Color.DimGray;
            tbInputLine.BorderThickness = 1;
            tbInputLine.DefaultStyle = true;
            tbInputLine.DisabledBackColor = SystemColors.Control;
            tbInputLine.DisabledBorderColor = SystemColors.InactiveBorder;
            tbInputLine.DisabledForeColor = SystemColors.GrayText;
            tbInputLine.HighlightColor = SystemColors.Highlight;
            tbInputLine.HintForeColor = SystemColors.GrayText;
            tbInputLine.HintText = "číslo/názov vstupu (Line In)";
            resources.ApplyResources(tbInputLine, "tbInputLine");
            tbInputLine.Name = "tbInputLine";
            // 
            // label24
            // 
            resources.ApplyResources(label24, "label24");
            label24.Name = "label24";
            // 
            // label23
            // 
            resources.ApplyResources(label23, "label23");
            label23.Name = "label23";
            // 
            // label22
            // 
            resources.ApplyResources(label22, "label22");
            label22.Name = "label22";
            // 
            // tbSoundCard
            // 
            tbSoundCard.BorderColor = Color.DimGray;
            tbSoundCard.BorderThickness = 1;
            tbSoundCard.DefaultStyle = true;
            tbSoundCard.DisabledBackColor = SystemColors.Control;
            tbSoundCard.DisabledBorderColor = SystemColors.InactiveBorder;
            tbSoundCard.DisabledForeColor = SystemColors.GrayText;
            tbSoundCard.HighlightColor = SystemColors.Highlight;
            tbSoundCard.HintForeColor = SystemColors.GrayText;
            tbSoundCard.HintText = "mixér alebo p.č. (Realtek HD:Wave)";
            resources.ApplyResources(tbSoundCard, "tbSoundCard");
            tbSoundCard.Name = "tbSoundCard";
            // 
            // tbAudioName
            // 
            tbAudioName.BorderColor = Color.DimGray;
            tbAudioName.BorderThickness = 1;
            tbAudioName.DefaultStyle = true;
            tbAudioName.DisabledBackColor = SystemColors.Control;
            tbAudioName.DisabledBorderColor = SystemColors.InactiveBorder;
            tbAudioName.DisabledForeColor = SystemColors.GrayText;
            tbAudioName.HighlightColor = SystemColors.Highlight;
            tbAudioName.HintForeColor = SystemColors.GrayText;
            tbAudioName.HintText = null;
            resources.ApplyResources(tbAudioName, "tbAudioName");
            tbAudioName.Name = "tbAudioName";
            tbAudioName.TextChanged += tbAudioName_TextChanged;
            // 
            // label8
            // 
            resources.ApplyResources(label8, "label8");
            label8.Name = "label8";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // tbAudioNazovFronta
            // 
            tbAudioNazovFronta.BorderColor = Color.DimGray;
            tbAudioNazovFronta.BorderThickness = 1;
            tbAudioNazovFronta.DefaultStyle = true;
            tbAudioNazovFronta.DisabledBackColor = SystemColors.Control;
            tbAudioNazovFronta.DisabledBorderColor = SystemColors.InactiveBorder;
            tbAudioNazovFronta.DisabledForeColor = SystemColors.GrayText;
            tbAudioNazovFronta.HighlightColor = SystemColors.Highlight;
            tbAudioNazovFronta.HintForeColor = SystemColors.GrayText;
            tbAudioNazovFronta.HintText = null;
            resources.ApplyResources(tbAudioNazovFronta, "tbAudioNazovFronta");
            tbAudioNazovFronta.Name = "tbAudioNazovFronta";
            // 
            // tbAudioNazovSkratka
            // 
            tbAudioNazovSkratka.BorderColor = Color.DimGray;
            tbAudioNazovSkratka.BorderThickness = 1;
            tbAudioNazovSkratka.DefaultStyle = true;
            tbAudioNazovSkratka.DisabledBackColor = SystemColors.Control;
            tbAudioNazovSkratka.DisabledBorderColor = SystemColors.InactiveBorder;
            tbAudioNazovSkratka.DisabledForeColor = SystemColors.GrayText;
            tbAudioNazovSkratka.HighlightColor = SystemColors.Highlight;
            tbAudioNazovSkratka.HintForeColor = SystemColors.GrayText;
            tbAudioNazovSkratka.HintText = null;
            resources.ApplyResources(tbAudioNazovSkratka, "tbAudioNazovSkratka");
            tbAudioNazovSkratka.Name = "tbAudioNazovSkratka";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // cbCustomOnly
            // 
            resources.ApplyResources(cbCustomOnly, "cbCustomOnly");
            cbCustomOnly.BorderColor = Color.Black;
            cbCustomOnly.BoxBackColor = Color.White;
            cbCustomOnly.DefaultStyle = true;
            cbCustomOnly.DisabledForeColor = Color.DimGray;
            cbCustomOnly.HighlightColor = SystemColors.Highlight;
            cbCustomOnly.MarkColor = Color.Black;
            cbCustomOnly.Name = "cbCustomOnly";
            cbCustomOnly.UseVisualStyleBackColor = true;
            cbCustomOnly.CheckedChanged += cbCustomOnly_CheckedChanged;
            // 
            // cbAudioStanica
            // 
            cbAudioStanica.DefaultStyle = true;
            cbAudioStanica.DropDownBackColor = Color.White;
            cbAudioStanica.DropDownSelectedRowBackColor = SystemColors.Highlight;
            cbAudioStanica.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAudioStanica.FormattingEnabled = true;
            resources.ApplyResources(cbAudioStanica, "cbAudioStanica");
            cbAudioStanica.Name = "cbAudioStanica";
            exComboBoxStyle9.ArrowColor = null;
            exComboBoxStyle9.BackColor = null;
            exComboBoxStyle9.BorderColor = null;
            exComboBoxStyle9.ButtonBackColor = null;
            exComboBoxStyle9.ButtonBorderColor = null;
            exComboBoxStyle9.ButtonRenderFirst = null;
            exComboBoxStyle9.ForeColor = null;
            cbAudioStanica.StyleDisabled = exComboBoxStyle9;
            exComboBoxStyle10.ArrowColor = null;
            exComboBoxStyle10.BackColor = null;
            exComboBoxStyle10.BorderColor = null;
            exComboBoxStyle10.ButtonBackColor = null;
            exComboBoxStyle10.ButtonBorderColor = null;
            exComboBoxStyle10.ButtonRenderFirst = null;
            exComboBoxStyle10.ForeColor = null;
            cbAudioStanica.StyleHighlight = exComboBoxStyle10;
            exComboBoxStyle11.ArrowColor = null;
            exComboBoxStyle11.BackColor = null;
            exComboBoxStyle11.BorderColor = null;
            exComboBoxStyle11.ButtonBackColor = null;
            exComboBoxStyle11.ButtonBorderColor = null;
            exComboBoxStyle11.ButtonRenderFirst = null;
            exComboBoxStyle11.ForeColor = null;
            cbAudioStanica.StyleNormal = exComboBoxStyle11;
            exComboBoxStyle12.ArrowColor = null;
            exComboBoxStyle12.BackColor = null;
            exComboBoxStyle12.BorderColor = null;
            exComboBoxStyle12.ButtonBackColor = null;
            exComboBoxStyle12.ButtonBorderColor = null;
            exComboBoxStyle12.ButtonRenderFirst = null;
            exComboBoxStyle12.ForeColor = null;
            cbAudioStanica.StyleSelected = exComboBoxStyle12;
            cbAudioStanica.UseDarkScrollBar = false;
            // 
            // bAudioDelete
            // 
            resources.ApplyResources(bAudioDelete, "bAudioDelete");
            bAudioDelete.DefaultStyle = true;
            bAudioDelete.Name = "bAudioDelete";
            bAudioDelete.UseVisualStyleBackColor = true;
            bAudioDelete.Click += bAudioDelete_Click;
            // 
            // bAudioEdit
            // 
            resources.ApplyResources(bAudioEdit, "bAudioEdit");
            bAudioEdit.DefaultStyle = true;
            bAudioEdit.Name = "bAudioEdit";
            bAudioEdit.UseVisualStyleBackColor = true;
            bAudioEdit.Click += bAudioEdit_Click;
            // 
            // bAudioAdd
            // 
            resources.ApplyResources(bAudioAdd, "bAudioAdd");
            bAudioAdd.DefaultStyle = true;
            bAudioAdd.Name = "bAudioAdd";
            bAudioAdd.UseVisualStyleBackColor = true;
            bAudioAdd.Click += bAudioAdd_Click;
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // bSave
            // 
            resources.ApplyResources(bSave, "bSave");
            bSave.DefaultStyle = true;
            bSave.Name = "bSave";
            bSave.UseVisualStyleBackColor = true;
            bSave.Click += bSave_Click;
            // 
            // FGlobalSettings
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
            Name = "FGlobalSettings";
            ShowInTaskbar = false;
            HelpButtonClicked += FGlobalSettings_HelpButtonClicked;
            FormClosed += FGlobalSettings_FormClosed;
            Load += FGlobalSettings_Load;
            tabControl.ResumeLayout(false);
            tpGrafikony.ResumeLayout(false);
            tpGrafikony.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((ISupportInitialize)pbColor).EndInit();
            ((ISupportInitialize)nudHlaseniePort).EndInit();
            ((ISupportInitialize)nudTabPort).EndInit();
            tpJazyky.ResumeLayout(false);
            tpJazyky.PerformLayout();
            ((ISupportInitialize)dgvLanguagesRawBank).EndInit();
            ((ISupportInitialize)languageBindingSource).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            tpMeskania.ResumeLayout(false);
            tpMeskania.PerformLayout();
            groupBox10.ResumeLayout(false);
            groupBox10.PerformLayout();
            ((ISupportInitialize)nudMeskanie).EndInit();
            tpTrainTypes.ResumeLayout(false);
            tpTrainTypes.PerformLayout();
            groupBox11.ResumeLayout(false);
            groupBox11.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            tpAudio.ResumeLayout(false);
            tpAudio.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private ExTabControl tabControl;
        private System.Windows.Forms.TabPage tpJazyky;
        private ExControls.ExButton bSave;
        private System.Windows.Forms.ListBox listLanguages;
        private System.Windows.Forms.Label label15;
        private ExGroupBox groupBox4;
        private ExTextBox tbLanguageSkratka;
        private System.Windows.Forms.Label label17;
        private ExCheckBox cbIsBasic;
        private ExTextBox tbLanguageName;
        private ExControls.ExButton bLanguageRemove;
        private ExControls.ExButton bLanguageEdit;
        private ExControls.ExButton bLanguageAdd;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TabPage tpGrafikony;
        private System.Windows.Forms.ListBox listGrafikony;
        private System.Windows.Forms.Label label3;
        private ExGroupBox groupBox1;
        private ExTextBox tbGrafikonStanica;
        private ExControls.ExButton bGrafikonDelete;
        private System.Windows.Forms.Label label7;
        private ExTextBox tbGrafikonPath;
        private System.Windows.Forms.Label label9;
        private ExTextBox tbGrafikonObdobie;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tpMeskania;
        private System.Windows.Forms.ListBox listMeskania;
        private System.Windows.Forms.Label label38;
        private ExGroupBox groupBox10;
        private ExNumericUpDown nudMeskanie;
        private ExControls.ExButton bMeskanieDelete;
        private ExControls.ExButton bMeskanieEdit;
        private ExControls.ExButton bMeskanieAdd;
        private System.Windows.Forms.Label label39;
        private ExNumericUpDown nudHlaseniePort;
        private ExNumericUpDown nudTabPort;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ColorDialog colorDialogFarba;
        private System.Windows.Forms.Label label13;
        private ExControls.ExButton bGrafikonEdit;
        private ExControls.ExButton bEditColor;
        private System.Windows.Forms.PictureBox pbColor;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tpTrainTypes;
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
        private System.Windows.Forms.Label label14;
        private ExGroupBox groupBox3;
        private ExComboBox cbDefTrainTypSkratka;
        private ExControls.ExButton bDefTrainTypDelete;
        private ExControls.ExButton bDefTrainTypEdit;
        private ExControls.ExButton bDefTrainTypAdd;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TabPage tpAudio;
        private System.Windows.Forms.ListBox listAudio;
        private System.Windows.Forms.Label label1;
        private ExGroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private ExTextBox tbAudioNazovFronta;
        private ExTextBox tbAudioNazovSkratka;
        private System.Windows.Forms.Label label5;
        private ExCheckBox cbCustomOnly;
        private ExComboBox cbAudioStanica;
        private ExControls.ExButton bAudioDelete;
        private ExControls.ExButton bAudioEdit;
        private ExControls.ExButton bAudioAdd;
        private System.Windows.Forms.Label label2;
        private ExTextBox tbAudioName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dgvLanguagesRawBank;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.BindingSource languageBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fyzBankNameDataGridViewTextBoxColumn;
        private Label label20;
        private ExTextBox tbDefaultTrainTypText;
        private ExTextBox tbDefaultTrainTypSkratka;
        private Label label21;
        private Label label22;
        private ExTextBox tbSoundCard;
        private Label label26;
        private Label label25;
        private ExTextBox tbNode;
        private ExTextBox tbExchange;
        private ExTextBox tbAmplifier;
        private ExTextBox tbInputLine;
        private Label label24;
        private Label label23;
    }
}