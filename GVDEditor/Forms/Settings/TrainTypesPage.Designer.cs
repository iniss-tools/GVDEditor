using ExControls;

namespace GVDEditor.Forms.Settings
{
    partial class TrainTypesPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TrainTypesPage));
            tlpMain = new TableLayoutPanel();
            lInfo = new Label();
            tlpTools = new TableLayoutPanel();
            flpButtons = new FlowLayoutPanel();
            bAdd = new ExButton();
            bDelete = new ExButton();
            lFilter = new Label();
            tbFilter = new ExTextBox();
            dgv = new DataGridView();
            lHint = new Label();
            tlpBottom = new TableLayoutPanel();
            lDetail = new Label();
            pSlots = new Panel();
            colKey = new DataGridViewTextBoxColumn();
            colText = new DataGridViewTextBoxColumn();
            colKind = new DataGridViewExComboBoxColumn();
            colCategory = new DataGridViewTextBoxColumn();
            colLook = new DataGridViewTextBoxColumn();
            colTrains = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)dgv).BeginInit();
            tlpMain.SuspendLayout();
            tlpTools.SuspendLayout();
            flpButtons.SuspendLayout();
            tlpBottom.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lInfo, 0, 0);
            tlpMain.Controls.Add(tlpTools, 0, 1);
            tlpMain.Controls.Add(dgv, 0, 2);
            tlpMain.Controls.Add(lHint, 0, 3);
            tlpMain.Controls.Add(tlpBottom, 0, 4);
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
            dgv.Columns.AddRange(new DataGridViewColumn[] { colKey, colText, colKind, colCategory, colLook, colTrains });
            dgv.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgv.MultiSelect = false;
            dgv.Name = "dgv";
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv.CellDoubleClick += dgv_CellDoubleClick;
            dgv.CellValueChanged += dgv_CellValueChanged;
            dgv.CurrentCellChanged += dgv_CurrentCellChanged;
            dgv.KeyDown += dgv_KeyDown;
            dgv.CurrentCellDirtyStateChanged += dgv_CurrentCellDirtyStateChanged;
            dgv.DataError += dgv_DataError;
            // 
            // lHint
            // 
            resources.ApplyResources(lHint, "lHint");
            lHint.Name = "lHint";
            // 
            // tlpBottom
            // 
            resources.ApplyResources(tlpBottom, "tlpBottom");
            tlpBottom.Controls.Add(lDetail, 0, 0);
            tlpBottom.Controls.Add(pSlots, 1, 0);
            tlpBottom.Name = "tlpBottom";
            // 
            // lDetail
            // 
            resources.ApplyResources(lDetail, "lDetail");
            lDetail.Name = "lDetail";
            // 
            // pSlots
            // 
            resources.ApplyResources(pSlots, "pSlots");
            pSlots.Name = "pSlots";
            pSlots.Paint += pSlots_Paint;
            // 
            // colKey
            // 
            resources.ApplyResources(colKey, "colKey");
            colKey.Name = "colKey";
            colKey.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colText
            // 
            resources.ApplyResources(colText, "colText");
            colText.Name = "colText";
            colText.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colKind
            // 
            resources.ApplyResources(colKind, "colKind");
            colKind.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colKind.Name = "colKind";
            colKind.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colCategory
            // 
            resources.ApplyResources(colCategory, "colCategory");
            colCategory.Name = "colCategory";
            colCategory.ReadOnly = true;
            colCategory.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colLook
            // 
            resources.ApplyResources(colLook, "colLook");
            colLook.Name = "colLook";
            colLook.ReadOnly = true;
            colLook.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colTrains
            // 
            resources.ApplyResources(colTrains, "colTrains");
            colTrains.Name = "colTrains";
            colTrains.ReadOnly = true;
            colTrains.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // TrainTypesPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "TrainTypesPage";
            ((ISupportInitialize)dgv).EndInit();
            tlpBottom.ResumeLayout(false);
            tlpBottom.PerformLayout();
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
        private ExButton bDelete;
        private Label lFilter;
        private ExTextBox tbFilter;
        private DataGridView dgv;
        private Label lHint;
        private TableLayoutPanel tlpBottom;
        private Label lDetail;
        private Panel pSlots;
        private DataGridViewTextBoxColumn colKey;
        private DataGridViewTextBoxColumn colText;
        private DataGridViewExComboBoxColumn colKind;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewTextBoxColumn colLook;
        private DataGridViewTextBoxColumn colTrains;
    }
}
