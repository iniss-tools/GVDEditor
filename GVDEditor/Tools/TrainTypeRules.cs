using System.Globalization;
using System.Text.RegularExpressions;
using GVDEditor.Entities;
using GVDEditor.Properties;

namespace GVDEditor.Tools;

/// <summary>
///     Pravidla zoznamu typov vlakov (TrTypes.txt). INISS ma zabudovanu tabulku druhov, kazdy na jednom mieste,
///     a styri skupiny po devat volnych miest pre vlastne typy (Os, R, X, Sl).
/// </summary>
internal static partial class TrainTypeRules
{
    /// <summary>
    ///     Skupiny vlastnych typov v poradi, v akom sa ponukaju.
    /// </summary>
    public static readonly string[] CustomGroups = ["Os", "R", "X", "Sl"];

    /// <summary>
    ///     Pocet volnych miest v skupine.
    /// </summary>
    public const int SlotsPerGroup = 9;

    [GeneratedRegex("^(Os|R|X|Sl)[0-9]+$")]
    private static partial Regex CustomCategory();

    /// <summary>
    ///     Skupina vlastneho typu podla kategorie (R3 -> R), alebo <see langword="null" /> pri zabudovanom druhu.
    ///     Pripusta aj cislo nad 9 - to je chyba, ktoru hlasi <see cref="CheckCategory" />.
    /// </summary>
    public static string? GroupOf(string category)
    {
        var match = CustomCategory().Match(category);
        return match.Success ? match.Groups[1].Value : null;
    }

    /// <summary>
    ///     Chyba skratky typu na pozicii <paramref name="index" />.
    /// </summary>
    public static string? CheckKey(IReadOnlyList<TrainType> types, int index)
    {
        var key = types[index].Key.Trim();
        if (key.Length == 0)
            return Resources.FGlobalSettings_Nebola_zadaná_skratka_typu_vlaku;

        if (HasForbiddenChar(key))
            return Resources.TrainTypeRules_Znaky;

        for (var i = 0; i < types.Count; i++)
            if (i != index && types[i].Key.Trim() == key)
                return Resources.FGlobalSettings_Vybraný_typ_vlaku_sa_už_v_zozname_nachádza;

        return null;
    }

    /// <summary>
    ///     Chyba textu na tabuli - subor je CSV bez uvodzoviek, ciarka by rozdelila stlpec.
    /// </summary>
    public static string? CheckText(string text) => HasForbiddenChar(text) ? Resources.TrainTypeRules_Znaky : null;

    /// <summary>
    ///     Chyba kategorie typu na pozicii <paramref name="index" /> - zabudovany druh dvakrat (druhy riadok by
    ///     v INISSe prepisal prvy) alebo viac vlastnych typov v skupine, nez je volnych miest.
    /// </summary>
    public static string? CheckCategory(IReadOnlyList<TrainType> types, int index)
    {
        var category = types[index].CategoryTrain;
        var group = GroupOf(category);
        if (group is not null)
        {
            var number = int.Parse(category[group.Length..], CultureInfo.InvariantCulture);
            return number is < 1 or > SlotsPerGroup ? Resources.FGlobalSettings_Maximálny_počet_typov_vlakov_tohto_druhu_je_9 : null;
        }

        for (var i = 0; i < index; i++)
            if (types[i].CategoryTrain == category)
                return string.Format(CultureInfo.CurrentCulture, Resources.FGlobalSettings_Kategoria_typu_obsadena, category);

        return null;
    }

    /// <summary>
    ///     Pocet vlastnych typov v skupine <paramref name="group" /> (okrem <paramref name="exclude" />).
    /// </summary>
    public static int CountInGroup(IEnumerable<TrainType> types, string group, TrainType? exclude = null) =>
        types.Count(t => !ReferenceEquals(t, exclude) && GroupOf(t.CategoryTrain) == group);

    /// <summary>
    ///     Precisluje vlastne typy v kazdej skupine od 1 v poradi zoznamu (R2, R5 -> R1, R2). Vlaky sa na typ
    ///     odkazuju skratkou, nie kategoriou, takze ich sa to netyka.
    /// </summary>
    public static void Renumber(IEnumerable<TrainType> types)
    {
        var counters = CustomGroups.ToDictionary(g => g, _ => 0);
        foreach (var type in types)
            if (GroupOf(type.CategoryTrain) is { } group)
                type.CategoryTrain = group + ++counters[group];
    }

    private static bool HasForbiddenChar(string value) => value.Contains(',') || value.Contains('"');
}
