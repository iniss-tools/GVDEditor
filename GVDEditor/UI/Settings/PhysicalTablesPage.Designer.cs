using ExControls;

namespace GVDEditor.UI.Settings
{
    partial class PhysicalTablesPage
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(PhysicalTablesPage));
            tlpMain = new TableLayoutPanel();
            lInfo = new Label();
            tlpTools = new TableLayoutPanel();
            flpButtons = new FlowLayoutPanel();
            bAdd = new ExButton();
            bDuplicate = new ExButton();
            bDelete = new ExButton();
            lFilter = new Label();
            tbFilter = new ExTextBox();
            splitMain = new SplitContainer();
            dgv = new DataGridView();
            pDetail = new Panel();
            tlpDetail = new TableLayoutPanel();
            lHint = new Label();
            lBasic = new Label();
            lName = new Label();
            tbName = new ExTextBox();
            lKey = new Label();
            tbKey = new ExTextBox();
            lCatalog = new Label();
            cbCatalog = new ExComboBox();
            lRecCount = new Label();
            nudRecCount = new ExNumericUpDown();
            lComm = new Label();
            lPort = new Label();
            nudPort = new ExNumericUpDown();
            lId = new Label();
            nudId = new ExNumericUpDown();
            lIdNote = new Label();
            lXml = new Label();
            tbXml = new ExTextBox();
            lAdvanced = new Label();
            lReverse = new Label();
            tbReverse = new ExTextBox();
            lRem = new Label();
            tbRem = new ExTextBox();
            lCommentHeader = new Label();
            tbComment = new ExTextBox();
            lUseHeader = new Label();
            lUse = new Label();
            colName = new DataGridViewTextBoxColumn();
            colKey = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)splitMain).BeginInit();
            ((ISupportInitialize)dgv).BeginInit();
            ((ISupportInitialize)nudRecCount).BeginInit();
            ((ISupportInitialize)nudPort).BeginInit();
            ((ISupportInitialize)nudId).BeginInit();
            tlpMain.SuspendLayout();
            tlpTools.SuspendLayout();
            flpButtons.SuspendLayout();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            tlpDetail.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lInfo, 0, 0);
            tlpMain.Controls.Add(tlpTools, 0, 1);
            tlpMain.Controls.Add(splitMain, 0, 2);
            tlpMain.Controls.Add(lHint, 0, 3);
            tlpMain.Name = "tlpMain";
            // 
            // lInfo
            // 
            resources.ApplyResources(lInfo, "lInfo");
            lInfo.Name = "lInfo";
            // 
            // tlpTools
            // 
            resources.ApplyResources(tlpTools, "tlpTools");
            tlpTools.Controls.Add(flpButtons, 0, 0);
            tlpTools.Controls.Add(lFilter, 2, 0);
            tlpTools.Controls.Add(tbFilter, 3, 0);
            tlpTools.Name = "tlpTools";
            // 
            // flpButtons
            // 
            resources.ApplyResources(flpButtons, "flpButtons");
            flpButtons.Controls.Add(bAdd);
            flpButtons.Controls.Add(bDuplicate);
            flpButtons.Controls.Add(bDelete);
            flpButtons.Name = "flpButtons";
            flpButtons.WrapContents = false;
            // 
            // bAdd
            // 
            resources.ApplyResources(bAdd, "bAdd");
            bAdd.Name = "bAdd";
            bAdd.UseVisualStyleBackColor = true;
            bAdd.Click += bAdd_Click;
            // 
            // bDuplicate
            // 
            resources.ApplyResources(bDuplicate, "bDuplicate");
            bDuplicate.Name = "bDuplicate";
            bDuplicate.UseVisualStyleBackColor = true;
            bDuplicate.Click += bDuplicate_Click;
            // 
            // bDelete
            // 
            resources.ApplyResources(bDelete, "bDelete");
            bDelete.Name = "bDelete";
            bDelete.UseVisualStyleBackColor = true;
            bDelete.Click += bDelete_Click;
            // 
            // lFilter
            // 
            resources.ApplyResources(lFilter, "lFilter");
            lFilter.Name = "lFilter";
            // 
            // tbFilter
            // 
            resources.ApplyResources(tbFilter, "tbFilter");
            tbFilter.Name = "tbFilter";
            // 
            // splitMain
            // 
            resources.ApplyResources(splitMain, "splitMain");
            splitMain.FixedPanel = FixedPanel.Panel1;
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(dgv);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(pDetail);
            // 
            // dgv
            // 
            resources.ApplyResources(dgv, "dgv");
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Columns.AddRange(new DataGridViewColumn[] { colName, colKey });
            dgv.MultiSelect = false;
            dgv.Name = "dgv";
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.KeyDown += dgv_KeyDown;
            // 
            // pDetail
            // 
            resources.ApplyResources(pDetail, "pDetail");
            pDetail.Controls.Add(tlpDetail);
            pDetail.Name = "pDetail";
            // 
            // tlpDetail
            // 
            resources.ApplyResources(tlpDetail, "tlpDetail");
            tlpDetail.Controls.Add(lBasic, 0, 0);
            tlpDetail.Controls.Add(lName, 0, 1);
            tlpDetail.Controls.Add(tbName, 1, 1);
            tlpDetail.Controls.Add(lKey, 0, 2);
            tlpDetail.Controls.Add(tbKey, 1, 2);
            tlpDetail.Controls.Add(lCatalog, 0, 3);
            tlpDetail.Controls.Add(cbCatalog, 1, 3);
            tlpDetail.Controls.Add(lRecCount, 0, 4);
            tlpDetail.Controls.Add(nudRecCount, 1, 4);
            tlpDetail.Controls.Add(lComm, 0, 5);
            tlpDetail.Controls.Add(lPort, 0, 6);
            tlpDetail.Controls.Add(nudPort, 1, 6);
            tlpDetail.Controls.Add(lId, 0, 7);
            tlpDetail.Controls.Add(nudId, 1, 7);
            tlpDetail.Controls.Add(lIdNote, 1, 8);
            tlpDetail.Controls.Add(lXml, 0, 9);
            tlpDetail.Controls.Add(tbXml, 1, 9);
            tlpDetail.Controls.Add(lAdvanced, 0, 10);
            tlpDetail.Controls.Add(lReverse, 0, 11);
            tlpDetail.Controls.Add(tbReverse, 1, 11);
            tlpDetail.Controls.Add(lRem, 0, 12);
            tlpDetail.Controls.Add(tbRem, 1, 12);
            tlpDetail.Controls.Add(lCommentHeader, 0, 13);
            tlpDetail.Controls.Add(tbComment, 0, 14);
            tlpDetail.Controls.Add(lUseHeader, 0, 15);
            tlpDetail.Controls.Add(lUse, 0, 16);
            tlpDetail.Name = "tlpDetail";
            tlpDetail.SetColumnSpan(lBasic, 2);
            tlpDetail.SetColumnSpan(lComm, 2);
            tlpDetail.SetColumnSpan(lAdvanced, 2);
            tlpDetail.SetColumnSpan(lCommentHeader, 2);
            tlpDetail.SetColumnSpan(tbComment, 2);
            tlpDetail.SetColumnSpan(lUseHeader, 2);
            tlpDetail.SetColumnSpan(lUse, 2);
            // 
            // lBasic
            // 
            resources.ApplyResources(lBasic, "lBasic");
            lBasic.Name = "lBasic";
            // 
            // lName
            // 
            resources.ApplyResources(lName, "lName");
            lName.Name = "lName";
            // 
            // tbName
            // 
            resources.ApplyResources(tbName, "tbName");
            tbName.Name = "tbName";
            tbName.TextChanged += Field_Changed;
            // 
            // lKey
            // 
            resources.ApplyResources(lKey, "lKey");
            lKey.Name = "lKey";
            // 
            // tbKey
            // 
            resources.ApplyResources(tbKey, "tbKey");
            tbKey.Name = "tbKey";
            tbKey.TextChanged += Field_Changed;
            // 
            // lCatalog
            // 
            resources.ApplyResources(lCatalog, "lCatalog");
            lCatalog.Name = "lCatalog";
            // 
            // cbCatalog
            // 
            resources.ApplyResources(cbCatalog, "cbCatalog");
            cbCatalog.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCatalog.FormattingEnabled = true;
            cbCatalog.Name = "cbCatalog";
            cbCatalog.SelectionChangeCommitted += Field_Changed;
            // 
            // lRecCount
            // 
            resources.ApplyResources(lRecCount, "lRecCount");
            lRecCount.Name = "lRecCount";
            // 
            // nudRecCount
            // 
            resources.ApplyResources(nudRecCount, "nudRecCount");
            nudRecCount.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudRecCount.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            nudRecCount.Name = "nudRecCount";
            nudRecCount.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudRecCount.ValueChanged += Field_Changed;
            // 
            // lComm
            // 
            resources.ApplyResources(lComm, "lComm");
            lComm.Name = "lComm";
            // 
            // lPort
            // 
            resources.ApplyResources(lPort, "lPort");
            lPort.Name = "lPort";
            // 
            // nudPort
            // 
            resources.ApplyResources(nudPort, "nudPort");
            nudPort.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudPort.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            nudPort.Name = "nudPort";
            nudPort.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudPort.ValueChanged += Field_Changed;
            // 
            // lId
            // 
            resources.ApplyResources(lId, "lId");
            lId.Name = "lId";
            // 
            // nudId
            // 
            resources.ApplyResources(nudId, "nudId");
            nudId.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudId.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            nudId.Name = "nudId";
            nudId.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudId.ValueChanged += Field_Changed;
            // 
            // lIdNote
            // 
            resources.ApplyResources(lIdNote, "lIdNote");
            lIdNote.Name = "lIdNote";
            // 
            // lXml
            // 
            resources.ApplyResources(lXml, "lXml");
            lXml.Name = "lXml";
            // 
            // tbXml
            // 
            resources.ApplyResources(tbXml, "tbXml");
            tbXml.Name = "tbXml";
            tbXml.TextChanged += Field_Changed;
            // 
            // lAdvanced
            // 
            resources.ApplyResources(lAdvanced, "lAdvanced");
            lAdvanced.Name = "lAdvanced";
            // 
            // lReverse
            // 
            resources.ApplyResources(lReverse, "lReverse");
            lReverse.Name = "lReverse";
            // 
            // tbReverse
            // 
            resources.ApplyResources(tbReverse, "tbReverse");
            tbReverse.Name = "tbReverse";
            tbReverse.TextChanged += Field_Changed;
            // 
            // lRem
            // 
            resources.ApplyResources(lRem, "lRem");
            lRem.Name = "lRem";
            // 
            // tbRem
            // 
            resources.ApplyResources(tbRem, "tbRem");
            tbRem.Name = "tbRem";
            tbRem.TextChanged += Field_Changed;
            // 
            // lCommentHeader
            // 
            resources.ApplyResources(lCommentHeader, "lCommentHeader");
            lCommentHeader.Name = "lCommentHeader";
            // 
            // tbComment
            // 
            resources.ApplyResources(tbComment, "tbComment");
            tbComment.Multiline = true;
            tbComment.Name = "tbComment";
            tbComment.TextChanged += Field_Changed;
            // 
            // lUseHeader
            // 
            resources.ApplyResources(lUseHeader, "lUseHeader");
            lUseHeader.Name = "lUseHeader";
            // 
            // lUse
            // 
            resources.ApplyResources(lUse, "lUse");
            lUse.Name = "lUse";
            // 
            // lHint
            // 
            resources.ApplyResources(lHint, "lHint");
            lHint.Name = "lHint";
            // 
            // colName
            // 
            resources.ApplyResources(colName, "colName");
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colName.Name = "colName";
            colName.ReadOnly = true;
            colName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colKey
            // 
            resources.ApplyResources(colKey, "colKey");
            colKey.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colKey.Name = "colKey";
            colKey.ReadOnly = true;
            colKey.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // PhysicalTablesPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "PhysicalTablesPage";
            ((ISupportInitialize)splitMain).EndInit();
            ((ISupportInitialize)dgv).EndInit();
            ((ISupportInitialize)nudRecCount).EndInit();
            ((ISupportInitialize)nudPort).EndInit();
            ((ISupportInitialize)nudId).EndInit();
            tlpDetail.ResumeLayout(false);
            tlpDetail.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            splitMain.ResumeLayout(false);
            flpButtons.ResumeLayout(false);
            flpButtons.PerformLayout();
            tlpTools.ResumeLayout(false);
            tlpTools.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lInfo;
        private TableLayoutPanel tlpTools;
        private FlowLayoutPanel flpButtons;
        private ExButton bAdd;
        private ExButton bDuplicate;
        private ExButton bDelete;
        private Label lFilter;
        private ExTextBox tbFilter;
        private SplitContainer splitMain;
        private DataGridView dgv;
        private Panel pDetail;
        private TableLayoutPanel tlpDetail;
        private Label lHint;
        private Label lBasic;
        private Label lName;
        private ExTextBox tbName;
        private Label lKey;
        private ExTextBox tbKey;
        private Label lCatalog;
        private ExComboBox cbCatalog;
        private Label lRecCount;
        private ExNumericUpDown nudRecCount;
        private Label lComm;
        private Label lPort;
        private ExNumericUpDown nudPort;
        private Label lId;
        private ExNumericUpDown nudId;
        private Label lIdNote;
        private Label lXml;
        private ExTextBox tbXml;
        private Label lAdvanced;
        private Label lReverse;
        private ExTextBox tbReverse;
        private Label lRem;
        private ExTextBox tbRem;
        private Label lCommentHeader;
        private ExTextBox tbComment;
        private Label lUseHeader;
        private Label lUse;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colKey;
    }
}
