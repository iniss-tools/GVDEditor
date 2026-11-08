using GVDEditor.Config;

namespace GVDEditor.Integration;

/// <summary>
/// Konfiguracie spustania otvorenej instalacie: ulozene na tomto pocitaci (config.xml) aj zdielane v datach
/// instalacie, a vybrana konfiguracia.
/// </summary>
internal sealed class RunConfigurationSet
{
    /// <summary>
    /// Vytvori mnozinu pre instalaciu <paramref name="installationDir" />.
    /// </summary>
    public RunConfigurationSet(string installationDir, IEnumerable<RunConfiguration> items, string? selectedId)
    {
        InstallationDir = installationDir;
        Items = items.ToList();
        Sort();
        SelectedId = selectedId;
    }

    /// <summary>Priecinok instalacie.</summary>
    public string InstallationDir { get; }

    /// <summary>Konfiguracie zoradene podla nazvu.</summary>
    public List<RunConfiguration> Items { get; private set; }

    /// <summary>Identifikator vybranej konfiguracie.</summary>
    public string? SelectedId { get; set; }

    /// <summary>
    /// Konfiguracie su predvolene (instalacia ziadne ulozene nemala) - ulozia sa az pri prvej zmene.
    /// </summary>
    public bool IsDefault { get; init; }

    /// <summary>Vybrana konfiguracia, inak prva.</summary>
    public RunConfiguration? Selected => Find(SelectedId) ?? Items.FirstOrDefault();

    /// <summary>Konfiguracia podla identifikatora.</summary>
    public RunConfiguration? Find(string? id) => id is null ? null : Items.Find(i => i.Id == id);

    /// <summary>Nahradi vsetky konfiguracie (okno Konfiguracie spustania).</summary>
    public void Replace(IEnumerable<RunConfiguration> items)
    {
        Items = items.ToList();
        Sort();
    }

    private void Sort() => Items.Sort((a, b) => StringComparer.CurrentCultureIgnoreCase.Compare(a.Name, b.Name));
}
