using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.TabTabEditor;
using GVDEditor.UI.TabTab;
using GVDEditor.Properties;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranka TabTab v okne Lokalne nastavenia - zoznam sekcii TabTab, obsah vybranej sekcie a kde sa pouziva.
/// Sekcie sa upravuju v editore TabTab (samostatne okno), ktory ich zapise do <see cref="_ctx.Document.TabTabs" />.
/// </summary>
public partial class TabTabPage : UserControl
{
    /// <summary>
    /// Kontext editora - nastavi ho <c>LoadData</c>.
    /// </summary>
    private EditorContext _ctx = null!;

    private readonly ItemListSupport<TableTabTab> _list;
    private Station? _station;
    private bool _loaded;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public TabTabPage()
    {
        InitializeComponent();
        _list = new ItemListSupport<TableTabTab>(dgv, tbFilter, Sections, tab => [tab.Key, Lines(tab)]);
        _list.SelectionChanged += (_, _) => ShowCurrent();
    }

    /// <summary>
    /// Naplni stranku - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name=\"context\">kontext editora</param>
    /// <param name="station">stanica grafikonu - editor podla nej ponuka stanice</param>
    internal void LoadData(EditorContext context, Station station)
    {
        _ctx = context;
        _station = station;
        foreach (var header in new[] { lTextHeader, lUseHeader })
            header.Font = new Font(Font, FontStyle.Bold);
        // obsah sekcie je zarovnany do stlpcov - neproporcionalne pismo
        tbText.Font = new Font(FontFamily.GenericMonospace, Font.SizeInPoints);
        _list.CaptureColors();

        _loaded = true;
        _list.Fill(Sections().FirstOrDefault());
    }

    /// <inheritdoc />
    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        // stlpce katalogovych tabul sa mohli zmenit na inej stranke okna
        if (Visible && _loaded)
            _list.Fill(_list.Current);
    }

    /// <summary>
    /// Otvori editor TabTab (napr. hned po otvoreni okna z hlavneho menu).
    /// </summary>
    public void OpenEditor() => bEditor_Click(this, EventArgs.Empty);

    // prvou polozkou zoznamu je zabudovana prazdna sekcia „Ziadny“ - v zozname nie je
    private IEnumerable<TableTabTab> Sections() => _ctx.Document.TabTabs.Where(tab => tab != TableTabTab.Empty);

    private static string Lines(TableTabTab tab) =>
        (tab.Text ?? "").Split('\n').Count(line => line.Trim().Length > 0).ToString(CultureInfo.CurrentCulture);

    private void ShowCurrent()
    {
        var tab = _list.Current;
        tbText.Text = (tab?.Text ?? "").Replace("\r\n", "\n").Replace("\n", Environment.NewLine);

        var usage = tab is null ? [] : TabTabSections.FindUsage(tab, _ctx.Document.TableCatalogs);
        lUse.Text = tab is null ? "" : UsageText.Format(usage, true, Resources.TablesPage_Nepouziva);
        bDelete.Enabled = tab is not null && usage.Count == 0;
    }

    private void bEditor_Click(object? sender, EventArgs e)
    {
        // editor sa otvori na vybranej sekcii, bez vyberu prazdny
        var current = _list.Current;
        using (var form = new FTabTab(_ctx, current, _station))
            form.ShowDialog(FindForm());

        // editor mohol sekcie pridat, premenovat aj odstranit
        _list.Fill(current is not null && _ctx.Document.TabTabs.Contains(current) ? current : Sections().FirstOrDefault());
    }

    private void bDelete_Click(object? sender, EventArgs e)
    {
        if (_list.Current is not { } tab || TabTabSections.FindUsage(tab, _ctx.Document.TableCatalogs).Count > 0)
            return;

        var sections = Sections().ToList();
        var index = sections.IndexOf(tab);
        _ctx.Document.TabTabs.Remove(tab);
        sections.Remove(tab);
        _list.Fill(sections.Count == 0 ? null : sections[Math.Min(index, sections.Count - 1)]);
    }

    private void dgv_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            bEditor_Click(this, EventArgs.Empty);
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Enter:
                bEditor_Click(this, EventArgs.Empty);
                e.Handled = true;
                break;
            case Keys.Delete:
                bDelete_Click(this, EventArgs.Empty);
                e.Handled = true;
                break;
        }
    }
}
