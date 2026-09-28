using ExControls;

namespace GVDEditor.UI.EditTrain
{
    partial class TrainDodatkyPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TrainDodatkyPage));
            tlpMain = new TableLayoutPanel();
            lListHeader = new Label();
            lDetailHeader = new Label();
            listDodatky = new ListBox();
            tlpDetail = new TableLayoutPanel();
            flpAdd = new FlowLayoutPanel();
            cbAdd = new ExComboBox();
            bAdd = new ExButton();
            bRemove = new ExButton();
            lHint = new Label();
            lText = new Label();
            lInfo = new Label();
            matrix = new ReportMatrix();
            tlpMain.SuspendLayout();
            tlpDetail.SuspendLayout();
            flpAdd.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lListHeader, 0, 0);
            tlpMain.Controls.Add(lDetailHeader, 1, 0);
            tlpMain.Controls.Add(listDodatky, 0, 1);
            tlpMain.Controls.Add(tlpDetail, 1, 1);
            tlpMain.Controls.Add(flpAdd, 0, 2);
            tlpMain.Controls.Add(lHint, 0, 3);
            tlpMain.Name = "tlpMain";
            tlpMain.SetColumnSpan(flpAdd, 2);
            tlpMain.SetColumnSpan(lHint, 2);
            // 
            // lListHeader
            // 
            resources.ApplyResources(lListHeader, "lListHeader");
            lListHeader.Name = "lListHeader";
            // 
            // lDetailHeader
            // 
            resources.ApplyResources(lDetailHeader, "lDetailHeader");
            lDetailHeader.Name = "lDetailHeader";
            // 
            // listDodatky
            // 
            resources.ApplyResources(listDodatky, "listDodatky");
            listDodatky.FormattingEnabled = true;
            listDodatky.IntegralHeight = false;
            listDodatky.Name = "listDodatky";
            listDodatky.SelectedIndexChanged += listDodatky_SelectedIndexChanged;
            listDodatky.Format += listDodatky_Format;
            listDodatky.KeyDown += listDodatky_KeyDown;
            // 
            // tlpDetail
            // 
            resources.ApplyResources(tlpDetail, "tlpDetail");
            tlpDetail.Controls.Add(lText, 0, 0);
            tlpDetail.Controls.Add(lInfo, 0, 1);
            tlpDetail.Controls.Add(matrix, 0, 2);
            tlpDetail.Name = "tlpDetail";
            // 
            // lText
            // 
            resources.ApplyResources(lText, "lText");
            lText.Name = "lText";
            // 
            // lInfo
            // 
            resources.ApplyResources(lInfo, "lInfo");
            lInfo.Name = "lInfo";
            // 
            // matrix
            // 
            resources.ApplyResources(matrix, "matrix");
            matrix.Name = "matrix";
            matrix.Changed += matrix_Changed;
            // 
            // flpAdd
            // 
            resources.ApplyResources(flpAdd, "flpAdd");
            flpAdd.Controls.Add(cbAdd);
            flpAdd.Controls.Add(bAdd);
            flpAdd.Controls.Add(bRemove);
            flpAdd.Name = "flpAdd";
            // 
            // cbAdd
            // 
            resources.ApplyResources(cbAdd, "cbAdd");
            cbAdd.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbAdd.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbAdd.DropDownStyle = ComboBoxStyle.DropDown;
            cbAdd.FormattingEnabled = true;
            cbAdd.Name = "cbAdd";
            cbAdd.Format += cbAdd_Format;
            cbAdd.KeyDown += cbAdd_KeyDown;
            // 
            // bAdd
            // 
            resources.ApplyResources(bAdd, "bAdd");
            bAdd.Name = "bAdd";
            bAdd.UseVisualStyleBackColor = true;
            bAdd.Click += bAdd_Click;
            // 
            // bRemove
            // 
            resources.ApplyResources(bRemove, "bRemove");
            bRemove.Name = "bRemove";
            bRemove.UseVisualStyleBackColor = true;
            bRemove.Click += bRemove_Click;
            // 
            // lHint
            // 
            resources.ApplyResources(lHint, "lHint");
            lHint.Name = "lHint";
            // 
            // TrainDodatkyPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "TrainDodatkyPage";
            flpAdd.ResumeLayout(false);
            flpAdd.PerformLayout();
            tlpDetail.ResumeLayout(false);
            tlpDetail.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lListHeader;
        private Label lDetailHeader;
        private ListBox listDodatky;
        private TableLayoutPanel tlpDetail;
        private FlowLayoutPanel flpAdd;
        private ExComboBox cbAdd;
        private ExButton bAdd;
        private ExButton bRemove;
        private Label lHint;
        private Label lText;
        private Label lInfo;
        private ReportMatrix matrix;
    }
}
