using ExControls;

namespace GVDEditor.Forms.Settings
{
    partial class TableTextsPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TableTextsPage));
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
            lRealHeader = new Label();
            lRealNote = new Label();
            flpReal = new FlowLayoutPanel();
            bRealAdd = new ExButton();
            bRealDelete = new ExButton();
            dgvReal = new DataGridView();
            lTrainsHeader = new Label();
            flpTrains = new FlowLayoutPanel();
            cbAddTrain = new ExComboBox();
            bTrainAdd = new ExButton();
            dgvTrains = new DataGridView();
            flpTrainTools = new FlowLayoutPanel();
            bTrainRemove = new ExButton();
            bGenerate = new ExButton();
            lFont = new Label();
            cbFont = new ExComboBox();
            lCommentHeader = new Label();
            tbComment = new ExTextBox();
            colName = new DataGridViewTextBoxColumn();
            colKey = new DataGridViewTextBoxColumn();
            colRealTable = new DataGridViewExComboBoxColumn();
            colRealItem = new DataGridViewExComboBoxColumn();
            colTrain = new DataGridViewTextBoxColumn();
            colText = new DataGridViewTextBoxColumn();
            colFont = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)splitMain).BeginInit();
            ((ISupportInitialize)dgv).BeginInit();
            ((ISupportInitialize)dgvReal).BeginInit();
            ((ISupportInitialize)dgvTrains).BeginInit();
            tlpMain.SuspendLayout();
            tlpTools.SuspendLayout();
            flpButtons.SuspendLayout();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            tlpDetail.SuspendLayout();
            flpReal.SuspendLayout();
            flpTrains.SuspendLayout();
            flpTrainTools.SuspendLayout();
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
            tlpDetail.Controls.Add(lRealHeader, 0, 3);
            tlpDetail.Controls.Add(lRealNote, 0, 4);
            tlpDetail.Controls.Add(flpReal, 0, 5);
            tlpDetail.Controls.Add(dgvReal, 0, 6);
            tlpDetail.Controls.Add(lTrainsHeader, 0, 7);
            tlpDetail.Controls.Add(flpTrains, 0, 8);
            tlpDetail.Controls.Add(dgvTrains, 0, 9);
            tlpDetail.Controls.Add(flpTrainTools, 0, 10);
            tlpDetail.Controls.Add(lFont, 0, 11);
            tlpDetail.Controls.Add(cbFont, 1, 11);
            tlpDetail.Controls.Add(lCommentHeader, 0, 12);
            tlpDetail.Controls.Add(tbComment, 0, 13);
            tlpDetail.Name = "tlpDetail";
            tlpDetail.SetColumnSpan(lBasic, 2);
            tlpDetail.SetColumnSpan(lRealHeader, 2);
            tlpDetail.SetColumnSpan(lRealNote, 2);
            tlpDetail.SetColumnSpan(flpReal, 2);
            tlpDetail.SetColumnSpan(dgvReal, 2);
            tlpDetail.SetColumnSpan(lTrainsHeader, 2);
            tlpDetail.SetColumnSpan(flpTrains, 2);
            tlpDetail.SetColumnSpan(dgvTrains, 2);
            tlpDetail.SetColumnSpan(flpTrainTools, 2);
            tlpDetail.SetColumnSpan(lCommentHeader, 2);
            tlpDetail.SetColumnSpan(tbComment, 2);
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
            // lRealHeader
            // 
            resources.ApplyResources(lRealHeader, "lRealHeader");
            lRealHeader.Name = "lRealHeader";
            // 
            // lRealNote
            // 
            resources.ApplyResources(lRealNote, "lRealNote");
            lRealNote.Name = "lRealNote";
            // 
            // flpReal
            // 
            resources.ApplyResources(flpReal, "flpReal");
            flpReal.Controls.Add(bRealAdd);
            flpReal.Controls.Add(bRealDelete);
            flpReal.Name = "flpReal";
            // 
            // bRealAdd
            // 
            resources.ApplyResources(bRealAdd, "bRealAdd");
            bRealAdd.Name = "bRealAdd";
            bRealAdd.UseVisualStyleBackColor = true;
            bRealAdd.Click += bRealAdd_Click;
            // 
            // bRealDelete
            // 
            resources.ApplyResources(bRealDelete, "bRealDelete");
            bRealDelete.Name = "bRealDelete";
            bRealDelete.UseVisualStyleBackColor = true;
            bRealDelete.Click += bRealDelete_Click;
            // 
            // dgvReal
            // 
            resources.ApplyResources(dgvReal, "dgvReal");
            dgvReal.AllowUserToAddRows = false;
            dgvReal.AllowUserToDeleteRows = false;
            dgvReal.AllowUserToResizeRows = false;
            dgvReal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReal.Columns.AddRange(new DataGridViewColumn[] { colRealTable, colRealItem });
            dgvReal.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvReal.MultiSelect = false;
            dgvReal.Name = "dgvReal";
            dgvReal.RowHeadersVisible = false;
            dgvReal.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvReal.CellValueChanged += dgvReal_CellValueChanged;
            dgvReal.CurrentCellDirtyStateChanged += dgvReal_CurrentCellDirtyStateChanged;
            dgvReal.DataError += dgvReal_DataError;
            dgvReal.CurrentCellChanged += dgvReal_CurrentCellChanged;
            // 
            // lTrainsHeader
            // 
            resources.ApplyResources(lTrainsHeader, "lTrainsHeader");
            lTrainsHeader.Name = "lTrainsHeader";
            // 
            // flpTrains
            // 
            resources.ApplyResources(flpTrains, "flpTrains");
            flpTrains.Controls.Add(cbAddTrain);
            flpTrains.Controls.Add(bTrainAdd);
            flpTrains.Name = "flpTrains";
            // 
            // cbAddTrain
            // 
            resources.ApplyResources(cbAddTrain, "cbAddTrain");
            cbAddTrain.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAddTrain.FormattingEnabled = true;
            cbAddTrain.Name = "cbAddTrain";
            // 
            // bTrainAdd
            // 
            resources.ApplyResources(bTrainAdd, "bTrainAdd");
            bTrainAdd.Name = "bTrainAdd";
            bTrainAdd.UseVisualStyleBackColor = true;
            bTrainAdd.Click += bTrainAdd_Click;
            // 
            // dgvTrains
            // 
            resources.ApplyResources(dgvTrains, "dgvTrains");
            dgvTrains.AllowUserToAddRows = false;
            dgvTrains.AllowUserToDeleteRows = false;
            dgvTrains.AllowUserToResizeRows = false;
            dgvTrains.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTrains.Columns.AddRange(new DataGridViewColumn[] { colTrain, colText, colFont });
            dgvTrains.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgvTrains.MultiSelect = false;
            dgvTrains.Name = "dgvTrains";
            dgvTrains.RowHeadersVisible = false;
            dgvTrains.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvTrains.CellValueChanged += dgvTrains_CellValueChanged;
            dgvTrains.CurrentCellChanged += dgvTrains_CurrentCellChanged;
            dgvTrains.CellDoubleClick += dgvTrains_CellDoubleClick;
            // 
            // flpTrainTools
            // 
            resources.ApplyResources(flpTrainTools, "flpTrainTools");
            flpTrainTools.Controls.Add(bTrainRemove);
            flpTrainTools.Controls.Add(bGenerate);
            flpTrainTools.Name = "flpTrainTools";
            // 
            // bTrainRemove
            // 
            resources.ApplyResources(bTrainRemove, "bTrainRemove");
            bTrainRemove.Name = "bTrainRemove";
            bTrainRemove.UseVisualStyleBackColor = true;
            bTrainRemove.Click += bTrainRemove_Click;
            // 
            // bGenerate
            // 
            resources.ApplyResources(bGenerate, "bGenerate");
            bGenerate.Name = "bGenerate";
            bGenerate.UseVisualStyleBackColor = true;
            bGenerate.Click += bGenerate_Click;
            // 
            // lFont
            // 
            resources.ApplyResources(lFont, "lFont");
            lFont.Name = "lFont";
            // 
            // cbFont
            // 
            resources.ApplyResources(cbFont, "cbFont");
            cbFont.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFont.FormattingEnabled = true;
            cbFont.Name = "cbFont";
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
            // colRealTable
            // 
            resources.ApplyResources(colRealTable, "colRealTable");
            colRealTable.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colRealTable.Name = "colRealTable";
            colRealTable.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colRealItem
            // 
            resources.ApplyResources(colRealItem, "colRealItem");
            colRealItem.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colRealItem.Name = "colRealItem";
            colRealItem.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colTrain
            // 
            resources.ApplyResources(colTrain, "colTrain");
            colTrain.Name = "colTrain";
            colTrain.ReadOnly = true;
            colTrain.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colText
            // 
            resources.ApplyResources(colText, "colText");
            colText.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colText.Name = "colText";
            colText.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colFont
            // 
            resources.ApplyResources(colFont, "colFont");
            colFont.Name = "colFont";
            colFont.ReadOnly = true;
            colFont.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // TableTextsPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "TableTextsPage";
            ((ISupportInitialize)splitMain).EndInit();
            ((ISupportInitialize)dgv).EndInit();
            ((ISupportInitialize)dgvReal).EndInit();
            ((ISupportInitialize)dgvTrains).EndInit();
            flpTrainTools.ResumeLayout(false);
            flpTrainTools.PerformLayout();
            flpTrains.ResumeLayout(false);
            flpTrains.PerformLayout();
            flpReal.ResumeLayout(false);
            flpReal.PerformLayout();
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
        private Label lRealHeader;
        private Label lRealNote;
        private FlowLayoutPanel flpReal;
        private ExButton bRealAdd;
        private ExButton bRealDelete;
        private DataGridView dgvReal;
        private Label lTrainsHeader;
        private FlowLayoutPanel flpTrains;
        private ExComboBox cbAddTrain;
        private ExButton bTrainAdd;
        private DataGridView dgvTrains;
        private FlowLayoutPanel flpTrainTools;
        private ExButton bTrainRemove;
        private ExButton bGenerate;
        private Label lFont;
        private ExComboBox cbFont;
        private Label lCommentHeader;
        private ExTextBox tbComment;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colKey;
        private DataGridViewExComboBoxColumn colRealTable;
        private DataGridViewExComboBoxColumn colRealItem;
        private DataGridViewTextBoxColumn colTrain;
        private DataGridViewTextBoxColumn colText;
        private DataGridViewTextBoxColumn colFont;
    }
}
