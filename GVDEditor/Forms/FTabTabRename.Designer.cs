
using ExControls;

namespace GVDEditor.Forms
{
    partial class FTabTabRename
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbName = new ExTextBox();
            this.bEdit = new ExControls.ExButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Názov TabTab:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(124, 12);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(254, 22);
            this.tbName.TabIndex = 1;
            // 
            // bEdit
            // 
            this.bEdit.AutoSize = true;
            this.bEdit.Location = new System.Drawing.Point(151, 44);
            this.bEdit.Name = "bEdit";
            this.bEdit.Size = new System.Drawing.Size(84, 36);
            this.bEdit.TabIndex = 2;
            this.bEdit.Text = "Uložiť";
            this.bEdit.UseVisualStyleBackColor = true;
            this.bEdit.Click += new System.EventHandler(this.bEdit_Click);
            // 
            // FTabTabRename
            // 
            this.AcceptButton = this.bEdit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 92);
            this.Controls.Add(this.bEdit);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FTabTabRename";
            this.ShowInTaskbar = false;
            this.Text = "Upraviť názov ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private ExTextBox tbName;
        private ExControls.ExButton bEdit;
    }
}