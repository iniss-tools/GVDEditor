using ExControls;

namespace GVDEditor.Forms
{
    partial class FDateLimitEdit
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
            this.bSave = new ExControls.ExButton();
            this.bStorno = new ExControls.ExButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDateRemNew = new ExControls.ExTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDateRemOrig = new ExControls.ExTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bSave
            // 
            this.bSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bSave.Location = new System.Drawing.Point(293, 63);
            this.bSave.Margin = new System.Windows.Forms.Padding(2);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(56, 27);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "Upraviť";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bStorno
            // 
            this.bStorno.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bStorno.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bStorno.Location = new System.Drawing.Point(353, 63);
            this.bStorno.Margin = new System.Windows.Forms.Padding(2);
            this.bStorno.Name = "bStorno";
            this.bStorno.Size = new System.Drawing.Size(56, 27);
            this.bStorno.TabIndex = 1;
            this.bStorno.Text = "Zrušiť";
            this.bStorno.UseVisualStyleBackColor = true;
            this.bStorno.Click += new System.EventHandler(this.bStorno_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pôvodné dát. obm.:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDateRemNew
            // 
            this.tbDateRemNew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDateRemNew.BorderColor = System.Drawing.Color.DimGray;
            this.tbDateRemNew.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbDateRemNew.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbDateRemNew.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDateRemNew.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbDateRemNew.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDateRemNew.HintText = null;
            this.tbDateRemNew.Location = new System.Drawing.Point(115, 29);
            this.tbDateRemNew.Margin = new System.Windows.Forms.Padding(2);
            this.tbDateRemNew.Name = "tbDateRemNew";
            this.tbDateRemNew.Size = new System.Drawing.Size(293, 20);
            this.tbDateRemNew.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 36);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nové dát. obm.:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDateRemOrig
            // 
            this.tbDateRemOrig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDateRemOrig.BorderColor = System.Drawing.Color.DimGray;
            this.tbDateRemOrig.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbDateRemOrig.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbDateRemOrig.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDateRemOrig.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbDateRemOrig.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDateRemOrig.HintText = null;
            this.tbDateRemOrig.Location = new System.Drawing.Point(115, 5);
            this.tbDateRemOrig.Margin = new System.Windows.Forms.Padding(2);
            this.tbDateRemOrig.Name = "tbDateRemOrig";
            this.tbDateRemOrig.ReadOnly = true;
            this.tbDateRemOrig.Size = new System.Drawing.Size(293, 20);
            this.tbDateRemOrig.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.bStorno);
            this.panel1.Controls.Add(this.tbDateRemOrig);
            this.panel1.Controls.Add(this.bSave);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbDateRemNew);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(420, 101);
            this.panel1.TabIndex = 4;
            // 
            // FDateLimitEdit
            // 
            this.AcceptButton = this.bSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.ClientSize = new System.Drawing.Size(420, 101);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1444, 140);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(289, 140);
            this.Name = "FDateLimitEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Úprava dátumového obmedzenia vlaku";
            this.Load += new System.EventHandler(this.FDateRemEdit_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExControls.ExButton bSave;
        private ExControls.ExButton bStorno;
        private System.Windows.Forms.Label label1;
        private ExTextBox tbDateRemNew;
        private System.Windows.Forms.Label label2;
        private ExTextBox tbDateRemOrig;
        private Panel panel1;
    }
}