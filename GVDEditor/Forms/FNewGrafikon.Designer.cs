using ExControls;
using GVDEditor.Entities;

namespace GVDEditor.Forms
{
    partial class FNewGrafikon
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FNewGrafikon));
            this.groupBox1 = new ExControls.ExGroupBox();
            this.tbDirIniss = new ExControls.ExTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Grafikon = new ExControls.ExGroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpGVDDo = new System.Windows.Forms.DateTimePicker();
            this.dtpGVDOd = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpDataDo = new System.Windows.Forms.DateTimePicker();
            this.dtpDataOd = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDirName = new ExControls.ExTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbStationName = new ExControls.ExComboBox();
            this.stanicaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.bCreate = new ExControls.ExButton();
            this.bZrusit = new ExControls.ExButton();
            this.groupBox2 = new ExControls.ExGroupBox();
            this.rbNewObd = new ExControls.ExRadioButton();
            this.rbNewStation = new ExControls.ExRadioButton();
            this.groupBox3 = new ExControls.ExGroupBox();
            this.bEditColor = new ExControls.ExButton();
            this.pbColor = new System.Windows.Forms.PictureBox();
            this.nudHlaseniePort = new ExControls.ExNumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.nudTabPort = new ExControls.ExNumericUpDown();
            this.colorDialogFarba = new System.Windows.Forms.ColorDialog();
            this.cbCustomStation = new ExControls.ExCheckBox();
            this.nudIDStation = new ExControls.ExNumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tbStationName = new ExControls.ExTextBox();
            this.groupBox4 = new ExControls.ExGroupBox();
            this.dynamicLineSeparator1 = new ExControls.ExLineSeparator();
            this.groupBox1.SuspendLayout();
            this.Grafikon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stanicaBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHlaseniePort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTabPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIDStation)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbDirIniss);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tbDirIniss
            // 
            this.tbDirIniss.BorderColor = System.Drawing.Color.DimGray;
            this.tbDirIniss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDirIniss.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbDirIniss.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbDirIniss.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDirIniss.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbDirIniss.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDirIniss.HintText = null;
            resources.ApplyResources(this.tbDirIniss, "tbDirIniss");
            this.tbDirIniss.Name = "tbDirIniss";
            this.tbDirIniss.ReadOnly = true;
            this.tbDirIniss.TextChanged += new System.EventHandler(this.tbDirIniss_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Grafikon
            // 
            this.Grafikon.Controls.Add(this.label7);
            this.Grafikon.Controls.Add(this.dtpGVDDo);
            this.Grafikon.Controls.Add(this.dtpGVDOd);
            this.Grafikon.Controls.Add(this.label6);
            this.Grafikon.Controls.Add(this.dtpDataDo);
            this.Grafikon.Controls.Add(this.dtpDataOd);
            this.Grafikon.Controls.Add(this.label3);
            this.Grafikon.Controls.Add(this.label2);
            resources.ApplyResources(this.Grafikon, "Grafikon");
            this.Grafikon.Name = "Grafikon";
            this.Grafikon.TabStop = false;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // dtpGVDDo
            // 
            this.dtpGVDDo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpGVDDo, "dtpGVDDo");
            this.dtpGVDDo.Name = "dtpGVDDo";
            // 
            // dtpGVDOd
            // 
            this.dtpGVDOd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpGVDOd, "dtpGVDOd");
            this.dtpGVDOd.Name = "dtpGVDOd";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // dtpDataDo
            // 
            this.dtpDataDo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpDataDo, "dtpDataDo");
            this.dtpDataDo.Name = "dtpDataDo";
            // 
            // dtpDataOd
            // 
            this.dtpDataOd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpDataOd, "dtpDataOd");
            this.dtpDataOd.Name = "dtpDataOd";
            this.dtpDataOd.ValueChanged += new System.EventHandler(this.dtpZacPlatnosti_ValueChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbDirName
            // 
            this.tbDirName.BorderColor = System.Drawing.Color.DimGray;
            this.tbDirName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDirName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbDirName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbDirName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDirName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbDirName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDirName.HintText = null;
            resources.ApplyResources(this.tbDirName, "tbDirName");
            this.tbDirName.Name = "tbDirName";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cbStationName
            // 
            this.cbStationName.DataSource = this.stanicaBindingSource;
            this.cbStationName.DisplayMember = "Name";
            this.cbStationName.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
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
            this.cbStationName.StyleSelected.ArrowColor = null;
            this.cbStationName.StyleSelected.BackColor = null;
            this.cbStationName.StyleSelected.BorderColor = null;
            this.cbStationName.StyleSelected.ButtonBackColor = null;
            this.cbStationName.StyleSelected.ButtonBorderColor = null;
            this.cbStationName.StyleSelected.ButtonRenderFirst = null;
            this.cbStationName.StyleSelected.ForeColor = null;
            this.cbStationName.UseDarkScrollBar = false;
            this.cbStationName.SelectedIndexChanged += new System.EventHandler(this.cbStationName_SelectedIndexChanged);
            // 
            // stanicaBindingSource
            // 
            this.stanicaBindingSource.DataSource = typeof(GVDEditor.Entities.Station);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // bCreate
            // 
            resources.ApplyResources(this.bCreate, "bCreate");
            this.bCreate.Name = "bCreate";
            this.bCreate.UseVisualStyleBackColor = true;
            this.bCreate.Click += new System.EventHandler(this.bCreate_Click);
            // 
            // bZrusit
            // 
            resources.ApplyResources(this.bZrusit, "bZrusit");
            this.bZrusit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bZrusit.Name = "bZrusit";
            this.bZrusit.UseVisualStyleBackColor = true;
            this.bZrusit.Click += new System.EventHandler(this.bZrusit_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbNewObd);
            this.groupBox2.Controls.Add(this.rbNewStation);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // rbNewObd
            // 
            resources.ApplyResources(this.rbNewObd, "rbNewObd");
            this.rbNewObd.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbNewObd.Name = "rbNewObd";
            this.rbNewObd.UseVisualStyleBackColor = true;
            // 
            // rbNewStation
            // 
            resources.ApplyResources(this.rbNewStation, "rbNewStation");
            this.rbNewStation.BorderColor = System.Drawing.Color.Empty;
            this.rbNewStation.BoxBackColor = System.Drawing.Color.Empty;
            this.rbNewStation.Checked = true;
            this.rbNewStation.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbNewStation.MarkColor = System.Drawing.Color.Empty;
            this.rbNewStation.Name = "rbNewStation";
            this.rbNewStation.TabStop = true;
            this.rbNewStation.UseVisualStyleBackColor = true;
            this.rbNewStation.CheckedChanged += new System.EventHandler(this.rbNewStation_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.bEditColor);
            this.groupBox3.Controls.Add(this.pbColor);
            this.groupBox3.Controls.Add(this.nudHlaseniePort);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.nudTabPort);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
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
            // nudHlaseniePort
            // 
            this.nudHlaseniePort.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudHlaseniePort, "nudHlaseniePort");
            this.nudHlaseniePort.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudHlaseniePort.Name = "nudHlaseniePort";
            this.nudHlaseniePort.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
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
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
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
            // cbCustomStation
            // 
            resources.ApplyResources(this.cbCustomStation, "cbCustomStation");
            this.cbCustomStation.BoxBackColor = System.Drawing.Color.White;
            this.cbCustomStation.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbCustomStation.Name = "cbCustomStation";
            this.cbCustomStation.UseVisualStyleBackColor = true;
            this.cbCustomStation.CheckedChanged += new System.EventHandler(this.cbCustomStation_CheckedChanged);
            // 
            // nudIDStation
            // 
            this.nudIDStation.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudIDStation, "nudIDStation");
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
            this.nudIDStation.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            this.nudIDStation.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
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
            this.tbStationName.TextChanged += new System.EventHandler(this.tbStationName_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dynamicLineSeparator1);
            this.groupBox4.Controls.Add(this.tbStationName);
            this.groupBox4.Controls.Add(this.cbCustomStation);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.nudIDStation);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.cbStationName);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // dynamicLineSeparator1
            // 
            this.dynamicLineSeparator1.LineOrientation = ExControls.LineOrientation.Vertical;
            resources.ApplyResources(this.dynamicLineSeparator1, "dynamicLineSeparator1");
            this.dynamicLineSeparator1.Name = "dynamicLineSeparator1";
            // 
            // FNewGrafikon
            // 
            this.AcceptButton = this.bCreate;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bZrusit;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bZrusit);
            this.Controls.Add(this.tbDirName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bCreate);
            this.Controls.Add(this.Grafikon);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FNewGrafikon";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FNewGrafikon_HelpButtonClicked);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Grafikon.ResumeLayout(false);
            this.Grafikon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stanicaBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHlaseniePort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTabPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIDStation)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExGroupBox groupBox1;
        private ExTextBox tbDirIniss;
        private System.Windows.Forms.Label label1;
        private ExGroupBox Grafikon;
        private System.Windows.Forms.DateTimePicker dtpDataDo;
        private System.Windows.Forms.DateTimePicker dtpDataOd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private ExControls.ExButton bCreate;
        private ExControls.ExButton bZrusit;
        private ExComboBox cbStationName;
        private ExGroupBox groupBox2;
        private ExRadioButton rbNewObd;
        private ExRadioButton rbNewStation;
        private ExTextBox tbDirName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.BindingSource stanicaBindingSource;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpGVDDo;
        private System.Windows.Forms.DateTimePicker dtpGVDOd;
        private System.Windows.Forms.Label label6;
        private ExGroupBox groupBox3;
        private ExControls.ExButton bEditColor;
        private System.Windows.Forms.PictureBox pbColor;
        private ExNumericUpDown nudHlaseniePort;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private ExNumericUpDown nudTabPort;
        private System.Windows.Forms.ColorDialog colorDialogFarba;
        private ExCheckBox cbCustomStation;
        private ExNumericUpDown nudIDStation;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private ExTextBox tbStationName;
        private ExGroupBox groupBox4;
        private ExLineSeparator dynamicLineSeparator1;
    }
}