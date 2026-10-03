using ExControls;

namespace GVDEditor.UI.InissSettings
{
    partial class FDriverWizard
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
            var resources = new ComponentResourceManager(typeof(FDriverWizard));
            tlpMain = new TableLayoutPanel();
            lIntro = new Label();
            lSection = new Label();
            cbSection = new ExComboBox();
            lClass = new Label();
            cbClass = new ExComboBox();
            lConnection = new Label();
            cbConnection = new ExComboBox();
            lComPort = new Label();
            cbComPort = new ExComboBox();
            lParams = new Label();
            tbParams = new ExTextBox();
            lHost = new Label();
            tbHost = new ExTextBox();
            lNetPort = new Label();
            nudNetPort = new ExNumericUpDown();
            lPipe = new Label();
            tbPipe = new ExTextBox();
            lLine = new Label();
            nudLine = new ExNumericUpDown();
            lTarget = new Label();
            cbTarget = new ExComboBox();
            lPreview = new Label();
            flpButtons = new FlowLayoutPanel();
            bCancel = new ExButton();
            bOK = new ExButton();
            tlpMain.SuspendLayout();
            flpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lIntro, 0, 0);
            tlpMain.SetColumnSpan(lIntro, 2);
            tlpMain.Controls.Add(lSection, 0, 1);
            tlpMain.Controls.Add(cbSection, 1, 1);
            tlpMain.Controls.Add(lClass, 0, 2);
            tlpMain.Controls.Add(cbClass, 1, 2);
            tlpMain.Controls.Add(lConnection, 0, 3);
            tlpMain.Controls.Add(cbConnection, 1, 3);
            tlpMain.Controls.Add(lComPort, 0, 4);
            tlpMain.Controls.Add(cbComPort, 1, 4);
            tlpMain.Controls.Add(lParams, 0, 5);
            tlpMain.Controls.Add(tbParams, 1, 5);
            tlpMain.Controls.Add(lHost, 0, 6);
            tlpMain.Controls.Add(tbHost, 1, 6);
            tlpMain.Controls.Add(lNetPort, 0, 7);
            tlpMain.Controls.Add(nudNetPort, 1, 7);
            tlpMain.Controls.Add(lPipe, 0, 8);
            tlpMain.Controls.Add(tbPipe, 1, 8);
            tlpMain.Controls.Add(lLine, 0, 9);
            tlpMain.Controls.Add(nudLine, 1, 9);
            tlpMain.Controls.Add(lTarget, 0, 10);
            tlpMain.Controls.Add(cbTarget, 1, 10);
            tlpMain.Controls.Add(lPreview, 0, 11);
            tlpMain.SetColumnSpan(lPreview, 2);
            tlpMain.Controls.Add(flpButtons, 0, 13);
            tlpMain.SetColumnSpan(flpButtons, 2);
            tlpMain.Name = "tlpMain";
            // 
            // lIntro
            // 
            resources.ApplyResources(lIntro, "lIntro");
            lIntro.Name = "lIntro";
            // 
            // lSection
            // 
            resources.ApplyResources(lSection, "lSection");
            lSection.Name = "lSection";
            // 
            // cbSection
            // 
            resources.ApplyResources(cbSection, "cbSection");
            cbSection.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSection.FormattingEnabled = true;
            cbSection.Name = "cbSection";
            // 
            // lClass
            // 
            resources.ApplyResources(lClass, "lClass");
            lClass.Name = "lClass";
            // 
            // cbClass
            // 
            resources.ApplyResources(cbClass, "cbClass");
            cbClass.DropDownStyle = ComboBoxStyle.DropDownList;
            cbClass.FormattingEnabled = true;
            cbClass.Name = "cbClass";
            // 
            // lConnection
            // 
            resources.ApplyResources(lConnection, "lConnection");
            lConnection.Name = "lConnection";
            // 
            // cbConnection
            // 
            resources.ApplyResources(cbConnection, "cbConnection");
            cbConnection.DropDownStyle = ComboBoxStyle.DropDownList;
            cbConnection.FormattingEnabled = true;
            cbConnection.Name = "cbConnection";
            // 
            // lComPort
            // 
            resources.ApplyResources(lComPort, "lComPort");
            lComPort.Name = "lComPort";
            // 
            // cbComPort
            // 
            resources.ApplyResources(cbComPort, "cbComPort");
            cbComPort.FormattingEnabled = true;
            cbComPort.Name = "cbComPort";
            // 
            // lParams
            // 
            resources.ApplyResources(lParams, "lParams");
            lParams.Name = "lParams";
            // 
            // tbParams
            // 
            resources.ApplyResources(tbParams, "tbParams");
            tbParams.Name = "tbParams";
            // 
            // lHost
            // 
            resources.ApplyResources(lHost, "lHost");
            lHost.Name = "lHost";
            // 
            // tbHost
            // 
            resources.ApplyResources(tbHost, "tbHost");
            tbHost.Name = "tbHost";
            // 
            // lNetPort
            // 
            resources.ApplyResources(lNetPort, "lNetPort");
            lNetPort.Name = "lNetPort";
            // 
            // nudNetPort
            // 
            resources.ApplyResources(nudNetPort, "nudNetPort");
            nudNetPort.TextAlign = HorizontalAlignment.Right;
            nudNetPort.Name = "nudNetPort";
            // 
            // lPipe
            // 
            resources.ApplyResources(lPipe, "lPipe");
            lPipe.Name = "lPipe";
            // 
            // tbPipe
            // 
            resources.ApplyResources(tbPipe, "tbPipe");
            tbPipe.Name = "tbPipe";
            // 
            // lLine
            // 
            resources.ApplyResources(lLine, "lLine");
            lLine.Name = "lLine";
            // 
            // nudLine
            // 
            resources.ApplyResources(nudLine, "nudLine");
            nudLine.TextAlign = HorizontalAlignment.Right;
            nudLine.Name = "nudLine";
            // 
            // lTarget
            // 
            resources.ApplyResources(lTarget, "lTarget");
            lTarget.Name = "lTarget";
            // 
            // cbTarget
            // 
            resources.ApplyResources(cbTarget, "cbTarget");
            cbTarget.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTarget.FormattingEnabled = true;
            cbTarget.Name = "cbTarget";
            // 
            // lPreview
            // 
            resources.ApplyResources(lPreview, "lPreview");
            lPreview.Name = "lPreview";
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
            bCancel.UseVisualStyleBackColor = true;
            bCancel.Name = "bCancel";
            // 
            // bOK
            // 
            resources.ApplyResources(bOK, "bOK");
            bOK.UseVisualStyleBackColor = true;
            bOK.Name = "bOK";
            // 
            // FDriverWizard
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            AcceptButton = bOK;
            CancelButton = bCancel;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            Name = "FDriverWizard";
            flpButtons.ResumeLayout(false);
            flpButtons.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lIntro;
        private Label lSection;
        private ExComboBox cbSection;
        private Label lClass;
        private ExComboBox cbClass;
        private Label lConnection;
        private ExComboBox cbConnection;
        private Label lComPort;
        private ExComboBox cbComPort;
        private Label lParams;
        private ExTextBox tbParams;
        private Label lHost;
        private ExTextBox tbHost;
        private Label lNetPort;
        private ExNumericUpDown nudNetPort;
        private Label lPipe;
        private ExTextBox tbPipe;
        private Label lLine;
        private ExNumericUpDown nudLine;
        private Label lTarget;
        private ExComboBox cbTarget;
        private Label lPreview;
        private FlowLayoutPanel flpButtons;
        private ExButton bCancel;
        private ExButton bOK;
    }
}
