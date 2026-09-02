using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - priradenie staníc z programu ELIS k staniciam grafikonu.
/// </summary>
/// <remarks>
///     ELIS pomenúva niektoré stanice inak než zvuková banka. Tento dialóg sa pýta len na tie,
///     ktoré sa nepodarilo priradiť automaticky, a výsledok sa uloží do ELISMAP.TXT, aby sa
///     pri ďalšom importe už nepýtal. Stanica sa nikdy nezakladá sama - inak by v grafikone
///     vznikli dva názvy tej istej stanice.
/// </remarks>
public partial class FELISStations : Form
{
    /// <summary>Polozka v zozname, ktora znamena vynechanie stanice z trasy.</summary>
    private static readonly string SkipItem = Resources.FELISStations_vynechať;

    private readonly List<string> _names;
    private readonly List<Station> _stations;

    /// <summary>
    ///     Vysledne priradenie: nazov z ELIS -> ID stanice, alebo
    ///     <see cref="TxtParser.ELIS_MAP_SKIP" /> ak sa ma stanica vynechat.
    /// </summary>
    internal Dictionary<string, string> Result { get; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FELISStations" />.
    /// </summary>
    /// <param name="unresolvedNames">Nazvy z ELIS, ktore sa nepodarilo priradit automaticky.</param>
    public FELISStations(List<string> unresolvedNames)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();

        _names = unresolvedNames;
        _stations = GlobData.Stations.Concat(GlobData.CustomStations)
            .GroupBy(s => s.ID)
            .Select(g => g.First())
            .OrderBy(s => s.Name, StringComparer.CurrentCulture)
            .ToList();

        lInfo.Text =
            string.Format(Resources.FELISStations_FELISStations_Týchto__0__staníc_z_programu_ELIS_sa_nepodarilo_priradiť_automaticky, _names.Count);

        FillGrid();
    }

    private void FillGrid()
    {
        var items = new List<string> { SkipItem };
        items.AddRange(_stations.Select(s => s.Name));

        colStation.Items.Clear();
        colStation.Items.AddRange(items.Cast<object>().ToArray());

        dgvStations.Rows.Clear();
        foreach (var name in _names)
        {
            var suggestion = ELISBridgeClient.Suggest(name);

            //do bunky smie ist len hodnota, ktora je v zozname, inak DataGridView hlasi chybu
            var value = suggestion is not null && items.Contains(suggestion.Name) ? suggestion.Name : SkipItem;
            var index = dgvStations.Rows.Add(name, value);

            //navrhnute priradenie zvyraznime, nech je vidiet, co program odporučil
            if (value != SkipItem)
                dgvStations.Rows[index].Cells[1].Style.ForeColor = Color.SteelBlue;
        }
    }

    private void bCreate_Click(object sender, EventArgs e)
    {
        if (dgvStations.SelectedRows.Count == 0)
        {
            Utils.ShowError(Resources.FELISStations_bCreate_Click_Najprv_vyberte_riadky__pre_ktoré_sa_má_založiť_nová_stanica_);
            return;
        }

        var created = new List<string>();
        foreach (DataGridViewRow row in dgvStations.SelectedRows)
        {
            var elisName = (string)row.Cells[0].Value;

            //ak uz stanica s tym nazvom existuje, nezakladame druhu - iba ju priradime
            var existing = _stations.FirstOrDefault(s => s.Name == elisName);
            if (existing is null)
            {
                var station = new Station(NextFreeId(), elisName) { IsCustom = true };
                GlobData.CustomStations.Add(station);
                _stations.Add(station);
                created.Add(elisName);
            }

            row.Cells[1].Value = elisName;
            row.Cells[1].Style.ForeColor = Color.SeaGreen;
        }

        if (created.Count != 0)
        {
            _stations.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.CurrentCulture));
            RefreshComboItems();
        }
    }

    private void RefreshComboItems()
    {
        //zapamatame si aktualne hodnoty, lebo zmena poloziek ich vymaze
        var current = new string[dgvStations.Rows.Count];
        for (var i = 0; i < dgvStations.Rows.Count; i++)
            current[i] = dgvStations.Rows[i].Cells[1].Value as string;

        var items = new List<string> { SkipItem };
        items.AddRange(_stations.Select(s => s.Name));

        colStation.Items.Clear();
        colStation.Items.AddRange(items.Cast<object>().ToArray());

        for (var i = 0; i < dgvStations.Rows.Count; i++)
            dgvStations.Rows[i].Cells[1].Value = current[i];
    }

    /// <summary>
    ///     Vrati najnizsie volne ID pre novu pouzivatelom definovanu stanicu.
    /// </summary>
    private static string NextFreeId()
    {
        var used = new HashSet<string>(GlobData.Stations.Select(s => s.ID));
        foreach (var station in GlobData.CustomStations)
            used.Add(station.ID);

        var id = 9000001;
        while (used.Contains(id.ToString()))
            id++;

        return id.ToString();
    }

    private void bSkipAll_Click(object sender, EventArgs e)
    {
        foreach (DataGridViewRow row in dgvStations.Rows)
        {
            row.Cells[1].Value = SkipItem;
            row.Cells[1].Style.ForeColor = dgvStations.DefaultCellStyle.ForeColor;
        }
    }

    private void bOK_Click(object sender, EventArgs e)
    {
        Result.Clear();

        foreach (DataGridViewRow row in dgvStations.Rows)
        {
            var elisName = (string)row.Cells[0].Value;
            var chosen = row.Cells[1].Value as string;

            if (string.IsNullOrEmpty(chosen) || chosen == SkipItem)
            {
                Result[elisName] = TxtParser.ELIS_MAP_SKIP;
                continue;
            }

            var station = _stations.FirstOrDefault(s => s.Name == chosen);
            Result[elisName] = station is null ? TxtParser.ELIS_MAP_SKIP : station.ID;
        }

        DialogResult = DialogResult.OK;
    }

    private void bStorno_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;
}
