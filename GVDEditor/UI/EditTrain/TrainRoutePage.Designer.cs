using ExControls;

namespace GVDEditor.UI.EditTrain
{
    partial class TrainRoutePage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TrainRoutePage));
            split = new SplitContainer();
            tlpStations = new TableLayoutPanel();
            tlpRoute = new TableLayoutPanel();
            lStationsHeader = new Label();
            tbSearch = new ExTextBox();
            cbCustom = new ExCheckBox();
            listStations = new ListBox();
            bAddFrom = new ExButton();
            bAddTo = new ExButton();
            lFromHeader = new Label();
            routeFrom = new RouteEditor();
            tlpStation = new TableLayoutPanel();
            lToHeader = new Label();
            routeTo = new RouteEditor();
            lRouting = new Label();
            lHint = new Label();
            lStationName = new Label();
            lArrival = new Label();
            mtArrival = new ExMaskedTextBox();
            lArrTrack = new Label();
            cbArrTrack = new ExComboBox();
            lArrLine = new Label();
            tbArrLine = new ExTextBox();
            lDeparture = new Label();
            mtDeparture = new ExMaskedTextBox();
            lDepTrack = new Label();
            cbDepTrack = new ExComboBox();
            lDepLine = new Label();
            tbDepLine = new ExTextBox();
            ((ISupportInitialize)split).BeginInit();
            split.Panel1.SuspendLayout();
            split.Panel2.SuspendLayout();
            split.SuspendLayout();
            tlpStations.SuspendLayout();
            tlpRoute.SuspendLayout();
            tlpStation.SuspendLayout();
            SuspendLayout();
            // 
            // split
            // 
            resources.ApplyResources(split, "split");
            split.FixedPanel = FixedPanel.Panel1;
            split.Name = "split";
            // 
            // split.Panel1
            // 
            split.Panel1.Controls.Add(tlpStations);
            // 
            // split.Panel2
            // 
            split.Panel2.Controls.Add(tlpRoute);
            // 
            // tlpStations
            // 
            resources.ApplyResources(tlpStations, "tlpStations");
            tlpStations.Controls.Add(lStationsHeader, 0, 0);
            tlpStations.Controls.Add(tbSearch, 0, 1);
            tlpStations.Controls.Add(cbCustom, 0, 2);
            tlpStations.Controls.Add(listStations, 0, 3);
            tlpStations.Controls.Add(bAddFrom, 0, 4);
            tlpStations.Controls.Add(bAddTo, 0, 5);
            tlpStations.Name = "tlpStations";
            // 
            // lStationsHeader
            // 
            resources.ApplyResources(lStationsHeader, "lStationsHeader");
            lStationsHeader.Name = "lStationsHeader";
            // 
            // tbSearch
            // 
            resources.ApplyResources(tbSearch, "tbSearch");
            tbSearch.Name = "tbSearch";
            tbSearch.TextChanged += Filter_Changed;
            tbSearch.KeyDown += tbSearch_KeyDown;
            // 
            // cbCustom
            // 
            resources.ApplyResources(cbCustom, "cbCustom");
            cbCustom.Name = "cbCustom";
            cbCustom.UseVisualStyleBackColor = true;
            cbCustom.CheckedChanged += Filter_Changed;
            // 
            // listStations
            // 
            resources.ApplyResources(listStations, "listStations");
            listStations.FormattingEnabled = true;
            listStations.IntegralHeight = false;
            listStations.Name = "listStations";
            listStations.DoubleClick += listStations_DoubleClick;
            listStations.KeyDown += listStations_KeyDown;
            // 
            // bAddFrom
            // 
            resources.ApplyResources(bAddFrom, "bAddFrom");
            bAddFrom.Name = "bAddFrom";
            bAddFrom.UseVisualStyleBackColor = true;
            bAddFrom.Click += bAddFrom_Click;
            // 
            // bAddTo
            // 
            resources.ApplyResources(bAddTo, "bAddTo");
            bAddTo.Name = "bAddTo";
            bAddTo.UseVisualStyleBackColor = true;
            bAddTo.Click += bAddTo_Click;
            // 
            // tlpRoute
            // 
            resources.ApplyResources(tlpRoute, "tlpRoute");
            tlpRoute.Controls.Add(lFromHeader, 0, 0);
            tlpRoute.Controls.Add(routeFrom, 0, 1);
            tlpRoute.Controls.Add(tlpStation, 0, 2);
            tlpRoute.Controls.Add(lToHeader, 0, 3);
            tlpRoute.Controls.Add(routeTo, 0, 4);
            tlpRoute.Controls.Add(lRouting, 0, 5);
            tlpRoute.Controls.Add(lHint, 0, 6);
            tlpRoute.Name = "tlpRoute";
            // 
            // lFromHeader
            // 
            resources.ApplyResources(lFromHeader, "lFromHeader");
            lFromHeader.Name = "lFromHeader";
            // 
            // routeFrom
            // 
            resources.ApplyResources(routeFrom, "routeFrom");
            routeFrom.Name = "routeFrom";
            routeFrom.Enter += routeFrom_Enter;
            // 
            // tlpStation
            // 
            resources.ApplyResources(tlpStation, "tlpStation");
            tlpStation.Controls.Add(lStationName, 0, 0);
            tlpStation.Controls.Add(lArrival, 0, 1);
            tlpStation.Controls.Add(mtArrival, 1, 1);
            tlpStation.Controls.Add(lArrTrack, 2, 1);
            tlpStation.Controls.Add(cbArrTrack, 3, 1);
            tlpStation.Controls.Add(lArrLine, 4, 1);
            tlpStation.Controls.Add(tbArrLine, 5, 1);
            tlpStation.Controls.Add(lDeparture, 0, 2);
            tlpStation.Controls.Add(mtDeparture, 1, 2);
            tlpStation.Controls.Add(lDepTrack, 2, 2);
            tlpStation.Controls.Add(cbDepTrack, 3, 2);
            tlpStation.Controls.Add(lDepLine, 4, 2);
            tlpStation.Controls.Add(tbDepLine, 5, 2);
            tlpStation.Name = "tlpStation";
            tlpStation.SetColumnSpan(lStationName, 6);
            // 
            // lStationName
            // 
            resources.ApplyResources(lStationName, "lStationName");
            lStationName.Name = "lStationName";
            // 
            // lArrival
            // 
            resources.ApplyResources(lArrival, "lArrival");
            lArrival.Name = "lArrival";
            // 
            // mtArrival
            // 
            resources.ApplyResources(mtArrival, "mtArrival");
            mtArrival.Mask = "00:00";
            mtArrival.Name = "mtArrival";
            mtArrival.TextAlign = HorizontalAlignment.Center;
            mtArrival.TextChanged += Time_TextChanged;
            // 
            // lArrTrack
            // 
            resources.ApplyResources(lArrTrack, "lArrTrack");
            lArrTrack.Name = "lArrTrack";
            // 
            // cbArrTrack
            // 
            resources.ApplyResources(cbArrTrack, "cbArrTrack");
            cbArrTrack.DropDownStyle = ComboBoxStyle.DropDownList;
            cbArrTrack.FormattingEnabled = true;
            cbArrTrack.Name = "cbArrTrack";
            cbArrTrack.SelectionChangeCommitted += cbArrTrack_SelectionChangeCommitted;
            // 
            // lArrLine
            // 
            resources.ApplyResources(lArrLine, "lArrLine");
            lArrLine.Name = "lArrLine";
            // 
            // tbArrLine
            // 
            resources.ApplyResources(tbArrLine, "tbArrLine");
            tbArrLine.Name = "tbArrLine";
            tbArrLine.TextChanged += Line_TextChanged;
            // 
            // lDeparture
            // 
            resources.ApplyResources(lDeparture, "lDeparture");
            lDeparture.Name = "lDeparture";
            // 
            // mtDeparture
            // 
            resources.ApplyResources(mtDeparture, "mtDeparture");
            mtDeparture.Mask = "00:00";
            mtDeparture.Name = "mtDeparture";
            mtDeparture.TextAlign = HorizontalAlignment.Center;
            mtDeparture.TextChanged += Time_TextChanged;
            // 
            // lDepTrack
            // 
            resources.ApplyResources(lDepTrack, "lDepTrack");
            lDepTrack.Name = "lDepTrack";
            // 
            // cbDepTrack
            // 
            resources.ApplyResources(cbDepTrack, "cbDepTrack");
            cbDepTrack.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDepTrack.FormattingEnabled = true;
            cbDepTrack.Name = "cbDepTrack";
            cbDepTrack.SelectionChangeCommitted += cbDepTrack_SelectionChangeCommitted;
            // 
            // lDepLine
            // 
            resources.ApplyResources(lDepLine, "lDepLine");
            lDepLine.Name = "lDepLine";
            // 
            // tbDepLine
            // 
            resources.ApplyResources(tbDepLine, "tbDepLine");
            tbDepLine.Name = "tbDepLine";
            tbDepLine.TextChanged += Line_TextChanged;
            // 
            // lToHeader
            // 
            resources.ApplyResources(lToHeader, "lToHeader");
            lToHeader.Name = "lToHeader";
            // 
            // routeTo
            // 
            resources.ApplyResources(routeTo, "routeTo");
            routeTo.Name = "routeTo";
            routeTo.Enter += routeTo_Enter;
            // 
            // lRouting
            // 
            resources.ApplyResources(lRouting, "lRouting");
            lRouting.Name = "lRouting";
            // 
            // lHint
            // 
            resources.ApplyResources(lHint, "lHint");
            lHint.Name = "lHint";
            // 
            // TrainRoutePage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(split);
            Name = "TrainRoutePage";
            ((ISupportInitialize)split).EndInit();
            tlpStation.ResumeLayout(false);
            tlpStation.PerformLayout();
            tlpRoute.ResumeLayout(false);
            tlpRoute.PerformLayout();
            tlpStations.ResumeLayout(false);
            tlpStations.PerformLayout();
            split.Panel1.ResumeLayout(false);
            split.Panel2.ResumeLayout(false);
            split.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer split;
        private TableLayoutPanel tlpStations;
        private TableLayoutPanel tlpRoute;
        private Label lStationsHeader;
        private ExTextBox tbSearch;
        private ExCheckBox cbCustom;
        private ListBox listStations;
        private ExButton bAddFrom;
        private ExButton bAddTo;
        private Label lFromHeader;
        private RouteEditor routeFrom;
        private TableLayoutPanel tlpStation;
        private Label lToHeader;
        private RouteEditor routeTo;
        private Label lRouting;
        private Label lHint;
        private Label lStationName;
        private Label lArrival;
        private ExMaskedTextBox mtArrival;
        private Label lArrTrack;
        private ExComboBox cbArrTrack;
        private Label lArrLine;
        private ExTextBox tbArrLine;
        private Label lDeparture;
        private ExMaskedTextBox mtDeparture;
        private Label lDepTrack;
        private ExComboBox cbDepTrack;
        private Label lDepLine;
        private ExTextBox tbDepLine;
    }
}
