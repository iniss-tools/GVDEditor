using ExControls;

namespace GVDEditor.UI.Dialogs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FDatObm));
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
            this.dpStart = new ExControls.ExDateTimePicker();
            this.dpEnd = new ExControls.ExDateTimePicker();
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
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.tbDatObm);
            this.groupBox4.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // tbDatObm
            // 
            resources.ApplyResources(this.tbDatObm, "tbDatObm");
            this.tbDatObm.BorderColor = System.Drawing.Color.DimGray;
            this.tbDatObm.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbDatObm.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbDatObm.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDatObm.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbDatObm.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDatObm.HintText = null;
            this.tbDatObm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbDatObm.Name = "tbDatObm";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.bCopy);
            this.groupBox3.Controls.Add(this.tbBitArray);
            this.groupBox3.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // bCopy
            // 
            resources.ApplyResources(this.bCopy, "bCopy");
            this.bCopy.Name = "bCopy";
            this.bCopy.UseVisualStyleBackColor = true;
            this.bCopy.Click += new System.EventHandler(this.bCopy_Click);
            // 
            // tbBitArray
            // 
            resources.ApplyResources(this.tbBitArray, "tbBitArray");
            this.tbBitArray.BorderColor = System.Drawing.Color.DimGray;
            this.tbBitArray.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbBitArray.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbBitArray.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbBitArray.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbBitArray.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbBitArray.HintText = null;
            this.tbBitArray.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbBitArray.Name = "tbBitArray";
            this.tbBitArray.ReadOnly = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.cbSpecDays);
            this.groupBox2.Controls.Add(this.cbSkipDateRangeCheck);
            this.groupBox2.Controls.Add(this.cbMonthRoman);
            this.groupBox2.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cbSpecDays
            // 
            resources.ApplyResources(this.cbSpecDays, "cbSpecDays");
            this.cbSpecDays.BoxBackColor = System.Drawing.Color.White;
            this.cbSpecDays.Checked = true;
            this.cbSpecDays.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSpecDays.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbSpecDays.Name = "cbSpecDays";
            this.cbSpecDays.UseVisualStyleBackColor = true;
            // 
            // cbSkipDateRangeCheck
            // 
            resources.ApplyResources(this.cbSkipDateRangeCheck, "cbSkipDateRangeCheck");
            this.cbSkipDateRangeCheck.BoxBackColor = System.Drawing.Color.White;
            this.cbSkipDateRangeCheck.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbSkipDateRangeCheck.Name = "cbSkipDateRangeCheck";
            this.cbSkipDateRangeCheck.UseVisualStyleBackColor = true;
            // 
            // cbMonthRoman
            // 
            resources.ApplyResources(this.cbMonthRoman, "cbMonthRoman");
            this.cbMonthRoman.BoxBackColor = System.Drawing.Color.White;
            this.cbMonthRoman.Checked = true;
            this.cbMonthRoman.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMonthRoman.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbMonthRoman.Name = "cbMonthRoman";
            this.cbMonthRoman.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dpStart);
            this.groupBox1.Controls.Add(this.dpEnd);
            this.groupBox1.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            // 
            // dpStart
            // 
            this.dpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dpStart, "dpStart");
            this.dpStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dpStart.Name = "dpStart";
            // 
            // dpEnd
            // 
            this.dpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dpEnd, "dpEnd");
            this.dpEnd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dpEnd.Name = "dpEnd";
            // 
            // bGenerate
            // 
            resources.ApplyResources(this.bGenerate, "bGenerate");
            this.bGenerate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bGenerate.Name = "bGenerate";
            this.bGenerate.UseVisualStyleBackColor = true;
            this.bGenerate.Click += new System.EventHandler(this.bGenerate_Click);
            // 
            // FDatObm
            // 
            this.AcceptButton = this.bGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(868, 433);
            this.Name = "FDatObm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
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
        private ExControls.ExDateTimePicker dpStart;
        private ExControls.ExDateTimePicker dpEnd;
        private ExControls.ExButton bGenerate;
    }
}