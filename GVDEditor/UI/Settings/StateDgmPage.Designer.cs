using ExControls;

namespace GVDEditor.UI.Settings
{
    partial class StateDgmPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(StateDgmPage));
            tlpMain = new TableLayoutPanel();
            lInfo = new Label();
            lStatus = new Label();
            bOpen = new ExButton();
            tlpMain.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lInfo, 0, 0);
            tlpMain.Controls.Add(lStatus, 0, 1);
            tlpMain.Controls.Add(bOpen, 0, 2);
            tlpMain.Name = "tlpMain";
            // 
            // lInfo
            // 
            resources.ApplyResources(lInfo, "lInfo");
            lInfo.Name = "lInfo";
            // 
            // lStatus
            // 
            resources.ApplyResources(lStatus, "lStatus");
            lStatus.Name = "lStatus";
            // 
            // bOpen
            // 
            resources.ApplyResources(bOpen, "bOpen");
            bOpen.Name = "bOpen";
            bOpen.UseVisualStyleBackColor = true;
            bOpen.Click += bOpen_Click;
            // 
            // StateDgmPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "StateDgmPage";
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lInfo;
        private Label lStatus;
        private ExButton bOpen;
    }
}
