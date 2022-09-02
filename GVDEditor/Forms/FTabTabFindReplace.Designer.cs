
using ExControls;

namespace GVDEditor.Forms
{
    partial class FTabTabFindReplace
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
            this.tabControl = new ExControls.ExTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lFind = new System.Windows.Forms.Label();
            this.bFindOrReplace = new ExControls.ExButton();
            this.bCountOrReplaceAll = new ExControls.ExButton();
            this.bFindClose = new ExControls.ExButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cboxSearchingCyclic = new ExControls.ExCheckBox();
            this.cboxSearchingCaseSensitive = new ExControls.ExCheckBox();
            this.cboxSearchingWholeWord = new ExControls.ExCheckBox();
            this.cboxBackSearching = new ExControls.ExCheckBox();
            this.lReplace = new ExControls.ExLabel();
            this.cbReplace = new ExControls.ExComboBox();
            this.cbFind = new ExControls.ExComboBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.exGroupBox1 = new ExControls.ExGroupBox();
            this.rbRegExSearching = new ExControls.ExRadioButton();
            this.rbNormalSearching = new ExControls.ExRadioButton();
            this.exGroupBox2 = new ExControls.ExGroupBox();
            this.cboxTransparency = new ExControls.ExCheckBox();
            this.rbTransAlways = new ExControls.ExRadioButton();
            this.rbTransOnlyOnFocusLost = new ExControls.ExRadioButton();
            this.barTransparency = new System.Windows.Forms.TrackBar();
            this.tabControl.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.exGroupBox1.SuspendLayout();
            this.exGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barTransparency)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl, 2);
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.HeaderBackColor = System.Drawing.SystemColors.Control;
            this.tabControl.HighlightBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tabControl.Location = new System.Drawing.Point(2, 2);
            this.tabControl.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(460, 19);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(452, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Hľadať";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(452, 0);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Nahradiť";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.lFind, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.bFindOrReplace, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.bCountOrReplaceAll, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.bFindClose, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lReplace, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.cbReplace, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.cbFind, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 25);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(460, 143);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // lFind
            // 
            this.lFind.AutoSize = true;
            this.lFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFind.Location = new System.Drawing.Point(2, 0);
            this.lFind.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lFind.Name = "lFind";
            this.lFind.Size = new System.Drawing.Size(53, 27);
            this.lFind.TabIndex = 0;
            this.lFind.Text = "Hľadať:";
            this.lFind.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bFindOrReplace
            // 
            this.bFindOrReplace.AutoSize = true;
            this.bFindOrReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bFindOrReplace.Location = new System.Drawing.Point(379, 2);
            this.bFindOrReplace.Margin = new System.Windows.Forms.Padding(2);
            this.bFindOrReplace.Name = "bFindOrReplace";
            this.bFindOrReplace.Size = new System.Drawing.Size(79, 23);
            this.bFindOrReplace.TabIndex = 5;
            this.bFindOrReplace.Text = "Hľadať ďalší";
            this.bFindOrReplace.UseVisualStyleBackColor = true;
            this.bFindOrReplace.Click += new System.EventHandler(this.bFindOrReplace_Click);
            // 
            // bCountOrReplaceAll
            // 
            this.bCountOrReplaceAll.AutoSize = true;
            this.bCountOrReplaceAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bCountOrReplaceAll.Location = new System.Drawing.Point(379, 29);
            this.bCountOrReplaceAll.Margin = new System.Windows.Forms.Padding(2);
            this.bCountOrReplaceAll.Name = "bCountOrReplaceAll";
            this.bCountOrReplaceAll.Size = new System.Drawing.Size(79, 23);
            this.bCountOrReplaceAll.TabIndex = 6;
            this.bCountOrReplaceAll.Text = "Spočítať";
            this.bCountOrReplaceAll.UseVisualStyleBackColor = true;
            this.bCountOrReplaceAll.Click += new System.EventHandler(this.bCountOrReplaceAll_Click);
            // 
            // bFindClose
            // 
            this.bFindClose.AutoSize = true;
            this.bFindClose.Dock = System.Windows.Forms.DockStyle.Top;
            this.bFindClose.Location = new System.Drawing.Point(379, 56);
            this.bFindClose.Margin = new System.Windows.Forms.Padding(2);
            this.bFindClose.Name = "bFindClose";
            this.bFindClose.Size = new System.Drawing.Size(79, 23);
            this.bFindClose.TabIndex = 7;
            this.bFindClose.Text = "Zatvoriť";
            this.bFindClose.UseVisualStyleBackColor = true;
            this.bFindClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.cboxSearchingCyclic);
            this.flowLayoutPanel1.Controls.Add(this.cboxSearchingCaseSensitive);
            this.flowLayoutPanel1.Controls.Add(this.cboxSearchingWholeWord);
            this.flowLayoutPanel1.Controls.Add(this.cboxBackSearching);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 56);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(373, 85);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // cboxSearchingCyclic
            // 
            this.cboxSearchingCyclic.AutoSize = true;
            this.cboxSearchingCyclic.BoxBackColor = System.Drawing.Color.White;
            this.cboxSearchingCyclic.Checked = true;
            this.cboxSearchingCyclic.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxSearchingCyclic.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxSearchingCyclic.Location = new System.Drawing.Point(2, 66);
            this.cboxSearchingCyclic.Margin = new System.Windows.Forms.Padding(2);
            this.cboxSearchingCyclic.Name = "cboxSearchingCyclic";
            this.cboxSearchingCyclic.Size = new System.Drawing.Size(65, 17);
            this.cboxSearchingCyclic.TabIndex = 3;
            this.cboxSearchingCyclic.Text = "Cyklicky";
            this.cboxSearchingCyclic.UseVisualStyleBackColor = true;
            // 
            // cboxSearchingCaseSensitive
            // 
            this.cboxSearchingCaseSensitive.AutoSize = true;
            this.cboxSearchingCaseSensitive.BoxBackColor = System.Drawing.Color.White;
            this.cboxSearchingCaseSensitive.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxSearchingCaseSensitive.Location = new System.Drawing.Point(2, 45);
            this.cboxSearchingCaseSensitive.Margin = new System.Windows.Forms.Padding(2);
            this.cboxSearchingCaseSensitive.Name = "cboxSearchingCaseSensitive";
            this.cboxSearchingCaseSensitive.Size = new System.Drawing.Size(184, 17);
            this.cboxSearchingCaseSensitive.TabIndex = 2;
            this.cboxSearchingCaseSensitive.Text = "Rozlišovať malé/VEĽKÉ písmená";
            this.cboxSearchingCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // cboxSearchingWholeWord
            // 
            this.cboxSearchingWholeWord.AutoSize = true;
            this.cboxSearchingWholeWord.BoxBackColor = System.Drawing.Color.White;
            this.cboxSearchingWholeWord.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxSearchingWholeWord.Location = new System.Drawing.Point(2, 24);
            this.cboxSearchingWholeWord.Margin = new System.Windows.Forms.Padding(2);
            this.cboxSearchingWholeWord.Name = "cboxSearchingWholeWord";
            this.cboxSearchingWholeWord.Size = new System.Drawing.Size(128, 17);
            this.cboxSearchingWholeWord.TabIndex = 1;
            this.cboxSearchingWholeWord.Text = "Hľadať iba celé slová";
            this.cboxSearchingWholeWord.UseVisualStyleBackColor = true;
            // 
            // cboxBackSearching
            // 
            this.cboxBackSearching.AutoSize = true;
            this.cboxBackSearching.BoxBackColor = System.Drawing.Color.White;
            this.cboxBackSearching.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxBackSearching.Location = new System.Drawing.Point(2, 3);
            this.cboxBackSearching.Margin = new System.Windows.Forms.Padding(2);
            this.cboxBackSearching.Name = "cboxBackSearching";
            this.cboxBackSearching.Size = new System.Drawing.Size(102, 17);
            this.cboxBackSearching.TabIndex = 0;
            this.cboxBackSearching.Text = "Smerom dozadu";
            this.cboxBackSearching.UseVisualStyleBackColor = true;
            this.cboxBackSearching.CheckedChanged += new System.EventHandler(this.CboxBackSearching_CheckedChanged);
            // 
            // lReplace
            // 
            this.lReplace.AutoSize = true;
            this.lReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lReplace.Location = new System.Drawing.Point(3, 27);
            this.lReplace.Name = "lReplace";
            this.lReplace.Size = new System.Drawing.Size(51, 27);
            this.lReplace.TabIndex = 2;
            this.lReplace.Text = "Nahradiť:";
            this.lReplace.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lReplace.Visible = false;
            // 
            // cbReplace
            // 
            this.cbReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbReplace.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbReplace.FormattingEnabled = true;
            this.cbReplace.Location = new System.Drawing.Point(60, 30);
            this.cbReplace.Name = "cbReplace";
            this.cbReplace.Size = new System.Drawing.Size(314, 21);
            this.cbReplace.StyleDisabled.ArrowColor = null;
            this.cbReplace.StyleDisabled.BackColor = null;
            this.cbReplace.StyleDisabled.BorderColor = null;
            this.cbReplace.StyleDisabled.ButtonBackColor = null;
            this.cbReplace.StyleDisabled.ButtonBorderColor = null;
            this.cbReplace.StyleDisabled.ButtonRenderFirst = null;
            this.cbReplace.StyleDisabled.ForeColor = null;
            this.cbReplace.StyleHighlight.ArrowColor = null;
            this.cbReplace.StyleHighlight.BackColor = null;
            this.cbReplace.StyleHighlight.BorderColor = null;
            this.cbReplace.StyleHighlight.ButtonBackColor = null;
            this.cbReplace.StyleHighlight.ButtonBorderColor = null;
            this.cbReplace.StyleHighlight.ButtonRenderFirst = null;
            this.cbReplace.StyleHighlight.ForeColor = null;
            this.cbReplace.StyleNormal.ArrowColor = null;
            this.cbReplace.StyleNormal.BackColor = null;
            this.cbReplace.StyleNormal.BorderColor = null;
            this.cbReplace.StyleNormal.ButtonBackColor = null;
            this.cbReplace.StyleNormal.ButtonBorderColor = null;
            this.cbReplace.StyleNormal.ButtonRenderFirst = null;
            this.cbReplace.StyleNormal.ForeColor = null;
            this.cbReplace.StyleSelected.ArrowColor = null;
            this.cbReplace.StyleSelected.BackColor = null;
            this.cbReplace.StyleSelected.BorderColor = null;
            this.cbReplace.StyleSelected.ButtonBackColor = null;
            this.cbReplace.StyleSelected.ButtonBorderColor = null;
            this.cbReplace.StyleSelected.ButtonRenderFirst = null;
            this.cbReplace.StyleSelected.ForeColor = null;
            this.cbReplace.TabIndex = 3;
            this.cbReplace.UseDarkScrollBar = false;
            this.cbReplace.Visible = false;
            // 
            // cbFind
            // 
            this.cbFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbFind.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbFind.FormattingEnabled = true;
            this.cbFind.Location = new System.Drawing.Point(60, 3);
            this.cbFind.Name = "cbFind";
            this.cbFind.Size = new System.Drawing.Size(314, 21);
            this.cbFind.StyleDisabled.ArrowColor = null;
            this.cbFind.StyleDisabled.BackColor = null;
            this.cbFind.StyleDisabled.BorderColor = null;
            this.cbFind.StyleDisabled.ButtonBackColor = null;
            this.cbFind.StyleDisabled.ButtonBorderColor = null;
            this.cbFind.StyleDisabled.ButtonRenderFirst = null;
            this.cbFind.StyleDisabled.ForeColor = null;
            this.cbFind.StyleHighlight.ArrowColor = null;
            this.cbFind.StyleHighlight.BackColor = null;
            this.cbFind.StyleHighlight.BorderColor = null;
            this.cbFind.StyleHighlight.ButtonBackColor = null;
            this.cbFind.StyleHighlight.ButtonBorderColor = null;
            this.cbFind.StyleHighlight.ButtonRenderFirst = null;
            this.cbFind.StyleHighlight.ForeColor = null;
            this.cbFind.StyleNormal.ArrowColor = null;
            this.cbFind.StyleNormal.BackColor = null;
            this.cbFind.StyleNormal.BorderColor = null;
            this.cbFind.StyleNormal.ButtonBackColor = null;
            this.cbFind.StyleNormal.ButtonBorderColor = null;
            this.cbFind.StyleNormal.ButtonRenderFirst = null;
            this.cbFind.StyleNormal.ForeColor = null;
            this.cbFind.StyleSelected.ArrowColor = null;
            this.cbFind.StyleSelected.BackColor = null;
            this.cbFind.StyleSelected.BorderColor = null;
            this.cbFind.StyleSelected.ButtonBackColor = null;
            this.cbFind.StyleSelected.ButtonBorderColor = null;
            this.cbFind.StyleSelected.ButtonRenderFirst = null;
            this.cbFind.StyleSelected.ForeColor = null;
            this.cbFind.TabIndex = 1;
            this.cbFind.UseDarkScrollBar = false;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 257);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip.Size = new System.Drawing.Size(464, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // tsslStatus
            // 
            this.tsslStatus.Name = "tsslStatus";
            this.tsslStatus.Size = new System.Drawing.Size(63, 17);
            this.tsslStatus.Text = "Pripravené";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.exGroupBox1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.exGroupBox2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tabControl, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(464, 257);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // exGroupBox1
            // 
            this.exGroupBox1.Controls.Add(this.rbRegExSearching);
            this.exGroupBox1.Controls.Add(this.rbNormalSearching);
            this.exGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exGroupBox1.Location = new System.Drawing.Point(2, 172);
            this.exGroupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.exGroupBox1.Name = "exGroupBox1";
            this.exGroupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.exGroupBox1.Size = new System.Drawing.Size(228, 83);
            this.exGroupBox1.TabIndex = 2;
            this.exGroupBox1.TabStop = false;
            this.exGroupBox1.Text = "Režim vyhľadávania";
            // 
            // rbRegExSearching
            // 
            this.rbRegExSearching.AutoSize = true;
            this.rbRegExSearching.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbRegExSearching.Location = new System.Drawing.Point(8, 38);
            this.rbRegExSearching.Margin = new System.Windows.Forms.Padding(2);
            this.rbRegExSearching.Name = "rbRegExSearching";
            this.rbRegExSearching.Size = new System.Drawing.Size(101, 17);
            this.rbRegExSearching.TabIndex = 1;
            this.rbRegExSearching.Text = "Regulárny výraz";
            this.rbRegExSearching.UseVisualStyleBackColor = true;
            this.rbRegExSearching.CheckedChanged += new System.EventHandler(this.rbRegExSearching_CheckedChanged);
            // 
            // rbNormalSearching
            // 
            this.rbNormalSearching.AutoSize = true;
            this.rbNormalSearching.Checked = true;
            this.rbNormalSearching.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbNormalSearching.Location = new System.Drawing.Point(8, 17);
            this.rbNormalSearching.Margin = new System.Windows.Forms.Padding(2);
            this.rbNormalSearching.Name = "rbNormalSearching";
            this.rbNormalSearching.Size = new System.Drawing.Size(69, 17);
            this.rbNormalSearching.TabIndex = 0;
            this.rbNormalSearching.TabStop = true;
            this.rbNormalSearching.Text = "Normálny";
            this.rbNormalSearching.UseVisualStyleBackColor = true;
            // 
            // exGroupBox2
            // 
            this.exGroupBox2.Controls.Add(this.cboxTransparency);
            this.exGroupBox2.Controls.Add(this.rbTransAlways);
            this.exGroupBox2.Controls.Add(this.rbTransOnlyOnFocusLost);
            this.exGroupBox2.Controls.Add(this.barTransparency);
            this.exGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exGroupBox2.Location = new System.Drawing.Point(234, 172);
            this.exGroupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.exGroupBox2.Name = "exGroupBox2";
            this.exGroupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.exGroupBox2.Size = new System.Drawing.Size(228, 83);
            this.exGroupBox2.TabIndex = 3;
            this.exGroupBox2.TabStop = false;
            this.exGroupBox2.Text = " ";
            // 
            // cboxTransparency
            // 
            this.cboxTransparency.AutoSize = true;
            this.cboxTransparency.BoxBackColor = System.Drawing.Color.White;
            this.cboxTransparency.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxTransparency.Location = new System.Drawing.Point(4, 0);
            this.cboxTransparency.Margin = new System.Windows.Forms.Padding(2);
            this.cboxTransparency.Name = "cboxTransparency";
            this.cboxTransparency.Size = new System.Drawing.Size(87, 17);
            this.cboxTransparency.TabIndex = 0;
            this.cboxTransparency.Text = "Priehľadnosť";
            this.cboxTransparency.UseVisualStyleBackColor = true;
            this.cboxTransparency.CheckedChanged += new System.EventHandler(this.cboxTransparency_CheckedChanged);
            // 
            // rbTransAlways
            // 
            this.rbTransAlways.AutoSize = true;
            this.rbTransAlways.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbTransAlways.Location = new System.Drawing.Point(108, 50);
            this.rbTransAlways.Margin = new System.Windows.Forms.Padding(2);
            this.rbTransAlways.Name = "rbTransAlways";
            this.rbTransAlways.Size = new System.Drawing.Size(48, 17);
            this.rbTransAlways.TabIndex = 3;
            this.rbTransAlways.Text = "Vždy";
            this.rbTransAlways.UseVisualStyleBackColor = true;
            // 
            // rbTransOnlyOnFocusLost
            // 
            this.rbTransOnlyOnFocusLost.AutoSize = true;
            this.rbTransOnlyOnFocusLost.Checked = true;
            this.rbTransOnlyOnFocusLost.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbTransOnlyOnFocusLost.Location = new System.Drawing.Point(4, 50);
            this.rbTransOnlyOnFocusLost.Margin = new System.Windows.Forms.Padding(2);
            this.rbTransOnlyOnFocusLost.Name = "rbTransOnlyOnFocusLost";
            this.rbTransOnlyOnFocusLost.Size = new System.Drawing.Size(101, 17);
            this.rbTransOnlyOnFocusLost.TabIndex = 2;
            this.rbTransOnlyOnFocusLost.TabStop = true;
            this.rbTransOnlyOnFocusLost.Text = "Pri strate fokusu";
            this.rbTransOnlyOnFocusLost.UseVisualStyleBackColor = true;
            this.rbTransOnlyOnFocusLost.CheckedChanged += new System.EventHandler(this.rbTransOnlyOnFocusLost_CheckedChanged);
            // 
            // barTransparency
            // 
            this.barTransparency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barTransparency.Enabled = false;
            this.barTransparency.LargeChange = 10;
            this.barTransparency.Location = new System.Drawing.Point(2, 15);
            this.barTransparency.Margin = new System.Windows.Forms.Padding(2);
            this.barTransparency.Maximum = 100;
            this.barTransparency.Minimum = 20;
            this.barTransparency.Name = "barTransparency";
            this.barTransparency.Size = new System.Drawing.Size(224, 66);
            this.barTransparency.SmallChange = 5;
            this.barTransparency.TabIndex = 1;
            this.barTransparency.TickStyle = System.Windows.Forms.TickStyle.None;
            this.barTransparency.Value = 100;
            this.barTransparency.Scroll += new System.EventHandler(this.barTransparency_Scroll);
            // 
            // FTabTabFindReplace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 279);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1480, 318);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(456, 318);
            this.Name = "FTabTabFindReplace";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Hľadať a nahradiť";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.FTabTabFindReplace_Activated);
            this.Deactivate += new System.EventHandler(this.FTabTabFindReplace_Deactivate);
            this.Load += new System.EventHandler(this.FTabTabFindReplace_Load);
            this.tabControl.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.exGroupBox1.ResumeLayout(false);
            this.exGroupBox1.PerformLayout();
            this.exGroupBox2.ResumeLayout(false);
            this.exGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barTransparency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExTabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ExGroupBox exGroupBox1;
        private ExGroupBox exGroupBox2;
        private ExCheckBox cboxTransparency;
        private System.Windows.Forms.TrackBar barTransparency;
        private ExRadioButton rbTransOnlyOnFocusLost;
        private ExRadioButton rbTransAlways;
        private ExRadioButton rbNormalSearching;
        private ExRadioButton rbRegExSearching;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private ExComboBox cbFind;
        private System.Windows.Forms.Label lFind;
        private ExControls.ExButton bFindOrReplace;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
        private ExControls.ExButton bCountOrReplaceAll;
        private ExControls.ExButton bFindClose;
        private ExCheckBox cboxBackSearching;
        private ExCheckBox cboxSearchingWholeWord;
        private ExCheckBox cboxSearchingCaseSensitive;
        private ExCheckBox cboxSearchingCyclic;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ExLabel lReplace;
        private ExComboBox cbReplace;
    }
}