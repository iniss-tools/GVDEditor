using ExControls;

namespace GVDEditor.UI.InissSettings
{
    partial class FCloneBranch
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
            var resources = new ComponentResourceManager(typeof(FCloneBranch));
            tlpMain = new TableLayoutPanel();
            lIntro = new Label();
            lName = new Label();
            tbName = new ExTextBox();
            lError = new Label();
            cboxRunConfig = new ExCheckBox();
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
            tlpMain.Controls.Add(lName, 0, 1);
            tlpMain.Controls.Add(tbName, 1, 1);
            tlpMain.Controls.Add(lError, 1, 2);
            tlpMain.Controls.Add(cboxRunConfig, 1, 3);
            tlpMain.Controls.Add(flpButtons, 0, 5);
            tlpMain.SetColumnSpan(flpButtons, 2);
            tlpMain.Name = "tlpMain";
            // 
            // lIntro
            // 
            resources.ApplyResources(lIntro, "lIntro");
            lIntro.Name = "lIntro";
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
            // 
            // lError
            // 
            resources.ApplyResources(lError, "lError");
            lError.Name = "lError";
            // 
            // cboxRunConfig
            // 
            resources.ApplyResources(cboxRunConfig, "cboxRunConfig");
            cboxRunConfig.UseVisualStyleBackColor = true;
            cboxRunConfig.Checked = true;
            cboxRunConfig.CheckState = CheckState.Checked;
            cboxRunConfig.Name = "cboxRunConfig";
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
            bCancel.UseVisualStyleBackColor = true;
            bCancel.DialogResult = DialogResult.Cancel;
            bCancel.Name = "bCancel";
            // 
            // bOK
            // 
            resources.ApplyResources(bOK, "bOK");
            bOK.UseVisualStyleBackColor = true;
            bOK.Name = "bOK";
            // 
            // FCloneBranch
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            AcceptButton = bOK;
            CancelButton = bCancel;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "FCloneBranch";
            flpButtons.ResumeLayout(false);
            flpButtons.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lIntro;
        private Label lName;
        private ExTextBox tbName;
        private Label lError;
        private ExCheckBox cboxRunConfig;
        private FlowLayoutPanel flpButtons;
        private ExButton bCancel;
        private ExButton bOK;
    }
}
