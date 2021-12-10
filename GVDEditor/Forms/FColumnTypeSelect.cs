using GVDEditor.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Vyber stlpca pre import.
/// </summary>
public partial class FColumnTypeSelect : Form
{
    private readonly List<ImportTrainColumnType> columnTypes = ImportTrainColumnType.GetValues();

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FColumnTypeSelect"/>.
    /// </summary>
    public FColumnTypeSelect()
    {
        InitializeComponent();
        FormUtils.SetFormFont(this);
        this.ApplyTheme();

        listColumnTypes.DataSource = columnTypes;
    }

    /// <summary>
    ///     Vracia stlpec vybrany pouzivatelom.
    /// </summary>
    public ImportTrainColumnType SelectedType { get; private set; }

    private void bOK_Click(object sender, EventArgs e)
    {
        SelectedType = (ImportTrainColumnType)listColumnTypes.SelectedItem;
        DialogResult = DialogResult.OK;
    }

    private void bStorno_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    private void bDontImport_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.No;
    }
}