using GVDEditor.Tools;
using ToolsCore.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Radenie vlaku.
/// </summary>
public partial class FRadenie : Form
{
    private readonly Dictionary<FyzLanguage, List<FyzSound>> _allSoundsLangs = new();
    private readonly BindingList<FyzSound> _selectedSounds = new();
    private BindingList<FyzSound> _soundInDir = new();

    /// <summary>
    ///     Vybrane zvuky reprezentujúce radenie.
    /// </summary>
    public List<FyzSound> SelSounds;

    /// <summary>
    ///     Vytvori novy formulár typu <see cref="FRadenie"/>.
    /// </summary>
    /// <param name="sounds">Zoznam fyzických zvukov reprezentujúcich radenie vlaku.</param>
    public FRadenie(List<FyzSound> sounds)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();

        SelSounds = sounds;

        foreach (var lang in GlobData.Languages)
            _allSoundsLangs.Add(lang, lang.IsBasic ? GlobData.Sounds : RawBankParser.ReadFyzZvukFile(GlobData.RawBankDir, lang));

        _selectedSounds.ListChanged += SelectedSounds_ListChanged;

        listAllSounds.DataSource = _soundInDir;
        listRadenie.DataSource = _selectedSounds;

        foreach (var sound in sounds) _selectedSounds.Add(sound);

        cbSoundDir.DataSource = FyzGroupType.GetValues();
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
            var list = new List<FyzSound>(_allSoundsLangs[(FyzLanguage)cbLanguage.SelectedItem]);
            _soundInDir = new BindingList<FyzSound>();
            foreach (var zvuk in list.Where(zvuk => zvuk.Group.Type.Equals(cbSoundDir.SelectedItem)))
                _soundInDir.Add(zvuk);

            listAllSounds.DataSource = _soundInDir;
        }
    }

    private void cbSoundDir_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbLanguage.SelectedIndex != -1 && cbSoundDir.SelectedIndex != -1)
        {
            var list = new List<FyzSound>(_allSoundsLangs[(FyzLanguage)cbLanguage.SelectedItem]);
            _soundInDir = new BindingList<FyzSound>();
            foreach (var zvuk in list.Where(zvuk => zvuk.Group.Type.Equals(cbSoundDir.SelectedItem)))
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
        var data = e.Data.GetData(typeof(FyzSound));
        _selectedSounds.Remove(data as FyzSound);
        _selectedSounds.Insert(index, data as FyzSound);
        listRadenie.SelectedItem = data;
    }

    private void listAllSounds_Format(object sender, ListControlConvertEventArgs e) => Format(e);

    private void listRadenie_Format(object sender, ListControlConvertEventArgs e) => Format(e);

    private static void Format(ListControlConvertEventArgs e)
    {
        if (e.ListItem is FyzSound sound)
        {
            var type = sound.Group.Type;
            if (
                type == FyzGroupType.K1 ||
                type == FyzGroupType.K2 ||
                type == FyzGroupType.K3 ||
                type == FyzGroupType.K4 ||
                type == FyzGroupType.N1 ||
                type == FyzGroupType.N2 ||
                type == FyzGroupType.N3 ||
                type == FyzGroupType.N4 ||
                type == FyzGroupType.N5 ||
                type == FyzGroupType.N10 ||
                type == FyzGroupType.N11 ||
                type == FyzGroupType.N12 ||
                type == FyzGroupType.N13 ||
                type == FyzGroupType.POZ1 ||
                type == FyzGroupType.POZ2 ||
                type == FyzGroupType.POZ3 ||
                type == FyzGroupType.POZ7 ||
                type == FyzGroupType.R1 ||
                type == FyzGroupType.R2 ||
                type == FyzGroupType.R3 ||
                type == FyzGroupType.R4 ||
                type == FyzGroupType.V1 ||
                type == FyzGroupType.V2 ||
                type == FyzGroupType.V4 ||
                type == FyzGroupType.V14 ||
                type == FyzGroupType.SLOVA ||
                type == FyzGroupType.VOZY1 ||
                type == FyzGroupType.VOZY1M ||
                type == FyzGroupType.VOZY2 ||
                type == FyzGroupType.VOZY2M ||
                type == FyzGroupType.VOZY3 ||
                type == FyzGroupType.VOZY3M ||
                type == FyzGroupType.VOZY4 ||
                type == FyzGroupType.VOZY4M ||
                type == FyzGroupType.VOZY5 ||
                type == FyzGroupType.VOZY5M ||
                type == FyzGroupType.VOZY6 ||
                type == FyzGroupType.VOZY6M ||
                type == FyzGroupType.VOZY7 ||
                type == FyzGroupType.VOZY7M ||
                type == FyzGroupType.VOZY8 ||
                type == FyzGroupType.VOZY8M
            )
                e.Value = sound.Text;
            else
                e.Value = sound.ToString();
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
            soundsS.Add(GlobData.RawBankDir + "\\" + fyzZvuk.Language.RelativePath + fyzZvuk.Group.RelativePath + fyzZvuk.FileName);

        var player = new WavPlayer(soundsS.ToArray(), GlobData.Config.PlayerSoundsOffset);
        player.StartPlay();
    }

    private void FRadenie_HelpButtonClicked(object sender, CancelEventArgs e)
    {
        Process.Start(LinkConsts.LINK_EDIT_TRAIN_RADENIE);
    }
}