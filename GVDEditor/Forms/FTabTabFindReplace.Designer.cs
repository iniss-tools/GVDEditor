
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
            ExComboBoxStyle exComboBoxStyle1 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle2 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle3 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle4 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle5 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle6 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle7 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle8 = new ExComboBoxStyle();
            tabControl = new ExTabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            lFind = new Label();
            bFindOrReplace = new ExButton();
            bCountOrReplaceAll = new ExButton();
            bFindClose = new ExButton();
            flowLayoutPanel1 = new FlowLayoutPanel();
            cboxSearchingCyclic = new ExCheckBox();
            cboxSearchingCaseSensitive = new ExCheckBox();
            cboxSearchingWholeWord = new ExCheckBox();
            cboxBackSearching = new ExCheckBox();
            lReplace = new ExLabel();
            cbReplace = new ExComboBox();
            cbFind = new ExComboBox();
            statusStrip = new StatusStrip();
            tsslStatus = new ToolStripStatusLabel();
            tableLayoutPanel1 = new TableLayoutPanel();
            exGroupBox1 = new ExGroupBox();
            rbRegExSearching = new ExRadioButton();
            rbNormalSearching = new ExRadioButton();
            exGroupBox2 = new ExGroupBox();
            cboxTransparency = new ExCheckBox();
            rbTransAlways = new ExRadioButton();
            rbTransOnlyOnFocusLost = new ExRadioButton();
            barTransparency = new TrackBar();
            tabControl.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            statusStrip.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            exGroupBox1.SuspendLayout();
            exGroupBox2.SuspendLayout();
            ((ISupportInitialize)barTransparency).BeginInit();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.ActiveHeaderBackColor = Color.White;
            tabControl.ActiveHeaderForeColor = Color.Black;
            tabControl.BorderColor = Color.LightGray;
            tabControl.BorderThickness = 1;
            tableLayoutPanel1.SetColumnSpan(tabControl, 2);
            tabControl.Controls.Add(tabPage1);
            tabControl.Controls.Add(tabPage2);
            tabControl.DefaultStyle = true;
            tabControl.Dock = DockStyle.Fill;
            tabControl.HeaderBackColor = SystemColors.Control;
            tabControl.HeaderForeColor = Color.Black;
            tabControl.HighlightBackColor = SystemColors.GradientInactiveCaption;
            tabControl.HighlightForeColor = Color.Black;
            tabControl.Location = new Point(2, 2);
            tabControl.Margin = new Padding(2);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(537, 23);
            tabControl.TabIndex = 0;
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(2);
            tabPage1.Size = new Size(529, 0);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Hľadať";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(2);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(2);
            tabPage2.Size = new Size(529, 0);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Nahradiť";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel1.SetColumnSpan(tableLayoutPanel2, 2);
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(lFind, 0, 0);
            tableLayoutPanel2.Controls.Add(bFindOrReplace, 2, 0);
            tableLayoutPanel2.Controls.Add(bCountOrReplaceAll, 2, 1);
            tableLayoutPanel2.Controls.Add(bFindClose, 2, 2);
            tableLayoutPanel2.Controls.Add(flowLayoutPanel1, 0, 2);
            tableLayoutPanel2.Controls.Add(lReplace, 0, 1);
            tableLayoutPanel2.Controls.Add(cbReplace, 1, 1);
            tableLayoutPanel2.Controls.Add(cbFind, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(2, 29);
            tableLayoutPanel2.Margin = new Padding(2);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
            tableLayoutPanel2.Size = new Size(537, 165);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // lFind
            // 
            lFind.AutoSize = true;
            lFind.Dock = DockStyle.Fill;
            lFind.Location = new Point(2, 0);
            lFind.Margin = new Padding(2, 0, 2, 0);
            lFind.Name = "lFind";
            lFind.Size = new Size(61, 33);
            lFind.TabIndex = 0;
            lFind.Text = "Hľadať:";
            lFind.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // bFindOrReplace
            // 
            bFindOrReplace.AutoSize = true;
            bFindOrReplace.DefaultStyle = true;
            bFindOrReplace.Dock = DockStyle.Fill;
            bFindOrReplace.Location = new Point(439, 2);
            bFindOrReplace.Margin = new Padding(2);
            bFindOrReplace.Name = "bFindOrReplace";
            bFindOrReplace.Size = new Size(96, 29);
            bFindOrReplace.TabIndex = 5;
            bFindOrReplace.Text = "Hľadať ďalší";
            bFindOrReplace.UseVisualStyleBackColor = true;
            bFindOrReplace.Click += bFindOrReplace_Click;
            // 
            // bCountOrReplaceAll
            // 
            bCountOrReplaceAll.AutoSize = true;
            bCountOrReplaceAll.DefaultStyle = true;
            bCountOrReplaceAll.Dock = DockStyle.Fill;
            bCountOrReplaceAll.Location = new Point(439, 35);
            bCountOrReplaceAll.Margin = new Padding(2);
            bCountOrReplaceAll.Name = "bCountOrReplaceAll";
            bCountOrReplaceAll.Size = new Size(96, 29);
            bCountOrReplaceAll.TabIndex = 6;
            bCountOrReplaceAll.Text = "Spočítať";
            bCountOrReplaceAll.UseVisualStyleBackColor = true;
            bCountOrReplaceAll.Click += bCountOrReplaceAll_Click;
            // 
            // bFindClose
            // 
            bFindClose.AutoSize = true;
            bFindClose.DefaultStyle = true;
            bFindClose.Dock = DockStyle.Top;
            bFindClose.Location = new Point(439, 68);
            bFindClose.Margin = new Padding(2);
            bFindClose.Name = "bFindClose";
            bFindClose.Size = new Size(96, 29);
            bFindClose.TabIndex = 7;
            bFindClose.Text = "Zatvoriť";
            bFindClose.UseVisualStyleBackColor = true;
            bFindClose.Click += bClose_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            tableLayoutPanel2.SetColumnSpan(flowLayoutPanel1, 2);
            flowLayoutPanel1.Controls.Add(cboxSearchingCyclic);
            flowLayoutPanel1.Controls.Add(cboxSearchingCaseSensitive);
            flowLayoutPanel1.Controls.Add(cboxSearchingWholeWord);
            flowLayoutPanel1.Controls.Add(cboxBackSearching);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.BottomUp;
            flowLayoutPanel1.Location = new Point(2, 68);
            flowLayoutPanel1.Margin = new Padding(2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(433, 95);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // cboxSearchingCyclic
            // 
            cboxSearchingCyclic.AutoSize = true;
            cboxSearchingCyclic.BorderColor = Color.Black;
            cboxSearchingCyclic.BoxBackColor = Color.White;
            cboxSearchingCyclic.Checked = true;
            cboxSearchingCyclic.CheckState = CheckState.Checked;
            cboxSearchingCyclic.DefaultStyle = true;
            cboxSearchingCyclic.DisabledForeColor = Color.DimGray;
            cboxSearchingCyclic.HighlightColor = SystemColors.Highlight;
            cboxSearchingCyclic.Location = new Point(2, 74);
            cboxSearchingCyclic.Margin = new Padding(2);
            cboxSearchingCyclic.MarkColor = Color.Black;
            cboxSearchingCyclic.Name = "cboxSearchingCyclic";
            cboxSearchingCyclic.Size = new Size(70, 19);
            cboxSearchingCyclic.TabIndex = 3;
            cboxSearchingCyclic.Text = "Cyklicky";
            cboxSearchingCyclic.UseVisualStyleBackColor = true;
            // 
            // cboxSearchingCaseSensitive
            // 
            cboxSearchingCaseSensitive.AutoSize = true;
            cboxSearchingCaseSensitive.BorderColor = Color.Black;
            cboxSearchingCaseSensitive.BoxBackColor = Color.White;
            cboxSearchingCaseSensitive.DefaultStyle = true;
            cboxSearchingCaseSensitive.DisabledForeColor = Color.DimGray;
            cboxSearchingCaseSensitive.HighlightColor = SystemColors.Highlight;
            cboxSearchingCaseSensitive.Location = new Point(2, 51);
            cboxSearchingCaseSensitive.Margin = new Padding(2);
            cboxSearchingCaseSensitive.MarkColor = Color.Black;
            cboxSearchingCaseSensitive.Name = "cboxSearchingCaseSensitive";
            cboxSearchingCaseSensitive.Size = new Size(194, 19);
            cboxSearchingCaseSensitive.TabIndex = 2;
            cboxSearchingCaseSensitive.Text = "Rozlišovať malé/VEĽKÉ písmená";
            cboxSearchingCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // cboxSearchingWholeWord
            // 
            cboxSearchingWholeWord.AutoSize = true;
            cboxSearchingWholeWord.BorderColor = Color.Black;
            cboxSearchingWholeWord.BoxBackColor = Color.White;
            cboxSearchingWholeWord.DefaultStyle = true;
            cboxSearchingWholeWord.DisabledForeColor = Color.DimGray;
            cboxSearchingWholeWord.HighlightColor = SystemColors.Highlight;
            cboxSearchingWholeWord.Location = new Point(2, 28);
            cboxSearchingWholeWord.Margin = new Padding(2);
            cboxSearchingWholeWord.MarkColor = Color.Black;
            cboxSearchingWholeWord.Name = "cboxSearchingWholeWord";
            cboxSearchingWholeWord.Size = new Size(136, 19);
            cboxSearchingWholeWord.TabIndex = 1;
            cboxSearchingWholeWord.Text = "Hľadať iba celé slová";
            cboxSearchingWholeWord.UseVisualStyleBackColor = true;
            // 
            // cboxBackSearching
            // 
            cboxBackSearching.AutoSize = true;
            cboxBackSearching.BorderColor = Color.Black;
            cboxBackSearching.BoxBackColor = Color.White;
            cboxBackSearching.DefaultStyle = true;
            cboxBackSearching.DisabledForeColor = Color.DimGray;
            cboxBackSearching.HighlightColor = SystemColors.Highlight;
            cboxBackSearching.Location = new Point(2, 5);
            cboxBackSearching.Margin = new Padding(2);
            cboxBackSearching.MarkColor = Color.Black;
            cboxBackSearching.Name = "cboxBackSearching";
            cboxBackSearching.Size = new Size(113, 19);
            cboxBackSearching.TabIndex = 0;
            cboxBackSearching.Text = "Smerom dozadu";
            cboxBackSearching.UseVisualStyleBackColor = true;
            cboxBackSearching.CheckedChanged += CboxBackSearching_CheckedChanged;
            // 
            // lReplace
            // 
            lReplace.AutoSize = true;
            lReplace.DisabledForeColor = Color.DimGray;
            lReplace.Dock = DockStyle.Fill;
            lReplace.Location = new Point(4, 33);
            lReplace.Margin = new Padding(4, 0, 4, 0);
            lReplace.Name = "lReplace";
            lReplace.Size = new Size(57, 33);
            lReplace.TabIndex = 2;
            lReplace.Text = "Nahradiť:";
            lReplace.TextAlign = ContentAlignment.MiddleLeft;
            lReplace.Visible = false;
            // 
            // cbReplace
            // 
            cbReplace.DefaultStyle = true;
            cbReplace.Dock = DockStyle.Fill;
            cbReplace.DropDownBackColor = Color.White;
            cbReplace.DropDownSelectedRowBackColor = SystemColors.Highlight;
            cbReplace.FormattingEnabled = true;
            cbReplace.Location = new Point(69, 36);
            cbReplace.Margin = new Padding(4, 3, 4, 3);
            cbReplace.Name = "cbReplace";
            cbReplace.Size = new Size(364, 23);
            exComboBoxStyle1.ArrowColor = null;
            exComboBoxStyle1.BackColor = null;
            exComboBoxStyle1.BorderColor = null;
            exComboBoxStyle1.ButtonBackColor = null;
            exComboBoxStyle1.ButtonBorderColor = null;
            exComboBoxStyle1.ButtonRenderFirst = null;
            exComboBoxStyle1.ForeColor = null;
            cbReplace.StyleDisabled = exComboBoxStyle1;
            exComboBoxStyle2.ArrowColor = null;
            exComboBoxStyle2.BackColor = null;
            exComboBoxStyle2.BorderColor = null;
            exComboBoxStyle2.ButtonBackColor = null;
            exComboBoxStyle2.ButtonBorderColor = null;
            exComboBoxStyle2.ButtonRenderFirst = null;
            exComboBoxStyle2.ForeColor = null;
            cbReplace.StyleHighlight = exComboBoxStyle2;
            exComboBoxStyle3.ArrowColor = null;
            exComboBoxStyle3.BackColor = null;
            exComboBoxStyle3.BorderColor = null;
            exComboBoxStyle3.ButtonBackColor = null;
            exComboBoxStyle3.ButtonBorderColor = null;
            exComboBoxStyle3.ButtonRenderFirst = null;
            exComboBoxStyle3.ForeColor = null;
            cbReplace.StyleNormal = exComboBoxStyle3;
            exComboBoxStyle4.ArrowColor = null;
            exComboBoxStyle4.BackColor = null;
            exComboBoxStyle4.BorderColor = null;
            exComboBoxStyle4.ButtonBackColor = null;
            exComboBoxStyle4.ButtonBorderColor = null;
            exComboBoxStyle4.ButtonRenderFirst = null;
            exComboBoxStyle4.ForeColor = null;
            cbReplace.StyleSelected = exComboBoxStyle4;
            cbReplace.TabIndex = 3;
            cbReplace.UseDarkScrollBar = false;
            cbReplace.Visible = false;
            // 
            // cbFind
            // 
            cbFind.DefaultStyle = true;
            cbFind.Dock = DockStyle.Fill;
            cbFind.DropDownBackColor = Color.White;
            cbFind.DropDownSelectedRowBackColor = SystemColors.Highlight;
            cbFind.FormattingEnabled = true;
            cbFind.Location = new Point(69, 3);
            cbFind.Margin = new Padding(4, 3, 4, 3);
            cbFind.Name = "cbFind";
            cbFind.Size = new Size(364, 23);
            exComboBoxStyle5.ArrowColor = null;
            exComboBoxStyle5.BackColor = null;
            exComboBoxStyle5.BorderColor = null;
            exComboBoxStyle5.ButtonBackColor = null;
            exComboBoxStyle5.ButtonBorderColor = null;
            exComboBoxStyle5.ButtonRenderFirst = null;
            exComboBoxStyle5.ForeColor = null;
            cbFind.StyleDisabled = exComboBoxStyle5;
            exComboBoxStyle6.ArrowColor = null;
            exComboBoxStyle6.BackColor = null;
            exComboBoxStyle6.BorderColor = null;
            exComboBoxStyle6.ButtonBackColor = null;
            exComboBoxStyle6.ButtonBorderColor = null;
            exComboBoxStyle6.ButtonRenderFirst = null;
            exComboBoxStyle6.ForeColor = null;
            cbFind.StyleHighlight = exComboBoxStyle6;
            exComboBoxStyle7.ArrowColor = null;
            exComboBoxStyle7.BackColor = null;
            exComboBoxStyle7.BorderColor = null;
            exComboBoxStyle7.ButtonBackColor = null;
            exComboBoxStyle7.ButtonBorderColor = null;
            exComboBoxStyle7.ButtonRenderFirst = null;
            exComboBoxStyle7.ForeColor = null;
            cbFind.StyleNormal = exComboBoxStyle7;
            exComboBoxStyle8.ArrowColor = null;
            exComboBoxStyle8.BackColor = null;
            exComboBoxStyle8.BorderColor = null;
            exComboBoxStyle8.ButtonBackColor = null;
            exComboBoxStyle8.ButtonBorderColor = null;
            exComboBoxStyle8.ButtonRenderFirst = null;
            exComboBoxStyle8.ForeColor = null;
            cbFind.StyleSelected = exComboBoxStyle8;
            cbFind.TabIndex = 1;
            cbFind.UseDarkScrollBar = false;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { tsslStatus });
            statusStrip.Location = new Point(0, 300);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 12, 0);
            statusStrip.Size = new Size(541, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            // 
            // tsslStatus
            // 
            tsslStatus.Name = "tsslStatus";
            tsslStatus.Size = new Size(63, 17);
            tsslStatus.Text = "Pripravené";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(exGroupBox1, 0, 2);
            tableLayoutPanel1.Controls.Add(exGroupBox2, 1, 2);
            tableLayoutPanel1.Controls.Add(tabControl, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(541, 300);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // exGroupBox1
            // 
            exGroupBox1.BorderColor = Color.LightGray;
            exGroupBox1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            exGroupBox1.BorderThickness = 1;
            exGroupBox1.Controls.Add(rbRegExSearching);
            exGroupBox1.Controls.Add(rbNormalSearching);
            exGroupBox1.DefaultStyle = true;
            exGroupBox1.DisabledForeColor = SystemColors.GrayText;
            exGroupBox1.Dock = DockStyle.Fill;
            exGroupBox1.Location = new Point(2, 198);
            exGroupBox1.Margin = new Padding(2);
            exGroupBox1.Name = "exGroupBox1";
            exGroupBox1.Padding = new Padding(2);
            exGroupBox1.Size = new Size(266, 100);
            exGroupBox1.TabIndex = 2;
            exGroupBox1.TabStop = false;
            exGroupBox1.Text = "Režim vyhľadávania";
            // 
            // rbRegExSearching
            // 
            rbRegExSearching.AutoSize = true;
            rbRegExSearching.BorderColor = Color.Black;
            rbRegExSearching.BoxBackColor = Color.White;
            rbRegExSearching.DefaultStyle = true;
            rbRegExSearching.DisabledForeColor = Color.DimGray;
            rbRegExSearching.HighlightColor = SystemColors.Highlight;
            rbRegExSearching.Location = new Point(9, 44);
            rbRegExSearching.Margin = new Padding(2);
            rbRegExSearching.MarkColor = Color.Black;
            rbRegExSearching.Name = "rbRegExSearching";
            rbRegExSearching.Size = new Size(108, 19);
            rbRegExSearching.TabIndex = 1;
            rbRegExSearching.Text = "Regulárny výraz";
            rbRegExSearching.UseVisualStyleBackColor = true;
            rbRegExSearching.CheckedChanged += rbRegExSearching_CheckedChanged;
            // 
            // rbNormalSearching
            // 
            rbNormalSearching.AutoSize = true;
            rbNormalSearching.BorderColor = Color.Black;
            rbNormalSearching.BoxBackColor = Color.White;
            rbNormalSearching.Checked = true;
            rbNormalSearching.DefaultStyle = true;
            rbNormalSearching.DisabledForeColor = Color.DimGray;
            rbNormalSearching.HighlightColor = SystemColors.Highlight;
            rbNormalSearching.Location = new Point(9, 20);
            rbNormalSearching.Margin = new Padding(2);
            rbNormalSearching.MarkColor = Color.Black;
            rbNormalSearching.Name = "rbNormalSearching";
            rbNormalSearching.Size = new Size(78, 19);
            rbNormalSearching.TabIndex = 0;
            rbNormalSearching.TabStop = true;
            rbNormalSearching.Text = "Normálny";
            rbNormalSearching.UseVisualStyleBackColor = true;
            // 
            // exGroupBox2
            // 
            exGroupBox2.BorderColor = Color.LightGray;
            exGroupBox2.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            exGroupBox2.BorderThickness = 1;
            exGroupBox2.Controls.Add(cboxTransparency);
            exGroupBox2.Controls.Add(rbTransAlways);
            exGroupBox2.Controls.Add(rbTransOnlyOnFocusLost);
            exGroupBox2.Controls.Add(barTransparency);
            exGroupBox2.DefaultStyle = true;
            exGroupBox2.DisabledForeColor = SystemColors.GrayText;
            exGroupBox2.Dock = DockStyle.Fill;
            exGroupBox2.Location = new Point(272, 198);
            exGroupBox2.Margin = new Padding(2);
            exGroupBox2.Name = "exGroupBox2";
            exGroupBox2.Padding = new Padding(2);
            exGroupBox2.Size = new Size(267, 100);
            exGroupBox2.TabIndex = 3;
            exGroupBox2.TabStop = false;
            exGroupBox2.Text = " ";
            // 
            // cboxTransparency
            // 
            cboxTransparency.AutoSize = true;
            cboxTransparency.BorderColor = Color.Black;
            cboxTransparency.BoxBackColor = Color.White;
            cboxTransparency.DefaultStyle = true;
            cboxTransparency.DisabledForeColor = Color.DimGray;
            cboxTransparency.HighlightColor = SystemColors.Highlight;
            cboxTransparency.Location = new Point(5, 0);
            cboxTransparency.Margin = new Padding(2);
            cboxTransparency.MarkColor = Color.Black;
            cboxTransparency.Name = "cboxTransparency";
            cboxTransparency.Size = new Size(94, 19);
            cboxTransparency.TabIndex = 0;
            cboxTransparency.Text = "Priehľadnosť";
            cboxTransparency.UseVisualStyleBackColor = true;
            cboxTransparency.CheckedChanged += cboxTransparency_CheckedChanged;
            // 
            // rbTransAlways
            // 
            rbTransAlways.AutoSize = true;
            rbTransAlways.BorderColor = Color.Black;
            rbTransAlways.BoxBackColor = Color.White;
            rbTransAlways.DefaultStyle = true;
            rbTransAlways.DisabledForeColor = Color.DimGray;
            rbTransAlways.HighlightColor = SystemColors.Highlight;
            rbTransAlways.Location = new Point(126, 58);
            rbTransAlways.Margin = new Padding(2);
            rbTransAlways.MarkColor = Color.Black;
            rbTransAlways.Name = "rbTransAlways";
            rbTransAlways.Size = new Size(50, 19);
            rbTransAlways.TabIndex = 3;
            rbTransAlways.Text = "Vždy";
            rbTransAlways.UseVisualStyleBackColor = true;
            // 
            // rbTransOnlyOnFocusLost
            // 
            rbTransOnlyOnFocusLost.AutoSize = true;
            rbTransOnlyOnFocusLost.BorderColor = Color.Black;
            rbTransOnlyOnFocusLost.BoxBackColor = Color.White;
            rbTransOnlyOnFocusLost.Checked = true;
            rbTransOnlyOnFocusLost.DefaultStyle = true;
            rbTransOnlyOnFocusLost.DisabledForeColor = Color.DimGray;
            rbTransOnlyOnFocusLost.HighlightColor = SystemColors.Highlight;
            rbTransOnlyOnFocusLost.Location = new Point(5, 58);
            rbTransOnlyOnFocusLost.Margin = new Padding(2);
            rbTransOnlyOnFocusLost.MarkColor = Color.Black;
            rbTransOnlyOnFocusLost.Name = "rbTransOnlyOnFocusLost";
            rbTransOnlyOnFocusLost.Size = new Size(110, 19);
            rbTransOnlyOnFocusLost.TabIndex = 2;
            rbTransOnlyOnFocusLost.TabStop = true;
            rbTransOnlyOnFocusLost.Text = "Pri strate fokusu";
            rbTransOnlyOnFocusLost.UseVisualStyleBackColor = true;
            rbTransOnlyOnFocusLost.CheckedChanged += rbTransOnlyOnFocusLost_CheckedChanged;
            // 
            // barTransparency
            // 
            barTransparency.Dock = DockStyle.Fill;
            barTransparency.Enabled = false;
            barTransparency.LargeChange = 10;
            barTransparency.Location = new Point(2, 18);
            barTransparency.Margin = new Padding(2);
            barTransparency.Maximum = 100;
            barTransparency.Minimum = 20;
            barTransparency.Name = "barTransparency";
            barTransparency.Size = new Size(263, 80);
            barTransparency.SmallChange = 5;
            barTransparency.TabIndex = 1;
            barTransparency.TickStyle = TickStyle.None;
            barTransparency.Value = 100;
            barTransparency.Scroll += barTransparency_Scroll;
            // 
            // FTabTabFindReplace
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(541, 322);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(statusStrip);
            Margin = new Padding(2);
            MaximizeBox = false;
            MaximumSize = new Size(1724, 361);
            MinimizeBox = false;
            MinimumSize = new Size(529, 361);
            Name = "FTabTabFindReplace";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Hľadať a nahradiť";
            TopMost = true;
            Activated += FTabTabFindReplace_Activated;
            Deactivate += FTabTabFindReplace_Deactivate;
            Load += FTabTabFindReplace_Load;
            tabControl.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            exGroupBox1.ResumeLayout(false);
            exGroupBox1.PerformLayout();
            exGroupBox2.ResumeLayout(false);
            exGroupBox2.PerformLayout();
            ((ISupportInitialize)barTransparency).EndInit();
            ResumeLayout(false);
            PerformLayout();

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