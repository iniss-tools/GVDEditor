using ExControls;

namespace GVDEditor.UI.Settings
{
    partial class GrafikonPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(GrafikonPage));
            pScroll = new Panel();
            tlpGrafikon = new TableLayoutPanel();
            lPeriod = new Label();
            lGvdFrom = new Label();
            dtpGVDOd = new ExDateTimePicker();
            lGvdTo = new Label();
            dtpGVDDo = new ExDateTimePicker();
            lDataFrom = new Label();
            dtpDataOd = new ExDateTimePicker();
            lDataTo = new Label();
            dtpDataDo = new ExDateTimePicker();
            lStation = new Label();
            lStationName = new Label();
            cbStationName = new ExComboBox();
            cbCustomStation = new ExCheckBox();
            lCustomId = new Label();
            nudIDStation = new ExNumericUpDown();
            lCustomName = new Label();
            tbGVDStationName = new ExTextBox();
            lFiles = new Label();
            lDir = new Label();
            flpDir = new FlowLayoutPanel();
            tbDir = new ExTextBox();
            bOpenDir = new ExButton();
            lDirName = new Label();
            flpDirName = new FlowLayoutPanel();
            tbDirName = new ExTextBox();
            bDirChange = new ExButton();
            lDirNote = new Label();
            lHint = new Label();
            ((ISupportInitialize)nudIDStation).BeginInit();
            tlpGrafikon.SuspendLayout();
            flpDir.SuspendLayout();
            flpDirName.SuspendLayout();
            SuspendLayout();
            // 
            // pScroll
            // 
            resources.ApplyResources(pScroll, "pScroll");
            pScroll.Controls.Add(tlpGrafikon);
            pScroll.Name = "pScroll";
            // 
            // tlpGrafikon
            // 
            resources.ApplyResources(tlpGrafikon, "tlpGrafikon");
            tlpGrafikon.Controls.Add(lPeriod, 0, 0);
            tlpGrafikon.Controls.Add(lGvdFrom, 0, 1);
            tlpGrafikon.Controls.Add(dtpGVDOd, 1, 1);
            tlpGrafikon.Controls.Add(lGvdTo, 0, 2);
            tlpGrafikon.Controls.Add(dtpGVDDo, 1, 2);
            tlpGrafikon.Controls.Add(lDataFrom, 0, 3);
            tlpGrafikon.Controls.Add(dtpDataOd, 1, 3);
            tlpGrafikon.Controls.Add(lDataTo, 0, 4);
            tlpGrafikon.Controls.Add(dtpDataDo, 1, 4);
            tlpGrafikon.Controls.Add(lStation, 0, 5);
            tlpGrafikon.Controls.Add(lStationName, 0, 6);
            tlpGrafikon.Controls.Add(cbStationName, 1, 6);
            tlpGrafikon.Controls.Add(cbCustomStation, 1, 7);
            tlpGrafikon.Controls.Add(lCustomId, 0, 8);
            tlpGrafikon.Controls.Add(nudIDStation, 1, 8);
            tlpGrafikon.Controls.Add(lCustomName, 0, 9);
            tlpGrafikon.Controls.Add(tbGVDStationName, 1, 9);
            tlpGrafikon.Controls.Add(lFiles, 0, 10);
            tlpGrafikon.Controls.Add(lDir, 0, 11);
            tlpGrafikon.Controls.Add(flpDir, 1, 11);
            tlpGrafikon.Controls.Add(lDirName, 0, 12);
            tlpGrafikon.Controls.Add(flpDirName, 1, 12);
            tlpGrafikon.Controls.Add(lDirNote, 0, 13);
            tlpGrafikon.Controls.Add(lHint, 0, 14);
            tlpGrafikon.Name = "tlpGrafikon";
            tlpGrafikon.SetColumnSpan(lPeriod, 2);
            tlpGrafikon.SetColumnSpan(lStation, 2);
            tlpGrafikon.SetColumnSpan(lFiles, 2);
            tlpGrafikon.SetColumnSpan(lDirNote, 2);
            tlpGrafikon.SetColumnSpan(lHint, 2);
            // 
            // lPeriod
            // 
            resources.ApplyResources(lPeriod, "lPeriod");
            lPeriod.Name = "lPeriod";
            // 
            // lGvdFrom
            // 
            resources.ApplyResources(lGvdFrom, "lGvdFrom");
            lGvdFrom.Name = "lGvdFrom";
            // 
            // dtpGVDOd
            // 
            resources.ApplyResources(dtpGVDOd, "dtpGVDOd");
            dtpGVDOd.Name = "dtpGVDOd";
            dtpGVDOd.ValueChanged += Period_Changed;
            // 
            // lGvdTo
            // 
            resources.ApplyResources(lGvdTo, "lGvdTo");
            lGvdTo.Name = "lGvdTo";
            // 
            // dtpGVDDo
            // 
            resources.ApplyResources(dtpGVDDo, "dtpGVDDo");
            dtpGVDDo.Name = "dtpGVDDo";
            dtpGVDDo.ValueChanged += Period_Changed;
            // 
            // lDataFrom
            // 
            resources.ApplyResources(lDataFrom, "lDataFrom");
            lDataFrom.Name = "lDataFrom";
            // 
            // dtpDataOd
            // 
            resources.ApplyResources(dtpDataOd, "dtpDataOd");
            dtpDataOd.Name = "dtpDataOd";
            dtpDataOd.ValueChanged += dtpDataOd_ValueChanged;
            // 
            // lDataTo
            // 
            resources.ApplyResources(lDataTo, "lDataTo");
            lDataTo.Name = "lDataTo";
            // 
            // dtpDataDo
            // 
            resources.ApplyResources(dtpDataDo, "dtpDataDo");
            dtpDataDo.Name = "dtpDataDo";
            dtpDataDo.ValueChanged += Period_Changed;
            // 
            // lStation
            // 
            resources.ApplyResources(lStation, "lStation");
            lStation.Name = "lStation";
            // 
            // lStationName
            // 
            resources.ApplyResources(lStationName, "lStationName");
            lStationName.Name = "lStationName";
            // 
            // cbStationName
            // 
            resources.ApplyResources(cbStationName, "cbStationName");
            cbStationName.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStationName.FormattingEnabled = true;
            cbStationName.Name = "cbStationName";
            cbStationName.SelectionChangeCommitted += cbStationName_SelectionChangeCommitted;
            // 
            // cbCustomStation
            // 
            resources.ApplyResources(cbCustomStation, "cbCustomStation");
            cbCustomStation.Name = "cbCustomStation";
            cbCustomStation.UseVisualStyleBackColor = true;
            cbCustomStation.CheckedChanged += cbCustomStation_CheckedChanged;
            // 
            // lCustomId
            // 
            resources.ApplyResources(lCustomId, "lCustomId");
            lCustomId.Name = "lCustomId";
            // 
            // nudIDStation
            // 
            resources.ApplyResources(nudIDStation, "nudIDStation");
            nudIDStation.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            nudIDStation.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            nudIDStation.Name = "nudIDStation";
            nudIDStation.Value = new decimal(new int[] { 0, 0, 0, 0 });
            nudIDStation.ValueChanged += Station_Changed;
            // 
            // lCustomName
            // 
            resources.ApplyResources(lCustomName, "lCustomName");
            lCustomName.Name = "lCustomName";
            // 
            // tbGVDStationName
            // 
            resources.ApplyResources(tbGVDStationName, "tbGVDStationName");
            tbGVDStationName.Name = "tbGVDStationName";
            tbGVDStationName.TextChanged += tbGVDStationName_TextChanged;
            // 
            // lFiles
            // 
            resources.ApplyResources(lFiles, "lFiles");
            lFiles.Name = "lFiles";
            // 
            // lDir
            // 
            resources.ApplyResources(lDir, "lDir");
            lDir.Name = "lDir";
            // 
            // flpDir
            // 
            resources.ApplyResources(flpDir, "flpDir");
            flpDir.Controls.Add(tbDir);
            flpDir.Controls.Add(bOpenDir);
            flpDir.Name = "flpDir";
            // 
            // tbDir
            // 
            resources.ApplyResources(tbDir, "tbDir");
            tbDir.Name = "tbDir";
            // 
            // bOpenDir
            // 
            resources.ApplyResources(bOpenDir, "bOpenDir");
            bOpenDir.Name = "bOpenDir";
            bOpenDir.UseVisualStyleBackColor = true;
            bOpenDir.Click += bOpenDir_Click;
            // 
            // lDirName
            // 
            resources.ApplyResources(lDirName, "lDirName");
            lDirName.Name = "lDirName";
            // 
            // flpDirName
            // 
            resources.ApplyResources(flpDirName, "flpDirName");
            flpDirName.Controls.Add(tbDirName);
            flpDirName.Controls.Add(bDirChange);
            flpDirName.Name = "flpDirName";
            // 
            // tbDirName
            // 
            resources.ApplyResources(tbDirName, "tbDirName");
            tbDirName.Name = "tbDirName";
            // 
            // bDirChange
            // 
            resources.ApplyResources(bDirChange, "bDirChange");
            bDirChange.Name = "bDirChange";
            bDirChange.UseVisualStyleBackColor = true;
            bDirChange.Click += bDirChange_Click;
            // 
            // lDirNote
            // 
            resources.ApplyResources(lDirNote, "lDirNote");
            lDirNote.Name = "lDirNote";
            // 
            // lHint
            // 
            resources.ApplyResources(lHint, "lHint");
            lHint.Name = "lHint";
            // 
            // GrafikonPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pScroll);
            Name = "GrafikonPage";
            ((ISupportInitialize)nudIDStation).EndInit();
            flpDirName.ResumeLayout(false);
            flpDirName.PerformLayout();
            flpDir.ResumeLayout(false);
            flpDir.PerformLayout();
            tlpGrafikon.ResumeLayout(false);
            tlpGrafikon.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pScroll;
        private TableLayoutPanel tlpGrafikon;
        private Label lPeriod;
        private Label lGvdFrom;
        private ExDateTimePicker dtpGVDOd;
        private Label lGvdTo;
        private ExDateTimePicker dtpGVDDo;
        private Label lDataFrom;
        private ExDateTimePicker dtpDataOd;
        private Label lDataTo;
        private ExDateTimePicker dtpDataDo;
        private Label lStation;
        private Label lStationName;
        private ExComboBox cbStationName;
        private ExCheckBox cbCustomStation;
        private Label lCustomId;
        private ExNumericUpDown nudIDStation;
        private Label lCustomName;
        private ExTextBox tbGVDStationName;
        private Label lFiles;
        private Label lDir;
        private FlowLayoutPanel flpDir;
        private ExTextBox tbDir;
        private ExButton bOpenDir;
        private Label lDirName;
        private FlowLayoutPanel flpDirName;
        private ExTextBox tbDirName;
        private ExButton bDirChange;
        private Label lDirNote;
        private Label lHint;
    }
}
