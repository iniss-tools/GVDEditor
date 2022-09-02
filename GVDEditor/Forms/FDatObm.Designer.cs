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
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbDatObm);
            this.groupBox4.Location = new System.Drawing.Point(9, 113);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(550, 49);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Dátumové obmedzenie (text):";
            // 
            // tbDatObm
            // 
            this.tbDatObm.BorderColor = System.Drawing.Color.DimGray;
            this.tbDatObm.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbDatObm.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbDatObm.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDatObm.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbDatObm.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDatObm.HintText = null;
            this.tbDatObm.Location = new System.Drawing.Point(5, 18);
            this.tbDatObm.Name = "tbDatObm";
            this.tbDatObm.Size = new System.Drawing.Size(540, 20);
            this.tbDatObm.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.bCopy);
            this.groupBox3.Controls.Add(this.tbBitArray);
            this.groupBox3.Location = new System.Drawing.Point(9, 167);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(550, 52);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pole bitov:";
            // 
            // bCopy
            // 
            this.bCopy.AutoSize = true;
            this.bCopy.Location = new System.Drawing.Point(474, 17);
            this.bCopy.Margin = new System.Windows.Forms.Padding(2);
            this.bCopy.Name = "bCopy";
            this.bCopy.Size = new System.Drawing.Size(71, 23);
            this.bCopy.TabIndex = 1;
            this.bCopy.Text = "Skopírovať";
            this.bCopy.UseVisualStyleBackColor = true;
            this.bCopy.Click += new System.EventHandler(this.bCopy_Click);
            // 
            // tbBitArray
            // 
            this.tbBitArray.BorderColor = System.Drawing.Color.DimGray;
            this.tbBitArray.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbBitArray.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbBitArray.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbBitArray.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbBitArray.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbBitArray.HintText = null;
            this.tbBitArray.Location = new System.Drawing.Point(5, 18);
            this.tbBitArray.Name = "tbBitArray";
            this.tbBitArray.ReadOnly = true;
            this.tbBitArray.Size = new System.Drawing.Size(457, 20);
            this.tbBitArray.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbSpecDays);
            this.groupBox2.Controls.Add(this.cbSkipDateRangeCheck);
            this.groupBox2.Controls.Add(this.cbMonthRoman);
            this.groupBox2.Location = new System.Drawing.Point(292, 10);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(266, 98);
            this.groupBox2.TabIndex = 1;
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
            this.cbSpecDays.Location = new System.Drawing.Point(14, 63);
            this.cbSpecDays.Margin = new System.Windows.Forms.Padding(2);
            this.cbSpecDays.Name = "cbSpecDays";
            this.cbSpecDays.Size = new System.Drawing.Size(180, 17);
            this.cbSpecDays.TabIndex = 2;
            this.cbSpecDays.Text = "Používať sviatky a pracovné dni";
            this.cbSpecDays.UseVisualStyleBackColor = true;
            // 
            // cbSkipDateRangeCheck
            // 
            this.cbSkipDateRangeCheck.AutoSize = true;
            this.cbSkipDateRangeCheck.BoxBackColor = System.Drawing.Color.White;
            this.cbSkipDateRangeCheck.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbSkipDateRangeCheck.Location = new System.Drawing.Point(14, 41);
            this.cbSkipDateRangeCheck.Margin = new System.Windows.Forms.Padding(2);
            this.cbSkipDateRangeCheck.Name = "cbSkipDateRangeCheck";
            this.cbSkipDateRangeCheck.Size = new System.Drawing.Size(239, 17);
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
            this.cbMonthRoman.Location = new System.Drawing.Point(14, 18);
            this.cbMonthRoman.Margin = new System.Windows.Forms.Padding(2);
            this.cbMonthRoman.Name = "cbMonthRoman";
            this.cbMonthRoman.Size = new System.Drawing.Size(186, 17);
            this.cbMonthRoman.TabIndex = 0;
            this.cbMonthRoman.Text = "Čísla mesiacov rímskymi číslicami";
            this.cbMonthRoman.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dpStart);
            this.groupBox1.Controls.Add(this.dpEnd);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(279, 98);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Základné nastavenia:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Začiatok platnosti grafikonu:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Koniec platnosti grafikonu:";
            // 
            // dpStart
            // 
            this.dpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpStart.Location = new System.Drawing.Point(162, 28);
            this.dpStart.Name = "dpStart";
            this.dpStart.Size = new System.Drawing.Size(108, 20);
            this.dpStart.TabIndex = 1;
            // 
            // dpEnd
            // 
            this.dpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpEnd.Location = new System.Drawing.Point(162, 58);
            this.dpEnd.Name = "dpEnd";
            this.dpEnd.Size = new System.Drawing.Size(108, 20);
            this.dpEnd.TabIndex = 3;
            // 
            // bGenerate
            // 
            this.bGenerate.Location = new System.Drawing.Point(241, 224);
            this.bGenerate.Name = "bGenerate";
            this.bGenerate.Size = new System.Drawing.Size(86, 25);
            this.bGenerate.TabIndex = 4;
            this.bGenerate.Text = "Generovať";
            this.bGenerate.UseVisualStyleBackColor = true;
            this.bGenerate.Click += new System.EventHandler(this.bGenerate_Click);
            // 
            // FDatObm
            // 
            this.AcceptButton = this.bGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 256);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FDatObm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Generátor dátum. obmedzenia";
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

        private ExGroupBox groupBox4;
        private ExTextBox tbDatObm;
        private ExGroupBox groupBox3;
        private ExTextBox tbBitArray;
        private ExGroupBox groupBox2;
        private ExCheckBox cbSpecDays;
        private ExCheckBox cbSkipDateRangeCheck;
        private ExCheckBox cbMonthRoman;
        private ExGroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dpStart;
        private System.Windows.Forms.DateTimePicker dpEnd;
        private ExControls.ExButton bGenerate;
        private ExControls.ExButton bCopy;
    }
}