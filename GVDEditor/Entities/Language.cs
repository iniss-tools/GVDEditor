// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
namespace GVDEditor.Entities;

/// <summary>
///     Jazyková mútácia používaná pre vlak
/// </summary>
public record Language
{
    /// <summary>
    ///     Kľúč jazyka
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    ///     Je jazyk v stanici hlavný
    /// </summary>
    public bool IsBasic { get; set; }

    /// <summary>
    ///     Celý názov jazyka
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Názov jazyka vo zvukovej banke
    /// </summary>
    public string FyzBankName { get; set; }

    /// <summary>
    ///     Súbor FYZZVUK.dat jazyka vo zvukovej banky
    /// </summary>
    public string FileFyzZvuk { get; set; }

    /// <summary>
    ///     Relatívna cesta k priečinku zvukov jazyka vo zvukej banke
    /// </summary>
    public string RelativePath { get; set; }

    /// <summary>
    ///     This
    /// </summary>
    public Language This => this;

    /// <summary>
    ///     Vráti jazyk z listujazyk z listu podľa kľúča jazyka
    /// </summary>
    /// <param name="langs">list zvukov</param>
    /// <param name="key">kľúč jazyka</param>
    /// <returns>jazyk z listu, resp. <see langword="null" /> ak nebola nájdená zhoda</returns>
    public static Language GetLanguageFromKey(IEnumerable<Language> langs, string key) => langs.FirstOrDefault(jazyk => jazyk.Key == key);

    /// <summary>
    ///     Vráti hlavný jazyk z listu zvukov
    /// </summary>
    /// <param name="langs">list jazykov</param>
    /// <returns>hlavný jazyk alebo <see langword="null" /> ak zadaný list neobsahuje hlavný jazyk</returns>
    public static Language GetBasicLanguage(IEnumerable<Language> langs) => langs.FirstOrDefault(jazyk => jazyk.IsBasic);

    /// <summary>
    ///     Zistí, či v zadanom poli jazykov sa nachádza prvok s rovnakým kľúčom ako zadaný kľúč
    /// </summary>
    /// <param name="languages">list jazykov</param>
    /// <param name="key">kľúč jazyka</param>
    /// <returns>
    ///     <see langword="true" /> ak sa v poli nachádza prvok s rovnakým kľučom ako zadaný kľúč, inak
    ///     <see langword="false" />
    /// </returns>
    public static bool ContainsKey(ICollection<Language> languages, string key)
    {
        if (key == null || languages == null || languages.Count == 0) 
            return false;

        return languages.Any(language => language.Key == key);
    }

    /// <inheritdoc />
    public override string ToString() => Name;
}