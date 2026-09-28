using ExControls;

namespace GVDEditor.UI.EditTrain
{
    partial class TrainBasicsPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TrainBasicsPage));
            pScroll = new Panel();
            tlpMain = new TableLayoutPanel();
            lBanner = new Label();
            lTrainHeader = new Label();
            lNumber = new Label();
            tbNumber = new ExTextBox();
            lType = new Label();
            cbType = new ExComboBox();
            lName = new Label();
            cbName = new ExComboBox();
            lOperator = new Label();
            cbOperator = new ExComboBox();
            cbRenameSiblings = new ExCheckBox();
            lFlagsHeader = new Label();
            tlpFlags = new TableLayoutPanel();
            boxMiestenkovy = new ExCheckBox();
            boxMedzistatny = new ExCheckBox();
            boxDialkovy = new ExCheckBox();
            boxMimoriadny = new ExCheckBox();
            boxNizkopodlazny = new ExCheckBox();
            boxLozkovy = new ExCheckBox();
            boxPrestup = new ExCheckBox();
            boxMotorovy = new ExCheckBox();
            lLockoutHeader = new Label();
            lLockout = new Label();
            cbLockout = new ExComboBox();
            lLockoutInfo = new Label();
            lHint = new Label();
            tlpMain.SuspendLayout();
            tlpFlags.SuspendLayout();
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
            tlpMain.Controls.Add(lBanner, 0, 0);
            tlpMain.Controls.Add(lTrainHeader, 0, 1);
            tlpMain.Controls.Add(lNumber, 0, 2);
            tlpMain.Controls.Add(tbNumber, 1, 2);
            tlpMain.Controls.Add(lType, 0, 3);
            tlpMain.Controls.Add(cbType, 1, 3);
            tlpMain.Controls.Add(lName, 0, 4);
            tlpMain.Controls.Add(cbName, 1, 4);
            tlpMain.Controls.Add(lOperator, 0, 5);
            tlpMain.Controls.Add(cbOperator, 1, 5);
            tlpMain.Controls.Add(cbRenameSiblings, 1, 6);
            tlpMain.Controls.Add(lFlagsHeader, 0, 7);
            tlpMain.Controls.Add(tlpFlags, 0, 8);
            tlpMain.Controls.Add(lLockoutHeader, 0, 9);
            tlpMain.Controls.Add(lLockout, 0, 10);
            tlpMain.Controls.Add(cbLockout, 1, 10);
            tlpMain.Controls.Add(lLockoutInfo, 1, 11);
            tlpMain.Controls.Add(lHint, 0, 12);
            tlpMain.Name = "tlpMain";
            tlpMain.SetColumnSpan(lBanner, 2);
            tlpMain.SetColumnSpan(lTrainHeader, 2);
            tlpMain.SetColumnSpan(lFlagsHeader, 2);
            tlpMain.SetColumnSpan(tlpFlags, 2);
            tlpMain.SetColumnSpan(lLockoutHeader, 2);
            tlpMain.SetColumnSpan(lHint, 2);
            // 
            // lBanner
            // 
            resources.ApplyResources(lBanner, "lBanner");
            lBanner.Name = "lBanner";
            // 
            // lTrainHeader
            // 
            resources.ApplyResources(lTrainHeader, "lTrainHeader");
            lTrainHeader.Name = "lTrainHeader";
            // 
            // lNumber
            // 
            resources.ApplyResources(lNumber, "lNumber");
            lNumber.Name = "lNumber";
            // 
            // tbNumber
            // 
            resources.ApplyResources(tbNumber, "tbNumber");
            tbNumber.Name = "tbNumber";
            tbNumber.TextChanged += tbNumber_TextChanged;
            tbNumber.Validated += tbNumber_Validated;
            // 
            // lType
            // 
            resources.ApplyResources(lType, "lType");
            lType.Name = "lType";
            // 
            // cbType
            // 
            resources.ApplyResources(cbType, "cbType");
            cbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbType.FormattingEnabled = true;
            cbType.Name = "cbType";
            cbType.SelectionChangeCommitted += cbType_SelectionChangeCommitted;
            // 
            // lName
            // 
            resources.ApplyResources(lName, "lName");
            lName.Name = "lName";
            // 
            // cbName
            // 
            resources.ApplyResources(cbName, "cbName");
            cbName.DropDownStyle = ComboBoxStyle.DropDown;
            cbName.FormattingEnabled = true;
            cbName.Name = "cbName";
            cbName.TextChanged += cbName_TextChanged;
            // 
            // lOperator
            // 
            resources.ApplyResources(lOperator, "lOperator");
            lOperator.Name = "lOperator";
            // 
            // cbOperator
            // 
            resources.ApplyResources(cbOperator, "cbOperator");
            cbOperator.DropDownStyle = ComboBoxStyle.DropDownList;
            cbOperator.FormattingEnabled = true;
            cbOperator.Name = "cbOperator";
            cbOperator.SelectionChangeCommitted += cbOperator_SelectionChangeCommitted;
            // 
            // cbRenameSiblings
            // 
            resources.ApplyResources(cbRenameSiblings, "cbRenameSiblings");
            cbRenameSiblings.Name = "cbRenameSiblings";
            cbRenameSiblings.UseVisualStyleBackColor = true;
            cbRenameSiblings.CheckedChanged += cbRenameSiblings_CheckedChanged;
            // 
            // lFlagsHeader
            // 
            resources.ApplyResources(lFlagsHeader, "lFlagsHeader");
            lFlagsHeader.Name = "lFlagsHeader";
            // 
            // tlpFlags
            // 
            resources.ApplyResources(tlpFlags, "tlpFlags");
            tlpFlags.Controls.Add(boxMiestenkovy, 0, 0);
            tlpFlags.Controls.Add(boxMedzistatny, 1, 0);
            tlpFlags.Controls.Add(boxDialkovy, 2, 0);
            tlpFlags.Controls.Add(boxMimoriadny, 3, 0);
            tlpFlags.Controls.Add(boxNizkopodlazny, 0, 1);
            tlpFlags.Controls.Add(boxLozkovy, 1, 1);
            tlpFlags.Controls.Add(boxPrestup, 2, 1);
            tlpFlags.Controls.Add(boxMotorovy, 3, 1);
            tlpFlags.Name = "tlpFlags";
            // 
            // boxMiestenkovy
            // 
            resources.ApplyResources(boxMiestenkovy, "boxMiestenkovy");
            boxMiestenkovy.Name = "boxMiestenkovy";
            boxMiestenkovy.UseVisualStyleBackColor = true;
            boxMiestenkovy.CheckedChanged += Flag_CheckedChanged;
            // 
            // boxMedzistatny
            // 
            resources.ApplyResources(boxMedzistatny, "boxMedzistatny");
            boxMedzistatny.Name = "boxMedzistatny";
            boxMedzistatny.UseVisualStyleBackColor = true;
            boxMedzistatny.CheckedChanged += Flag_CheckedChanged;
            // 
            // boxDialkovy
            // 
            resources.ApplyResources(boxDialkovy, "boxDialkovy");
            boxDialkovy.Name = "boxDialkovy";
            boxDialkovy.UseVisualStyleBackColor = true;
            boxDialkovy.CheckedChanged += Flag_CheckedChanged;
            // 
            // boxMimoriadny
            // 
            resources.ApplyResources(boxMimoriadny, "boxMimoriadny");
            boxMimoriadny.Name = "boxMimoriadny";
            boxMimoriadny.UseVisualStyleBackColor = true;
            boxMimoriadny.CheckedChanged += Flag_CheckedChanged;
            // 
            // boxNizkopodlazny
            // 
            resources.ApplyResources(boxNizkopodlazny, "boxNizkopodlazny");
            boxNizkopodlazny.Name = "boxNizkopodlazny";
            boxNizkopodlazny.UseVisualStyleBackColor = true;
            boxNizkopodlazny.CheckedChanged += Flag_CheckedChanged;
            // 
            // boxLozkovy
            // 
            resources.ApplyResources(boxLozkovy, "boxLozkovy");
            boxLozkovy.Name = "boxLozkovy";
            boxLozkovy.UseVisualStyleBackColor = true;
            boxLozkovy.CheckedChanged += Flag_CheckedChanged;
            // 
            // boxPrestup
            // 
            resources.ApplyResources(boxPrestup, "boxPrestup");
            boxPrestup.Name = "boxPrestup";
            boxPrestup.UseVisualStyleBackColor = true;
            boxPrestup.CheckedChanged += Flag_CheckedChanged;
            // 
            // boxMotorovy
            // 
            resources.ApplyResources(boxMotorovy, "boxMotorovy");
            boxMotorovy.Name = "boxMotorovy";
            boxMotorovy.UseVisualStyleBackColor = true;
            boxMotorovy.CheckedChanged += Flag_CheckedChanged;
            // 
            // lLockoutHeader
            // 
            resources.ApplyResources(lLockoutHeader, "lLockoutHeader");
            lLockoutHeader.Name = "lLockoutHeader";
            // 
            // lLockout
            // 
            resources.ApplyResources(lLockout, "lLockout");
            lLockout.Name = "lLockout";
            // 
            // cbLockout
            // 
            resources.ApplyResources(cbLockout, "cbLockout");
            cbLockout.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLockout.FormattingEnabled = true;
            cbLockout.Name = "cbLockout";
            cbLockout.SelectionChangeCommitted += cbLockout_SelectionChangeCommitted;
            // 
            // lLockoutInfo
            // 
            resources.ApplyResources(lLockoutInfo, "lLockoutInfo");
            lLockoutInfo.Name = "lLockoutInfo";
            // 
            // lHint
            // 
            resources.ApplyResources(lHint, "lHint");
            lHint.Name = "lHint";
            // 
            // TrainBasicsPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pScroll);
            Name = "TrainBasicsPage";
            tlpFlags.ResumeLayout(false);
            tlpFlags.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pScroll;
        private TableLayoutPanel tlpMain;
        private Label lBanner;
        private Label lTrainHeader;
        private Label lNumber;
        private ExTextBox tbNumber;
        private Label lType;
        private ExComboBox cbType;
        private Label lName;
        private ExComboBox cbName;
        private Label lOperator;
        private ExComboBox cbOperator;
        private ExCheckBox cbRenameSiblings;
        private Label lFlagsHeader;
        private TableLayoutPanel tlpFlags;
        private ExCheckBox boxMiestenkovy;
        private ExCheckBox boxMedzistatny;
        private ExCheckBox boxDialkovy;
        private ExCheckBox boxMimoriadny;
        private ExCheckBox boxNizkopodlazny;
        private ExCheckBox boxLozkovy;
        private ExCheckBox boxPrestup;
        private ExCheckBox boxMotorovy;
        private Label lLockoutHeader;
        private Label lLockout;
        private ExComboBox cbLockout;
        private Label lLockoutInfo;
        private Label lHint;
    }
}
