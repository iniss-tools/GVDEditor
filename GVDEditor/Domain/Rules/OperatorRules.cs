using System.Globalization;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Rules;

/// <summary>
/// Pravidla ciselnika dopravcov grafikonu (Vlastnik.txt). Nazov je zaroven meno nahravky dopravcu
/// v zvukovej banke a zapisuje sa v uvodzovkach bez escapovania.
/// </summary>
internal static class OperatorRules
{
    /// <summary>
    /// Chyba nazvu dopravcu na pozicii <paramref name="index" />.
    /// </summary>
    /// <param name="names">nazvy vsetkych dopravcov v poradi zoznamu</param>
    /// <param name="index">pozicia kontrolovaneho dopravcu</param>
    /// <returns>Text chyby, alebo <see langword="null" />, ak je nazov v poriadku.</returns>
    public static string? CheckName(IReadOnlyList<string> names, int index)
    {
        var name = names[index].Trim();
        if (name.Length == 0)
            return Resources.OperatorRules_Nazov_prazdny;

        if (name.Contains('"'))
            return Resources.SettingsRules_Uvodzovky;

        // nahravky su subory - nazvy lisiace sa len velkostou pismen by viedli na tu istu nahravku
        for (var i = 0; i < names.Count; i++)
            if (i != index && string.Equals(names[i].Trim(), name, StringComparison.OrdinalIgnoreCase))
                return string.Format(CultureInfo.CurrentCulture, Resources.OperatorRules_Nazov_existuje, name);

        return null;
    }

    /// <summary>
    /// Cislo pre noveho dopravcu - o jedno vyssie nez najvyssie pouzite, aby nekolidovalo s dopravcom,
    /// ktory medzitym zmizol zo zoznamu, ani s cislovanim s medzerami.
    /// </summary>
    public static int NextId(IEnumerable<int> ids) => Math.Max(1, ids.DefaultIfEmpty(0).Max() + 1);
}
