using ExControls;

namespace GVDEditor.UI.Settings
{
    partial class GrafikonyPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(GrafikonyPage));
            tlpMain = new TableLayoutPanel();
            lInfo = new Label();
            flpButtons = new FlowLayoutPanel();
            bColor = new ExButton();
            bNoColor = new ExButton();
            bOpenDir = new ExButton();
            bDelete = new ExButton();
            dgv = new DataGridView();
            lHint = new Label();
            colStation = new DataGridViewTextBoxColumn();
            colPeriod = new DataGridViewTextBoxColumn();
            colTablePort = new DataGridViewTextBoxColumn();
            colReportPort = new DataGridViewTextBoxColumn();
            colColor = new DataGridViewTextBoxColumn();
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
            flpButtons.Controls.Add(bColor);
            flpButtons.Controls.Add(bNoColor);
            flpButtons.Controls.Add(bOpenDir);
            flpButtons.Controls.Add(bDelete);
            flpButtons.Name = "flpButtons";
            flpButtons.WrapContents = false;
            // 
            // bColor
            // 
            resources.ApplyResources(bColor, "bColor");
            bColor.Name = "bColor";
            bColor.UseVisualStyleBackColor = true;
            bColor.Click += bColor_Click;
            // 
            // bNoColor
            // 
            resources.ApplyResources(bNoColor, "bNoColor");
            bNoColor.Name = "bNoColor";
            bNoColor.UseVisualStyleBackColor = true;
            bNoColor.Click += bNoColor_Click;
            // 
            // bOpenDir
            // 
            resources.ApplyResources(bOpenDir, "bOpenDir");
            bOpenDir.Name = "bOpenDir";
            bOpenDir.UseVisualStyleBackColor = true;
            bOpenDir.Click += bOpenDir_Click;
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
            dgv.Columns.AddRange(new DataGridViewColumn[] { colStation, colPeriod, colTablePort, colReportPort, colColor });
            dgv.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgv.MultiSelect = false;
            dgv.Name = "dgv";
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv.CellDoubleClick += dgv_CellDoubleClick;
            dgv.CellValueChanged += dgv_CellValueChanged;
            dgv.CurrentCellChanged += dgv_CurrentCellChanged;
            dgv.KeyDown += dgv_KeyDown;
            dgv.CellPainting += dgv_CellPainting;
            // 
            // lHint
            // 
            resources.ApplyResources(lHint, "lHint");
            lHint.Name = "lHint";
            // 
            // colStation
            // 
            resources.ApplyResources(colStation, "colStation");
            colStation.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colStation.Name = "colStation";
            colStation.ReadOnly = true;
            colStation.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colPeriod
            // 
            resources.ApplyResources(colPeriod, "colPeriod");
            colPeriod.Name = "colPeriod";
            colPeriod.ReadOnly = true;
            colPeriod.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colTablePort
            // 
            resources.ApplyResources(colTablePort, "colTablePort");
            colTablePort.Name = "colTablePort";
            colTablePort.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colReportPort
            // 
            resources.ApplyResources(colReportPort, "colReportPort");
            colReportPort.Name = "colReportPort";
            colReportPort.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colColor
            // 
            resources.ApplyResources(colColor, "colColor");
            colColor.Name = "colColor";
            colColor.ReadOnly = true;
            colColor.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // GrafikonyPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "GrafikonyPage";
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
        private ExButton bColor;
        private ExButton bNoColor;
        private ExButton bOpenDir;
        private ExButton bDelete;
        private DataGridView dgv;
        private Label lHint;
        private DataGridViewTextBoxColumn colStation;
        private DataGridViewTextBoxColumn colPeriod;
        private DataGridViewTextBoxColumn colTablePort;
        private DataGridViewTextBoxColumn colReportPort;
        private DataGridViewTextBoxColumn colColor;
    }
}
