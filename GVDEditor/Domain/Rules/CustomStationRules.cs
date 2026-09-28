using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Rules;

/// <summary>
/// Pravidla vlastnych stanic grafikonu (Stanice.txt). GVDEditor pri nacitani vynecha stanicu, ktorej nazov
/// je v zvukovej banke alebo patri stanici grafikonu - taka stanica by po ulozeni zmizla.
/// </summary>
internal static class CustomStationRules
{
    /// <summary>
    /// Chyba cisla vlastnej stanice na pozicii <paramref name="index" />.
    /// </summary>
    /// <param name="ids">cisla vsetkych vlastnych stanic v poradi zoznamu</param>
    /// <param name="index">pozicia kontrolovanej stanice</param>
    /// <param name="bank">stanice zvukovej banky</param>
    /// <returns>Text chyby, alebo <see langword="null" />, ak je cislo v poriadku.</returns>
    public static string? CheckId(IReadOnlyList<string> ids, int index, IEnumerable<Station> bank)
    {
        var id = ids[index].Trim();
        if (!TryParseId(id, out _))
            return Resources.CustomStationRules_Cislo_zle;

        var inBank = bank.FirstOrDefault(station => station.ID == id);
        if (inBank != null)
            return string.Format(CultureInfo.CurrentCulture, Resources.CustomStationRules_Cislo_banka, id, inBank.Name);

        for (var i = 0; i < ids.Count; i++)
            if (i != index && ids[i].Trim() == id)
                return string.Format(CultureInfo.CurrentCulture, Resources.CustomStationRules_Cislo_existuje, id);

        return null;
    }

    /// <summary>
    /// Chyba nazvu vlastnej stanice na pozicii <paramref name="index" />.
    /// </summary>
    /// <param name="names">nazvy vsetkych vlastnych stanic v poradi zoznamu</param>
    /// <param name="index">pozicia kontrolovanej stanice</param>
    /// <param name="bank">stanice zvukovej banky</param>
    /// <param name="gvdStationName">nazov stanice grafikonu</param>
    /// <returns>Text chyby, alebo <see langword="null" />, ak je nazov v poriadku.</returns>
    public static string? CheckName(IReadOnlyList<string> names, int index, IEnumerable<Station> bank, string gvdStationName)
    {
        var name = names[index].Trim();
        if (name.Length == 0)
            return Resources.CustomStationRules_Nazov_prazdny;

        if (name.Contains('"'))
            return Resources.SettingsRules_Uvodzovky;

        if (Station.ContainsName(bank, name))
            return string.Format(CultureInfo.CurrentCulture, Resources.CustomStationRules_Nazov_banka, name);

        if (name == gvdStationName)
            return string.Format(CultureInfo.CurrentCulture, Resources.CustomStationRules_Nazov_grafikon, name);

        for (var i = 0; i < names.Count; i++)
            if (i != index && names[i].Trim() == name)
                return string.Format(CultureInfo.CurrentCulture, Resources.CustomStationRules_Nazov_existuje, name);

        return null;
    }

    /// <summary>
    /// Cislo pre novu vlastnu stanicu - o jedno vyssie nez najvyssie cislo v banke aj medzi vlastnymi stanicami.
    /// </summary>
    public static string SuggestId(IEnumerable<string> ids)
    {
        var max = 0;
        foreach (var id in ids)
            if (TryParseId(id, out var number) && number > max)
                max = number;

        return (max + 1).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Cislo stanice je kladne cele cislo bez znamienka a medzier.
    /// </summary>
    public static bool TryParseId(string id, out int number) =>
        int.TryParse(id, NumberStyles.None, CultureInfo.InvariantCulture, out number) && number > 0;
}
