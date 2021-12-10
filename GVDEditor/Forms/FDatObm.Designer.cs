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
            this.groupBox4.Location = new System.Drawing.Point(12, 139);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(733, 60);
            this.groupBox4.TabIndex = 17;
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
            this.tbDatObm.Location = new System.Drawing.Point(7, 22);
            this.tbDatObm.Margin = new System.Windows.Forms.Padding(4);
            this.tbDatObm.Name = "tbDatObm";
            this.tbDatObm.Size = new System.Drawing.Size(719, 22);
            this.tbDatObm.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.bCopy);
            this.groupBox3.Controls.Add(this.tbBitArray);
            this.groupBox3.Location = new System.Drawing.Point(12, 205);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(733, 64);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pole bitov:";
            // 
            // bCopy
            // 
            this.bCopy.AutoSize = true;
            this.bCopy.Location = new System.Drawing.Point(639, 21);
            this.bCopy.Name = "bCopy";
            this.bCopy.Size = new System.Drawing.Size(90, 26);
            this.bCopy.TabIndex = 3;
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
            this.tbBitArray.Location = new System.Drawing.Point(7, 22);
            this.tbBitArray.Margin = new System.Windows.Forms.Padding(4);
            this.tbBitArray.Name = "tbBitArray";
            this.tbBitArray.ReadOnly = true;
            this.tbBitArray.Size = new System.Drawing.Size(625, 22);
            this.tbBitArray.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbSpecDays);
            this.groupBox2.Controls.Add(this.cbSkipDateRangeCheck);
            this.groupBox2.Controls.Add(this.cbMonthRoman);
            this.groupBox2.Location = new System.Drawing.Point(390, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(355, 121);
            this.groupBox2.TabIndex = 15;
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
            this.cbSpecDays.Location = new System.Drawing.Point(19, 77);
            this.cbSpecDays.Name = "cbSpecDays";
            this.cbSpecDays.Size = new System.Drawing.Size(218, 20);
            this.cbSpecDays.TabIndex = 2;
            this.cbSpecDays.Text = "Používať sviatky a pracovné dni";
            this.cbSpecDays.UseVisualStyleBackColor = true;
            // 
            // cbSkipDateRangeCheck
            // 
            this.cbSkipDateRangeCheck.AutoSize = true;
            this.cbSkipDateRangeCheck.BoxBackColor = System.Drawing.Color.White;
            this.cbSkipDateRangeCheck.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbSkipDateRangeCheck.Location = new System.Drawing.Point(19, 50);
            this.cbSkipDateRangeCheck.Name = "cbSkipDateRangeCheck";
            this.cbSkipDateRangeCheck.Size = new System.Drawing.Size(297, 20);
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
            this.cbMonthRoman.Location = new System.Drawing.Point(19, 22);
            this.cbMonthRoman.Name = "cbMonthRoman";
            this.cbMonthRoman.Size = new System.Drawing.Size(232, 20);
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
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(372, 121);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Základné nastavenia:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Začiatok platnosti grafikonu:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 71);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Koniec platnosti grafikonu:";
            // 
            // dpStart
            // 
            this.dpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpStart.Location = new System.Drawing.Point(216, 35);
            this.dpStart.Margin = new System.Windows.Forms.Padding(4);
            this.dpStart.Name = "dpStart";
            this.dpStart.Size = new System.Drawing.Size(143, 22);
            this.dpStart.TabIndex = 3;
            // 
            // dpEnd
            // 
            this.dpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpEnd.Location = new System.Drawing.Point(216, 71);
            this.dpEnd.Margin = new System.Windows.Forms.Padding(4);
            this.dpEnd.Name = "dpEnd";
            this.dpEnd.Size = new System.Drawing.Size(143, 22);
            this.dpEnd.TabIndex = 4;
            // 
            // bGenerate
            // 
            this.bGenerate.Location = new System.Drawing.Point(321, 276);
            this.bGenerate.Margin = new System.Windows.Forms.Padding(4);
            this.bGenerate.Name = "bGenerate";
            this.bGenerate.Size = new System.Drawing.Size(115, 31);
            this.bGenerate.TabIndex = 13;
            this.bGenerate.Text = "Generovať";
            this.bGenerate.UseVisualStyleBackColor = true;
            this.bGenerate.Click += new System.EventHandler(this.bGenerate_Click);
            // 
            // FDatObm
            // 
            this.AcceptButton = this.bGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 315);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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