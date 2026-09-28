using ExControls;

namespace GVDEditor.UI.Settings
{
    partial class GrafikonLanguagesPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(GrafikonLanguagesPage));
            tlpMain = new TableLayoutPanel();
            lInfo = new Label();
            clbLanguages = new ExCheckedListBox();
            lUsage = new Label();
            tlpMain.SuspendLayout();
            SuspendLayout();
            //
            // tlpMain
            //
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lInfo, 0, 0);
            tlpMain.Controls.Add(clbLanguages, 0, 1);
            tlpMain.Controls.Add(lUsage, 0, 2);
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
            // lUsage
            //
            resources.ApplyResources(lUsage, "lUsage");
            lUsage.Name = "lUsage";
            //
            // GrafikonLanguagesPage
            //
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "GrafikonLanguagesPage";
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lInfo;
        private ExCheckedListBox clbLanguages;
        private Label lUsage;
    }
}
