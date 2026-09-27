using ExControls;
using GVDEditor.Controls;

namespace GVDEditor.Forms.Settings
{
    partial class FontsPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FontsPage));
            tlpMain = new TableLayoutPanel();
            lInfo = new Label();
            flpButtons = new FlowLayoutPanel();
            bAdd = new ExButton();
            bDuplicate = new ExButton();
            bDelete = new ExButton();
            listFonts = new ListBox();
            pDetail = new Panel();
            tlpDetail = new TableLayoutPanel();
            lName = new Label();
            tbName = new ExTextBox();
            lLook = new Label();
            picker = new TableFontPicker();
            lProblem = new Label();
            lData = new Label();
            lDataNote = new Label();
            lType = new Label();
            flpType = new FlowLayoutPanel();
            cbType = new ExComboBox();
            lTypeAuto = new Label();
            lSize = new Label();
            flpSize = new FlowLayoutPanel();
            nudSize = new ExNumericUpDown();
            nudWidth = new ExNumericUpDown();
            lWidthAuto = new Label();
            lChars = new Label();
            flpChars1 = new FlowLayoutPanel();
            cbProp = new ExCheckBox();
            lPropAuto = new Label();
            cbDia = new ExCheckBox();
            cbLower = new ExCheckBox();
            cbUpper = new ExCheckBox();
            flpChars2 = new FlowLayoutPanel();
            cbNum = new ExCheckBox();
            cbSpec = new ExCheckBox();
            cbSpecAssign = new ExCheckBox();
            lFile = new Label();
            tbFile = new ExTextBox();
            lUseHeader = new Label();
            lUse = new Label();
            flpDir = new FlowLayoutPanel();
            lDir = new Label();
            tbDir = new ExTextBox();
            bDir = new ExButton();
            ((ISupportInitialize)nudSize).BeginInit();
            ((ISupportInitialize)nudWidth).BeginInit();
            tlpMain.SuspendLayout();
            flpButtons.SuspendLayout();
            tlpDetail.SuspendLayout();
            flpType.SuspendLayout();
            flpSize.SuspendLayout();
            flpChars1.SuspendLayout();
            flpChars2.SuspendLayout();
            flpDir.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lInfo, 0, 0);
            tlpMain.Controls.Add(flpButtons, 0, 1);
            tlpMain.Controls.Add(listFonts, 0, 2);
            tlpMain.Controls.Add(pDetail, 1, 2);
            tlpMain.Controls.Add(flpDir, 0, 3);
            tlpMain.Name = "tlpMain";
            tlpMain.SetColumnSpan(lInfo, 2);
            tlpMain.SetColumnSpan(flpButtons, 2);
            tlpMain.SetColumnSpan(flpDir, 2);
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
            // listFonts
            // 
            resources.ApplyResources(listFonts, "listFonts");
            listFonts.DrawMode = DrawMode.OwnerDrawFixed;
            listFonts.FormattingEnabled = true;
            listFonts.IntegralHeight = false;
            listFonts.Name = "listFonts";
            listFonts.DrawItem += listFonts_DrawItem;
            listFonts.SelectedIndexChanged += listFonts_SelectedIndexChanged;
            listFonts.KeyDown += listFonts_KeyDown;
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
            tlpDetail.Controls.Add(lName, 0, 0);
            tlpDetail.Controls.Add(tbName, 1, 0);
            tlpDetail.Controls.Add(lLook, 0, 1);
            tlpDetail.Controls.Add(picker, 0, 2);
            tlpDetail.Controls.Add(lProblem, 0, 3);
            tlpDetail.Controls.Add(lData, 0, 4);
            tlpDetail.Controls.Add(lDataNote, 0, 5);
            tlpDetail.Controls.Add(lType, 0, 6);
            tlpDetail.Controls.Add(flpType, 1, 6);
            tlpDetail.Controls.Add(lSize, 0, 7);
            tlpDetail.Controls.Add(flpSize, 1, 7);
            tlpDetail.Controls.Add(lChars, 0, 8);
            tlpDetail.Controls.Add(flpChars1, 1, 8);
            tlpDetail.Controls.Add(flpChars2, 1, 9);
            tlpDetail.Controls.Add(lFile, 0, 10);
            tlpDetail.Controls.Add(tbFile, 1, 10);
            tlpDetail.Controls.Add(lUseHeader, 0, 11);
            tlpDetail.Controls.Add(lUse, 0, 12);
            tlpDetail.Name = "tlpDetail";
            tlpDetail.SetColumnSpan(lLook, 2);
            tlpDetail.SetColumnSpan(picker, 2);
            tlpDetail.SetColumnSpan(lProblem, 2);
            tlpDetail.SetColumnSpan(lData, 2);
            tlpDetail.SetColumnSpan(lDataNote, 2);
            tlpDetail.SetColumnSpan(lUseHeader, 2);
            tlpDetail.SetColumnSpan(lUse, 2);
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
            tbName.TextChanged += tbName_TextChanged;
            // 
            // lLook
            // 
            resources.ApplyResources(lLook, "lLook");
            lLook.Name = "lLook";
            // 
            // picker
            // 
            resources.ApplyResources(picker, "picker");
            picker.Name = "picker";
            picker.ValueChanged += picker_ValueChanged;
            // 
            // lProblem
            // 
            resources.ApplyResources(lProblem, "lProblem");
            lProblem.Name = "lProblem";
            // 
            // lData
            // 
            resources.ApplyResources(lData, "lData");
            lData.Name = "lData";
            // 
            // lDataNote
            // 
            resources.ApplyResources(lDataNote, "lDataNote");
            lDataNote.Name = "lDataNote";
            // 
            // lType
            // 
            resources.ApplyResources(lType, "lType");
            lType.Name = "lType";
            // 
            // flpType
            // 
            resources.ApplyResources(flpType, "flpType");
            flpType.Controls.Add(cbType);
            flpType.Controls.Add(lTypeAuto);
            flpType.Name = "flpType";
            // 
            // cbType
            // 
            resources.ApplyResources(cbType, "cbType");
            cbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbType.FormattingEnabled = true;
            cbType.Name = "cbType";
            cbType.SelectionChangeCommitted += cbType_SelectionChangeCommitted;
            // 
            // lTypeAuto
            // 
            resources.ApplyResources(lTypeAuto, "lTypeAuto");
            lTypeAuto.Name = "lTypeAuto";
            // 
            // lSize
            // 
            resources.ApplyResources(lSize, "lSize");
            lSize.Name = "lSize";
            // 
            // flpSize
            // 
            resources.ApplyResources(flpSize, "flpSize");
            flpSize.Controls.Add(nudSize);
            flpSize.Controls.Add(nudWidth);
            flpSize.Controls.Add(lWidthAuto);
            flpSize.Name = "flpSize";
            // 
            // nudSize
            // 
            resources.ApplyResources(nudSize, "nudSize");
            nudSize.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSize.Name = "nudSize";
            nudSize.Value = new decimal(new int[] { 7, 0, 0, 0 });
            nudSize.ValueChanged += nudSize_ValueChanged;
            // 
            // nudWidth
            // 
            resources.ApplyResources(nudWidth, "nudWidth");
            nudWidth.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudWidth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudWidth.Name = "nudWidth";
            nudWidth.Value = new decimal(new int[] { 6, 0, 0, 0 });
            nudWidth.ValueChanged += nudWidth_ValueChanged;
            // 
            // lWidthAuto
            // 
            resources.ApplyResources(lWidthAuto, "lWidthAuto");
            lWidthAuto.Name = "lWidthAuto";
            // 
            // lChars
            // 
            resources.ApplyResources(lChars, "lChars");
            lChars.Name = "lChars";
            // 
            // flpChars1
            // 
            resources.ApplyResources(flpChars1, "flpChars1");
            flpChars1.Controls.Add(cbProp);
            flpChars1.Controls.Add(lPropAuto);
            flpChars1.Controls.Add(cbDia);
            flpChars1.Controls.Add(cbLower);
            flpChars1.Controls.Add(cbUpper);
            flpChars1.Name = "flpChars1";
            // 
            // cbProp
            // 
            resources.ApplyResources(cbProp, "cbProp");
            cbProp.Name = "cbProp";
            cbProp.UseVisualStyleBackColor = true;
            cbProp.CheckedChanged += cbProp_CheckedChanged;
            // 
            // lPropAuto
            // 
            resources.ApplyResources(lPropAuto, "lPropAuto");
            lPropAuto.Name = "lPropAuto";
            // 
            // cbDia
            // 
            resources.ApplyResources(cbDia, "cbDia");
            cbDia.Name = "cbDia";
            cbDia.UseVisualStyleBackColor = true;
            cbDia.CheckedChanged += Flag_CheckedChanged;
            // 
            // cbLower
            // 
            resources.ApplyResources(cbLower, "cbLower");
            cbLower.Name = "cbLower";
            cbLower.UseVisualStyleBackColor = true;
            cbLower.CheckedChanged += Flag_CheckedChanged;
            // 
            // cbUpper
            // 
            resources.ApplyResources(cbUpper, "cbUpper");
            cbUpper.Name = "cbUpper";
            cbUpper.UseVisualStyleBackColor = true;
            cbUpper.CheckedChanged += Flag_CheckedChanged;
            // 
            // flpChars2
            // 
            resources.ApplyResources(flpChars2, "flpChars2");
            flpChars2.Controls.Add(cbNum);
            flpChars2.Controls.Add(cbSpec);
            flpChars2.Controls.Add(cbSpecAssign);
            flpChars2.Name = "flpChars2";
            // 
            // cbNum
            // 
            resources.ApplyResources(cbNum, "cbNum");
            cbNum.Name = "cbNum";
            cbNum.UseVisualStyleBackColor = true;
            cbNum.CheckedChanged += Flag_CheckedChanged;
            // 
            // cbSpec
            // 
            resources.ApplyResources(cbSpec, "cbSpec");
            cbSpec.Name = "cbSpec";
            cbSpec.UseVisualStyleBackColor = true;
            cbSpec.CheckedChanged += Flag_CheckedChanged;
            // 
            // cbSpecAssign
            // 
            resources.ApplyResources(cbSpecAssign, "cbSpecAssign");
            cbSpecAssign.Name = "cbSpecAssign";
            cbSpecAssign.UseVisualStyleBackColor = true;
            cbSpecAssign.CheckedChanged += Flag_CheckedChanged;
            // 
            // lFile
            // 
            resources.ApplyResources(lFile, "lFile");
            lFile.Name = "lFile";
            // 
            // tbFile
            // 
            resources.ApplyResources(tbFile, "tbFile");
            tbFile.Name = "tbFile";
            tbFile.TextChanged += tbFile_TextChanged;
            // 
            // lUseHeader
            // 
            resources.ApplyResources(lUseHeader, "lUseHeader");
            lUseHeader.Name = "lUseHeader";
            // 
            // lUse
            // 
            resources.ApplyResources(lUse, "lUse");
            lUse.Name = "lUse";
            // 
            // flpDir
            // 
            resources.ApplyResources(flpDir, "flpDir");
            flpDir.Controls.Add(lDir);
            flpDir.Controls.Add(tbDir);
            flpDir.Controls.Add(bDir);
            flpDir.Name = "flpDir";
            flpDir.WrapContents = false;
            // 
            // lDir
            // 
            resources.ApplyResources(lDir, "lDir");
            lDir.Name = "lDir";
            // 
            // tbDir
            // 
            resources.ApplyResources(tbDir, "tbDir");
            tbDir.Name = "tbDir";
            // 
            // bDir
            // 
            resources.ApplyResources(bDir, "bDir");
            bDir.Name = "bDir";
            bDir.UseVisualStyleBackColor = true;
            bDir.Click += bDir_Click;
            // 
            // FontsPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "FontsPage";
            ((ISupportInitialize)nudSize).EndInit();
            ((ISupportInitialize)nudWidth).EndInit();
            flpDir.ResumeLayout(false);
            flpDir.PerformLayout();
            flpChars2.ResumeLayout(false);
            flpChars2.PerformLayout();
            flpChars1.ResumeLayout(false);
            flpChars1.PerformLayout();
            flpSize.ResumeLayout(false);
            flpSize.PerformLayout();
            flpType.ResumeLayout(false);
            flpType.PerformLayout();
            tlpDetail.ResumeLayout(false);
            tlpDetail.PerformLayout();
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
        private ListBox listFonts;
        private Panel pDetail;
        private TableLayoutPanel tlpDetail;
        private Label lName;
        private ExTextBox tbName;
        private Label lLook;
        private TableFontPicker picker;
        private Label lProblem;
        private Label lData;
        private Label lDataNote;
        private Label lType;
        private FlowLayoutPanel flpType;
        private ExComboBox cbType;
        private Label lTypeAuto;
        private Label lSize;
        private FlowLayoutPanel flpSize;
        private ExNumericUpDown nudSize;
        private ExNumericUpDown nudWidth;
        private Label lWidthAuto;
        private Label lChars;
        private FlowLayoutPanel flpChars1;
        private ExCheckBox cbProp;
        private Label lPropAuto;
        private ExCheckBox cbDia;
        private ExCheckBox cbLower;
        private ExCheckBox cbUpper;
        private FlowLayoutPanel flpChars2;
        private ExCheckBox cbNum;
        private ExCheckBox cbSpec;
        private ExCheckBox cbSpecAssign;
        private Label lFile;
        private ExTextBox tbFile;
        private Label lUseHeader;
        private Label lUse;
        private FlowLayoutPanel flpDir;
        private Label lDir;
        private ExTextBox tbDir;
        private ExButton bDir;
    }
}
