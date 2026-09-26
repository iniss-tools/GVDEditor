using GVDEditor.Entities;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.Tools;

/// <summary>
///     Kontroly pred importom grafikonu (Subor → Importovat → Grafikon…) - priecinok sa skopiruje do DATA
///     a zapise do DirList.TXT.
/// </summary>
internal static class GVDImport
{
    /// <summary>
    ///     Skontroluje, ci sa grafikon z priecinka <paramref name="sourcePath" /> da pridat do instalacie.
    /// </summary>
    /// <param name="sourcePath">vybrany priecinok s grafikonom</param>
    /// <param name="dataDir">priecinok DATA otvorenej instalacie</param>
    /// <param name="gvd">hlavicka importovaneho grafikonu (Grafikon.txt)</param>
    /// <param name="existing">grafikony instalacie</param>
    /// <param name="targetPath">priecinok, v ktorom bude grafikon v DATA</param>
    /// <returns>Text chyby, alebo <see langword="null" />, ak sa grafikon da importovat.</returns>
    public static string? Check(string sourcePath, string dataDir, GVDInfo gvd, IReadOnlyCollection<GVDDirectory> existing,
        out string targetPath)
    {
        var source = Path.GetFullPath(sourcePath).TrimEnd(Path.DirectorySeparatorChar);
        var dirName = Path.GetFileName(source);
        targetPath = Path.Combine(dataDir, dirName ?? "");

        if (string.IsNullOrEmpty(dirName))
            return Resources.FMain_Názov_priečinka_je_prázdny;

        if (existing.Any(d => string.Equals(d.Dir.DirName, dirName, StringComparison.OrdinalIgnoreCase)))
            return Resources.Priečinok_s_týmto_názvom_už_existuje__Zmeňte_jeho_názov;

        // priecinok uz lezi v DATA (napr. nakopirovany rucne) - len sa zapise do DirList.TXT
        var inPlace = string.Equals(source, Path.GetFullPath(targetPath), StringComparison.OrdinalIgnoreCase);
        if (!inPlace && Directory.Exists(targetPath))
            return Resources.Priečinok_s_týmto_názvom_už_existuje__Zmeňte_jeho_názov;

        // kopia do seba samej (napr. vybrany samotny priecinok DATA)
        if (!inPlace && Path.GetFullPath(targetPath).StartsWith(source + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            return Resources.FMain_Import_grafikonu_do_seba;

        var period = new Interval(gvd.StartValidTimeTable, gvd.EndValidTimeTable);
        if (existing.Any(d => d.GVD.ThisStation.Name == gvd.ThisStation.Name &&
                              period.Overlaps(new Interval(d.GVD.StartValidTimeTable, d.GVD.EndValidTimeTable))))
            return Resources.FNewGrafikon_Zadané_obdobie_platnosti_tejto_stanice_už_existuje;

        return null;
    }
}
