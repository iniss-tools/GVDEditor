using GVDEditor.Entities;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Radenie vlaku.
/// </summary>
public partial class FRadenie : Form
{
    private readonly Dictionary<Language, List<FyzZvuk>> _allSoundsLangs = new();
    private readonly BindingList<FyzZvuk> _selectedSounds = new();
    private BindingList<FyzZvuk> _soundInDir = new();

    /// <summary>
    ///     Vybrane zvuky reprezentujúce radenie.
    /// </summary>
    public List<FyzZvuk> SelSounds;

    /// <summary>
    ///     Vytvori novy formulár typu <see cref="FRadenie"/>.
    /// </summary>
    /// <param name="sounds">Zoznam fyzických zvukov reprezentujúcich radenie vlaku.</param>
    public FRadenie(List<FyzZvuk> sounds)
    {
        InitializeComponent();
        FormUtils.SetFormFont(this);
        this.ApplyTheme();

        SelSounds = sounds;

        foreach (var lang in GlobData.Languages)
            _allSoundsLangs.Add(lang,
                lang.IsBasic ? GlobData.Sounds : RawBankReader.ReadFyzZvukFile(GlobData.RawBankDir, lang));

        _selectedSounds.ListChanged += SelectedSounds_ListChanged;

        listAllSounds.DataSource = _soundInDir;
        listRadenie.DataSource = _selectedSounds;

        foreach (var sound in sounds) _selectedSounds.Add(sound);

        cbSoundDir.DataSource = FyzZvukDirType.GetValues();
        cbLanguage.DataSource = GlobData.Languages;
        if (GlobData.Languages.Count != 0) cbLanguage.SelectedIndex = 0;

        cbSoundDir.SelectedIndex = 0;
    }

    private void bSave_Click(object sender, EventArgs e)
    {
        SelSounds = _selectedSounds.ToList();
        DialogResult = DialogResult.OK;
    }

    private void bStorno_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

    private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbLanguage.SelectedIndex != -1 && cbSoundDir.SelectedIndex != -1)
        {
            var list = new List<FyzZvuk>(_allSoundsLangs[(Language)cbLanguage.SelectedItem]);
            _soundInDir = new BindingList<FyzZvuk>();
            foreach (var zvuk in list.Where(zvuk => zvuk.Dir.DirType.Equals(cbSoundDir.SelectedItem)))
                _soundInDir.Add(zvuk);

            listAllSounds.DataSource = _soundInDir;
        }
    }

    private void cbSoundDir_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbLanguage.SelectedIndex != -1 && cbSoundDir.SelectedIndex != -1)
        {
            var list = new List<FyzZvuk>(_allSoundsLangs[(Language)cbLanguage.SelectedItem]);
            _soundInDir = new BindingList<FyzZvuk>();
            foreach (var zvuk in list.Where(zvuk => zvuk.Dir.DirType.Equals(cbSoundDir.SelectedItem)))
                _soundInDir.Add(zvuk);
            listAllSounds.DataSource = _soundInDir;
        }
    }

    private void listAllSounds_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listAllSounds.SelectedIndex != -1)
            tbTextSound.Text = _soundInDir[listAllSounds.SelectedIndex].Text;
    }

    private void listRadenie_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listRadenie.SelectedIndex != -1)
            tbTextSound.Text = _selectedSounds[listRadenie.SelectedIndex].Text;
    }

    private void listAllSounds_DoubleClick(object sender, EventArgs e)
    {
        if (listAllSounds.SelectedIndex != -1) 
            _selectedSounds.Add(_soundInDir[listAllSounds.SelectedIndex]);
    }

    private void listRadenie_DoubleClick(object sender, EventArgs e)
    {
        if (listRadenie.SelectedIndex != -1) 
            _selectedSounds.RemoveAt(listRadenie.SelectedIndex);
    }

    private void bAdd_Click(object sender, EventArgs e)
    {
        if (listAllSounds.SelectedIndex != -1) 
            _selectedSounds.Add(_soundInDir[listAllSounds.SelectedIndex]);
    }

    private void bDelete_Click(object sender, EventArgs e)
    {
        if (listRadenie.SelectedIndex != -1) 
            _selectedSounds.RemoveAt(listRadenie.SelectedIndex);
    }

    private void bSkor_Click(object sender, EventArgs e)
    {
        if (listRadenie.SelectedIndex != -1)
        {
            var sel = listRadenie.SelectedIndex;
            var sound = _selectedSounds[sel];

            if (sel - 1 >= 0)
            {
                _selectedSounds.RemoveAt(sel);
                _selectedSounds.Insert(sel - 1, sound);
                listRadenie.SelectedIndex = sel - 1;
            }
        }
    }

    private void bNeskor_Click(object sender, EventArgs e)
    {
        if (listRadenie.SelectedIndex != -1)
        {
            var sel = listRadenie.SelectedIndex;
            var sound = _selectedSounds[sel];

            if (sel + 1 < _selectedSounds.Count)
            {
                _selectedSounds.RemoveAt(sel);
                _selectedSounds.Insert(sel + 1, sound);
                listRadenie.SelectedIndex = sel + 1;
            }
        }
    }

    private void SelectedSounds_ListChanged(object sender, ListChangedEventArgs e)
    {
        var sb = new StringBuilder();
        foreach (var zvuk in _selectedSounds) sb.Append(zvuk.Text + " ");

        tbTextRadenie.Text = sb.ToString().Trim();
    }

    private void listRadenie_MouseDown(object sender, MouseEventArgs e)
    {
        if (listRadenie.SelectedItem == null) return;
        listRadenie.DoDragDrop(listRadenie.SelectedItem, DragDropEffects.Move);
    }

    private void listRadenie_DragOver(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Move;

    private void listRadenie_DragDrop(object sender, DragEventArgs e)
    {
        var point = listRadenie.PointToClient(new Point(e.X, e.Y));
        var index = listRadenie.IndexFromPoint(point);
        if (index < 0) index = listRadenie.Items.Count - 1;
        var data = e.Data.GetData(typeof(FyzZvuk));
        _selectedSounds.Remove(data as FyzZvuk);
        _selectedSounds.Insert(index, data as FyzZvuk);
        listRadenie.SelectedItem = data;
    }

    private void listAllSounds_Format(object sender, ListControlConvertEventArgs e) => Format(e);

    private void listRadenie_Format(object sender, ListControlConvertEventArgs e) => Format(e);

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
            _selectedSounds.RemoveAt(listRadenie.SelectedIndex);
    }

    private void bPlay_Click(object sender, EventArgs e)
    {
        var soundsS = new List<string>();

        foreach (var fyzZvuk in _selectedSounds)
            soundsS.Add(GlobData.RawBankDir + "\\" + fyzZvuk.Language.RelativePath + fyzZvuk.Dir.RelativePath + fyzZvuk.FileName);

        var player = new WAVPlayer(soundsS.ToArray(), GlobData.Config.PlayerWordPause);
        player.StartPlay();
    }

    private void FRadenie_HelpButtonClicked(object sender, CancelEventArgs e)
    {
        Process.Start(LinkConsts.LINK_EDIT_TRAIN_RADENIE);
    }
}