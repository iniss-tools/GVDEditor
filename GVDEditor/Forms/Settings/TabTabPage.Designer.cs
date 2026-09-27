using ExControls;

namespace GVDEditor.Forms.Settings
{
    partial class TabTabPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TabTabPage));
            tlpMain = new TableLayoutPanel();
            lInfo = new Label();
            tlpTools = new TableLayoutPanel();
            flpButtons = new FlowLayoutPanel();
            bEditor = new ExButton();
            bDelete = new ExButton();
            lFilter = new Label();
            tbFilter = new ExTextBox();
            splitMain = new SplitContainer();
            dgv = new DataGridView();
            tlpDetail = new TableLayoutPanel();
            lHint = new Label();
            lTextHeader = new Label();
            tbText = new ExTextBox();
            lUseHeader = new Label();
            lUse = new Label();
            colName = new DataGridViewTextBoxColumn();
            colRows = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)splitMain).BeginInit();
            ((ISupportInitialize)dgv).BeginInit();
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
            flpButtons.Controls.Add(bEditor);
            flpButtons.Controls.Add(bDelete);
            flpButtons.Name = "flpButtons";
            flpButtons.WrapContents = false;
            // 
            // bEditor
            // 
            resources.ApplyResources(bEditor, "bEditor");
            bEditor.Name = "bEditor";
            bEditor.UseVisualStyleBackColor = true;
            bEditor.Click += bEditor_Click;
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
            splitMain.Panel2.Controls.Add(tlpDetail);
            // 
            // dgv
            // 
            resources.ApplyResources(dgv, "dgv");
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Columns.AddRange(new DataGridViewColumn[] { colName, colRows });
            dgv.MultiSelect = false;
            dgv.Name = "dgv";
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.KeyDown += dgv_KeyDown;
            dgv.CellDoubleClick += dgv_CellDoubleClick;
            // 
            // tlpDetail
            // 
            resources.ApplyResources(tlpDetail, "tlpDetail");
            tlpDetail.Controls.Add(lTextHeader, 0, 0);
            tlpDetail.Controls.Add(tbText, 0, 1);
            tlpDetail.Controls.Add(lUseHeader, 0, 2);
            tlpDetail.Controls.Add(lUse, 0, 3);
            tlpDetail.Name = "tlpDetail";
            // 
            // lTextHeader
            // 
            resources.ApplyResources(lTextHeader, "lTextHeader");
            lTextHeader.Name = "lTextHeader";
            // 
            // tbText
            // 
            resources.ApplyResources(tbText, "tbText");
            tbText.Multiline = true;
            tbText.Name = "tbText";
            tbText.ReadOnly = true;
            tbText.WordWrap = false;
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
            // colRows
            // 
            resources.ApplyResources(colRows, "colRows");
            colRows.Name = "colRows";
            colRows.ReadOnly = true;
            colRows.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // TabTabPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "TabTabPage";
            ((ISupportInitialize)splitMain).EndInit();
            ((ISupportInitialize)dgv).EndInit();
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
        private ExButton bEditor;
        private ExButton bDelete;
        private Label lFilter;
        private ExTextBox tbFilter;
        private SplitContainer splitMain;
        private DataGridView dgv;
        private TableLayoutPanel tlpDetail;
        private Label lHint;
        private Label lTextHeader;
        private ExTextBox tbText;
        private Label lUseHeader;
        private Label lUse;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colRows;
    }
}
