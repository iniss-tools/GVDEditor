using ExControls;
using GVDEditor.Controls;

namespace GVDEditor.Forms.EditTrain
{
    partial class TrainValidityPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TrainValidityPage));
            pScroll = new Panel();
            tlpMain = new TableLayoutPanel();
            lLimitHeader = new Label();
            lLimit = new Label();
            flpLimit = new FlowLayoutPanel();
            tbDateLimit = new ExTextBox();
            bEditLimit = new ExButton();
            lFrom = new Label();
            dtpFrom = new ExDateTimePicker();
            lTo = new Label();
            dtpTo = new ExDateTimePicker();
            llCalendar = new LinkLabel();
            lVariantHeader = new Label();
            lVariantInfo = new Label();
            strip = new VariantCalendarStrip();
            dgvVariants = new DataGridView();
            flpOther = new FlowLayoutPanel();
            bEditOther = new ExButton();
            bGiveThis = new ExButton();
            bGiveOther = new ExButton();
            lHint = new Label();
            colPosition = new DataGridViewTextBoxColumn();
            colRoute = new DataGridViewTextBoxColumn();
            colPeriod = new DataGridViewTextBoxColumn();
            colLimit = new DataGridViewTextBoxColumn();
            colCommon = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)dgvVariants).BeginInit();
            tlpMain.SuspendLayout();
            flpLimit.SuspendLayout();
            flpOther.SuspendLayout();
            SuspendLayout();
            // 
            // pScroll
            // 
            resources.ApplyResources(pScroll, "pScroll");
            pScroll.Controls.Add(tlpMain);
            pScroll.Name = "pScroll";
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lLimitHeader, 0, 0);
            tlpMain.Controls.Add(lLimit, 0, 1);
            tlpMain.Controls.Add(flpLimit, 1, 1);
            tlpMain.Controls.Add(lFrom, 0, 2);
            tlpMain.Controls.Add(dtpFrom, 1, 2);
            tlpMain.Controls.Add(lTo, 0, 3);
            tlpMain.Controls.Add(dtpTo, 1, 3);
            tlpMain.Controls.Add(llCalendar, 1, 4);
            tlpMain.Controls.Add(lVariantHeader, 0, 5);
            tlpMain.Controls.Add(lVariantInfo, 0, 6);
            tlpMain.Controls.Add(strip, 0, 7);
            tlpMain.Controls.Add(dgvVariants, 0, 8);
            tlpMain.Controls.Add(flpOther, 0, 9);
            tlpMain.Controls.Add(lHint, 0, 10);
            tlpMain.Name = "tlpMain";
            tlpMain.SetColumnSpan(lLimitHeader, 2);
            tlpMain.SetColumnSpan(lVariantHeader, 2);
            tlpMain.SetColumnSpan(lVariantInfo, 2);
            tlpMain.SetColumnSpan(strip, 2);
            tlpMain.SetColumnSpan(dgvVariants, 2);
            tlpMain.SetColumnSpan(flpOther, 2);
            tlpMain.SetColumnSpan(lHint, 2);
            // 
            // lLimitHeader
            // 
            resources.ApplyResources(lLimitHeader, "lLimitHeader");
            lLimitHeader.Name = "lLimitHeader";
            // 
            // lLimit
            // 
            resources.ApplyResources(lLimit, "lLimit");
            lLimit.Name = "lLimit";
            // 
            // flpLimit
            // 
            resources.ApplyResources(flpLimit, "flpLimit");
            flpLimit.Controls.Add(tbDateLimit);
            flpLimit.Controls.Add(bEditLimit);
            flpLimit.Name = "flpLimit";
            // 
            // tbDateLimit
            // 
            resources.ApplyResources(tbDateLimit, "tbDateLimit");
            tbDateLimit.Name = "tbDateLimit";
            tbDateLimit.TextChanged += tbDateLimit_TextChanged;
            // 
            // bEditLimit
            // 
            resources.ApplyResources(bEditLimit, "bEditLimit");
            bEditLimit.Name = "bEditLimit";
            bEditLimit.UseVisualStyleBackColor = true;
            bEditLimit.Click += bEditLimit_Click;
            // 
            // lFrom
            // 
            resources.ApplyResources(lFrom, "lFrom");
            lFrom.Name = "lFrom";
            // 
            // dtpFrom
            // 
            resources.ApplyResources(dtpFrom, "dtpFrom");
            dtpFrom.Name = "dtpFrom";
            dtpFrom.ValueChanged += Period_ValueChanged;
            // 
            // lTo
            // 
            resources.ApplyResources(lTo, "lTo");
            lTo.Name = "lTo";
            // 
            // dtpTo
            // 
            resources.ApplyResources(dtpTo, "dtpTo");
            dtpTo.Name = "dtpTo";
            dtpTo.ValueChanged += Period_ValueChanged;
            // 
            // llCalendar
            // 
            resources.ApplyResources(llCalendar, "llCalendar");
            llCalendar.Name = "llCalendar";
            llCalendar.TabStop = true;
            llCalendar.LinkClicked += llCalendar_LinkClicked;
            // 
            // lVariantHeader
            // 
            resources.ApplyResources(lVariantHeader, "lVariantHeader");
            lVariantHeader.Name = "lVariantHeader";
            // 
            // lVariantInfo
            // 
            resources.ApplyResources(lVariantInfo, "lVariantInfo");
            lVariantInfo.Name = "lVariantInfo";
            // 
            // strip
            // 
            resources.ApplyResources(strip, "strip");
            strip.Name = "strip";
            // 
            // dgvVariants
            // 
            resources.ApplyResources(dgvVariants, "dgvVariants");
            dgvVariants.AllowUserToAddRows = false;
            dgvVariants.AllowUserToDeleteRows = false;
            dgvVariants.AllowUserToResizeRows = false;
            dgvVariants.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVariants.Columns.AddRange(new DataGridViewColumn[] { colPosition, colRoute, colPeriod, colLimit, colCommon });
            dgvVariants.MultiSelect = false;
            dgvVariants.Name = "dgvVariants";
            dgvVariants.ReadOnly = true;
            dgvVariants.RowHeadersVisible = false;
            dgvVariants.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVariants.CellDoubleClick += dgvVariants_CellDoubleClick;
            dgvVariants.SelectionChanged += dgvVariants_SelectionChanged;
            // 
            // flpOther
            // 
            resources.ApplyResources(flpOther, "flpOther");
            flpOther.Controls.Add(bEditOther);
            flpOther.Controls.Add(bGiveThis);
            flpOther.Controls.Add(bGiveOther);
            flpOther.Name = "flpOther";
            // 
            // bEditOther
            // 
            resources.ApplyResources(bEditOther, "bEditOther");
            bEditOther.Name = "bEditOther";
            bEditOther.UseVisualStyleBackColor = true;
            bEditOther.Click += bEditOther_Click;
            // 
            // bGiveThis
            // 
            resources.ApplyResources(bGiveThis, "bGiveThis");
            bGiveThis.Name = "bGiveThis";
            bGiveThis.UseVisualStyleBackColor = true;
            bGiveThis.Click += bGiveThis_Click;
            // 
            // bGiveOther
            // 
            resources.ApplyResources(bGiveOther, "bGiveOther");
            bGiveOther.Name = "bGiveOther";
            bGiveOther.UseVisualStyleBackColor = true;
            bGiveOther.Click += bGiveOther_Click;
            // 
            // lHint
            // 
            resources.ApplyResources(lHint, "lHint");
            lHint.Name = "lHint";
            // 
            // colPosition
            // 
            resources.ApplyResources(colPosition, "colPosition");
            colPosition.Name = "colPosition";
            colPosition.ReadOnly = true;
            colPosition.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colRoute
            // 
            resources.ApplyResources(colRoute, "colRoute");
            colRoute.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colRoute.Name = "colRoute";
            colRoute.ReadOnly = true;
            colRoute.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colPeriod
            // 
            resources.ApplyResources(colPeriod, "colPeriod");
            colPeriod.Name = "colPeriod";
            colPeriod.ReadOnly = true;
            colPeriod.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colLimit
            // 
            resources.ApplyResources(colLimit, "colLimit");
            colLimit.Name = "colLimit";
            colLimit.ReadOnly = true;
            colLimit.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colCommon
            // 
            resources.ApplyResources(colCommon, "colCommon");
            colCommon.Name = "colCommon";
            colCommon.ReadOnly = true;
            colCommon.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // TrainValidityPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pScroll);
            Name = "TrainValidityPage";
            ((ISupportInitialize)dgvVariants).EndInit();
            flpOther.ResumeLayout(false);
            flpOther.PerformLayout();
            flpLimit.ResumeLayout(false);
            flpLimit.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pScroll;
        private TableLayoutPanel tlpMain;
        private Label lLimitHeader;
        private Label lLimit;
        private FlowLayoutPanel flpLimit;
        private ExTextBox tbDateLimit;
        private ExButton bEditLimit;
        private Label lFrom;
        private ExDateTimePicker dtpFrom;
        private Label lTo;
        private ExDateTimePicker dtpTo;
        private LinkLabel llCalendar;
        private Label lVariantHeader;
        private Label lVariantInfo;
        private VariantCalendarStrip strip;
        private DataGridView dgvVariants;
        private FlowLayoutPanel flpOther;
        private ExButton bEditOther;
        private ExButton bGiveThis;
        private ExButton bGiveOther;
        private Label lHint;
        private DataGridViewTextBoxColumn colPosition;
        private DataGridViewTextBoxColumn colRoute;
        private DataGridViewTextBoxColumn colPeriod;
        private DataGridViewTextBoxColumn colLimit;
        private DataGridViewTextBoxColumn colCommon;
    }
}
