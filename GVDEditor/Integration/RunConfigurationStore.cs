using GVDEditor.Config;
using ToolsCore.XML;

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

/// <summary>
/// Nacitanie a ulozenie konfiguracii spustania. Konfiguracie tohto pocitaca su v config.xml podla priecinka
/// instalacie, zdielane v subore <c>.gvdeditor\RunConfigurations.xml</c> v priecinku instalacie (putuju s datami).
/// Vyber konfiguracie je vzdy len na tomto pocitaci.
/// </summary>
internal static class RunConfigurationStore
{
    /// <summary>Priecinok GVDEditora v datach instalacie.</summary>
    public const string SharedDirName = ".gvdeditor";

    /// <summary>Subor zdielanych konfiguracii.</summary>
    public const string SharedFileName = "RunConfigurations.xml";

    /// <summary>Cesta k suboru zdielanych konfiguracii instalacie.</summary>
    public static string SharedPath(string installationDir) => Path.Combine(installationDir, SharedDirName, SharedFileName);

    /// <summary>
    /// Nacita konfiguracie instalacie. Ak instalacia nema ulozenu ziadnu (ani zdielanu), vrati predvolene - jednu na
    /// kazdy program INISS s povodnym spolocnym nastavenim spustania; tie sa na disk zapisu az pri prvej zmene.
    /// </summary>
    /// <param name="config">konfiguracia programu</param>
    /// <param name="installationDir">priecinok instalacie</param>
    /// <param name="programs">programy INISS instalacie (pre predvolene konfiguracie)</param>
    /// <exception cref="InvalidOperationException">Subor zdielanych konfiguracii sa neda precitat - lokalne konfiguracie
    /// su vo vynimke <see cref="RunConfigurationLoadException.Partial" />.</exception>
    public static RunConfigurationSet Load(GVDEditorConfig config, string installationDir, IReadOnlyList<string> programs)
    {
        var local = Find(config, installationDir);
        var sharedPath = SharedPath(installationDir);
        var shared = new List<RunConfiguration>();
        Exception? error = null;
        if (File.Exists(sharedPath))
        {
            try
            {
                shared = XmlSerialization.ReadData<SharedRunConfigurations>(sharedPath).Items;
            }
            catch (Exception e) when (e is IOException or UnauthorizedAccessException or InvalidOperationException)
            {
                error = e;
            }

            foreach (var item in shared) item.Shared = true;
        }

        RunConfigurationSet set;
        if (local is null && shared.Count == 0 && error is null)
            set = new RunConfigurationSet(installationDir, RunConfigurations.Defaults(programs, config.StartupINISSConfig), null) { IsDefault = true };
        else
        {
            var items = (local?.Items ?? []).Select(i => i with { Shared = false })
                .Concat(shared.Where(s => local?.Items.TrueForAll(l => l.Id != s.Id) ?? true));
            set = new RunConfigurationSet(installationDir, items, local?.Selected);
        }

        return error is null ? set : throw new RunConfigurationLoadException(set, error);
    }

    /// <summary>
    /// Zapise konfiguracie: lokalne do <paramref name="config" /> (config.xml zapise volajuci), zdielane do dat
    /// instalacie. Ak ziadna zdielana nie je, subor zdielanych konfiguracii sa odstrani.
    /// </summary>
    /// <exception cref="IOException">Subor zdielanych konfiguracii sa nepodarilo zapisat.</exception>
    /// <exception cref="UnauthorizedAccessException">Do priecinka instalacie sa neda zapisovat.</exception>
    public static void Save(GVDEditorConfig config, RunConfigurationSet set)
    {
        var sharedPath = SharedPath(set.InstallationDir);
        var shared = set.Items.Where(i => i.Shared).ToList();
        if (shared.Count > 0)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(sharedPath)!);
            XmlSerialization.WriteData(sharedPath, new SharedRunConfigurations { Items = shared });
        }
        else if (File.Exists(sharedPath))
        {
            File.Delete(sharedPath);
            var dir = Path.GetDirectoryName(sharedPath)!;
            if (!Directory.EnumerateFileSystemEntries(dir).Any())
                Directory.Delete(dir);
        }

        SaveSelection(config, set);
    }

    /// <summary>
    /// Zapise do <paramref name="config" /> lokalne konfiguracie a vyber (zdielany subor nemeni).
    /// </summary>
    public static void SaveSelection(GVDEditorConfig config, RunConfigurationSet set)
    {
        var entry = Find(config, set.InstallationDir);
        if (entry is null)
        {
            entry = new InstallationRunConfigurations { Dir = Normalize(set.InstallationDir) };
            config.RunConfigurations.Add(entry);
        }

        entry.Selected = set.SelectedId ?? "";
        entry.Items = set.Items.Where(i => !i.Shared).Select(i => i with { }).ToList();
    }

    private static InstallationRunConfigurations? Find(GVDEditorConfig config, string installationDir)
    {
        var dir = Normalize(installationDir);
        return config.RunConfigurations.Find(r => string.Equals(Normalize(r.Dir), dir, StringComparison.OrdinalIgnoreCase));
    }

    private static string Normalize(string dir)
    {
        try
        {
            return Path.TrimEndingDirectorySeparator(Path.GetFullPath(dir));
        }
        catch (Exception e) when (e is ArgumentException or NotSupportedException or PathTooLongException)
        {
            return dir.TrimEnd('\\', '/');
        }
    }
}

/// <summary>
/// Subor zdielanych konfiguracii sa nepodarilo precitat; <see cref="Partial" /> obsahuje konfiguracie tohto pocitaca.
/// </summary>
internal sealed class RunConfigurationLoadException(RunConfigurationSet partial, Exception inner) : InvalidOperationException(inner.Message, inner)
{
    /// <summary>Konfiguracie, ktore sa nacitat dali.</summary>
    public RunConfigurationSet Partial { get; } = partial;
}
