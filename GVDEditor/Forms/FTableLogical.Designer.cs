using ExControls;

namespace GVDEditor.Forms
{
    partial class FTableLogical
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FTableLogical));
            ExComboBoxStyle exComboBoxStyle1 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle2 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle3 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle4 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle5 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle6 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle7 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle8 = new ExComboBoxStyle();
            bSave = new ExButton();
            bStorno = new ExButton();
            groupBox4 = new ExGroupBox();
            tbComment = new ExTextBox();
            gboxSimple = new ExGroupBox();
            dgvZostava = new DataGridView();
            colTable = new DataGridViewTextBoxColumn();
            colFirstRecord = new DataGridViewTextBoxColumn();
            colLastRecord = new DataGridViewTextBoxColumn();
            colStartRow = new DataGridViewTextBoxColumn();
            colTypeView = new DataGridViewExComboBoxColumn();
            bRemoveTab = new ExButton();
            bAddTab = new ExButton();
            label5 = new Label();
            label4 = new Label();
            listFyzTab = new ListBox();
            groupBox1 = new ExGroupBox();
            nudCountRecords = new ExNumericUpDown();
            cbTypeView = new ExComboBox();
            label7 = new Label();
            tbKey = new ExTextBox();
            tbName = new ExTextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            labelStation = new Label();
            cbIdStation = new ExComboBox();
            groupBox4.SuspendLayout();
            gboxSimple.SuspendLayout();
            ((ISupportInitialize)dgvZostava).BeginInit();
            groupBox1.SuspendLayout();
            ((ISupportInitialize)nudCountRecords).BeginInit();
            SuspendLayout();
            // 
            // bSave
            // 
            resources.ApplyResources(bSave, "bSave");
            bSave.DefaultStyle = true;
            bSave.DialogResult = DialogResult.Cancel;
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
            tbComment.AcceptsReturn = true;
            tbComment.AcceptsTab = true;
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
            // gboxSimple
            // 
            gboxSimple.BorderColor = Color.LightGray;
            gboxSimple.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            gboxSimple.BorderThickness = 1;
            gboxSimple.Controls.Add(dgvZostava);
            gboxSimple.Controls.Add(bRemoveTab);
            gboxSimple.Controls.Add(bAddTab);
            gboxSimple.Controls.Add(label5);
            gboxSimple.Controls.Add(label4);
            gboxSimple.Controls.Add(listFyzTab);
            gboxSimple.DefaultStyle = true;
            gboxSimple.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(gboxSimple, "gboxSimple");
            gboxSimple.Name = "gboxSimple";
            gboxSimple.TabStop = false;
            // 
            // dgvZostava
            // 
            dgvZostava.AllowUserToAddRows = false;
            dgvZostava.AllowUserToDeleteRows = false;
            dgvZostava.AllowUserToResizeColumns = false;
            dgvZostava.AllowUserToResizeRows = false;
            dgvZostava.AutoGenerateColumns = false;
            dgvZostava.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvZostava.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvZostava.Columns.AddRange(new DataGridViewColumn[] { colTable, colFirstRecord, colLastRecord, colStartRow, colTypeView });
            resources.ApplyResources(dgvZostava, "dgvZostava");
            dgvZostava.Name = "dgvZostava";
            dgvZostava.RowHeadersVisible = false;
            dgvZostava.RowTemplate.Height = 24;
            dgvZostava.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvZostava.CellValidating += dgvZostava_CellValidating;
            dgvZostava.CellValueChanged += dgvZostava_CellValueChanged;
            dgvZostava.CurrentCellDirtyStateChanged += dgvZostava_CurrentCellDirtyStateChanged;
            dgvZostava.DataError += dgvZostava_DataError;
            // 
            // bRemoveTab
            // 
            resources.ApplyResources(bRemoveTab, "bRemoveTab");
            bRemoveTab.DefaultStyle = true;
            bRemoveTab.Name = "bRemoveTab";
            bRemoveTab.UseVisualStyleBackColor = true;
            bRemoveTab.Click += bRemoveTab_Click;
            // 
            // bAddTab
            // 
            resources.ApplyResources(bAddTab, "bAddTab");
            bAddTab.DefaultStyle = true;
            bAddTab.Name = "bAddTab";
            bAddTab.UseVisualStyleBackColor = true;
            bAddTab.Click += bAddTab_Click;
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // listFyzTab
            // 
            listFyzTab.FormattingEnabled = true;
            resources.ApplyResources(listFyzTab, "listFyzTab");
            listFyzTab.Name = "listFyzTab";
            listFyzTab.DoubleClick += listFyzTab_DoubleClick;
            // 
            // groupBox1
            // 
            groupBox1.BorderColor = Color.LightGray;
            groupBox1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            groupBox1.BorderThickness = 1;
            groupBox1.Controls.Add(nudCountRecords);
            groupBox1.Controls.Add(cbTypeView);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(tbKey);
            groupBox1.Controls.Add(tbName);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(labelStation);
            groupBox1.Controls.Add(cbIdStation);
            groupBox1.DefaultStyle = true;
            groupBox1.DisabledForeColor = SystemColors.GrayText;
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // nudCountRecords
            // 
            nudCountRecords.ArrowsColor = Color.Black;
            nudCountRecords.BorderColor = Color.Gainsboro;
            nudCountRecords.DefaultStyle = true;
            nudCountRecords.HighlightColor = SystemColors.Highlight;
            resources.ApplyResources(nudCountRecords, "nudCountRecords");
            nudCountRecords.Name = "nudCountRecords";
            nudCountRecords.SelectedButtonColor = SystemColors.Highlight;
            nudCountRecords.ValueChanged += nudCountRecords_ValueChanged;
            // 
            // cbTypeView
            // 
            cbTypeView.DefaultStyle = true;
            cbTypeView.DropDownBackColor = Color.White;
            cbTypeView.DropDownSelectedRowBackColor = SystemColors.Highlight;
            cbTypeView.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTypeView.FormattingEnabled = true;
            resources.ApplyResources(cbTypeView, "cbTypeView");
            cbTypeView.Name = "cbTypeView";
            cbTypeView.SelectedIndexChanged += cbTypeView_SelectedIndexChanged;
            exComboBoxStyle1.ArrowColor = null;
            exComboBoxStyle1.BackColor = null;
            exComboBoxStyle1.BorderColor = null;
            exComboBoxStyle1.ButtonBackColor = null;
            exComboBoxStyle1.ButtonBorderColor = null;
            exComboBoxStyle1.ButtonRenderFirst = null;
            exComboBoxStyle1.ForeColor = null;
            cbTypeView.StyleDisabled = exComboBoxStyle1;
            exComboBoxStyle2.ArrowColor = null;
            exComboBoxStyle2.BackColor = null;
            exComboBoxStyle2.BorderColor = null;
            exComboBoxStyle2.ButtonBackColor = null;
            exComboBoxStyle2.ButtonBorderColor = null;
            exComboBoxStyle2.ButtonRenderFirst = null;
            exComboBoxStyle2.ForeColor = null;
            cbTypeView.StyleHighlight = exComboBoxStyle2;
            exComboBoxStyle3.ArrowColor = null;
            exComboBoxStyle3.BackColor = null;
            exComboBoxStyle3.BorderColor = null;
            exComboBoxStyle3.ButtonBackColor = null;
            exComboBoxStyle3.ButtonBorderColor = null;
            exComboBoxStyle3.ButtonRenderFirst = null;
            exComboBoxStyle3.ForeColor = null;
            cbTypeView.StyleNormal = exComboBoxStyle3;
            exComboBoxStyle4.ArrowColor = null;
            exComboBoxStyle4.BackColor = null;
            exComboBoxStyle4.BorderColor = null;
            exComboBoxStyle4.ButtonBackColor = null;
            exComboBoxStyle4.ButtonBorderColor = null;
            exComboBoxStyle4.ButtonRenderFirst = null;
            exComboBoxStyle4.ForeColor = null;
            cbTypeView.StyleSelected = exComboBoxStyle4;
            cbTypeView.UseDarkScrollBar = false;
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
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
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
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
            // labelStation
            // 
            resources.ApplyResources(labelStation, "labelStation");
            labelStation.Name = "labelStation";
            // 
            // cbIdStation
            // 
            cbIdStation.DefaultStyle = true;
            cbIdStation.DropDownBackColor = Color.White;
            cbIdStation.DropDownSelectedRowBackColor = SystemColors.Highlight;
            cbIdStation.FormattingEnabled = true;
            resources.ApplyResources(cbIdStation, "cbIdStation");
            cbIdStation.Name = "cbIdStation";
            exComboBoxStyle5.ArrowColor = null;
            exComboBoxStyle5.BackColor = null;
            exComboBoxStyle5.BorderColor = null;
            exComboBoxStyle5.ButtonBackColor = null;
            exComboBoxStyle5.ButtonBorderColor = null;
            exComboBoxStyle5.ButtonRenderFirst = null;
            exComboBoxStyle5.ForeColor = null;
            cbIdStation.StyleDisabled = exComboBoxStyle5;
            exComboBoxStyle6.ArrowColor = null;
            exComboBoxStyle6.BackColor = null;
            exComboBoxStyle6.BorderColor = null;
            exComboBoxStyle6.ButtonBackColor = null;
            exComboBoxStyle6.ButtonBorderColor = null;
            exComboBoxStyle6.ButtonRenderFirst = null;
            exComboBoxStyle6.ForeColor = null;
            cbIdStation.StyleHighlight = exComboBoxStyle6;
            exComboBoxStyle7.ArrowColor = null;
            exComboBoxStyle7.BackColor = null;
            exComboBoxStyle7.BorderColor = null;
            exComboBoxStyle7.ButtonBackColor = null;
            exComboBoxStyle7.ButtonBorderColor = null;
            exComboBoxStyle7.ButtonRenderFirst = null;
            exComboBoxStyle7.ForeColor = null;
            cbIdStation.StyleNormal = exComboBoxStyle7;
            exComboBoxStyle8.ArrowColor = null;
            exComboBoxStyle8.BackColor = null;
            exComboBoxStyle8.BorderColor = null;
            exComboBoxStyle8.ButtonBackColor = null;
            exComboBoxStyle8.ButtonBorderColor = null;
            exComboBoxStyle8.ButtonRenderFirst = null;
            exComboBoxStyle8.ForeColor = null;
            cbIdStation.StyleSelected = exComboBoxStyle8;
            cbIdStation.UseDarkScrollBar = false;
            // 
            // colTable
            // 
            colTable.DataPropertyName = "Table";
            colTable.FillWeight = 180F;
            resources.ApplyResources(colTable, "colTable");
            colTable.Name = "colTable";
            colTable.ReadOnly = true;
            // 
            // colFirstRecord
            // 
            colFirstRecord.DataPropertyName = "FirstRecord";
            colFirstRecord.FillWeight = 70F;
            resources.ApplyResources(colFirstRecord, "colFirstRecord");
            colFirstRecord.Name = "colFirstRecord";
            // 
            // colLastRecord
            // 
            colLastRecord.DataPropertyName = "LastRecord";
            colLastRecord.FillWeight = 70F;
            resources.ApplyResources(colLastRecord, "colLastRecord");
            colLastRecord.Name = "colLastRecord";
            // 
            // colStartRow
            // 
            colStartRow.DataPropertyName = "StartRow";
            colStartRow.FillWeight = 70F;
            resources.ApplyResources(colStartRow, "colStartRow");
            colStartRow.Name = "colStartRow";
            // 
            // colTypeView
            // 
            colTypeView.DataPropertyName = "TypeView";
            colTypeView.DisplayMember = "Name";
            colTypeView.FillWeight = 130F;
            resources.ApplyResources(colTypeView, "colTypeView");
            colTypeView.Name = "colTypeView";
            colTypeView.ValueMember = "This";
            // 
            // FTableLogical
            // 
            AcceptButton = bSave;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = bStorno;
            Controls.Add(groupBox4);
            Controls.Add(gboxSimple);
            Controls.Add(groupBox1);
            Controls.Add(bStorno);
            Controls.Add(bSave);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            HelpButton = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FTableLogical";
            ShowInTaskbar = false;
            HelpButtonClicked += FTableLogical_HelpButtonClicked;
            Shown += FTableLogical_Shown;
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            gboxSimple.ResumeLayout(false);
            gboxSimple.PerformLayout();
            ((ISupportInitialize)dgvZostava).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((ISupportInitialize)nudCountRecords).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private ExControls.ExButton bSave;
        private ExControls.ExButton bStorno;
        private ExGroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private ExComboBox cbTypeView;
        private ExControls.ExComboBox cbIdStation;
        private ExTextBox tbKey;
        private ExTextBox tbName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelStation;
        private ExGroupBox gboxSimple;
        private ExControls.ExButton bRemoveTab;
        private ExControls.ExButton bAddTab;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listFyzTab;
        private ExNumericUpDown nudCountRecords;
        private System.Windows.Forms.Label label7;
        private ExGroupBox groupBox4;
        private ExTextBox tbComment;
        private System.Windows.Forms.DataGridView dgvZostava;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFirstRecord;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastRecord;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartRow;
        private ExControls.DataGridViewExComboBoxColumn colTypeView;
    }
}