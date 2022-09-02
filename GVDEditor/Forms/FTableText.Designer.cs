using ExControls;
using GVDEditor.Entities;

namespace GVDEditor.Forms
{
    partial class FTableText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTableText));
            this.bSave = new ExControls.ExButton();
            this.bStorno = new ExControls.ExButton();
            this.groupBox1 = new ExControls.ExGroupBox();
            this.tbKey = new ExControls.ExTextBox();
            this.tbName = new ExControls.ExTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new ExControls.ExGroupBox();
            this.cbCatalogItem = new ExControls.ExComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbCatalogTable = new ExControls.ExComboBox();
            this.bReDelete = new ExControls.ExButton();
            this.bReEdit = new ExControls.ExButton();
            this.bReAdd = new ExControls.ExButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listRealisations = new System.Windows.Forms.ListBox();
            this.groupBox3 = new ExControls.ExGroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.nudFont = new ExControls.ExNumericUpDown();
            this.bGenerate = new ExControls.ExButton();
            this.bTextEdit = new ExControls.ExButton();
            this.tbTrainText = new ExControls.ExTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.listTrains = new System.Windows.Forms.ListBox();
            this.groupBox4 = new ExControls.ExGroupBox();
            this.tbComment = new ExControls.ExTextBox();
            this.tableTextRealizationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFont)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableTextRealizationBindingSource)).BeginInit();
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbKey);
            this.groupBox1.Controls.Add(this.tbName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
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
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbCatalogItem);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cbCatalogTable);
            this.groupBox2.Controls.Add(this.bReDelete);
            this.groupBox2.Controls.Add(this.bReEdit);
            this.groupBox2.Controls.Add(this.bReAdd);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.listRealisations);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cbCatalogItem
            // 
            this.cbCatalogItem.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbCatalogItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCatalogItem.FormattingEnabled = true;
            resources.ApplyResources(this.cbCatalogItem, "cbCatalogItem");
            this.cbCatalogItem.Name = "cbCatalogItem";
            this.cbCatalogItem.StyleDisabled.ArrowColor = null;
            this.cbCatalogItem.StyleDisabled.BackColor = null;
            this.cbCatalogItem.StyleDisabled.BorderColor = null;
            this.cbCatalogItem.StyleDisabled.ButtonBackColor = null;
            this.cbCatalogItem.StyleDisabled.ButtonBorderColor = null;
            this.cbCatalogItem.StyleDisabled.ButtonRenderFirst = null;
            this.cbCatalogItem.StyleDisabled.ForeColor = null;
            this.cbCatalogItem.StyleHighlight.ArrowColor = null;
            this.cbCatalogItem.StyleHighlight.BackColor = null;
            this.cbCatalogItem.StyleHighlight.BorderColor = null;
            this.cbCatalogItem.StyleHighlight.ButtonBackColor = null;
            this.cbCatalogItem.StyleHighlight.ButtonBorderColor = null;
            this.cbCatalogItem.StyleHighlight.ButtonRenderFirst = null;
            this.cbCatalogItem.StyleHighlight.ForeColor = null;
            this.cbCatalogItem.StyleNormal.ArrowColor = null;
            this.cbCatalogItem.StyleNormal.BackColor = null;
            this.cbCatalogItem.StyleNormal.BorderColor = null;
            this.cbCatalogItem.StyleNormal.ButtonBackColor = null;
            this.cbCatalogItem.StyleNormal.ButtonBorderColor = null;
            this.cbCatalogItem.StyleNormal.ButtonRenderFirst = null;
            this.cbCatalogItem.StyleNormal.ForeColor = null;
            this.cbCatalogItem.StyleSelected.ArrowColor = null;
            this.cbCatalogItem.StyleSelected.BackColor = null;
            this.cbCatalogItem.StyleSelected.BorderColor = null;
            this.cbCatalogItem.StyleSelected.ButtonBackColor = null;
            this.cbCatalogItem.StyleSelected.ButtonBorderColor = null;
            this.cbCatalogItem.StyleSelected.ButtonRenderFirst = null;
            this.cbCatalogItem.StyleSelected.ForeColor = null;
            this.cbCatalogItem.UseDarkScrollBar = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cbCatalogTable
            // 
            this.cbCatalogTable.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbCatalogTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCatalogTable.FormattingEnabled = true;
            resources.ApplyResources(this.cbCatalogTable, "cbCatalogTable");
            this.cbCatalogTable.Name = "cbCatalogTable";
            this.cbCatalogTable.StyleDisabled.ArrowColor = null;
            this.cbCatalogTable.StyleDisabled.BackColor = null;
            this.cbCatalogTable.StyleDisabled.BorderColor = null;
            this.cbCatalogTable.StyleDisabled.ButtonBackColor = null;
            this.cbCatalogTable.StyleDisabled.ButtonBorderColor = null;
            this.cbCatalogTable.StyleDisabled.ButtonRenderFirst = null;
            this.cbCatalogTable.StyleDisabled.ForeColor = null;
            this.cbCatalogTable.StyleHighlight.ArrowColor = null;
            this.cbCatalogTable.StyleHighlight.BackColor = null;
            this.cbCatalogTable.StyleHighlight.BorderColor = null;
            this.cbCatalogTable.StyleHighlight.ButtonBackColor = null;
            this.cbCatalogTable.StyleHighlight.ButtonBorderColor = null;
            this.cbCatalogTable.StyleHighlight.ButtonRenderFirst = null;
            this.cbCatalogTable.StyleHighlight.ForeColor = null;
            this.cbCatalogTable.StyleNormal.ArrowColor = null;
            this.cbCatalogTable.StyleNormal.BackColor = null;
            this.cbCatalogTable.StyleNormal.BorderColor = null;
            this.cbCatalogTable.StyleNormal.ButtonBackColor = null;
            this.cbCatalogTable.StyleNormal.ButtonBorderColor = null;
            this.cbCatalogTable.StyleNormal.ButtonRenderFirst = null;
            this.cbCatalogTable.StyleNormal.ForeColor = null;
            this.cbCatalogTable.StyleSelected.ArrowColor = null;
            this.cbCatalogTable.StyleSelected.BackColor = null;
            this.cbCatalogTable.StyleSelected.BorderColor = null;
            this.cbCatalogTable.StyleSelected.ButtonBackColor = null;
            this.cbCatalogTable.StyleSelected.ButtonBorderColor = null;
            this.cbCatalogTable.StyleSelected.ButtonRenderFirst = null;
            this.cbCatalogTable.StyleSelected.ForeColor = null;
            this.cbCatalogTable.UseDarkScrollBar = false;
            this.cbCatalogTable.SelectedIndexChanged += new System.EventHandler(this.cbCatalogTable_SelectedIndexChanged);
            // 
            // bReDelete
            // 
            resources.ApplyResources(this.bReDelete, "bReDelete");
            this.bReDelete.Name = "bReDelete";
            this.bReDelete.UseVisualStyleBackColor = true;
            this.bReDelete.Click += new System.EventHandler(this.bReDelete_Click);
            // 
            // bReEdit
            // 
            resources.ApplyResources(this.bReEdit, "bReEdit");
            this.bReEdit.Name = "bReEdit";
            this.bReEdit.UseVisualStyleBackColor = true;
            this.bReEdit.Click += new System.EventHandler(this.bReEdit_Click);
            // 
            // bReAdd
            // 
            resources.ApplyResources(this.bReAdd, "bReAdd");
            this.bReAdd.Name = "bReAdd";
            this.bReAdd.UseVisualStyleBackColor = true;
            this.bReAdd.Click += new System.EventHandler(this.bReAdd_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // listRealisations
            // 
            this.listRealisations.FormattingEnabled = true;
            resources.ApplyResources(this.listRealisations, "listRealisations");
            this.listRealisations.Name = "listRealisations";
            this.listRealisations.SelectedIndexChanged += new System.EventHandler(this.listRealisations_SelectedIndexChanged);
            this.listRealisations.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.listRealisations_Format);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.nudFont);
            this.groupBox3.Controls.Add(this.bGenerate);
            this.groupBox3.Controls.Add(this.bTextEdit);
            this.groupBox3.Controls.Add(this.tbTrainText);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.listTrains);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // nudFont
            // 
            this.nudFont.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudFont, "nudFont");
            this.nudFont.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudFont.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudFont.Name = "nudFont";
            this.nudFont.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            this.nudFont.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudFont.ValueChanged += new System.EventHandler(this.nudFont_ValueChanged);
            // 
            // bGenerate
            // 
            resources.ApplyResources(this.bGenerate, "bGenerate");
            this.bGenerate.Name = "bGenerate";
            this.bGenerate.UseVisualStyleBackColor = true;
            this.bGenerate.Click += new System.EventHandler(this.bGenerate_Click);
            // 
            // bTextEdit
            // 
            resources.ApplyResources(this.bTextEdit, "bTextEdit");
            this.bTextEdit.Name = "bTextEdit";
            this.bTextEdit.UseVisualStyleBackColor = true;
            this.bTextEdit.Click += new System.EventHandler(this.bTextEdit_Click);
            // 
            // tbTrainText
            // 
            this.tbTrainText.BorderColor = System.Drawing.Color.DimGray;
            this.tbTrainText.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbTrainText.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbTrainText.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbTrainText.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbTrainText.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbTrainText.HintText = null;
            resources.ApplyResources(this.tbTrainText, "tbTrainText");
            this.tbTrainText.Name = "tbTrainText";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // listTrains
            // 
            this.listTrains.FormattingEnabled = true;
            resources.ApplyResources(this.listTrains, "listTrains");
            this.listTrains.Name = "listTrains";
            this.listTrains.SelectedIndexChanged += new System.EventHandler(this.listTrains_SelectedIndexChanged);
            this.listTrains.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.listTrains_Format);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbComment);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // tbComment
            // 
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
            // tableTextRealizationBindingSource
            // 
            this.tableTextRealizationBindingSource.DataSource = typeof(GVDEditor.Entities.TableTextRealization);
            // 
            // FTableText
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
            this.Name = "FTableText";
            this.ShowInTaskbar = false;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FTableText_HelpButtonClicked);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFont)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableTextRealizationBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExControls.ExButton bSave;
        private ExControls.ExButton bStorno;
        private ExGroupBox groupBox1;
        private ExTextBox tbKey;
        private ExTextBox tbName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private ExGroupBox groupBox2;
        private ExControls.ExButton bReDelete;
        private ExControls.ExButton bReEdit;
        private ExControls.ExButton bReAdd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listRealisations;
        private ExComboBox cbCatalogItem;
        private System.Windows.Forms.Label label5;
        private ExComboBox cbCatalogTable;
        private ExGroupBox groupBox3;
        private ExControls.ExButton bTextEdit;
        private ExTextBox tbTrainText;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox listTrains;
        private ExGroupBox groupBox4;
        private ExTextBox tbComment;
        private ExControls.ExButton bGenerate;
        private System.Windows.Forms.BindingSource tableTextRealizationBindingSource;
        private System.Windows.Forms.Label label8;
        private ExNumericUpDown nudFont;
    }
}