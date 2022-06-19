namespace GVDEditor.Forms
{
    partial class FOpenGvdError
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
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rtbMessage = new ExControls.ExRichTextBox();
            this.bOK = new ExControls.ExButton();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pbIcon
            // 
            this.pbIcon.Location = new System.Drawing.Point(12, 12);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(100, 100);
            this.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbIcon.TabIndex = 0;
            this.pbIcon.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(130, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(258, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Pri načítavaní grafikonu sa vyskytla chyba:";
            // 
            // rtbMessage
            // 
            this.rtbMessage.BorderColor = System.Drawing.Color.DimGray;
            this.rtbMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMessage.DefaultStyle = false;
            this.rtbMessage.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.rtbMessage.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.rtbMessage.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.rtbMessage.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rtbMessage.Location = new System.Drawing.Point(133, 54);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.ReadOnly = true;
            this.rtbMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbMessage.Size = new System.Drawing.Size(429, 225);
            this.rtbMessage.TabIndex = 2;
            this.rtbMessage.Text = "";
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(256, 305);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(76, 34);
            this.bOK.TabIndex = 1;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // FOpenGvdError
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 351);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.rtbMessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbIcon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FOpenGvdError";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chyba";
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pbIcon;
        private Label label1;
        private ExControls.ExRichTextBox rtbMessage;
        private ExControls.ExButton bOK;
    }
}