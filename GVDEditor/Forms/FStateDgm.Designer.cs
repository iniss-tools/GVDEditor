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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FStateDgm));
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbCheck = new System.Windows.Forms.ToolStripButton();
            this.tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddNew = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiNewCategory = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewState = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewDesign = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewTimePoint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbUp = new System.Windows.Forms.ToolStripButton();
            this.tsbDown = new System.Windows.Forms.ToolStripButton();
            this.tss2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddTemplate = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiTplSK = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTplCZ = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTplILTIS = new System.Windows.Forms.ToolStripMenuItem();
            this.tss3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCalendar = new System.Windows.Forms.ToolStripButton();
            this.scOuter = new System.Windows.Forms.SplitContainer();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.tvNav = new System.Windows.Forms.TreeView();
            this.scRight = new System.Windows.Forms.SplitContainer();
            this.tcCenter = new ExControls.ExTabControl();
            this.tpGraph = new System.Windows.Forms.TabPage();
            this.pnlGraph = new System.Windows.Forms.Panel();
            this.tpText = new System.Windows.Forms.TabPage();
            this.tbText = new ExControls.ExTextBox();
            this.pnlProps = new System.Windows.Forms.Panel();
            this.tcBottom = new ExControls.ExTabControl();
            this.tpEvents = new System.Windows.Forms.TabPage();
            this.dgvEvents = new System.Windows.Forms.DataGridView();
            this.cEvCtrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cEvDesign = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cEvKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cEvClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cEvNext = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cEvReport = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cEvDialog = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsEvents = new System.Windows.Forms.ToolStrip();
            this.tsbEvAdd = new System.Windows.Forms.ToolStripButton();
            this.tsbEvEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbEvDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbEvUp = new System.Windows.Forms.ToolStripButton();
            this.tsbEvDown = new System.Windows.Forms.ToolStripButton();
            this.tpStarters = new System.Windows.Forms.TabPage();
            this.dgvStarters = new System.Windows.Forms.DataGridView();
            this.cStKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cStEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cStText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsStarters = new System.Windows.Forms.ToolStrip();
            this.tsbStAdd = new System.Windows.Forms.ToolStripButton();
            this.tsbStEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbStDelete = new System.Windows.Forms.ToolStripButton();
            this.tpProblems = new System.Windows.Forms.TabPage();
            this.dgvProblems = new System.Windows.Forms.DataGridView();
            this.cProbType = new System.Windows.Forms.DataGridViewImageColumn();
            this.cProbCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cProbPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cProbMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scOuter)).BeginInit();
            this.scOuter.Panel1.SuspendLayout();
            this.scOuter.Panel2.SuspendLayout();
            this.scOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scRight)).BeginInit();
            this.scRight.Panel1.SuspendLayout();
            this.scRight.Panel2.SuspendLayout();
            this.scRight.SuspendLayout();
            this.tcCenter.SuspendLayout();
            this.tpGraph.SuspendLayout();
            this.tpText.SuspendLayout();
            this.tcBottom.SuspendLayout();
            this.tpEvents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).BeginInit();
            this.tsEvents.SuspendLayout();
            this.tpStarters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStarters)).BeginInit();
            this.tsStarters.SuspendLayout();
            this.tpProblems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProblems)).BeginInit();
            this.ssMain.SuspendLayout();
            this.SuspendLayout();
            //
            // tsMain
            //
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSave,
            this.tsbCheck,
            this.tss1,
            this.tsddNew,
            this.tsbDelete,
            this.tsbUp,
            this.tsbDown,
            this.tss2,
            this.tsddTemplate,
            this.tss3,
            this.tsbCalendar});
            this.tsMain.Name = "tsMain";
            resources.ApplyResources(this.tsMain, "tsMain");
            //
            // tsbSave
            //
            this.tsbSave.Image = global::ToolsCore.GlobalResources.save;
            this.tsbSave.Name = "tsbSave";
            resources.ApplyResources(this.tsbSave, "tsbSave");
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            //
            // tsbCheck
            //
            this.tsbCheck.Image = global::ToolsCore.GlobalResources.analyze;
            this.tsbCheck.Name = "tsbCheck";
            resources.ApplyResources(this.tsbCheck, "tsbCheck");
            this.tsbCheck.Click += new System.EventHandler(this.tsbCheck_Click);
            //
            // tss1
            //
            this.tss1.Name = "tss1";
            //
            // tsddNew
            //
            this.tsddNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewCategory,
            this.tsmiNewState,
            this.tsmiNewDesign,
            this.tsmiNewTimePoint});
            this.tsddNew.Image = global::ToolsCore.GlobalResources.add;
            this.tsddNew.Name = "tsddNew";
            resources.ApplyResources(this.tsddNew, "tsddNew");
            //
            // tsmiNewCategory
            //
            this.tsmiNewCategory.Name = "tsmiNewCategory";
            resources.ApplyResources(this.tsmiNewCategory, "tsmiNewCategory");
            this.tsmiNewCategory.Click += new System.EventHandler(this.tsmiNewCategory_Click);
            //
            // tsmiNewState
            //
            this.tsmiNewState.Name = "tsmiNewState";
            resources.ApplyResources(this.tsmiNewState, "tsmiNewState");
            this.tsmiNewState.Click += new System.EventHandler(this.tsmiNewState_Click);
            //
            // tsmiNewDesign
            //
            this.tsmiNewDesign.Name = "tsmiNewDesign";
            resources.ApplyResources(this.tsmiNewDesign, "tsmiNewDesign");
            this.tsmiNewDesign.Click += new System.EventHandler(this.tsmiNewDesign_Click);
            //
            // tsmiNewTimePoint
            //
            this.tsmiNewTimePoint.Name = "tsmiNewTimePoint";
            resources.ApplyResources(this.tsmiNewTimePoint, "tsmiNewTimePoint");
            this.tsmiNewTimePoint.Click += new System.EventHandler(this.tsmiNewTimePoint_Click);
            //
            // tsbDelete
            //
            this.tsbDelete.Image = global::ToolsCore.GlobalResources.delete;
            this.tsbDelete.Name = "tsbDelete";
            resources.ApplyResources(this.tsbDelete, "tsbDelete");
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            //
            // tsbUp
            //
            this.tsbUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUp.Image = global::ToolsCore.GlobalResources.sort_up;
            this.tsbUp.Name = "tsbUp";
            resources.ApplyResources(this.tsbUp, "tsbUp");
            this.tsbUp.Click += new System.EventHandler(this.tsbUp_Click);
            //
            // tsbDown
            //
            this.tsbDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDown.Image = global::ToolsCore.GlobalResources.sort_down;
            this.tsbDown.Name = "tsbDown";
            resources.ApplyResources(this.tsbDown, "tsbDown");
            this.tsbDown.Click += new System.EventHandler(this.tsbDown_Click);
            //
            // tss2
            //
            this.tss2.Name = "tss2";
            //
            // tsddTemplate
            //
            this.tsddTemplate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTplSK,
            this.tsmiTplCZ,
            this.tsmiTplILTIS});
            this.tsddTemplate.Image = global::ToolsCore.GlobalResources.file;
            this.tsddTemplate.Name = "tsddTemplate";
            resources.ApplyResources(this.tsddTemplate, "tsddTemplate");
            //
            // tsmiTplSK
            //
            this.tsmiTplSK.Name = "tsmiTplSK";
            resources.ApplyResources(this.tsmiTplSK, "tsmiTplSK");
            this.tsmiTplSK.Click += new System.EventHandler(this.tsmiTpl_Click);
            //
            // tsmiTplCZ
            //
            this.tsmiTplCZ.Name = "tsmiTplCZ";
            resources.ApplyResources(this.tsmiTplCZ, "tsmiTplCZ");
            this.tsmiTplCZ.Click += new System.EventHandler(this.tsmiTpl_Click);
            //
            // tsmiTplILTIS
            //
            this.tsmiTplILTIS.Name = "tsmiTplILTIS";
            resources.ApplyResources(this.tsmiTplILTIS, "tsmiTplILTIS");
            this.tsmiTplILTIS.Click += new System.EventHandler(this.tsmiTpl_Click);
            //
            // tss3
            //
            this.tss3.Name = "tss3";
            //
            // tsbCalendar
            //
            this.tsbCalendar.Image = global::ToolsCore.GlobalResources.calendar;
            this.tsbCalendar.Name = "tsbCalendar";
            resources.ApplyResources(this.tsbCalendar, "tsbCalendar");
            this.tsbCalendar.Click += new System.EventHandler(this.tsbCalendar_Click);
            //
            // scOuter
            //
            this.scOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scOuter.Name = "scOuter";
            this.scOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // scOuter.Panel1
            //
            this.scOuter.Panel1.Controls.Add(this.scMain);
            //
            // scOuter.Panel2
            //
            this.scOuter.Panel2.Controls.Add(this.tcBottom);
            this.scOuter.SplitterDistance = 480;
            //
            // scMain
            //
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Name = "scMain";
            //
            // scMain.Panel1
            //
            this.scMain.Panel1.Controls.Add(this.tvNav);
            //
            // scMain.Panel2
            //
            this.scMain.Panel2.Controls.Add(this.scRight);
            this.scMain.SplitterDistance = 260;
            //
            // tvNav
            //
            this.tvNav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvNav.HideSelection = false;
            this.tvNav.Name = "tvNav";
            this.tvNav.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvNav_AfterSelect);
            //
            // scRight
            //
            this.scRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scRight.Name = "scRight";
            //
            // scRight.Panel1
            //
            this.scRight.Panel1.Controls.Add(this.tcCenter);
            //
            // scRight.Panel2
            //
            this.scRight.Panel2.Controls.Add(this.pnlProps);
            this.scRight.SplitterDistance = 480;
            //
            // tcCenter
            //
            this.tcCenter.Controls.Add(this.tpGraph);
            this.tcCenter.Controls.Add(this.tpText);
            this.tcCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcCenter.Name = "tcCenter";
            this.tcCenter.SelectedIndex = 0;
            this.tcCenter.SelectedIndexChanged += new System.EventHandler(this.tcCenter_SelectedIndexChanged);
            //
            // tpGraph
            //
            this.tpGraph.Controls.Add(this.pnlGraph);
            this.tpGraph.Name = "tpGraph";
            this.tpGraph.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.tpGraph, "tpGraph");
            this.tpGraph.UseVisualStyleBackColor = true;
            //
            // pnlGraph
            //
            this.pnlGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGraph.Name = "pnlGraph";
            //
            // tpText
            //
            this.tpText.Controls.Add(this.tbText);
            this.tpText.Name = "tpText";
            this.tpText.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.tpText, "tpText");
            this.tpText.UseVisualStyleBackColor = true;
            //
            // tbText
            //
            this.tbText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbText.Font = new System.Drawing.Font("Consolas", 9F);
            this.tbText.Multiline = true;
            this.tbText.Name = "tbText";
            this.tbText.ReadOnly = true;
            this.tbText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbText.WordWrap = false;
            //
            // pnlProps
            //
            this.pnlProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProps.Name = "pnlProps";
            //
            // tcBottom
            //
            this.tcBottom.Controls.Add(this.tpEvents);
            this.tcBottom.Controls.Add(this.tpStarters);
            this.tcBottom.Controls.Add(this.tpProblems);
            this.tcBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcBottom.Name = "tcBottom";
            this.tcBottom.SelectedIndex = 0;
            //
            // tpEvents
            //
            this.tpEvents.Controls.Add(this.dgvEvents);
            this.tpEvents.Controls.Add(this.tsEvents);
            this.tpEvents.Name = "tpEvents";
            resources.ApplyResources(this.tpEvents, "tpEvents");
            this.tpEvents.UseVisualStyleBackColor = true;
            //
            // dgvEvents
            //
            this.dgvEvents.AllowUserToAddRows = false;
            this.dgvEvents.AllowUserToDeleteRows = false;
            this.dgvEvents.AllowUserToResizeRows = false;
            this.dgvEvents.AutoGenerateColumns = false;
            this.dgvEvents.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvEvents.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEvents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cEvCtrl,
            this.cEvDesign,
            this.cEvKey,
            this.cEvClass,
            this.cEvNext,
            this.cEvReport,
            this.cEvDialog});
            this.dgvEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEvents.MultiSelect = false;
            this.dgvEvents.Name = "dgvEvents";
            this.dgvEvents.ReadOnly = true;
            this.dgvEvents.RowHeadersVisible = false;
            this.dgvEvents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEvents.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEvents_CellDoubleClick);
            this.dgvEvents.SelectionChanged += new System.EventHandler(this.dgvEvents_SelectionChanged);
            //
            // event columns
            //
            this.cEvCtrl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cEvCtrl.DataPropertyName = "CtrlId";
            this.cEvCtrl.Name = "cEvCtrl";
            this.cEvCtrl.ReadOnly = true;
            resources.ApplyResources(this.cEvCtrl, "cEvCtrl");
            this.cEvDesign.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cEvDesign.DataPropertyName = "Design";
            this.cEvDesign.Name = "cEvDesign";
            this.cEvDesign.ReadOnly = true;
            resources.ApplyResources(this.cEvDesign, "cEvDesign");
            this.cEvKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cEvKey.DataPropertyName = "Key";
            this.cEvKey.Name = "cEvKey";
            this.cEvKey.ReadOnly = true;
            resources.ApplyResources(this.cEvKey, "cEvKey");
            this.cEvClass.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cEvClass.DataPropertyName = "Class";
            this.cEvClass.Name = "cEvClass";
            this.cEvClass.ReadOnly = true;
            resources.ApplyResources(this.cEvClass, "cEvClass");
            this.cEvNext.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cEvNext.DataPropertyName = "NextState";
            this.cEvNext.Name = "cEvNext";
            this.cEvNext.ReadOnly = true;
            resources.ApplyResources(this.cEvNext, "cEvNext");
            this.cEvReport.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cEvReport.DataPropertyName = "ReportKey";
            this.cEvReport.Name = "cEvReport";
            this.cEvReport.ReadOnly = true;
            resources.ApplyResources(this.cEvReport, "cEvReport");
            this.cEvDialog.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cEvDialog.DataPropertyName = "Dialog";
            this.cEvDialog.Name = "cEvDialog";
            this.cEvDialog.ReadOnly = true;
            resources.ApplyResources(this.cEvDialog, "cEvDialog");
            //
            // tsEvents
            //
            this.tsEvents.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsEvents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbEvAdd,
            this.tsbEvEdit,
            this.tsbEvDelete,
            this.tsbEvUp,
            this.tsbEvDown});
            this.tsEvents.Name = "tsEvents";
            resources.ApplyResources(this.tsEvents, "tsEvents");
            //
            // tsbEvAdd
            //
            this.tsbEvAdd.Image = global::ToolsCore.GlobalResources.add;
            this.tsbEvAdd.Name = "tsbEvAdd";
            resources.ApplyResources(this.tsbEvAdd, "tsbEvAdd");
            this.tsbEvAdd.Click += new System.EventHandler(this.tsbEvAdd_Click);
            //
            // tsbEvEdit
            //
            this.tsbEvEdit.Image = global::ToolsCore.GlobalResources.edit;
            this.tsbEvEdit.Name = "tsbEvEdit";
            resources.ApplyResources(this.tsbEvEdit, "tsbEvEdit");
            this.tsbEvEdit.Click += new System.EventHandler(this.tsbEvEdit_Click);
            //
            // tsbEvDelete
            //
            this.tsbEvDelete.Image = global::ToolsCore.GlobalResources.delete;
            this.tsbEvDelete.Name = "tsbEvDelete";
            resources.ApplyResources(this.tsbEvDelete, "tsbEvDelete");
            this.tsbEvDelete.Click += new System.EventHandler(this.tsbEvDelete_Click);
            //
            // tsbEvUp
            //
            this.tsbEvUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEvUp.Image = global::ToolsCore.GlobalResources.sort_up;
            this.tsbEvUp.Name = "tsbEvUp";
            resources.ApplyResources(this.tsbEvUp, "tsbEvUp");
            this.tsbEvUp.Click += new System.EventHandler(this.tsbEvUp_Click);
            //
            // tsbEvDown
            //
            this.tsbEvDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEvDown.Image = global::ToolsCore.GlobalResources.sort_down;
            this.tsbEvDown.Name = "tsbEvDown";
            resources.ApplyResources(this.tsbEvDown, "tsbEvDown");
            this.tsbEvDown.Click += new System.EventHandler(this.tsbEvDown_Click);
            //
            // tpStarters
            //
            this.tpStarters.Controls.Add(this.dgvStarters);
            this.tpStarters.Controls.Add(this.tsStarters);
            this.tpStarters.Name = "tpStarters";
            resources.ApplyResources(this.tpStarters, "tpStarters");
            this.tpStarters.UseVisualStyleBackColor = true;
            //
            // dgvStarters
            //
            this.dgvStarters.AllowUserToAddRows = false;
            this.dgvStarters.AllowUserToDeleteRows = false;
            this.dgvStarters.AllowUserToResizeRows = false;
            this.dgvStarters.AutoGenerateColumns = false;
            this.dgvStarters.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvStarters.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStarters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStarters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cStKey,
            this.cStEvent,
            this.cStText});
            this.dgvStarters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStarters.MultiSelect = false;
            this.dgvStarters.Name = "dgvStarters";
            this.dgvStarters.ReadOnly = true;
            this.dgvStarters.RowHeadersVisible = false;
            this.dgvStarters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStarters.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStarters_CellDoubleClick);
            this.dgvStarters.SelectionChanged += new System.EventHandler(this.dgvStarters_SelectionChanged);
            //
            // starter columns
            //
            this.cStKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cStKey.DataPropertyName = "Key";
            this.cStKey.Name = "cStKey";
            this.cStKey.ReadOnly = true;
            resources.ApplyResources(this.cStKey, "cStKey");
            this.cStEvent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cStEvent.DataPropertyName = "EventKey";
            this.cStEvent.Name = "cStEvent";
            this.cStEvent.ReadOnly = true;
            resources.ApplyResources(this.cStEvent, "cStEvent");
            this.cStText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cStText.DataPropertyName = "Text";
            this.cStText.Name = "cStText";
            this.cStText.ReadOnly = true;
            resources.ApplyResources(this.cStText, "cStText");
            //
            // tsStarters
            //
            this.tsStarters.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsStarters.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbStAdd,
            this.tsbStEdit,
            this.tsbStDelete});
            this.tsStarters.Name = "tsStarters";
            resources.ApplyResources(this.tsStarters, "tsStarters");
            //
            // tsbStAdd
            //
            this.tsbStAdd.Image = global::ToolsCore.GlobalResources.add;
            this.tsbStAdd.Name = "tsbStAdd";
            resources.ApplyResources(this.tsbStAdd, "tsbStAdd");
            this.tsbStAdd.Click += new System.EventHandler(this.tsbStAdd_Click);
            //
            // tsbStEdit
            //
            this.tsbStEdit.Image = global::ToolsCore.GlobalResources.edit;
            this.tsbStEdit.Name = "tsbStEdit";
            resources.ApplyResources(this.tsbStEdit, "tsbStEdit");
            this.tsbStEdit.Click += new System.EventHandler(this.tsbStEdit_Click);
            //
            // tsbStDelete
            //
            this.tsbStDelete.Image = global::ToolsCore.GlobalResources.delete;
            this.tsbStDelete.Name = "tsbStDelete";
            resources.ApplyResources(this.tsbStDelete, "tsbStDelete");
            this.tsbStDelete.Click += new System.EventHandler(this.tsbStDelete_Click);
            //
            // tpProblems
            //
            this.tpProblems.Controls.Add(this.dgvProblems);
            this.tpProblems.Name = "tpProblems";
            resources.ApplyResources(this.tpProblems, "tpProblems");
            this.tpProblems.UseVisualStyleBackColor = true;
            //
            // dgvProblems
            //
            this.dgvProblems.AllowUserToAddRows = false;
            this.dgvProblems.AllowUserToDeleteRows = false;
            this.dgvProblems.AllowUserToResizeRows = false;
            this.dgvProblems.AutoGenerateColumns = false;
            this.dgvProblems.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvProblems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProblems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProblems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cProbType,
            this.cProbCode,
            this.cProbPath,
            this.cProbMessage});
            this.dgvProblems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProblems.MultiSelect = false;
            this.dgvProblems.Name = "dgvProblems";
            this.dgvProblems.ReadOnly = true;
            this.dgvProblems.RowHeadersVisible = false;
            this.dgvProblems.RowTemplate.Height = 22;
            this.dgvProblems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProblems.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProblems_CellDoubleClick);
            this.dgvProblems.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvProblems_CellFormatting);
            //
            // problem columns
            //
            this.cProbType.DataPropertyName = "Severity";
            this.cProbType.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.cProbType.Name = "cProbType";
            this.cProbType.ReadOnly = true;
            this.cProbType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cProbType.Width = 40;
            resources.ApplyResources(this.cProbType, "cProbType");
            this.cProbCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cProbCode.DataPropertyName = "Code";
            this.cProbCode.Name = "cProbCode";
            this.cProbCode.ReadOnly = true;
            resources.ApplyResources(this.cProbCode, "cProbCode");
            this.cProbPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cProbPath.DataPropertyName = "Path";
            this.cProbPath.Name = "cProbPath";
            this.cProbPath.ReadOnly = true;
            resources.ApplyResources(this.cProbPath, "cProbPath");
            this.cProbMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cProbMessage.DataPropertyName = "Message";
            this.cProbMessage.Name = "cProbMessage";
            this.cProbMessage.ReadOnly = true;
            resources.ApplyResources(this.cProbMessage, "cProbMessage");
            //
            // ssMain
            //
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslStatus});
            this.ssMain.Name = "ssMain";
            resources.ApplyResources(this.ssMain, "ssMain");
            //
            // tsslStatus
            //
            this.tsslStatus.Name = "tsslStatus";
            this.tsslStatus.Spring = true;
            this.tsslStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // FStateDgm
            //
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scOuter);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.KeyPreview = true;
            this.Name = "FStateDgm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FStateDgm_FormClosing);
            this.Load += new System.EventHandler(this.FStateDgm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FStateDgm_KeyDown);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.scOuter.Panel1.ResumeLayout(false);
            this.scOuter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scOuter)).EndInit();
            this.scOuter.ResumeLayout(false);
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.scRight.Panel1.ResumeLayout(false);
            this.scRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scRight)).EndInit();
            this.scRight.ResumeLayout(false);
            this.tcCenter.ResumeLayout(false);
            this.tpGraph.ResumeLayout(false);
            this.tpText.ResumeLayout(false);
            this.tpText.PerformLayout();
            this.tcBottom.ResumeLayout(false);
            this.tpEvents.ResumeLayout(false);
            this.tpEvents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).EndInit();
            this.tsEvents.ResumeLayout(false);
            this.tsEvents.PerformLayout();
            this.tpStarters.ResumeLayout(false);
            this.tpStarters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStarters)).EndInit();
            this.tsStarters.ResumeLayout(false);
            this.tsStarters.PerformLayout();
            this.tpProblems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProblems)).EndInit();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
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
        private ExControls.ExTextBox tbText;
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
