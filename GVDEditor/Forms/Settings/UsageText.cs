using System.Globalization;
using GVDEditor.Properties;

namespace GVDEditor.Forms.Settings;

/// <summary>
///     Text „Pouziva sa v“ pod udajmi tabule: najviac niekolko riadkov, zvysok sa zhrnie poctom.
/// </summary>
internal static class UsageText
{
    private const int Shown = 6;

    /// <summary>
    ///     Zostavi text z miest pouzitia.
    /// </summary>
    /// <param name="usage">miesta, kde sa polozka pouziva</param>
    /// <param name="blocksDelete">ci pouzita polozka nejde odstranit (pripise sa vysvetlenie)</param>
    /// <param name="noUsage">text, ked sa polozka nikde nepouziva</param>
    public static string Format(IReadOnlyList<string> usage, bool blocksDelete, string noUsage)
    {
        if (usage.Count == 0)
            return noUsage;

        var lines = usage.Take(usage.Count > Shown ? Shown - 1 : Shown).Select(u => "– " + u).ToList();
        if (usage.Count > Shown)
            lines.Add(string.Format(CultureInfo.CurrentCulture, Resources.TablesPage_Dalsie, usage.Count - lines.Count));
        if (blocksDelete)
            lines.Add(Resources.TablesPage_Pouzivana_neodstranit);
        return string.Join(Environment.NewLine, lines);
    }
}
