using ExControls;
using GVDEditor.Entities;

namespace GVDEditor.Forms
{
    partial class FTableColumnOrder
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTableColumnOrder));
            this.bSave = new System.Windows.Forms.Button();
            this.groupBox1 = new ExGroupBox();
            this.nudTypeCountLines = new ExNumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cbViewMode = new ExComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbViewType = new ExComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new ExGroupBox();
            this.bSetForAll = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.bDelete = new System.Windows.Forms.Button();
            this.bAdd = new System.Windows.Forms.Button();
            this.listOrder = new System.Windows.Forms.ListBox();
            this.tableItemBindingSource = new System.Windows.Forms.BindingSource();
            this.label3 = new System.Windows.Forms.Label();
            this.listColumns = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTypeCountLines)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableItemBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // bSave
            // 
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.Name = "bSave";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudTypeCountLines);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbViewMode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbViewType);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // nudTypeCountLines
            // 
            resources.ApplyResources(this.nudTypeCountLines, "nudTypeCountLines");
            this.nudTypeCountLines.Name = "nudTypeCountLines";
            this.nudTypeCountLines.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cbViewMode
            // 
            this.cbViewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbViewMode.FormattingEnabled = true;
            resources.ApplyResources(this.cbViewMode, "cbViewMode");
            this.cbViewMode.Name = "cbViewMode";
            this.cbViewMode.SelectedIndexChanged += new System.EventHandler(this.cbViewMode_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cbViewType
            // 
            this.cbViewType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbViewType.FormattingEnabled = true;
            resources.ApplyResources(this.cbViewType, "cbViewType");
            this.cbViewType.Name = "cbViewType";
            this.cbViewType.SelectedIndexChanged += new System.EventHandler(this.cbViewType_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bSetForAll);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.bDelete);
            this.groupBox2.Controls.Add(this.bAdd);
            this.groupBox2.Controls.Add(this.listOrder);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.listColumns);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // bSetForAll
            // 
            resources.ApplyResources(this.bSetForAll, "bSetForAll");
            this.bSetForAll.Name = "bSetForAll";
            this.bSetForAll.UseVisualStyleBackColor = true;
            this.bSetForAll.Click += new System.EventHandler(this.bSetForAll_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // bDelete
            // 
            resources.ApplyResources(this.bDelete, "bDelete");
            this.bDelete.Name = "bDelete";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bAdd
            // 
            resources.ApplyResources(this.bAdd, "bAdd");
            this.bAdd.Name = "bAdd";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // listOrder
            // 
            this.listOrder.AllowDrop = true;
            this.listOrder.DataSource = this.tableItemBindingSource;
            this.listOrder.DisplayMember = "Name";
            this.listOrder.FormattingEnabled = true;
            resources.ApplyResources(this.listOrder, "listOrder");
            this.listOrder.Name = "listOrder";
            this.listOrder.DragDrop += new System.Windows.Forms.DragEventHandler(this.listOrder_DragDrop);
            this.listOrder.DragOver += new System.Windows.Forms.DragEventHandler(this.listOrder_DragOver);
            this.listOrder.DoubleClick += new System.EventHandler(this.listOrder_DoubleClick);
            this.listOrder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listOrder_MouseDown);
            // 
            // tableItemBindingSource
            // 
            this.tableItemBindingSource.DataSource = typeof(GVDEditor.Entities.TableItem);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // listColumns
            // 
            this.listColumns.DataSource = this.tableItemBindingSource;
            this.listColumns.DisplayMember = "Name";
            this.listColumns.FormattingEnabled = true;
            resources.ApplyResources(this.listColumns, "listColumns");
            this.listColumns.Name = "listColumns";
            this.listColumns.DoubleClick += new System.EventHandler(this.listColumns_DoubleClick);
            // 
            // FTableColumnOrder
            // 
            this.AcceptButton = this.bSave;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FTableColumnOrder";
            this.ShowInTaskbar = false;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FTableColumnOrder_HelpButtonClicked);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTypeCountLines)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableItemBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bSave;
        private ExGroupBox groupBox1;
        private ExComboBox cbViewMode;
        private System.Windows.Forms.Label label2;
        private ExComboBox cbViewType;
        private System.Windows.Forms.Label label1;
        private ExGroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.ListBox listOrder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listColumns;
        private System.Windows.Forms.Button bSetForAll;
        private System.Windows.Forms.BindingSource tableItemBindingSource;
        private System.Windows.Forms.Label label5;
        private ExNumericUpDown nudTypeCountLines;
    }
}