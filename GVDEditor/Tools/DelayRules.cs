using System.Globalization;

namespace GVDEditor.Tools;

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
