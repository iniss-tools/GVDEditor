namespace GVDEditor.Tools;

/// <summary>
///     Argumenty prikazoveho riadka INISSu (Nastavenia programu → Spustanie INISS). INISS pozna parametre s prefixom
///     <c>/</c> alebo <c>-</c> a na velkosti pismen mu nezalezi.
/// </summary>
internal static class INISSArgs
{
    /// <summary>
    ///     Ci argumenty obsahuju parameter <paramref name="name" /> (bez prefixu, napr. <c>Minimize</c>).
    /// </summary>
    public static bool Has(string args, string name) =>
        Tokens(args).Any(t => t.Length > 1 && t[0] is '/' or '-' && string.Equals(t[1..], name, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    ///     Hodnota parametra <c>/Reg:</c>, alebo <see langword="null" />, ak v argumentoch nie je.
    /// </summary>
    public static string? Registry(string args)
    {
        foreach (var t in Tokens(args))
            if (t.Length > 5 && t[0] is '/' or '-' && t[1..5].Equals("reg:", StringComparison.OrdinalIgnoreCase))
                return t[5..];

        return null;
    }

    /// <summary>
    ///     Rozdeli argumenty podla medzier; text v uvodzovkach tvori jeden argument (uvodzovky sa odstrania).
    /// </summary>
    public static IEnumerable<string> Tokens(string? args)
    {
        if (string.IsNullOrWhiteSpace(args))
            yield break;

        var token = new StringBuilder();
        var quoted = false;
        foreach (var c in args)
        {
            if (c == '"')
            {
                quoted = !quoted;
                continue;
            }

            if (char.IsWhiteSpace(c) && !quoted)
            {
                if (token.Length > 0) yield return token.ToString();
                token.Clear();
                continue;
            }

            token.Append(c);
        }

        if (token.Length > 0) yield return token.ToString();
    }
}
