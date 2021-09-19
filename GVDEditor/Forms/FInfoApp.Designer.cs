using ExControls;

namespace GVDEditor.Forms
{
    partial class FInfoApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FInfoApp));
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.lAppName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lAppVersion = new System.Windows.Forms.Label();
            this.groupBox1 = new ExControls.ExGroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bOK = new System.Windows.Forms.Button();
            this.groupBox2 = new ExControls.ExGroupBox();
            this.linkEmail = new System.Windows.Forms.LinkLabel();
            this.linkWeb = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // picIcon
            // 
            this.picIcon.Image = global::GVDEditor.Properties.Resources.icon;
            resources.ApplyResources(this.picIcon, "picIcon");
            this.picIcon.Name = "picIcon";
            this.picIcon.TabStop = false;
            // 
            // lAppName
            // 
            resources.ApplyResources(this.lAppName, "lAppName");
            this.lAppName.Name = "lAppName";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lAppVersion
            // 
            resources.ApplyResources(this.lAppVersion, "lAppVersion");
            this.lAppVersion.Name = "lAppVersion";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // bOK
            // 
            resources.ApplyResources(this.bOK, "bOK");
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bOK.Name = "bOK";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.linkEmail);
            this.groupBox2.Controls.Add(this.linkWeb);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // linkEmail
            // 
            resources.ApplyResources(this.linkEmail, "linkEmail");
            this.linkEmail.Name = "linkEmail";
            this.linkEmail.TabStop = true;
            this.linkEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkWeb
            // 
            resources.ApplyResources(this.linkWeb, "linkWeb");
            this.linkWeb.Name = "linkWeb";
            this.linkWeb.TabStop = true;
            this.linkWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // FInformation
            // 
            this.AcceptButton = this.bOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lAppVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lAppName);
            this.Controls.Add(this.picIcon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FInfoApp";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.InformationForm_HelpButtonClicked);
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.Label lAppName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lAppVersion;
        private ExGroupBox groupBox1;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Label label4;
        private ExGroupBox groupBox2;
        private System.Windows.Forms.LinkLabel linkEmail;
        private System.Windows.Forms.LinkLabel linkWeb;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
    }
}