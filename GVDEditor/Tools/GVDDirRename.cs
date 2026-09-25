using GVDEditor.Entities;
using GVDEditor.Properties;

namespace GVDEditor.Tools;

/// <summary>
///     Premenovanie priecinka grafikonu (Lokalne nastavenia → Grafikon). Tlacidlo Zmenit nazov len overi a naplanuje,
///     priecinok sa premenuje az tlacidlom Ulozit.
/// </summary>
internal static class GVDDirRename
{
    /// <summary>
    ///     Overi novy nazov priecinka grafikonu.
    /// </summary>
    /// <param name="dirname">Novy nazov priecinka (uz orezany).</param>
    /// <param name="oldFullPath">Sucasna cesta k priecinku grafikonu.</param>
    /// <param name="dataDir">Priecinok DATA.</param>
    /// <param name="fullPath">Cesta k priecinku s novym nazvom.</param>
    /// <returns>Text chyby alebo <see langword="null" />, ak sa priecinok smie premenovat.</returns>
    public static string? Validate(string dirname, string oldFullPath, string dataDir, out string fullPath)
    {
        //názov ide priamo do cesty, takže sa musí overiť skôr, než sa s ním čokoľvek spraví
        fullPath = dataDir + Path.DirectorySeparatorChar + dirname;

        if (string.IsNullOrEmpty(dirname))
            return Resources.FLocalSettings_Názov_priečinka_grafikonu_je_prázdny;

        var invalidIndex = dirname.IndexOfAny(Path.GetInvalidFileNameChars());
        if (invalidIndex != -1)
            return string.Format(Resources.FLocalSettings_Názov_priečinka_grafikonu_obsahuje_nepovolený_znak,
                $"'{dirname[invalidIndex]}'");

        //Windows koncovú bodku z názvu priečinka ticho zahodí - priečinok na disku by sa potom
        //volal inak, než čo je zapísané v DIRLIST.txt, a grafikon by sa nabudúce nenašiel
        if (dirname.EndsWith(".", StringComparison.Ordinal))
            return Resources.FLocalSettings_Názov_priečinka_grafikonu_nesmie_končiť_bodkou;

        //zmena len vo veľkosti písmen je na Windowse platné premenovanie,
        //hoci Directory.Exists na taký názov vráti true
        var onlyCaseChanged = string.Equals(fullPath, oldFullPath, StringComparison.OrdinalIgnoreCase);
        if (!onlyCaseChanged && Directory.Exists(fullPath))
            return Resources.Priečinok_s_týmto_názvom_už_existuje__Zmeňte_jeho_názov;

        return null;
    }

    /// <summary>
    ///     Po premenovani priecinka na disku prepise nazov a cestu v zazname grafikonu aj v zozname
    ///     <paramref name="allDirs" /> (obsah <c>DirList.TXT</c>), kde moze byt ina instancia toho isteho zaznamu.
    /// </summary>
    /// <param name="dir">Zaznam upravovaneho grafikonu.</param>
    /// <param name="allDirs">Vsetky zaznamy <c>DirList.TXT</c>.</param>
    /// <param name="dirname">Novy nazov priecinka.</param>
    /// <param name="fullPath">Nova cesta k priecinku.</param>
    public static void UpdateEntries(DirList dir, IEnumerable<DirList> allDirs, string dirname, string fullPath)
    {
        var oldFullPath = dir.FullPath;
        foreach (var entry in allDirs.Where(entry => ReferenceEquals(entry, dir) ||
                                                     string.Equals(entry.FullPath, oldFullPath, StringComparison.OrdinalIgnoreCase)))
        {
            entry.DirName = dirname;
            entry.FullPath = fullPath;
        }

        dir.DirName = dirname;
        dir.FullPath = fullPath;
    }
}
