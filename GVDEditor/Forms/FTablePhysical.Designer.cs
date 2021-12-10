using ExControls;

namespace GVDEditor.Forms
{
    partial class FTablePhysical
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTablePhysical));
            ExControls.ExComboBoxStyle exComboBoxStyle1 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle2 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle3 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle4 = new ExControls.ExComboBoxStyle();
            this.bSave = new ExControls.ExButton();
            this.bStorno = new ExControls.ExButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbName = new ExControls.ExTextBox();
            this.tbKey = new ExControls.ExTextBox();
            this.nudComPort = new ExControls.ExNumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudID = new ExControls.ExNumericUpDown();
            this.cbXML = new ExControls.ExCheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new ExControls.ExGroupBox();
            this.nudRecCount = new ExControls.ExNumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.cbCatalogTable = new ExControls.ExComboBox();
            this.groupBox2 = new ExControls.ExGroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbXMLName = new ExControls.ExTextBox();
            this.groupBox3 = new ExControls.ExGroupBox();
            this.tbComment = new ExControls.ExTextBox();
            this.cbAdvanced = new ExControls.ExCheckBox();
            this.groupBox4 = new ExControls.ExGroupBox();
            this.tbReverseArrows = new ExControls.ExTextBox();
            this.tbREM = new ExControls.ExTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudComPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudID)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecCount)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // bSave
            // 
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbName
            // 
            this.tbName.BorderColor = System.Drawing.Color.DimGray;
            this.tbName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbName.HintText = null;
            resources.ApplyResources(this.tbName, "tbName");
            this.tbName.Name = "tbName";
            // 
            // tbKey
            // 
            this.tbKey.BorderColor = System.Drawing.Color.DimGray;
            this.tbKey.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbKey.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbKey.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbKey.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbKey.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbKey.HintText = null;
            resources.ApplyResources(this.tbKey, "tbKey");
            this.tbKey.Name = "tbKey";
            // 
            // nudComPort
            // 
            this.nudComPort.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudComPort, "nudComPort");
            this.nudComPort.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudComPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudComPort.Name = "nudComPort";
            this.nudComPort.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
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
            // nudID
            // 
            this.nudID.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudID, "nudID");
            this.nudID.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudID.Name = "nudID";
            this.nudID.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            // 
            // cbXML
            // 
            resources.ApplyResources(this.cbXML, "cbXML");
            this.cbXML.BoxBackColor = System.Drawing.Color.White;
            this.cbXML.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbXML.Name = "cbXML";
            this.cbXML.UseVisualStyleBackColor = true;
            this.cbXML.CheckedChanged += new System.EventHandler(this.cbXML_CheckedChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudRecCount);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbCatalogTable);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbKey);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // nudRecCount
            // 
            this.nudRecCount.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudRecCount, "nudRecCount");
            this.nudRecCount.Name = "nudRecCount";
            this.nudRecCount.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cbCatalogTable
            // 
            this.cbCatalogTable.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbCatalogTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCatalogTable.FormattingEnabled = true;
            resources.ApplyResources(this.cbCatalogTable, "cbCatalogTable");
            this.cbCatalogTable.Name = "cbCatalogTable";
            exComboBoxStyle1.ArrowColor = null;
            exComboBoxStyle1.BackColor = null;
            exComboBoxStyle1.BorderColor = null;
            exComboBoxStyle1.ButtonBackColor = null;
            exComboBoxStyle1.ButtonBorderColor = null;
            exComboBoxStyle1.ButtonRenderFirst = null;
            exComboBoxStyle1.ForeColor = null;
            this.cbCatalogTable.StyleDisabled = exComboBoxStyle1;
            exComboBoxStyle2.ArrowColor = null;
            exComboBoxStyle2.BackColor = null;
            exComboBoxStyle2.BorderColor = null;
            exComboBoxStyle2.ButtonBackColor = null;
            exComboBoxStyle2.ButtonBorderColor = null;
            exComboBoxStyle2.ButtonRenderFirst = null;
            exComboBoxStyle2.ForeColor = null;
            this.cbCatalogTable.StyleHighlight = exComboBoxStyle2;
            exComboBoxStyle3.ArrowColor = null;
            exComboBoxStyle3.BackColor = null;
            exComboBoxStyle3.BorderColor = null;
            exComboBoxStyle3.ButtonBackColor = null;
            exComboBoxStyle3.ButtonBorderColor = null;
            exComboBoxStyle3.ButtonRenderFirst = null;
            exComboBoxStyle3.ForeColor = null;
            this.cbCatalogTable.StyleNormal = exComboBoxStyle3;
            exComboBoxStyle4.ArrowColor = null;
            exComboBoxStyle4.BackColor = null;
            exComboBoxStyle4.BorderColor = null;
            exComboBoxStyle4.ButtonBackColor = null;
            exComboBoxStyle4.ButtonBorderColor = null;
            exComboBoxStyle4.ButtonRenderFirst = null;
            exComboBoxStyle4.ForeColor = null;
            this.cbCatalogTable.StyleSelected = exComboBoxStyle4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbXMLName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.nudComPort);
            this.groupBox2.Controls.Add(this.cbXML);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.nudID);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // tbXMLName
            // 
            this.tbXMLName.BorderColor = System.Drawing.Color.DimGray;
            this.tbXMLName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbXMLName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbXMLName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.tbXMLName, "tbXMLName");
            this.tbXMLName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbXMLName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbXMLName.HintText = null;
            this.tbXMLName.Name = "tbXMLName";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbComment);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // tbComment
            // 
            this.tbComment.AcceptsReturn = true;
            this.tbComment.AcceptsTab = true;
            this.tbComment.BorderColor = System.Drawing.Color.DimGray;
            this.tbComment.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbComment.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbComment.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.tbComment, "tbComment");
            this.tbComment.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbComment.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbComment.HintText = null;
            this.tbComment.Name = "tbComment";
            // 
            // cbAdvanced
            // 
            resources.ApplyResources(this.cbAdvanced, "cbAdvanced");
            this.cbAdvanced.BoxBackColor = System.Drawing.Color.White;
            this.cbAdvanced.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cbAdvanced.Name = "cbAdvanced";
            this.cbAdvanced.UseVisualStyleBackColor = true;
            this.cbAdvanced.CheckedChanged += new System.EventHandler(this.cbAdvanced_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbAdvanced);
            this.groupBox4.Controls.Add(this.tbReverseArrows);
            this.groupBox4.Controls.Add(this.tbREM);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label8);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // tbReverseArrows
            // 
            this.tbReverseArrows.BorderColor = System.Drawing.Color.DimGray;
            this.tbReverseArrows.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbReverseArrows.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbReverseArrows.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.tbReverseArrows, "tbReverseArrows");
            this.tbReverseArrows.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbReverseArrows.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbReverseArrows.HintText = null;
            this.tbReverseArrows.Name = "tbReverseArrows";
            // 
            // tbREM
            // 
            this.tbREM.BorderColor = System.Drawing.Color.DimGray;
            this.tbREM.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbREM.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbREM.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.tbREM, "tbREM");
            this.tbREM.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbREM.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbREM.HintText = null;
            this.tbREM.Name = "tbREM";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // FTablePhysical
            // 
            this.AcceptButton = this.bSave;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bStorno);
            this.Controls.Add(this.bSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FTablePhysical";
            this.ShowInTaskbar = false;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FTablePhysical_HelpButtonClicked);
            ((System.ComponentModel.ISupportInitialize)(this.nudComPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudID)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecCount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExControls.ExButton bSave;
        private ExControls.ExButton bStorno;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ExTextBox tbName;
        private ExTextBox tbKey;
        private ExNumericUpDown nudComPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private ExNumericUpDown nudID;
        private ExCheckBox cbXML;
        private System.Windows.Forms.Label label5;
        private ExGroupBox groupBox1;
        private ExGroupBox groupBox2;
        private ExGroupBox groupBox3;
        private ExTextBox tbComment;
        private ExComboBox cbCatalogTable;
        private System.Windows.Forms.Label label6;
        private ExTextBox tbXMLName;
        private ExNumericUpDown nudRecCount;
        private System.Windows.Forms.Label label7;
        private ExCheckBox cbAdvanced;
        private ExGroupBox groupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private ExTextBox tbREM;
        private ExTextBox tbReverseArrows;
    }
}