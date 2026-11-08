namespace Iniss.PlanRadenia;

/// <summary>
/// Gramatika riadku poznamky ako konecny automat nad znakmi:
/// <c>• č. 364, č. 365 - ide v ⑤, ⑦, 2., 6., 30.IV., nejde od 20.XII. do 3.I.</c>, <c>• č. 344 - Krakow Glowny</c>,
/// <c>• Vozne č. 1, 2, 3 budú zaradené do 31.VIII. vo vlaku od ŽST Banská Bystrica.</c>
/// Datum je cislo, bodka, rimsky mesiac a bodka; klucove slova su zo slovnika, ine slova (nazvy stanic) su volne
/// a zacinaju az za medzerou. Ciarka je len za cislom, datumom a cislom v kruzku.
/// </summary>
/// <remarks>
/// Stavy: START (pred odrazkou), T (zaciatok tokenu), S (za medzerou), TC (za ciarkou), PC (za „č.“), C1-C4 (cislo
/// vozna), F (volne slovo), N1-N4 (cislo), DD (bodka za dnom), RD (bodka za mesiacom), P (bodka za slovom), C (cislo
/// v kruzku), W:predpona (klucove slovo), R:predpona (rimsky mesiac).
/// </remarks>
internal sealed class NoteGrammar
{
    /// <summary>Cena pismena volneho slova - klucove slova a cisla maju prednost.</summary>
    private const float FreeLetter = 0.35f;

    private static readonly string[] Keywords =
        ["č", "por", "ide", "nejde", "od", "do", "v", "vo", "okrem", "vlaku", "ŽST", "Vozne", "Vozeň", "budú", "bude", "zaradené", "zaradený"];

    /// <summary>Skratky - vzdy s bodkou.</summary>
    private static readonly HashSet<string> Dotted = ["č", "por"];

    private static readonly string[] Roman = ["I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII"];

    private const string Digits = "0123456789";

    /// <summary>Dni v tyzdni ①-⑦ a kladivka (pracovne dni) - kladivka je vo vzoroch ako %.</summary>
    public const string Symbols = "\u2460\u2461\u2462\u2463\u2464\u2465\u2466%";

    private readonly Dictionary<string, int> _index = [];

    public NoteGrammar(IEnumerable<char> letters)
    {
        var letterSet = letters.Where(char.IsLetter).Distinct().OrderBy(c => c).ToArray();
        States = ["START", "T", "S", "SP", "TC", "PC", "C1", "C2", "C3", "C4", "F", "FD", "N1", "N2", "N3", "N4", "DD", "RD", "P", "C"];
        States.AddRange(Keywords.SelectMany(k => Enumerable.Range(1, k.Length).Select(i => "W:" + k[..i])).Distinct().Order(StringComparer.Ordinal));
        States.AddRange(Roman.SelectMany(r => Enumerable.Range(1, r.Length).Select(i => "R:" + r[..i])).Distinct().Order(StringComparer.Ordinal));
        for (var i = 0; i < States.Count; i++)
            _index[States[i]] = i;

        foreach (var state in States)
            foreach (var (c, target, cost) in Next(state, letterSet))
            {
                if (!Edges.TryGetValue(c, out var list))
                    Edges[c] = list = [];
                list.Add((_index[state], _index[target], cost));
            }

        Start = _index["START"];
        Token = _index["T"];
        Space = _index["S"];
        SpaceAfterDot = _index["SP"];
        Dot = _index["P"];
        CarDot = _index["PC"];
        Finals = States.Where(s => IsTokenStart(s) || s is "F" or "DD" or "C" || (s.Length == 2 && s[0] is 'N' or 'C')).Select(s => _index[s]).ToArray();
        SpaceSources = States.Where(s => (IsTokenStart(s) && s is not ("P" or "PC")) || s is "F" or "DD" or "RD" || (s.Length == 2 && s[0] is 'N' or 'C'))
            .Select(s => _index[s]).ToArray();
    }

    public List<string> States { get; }

    /// <summary>Prechody podla znaku: (zo stavu, do stavu, cena).</summary>
    public Dictionary<char, List<(int From, int To, float Cost)>> Edges { get; } = [];

    public int Start { get; }

    /// <summary>Zaciatok tokenu - riadok bez odrazky (pokracovanie poznamky) zacina tu.</summary>
    public int Token { get; }

    public int Space { get; }

    /// <summary>Medzera za skratkou s bodkou (okrem „č.“) - za nou nemoze byt cislo.</summary>
    public int SpaceAfterDot { get; }

