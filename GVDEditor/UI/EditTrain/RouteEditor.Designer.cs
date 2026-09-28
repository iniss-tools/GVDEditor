using ExControls;

namespace GVDEditor.UI.EditTrain
{
    partial class RouteEditor
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(RouteEditor));
            tlpMain = new TableLayoutPanel();
            dgv = new DataGridView();
            flpButtons = new FlowLayoutPanel();
            bUp = new ExButton();
            bDown = new ExButton();
            bRemove = new ExButton();
            bClear = new ExButton();
            colLong = new DataGridViewExCheckBoxColumn();
            colShort = new DataGridViewExCheckBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)dgv).BeginInit();
            tlpMain.SuspendLayout();
            flpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(dgv, 0, 1);
            tlpMain.Controls.Add(flpButtons, 0, 0);
            tlpMain.Name = "tlpMain";
            // 
            // dgv
            // 
            resources.ApplyResources(dgv, "dgv");
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Columns.AddRange(new DataGridViewColumn[] { colLong, colShort, colName });
            dgv.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgv.MultiSelect = false;
            dgv.Name = "dgv";
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.CurrentCellDirtyStateChanged += dgv_CurrentCellDirtyStateChanged;
            dgv.SelectionChanged += dgv_SelectionChanged;
            dgv.KeyDown += dgv_KeyDown;
            // 
            // flpButtons
            // 
            resources.ApplyResources(flpButtons, "flpButtons");
            flpButtons.Controls.Add(bUp);
            flpButtons.Controls.Add(bDown);
            flpButtons.Controls.Add(bRemove);
            flpButtons.Controls.Add(bClear);
            flpButtons.Name = "flpButtons";
            flpButtons.WrapContents = false;
            // 
            // bUp
            // 
            resources.ApplyResources(bUp, "bUp");
            bUp.Name = "bUp";
            bUp.UseVisualStyleBackColor = true;
            bUp.Click += bUp_Click;
            // 
            // bDown
            // 
            resources.ApplyResources(bDown, "bDown");
            bDown.Name = "bDown";
            bDown.UseVisualStyleBackColor = true;
            bDown.Click += bDown_Click;
            // 
            // bRemove
            // 
            resources.ApplyResources(bRemove, "bRemove");
            bRemove.Name = "bRemove";
            bRemove.UseVisualStyleBackColor = true;
            bRemove.Click += bRemove_Click;
            // 
            // bClear
            // 
            resources.ApplyResources(bClear, "bClear");
            bClear.Name = "bClear";
            bClear.UseVisualStyleBackColor = true;
            bClear.Click += bClear_Click;
            // 
            // colLong
            // 
            resources.ApplyResources(colLong, "colLong");
            colLong.DataPropertyName = "IsInLongReport";
            colLong.Name = "colLong";
            // 
            // colShort
            // 
            resources.ApplyResources(colShort, "colShort");
            colShort.DataPropertyName = "IsInShortReport";
            colShort.Name = "colShort";
            // 
            // colName
            // 
            resources.ApplyResources(colName, "colName");
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colName.DataPropertyName = "Name";
            colName.Name = "colName";
            colName.ReadOnly = true;
            colName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // RouteEditor
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "RouteEditor";
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
        private DataGridView dgv;
        private FlowLayoutPanel flpButtons;
        private ExButton bUp;
        private ExButton bDown;
        private ExButton bRemove;
        private ExButton bClear;
        private DataGridViewExCheckBoxColumn colLong;
        private DataGridViewExCheckBoxColumn colShort;
        private DataGridViewTextBoxColumn colName;
    }
}
