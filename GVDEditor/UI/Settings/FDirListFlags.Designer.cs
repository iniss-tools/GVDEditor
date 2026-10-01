using ExControls;

namespace GVDEditor.UI.Settings
{
    partial class FDirListFlags
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FDirListFlags));
            tlpMain = new TableLayoutPanel();
            lIntro = new Label();
            gbMessages = new ExGroupBox();
            flpMessages = new FlowLayoutPanel();
            cboxSpread = new ExCheckBox();
            cboxDeparture = new ExCheckBox();
            lCreate = new Label();
            rbCreateNone = new ExRadioButton();
            rbCreate = new ExRadioButton();
            rbCreateWithoutCategori = new ExRadioButton();
            gbActive = new ExGroupBox();
            flpActive = new FlowLayoutPanel();
            cbSwitch = new ExComboBox();
            lInactive = new Label();
            lCode = new Label();
            flpButtons = new FlowLayoutPanel();
            bCancel = new ExButton();
            bOK = new ExButton();
            tlpMain.SuspendLayout();
            gbMessages.SuspendLayout();
            flpMessages.SuspendLayout();
            gbActive.SuspendLayout();
            flpActive.SuspendLayout();
            flpButtons.SuspendLayout();
            SuspendLayout();
            //
            // tlpMain
            //
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lIntro, 0, 0);
            tlpMain.Controls.Add(gbMessages, 0, 1);
            tlpMain.Controls.Add(gbActive, 0, 2);
            tlpMain.Controls.Add(lCode, 0, 3);
            tlpMain.Controls.Add(flpButtons, 0, 5);
            tlpMain.Name = "tlpMain";
            //
            // lIntro
            //
            resources.ApplyResources(lIntro, "lIntro");
            lIntro.Name = "lIntro";
            //
            // gbMessages
            //
            resources.ApplyResources(gbMessages, "gbMessages");
            gbMessages.Controls.Add(flpMessages);
            gbMessages.Name = "gbMessages";
            gbMessages.TabStop = false;
            //
            // flpMessages
            //
            resources.ApplyResources(flpMessages, "flpMessages");
            flpMessages.Controls.Add(cboxSpread);
            flpMessages.Controls.Add(cboxDeparture);
            flpMessages.Controls.Add(lCreate);
            flpMessages.Controls.Add(rbCreateNone);
            flpMessages.Controls.Add(rbCreate);
            flpMessages.Controls.Add(rbCreateWithoutCategori);
            flpMessages.Name = "flpMessages";
            //
            // cboxSpread
            //
            resources.ApplyResources(cboxSpread, "cboxSpread");
            cboxSpread.BoxBackColor = System.Drawing.Color.White;
            cboxSpread.HighlightColor = System.Drawing.SystemColors.Highlight;
            cboxSpread.Name = "cboxSpread";
            cboxSpread.UseVisualStyleBackColor = true;
            //
            // cboxDeparture
            //
            resources.ApplyResources(cboxDeparture, "cboxDeparture");
            cboxDeparture.BoxBackColor = System.Drawing.Color.White;
            cboxDeparture.HighlightColor = System.Drawing.SystemColors.Highlight;
            cboxDeparture.Name = "cboxDeparture";
            cboxDeparture.UseVisualStyleBackColor = true;
            //
            // lCreate
            //
            resources.ApplyResources(lCreate, "lCreate");
            lCreate.Name = "lCreate";
            //
            // rbCreateNone
            //
            resources.ApplyResources(rbCreateNone, "rbCreateNone");
            rbCreateNone.HighlightColor = System.Drawing.SystemColors.Highlight;
            rbCreateNone.Name = "rbCreateNone";
            rbCreateNone.TabStop = true;
            rbCreateNone.UseVisualStyleBackColor = true;
            //
            // rbCreate
            //
            resources.ApplyResources(rbCreate, "rbCreate");
            rbCreate.HighlightColor = System.Drawing.SystemColors.Highlight;
            rbCreate.Name = "rbCreate";
            rbCreate.UseVisualStyleBackColor = true;
            //
            // rbCreateWithoutCategori
            //
            resources.ApplyResources(rbCreateWithoutCategori, "rbCreateWithoutCategori");
            rbCreateWithoutCategori.HighlightColor = System.Drawing.SystemColors.Highlight;
            rbCreateWithoutCategori.Name = "rbCreateWithoutCategori";
            rbCreateWithoutCategori.UseVisualStyleBackColor = true;
            //
            // gbActive
            //
            resources.ApplyResources(gbActive, "gbActive");
            gbActive.Controls.Add(flpActive);
            gbActive.Name = "gbActive";
            gbActive.TabStop = false;
            //
            // flpActive
            //
            resources.ApplyResources(flpActive, "flpActive");
            flpActive.Controls.Add(cbSwitch);
            flpActive.Controls.Add(lInactive);
            flpActive.Name = "flpActive";
            //
            // cbSwitch
            //
            cbSwitch.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            cbSwitch.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSwitch.FormattingEnabled = true;
            resources.ApplyResources(cbSwitch, "cbSwitch");
            cbSwitch.Name = "cbSwitch";
            cbSwitch.UseDarkScrollBar = false;
            //
            // lInactive
            //
            resources.ApplyResources(lInactive, "lInactive");
            lInactive.Name = "lInactive";
            //
            // lCode
            //
            resources.ApplyResources(lCode, "lCode");
            lCode.Name = "lCode";
            //
            // flpButtons
            //
            resources.ApplyResources(flpButtons, "flpButtons");
            flpButtons.Controls.Add(bCancel);
            flpButtons.Controls.Add(bOK);
            flpButtons.Name = "flpButtons";
            //
            // bCancel
            //
            resources.ApplyResources(bCancel, "bCancel");
            bCancel.DialogResult = DialogResult.Cancel;
            bCancel.Name = "bCancel";
            bCancel.UseVisualStyleBackColor = true;
            //
            // bOK
            //
            resources.ApplyResources(bOK, "bOK");
            bOK.DialogResult = DialogResult.OK;
            bOK.Name = "bOK";
            bOK.UseVisualStyleBackColor = true;
            //
            // FDirListFlags
            //
            AcceptButton = bOK;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = bCancel;
            Controls.Add(tlpMain);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FDirListFlags";
            ShowIcon = false;
            ShowInTaskbar = false;
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            gbMessages.ResumeLayout(false);
            gbMessages.PerformLayout();
            flpMessages.ResumeLayout(false);
            flpMessages.PerformLayout();
            gbActive.ResumeLayout(false);
            gbActive.PerformLayout();
            flpActive.ResumeLayout(false);
            flpActive.PerformLayout();
            flpButtons.ResumeLayout(false);
            flpButtons.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lIntro;
        private ExGroupBox gbMessages;
        private FlowLayoutPanel flpMessages;
        private ExCheckBox cboxSpread;
        private ExCheckBox cboxDeparture;
        private Label lCreate;
        private ExRadioButton rbCreateNone;
        private ExRadioButton rbCreate;
        private ExRadioButton rbCreateWithoutCategori;
        private ExGroupBox gbActive;
        private FlowLayoutPanel flpActive;
        private ExComboBox cbSwitch;
        private Label lInactive;
        private Label lCode;
        private FlowLayoutPanel flpButtons;
        private ExButton bCancel;
        private ExButton bOK;
    }
}
