using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using System.Globalization;
using ToolsCore.Iniss.Entities;
using ToolsCore.Iniss.Tools;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Iniss.Tools.ParseUtils;
using static ToolsCore.Iniss.Tools.PathUtils;
using static GVDEditor.Formats.FormatCommon;

namespace GVDEditor.Formats;

/// <summary>
/// Jazyky stanice, typy a varianty hlaseni (globalny a lokalny Categori.txt).
/// </summary>
internal static class CategoriFile
{
    /// <summary>
    /// Vrati informacie o jazykovych mutaciach hlaseni (pre vsetky GVD)
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="jazykyFromBank">jazykove mutacie z banky zvukov</param>
    /// <param name="maxLangs">maximalny pocet jazykovych mutacii (podla zvukovej banky)</param>
    /// <returns>jazyky</returns>
    public static List<FyzLanguage> ReadGlobal(string path, IList<FyzLanguage> jazykyFromBank, int maxLangs, LoadWarnings warnings)
    {
        var file = CombinePath(path, FILE_CATEGORI)!;

        var jazyky = new List<FyzLanguage>();

        var categoriF = new TxtPropsAreasFields(file);

        var count = int.Parse(categoriF.Get("MAIN", "COUNT_LANGUAGES"), CultureInfo.InvariantCulture);

        if (count > maxLangs)
            warnings.Add(string.Format(CultureInfo.CurrentCulture, Resources.Categori_TooManyLanguages, file, count, maxLangs));

        for (var i = 1; i <= count; i++)
        {
            var area = $"LANGUAGE_{i.PadZeros(2)}";

            if (!categoriF.GetAreas().Contains(area))
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, FORMAT_EX_AREA, file, area));

            var key = categoriF.Get(area, "KEY").ANSItoUTF();
            var isBasic = ParseIntOrDefault(categoriF.Get(area, "IS_BASIC", false)).ToBool();
            // NAME je nepovinne - INISS ma pre styri zname kluce zabudovane nazvy
            var name = categoriF.Get(area, "NAME", false)?.ANSItoUTF() ?? FyzLanguage.BuiltInName(key);

            if (!FyzLanguage.ContainsKey(jazykyFromBank, key))
            {
                // INISS neznamy kluc preskoci s varovanim; rovnako sa spravame aj my
                warnings.Add(string.Format(CultureInfo.CurrentCulture, Resources.Categori_LanguageMissing, file, key));
                continue;
            }

