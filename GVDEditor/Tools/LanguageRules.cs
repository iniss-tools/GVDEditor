using GVDEditor.Properties;
using ToolsCore.Entities;

namespace GVDEditor.Tools;

/// <summary>
///     Pravidla INISSu pre jazyky stanice (globalny Categori.TXT): najviac tri jazyky, len styri zname kluce
///     a prave jeden hlavny jazyk.
/// </summary>
internal static class LanguageRules
{
    /// <summary>
    ///     Najvacsi pocet jazykov - pri vacsom COUNT_LANGUAGES INISS nacitanie prerusi.
    /// </summary>
    public const int MaxLanguages = 3;

    /// <summary>
    ///     Kluce jazykov, ktore INISS pozna; ine preskoci.
    /// </summary>
    public static readonly string[] InissKeys = ["SK", "CZ", "GB", "D"];

    /// <summary>
    ///     Skontroluje zoznam jazykov po pridani alebo uprave.
    /// </summary>
    /// <param name="languages">jazyky tak, ako by po zmene vyzerali</param>
    /// <param name="bankKeys">kluce jazykov zvukovej banky</param>
    /// <returns>Text chyby, alebo <see langword="null" />, ak je zoznam v poriadku.</returns>
    public static string? Check(IReadOnlyList<FyzLanguage> languages, IEnumerable<string> bankKeys)
    {
        if (languages.Count > MaxLanguages)
            return string.Format(Resources.LanguageRules_Najviac_jazykov, MaxLanguages);

        var bank = bankKeys.ToList();
        foreach (var language in languages)
        {
            if (!InissKeys.Contains(language.Key))
                return string.Format(Resources.LanguageRules_Neznamy_kluc, language.Key, string.Join(", ", InissKeys));

            if (!bank.Contains(language.Key))
                return Resources.FGlobalSettings_Kľúč_jazyka_sa_nezhoduje_so_žiadnym_jazykom_nacházajúci_sa_v_zvukovej_banke;
        }

        if (languages.GroupBy(l => l.Key).Any(g => g.Count() > 1))
            return Resources.FGlobalSettings_Zadaný_jazyk_sa_sa_už_v_zozname_nachádza;

        return languages.Count(l => l.IsBasic) switch
        {
            0 => Resources.LanguageRules_Chyba_hlavny_jazyk,
            > 1 => Resources.FGlobalSettings_bLanguageAdd_Click_Iba_1_jazyk_môže_byť_hlavný,
            _ => null
        };
    }
}
