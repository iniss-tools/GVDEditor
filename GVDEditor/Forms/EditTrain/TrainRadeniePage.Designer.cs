using ExControls;

namespace GVDEditor.Forms.EditTrain
{
    partial class TrainRadeniePage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TrainRadeniePage));
            tlpMain = new TableLayoutPanel();
            lBanner = new Label();
            lListHeader = new Label();
            lDetailHeader = new Label();
            listRadenia = new ListBox();
            tlpDetail = new TableLayoutPanel();
            flpButtons = new FlowLayoutPanel();
            bNew = new ExButton();
            bDuplicate = new ExButton();
            bRemove = new ExButton();
            lHint = new Label();
            lTextLabel = new Label();
            tbText = new ExTextBox();
            flpText = new FlowLayoutPanel();
            bCompose = new ExButton();
            bPlay = new ExButton();
            cbValidity = new ExCheckBox();
            lFrom = new Label();
            dtpFrom = new ExDateTimePicker();
            lTo = new Label();
            dtpTo = new ExDateTimePicker();
            lLimit = new Label();
            flpLimit = new FlowLayoutPanel();
            tbLimit = new ExTextBox();
            bLimit = new ExButton();
            lDest = new Label();
            cbDest = new ExComboBox();
            lWhen = new Label();
            lInfo = new Label();
            matrix = new ReportMatrix();
            tlpMain.SuspendLayout();
            tlpDetail.SuspendLayout();
            flpButtons.SuspendLayout();
            flpText.SuspendLayout();
            flpLimit.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lBanner, 0, 0);
            tlpMain.Controls.Add(lListHeader, 0, 1);
            tlpMain.Controls.Add(lDetailHeader, 1, 1);
            tlpMain.Controls.Add(listRadenia, 0, 2);
            tlpMain.Controls.Add(tlpDetail, 1, 2);
            tlpMain.Controls.Add(flpButtons, 0, 3);
            tlpMain.Controls.Add(lHint, 0, 4);
            tlpMain.Name = "tlpMain";
            tlpMain.SetColumnSpan(lBanner, 2);
            tlpMain.SetColumnSpan(flpButtons, 2);
            tlpMain.SetColumnSpan(lHint, 2);
            // 
            // lBanner
            // 
            resources.ApplyResources(lBanner, "lBanner");
            lBanner.Name = "lBanner";
            // 
            // lListHeader
            // 
            resources.ApplyResources(lListHeader, "lListHeader");
            lListHeader.Name = "lListHeader";
            // 
            // lDetailHeader
            // 
            resources.ApplyResources(lDetailHeader, "lDetailHeader");
            lDetailHeader.Name = "lDetailHeader";
            // 
            // listRadenia
            // 
            resources.ApplyResources(listRadenia, "listRadenia");
            listRadenia.FormattingEnabled = true;
            listRadenia.IntegralHeight = false;
            listRadenia.Name = "listRadenia";
            listRadenia.SelectedIndexChanged += listRadenia_SelectedIndexChanged;
            listRadenia.Format += listRadenia_Format;
            listRadenia.KeyDown += listRadenia_KeyDown;
            // 
            // tlpDetail
            // 
            resources.ApplyResources(tlpDetail, "tlpDetail");
            tlpDetail.Controls.Add(lTextLabel, 0, 0);
            tlpDetail.Controls.Add(tbText, 1, 0);
            tlpDetail.Controls.Add(flpText, 1, 1);
            tlpDetail.Controls.Add(cbValidity, 0, 2);
            tlpDetail.Controls.Add(lFrom, 0, 3);
            tlpDetail.Controls.Add(dtpFrom, 1, 3);
            tlpDetail.Controls.Add(lTo, 0, 4);
            tlpDetail.Controls.Add(dtpTo, 1, 4);
            tlpDetail.Controls.Add(lLimit, 0, 5);
            tlpDetail.Controls.Add(flpLimit, 1, 5);
            tlpDetail.Controls.Add(lDest, 0, 6);
            tlpDetail.Controls.Add(cbDest, 1, 6);
            tlpDetail.Controls.Add(lWhen, 0, 7);
            tlpDetail.Controls.Add(lInfo, 0, 8);
            tlpDetail.Controls.Add(matrix, 0, 9);
            tlpDetail.Name = "tlpDetail";
            tlpDetail.SetColumnSpan(cbValidity, 2);
            tlpDetail.SetColumnSpan(lWhen, 2);
            tlpDetail.SetColumnSpan(lInfo, 2);
            tlpDetail.SetColumnSpan(matrix, 2);
            // 
            // lTextLabel
            // 
            resources.ApplyResources(lTextLabel, "lTextLabel");
            lTextLabel.Name = "lTextLabel";
            // 
            // tbText
            // 
            resources.ApplyResources(tbText, "tbText");
            tbText.Name = "tbText";
            tbText.ScrollBars = ScrollBars.Vertical;
            // 
            // flpText
            // 
            resources.ApplyResources(flpText, "flpText");
            flpText.Controls.Add(bCompose);
            flpText.Controls.Add(bPlay);
            flpText.Name = "flpText";
            // 
            // bCompose
            // 
            resources.ApplyResources(bCompose, "bCompose");
            bCompose.Name = "bCompose";
            bCompose.UseVisualStyleBackColor = true;
            bCompose.Click += bCompose_Click;
            // 
            // bPlay
            // 
            resources.ApplyResources(bPlay, "bPlay");
            bPlay.Name = "bPlay";
            bPlay.UseVisualStyleBackColor = true;
            bPlay.Click += bPlay_Click;
            // 
            // cbValidity
            // 
            resources.ApplyResources(cbValidity, "cbValidity");
            cbValidity.Name = "cbValidity";
            cbValidity.UseVisualStyleBackColor = true;
            cbValidity.CheckedChanged += Detail_Changed;
            // 
            // lFrom
            // 
            resources.ApplyResources(lFrom, "lFrom");
            lFrom.Name = "lFrom";
            // 
            // dtpFrom
            // 
            resources.ApplyResources(dtpFrom, "dtpFrom");
            dtpFrom.Name = "dtpFrom";
            dtpFrom.ValueChanged += Detail_Changed;
            // 
            // lTo
            // 
            resources.ApplyResources(lTo, "lTo");
            lTo.Name = "lTo";
            // 
            // dtpTo
            // 
            resources.ApplyResources(dtpTo, "dtpTo");
            dtpTo.Name = "dtpTo";
            dtpTo.ValueChanged += Detail_Changed;
            // 
            // lLimit
            // 
            resources.ApplyResources(lLimit, "lLimit");
            lLimit.Name = "lLimit";
            // 
            // flpLimit
            // 
            resources.ApplyResources(flpLimit, "flpLimit");
            flpLimit.Controls.Add(tbLimit);
            flpLimit.Controls.Add(bLimit);
            flpLimit.Name = "flpLimit";
            // 
            // tbLimit
            // 
            resources.ApplyResources(tbLimit, "tbLimit");
            tbLimit.Name = "tbLimit";
            tbLimit.TextChanged += Detail_Changed;
            // 
            // bLimit
            // 
            resources.ApplyResources(bLimit, "bLimit");
            bLimit.Name = "bLimit";
            bLimit.UseVisualStyleBackColor = true;
            bLimit.Click += bLimit_Click;
            // 
            // lDest
            // 
            resources.ApplyResources(lDest, "lDest");
            lDest.Name = "lDest";
            // 
            // cbDest
            // 
            resources.ApplyResources(cbDest, "cbDest");
            cbDest.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDest.FormattingEnabled = true;
            cbDest.Name = "cbDest";
            cbDest.SelectionChangeCommitted += Detail_Changed;
            // 
            // lWhen
            // 
            resources.ApplyResources(lWhen, "lWhen");
            lWhen.Name = "lWhen";
            // 
            // lInfo
            // 
            resources.ApplyResources(lInfo, "lInfo");
            lInfo.Name = "lInfo";
            // 
            // matrix
            // 
            resources.ApplyResources(matrix, "matrix");
            matrix.Name = "matrix";
            matrix.Changed += matrix_Changed;
            // 
            // flpButtons
            // 
            resources.ApplyResources(flpButtons, "flpButtons");
            flpButtons.Controls.Add(bNew);
            flpButtons.Controls.Add(bDuplicate);
            flpButtons.Controls.Add(bRemove);
            flpButtons.Name = "flpButtons";
            // 
            // bNew
            // 
            resources.ApplyResources(bNew, "bNew");
            bNew.Name = "bNew";
            bNew.UseVisualStyleBackColor = true;
            bNew.Click += bNew_Click;
            // 
            // bDuplicate
            // 
            resources.ApplyResources(bDuplicate, "bDuplicate");
            bDuplicate.Name = "bDuplicate";
            bDuplicate.UseVisualStyleBackColor = true;
            bDuplicate.Click += bDuplicate_Click;
            // 
            // bRemove
            // 
            resources.ApplyResources(bRemove, "bRemove");
            bRemove.Name = "bRemove";
            bRemove.UseVisualStyleBackColor = true;
            bRemove.Click += bRemove_Click;
            // 
            // lHint
            // 
            resources.ApplyResources(lHint, "lHint");
            lHint.Name = "lHint";
            // 
            // TrainRadeniePage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "TrainRadeniePage";
            flpButtons.ResumeLayout(false);
            flpButtons.PerformLayout();
            flpLimit.ResumeLayout(false);
            flpLimit.PerformLayout();
            flpText.ResumeLayout(false);
            flpText.PerformLayout();
            tlpDetail.ResumeLayout(false);
            tlpDetail.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lBanner;
        private Label lListHeader;
        private Label lDetailHeader;
        private ListBox listRadenia;
        private TableLayoutPanel tlpDetail;
        private FlowLayoutPanel flpButtons;
        private ExButton bNew;
        private ExButton bDuplicate;
        private ExButton bRemove;
        private Label lHint;
        private Label lTextLabel;
        private ExTextBox tbText;
        private FlowLayoutPanel flpText;
        private ExButton bCompose;
        private ExButton bPlay;
        private ExCheckBox cbValidity;
        private Label lFrom;
        private ExDateTimePicker dtpFrom;
        private Label lTo;
        private ExDateTimePicker dtpTo;
        private Label lLimit;
        private FlowLayoutPanel flpLimit;
        private ExTextBox tbLimit;
        private ExButton bLimit;
        private Label lDest;
        private ExComboBox cbDest;
        private Label lWhen;
        private Label lInfo;
        private ReportMatrix matrix;
    }
}
