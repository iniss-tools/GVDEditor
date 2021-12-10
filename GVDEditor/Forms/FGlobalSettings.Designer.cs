using ExControls;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FGlobalSettings));
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
            this.tabControl = new ExControls.ExTabControl();
            this.tpGrafikony = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.listGrafikony = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new ExControls.ExGroupBox();
            this.bGrafikonEdit = new ExControls.ExButton();
            this.bEditColor = new ExControls.ExButton();
            this.pbColor = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.nudHlaseniePort = new ExControls.ExNumericUpDown();
            this.nudTabPort = new ExControls.ExNumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbGrafikonPath = new ExControls.ExTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbGrafikonObdobie = new ExControls.ExTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbGrafikonStanica = new ExControls.ExTextBox();
            this.bGrafikonDelete = new ExControls.ExButton();
            this.label7 = new System.Windows.Forms.Label();
            this.tpJazyky = new System.Windows.Forms.TabPage();
            this.dgvLanguagesRawBank = new System.Windows.Forms.DataGridView();
            this.keyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fyzBankNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.languageBindingSource = new System.Windows.Forms.BindingSource();
            this.label19 = new System.Windows.Forms.Label();
            this.listLanguages = new System.Windows.Forms.ListBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox4 = new ExControls.ExGroupBox();
            this.tbLanguageSkratka = new ExControls.ExTextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cbIsBasic = new ExControls.ExCheckBox();
            this.tbLanguageName = new ExControls.ExTextBox();
            this.bLanguageRemove = new ExControls.ExButton();
            this.bLanguageEdit = new ExControls.ExButton();
            this.bLanguageAdd = new ExControls.ExButton();
            this.label16 = new System.Windows.Forms.Label();
            this.tpMeskania = new System.Windows.Forms.TabPage();
            this.listMeskania = new System.Windows.Forms.ListBox();
            this.label38 = new System.Windows.Forms.Label();
            this.groupBox10 = new ExControls.ExGroupBox();
            this.nudMeskanie = new ExControls.ExNumericUpDown();
            this.bMeskanieDelete = new ExControls.ExButton();
            this.bMeskanieEdit = new ExControls.ExButton();
            this.bMeskanieAdd = new ExControls.ExButton();
            this.label39 = new System.Windows.Forms.Label();
            this.tpTrainTypes = new System.Windows.Forms.TabPage();
            this.groupBox11 = new ExControls.ExGroupBox();
            this.label42 = new System.Windows.Forms.Label();
            this.tbCustomTrainTypText = new ExControls.ExTextBox();
            this.tbCustomTrainTypSkratka = new ExControls.ExTextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.cbCustomTrainTypDruh = new ExControls.ExComboBox();
            this.label40 = new System.Windows.Forms.Label();
            this.bCustomTrainTypDelete = new ExControls.ExButton();
            this.bCustomTrainTypAdd = new ExControls.ExButton();
            this.bCustomTrainTypEdit = new ExControls.ExButton();
            this.listTrainTypes = new System.Windows.Forms.ListBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox3 = new ExControls.ExGroupBox();
            this.cbDefTrainTypSkratka = new ExControls.ExComboBox();
            this.bDefTrainTypDelete = new ExControls.ExButton();
            this.bDefTrainTypEdit = new ExControls.ExButton();
            this.bDefTrainTypAdd = new ExControls.ExButton();
            this.label18 = new System.Windows.Forms.Label();
            this.tpAudio = new System.Windows.Forms.TabPage();
            this.listAudio = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new ExControls.ExGroupBox();
            this.tbAudioName = new ExControls.ExTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbAudioNazovFronta = new ExControls.ExTextBox();
            this.tbAudioNazovSkratka = new ExControls.ExTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbCustomOnly = new ExControls.ExCheckBox();
            this.cbAudioStanica = new ExControls.ExComboBox();
            this.bAudioDelete = new ExControls.ExButton();
            this.bAudioEdit = new ExControls.ExButton();
            this.bAudioAdd = new ExControls.ExButton();
            this.label2 = new System.Windows.Forms.Label();
            this.bSave = new ExControls.ExButton();
            this.colorDialogFarba = new System.Windows.Forms.ColorDialog();
            this.tabControl.SuspendLayout();
            this.tpGrafikony.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHlaseniePort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTabPort)).BeginInit();
            this.tpJazyky.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLanguagesRawBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.languageBindingSource)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tpMeskania.SuspendLayout();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeskanie)).BeginInit();
            this.tpTrainTypes.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tpAudio.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpGrafikony);
            this.tabControl.Controls.Add(this.tpJazyky);
            this.tabControl.Controls.Add(this.tpMeskania);
            this.tabControl.Controls.Add(this.tpTrainTypes);
            this.tabControl.Controls.Add(this.tpAudio);
            this.tabControl.HeaderBackColor = System.Drawing.Color.LightGray;
            this.tabControl.HighlightBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // tpGrafikony
            // 
            this.tpGrafikony.BackColor = System.Drawing.Color.Transparent;
            this.tpGrafikony.Controls.Add(this.label13);
            this.tpGrafikony.Controls.Add(this.listGrafikony);
            this.tpGrafikony.Controls.Add(this.label3);
            this.tpGrafikony.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tpGrafikony, "tpGrafikony");
            this.tpGrafikony.Name = "tpGrafikony";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // listGrafikony
            // 
            this.listGrafikony.FormattingEnabled = true;
            resources.ApplyResources(this.listGrafikony, "listGrafikony");
            this.listGrafikony.Name = "listGrafikony";
            this.listGrafikony.SelectedIndexChanged += new System.EventHandler(this.listGrafikony_SelectedIndexChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bGrafikonEdit);
            this.groupBox1.Controls.Add(this.bEditColor);
            this.groupBox1.Controls.Add(this.pbColor);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.nudHlaseniePort);
            this.groupBox1.Controls.Add(this.nudTabPort);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tbGrafikonPath);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tbGrafikonObdobie);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbGrafikonStanica);
            this.groupBox1.Controls.Add(this.bGrafikonDelete);
            this.groupBox1.Controls.Add(this.label7);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // bGrafikonEdit
            // 
            resources.ApplyResources(this.bGrafikonEdit, "bGrafikonEdit");
            this.bGrafikonEdit.Name = "bGrafikonEdit";
            this.bGrafikonEdit.UseVisualStyleBackColor = true;
            this.bGrafikonEdit.Click += new System.EventHandler(this.bGrafikonEdit_Click);
            // 
            // bEditColor
            // 
            resources.ApplyResources(this.bEditColor, "bEditColor");
            this.bEditColor.Name = "bEditColor";
            this.bEditColor.UseVisualStyleBackColor = true;
            this.bEditColor.Click += new System.EventHandler(this.bEditColor_Click);
            // 
            // pbColor
            // 
            this.pbColor.BackColor = System.Drawing.Color.White;
            this.pbColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pbColor, "pbColor");
            this.pbColor.Name = "pbColor";
            this.pbColor.TabStop = false;
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // nudHlaseniePort
            // 
            this.nudHlaseniePort.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudHlaseniePort, "nudHlaseniePort");
            this.nudHlaseniePort.Name = "nudHlaseniePort";
            this.nudHlaseniePort.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            // 
            // nudTabPort
            // 
            this.nudTabPort.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudTabPort, "nudTabPort");
            this.nudTabPort.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudTabPort.Name = "nudTabPort";
            this.nudTabPort.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // tbGrafikonPath
            // 
            this.tbGrafikonPath.BorderColor = System.Drawing.Color.DimGray;
            this.tbGrafikonPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbGrafikonPath.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbGrafikonPath.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbGrafikonPath.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbGrafikonPath.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbGrafikonPath.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbGrafikonPath.HintText = null;
            resources.ApplyResources(this.tbGrafikonPath, "tbGrafikonPath");
            this.tbGrafikonPath.Name = "tbGrafikonPath";
            this.tbGrafikonPath.ReadOnly = true;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // tbGrafikonObdobie
            // 
            this.tbGrafikonObdobie.BorderColor = System.Drawing.Color.DimGray;
            this.tbGrafikonObdobie.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbGrafikonObdobie.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbGrafikonObdobie.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbGrafikonObdobie.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbGrafikonObdobie.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbGrafikonObdobie.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbGrafikonObdobie.HintText = null;
            resources.ApplyResources(this.tbGrafikonObdobie, "tbGrafikonObdobie");
            this.tbGrafikonObdobie.Name = "tbGrafikonObdobie";
            this.tbGrafikonObdobie.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tbGrafikonStanica
            // 
            this.tbGrafikonStanica.BorderColor = System.Drawing.Color.DimGray;
            this.tbGrafikonStanica.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbGrafikonStanica.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbGrafikonStanica.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbGrafikonStanica.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbGrafikonStanica.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbGrafikonStanica.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbGrafikonStanica.HintText = null;
            resources.ApplyResources(this.tbGrafikonStanica, "tbGrafikonStanica");
            this.tbGrafikonStanica.Name = "tbGrafikonStanica";
            this.tbGrafikonStanica.ReadOnly = true;
            // 
            // bGrafikonDelete
            // 
            resources.ApplyResources(this.bGrafikonDelete, "bGrafikonDelete");
            this.bGrafikonDelete.Name = "bGrafikonDelete";
            this.bGrafikonDelete.UseVisualStyleBackColor = true;
            this.bGrafikonDelete.Click += new System.EventHandler(this.bGrafikonDelete_Click);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // tpJazyky
            // 
            this.tpJazyky.BackColor = System.Drawing.Color.Transparent;
            this.tpJazyky.Controls.Add(this.dgvLanguagesRawBank);
            this.tpJazyky.Controls.Add(this.label19);
            this.tpJazyky.Controls.Add(this.listLanguages);
            this.tpJazyky.Controls.Add(this.label15);
            this.tpJazyky.Controls.Add(this.groupBox4);
            resources.ApplyResources(this.tpJazyky, "tpJazyky");
            this.tpJazyky.Name = "tpJazyky";
            // 
            // dgvLanguagesRawBank
            // 
            this.dgvLanguagesRawBank.AllowUserToAddRows = false;
            this.dgvLanguagesRawBank.AllowUserToDeleteRows = false;
            this.dgvLanguagesRawBank.AllowUserToResizeRows = false;
            this.dgvLanguagesRawBank.AutoGenerateColumns = false;
            this.dgvLanguagesRawBank.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLanguagesRawBank.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.keyDataGridViewTextBoxColumn,
            this.fyzBankNameDataGridViewTextBoxColumn});
            this.dgvLanguagesRawBank.DataSource = this.languageBindingSource;
            resources.ApplyResources(this.dgvLanguagesRawBank, "dgvLanguagesRawBank");
            this.dgvLanguagesRawBank.Name = "dgvLanguagesRawBank";
            this.dgvLanguagesRawBank.ReadOnly = true;
            this.dgvLanguagesRawBank.RowHeadersVisible = false;
            this.dgvLanguagesRawBank.RowTemplate.Height = 24;
            // 
            // keyDataGridViewTextBoxColumn
            // 
            this.keyDataGridViewTextBoxColumn.DataPropertyName = "Key";
            resources.ApplyResources(this.keyDataGridViewTextBoxColumn, "keyDataGridViewTextBoxColumn");
            this.keyDataGridViewTextBoxColumn.Name = "keyDataGridViewTextBoxColumn";
            this.keyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fyzBankNameDataGridViewTextBoxColumn
            // 
            this.fyzBankNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fyzBankNameDataGridViewTextBoxColumn.DataPropertyName = "FyzBankName";
            resources.ApplyResources(this.fyzBankNameDataGridViewTextBoxColumn, "fyzBankNameDataGridViewTextBoxColumn");
            this.fyzBankNameDataGridViewTextBoxColumn.Name = "fyzBankNameDataGridViewTextBoxColumn";
            this.fyzBankNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // languageBindingSource
            // 
            this.languageBindingSource.DataSource = typeof(GVDEditor.Entities.Language);
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // listLanguages
            // 
            this.listLanguages.FormattingEnabled = true;
            resources.ApplyResources(this.listLanguages, "listLanguages");
            this.listLanguages.Name = "listLanguages";
            this.listLanguages.SelectedIndexChanged += new System.EventHandler(this.listLanguages_SelectedIndexChanged);
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbLanguageSkratka);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.cbIsBasic);
            this.groupBox4.Controls.Add(this.tbLanguageName);
            this.groupBox4.Controls.Add(this.bLanguageRemove);
            this.groupBox4.Controls.Add(this.bLanguageEdit);
            this.groupBox4.Controls.Add(this.bLanguageAdd);
            this.groupBox4.Controls.Add(this.label16);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // tbLanguageSkratka
            // 
            this.tbLanguageSkratka.BorderColor = System.Drawing.Color.DimGray;
            this.tbLanguageSkratka.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbLanguageSkratka.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbLanguageSkratka.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbLanguageSkratka.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbLanguageSkratka.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbLanguageSkratka.HintText = null;
            resources.ApplyResources(this.tbLanguageSkratka, "tbLanguageSkratka");
            this.tbLanguageSkratka.Name = "tbLanguageSkratka";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // cbIsBasic
            // 
            resources.ApplyResources(this.cbIsBasic, "cbIsBasic");
            this.cbIsBasic.BoxBackColor = System.Drawing.Color.White;
            this.cbIsBasic.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbIsBasic.Name = "cbIsBasic";
            this.cbIsBasic.UseVisualStyleBackColor = true;
            // 
            // tbLanguageName
            // 
            this.tbLanguageName.BorderColor = System.Drawing.Color.DimGray;
            this.tbLanguageName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbLanguageName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbLanguageName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbLanguageName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbLanguageName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbLanguageName.HintText = null;
            resources.ApplyResources(this.tbLanguageName, "tbLanguageName");
            this.tbLanguageName.Name = "tbLanguageName";
            // 
            // bLanguageRemove
            // 
            resources.ApplyResources(this.bLanguageRemove, "bLanguageRemove");
            this.bLanguageRemove.Name = "bLanguageRemove";
            this.bLanguageRemove.UseVisualStyleBackColor = true;
            this.bLanguageRemove.Click += new System.EventHandler(this.bLanguageRemove_Click);
            // 
            // bLanguageEdit
            // 
            resources.ApplyResources(this.bLanguageEdit, "bLanguageEdit");
            this.bLanguageEdit.Name = "bLanguageEdit";
            this.bLanguageEdit.UseVisualStyleBackColor = true;
            this.bLanguageEdit.Click += new System.EventHandler(this.bLanguageEdit_Click);
            // 
            // bLanguageAdd
            // 
            resources.ApplyResources(this.bLanguageAdd, "bLanguageAdd");
            this.bLanguageAdd.Name = "bLanguageAdd";
            this.bLanguageAdd.UseVisualStyleBackColor = true;
            this.bLanguageAdd.Click += new System.EventHandler(this.bLanguageAdd_Click);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // tpMeskania
            // 
            this.tpMeskania.BackColor = System.Drawing.Color.Transparent;
            this.tpMeskania.Controls.Add(this.listMeskania);
            this.tpMeskania.Controls.Add(this.label38);
            this.tpMeskania.Controls.Add(this.groupBox10);
            resources.ApplyResources(this.tpMeskania, "tpMeskania");
            this.tpMeskania.Name = "tpMeskania";
            // 
            // listMeskania
            // 
            this.listMeskania.FormattingEnabled = true;
            resources.ApplyResources(this.listMeskania, "listMeskania");
            this.listMeskania.Name = "listMeskania";
            this.listMeskania.SelectedIndexChanged += new System.EventHandler(this.listMeskania_SelectedIndexChanged);
            // 
            // label38
            // 
            resources.ApplyResources(this.label38, "label38");
            this.label38.Name = "label38";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.nudMeskanie);
            this.groupBox10.Controls.Add(this.bMeskanieDelete);
            this.groupBox10.Controls.Add(this.bMeskanieEdit);
            this.groupBox10.Controls.Add(this.bMeskanieAdd);
            this.groupBox10.Controls.Add(this.label39);
            resources.ApplyResources(this.groupBox10, "groupBox10");
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.TabStop = false;
            // 
            // nudMeskanie
            // 
            this.nudMeskanie.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.nudMeskanie.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            resources.ApplyResources(this.nudMeskanie, "nudMeskanie");
            this.nudMeskanie.Maximum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.nudMeskanie.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudMeskanie.Name = "nudMeskanie";
            this.nudMeskanie.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            this.nudMeskanie.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // bMeskanieDelete
            // 
            resources.ApplyResources(this.bMeskanieDelete, "bMeskanieDelete");
            this.bMeskanieDelete.Name = "bMeskanieDelete";
            this.bMeskanieDelete.UseVisualStyleBackColor = true;
            this.bMeskanieDelete.Click += new System.EventHandler(this.bMeskanieDelete_Click);
            // 
            // bMeskanieEdit
            // 
            resources.ApplyResources(this.bMeskanieEdit, "bMeskanieEdit");
            this.bMeskanieEdit.Name = "bMeskanieEdit";
            this.bMeskanieEdit.UseVisualStyleBackColor = true;
            this.bMeskanieEdit.Click += new System.EventHandler(this.bMeskanieEdit_Click);
            // 
            // bMeskanieAdd
            // 
            resources.ApplyResources(this.bMeskanieAdd, "bMeskanieAdd");
            this.bMeskanieAdd.Name = "bMeskanieAdd";
            this.bMeskanieAdd.UseVisualStyleBackColor = true;
            this.bMeskanieAdd.Click += new System.EventHandler(this.bMeskanieAdd_Click);
            // 
            // label39
            // 
            resources.ApplyResources(this.label39, "label39");
            this.label39.Name = "label39";
            // 
            // tpTrainTypes
            // 
            this.tpTrainTypes.BackColor = System.Drawing.Color.Transparent;
            this.tpTrainTypes.Controls.Add(this.groupBox11);
            this.tpTrainTypes.Controls.Add(this.listTrainTypes);
            this.tpTrainTypes.Controls.Add(this.label14);
            this.tpTrainTypes.Controls.Add(this.groupBox3);
            resources.ApplyResources(this.tpTrainTypes, "tpTrainTypes");
            this.tpTrainTypes.Name = "tpTrainTypes";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label42);
            this.groupBox11.Controls.Add(this.tbCustomTrainTypText);
            this.groupBox11.Controls.Add(this.tbCustomTrainTypSkratka);
            this.groupBox11.Controls.Add(this.label41);
            this.groupBox11.Controls.Add(this.cbCustomTrainTypDruh);
            this.groupBox11.Controls.Add(this.label40);
            this.groupBox11.Controls.Add(this.bCustomTrainTypDelete);
            this.groupBox11.Controls.Add(this.bCustomTrainTypAdd);
            this.groupBox11.Controls.Add(this.bCustomTrainTypEdit);
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
            // cbCustomTrainTypDruh
            // 
            this.cbCustomTrainTypDruh.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbCustomTrainTypDruh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCustomTrainTypDruh.FormattingEnabled = true;
            this.cbCustomTrainTypDruh.Items.AddRange(new object[] {
            resources.GetString("cbCustomTrainTypDruh.Items"),
            resources.GetString("cbCustomTrainTypDruh.Items1"),
            resources.GetString("cbCustomTrainTypDruh.Items2"),
            resources.GetString("cbCustomTrainTypDruh.Items3")});
            resources.ApplyResources(this.cbCustomTrainTypDruh, "cbCustomTrainTypDruh");
            this.cbCustomTrainTypDruh.Name = "cbCustomTrainTypDruh";
            exComboBoxStyle1.ArrowColor = null;
            exComboBoxStyle1.BackColor = null;
            exComboBoxStyle1.BorderColor = null;
            exComboBoxStyle1.ButtonBackColor = null;
            exComboBoxStyle1.ButtonBorderColor = null;
            exComboBoxStyle1.ButtonRenderFirst = null;
            exComboBoxStyle1.ForeColor = null;
            this.cbCustomTrainTypDruh.StyleDisabled = exComboBoxStyle1;
            exComboBoxStyle2.ArrowColor = null;
            exComboBoxStyle2.BackColor = null;
            exComboBoxStyle2.BorderColor = null;
            exComboBoxStyle2.ButtonBackColor = null;
            exComboBoxStyle2.ButtonBorderColor = null;
            exComboBoxStyle2.ButtonRenderFirst = null;
            exComboBoxStyle2.ForeColor = null;
            this.cbCustomTrainTypDruh.StyleHighlight = exComboBoxStyle2;
            exComboBoxStyle3.ArrowColor = null;
            exComboBoxStyle3.BackColor = null;
            exComboBoxStyle3.BorderColor = null;
            exComboBoxStyle3.ButtonBackColor = null;
            exComboBoxStyle3.ButtonBorderColor = null;
            exComboBoxStyle3.ButtonRenderFirst = null;
            exComboBoxStyle3.ForeColor = null;
            this.cbCustomTrainTypDruh.StyleNormal = exComboBoxStyle3;
            exComboBoxStyle4.ArrowColor = null;
            exComboBoxStyle4.BackColor = null;
            exComboBoxStyle4.BorderColor = null;
            exComboBoxStyle4.ButtonBackColor = null;
            exComboBoxStyle4.ButtonBorderColor = null;
            exComboBoxStyle4.ButtonRenderFirst = null;
            exComboBoxStyle4.ForeColor = null;
            this.cbCustomTrainTypDruh.StyleSelected = exComboBoxStyle4;
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
            this.bCustomTrainTypDelete.UseVisualStyleBackColor = true;
            this.bCustomTrainTypDelete.Click += new System.EventHandler(this.bCustomTrainTypDelete_Click);
            // 
            // bCustomTrainTypAdd
            // 
            resources.ApplyResources(this.bCustomTrainTypAdd, "bCustomTrainTypAdd");
            this.bCustomTrainTypAdd.Name = "bCustomTrainTypAdd";
            this.bCustomTrainTypAdd.UseVisualStyleBackColor = true;
            this.bCustomTrainTypAdd.Click += new System.EventHandler(this.bCustomTrainTypAdd_Click);
            // 
            // bCustomTrainTypEdit
            // 
            resources.ApplyResources(this.bCustomTrainTypEdit, "bCustomTrainTypEdit");
            this.bCustomTrainTypEdit.Name = "bCustomTrainTypEdit";
            this.bCustomTrainTypEdit.UseVisualStyleBackColor = true;
            this.bCustomTrainTypEdit.Click += new System.EventHandler(this.bCustomTrainTypEdit_Click);
            // 
            // listTrainTypes
            // 
            this.listTrainTypes.DisplayMember = "Name";
            this.listTrainTypes.FormattingEnabled = true;
            resources.ApplyResources(this.listTrainTypes, "listTrainTypes");
            this.listTrainTypes.Name = "listTrainTypes";
            this.listTrainTypes.SelectedIndexChanged += new System.EventHandler(this.listTrainTypes_SelectedIndexChanged);
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbDefTrainTypSkratka);
            this.groupBox3.Controls.Add(this.bDefTrainTypDelete);
            this.groupBox3.Controls.Add(this.bDefTrainTypEdit);
            this.groupBox3.Controls.Add(this.bDefTrainTypAdd);
            this.groupBox3.Controls.Add(this.label18);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // cbDefTrainTypSkratka
            // 
            this.cbDefTrainTypSkratka.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbDefTrainTypSkratka.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefTrainTypSkratka.FormattingEnabled = true;
            resources.ApplyResources(this.cbDefTrainTypSkratka, "cbDefTrainTypSkratka");
            this.cbDefTrainTypSkratka.Name = "cbDefTrainTypSkratka";
            exComboBoxStyle5.ArrowColor = null;
            exComboBoxStyle5.BackColor = null;
            exComboBoxStyle5.BorderColor = null;
            exComboBoxStyle5.ButtonBackColor = null;
            exComboBoxStyle5.ButtonBorderColor = null;
            exComboBoxStyle5.ButtonRenderFirst = null;
            exComboBoxStyle5.ForeColor = null;
            this.cbDefTrainTypSkratka.StyleDisabled = exComboBoxStyle5;
            exComboBoxStyle6.ArrowColor = null;
            exComboBoxStyle6.BackColor = null;
            exComboBoxStyle6.BorderColor = null;
            exComboBoxStyle6.ButtonBackColor = null;
            exComboBoxStyle6.ButtonBorderColor = null;
            exComboBoxStyle6.ButtonRenderFirst = null;
            exComboBoxStyle6.ForeColor = null;
            this.cbDefTrainTypSkratka.StyleHighlight = exComboBoxStyle6;
            exComboBoxStyle7.ArrowColor = null;
            exComboBoxStyle7.BackColor = null;
            exComboBoxStyle7.BorderColor = null;
            exComboBoxStyle7.ButtonBackColor = null;
            exComboBoxStyle7.ButtonBorderColor = null;
            exComboBoxStyle7.ButtonRenderFirst = null;
            exComboBoxStyle7.ForeColor = null;
            this.cbDefTrainTypSkratka.StyleNormal = exComboBoxStyle7;
            exComboBoxStyle8.ArrowColor = null;
            exComboBoxStyle8.BackColor = null;
            exComboBoxStyle8.BorderColor = null;
            exComboBoxStyle8.ButtonBackColor = null;
            exComboBoxStyle8.ButtonBorderColor = null;
            exComboBoxStyle8.ButtonRenderFirst = null;
            exComboBoxStyle8.ForeColor = null;
            this.cbDefTrainTypSkratka.StyleSelected = exComboBoxStyle8;
            // 
            // bDefTrainTypDelete
            // 
            resources.ApplyResources(this.bDefTrainTypDelete, "bDefTrainTypDelete");
            this.bDefTrainTypDelete.Name = "bDefTrainTypDelete";
            this.bDefTrainTypDelete.UseVisualStyleBackColor = true;
            this.bDefTrainTypDelete.Click += new System.EventHandler(this.bDefTrainTypDelete_Click);
            // 
            // bDefTrainTypEdit
            // 
            resources.ApplyResources(this.bDefTrainTypEdit, "bDefTrainTypEdit");
            this.bDefTrainTypEdit.Name = "bDefTrainTypEdit";
            this.bDefTrainTypEdit.UseVisualStyleBackColor = true;
            this.bDefTrainTypEdit.Click += new System.EventHandler(this.bDefTrainTypEdit_Click);
            // 
            // bDefTrainTypAdd
            // 
            resources.ApplyResources(this.bDefTrainTypAdd, "bDefTrainTypAdd");
            this.bDefTrainTypAdd.Name = "bDefTrainTypAdd";
            this.bDefTrainTypAdd.UseVisualStyleBackColor = true;
            this.bDefTrainTypAdd.Click += new System.EventHandler(this.bDefTrainTypAdd_Click);
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // tpAudio
            // 
            this.tpAudio.BackColor = System.Drawing.Color.Transparent;
            this.tpAudio.Controls.Add(this.listAudio);
            this.tpAudio.Controls.Add(this.label1);
            this.tpAudio.Controls.Add(this.groupBox2);
            resources.ApplyResources(this.tpAudio, "tpAudio");
            this.tpAudio.Name = "tpAudio";
            // 
            // listAudio
            // 
            this.listAudio.DisplayMember = "Name";
            this.listAudio.FormattingEnabled = true;
            resources.ApplyResources(this.listAudio, "listAudio");
            this.listAudio.Name = "listAudio";
            this.listAudio.SelectedIndexChanged += new System.EventHandler(this.listAudio_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbAudioName);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbAudioNazovFronta);
            this.groupBox2.Controls.Add(this.tbAudioNazovSkratka);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cbCustomOnly);
            this.groupBox2.Controls.Add(this.cbAudioStanica);
            this.groupBox2.Controls.Add(this.bAudioDelete);
            this.groupBox2.Controls.Add(this.bAudioEdit);
            this.groupBox2.Controls.Add(this.bAudioAdd);
            this.groupBox2.Controls.Add(this.label2);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // tbAudioName
            // 
            this.tbAudioName.BorderColor = System.Drawing.Color.DimGray;
            this.tbAudioName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbAudioName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbAudioName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbAudioName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbAudioName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbAudioName.HintText = null;
            resources.ApplyResources(this.tbAudioName, "tbAudioName");
            this.tbAudioName.Name = "tbAudioName";
            this.tbAudioName.TextChanged += new System.EventHandler(this.tbAudioName_TextChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // tbAudioNazovFronta
            // 
            this.tbAudioNazovFronta.BorderColor = System.Drawing.Color.DimGray;
            this.tbAudioNazovFronta.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbAudioNazovFronta.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbAudioNazovFronta.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbAudioNazovFronta.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbAudioNazovFronta.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbAudioNazovFronta.HintText = null;
            resources.ApplyResources(this.tbAudioNazovFronta, "tbAudioNazovFronta");
            this.tbAudioNazovFronta.Name = "tbAudioNazovFronta";
            // 
            // tbAudioNazovSkratka
            // 
            this.tbAudioNazovSkratka.BorderColor = System.Drawing.Color.DimGray;
            this.tbAudioNazovSkratka.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbAudioNazovSkratka.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbAudioNazovSkratka.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbAudioNazovSkratka.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbAudioNazovSkratka.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbAudioNazovSkratka.HintText = null;
            resources.ApplyResources(this.tbAudioNazovSkratka, "tbAudioNazovSkratka");
            this.tbAudioNazovSkratka.Name = "tbAudioNazovSkratka";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cbCustomOnly
            // 
            resources.ApplyResources(this.cbCustomOnly, "cbCustomOnly");
            this.cbCustomOnly.BoxBackColor = System.Drawing.Color.White;
            this.cbCustomOnly.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbCustomOnly.Name = "cbCustomOnly";
            this.cbCustomOnly.UseVisualStyleBackColor = true;
            this.cbCustomOnly.CheckedChanged += new System.EventHandler(this.cbCustomOnly_CheckedChanged);
            // 
            // cbAudioStanica
            // 
            this.cbAudioStanica.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbAudioStanica.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAudioStanica.FormattingEnabled = true;
            resources.ApplyResources(this.cbAudioStanica, "cbAudioStanica");
            this.cbAudioStanica.Name = "cbAudioStanica";
            exComboBoxStyle9.ArrowColor = null;
            exComboBoxStyle9.BackColor = null;
            exComboBoxStyle9.BorderColor = null;
            exComboBoxStyle9.ButtonBackColor = null;
            exComboBoxStyle9.ButtonBorderColor = null;
            exComboBoxStyle9.ButtonRenderFirst = null;
            exComboBoxStyle9.ForeColor = null;
            this.cbAudioStanica.StyleDisabled = exComboBoxStyle9;
            exComboBoxStyle10.ArrowColor = null;
            exComboBoxStyle10.BackColor = null;
            exComboBoxStyle10.BorderColor = null;
            exComboBoxStyle10.ButtonBackColor = null;
            exComboBoxStyle10.ButtonBorderColor = null;
            exComboBoxStyle10.ButtonRenderFirst = null;
            exComboBoxStyle10.ForeColor = null;
            this.cbAudioStanica.StyleHighlight = exComboBoxStyle10;
            exComboBoxStyle11.ArrowColor = null;
            exComboBoxStyle11.BackColor = null;
            exComboBoxStyle11.BorderColor = null;
            exComboBoxStyle11.ButtonBackColor = null;
            exComboBoxStyle11.ButtonBorderColor = null;
            exComboBoxStyle11.ButtonRenderFirst = null;
            exComboBoxStyle11.ForeColor = null;
            this.cbAudioStanica.StyleNormal = exComboBoxStyle11;
            exComboBoxStyle12.ArrowColor = null;
            exComboBoxStyle12.BackColor = null;
            exComboBoxStyle12.BorderColor = null;
            exComboBoxStyle12.ButtonBackColor = null;
            exComboBoxStyle12.ButtonBorderColor = null;
            exComboBoxStyle12.ButtonRenderFirst = null;
            exComboBoxStyle12.ForeColor = null;
            this.cbAudioStanica.StyleSelected = exComboBoxStyle12;
            // 
            // bAudioDelete
            // 
            resources.ApplyResources(this.bAudioDelete, "bAudioDelete");
            this.bAudioDelete.Name = "bAudioDelete";
            this.bAudioDelete.UseVisualStyleBackColor = true;
            this.bAudioDelete.Click += new System.EventHandler(this.bAudioDelete_Click);
            // 
            // bAudioEdit
            // 
            resources.ApplyResources(this.bAudioEdit, "bAudioEdit");
            this.bAudioEdit.Name = "bAudioEdit";
            this.bAudioEdit.UseVisualStyleBackColor = true;
            this.bAudioEdit.Click += new System.EventHandler(this.bAudioEdit_Click);
            // 
            // bAudioAdd
            // 
            resources.ApplyResources(this.bAudioAdd, "bAudioAdd");
            this.bAudioAdd.Name = "bAudioAdd";
            this.bAudioAdd.UseVisualStyleBackColor = true;
            this.bAudioAdd.Click += new System.EventHandler(this.bAudioAdd_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // bSave
            // 
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.Name = "bSave";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // FGlobalSettings
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
            this.Name = "FGlobalSettings";
            this.ShowInTaskbar = false;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FGlobalSettings_HelpButtonClicked);
            this.tabControl.ResumeLayout(false);
            this.tpGrafikony.ResumeLayout(false);
            this.tpGrafikony.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHlaseniePort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTabPort)).EndInit();
            this.tpJazyky.ResumeLayout(false);
            this.tpJazyky.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLanguagesRawBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.languageBindingSource)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tpMeskania.ResumeLayout(false);
            this.tpMeskania.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeskanie)).EndInit();
            this.tpTrainTypes.ResumeLayout(false);
            this.tpTrainTypes.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tpAudio.ResumeLayout(false);
            this.tpAudio.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}