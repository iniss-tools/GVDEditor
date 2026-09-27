using ExControls;

namespace GVDEditor.Forms.EditTrain
{
    partial class TrainLanguagesPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TrainLanguagesPage));
            tlpMain = new TableLayoutPanel();
            lInfo = new Label();
            clbLanguages = new ExCheckedListBox();
            tlpMain.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lInfo, 0, 0);
            tlpMain.Controls.Add(clbLanguages, 0, 1);
            tlpMain.Name = "tlpMain";
            // 
            // lInfo
            // 
            resources.ApplyResources(lInfo, "lInfo");
            lInfo.Name = "lInfo";
            // 
            // clbLanguages
            // 
            resources.ApplyResources(clbLanguages, "clbLanguages");
            clbLanguages.CheckOnClick = true;
            clbLanguages.FormattingEnabled = true;
            clbLanguages.Name = "clbLanguages";
            clbLanguages.ItemCheck += clbLanguages_ItemCheck;
            // 
            // TrainLanguagesPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "TrainLanguagesPage";
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lInfo;
        private ExCheckedListBox clbLanguages;
    }
}
