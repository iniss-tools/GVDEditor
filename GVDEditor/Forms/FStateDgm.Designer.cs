namespace GVDEditor.Forms
{
    partial class FStateDgm
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FStateDgm));
            scOuter = new SplitContainer();
            scMain = new SplitContainer();
            tvNav = new TreeView();
            scRight = new SplitContainer();
            tcCenter = new ExControls.ExTabControl();
            tpGraph = new TabPage();
            pnlGraph = new Panel();
            tpText = new TabPage();
            scText = new GVDEditor.Controls.MyScintilla();
            pnlProps = new Panel();
            tcBottom = new ExControls.ExTabControl();
            tpEvents = new TabPage();
            dgvEvents = new DataGridView();
            cEvCtrl = new DataGridViewTextBoxColumn();
            cEvDesign = new DataGridViewTextBoxColumn();
            cEvKey = new DataGridViewTextBoxColumn();
            cEvClass = new DataGridViewTextBoxColumn();
            cEvNext = new DataGridViewTextBoxColumn();
            cEvReport = new DataGridViewTextBoxColumn();
            cEvDialog = new DataGridViewTextBoxColumn();
            tsEvents = new ToolStrip();
            tsbEvAdd = new ToolStripButton();
            tsbEvEdit = new ToolStripButton();
            tsbEvDelete = new ToolStripButton();
            tsbEvUp = new ToolStripButton();
            tsbEvDown = new ToolStripButton();
            tpStarters = new TabPage();
            dgvStarters = new DataGridView();
            cStKey = new DataGridViewTextBoxColumn();
            cStEvent = new DataGridViewTextBoxColumn();
            cStText = new DataGridViewTextBoxColumn();
            tsStarters = new ToolStrip();
            tsbStAdd = new ToolStripButton();
            tsbStEdit = new ToolStripButton();
            tsbStDelete = new ToolStripButton();
            tpProblems = new TabPage();
            dgvProblems = new DataGridView();
            cProbType = new DataGridViewImageColumn();
            cProbCode = new DataGridViewTextBoxColumn();
            cProbPath = new DataGridViewTextBoxColumn();
            cProbMessage = new DataGridViewTextBoxColumn();
            tsMain = new ToolStrip();
            tsbSave = new ToolStripButton();
            tsbCheck = new ToolStripButton();
            tss1 = new ToolStripSeparator();
            tsddNew = new ToolStripDropDownButton();
            tsmiNewCategory = new ToolStripMenuItem();
            tsmiNewState = new ToolStripMenuItem();
            tsmiNewDesign = new ToolStripMenuItem();
            tsmiNewTimePoint = new ToolStripMenuItem();
            tsbDelete = new ToolStripButton();
            tsbUp = new ToolStripButton();
            tsbDown = new ToolStripButton();
            tss2 = new ToolStripSeparator();
            tsddTemplate = new ToolStripDropDownButton();
            tsmiTplSK = new ToolStripMenuItem();
            tsmiTplCZ = new ToolStripMenuItem();
            tsmiTplILTIS = new ToolStripMenuItem();
            tss3 = new ToolStripSeparator();
            tsbCalendar = new ToolStripButton();
            ssMain = new StatusStrip();
            tsslStatus = new ToolStripStatusLabel();
            ((ISupportInitialize)scOuter).BeginInit();
            scOuter.Panel1.SuspendLayout();
            scOuter.Panel2.SuspendLayout();
            scOuter.SuspendLayout();
            ((ISupportInitialize)scMain).BeginInit();
            scMain.Panel1.SuspendLayout();
            scMain.Panel2.SuspendLayout();
            scMain.SuspendLayout();
            ((ISupportInitialize)scRight).BeginInit();
            scRight.Panel1.SuspendLayout();
            scRight.Panel2.SuspendLayout();
            scRight.SuspendLayout();
            tcCenter.SuspendLayout();
            tpGraph.SuspendLayout();
            tpText.SuspendLayout();
            tcBottom.SuspendLayout();
            tpEvents.SuspendLayout();
            ((ISupportInitialize)dgvEvents).BeginInit();
            tsEvents.SuspendLayout();
            tpStarters.SuspendLayout();
            ((ISupportInitialize)dgvStarters).BeginInit();
            tsStarters.SuspendLayout();
            tpProblems.SuspendLayout();
            ((ISupportInitialize)dgvProblems).BeginInit();
            tsMain.SuspendLayout();
            ssMain.SuspendLayout();
            SuspendLayout();
            // 
            // scOuter
            // 
            resources.ApplyResources(scOuter, "scOuter");
            scOuter.Name = "scOuter";
            // 
            // scOuter.Panel1
            // 
            scOuter.Panel1.Controls.Add(scMain);
            // 
            // scOuter.Panel2
            // 
            scOuter.Panel2.Controls.Add(tcBottom);
            // 
            // scMain
            // 
            resources.ApplyResources(scMain, "scMain");
            scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            scMain.Panel1.Controls.Add(tvNav);
            // 
            // scMain.Panel2
            // 
            scMain.Panel2.Controls.Add(scRight);
            // 
            // tvNav
            // 
            resources.ApplyResources(tvNav, "tvNav");
            tvNav.HideSelection = false;
            tvNav.ItemHeight = 24;
            tvNav.Name = "tvNav";
            tvNav.AfterSelect += tvNav_AfterSelect;
            // 
            // scRight
            // 
            resources.ApplyResources(scRight, "scRight");
            scRight.Name = "scRight";
            // 
            // scRight.Panel1
            // 
            scRight.Panel1.Controls.Add(tcCenter);
            // 
            // scRight.Panel2
            // 
            scRight.Panel2.Controls.Add(pnlProps);
            // 
            // tcCenter
            // 
            tcCenter.ActiveHeaderBackColor = Color.White;
            tcCenter.ActiveHeaderForeColor = Color.Black;
            tcCenter.BorderColor = Color.LightGray;
            tcCenter.BorderThickness = 1;
            tcCenter.Controls.Add(tpGraph);
            tcCenter.Controls.Add(tpText);
            tcCenter.DefaultStyle = true;
            resources.ApplyResources(tcCenter, "tcCenter");
            tcCenter.HeaderBackColor = SystemColors.Control;
            tcCenter.HeaderForeColor = Color.Black;
            tcCenter.HighlightBackColor = SystemColors.GradientInactiveCaption;
            tcCenter.HighlightForeColor = Color.Black;
            tcCenter.Name = "tcCenter";
            tcCenter.SelectedIndex = 0;
            tcCenter.SelectedIndexChanged += tcCenter_SelectedIndexChanged;
            // 
            // tpGraph
            // 
            tpGraph.Controls.Add(pnlGraph);
            resources.ApplyResources(tpGraph, "tpGraph");
            tpGraph.Name = "tpGraph";
            tpGraph.UseVisualStyleBackColor = true;
            // 
            // pnlGraph
            // 
            resources.ApplyResources(pnlGraph, "pnlGraph");
            pnlGraph.Name = "pnlGraph";
            // 
            // tpText
            // 
            tpText.Controls.Add(scText);
            resources.ApplyResources(tpText, "tpText");
            tpText.Name = "tpText";
            tpText.UseVisualStyleBackColor = true;
            // 
            // scText
            // 
            resources.ApplyResources(scText, "scText");
            scText.Name = "scText";
            // 
            // pnlProps
            // 
            resources.ApplyResources(pnlProps, "pnlProps");
            pnlProps.Name = "pnlProps";
            // 
            // tcBottom
            // 
            tcBottom.ActiveHeaderBackColor = Color.White;
            tcBottom.ActiveHeaderForeColor = Color.Black;
            tcBottom.BorderColor = Color.LightGray;
            tcBottom.BorderThickness = 1;
            tcBottom.Controls.Add(tpEvents);
            tcBottom.Controls.Add(tpStarters);
            tcBottom.Controls.Add(tpProblems);
            tcBottom.DefaultStyle = true;
            resources.ApplyResources(tcBottom, "tcBottom");
            tcBottom.HeaderBackColor = SystemColors.Control;
            tcBottom.HeaderForeColor = Color.Black;
            tcBottom.HighlightBackColor = SystemColors.GradientInactiveCaption;
            tcBottom.HighlightForeColor = Color.Black;
            tcBottom.Name = "tcBottom";
            tcBottom.SelectedIndex = 0;
            // 
            // tpEvents
            // 
            tpEvents.Controls.Add(dgvEvents);
            tpEvents.Controls.Add(tsEvents);
            resources.ApplyResources(tpEvents, "tpEvents");
            tpEvents.Name = "tpEvents";
            tpEvents.UseVisualStyleBackColor = true;
            // 
            // dgvEvents
            // 
            dgvEvents.AllowUserToAddRows = false;
            dgvEvents.AllowUserToDeleteRows = false;
            dgvEvents.AllowUserToResizeRows = false;
            dgvEvents.BackgroundColor = SystemColors.Control;
            dgvEvents.BorderStyle = BorderStyle.None;
            dgvEvents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEvents.Columns.AddRange(new DataGridViewColumn[] { cEvCtrl, cEvDesign, cEvKey, cEvClass, cEvNext, cEvReport, cEvDialog });
            resources.ApplyResources(dgvEvents, "dgvEvents");
            dgvEvents.MultiSelect = false;
            dgvEvents.Name = "dgvEvents";
            dgvEvents.ReadOnly = true;
            dgvEvents.RowHeadersVisible = false;
            dgvEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEvents.CellDoubleClick += dgvEvents_CellDoubleClick;
            dgvEvents.SelectionChanged += dgvEvents_SelectionChanged;
            // 
            // tsEvents
            // 
            tsEvents.GripStyle = ToolStripGripStyle.Hidden;
            tsEvents.Items.AddRange(new ToolStripItem[] { tsbEvAdd, tsbEvEdit, tsbEvDelete, tsbEvUp, tsbEvDown });
            resources.ApplyResources(tsEvents, "tsEvents");
            tsEvents.Name = "tsEvents";
            // 
            // tsbEvAdd
            // 
            resources.ApplyResources(tsbEvAdd, "tsbEvAdd");
            tsbEvAdd.Name = "tsbEvAdd";
            tsbEvAdd.Click += tsbEvAdd_Click;
            // 
            // tsbEvEdit
            // 
            resources.ApplyResources(tsbEvEdit, "tsbEvEdit");
            tsbEvEdit.Name = "tsbEvEdit";
            tsbEvEdit.Click += tsbEvEdit_Click;
            // 
            // tsbEvDelete
            // 
            resources.ApplyResources(tsbEvDelete, "tsbEvDelete");
            tsbEvDelete.Name = "tsbEvDelete";
            tsbEvDelete.Click += tsbEvDelete_Click;
            // 
            // tsbEvUp
            // 
            tsbEvUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(tsbEvUp, "tsbEvUp");
            tsbEvUp.Name = "tsbEvUp";
            tsbEvUp.Click += tsbEvUp_Click;
            // 
            // tsbEvDown
            // 
            tsbEvDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(tsbEvDown, "tsbEvDown");
            tsbEvDown.Name = "tsbEvDown";
            tsbEvDown.Click += tsbEvDown_Click;
            // 
            // tpStarters
            // 
            tpStarters.Controls.Add(dgvStarters);
            tpStarters.Controls.Add(tsStarters);
            resources.ApplyResources(tpStarters, "tpStarters");
            tpStarters.Name = "tpStarters";
            tpStarters.UseVisualStyleBackColor = true;
            // 
            // dgvStarters
            // 
            dgvStarters.AllowUserToAddRows = false;
            dgvStarters.AllowUserToDeleteRows = false;
            dgvStarters.AllowUserToResizeRows = false;
            dgvStarters.BackgroundColor = SystemColors.Control;
            dgvStarters.BorderStyle = BorderStyle.None;
            dgvStarters.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStarters.Columns.AddRange(new DataGridViewColumn[] { cStKey, cStEvent, cStText });
            resources.ApplyResources(dgvStarters, "dgvStarters");
            dgvStarters.MultiSelect = false;
            dgvStarters.Name = "dgvStarters";
            dgvStarters.ReadOnly = true;
            dgvStarters.RowHeadersVisible = false;
            dgvStarters.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStarters.CellDoubleClick += dgvStarters_CellDoubleClick;
            dgvStarters.SelectionChanged += dgvStarters_SelectionChanged;
            // 
            // tsStarters
            // 
            tsStarters.GripStyle = ToolStripGripStyle.Hidden;
            tsStarters.Items.AddRange(new ToolStripItem[] { tsbStAdd, tsbStEdit, tsbStDelete });
            resources.ApplyResources(tsStarters, "tsStarters");
            tsStarters.Name = "tsStarters";
            // 
            // tsbStAdd
            // 
            resources.ApplyResources(tsbStAdd, "tsbStAdd");
            tsbStAdd.Name = "tsbStAdd";
            tsbStAdd.Click += tsbStAdd_Click;
            // 
            // tsbStEdit
            // 
            resources.ApplyResources(tsbStEdit, "tsbStEdit");
            tsbStEdit.Name = "tsbStEdit";
            tsbStEdit.Click += tsbStEdit_Click;
            // 
            // tsbStDelete
            // 
            resources.ApplyResources(tsbStDelete, "tsbStDelete");
            tsbStDelete.Name = "tsbStDelete";
            tsbStDelete.Click += tsbStDelete_Click;
            // 
            // tpProblems
            // 
            tpProblems.Controls.Add(dgvProblems);
            resources.ApplyResources(tpProblems, "tpProblems");
            tpProblems.Name = "tpProblems";
            tpProblems.UseVisualStyleBackColor = true;
            // 
            // dgvProblems
            // 
            dgvProblems.AllowUserToAddRows = false;
            dgvProblems.AllowUserToDeleteRows = false;
            dgvProblems.AllowUserToResizeRows = false;
            dgvProblems.BackgroundColor = SystemColors.Control;
            dgvProblems.BorderStyle = BorderStyle.None;
            dgvProblems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProblems.Columns.AddRange(new DataGridViewColumn[] { cProbType, cProbCode, cProbPath, cProbMessage });
            resources.ApplyResources(dgvProblems, "dgvProblems");
            dgvProblems.MultiSelect = false;
            dgvProblems.Name = "dgvProblems";
            dgvProblems.ReadOnly = true;
            dgvProblems.RowHeadersVisible = false;
            dgvProblems.RowTemplate.Height = 22;
            dgvProblems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProblems.CellDoubleClick += dgvProblems_CellDoubleClick;
            dgvProblems.CellFormatting += dgvProblems_CellFormatting;
            // 
            // tsMain
            // 
            tsMain.GripStyle = ToolStripGripStyle.Hidden;
            tsMain.Items.AddRange(new ToolStripItem[] { tsbSave, tsbCheck, tss1, tsddNew, tsbDelete, tsbUp, tsbDown, tss2, tsddTemplate, tss3, tsbCalendar });
            resources.ApplyResources(tsMain, "tsMain");
            tsMain.Name = "tsMain";
            // 
            // tsbSave
            // 
            resources.ApplyResources(tsbSave, "tsbSave");
            tsbSave.Name = "tsbSave";
            tsbSave.Click += tsbSave_Click;
            // 
            // tsbCheck
            // 
            resources.ApplyResources(tsbCheck, "tsbCheck");
            tsbCheck.Name = "tsbCheck";
            tsbCheck.Click += tsbCheck_Click;
            // 
            // tss1
            // 
            tss1.Name = "tss1";
            resources.ApplyResources(tss1, "tss1");
            // 
            // tsddNew
            // 
            tsddNew.DropDownItems.AddRange(new ToolStripItem[] { tsmiNewCategory, tsmiNewState, tsmiNewDesign, tsmiNewTimePoint });
            resources.ApplyResources(tsddNew, "tsddNew");
            tsddNew.Name = "tsddNew";
            // 
            // tsmiNewCategory
            // 
            tsmiNewCategory.Name = "tsmiNewCategory";
            resources.ApplyResources(tsmiNewCategory, "tsmiNewCategory");
            tsmiNewCategory.Click += tsmiNewCategory_Click;
            // 
            // tsmiNewState
            // 
            tsmiNewState.Name = "tsmiNewState";
            resources.ApplyResources(tsmiNewState, "tsmiNewState");
            tsmiNewState.Click += tsmiNewState_Click;
            // 
            // tsmiNewDesign
            // 
            tsmiNewDesign.Name = "tsmiNewDesign";
            resources.ApplyResources(tsmiNewDesign, "tsmiNewDesign");
            tsmiNewDesign.Click += tsmiNewDesign_Click;
            // 
            // tsmiNewTimePoint
            // 
            tsmiNewTimePoint.Name = "tsmiNewTimePoint";
            resources.ApplyResources(tsmiNewTimePoint, "tsmiNewTimePoint");
            tsmiNewTimePoint.Click += tsmiNewTimePoint_Click;
            // 
            // tsbDelete
            // 
            resources.ApplyResources(tsbDelete, "tsbDelete");
            tsbDelete.Name = "tsbDelete";
            tsbDelete.Click += tsbDelete_Click;
            // 
            // tsbUp
            // 
            tsbUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(tsbUp, "tsbUp");
            tsbUp.Name = "tsbUp";
            tsbUp.Click += tsbUp_Click;
            // 
            // tsbDown
            // 
            tsbDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(tsbDown, "tsbDown");
            tsbDown.Name = "tsbDown";
            tsbDown.Click += tsbDown_Click;
            // 
            // tss2
            // 
            tss2.Name = "tss2";
            resources.ApplyResources(tss2, "tss2");
            // 
            // tsddTemplate
            // 
            tsddTemplate.DropDownItems.AddRange(new ToolStripItem[] { tsmiTplSK, tsmiTplCZ, tsmiTplILTIS });
            resources.ApplyResources(tsddTemplate, "tsddTemplate");
            tsddTemplate.Name = "tsddTemplate";
            // 
            // tsmiTplSK
            // 
            tsmiTplSK.Name = "tsmiTplSK";
            resources.ApplyResources(tsmiTplSK, "tsmiTplSK");
            tsmiTplSK.Click += tsmiTpl_Click;
            // 
            // tsmiTplCZ
            // 
            tsmiTplCZ.Name = "tsmiTplCZ";
            resources.ApplyResources(tsmiTplCZ, "tsmiTplCZ");
            tsmiTplCZ.Click += tsmiTpl_Click;
            // 
            // tsmiTplILTIS
            // 
            tsmiTplILTIS.Name = "tsmiTplILTIS";
            resources.ApplyResources(tsmiTplILTIS, "tsmiTplILTIS");
            tsmiTplILTIS.Click += tsmiTpl_Click;
            // 
            // tss3
            // 
            tss3.Name = "tss3";
            resources.ApplyResources(tss3, "tss3");
            // 
            // tsbCalendar
            // 
            resources.ApplyResources(tsbCalendar, "tsbCalendar");
            tsbCalendar.Name = "tsbCalendar";
            tsbCalendar.Click += tsbCalendar_Click;
            // 
            // ssMain
            // 
            ssMain.Items.AddRange(new ToolStripItem[] { tsslStatus });
            resources.ApplyResources(ssMain, "ssMain");
            ssMain.Name = "ssMain";
            // 
            // tsslStatus
            // 
            tsslStatus.Name = "tsslStatus";
            resources.ApplyResources(tsslStatus, "tsslStatus");
            tsslStatus.Spring = true;
            // 
            // cEvCtrl
            // 
            cEvCtrl.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            cEvCtrl.DataPropertyName = "CtrlId";
            resources.ApplyResources(cEvCtrl, "cEvCtrl");
            cEvCtrl.Name = "cEvCtrl";
            cEvCtrl.ReadOnly = true;
            // 
            // cEvDesign
            // 
            cEvDesign.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            cEvDesign.DataPropertyName = "Design";
            resources.ApplyResources(cEvDesign, "cEvDesign");
            cEvDesign.Name = "cEvDesign";
            cEvDesign.ReadOnly = true;
            // 
            // cEvKey
            // 
            cEvKey.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            cEvKey.DataPropertyName = "Key";
            resources.ApplyResources(cEvKey, "cEvKey");
            cEvKey.Name = "cEvKey";
            cEvKey.ReadOnly = true;
            // 
            // cEvClass
            // 
            cEvClass.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            cEvClass.DataPropertyName = "Class";
            resources.ApplyResources(cEvClass, "cEvClass");
            cEvClass.Name = "cEvClass";
            cEvClass.ReadOnly = true;
            // 
            // cEvNext
            // 
            cEvNext.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            cEvNext.DataPropertyName = "NextState";
            resources.ApplyResources(cEvNext, "cEvNext");
            cEvNext.Name = "cEvNext";
            cEvNext.ReadOnly = true;
            // 
            // cEvReport
            // 
            cEvReport.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            cEvReport.DataPropertyName = "ReportKey";
            resources.ApplyResources(cEvReport, "cEvReport");
            cEvReport.Name = "cEvReport";
            cEvReport.ReadOnly = true;
            // 
            // cEvDialog
            // 
            cEvDialog.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            cEvDialog.DataPropertyName = "Dialog";
            resources.ApplyResources(cEvDialog, "cEvDialog");
            cEvDialog.Name = "cEvDialog";
            cEvDialog.ReadOnly = true;
            // 
            // cStKey
            // 
            cStKey.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            cStKey.DataPropertyName = "Key";
            resources.ApplyResources(cStKey, "cStKey");
            cStKey.Name = "cStKey";
            cStKey.ReadOnly = true;
            // 
            // cStEvent
            // 
            cStEvent.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            cStEvent.DataPropertyName = "EventKey";
            resources.ApplyResources(cStEvent, "cStEvent");
            cStEvent.Name = "cStEvent";
            cStEvent.ReadOnly = true;
            // 
            // cStText
            // 
            cStText.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            cStText.DataPropertyName = "Text";
            resources.ApplyResources(cStText, "cStText");
            cStText.Name = "cStText";
            cStText.ReadOnly = true;
            // 
            // cProbType
            // 
            cProbType.DataPropertyName = "Severity";
            cProbType.ImageLayout = DataGridViewImageCellLayout.Zoom;
            resources.ApplyResources(cProbType, "cProbType");
            cProbType.Name = "cProbType";
            cProbType.ReadOnly = true;
            cProbType.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // cProbCode
            // 
            cProbCode.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            cProbCode.DataPropertyName = "Code";
            resources.ApplyResources(cProbCode, "cProbCode");
            cProbCode.Name = "cProbCode";
            cProbCode.ReadOnly = true;
            // 
            // cProbPath
            // 
            cProbPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            cProbPath.DataPropertyName = "Path";
            resources.ApplyResources(cProbPath, "cProbPath");
            cProbPath.Name = "cProbPath";
            cProbPath.ReadOnly = true;
            // 
            // cProbMessage
            // 
            cProbMessage.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            cProbMessage.DataPropertyName = "Message";
            resources.ApplyResources(cProbMessage, "cProbMessage");
            cProbMessage.Name = "cProbMessage";
            cProbMessage.ReadOnly = true;
            // 
            // FStateDgm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(scOuter);
            Controls.Add(ssMain);
            Controls.Add(tsMain);
            KeyPreview = true;
            Name = "FStateDgm";
            ShowIcon = false;
            WindowState = FormWindowState.Maximized;
            FormClosing += FStateDgm_FormClosing;
            Load += FStateDgm_Load;
            KeyDown += FStateDgm_KeyDown;
            scOuter.Panel1.ResumeLayout(false);
            scOuter.Panel2.ResumeLayout(false);
            ((ISupportInitialize)scOuter).EndInit();
            scOuter.ResumeLayout(false);
            scMain.Panel1.ResumeLayout(false);
            scMain.Panel2.ResumeLayout(false);
            ((ISupportInitialize)scMain).EndInit();
            scMain.ResumeLayout(false);
            scRight.Panel1.ResumeLayout(false);
            scRight.Panel2.ResumeLayout(false);
            ((ISupportInitialize)scRight).EndInit();
            scRight.ResumeLayout(false);
            tcCenter.ResumeLayout(false);
            tpGraph.ResumeLayout(false);
            tpText.ResumeLayout(false);
            tcBottom.ResumeLayout(false);
            tpEvents.ResumeLayout(false);
            tpEvents.PerformLayout();
            ((ISupportInitialize)dgvEvents).EndInit();
            tsEvents.ResumeLayout(false);
            tsEvents.PerformLayout();
            tpStarters.ResumeLayout(false);
            tpStarters.PerformLayout();
            ((ISupportInitialize)dgvStarters).EndInit();
            tsStarters.ResumeLayout(false);
            tsStarters.PerformLayout();
            tpProblems.ResumeLayout(false);
            ((ISupportInitialize)dgvProblems).EndInit();
            tsMain.ResumeLayout(false);
            tsMain.PerformLayout();
            ssMain.ResumeLayout(false);
            ssMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbCheck;
        private System.Windows.Forms.ToolStripSeparator tss1;
        private System.Windows.Forms.ToolStripDropDownButton tsddNew;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewCategory;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewState;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewDesign;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewTimePoint;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripButton tsbUp;
        private System.Windows.Forms.ToolStripButton tsbDown;
        private System.Windows.Forms.ToolStripSeparator tss2;
        private System.Windows.Forms.ToolStripDropDownButton tsddTemplate;
        private System.Windows.Forms.ToolStripMenuItem tsmiTplSK;
        private System.Windows.Forms.ToolStripMenuItem tsmiTplCZ;
        private System.Windows.Forms.ToolStripMenuItem tsmiTplILTIS;
        private System.Windows.Forms.ToolStripSeparator tss3;
        private System.Windows.Forms.ToolStripButton tsbCalendar;
        private System.Windows.Forms.SplitContainer scOuter;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.TreeView tvNav;
        private System.Windows.Forms.SplitContainer scRight;
        private ExControls.ExTabControl tcCenter;
        private System.Windows.Forms.TabPage tpGraph;
        private System.Windows.Forms.Panel pnlGraph;
        private System.Windows.Forms.TabPage tpText;
        private GVDEditor.Controls.MyScintilla scText;
        private System.Windows.Forms.Panel pnlProps;
        private ExControls.ExTabControl tcBottom;
        private System.Windows.Forms.TabPage tpEvents;
        private System.Windows.Forms.DataGridView dgvEvents;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEvCtrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEvDesign;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEvKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEvClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEvNext;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEvReport;
        private System.Windows.Forms.DataGridViewTextBoxColumn cEvDialog;
        private System.Windows.Forms.ToolStrip tsEvents;
        private System.Windows.Forms.ToolStripButton tsbEvAdd;
        private System.Windows.Forms.ToolStripButton tsbEvEdit;
        private System.Windows.Forms.ToolStripButton tsbEvDelete;
        private System.Windows.Forms.ToolStripButton tsbEvUp;
        private System.Windows.Forms.ToolStripButton tsbEvDown;
        private System.Windows.Forms.TabPage tpStarters;
        private System.Windows.Forms.DataGridView dgvStarters;
        private System.Windows.Forms.DataGridViewTextBoxColumn cStKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn cStEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn cStText;
        private System.Windows.Forms.ToolStrip tsStarters;
        private System.Windows.Forms.ToolStripButton tsbStAdd;
        private System.Windows.Forms.ToolStripButton tsbStEdit;
        private System.Windows.Forms.ToolStripButton tsbStDelete;
        private System.Windows.Forms.TabPage tpProblems;
        private System.Windows.Forms.DataGridView dgvProblems;
        private System.Windows.Forms.DataGridViewImageColumn cProbType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProbCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProbPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProbMessage;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
    }
}
