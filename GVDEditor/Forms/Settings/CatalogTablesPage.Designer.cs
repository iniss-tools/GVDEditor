using ExControls;
using GVDEditor.Controls;

namespace GVDEditor.Forms.Settings
{
    partial class CatalogTablesPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(CatalogTablesPage));
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
            lManufacturer = new Label();
            cbManufacturer = new ExComboBox();
            lMaxRec = new Label();
            nudMaxRec = new ExNumericUpDown();
            lMinHeight = new Label();
            nudMinHeight = new ExNumericUpDown();
            lColumns = new Label();
            ruler = new CatalogRuler();
            flpColumns = new FlowLayoutPanel();
            bColAdd = new ExButton();
            bColDelete = new ExButton();
            bColUp = new ExButton();
            bColDown = new ExButton();
            dgvColumns = new DataGridView();
            lColumn = new Label();
            lColKey = new Label();
            tbColKey = new ExTextBox();
            lFill = new Label();
            cbFill = new ExComboBox();
            lAlign = new Label();
            cbAlign = new ExComboBox();
            lFont = new Label();
            cbFont = new ExComboBox();
            lDivType = new Label();
            cbDivType = new ExComboBox();
            lDivNote = new Label();
            lTab1 = new Label();
            cbTab1 = new ExComboBox();
            lTab2 = new Label();
            cbTab2 = new ExComboBox();
            lOrder = new Label();
            lOrderNote = new Label();
            flpOrder = new FlowLayoutPanel();
            bColumnOrder = new ExButton();
            lRows = new Label();
            lRowsNote = new Label();
            dgvRows = new DataGridView();
            flpRows = new FlowLayoutPanel();
            bRowsSetAll = new ExButton();
            lCommentHeader = new Label();
            tbComment = new ExTextBox();
            lUseHeader = new Label();
            lUse = new Label();
            colName = new DataGridViewTextBoxColumn();
            colKey = new DataGridViewTextBoxColumn();
            colColName = new DataGridViewTextBoxColumn();
            colColLine = new DataGridViewTextBoxColumn();
            colColStart = new DataGridViewTextBoxColumn();
            colColEnd = new DataGridViewTextBoxColumn();
            colRowNo = new DataGridViewTextBoxColumn();
            colRowHeight = new DataGridViewTextBoxColumn();
            colRowWidth = new DataGridViewTextBoxColumn();
            colRowSize = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)splitMain).BeginInit();
            ((ISupportInitialize)dgv).BeginInit();
            ((ISupportInitialize)nudMaxRec).BeginInit();
            ((ISupportInitialize)nudMinHeight).BeginInit();
            ((ISupportInitialize)dgvColumns).BeginInit();
            ((ISupportInitialize)dgvRows).BeginInit();
            tlpMain.SuspendLayout();
            tlpTools.SuspendLayout();
            flpButtons.SuspendLayout();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            tlpDetail.SuspendLayout();
            flpColumns.SuspendLayout();
            flpOrder.SuspendLayout();
            flpRows.SuspendLayout();
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
            tlpDetail.Controls.Add(lManufacturer, 0, 3);
            tlpDetail.Controls.Add(cbManufacturer, 1, 3);
            tlpDetail.Controls.Add(lMaxRec, 0, 4);
            tlpDetail.Controls.Add(nudMaxRec, 1, 4);
            tlpDetail.Controls.Add(lMinHeight, 0, 5);
            tlpDetail.Controls.Add(nudMinHeight, 1, 5);
            tlpDetail.Controls.Add(lColumns, 0, 6);
            tlpDetail.Controls.Add(ruler, 0, 7);
            tlpDetail.Controls.Add(flpColumns, 0, 8);
            tlpDetail.Controls.Add(dgvColumns, 0, 9);
            tlpDetail.Controls.Add(lColumn, 0, 10);
            tlpDetail.Controls.Add(lColKey, 0, 11);
            tlpDetail.Controls.Add(tbColKey, 1, 11);
            tlpDetail.Controls.Add(lFill, 0, 12);
            tlpDetail.Controls.Add(cbFill, 1, 12);
            tlpDetail.Controls.Add(lAlign, 0, 13);
            tlpDetail.Controls.Add(cbAlign, 1, 13);
            tlpDetail.Controls.Add(lFont, 0, 14);
            tlpDetail.Controls.Add(cbFont, 1, 14);
            tlpDetail.Controls.Add(lDivType, 0, 15);
            tlpDetail.Controls.Add(cbDivType, 1, 15);
            tlpDetail.Controls.Add(lDivNote, 1, 16);
            tlpDetail.Controls.Add(lTab1, 0, 17);
            tlpDetail.Controls.Add(cbTab1, 1, 17);
            tlpDetail.Controls.Add(lTab2, 0, 18);
            tlpDetail.Controls.Add(cbTab2, 1, 18);
            tlpDetail.Controls.Add(lOrder, 0, 19);
            tlpDetail.Controls.Add(lOrderNote, 0, 20);
            tlpDetail.Controls.Add(flpOrder, 0, 21);
            tlpDetail.Controls.Add(lRows, 0, 22);
            tlpDetail.Controls.Add(lRowsNote, 0, 23);
            tlpDetail.Controls.Add(dgvRows, 0, 24);
            tlpDetail.Controls.Add(flpRows, 0, 25);
            tlpDetail.Controls.Add(lCommentHeader, 0, 26);
            tlpDetail.Controls.Add(tbComment, 0, 27);
            tlpDetail.Controls.Add(lUseHeader, 0, 28);
            tlpDetail.Controls.Add(lUse, 0, 29);
            tlpDetail.Name = "tlpDetail";
            tlpDetail.SetColumnSpan(lBasic, 2);
            tlpDetail.SetColumnSpan(lColumns, 2);
            tlpDetail.SetColumnSpan(ruler, 2);
            tlpDetail.SetColumnSpan(flpColumns, 2);
            tlpDetail.SetColumnSpan(dgvColumns, 2);
            tlpDetail.SetColumnSpan(lColumn, 2);
            tlpDetail.SetColumnSpan(lOrder, 2);
            tlpDetail.SetColumnSpan(lOrderNote, 2);
            tlpDetail.SetColumnSpan(flpOrder, 2);
            tlpDetail.SetColumnSpan(lRows, 2);
            tlpDetail.SetColumnSpan(lRowsNote, 2);
            tlpDetail.SetColumnSpan(dgvRows, 2);
            tlpDetail.SetColumnSpan(flpRows, 2);
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
            // lManufacturer
            // 
            resources.ApplyResources(lManufacturer, "lManufacturer");
            lManufacturer.Name = "lManufacturer";
            // 
            // cbManufacturer
            // 
            resources.ApplyResources(cbManufacturer, "cbManufacturer");
            cbManufacturer.DropDownStyle = ComboBoxStyle.DropDownList;
            cbManufacturer.FormattingEnabled = true;
            cbManufacturer.Name = "cbManufacturer";
            cbManufacturer.SelectionChangeCommitted += Field_Changed;
            // 
            // lMaxRec
            // 
            resources.ApplyResources(lMaxRec, "lMaxRec");
            lMaxRec.Name = "lMaxRec";
            // 
            // nudMaxRec
            // 
            resources.ApplyResources(nudMaxRec, "nudMaxRec");
            nudMaxRec.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            nudMaxRec.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            nudMaxRec.Name = "nudMaxRec";
            nudMaxRec.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudMaxRec.ValueChanged += Field_Changed;
            // 
            // lMinHeight
            // 
            resources.ApplyResources(lMinHeight, "lMinHeight");
            lMinHeight.Name = "lMinHeight";
            // 
            // nudMinHeight
            // 
            resources.ApplyResources(nudMinHeight, "nudMinHeight");
            nudMinHeight.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudMinHeight.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            nudMinHeight.Name = "nudMinHeight";
            nudMinHeight.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudMinHeight.ValueChanged += Field_Changed;
            // 
            // lColumns
            // 
            resources.ApplyResources(lColumns, "lColumns");
            lColumns.Name = "lColumns";
            // 
            // ruler
            // 
            resources.ApplyResources(ruler, "ruler");
            ruler.Name = "ruler";
            ruler.ColumnClicked += ruler_ColumnClicked;
            // 
            // flpColumns
            // 
            resources.ApplyResources(flpColumns, "flpColumns");
            flpColumns.Controls.Add(bColAdd);
            flpColumns.Controls.Add(bColDelete);
            flpColumns.Controls.Add(bColUp);
            flpColumns.Controls.Add(bColDown);
            flpColumns.Name = "flpColumns";
            // 
            // bColAdd
            // 
            resources.ApplyResources(bColAdd, "bColAdd");
            bColAdd.Name = "bColAdd";
            bColAdd.UseVisualStyleBackColor = true;
            bColAdd.Click += bColAdd_Click;
            // 
            // bColDelete
            // 
            resources.ApplyResources(bColDelete, "bColDelete");
            bColDelete.Name = "bColDelete";
            bColDelete.UseVisualStyleBackColor = true;
            bColDelete.Click += bColDelete_Click;
            // 
            // bColUp
            // 
            resources.ApplyResources(bColUp, "bColUp");
            bColUp.Name = "bColUp";
            bColUp.UseVisualStyleBackColor = true;
            bColUp.Click += bColUp_Click;
            // 
            // bColDown
            // 
            resources.ApplyResources(bColDown, "bColDown");
            bColDown.Name = "bColDown";
            bColDown.UseVisualStyleBackColor = true;
            bColDown.Click += bColDown_Click;
            // 
            // dgvColumns
            // 
            resources.ApplyResources(dgvColumns, "dgvColumns");
            dgvColumns.AllowUserToAddRows = false;
            dgvColumns.AllowUserToDeleteRows = false;
            dgvColumns.AllowUserToResizeRows = false;
            dgvColumns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvColumns.Columns.AddRange(new DataGridViewColumn[] { colColName, colColLine, colColStart, colColEnd });
            dgvColumns.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgvColumns.MultiSelect = false;
            dgvColumns.Name = "dgvColumns";
            dgvColumns.RowHeadersVisible = false;
            dgvColumns.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvColumns.CellValueChanged += dgvColumns_CellValueChanged;
            dgvColumns.CurrentCellChanged += dgvColumns_CurrentCellChanged;
            dgvColumns.CellDoubleClick += dgvColumns_CellDoubleClick;
            dgvColumns.DataError += dgvColumns_DataError;
            // 
            // lColumn
            // 
            resources.ApplyResources(lColumn, "lColumn");
            lColumn.Name = "lColumn";
            // 
            // lColKey
            // 
            resources.ApplyResources(lColKey, "lColKey");
            lColKey.Name = "lColKey";
            // 
            // tbColKey
            // 
            resources.ApplyResources(tbColKey, "tbColKey");
            tbColKey.Name = "tbColKey";
            tbColKey.Validated += tbColKey_Validated;
            // 
            // lFill
            // 
            resources.ApplyResources(lFill, "lFill");
            lFill.Name = "lFill";
            // 
            // cbFill
            // 
            resources.ApplyResources(cbFill, "cbFill");
            cbFill.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFill.FormattingEnabled = true;
            cbFill.Name = "cbFill";
            cbFill.SelectionChangeCommitted += Column_Changed;
            // 
            // lAlign
            // 
            resources.ApplyResources(lAlign, "lAlign");
            lAlign.Name = "lAlign";
            // 
            // cbAlign
            // 
            resources.ApplyResources(cbAlign, "cbAlign");
            cbAlign.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAlign.FormattingEnabled = true;
            cbAlign.Name = "cbAlign";
            cbAlign.SelectionChangeCommitted += Column_Changed;
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
            // lDivType
            // 
            resources.ApplyResources(lDivType, "lDivType");
            lDivType.Name = "lDivType";
            // 
            // cbDivType
            // 
            resources.ApplyResources(cbDivType, "cbDivType");
            cbDivType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDivType.FormattingEnabled = true;
            cbDivType.Name = "cbDivType";
            cbDivType.SelectionChangeCommitted += Column_Changed;
            // 
            // lDivNote
            // 
            resources.ApplyResources(lDivNote, "lDivNote");
            lDivNote.Name = "lDivNote";
            // 
            // lTab1
            // 
            resources.ApplyResources(lTab1, "lTab1");
            lTab1.Name = "lTab1";
            // 
            // cbTab1
            // 
            resources.ApplyResources(cbTab1, "cbTab1");
            cbTab1.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTab1.FormattingEnabled = true;
            cbTab1.Name = "cbTab1";
            cbTab1.SelectionChangeCommitted += Column_Changed;
            // 
            // lTab2
            // 
            resources.ApplyResources(lTab2, "lTab2");
            lTab2.Name = "lTab2";
            // 
            // cbTab2
            // 
            resources.ApplyResources(cbTab2, "cbTab2");
            cbTab2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTab2.FormattingEnabled = true;
            cbTab2.Name = "cbTab2";
            cbTab2.SelectionChangeCommitted += Column_Changed;
            // 
            // lOrder
            // 
            resources.ApplyResources(lOrder, "lOrder");
            lOrder.Name = "lOrder";
            // 
            // lOrderNote
            // 
            resources.ApplyResources(lOrderNote, "lOrderNote");
            lOrderNote.Name = "lOrderNote";
            // 
            // flpOrder
            // 
            resources.ApplyResources(flpOrder, "flpOrder");
            flpOrder.Controls.Add(bColumnOrder);
            flpOrder.Name = "flpOrder";
            // 
            // bColumnOrder
            // 
            resources.ApplyResources(bColumnOrder, "bColumnOrder");
            bColumnOrder.Name = "bColumnOrder";
            bColumnOrder.UseVisualStyleBackColor = true;
            bColumnOrder.Click += bColumnOrder_Click;
            // 
            // lRows
            // 
            resources.ApplyResources(lRows, "lRows");
            lRows.Name = "lRows";
            // 
            // lRowsNote
            // 
            resources.ApplyResources(lRowsNote, "lRowsNote");
            lRowsNote.Name = "lRowsNote";
            // 
            // dgvRows
            // 
            resources.ApplyResources(dgvRows, "dgvRows");
            dgvRows.AllowUserToAddRows = false;
            dgvRows.AllowUserToDeleteRows = false;
            dgvRows.AllowUserToResizeRows = false;
            dgvRows.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRows.Columns.AddRange(new DataGridViewColumn[] { colRowNo, colRowHeight, colRowWidth, colRowSize });
            dgvRows.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgvRows.MultiSelect = false;
            dgvRows.Name = "dgvRows";
            dgvRows.RowHeadersVisible = false;
            dgvRows.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvRows.CellValueChanged += dgvRows_CellValueChanged;
            dgvRows.CellDoubleClick += dgvRows_CellDoubleClick;
            dgvRows.DataError += dgvColumns_DataError;
            // 
            // flpRows
            // 
            resources.ApplyResources(flpRows, "flpRows");
            flpRows.Controls.Add(bRowsSetAll);
            flpRows.Name = "flpRows";
            // 
            // bRowsSetAll
            // 
            resources.ApplyResources(bRowsSetAll, "bRowsSetAll");
            bRowsSetAll.Name = "bRowsSetAll";
            bRowsSetAll.UseVisualStyleBackColor = true;
            bRowsSetAll.Click += bRowsSetAll_Click;
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
            // colColName
            // 
            resources.ApplyResources(colColName, "colColName");
            colColName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colColName.Name = "colColName";
            colColName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colColLine
            // 
            resources.ApplyResources(colColLine, "colColLine");
            colColLine.Name = "colColLine";
            colColLine.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colColStart
            // 
            resources.ApplyResources(colColStart, "colColStart");
            colColStart.Name = "colColStart";
            colColStart.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colColEnd
            // 
            resources.ApplyResources(colColEnd, "colColEnd");
            colColEnd.Name = "colColEnd";
            colColEnd.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colRowNo
            // 
            resources.ApplyResources(colRowNo, "colRowNo");
            colRowNo.Name = "colRowNo";
            colRowNo.ReadOnly = true;
            colRowNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colRowHeight
            // 
            resources.ApplyResources(colRowHeight, "colRowHeight");
            colRowHeight.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colRowHeight.Name = "colRowHeight";
            colRowHeight.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colRowWidth
            // 
            resources.ApplyResources(colRowWidth, "colRowWidth");
            colRowWidth.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colRowWidth.Name = "colRowWidth";
            colRowWidth.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colRowSize
            // 
            resources.ApplyResources(colRowSize, "colRowSize");
            colRowSize.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colRowSize.Name = "colRowSize";
            colRowSize.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // CatalogTablesPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "CatalogTablesPage";
            ((ISupportInitialize)splitMain).EndInit();
            ((ISupportInitialize)dgv).EndInit();
            ((ISupportInitialize)nudMaxRec).EndInit();
            ((ISupportInitialize)nudMinHeight).EndInit();
            ((ISupportInitialize)dgvColumns).EndInit();
            ((ISupportInitialize)dgvRows).EndInit();
            flpRows.ResumeLayout(false);
            flpRows.PerformLayout();
            flpOrder.ResumeLayout(false);
            flpOrder.PerformLayout();
            flpColumns.ResumeLayout(false);
            flpColumns.PerformLayout();
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
        private Label lManufacturer;
        private ExComboBox cbManufacturer;
        private Label lMaxRec;
        private ExNumericUpDown nudMaxRec;
        private Label lMinHeight;
        private ExNumericUpDown nudMinHeight;
        private Label lColumns;
        private CatalogRuler ruler;
        private FlowLayoutPanel flpColumns;
        private ExButton bColAdd;
        private ExButton bColDelete;
        private ExButton bColUp;
        private ExButton bColDown;
        private DataGridView dgvColumns;
        private Label lColumn;
        private Label lColKey;
        private ExTextBox tbColKey;
        private Label lFill;
        private ExComboBox cbFill;
        private Label lAlign;
        private ExComboBox cbAlign;
        private Label lFont;
        private ExComboBox cbFont;
        private Label lDivType;
        private ExComboBox cbDivType;
        private Label lDivNote;
        private Label lTab1;
        private ExComboBox cbTab1;
        private Label lTab2;
        private ExComboBox cbTab2;
        private Label lOrder;
        private Label lOrderNote;
        private FlowLayoutPanel flpOrder;
        private ExButton bColumnOrder;
        private Label lRows;
        private Label lRowsNote;
        private DataGridView dgvRows;
        private FlowLayoutPanel flpRows;
        private ExButton bRowsSetAll;
        private Label lCommentHeader;
        private ExTextBox tbComment;
        private Label lUseHeader;
        private Label lUse;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colKey;
        private DataGridViewTextBoxColumn colColName;
        private DataGridViewTextBoxColumn colColLine;
        private DataGridViewTextBoxColumn colColStart;
        private DataGridViewTextBoxColumn colColEnd;
        private DataGridViewTextBoxColumn colRowNo;
        private DataGridViewTextBoxColumn colRowHeight;
        private DataGridViewTextBoxColumn colRowWidth;
        private DataGridViewTextBoxColumn colRowSize;
    }
}
