using ExControls;

namespace GVDEditor.Forms
{
    partial class FImportData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FImportData));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.bStorno = new ExControls.ExButton();
            this.bImport = new ExControls.ExButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.cbEncoding = new ExControls.ExComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.cbDataType = new ExControls.ExComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.bClipboard = new ExControls.ExButton();
            this.bCSV = new ExControls.ExButton();
            this.bXLS = new ExControls.ExButton();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new ExControls.ExGroupBox();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.rbRemoveAndInsert = new ExControls.ExRadioButton();
            this.rbAppend = new ExControls.ExRadioButton();
            this.cboxFirstHeader = new ExControls.ExCheckBox();
            this.ofDialogCSV = new System.Windows.Forms.OpenFileDialog();
            this.ofDialogXLS = new System.Windows.Forms.OpenFileDialog();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.bStorno);
            this.flowLayoutPanel1.Controls.Add(this.bImport);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // bStorno
            // 
            resources.ApplyResources(this.bStorno, "bStorno");
            this.bStorno.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bStorno.Name = "bStorno";
            this.bStorno.UseVisualStyleBackColor = true;
            this.bStorno.Click += new System.EventHandler(this.bStorno_Click);
            // 
            // bImport
            // 
            resources.ApplyResources(this.bImport, "bImport");
            this.bImport.Name = "bImport";
            this.bImport.UseVisualStyleBackColor = true;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel5, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // flowLayoutPanel2
            // 
            resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
            this.flowLayoutPanel2.Controls.Add(this.cbEncoding);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            // 
            // cbEncoding
            // 
            this.cbEncoding.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEncoding.FormattingEnabled = true;
            this.cbEncoding.Items.AddRange(new object[] {
            resources.GetString("cbEncoding.Items"),
            resources.GetString("cbEncoding.Items1")});
            resources.ApplyResources(this.cbEncoding, "cbEncoding");
            this.cbEncoding.Name = "cbEncoding";
            this.cbEncoding.UseDarkScrollBar = false;
            this.cbEncoding.SelectedIndexChanged += new System.EventHandler(this.cbEncoding_SelectedIndexChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // flowLayoutPanel5
            // 
            resources.ApplyResources(this.flowLayoutPanel5, "flowLayoutPanel5");
            this.flowLayoutPanel5.Controls.Add(this.cbDataType);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            // 
            // cbDataType
            // 
            resources.ApplyResources(this.cbDataType, "cbDataType");
            this.cbDataType.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataType.FormattingEnabled = true;
            this.cbDataType.Items.AddRange(new object[] {
            resources.GetString("cbDataType.Items")});
            this.cbDataType.Name = "cbDataType";
            this.cbDataType.UseDarkScrollBar = false;
            this.cbDataType.SelectedIndexChanged += new System.EventHandler(this.cbDataType_SelectedIndexChanged);
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.dgvData, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.cboxFirstHeader, 1, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // flowLayoutPanel3
            // 
            resources.ApplyResources(this.flowLayoutPanel3, "flowLayoutPanel3");
            this.flowLayoutPanel3.Controls.Add(this.bClipboard);
            this.flowLayoutPanel3.Controls.Add(this.bCSV);
            this.flowLayoutPanel3.Controls.Add(this.bXLS);
            this.flowLayoutPanel3.Controls.Add(this.flowLayoutPanel6);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            // 
            // bClipboard
            // 
            resources.ApplyResources(this.bClipboard, "bClipboard");
            this.bClipboard.Name = "bClipboard";
            this.bClipboard.UseVisualStyleBackColor = true;
            this.bClipboard.Click += new System.EventHandler(this.bClipboard_Click);
            // 
            // bCSV
            // 
            resources.ApplyResources(this.bCSV, "bCSV");
            this.bCSV.Name = "bCSV";
            this.bCSV.UseVisualStyleBackColor = true;
            this.bCSV.Click += new System.EventHandler(this.bCSV_Click);
            // 
            // bXLS
            // 
            resources.ApplyResources(this.bXLS, "bXLS");
            this.bXLS.Name = "bXLS";
            this.bXLS.UseVisualStyleBackColor = true;
            this.bXLS.Click += new System.EventHandler(this.bXLS_Click);
            // 
            // flowLayoutPanel6
            // 
            resources.ApplyResources(this.flowLayoutPanel6, "flowLayoutPanel6");
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            // 
            // dgvData
            // 
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel2.SetColumnSpan(this.dgvData, 2);
            resources.ApplyResources(this.dgvData, "dgvData");
            this.dgvData.Name = "dgvData";
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_ColumnHeaderMouseClick);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.tableLayoutPanel2.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.flowLayoutPanel4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // flowLayoutPanel4
            // 
            resources.ApplyResources(this.flowLayoutPanel4, "flowLayoutPanel4");
            this.flowLayoutPanel4.Controls.Add(this.rbRemoveAndInsert);
            this.flowLayoutPanel4.Controls.Add(this.rbAppend);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            // 
            // rbRemoveAndInsert
            // 
            resources.ApplyResources(this.rbRemoveAndInsert, "rbRemoveAndInsert");
            this.rbRemoveAndInsert.Checked = true;
            this.rbRemoveAndInsert.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbRemoveAndInsert.Name = "rbRemoveAndInsert";
            this.rbRemoveAndInsert.TabStop = true;
            this.rbRemoveAndInsert.UseVisualStyleBackColor = true;
            // 
            // rbAppend
            // 
            resources.ApplyResources(this.rbAppend, "rbAppend");
            this.rbAppend.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.rbAppend.Name = "rbAppend";
            this.rbAppend.UseVisualStyleBackColor = true;
            // 
            // cboxFirstHeader
            // 
            resources.ApplyResources(this.cboxFirstHeader, "cboxFirstHeader");
            this.cboxFirstHeader.BoxBackColor = System.Drawing.Color.White;
            this.cboxFirstHeader.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.cboxFirstHeader.Name = "cboxFirstHeader";
            this.cboxFirstHeader.UseVisualStyleBackColor = true;
            this.cboxFirstHeader.CheckedChanged += new System.EventHandler(this.cboxFirstHeader_CheckedChanged);
            // 
            // ofDialogCSV
            // 
            resources.ApplyResources(this.ofDialogCSV, "ofDialogCSV");
            // 
            // ofDialogXLS
            // 
            resources.ApplyResources(this.ofDialogXLS, "ofDialogXLS");
            // 
            // FImportData
            // 
            this.AcceptButton = this.bImport;
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.MinimizeBox = false;
            this.Name = "FImportData";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FImportData_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FImportData_DragEnter);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ExControls.ExButton bStorno;
        private ExControls.ExButton bImport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ExComboBox cbDataType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private ExComboBox cbEncoding;
        private System.Windows.Forms.Label label3;
        private ExGroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private ExControls.ExButton bClipboard;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private ExRadioButton rbRemoveAndInsert;
        private ExRadioButton rbAppend;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private ExControls.ExButton bCSV;
        private ExControls.ExButton bXLS;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
        private ExCheckBox cboxFirstHeader;
        private System.Windows.Forms.OpenFileDialog ofDialogCSV;
        private System.Windows.Forms.OpenFileDialog ofDialogXLS;
    }
}