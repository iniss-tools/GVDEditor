using ExControls;

namespace GVDEditor.UI.Settings
{
    partial class OperatorsPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(OperatorsPage));
            tlpMain = new TableLayoutPanel();
            lInfo = new Label();
            flpButtons = new FlowLayoutPanel();
            bAdd = new ExButton();
            bDelete = new ExButton();
            dgv = new DataGridView();
            lHint = new Label();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colTrains = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)dgv).BeginInit();
            tlpMain.SuspendLayout();
            flpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lInfo, 0, 0);
            tlpMain.Controls.Add(flpButtons, 0, 1);
            tlpMain.Controls.Add(dgv, 0, 2);
            tlpMain.Controls.Add(lHint, 0, 3);
            tlpMain.Name = "tlpMain";
            // 
            // lInfo
            // 
            resources.ApplyResources(lInfo, "lInfo");
            lInfo.Name = "lInfo";
            // 
            // flpButtons
            // 
            resources.ApplyResources(flpButtons, "flpButtons");
            flpButtons.Controls.Add(bAdd);
            flpButtons.Controls.Add(bDelete);
            flpButtons.Name = "flpButtons";
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
            // dgv
            // 
            resources.ApplyResources(dgv, "dgv");
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colTrains });
            dgv.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgv.MultiSelect = false;
            dgv.Name = "dgv";
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv.CellDoubleClick += dgv_CellDoubleClick;
            dgv.CellValueChanged += dgv_CellValueChanged;
            dgv.CurrentCellChanged += dgv_CurrentCellChanged;
            dgv.KeyDown += dgv_KeyDown;
            // 
            // lHint
            // 
            resources.ApplyResources(lHint, "lHint");
            lHint.Name = "lHint";
            // 
            // colId
            // 
            resources.ApplyResources(colId, "colId");
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colName
            // 
            resources.ApplyResources(colName, "colName");
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colName.Name = "colName";
            colName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colTrains
            // 
            resources.ApplyResources(colTrains, "colTrains");
            colTrains.Name = "colTrains";
            colTrains.ReadOnly = true;
            colTrains.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // OperatorsPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "OperatorsPage";
            ((ISupportInitialize)dgv).EndInit();
            flpButtons.ResumeLayout(false);
            flpButtons.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lInfo;
        private FlowLayoutPanel flpButtons;
        private ExButton bAdd;
        private ExButton bDelete;
        private DataGridView dgv;
        private Label lHint;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colTrains;
    }
}