            foreach (var language in jazykyFromBank)
                if (language.Key == key)
                {
                    language.Name = name;
                    language.IsBasic = isBasic;
                    jazyky.Add(language);
                }
        }

        return jazyky;
    }

    /// <summary>
    /// Zapise informacie o jazykovych mutaciach hlaseni pouzivanych na stanici
    /// </summary>
    /// <param name="jazyky">jazyky</param>
    /// <param name="dataDir">priecinok DATA instalacie INISS</param>
    public static void WriteGlobal(string dataDir, List<FyzLanguage> jazyky)
    {
        var file = CombinePath(dataDir, FILE_CATEGORI)!;

        var categoriF = new TxtPropsAreasFields(file, true);

        categoriF.Set("MAIN", "COUNT_LANGUAGES", jazyky.Count);
        for (var i = 1; i <= jazyky.Count; i++)
        {
            var area = $"LANGUAGE_{i.PadZeros(2)}";
            categoriF.Set(area, "KEY", jazyky[i - 1].Key, WriteType.WriteStringANSI);
            var isBasic = jazyky[i - 1].IsBasic ? 1 : 0;
            categoriF.Set(area, "IS_BASIC", isBasic);
            categoriF.Set(area, "NAME", jazyky[i - 1].Name, WriteType.WriteStringANSI);
        }

        categoriF.Save();
    }

    /// <summary>
    /// Nainicializuje data o variantach a typoch reportov a jazykovych mutaciach hlaseni (pre konkretne GVD)
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="globalLanguages">jazyky stanice (globalny Categori.txt) - nazov a hlavny jazyk sa beru z nich</param>
    public static (List<ReportVariant>,List<ReportType>,List<FyzLanguage>) ReadLocal(string path, IEnumerable<FyzLanguage> globalLanguages,
        LoadWarnings warnings)
    {
        var file = CombinePath(path, FILE_CATEGORI)!;

        var variants = new List<ReportVariant>();
        var types = new List<ReportType>();
        var languages = new List<FyzLanguage>();

        var categoriF = new TxtPropsAreasFields(file);

        var countV = int.Parse(categoriF.Get("MAIN", "COUNT_BASIC_REPORT_VARIANT"), CultureInfo.InvariantCulture);
        for (var i = 1; i <= countV; i++)
        {
            var area = $"VARIANT_{i.PadZeros(2)}";
            variants.Add(new ReportVariant(int.Parse(categoriF.Get(area, "KEY"), CultureInfo.InvariantCulture), categoriF.Get(area, "NAME").ANSItoUTF()));
        }

        if (ReportVariant.FixSwappedDefaultNames(variants))
            warnings.Add(string.Format(CultureInfo.InvariantCulture, Resources.TxtParser_Categori_prehodene_nazvy_variantov, file));

        var countT = int.Parse(categoriF.Get("MAIN", "COUNT_TYPE_BASIC_REPORT"), CultureInfo.InvariantCulture);
        for (var i = 1; i <= countT; i++)
        {
            var area = $"TYPE_REPORT_{i.PadZeros(2)}";
            var key = categoriF.Get(area, "KEY").ANSItoUTF();
            var name = categoriF.Get(area, "NAME").ANSItoUTF();
            var @char = categoriF.Get(area, "CHAR").ANSItoUTF();
            var bt = int.Parse(categoriF.Get(area, "BASE_TRAIN"), CultureInfo.InvariantCulture).ToBool();
            var pt = int.Parse(categoriF.Get(area, "PASS_THROUGH"), CultureInfo.InvariantCulture).ToBool();
            var tt = int.Parse(categoriF.Get(area, "TERMINATE_TRAIN"), CultureInfo.InvariantCulture).ToBool();
            var comp = int.Parse(categoriF.Get(area, "COMPLEMENT"), CultureInfo.InvariantCulture).ToBool();
            var typ = new ReportType(key, name, @char, bt, pt, tt, comp)
            {
                // INISS ich nacita, hoci nepouzije - zachovavame ich
                LockoutBase = ParseIntOrDefault(categoriF.Get(area, "LOCKOUT_BASE", false)).ToBool(),
                LockoutThrough = ParseIntOrDefault(categoriF.Get(area, "LOCKOUT_THROUGH", false)).ToBool(),
                LockoutTerminate = ParseIntOrDefault(categoriF.Get(area, "LOCKOUT_TERMINATE", false)).ToBool()
            };
            types.Add(typ);
        }

        var countL = int.Parse(categoriF.Get("MAIN", "COUNT_LANGUAGES"), CultureInfo.InvariantCulture);
        for (var i = 1; i <= countL; i++)
        {
            var area = $"LANGUAGE_{i.PadZeros(2)}";
            var key = categoriF.Get(area, "KEY");

            // INISS berie z lokalneho suboru len to, ktore jazyky grafikon pouziva - nazov a hlavny jazyk ma
            // z globalneho Categori.txt. NAME a IS_BASIC sa preto necitaju: prepisali by zdielane globalne
            // jazyky a lokalny nazov by sa pri ulozeni globalnych nastaveni dostal aj do globalneho suboru.
            var lang = globalLanguages.FirstOrDefault(language => language.Key == key)
                       ?? throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Categori_BadLanguageKey, key, file));
            if (!languages.Contains(lang))
                languages.Add(lang);
        }

        return (variants, types, languages);
    }

    /// <summary>
    /// Zapise data o variantach a typoch reportov a jazykovych mutaciach hlaseni pouzivanych na stanici (pre konkretne GVD).
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="varianty">verianty reportov</param>
    /// <param name="types">typy reportov</param>
    /// <param name="languages">jazyky</param>
    public static void WriteLocal(string path, List<ReportVariant> varianty, List<ReportType> types, IList<FyzLanguage> languages)
    {
        var file = CombinePath(path, FILE_CATEGORI)!;

        var categoriF = new TxtPropsAreasFields(file, true);

        categoriF.Set("MAIN", "COUNT_BASIC_REPORT_VARIANT", varianty.Count);
        categoriF.Set("MAIN", "COUNT_TYPE_BASIC_REPORT", types.Count);
        categoriF.Set("MAIN", "COUNT_LANGUAGES", languages.Count);

        for (var i = 0; i < varianty.Count; i++)
        {
            var variant = varianty[i];
            var ti = i + 1;

            var area = $"VARIANT_{ti.PadZeros(2)}";
            categoriF.Set(area, "KEY", variant.Key);
            categoriF.Set(area, "NAME", variant.Name, WriteType.WriteStringANSI);
        }


        for (var i = 0; i < types.Count; i++)
        {
            var typ = types[i];
            var ti = i + 1;

            var area = $"TYPE_REPORT_{ti.PadZeros(2)}";
            categoriF.Set(area, "KEY", typ.Key, WriteType.WriteStringANSI);
            categoriF.Set(area, "NAME", typ.Name, WriteType.WriteStringANSI);
            categoriF.Set(area, "CHAR", typ.Char, WriteType.WriteStringANSI);
            categoriF.Set(area, "BASE_TRAIN", typ.BaseTrain.ToNumber());
            categoriF.Set(area, "PASS_THROUGH", typ.PassThrough.ToNumber());
            categoriF.Set(area, "TERMINATE_TRAIN", typ.TerminateTrain.ToNumber());
            categoriF.Set(area, "COMPLEMENT", typ.Complement.ToNumber());
            if (typ.LockoutBase || typ.LockoutThrough || typ.LockoutTerminate)
            {
                categoriF.Set(area, "LOCKOUT_BASE", typ.LockoutBase.ToNumber());
                categoriF.Set(area, "LOCKOUT_THROUGH", typ.LockoutThrough.ToNumber());
                categoriF.Set(area, "LOCKOUT_TERMINATE", typ.LockoutTerminate.ToNumber());
            }
        }


        for (var i = 0; i < languages.Count; i++)
        {
            var language = languages[i];
            var ti = i + 1;

            var area = $"LANGUAGE_{ti.PadZeros(2)}";
            categoriF.Set(area, "KEY", language.Key, WriteType.WriteStringANSI);
            categoriF.Set(area, "IS_BASIC", language.IsBasic.ToNumber());
            categoriF.Set(area, "NAME", language.Name, WriteType.WriteStringANSI);
        }

        categoriF.Save();
    }
}
