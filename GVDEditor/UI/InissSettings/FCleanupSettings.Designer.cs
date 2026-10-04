using ExControls;

namespace GVDEditor.UI.InissSettings
{
    partial class FCleanupSettings
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
            var resources = new ComponentResourceManager(typeof(FCleanupSettings));
            tlpMain = new TableLayoutPanel();
            lIntro = new Label();
            dgvItems = new DataGridView();
            cDo = new DataGridViewCheckBoxColumn();
            cKey = new DataGridViewTextBoxColumn();
            cWhere = new DataGridViewTextBoxColumn();
            cValue = new DataGridViewTextBoxColumn();
            cWhy = new DataGridViewTextBoxColumn();
            cAction = new DataGridViewTextBoxColumn();
            lBackup = new Label();
            flpButtons = new FlowLayoutPanel();
            bCancel = new ExButton();
            bClean = new ExButton();
            ((ISupportInitialize)dgvItems).BeginInit();
            tlpMain.SuspendLayout();
            flpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lIntro, 0, 0);
            tlpMain.Controls.Add(dgvItems, 0, 1);
            tlpMain.Controls.Add(lBackup, 0, 2);
            tlpMain.Controls.Add(flpButtons, 0, 3);
            tlpMain.Name = "tlpMain";
            // 
            // lIntro
            // 
            resources.ApplyResources(lIntro, "lIntro");
            lIntro.Name = "lIntro";
            // 
            // dgvItems
            // 
            resources.ApplyResources(dgvItems, "dgvItems");
            dgvItems.Columns.AddRange(new DataGridViewColumn[] { cDo, cKey, cWhere, cValue, cWhy, cAction });
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.AllowUserToResizeRows = false;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.RowHeadersVisible = false;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.Name = "dgvItems";
            // 
            // cDo
            // 
            resources.ApplyResources(cDo, "cDo");
            cDo.SortMode = DataGridViewColumnSortMode.NotSortable;
            cDo.Name = "cDo";
            // 
            // cKey
            // 
            resources.ApplyResources(cKey, "cKey");
            cKey.ReadOnly = true;
            cKey.SortMode = DataGridViewColumnSortMode.NotSortable;
            cKey.Name = "cKey";
            // 
            // cWhere
            // 
            resources.ApplyResources(cWhere, "cWhere");
            cWhere.ReadOnly = true;
            cWhere.SortMode = DataGridViewColumnSortMode.NotSortable;
            cWhere.Name = "cWhere";
            // 
            // cValue
            // 
            resources.ApplyResources(cValue, "cValue");
            cValue.ReadOnly = true;
            cValue.SortMode = DataGridViewColumnSortMode.NotSortable;
            cValue.Name = "cValue";
            // 
            // cWhy
            // 
            resources.ApplyResources(cWhy, "cWhy");
            cWhy.ReadOnly = true;
            cWhy.SortMode = DataGridViewColumnSortMode.NotSortable;
            cWhy.Name = "cWhy";
            // 
            // cAction
            // 
            resources.ApplyResources(cAction, "cAction");
            cAction.ReadOnly = true;
            cAction.SortMode = DataGridViewColumnSortMode.NotSortable;
            cAction.Name = "cAction";
            // 
            // lBackup
            // 
            resources.ApplyResources(lBackup, "lBackup");
            lBackup.Name = "lBackup";
            // 
            // flpButtons
            // 
            resources.ApplyResources(flpButtons, "flpButtons");
            flpButtons.Controls.Add(bCancel);
            flpButtons.Controls.Add(bClean);
            flpButtons.Name = "flpButtons";
            // 
            // bCancel
            // 
            resources.ApplyResources(bCancel, "bCancel");
            bCancel.UseVisualStyleBackColor = true;
            bCancel.DialogResult = DialogResult.Cancel;
            bCancel.Name = "bCancel";
            // 
            // bClean
            // 
            resources.ApplyResources(bClean, "bClean");
            bClean.UseVisualStyleBackColor = true;
            bClean.Name = "bClean";
            // 
            // FCleanupSettings
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            CancelButton = bCancel;
            Name = "FCleanupSettings";
            flpButtons.ResumeLayout(false);
            flpButtons.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ((ISupportInitialize)dgvItems).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lIntro;
        private DataGridView dgvItems;
        private DataGridViewCheckBoxColumn cDo;
        private DataGridViewTextBoxColumn cKey;
        private DataGridViewTextBoxColumn cWhere;
        private DataGridViewTextBoxColumn cValue;
        private DataGridViewTextBoxColumn cWhy;
        private DataGridViewTextBoxColumn cAction;
        private Label lBackup;
        private FlowLayoutPanel flpButtons;
        private ExButton bCancel;
        private ExButton bClean;
    }
}
