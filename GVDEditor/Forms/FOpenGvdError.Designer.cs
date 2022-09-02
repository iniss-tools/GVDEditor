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
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbIcon
            // 
            this.pbIcon.Location = new System.Drawing.Point(11, 9);
            this.pbIcon.Margin = new System.Windows.Forms.Padding(2);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(72, 72);
            this.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbIcon.TabIndex = 0;
            this.pbIcon.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pri načítavaní grafikonu sa vyskytla chyba:";
            // 
            // rtbMessage
            // 
            this.rtbMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbMessage.BorderColor = System.Drawing.Color.DimGray;
            this.rtbMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMessage.DefaultStyle = false;
            this.rtbMessage.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.rtbMessage.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.rtbMessage.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.rtbMessage.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rtbMessage.Location = new System.Drawing.Point(99, 24);
            this.rtbMessage.Margin = new System.Windows.Forms.Padding(2);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.ReadOnly = true;
            this.rtbMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbMessage.Size = new System.Drawing.Size(323, 178);
            this.rtbMessage.TabIndex = 1;
            this.rtbMessage.Text = "";
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(365, 209);
            this.bOK.Margin = new System.Windows.Forms.Padding(2);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(57, 28);
            this.bOK.TabIndex = 2;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rtbMessage);
            this.panel1.Controls.Add(this.bOK);
            this.panel1.Controls.Add(this.pbIcon);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(433, 248);
            this.panel1.TabIndex = 3;
            // 
            // FOpenGvdError
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 248);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FOpenGvdError";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chyba";
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pbIcon;
        private Label label1;
        private ExControls.ExRichTextBox rtbMessage;
        private ExControls.ExButton bOK;
        private Panel panel1;
    }
}