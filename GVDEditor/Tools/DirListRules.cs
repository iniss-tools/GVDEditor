using System.Globalization;
using GVDEditor.Properties;

namespace GVDEditor.Tools;

/// <summary>
///     Udaje grafikonu v zozname grafikonov (DirList.TXT), ktore sa upravuju v okne Globalne nastavenia.
/// </summary>
internal static class DirListRules
{
    /// <summary>
    ///     Najvacsie cislo komunikacnej linky (portu tabul a hlaseni), ktore GVDEditor pusti.
    /// </summary>
    public const int MaxPort = 255;

    /// <summary>
    ///     Prevedie text portu na cislo. Prazdny text a 0 znamenaju, ze grafikon vlastnu linku nema.
    /// </summary>
    /// <param name="text">text z bunky</param>
    /// <param name="port">cislo linky, alebo <see langword="null" />, ak ziadna nie je</param>
    /// <returns>Text chyby, alebo <see langword="null" />, ak je hodnota v poriadku.</returns>
    public static string? ParsePort(string? text, out int? port)
    {
        port = null;
        var value = (text ?? "").Trim();
        if (value.Length == 0)
            return null;

        if (!int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var number) || number > MaxPort)
            return string.Format(CultureInfo.CurrentCulture, Resources.DirListRules_Port, MaxPort);

        port = number == 0 ? null : number;
        return null;
    }
}
