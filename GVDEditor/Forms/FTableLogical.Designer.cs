using ExControls;
using GVDEditor.Entities;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTableLogical));
            ExControls.ExComboBoxStyle exComboBoxStyle1 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle2 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle3 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle4 = new ExControls.ExComboBoxStyle();
            this.bSave = new ExControls.ExButton();
            this.bStorno = new ExControls.ExButton();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLogicalZostavaBindingSource = new System.Windows.Forms.BindingSource();
            this.groupBox4 = new ExControls.ExGroupBox();
            this.tbComment = new ExControls.ExTextBox();
            this.gboxSimple = new ExControls.ExGroupBox();
            this.dgvZostava = new System.Windows.Forms.DataGridView();
            this.startRowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endRowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bRemoveTab = new ExControls.ExButton();
            this.bAddTab = new ExControls.ExButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listFyzTab = new System.Windows.Forms.ListBox();
            this.groupBox1 = new ExControls.ExGroupBox();
            this.nudCountRecords = new ExControls.ExNumericUpDown();
            this.cbTypeView = new ExControls.ExComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbKey = new ExControls.ExTextBox();
            this.tbName = new ExControls.ExTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tablePhysicalBindingSource = new System.Windows.Forms.BindingSource();
            ((System.ComponentModel.ISupportInitialize)(this.tableLogicalZostavaBindingSource)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.gboxSimple.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvZostava)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCountRecords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePhysicalBindingSource)).BeginInit();
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
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Table";
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // tableLogicalZostavaBindingSource
            // 
            this.tableLogicalZostavaBindingSource.DataSource = typeof(GVDEditor.Forms.FTableLogical.TableLogicalZostava);
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
            // gboxSimple
            // 
            this.gboxSimple.Controls.Add(this.dgvZostava);
            this.gboxSimple.Controls.Add(this.bRemoveTab);
            this.gboxSimple.Controls.Add(this.bAddTab);
            this.gboxSimple.Controls.Add(this.label5);
            this.gboxSimple.Controls.Add(this.label4);
            this.gboxSimple.Controls.Add(this.listFyzTab);
            resources.ApplyResources(this.gboxSimple, "gboxSimple");
            this.gboxSimple.Name = "gboxSimple";
            this.gboxSimple.TabStop = false;
            // 
            // dgvZostava
            // 
            this.dgvZostava.AllowUserToAddRows = false;
            this.dgvZostava.AllowUserToDeleteRows = false;
            this.dgvZostava.AllowUserToResizeColumns = false;
            this.dgvZostava.AllowUserToResizeRows = false;
            this.dgvZostava.AutoGenerateColumns = false;
            this.dgvZostava.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvZostava.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvZostava.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.startRowDataGridViewTextBoxColumn,
            this.endRowDataGridViewTextBoxColumn,
            this.tableDataGridViewTextBoxColumn});
            this.dgvZostava.DataSource = this.tableLogicalZostavaBindingSource;
            resources.ApplyResources(this.dgvZostava, "dgvZostava");
            this.dgvZostava.Name = "dgvZostava";
            this.dgvZostava.RowHeadersVisible = false;
            this.dgvZostava.RowTemplate.Height = 24;
            this.dgvZostava.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvZostava.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvZostava_CellValidating);
            // 
            // startRowDataGridViewTextBoxColumn
            // 
            this.startRowDataGridViewTextBoxColumn.DataPropertyName = "StartRow";
            resources.ApplyResources(this.startRowDataGridViewTextBoxColumn, "startRowDataGridViewTextBoxColumn");
            this.startRowDataGridViewTextBoxColumn.Name = "startRowDataGridViewTextBoxColumn";
            // 
            // endRowDataGridViewTextBoxColumn
            // 
            this.endRowDataGridViewTextBoxColumn.DataPropertyName = "EndRow";
            resources.ApplyResources(this.endRowDataGridViewTextBoxColumn, "endRowDataGridViewTextBoxColumn");
            this.endRowDataGridViewTextBoxColumn.Name = "endRowDataGridViewTextBoxColumn";
            // 
            // tableDataGridViewTextBoxColumn
            // 
            this.tableDataGridViewTextBoxColumn.DataPropertyName = "Table";
            resources.ApplyResources(this.tableDataGridViewTextBoxColumn, "tableDataGridViewTextBoxColumn");
            this.tableDataGridViewTextBoxColumn.Name = "tableDataGridViewTextBoxColumn";
            this.tableDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bRemoveTab
            // 
            resources.ApplyResources(this.bRemoveTab, "bRemoveTab");
            this.bRemoveTab.Name = "bRemoveTab";
            this.bRemoveTab.UseVisualStyleBackColor = true;
            this.bRemoveTab.Click += new System.EventHandler(this.bRemoveTab_Click);
            // 
            // bAddTab
            // 
            resources.ApplyResources(this.bAddTab, "bAddTab");
            this.bAddTab.Name = "bAddTab";
            this.bAddTab.UseVisualStyleBackColor = true;
            this.bAddTab.Click += new System.EventHandler(this.bAddTab_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // listFyzTab
            // 
            this.listFyzTab.FormattingEnabled = true;
            resources.ApplyResources(this.listFyzTab, "listFyzTab");
            this.listFyzTab.Name = "listFyzTab";
            this.listFyzTab.DoubleClick += new System.EventHandler(this.listFyzTab_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudCountRecords);
            this.groupBox1.Controls.Add(this.cbTypeView);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbKey);
            this.groupBox1.Controls.Add(this.tbName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // nudCountRecords
            // 
            this.nudCountRecords.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudCountRecords, "nudCountRecords");
            this.nudCountRecords.Name = "nudCountRecords";
            this.nudCountRecords.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            // 
            // cbTypeView
            // 
            this.cbTypeView.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbTypeView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTypeView.FormattingEnabled = true;
            resources.ApplyResources(this.cbTypeView, "cbTypeView");
            this.cbTypeView.Name = "cbTypeView";
            exComboBoxStyle1.ArrowColor = null;
            exComboBoxStyle1.BackColor = null;
            exComboBoxStyle1.BorderColor = null;
            exComboBoxStyle1.ButtonBackColor = null;
            exComboBoxStyle1.ButtonBorderColor = null;
            exComboBoxStyle1.ButtonRenderFirst = null;
            exComboBoxStyle1.ForeColor = null;
            this.cbTypeView.StyleDisabled = exComboBoxStyle1;
            exComboBoxStyle2.ArrowColor = null;
            exComboBoxStyle2.BackColor = null;
            exComboBoxStyle2.BorderColor = null;
            exComboBoxStyle2.ButtonBackColor = null;
            exComboBoxStyle2.ButtonBorderColor = null;
            exComboBoxStyle2.ButtonRenderFirst = null;
            exComboBoxStyle2.ForeColor = null;
            this.cbTypeView.StyleHighlight = exComboBoxStyle2;
            exComboBoxStyle3.ArrowColor = null;
            exComboBoxStyle3.BackColor = null;
            exComboBoxStyle3.BorderColor = null;
            exComboBoxStyle3.ButtonBackColor = null;
            exComboBoxStyle3.ButtonBorderColor = null;
            exComboBoxStyle3.ButtonRenderFirst = null;
            exComboBoxStyle3.ForeColor = null;
            this.cbTypeView.StyleNormal = exComboBoxStyle3;
            exComboBoxStyle4.ArrowColor = null;
            exComboBoxStyle4.BackColor = null;
            exComboBoxStyle4.BorderColor = null;
            exComboBoxStyle4.ButtonBackColor = null;
            exComboBoxStyle4.ButtonBorderColor = null;
            exComboBoxStyle4.ButtonRenderFirst = null;
            exComboBoxStyle4.ForeColor = null;
            this.cbTypeView.StyleSelected = exComboBoxStyle4;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
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
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Table";
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Table";
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Table";
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Table";
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // tablePhysicalBindingSource
            // 
            this.tablePhysicalBindingSource.DataSource = typeof(GVDEditor.Entities.TablePhysical);
            // 
            // FTableLogical
            // 
            this.AcceptButton = this.bSave;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.gboxSimple);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bStorno);
            this.Controls.Add(this.bSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FTableLogical";
            this.ShowInTaskbar = false;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FTableLogical_HelpButtonClicked);
            ((System.ComponentModel.ISupportInitialize)(this.tableLogicalZostavaBindingSource)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.gboxSimple.ResumeLayout(false);
            this.gboxSimple.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvZostava)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCountRecords)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePhysicalBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExControls.ExButton bSave;
        private ExControls.ExButton bStorno;
        private ExGroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private ExComboBox cbTypeView;
        private ExTextBox tbKey;
        private ExTextBox tbName;
        private System.Windows.Forms.Label label3;
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
        private System.Windows.Forms.BindingSource tablePhysicalBindingSource;
        private System.Windows.Forms.DataGridView dgvZostava;
        private System.Windows.Forms.BindingSource tableLogicalZostavaBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn startRowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endRowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    }
}