using ExControls;

namespace GVDEditor.UI.Settings
{
    partial class AudioPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AudioPage));
            tlpMain = new TableLayoutPanel();
            lInfo = new Label();
            flpButtons = new FlowLayoutPanel();
            bAdd = new ExButton();
            bDuplicate = new ExButton();
            bDelete = new ExButton();
            splitMain = new SplitContainer();
            dgv = new DataGridView();
            pDetail = new Panel();
            tlpDetail = new TableLayoutPanel();
            lHint = new Label();
            lBasic = new Label();
            lStation = new Label();
            cbStation = new ExComboBox();
            cbCustomOnly = new ExCheckBox();
            lName = new Label();
            tbName = new ExTextBox();
            lShortName = new Label();
            tbShortName = new ExTextBox();
            lQueue = new Label();
            tbQueue = new ExTextBox();
            lBasicNote = new Label();
            lTech = new Label();
            lSoundCard = new Label();
            tbSoundCard = new ExTextBox();
            lInputLine = new Label();
            tbInputLine = new ExTextBox();
            lAmplifier = new Label();
            tbAmplifier = new ExTextBox();
            lExchange = new Label();
            tbExchange = new ExTextBox();
            lNode = new Label();
            tbNode = new ExTextBox();
            lTechNote = new Label();
            colName = new DataGridViewTextBoxColumn();
            colStation = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)splitMain).BeginInit();
            ((ISupportInitialize)dgv).BeginInit();
            tlpMain.SuspendLayout();
            flpButtons.SuspendLayout();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            tlpDetail.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lInfo, 0, 0);
            tlpMain.Controls.Add(flpButtons, 0, 1);
            tlpMain.Controls.Add(splitMain, 0, 2);
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
            flpButtons.Controls.Add(bAdd);
            flpButtons.Controls.Add(bDuplicate);
            flpButtons.Controls.Add(bDelete);
            flpButtons.Name = "flpButtons";
            flpButtons.WrapContents = false;
            // 
            // bAdd
            // 
            resources.ApplyResources(bAdd, "bAdd");
            bAdd.Name = "bAdd";
            bAdd.UseVisualStyleBackColor = true;
            bAdd.Click += bAdd_Click;
            // 
            // bDuplicate
            // 
            resources.ApplyResources(bDuplicate, "bDuplicate");
            bDuplicate.Name = "bDuplicate";
            bDuplicate.UseVisualStyleBackColor = true;
            bDuplicate.Click += bDuplicate_Click;
            // 
            // bDelete
            // 
            resources.ApplyResources(bDelete, "bDelete");
            bDelete.Name = "bDelete";
            bDelete.UseVisualStyleBackColor = true;
            bDelete.Click += bDelete_Click;
            // 
            // splitMain
            // 
            resources.ApplyResources(splitMain, "splitMain");
            splitMain.FixedPanel = FixedPanel.Panel1;
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(dgv);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(pDetail);
            // 
            // dgv
            // 
            resources.ApplyResources(dgv, "dgv");
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Columns.AddRange(new DataGridViewColumn[] { colName, colStation });
            dgv.MultiSelect = false;
            dgv.Name = "dgv";
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.CurrentCellChanged += dgv_CurrentCellChanged;
            dgv.KeyDown += dgv_KeyDown;
            // 
            // pDetail
            // 
            resources.ApplyResources(pDetail, "pDetail");
            pDetail.Controls.Add(tlpDetail);
            pDetail.Name = "pDetail";
            // 
            // tlpDetail
            // 
            resources.ApplyResources(tlpDetail, "tlpDetail");
            tlpDetail.Controls.Add(lBasic, 0, 0);
            tlpDetail.Controls.Add(lStation, 0, 1);
            tlpDetail.Controls.Add(cbStation, 1, 1);
            tlpDetail.Controls.Add(cbCustomOnly, 1, 2);
            tlpDetail.Controls.Add(lName, 0, 3);
            tlpDetail.Controls.Add(tbName, 1, 3);
            tlpDetail.Controls.Add(lShortName, 0, 4);
            tlpDetail.Controls.Add(tbShortName, 1, 4);
            tlpDetail.Controls.Add(lQueue, 0, 5);
            tlpDetail.Controls.Add(tbQueue, 1, 5);
            tlpDetail.Controls.Add(lBasicNote, 0, 6);
            tlpDetail.Controls.Add(lTech, 0, 7);
            tlpDetail.Controls.Add(lSoundCard, 0, 8);
            tlpDetail.Controls.Add(tbSoundCard, 1, 8);
            tlpDetail.Controls.Add(lInputLine, 0, 9);
            tlpDetail.Controls.Add(tbInputLine, 1, 9);
            tlpDetail.Controls.Add(lAmplifier, 0, 10);
            tlpDetail.Controls.Add(tbAmplifier, 1, 10);
            tlpDetail.Controls.Add(lExchange, 0, 11);
            tlpDetail.Controls.Add(tbExchange, 1, 11);
            tlpDetail.Controls.Add(lNode, 0, 12);
            tlpDetail.Controls.Add(tbNode, 1, 12);
            tlpDetail.Controls.Add(lTechNote, 0, 13);
            tlpDetail.Name = "tlpDetail";
            tlpDetail.SetColumnSpan(lBasic, 2);
            tlpDetail.SetColumnSpan(lBasicNote, 2);
            tlpDetail.SetColumnSpan(lTech, 2);
            tlpDetail.SetColumnSpan(lTechNote, 2);
            // 
            // lBasic
            // 
            resources.ApplyResources(lBasic, "lBasic");
            lBasic.Name = "lBasic";
            // 
            // lStation
            // 
            resources.ApplyResources(lStation, "lStation");
            lStation.Name = "lStation";
            // 
            // cbStation
            // 
            resources.ApplyResources(cbStation, "cbStation");
            cbStation.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStation.FormattingEnabled = true;
            cbStation.Name = "cbStation";
            cbStation.SelectionChangeCommitted += cbStation_SelectionChangeCommitted;
            // 
            // cbCustomOnly
            // 
            resources.ApplyResources(cbCustomOnly, "cbCustomOnly");
            cbCustomOnly.Name = "cbCustomOnly";
            cbCustomOnly.UseVisualStyleBackColor = true;
            cbCustomOnly.CheckedChanged += cbCustomOnly_CheckedChanged;
            // 
            // lName
            // 
            resources.ApplyResources(lName, "lName");
            lName.Name = "lName";
            // 
            // tbName
            // 
            resources.ApplyResources(tbName, "tbName");
            tbName.Name = "tbName";
            tbName.TextChanged += Field_Changed;
            // 
            // lShortName
            // 
            resources.ApplyResources(lShortName, "lShortName");
            lShortName.Name = "lShortName";
            // 
            // tbShortName
            // 
            resources.ApplyResources(tbShortName, "tbShortName");
            tbShortName.Name = "tbShortName";
            tbShortName.TextChanged += Field_Changed;
            // 
            // lQueue
            // 
            resources.ApplyResources(lQueue, "lQueue");
            lQueue.Name = "lQueue";
            // 
            // tbQueue
            // 
            resources.ApplyResources(tbQueue, "tbQueue");
            tbQueue.Name = "tbQueue";
            tbQueue.TextChanged += Field_Changed;
            // 
            // lBasicNote
            // 
            resources.ApplyResources(lBasicNote, "lBasicNote");
            lBasicNote.Name = "lBasicNote";
            // 
            // lTech
            // 
            resources.ApplyResources(lTech, "lTech");
            lTech.Name = "lTech";
            // 
            // lSoundCard
            // 
            resources.ApplyResources(lSoundCard, "lSoundCard");
            lSoundCard.Name = "lSoundCard";
            // 
            // tbSoundCard
            // 
            resources.ApplyResources(tbSoundCard, "tbSoundCard");
            tbSoundCard.Name = "tbSoundCard";
            tbSoundCard.TextChanged += Field_Changed;
            // 
            // lInputLine
            // 
            resources.ApplyResources(lInputLine, "lInputLine");
            lInputLine.Name = "lInputLine";
            // 
            // tbInputLine
            // 
            resources.ApplyResources(tbInputLine, "tbInputLine");
            tbInputLine.Name = "tbInputLine";
            tbInputLine.TextChanged += Field_Changed;
            // 
            // lAmplifier
            // 
            resources.ApplyResources(lAmplifier, "lAmplifier");
            lAmplifier.Name = "lAmplifier";
            // 
            // tbAmplifier
            // 
            resources.ApplyResources(tbAmplifier, "tbAmplifier");
            tbAmplifier.Name = "tbAmplifier";
            tbAmplifier.TextChanged += Field_Changed;
            // 
            // lExchange
            // 
            resources.ApplyResources(lExchange, "lExchange");
            lExchange.Name = "lExchange";
            // 
            // tbExchange
            // 
            resources.ApplyResources(tbExchange, "tbExchange");
            tbExchange.Name = "tbExchange";
            tbExchange.TextChanged += Field_Changed;
            // 
            // lNode
            // 
            resources.ApplyResources(lNode, "lNode");
            lNode.Name = "lNode";
            // 
            // tbNode
            // 
            resources.ApplyResources(tbNode, "tbNode");
            tbNode.Name = "tbNode";
            tbNode.TextChanged += Field_Changed;
            // 
            // lTechNote
            // 
            resources.ApplyResources(lTechNote, "lTechNote");
            lTechNote.Name = "lTechNote";
            // 
            // lHint
            // 
            resources.ApplyResources(lHint, "lHint");
            lHint.Name = "lHint";
            // 
            // colName
            // 
            resources.ApplyResources(colName, "colName");
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colName.Name = "colName";
            colName.ReadOnly = true;
            colName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colStation
            // 
            resources.ApplyResources(colStation, "colStation");
            colStation.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colStation.Name = "colStation";
            colStation.ReadOnly = true;
            colStation.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // AudioPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "AudioPage";
            ((ISupportInitialize)splitMain).EndInit();
            ((ISupportInitialize)dgv).EndInit();
            tlpDetail.ResumeLayout(false);
            tlpDetail.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            splitMain.ResumeLayout(false);
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
        private ExButton bAdd;
        private ExButton bDuplicate;
        private ExButton bDelete;
        private SplitContainer splitMain;
        private DataGridView dgv;
        private Panel pDetail;
        private TableLayoutPanel tlpDetail;
        private Label lHint;
        private Label lBasic;
        private Label lStation;
        private ExComboBox cbStation;
        private ExCheckBox cbCustomOnly;
        private Label lName;
        private ExTextBox tbName;
        private Label lShortName;
        private ExTextBox tbShortName;
        private Label lQueue;
        private ExTextBox tbQueue;
        private Label lBasicNote;
        private Label lTech;
        private Label lSoundCard;
        private ExTextBox tbSoundCard;
        private Label lInputLine;
        private ExTextBox tbInputLine;
        private Label lAmplifier;
        private ExTextBox tbAmplifier;
        private Label lExchange;
        private ExTextBox tbExchange;
        private Label lNode;
        private ExTextBox tbNode;
        private Label lTechNote;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colStation;
    }
}
