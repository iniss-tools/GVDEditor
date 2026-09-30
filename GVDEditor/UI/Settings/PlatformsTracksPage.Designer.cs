using ExControls;

namespace GVDEditor.UI.Settings
{
    partial class PlatformsTracksPage
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(PlatformsTracksPage));
            tlpMain = new TableLayoutPanel();
            lInfo = new Label();
            flpButtons = new FlowLayoutPanel();
            bAddPlatform = new ExButton();
            bAddTrack = new ExButton();
            bDelete = new ExButton();
            tvTracks = new ExTreeView();
            pDetail = new Panel();
            tlpTrack = new TableLayoutPanel();
            tlpPlatform = new TableLayoutPanel();
            lHint = new Label();
            lIdent = new Label();
            lTrKey = new Label();
            tbTrKey = new ExTextBox();
            lTrName = new Label();
            tbTrName = new ExTextBox();
            lTrFull = new Label();
            tbTrFull = new ExTextBox();
            lTrPlatform = new Label();
            cbTrPlatform = new ExComboBox();
            lBoards = new Label();
            lTrText = new Label();
            tbTrText = new ExTextBox();
            lTrPlatText = new Label();
            tbTrPlatText = new ExTextBox();
            lTrAlt = new Label();
            tbTrAlt = new ExTextBox();
            lTrTables = new Label();
            clbTables = new ExCheckedListBox();
            lSound = new Label();
            lTrSound = new Label();
            tbTrSound = new ExTextBox();
            lTrUse = new Label();
            lPlatHeader = new Label();
            lPlatKey = new Label();
            tbPlatKey = new ExTextBox();
            lPlatName = new Label();
            tbPlatName = new ExTextBox();
            lPlatSound = new Label();
            tbPlatSound = new ExTextBox();
            lPlatNote = new Label();
            tlpMain.SuspendLayout();
            flpButtons.SuspendLayout();
            tlpTrack.SuspendLayout();
            tlpPlatform.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(tlpMain, "tlpMain");
            tlpMain.Controls.Add(lInfo, 0, 0);
            tlpMain.Controls.Add(flpButtons, 0, 1);
            tlpMain.Controls.Add(tvTracks, 0, 2);
            tlpMain.Controls.Add(pDetail, 1, 2);
            tlpMain.Controls.Add(lHint, 0, 3);
            tlpMain.Name = "tlpMain";
            tlpMain.SetColumnSpan(lInfo, 2);
            tlpMain.SetColumnSpan(flpButtons, 2);
            tlpMain.SetColumnSpan(lHint, 2);
            // 
            // lInfo
            // 
            resources.ApplyResources(lInfo, "lInfo");
            lInfo.Name = "lInfo";
            // 
            // flpButtons
            // 
            resources.ApplyResources(flpButtons, "flpButtons");
            flpButtons.Controls.Add(bAddPlatform);
            flpButtons.Controls.Add(bAddTrack);
            flpButtons.Controls.Add(bDelete);
            flpButtons.Name = "flpButtons";
            // 
            // bAddPlatform
            // 
            resources.ApplyResources(bAddPlatform, "bAddPlatform");
            bAddPlatform.Name = "bAddPlatform";
            bAddPlatform.UseVisualStyleBackColor = true;
            bAddPlatform.Click += bAddPlatform_Click;
            // 
            // bAddTrack
            // 
            resources.ApplyResources(bAddTrack, "bAddTrack");
            bAddTrack.Name = "bAddTrack";
            bAddTrack.UseVisualStyleBackColor = true;
            bAddTrack.Click += bAddTrack_Click;
            // 
            // bDelete
            // 
            resources.ApplyResources(bDelete, "bDelete");
            bDelete.Name = "bDelete";
            bDelete.UseVisualStyleBackColor = true;
            bDelete.Click += bDelete_Click;
            // 
            // tvTracks
            // 
            resources.ApplyResources(tvTracks, "tvTracks");
            tvTracks.FullRowSelect = true;
            tvTracks.HideSelection = false;
            tvTracks.Name = "tvTracks";
            tvTracks.ShowLines = false;
            tvTracks.ShowNodeToolTips = true;
            tvTracks.Style = ExTreeViewStyle.Light;
            tvTracks.AfterSelect += tvTracks_AfterSelect;
            tvTracks.KeyDown += tvTracks_KeyDown;
            // 
            // pDetail
            // 
            resources.ApplyResources(pDetail, "pDetail");
            pDetail.Controls.Add(tlpTrack);
            pDetail.Controls.Add(tlpPlatform);
            pDetail.Name = "pDetail";
            // 
            // tlpTrack
            // 
            resources.ApplyResources(tlpTrack, "tlpTrack");
            tlpTrack.Controls.Add(lIdent, 0, 0);
            tlpTrack.Controls.Add(lTrKey, 0, 1);
            tlpTrack.Controls.Add(tbTrKey, 1, 1);
            tlpTrack.Controls.Add(lTrName, 0, 2);
            tlpTrack.Controls.Add(tbTrName, 1, 2);
            tlpTrack.Controls.Add(lTrFull, 0, 3);
            tlpTrack.Controls.Add(tbTrFull, 1, 3);
            tlpTrack.Controls.Add(lTrPlatform, 0, 4);
            tlpTrack.Controls.Add(cbTrPlatform, 1, 4);
            tlpTrack.Controls.Add(lBoards, 0, 5);
            tlpTrack.Controls.Add(lTrText, 0, 6);
            tlpTrack.Controls.Add(tbTrText, 1, 6);
            tlpTrack.Controls.Add(lTrPlatText, 0, 7);
            tlpTrack.Controls.Add(tbTrPlatText, 1, 7);
            tlpTrack.Controls.Add(lTrAlt, 0, 8);
            tlpTrack.Controls.Add(tbTrAlt, 1, 8);
            tlpTrack.Controls.Add(lTrTables, 0, 9);
            tlpTrack.Controls.Add(clbTables, 1, 9);
            tlpTrack.Controls.Add(lSound, 0, 10);
            tlpTrack.Controls.Add(lTrSound, 0, 11);
            tlpTrack.Controls.Add(tbTrSound, 1, 11);
            tlpTrack.Controls.Add(lTrUse, 0, 12);
            tlpTrack.Name = "tlpTrack";
            tlpTrack.SetColumnSpan(lIdent, 2);
            tlpTrack.SetColumnSpan(lBoards, 2);
            tlpTrack.SetColumnSpan(lSound, 2);
            tlpTrack.SetColumnSpan(lTrUse, 2);
            // 
            // lIdent
            // 
            resources.ApplyResources(lIdent, "lIdent");
            lIdent.Name = "lIdent";
            // 
            // lTrKey
            // 
            resources.ApplyResources(lTrKey, "lTrKey");
            lTrKey.Name = "lTrKey";
            // 
            // tbTrKey
            // 
            resources.ApplyResources(tbTrKey, "tbTrKey");
            tbTrKey.Name = "tbTrKey";
            tbTrKey.TextChanged += Track_Changed;
            // 
            // lTrName
            // 
            resources.ApplyResources(lTrName, "lTrName");
            lTrName.Name = "lTrName";
            // 
            // tbTrName
            // 
            resources.ApplyResources(tbTrName, "tbTrName");
            tbTrName.Name = "tbTrName";
            tbTrName.TextChanged += Track_Changed;
            // 
            // lTrFull
            // 
            resources.ApplyResources(lTrFull, "lTrFull");
            lTrFull.Name = "lTrFull";
            // 
            // tbTrFull
            // 
            resources.ApplyResources(tbTrFull, "tbTrFull");
            tbTrFull.Name = "tbTrFull";
            tbTrFull.TextChanged += Track_Changed;
            // 
            // lTrPlatform
            // 
            resources.ApplyResources(lTrPlatform, "lTrPlatform");
            lTrPlatform.Name = "lTrPlatform";
            // 
            // cbTrPlatform
            // 
            resources.ApplyResources(cbTrPlatform, "cbTrPlatform");
            cbTrPlatform.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTrPlatform.FormattingEnabled = true;
            cbTrPlatform.Name = "cbTrPlatform";
            cbTrPlatform.SelectionChangeCommitted += cbTrPlatform_SelectionChangeCommitted;
            // 
            // lBoards
            // 
            resources.ApplyResources(lBoards, "lBoards");
            lBoards.Name = "lBoards";
            // 
            // lTrText
            // 
            resources.ApplyResources(lTrText, "lTrText");
            lTrText.Name = "lTrText";
            // 
            // tbTrText
            // 
            resources.ApplyResources(tbTrText, "tbTrText");
            tbTrText.Name = "tbTrText";
            tbTrText.TextChanged += Track_Changed;
            // 
            // lTrPlatText
            // 
            resources.ApplyResources(lTrPlatText, "lTrPlatText");
            lTrPlatText.Name = "lTrPlatText";
            // 
            // tbTrPlatText
            // 
            resources.ApplyResources(tbTrPlatText, "tbTrPlatText");
            tbTrPlatText.Name = "tbTrPlatText";
            tbTrPlatText.TextChanged += Track_Changed;
            // 
            // lTrAlt
            // 
            resources.ApplyResources(lTrAlt, "lTrAlt");
            lTrAlt.Name = "lTrAlt";
            // 
            // tbTrAlt
            // 
            resources.ApplyResources(tbTrAlt, "tbTrAlt");
            tbTrAlt.Name = "tbTrAlt";
            tbTrAlt.TextChanged += Track_Changed;
            // 
            // lTrTables
            // 
            resources.ApplyResources(lTrTables, "lTrTables");
            lTrTables.Name = "lTrTables";
            // 
            // clbTables
            // 
            resources.ApplyResources(clbTables, "clbTables");
            clbTables.CheckOnClick = true;
            clbTables.FormattingEnabled = true;
            clbTables.Name = "clbTables";
            clbTables.ItemCheck += clbTables_ItemCheck;
            // 
            // lSound
            // 
            resources.ApplyResources(lSound, "lSound");
            lSound.Name = "lSound";
            // 
            // lTrSound
            // 
            resources.ApplyResources(lTrSound, "lTrSound");
            lTrSound.Name = "lTrSound";
            // 
            // tbTrSound
            // 
            resources.ApplyResources(tbTrSound, "tbTrSound");
            tbTrSound.Name = "tbTrSound";
            tbTrSound.TextChanged += Track_Changed;
            // 
            // lTrUse
            // 
            resources.ApplyResources(lTrUse, "lTrUse");
            lTrUse.Name = "lTrUse";
            // 
            // tlpPlatform
            // 
            resources.ApplyResources(tlpPlatform, "tlpPlatform");
            tlpPlatform.Controls.Add(lPlatHeader, 0, 0);
            tlpPlatform.Controls.Add(lPlatKey, 0, 1);
            tlpPlatform.Controls.Add(tbPlatKey, 1, 1);
            tlpPlatform.Controls.Add(lPlatName, 0, 2);
            tlpPlatform.Controls.Add(tbPlatName, 1, 2);
            tlpPlatform.Controls.Add(lPlatSound, 0, 3);
            tlpPlatform.Controls.Add(tbPlatSound, 1, 3);
            tlpPlatform.Controls.Add(lPlatNote, 0, 4);
            tlpPlatform.Name = "tlpPlatform";
            tlpPlatform.SetColumnSpan(lPlatHeader, 2);
            tlpPlatform.SetColumnSpan(lPlatNote, 2);
            // 
            // lPlatHeader
            // 
            resources.ApplyResources(lPlatHeader, "lPlatHeader");
            lPlatHeader.Name = "lPlatHeader";
            // 
            // lPlatKey
            // 
            resources.ApplyResources(lPlatKey, "lPlatKey");
            lPlatKey.Name = "lPlatKey";
            // 
            // tbPlatKey
            // 
            resources.ApplyResources(tbPlatKey, "tbPlatKey");
            tbPlatKey.Name = "tbPlatKey";
            tbPlatKey.TextChanged += Platform_Changed;
            // 
            // lPlatName
            // 
            resources.ApplyResources(lPlatName, "lPlatName");
            lPlatName.Name = "lPlatName";
            // 
            // tbPlatName
            // 
            resources.ApplyResources(tbPlatName, "tbPlatName");
            tbPlatName.Name = "tbPlatName";
            tbPlatName.TextChanged += Platform_Changed;
            // 
            // lPlatSound
            // 
            resources.ApplyResources(lPlatSound, "lPlatSound");
            lPlatSound.Name = "lPlatSound";
            // 
            // tbPlatSound
            // 
            resources.ApplyResources(tbPlatSound, "tbPlatSound");
            tbPlatSound.Name = "tbPlatSound";
            tbPlatSound.TextChanged += Platform_Changed;
            // 
            // lPlatNote
            // 
            resources.ApplyResources(lPlatNote, "lPlatNote");
            lPlatNote.Name = "lPlatNote";
            // 
            // lHint
            // 
            resources.ApplyResources(lHint, "lHint");
            lHint.Name = "lHint";
            // 
            // PlatformsTracksPage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "PlatformsTracksPage";
            tlpPlatform.ResumeLayout(false);
            tlpPlatform.PerformLayout();
            tlpTrack.ResumeLayout(false);
            tlpTrack.PerformLayout();
            flpButtons.ResumeLayout(false);
            flpButtons.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lInfo;
        private FlowLayoutPanel flpButtons;
        private ExButton bAddPlatform;
        private ExButton bAddTrack;
        private ExButton bDelete;
        private ExTreeView tvTracks;
        private Panel pDetail;
        private TableLayoutPanel tlpTrack;
        private TableLayoutPanel tlpPlatform;
        private Label lHint;
        private Label lIdent;
        private Label lTrKey;
        private ExTextBox tbTrKey;
        private Label lTrName;
        private ExTextBox tbTrName;
        private Label lTrFull;
        private ExTextBox tbTrFull;
        private Label lTrPlatform;
        private ExComboBox cbTrPlatform;
        private Label lBoards;
        private Label lTrText;
        private ExTextBox tbTrText;
        private Label lTrPlatText;
        private ExTextBox tbTrPlatText;
        private Label lTrAlt;
        private ExTextBox tbTrAlt;
        private Label lTrTables;
        private ExCheckedListBox clbTables;
        private Label lSound;
        private Label lTrSound;
        private ExTextBox tbTrSound;
        private Label lTrUse;
        private Label lPlatHeader;
        private Label lPlatKey;
        private ExTextBox tbPlatKey;
        private Label lPlatName;
        private ExTextBox tbPlatName;
        private Label lPlatSound;
        private ExTextBox tbPlatSound;
        private Label lPlatNote;
    }
}
