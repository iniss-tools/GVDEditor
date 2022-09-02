using ExControls;

namespace GVDEditor.Forms
{
    partial class FTableCatalog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTableCatalog));
            this.bSave = new ExControls.ExButton();
            this.bStorno = new ExControls.ExButton();
            this.groupBox5 = new ExControls.ExGroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.bSetColumnOrder = new ExControls.ExButton();
            this.groupBox4 = new ExControls.ExGroupBox();
            this.tbComment = new ExControls.ExTextBox();
            this.groupBox3 = new ExControls.ExGroupBox();
            this.nudMinHeight = new ExControls.ExNumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.nudMaxRecCount = new ExControls.ExNumericUpDown();
            this.bSetAll = new ExControls.ExButton();
            this.nudSize = new ExControls.ExNumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.nudWidth = new ExControls.ExNumericUpDown();
            this.nudHeight = new ExControls.ExNumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.bRowEdit = new ExControls.ExButton();
            this.listRows = new System.Windows.Forms.ListBox();
            this.groupBox2 = new ExControls.ExGroupBox();
            this.exLineSeparator2 = new ExControls.ExLineSeparator();
            this.exLineSeparator1 = new ExControls.ExLineSeparator();
            this.bDown = new ExControls.ExButton();
            this.bUp = new ExControls.ExButton();
            this.cbDivType = new ExControls.ExComboBox();
            this.nudFont = new ExControls.ExNumericUpDown();
            this.nudLine = new ExControls.ExNumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cbTab2 = new ExControls.ExComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbTab1 = new ExControls.ExComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.rbRight = new ExControls.ExRadioButton();
            this.rbCenter = new ExControls.ExRadioButton();
            this.rbLeft = new ExControls.ExRadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.nudEnd = new ExControls.ExNumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudStart = new ExControls.ExNumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.cbColumnFill = new ExControls.ExComboBox();
            this.tbColumnKey = new ExControls.ExTextBox();
            this.tbColumnName = new ExControls.ExTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.bColumnDelete = new ExControls.ExButton();
            this.bColumnEdit = new ExControls.ExButton();
            this.bColumnAdd = new ExControls.ExButton();
            this.listColumns = new System.Windows.Forms.ListBox();
            this.groupBox1 = new ExControls.ExGroupBox();
            this.cbManufacturer = new ExControls.ExComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbKey = new ExControls.ExTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbName = new ExControls.ExTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxRecCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFont)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bSave
            // 
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.Name = "bSave";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bStorno
            // 
            resources.ApplyResources(this.bStorno, "bStorno");
            this.bStorno.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bStorno.Name = "bStorno";
            this.bStorno.UseVisualStyleBackColor = true;
            this.bStorno.Click += new System.EventHandler(this.bStorno_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.bSetColumnOrder);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // bSetColumnOrder
            // 
            resources.ApplyResources(this.bSetColumnOrder, "bSetColumnOrder");
            this.bSetColumnOrder.Name = "bSetColumnOrder";
            this.bSetColumnOrder.UseVisualStyleBackColor = true;
            this.bSetColumnOrder.Click += new System.EventHandler(this.bSetColumnOrder_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbComment);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // tbComment
            // 
            this.tbComment.AcceptsReturn = true;
            this.tbComment.AcceptsTab = true;
            this.tbComment.BorderColor = System.Drawing.Color.DimGray;
            this.tbComment.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbComment.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbComment.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.tbComment, "tbComment");
            this.tbComment.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbComment.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbComment.HintText = null;
            this.tbComment.Name = "tbComment";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nudMinHeight);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.nudMaxRecCount);
            this.groupBox3.Controls.Add(this.bSetAll);
            this.groupBox3.Controls.Add(this.nudSize);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.nudWidth);
            this.groupBox3.Controls.Add(this.nudHeight);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.bRowEdit);
            this.groupBox3.Controls.Add(this.listRows);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // nudMinHeight
            // 
            this.nudMinHeight.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudMinHeight, "nudMinHeight");
            this.nudMinHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudMinHeight.Name = "nudMinHeight";
            this.nudMinHeight.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // nudMaxRecCount
            // 
            this.nudMaxRecCount.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudMaxRecCount, "nudMaxRecCount");
            this.nudMaxRecCount.Name = "nudMaxRecCount";
            this.nudMaxRecCount.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            this.nudMaxRecCount.ValueChanged += new System.EventHandler(this.nudMaxRecCount_ValueChanged);
            // 
            // bSetAll
            // 
            resources.ApplyResources(this.bSetAll, "bSetAll");
            this.bSetAll.Name = "bSetAll";
            this.bSetAll.UseVisualStyleBackColor = true;
            this.bSetAll.Click += new System.EventHandler(this.bSetAll_Click);
            // 
            // nudSize
            // 
            this.nudSize.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudSize, "nudSize");
            this.nudSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudSize.Name = "nudSize";
            this.nudSize.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // nudWidth
            // 
            this.nudWidth.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudWidth, "nudWidth");
            this.nudWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            // 
            // nudHeight
            // 
            this.nudHeight.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudHeight, "nudHeight");
            this.nudHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            this.nudHeight.Validating += new System.ComponentModel.CancelEventHandler(this.nudHeight_Validating);
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // bRowEdit
            // 
            resources.ApplyResources(this.bRowEdit, "bRowEdit");
            this.bRowEdit.Name = "bRowEdit";
            this.bRowEdit.UseVisualStyleBackColor = true;
            this.bRowEdit.Click += new System.EventHandler(this.bRowEdit_Click);
            // 
            // listRows
            // 
            this.listRows.FormattingEnabled = true;
            resources.ApplyResources(this.listRows, "listRows");
            this.listRows.Name = "listRows";
            this.listRows.SelectedIndexChanged += new System.EventHandler(this.listRows_SelectedIndexChanged);
            this.listRows.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.listRows_Format);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.exLineSeparator2);
            this.groupBox2.Controls.Add(this.exLineSeparator1);
            this.groupBox2.Controls.Add(this.bDown);
            this.groupBox2.Controls.Add(this.bUp);
            this.groupBox2.Controls.Add(this.cbDivType);
            this.groupBox2.Controls.Add(this.nudFont);
            this.groupBox2.Controls.Add(this.nudLine);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.cbTab2);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.cbTab1);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.rbRight);
            this.groupBox2.Controls.Add(this.rbCenter);
            this.groupBox2.Controls.Add(this.rbLeft);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.nudEnd);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.nudStart);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.cbColumnFill);
            this.groupBox2.Controls.Add(this.tbColumnKey);
            this.groupBox2.Controls.Add(this.tbColumnName);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.bColumnDelete);
            this.groupBox2.Controls.Add(this.bColumnEdit);
            this.groupBox2.Controls.Add(this.bColumnAdd);
            this.groupBox2.Controls.Add(this.listColumns);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // exLineSeparator2
            // 
            resources.ApplyResources(this.exLineSeparator2, "exLineSeparator2");
            this.exLineSeparator2.Name = "exLineSeparator2";
            // 
            // exLineSeparator1
            // 
            this.exLineSeparator1.LineOrientation = ExControls.LineOrientation.Vertical;
            resources.ApplyResources(this.exLineSeparator1, "exLineSeparator1");
            this.exLineSeparator1.Name = "exLineSeparator1";
            // 
            // bDown
            // 
            resources.ApplyResources(this.bDown, "bDown");
            this.bDown.Name = "bDown";
            this.bDown.UseVisualStyleBackColor = true;
            this.bDown.Click += new System.EventHandler(this.bDown_Click);
            // 
            // bUp
            // 
            resources.ApplyResources(this.bUp, "bUp");
            this.bUp.Name = "bUp";
            this.bUp.UseVisualStyleBackColor = true;
            this.bUp.Click += new System.EventHandler(this.bUp_Click);
            // 
            // cbDivType
            // 
            this.cbDivType.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbDivType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDivType.FormattingEnabled = true;
            resources.ApplyResources(this.cbDivType, "cbDivType");
            this.cbDivType.Name = "cbDivType";
            this.cbDivType.StyleDisabled.ArrowColor = null;
            this.cbDivType.StyleDisabled.BackColor = null;
            this.cbDivType.StyleDisabled.BorderColor = null;
            this.cbDivType.StyleDisabled.ButtonBackColor = null;
            this.cbDivType.StyleDisabled.ButtonBorderColor = null;
            this.cbDivType.StyleDisabled.ButtonRenderFirst = null;
            this.cbDivType.StyleDisabled.ForeColor = null;
            this.cbDivType.StyleHighlight.ArrowColor = null;
            this.cbDivType.StyleHighlight.BackColor = null;
            this.cbDivType.StyleHighlight.BorderColor = null;
            this.cbDivType.StyleHighlight.ButtonBackColor = null;
            this.cbDivType.StyleHighlight.ButtonBorderColor = null;
            this.cbDivType.StyleHighlight.ButtonRenderFirst = null;
            this.cbDivType.StyleHighlight.ForeColor = null;
            this.cbDivType.StyleNormal.ArrowColor = null;
            this.cbDivType.StyleNormal.BackColor = null;
            this.cbDivType.StyleNormal.BorderColor = null;
            this.cbDivType.StyleNormal.ButtonBackColor = null;
            this.cbDivType.StyleNormal.ButtonBorderColor = null;
            this.cbDivType.StyleNormal.ButtonRenderFirst = null;
            this.cbDivType.StyleNormal.ForeColor = null;
            this.cbDivType.StyleSelected.ArrowColor = null;
            this.cbDivType.StyleSelected.BackColor = null;
            this.cbDivType.StyleSelected.BorderColor = null;
            this.cbDivType.StyleSelected.ButtonBackColor = null;
            this.cbDivType.StyleSelected.ButtonBorderColor = null;
            this.cbDivType.StyleSelected.ButtonRenderFirst = null;
            this.cbDivType.StyleSelected.ForeColor = null;
            this.cbDivType.UseDarkScrollBar = false;
            // 
            // nudFont
            // 
            this.nudFont.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudFont, "nudFont");
            this.nudFont.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudFont.Name = "nudFont";
            this.nudFont.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            this.nudFont.ValueChanged += new System.EventHandler(this.nudFont_ValueChanged);
            // 
            // nudLine
            // 
            this.nudLine.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudLine, "nudLine");
            this.nudLine.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudLine.Name = "nudLine";
            this.nudLine.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // cbTab2
            // 
            this.cbTab2.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbTab2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTab2.FormattingEnabled = true;
            resources.ApplyResources(this.cbTab2, "cbTab2");
            this.cbTab2.Name = "cbTab2";
            this.cbTab2.StyleDisabled.ArrowColor = null;
            this.cbTab2.StyleDisabled.BackColor = null;
            this.cbTab2.StyleDisabled.BorderColor = null;
            this.cbTab2.StyleDisabled.ButtonBackColor = null;
            this.cbTab2.StyleDisabled.ButtonBorderColor = null;
            this.cbTab2.StyleDisabled.ButtonRenderFirst = null;
            this.cbTab2.StyleDisabled.ForeColor = null;
            this.cbTab2.StyleHighlight.ArrowColor = null;
            this.cbTab2.StyleHighlight.BackColor = null;
            this.cbTab2.StyleHighlight.BorderColor = null;
            this.cbTab2.StyleHighlight.ButtonBackColor = null;
            this.cbTab2.StyleHighlight.ButtonBorderColor = null;
            this.cbTab2.StyleHighlight.ButtonRenderFirst = null;
            this.cbTab2.StyleHighlight.ForeColor = null;
            this.cbTab2.StyleNormal.ArrowColor = null;
            this.cbTab2.StyleNormal.BackColor = null;
            this.cbTab2.StyleNormal.BorderColor = null;
            this.cbTab2.StyleNormal.ButtonBackColor = null;
            this.cbTab2.StyleNormal.ButtonBorderColor = null;
            this.cbTab2.StyleNormal.ButtonRenderFirst = null;
            this.cbTab2.StyleNormal.ForeColor = null;
            this.cbTab2.StyleSelected.ArrowColor = null;
            this.cbTab2.StyleSelected.BackColor = null;
            this.cbTab2.StyleSelected.BorderColor = null;
            this.cbTab2.StyleSelected.ButtonBackColor = null;
            this.cbTab2.StyleSelected.ButtonBorderColor = null;
            this.cbTab2.StyleSelected.ButtonRenderFirst = null;
            this.cbTab2.StyleSelected.ForeColor = null;
            this.cbTab2.UseDarkScrollBar = false;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // cbTab1
            // 
            this.cbTab1.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbTab1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTab1.FormattingEnabled = true;
            resources.ApplyResources(this.cbTab1, "cbTab1");
            this.cbTab1.Name = "cbTab1";
            this.cbTab1.StyleDisabled.ArrowColor = null;
            this.cbTab1.StyleDisabled.BackColor = null;
            this.cbTab1.StyleDisabled.BorderColor = null;
            this.cbTab1.StyleDisabled.ButtonBackColor = null;
            this.cbTab1.StyleDisabled.ButtonBorderColor = null;
            this.cbTab1.StyleDisabled.ButtonRenderFirst = null;
            this.cbTab1.StyleDisabled.ForeColor = null;
            this.cbTab1.StyleHighlight.ArrowColor = null;
            this.cbTab1.StyleHighlight.BackColor = null;
            this.cbTab1.StyleHighlight.BorderColor = null;
            this.cbTab1.StyleHighlight.ButtonBackColor = null;
            this.cbTab1.StyleHighlight.ButtonBorderColor = null;
            this.cbTab1.StyleHighlight.ButtonRenderFirst = null;
            this.cbTab1.StyleHighlight.ForeColor = null;
            this.cbTab1.StyleNormal.ArrowColor = null;
            this.cbTab1.StyleNormal.BackColor = null;
            this.cbTab1.StyleNormal.BorderColor = null;
            this.cbTab1.StyleNormal.ButtonBackColor = null;
            this.cbTab1.StyleNormal.ButtonBorderColor = null;
            this.cbTab1.StyleNormal.ButtonRenderFirst = null;
            this.cbTab1.StyleNormal.ForeColor = null;
            this.cbTab1.StyleSelected.ArrowColor = null;
            this.cbTab1.StyleSelected.BackColor = null;
            this.cbTab1.StyleSelected.BorderColor = null;
            this.cbTab1.StyleSelected.ButtonBackColor = null;
            this.cbTab1.StyleSelected.ButtonBorderColor = null;
            this.cbTab1.StyleSelected.ButtonRenderFirst = null;
            this.cbTab1.StyleSelected.ForeColor = null;
            this.cbTab1.UseDarkScrollBar = false;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // rbRight
            // 
            resources.ApplyResources(this.rbRight, "rbRight");
            this.rbRight.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbRight.Name = "rbRight";
            this.rbRight.UseVisualStyleBackColor = true;
            // 
            // rbCenter
            // 
            resources.ApplyResources(this.rbCenter, "rbCenter");
            this.rbCenter.Checked = true;
            this.rbCenter.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbCenter.Name = "rbCenter";
            this.rbCenter.TabStop = true;
            this.rbCenter.UseVisualStyleBackColor = true;
            // 
            // rbLeft
            // 
            resources.ApplyResources(this.rbLeft, "rbLeft");
            this.rbLeft.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbLeft.Name = "rbLeft";
            this.rbLeft.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // nudEnd
            // 
            this.nudEnd.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudEnd, "nudEnd");
            this.nudEnd.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudEnd.Name = "nudEnd";
            this.nudEnd.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // nudStart
            // 
            this.nudStart.HighlightColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.nudStart, "nudStart");
            this.nudStart.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudStart.Name = "nudStart";
            this.nudStart.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cbColumnFill
            // 
            this.cbColumnFill.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbColumnFill.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColumnFill.FormattingEnabled = true;
            resources.ApplyResources(this.cbColumnFill, "cbColumnFill");
            this.cbColumnFill.Name = "cbColumnFill";
            this.cbColumnFill.StyleDisabled.ArrowColor = null;
            this.cbColumnFill.StyleDisabled.BackColor = null;
            this.cbColumnFill.StyleDisabled.BorderColor = null;
            this.cbColumnFill.StyleDisabled.ButtonBackColor = null;
            this.cbColumnFill.StyleDisabled.ButtonBorderColor = null;
            this.cbColumnFill.StyleDisabled.ButtonRenderFirst = null;
            this.cbColumnFill.StyleDisabled.ForeColor = null;
            this.cbColumnFill.StyleHighlight.ArrowColor = null;
            this.cbColumnFill.StyleHighlight.BackColor = null;
            this.cbColumnFill.StyleHighlight.BorderColor = null;
            this.cbColumnFill.StyleHighlight.ButtonBackColor = null;
            this.cbColumnFill.StyleHighlight.ButtonBorderColor = null;
            this.cbColumnFill.StyleHighlight.ButtonRenderFirst = null;
            this.cbColumnFill.StyleHighlight.ForeColor = null;
            this.cbColumnFill.StyleNormal.ArrowColor = null;
            this.cbColumnFill.StyleNormal.BackColor = null;
            this.cbColumnFill.StyleNormal.BorderColor = null;
            this.cbColumnFill.StyleNormal.ButtonBackColor = null;
            this.cbColumnFill.StyleNormal.ButtonBorderColor = null;
            this.cbColumnFill.StyleNormal.ButtonRenderFirst = null;
            this.cbColumnFill.StyleNormal.ForeColor = null;
            this.cbColumnFill.StyleSelected.ArrowColor = null;
            this.cbColumnFill.StyleSelected.BackColor = null;
            this.cbColumnFill.StyleSelected.BorderColor = null;
            this.cbColumnFill.StyleSelected.ButtonBackColor = null;
            this.cbColumnFill.StyleSelected.ButtonBorderColor = null;
            this.cbColumnFill.StyleSelected.ButtonRenderFirst = null;
            this.cbColumnFill.StyleSelected.ForeColor = null;
            this.cbColumnFill.UseDarkScrollBar = false;
            // 
            // tbColumnKey
            // 
            this.tbColumnKey.BorderColor = System.Drawing.Color.DimGray;
            this.tbColumnKey.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbColumnKey.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbColumnKey.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbColumnKey.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbColumnKey.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbColumnKey.HintText = null;
            resources.ApplyResources(this.tbColumnKey, "tbColumnKey");
            this.tbColumnKey.Name = "tbColumnKey";
            // 
            // tbColumnName
            // 
            this.tbColumnName.BorderColor = System.Drawing.Color.DimGray;
            this.tbColumnName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbColumnName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbColumnName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbColumnName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbColumnName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbColumnName.HintText = null;
            resources.ApplyResources(this.tbColumnName, "tbColumnName");
            this.tbColumnName.Name = "tbColumnName";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // bColumnDelete
            // 
            resources.ApplyResources(this.bColumnDelete, "bColumnDelete");
            this.bColumnDelete.Name = "bColumnDelete";
            this.bColumnDelete.UseVisualStyleBackColor = true;
            this.bColumnDelete.Click += new System.EventHandler(this.bColumnDelete_Click);
            // 
            // bColumnEdit
            // 
            resources.ApplyResources(this.bColumnEdit, "bColumnEdit");
            this.bColumnEdit.Name = "bColumnEdit";
            this.bColumnEdit.UseVisualStyleBackColor = true;
            this.bColumnEdit.Click += new System.EventHandler(this.bColumnEdit_Click);
            // 
            // bColumnAdd
            // 
            resources.ApplyResources(this.bColumnAdd, "bColumnAdd");
            this.bColumnAdd.Name = "bColumnAdd";
            this.bColumnAdd.UseVisualStyleBackColor = true;
            this.bColumnAdd.Click += new System.EventHandler(this.bColumnAdd_Click);
            // 
            // listColumns
            // 
            this.listColumns.FormattingEnabled = true;
            resources.ApplyResources(this.listColumns, "listColumns");
            this.listColumns.Name = "listColumns";
            this.listColumns.SelectedIndexChanged += new System.EventHandler(this.listColumns_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbManufacturer);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbKey);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbName);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cbManufacturer
            // 
            this.cbManufacturer.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbManufacturer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbManufacturer.FormattingEnabled = true;
            resources.ApplyResources(this.cbManufacturer, "cbManufacturer");
            this.cbManufacturer.Name = "cbManufacturer";
            this.cbManufacturer.StyleDisabled.ArrowColor = null;
            this.cbManufacturer.StyleDisabled.BackColor = null;
            this.cbManufacturer.StyleDisabled.BorderColor = null;
            this.cbManufacturer.StyleDisabled.ButtonBackColor = null;
            this.cbManufacturer.StyleDisabled.ButtonBorderColor = null;
            this.cbManufacturer.StyleDisabled.ButtonRenderFirst = null;
            this.cbManufacturer.StyleDisabled.ForeColor = null;
            this.cbManufacturer.StyleHighlight.ArrowColor = null;
            this.cbManufacturer.StyleHighlight.BackColor = null;
            this.cbManufacturer.StyleHighlight.BorderColor = null;
            this.cbManufacturer.StyleHighlight.ButtonBackColor = null;
            this.cbManufacturer.StyleHighlight.ButtonBorderColor = null;
            this.cbManufacturer.StyleHighlight.ButtonRenderFirst = null;
            this.cbManufacturer.StyleHighlight.ForeColor = null;
            this.cbManufacturer.StyleNormal.ArrowColor = null;
            this.cbManufacturer.StyleNormal.BackColor = null;
            this.cbManufacturer.StyleNormal.BorderColor = null;
            this.cbManufacturer.StyleNormal.ButtonBackColor = null;
            this.cbManufacturer.StyleNormal.ButtonBorderColor = null;
            this.cbManufacturer.StyleNormal.ButtonRenderFirst = null;
            this.cbManufacturer.StyleNormal.ForeColor = null;
            this.cbManufacturer.StyleSelected.ArrowColor = null;
            this.cbManufacturer.StyleSelected.BackColor = null;
            this.cbManufacturer.StyleSelected.BorderColor = null;
            this.cbManufacturer.StyleSelected.ButtonBackColor = null;
            this.cbManufacturer.StyleSelected.ButtonBorderColor = null;
            this.cbManufacturer.StyleSelected.ButtonRenderFirst = null;
            this.cbManufacturer.StyleSelected.ForeColor = null;
            this.cbManufacturer.UseDarkScrollBar = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbKey
            // 
            this.tbKey.BorderColor = System.Drawing.Color.DimGray;
            this.tbKey.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbKey.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbKey.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbKey.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbKey.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbKey.HintText = null;
            resources.ApplyResources(this.tbKey, "tbKey");
            this.tbKey.Name = "tbKey";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tbName
            // 
            this.tbName.BorderColor = System.Drawing.Color.DimGray;
            this.tbName.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbName.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbName.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbName.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbName.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbName.HintText = null;
            resources.ApplyResources(this.tbName, "tbName");
            this.tbName.Name = "tbName";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // FTableCatalog
            // 
            this.AcceptButton = this.bSave;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bStorno);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FTableCatalog";
            this.ShowInTaskbar = false;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FTableCatalog_HelpButtonClicked);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxRecCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFont)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExGroupBox groupBox1;
        private ExComboBox cbManufacturer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private ExTextBox tbKey;
        private ExTextBox tbName;
        private System.Windows.Forms.Label label1;
        private ExControls.ExButton bSave;
        private ExControls.ExButton bStorno;
        private ExGroupBox groupBox2;
        private ExComboBox cbColumnFill;
        private ExTextBox tbColumnKey;
        private ExTextBox tbColumnName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private ExControls.ExButton bColumnDelete;
        private ExControls.ExButton bColumnEdit;
        private ExControls.ExButton bColumnAdd;
        private System.Windows.Forms.ListBox listColumns;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private ExComboBox cbTab2;
        private System.Windows.Forms.Label label11;
        private ExComboBox cbTab1;
        private System.Windows.Forms.Label label10;
        private ExRadioButton rbRight;
        private ExRadioButton rbCenter;
        private ExRadioButton rbLeft;
        private System.Windows.Forms.Label label9;
        private ExNumericUpDown nudEnd;
        private System.Windows.Forms.Label label8;
        private ExNumericUpDown nudStart;
        private System.Windows.Forms.Label label7;
        private ExNumericUpDown nudLine;
        private ExNumericUpDown nudMaxRecCount;
        private System.Windows.Forms.Label label16;
        private ExGroupBox groupBox3;
        private ExNumericUpDown nudSize;
        private ExNumericUpDown nudWidth;
        private ExNumericUpDown nudHeight;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private ExControls.ExButton bRowEdit;
        private System.Windows.Forms.ListBox listRows;
        private ExGroupBox groupBox4;
        private ExTextBox tbComment;
        private ExNumericUpDown nudFont;
        private ExComboBox cbDivType;
        private ExControls.ExButton bSetAll;
        private ExGroupBox groupBox5;
        private ExControls.ExButton bSetColumnOrder;
        private System.Windows.Forms.Label label15;
        private ExNumericUpDown nudMinHeight;
        private System.Windows.Forms.Label label20;
        private ExLineSeparator exLineSeparator2;
        private ExLineSeparator exLineSeparator1;
        private ExControls.ExButton bDown;
        private ExControls.ExButton bUp;
    }
}