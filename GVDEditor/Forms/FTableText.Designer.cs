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
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FTableText));
            ExComboBoxStyle exComboBoxStyle9 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle10 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle11 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle12 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle13 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle14 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle15 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle16 = new ExComboBoxStyle();
            bSave = new ExButton();
            bStorno = new ExButton();
            groupBox1 = new ExGroupBox();
            tbKey = new ExTextBox();
            tbName = new ExTextBox();
            label2 = new Label();
            label1 = new Label();
            groupBox2 = new ExGroupBox();
            cbCatalogItem = new ExComboBox();
            label5 = new Label();
            cbCatalogTable = new ExComboBox();
            bReDelete = new ExButton();
            bReEdit = new ExButton();
            bReAdd = new ExButton();
            label4 = new Label();
            label3 = new Label();
            listRealisations = new ListBox();
            groupBox3 = new ExGroupBox();
            label8 = new Label();
            nudFont = new ExNumericUpDown();
            bGenerate = new ExButton();
            bTextEdit = new ExButton();
            tbTrainText = new ExTextBox();
            label7 = new Label();
            label6 = new Label();
            listTrains = new ListBox();
            groupBox4 = new ExGroupBox();
            tbComment = new ExTextBox();
            tableTextRealizationBindingSource = new BindingSource(components);
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((ISupportInitialize)nudFont).BeginInit();
            groupBox4.SuspendLayout();
            ((ISupportInitialize)tableTextRealizationBindingSource).BeginInit();
            SuspendLayout();
            // 
            // bSave
            // 
            resources.ApplyResources(bSave, "bSave");
            bSave.DefaultStyle = true;
            bSave.Name = "bSave";
            bSave.UseVisualStyleBackColor = true;
            bSave.Click += bSave_Click;
            // 
            // bStorno
            // 
            resources.ApplyResources(bStorno, "bStorno");
            bStorno.DefaultStyle = true;
            bStorno.DialogResult = DialogResult.Cancel;
            bStorno.Name = "bStorno";
            bStorno.UseVisualStyleBackColor = true;
            bStorno.Click += bStorno_Click;
            // 
            // groupBox1
            // 
            groupBox1.BorderColor = Color.LightGray;
            groupBox1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox1.BorderThickness = 1;
            groupBox1.Controls.Add(tbKey);
            groupBox1.Controls.Add(tbName);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.DefaultStyle = true;
            groupBox1.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // tbKey
            // 
            tbKey.BorderColor = Color.DimGray;
            tbKey.BorderThickness = 1;
            tbKey.DefaultStyle = true;
            tbKey.DisabledBackColor = SystemColors.Control;
            tbKey.DisabledBorderColor = SystemColors.InactiveBorder;
            tbKey.DisabledForeColor = SystemColors.GrayText;
            tbKey.HighlightColor = SystemColors.Highlight;
            tbKey.HintForeColor = SystemColors.GrayText;
            tbKey.HintText = null;
            resources.ApplyResources(tbKey, "tbKey");
            tbKey.Name = "tbKey";
            // 
            // tbName
            // 
            tbName.BorderColor = Color.DimGray;
            tbName.BorderThickness = 1;
            tbName.DefaultStyle = true;
            tbName.DisabledBackColor = SystemColors.Control;
            tbName.DisabledBorderColor = SystemColors.InactiveBorder;
            tbName.DisabledForeColor = SystemColors.GrayText;
            tbName.HighlightColor = SystemColors.Highlight;
            tbName.HintForeColor = SystemColors.GrayText;
            tbName.HintText = null;
            resources.ApplyResources(tbName, "tbName");
            tbName.Name = "tbName";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // groupBox2
            // 
            groupBox2.BorderColor = Color.LightGray;
            groupBox2.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox2.BorderThickness = 1;
            groupBox2.Controls.Add(cbCatalogItem);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(cbCatalogTable);
            groupBox2.Controls.Add(bReDelete);
            groupBox2.Controls.Add(bReEdit);
            groupBox2.Controls.Add(bReAdd);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(listRealisations);
            groupBox2.DefaultStyle = true;
            groupBox2.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox2, "groupBox2");
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            // 
            // cbCatalogItem
            // 
            cbCatalogItem.DefaultStyle = true;
            cbCatalogItem.DropDownBackColor = Color.White;
            cbCatalogItem.DropDownSelectedRowBackColor = SystemColors.Highlight;
            cbCatalogItem.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCatalogItem.FormattingEnabled = true;
            resources.ApplyResources(cbCatalogItem, "cbCatalogItem");
            cbCatalogItem.Name = "cbCatalogItem";
            exComboBoxStyle9.ArrowColor = null;
            exComboBoxStyle9.BackColor = null;
            exComboBoxStyle9.BorderColor = null;
            exComboBoxStyle9.ButtonBackColor = null;
            exComboBoxStyle9.ButtonBorderColor = null;
            exComboBoxStyle9.ButtonRenderFirst = null;
            exComboBoxStyle9.ForeColor = null;
            cbCatalogItem.StyleDisabled = exComboBoxStyle9;
            exComboBoxStyle10.ArrowColor = null;
            exComboBoxStyle10.BackColor = null;
            exComboBoxStyle10.BorderColor = null;
            exComboBoxStyle10.ButtonBackColor = null;
            exComboBoxStyle10.ButtonBorderColor = null;
            exComboBoxStyle10.ButtonRenderFirst = null;
            exComboBoxStyle10.ForeColor = null;
            cbCatalogItem.StyleHighlight = exComboBoxStyle10;
            exComboBoxStyle11.ArrowColor = null;
            exComboBoxStyle11.BackColor = null;
            exComboBoxStyle11.BorderColor = null;
            exComboBoxStyle11.ButtonBackColor = null;
            exComboBoxStyle11.ButtonBorderColor = null;
            exComboBoxStyle11.ButtonRenderFirst = null;
            exComboBoxStyle11.ForeColor = null;
            cbCatalogItem.StyleNormal = exComboBoxStyle11;
            exComboBoxStyle12.ArrowColor = null;
            exComboBoxStyle12.BackColor = null;
            exComboBoxStyle12.BorderColor = null;
            exComboBoxStyle12.ButtonBackColor = null;
            exComboBoxStyle12.ButtonBorderColor = null;
            exComboBoxStyle12.ButtonRenderFirst = null;
            exComboBoxStyle12.ForeColor = null;
            cbCatalogItem.StyleSelected = exComboBoxStyle12;
            cbCatalogItem.UseDarkScrollBar = false;
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // cbCatalogTable
            // 
            cbCatalogTable.DefaultStyle = true;
            cbCatalogTable.DropDownBackColor = Color.White;
            cbCatalogTable.DropDownSelectedRowBackColor = SystemColors.Highlight;
            cbCatalogTable.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCatalogTable.FormattingEnabled = true;
            resources.ApplyResources(cbCatalogTable, "cbCatalogTable");
            cbCatalogTable.Name = "cbCatalogTable";
            exComboBoxStyle13.ArrowColor = null;
            exComboBoxStyle13.BackColor = null;
            exComboBoxStyle13.BorderColor = null;
            exComboBoxStyle13.ButtonBackColor = null;
            exComboBoxStyle13.ButtonBorderColor = null;
            exComboBoxStyle13.ButtonRenderFirst = null;
            exComboBoxStyle13.ForeColor = null;
            cbCatalogTable.StyleDisabled = exComboBoxStyle13;
            exComboBoxStyle14.ArrowColor = null;
            exComboBoxStyle14.BackColor = null;
            exComboBoxStyle14.BorderColor = null;
            exComboBoxStyle14.ButtonBackColor = null;
            exComboBoxStyle14.ButtonBorderColor = null;
            exComboBoxStyle14.ButtonRenderFirst = null;
            exComboBoxStyle14.ForeColor = null;
            cbCatalogTable.StyleHighlight = exComboBoxStyle14;
            exComboBoxStyle15.ArrowColor = null;
            exComboBoxStyle15.BackColor = null;
            exComboBoxStyle15.BorderColor = null;
            exComboBoxStyle15.ButtonBackColor = null;
            exComboBoxStyle15.ButtonBorderColor = null;
            exComboBoxStyle15.ButtonRenderFirst = null;
            exComboBoxStyle15.ForeColor = null;
            cbCatalogTable.StyleNormal = exComboBoxStyle15;
            exComboBoxStyle16.ArrowColor = null;
            exComboBoxStyle16.BackColor = null;
            exComboBoxStyle16.BorderColor = null;
            exComboBoxStyle16.ButtonBackColor = null;
            exComboBoxStyle16.ButtonBorderColor = null;
            exComboBoxStyle16.ButtonRenderFirst = null;
            exComboBoxStyle16.ForeColor = null;
            cbCatalogTable.StyleSelected = exComboBoxStyle16;
            cbCatalogTable.UseDarkScrollBar = false;
            cbCatalogTable.SelectedIndexChanged += cbCatalogTable_SelectedIndexChanged;
            // 
            // bReDelete
            // 
            resources.ApplyResources(bReDelete, "bReDelete");
            bReDelete.DefaultStyle = true;
            bReDelete.Name = "bReDelete";
            bReDelete.UseVisualStyleBackColor = true;
            bReDelete.Click += bReDelete_Click;
            // 
            // bReEdit
            // 
            resources.ApplyResources(bReEdit, "bReEdit");
            bReEdit.DefaultStyle = true;
            bReEdit.Name = "bReEdit";
            bReEdit.UseVisualStyleBackColor = true;
            bReEdit.Click += bReEdit_Click;
            // 
            // bReAdd
            // 
            resources.ApplyResources(bReAdd, "bReAdd");
            bReAdd.DefaultStyle = true;
            bReAdd.Name = "bReAdd";
            bReAdd.UseVisualStyleBackColor = true;
            bReAdd.Click += bReAdd_Click;
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // listRealisations
            // 
            listRealisations.FormattingEnabled = true;
            resources.ApplyResources(listRealisations, "listRealisations");
            listRealisations.Name = "listRealisations";
            listRealisations.SelectedIndexChanged += listRealisations_SelectedIndexChanged;
            listRealisations.Format += listRealisations_Format;
            // 
            // groupBox3
            // 
            groupBox3.BorderColor = Color.LightGray;
            groupBox3.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox3.BorderThickness = 1;
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(nudFont);
            groupBox3.Controls.Add(bGenerate);
            groupBox3.Controls.Add(bTextEdit);
            groupBox3.Controls.Add(tbTrainText);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(listTrains);
            groupBox3.DefaultStyle = true;
            groupBox3.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox3, "groupBox3");
            groupBox3.Name = "groupBox3";
            groupBox3.TabStop = false;
            // 
            // label8
            // 
            resources.ApplyResources(label8, "label8");
            label8.Name = "label8";
            // 
            // nudFont
            // 
            nudFont.ArrowsColor = Color.Black;
            nudFont.BorderColor = Color.Gainsboro;
            nudFont.DefaultStyle = true;
            nudFont.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(nudFont, "nudFont");
            nudFont.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            nudFont.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            nudFont.Name = "nudFont";
            nudFont.SelectedButtonColor = SystemColors.Highlight;
            nudFont.Value = new decimal(new int[] { 1, 0, 0, int.MinValue });
            nudFont.ValueChanged += nudFont_ValueChanged;
            // 
            // bGenerate
            // 
            resources.ApplyResources(bGenerate, "bGenerate");
            bGenerate.DefaultStyle = true;
            bGenerate.Name = "bGenerate";
            bGenerate.UseVisualStyleBackColor = true;
            bGenerate.Click += bGenerate_Click;
            // 
            // bTextEdit
            // 
            resources.ApplyResources(bTextEdit, "bTextEdit");
            bTextEdit.DefaultStyle = true;
            bTextEdit.Name = "bTextEdit";
            bTextEdit.UseVisualStyleBackColor = true;
            bTextEdit.Click += bTextEdit_Click;
            // 
            // tbTrainText
            // 
            tbTrainText.BorderColor = Color.DimGray;
            tbTrainText.BorderThickness = 1;
            tbTrainText.DefaultStyle = true;
            tbTrainText.DisabledBackColor = SystemColors.Control;
            tbTrainText.DisabledBorderColor = SystemColors.InactiveBorder;
            tbTrainText.DisabledForeColor = SystemColors.GrayText;
            tbTrainText.HighlightColor = SystemColors.Highlight;
            tbTrainText.HintForeColor = SystemColors.GrayText;
            tbTrainText.HintText = null;
            resources.ApplyResources(tbTrainText, "tbTrainText");
            tbTrainText.Name = "tbTrainText";
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // listTrains
            // 
            listTrains.FormattingEnabled = true;
            resources.ApplyResources(listTrains, "listTrains");
            listTrains.Name = "listTrains";
            listTrains.SelectedIndexChanged += listTrains_SelectedIndexChanged;
            listTrains.Format += listTrains_Format;
            // 
            // groupBox4
            // 
            groupBox4.BorderColor = Color.LightGray;
            groupBox4.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox4.BorderThickness = 1;
            groupBox4.Controls.Add(tbComment);
            groupBox4.DefaultStyle = true;
            groupBox4.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox4, "groupBox4");
            groupBox4.Name = "groupBox4";
            groupBox4.TabStop = false;
            // 
            // tbComment
            // 
            tbComment.BorderColor = Color.DimGray;
            tbComment.BorderThickness = 1;
            tbComment.DefaultStyle = true;
            tbComment.DisabledBackColor = SystemColors.Control;
            tbComment.DisabledBorderColor = SystemColors.InactiveBorder;
            tbComment.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(tbComment, "tbComment");
            tbComment.HighlightColor = SystemColors.Highlight;
            tbComment.HintForeColor = SystemColors.GrayText;
            tbComment.HintText = null;
            tbComment.Name = "tbComment";
            // 
            // tableTextRealizationBindingSource
            // 
            tableTextRealizationBindingSource.DataSource = typeof(TableTextRealization);
            // 
            // FTableText
            // 
            AcceptButton = bSave;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = bStorno;
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(bStorno);
            Controls.Add(bSave);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            HelpButton = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FTableText";
            ShowInTaskbar = false;
            HelpButtonClicked += FTableText_HelpButtonClicked;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((ISupportInitialize)nudFont).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((ISupportInitialize)tableTextRealizationBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();

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