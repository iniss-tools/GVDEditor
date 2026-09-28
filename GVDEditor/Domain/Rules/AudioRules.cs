using System.Globalization;
using System.Text.RegularExpressions;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Rules;

/// <summary>
///     Kontrola audio liniek (zvukovych okruhov) tak, ako ich INISS cita pri starte.
/// </summary>
internal static partial class AudioRules
{
    /// <summary>
    ///     Pole audio linky, ku ktoremu sa chyba viaze.
    /// </summary>
    public enum Field
    {
        Station,
        Name,
        ShortName,
        Queue,
        Amplifier,
        Exchange,
        Node
    }

    /// <summary>
    ///     Kluc testovacieho okruhu namiesto cisla stanice.
    /// </summary>
    public const string TestKey = "TEST";

    /// <summary>
    ///     Ci je linka testovacim okruhom (na velkosti pismen nezalezi).
    /// </summary>
    public static bool IsTest(Station station) => string.Equals(station.ID, TestKey, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    ///     Prva chyba linky na pozicii <paramref name="index" />.
    /// </summary>
    /// <returns>Pole a text chyby, alebo <see langword="null" />, ak je linka v poriadku.</returns>
    public static (Field Field, string Message)? Check(IReadOnlyList<Audio> audios, int index)
    {
        var audio = audios[index];
        var name = audio.Name.Trim();
        if (name.Length == 0)
            return (Field.Name, Resources.AudioRules_Nazov);

        for (var i = 0; i < index; i++)
            if (audios[i].Name.Trim() == name)
                return (Field.Name, Resources.FGlobalSettings_Názov_tejto_audio_linky_už_existuje);

        if (IsTest(audio.Station) && audios.Take(index).Any(a => IsTest(a.Station)))
            return (Field.Station, Resources.FGlobalSettings_Testovaci_okruh_uz_je);

        if (audio.ShortName.Trim().Length == 0)
            return (Field.ShortName, Resources.AudioRules_Protokol);

        var queue = audio.QueueName.Trim();
        if (queue.Length == 0)
            return (Field.Queue, Resources.AudioRules_Fronta);

        if (CheckAmplifier(audio.AmplifierPort) is { } amplifier)
            return (Field.Amplifier, amplifier);

        if (CheckExchange(audio.ExchangeParameter) is { } exchange)
            return (Field.Exchange, exchange);

        if (CheckNode(audio.Node) is { } node)
            return (Field.Node, node);

        // linky s rovnakou frontou zdielaju jeden vystup - na inom uzle ho INISS druhykrat nepripravi
        for (var i = 0; i < index; i++)
            if (audios[i].QueueName.Trim() == queue && NodeKey(audios[i].Node) != NodeKey(audio.Node))
                return (Field.Node, string.Format(CultureInfo.CurrentCulture, Resources.AudioRules_Fronta_Uzol, queue, audios[i].Name));

        return null;
    }

    /// <summary>
    ///     Spinanie zosilnovaca: cislo vystupu ustredne 0-63, <c>E1</c>-<c>E99</c> / <c>EE1</c>-<c>EE99</c> port
    ///     ELSVO, <c>Z…</c> (INISS ho ulozi, ale nepouzije), alebo nic.
    /// </summary>
    public static string? CheckAmplifier(string? value)
    {
        var text = (value ?? "").Trim();
        if (text.Length == 0 || text[0] == 'Z')
            return null;

        var elsvo = text.StartsWith("EE", StringComparison.Ordinal) ? 2 : text.StartsWith('E') ? 1 : 0;
        if (elsvo > 0)
            return TryNumber(text[elsvo..], out var port) && port is >= 1 and <= 99 ? null : Resources.AudioRules_Zosilnovac;

        return TryNumber(text, out var output) && output is >= 0 and <= 63 ? null : Resources.AudioRules_Zosilnovac;
    }

    /// <summary>
    ///     Parameter ustredne: cele cislo 0-65535 (posiela sa v dolnom slove spravy ovladacu), alebo nic.
    /// </summary>
    public static string? CheckExchange(string? value)
    {
        var text = (value ?? "").Trim();
        if (text.Length == 0)
            return null;

        return TryNumber(text, out var number) && number <= ushort.MaxValue ? null : Resources.AudioRules_Ustredna;
    }

    /// <summary>
    ///     Uzol: cislo pocitaca, seriovy port <c>COMn</c> (aj <c>\\.\COMn</c>, <c>//./COMn</c>), alebo nic.
    /// </summary>
    public static string? CheckNode(string? value)
    {
        var text = (value ?? "").Trim();
        if (text.Length == 0 || TryNumber(text, out _) || ComPort().IsMatch(text))
            return null;

        return Resources.AudioRules_Uzol;
    }

    // prazdny uzol a 0 su ten isty (tento) pocitac
    private static string NodeKey(string? node)
    {
        var text = (node ?? "").Trim();
        return text.Length == 0 || text == "0" ? "" : text.ToUpperInvariant();
    }

    private static bool TryNumber(string text, out int number) =>
        int.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out number);

    [GeneratedRegex(@"^(\\\\\.\\|//\./)?COM\d+$", RegexOptions.IgnoreCase)]
    private static partial Regex ComPort();

    /// <summary>
    ///     Nazov novej linky, ktory este v zozname nie je (<see cref="TableRules.Unique" />).
    /// </summary>
    public static string UniqueName(IEnumerable<Audio> audios, string name) =>
        TableRules.Unique(audios.Select(a => a.Name), name);
}