    /// <summary>Bodka za slovom (skratka).</summary>
    public int Dot { get; }

    /// <summary>Bodka za „č.“ - medzera za nou stav nemeni (nasleduje cislo vozna).</summary>
    public int CarDot { get; }

    public int[] Finals { get; }

    /// <summary>Stavy, za ktorymi moze byt medzera (nie vnutri slova).</summary>
    public int[] SpaceSources { get; }

    /// <summary>
    /// Ci znak prechodu zacina novy token - v texte je pred nim medzera (pred cislom, slovom, symbolom a pomlckou
    /// medzi tokenmi; nie pred bodkou, ciarkou, rimskym mesiacom a vnutri slova).
    /// </summary>
    public bool StartsToken(int from, int to, char c)
    {
        string f = States[from], t = States[to];
        if (f == "START" || c is '.' or ',')
            return false;
        return t switch
        {
            "F" => f is "S" or "SP",
            "N1" or "C1" or "C" => true,
            "T" => c == '-',
            _ => t.StartsWith("W:", StringComparison.Ordinal) && t.Length == 3,
        };
    }

    /// <summary>Stav, z ktoreho moze zacat dalsi token (aj bez medzery).</summary>
    private static bool IsTokenStart(string s) =>
        s is "T" or "S" or "SP" or "TC" or "P" or "C" or "RD" or "PC" || (s.StartsWith("W:", StringComparison.Ordinal) && Keywords.Contains(s[2..]) && !Dotted.Contains(s[2..]));

    private static IEnumerable<(char, string, float)> Next(string s, char[] letters)
    {
        if (s == "START")
        {
            yield return ('•', "T", 0);
            yield break;
        }

        if (IsTokenStart(s) || s is "F" or "DD")
        {
            foreach (var c in letters)
            {
                if (Keywords.Any(k => k[0] == c))
                    yield return (c, "W:" + c, 0);
                // volne slovo (nazov stanice) len za medzerou
                if (s is "S" or "SP")
                    yield return (c, "F", FreeLetter);
            }

            if (s == "PC")
                foreach (var c in Digits)
                    yield return (c, "C1", 0);
            // za datumom a cislom v kruzku nasleduje dalsie cislo az za ciarkou, za skratkou s bodkou (okrem č.) ziadne
            else if (s is not ("DD" or "RD" or "C" or "P" or "SP"))
                foreach (var c in Digits)
                    yield return (c, "N1", 0);

            foreach (var c in Symbols)
                yield return (c, "C", 0);
            if (s is not ("TC" or "DD"))
                yield return ('-', "T", 0);
            if (s is "DD" or "RD" or "C")
                yield return (',', "TC", 0);
        }

        if (s.StartsWith("W:", StringComparison.Ordinal))
        {
            var prefix = s[2..];
            foreach (var c in letters)
                if (Keywords.Any(k => k.StartsWith(prefix + c, StringComparison.Ordinal)))
                    yield return (c, "W:" + prefix + c, 0);
            if (Keywords.Contains(prefix))
                yield return ('.', prefix == "č" ? "PC" : "P", 0);
        }

        if (s == "F")
        {
            foreach (var c in letters)
                yield return (c, "F", FreeLetter);
            yield return ('.', "P", 0);
            yield return ('-', "FD", 0);
        }

        if (s == "FD")
            foreach (var c in letters)
                yield return (c, "F", FreeLetter);

        if (s is "C1" or "C2" or "C3")
            foreach (var c in Digits)
                yield return (c, "C" + (s[1] - '0' + 1), 0);
        if (s is "C1" or "C2" or "C3" or "C4")
        {
            yield return (',', "TC", 0);
            yield return ('-', "T", 0);
        }

        if (s is "N1" or "N2" or "N3")
            foreach (var c in Digits)
                yield return (c, "N" + (s[1] - '0' + 1), 0);
        if (s is "N1" or "N2" or "N3" or "N4")
        {
            yield return (',', "TC", 0);
            yield return ('-', "T", 0);
            if (s is "N1" or "N2")
                yield return ('.', "DD", 0);
        }

        if (s == "DD")
            foreach (var r in "IVX")
                yield return (r, "R:" + r, 0);

        if (s.StartsWith("R:", StringComparison.Ordinal))
        {
            var prefix = s[2..];
            foreach (var r in "IVX")
                if (Roman.Any(x => x.StartsWith(prefix + r, StringComparison.Ordinal)))
                    yield return (r, "R:" + prefix + r, 0);
            if (Roman.Contains(prefix))
                yield return ('.', "RD", 0);
        }
    }
}
