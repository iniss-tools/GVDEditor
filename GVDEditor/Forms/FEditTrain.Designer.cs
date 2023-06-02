using System.Windows.Forms;
using ExControls;
using GVDEditor.Entities;

namespace GVDEditor.Forms
{
    partial class FEditTrain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FEditTrain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bSave = new ExControls.ExButton();
            this.bZrusit = new ExControls.ExButton();
            this.tabControl1 = new ExControls.ExTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new ExControls.ExGroupBox();
            this.boxDialkovy = new ExControls.ExCheckBox();
            this.boxNizkopodlazny = new ExControls.ExCheckBox();
            this.boxMiestenkovy = new ExControls.ExCheckBox();
            this.boxMimoriadny = new ExControls.ExCheckBox();
            this.boxMedzistatny = new ExControls.ExCheckBox();
            this.boxLozkovy = new ExControls.ExCheckBox();
            this.groupBox5 = new ExControls.ExGroupBox();
            this.lVariantHelp = new System.Windows.Forms.Label();
            this.nudVarianta = new ExControls.ExNumericUpDown();
            this.groupBox4 = new ExControls.ExGroupBox();
            this.clbJazyky = new ExControls.ExCheckedListBox();
            this.groupBox3 = new ExControls.ExGroupBox();
            this.bEditLimit = new ExControls.ExButton();
            this.label16 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.dtpPlatnostDo = new System.Windows.Forms.DateTimePicker();
            this.dtpPlatnostOd = new System.Windows.Forms.DateTimePicker();
            this.tDatumoveObmedzenie = new ExControls.ExTextBox();
            this.groupBox2 = new ExControls.ExGroupBox();
            this.mtOdchod = new ExControls.ExMaskedTextBox();
            this.mtPrichod = new ExControls.ExMaskedTextBox();
            this.tbLinkaOdchod = new ExControls.ExTextBox();
            this.tbLinkaPrichod = new ExControls.ExTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbKolajOdchod = new ExControls.ExComboBox();
            this.cbKolajPrichod = new ExControls.ExComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new ExControls.ExGroupBox();
            this.cbNazov = new ExControls.ExComboBox();
            this.cbDopravca = new ExControls.ExComboBox();
            this.cbTyp = new ExControls.ExComboBox();
            this.tbCislo = new ExControls.ExTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cbZoSmeruCustom = new ExControls.ExCheckBox();
            this.dgvTrasaZo = new System.Windows.Forms.DataGridView();
            this.isVDlhomHlaseniDataGridViewCheckBoxColumn2 = new ExControls.DataGridViewExCheckBoxColumn();
            this.isVKratkomHlaseniDataGridViewCheckBoxColumn2 = new ExControls.DataGridViewExCheckBoxColumn();
            this.nameDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stanicaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bDeleteZo = new ExControls.ExButton();
            this.bAddZo = new ExControls.ExButton();
            this.bNeskorZo = new ExControls.ExButton();
            this.bSkorZo = new ExControls.ExButton();
            this.listStaniceZo = new System.Windows.Forms.ListBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cbDoSmeruCustom = new ExControls.ExCheckBox();
            this.dgvTrasaDo = new System.Windows.Forms.DataGridView();
            this.isVDlhomHlaseniDataGridViewCheckBoxColumn3 = new ExControls.DataGridViewExCheckBoxColumn();
            this.isVKratkomHlaseniDataGridViewCheckBoxColumn3 = new ExControls.DataGridViewExCheckBoxColumn();
            this.nameDataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bDeleteDo = new ExControls.ExButton();
            this.bAddDo = new ExControls.ExButton();
            this.bNeskorDo = new ExControls.ExButton();
            this.bSkorDo = new ExControls.ExButton();
            this.listStaniceDo = new System.Windows.Forms.ListBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.dgvDoplnokSet = new System.Windows.Forms.DataGridView();
            this.bDoplnkyDelete = new ExControls.ExButton();
            this.label20 = new System.Windows.Forms.Label();
            this.bDoplnkyEdit = new ExControls.ExButton();
            this.bDoplnkyAdd = new ExControls.ExButton();
            this.listVybrateDoplnky = new System.Windows.Forms.ListBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tbTextDoplnku = new ExControls.ExTextBox();
            this.listAllDoplnky = new System.Windows.Forms.ListBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.bEditLimitRadenie = new ExControls.ExButton();
            this.label26 = new System.Windows.Forms.Label();
            this.dgvRadenieSet = new System.Windows.Forms.DataGridView();
            this.bPlay = new ExControls.ExButton();
            this.bEditRadenie = new ExControls.ExButton();
            this.dtpRadenieDo = new System.Windows.Forms.DateTimePicker();
            this.dtpRadenieOd = new System.Windows.Forms.DateTimePicker();
            this.label27 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.tbDateRemRadenie = new ExControls.ExTextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.bRadenieAdd = new ExControls.ExButton();
            this.label23 = new System.Windows.Forms.Label();
            this.listRadenia = new System.Windows.Forms.ListBox();
            this.label22 = new System.Windows.Forms.Label();
            this.bRadenieDelete = new ExControls.ExButton();
            this.bRadenieEdit = new ExControls.ExButton();
            this.tbRadenie = new ExControls.ExTextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVarianta)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrasaZo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stanicaBindingSource)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrasaDo)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoplnokSet)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRadenieSet)).BeginInit();
            this.SuspendLayout();
            // 
            // bSave
            // 
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.Name = "bSave";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bZrusit
            // 
            resources.ApplyResources(this.bZrusit, "bZrusit");
            this.bZrusit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bZrusit.Name = "bZrusit";
            this.bZrusit.UseVisualStyleBackColor = true;
            this.bZrusit.Click += new System.EventHandler(this.bZrusit_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.BorderColor = System.Drawing.Color.DarkGray;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.HeaderBackColor = System.Drawing.Color.Gainsboro;
            this.tabControl1.HighlightBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.boxDialkovy);
            this.groupBox6.Controls.Add(this.boxNizkopodlazny);
            this.groupBox6.Controls.Add(this.boxMiestenkovy);
            this.groupBox6.Controls.Add(this.boxMimoriadny);
            this.groupBox6.Controls.Add(this.boxMedzistatny);
            this.groupBox6.Controls.Add(this.boxLozkovy);
            this.groupBox6.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // boxDialkovy
            // 
            resources.ApplyResources(this.boxDialkovy, "boxDialkovy");
            this.boxDialkovy.BoxBackColor = System.Drawing.Color.White;
            this.boxDialkovy.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.boxDialkovy.Name = "boxDialkovy";
            this.boxDialkovy.UseVisualStyleBackColor = true;
            // 
            // boxNizkopodlazny
            // 
            resources.ApplyResources(this.boxNizkopodlazny, "boxNizkopodlazny");
            this.boxNizkopodlazny.BoxBackColor = System.Drawing.Color.White;
            this.boxNizkopodlazny.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.boxNizkopodlazny.Name = "boxNizkopodlazny";
            this.boxNizkopodlazny.UseVisualStyleBackColor = true;
            // 
            // boxMiestenkovy
            // 
            resources.ApplyResources(this.boxMiestenkovy, "boxMiestenkovy");
            this.boxMiestenkovy.BoxBackColor = System.Drawing.Color.White;
            this.boxMiestenkovy.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.boxMiestenkovy.Name = "boxMiestenkovy";
            this.boxMiestenkovy.UseVisualStyleBackColor = true;
            // 
            // boxMimoriadny
            // 
            resources.ApplyResources(this.boxMimoriadny, "boxMimoriadny");
            this.boxMimoriadny.BoxBackColor = System.Drawing.Color.White;
            this.boxMimoriadny.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.boxMimoriadny.Name = "boxMimoriadny";
            this.boxMimoriadny.UseVisualStyleBackColor = true;
            // 
            // boxMedzistatny
            // 
            resources.ApplyResources(this.boxMedzistatny, "boxMedzistatny");
            this.boxMedzistatny.BoxBackColor = System.Drawing.Color.White;
            this.boxMedzistatny.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.boxMedzistatny.Name = "boxMedzistatny";
            this.boxMedzistatny.UseVisualStyleBackColor = true;
            // 
            // boxLozkovy
            // 
            resources.ApplyResources(this.boxLozkovy, "boxLozkovy");
            this.boxLozkovy.BoxBackColor = System.Drawing.Color.White;
            this.boxLozkovy.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.boxLozkovy.Name = "boxLozkovy";
            this.boxLozkovy.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lVariantHelp);
            this.groupBox5.Controls.Add(this.nudVarianta);
            this.groupBox5.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // lVariantHelp
            // 
            resources.ApplyResources(this.lVariantHelp, "lVariantHelp");
            this.lVariantHelp.Name = "lVariantHelp";
            // 
            // nudVarianta
            // 
            this.nudVarianta.ArrowsColor = System.Drawing.Color.Empty;
            this.nudVarianta.BorderColor = System.Drawing.Color.Empty;
            this.nudVarianta.HighlightColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.nudVarianta, "nudVarianta");
            this.nudVarianta.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudVarianta.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudVarianta.Name = "nudVarianta";
            this.nudVarianta.SelectedButtonColor = System.Drawing.Color.Empty;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.clbJazyky);
            this.groupBox4.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // clbJazyky
            // 
            this.clbJazyky.FormattingEnabled = true;
            this.clbJazyky.HighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            resources.ApplyResources(this.clbJazyky, "clbJazyky");
            this.clbJazyky.MultiColumn = true;
            this.clbJazyky.Name = "clbJazyky";
            this.clbJazyky.SquareBackColor = System.Drawing.Color.White;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.bEditLimit);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.dtpPlatnostDo);
            this.groupBox3.Controls.Add(this.dtpPlatnostOd);
            this.groupBox3.Controls.Add(this.tDatumoveObmedzenie);
            this.groupBox3.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // bEditLimit
            // 
            resources.ApplyResources(this.bEditLimit, "bEditLimit");
            this.bEditLimit.Name = "bEditLimit";
            this.bEditLimit.UseVisualStyleBackColor = true;
            this.bEditLimit.Click += new System.EventHandler(this.BEditLimit_Click);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // dtpPlatnostDo
            // 
            this.dtpPlatnostDo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpPlatnostDo, "dtpPlatnostDo");
            this.dtpPlatnostDo.Name = "dtpPlatnostDo";
            // 
            // dtpPlatnostOd
            // 
            this.dtpPlatnostOd.CalendarMonthBackground = System.Drawing.SystemColors.ScrollBar;
            this.dtpPlatnostOd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpPlatnostOd, "dtpPlatnostOd");
            this.dtpPlatnostOd.Name = "dtpPlatnostOd";
            // 
            // tDatumoveObmedzenie
            // 
            this.tDatumoveObmedzenie.BorderColor = System.Drawing.Color.DimGray;
            this.tDatumoveObmedzenie.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tDatumoveObmedzenie.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tDatumoveObmedzenie.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tDatumoveObmedzenie.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tDatumoveObmedzenie.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tDatumoveObmedzenie.HintText = null;
            resources.ApplyResources(this.tDatumoveObmedzenie, "tDatumoveObmedzenie");
            this.tDatumoveObmedzenie.Name = "tDatumoveObmedzenie";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.mtOdchod);
            this.groupBox2.Controls.Add(this.mtPrichod);
            this.groupBox2.Controls.Add(this.tbLinkaOdchod);
            this.groupBox2.Controls.Add(this.tbLinkaPrichod);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.cbKolajOdchod);
            this.groupBox2.Controls.Add(this.cbKolajPrichod);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // mtOdchod
            // 
            this.mtOdchod.BeepOnError = true;
            this.mtOdchod.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.mtOdchod.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.mtOdchod, "mtOdchod");
            this.mtOdchod.Name = "mtOdchod";
            this.mtOdchod.ValidatingType = typeof(System.DateTime);
            // 
            // mtPrichod
            // 
            this.mtPrichod.BeepOnError = true;
            this.mtPrichod.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.mtPrichod.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.mtPrichod, "mtPrichod");
            this.mtPrichod.Name = "mtPrichod";
            this.mtPrichod.ValidatingType = typeof(System.DateTime);
            // 
            // tbLinkaOdchod
            // 
            this.tbLinkaOdchod.BorderColor = System.Drawing.Color.DimGray;
            this.tbLinkaOdchod.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbLinkaOdchod.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbLinkaOdchod.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbLinkaOdchod.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbLinkaOdchod.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbLinkaOdchod.HintText = null;
            resources.ApplyResources(this.tbLinkaOdchod, "tbLinkaOdchod");
            this.tbLinkaOdchod.Name = "tbLinkaOdchod";
            this.tbLinkaOdchod.TextChanged += new System.EventHandler(this.tbLinkaOdchod_TextChanged);
            // 
            // tbLinkaPrichod
            // 
            this.tbLinkaPrichod.BorderColor = System.Drawing.Color.DimGray;
            this.tbLinkaPrichod.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbLinkaPrichod.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbLinkaPrichod.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbLinkaPrichod.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbLinkaPrichod.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbLinkaPrichod.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbLinkaPrichod.HintText = "";
            resources.ApplyResources(this.tbLinkaPrichod, "tbLinkaPrichod");
            this.tbLinkaPrichod.Name = "tbLinkaPrichod";
            this.tbLinkaPrichod.TextChanged += new System.EventHandler(this.tbLinkaPrichod_TextChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // cbKolajOdchod
            // 
            this.cbKolajOdchod.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbKolajOdchod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKolajOdchod.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbKolajOdchod.FormattingEnabled = true;
            resources.ApplyResources(this.cbKolajOdchod, "cbKolajOdchod");
            this.cbKolajOdchod.Name = "cbKolajOdchod";
            this.cbKolajOdchod.StyleDisabled.ArrowColor = null;
            this.cbKolajOdchod.StyleDisabled.BackColor = null;
            this.cbKolajOdchod.StyleDisabled.BorderColor = null;
            this.cbKolajOdchod.StyleDisabled.ButtonBackColor = null;
            this.cbKolajOdchod.StyleDisabled.ButtonBorderColor = null;
            this.cbKolajOdchod.StyleDisabled.ButtonRenderFirst = null;
            this.cbKolajOdchod.StyleDisabled.ForeColor = null;
            this.cbKolajOdchod.StyleHighlight.ArrowColor = null;
            this.cbKolajOdchod.StyleHighlight.BackColor = null;
            this.cbKolajOdchod.StyleHighlight.BorderColor = null;
            this.cbKolajOdchod.StyleHighlight.ButtonBackColor = null;
            this.cbKolajOdchod.StyleHighlight.ButtonBorderColor = null;
            this.cbKolajOdchod.StyleHighlight.ButtonRenderFirst = null;
            this.cbKolajOdchod.StyleHighlight.ForeColor = null;
            this.cbKolajOdchod.StyleNormal.ArrowColor = null;
            this.cbKolajOdchod.StyleNormal.BackColor = null;
            this.cbKolajOdchod.StyleNormal.BorderColor = null;
            this.cbKolajOdchod.StyleNormal.ButtonBackColor = null;
            this.cbKolajOdchod.StyleNormal.ButtonBorderColor = null;
            this.cbKolajOdchod.StyleNormal.ButtonRenderFirst = null;
            this.cbKolajOdchod.StyleNormal.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbKolajOdchod.StyleSelected.ArrowColor = null;
            this.cbKolajOdchod.StyleSelected.BackColor = null;
            this.cbKolajOdchod.StyleSelected.BorderColor = null;
            this.cbKolajOdchod.StyleSelected.ButtonBackColor = null;
            this.cbKolajOdchod.StyleSelected.ButtonBorderColor = null;
            this.cbKolajOdchod.StyleSelected.ButtonRenderFirst = null;
            this.cbKolajOdchod.StyleSelected.ForeColor = null;
            this.cbKolajOdchod.UseDarkScrollBar = false;
            this.cbKolajOdchod.SelectedIndexChanged += new System.EventHandler(this.cbKolajOdchod_SelectedIndexChanged);
            // 
            // cbKolajPrichod
            // 
            this.cbKolajPrichod.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbKolajPrichod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKolajPrichod.FormattingEnabled = true;
            resources.ApplyResources(this.cbKolajPrichod, "cbKolajPrichod");
            this.cbKolajPrichod.Name = "cbKolajPrichod";
            this.cbKolajPrichod.StyleDisabled.ArrowColor = null;
            this.cbKolajPrichod.StyleDisabled.BackColor = null;
            this.cbKolajPrichod.StyleDisabled.BorderColor = null;
            this.cbKolajPrichod.StyleDisabled.ButtonBackColor = null;
            this.cbKolajPrichod.StyleDisabled.ButtonBorderColor = null;
            this.cbKolajPrichod.StyleDisabled.ButtonRenderFirst = null;
            this.cbKolajPrichod.StyleDisabled.ForeColor = null;
            this.cbKolajPrichod.StyleHighlight.ArrowColor = null;
            this.cbKolajPrichod.StyleHighlight.BackColor = null;
            this.cbKolajPrichod.StyleHighlight.BorderColor = null;
            this.cbKolajPrichod.StyleHighlight.ButtonBackColor = null;
            this.cbKolajPrichod.StyleHighlight.ButtonBorderColor = null;
            this.cbKolajPrichod.StyleHighlight.ButtonRenderFirst = null;
            this.cbKolajPrichod.StyleHighlight.ForeColor = null;
            this.cbKolajPrichod.StyleNormal.ArrowColor = null;
            this.cbKolajPrichod.StyleNormal.BackColor = null;
            this.cbKolajPrichod.StyleNormal.BorderColor = null;
            this.cbKolajPrichod.StyleNormal.ButtonBackColor = null;
            this.cbKolajPrichod.StyleNormal.ButtonBorderColor = null;
            this.cbKolajPrichod.StyleNormal.ButtonRenderFirst = null;
            this.cbKolajPrichod.StyleNormal.ForeColor = null;
            this.cbKolajPrichod.StyleSelected.ArrowColor = null;
            this.cbKolajPrichod.StyleSelected.BackColor = null;
            this.cbKolajPrichod.StyleSelected.BorderColor = null;
            this.cbKolajPrichod.StyleSelected.ButtonBackColor = null;
            this.cbKolajPrichod.StyleSelected.ButtonBorderColor = null;
            this.cbKolajPrichod.StyleSelected.ButtonRenderFirst = null;
            this.cbKolajPrichod.StyleSelected.ForeColor = null;
            this.cbKolajPrichod.UseDarkScrollBar = false;
            this.cbKolajPrichod.SelectedIndexChanged += new System.EventHandler(this.cbKolajPrichod_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.cbNazov);
            this.groupBox1.Controls.Add(this.cbDopravca);
            this.groupBox1.Controls.Add(this.cbTyp);
            this.groupBox1.Controls.Add(this.tbCislo);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cbNazov
            // 
            this.cbNazov.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbNazov.FormattingEnabled = true;
            resources.ApplyResources(this.cbNazov, "cbNazov");
            this.cbNazov.Name = "cbNazov";
            this.cbNazov.StyleDisabled.ArrowColor = null;
            this.cbNazov.StyleDisabled.BackColor = null;
            this.cbNazov.StyleDisabled.BorderColor = null;
            this.cbNazov.StyleDisabled.ButtonBackColor = null;
            this.cbNazov.StyleDisabled.ButtonBorderColor = null;
            this.cbNazov.StyleDisabled.ButtonRenderFirst = null;
            this.cbNazov.StyleDisabled.ForeColor = null;
            this.cbNazov.StyleHighlight.ArrowColor = null;
            this.cbNazov.StyleHighlight.BackColor = null;
            this.cbNazov.StyleHighlight.BorderColor = null;
            this.cbNazov.StyleHighlight.ButtonBackColor = null;
            this.cbNazov.StyleHighlight.ButtonBorderColor = null;
            this.cbNazov.StyleHighlight.ButtonRenderFirst = null;
            this.cbNazov.StyleHighlight.ForeColor = null;
            this.cbNazov.StyleNormal.ArrowColor = null;
            this.cbNazov.StyleNormal.BackColor = null;
            this.cbNazov.StyleNormal.BorderColor = null;
            this.cbNazov.StyleNormal.ButtonBackColor = null;
            this.cbNazov.StyleNormal.ButtonBorderColor = null;
            this.cbNazov.StyleNormal.ButtonRenderFirst = null;
            this.cbNazov.StyleNormal.ForeColor = null;
            this.cbNazov.StyleSelected.ArrowColor = null;
            this.cbNazov.StyleSelected.BackColor = null;
            this.cbNazov.StyleSelected.BorderColor = null;
            this.cbNazov.StyleSelected.ButtonBackColor = null;
            this.cbNazov.StyleSelected.ButtonBorderColor = null;
            this.cbNazov.StyleSelected.ButtonRenderFirst = null;
            this.cbNazov.StyleSelected.ForeColor = null;
            this.cbNazov.UseDarkScrollBar = false;
            // 
            // cbDopravca
            // 
            this.cbDopravca.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbDopravca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDopravca.FormattingEnabled = true;
            resources.ApplyResources(this.cbDopravca, "cbDopravca");
            this.cbDopravca.Name = "cbDopravca";
            this.cbDopravca.StyleDisabled.ArrowColor = null;
            this.cbDopravca.StyleDisabled.BackColor = null;
            this.cbDopravca.StyleDisabled.BorderColor = null;
            this.cbDopravca.StyleDisabled.ButtonBackColor = null;
            this.cbDopravca.StyleDisabled.ButtonBorderColor = null;
            this.cbDopravca.StyleDisabled.ButtonRenderFirst = null;
            this.cbDopravca.StyleDisabled.ForeColor = null;
            this.cbDopravca.StyleHighlight.ArrowColor = null;
            this.cbDopravca.StyleHighlight.BackColor = null;
            this.cbDopravca.StyleHighlight.BorderColor = null;
            this.cbDopravca.StyleHighlight.ButtonBackColor = null;
            this.cbDopravca.StyleHighlight.ButtonBorderColor = null;
            this.cbDopravca.StyleHighlight.ButtonRenderFirst = null;
            this.cbDopravca.StyleHighlight.ForeColor = null;
            this.cbDopravca.StyleNormal.ArrowColor = null;
            this.cbDopravca.StyleNormal.BackColor = null;
            this.cbDopravca.StyleNormal.BorderColor = null;
            this.cbDopravca.StyleNormal.ButtonBackColor = null;
            this.cbDopravca.StyleNormal.ButtonBorderColor = null;
            this.cbDopravca.StyleNormal.ButtonRenderFirst = null;
            this.cbDopravca.StyleNormal.ForeColor = null;
            this.cbDopravca.StyleSelected.ArrowColor = null;
            this.cbDopravca.StyleSelected.BackColor = null;
            this.cbDopravca.StyleSelected.BorderColor = null;
            this.cbDopravca.StyleSelected.ButtonBackColor = null;
            this.cbDopravca.StyleSelected.ButtonBorderColor = null;
            this.cbDopravca.StyleSelected.ButtonRenderFirst = null;
            this.cbDopravca.StyleSelected.ForeColor = null;
            this.cbDopravca.UseDarkScrollBar = false;
            // 
            // cbTyp
            // 
            this.cbTyp.DropDownSelectedRowBackColor = System.Drawing.Color.Empty;
            this.cbTyp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTyp.FormattingEnabled = true;
            resources.ApplyResources(this.cbTyp, "cbTyp");
            this.cbTyp.Name = "cbTyp";
            this.cbTyp.StyleDisabled.ArrowColor = null;
            this.cbTyp.StyleDisabled.BackColor = null;
            this.cbTyp.StyleDisabled.BorderColor = null;
            this.cbTyp.StyleDisabled.ButtonBackColor = null;
            this.cbTyp.StyleDisabled.ButtonBorderColor = null;
            this.cbTyp.StyleDisabled.ButtonRenderFirst = null;
            this.cbTyp.StyleDisabled.ForeColor = null;
            this.cbTyp.StyleHighlight.ArrowColor = null;
            this.cbTyp.StyleHighlight.BackColor = null;
            this.cbTyp.StyleHighlight.BorderColor = null;
            this.cbTyp.StyleHighlight.ButtonBackColor = null;
            this.cbTyp.StyleHighlight.ButtonBorderColor = null;
            this.cbTyp.StyleHighlight.ButtonRenderFirst = null;
            this.cbTyp.StyleHighlight.ForeColor = null;
            this.cbTyp.StyleNormal.ArrowColor = null;
            this.cbTyp.StyleNormal.BackColor = null;
            this.cbTyp.StyleNormal.BorderColor = null;
            this.cbTyp.StyleNormal.ButtonBackColor = null;
            this.cbTyp.StyleNormal.ButtonBorderColor = null;
            this.cbTyp.StyleNormal.ButtonRenderFirst = null;
            this.cbTyp.StyleNormal.ForeColor = null;
            this.cbTyp.StyleSelected.ArrowColor = null;
            this.cbTyp.StyleSelected.BackColor = null;
            this.cbTyp.StyleSelected.BorderColor = null;
            this.cbTyp.StyleSelected.ButtonBackColor = null;
            this.cbTyp.StyleSelected.ButtonBorderColor = null;
            this.cbTyp.StyleSelected.ButtonRenderFirst = null;
            this.cbTyp.StyleSelected.ForeColor = null;
            this.cbTyp.UseDarkScrollBar = false;
            // 
            // tbCislo
            // 
            this.tbCislo.BackColor = System.Drawing.SystemColors.Window;
            this.tbCislo.BorderColor = System.Drawing.Color.DimGray;
            this.tbCislo.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbCislo.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbCislo.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCislo.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbCislo.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbCislo.HintText = "";
            resources.ApplyResources(this.tbCislo, "tbCislo");
            this.tbCislo.Name = "tbCislo";
            this.tbCislo.TextChanged += new System.EventHandler(this.tbCislo_TextChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Transparent;
            this.tabPage2.Controls.Add(this.cbZoSmeruCustom);
            this.tabPage2.Controls.Add(this.dgvTrasaZo);
            this.tabPage2.Controls.Add(this.bDeleteZo);
            this.tabPage2.Controls.Add(this.bAddZo);
            this.tabPage2.Controls.Add(this.bNeskorZo);
            this.tabPage2.Controls.Add(this.bSkorZo);
            this.tabPage2.Controls.Add(this.listStaniceZo);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label12);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            // 
            // cbZoSmeruCustom
            // 
            resources.ApplyResources(this.cbZoSmeruCustom, "cbZoSmeruCustom");
            this.cbZoSmeruCustom.BoxBackColor = System.Drawing.Color.White;
            this.cbZoSmeruCustom.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbZoSmeruCustom.Name = "cbZoSmeruCustom";
            this.cbZoSmeruCustom.UseVisualStyleBackColor = true;
            this.cbZoSmeruCustom.CheckedChanged += new System.EventHandler(this.cbZoSmeruCustom_CheckedChanged);
            // 
            // dgvTrasaZo
            // 
            this.dgvTrasaZo.AllowUserToAddRows = false;
            this.dgvTrasaZo.AllowUserToResizeRows = false;
            this.dgvTrasaZo.AutoGenerateColumns = false;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTrasaZo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvTrasaZo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrasaZo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.isVDlhomHlaseniDataGridViewCheckBoxColumn2,
            this.isVKratkomHlaseniDataGridViewCheckBoxColumn2,
            this.nameDataGridViewTextBoxColumn2});
            this.dgvTrasaZo.DataSource = this.stanicaBindingSource;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTrasaZo.DefaultCellStyle = dataGridViewCellStyle14;
            resources.ApplyResources(this.dgvTrasaZo, "dgvTrasaZo");
            this.dgvTrasaZo.MultiSelect = false;
            this.dgvTrasaZo.Name = "dgvTrasaZo";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTrasaZo.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.dgvTrasaZo.RowHeadersVisible = false;
            this.dgvTrasaZo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrasaZo.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrasaZo_CellDoubleClick);
            // 
            // isVDlhomHlaseniDataGridViewCheckBoxColumn2
            // 
            this.isVDlhomHlaseniDataGridViewCheckBoxColumn2.DataPropertyName = "IsInLongReport";
            resources.ApplyResources(this.isVDlhomHlaseniDataGridViewCheckBoxColumn2, "isVDlhomHlaseniDataGridViewCheckBoxColumn2");
            this.isVDlhomHlaseniDataGridViewCheckBoxColumn2.Name = "isVDlhomHlaseniDataGridViewCheckBoxColumn2";
            this.isVDlhomHlaseniDataGridViewCheckBoxColumn2.SquareBackColor = System.Drawing.Color.White;
            // 
            // isVKratkomHlaseniDataGridViewCheckBoxColumn2
            // 
            this.isVKratkomHlaseniDataGridViewCheckBoxColumn2.DataPropertyName = "IsInShortReport";
            resources.ApplyResources(this.isVKratkomHlaseniDataGridViewCheckBoxColumn2, "isVKratkomHlaseniDataGridViewCheckBoxColumn2");
            this.isVKratkomHlaseniDataGridViewCheckBoxColumn2.Name = "isVKratkomHlaseniDataGridViewCheckBoxColumn2";
            this.isVKratkomHlaseniDataGridViewCheckBoxColumn2.SquareBackColor = System.Drawing.Color.White;
            // 
            // nameDataGridViewTextBoxColumn2
            // 
            this.nameDataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn2.DataPropertyName = "Name";
            resources.ApplyResources(this.nameDataGridViewTextBoxColumn2, "nameDataGridViewTextBoxColumn2");
            this.nameDataGridViewTextBoxColumn2.Name = "nameDataGridViewTextBoxColumn2";
            this.nameDataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // stanicaBindingSource
            // 
            this.stanicaBindingSource.DataSource = typeof(GVDEditor.Entities.Station);
            // 
            // bDeleteZo
            // 
            resources.ApplyResources(this.bDeleteZo, "bDeleteZo");
            this.bDeleteZo.Name = "bDeleteZo";
            this.bDeleteZo.UseVisualStyleBackColor = true;
            this.bDeleteZo.Click += new System.EventHandler(this.bDeleteZo_Click);
            // 
            // bAddZo
            // 
            resources.ApplyResources(this.bAddZo, "bAddZo");
            this.bAddZo.Name = "bAddZo";
            this.bAddZo.UseVisualStyleBackColor = true;
            this.bAddZo.Click += new System.EventHandler(this.bAddZo_Click);
            // 
            // bNeskorZo
            // 
            resources.ApplyResources(this.bNeskorZo, "bNeskorZo");
            this.bNeskorZo.Name = "bNeskorZo";
            this.bNeskorZo.UseVisualStyleBackColor = true;
            this.bNeskorZo.Click += new System.EventHandler(this.bNeskorZo_Click);
            // 
            // bSkorZo
            // 
            resources.ApplyResources(this.bSkorZo, "bSkorZo");
            this.bSkorZo.Name = "bSkorZo";
            this.bSkorZo.UseVisualStyleBackColor = true;
            this.bSkorZo.Click += new System.EventHandler(this.bSkorZo_Click);
            // 
            // listStaniceZo
            // 
            this.listStaniceZo.FormattingEnabled = true;
            resources.ApplyResources(this.listStaniceZo, "listStaniceZo");
            this.listStaniceZo.Name = "listStaniceZo";
            this.listStaniceZo.Sorted = true;
            this.listStaniceZo.DoubleClick += new System.EventHandler(this.listStaniceZo_DoubleClick);
            this.listStaniceZo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listStaniceZo_KeyPress);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Transparent;
            this.tabPage3.Controls.Add(this.cbDoSmeruCustom);
            this.tabPage3.Controls.Add(this.dgvTrasaDo);
            this.tabPage3.Controls.Add(this.bDeleteDo);
            this.tabPage3.Controls.Add(this.bAddDo);
            this.tabPage3.Controls.Add(this.bNeskorDo);
            this.tabPage3.Controls.Add(this.bSkorDo);
            this.tabPage3.Controls.Add(this.listStaniceDo);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.label15);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            // 
            // cbDoSmeruCustom
            // 
            resources.ApplyResources(this.cbDoSmeruCustom, "cbDoSmeruCustom");
            this.cbDoSmeruCustom.BoxBackColor = System.Drawing.Color.White;
            this.cbDoSmeruCustom.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbDoSmeruCustom.Name = "cbDoSmeruCustom";
            this.cbDoSmeruCustom.UseVisualStyleBackColor = true;
            this.cbDoSmeruCustom.CheckedChanged += new System.EventHandler(this.cbDoSmeruCustom_CheckedChanged);
            // 
            // dgvTrasaDo
            // 
            this.dgvTrasaDo.AllowUserToAddRows = false;
            this.dgvTrasaDo.AllowUserToResizeRows = false;
            this.dgvTrasaDo.AutoGenerateColumns = false;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTrasaDo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.dgvTrasaDo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrasaDo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.isVDlhomHlaseniDataGridViewCheckBoxColumn3,
            this.isVKratkomHlaseniDataGridViewCheckBoxColumn3,
            this.nameDataGridViewTextBoxColumn3});
            this.dgvTrasaDo.DataSource = this.stanicaBindingSource;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTrasaDo.DefaultCellStyle = dataGridViewCellStyle17;
            resources.ApplyResources(this.dgvTrasaDo, "dgvTrasaDo");
            this.dgvTrasaDo.MultiSelect = false;
            this.dgvTrasaDo.Name = "dgvTrasaDo";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTrasaDo.RowHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.dgvTrasaDo.RowHeadersVisible = false;
            this.dgvTrasaDo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrasaDo.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrasaDo_CellDoubleClick);
            // 
            // isVDlhomHlaseniDataGridViewCheckBoxColumn3
            // 
            this.isVDlhomHlaseniDataGridViewCheckBoxColumn3.DataPropertyName = "IsInLongReport";
            resources.ApplyResources(this.isVDlhomHlaseniDataGridViewCheckBoxColumn3, "isVDlhomHlaseniDataGridViewCheckBoxColumn3");
            this.isVDlhomHlaseniDataGridViewCheckBoxColumn3.Name = "isVDlhomHlaseniDataGridViewCheckBoxColumn3";
            this.isVDlhomHlaseniDataGridViewCheckBoxColumn3.SquareBackColor = System.Drawing.Color.White;
            // 
            // isVKratkomHlaseniDataGridViewCheckBoxColumn3
            // 
            this.isVKratkomHlaseniDataGridViewCheckBoxColumn3.DataPropertyName = "IsInShortReport";
            resources.ApplyResources(this.isVKratkomHlaseniDataGridViewCheckBoxColumn3, "isVKratkomHlaseniDataGridViewCheckBoxColumn3");
            this.isVKratkomHlaseniDataGridViewCheckBoxColumn3.Name = "isVKratkomHlaseniDataGridViewCheckBoxColumn3";
            this.isVKratkomHlaseniDataGridViewCheckBoxColumn3.SquareBackColor = System.Drawing.Color.White;
            // 
            // nameDataGridViewTextBoxColumn3
            // 
            this.nameDataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn3.DataPropertyName = "Name";
            resources.ApplyResources(this.nameDataGridViewTextBoxColumn3, "nameDataGridViewTextBoxColumn3");
            this.nameDataGridViewTextBoxColumn3.Name = "nameDataGridViewTextBoxColumn3";
            this.nameDataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // bDeleteDo
            // 
            resources.ApplyResources(this.bDeleteDo, "bDeleteDo");
            this.bDeleteDo.Name = "bDeleteDo";
            this.bDeleteDo.UseVisualStyleBackColor = true;
            this.bDeleteDo.Click += new System.EventHandler(this.bDeleteDo_Click);
            // 
            // bAddDo
            // 
            resources.ApplyResources(this.bAddDo, "bAddDo");
            this.bAddDo.Name = "bAddDo";
            this.bAddDo.UseVisualStyleBackColor = true;
            this.bAddDo.Click += new System.EventHandler(this.bAddDo_Click);
            // 
            // bNeskorDo
            // 
            resources.ApplyResources(this.bNeskorDo, "bNeskorDo");
            this.bNeskorDo.Name = "bNeskorDo";
            this.bNeskorDo.UseVisualStyleBackColor = true;
            this.bNeskorDo.Click += new System.EventHandler(this.bNeskorDo_Click);
            // 
            // bSkorDo
            // 
            resources.ApplyResources(this.bSkorDo, "bSkorDo");
            this.bSkorDo.Name = "bSkorDo";
            this.bSkorDo.UseVisualStyleBackColor = true;
            this.bSkorDo.Click += new System.EventHandler(this.bSkorDo_Click);
            // 
            // listStaniceDo
            // 
            this.listStaniceDo.FormattingEnabled = true;
            resources.ApplyResources(this.listStaniceDo, "listStaniceDo");
            this.listStaniceDo.Name = "listStaniceDo";
            this.listStaniceDo.Sorted = true;
            this.listStaniceDo.DoubleClick += new System.EventHandler(this.listStaniceDo_DoubleClick);
            this.listStaniceDo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listStaniceDo_KeyPress);
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.Transparent;
            this.tabPage5.Controls.Add(this.dgvDoplnokSet);
            this.tabPage5.Controls.Add(this.bDoplnkyDelete);
            this.tabPage5.Controls.Add(this.label20);
            this.tabPage5.Controls.Add(this.bDoplnkyEdit);
            this.tabPage5.Controls.Add(this.bDoplnkyAdd);
            this.tabPage5.Controls.Add(this.listVybrateDoplnky);
            this.tabPage5.Controls.Add(this.label19);
            this.tabPage5.Controls.Add(this.label18);
            this.tabPage5.Controls.Add(this.tbTextDoplnku);
            this.tabPage5.Controls.Add(this.listAllDoplnky);
            this.tabPage5.Controls.Add(this.label17);
            resources.ApplyResources(this.tabPage5, "tabPage5");
            this.tabPage5.Name = "tabPage5";
            // 
            // dgvDoplnokSet
            // 
            this.dgvDoplnokSet.AllowUserToAddRows = false;
            this.dgvDoplnokSet.AllowUserToDeleteRows = false;
            this.dgvDoplnokSet.AllowUserToResizeRows = false;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDoplnokSet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.dgvDoplnokSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDoplnokSet.DefaultCellStyle = dataGridViewCellStyle20;
            resources.ApplyResources(this.dgvDoplnokSet, "dgvDoplnokSet");
            this.dgvDoplnokSet.Name = "dgvDoplnokSet";
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDoplnokSet.RowHeadersDefaultCellStyle = dataGridViewCellStyle21;
            this.dgvDoplnokSet.RowHeadersVisible = false;
            this.dgvDoplnokSet.RowTemplate.Height = 24;
            // 
            // bDoplnkyDelete
            // 
            resources.ApplyResources(this.bDoplnkyDelete, "bDoplnkyDelete");
            this.bDoplnkyDelete.Name = "bDoplnkyDelete";
            this.bDoplnkyDelete.UseVisualStyleBackColor = true;
            this.bDoplnkyDelete.Click += new System.EventHandler(this.bDoplnkyDelete_Click);
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // bDoplnkyEdit
            // 
            resources.ApplyResources(this.bDoplnkyEdit, "bDoplnkyEdit");
            this.bDoplnkyEdit.Name = "bDoplnkyEdit";
            this.bDoplnkyEdit.UseVisualStyleBackColor = true;
            this.bDoplnkyEdit.Click += new System.EventHandler(this.bDoplnkyEdit_Click);
            // 
            // bDoplnkyAdd
            // 
            resources.ApplyResources(this.bDoplnkyAdd, "bDoplnkyAdd");
            this.bDoplnkyAdd.Name = "bDoplnkyAdd";
            this.bDoplnkyAdd.UseVisualStyleBackColor = true;
            this.bDoplnkyAdd.Click += new System.EventHandler(this.bDoplnkyAdd_Click);
            // 
            // listVybrateDoplnky
            // 
            this.listVybrateDoplnky.FormattingEnabled = true;
            resources.ApplyResources(this.listVybrateDoplnky, "listVybrateDoplnky");
            this.listVybrateDoplnky.Name = "listVybrateDoplnky";
            this.listVybrateDoplnky.SelectedIndexChanged += new System.EventHandler(this.listVybrateDoplnky_SelectedIndexChanged);
            this.listVybrateDoplnky.DoubleClick += new System.EventHandler(this.listVybrateDoplnky_DoubleClick);
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // tbTextDoplnku
            // 
            this.tbTextDoplnku.BorderColor = System.Drawing.Color.DimGray;
            this.tbTextDoplnku.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTextDoplnku.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbTextDoplnku.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbTextDoplnku.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbTextDoplnku.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbTextDoplnku.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbTextDoplnku.HintText = null;
            resources.ApplyResources(this.tbTextDoplnku, "tbTextDoplnku");
            this.tbTextDoplnku.Name = "tbTextDoplnku";
            this.tbTextDoplnku.ReadOnly = true;
            // 
            // listAllDoplnky
            // 
            this.listAllDoplnky.FormattingEnabled = true;
            resources.ApplyResources(this.listAllDoplnky, "listAllDoplnky");
            this.listAllDoplnky.Name = "listAllDoplnky";
            this.listAllDoplnky.SelectedIndexChanged += new System.EventHandler(this.listAllDoplnky_SelectedIndexChanged);
            this.listAllDoplnky.DoubleClick += new System.EventHandler(this.listAllDoplnky_DoubleClick);
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.Transparent;
            this.tabPage4.Controls.Add(this.bEditLimitRadenie);
            this.tabPage4.Controls.Add(this.label26);
            this.tabPage4.Controls.Add(this.dgvRadenieSet);
            this.tabPage4.Controls.Add(this.bPlay);
            this.tabPage4.Controls.Add(this.bEditRadenie);
            this.tabPage4.Controls.Add(this.dtpRadenieDo);
            this.tabPage4.Controls.Add(this.dtpRadenieOd);
            this.tabPage4.Controls.Add(this.label27);
            this.tabPage4.Controls.Add(this.label25);
            this.tabPage4.Controls.Add(this.tbDateRemRadenie);
            this.tabPage4.Controls.Add(this.label24);
            this.tabPage4.Controls.Add(this.bRadenieAdd);
            this.tabPage4.Controls.Add(this.label23);
            this.tabPage4.Controls.Add(this.listRadenia);
            this.tabPage4.Controls.Add(this.label22);
            this.tabPage4.Controls.Add(this.bRadenieDelete);
            this.tabPage4.Controls.Add(this.bRadenieEdit);
            this.tabPage4.Controls.Add(this.tbRadenie);
            this.tabPage4.Controls.Add(this.label21);
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.Name = "tabPage4";
            // 
            // bEditLimitRadenie
            // 
            resources.ApplyResources(this.bEditLimitRadenie, "bEditLimitRadenie");
            this.bEditLimitRadenie.Name = "bEditLimitRadenie";
            this.bEditLimitRadenie.UseVisualStyleBackColor = true;
            this.bEditLimitRadenie.Click += new System.EventHandler(this.BEditLimitRadenie_Click);
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // dgvRadenieSet
            // 
            this.dgvRadenieSet.AllowUserToAddRows = false;
            this.dgvRadenieSet.AllowUserToDeleteRows = false;
            this.dgvRadenieSet.AllowUserToResizeRows = false;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRadenieSet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.dgvRadenieSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle23.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle23.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle23.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRadenieSet.DefaultCellStyle = dataGridViewCellStyle23;
            resources.ApplyResources(this.dgvRadenieSet, "dgvRadenieSet");
            this.dgvRadenieSet.Name = "dgvRadenieSet";
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle24.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle24.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRadenieSet.RowHeadersDefaultCellStyle = dataGridViewCellStyle24;
            this.dgvRadenieSet.RowHeadersVisible = false;
            this.dgvRadenieSet.RowTemplate.Height = 24;
            // 
            // bPlay
            // 
            resources.ApplyResources(this.bPlay, "bPlay");
            this.bPlay.Name = "bPlay";
            this.bPlay.UseVisualStyleBackColor = true;
            this.bPlay.Click += new System.EventHandler(this.bPlay_Click);
            // 
            // bEditRadenie
            // 
            resources.ApplyResources(this.bEditRadenie, "bEditRadenie");
            this.bEditRadenie.Name = "bEditRadenie";
            this.bEditRadenie.UseVisualStyleBackColor = true;
            this.bEditRadenie.Click += new System.EventHandler(this.bEditRadenie_Click);
            // 
            // dtpRadenieDo
            // 
            this.dtpRadenieDo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpRadenieDo, "dtpRadenieDo");
            this.dtpRadenieDo.Name = "dtpRadenieDo";
            // 
            // dtpRadenieOd
            // 
            this.dtpRadenieOd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpRadenieOd, "dtpRadenieOd");
            this.dtpRadenieOd.Name = "dtpRadenieOd";
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.Name = "label27";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // tbDateRemRadenie
            // 
            this.tbDateRemRadenie.BorderColor = System.Drawing.Color.DimGray;
            this.tbDateRemRadenie.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbDateRemRadenie.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbDateRemRadenie.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDateRemRadenie.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbDateRemRadenie.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDateRemRadenie.HintText = null;
            resources.ApplyResources(this.tbDateRemRadenie, "tbDateRemRadenie");
            this.tbDateRemRadenie.Name = "tbDateRemRadenie";
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // bRadenieAdd
            // 
            resources.ApplyResources(this.bRadenieAdd, "bRadenieAdd");
            this.bRadenieAdd.Name = "bRadenieAdd";
            this.bRadenieAdd.UseVisualStyleBackColor = true;
            this.bRadenieAdd.Click += new System.EventHandler(this.bRadenieAdd_Click);
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // listRadenia
            // 
            this.listRadenia.FormattingEnabled = true;
            resources.ApplyResources(this.listRadenia, "listRadenia");
            this.listRadenia.Name = "listRadenia";
            this.listRadenia.SelectedIndexChanged += new System.EventHandler(this.listRadenia_SelectedIndexChanged);
            this.listRadenia.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.listRadenia_Format);
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // bRadenieDelete
            // 
            resources.ApplyResources(this.bRadenieDelete, "bRadenieDelete");
            this.bRadenieDelete.Name = "bRadenieDelete";
            this.bRadenieDelete.UseVisualStyleBackColor = true;
            this.bRadenieDelete.Click += new System.EventHandler(this.bRadenieDelete_Click);
            // 
            // bRadenieEdit
            // 
            resources.ApplyResources(this.bRadenieEdit, "bRadenieEdit");
            this.bRadenieEdit.Name = "bRadenieEdit";
            this.bRadenieEdit.UseVisualStyleBackColor = true;
            this.bRadenieEdit.Click += new System.EventHandler(this.bRadenieEdit_Click);
            // 
            // tbRadenie
            // 
            this.tbRadenie.BorderColor = System.Drawing.Color.DimGray;
            this.tbRadenie.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbRadenie.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbRadenie.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbRadenie.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbRadenie.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbRadenie.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbRadenie.HintText = null;
            resources.ApplyResources(this.tbRadenie, "tbRadenie");
            this.tbRadenie.Name = "tbRadenie";
            this.tbRadenie.ReadOnly = true;
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // FEditTrain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.bZrusit);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FEditTrain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FEditTrain_HelpButtonClicked);
            this.Load += new System.EventHandler(this.FEditTrain_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVarianta)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrasaZo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stanicaBindingSource)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrasaDo)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoplnokSet)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRadenieSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExTabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private ExGroupBox groupBox3;
        private ExTextBox tDatumoveObmedzenie;
        private ExGroupBox groupBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private ExComboBox cbKolajOdchod;
        private ExComboBox cbKolajPrichod;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private ExGroupBox groupBox1;
        private ExComboBox cbDopravca;
        private ExComboBox cbTyp;
        private ExCheckBox boxLozkovy;
        private ExCheckBox boxMedzistatny;
        private ExCheckBox boxMiestenkovy;
        private ExTextBox tbCislo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvTrasaZo;
        private ExControls.ExButton bDeleteZo;
        private ExControls.ExButton bAddZo;
        private ExControls.ExButton bNeskorZo;
        private ExControls.ExButton bSkorZo;
        private System.Windows.Forms.ListBox listStaniceZo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgvTrasaDo;
        private ExControls.ExButton bDeleteDo;
        private ExControls.ExButton bAddDo;
        private ExControls.ExButton bNeskorDo;
        private ExControls.ExButton bSkorDo;
        private System.Windows.Forms.ListBox listStaniceDo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TabPage tabPage4;
        private ExControls.ExButton bSave;
        private ExControls.ExButton bZrusit;
        private ExCheckedListBox clbJazyky;
        private ExComboBox cbNazov;
        private System.Windows.Forms.BindingSource stanicaBindingSource;
        private ExGroupBox groupBox4;
        private ExCheckBox boxNizkopodlazny;
        private ExCheckBox boxDialkovy;
        private ExCheckBox boxMimoriadny;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtpPlatnostDo;
        private DateTimePicker dtpPlatnostOd;
        private ExTextBox tbLinkaOdchod;
        private ExTextBox tbLinkaPrichod;
        private System.Windows.Forms.TabPage tabPage5;
        private ExControls.ExButton bDoplnkyEdit;
        private ExControls.ExButton bDoplnkyAdd;
        private System.Windows.Forms.ListBox listVybrateDoplnky;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private ExTextBox tbTextDoplnku;
        private System.Windows.Forms.ListBox listAllDoplnky;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private ExCheckBox cbZoSmeruCustom;
        private ExCheckBox cbDoSmeruCustom;
        private ExControls.ExButton bRadenieDelete;
        private ExControls.ExButton bRadenieEdit;
        private ExTextBox tbRadenie;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.DateTimePicker dtpRadenieDo;
        private System.Windows.Forms.DateTimePicker dtpRadenieOd;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label25;
        private ExTextBox tbDateRemRadenie;
        private System.Windows.Forms.Label label24;
        private ExControls.ExButton bRadenieAdd;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ListBox listRadenia;
        private ExControls.ExButton bEditRadenie;
        private ExControls.ExButton bPlay;
        private ExGroupBox groupBox5;
        private ExNumericUpDown nudVarianta;
        private DataGridViewExCheckBoxColumn isVDlhomHlaseniDataGridViewCheckBoxColumn2;
        private DataGridViewExCheckBoxColumn isVKratkomHlaseniDataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn2;
        private ExControls.ExButton bDoplnkyDelete;
        private System.Windows.Forms.DataGridView dgvDoplnokSet;
        private System.Windows.Forms.DataGridView dgvRadenieSet;
        private ExGroupBox groupBox6;
        private System.Windows.Forms.Label label26;
        private Label lVariantHelp;
        private DataGridViewExCheckBoxColumn isVDlhomHlaseniDataGridViewCheckBoxColumn3;
        private DataGridViewExCheckBoxColumn isVKratkomHlaseniDataGridViewCheckBoxColumn3;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn3;
        private ExMaskedTextBox mtPrichod;
        private ExMaskedTextBox mtOdchod;
        private ExButton bEditLimit;
        private ExButton bEditLimitRadenie;
    }
}