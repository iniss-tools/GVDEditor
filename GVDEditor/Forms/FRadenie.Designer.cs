using ExControls;

namespace GVDEditor.Forms
{
    partial class FRadenie
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRadenie));
            this.bSave = new ExControls.ExButton();
            this.bStorno = new ExControls.ExButton();
            this.cbSoundDir = new ExControls.ExComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listAllSounds = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bAdd = new ExControls.ExButton();
            this.bDelete = new ExControls.ExButton();
            this.bSkor = new ExControls.ExButton();
            this.bNeskor = new ExControls.ExButton();
            this.listRadenie = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbLanguage = new ExControls.ExComboBox();
            this.tbTextSound = new ExControls.ExTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbTextRadenie = new ExControls.ExTextBox();
            this.bPlay = new ExControls.ExButton();
            this.SuspendLayout();
            // 
            // bSave
            // 
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.Name = "bSave";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bStorno
            // 
            resources.ApplyResources(this.bStorno, "bStorno");
            this.bStorno.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bStorno.Name = "bStorno";
            this.bStorno.UseVisualStyleBackColor = true;
            this.bStorno.Click += new System.EventHandler(this.bStorno_Click);
            // 
            // cbSoundDir
            // 
            this.cbSoundDir.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbSoundDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSoundDir.FormattingEnabled = true;
            resources.ApplyResources(this.cbSoundDir, "cbSoundDir");
            this.cbSoundDir.Name = "cbSoundDir";
            this.cbSoundDir.UseDarkScrollBar = false;
            this.cbSoundDir.SelectedIndexChanged += new System.EventHandler(this.cbSoundDir_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // listAllSounds
            // 
            this.listAllSounds.FormattingEnabled = true;
            resources.ApplyResources(this.listAllSounds, "listAllSounds");
            this.listAllSounds.Name = "listAllSounds";
            this.listAllSounds.SelectedIndexChanged += new System.EventHandler(this.listAllSounds_SelectedIndexChanged);
            this.listAllSounds.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.listAllSounds_Format);
            this.listAllSounds.DoubleClick += new System.EventHandler(this.listAllSounds_DoubleClick);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // bAdd
            // 
            resources.ApplyResources(this.bAdd, "bAdd");
            this.bAdd.Name = "bAdd";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bDelete
            // 
            resources.ApplyResources(this.bDelete, "bDelete");
            this.bDelete.Name = "bDelete";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bSkor
            // 
            resources.ApplyResources(this.bSkor, "bSkor");
            this.bSkor.Name = "bSkor";
            this.bSkor.UseVisualStyleBackColor = true;
            this.bSkor.Click += new System.EventHandler(this.bSkor_Click);
            // 
            // bNeskor
            // 
            resources.ApplyResources(this.bNeskor, "bNeskor");
            this.bNeskor.Name = "bNeskor";
            this.bNeskor.UseVisualStyleBackColor = true;
            this.bNeskor.Click += new System.EventHandler(this.bNeskor_Click);
            // 
            // listRadenie
            // 
            this.listRadenie.AllowDrop = true;
            this.listRadenie.FormattingEnabled = true;
            resources.ApplyResources(this.listRadenie, "listRadenie");
            this.listRadenie.Name = "listRadenie";
            this.listRadenie.SelectedIndexChanged += new System.EventHandler(this.listRadenie_SelectedIndexChanged);
            this.listRadenie.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.listRadenie_Format);
            this.listRadenie.DragDrop += new System.Windows.Forms.DragEventHandler(this.listRadenie_DragDrop);
            this.listRadenie.DragOver += new System.Windows.Forms.DragEventHandler(this.listRadenie_DragOver);
            this.listRadenie.DoubleClick += new System.EventHandler(this.listRadenie_DoubleClick);
            this.listRadenie.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listRadenie_KeyDown);
            this.listRadenie.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listRadenie_MouseDown);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cbLanguage
            // 
            this.cbLanguage.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguage.FormattingEnabled = true;
            resources.ApplyResources(this.cbLanguage, "cbLanguage");
            this.cbLanguage.Name = "cbLanguage";
            this.cbLanguage.UseDarkScrollBar = false;
            this.cbLanguage.SelectedIndexChanged += new System.EventHandler(this.cbLanguage_SelectedIndexChanged);
            // 
            // tbTextSound
            // 
            this.tbTextSound.BorderColor = System.Drawing.Color.DimGray;
            this.tbTextSound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTextSound.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbTextSound.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbTextSound.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbTextSound.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbTextSound.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbTextSound.HintText = null;
            resources.ApplyResources(this.tbTextSound, "tbTextSound");
            this.tbTextSound.Name = "tbTextSound";
            this.tbTextSound.ReadOnly = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // tbTextRadenie
            // 
            this.tbTextRadenie.BorderColor = System.Drawing.Color.DimGray;
            this.tbTextRadenie.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTextRadenie.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbTextRadenie.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbTextRadenie.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbTextRadenie.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbTextRadenie.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbTextRadenie.HintText = null;
            resources.ApplyResources(this.tbTextRadenie, "tbTextRadenie");
            this.tbTextRadenie.Name = "tbTextRadenie";
            this.tbTextRadenie.ReadOnly = true;
            // 
            // bPlay
            // 
            resources.ApplyResources(this.bPlay, "bPlay");
            this.bPlay.Name = "bPlay";
            this.bPlay.UseVisualStyleBackColor = true;
            this.bPlay.Click += new System.EventHandler(this.bPlay_Click);
            // 
            // FRadenie
            // 
            this.AcceptButton = this.bSave;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.Controls.Add(this.bPlay);
            this.Controls.Add(this.tbTextRadenie);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbTextSound);
            this.Controls.Add(this.cbLanguage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listRadenie);
            this.Controls.Add(this.bNeskor);
            this.Controls.Add(this.bSkor);
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listAllSounds);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbSoundDir);
            this.Controls.Add(this.bStorno);
            this.Controls.Add(this.bSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FRadenie";
            this.ShowInTaskbar = false;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FRadenie_HelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExControls.ExButton bSave;
        private ExControls.ExButton bStorno;
        private ExComboBox cbSoundDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listAllSounds;
        private System.Windows.Forms.Label label2;
        private ExControls.ExButton bAdd;
        private ExControls.ExButton bDelete;
        private ExControls.ExButton bSkor;
        private ExControls.ExButton bNeskor;
        private System.Windows.Forms.ListBox listRadenie;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private ExComboBox cbLanguage;
        private ExTextBox tbTextSound;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private ExTextBox tbTextRadenie;
        private ExControls.ExButton bPlay;
    }
}