using ExControls;

namespace GVDEditor.Forms.Settings
{
    partial class TablesPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TablesPage));
            tlpMain = new TableLayoutPanel();
            lInfo = new Label();
            tlpTools = new TableLayoutPanel();
            flpButtons = new FlowLayoutPanel();
            bAdd = new ExButton();
            bDuplicate = new ExButton();
            bEdit = new ExButton();
            bDelete = new ExButton();
            lFilter = new Label();
            tbFilter = new ExTextBox();
            dgv = new DataGridView();
            lUseHeader = new Label();
            lUse = new Label();
            colName = new DataGridViewTextBoxColumn();
            colKey = new DataGridViewTextBoxColumn();
            colDetail = new DataGridViewTextBoxColumn();
            colComment = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)dgv).BeginInit();
            tlpMain.SuspendLayout();
            tlpTools.SuspendLayout();
            flpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lInfo, 0, 0);
            tlpMain.Controls.Add(tlpTools, 0, 1);
            tlpMain.Controls.Add(dgv, 0, 2);
            tlpMain.Controls.Add(lUseHeader, 0, 3);
            tlpMain.Controls.Add(lUse, 0, 4);
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
            flpButtons.Controls.Add(bEdit);
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
            // bEdit
            // 
            resources.ApplyResources(bEdit, "bEdit");
            bEdit.Name = "bEdit";
            bEdit.UseVisualStyleBackColor = true;
            bEdit.Click += bEdit_Click;
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
            tbFilter.TextChanged += tbFilter_TextChanged;
            // 
            // dgv
            // 
            resources.ApplyResources(dgv, "dgv");
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Columns.AddRange(new DataGridViewColumn[] { colName, colKey, colDetail, colComment });
            dgv.MultiSelect = false;
            dgv.Name = "dgv";
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.CellDoubleClick += dgv_CellDoubleClick;
            dgv.CurrentCellChanged += dgv_CurrentCellChanged;
            dgv.KeyDown += dgv_KeyDown;
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
            // colName
            // 
            resources.ApplyResources(colName, "colName");
            colName.Name = "colName";
            colName.ReadOnly = true;
            colName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colKey
            // 
            resources.ApplyResources(colKey, "colKey");
            colKey.Name = "colKey";
            colKey.ReadOnly = true;
            colKey.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colDetail
            // 
            resources.ApplyResources(colDetail, "colDetail");
            colDetail.Name = "colDetail";
            colDetail.ReadOnly = true;
            colDetail.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colComment
            // 
            resources.ApplyResources(colComment, "colComment");
            colComment.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colComment.Name = "colComment";
            colComment.ReadOnly = true;
            colComment.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // TablesPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "TablesPage";
            ((ISupportInitialize)dgv).EndInit();
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
        private ExButton bEdit;
        private ExButton bDelete;
        private Label lFilter;
        private ExTextBox tbFilter;
        private DataGridView dgv;
        private Label lUseHeader;
        private Label lUse;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colKey;
        private DataGridViewTextBoxColumn colDetail;
        private DataGridViewTextBoxColumn colComment;
    }
}
