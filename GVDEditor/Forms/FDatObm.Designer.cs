using ExControls;

namespace GVDEditor.Forms
{
    partial class FDatObm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox4 = new ExControls.ExGroupBox();
            this.tbDatObm = new ExControls.ExTextBox();
            this.groupBox3 = new ExControls.ExGroupBox();
            this.bCopy = new ExControls.ExButton();
            this.tbBitArray = new ExControls.ExTextBox();
            this.groupBox2 = new ExControls.ExGroupBox();
            this.cbSpecDays = new ExControls.ExCheckBox();
            this.cbSkipDateRangeCheck = new ExControls.ExCheckBox();
            this.cbMonthRoman = new ExControls.ExCheckBox();
            this.groupBox1 = new ExControls.ExGroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dpStart = new System.Windows.Forms.DateTimePicker();
            this.dpEnd = new System.Windows.Forms.DateTimePicker();
            this.bGenerate = new ExControls.ExButton();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.bGenerate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(852, 394);
            this.panel1.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.tbDatObm);
            this.groupBox4.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox4.Location = new System.Drawing.Point(16, 175);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(818, 75);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Dátumové obmedzenie (text):";
            // 
            // tbDatObm
            // 
            this.tbDatObm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDatObm.BorderColor = System.Drawing.Color.DimGray;
            this.tbDatObm.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbDatObm.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbDatObm.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDatObm.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbDatObm.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDatObm.HintText = null;
            this.tbDatObm.Location = new System.Drawing.Point(8, 28);
            this.tbDatObm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbDatObm.Name = "tbDatObm";
            this.tbDatObm.Size = new System.Drawing.Size(800, 26);
            this.tbDatObm.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.bCopy);
            this.groupBox3.Controls.Add(this.tbBitArray);
            this.groupBox3.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox3.Location = new System.Drawing.Point(16, 258);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(818, 80);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pole bitov:";
            // 
            // bCopy
            // 
            this.bCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bCopy.AutoSize = true;
            this.bCopy.Location = new System.Drawing.Point(703, 28);
            this.bCopy.Name = "bCopy";
            this.bCopy.Size = new System.Drawing.Size(105, 30);
            this.bCopy.TabIndex = 1;
            this.bCopy.Text = "Skopírovať";
            this.bCopy.UseVisualStyleBackColor = true;
            this.bCopy.Click += new System.EventHandler(this.bCopy_Click);
            // 
            // tbBitArray
            // 
            this.tbBitArray.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBitArray.BorderColor = System.Drawing.Color.DimGray;
            this.tbBitArray.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbBitArray.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbBitArray.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbBitArray.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbBitArray.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbBitArray.HintText = null;
            this.tbBitArray.Location = new System.Drawing.Point(8, 28);
            this.tbBitArray.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbBitArray.Name = "tbBitArray";
            this.tbBitArray.ReadOnly = true;
            this.tbBitArray.Size = new System.Drawing.Size(672, 26);
            this.tbBitArray.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cbSpecDays);
            this.groupBox2.Controls.Add(this.cbSkipDateRangeCheck);
            this.groupBox2.Controls.Add(this.cbMonthRoman);
            this.groupBox2.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox2.Location = new System.Drawing.Point(441, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(393, 151);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Doplnkové nastavenia:";
            // 
            // cbSpecDays
            // 
            this.cbSpecDays.AutoSize = true;
            this.cbSpecDays.BoxBackColor = System.Drawing.Color.White;
            this.cbSpecDays.Checked = true;
            this.cbSpecDays.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSpecDays.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbSpecDays.Location = new System.Drawing.Point(21, 97);
            this.cbSpecDays.Name = "cbSpecDays";
            this.cbSpecDays.Size = new System.Drawing.Size(247, 24);
            this.cbSpecDays.TabIndex = 2;
            this.cbSpecDays.Text = "Používať sviatky a pracovné dni";
            this.cbSpecDays.UseVisualStyleBackColor = true;
            // 
            // cbSkipDateRangeCheck
            // 
            this.cbSkipDateRangeCheck.AutoSize = true;
            this.cbSkipDateRangeCheck.BoxBackColor = System.Drawing.Color.White;
            this.cbSkipDateRangeCheck.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbSkipDateRangeCheck.Location = new System.Drawing.Point(21, 63);
            this.cbSkipDateRangeCheck.Name = "cbSkipDateRangeCheck";
            this.cbSkipDateRangeCheck.Size = new System.Drawing.Size(341, 24);
            this.cbSkipDateRangeCheck.TabIndex = 1;
            this.cbSkipDateRangeCheck.Text = "Preskakovať chyby dátumov mimo grafikonu ";
            this.cbSkipDateRangeCheck.UseVisualStyleBackColor = true;
            // 
            // cbMonthRoman
            // 
            this.cbMonthRoman.AutoSize = true;
            this.cbMonthRoman.BoxBackColor = System.Drawing.Color.White;
            this.cbMonthRoman.Checked = true;
            this.cbMonthRoman.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMonthRoman.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbMonthRoman.Location = new System.Drawing.Point(21, 28);
            this.cbMonthRoman.Name = "cbMonthRoman";
            this.cbMonthRoman.Size = new System.Drawing.Size(258, 24);
            this.cbMonthRoman.TabIndex = 0;
            this.cbMonthRoman.Text = "Čísla mesiacov rímskymi číslicami";
            this.cbMonthRoman.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dpStart);
            this.groupBox1.Controls.Add(this.dpEnd);
            this.groupBox1.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox1.Location = new System.Drawing.Point(16, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 151);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Základné nastavenia:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 43);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Začiatok platnosti grafikonu:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 89);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Koniec platnosti grafikonu:";
            // 
            // dpStart
            // 
            this.dpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpStart.Location = new System.Drawing.Point(243, 43);
            this.dpStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dpStart.Name = "dpStart";
            this.dpStart.Size = new System.Drawing.Size(160, 26);
            this.dpStart.TabIndex = 1;
            // 
            // dpEnd
            // 
            this.dpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpEnd.Location = new System.Drawing.Point(243, 89);
            this.dpEnd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dpEnd.Name = "dpEnd";
            this.dpEnd.Size = new System.Drawing.Size(160, 26);
            this.dpEnd.TabIndex = 3;
            // 
            // bGenerate
            // 
            this.bGenerate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bGenerate.Location = new System.Drawing.Point(364, 346);
            this.bGenerate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bGenerate.Name = "bGenerate";
            this.bGenerate.Size = new System.Drawing.Size(130, 38);
            this.bGenerate.TabIndex = 9;
            this.bGenerate.Text = "Generovať";
            this.bGenerate.UseVisualStyleBackColor = true;
            this.bGenerate.Click += new System.EventHandler(this.bGenerate_Click);
            // 
            // FDatObm
            // 
            this.AcceptButton = this.bGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 394);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(868, 433);
            this.Name = "FDatObm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Generátor dátum. obmedzenia";
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ExGroupBox groupBox4;
        private ExTextBox tbDatObm;
        private ExGroupBox groupBox3;
        private ExControls.ExButton bCopy;
        private ExTextBox tbBitArray;
        private ExGroupBox groupBox2;
        private ExCheckBox cbSpecDays;
        private ExCheckBox cbSkipDateRangeCheck;
        private ExCheckBox cbMonthRoman;
        private ExGroupBox groupBox1;
        private Label label1;
        private Label label2;
        private DateTimePicker dpStart;
        private DateTimePicker dpEnd;
        private ExControls.ExButton bGenerate;
    }
}