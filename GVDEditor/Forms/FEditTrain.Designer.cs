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
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FEditTrain));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            bSave = new ExButton();
            bZrusit = new ExButton();
            llCalendar = new LinkLabel();
            tabControl1 = new ExTabControl();
            tabPage1 = new TabPage();
            groupBox6 = new ExGroupBox();
            boxPrestup = new ExCheckBox();
            boxDialkovy = new ExCheckBox();
            boxNizkopodlazny = new ExCheckBox();
            boxMiestenkovy = new ExCheckBox();
            boxMimoriadny = new ExCheckBox();
            boxMedzistatny = new ExCheckBox();
            boxLozkovy = new ExCheckBox();
            boxMotorovy = new ExCheckBox();
            groupBox5 = new ExGroupBox();
            lVariantHelp = new Label();
            nudVarianta = new ExNumericUpDown();
            lVyluka = new Label();
            cbVyluka = new ExComboBox();
            groupBox4 = new ExGroupBox();
            clbJazyky = new ExCheckedListBox();
            groupBox3 = new ExGroupBox();
            bEditLimit = new ExButton();
            label16 = new Label();
            label11 = new Label();
            dtpPlatnostDo = new ExDateTimePicker();
            dtpPlatnostOd = new ExDateTimePicker();
            tDatumoveObmedzenie = new ExTextBox();
            groupBox2 = new ExGroupBox();
            mtOdchod = new ExMaskedTextBox();
            mtPrichod = new ExMaskedTextBox();
            tbLinkaOdchod = new ExTextBox();
            tbLinkaPrichod = new ExTextBox();
            label8 = new Label();
            label7 = new Label();
            label10 = new Label();
            label9 = new Label();
            cbKolajOdchod = new ExComboBox();
            cbKolajPrichod = new ExComboBox();
            label4 = new Label();
            label5 = new Label();
            groupBox1 = new ExGroupBox();
            cbNazov = new ExComboBox();
            cbDopravca = new ExComboBox();
            cbTyp = new ExComboBox();
            tbCislo = new ExTextBox();
            label6 = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            tabPage2 = new TabPage();
            cbZoSmeruCustom = new ExCheckBox();
            dgvTrasaZo = new DataGridView();
            isVDlhomHlaseniDataGridViewCheckBoxColumn2 = new DataGridViewExCheckBoxColumn();
            isVKratkomHlaseniDataGridViewCheckBoxColumn2 = new DataGridViewExCheckBoxColumn();
            nameDataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            stanicaBindingSource = new BindingSource(components);
            bDeleteZo = new ExButton();
            bAddZo = new ExButton();
            bNeskorZo = new ExButton();
            bSkorZo = new ExButton();
            listStaniceZo = new ListBox();
            label13 = new Label();
            label12 = new Label();
            tabPage3 = new TabPage();
            cbDoSmeruCustom = new ExCheckBox();
            dgvTrasaDo = new DataGridView();
            isVDlhomHlaseniDataGridViewCheckBoxColumn3 = new DataGridViewExCheckBoxColumn();
            isVKratkomHlaseniDataGridViewCheckBoxColumn3 = new DataGridViewExCheckBoxColumn();
            nameDataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            bDeleteDo = new ExButton();
            bAddDo = new ExButton();
            bNeskorDo = new ExButton();
            bSkorDo = new ExButton();
            listStaniceDo = new ListBox();
            label14 = new Label();
            label15 = new Label();
            tabPage5 = new TabPage();
            dgvDoplnokSet = new DataGridView();
            bDoplnkyDelete = new ExButton();
            label20 = new Label();
            bDoplnkyEdit = new ExButton();
            bDoplnkyAdd = new ExButton();
            listVybrateDoplnky = new ListBox();
            label19 = new Label();
            label18 = new Label();
            tbTextDoplnku = new ExTextBox();
            listAllDoplnky = new ListBox();
            label17 = new Label();
            tabPage4 = new TabPage();
            cbRadenieEndStation = new ExComboBox();
            bEditLimitRadenie = new ExButton();
            label26 = new Label();
            dgvRadenieSet = new DataGridView();
            bPlay = new ExButton();
            bEditRadenie = new ExButton();
            dtpRadenieDo = new ExDateTimePicker();
            dtpRadenieOd = new ExDateTimePicker();
            label27 = new Label();
            label25 = new Label();
            tbDateRemRadenie = new ExTextBox();
            label24 = new Label();
            bRadenieAdd = new ExButton();
            label23 = new Label();
            listRadenia = new ListBox();
            label22 = new Label();
            bRadenieDelete = new ExButton();
            bRadenieEdit = new ExButton();
            tbRadenie = new ExTextBox();
            label21 = new Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox5.SuspendLayout();
            ((ISupportInitialize)nudVarianta).BeginInit();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((ISupportInitialize)dgvTrasaZo).BeginInit();
            ((ISupportInitialize)stanicaBindingSource).BeginInit();
            tabPage3.SuspendLayout();
            ((ISupportInitialize)dgvTrasaDo).BeginInit();
            tabPage5.SuspendLayout();
            ((ISupportInitialize)dgvDoplnokSet).BeginInit();
            tabPage4.SuspendLayout();
            ((ISupportInitialize)dgvRadenieSet).BeginInit();
            SuspendLayout();
            // 
            // bSave
            // 
            resources.ApplyResources(bSave, "bSave");
            bSave.Name = "bSave";
            bSave.UseVisualStyleBackColor = true;
            bSave.Click += bSave_Click;
            // 
            // bZrusit
            // 
            resources.ApplyResources(bZrusit, "bZrusit");
            bZrusit.DialogResult = DialogResult.Cancel;
            bZrusit.Name = "bZrusit";
            bZrusit.UseVisualStyleBackColor = true;
            bZrusit.Click += bZrusit_Click;
            // 
            // llCalendar
            // 
            resources.ApplyResources(llCalendar, "llCalendar");
            llCalendar.Name = "llCalendar";
            llCalendar.TabStop = true;
            llCalendar.LinkClicked += llCalendar_LinkClicked;
            // 
            // tabControl1
            // 
            tabControl1.BorderColor = Color.DarkGray;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.HeaderBackColor = Color.Gainsboro;
            tabControl1.HighlightBackColor = SystemColors.GradientInactiveCaption;
            resources.ApplyResources(tabControl1, "tabControl1");
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.Transparent;
            tabPage1.Controls.Add(groupBox6);
            tabPage1.Controls.Add(groupBox5);
            tabPage1.Controls.Add(groupBox4);
            tabPage1.Controls.Add(groupBox3);
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Controls.Add(groupBox1);
            resources.ApplyResources(tabPage1, "tabPage1");
            tabPage1.Name = "tabPage1";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(boxPrestup);
            groupBox6.Controls.Add(boxDialkovy);
            groupBox6.Controls.Add(boxNizkopodlazny);
            groupBox6.Controls.Add(boxMiestenkovy);
            groupBox6.Controls.Add(boxMimoriadny);
            groupBox6.Controls.Add(boxMedzistatny);
            groupBox6.Controls.Add(boxLozkovy);
            groupBox6.Controls.Add(boxMotorovy);
            groupBox6.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox6, "groupBox6");
            groupBox6.Name = "groupBox6";
            groupBox6.TabStop = false;
            // 
            // boxPrestup
            // 
            resources.ApplyResources(boxPrestup, "boxPrestup");
            boxPrestup.BoxBackColor = Color.White;
            boxPrestup.HighlightColor = SystemColors.Highlight;
            boxPrestup.Name = "boxPrestup";
            boxPrestup.UseVisualStyleBackColor = true;
            // 
            // boxDialkovy
            // 
            resources.ApplyResources(boxDialkovy, "boxDialkovy");
            boxDialkovy.BoxBackColor = Color.White;
            boxDialkovy.HighlightColor = SystemColors.Highlight;
            boxDialkovy.Name = "boxDialkovy";
            boxDialkovy.UseVisualStyleBackColor = true;
            // 
            // boxNizkopodlazny
            // 
            resources.ApplyResources(boxNizkopodlazny, "boxNizkopodlazny");
            boxNizkopodlazny.BoxBackColor = Color.White;
            boxNizkopodlazny.HighlightColor = SystemColors.Highlight;
            boxNizkopodlazny.Name = "boxNizkopodlazny";
            boxNizkopodlazny.UseVisualStyleBackColor = true;
            // 
            // boxMiestenkovy
            // 
            resources.ApplyResources(boxMiestenkovy, "boxMiestenkovy");
            boxMiestenkovy.BoxBackColor = Color.White;
            boxMiestenkovy.HighlightColor = SystemColors.Highlight;
            boxMiestenkovy.Name = "boxMiestenkovy";
            boxMiestenkovy.UseVisualStyleBackColor = true;
            // 
            // boxMimoriadny
            // 
            resources.ApplyResources(boxMimoriadny, "boxMimoriadny");
            boxMimoriadny.BoxBackColor = Color.White;
            boxMimoriadny.HighlightColor = SystemColors.Highlight;
            boxMimoriadny.Name = "boxMimoriadny";
            boxMimoriadny.UseVisualStyleBackColor = true;
            // 
            // boxMedzistatny
            // 
            resources.ApplyResources(boxMedzistatny, "boxMedzistatny");
            boxMedzistatny.BoxBackColor = Color.White;
            boxMedzistatny.HighlightColor = SystemColors.Highlight;
            boxMedzistatny.Name = "boxMedzistatny";
            boxMedzistatny.UseVisualStyleBackColor = true;
            // 
            // boxLozkovy
            // 
            resources.ApplyResources(boxLozkovy, "boxLozkovy");
            boxLozkovy.BoxBackColor = Color.White;
            boxLozkovy.HighlightColor = SystemColors.Highlight;
            boxLozkovy.Name = "boxLozkovy";
            boxLozkovy.UseVisualStyleBackColor = true;
            // 
            // boxMotorovy
            // 
            resources.ApplyResources(boxMotorovy, "boxMotorovy");
            boxMotorovy.BoxBackColor = Color.White;
            boxMotorovy.HighlightColor = SystemColors.Highlight;
            boxMotorovy.Name = "boxMotorovy";
            boxMotorovy.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(lVariantHelp);
            groupBox5.Controls.Add(nudVarianta);
            groupBox5.Controls.Add(lVyluka);
            groupBox5.Controls.Add(cbVyluka);
            groupBox5.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox5, "groupBox5");
            groupBox5.Name = "groupBox5";
            groupBox5.TabStop = false;
            // 
            // lVariantHelp
            // 
            resources.ApplyResources(lVariantHelp, "lVariantHelp");
            lVariantHelp.Name = "lVariantHelp";
            // 
            // nudVarianta
            // 
            nudVarianta.ArrowsColor = Color.Empty;
            nudVarianta.BorderColor = Color.Empty;
            nudVarianta.HighlightColor = Color.Empty;
            resources.ApplyResources(nudVarianta, "nudVarianta");
            nudVarianta.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            nudVarianta.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            nudVarianta.Name = "nudVarianta";
            nudVarianta.SelectedButtonColor = Color.Empty;
            // 
            // lVyluka
            // 
            resources.ApplyResources(lVyluka, "lVyluka");
            lVyluka.Name = "lVyluka";
            // 
            // cbVyluka
            // 
            cbVyluka.DropDownSelectedRowBackColor = Color.Empty;
            cbVyluka.DropDownStyle = ComboBoxStyle.DropDownList;
            cbVyluka.ForeColor = SystemColors.WindowText;
            cbVyluka.FormattingEnabled = true;
            resources.ApplyResources(cbVyluka, "cbVyluka");
            cbVyluka.Name = "cbVyluka";
            cbVyluka.StyleDisabled.ArrowColor = null;
            cbVyluka.StyleDisabled.BackColor = null;
            cbVyluka.StyleDisabled.BorderColor = null;
            cbVyluka.StyleDisabled.ButtonBackColor = null;
            cbVyluka.StyleDisabled.ButtonBorderColor = null;
            cbVyluka.StyleDisabled.ButtonRenderFirst = null;
            cbVyluka.StyleDisabled.ForeColor = null;
            cbVyluka.StyleHighlight.ArrowColor = null;
            cbVyluka.StyleHighlight.BackColor = null;
            cbVyluka.StyleHighlight.BorderColor = null;
            cbVyluka.StyleHighlight.ButtonBackColor = null;
            cbVyluka.StyleHighlight.ButtonBorderColor = null;
            cbVyluka.StyleHighlight.ButtonRenderFirst = null;
            cbVyluka.StyleHighlight.ForeColor = null;
            cbVyluka.StyleNormal.ArrowColor = null;
            cbVyluka.StyleNormal.BackColor = null;
            cbVyluka.StyleNormal.BorderColor = null;
            cbVyluka.StyleNormal.ButtonBackColor = null;
            cbVyluka.StyleNormal.ButtonBorderColor = null;
            cbVyluka.StyleNormal.ButtonRenderFirst = null;
            cbVyluka.StyleNormal.ForeColor = SystemColors.WindowText;
            cbVyluka.StyleSelected.ArrowColor = null;
            cbVyluka.StyleSelected.BackColor = null;
            cbVyluka.StyleSelected.BorderColor = null;
            cbVyluka.StyleSelected.ButtonBackColor = null;
            cbVyluka.StyleSelected.ButtonBorderColor = null;
            cbVyluka.StyleSelected.ButtonRenderFirst = null;
            cbVyluka.StyleSelected.ForeColor = null;
            cbVyluka.UseDarkScrollBar = false;
            cbVyluka.SelectedIndexChanged += cbVyluka_SelectedIndexChanged;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(clbJazyky);
            groupBox4.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox4, "groupBox4");
            groupBox4.Name = "groupBox4";
            groupBox4.TabStop = false;
            // 
            // clbJazyky
            // 
            clbJazyky.FormattingEnabled = true;
            clbJazyky.HighlightColor = Color.FromArgb(0, 120, 215);
            resources.ApplyResources(clbJazyky, "clbJazyky");
            clbJazyky.MultiColumn = true;
            clbJazyky.Name = "clbJazyky";
            clbJazyky.SquareBackColor = Color.White;
            // 
            // groupBox3
            // 
            resources.ApplyResources(groupBox3, "groupBox3");
            groupBox3.Controls.Add(bEditLimit);
            groupBox3.Controls.Add(label16);
            groupBox3.Controls.Add(label11);
            groupBox3.Controls.Add(dtpPlatnostDo);
            groupBox3.Controls.Add(dtpPlatnostOd);
            groupBox3.Controls.Add(tDatumoveObmedzenie);
            groupBox3.DisabledForeColor = SystemColors.GrayText;
            groupBox3.Name = "groupBox3";
            groupBox3.TabStop = false;
            // 
            // bEditLimit
            // 
            resources.ApplyResources(bEditLimit, "bEditLimit");
            bEditLimit.Name = "bEditLimit";
            bEditLimit.UseVisualStyleBackColor = true;
            bEditLimit.Click += BEditLimit_Click;
            // 
            // label16
            // 
            resources.ApplyResources(label16, "label16");
            label16.Name = "label16";
            // 
            // label11
            // 
            resources.ApplyResources(label11, "label11");
            label11.Name = "label11";
            // 
            // dtpPlatnostDo
            // 
            // 
            // 
            // 
            dtpPlatnostDo.Calendar.HighlightColor = Color.Empty;
            dtpPlatnostDo.Calendar.HighlightForeColor = Color.Empty;
            dtpPlatnostDo.Calendar.Location = (Point)resources.GetObject("dtpPlatnostDo.Calendar.Location");
            dtpPlatnostDo.Calendar.Name = "";
            dtpPlatnostDo.Calendar.Size = (Size)resources.GetObject("dtpPlatnostDo.Calendar.Size");
            dtpPlatnostDo.Calendar.TabIndex = (int)resources.GetObject("dtpPlatnostDo.Calendar.TabIndex");
            dtpPlatnostDo.Calendar.TodayBorderColor = Color.Empty;
            dtpPlatnostDo.DisabledBackColor = SystemColors.Control;
            dtpPlatnostDo.DisabledForeColor = SystemColors.GrayText;
            dtpPlatnostDo.Format = DateTimePickerFormat.Short;
            dtpPlatnostDo.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(dtpPlatnostDo, "dtpPlatnostDo");
            dtpPlatnostDo.Name = "dtpPlatnostDo";
            dtpPlatnostDo.SelectedFieldBackColor = SystemColors.Highlight;
            dtpPlatnostDo.SelectedFieldForeColor = SystemColors.HighlightText;
            // 
            // dtpPlatnostOd
            // 
            // 
            // 
            // 
            dtpPlatnostOd.Calendar.HighlightColor = Color.Empty;
            dtpPlatnostOd.Calendar.HighlightForeColor = Color.Empty;
            dtpPlatnostOd.Calendar.Location = (Point)resources.GetObject("dtpPlatnostOd.Calendar.Location");
            dtpPlatnostOd.Calendar.Name = "";
            dtpPlatnostOd.Calendar.Size = (Size)resources.GetObject("dtpPlatnostOd.Calendar.Size");
            dtpPlatnostOd.Calendar.TabIndex = (int)resources.GetObject("dtpPlatnostOd.Calendar.TabIndex");
            dtpPlatnostOd.Calendar.TodayBorderColor = Color.Empty;
            dtpPlatnostOd.DisabledBackColor = SystemColors.Control;
            dtpPlatnostOd.DisabledForeColor = SystemColors.GrayText;
            dtpPlatnostOd.Format = DateTimePickerFormat.Short;
            dtpPlatnostOd.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(dtpPlatnostOd, "dtpPlatnostOd");
            dtpPlatnostOd.Name = "dtpPlatnostOd";
            dtpPlatnostOd.SelectedFieldBackColor = SystemColors.Highlight;
            dtpPlatnostOd.SelectedFieldForeColor = SystemColors.HighlightText;
            // 
            // tDatumoveObmedzenie
            // 
            tDatumoveObmedzenie.BorderColor = Color.DimGray;
            tDatumoveObmedzenie.DisabledBackColor = SystemColors.Control;
            tDatumoveObmedzenie.DisabledBorderColor = SystemColors.InactiveBorder;
            tDatumoveObmedzenie.DisabledForeColor = SystemColors.GrayText;
            tDatumoveObmedzenie.HighlightColor = SystemColors.Highlight;
            tDatumoveObmedzenie.HintForeColor = SystemColors.GrayText;
            tDatumoveObmedzenie.HintText = null;
            resources.ApplyResources(tDatumoveObmedzenie, "tDatumoveObmedzenie");
            tDatumoveObmedzenie.Name = "tDatumoveObmedzenie";
            // 
            // groupBox2
            // 
            resources.ApplyResources(groupBox2, "groupBox2");
            groupBox2.Controls.Add(mtOdchod);
            groupBox2.Controls.Add(mtPrichod);
            groupBox2.Controls.Add(tbLinkaOdchod);
            groupBox2.Controls.Add(tbLinkaPrichod);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(cbKolajOdchod);
            groupBox2.Controls.Add(cbKolajPrichod);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label5);
            groupBox2.DisabledForeColor = SystemColors.GrayText;
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            // 
            // mtOdchod
            // 
            mtOdchod.BeepOnError = true;
            mtOdchod.DisabledBorderColor = SystemColors.InactiveBorder;
            mtOdchod.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(mtOdchod, "mtOdchod");
            mtOdchod.Name = "mtOdchod";
            mtOdchod.ValidatingType = typeof(DateTime);
            // 
            // mtPrichod
            // 
            mtPrichod.BeepOnError = true;
            mtPrichod.DisabledBorderColor = SystemColors.InactiveBorder;
            mtPrichod.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(mtPrichod, "mtPrichod");
            mtPrichod.Name = "mtPrichod";
            mtPrichod.ValidatingType = typeof(DateTime);
            // 
            // tbLinkaOdchod
            // 
            tbLinkaOdchod.BorderColor = Color.DimGray;
            tbLinkaOdchod.DisabledBackColor = SystemColors.Control;
            tbLinkaOdchod.DisabledBorderColor = SystemColors.InactiveBorder;
            tbLinkaOdchod.DisabledForeColor = SystemColors.GrayText;
            tbLinkaOdchod.HighlightColor = SystemColors.Highlight;
            tbLinkaOdchod.HintForeColor = SystemColors.GrayText;
            tbLinkaOdchod.HintText = null;
            resources.ApplyResources(tbLinkaOdchod, "tbLinkaOdchod");
            tbLinkaOdchod.Name = "tbLinkaOdchod";
            tbLinkaOdchod.TextChanged += tbLinkaOdchod_TextChanged;
            // 
            // tbLinkaPrichod
            // 
            tbLinkaPrichod.BorderColor = Color.DimGray;
            tbLinkaPrichod.Cursor = Cursors.IBeam;
            tbLinkaPrichod.DisabledBackColor = SystemColors.Control;
            tbLinkaPrichod.DisabledBorderColor = SystemColors.InactiveBorder;
            tbLinkaPrichod.DisabledForeColor = SystemColors.GrayText;
            tbLinkaPrichod.HighlightColor = SystemColors.Highlight;
            tbLinkaPrichod.HintForeColor = SystemColors.GrayText;
            tbLinkaPrichod.HintText = "";
            resources.ApplyResources(tbLinkaPrichod, "tbLinkaPrichod");
            tbLinkaPrichod.Name = "tbLinkaPrichod";
            tbLinkaPrichod.TextChanged += tbLinkaPrichod_TextChanged;
            // 
            // label8
            // 
            resources.ApplyResources(label8, "label8");
            label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // label10
            // 
            resources.ApplyResources(label10, "label10");
            label10.Name = "label10";
            // 
            // label9
            // 
            resources.ApplyResources(label9, "label9");
            label9.Name = "label9";
            // 
            // cbKolajOdchod
            // 
            cbKolajOdchod.DropDownSelectedRowBackColor = Color.Empty;
            cbKolajOdchod.DropDownStyle = ComboBoxStyle.DropDownList;
            cbKolajOdchod.ForeColor = SystemColors.WindowText;
            cbKolajOdchod.FormattingEnabled = true;
            resources.ApplyResources(cbKolajOdchod, "cbKolajOdchod");
            cbKolajOdchod.Name = "cbKolajOdchod";
            cbKolajOdchod.StyleDisabled.ArrowColor = null;
            cbKolajOdchod.StyleDisabled.BackColor = null;
            cbKolajOdchod.StyleDisabled.BorderColor = null;
            cbKolajOdchod.StyleDisabled.ButtonBackColor = null;
            cbKolajOdchod.StyleDisabled.ButtonBorderColor = null;
            cbKolajOdchod.StyleDisabled.ButtonRenderFirst = null;
            cbKolajOdchod.StyleDisabled.ForeColor = null;
            cbKolajOdchod.StyleHighlight.ArrowColor = null;
            cbKolajOdchod.StyleHighlight.BackColor = null;
            cbKolajOdchod.StyleHighlight.BorderColor = null;
            cbKolajOdchod.StyleHighlight.ButtonBackColor = null;
            cbKolajOdchod.StyleHighlight.ButtonBorderColor = null;
            cbKolajOdchod.StyleHighlight.ButtonRenderFirst = null;
            cbKolajOdchod.StyleHighlight.ForeColor = null;
            cbKolajOdchod.StyleNormal.ArrowColor = null;
            cbKolajOdchod.StyleNormal.BackColor = null;
            cbKolajOdchod.StyleNormal.BorderColor = null;
            cbKolajOdchod.StyleNormal.ButtonBackColor = null;
            cbKolajOdchod.StyleNormal.ButtonBorderColor = null;
            cbKolajOdchod.StyleNormal.ButtonRenderFirst = null;
            cbKolajOdchod.StyleNormal.ForeColor = SystemColors.WindowText;
            cbKolajOdchod.StyleSelected.ArrowColor = null;
            cbKolajOdchod.StyleSelected.BackColor = null;
            cbKolajOdchod.StyleSelected.BorderColor = null;
            cbKolajOdchod.StyleSelected.ButtonBackColor = null;
            cbKolajOdchod.StyleSelected.ButtonBorderColor = null;
            cbKolajOdchod.StyleSelected.ButtonRenderFirst = null;
            cbKolajOdchod.StyleSelected.ForeColor = null;
            cbKolajOdchod.UseDarkScrollBar = false;
            cbKolajOdchod.SelectedIndexChanged += cbKolajOdchod_SelectedIndexChanged;
            // 
            // cbKolajPrichod
            // 
            cbKolajPrichod.DropDownSelectedRowBackColor = Color.Empty;
            cbKolajPrichod.DropDownStyle = ComboBoxStyle.DropDownList;
            cbKolajPrichod.FormattingEnabled = true;
            resources.ApplyResources(cbKolajPrichod, "cbKolajPrichod");
            cbKolajPrichod.Name = "cbKolajPrichod";
            cbKolajPrichod.StyleDisabled.ArrowColor = null;
            cbKolajPrichod.StyleDisabled.BackColor = null;
            cbKolajPrichod.StyleDisabled.BorderColor = null;
            cbKolajPrichod.StyleDisabled.ButtonBackColor = null;
            cbKolajPrichod.StyleDisabled.ButtonBorderColor = null;
            cbKolajPrichod.StyleDisabled.ButtonRenderFirst = null;
            cbKolajPrichod.StyleDisabled.ForeColor = null;
            cbKolajPrichod.StyleHighlight.ArrowColor = null;
            cbKolajPrichod.StyleHighlight.BackColor = null;
            cbKolajPrichod.StyleHighlight.BorderColor = null;
            cbKolajPrichod.StyleHighlight.ButtonBackColor = null;
            cbKolajPrichod.StyleHighlight.ButtonBorderColor = null;
            cbKolajPrichod.StyleHighlight.ButtonRenderFirst = null;
            cbKolajPrichod.StyleHighlight.ForeColor = null;
            cbKolajPrichod.StyleNormal.ArrowColor = null;
            cbKolajPrichod.StyleNormal.BackColor = null;
            cbKolajPrichod.StyleNormal.BorderColor = null;
            cbKolajPrichod.StyleNormal.ButtonBackColor = null;
            cbKolajPrichod.StyleNormal.ButtonBorderColor = null;
            cbKolajPrichod.StyleNormal.ButtonRenderFirst = null;
            cbKolajPrichod.StyleNormal.ForeColor = null;
            cbKolajPrichod.StyleSelected.ArrowColor = null;
            cbKolajPrichod.StyleSelected.BackColor = null;
            cbKolajPrichod.StyleSelected.BorderColor = null;
            cbKolajPrichod.StyleSelected.ButtonBackColor = null;
            cbKolajPrichod.StyleSelected.ButtonBorderColor = null;
            cbKolajPrichod.StyleSelected.ButtonRenderFirst = null;
            cbKolajPrichod.StyleSelected.ForeColor = null;
            cbKolajPrichod.UseDarkScrollBar = false;
            cbKolajPrichod.SelectedIndexChanged += cbKolajPrichod_SelectedIndexChanged;
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // groupBox1
            // 
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Controls.Add(cbNazov);
            groupBox1.Controls.Add(cbDopravca);
            groupBox1.Controls.Add(cbTyp);
            groupBox1.Controls.Add(tbCislo);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label3);
            groupBox1.DisabledForeColor = SystemColors.GrayText;
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // cbNazov
            // 
            cbNazov.DropDownSelectedRowBackColor = Color.Empty;
            cbNazov.FormattingEnabled = true;
            resources.ApplyResources(cbNazov, "cbNazov");
            cbNazov.Name = "cbNazov";
            cbNazov.StyleDisabled.ArrowColor = null;
            cbNazov.StyleDisabled.BackColor = null;
            cbNazov.StyleDisabled.BorderColor = null;
            cbNazov.StyleDisabled.ButtonBackColor = null;
            cbNazov.StyleDisabled.ButtonBorderColor = null;
            cbNazov.StyleDisabled.ButtonRenderFirst = null;
            cbNazov.StyleDisabled.ForeColor = null;
            cbNazov.StyleHighlight.ArrowColor = null;
            cbNazov.StyleHighlight.BackColor = null;
            cbNazov.StyleHighlight.BorderColor = null;
            cbNazov.StyleHighlight.ButtonBackColor = null;
            cbNazov.StyleHighlight.ButtonBorderColor = null;
            cbNazov.StyleHighlight.ButtonRenderFirst = null;
            cbNazov.StyleHighlight.ForeColor = null;
            cbNazov.StyleNormal.ArrowColor = null;
            cbNazov.StyleNormal.BackColor = null;
            cbNazov.StyleNormal.BorderColor = null;
            cbNazov.StyleNormal.ButtonBackColor = null;
            cbNazov.StyleNormal.ButtonBorderColor = null;
            cbNazov.StyleNormal.ButtonRenderFirst = null;
            cbNazov.StyleNormal.ForeColor = null;
            cbNazov.StyleSelected.ArrowColor = null;
            cbNazov.StyleSelected.BackColor = null;
            cbNazov.StyleSelected.BorderColor = null;
            cbNazov.StyleSelected.ButtonBackColor = null;
            cbNazov.StyleSelected.ButtonBorderColor = null;
            cbNazov.StyleSelected.ButtonRenderFirst = null;
            cbNazov.StyleSelected.ForeColor = null;
            cbNazov.UseDarkScrollBar = false;
            // 
            // cbDopravca
            // 
            cbDopravca.DropDownSelectedRowBackColor = Color.Empty;
            cbDopravca.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDopravca.FormattingEnabled = true;
            resources.ApplyResources(cbDopravca, "cbDopravca");
            cbDopravca.Name = "cbDopravca";
            cbDopravca.StyleDisabled.ArrowColor = null;
            cbDopravca.StyleDisabled.BackColor = null;
            cbDopravca.StyleDisabled.BorderColor = null;
            cbDopravca.StyleDisabled.ButtonBackColor = null;
            cbDopravca.StyleDisabled.ButtonBorderColor = null;
            cbDopravca.StyleDisabled.ButtonRenderFirst = null;
            cbDopravca.StyleDisabled.ForeColor = null;
            cbDopravca.StyleHighlight.ArrowColor = null;
            cbDopravca.StyleHighlight.BackColor = null;
            cbDopravca.StyleHighlight.BorderColor = null;
            cbDopravca.StyleHighlight.ButtonBackColor = null;
            cbDopravca.StyleHighlight.ButtonBorderColor = null;
            cbDopravca.StyleHighlight.ButtonRenderFirst = null;
            cbDopravca.StyleHighlight.ForeColor = null;
            cbDopravca.StyleNormal.ArrowColor = null;
            cbDopravca.StyleNormal.BackColor = null;
            cbDopravca.StyleNormal.BorderColor = null;
            cbDopravca.StyleNormal.ButtonBackColor = null;
            cbDopravca.StyleNormal.ButtonBorderColor = null;
            cbDopravca.StyleNormal.ButtonRenderFirst = null;
            cbDopravca.StyleNormal.ForeColor = null;
            cbDopravca.StyleSelected.ArrowColor = null;
            cbDopravca.StyleSelected.BackColor = null;
            cbDopravca.StyleSelected.BorderColor = null;
            cbDopravca.StyleSelected.ButtonBackColor = null;
            cbDopravca.StyleSelected.ButtonBorderColor = null;
            cbDopravca.StyleSelected.ButtonRenderFirst = null;
            cbDopravca.StyleSelected.ForeColor = null;
            cbDopravca.UseDarkScrollBar = false;
            // 
            // cbTyp
            // 
            cbTyp.DropDownSelectedRowBackColor = Color.Empty;
            cbTyp.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTyp.FormattingEnabled = true;
            resources.ApplyResources(cbTyp, "cbTyp");
            cbTyp.Name = "cbTyp";
            cbTyp.StyleDisabled.ArrowColor = null;
            cbTyp.StyleDisabled.BackColor = null;
            cbTyp.StyleDisabled.BorderColor = null;
            cbTyp.StyleDisabled.ButtonBackColor = null;
            cbTyp.StyleDisabled.ButtonBorderColor = null;
            cbTyp.StyleDisabled.ButtonRenderFirst = null;
            cbTyp.StyleDisabled.ForeColor = null;
            cbTyp.StyleHighlight.ArrowColor = null;
            cbTyp.StyleHighlight.BackColor = null;
            cbTyp.StyleHighlight.BorderColor = null;
            cbTyp.StyleHighlight.ButtonBackColor = null;
            cbTyp.StyleHighlight.ButtonBorderColor = null;
            cbTyp.StyleHighlight.ButtonRenderFirst = null;
            cbTyp.StyleHighlight.ForeColor = null;
            cbTyp.StyleNormal.ArrowColor = null;
            cbTyp.StyleNormal.BackColor = null;
            cbTyp.StyleNormal.BorderColor = null;
            cbTyp.StyleNormal.ButtonBackColor = null;
            cbTyp.StyleNormal.ButtonBorderColor = null;
            cbTyp.StyleNormal.ButtonRenderFirst = null;
            cbTyp.StyleNormal.ForeColor = null;
            cbTyp.StyleSelected.ArrowColor = null;
            cbTyp.StyleSelected.BackColor = null;
            cbTyp.StyleSelected.BorderColor = null;
            cbTyp.StyleSelected.ButtonBackColor = null;
            cbTyp.StyleSelected.ButtonBorderColor = null;
            cbTyp.StyleSelected.ButtonRenderFirst = null;
            cbTyp.StyleSelected.ForeColor = null;
            cbTyp.UseDarkScrollBar = false;
            // 
            // tbCislo
            // 
            tbCislo.BackColor = SystemColors.Window;
            tbCislo.BorderColor = Color.DimGray;
            tbCislo.DisabledBackColor = SystemColors.Control;
            tbCislo.DisabledBorderColor = SystemColors.InactiveBorder;
            tbCislo.DisabledForeColor = SystemColors.GrayText;
            tbCislo.HighlightColor = SystemColors.Highlight;
            tbCislo.HintForeColor = SystemColors.GrayText;
            tbCislo.HintText = "";
            resources.ApplyResources(tbCislo, "tbCislo");
            tbCislo.Name = "tbCislo";
            tbCislo.TextChanged += tbCislo_TextChanged;
            tbCislo.Validated += tbCislo_Validated;
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
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
            // tabPage2
            // 
            tabPage2.BackColor = Color.Transparent;
            tabPage2.Controls.Add(cbZoSmeruCustom);
            tabPage2.Controls.Add(dgvTrasaZo);
            tabPage2.Controls.Add(bDeleteZo);
            tabPage2.Controls.Add(bAddZo);
            tabPage2.Controls.Add(bNeskorZo);
            tabPage2.Controls.Add(bSkorZo);
            tabPage2.Controls.Add(listStaniceZo);
            tabPage2.Controls.Add(label13);
            tabPage2.Controls.Add(label12);
            resources.ApplyResources(tabPage2, "tabPage2");
            tabPage2.Name = "tabPage2";
            // 
            // cbZoSmeruCustom
            // 
            resources.ApplyResources(cbZoSmeruCustom, "cbZoSmeruCustom");
            cbZoSmeruCustom.BoxBackColor = Color.White;
            cbZoSmeruCustom.HighlightColor = SystemColors.Highlight;
            cbZoSmeruCustom.Name = "cbZoSmeruCustom";
            cbZoSmeruCustom.UseVisualStyleBackColor = true;
            cbZoSmeruCustom.CheckedChanged += cbZoSmeruCustom_CheckedChanged;
            // 
            // dgvTrasaZo
            // 
            dgvTrasaZo.AllowUserToAddRows = false;
            dgvTrasaZo.AllowUserToResizeRows = false;
            dgvTrasaZo.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvTrasaZo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvTrasaZo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTrasaZo.Columns.AddRange(new DataGridViewColumn[] { isVDlhomHlaseniDataGridViewCheckBoxColumn2, isVKratkomHlaseniDataGridViewCheckBoxColumn2, nameDataGridViewTextBoxColumn2 });
            dgvTrasaZo.DataSource = stanicaBindingSource;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvTrasaZo.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(dgvTrasaZo, "dgvTrasaZo");
            dgvTrasaZo.MultiSelect = false;
            dgvTrasaZo.Name = "dgvTrasaZo";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvTrasaZo.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvTrasaZo.RowHeadersVisible = false;
            dgvTrasaZo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTrasaZo.CellDoubleClick += dgvTrasaZo_CellDoubleClick;
            // 
            // isVDlhomHlaseniDataGridViewCheckBoxColumn2
            // 
            isVDlhomHlaseniDataGridViewCheckBoxColumn2.DataPropertyName = "IsInLongReport";
            resources.ApplyResources(isVDlhomHlaseniDataGridViewCheckBoxColumn2, "isVDlhomHlaseniDataGridViewCheckBoxColumn2");
            isVDlhomHlaseniDataGridViewCheckBoxColumn2.HighlightColor = Color.FromArgb(0, 120, 215);
            isVDlhomHlaseniDataGridViewCheckBoxColumn2.Name = "isVDlhomHlaseniDataGridViewCheckBoxColumn2";
            isVDlhomHlaseniDataGridViewCheckBoxColumn2.SquareBackColor = Color.White;
            // 
            // isVKratkomHlaseniDataGridViewCheckBoxColumn2
            // 
            isVKratkomHlaseniDataGridViewCheckBoxColumn2.DataPropertyName = "IsInShortReport";
            resources.ApplyResources(isVKratkomHlaseniDataGridViewCheckBoxColumn2, "isVKratkomHlaseniDataGridViewCheckBoxColumn2");
            isVKratkomHlaseniDataGridViewCheckBoxColumn2.HighlightColor = Color.FromArgb(0, 120, 215);
            isVKratkomHlaseniDataGridViewCheckBoxColumn2.Name = "isVKratkomHlaseniDataGridViewCheckBoxColumn2";
            isVKratkomHlaseniDataGridViewCheckBoxColumn2.SquareBackColor = Color.White;
            // 
            // nameDataGridViewTextBoxColumn2
            // 
            nameDataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            nameDataGridViewTextBoxColumn2.DataPropertyName = "Name";
            resources.ApplyResources(nameDataGridViewTextBoxColumn2, "nameDataGridViewTextBoxColumn2");
            nameDataGridViewTextBoxColumn2.Name = "nameDataGridViewTextBoxColumn2";
            nameDataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // stanicaBindingSource
            // 
            stanicaBindingSource.DataSource = typeof(Station);
            // 
            // bDeleteZo
            // 
            resources.ApplyResources(bDeleteZo, "bDeleteZo");
            bDeleteZo.Name = "bDeleteZo";
            bDeleteZo.UseVisualStyleBackColor = true;
            bDeleteZo.Click += bDeleteZo_Click;
            // 
            // bAddZo
            // 
            resources.ApplyResources(bAddZo, "bAddZo");
            bAddZo.Name = "bAddZo";
            bAddZo.UseVisualStyleBackColor = true;
            bAddZo.Click += bAddZo_Click;
            // 
            // bNeskorZo
            // 
            resources.ApplyResources(bNeskorZo, "bNeskorZo");
            bNeskorZo.Name = "bNeskorZo";
            bNeskorZo.UseVisualStyleBackColor = true;
            bNeskorZo.Click += bNeskorZo_Click;
            // 
            // bSkorZo
            // 
            resources.ApplyResources(bSkorZo, "bSkorZo");
            bSkorZo.Name = "bSkorZo";
            bSkorZo.UseVisualStyleBackColor = true;
            bSkorZo.Click += bSkorZo_Click;
            // 
            // listStaniceZo
            // 
            listStaniceZo.FormattingEnabled = true;
            resources.ApplyResources(listStaniceZo, "listStaniceZo");
            listStaniceZo.Name = "listStaniceZo";
            listStaniceZo.Sorted = true;
            listStaniceZo.DoubleClick += listStaniceZo_DoubleClick;
            listStaniceZo.KeyPress += listStaniceZo_KeyPress;
            // 
            // label13
            // 
            resources.ApplyResources(label13, "label13");
            label13.Name = "label13";
            // 
            // label12
            // 
            resources.ApplyResources(label12, "label12");
            label12.Name = "label12";
            // 
            // tabPage3
            // 
            tabPage3.BackColor = Color.Transparent;
            tabPage3.Controls.Add(cbDoSmeruCustom);
            tabPage3.Controls.Add(dgvTrasaDo);
            tabPage3.Controls.Add(bDeleteDo);
            tabPage3.Controls.Add(bAddDo);
            tabPage3.Controls.Add(bNeskorDo);
            tabPage3.Controls.Add(bSkorDo);
            tabPage3.Controls.Add(listStaniceDo);
            tabPage3.Controls.Add(label14);
            tabPage3.Controls.Add(label15);
            resources.ApplyResources(tabPage3, "tabPage3");
            tabPage3.Name = "tabPage3";
            // 
            // cbDoSmeruCustom
            // 
            resources.ApplyResources(cbDoSmeruCustom, "cbDoSmeruCustom");
            cbDoSmeruCustom.BoxBackColor = Color.White;
            cbDoSmeruCustom.HighlightColor = SystemColors.Highlight;
            cbDoSmeruCustom.Name = "cbDoSmeruCustom";
            cbDoSmeruCustom.UseVisualStyleBackColor = true;
            cbDoSmeruCustom.CheckedChanged += cbDoSmeruCustom_CheckedChanged;
            // 
            // dgvTrasaDo
            // 
            dgvTrasaDo.AllowUserToAddRows = false;
            dgvTrasaDo.AllowUserToResizeRows = false;
            dgvTrasaDo.AutoGenerateColumns = false;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvTrasaDo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvTrasaDo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTrasaDo.Columns.AddRange(new DataGridViewColumn[] { isVDlhomHlaseniDataGridViewCheckBoxColumn3, isVKratkomHlaseniDataGridViewCheckBoxColumn3, nameDataGridViewTextBoxColumn3 });
            dgvTrasaDo.DataSource = stanicaBindingSource;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvTrasaDo.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(dgvTrasaDo, "dgvTrasaDo");
            dgvTrasaDo.MultiSelect = false;
            dgvTrasaDo.Name = "dgvTrasaDo";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Control;
            dataGridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            dataGridViewCellStyle6.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvTrasaDo.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvTrasaDo.RowHeadersVisible = false;
            dgvTrasaDo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTrasaDo.CellDoubleClick += dgvTrasaDo_CellDoubleClick;
            // 
            // isVDlhomHlaseniDataGridViewCheckBoxColumn3
            // 
            isVDlhomHlaseniDataGridViewCheckBoxColumn3.DataPropertyName = "IsInLongReport";
            resources.ApplyResources(isVDlhomHlaseniDataGridViewCheckBoxColumn3, "isVDlhomHlaseniDataGridViewCheckBoxColumn3");
            isVDlhomHlaseniDataGridViewCheckBoxColumn3.HighlightColor = Color.FromArgb(0, 120, 215);
            isVDlhomHlaseniDataGridViewCheckBoxColumn3.Name = "isVDlhomHlaseniDataGridViewCheckBoxColumn3";
            isVDlhomHlaseniDataGridViewCheckBoxColumn3.SquareBackColor = Color.White;
            // 
            // isVKratkomHlaseniDataGridViewCheckBoxColumn3
            // 
            isVKratkomHlaseniDataGridViewCheckBoxColumn3.DataPropertyName = "IsInShortReport";
            resources.ApplyResources(isVKratkomHlaseniDataGridViewCheckBoxColumn3, "isVKratkomHlaseniDataGridViewCheckBoxColumn3");
            isVKratkomHlaseniDataGridViewCheckBoxColumn3.HighlightColor = Color.FromArgb(0, 120, 215);
            isVKratkomHlaseniDataGridViewCheckBoxColumn3.Name = "isVKratkomHlaseniDataGridViewCheckBoxColumn3";
            isVKratkomHlaseniDataGridViewCheckBoxColumn3.SquareBackColor = Color.White;
            // 
            // nameDataGridViewTextBoxColumn3
            // 
            nameDataGridViewTextBoxColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            nameDataGridViewTextBoxColumn3.DataPropertyName = "Name";
            resources.ApplyResources(nameDataGridViewTextBoxColumn3, "nameDataGridViewTextBoxColumn3");
            nameDataGridViewTextBoxColumn3.Name = "nameDataGridViewTextBoxColumn3";
            nameDataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // bDeleteDo
            // 
            resources.ApplyResources(bDeleteDo, "bDeleteDo");
            bDeleteDo.Name = "bDeleteDo";
            bDeleteDo.UseVisualStyleBackColor = true;
            bDeleteDo.Click += bDeleteDo_Click;
            // 
            // bAddDo
            // 
            resources.ApplyResources(bAddDo, "bAddDo");
            bAddDo.Name = "bAddDo";
            bAddDo.UseVisualStyleBackColor = true;
            bAddDo.Click += bAddDo_Click;
            // 
            // bNeskorDo
            // 
            resources.ApplyResources(bNeskorDo, "bNeskorDo");
            bNeskorDo.Name = "bNeskorDo";
            bNeskorDo.UseVisualStyleBackColor = true;
            bNeskorDo.Click += bNeskorDo_Click;
            // 
            // bSkorDo
            // 
            resources.ApplyResources(bSkorDo, "bSkorDo");
            bSkorDo.Name = "bSkorDo";
            bSkorDo.UseVisualStyleBackColor = true;
            bSkorDo.Click += bSkorDo_Click;
            // 
            // listStaniceDo
            // 
            listStaniceDo.FormattingEnabled = true;
            resources.ApplyResources(listStaniceDo, "listStaniceDo");
            listStaniceDo.Name = "listStaniceDo";
            listStaniceDo.Sorted = true;
            listStaniceDo.DoubleClick += listStaniceDo_DoubleClick;
            listStaniceDo.KeyPress += listStaniceDo_KeyPress;
            // 
            // label14
            // 
            resources.ApplyResources(label14, "label14");
            label14.Name = "label14";
            // 
            // label15
            // 
            resources.ApplyResources(label15, "label15");
            label15.Name = "label15";
            // 
            // tabPage5
            // 
            tabPage5.BackColor = Color.Transparent;
            tabPage5.Controls.Add(dgvDoplnokSet);
            tabPage5.Controls.Add(bDoplnkyDelete);
            tabPage5.Controls.Add(label20);
            tabPage5.Controls.Add(bDoplnkyEdit);
            tabPage5.Controls.Add(bDoplnkyAdd);
            tabPage5.Controls.Add(listVybrateDoplnky);
            tabPage5.Controls.Add(label19);
            tabPage5.Controls.Add(label18);
            tabPage5.Controls.Add(tbTextDoplnku);
            tabPage5.Controls.Add(listAllDoplnky);
            tabPage5.Controls.Add(label17);
            resources.ApplyResources(tabPage5, "tabPage5");
            tabPage5.Name = "tabPage5";
            // 
            // dgvDoplnokSet
            // 
            dgvDoplnokSet.AllowUserToAddRows = false;
            dgvDoplnokSet.AllowUserToDeleteRows = false;
            dgvDoplnokSet.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = SystemColors.Control;
            dataGridViewCellStyle7.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            dataGridViewCellStyle7.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            dgvDoplnokSet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dgvDoplnokSet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            dgvDoplnokSet.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(dgvDoplnokSet, "dgvDoplnokSet");
            dgvDoplnokSet.Name = "dgvDoplnokSet";
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = SystemColors.Control;
            dataGridViewCellStyle9.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            dgvDoplnokSet.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dgvDoplnokSet.RowHeadersVisible = false;
            dgvDoplnokSet.RowTemplate.Height = 24;
            // 
            // bDoplnkyDelete
            // 
            resources.ApplyResources(bDoplnkyDelete, "bDoplnkyDelete");
            bDoplnkyDelete.Name = "bDoplnkyDelete";
            bDoplnkyDelete.UseVisualStyleBackColor = true;
            bDoplnkyDelete.Click += bDoplnkyDelete_Click;
            // 
            // label20
            // 
            resources.ApplyResources(label20, "label20");
            label20.Name = "label20";
            // 
            // bDoplnkyEdit
            // 
            resources.ApplyResources(bDoplnkyEdit, "bDoplnkyEdit");
            bDoplnkyEdit.Name = "bDoplnkyEdit";
            bDoplnkyEdit.UseVisualStyleBackColor = true;
            bDoplnkyEdit.Click += bDoplnkyEdit_Click;
            // 
            // bDoplnkyAdd
            // 
            resources.ApplyResources(bDoplnkyAdd, "bDoplnkyAdd");
            bDoplnkyAdd.Name = "bDoplnkyAdd";
            bDoplnkyAdd.UseVisualStyleBackColor = true;
            bDoplnkyAdd.Click += bDoplnkyAdd_Click;
            // 
            // listVybrateDoplnky
            // 
            listVybrateDoplnky.FormattingEnabled = true;
            resources.ApplyResources(listVybrateDoplnky, "listVybrateDoplnky");
            listVybrateDoplnky.Name = "listVybrateDoplnky";
            listVybrateDoplnky.SelectedIndexChanged += listVybrateDoplnky_SelectedIndexChanged;
            listVybrateDoplnky.DoubleClick += listVybrateDoplnky_DoubleClick;
            // 
            // label19
            // 
            resources.ApplyResources(label19, "label19");
            label19.Name = "label19";
            // 
            // label18
            // 
            resources.ApplyResources(label18, "label18");
            label18.Name = "label18";
            // 
            // tbTextDoplnku
            // 
            tbTextDoplnku.BorderColor = Color.DimGray;
            tbTextDoplnku.BorderStyle = BorderStyle.FixedSingle;
            tbTextDoplnku.DisabledBackColor = SystemColors.Control;
            tbTextDoplnku.DisabledBorderColor = SystemColors.InactiveBorder;
            tbTextDoplnku.DisabledForeColor = SystemColors.GrayText;
            tbTextDoplnku.HighlightColor = SystemColors.Highlight;
            tbTextDoplnku.HintForeColor = SystemColors.GrayText;
            tbTextDoplnku.HintText = null;
            resources.ApplyResources(tbTextDoplnku, "tbTextDoplnku");
            tbTextDoplnku.Name = "tbTextDoplnku";
            tbTextDoplnku.ReadOnly = true;
            // 
            // listAllDoplnky
            // 
            listAllDoplnky.FormattingEnabled = true;
            resources.ApplyResources(listAllDoplnky, "listAllDoplnky");
            listAllDoplnky.Name = "listAllDoplnky";
            listAllDoplnky.SelectedIndexChanged += listAllDoplnky_SelectedIndexChanged;
            listAllDoplnky.DoubleClick += listAllDoplnky_DoubleClick;
            // 
            // label17
            // 
            resources.ApplyResources(label17, "label17");
            label17.Name = "label17";
            // 
            // tabPage4
            // 
            tabPage4.BackColor = Color.Transparent;
            tabPage4.Controls.Add(cbRadenieEndStation);
            tabPage4.Controls.Add(bEditLimitRadenie);
            tabPage4.Controls.Add(label26);
            tabPage4.Controls.Add(dgvRadenieSet);
            tabPage4.Controls.Add(bPlay);
            tabPage4.Controls.Add(bEditRadenie);
            tabPage4.Controls.Add(dtpRadenieDo);
            tabPage4.Controls.Add(dtpRadenieOd);
            tabPage4.Controls.Add(label27);
            tabPage4.Controls.Add(label25);
            tabPage4.Controls.Add(tbDateRemRadenie);
            tabPage4.Controls.Add(label24);
            tabPage4.Controls.Add(bRadenieAdd);
            tabPage4.Controls.Add(label23);
            tabPage4.Controls.Add(listRadenia);
            tabPage4.Controls.Add(label22);
            tabPage4.Controls.Add(bRadenieDelete);
            tabPage4.Controls.Add(bRadenieEdit);
            tabPage4.Controls.Add(tbRadenie);
            tabPage4.Controls.Add(label21);
            resources.ApplyResources(tabPage4, "tabPage4");
            tabPage4.Name = "tabPage4";
            // 
            // cbRadenieEndStation
            // 
            cbRadenieEndStation.DropDownSelectedRowBackColor = SystemColors.Highlight;
            cbRadenieEndStation.FormattingEnabled = true;
            resources.ApplyResources(cbRadenieEndStation, "cbRadenieEndStation");
            cbRadenieEndStation.Name = "cbRadenieEndStation";
            cbRadenieEndStation.StyleDisabled.ArrowColor = null;
            cbRadenieEndStation.StyleDisabled.BackColor = null;
            cbRadenieEndStation.StyleDisabled.BorderColor = null;
            cbRadenieEndStation.StyleDisabled.ButtonBackColor = null;
            cbRadenieEndStation.StyleDisabled.ButtonBorderColor = null;
            cbRadenieEndStation.StyleDisabled.ButtonRenderFirst = null;
            cbRadenieEndStation.StyleDisabled.ForeColor = null;
            cbRadenieEndStation.StyleHighlight.ArrowColor = null;
            cbRadenieEndStation.StyleHighlight.BackColor = null;
            cbRadenieEndStation.StyleHighlight.BorderColor = null;
            cbRadenieEndStation.StyleHighlight.ButtonBackColor = null;
            cbRadenieEndStation.StyleHighlight.ButtonBorderColor = null;
            cbRadenieEndStation.StyleHighlight.ButtonRenderFirst = null;
            cbRadenieEndStation.StyleHighlight.ForeColor = null;
            cbRadenieEndStation.StyleNormal.ArrowColor = null;
            cbRadenieEndStation.StyleNormal.BackColor = null;
            cbRadenieEndStation.StyleNormal.BorderColor = null;
            cbRadenieEndStation.StyleNormal.ButtonBackColor = null;
            cbRadenieEndStation.StyleNormal.ButtonBorderColor = null;
            cbRadenieEndStation.StyleNormal.ButtonRenderFirst = null;
            cbRadenieEndStation.StyleNormal.ForeColor = null;
            cbRadenieEndStation.StyleSelected.ArrowColor = null;
            cbRadenieEndStation.StyleSelected.BackColor = null;
            cbRadenieEndStation.StyleSelected.BorderColor = null;
            cbRadenieEndStation.StyleSelected.ButtonBackColor = null;
            cbRadenieEndStation.StyleSelected.ButtonBorderColor = null;
            cbRadenieEndStation.StyleSelected.ButtonRenderFirst = null;
            cbRadenieEndStation.StyleSelected.ForeColor = null;
            cbRadenieEndStation.UseDarkScrollBar = false;
            // 
            // bEditLimitRadenie
            // 
            resources.ApplyResources(bEditLimitRadenie, "bEditLimitRadenie");
            bEditLimitRadenie.Name = "bEditLimitRadenie";
            bEditLimitRadenie.UseVisualStyleBackColor = true;
            bEditLimitRadenie.Click += BEditLimitRadenie_Click;
            // 
            // label26
            // 
            resources.ApplyResources(label26, "label26");
            label26.Name = "label26";
            // 
            // dgvRadenieSet
            // 
            dgvRadenieSet.AllowUserToAddRows = false;
            dgvRadenieSet.AllowUserToDeleteRows = false;
            dgvRadenieSet.AllowUserToResizeRows = false;
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = SystemColors.Control;
            dataGridViewCellStyle10.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            dataGridViewCellStyle10.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            dgvRadenieSet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            dgvRadenieSet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = SystemColors.Window;
            dataGridViewCellStyle11.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            dataGridViewCellStyle11.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
            dgvRadenieSet.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(dgvRadenieSet, "dgvRadenieSet");
            dgvRadenieSet.Name = "dgvRadenieSet";
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = SystemColors.Control;
            dataGridViewCellStyle12.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            dataGridViewCellStyle12.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = DataGridViewTriState.True;
            dgvRadenieSet.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            dgvRadenieSet.RowHeadersVisible = false;
            dgvRadenieSet.RowTemplate.Height = 24;
            // 
            // bPlay
            // 
            resources.ApplyResources(bPlay, "bPlay");
            bPlay.Name = "bPlay";
            bPlay.UseVisualStyleBackColor = true;
            bPlay.Click += bPlay_Click;
            // 
            // bEditRadenie
            // 
            resources.ApplyResources(bEditRadenie, "bEditRadenie");
            bEditRadenie.Name = "bEditRadenie";
            bEditRadenie.UseVisualStyleBackColor = true;
            bEditRadenie.Click += bEditRadenie_Click;
            // 
            // dtpRadenieDo
            // 
            // 
            // 
            // 
            dtpRadenieDo.Calendar.HighlightColor = Color.Empty;
            dtpRadenieDo.Calendar.HighlightForeColor = Color.Empty;
            dtpRadenieDo.Calendar.Location = (Point)resources.GetObject("dtpRadenieDo.Calendar.Location");
            dtpRadenieDo.Calendar.Name = "";
            dtpRadenieDo.Calendar.Size = (Size)resources.GetObject("dtpRadenieDo.Calendar.Size");
            dtpRadenieDo.Calendar.TabIndex = (int)resources.GetObject("dtpRadenieDo.Calendar.TabIndex");
            dtpRadenieDo.Calendar.TodayBorderColor = Color.Empty;
            dtpRadenieDo.DisabledBackColor = SystemColors.Control;
            dtpRadenieDo.DisabledForeColor = SystemColors.GrayText;
            dtpRadenieDo.Format = DateTimePickerFormat.Short;
            dtpRadenieDo.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(dtpRadenieDo, "dtpRadenieDo");
            dtpRadenieDo.Name = "dtpRadenieDo";
            dtpRadenieDo.SelectedFieldBackColor = SystemColors.Highlight;
            dtpRadenieDo.SelectedFieldForeColor = SystemColors.HighlightText;
            // 
            // dtpRadenieOd
            // 
            // 
            // 
            // 
            dtpRadenieOd.Calendar.HighlightColor = Color.Empty;
            dtpRadenieOd.Calendar.HighlightForeColor = Color.Empty;
            dtpRadenieOd.Calendar.Location = (Point)resources.GetObject("dtpRadenieOd.Calendar.Location");
            dtpRadenieOd.Calendar.Name = "";
            dtpRadenieOd.Calendar.Size = (Size)resources.GetObject("dtpRadenieOd.Calendar.Size");
            dtpRadenieOd.Calendar.TabIndex = (int)resources.GetObject("dtpRadenieOd.Calendar.TabIndex");
            dtpRadenieOd.Calendar.TodayBorderColor = Color.Empty;
            dtpRadenieOd.DisabledBackColor = SystemColors.Control;
            dtpRadenieOd.DisabledForeColor = SystemColors.GrayText;
            dtpRadenieOd.Format = DateTimePickerFormat.Short;
            dtpRadenieOd.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(dtpRadenieOd, "dtpRadenieOd");
            dtpRadenieOd.Name = "dtpRadenieOd";
            dtpRadenieOd.SelectedFieldBackColor = SystemColors.Highlight;
            dtpRadenieOd.SelectedFieldForeColor = SystemColors.HighlightText;
            // 
            // label27
            // 
            resources.ApplyResources(label27, "label27");
            label27.Name = "label27";
            // 
            // label25
            // 
            resources.ApplyResources(label25, "label25");
            label25.Name = "label25";
            // 
            // tbDateRemRadenie
            // 
            tbDateRemRadenie.BorderColor = Color.DimGray;
            tbDateRemRadenie.DisabledBackColor = SystemColors.Control;
            tbDateRemRadenie.DisabledBorderColor = SystemColors.InactiveBorder;
            tbDateRemRadenie.DisabledForeColor = SystemColors.GrayText;
            tbDateRemRadenie.HighlightColor = SystemColors.Highlight;
            tbDateRemRadenie.HintForeColor = SystemColors.GrayText;
            tbDateRemRadenie.HintText = null;
            resources.ApplyResources(tbDateRemRadenie, "tbDateRemRadenie");
            tbDateRemRadenie.Name = "tbDateRemRadenie";
            // 
            // label24
            // 
            resources.ApplyResources(label24, "label24");
            label24.Name = "label24";
            // 
            // bRadenieAdd
            // 
            resources.ApplyResources(bRadenieAdd, "bRadenieAdd");
            bRadenieAdd.Name = "bRadenieAdd";
            bRadenieAdd.UseVisualStyleBackColor = true;
            bRadenieAdd.Click += bRadenieAdd_Click;
            // 
            // label23
            // 
            resources.ApplyResources(label23, "label23");
            label23.Name = "label23";
            // 
            // listRadenia
            // 
            listRadenia.FormattingEnabled = true;
            resources.ApplyResources(listRadenia, "listRadenia");
            listRadenia.Name = "listRadenia";
            listRadenia.SelectedIndexChanged += listRadenia_SelectedIndexChanged;
            listRadenia.Format += listRadenia_Format;
            // 
            // label22
            // 
            resources.ApplyResources(label22, "label22");
            label22.Name = "label22";
            // 
            // bRadenieDelete
            // 
            resources.ApplyResources(bRadenieDelete, "bRadenieDelete");
            bRadenieDelete.Name = "bRadenieDelete";
            bRadenieDelete.UseVisualStyleBackColor = true;
            bRadenieDelete.Click += bRadenieDelete_Click;
            // 
            // bRadenieEdit
            // 
            resources.ApplyResources(bRadenieEdit, "bRadenieEdit");
            bRadenieEdit.Name = "bRadenieEdit";
            bRadenieEdit.UseVisualStyleBackColor = true;
            bRadenieEdit.Click += bRadenieEdit_Click;
            // 
            // tbRadenie
            // 
            tbRadenie.BorderColor = Color.DimGray;
            tbRadenie.BorderStyle = BorderStyle.FixedSingle;
            tbRadenie.DisabledBackColor = SystemColors.Control;
            tbRadenie.DisabledBorderColor = SystemColors.InactiveBorder;
            tbRadenie.DisabledForeColor = SystemColors.GrayText;
            tbRadenie.HighlightColor = SystemColors.Highlight;
            tbRadenie.HintForeColor = SystemColors.GrayText;
            tbRadenie.HintText = null;
            resources.ApplyResources(tbRadenie, "tbRadenie");
            tbRadenie.Name = "tbRadenie";
            tbRadenie.ReadOnly = true;
            // 
            // label21
            // 
            resources.ApplyResources(label21, "label21");
            label21.Name = "label21";
            // 
            // FEditTrain
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(bSave);
            Controls.Add(llCalendar);
            Controls.Add(bZrusit);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            HelpButton = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FEditTrain";
            ShowIcon = false;
            ShowInTaskbar = false;
            HelpButtonClicked += FEditTrain_HelpButtonClicked;
            Load += FEditTrain_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ((ISupportInitialize)nudVarianta).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((ISupportInitialize)dgvTrasaZo).EndInit();
            ((ISupportInitialize)stanicaBindingSource).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ((ISupportInitialize)dgvTrasaDo).EndInit();
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            ((ISupportInitialize)dgvDoplnokSet).EndInit();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ((ISupportInitialize)dgvRadenieSet).EndInit();
            ResumeLayout(false);
            PerformLayout();

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
        private System.Windows.Forms.LinkLabel llCalendar;
        private ExCheckedListBox clbJazyky;
        private ExComboBox cbNazov;
        private System.Windows.Forms.BindingSource stanicaBindingSource;
        private ExGroupBox groupBox4;
        private ExCheckBox boxNizkopodlazny;
        private ExCheckBox boxPrestup;
        private ExControls.ExCheckBox boxMotorovy;
        private ExCheckBox boxDialkovy;
        private ExCheckBox boxMimoriadny;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label11;
        private ExControls.ExDateTimePicker dtpPlatnostDo;
        private ExControls.ExDateTimePicker dtpPlatnostOd;
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
        private ExControls.ExDateTimePicker dtpRadenieDo;
        private ExControls.ExDateTimePicker dtpRadenieOd;
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
        private ExControls.ExComboBox cbVyluka;
        private DataGridViewExCheckBoxColumn isVDlhomHlaseniDataGridViewCheckBoxColumn2;
        private DataGridViewExCheckBoxColumn isVKratkomHlaseniDataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn2;
        private ExControls.ExButton bDoplnkyDelete;
        private System.Windows.Forms.DataGridView dgvDoplnokSet;
        private System.Windows.Forms.DataGridView dgvRadenieSet;
        private ExGroupBox groupBox6;
        private System.Windows.Forms.Label label26;
        private Label lVariantHelp;
        private System.Windows.Forms.Label lVyluka;
        private DataGridViewExCheckBoxColumn isVDlhomHlaseniDataGridViewCheckBoxColumn3;
        private DataGridViewExCheckBoxColumn isVKratkomHlaseniDataGridViewCheckBoxColumn3;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn3;
        private ExMaskedTextBox mtPrichod;
        private ExMaskedTextBox mtOdchod;
        private ExButton bEditLimit;
        private ExButton bEditLimitRadenie;
        private ExComboBox cbRadenieEndStation;
    }
}