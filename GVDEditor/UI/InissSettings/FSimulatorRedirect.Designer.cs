using ExControls;

namespace GVDEditor.UI.InissSettings
{
    partial class FSimulatorRedirect
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
            var resources = new ComponentResourceManager(typeof(FSimulatorRedirect));
            tlpMain = new TableLayoutPanel();
            lIntro = new Label();
            tlpTarget = new TableLayoutPanel();
            lHost = new Label();
            tbHost = new ExTextBox();
            lBasePort = new Label();
            tbBasePort = new ExTextBox();
            lUrl = new Label();
            tbUrl = new ExTextBox();
            bProbe = new ExButton();
            lApiKey = new Label();
            tbApiKey = new ExTextBox();
            cboxPrepare = new ExCheckBox();
            lStatus = new Label();
            dgvLines = new DataGridView();
            cDo = new DataGridViewCheckBoxColumn();
            cLine = new DataGridViewTextBoxColumn();
            cSection = new DataGridViewTextBoxColumn();
            cClass = new DataGridViewTextBoxColumn();
            cTables = new DataGridViewTextBoxColumn();
            cPort = new DataGridViewTextBoxColumn();
            cNew = new DataGridViewTextBoxColumn();
            cNote = new DataGridViewTextBoxColumn();
            flpButtons = new FlowLayoutPanel();
            bClose = new ExButton();
            bApply = new ExButton();
            bUndo = new ExButton();
            ((ISupportInitialize)dgvLines).BeginInit();
            tlpMain.SuspendLayout();
            tlpTarget.SuspendLayout();
            flpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lIntro, 0, 0);
            tlpMain.Controls.Add(tlpTarget, 0, 1);
            tlpMain.Controls.Add(lStatus, 0, 2);
            tlpMain.Controls.Add(dgvLines, 0, 3);
            tlpMain.Controls.Add(flpButtons, 0, 4);
            tlpMain.Name = "tlpMain";
            // 
            // lIntro
            // 
            resources.ApplyResources(lIntro, "lIntro");
            lIntro.Name = "lIntro";
            // 
            // tlpTarget
            // 
            resources.ApplyResources(tlpTarget, "tlpTarget");
            tlpTarget.Controls.Add(lHost, 0, 0);
            tlpTarget.Controls.Add(tbHost, 1, 0);
            tlpTarget.Controls.Add(lBasePort, 2, 0);
            tlpTarget.Controls.Add(tbBasePort, 3, 0);
            tlpTarget.Controls.Add(lUrl, 0, 1);
            tlpTarget.Controls.Add(tbUrl, 1, 1);
            tlpTarget.Controls.Add(bProbe, 2, 1);
            tlpTarget.SetColumnSpan(bProbe, 2);
            tlpTarget.Controls.Add(lApiKey, 0, 2);
            tlpTarget.Controls.Add(tbApiKey, 1, 2);
            tlpTarget.Controls.Add(cboxPrepare, 0, 3);
            tlpTarget.SetColumnSpan(cboxPrepare, 4);
            tlpTarget.Name = "tlpTarget";
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
            // lBasePort
            // 
            resources.ApplyResources(lBasePort, "lBasePort");
            lBasePort.Name = "lBasePort";
            // 
            // tbBasePort
            // 
            resources.ApplyResources(tbBasePort, "tbBasePort");
            tbBasePort.Name = "tbBasePort";
            // 
            // lUrl
            // 
            resources.ApplyResources(lUrl, "lUrl");
            lUrl.Name = "lUrl";
            // 
            // tbUrl
            // 
            resources.ApplyResources(tbUrl, "tbUrl");
            tbUrl.Name = "tbUrl";
            // 
            // bProbe
            // 
            resources.ApplyResources(bProbe, "bProbe");
            bProbe.UseVisualStyleBackColor = true;
            bProbe.Name = "bProbe";
            // 
            // lApiKey
            // 
            resources.ApplyResources(lApiKey, "lApiKey");
            lApiKey.Name = "lApiKey";
            // 
            // tbApiKey
            // 
            resources.ApplyResources(tbApiKey, "tbApiKey");
            tbApiKey.PasswordChar = '●';
            tbApiKey.Name = "tbApiKey";
            // 
            // cboxPrepare
            // 
            resources.ApplyResources(cboxPrepare, "cboxPrepare");
            cboxPrepare.UseVisualStyleBackColor = true;
            cboxPrepare.Name = "cboxPrepare";
            // 
            // lStatus
            // 
            resources.ApplyResources(lStatus, "lStatus");
            lStatus.Name = "lStatus";
            // 
            // dgvLines
            // 
            resources.ApplyResources(dgvLines, "dgvLines");
            dgvLines.Columns.AddRange(new DataGridViewColumn[] { cDo, cLine, cSection, cClass, cTables, cPort, cNew, cNote });
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.AllowUserToResizeRows = false;
            dgvLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLines.RowHeadersVisible = false;
            dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Name = "dgvLines";
            // 
            // cDo
            // 
            resources.ApplyResources(cDo, "cDo");
            cDo.SortMode = DataGridViewColumnSortMode.NotSortable;
            cDo.Name = "cDo";
            // 
            // cLine
            // 
            resources.ApplyResources(cLine, "cLine");
            cLine.ReadOnly = true;
            cLine.SortMode = DataGridViewColumnSortMode.NotSortable;
            cLine.Name = "cLine";
            // 
            // cSection
            // 
            resources.ApplyResources(cSection, "cSection");
            cSection.ReadOnly = true;
            cSection.SortMode = DataGridViewColumnSortMode.NotSortable;
            cSection.Name = "cSection";
            // 
            // cClass
            // 
            resources.ApplyResources(cClass, "cClass");
            cClass.ReadOnly = true;
            cClass.SortMode = DataGridViewColumnSortMode.NotSortable;
            cClass.Name = "cClass";
            // 
            // cTables
            // 
            resources.ApplyResources(cTables, "cTables");
            cTables.ReadOnly = true;
            cTables.SortMode = DataGridViewColumnSortMode.NotSortable;
            cTables.Name = "cTables";
            // 
            // cPort
            // 
            resources.ApplyResources(cPort, "cPort");
            cPort.ReadOnly = true;
            cPort.SortMode = DataGridViewColumnSortMode.NotSortable;
            cPort.Name = "cPort";
            // 
            // cNew
            // 
            resources.ApplyResources(cNew, "cNew");
            cNew.ReadOnly = true;
            cNew.SortMode = DataGridViewColumnSortMode.NotSortable;
            cNew.Name = "cNew";
            // 
            // cNote
            // 
            resources.ApplyResources(cNote, "cNote");
            cNote.ReadOnly = true;
            cNote.SortMode = DataGridViewColumnSortMode.NotSortable;
            cNote.Name = "cNote";
            // 
            // flpButtons
            // 
            resources.ApplyResources(flpButtons, "flpButtons");
            flpButtons.Controls.Add(bClose);
            flpButtons.Controls.Add(bApply);
            flpButtons.Controls.Add(bUndo);
            flpButtons.Name = "flpButtons";
            // 
            // bClose
            // 
            resources.ApplyResources(bClose, "bClose");
            bClose.UseVisualStyleBackColor = true;
            bClose.DialogResult = DialogResult.Cancel;
            bClose.Name = "bClose";
            // 
            // bApply
            // 
            resources.ApplyResources(bApply, "bApply");
            bApply.UseVisualStyleBackColor = true;
            bApply.Name = "bApply";
            // 
            // bUndo
            // 
            resources.ApplyResources(bUndo, "bUndo");
            bUndo.UseVisualStyleBackColor = true;
            bUndo.Name = "bUndo";
            // 
            // FSimulatorRedirect
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            CancelButton = bClose;
            Name = "FSimulatorRedirect";
            flpButtons.ResumeLayout(false);
            flpButtons.PerformLayout();
            tlpTarget.ResumeLayout(false);
            tlpTarget.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ((ISupportInitialize)dgvLines).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lIntro;
        private TableLayoutPanel tlpTarget;
        private Label lHost;
        private ExTextBox tbHost;
        private Label lBasePort;
        private ExTextBox tbBasePort;
        private Label lUrl;
        private ExTextBox tbUrl;
        private ExButton bProbe;
        private Label lApiKey;
        private ExTextBox tbApiKey;
        private ExCheckBox cboxPrepare;
        private Label lStatus;
        private DataGridView dgvLines;
        private DataGridViewCheckBoxColumn cDo;
        private DataGridViewTextBoxColumn cLine;
        private DataGridViewTextBoxColumn cSection;
        private DataGridViewTextBoxColumn cClass;
        private DataGridViewTextBoxColumn cTables;
        private DataGridViewTextBoxColumn cPort;
        private DataGridViewTextBoxColumn cNew;
        private DataGridViewTextBoxColumn cNote;
        private FlowLayoutPanel flpButtons;
        private ExButton bClose;
        private ExButton bApply;
        private ExButton bUndo;
    }
}
