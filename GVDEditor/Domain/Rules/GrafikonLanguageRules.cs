using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Entities;

namespace GVDEditor.Domain.Rules;

/// <summary>
///     Jazyky, ktore grafikon pouziva (sekcie LANGUAGE_nn lokalneho Categori.txt). INISS jazyk mimo nich u vlaku
///     preskoci; nazov a priznak hlavneho jazyka berie vzdy z globalneho Categori.txt.
/// </summary>
internal static class GrafikonLanguageRules
{
    /// <summary>
    ///     Jazyky grafikonu po zmene globalnych jazykov: zmazane vypadnu, nove sa nepridaju, poradie je globalne.
    /// </summary>
    public static List<FyzLanguage> Sync(IEnumerable<FyzLanguage> grafikon, IEnumerable<FyzLanguage> global)
    {
        var used = grafikon.ToHashSet(ReferenceEqualityComparer.Instance);
        return global.Where(used.Contains).ToList();
    }

    /// <summary>
    ///     Chyba, ktora brani ulozeniu vyberu; <see langword="null" />, ak je vyber v poriadku.
    /// </summary>
    /// <param name="selected">zapnute jazyky</param>
    /// <param name="global">vsetky jazyky stanice</param>
    /// <param name="radenia">radenia grafikonu - nahravka vo vypnutom jazyku by grafikon uz neotvorila</param>
    public static string? Check(IReadOnlyCollection<FyzLanguage> selected, IEnumerable<FyzLanguage> global, IEnumerable<Radenie> radenia)
    {
        if (selected.Count == 0)
            return Resources.GrafikonLanguages_Ziadny;

        if (FyzLanguage.GetBasicLanguage(global) is { } basic && !selected.Contains(basic))
            return string.Format(CultureInfo.CurrentCulture, Resources.GrafikonLanguages_Hlavny_Vypnuty, basic.Name);

        foreach (var radenie in radenia)
            if (radenie.Sounds.FirstOrDefault(sound => !selected.Contains(sound.Language)) is { } sound)
                return string.Format(CultureInfo.CurrentCulture, Resources.GrafikonLanguages_Radenie, sound.Language.Name, radenie.CisloVlaku);

        return null;
    }

    /// <summary>
    ///     Upozornenia k vypnutym jazykom, ktore maju vlaky zapnute - INISS ich pri nich preskoci.
    /// </summary>
    public static List<string> Warnings(IReadOnlyCollection<FyzLanguage> selected, IEnumerable<FyzLanguage> global, IEnumerable<Train> trains)
    {
        var trainList = trains as IReadOnlyCollection<Train> ?? trains.ToList();
        var warnings = new List<string>();
        foreach (var language in global.Where(language => !selected.Contains(language)))
        {
            var count = trainList.Count(train => train.Languages.Contains(language));
            if (count > 0)
                warnings.Add(string.Format(CultureInfo.CurrentCulture, Resources.GrafikonLanguages_Vlaky, language.Name, count));
        }

        return warnings;
    }

    /// <summary>
    ///     Jazyky na vyber pri vlaku alebo radeni: jazyky grafikonu a navyse tie, ktore uz vyber obsahuje (aby sa
    ///     neulozenim okna potichu nestratili), v poradi globalneho zoznamu.
    /// </summary>
    public static List<FyzLanguage> Offered(IEnumerable<FyzLanguage> global, IEnumerable<FyzLanguage> grafikon, IEnumerable<FyzLanguage> alreadyUsed)
    {
        var offered = grafikon.Concat(alreadyUsed).ToHashSet(ReferenceEqualityComparer.Instance);
        return global.Where(offered.Contains).ToList();
    }
}
