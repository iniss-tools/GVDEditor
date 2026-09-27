using ExControls;

namespace GVDEditor.Forms.Settings
{
    partial class LogicalTablesPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(LogicalTablesPage));
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
            lType = new Label();
            cbType = new ExComboBox();
            lCount = new Label();
            nudCount = new ExNumericUpDown();
            lStation = new Label();
            cbStation = new ExComboBox();
            lStationNote = new Label();
            lZostava = new Label();
            lZostavaNote = new Label();
            flpZostava = new FlowLayoutPanel();
            cbAddPhysical = new ExComboBox();
            bAddPhysical = new ExButton();
            dgvZostava = new DataGridView();
            flpZostavaTools = new FlowLayoutPanel();
            bSegmentRemove = new ExButton();
            lCommentHeader = new Label();
            tbComment = new ExTextBox();
            lUseHeader = new Label();
            lUse = new Label();
            colName = new DataGridViewTextBoxColumn();
            colKey = new DataGridViewTextBoxColumn();
            colPhysical = new DataGridViewTextBoxColumn();
            colFirst = new DataGridViewTextBoxColumn();
            colLast = new DataGridViewTextBoxColumn();
            colStartRow = new DataGridViewTextBoxColumn();
            colTypeView = new DataGridViewExComboBoxColumn();
            ((ISupportInitialize)splitMain).BeginInit();
            ((ISupportInitialize)dgv).BeginInit();
            ((ISupportInitialize)nudCount).BeginInit();
            ((ISupportInitialize)dgvZostava).BeginInit();
            tlpMain.SuspendLayout();
            tlpTools.SuspendLayout();
            flpButtons.SuspendLayout();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            tlpDetail.SuspendLayout();
            flpZostava.SuspendLayout();
            flpZostavaTools.SuspendLayout();
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
            tlpDetail.Controls.Add(lType, 0, 3);
            tlpDetail.Controls.Add(cbType, 1, 3);
            tlpDetail.Controls.Add(lCount, 0, 4);
            tlpDetail.Controls.Add(nudCount, 1, 4);
            tlpDetail.Controls.Add(lStation, 0, 5);
            tlpDetail.Controls.Add(cbStation, 1, 5);
            tlpDetail.Controls.Add(lStationNote, 1, 6);
            tlpDetail.Controls.Add(lZostava, 0, 7);
            tlpDetail.Controls.Add(lZostavaNote, 0, 8);
            tlpDetail.Controls.Add(flpZostava, 0, 9);
            tlpDetail.Controls.Add(dgvZostava, 0, 10);
            tlpDetail.Controls.Add(flpZostavaTools, 0, 11);
            tlpDetail.Controls.Add(lCommentHeader, 0, 12);
            tlpDetail.Controls.Add(tbComment, 0, 13);
            tlpDetail.Controls.Add(lUseHeader, 0, 14);
            tlpDetail.Controls.Add(lUse, 0, 15);
            tlpDetail.Name = "tlpDetail";
            tlpDetail.SetColumnSpan(lBasic, 2);
            tlpDetail.SetColumnSpan(lZostava, 2);
            tlpDetail.SetColumnSpan(lZostavaNote, 2);
            tlpDetail.SetColumnSpan(flpZostava, 2);
            tlpDetail.SetColumnSpan(dgvZostava, 2);
            tlpDetail.SetColumnSpan(flpZostavaTools, 2);
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
            // lType
            // 
            resources.ApplyResources(lType, "lType");
            lType.Name = "lType";
            // 
            // cbType
            // 
            resources.ApplyResources(cbType, "cbType");
            cbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbType.FormattingEnabled = true;
            cbType.Name = "cbType";
            cbType.SelectionChangeCommitted += cbType_SelectionChangeCommitted;
            // 
            // lCount
            // 
            resources.ApplyResources(lCount, "lCount");
            lCount.Name = "lCount";
            // 
            // nudCount
            // 
            resources.ApplyResources(nudCount, "nudCount");
            nudCount.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudCount.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            nudCount.Name = "nudCount";
            nudCount.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudCount.ValueChanged += Field_Changed;
            // 
            // lStation
            // 
            resources.ApplyResources(lStation, "lStation");
            lStation.Name = "lStation";
            // 
            // cbStation
            // 
            resources.ApplyResources(cbStation, "cbStation");
            cbStation.FormattingEnabled = true;
            cbStation.Name = "cbStation";
            cbStation.TextChanged += cbStation_TextChanged;
            cbStation.SelectionChangeCommitted += cbStation_TextChanged;
            // 
            // lStationNote
            // 
            resources.ApplyResources(lStationNote, "lStationNote");
            lStationNote.Name = "lStationNote";
            // 
            // lZostava
            // 
            resources.ApplyResources(lZostava, "lZostava");
            lZostava.Name = "lZostava";
            // 
            // lZostavaNote
            // 
            resources.ApplyResources(lZostavaNote, "lZostavaNote");
            lZostavaNote.Name = "lZostavaNote";
            // 
            // flpZostava
            // 
            resources.ApplyResources(flpZostava, "flpZostava");
            flpZostava.Controls.Add(cbAddPhysical);
            flpZostava.Controls.Add(bAddPhysical);
            flpZostava.Name = "flpZostava";
            // 
            // cbAddPhysical
            // 
            resources.ApplyResources(cbAddPhysical, "cbAddPhysical");
            cbAddPhysical.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAddPhysical.FormattingEnabled = true;
            cbAddPhysical.Name = "cbAddPhysical";
            // 
            // bAddPhysical
            // 
            resources.ApplyResources(bAddPhysical, "bAddPhysical");
            bAddPhysical.Name = "bAddPhysical";
            bAddPhysical.UseVisualStyleBackColor = true;
            bAddPhysical.Click += bAddPhysical_Click;
            // 
            // dgvZostava
            // 
            resources.ApplyResources(dgvZostava, "dgvZostava");
            dgvZostava.AllowUserToAddRows = false;
            dgvZostava.AllowUserToDeleteRows = false;
            dgvZostava.AllowUserToResizeRows = false;
            dgvZostava.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvZostava.Columns.AddRange(new DataGridViewColumn[] { colPhysical, colFirst, colLast, colStartRow, colTypeView });
            dgvZostava.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvZostava.MultiSelect = false;
            dgvZostava.Name = "dgvZostava";
            dgvZostava.RowHeadersVisible = false;
            dgvZostava.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvZostava.CellValueChanged += dgvZostava_CellValueChanged;
            dgvZostava.CurrentCellDirtyStateChanged += dgvZostava_CurrentCellDirtyStateChanged;
            dgvZostava.DataError += dgvZostava_DataError;
            dgvZostava.CurrentCellChanged += dgvZostava_CurrentCellChanged;
            // 
            // flpZostavaTools
            // 
            resources.ApplyResources(flpZostavaTools, "flpZostavaTools");
            flpZostavaTools.Controls.Add(bSegmentRemove);
            flpZostavaTools.Name = "flpZostavaTools";
            // 
            // bSegmentRemove
            // 
            resources.ApplyResources(bSegmentRemove, "bSegmentRemove");
            bSegmentRemove.Name = "bSegmentRemove";
            bSegmentRemove.UseVisualStyleBackColor = true;
            bSegmentRemove.Click += bSegmentRemove_Click;
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
            // colPhysical
            // 
            resources.ApplyResources(colPhysical, "colPhysical");
            colPhysical.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colPhysical.Name = "colPhysical";
            colPhysical.ReadOnly = true;
            colPhysical.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colFirst
            // 
            resources.ApplyResources(colFirst, "colFirst");
            colFirst.Name = "colFirst";
            colFirst.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colLast
            // 
            resources.ApplyResources(colLast, "colLast");
            colLast.Name = "colLast";
            colLast.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colStartRow
            // 
            resources.ApplyResources(colStartRow, "colStartRow");
            colStartRow.Name = "colStartRow";
            colStartRow.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colTypeView
            // 
            resources.ApplyResources(colTypeView, "colTypeView");
            colTypeView.Name = "colTypeView";
            colTypeView.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // LogicalTablesPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "LogicalTablesPage";
            ((ISupportInitialize)splitMain).EndInit();
            ((ISupportInitialize)dgv).EndInit();
            ((ISupportInitialize)nudCount).EndInit();
            ((ISupportInitialize)dgvZostava).EndInit();
            flpZostavaTools.ResumeLayout(false);
            flpZostavaTools.PerformLayout();
            flpZostava.ResumeLayout(false);
            flpZostava.PerformLayout();
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
        private Label lType;
        private ExComboBox cbType;
        private Label lCount;
        private ExNumericUpDown nudCount;
        private Label lStation;
        private ExComboBox cbStation;
        private Label lStationNote;
        private Label lZostava;
        private Label lZostavaNote;
        private FlowLayoutPanel flpZostava;
        private ExComboBox cbAddPhysical;
        private ExButton bAddPhysical;
        private DataGridView dgvZostava;
        private FlowLayoutPanel flpZostavaTools;
        private ExButton bSegmentRemove;
        private Label lCommentHeader;
        private ExTextBox tbComment;
        private Label lUseHeader;
        private Label lUse;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colKey;
        private DataGridViewTextBoxColumn colPhysical;
        private DataGridViewTextBoxColumn colFirst;
        private DataGridViewTextBoxColumn colLast;
        private DataGridViewTextBoxColumn colStartRow;
        private DataGridViewExComboBoxColumn colTypeView;
    }
}
