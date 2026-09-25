using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog premenovania TabTab.
/// </summary>
internal partial class FTabTabRename : Form
{
    private readonly List<string> _otherNames;

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FTabTabRename"/>.
    /// </summary>
    /// <param name="currentName">Aktualny nazov sekcie (pri premenovani); pri pridani <see langword="null"/>.</param>
    /// <param name="otherNames">Nazvy ostatnych sekcii - novy nazov sa s nimi nesmie zhodovat.</param>
    public FTabTabRename(string? currentName, IEnumerable<string> otherNames)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();

        _otherNames = otherNames.ToList();
        tbName.Text = currentName ?? "";
        tbName.SelectAll();
    }

    public string NewTabName { get; private set; } = "";

    private void bEdit_Click(object sender, EventArgs e)
    {
        var error = TabTabSections.ValidateName(tbName.Text, _otherNames, out var name);
        if (error is not null)
        {
            Utils.ShowError(error);
            DialogResult = DialogResult.None;
            tbName.Focus();
            return;
        }

        NewTabName = name;
        DialogResult = DialogResult.OK;
    }
}
