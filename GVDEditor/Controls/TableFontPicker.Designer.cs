using ExControls;

namespace GVDEditor.Controls
{
    partial class TableFontPicker
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TableFontPicker));
            tlp = new TableLayoutPanel();
            lFace = new Label();
            flpFace = new FlowLayoutPanel();
            rbFace0 = new ExRadioButton();
            rbFace1 = new ExRadioButton();
            rbFace2 = new ExRadioButton();
            rbFace3 = new ExRadioButton();
            rbExt = new ExRadioButton();
            nudExt = new ExNumericUpDown();
            lColor = new Label();
            flpColor = new FlowLayoutPanel();
            rbColor0 = new ExRadioButton();
            rbColor1 = new ExRadioButton();
            rbColor2 = new ExRadioButton();
            rbColor3 = new ExRadioButton();
            lEffects = new Label();
            flpEffects = new FlowLayoutPanel();
            cbBlink = new ExCheckBox();
            cbTall = new ExCheckBox();
            led = new LedPreview();
            lResult = new Label();
            flpManual = new FlowLayoutPanel();
            llManual = new LinkLabel();
            nudId = new ExNumericUpDown();
            lNote = new Label();
            ((ISupportInitialize)nudExt).BeginInit();
            ((ISupportInitialize)nudId).BeginInit();
            tlp.SuspendLayout();
            flpFace.SuspendLayout();
            flpColor.SuspendLayout();
            flpEffects.SuspendLayout();
            flpManual.SuspendLayout();
            SuspendLayout();
            // 
            // tlp
            // 
            resources.ApplyResources(tlp, "tlp");
            tlp.Controls.Add(lFace, 0, 0);
            tlp.Controls.Add(flpFace, 1, 0);
            tlp.Controls.Add(lColor, 0, 1);
            tlp.Controls.Add(flpColor, 1, 1);
            tlp.Controls.Add(lEffects, 0, 2);
            tlp.Controls.Add(flpEffects, 1, 2);
            tlp.Controls.Add(led, 0, 3);
            tlp.Controls.Add(lResult, 0, 4);
            tlp.Controls.Add(flpManual, 0, 5);
            tlp.Controls.Add(lNote, 0, 6);
            tlp.Name = "tlp";
            tlp.SetColumnSpan(led, 2);
            tlp.SetColumnSpan(lResult, 2);
            tlp.SetColumnSpan(flpManual, 2);
            tlp.SetColumnSpan(lNote, 2);
            // 
            // lFace
            // 
            resources.ApplyResources(lFace, "lFace");
            lFace.Name = "lFace";
            // 
            // flpFace
            // 
            resources.ApplyResources(flpFace, "flpFace");
            flpFace.Controls.Add(rbFace0);
            flpFace.Controls.Add(rbFace1);
            flpFace.Controls.Add(rbFace2);
            flpFace.Controls.Add(rbFace3);
            flpFace.Controls.Add(rbExt);
            flpFace.Controls.Add(nudExt);
            flpFace.Name = "flpFace";
            // 
            // rbFace0
            // 
            resources.ApplyResources(rbFace0, "rbFace0");
            rbFace0.Name = "rbFace0";
            rbFace0.UseVisualStyleBackColor = true;
            rbFace0.CheckedChanged += Part_Changed;
            // 
            // rbFace1
            // 
            resources.ApplyResources(rbFace1, "rbFace1");
            rbFace1.Name = "rbFace1";
            rbFace1.UseVisualStyleBackColor = true;
            rbFace1.CheckedChanged += Part_Changed;
            // 
            // rbFace2
            // 
            resources.ApplyResources(rbFace2, "rbFace2");
            rbFace2.Name = "rbFace2";
            rbFace2.UseVisualStyleBackColor = true;
            rbFace2.CheckedChanged += Part_Changed;
            // 
            // rbFace3
            // 
            resources.ApplyResources(rbFace3, "rbFace3");
            rbFace3.Name = "rbFace3";
            rbFace3.UseVisualStyleBackColor = true;
            rbFace3.CheckedChanged += Part_Changed;
            // 
            // rbExt
            // 
            resources.ApplyResources(rbExt, "rbExt");
            rbExt.Name = "rbExt";
            rbExt.UseVisualStyleBackColor = true;
            rbExt.CheckedChanged += Part_Changed;
            // 
            // nudExt
            // 
            resources.ApplyResources(nudExt, "nudExt");
            nudExt.Maximum = new decimal(new int[] { 15, 0, 0, 0 });
            nudExt.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudExt.Name = "nudExt";
            nudExt.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudExt.ValueChanged += Part_Changed;
            // 
            // lColor
            // 
            resources.ApplyResources(lColor, "lColor");
            lColor.Name = "lColor";
            // 
            // flpColor
            // 
            resources.ApplyResources(flpColor, "flpColor");
            flpColor.Controls.Add(rbColor0);
            flpColor.Controls.Add(rbColor1);
            flpColor.Controls.Add(rbColor2);
            flpColor.Controls.Add(rbColor3);
            flpColor.Name = "flpColor";
            // 
            // rbColor0
            // 
            resources.ApplyResources(rbColor0, "rbColor0");
            rbColor0.Name = "rbColor0";
            rbColor0.UseVisualStyleBackColor = true;
            rbColor0.CheckedChanged += Part_Changed;
            // 
            // rbColor1
            // 
            resources.ApplyResources(rbColor1, "rbColor1");
            rbColor1.Name = "rbColor1";
            rbColor1.UseVisualStyleBackColor = true;
            rbColor1.CheckedChanged += Part_Changed;
            // 
            // rbColor2
            // 
            resources.ApplyResources(rbColor2, "rbColor2");
            rbColor2.Name = "rbColor2";
            rbColor2.UseVisualStyleBackColor = true;
            rbColor2.CheckedChanged += Part_Changed;
            // 
            // rbColor3
            // 
            resources.ApplyResources(rbColor3, "rbColor3");
            rbColor3.Name = "rbColor3";
            rbColor3.UseVisualStyleBackColor = true;
            rbColor3.CheckedChanged += Part_Changed;
            // 
            // lEffects
            // 
            resources.ApplyResources(lEffects, "lEffects");
            lEffects.Name = "lEffects";
            // 
            // flpEffects
            // 
            resources.ApplyResources(flpEffects, "flpEffects");
            flpEffects.Controls.Add(cbBlink);
            flpEffects.Controls.Add(cbTall);
            flpEffects.Name = "flpEffects";
            // 
            // cbBlink
            // 
            resources.ApplyResources(cbBlink, "cbBlink");
            cbBlink.Name = "cbBlink";
            cbBlink.UseVisualStyleBackColor = true;
            cbBlink.CheckedChanged += Part_Changed;
            // 
            // cbTall
            // 
            resources.ApplyResources(cbTall, "cbTall");
            cbTall.Name = "cbTall";
            cbTall.UseVisualStyleBackColor = true;
            cbTall.CheckedChanged += Part_Changed;
            // 
            // led
            // 
            resources.ApplyResources(led, "led");
            led.Name = "led";
            // 
            // lResult
            // 
            resources.ApplyResources(lResult, "lResult");
            lResult.Name = "lResult";
            // 
            // flpManual
            // 
            resources.ApplyResources(flpManual, "flpManual");
            flpManual.Controls.Add(llManual);
            flpManual.Controls.Add(nudId);
            flpManual.Name = "flpManual";
            // 
            // llManual
            // 
            resources.ApplyResources(llManual, "llManual");
            llManual.Name = "llManual";
            llManual.TabStop = true;
            llManual.LinkClicked += llManual_LinkClicked;
            // 
            // nudId
            // 
            resources.ApplyResources(nudId, "nudId");
            nudId.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nudId.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            nudId.Name = "nudId";
            nudId.Value = new decimal(new int[] { 80, 0, 0, 0 });
            nudId.ValueChanged += nudId_ValueChanged;
            // 
            // lNote
            // 
            resources.ApplyResources(lNote, "lNote");
            lNote.Name = "lNote";
            // 
            // TableFontPicker
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlp);
            Name = "TableFontPicker";
            ((ISupportInitialize)nudExt).EndInit();
            ((ISupportInitialize)nudId).EndInit();
            flpManual.ResumeLayout(false);
            flpManual.PerformLayout();
            flpEffects.ResumeLayout(false);
            flpEffects.PerformLayout();
            flpColor.ResumeLayout(false);
            flpColor.PerformLayout();
            flpFace.ResumeLayout(false);
            flpFace.PerformLayout();
            tlp.ResumeLayout(false);
            tlp.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlp;
        private Label lFace;
        private FlowLayoutPanel flpFace;
        private ExRadioButton rbFace0;
        private ExRadioButton rbFace1;
        private ExRadioButton rbFace2;
        private ExRadioButton rbFace3;
        private ExRadioButton rbExt;
        private ExNumericUpDown nudExt;
        private Label lColor;
        private FlowLayoutPanel flpColor;
        private ExRadioButton rbColor0;
        private ExRadioButton rbColor1;
        private ExRadioButton rbColor2;
        private ExRadioButton rbColor3;
        private Label lEffects;
        private FlowLayoutPanel flpEffects;
        private ExCheckBox cbBlink;
        private ExCheckBox cbTall;
        private LedPreview led;
        private Label lResult;
        private FlowLayoutPanel flpManual;
        private LinkLabel llManual;
        private ExNumericUpDown nudId;
        private Label lNote;
    }
}
