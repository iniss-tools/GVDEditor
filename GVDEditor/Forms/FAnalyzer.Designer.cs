
namespace GVDEditor.Forms
{
    partial class FAnalyzer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAnalyzer));
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bAnalyze = new ExControls.ExButton();
            this.pbStatus = new System.Windows.Forms.ProgressBar();
            this.lStatus = new System.Windows.Forms.Label();
            this.bOK = new ExControls.ExButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.bFixSelected = new ExControls.ExButton();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.ProblemType = new System.Windows.Forms.DataGridViewImageColumn();
            this.textDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FixType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.solutionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iFixBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bgWorkAnalyze = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iFixBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pbStatus, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lStatus, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.bOK, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.dgvResults, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.bAnalyze);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Name = "panel1";
            // 
            // bAnalyze
            // 
            resources.ApplyResources(this.bAnalyze, "bAnalyze");
            this.bAnalyze.Name = "bAnalyze";
            this.bAnalyze.UseVisualStyleBackColor = true;
            this.bAnalyze.Click += new System.EventHandler(this.bAnalyze_Click);
            // 
            // pbStatus
            // 
            resources.ApplyResources(this.pbStatus, "pbStatus");
            this.pbStatus.Name = "pbStatus";
            // 
            // lStatus
            // 
            resources.ApplyResources(this.lStatus, "lStatus");
            this.lStatus.Name = "lStatus";
            // 
            // bOK
            // 
            resources.ApplyResources(this.bOK, "bOK");
            this.bOK.Name = "bOK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.bFixSelected);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // bFixSelected
            // 
            resources.ApplyResources(this.bFixSelected, "bFixSelected");
            this.bFixSelected.Name = "bFixSelected";
            this.bFixSelected.UseVisualStyleBackColor = true;
            this.bFixSelected.Click += new System.EventHandler(this.bFixSelected_Click);
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.AutoGenerateColumns = false;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProblemType,
            this.textDataGridViewTextBoxColumn,
            this.FixType,
            this.solutionDataGridViewTextBoxColumn});
            this.tableLayoutPanel1.SetColumnSpan(this.dgvResults, 3);
            this.dgvResults.DataSource = this.iFixBindingSource;
            resources.ApplyResources(this.dgvResults, "dgvResults");
            this.dgvResults.MultiSelect = false;
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.RowTemplate.Height = 24;
            this.dgvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResults.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvResults_CellFormatting);
            this.dgvResults.DoubleClick += new System.EventHandler(this.dgvResults_DoubleClick);
            // 
            // ProblemType
            // 
            resources.ApplyResources(this.ProblemType, "ProblemType");
            this.ProblemType.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.ProblemType.Name = "ProblemType";
            this.ProblemType.ReadOnly = true;
            // 
            // textDataGridViewTextBoxColumn
            // 
            this.textDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.textDataGridViewTextBoxColumn.DataPropertyName = "Text";
            resources.ApplyResources(this.textDataGridViewTextBoxColumn, "textDataGridViewTextBoxColumn");
            this.textDataGridViewTextBoxColumn.Name = "textDataGridViewTextBoxColumn";
            this.textDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // FixType
            // 
            resources.ApplyResources(this.FixType, "FixType");
            this.FixType.Name = "FixType";
            this.FixType.ReadOnly = true;
            this.FixType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FixType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // solutionDataGridViewTextBoxColumn
            // 
            this.solutionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.solutionDataGridViewTextBoxColumn.DataPropertyName = "Solution";
            resources.ApplyResources(this.solutionDataGridViewTextBoxColumn, "solutionDataGridViewTextBoxColumn");
            this.solutionDataGridViewTextBoxColumn.Name = "solutionDataGridViewTextBoxColumn";
            this.solutionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // iFixBindingSource
            // 
            this.iFixBindingSource.DataSource = typeof(GVDEditor.Tools.IProblem);
            // 
            // bgWorkAnalyze
            // 
            this.bgWorkAnalyze.WorkerReportsProgress = true;
            this.bgWorkAnalyze.WorkerSupportsCancellation = true;
            this.bgWorkAnalyze.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkAnalyze_DoWork);
            this.bgWorkAnalyze.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorkAnalyze_ProgressChanged);
            this.bgWorkAnalyze.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkAnalyze_RunWorkerCompleted);
            // 
            // FAnalyzer
            // 
            this.AcceptButton = this.bOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimizeBox = false;
            this.Name = "FAnalyzer";
            this.ShowIcon = false;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iFixBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ExControls.ExButton bAnalyze;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar pbStatus;
        private System.Windows.Forms.Label lStatus;
        private ExControls.ExButton bFixSelected;
        private ExControls.ExButton bOK;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.BindingSource iFixBindingSource;
        private System.ComponentModel.BackgroundWorker bgWorkAnalyze;
        private System.Windows.Forms.DataGridViewImageColumn ProblemType;
        private System.Windows.Forms.DataGridViewTextBoxColumn textDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FixType;
        private System.Windows.Forms.DataGridViewTextBoxColumn solutionDataGridViewTextBoxColumn;
    }
}