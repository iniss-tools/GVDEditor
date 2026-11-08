using System.Globalization;
using System.Text.RegularExpressions;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Rules;

/// <summary>
/// Spolocne pravidla zoznamov tabul a textov: nazov a jednoznacny kluc, nazov kopie.
/// </summary>
internal static partial class TableRules
{
    /// <summary>
    /// Chyba nazvu; <see langword="null" />, ak je v poriadku.
    /// </summary>
    public static string? CheckName(string? name) =>
        string.IsNullOrWhiteSpace(name) ? Resources.TableRules_Nazov : null;

    /// <summary>
    /// Chyba kluca polozky na pozicii <paramref name="index" /> - prazdny alebo rovnaky ako pri inej polozke.
    /// Na velkosti pismen zalezi (INISS kluce porovnava presne).
    /// </summary>
    public static string? CheckKey(IReadOnlyList<string?> keys, int index)
    {
        var key = keys[index]?.Trim() ?? "";
        if (key.Length == 0)
            return Resources.TableRules_Kluc;

        for (var i = 0; i < keys.Count; i++)
            if (i != index && (keys[i]?.Trim() ?? "") == key)
                return string.Format(CultureInfo.CurrentCulture, Resources.TableRules_Kluc_Existuje, key);

        return null;
    }

    /// <summary>
    /// Text, ktory v zozname este nie je: <paramref name="name" />, potom „name 2“, „name 3“… Cislo na konci sa
    /// neopakuje - kopia „name 2“ dostane „name 3“, nie „name 2 2“.
    /// </summary>
    public static string Unique(IEnumerable<string?> used, string name)
    {
        var set = used.Select(u => u?.Trim() ?? "").ToHashSet();
        name = name.Trim();
        if (!set.Contains(name))
            return name;

        var numbered = NumberedName().Match(name);
        var baseName = numbered.Success ? numbered.Groups[1].Value : name;
        for (var i = 2; ; i++)
            if (!set.Contains($"{baseName} {i}"))
                return $"{baseName} {i}";
    }

    [GeneratedRegex(@"^(.*\S) \d+$")]
    private static partial Regex NumberedName();
}
