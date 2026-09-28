using System.Globalization;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Rules;

/// <summary>
///     Casy meskania ponukane operatorovi (Zpozdeni.TXT). INISS kazdy riadok prevedie na cele cislo - nieco ine
///     (napr. "VICE480" v starsich instalaciach) zaloguje ako chybu a do ponuky nezaradi.
/// </summary>
internal static class DelayRules
{
    /// <summary>
    ///     Ci INISS hodnotu prijme - rovnako ako jeho prevod: medzery, volitelne znamienko, cislice, medzery.
    /// </summary>
    public static bool IsAcceptedByIniss(string value) => TryGetMinutes(value, out _);

    private static bool TryGetMinutes(string value, out int minutes) =>
        int.TryParse(value.Trim(' ', '\t'), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out minutes);

    /// <summary>
    ///     Chyba casu na pozicii <paramref name="index" /> - prazdna hodnota alebo cas, ktory je v zozname dvakrat.
    ///     Necislena hodnota chybou nie je (INISS ju len preskoci), na tu upozornuje <see cref="IsAcceptedByIniss" />.
    /// </summary>
    /// <returns>Text chyby, alebo <see langword="null" />, ak je cas v poriadku.</returns>
    public static string? CheckValue(IReadOnlyList<string> delays, int index)
    {
        var value = delays[index].Trim();
        if (value.Length == 0)
            return Resources.DelayRules_Prazdne;

        for (var i = 0; i < delays.Count; i++)
            if (i != index && delays[i].Trim() == value)
                return string.Format(CultureInfo.CurrentCulture, Resources.DelayRules_Existuje, value);

        return null;
    }

    /// <summary>
    ///     Pozicia, na ktoru patri <paramref name="value" />: cislo pred prvy vacsi cas, necislena hodnota na koniec.
    /// </summary>
    public static int InsertIndex(IReadOnlyList<string> delays, string value)
    {
        if (!TryGetMinutes(value, out var minutes))
            return delays.Count;

        for (var i = 0; i < delays.Count; i++)
            if (!TryGetMinutes(delays[i], out var other) || other > minutes)
                return i;

        return delays.Count;
    }
}
