using GVDEditor.Properties;
using ToolsCore.Iniss.Entities;

namespace GVDEditor.Domain.Rules;

/// <summary>
/// Pravidla INISSu pre jazyky stanice (globalny Categori.TXT): najviac tri jazyky, len styri zname kluce
/// a prave jeden hlavny jazyk.
/// </summary>
internal static class LanguageRules
{
    /// <summary>
    /// Najvacsi pocet jazykov - pri vacsom COUNT_LANGUAGES INISS nacitanie prerusi.
    /// </summary>
    public const int MaxLanguages = 3;

    /// <summary>
    /// Kluce jazykov, ktore INISS pozna; ine preskoci.
    /// </summary>
    public static readonly string[] InissKeys = ["SK", "CZ", "GB", "D"];

    /// <summary>
    /// Skontroluje zoznam jazykov po pridani alebo uprave.
    /// </summary>
    /// <param name="languages">jazyky tak, ako by po zmene vyzerali</param>
    /// <param name="bankKeys">kluce jazykov zvukovej banky</param>
    /// <returns>Text chyby, alebo <see langword="null" />, ak je zoznam v poriadku.</returns>
    public static string? Check(IReadOnlyList<FyzLanguage> languages, IEnumerable<string> bankKeys)
    {
        if (languages.Count > MaxLanguages)
            return string.Format(Resources.LanguageRules_Najviac_jazykov, MaxLanguages);

        var bank = bankKeys.ToList();
        for (var i = 0; i < languages.Count; i++)
        {
            var error = CheckLanguage(languages, i, bank);
            if (error != null)
                return error;
        }

        return CheckBasic(languages);
    }

    /// <summary>
    /// Chyba kluca jazyka na pozicii <paramref name="index" /> - kluc, ktory INISS nepozna, chyba v zvukovej banke
    /// alebo ho ma aj iny jazyk.
    /// </summary>
    /// <returns>Text chyby, alebo <see langword="null" />, ak je kluc v poriadku.</returns>
    public static string? CheckLanguage(IReadOnlyList<FyzLanguage> languages, int index, IReadOnlyCollection<string> bankKeys)
    {
        var key = languages[index].Key;
        if (!InissKeys.Contains(key))
            return string.Format(Resources.LanguageRules_Neznamy_kluc, key, string.Join(", ", InissKeys));

        if (!bankKeys.Contains(key))
            return Resources.FGlobalSettings_Kľúč_jazyka_sa_nezhoduje_so_žiadnym_jazykom_nacházajúci_sa_v_zvukovej_banke;

        for (var i = 0; i < languages.Count; i++)
            if (i != index && languages[i].Key == key)
                return Resources.FGlobalSettings_Zadaný_jazyk_sa_sa_už_v_zozname_nachádza;

        return null;
    }

    /// <summary>
    /// Chyba vyberu hlavneho jazyka - hlavny musi byt prave jeden.
    /// </summary>
    /// <returns>Text chyby, alebo <see langword="null" />, ak je hlavny prave jeden.</returns>
    public static string? CheckBasic(IReadOnlyList<FyzLanguage> languages) =>
        languages.Count(l => l.IsBasic) switch
        {
            0 => Resources.LanguageRules_Chyba_hlavny_jazyk,
            > 1 => Resources.FGlobalSettings_bLanguageAdd_Click_Iba_1_jazyk_môže_byť_hlavný,
            _ => null
        };
}
