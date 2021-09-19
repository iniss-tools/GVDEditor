using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GVDEditor.Entities;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Forms
{
    /// <summary>
    ///     Dialog - Radenie vlaku
    /// </summary>
    public partial class FRadenie : Form
    {
        private readonly Dictionary<Language, List<FyzZvuk>> AllSoundsLangs = new();
        private readonly BindingList<FyzZvuk> SelectedSounds = new();

        /// <summary>
        ///     Vybrane zvuky reprezentujuce radenie
        /// </summary>
        public List<FyzZvuk> SelSounds;

        private BindingList<FyzZvuk> SoundInDir = new();

        /// <summary>
        ///     Konstruktor
        /// </summary>
        /// <param name="sounds"></param>
        public FRadenie(List<FyzZvuk> sounds)
        {
            InitializeComponent();
            FormUtils.SetFormFont(this);
            this.ApplyTheme();

            SelSounds = sounds;

            foreach (var lang in GlobData.Languages)
                AllSoundsLangs.Add(lang,
                    lang.IsBasic ? GlobData.Sounds : RawBankReader.ReadFyzZvukFile(GlobData.RAWBANKDir, lang));

            SelectedSounds.ListChanged += SelectedSounds_ListChanged;

            listAllSounds.DataSource = SoundInDir;
            listRadenie.DataSource = SelectedSounds;

            foreach (var sound in sounds) SelectedSounds.Add(sound);

            cbSoundDir.DataSource = FyzZvukDirType.GetValues();
            cbLanguage.DataSource = GlobData.Languages;
            if (GlobData.Languages.Count != 0) cbLanguage.SelectedIndex = 0;

            cbSoundDir.SelectedIndex = 0;
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            SelSounds = SelectedSounds.ToList();

            DialogResult = DialogResult.OK;
        }

        private void bStorno_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLanguage.SelectedIndex != -1 && cbSoundDir.SelectedIndex != -1)
            {
                var list = new List<FyzZvuk>(AllSoundsLangs[(Language)cbLanguage.SelectedItem]);
                SoundInDir = new BindingList<FyzZvuk>();
                foreach (var zvuk in list)
                    if (zvuk.Dir.DirType.Equals(cbSoundDir.SelectedItem))
                        SoundInDir.Add(zvuk);

                listAllSounds.DataSource = SoundInDir;
            }
        }

        private void cbSoundDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLanguage.SelectedIndex != -1 && cbSoundDir.SelectedIndex != -1)
            {
                var list = new List<FyzZvuk>(AllSoundsLangs[(Language)cbLanguage.SelectedItem]);
                SoundInDir = new BindingList<FyzZvuk>();
                foreach (var zvuk in list)
                    if (zvuk.Dir.DirType.Equals(cbSoundDir.SelectedItem))
                        SoundInDir.Add(zvuk);
                listAllSounds.DataSource = SoundInDir;
            }
        }

        private void listAllSounds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listAllSounds.SelectedIndex != -1)
                tbTextSound.Text = SoundInDir[listAllSounds.SelectedIndex].Text;
        }

        private void listRadenie_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listRadenie.SelectedIndex != -1)
                tbTextSound.Text = SelectedSounds[listRadenie.SelectedIndex].Text;
        }

        private void listAllSounds_DoubleClick(object sender, EventArgs e)
        {
            if (listAllSounds.SelectedIndex != -1) SelectedSounds.Add(SoundInDir[listAllSounds.SelectedIndex]);
        }

        private void listRadenie_DoubleClick(object sender, EventArgs e)
        {
            if (listRadenie.SelectedIndex != -1) SelectedSounds.RemoveAt(listRadenie.SelectedIndex);
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            if (listAllSounds.SelectedIndex != -1) SelectedSounds.Add(SoundInDir[listAllSounds.SelectedIndex]);
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            if (listRadenie.SelectedIndex != -1) SelectedSounds.RemoveAt(listRadenie.SelectedIndex);
        }

        private void bSkor_Click(object sender, EventArgs e)
        {
            if (listRadenie.SelectedIndex != -1)
            {
                var sel = listRadenie.SelectedIndex;
                var sound = SelectedSounds[sel];

                if (sel - 1 >= 0)
                {
                    SelectedSounds.RemoveAt(sel);
                    SelectedSounds.Insert(sel - 1, sound);
                    listRadenie.SelectedIndex = sel - 1;
                }
            }
        }

        private void bNeskor_Click(object sender, EventArgs e)
        {
            if (listRadenie.SelectedIndex != -1)
            {
                var sel = listRadenie.SelectedIndex;
                var sound = SelectedSounds[sel];

                if (sel + 1 < SelectedSounds.Count)
                {
                    SelectedSounds.RemoveAt(sel);
                    SelectedSounds.Insert(sel + 1, sound);
                    listRadenie.SelectedIndex = sel + 1;
                }
            }
        }

        private void SelectedSounds_ListChanged(object sender, ListChangedEventArgs e)
        {
            var sb = new StringBuilder();
            foreach (var zvuk in SelectedSounds) sb.Append(zvuk.Text + " ");

            tbTextRadenie.Text = sb.ToString().Trim();
        }

        private void listRadenie_MouseDown(object sender, MouseEventArgs e)
        {
            if (listRadenie.SelectedItem == null) return;
            listRadenie.DoDragDrop(listRadenie.SelectedItem, DragDropEffects.Move);
        }

        private void listRadenie_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void listRadenie_DragDrop(object sender, DragEventArgs e)
        {
            var point = listRadenie.PointToClient(new Point(e.X, e.Y));
            var index = listRadenie.IndexFromPoint(point);
            if (index < 0) index = listRadenie.Items.Count - 1;
            var data = e.Data.GetData(typeof(FyzZvuk));
            SelectedSounds.Remove(data as FyzZvuk);
            SelectedSounds.Insert(index, data as FyzZvuk);
            listRadenie.SelectedItem = data;
        }

        private void listAllSounds_Format(object sender, ListControlConvertEventArgs e)
        {
            Format(e);
        }

        private void listRadenie_Format(object sender, ListControlConvertEventArgs e)
        {
            Format(e);
        }

        private static void Format(ListControlConvertEventArgs e)
        {
            if (e.ListItem is FyzZvuk zvuk)
            {
                var type = zvuk.Dir.DirType;
                if (
                    type == FyzZvukDirType.K1 ||
                    type == FyzZvukDirType.K2 ||
                    type == FyzZvukDirType.K3 ||
                    type == FyzZvukDirType.K4 ||
                    type == FyzZvukDirType.N1 ||
                    type == FyzZvukDirType.N2 ||
                    type == FyzZvukDirType.N3 ||
                    type == FyzZvukDirType.N4 ||
                    type == FyzZvukDirType.N5 ||
                    type == FyzZvukDirType.N10 ||
                    type == FyzZvukDirType.N11 ||
                    type == FyzZvukDirType.N12 ||
                    type == FyzZvukDirType.N13 ||
                    type == FyzZvukDirType.POZ1 ||
                    type == FyzZvukDirType.POZ2 ||
                    type == FyzZvukDirType.POZ3 ||
                    type == FyzZvukDirType.POZ7 ||
                    type == FyzZvukDirType.R1 ||
                    type == FyzZvukDirType.R2 ||
                    type == FyzZvukDirType.R3 ||
                    type == FyzZvukDirType.R4 ||
                    type == FyzZvukDirType.V1 ||
                    type == FyzZvukDirType.V2 ||
                    type == FyzZvukDirType.V4 ||
                    type == FyzZvukDirType.V14 ||
                    type == FyzZvukDirType.SLOVA ||
                    type == FyzZvukDirType.VOZY1 ||
                    type == FyzZvukDirType.VOZY1M ||
                    type == FyzZvukDirType.VOZY2 ||
                    type == FyzZvukDirType.VOZY2M ||
                    type == FyzZvukDirType.VOZY3 ||
                    type == FyzZvukDirType.VOZY3M ||
                    type == FyzZvukDirType.VOZY4 ||
                    type == FyzZvukDirType.VOZY4M ||
                    type == FyzZvukDirType.VOZY5 ||
                    type == FyzZvukDirType.VOZY5M ||
                    type == FyzZvukDirType.VOZY6 ||
                    type == FyzZvukDirType.VOZY6M ||
                    type == FyzZvukDirType.VOZY7 ||
                    type == FyzZvukDirType.VOZY7M ||
                    type == FyzZvukDirType.VOZY8 ||
                    type == FyzZvukDirType.VOZY8M
                )
                    e.Value = zvuk.Text;
                else
                    e.Value = zvuk.ToString();
            }
        }

        private void listRadenie_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && listRadenie.SelectedIndex != -1)
                SelectedSounds.RemoveAt(listRadenie.SelectedIndex);
        }

        private void bPlay_Click(object sender, EventArgs e)
        {
            var soundsS = new List<string>();

            foreach (var fyzZvuk in SelectedSounds)
                soundsS.Add(GlobData.RAWBANKDir + "\\" + fyzZvuk.Language.RelativePath + fyzZvuk.Dir.RelativePath + fyzZvuk.FileName);

            var player = new WAVPlayer(soundsS.ToArray(), GlobData.Config.PlayerWordPause);
            player.StartPlay();
        }

        private void FRadenie_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start(LinkConsts.LINK_EDIT_TRAIN_RADENIE);
        }
    }
}