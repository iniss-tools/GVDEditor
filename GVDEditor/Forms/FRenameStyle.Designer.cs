
namespace GVDEditor.Forms
{
    partial class FRenameStyle
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
            this.bRename = new ExControls.ExButton();
            this.tbStyleName = new ExControls.ExTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bRename
            // 
            this.bRename.AutoSize = true;
            this.bRename.Location = new System.Drawing.Point(150, 60);
            this.bRename.Name = "bRename";
            this.bRename.Size = new System.Drawing.Size(95, 32);
            this.bRename.TabIndex = 0;
            this.bRename.Text = "OK";
            this.bRename.UseVisualStyleBackColor = true;
            this.bRename.Click += new System.EventHandler(this.bRename_Click);
            // 
            // tbStyleName
            // 
            this.tbStyleName.BorderColor = System.Drawing.Color.DimGray;
            this.tbStyleName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbStyleName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbStyleName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbStyleName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbStyleName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbStyleName.HintText = null;
            this.tbStyleName.Location = new System.Drawing.Point(132, 12);
            this.tbStyleName.Name = "tbStyleName";
            this.tbStyleName.Size = new System.Drawing.Size(250, 22);
            this.tbStyleName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Nový názov štýlu:";
            // 
            // FRenameStyle
            // 
            this.AcceptButton = this.bRename;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 104);
            this.Controls.Add(this.tbStyleName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bRename);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FRenameStyle";
            this.ShowInTaskbar = false;
            this.Text = "Premenovať štýl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExControls.ExButton bRename;
        private ExControls.ExTextBox tbStyleName;
        private System.Windows.Forms.Label label1;
    }
}