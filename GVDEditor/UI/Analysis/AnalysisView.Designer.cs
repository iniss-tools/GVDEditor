namespace GVDEditor.UI.Analysis
{
    partial class AnalysisView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalysisView));
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.cSeverity = new System.Windows.Forms.DataGridViewImageColumn();
            this.cProblem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsResults = new System.Windows.Forms.ToolStrip();
            this.tsbErrors = new ToolsCore.Icons.IconToolStripButton();
            this.tsbWarnings = new ToolsCore.Icons.IconToolStripButton();
            this.tsbInfos = new ToolsCore.Icons.IconToolStripButton();
            this.tssResults1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAnalyze = new ToolsCore.Icons.IconToolStripButton();
            this.tsbFix = new ToolsCore.Icons.IconToolStripButton();
            this.tspbProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tslStatus = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.tsResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvResults
            // 
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cSeverity,
            this.cProblem,
            this.cFix,
            this.cSolution});
            resources.ApplyResources(this.dgvResults, "dgvResults");
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.AllowUserToResizeRows = false;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.MultiSelect = false;
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResults_CellDoubleClick);
            this.dgvResults.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvResults_CellFormatting);
            this.dgvResults.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvResults_KeyDown);
            this.dgvResults.SelectionChanged += new System.EventHandler(this.dgvResults_SelectionChanged);
            // 
            // tsResults
            // 
            this.tsResults.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbErrors,
            this.tsbWarnings,
            this.tsbInfos,
            this.tssResults1,
            this.tsbAnalyze,
            this.tsbFix,
            this.tspbProgress,
            this.tslStatus});
            resources.ApplyResources(this.tsResults, "tsResults");
            this.tsResults.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsResults.ImageScalingSize = new System.Drawing.Size(16, 16);
            this.tsResults.Padding = new System.Windows.Forms.Padding(3, 0, 1, 0);
            this.tsResults.Name = "tsResults";
            // 
            // tsbErrors
            // 
            resources.ApplyResources(this.tsbErrors, "tsbErrors");
            this.tsbErrors.Checked = true;
            this.tsbErrors.CheckOnClick = true;
            this.tsbErrors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbErrors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsbErrors.IconName = "status-error";
            this.tsbErrors.Name = "tsbErrors";
            this.tsbErrors.CheckedChanged += new System.EventHandler(this.Filter_CheckedChanged);
            // 
            // tsbWarnings
            // 
            resources.ApplyResources(this.tsbWarnings, "tsbWarnings");
            this.tsbWarnings.Checked = true;
            this.tsbWarnings.CheckOnClick = true;
            this.tsbWarnings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbWarnings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsbWarnings.IconName = "status-warning";
            this.tsbWarnings.Name = "tsbWarnings";
            this.tsbWarnings.CheckedChanged += new System.EventHandler(this.Filter_CheckedChanged);
            // 
            // tsbInfos
            // 
            resources.ApplyResources(this.tsbInfos, "tsbInfos");
            this.tsbInfos.Checked = true;
            this.tsbInfos.CheckOnClick = true;
            this.tsbInfos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbInfos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsbInfos.IconName = "status-info";
            this.tsbInfos.Name = "tsbInfos";
            this.tsbInfos.CheckedChanged += new System.EventHandler(this.Filter_CheckedChanged);
            // 
            // tssResults1
            // 
            resources.ApplyResources(this.tssResults1, "tssResults1");
            this.tssResults1.Name = "tssResults1";
            // 
            // tsbAnalyze
            // 
            resources.ApplyResources(this.tsbAnalyze, "tsbAnalyze");
            this.tsbAnalyze.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsbAnalyze.IconName = "rerun";
            this.tsbAnalyze.Name = "tsbAnalyze";
            this.tsbAnalyze.Click += new System.EventHandler(this.tsbAnalyze_Click);
            // 
            // tsbFix
            // 
            resources.ApplyResources(this.tsbFix, "tsbFix");
            this.tsbFix.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsbFix.IconName = "wrench";
            this.tsbFix.Name = "tsbFix";
            this.tsbFix.Click += new System.EventHandler(this.tsbFix_Click);
            // 
            // tspbProgress
            // 
            resources.ApplyResources(this.tspbProgress, "tspbProgress");
            this.tspbProgress.Visible = false;
            this.tspbProgress.Name = "tspbProgress";
            // 
            // tslStatus
            // 
            resources.ApplyResources(this.tslStatus, "tslStatus");
            this.tslStatus.Name = "tslStatus";
            // 
            // AnalysisView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvResults);
            this.Controls.Add(this.tsResults);
            this.Name = "AnalysisView";
            resources.ApplyResources(this, "$this");
            // 
            // cSeverity
            // 
            resources.ApplyResources(this.cSeverity, "cSeverity");
            this.cSeverity.ReadOnly = true;
            this.cSeverity.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Normal;
            this.cSeverity.Name = "cSeverity";
            // 
            // cProblem
            // 
            resources.ApplyResources(this.cProblem, "cProblem");
            this.cProblem.DataPropertyName = "Text";
            this.cProblem.ReadOnly = true;
            this.cProblem.Name = "cProblem";
            // 
            // cFix
            // 
            resources.ApplyResources(this.cFix, "cFix");
            this.cFix.ReadOnly = true;
            this.cFix.Name = "cFix";
            // 
            // cSolution
            // 
            resources.ApplyResources(this.cSolution, "cSolution");
            this.cSolution.DataPropertyName = "Solution";
            this.cSolution.ReadOnly = true;
            this.cSolution.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cSolution.Name = "cSolution";
            this.tsResults.ResumeLayout(false);
            this.tsResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.DataGridViewImageColumn cSeverity;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProblem;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFix;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSolution;
        private System.Windows.Forms.ToolStrip tsResults;
        private ToolsCore.Icons.IconToolStripButton tsbErrors;
        private ToolsCore.Icons.IconToolStripButton tsbWarnings;
        private ToolsCore.Icons.IconToolStripButton tsbInfos;
        private System.Windows.Forms.ToolStripSeparator tssResults1;
        private ToolsCore.Icons.IconToolStripButton tsbAnalyze;
        private ToolsCore.Icons.IconToolStripButton tsbFix;
        private System.Windows.Forms.ToolStripProgressBar tspbProgress;
        private System.Windows.Forms.ToolStripLabel tslStatus;
    }
}
