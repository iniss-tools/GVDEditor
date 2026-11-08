using System.Globalization;
using System.Text;

namespace Iniss.PlanRadenia;

/// <summary>
/// Porovnanie nazvov stanic bez diakritiky, interpunkcie a medzier (OCR a nazvy v zdrojoch sa lisia).
/// </summary>
internal static class Names
{
    public static bool Same(string a, string b) => Canonical(a) == Canonical(b);

    public static string Canonical(string name)
    {
        var decomposed = name.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();
        foreach (var c in decomposed)
            if (char.IsLetterOrDigit(c) && CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                builder.Append(c);
        return builder.ToString();
    }

    /// <summary>Text bez diakritiky (ł a ß nemaju rozklad - nahradzaju sa zvlast).</summary>
    public static string WithoutDiacritics(string text)
    {
        var builder = new StringBuilder();
        foreach (var c in text.Normalize(NormalizationForm.FormD))
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                builder.Append(c switch { 'ł' => 'l', 'Ł' => 'L', 'ß' => 's', _ => c });
        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}
