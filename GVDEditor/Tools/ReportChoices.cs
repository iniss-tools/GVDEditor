using GVDEditor.Entities;

namespace GVDEditor.Tools;

/// <summary>
///     Vyber hlaseni dodatku alebo radenia (tabulka Kedy hlasit): pri ktorom type hlasenia a v ktorej variante
///     (dlhe, kratke) zaznie.
/// </summary>
internal static class ReportChoices
{
    /// <summary>
    ///     Je hlasenie <paramref name="type" /> vo variante <paramref name="variant" /> vybrane.
    /// </summary>
    public static bool IsChosen(IEnumerable<ChosenReportType> chosen, ReportType type, ReportVariant variant) =>
        chosen.Any(item => item.Type == type && item.Variants.Contains(variant));

    /// <summary>
    ///     Zaskrtne alebo odskrtne hlasenie. Varianty ostavaju v poradi <paramref name="variants" /> a typ bez variant
    ///     sa zo zoznamu odstrani (rovnako ako ho zapisuje subor grafikonu).
    /// </summary>
    /// <param name="chosen">vybrane hlasenia (meni sa)</param>
    /// <param name="type">typ hlasenia</param>
    /// <param name="variant">varianta hlasenia</param>
    /// <param name="value">zaskrtnute</param>
    /// <param name="variants">vsetky varianty hlaseni v poradi grafikonu</param>
    public static void Set(List<ChosenReportType> chosen, ReportType type, ReportVariant variant, bool value,
        IReadOnlyList<ReportVariant> variants)
    {
        var item = chosen.FirstOrDefault(c => c.Type == type);
        if (value)
        {
            if (item == null)
            {
                item = new ChosenReportType { Type = type };
                chosen.Add(item);
            }

            if (!item.Variants.Contains(variant))
            {
                item.Variants.Add(variant);
                item.Variants.Sort((a, b) => IndexOf(variants, a).CompareTo(IndexOf(variants, b)));
            }
        }
        else if (item != null)
        {
            item.Variants.Remove(variant);
            if (item.Variants.Count == 0)
                chosen.Remove(item);
        }
    }

    /// <summary>
    ///     Pocet zaskrtnutych hlaseni.
    /// </summary>
    public static int Count(IEnumerable<ChosenReportType> chosen) => chosen.Sum(item => item.Variants.Count);

    private static int IndexOf(IReadOnlyList<ReportVariant> variants, ReportVariant variant)
    {
        for (var i = 0; i < variants.Count; i++)
            if (variants[i] == variant)
                return i;

        return int.MaxValue;
    }
}
