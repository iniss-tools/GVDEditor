namespace GVDEditor.Forms
{
    partial class FStateDgmItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FStateDgmItem));
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.bCancel = new ExControls.ExButton();
            this.bOk = new ExControls.ExButton();
            this.flpButtons.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlEditor
            //
            resources.ApplyResources(this.pnlEditor, "pnlEditor");
            this.pnlEditor.Name = "pnlEditor";
            //
            // flpButtons
            //
            resources.ApplyResources(this.flpButtons, "flpButtons");
            this.flpButtons.Controls.Add(this.bCancel);
            this.flpButtons.Controls.Add(this.bOk);
            this.flpButtons.Name = "flpButtons";
            //
            // bCancel
            //
            resources.ApplyResources(this.bCancel, "bCancel");
            this.bCancel.DefaultStyle = true;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Name = "bCancel";
            this.bCancel.UseVisualStyleBackColor = true;
            //
            // bOk
            //
            resources.ApplyResources(this.bOk, "bOk");
            this.bOk.DefaultStyle = true;
            this.bOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOk.Name = "bOk";
            this.bOk.UseVisualStyleBackColor = true;
            //
            // FStateDgmItem
            //
            this.AcceptButton = this.bOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.Controls.Add(this.pnlEditor);
            this.Controls.Add(this.flpButtons);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FStateDgmItem";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.flpButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlEditor;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private ExControls.ExButton bCancel;
        private ExControls.ExButton bOk;
    }
}
