using GVDEditor.Entities;

namespace GVDEditor.Tools;

/// <summary>
///     Zoznamy pre comboboxy Stanica a Obdobie v hlavnom okne - po zmene stanice grafikonu v lokalnych
///     nastaveniach sa musia prisposobit bez znovunacitania priecinka INISS.
/// </summary>
internal static class GVDSelectionLists
{
    /// <summary>
    ///     Upravi zoznam nazvov stanic po zmene stanice grafikonu z <paramref name="oldName" /> na
    ///     <paramref name="newName" />. Stary nazov nahradi novym (na tom istom mieste), ak ho uz nepouziva
    ///     ziadny grafikon; novy nazov prida, ak v zozname chyba. Nazov sa v zozname nikdy nezdvoji.
    /// </summary>
    /// <param name="stanice">Zoznam nazvov stanic (zdroj comboboxu Stanica).</param>
    /// <param name="dirs">Vsetky grafikony, uz s novou stanicou upraveneho grafikonu.</param>
    /// <param name="oldName">Nazov stanice pred zmenou.</param>
    /// <param name="newName">Nazov stanice po zmene.</param>
    public static void RenameStation(IList<string> stanice, IEnumerable<GVDDirectory> dirs, string oldName, string newName)
    {
        if (oldName == newName) return;

        var oldIndex = stanice.IndexOf(oldName);
        var oldUsed = dirs.Any(dir => dir.GVD.ThisStation.Name == oldName);
        var newPresent = stanice.Contains(newName);

        if (oldIndex >= 0 && !oldUsed)
        {
            if (newPresent) stanice.RemoveAt(oldIndex);
            else stanice[oldIndex] = newName;
        }
        else if (!newPresent)
        {
            stanice.Add(newName);
        }
    }

    /// <summary>
    ///     Vrati grafikony danej stanice v poradi zoznamu <paramref name="dirs" /> (zdroj comboboxu Obdobie).
    /// </summary>
    public static IEnumerable<GVDDirectory> PeriodsOf(IEnumerable<GVDDirectory> dirs, string station) =>
        dirs.Where(dir => dir.GVD.ThisStation.Name == station);
}
