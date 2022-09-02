using GVDEditor.Controls;
using GVDEditor.Tools;

namespace GVDEditor.Forms
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTabTab));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scText = new GVDEditor.Controls.MyScintilla();
            this.lbTabTabs = new System.Windows.Forms.ListBox();
            this.conMenuLbTabs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAddTabTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeleteTabTab = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiRenameTabTab = new System.Windows.Forms.ToolStripMenuItem();
            this.conMenuScText = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiCut = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveAll = new System.Windows.Forms.ToolStripButton();
            this.tsbStorno = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbUndo = new System.Windows.Forms.ToolStripButton();
            this.tsbRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAddTab = new System.Windows.Forms.ToolStripButton();
            this.tsbRemoveTab = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRename = new System.Windows.Forms.ToolStripButton();
            this.tsbFindReplace = new System.Windows.Forms.ToolStripButton();
            this.tsbReformat = new System.Windows.Forms.ToolStripButton();
            this.acMenu = new AutocompleteMenuNS.AutocompleteMenu();
            this.acImages = new System.Windows.Forms.ImageList(this.components);
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
            this.toolStripContainer1.SuspendLayout();
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
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lbTabTabs);
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
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
            this.tsmiAddTabTab.Click += new System.EventHandler(this.tsmiAddTabTab_Click);
            // 
            // tsmiDeleteTabTab
            // 
            resources.ApplyResources(this.tsmiDeleteTabTab, "tsmiDeleteTabTab");
            this.tsmiDeleteTabTab.Name = "tsmiDeleteTabTab";
            this.tsmiDeleteTabTab.Click += new System.EventHandler(this.tsmiDeleteTabTab_Click);
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
            this.tsmiRenameTabTab.Click += new System.EventHandler(this.tsmiRenameTabTab_Click);
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
            this.tsmiSave.Click += new System.EventHandler(this.tsmiSave_Click);
            // 
            // tsmiSaveAll
            // 
            resources.ApplyResources(this.tsmiSaveAll, "tsmiSaveAll");
            this.tsmiSaveAll.Name = "tsmiSaveAll";
            this.tsmiSaveAll.Click += new System.EventHandler(this.tsmiSaveAll_Click);
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
            this.tsmiUndo.Click += new System.EventHandler(this.tsmiUndo_Click);
            // 
            // tsmiRedo
            // 
            resources.ApplyResources(this.tsmiRedo, "tsmiRedo");
            this.tsmiRedo.Name = "tsmiRedo";
            this.tsmiRedo.Click += new System.EventHandler(this.tsmiRedo_Click);
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
            this.tsbReformat});
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Name = "toolStrip";
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = global::ToolsCore.GlobalResources.save;
            resources.ApplyResources(this.tsbSave, "tsbSave");
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbSaveAll
            // 
            this.tsbSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSaveAll.Image = global::ToolsCore.GlobalResources.save_all;
            resources.ApplyResources(this.tsbSaveAll, "tsbSaveAll");
            this.tsbSaveAll.Name = "tsbSaveAll";
            this.tsbSaveAll.Click += new System.EventHandler(this.tsbSaveAll_Click);
            // 
            // tsbStorno
            // 
            this.tsbStorno.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStorno.Image = global::ToolsCore.GlobalResources.export;
            resources.ApplyResources(this.tsbStorno, "tsbStorno");
            this.tsbStorno.Name = "tsbStorno";
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
            this.tsbUndo.Image = global::ToolsCore.GlobalResources.undo;
            resources.ApplyResources(this.tsbUndo, "tsbUndo");
            this.tsbUndo.Name = "tsbUndo";
            this.tsbUndo.Click += new System.EventHandler(this.tsbUndo_Click);
            // 
            // tsbRedo
            // 
            this.tsbRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRedo.Image = global::ToolsCore.GlobalResources.redo;
            resources.ApplyResources(this.tsbRedo, "tsbRedo");
            this.tsbRedo.Name = "tsbRedo";
            this.tsbRedo.Click += new System.EventHandler(this.tsbRedo_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // tsbAddTab
            // 
            this.tsbAddTab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddTab.Image = global::ToolsCore.GlobalResources.add;
            resources.ApplyResources(this.tsbAddTab, "tsbAddTab");
            this.tsbAddTab.Name = "tsbAddTab";
            this.tsbAddTab.Click += new System.EventHandler(this.tsbAddTab_Click);
            // 
            // tsbRemoveTab
            // 
            this.tsbRemoveTab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRemoveTab.Image = global::ToolsCore.GlobalResources.delete;
            resources.ApplyResources(this.tsbRemoveTab, "tsbRemoveTab");
            this.tsbRemoveTab.Name = "tsbRemoveTab";
            this.tsbRemoveTab.Click += new System.EventHandler(this.tsbRemoveTab_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // tsbRename
            // 
            this.tsbRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRename.Image = global::ToolsCore.GlobalResources.rename;
            resources.ApplyResources(this.tsbRename, "tsbRename");
            this.tsbRename.Name = "tsbRename";
            this.tsbRename.Click += new System.EventHandler(this.tsbRename_Click);
            // 
            // tsbFindReplace
            // 
            this.tsbFindReplace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFindReplace.Image = global::ToolsCore.GlobalResources.find_replace;
            resources.ApplyResources(this.tsbFindReplace, "tsbFindReplace");
            this.tsbFindReplace.Name = "tsbFindReplace";
            this.tsbFindReplace.Click += new System.EventHandler(this.tsbFindReplace_Click);
            // 
            // tsbReformat
            // 
            this.tsbReformat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbReformat.Image = global::ToolsCore.GlobalResources.wrench;
            resources.ApplyResources(this.tsbReformat, "tsbReformat");
            this.tsbReformat.Name = "tsbReformat";
            this.tsbReformat.Click += new System.EventHandler(this.tsbReformat_Click);
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
            this.acImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("acImages.ImageStream")));
            this.acImages.TransparentColor = System.Drawing.Color.Transparent;
            this.acImages.Images.SetKeyName(0, "function.png");
            this.acImages.Images.SetKeyName(1, "constant.png");
            this.acImages.Images.SetKeyName(2, "event.png");
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslTabTabName,
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
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FTabTab_KeyDown);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
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
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbStorno;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbUndo;
        private System.Windows.Forms.ToolStripButton tsbRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private AutocompleteMenuNS.AutocompleteMenu acMenu;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lbTabTabs;
        private System.Windows.Forms.ImageList acImages;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripButton tsbAddTab;
        private System.Windows.Forms.ToolStripButton tsbRemoveTab;
        private System.Windows.Forms.ToolStripButton tsbSaveAll;
        private System.Windows.Forms.ToolStripButton tsbReformat;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbRename;
        private System.Windows.Forms.ToolStripButton tsbFindReplace;
        private System.Windows.Forms.ContextMenuStrip conMenuScText;
        private System.Windows.Forms.ToolStripMenuItem tsmiUndo;
        private System.Windows.Forms.ToolStripMenuItem tsmiSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem tsmiRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsmiCut;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiPaste;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectAll;
        private System.Windows.Forms.ContextMenuStrip conMenuLbTabs;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddTabTab;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteTabTab;
        private System.Windows.Forms.ToolStripMenuItem tsmiRenameTabTab;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveAll;
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
    }
}