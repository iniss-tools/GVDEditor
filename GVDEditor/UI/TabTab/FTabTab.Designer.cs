using GVDEditor.UI.Controls;

namespace GVDEditor.UI.TabTab
{
    partial class FTabTab
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTabTab));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbTabTabs = new System.Windows.Forms.ListBox();
            this.conMenuLbTabs = new System.Windows.Forms.ContextMenuStrip();
            this.tsmiAddTabTab = new ToolsCore.Icons.IconToolStripMenuItem();
            this.tsmiDeleteTabTab = new ToolsCore.Icons.IconToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiRenameTabTab = new ToolsCore.Icons.IconToolStripMenuItem();
            this.conMenuScText = new System.Windows.Forms.ContextMenuStrip();
            this.tsmiSave = new ToolsCore.Icons.IconToolStripMenuItem();
            this.tsmiSaveAll = new ToolsCore.Icons.IconToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiUndo = new ToolsCore.Icons.IconToolStripMenuItem();
            this.tsmiRedo = new ToolsCore.Icons.IconToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiCut = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbSave = new ToolsCore.Icons.IconToolStripButton();
            this.tsbSaveAll = new ToolsCore.Icons.IconToolStripButton();
            this.tsbStorno = new ToolsCore.Icons.IconToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbUndo = new ToolsCore.Icons.IconToolStripButton();
            this.tsbRedo = new ToolsCore.Icons.IconToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAddTab = new ToolsCore.Icons.IconToolStripButton();
            this.tsbRemoveTab = new ToolsCore.Icons.IconToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRename = new ToolsCore.Icons.IconToolStripButton();
            this.tsbFindReplace = new ToolsCore.Icons.IconToolStripButton();
            this.tsbReformat = new ToolsCore.Icons.IconToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPreview = new ToolsCore.Icons.IconToolStripButton();
            this.acMenu = new AutocompleteMenuNS.AutocompleteMenu();
            this.acImages = new System.Windows.Forms.ImageList();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslTabTabName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslLenText = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslLen = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslLinesText = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslLines = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslRow = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslCol = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslPosText = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.scText = new GVDEditor.UI.Controls.MyScintilla();
            this.pProblems = new System.Windows.Forms.Panel();
            this.dgvProblems = new System.Windows.Forms.DataGridView();
            this.cProbType = new System.Windows.Forms.DataGridViewImageColumn();
            this.cProbCode = new System.Windows.Forms.DataGridViewLinkColumn();
            this.cProbLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cProbMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cProbSolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsProblems = new System.Windows.Forms.ToolStrip();
            this.tsbProbErrors = new ToolsCore.Icons.IconToolStripButton();
            this.tsbProbWarnings = new ToolsCore.Icons.IconToolStripButton();
            this.tsbProbInfos = new ToolsCore.Icons.IconToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbProbGoTo = new ToolsCore.Icons.IconToolStripButton();
            this.tsbProbFix = new ToolsCore.Icons.IconToolStripButton();
            this.conMenuProblems = new System.Windows.Forms.ContextMenuStrip();
            this.tsmiProbGoTo = new ToolsCore.Icons.IconToolStripMenuItem();
            this.tsmiProbFix = new ToolsCore.Icons.IconToolStripMenuItem();
            this.splitterProblems = new System.Windows.Forms.Splitter();
            this.tsslProblems = new ToolsCore.Icons.IconToolStripStatusLabel();
            this.toolStripContainer1.SuspendLayout();
            this.pProblems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProblems)).BeginInit();
            this.tsProblems.SuspendLayout();
            this.conMenuProblems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.conMenuLbTabs.SuspendLayout();
            this.conMenuScText.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            resources.ApplyResources(this.toolStripContainer1.ContentPanel, "toolStripContainer1.ContentPanel");
            resources.ApplyResources(this.toolStripContainer1, "toolStripContainer1");
            this.toolStripContainer1.Name = "toolStripContainer1";
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.scText);
            this.splitContainer1.Panel1.Controls.Add(this.splitterProblems);
            this.splitContainer1.Panel1.Controls.Add(this.pProblems);
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lbTabTabs);
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            // 
            // lbTabTabs
            // 
            this.lbTabTabs.ContextMenuStrip = this.conMenuLbTabs;
            resources.ApplyResources(this.lbTabTabs, "lbTabTabs");
            this.lbTabTabs.FormattingEnabled = true;
            this.lbTabTabs.Name = "lbTabTabs";
            this.lbTabTabs.SelectedIndexChanged += new System.EventHandler(this.lbTabTabs_SelectedIndexChanged);
            // 
            // conMenuLbTabs
            // 
            this.conMenuLbTabs.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.conMenuLbTabs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAddTabTab,
            this.tsmiDeleteTabTab,
            this.toolStripSeparator8,
            this.tsmiRenameTabTab});
            this.conMenuLbTabs.Name = "conMenuLbTabs";
            resources.ApplyResources(this.conMenuLbTabs, "conMenuLbTabs");
            // 
            // tsmiAddTabTab
            // 
            resources.ApplyResources(this.tsmiAddTabTab, "tsmiAddTabTab");
            this.tsmiAddTabTab.Name = "tsmiAddTabTab";
            this.tsmiAddTabTab.IconName = "add";
            // 
            // tsmiDeleteTabTab
            // 
            resources.ApplyResources(this.tsmiDeleteTabTab, "tsmiDeleteTabTab");
            this.tsmiDeleteTabTab.Name = "tsmiDeleteTabTab";
            this.tsmiDeleteTabTab.IconName = "delete";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // tsmiRenameTabTab
            // 
            resources.ApplyResources(this.tsmiRenameTabTab, "tsmiRenameTabTab");
            this.tsmiRenameTabTab.Name = "tsmiRenameTabTab";
            this.tsmiRenameTabTab.IconName = "rename";
            // 
            // conMenuScText
            // 
            this.conMenuScText.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.conMenuScText.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSave,
            this.tsmiSaveAll,
            this.toolStripSeparator7,
            this.tsmiUndo,
            this.tsmiRedo,
            this.toolStripSeparator5,
            this.tsmiCut,
            this.tsmiCopy,
            this.tsmiPaste,
            this.tsmiDelete,
            this.toolStripSeparator6,
            this.tsmiSelectAll});
            this.conMenuScText.Name = "contextMenuStrip1";
            this.conMenuScText.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            resources.ApplyResources(this.conMenuScText, "conMenuScText");
            // 
            // tsmiSave
            // 
            resources.ApplyResources(this.tsmiSave, "tsmiSave");
            this.tsmiSave.Name = "tsmiSave";
            this.tsmiSave.IconName = "save";
            // 
            // tsmiSaveAll
            // 
            resources.ApplyResources(this.tsmiSaveAll, "tsmiSaveAll");
            this.tsmiSaveAll.Name = "tsmiSaveAll";
            this.tsmiSaveAll.IconName = "save-all";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // tsmiUndo
            // 
            resources.ApplyResources(this.tsmiUndo, "tsmiUndo");
            this.tsmiUndo.Name = "tsmiUndo";
            this.tsmiUndo.IconName = "undo";
            // 
            // tsmiRedo
            // 
            resources.ApplyResources(this.tsmiRedo, "tsmiRedo");
            this.tsmiRedo.Name = "tsmiRedo";
            this.tsmiRedo.IconName = "redo";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // tsmiCut
            // 
            this.tsmiCut.Name = "tsmiCut";
            resources.ApplyResources(this.tsmiCut, "tsmiCut");
            this.tsmiCut.Click += new System.EventHandler(this.tsmiCut_Click);
            // 
            // tsmiCopy
            // 
            this.tsmiCopy.Name = "tsmiCopy";
            resources.ApplyResources(this.tsmiCopy, "tsmiCopy");
            this.tsmiCopy.Click += new System.EventHandler(this.tsmiCopy_Click);
            // 
            // tsmiPaste
            // 
            this.tsmiPaste.Name = "tsmiPaste";
            resources.ApplyResources(this.tsmiPaste, "tsmiPaste");
            this.tsmiPaste.Click += new System.EventHandler(this.tsmiPaste_Click);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            resources.ApplyResources(this.tsmiDelete, "tsmiDelete");
            this.tsmiDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // tsmiSelectAll
            // 
            this.tsmiSelectAll.Name = "tsmiSelectAll";
            resources.ApplyResources(this.tsmiSelectAll, "tsmiSelectAll");
            this.tsmiSelectAll.Click += new System.EventHandler(this.tsmiSelectAll_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSave,
            this.tsbSaveAll,
            this.tsbStorno,
            this.toolStripSeparator1,
            this.tsbUndo,
            this.tsbRedo,
            this.toolStripSeparator2,
            this.tsbAddTab,
            this.tsbRemoveTab,
            this.toolStripSeparator3,
            this.tsbRename,
            this.tsbFindReplace,
            this.tsbReformat,
            this.toolStripSeparator10,
            this.tsbPreview});
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Name = "toolStrip";
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbSave, "tsbSave");
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.IconName = "save";
            // 
            // tsbSaveAll
            // 
            this.tsbSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbSaveAll, "tsbSaveAll");
            this.tsbSaveAll.Name = "tsbSaveAll";
            this.tsbSaveAll.IconName = "save-all";
            // 
            // tsbStorno
            // 
            this.tsbStorno.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbStorno, "tsbStorno");
            this.tsbStorno.Name = "tsbStorno";
            this.tsbStorno.IconName = "exit";
            this.tsbStorno.Click += new System.EventHandler(this.tsbStorno_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tsbUndo
            // 
            this.tsbUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbUndo, "tsbUndo");
            this.tsbUndo.Name = "tsbUndo";
            this.tsbUndo.IconName = "undo";
            // 
            // tsbRedo
            // 
            this.tsbRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbRedo, "tsbRedo");
            this.tsbRedo.Name = "tsbRedo";
            this.tsbRedo.IconName = "redo";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // tsbAddTab
            // 
            this.tsbAddTab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbAddTab, "tsbAddTab");
            this.tsbAddTab.Name = "tsbAddTab";
            this.tsbAddTab.IconName = "add";
            // 
            // tsbRemoveTab
            // 
            this.tsbRemoveTab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbRemoveTab, "tsbRemoveTab");
            this.tsbRemoveTab.Name = "tsbRemoveTab";
            this.tsbRemoveTab.IconName = "delete";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // tsbRename
            // 
            this.tsbRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbRename, "tsbRename");
            this.tsbRename.Name = "tsbRename";
            this.tsbRename.IconName = "rename";
            // 
            // tsbFindReplace
            // 
            this.tsbFindReplace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbFindReplace, "tsbFindReplace");
            this.tsbFindReplace.Name = "tsbFindReplace";
            this.tsbFindReplace.IconName = "search";
            // 
            // tsbReformat
            // 
            this.tsbReformat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbReformat, "tsbReformat");
            this.tsbReformat.Name = "tsbReformat";
            this.tsbReformat.IconName = "text-indent";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            // 
            // tsbPreview
            // 
            this.tsbPreview.Name = "tsbPreview";
            this.tsbPreview.IconName = "eye";
            this.tsbPreview.Click += new System.EventHandler(this.tsbPreview_Click);
            // 
            // acMenu
            // 
            this.acMenu.AppearInterval = 100;
            this.acMenu.Colors = ((AutocompleteMenuNS.Colors)(resources.GetObject("acMenu.Colors")));
            this.acMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.acMenu.ImageList = this.acImages;
            this.acMenu.Items = new string[0];
            this.acMenu.MaximumSize = new System.Drawing.Size(250, 200);
            this.acMenu.MinFragmentLength = 1;
            this.acMenu.SearchPattern = "[\\w\\.\\#]";
            this.acMenu.TargetControlWrapper = null;
            this.acMenu.ToolTipDuration = 300000;
            // 
            // acImages
            // 
            this.acImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.acImages.ImageSize = new System.Drawing.Size(16, 16);
            this.acImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslTabTabName,
            this.tsslProblems,
            this.toolStripStatusLabel3,
            this.tsslLenText,
            this.tsslLen,
            this.tsslLinesText,
            this.tsslLines,
            this.toolStripStatusLabel1,
            this.tsslRow,
            this.toolStripStatusLabel2,
            this.tsslCol,
            this.tsslPosText,
            this.tsslPos});
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.SizingGrip = false;
            // 
            // tsslTabTabName
            // 
            this.tsslTabTabName.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslTabTabName.Name = "tsslTabTabName";
            resources.ApplyResources(this.tsslTabTabName, "tsslTabTabName");
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            this.toolStripStatusLabel3.Spring = true;
            // 
            // tsslLenText
            // 
            this.tsslLenText.Name = "tsslLenText";
            resources.ApplyResources(this.tsslLenText, "tsslLenText");
            // 
            // tsslLen
            // 
            this.tsslLen.Margin = new System.Windows.Forms.Padding(0, 4, 10, 2);
            this.tsslLen.Name = "tsslLen";
            resources.ApplyResources(this.tsslLen, "tsslLen");
            // 
            // tsslLinesText
            // 
            this.tsslLinesText.Name = "tsslLinesText";
            resources.ApplyResources(this.tsslLinesText, "tsslLinesText");
            // 
            // tsslLines
            // 
            this.tsslLines.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslLines.Name = "tsslLines";
            resources.ApplyResources(this.tsslLines, "tsslLines");
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            // 
            // tsslRow
            // 
            this.tsslRow.Margin = new System.Windows.Forms.Padding(0, 4, 10, 2);
            this.tsslRow.Name = "tsslRow";
            resources.ApplyResources(this.tsslRow, "tsslRow");
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            // 
            // tsslCol
            // 
            this.tsslCol.Margin = new System.Windows.Forms.Padding(0, 4, 10, 2);
            this.tsslCol.Name = "tsslCol";
            resources.ApplyResources(this.tsslCol, "tsslCol");
            // 
            // tsslPosText
            // 
            this.tsslPosText.Name = "tsslPosText";
            resources.ApplyResources(this.tsslPosText, "tsslPosText");
            // 
            // tsslPos
            // 
            this.tsslPos.Name = "tsslPos";
            resources.ApplyResources(this.tsslPos, "tsslPos");
            // 
            // tsslProblems
            // 
            this.tsslProblems.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslProblems.Name = "tsslProblems";
            this.tsslProblems.Size = new System.Drawing.Size(4, 17);
            //
            // pProblems
            //
            this.pProblems.Controls.Add(this.dgvProblems);
            this.pProblems.Controls.Add(this.tsProblems);
            this.pProblems.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pProblems.Name = "pProblems";
            this.pProblems.Size = new System.Drawing.Size(600, 150);
            this.pProblems.TabIndex = 2;
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
            this.cProbLine,
            this.cProbMessage,
            this.cProbSolution});
            this.dgvProblems.ContextMenuStrip = this.conMenuProblems;
            this.dgvProblems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProblems.MultiSelect = false;
            this.dgvProblems.Name = "dgvProblems";
            this.dgvProblems.ReadOnly = true;
            this.dgvProblems.RowHeadersVisible = false;
            this.dgvProblems.RowTemplate.Height = 22;
            this.dgvProblems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProblems.TabIndex = 1;
            this.dgvProblems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProblems_CellContentClick);
            this.dgvProblems.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProblems_CellDoubleClick);
            this.dgvProblems.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvProblems_CellFormatting);
            this.dgvProblems.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProblems_CellMouseDown);
            this.dgvProblems.SelectionChanged += new System.EventHandler(this.dgvProblems_SelectionChanged);
            this.dgvProblems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvProblems_KeyDown);
            //
            // tsProblems
            //
            this.tsProblems.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsProblems.ImageScalingSize = new System.Drawing.Size(16, 16);
            this.tsProblems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbProbErrors,
            this.tsbProbWarnings,
            this.tsbProbInfos,
            this.toolStripSeparator9,
            this.tsbProbGoTo,
            this.tsbProbFix});
            this.tsProblems.Name = "tsProblems";
            this.tsProblems.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            //
            // tsbProbErrors
            //
            this.tsbProbErrors.Checked = true;
            this.tsbProbErrors.CheckOnClick = true;
            this.tsbProbErrors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbProbErrors.Name = "tsbProbErrors";
            this.tsbProbErrors.IconName = "status-error";
            this.tsbProbErrors.CheckedChanged += new System.EventHandler(this.tsbProbFilter_CheckedChanged);
            //
            // tsbProbWarnings
            //
            this.tsbProbWarnings.Checked = true;
            this.tsbProbWarnings.CheckOnClick = true;
            this.tsbProbWarnings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbProbWarnings.Name = "tsbProbWarnings";
            this.tsbProbWarnings.IconName = "status-warning";
            this.tsbProbWarnings.CheckedChanged += new System.EventHandler(this.tsbProbFilter_CheckedChanged);
            //
            // tsbProbInfos
            //
            this.tsbProbInfos.Checked = true;
            this.tsbProbInfos.CheckOnClick = true;
            this.tsbProbInfos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbProbInfos.Name = "tsbProbInfos";
            this.tsbProbInfos.IconName = "status-info";
            this.tsbProbInfos.CheckedChanged += new System.EventHandler(this.tsbProbFilter_CheckedChanged);
            //
            // toolStripSeparator9
            //
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            //
            // tsbProbGoTo
            //
            this.tsbProbGoTo.Name = "tsbProbGoTo";
            this.tsbProbGoTo.IconName = "search";
            this.tsbProbGoTo.Click += new System.EventHandler(this.tsbProbGoTo_Click);
            //
            // tsbProbFix
            //
            this.tsbProbFix.Name = "tsbProbFix";
            this.tsbProbFix.IconName = "wrench";
            this.tsbProbFix.Click += new System.EventHandler(this.tsbProbFix_Click);
            //
            // conMenuProblems
            //
            this.conMenuProblems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiProbGoTo,
            this.tsmiProbFix});
            this.conMenuProblems.Name = "conMenuProblems";
            //
            // tsmiProbGoTo
            //
            this.tsmiProbGoTo.Name = "tsmiProbGoTo";
            this.tsmiProbGoTo.IconName = "search";
            this.tsmiProbGoTo.Click += new System.EventHandler(this.tsbProbGoTo_Click);
            //
            // tsmiProbFix
            //
            this.tsmiProbFix.Name = "tsmiProbFix";
            this.tsmiProbFix.IconName = "wrench";
            this.tsmiProbFix.Click += new System.EventHandler(this.tsbProbFix_Click);
            // 
            // splitterProblems
            // 
            this.splitterProblems.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterProblems.Name = "splitterProblems";
            this.splitterProblems.Size = new System.Drawing.Size(600, 4);
            this.splitterProblems.TabIndex = 3;
            this.splitterProblems.TabStop = false;
            // 
            // scText
            // 
            resources.ApplyResources(this.scText, "scText");
            this.scText.Name = "scText";
            this.scText.CharAdded += new System.EventHandler<ScintillaNET.CharAddedEventArgs>(this.scText_CharAdded);
            this.scText.StyleNeeded += new System.EventHandler<ScintillaNET.StyleNeededEventArgs>(this.scText_StyleNeeded);
            this.scText.UpdateUI += new System.EventHandler<ScintillaNET.UpdateUIEventArgs>(this.scText_UpdateUI);
            this.scText.TextChanged += new System.EventHandler(this.scText_TextChanged);
            this.scText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.scText_KeyPress);
            this.scText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scText_MouseDown);
            //
            // cProbType
            //
            this.cProbType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cProbType.DataPropertyName = "Severity";
            this.cProbType.Name = "cProbType";
            this.cProbType.ReadOnly = true;
            this.cProbType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cProbType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cProbType.Width = 50;
            resources.ApplyResources(this.cProbType, "cProbType");
            //
            // cProbCode
            //
            this.cProbCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cProbCode.DataPropertyName = "Code";
            this.cProbCode.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.cProbCode.Name = "cProbCode";
            this.cProbCode.ReadOnly = true;
            this.cProbCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cProbCode.TrackVisitedState = false;
            resources.ApplyResources(this.cProbCode, "cProbCode");
            //
            // cProbLine
            //
            this.cProbLine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cProbLine.DataPropertyName = "Line";
            this.cProbLine.Name = "cProbLine";
            this.cProbLine.ReadOnly = true;
            resources.ApplyResources(this.cProbLine, "cProbLine");
            //
            // cProbMessage
            //
            this.cProbMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cProbMessage.DataPropertyName = "Message";
            this.cProbMessage.FillWeight = 60F;
            this.cProbMessage.MinimumWidth = 300;
            this.cProbMessage.Name = "cProbMessage";
            this.cProbMessage.ReadOnly = true;
            resources.ApplyResources(this.cProbMessage, "cProbMessage");
            //
            // cProbSolution
            //
            this.cProbSolution.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cProbSolution.DataPropertyName = "Solution";
            this.cProbSolution.FillWeight = 40F;
            this.cProbSolution.MinimumWidth = 200;
            this.cProbSolution.Name = "cProbSolution";
            this.cProbSolution.ReadOnly = true;
            resources.ApplyResources(this.cProbSolution, "cProbSolution");
            //
            // FTabTab
            //
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.KeyPreview = true;
            this.Name = "FTabTab";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FTabTab_FormClosing);
            this.Load += new System.EventHandler(this.FTabTab_Load);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.pProblems.ResumeLayout(false);
            this.pProblems.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProblems)).EndInit();
            this.tsProblems.ResumeLayout(false);
            this.tsProblems.PerformLayout();
            this.conMenuProblems.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.conMenuLbTabs.ResumeLayout(false);
            this.conMenuScText.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MyScintilla scText;
        private System.Windows.Forms.ToolStrip toolStrip;
        private ToolsCore.Icons.IconToolStripButton tsbSave;
        private ToolsCore.Icons.IconToolStripButton tsbStorno;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private ToolsCore.Icons.IconToolStripButton tsbUndo;
        private ToolsCore.Icons.IconToolStripButton tsbRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private AutocompleteMenuNS.AutocompleteMenu acMenu;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lbTabTabs;
        private System.Windows.Forms.ImageList acImages;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private ToolsCore.Icons.IconToolStripButton tsbAddTab;
        private ToolsCore.Icons.IconToolStripButton tsbRemoveTab;
        private ToolsCore.Icons.IconToolStripButton tsbSaveAll;
        private ToolsCore.Icons.IconToolStripButton tsbReformat;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private ToolsCore.Icons.IconToolStripButton tsbPreview;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private ToolsCore.Icons.IconToolStripButton tsbRename;
        private ToolsCore.Icons.IconToolStripButton tsbFindReplace;
        private System.Windows.Forms.ContextMenuStrip conMenuScText;
        private ToolsCore.Icons.IconToolStripMenuItem tsmiUndo;
        private ToolsCore.Icons.IconToolStripMenuItem tsmiSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private ToolsCore.Icons.IconToolStripMenuItem tsmiRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsmiCut;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiPaste;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectAll;
        private System.Windows.Forms.ContextMenuStrip conMenuLbTabs;
        private ToolsCore.Icons.IconToolStripMenuItem tsmiAddTabTab;
        private ToolsCore.Icons.IconToolStripMenuItem tsmiDeleteTabTab;
        private ToolsCore.Icons.IconToolStripMenuItem tsmiRenameTabTab;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private ToolsCore.Icons.IconToolStripMenuItem tsmiSaveAll;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsslTabTabName;
        private System.Windows.Forms.ToolStripStatusLabel tsslLenText;
        private System.Windows.Forms.ToolStripStatusLabel tsslLen;
        private System.Windows.Forms.ToolStripStatusLabel tsslLinesText;
        private System.Windows.Forms.ToolStripStatusLabel tsslLines;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsslRow;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tsslCol;
        private System.Windows.Forms.ToolStripStatusLabel tsslPosText;
        private System.Windows.Forms.ToolStripStatusLabel tsslPos;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.Panel pProblems;
        private System.Windows.Forms.DataGridView dgvProblems;
        private System.Windows.Forms.DataGridViewImageColumn cProbType;
        private System.Windows.Forms.DataGridViewLinkColumn cProbCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProbLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProbMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProbSolution;
        private System.Windows.Forms.ToolStrip tsProblems;
        private ToolsCore.Icons.IconToolStripButton tsbProbErrors;
        private ToolsCore.Icons.IconToolStripButton tsbProbWarnings;
        private ToolsCore.Icons.IconToolStripButton tsbProbInfos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private ToolsCore.Icons.IconToolStripButton tsbProbGoTo;
        private ToolsCore.Icons.IconToolStripButton tsbProbFix;
        private System.Windows.Forms.ContextMenuStrip conMenuProblems;
        private ToolsCore.Icons.IconToolStripMenuItem tsmiProbGoTo;
        private ToolsCore.Icons.IconToolStripMenuItem tsmiProbFix;
        private System.Windows.Forms.Splitter splitterProblems;
        private ToolsCore.Icons.IconToolStripStatusLabel tsslProblems;
    }
}