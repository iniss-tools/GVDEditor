using ExControls;
using GVDEditor.Controls;

namespace GVDEditor.Forms
{
    partial class FTableFontPicker
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FTableFontPicker));
            tlpMain = new TableLayoutPanel();
            picker = new TableFontPicker();
            lExisting = new Label();
            tlpButtons = new TableLayoutPanel();
            cbAddToList = new ExCheckBox();
            bOK = new ExButton();
            bStorno = new ExButton();
            tlpMain.SuspendLayout();
            tlpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(picker, 0, 0);
            tlpMain.Controls.Add(lExisting, 0, 1);
            tlpMain.Controls.Add(tlpButtons, 0, 2);
            tlpMain.Name = "tlpMain";
            // 
            // picker
            // 
            resources.ApplyResources(picker, "picker");
            picker.Name = "picker";
            picker.ValueChanged += picker_ValueChanged;
            // 
            // lExisting
            // 
            resources.ApplyResources(lExisting, "lExisting");
            lExisting.Name = "lExisting";
            // 
            // tlpButtons
            // 
            resources.ApplyResources(tlpButtons, "tlpButtons");
            tlpButtons.Controls.Add(cbAddToList, 0, 0);
            tlpButtons.Controls.Add(bOK, 1, 0);
            tlpButtons.Controls.Add(bStorno, 2, 0);
            tlpButtons.Name = "tlpButtons";
            // 
            // cbAddToList
            // 
            resources.ApplyResources(cbAddToList, "cbAddToList");
            cbAddToList.Name = "cbAddToList";
            cbAddToList.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            resources.ApplyResources(bOK, "bOK");
            bOK.DialogResult = DialogResult.OK;
            bOK.Name = "bOK";
            bOK.UseVisualStyleBackColor = true;
            // 
            // bStorno
            // 
            resources.ApplyResources(bStorno, "bStorno");
            bStorno.DialogResult = DialogResult.Cancel;
            bStorno.Name = "bStorno";
            bStorno.UseVisualStyleBackColor = true;
            // 
            // FTableFontPicker
            // 
            resources.ApplyResources(this, "$this");
            AcceptButton = bOK;
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = bStorno;
            Controls.Add(tlpMain);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FTableFontPicker";
            ShowIcon = false;
            ShowInTaskbar = false;
            tlpButtons.ResumeLayout(false);
            tlpButtons.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private TableFontPicker picker;
        private Label lExisting;
        private TableLayoutPanel tlpButtons;
        private ExCheckBox cbAddToList;
        private ExButton bOK;
        private ExButton bStorno;
    }
}
