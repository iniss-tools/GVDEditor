using System.Numerics;

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

/// <summary>
/// Znak precitaneho riadku: poloha bunky v stlpcoch riadku.
/// </summary>
/// <param name="SpaceBefore">Pred znakom je v texte medzera (znak zacina novy token).</param>
internal readonly record struct NoteCell(char Char, int X, int Width, bool SpaceBefore);

/// <summary>
/// Precitany riadok poznamky.
/// </summary>
/// <param name="Text">Text s medzerami podla gramatiky (pred kazdym tokenom).</param>
/// <param name="Cells">Znaky (bez medzier).</param>
/// <param name="Cost">Priemerna odchylka obrazka od vzorov na znak - mera neistoty.</param>
internal sealed record NoteReading(string Text, List<NoteCell> Cells, double Cost);

/// <summary>
/// Citanie riadku poznamky vzormi pisma: Viterbi cez stlpce riadku a stavy gramatiky. Kazdy stlpec je bud prazdny,
/// alebo patri bunke znaku; cena znaku je sucet stvorcov rozdielov obrazka a vzoru.
/// </summary>
internal sealed class NoteDecoder
{
    /// <summary>Cena prazdneho stlpca navyse k jeho tmavosti.</summary>
    private const float Blank = 0.02f;

    /// <summary>Riadok bez odrazky (pokracovanie poznamky) - cena za vynechanu odrazku.</summary>
    private const float NoBullet = 1f;

    /// <summary>Prirazka za znak, ktory ma len nahradne vzory (pri porovnani s nazvami stanic).</summary>
    private const float SubstituteCost = 0.15f;

    private readonly NoteFont _font;
    private readonly NoteGrammar _grammar;

    public NoteDecoder(NoteFont font)
    {
        _font = font;
        var learned = font.Glyphs.Where(g => g.Learned).ToList();
        _grammar = new NoteGrammar(learned.Select(g => g.Char));
        Learned = learned;
    }

    private List<NoteGlyph> Learned { get; }

    /// <summary>Ceny vzoru na vsetkych zaciatkoch riadku (sucet stvorcov rozdielov; vektorovo cez zaciatky).</summary>
    private static float[] Costs(NoteLine line, NoteGlyph glyph)
    {
        var w = glyph.Width;
        var count = line.Width - w + 1;
        if (count <= 0)
            return [];
        var costs = new float[count];
        var ink = line.Ink.AsSpan();
        var size = Vector<float>.Count;
        for (var r = 0; r < NoteFont.Rows; r++)
            for (var k = 0; k < w; k++)
            {
                var t = glyph.Ink[r * w + k];
                var tv = new Vector<float>(t);
                var source = ink.Slice(r * line.Width + k, count);
                var x = 0;
                for (; x <= count - size; x += size)
                {
                    var d = new Vector<float>(source[x..]) - tv;
                    (new Vector<float>(costs.AsSpan(x)) + d * d).CopyTo(costs.AsSpan(x));
                }

                for (; x < count; x++)
                {
                    var d = source[x] - t;
                    costs[x] += d * d;
                }
            }

        return costs;
    }

    private sealed record Arc(char Char, int Width, int From, int To, float Cost, float[] Emission, bool StartsToken);

    public NoteReading Read(NoteLine line)
    {
        var g = _grammar;
        var states = g.States.Count;
        var width = line.Width;

        // najlepsi vzor pre kazdy znak a sirku
        var usable = Learned.Where(glyph => g.Edges.ContainsKey(glyph.Char)).ToList();
        var all = new float[usable.Count][];
        Parallel.For(0, usable.Count, i => all[i] = Costs(line, usable[i]));
        var emissions = new Dictionary<(char, int), float[]>();
        for (var i = 0; i < usable.Count; i++)
        {
            var costs = all[i];
            if (costs.Length == 0)
                continue;
            var key = (usable[i].Char, usable[i].Width);
            if (emissions.TryGetValue(key, out var current))
                for (var j = 0; j < costs.Length; j++)
                    current[j] = Math.Min(current[j], costs[j]);
            else
                emissions[key] = costs;
        }

        var arcs = new List<Arc>();
        foreach (var ((c, w), emission) in emissions)
            foreach (var (from, to, cost) in g.Edges[c])
                arcs.Add(new Arc(c, w, from, to, cost, emission, g.StartsToken(from, to, c)));

        var colsq = new float[width];
        for (var x = 0; x < width; x++)
            for (var r = 0; r < NoteFont.Rows; r++)
                colsq[x] += line[x, r] * line[x, r];

        const float inf = float.MaxValue / 4;
        var dp = new float[(width + 1) * states];
        // -1 = prazdny stlpec, <= -2 = medzera zo stavu (-2 - stav), inak index prechodu
        var back = new int[(width + 1) * states];
        Array.Fill(dp, inf);
        dp[g.Start] = 0;
        dp[g.Token] = NoBullet;

        for (var x = 1; x <= width; x++)
        {
            var row = x * states;
            var prev = (x - 1) * states;
            for (var s = 0; s < states; s++)
            {
                dp[row + s] = dp[prev + s] + colsq[x - 1] + Blank;
                back[row + s] = -1;
            }

            for (var a = 0; a < arcs.Count; a++)
            {
                var arc = arcs[a];
                if (arc.Width > x)
                    continue;
                var from = dp[(x - arc.Width) * states + arc.From];
                if (from >= inf)
                    continue;
                var value = from + arc.Cost + arc.Emission[x - arc.Width];
                if (value < dp[row + arc.To])
                {
                    dp[row + arc.To] = value;
                    back[row + arc.To] = a;
                }
            }

            if (x >= 2)
            {
                var source = -1;
                var best = inf;
                foreach (var s in g.SpaceSources)
                    if (dp[(x - 2) * states + s] < best)
                        (best, source) = (dp[(x - 2) * states + s], s);
                var gap = colsq[x - 2] + colsq[x - 1] + 2 * Blank;
                var value = best + gap;
                if (source >= 0 && value < dp[row + g.Space])
                {
                    dp[row + g.Space] = value;
                    back[row + g.Space] = -2 - source;
                }

                var afterDot = dp[(x - 2) * states + g.Dot] + gap;
                if (afterDot < dp[row + g.SpaceAfterDot])
                {
                    dp[row + g.SpaceAfterDot] = afterDot;
                    back[row + g.SpaceAfterDot] = -2 - g.Dot;
                }

                var afterCar = dp[(x - 2) * states + g.CarDot] + gap;
                if (afterCar < dp[row + g.CarDot])
                {
                    dp[row + g.CarDot] = afterCar;
                    back[row + g.CarDot] = -2 - g.CarDot;
                }
            }
        }

        var state = g.Finals.MinBy(s => dp[width * states + s]);
        var total = dp[width * states + state];
        var cells = new List<NoteCell>();
        var position = width;
        while (position > 0)
        {
            var b = back[position * states + state];
            if (b == -1)
                position--;
            else if (b <= -2)
            {
                state = -2 - b;
                position -= 2;
            }
            else
            {
                var arc = arcs[b];
                cells.Add(new NoteCell(arc.Char, position - arc.Width, arc.Width, arc.StartsToken));
                state = arc.From;
                position -= arc.Width;
            }
        }

        cells.Reverse();
        return new NoteReading(TextOf(cells), cells, cells.Count == 0 ? 0 : total / cells.Count);
    }

    /// <summary>Text z buniek s medzerami pred tokenmi.</summary>
    public static string TextOf(IEnumerable<NoteCell> cells)
    {
        var text = new System.Text.StringBuilder();
        foreach (var cell in cells)
        {
            if (cell.SpaceBefore && text.Length > 0)
                text.Append(' ');
            text.Append(cell.Char);
        }

        return text.ToString();
    }

    /// <summary>
    /// Zhoda casti riadku s textom (vynutene zarovnanie): znaky textu idu za sebou, medzi nimi najviac jeden prazdny
    /// stlpec, na mieste medzery v texte lubovolne vela. Znak bez naucenych vzorov pouzije nahradne vzory pisma,
    /// pismeno bez vzorov aj nahradnych vzorov pismeno bez diakritiky.
    /// </summary>
    /// <returns>Priemerna cena na znak, alebo <see cref="double.MaxValue" />, ak text na riadok nesedi.</returns>
    public double Match(NoteLine line, string text, Dictionary<char, List<(int Width, float[] Costs)>> cache)
    {
        var chars = new List<(char Char, bool Spaced)>();
        var spaced = false;
        foreach (var ch in text)
        {
            if (ch == ' ')
            {
                spaced = true;
                continue;
            }

            if (Resolve(ch) is { } c)
                chars.Add((c, spaced));
            spaced = false;
        }

        var width = line.Width;
        if (chars.Count == 0 || width == 0)
            return double.MaxValue;

        var blank = new float[width + 1];
        for (var x = 0; x < width; x++)
        {
            float sq = 0;
            for (var r = 0; r < NoteFont.Rows; r++)
                sq += line[x, r] * line[x, r];
            blank[x + 1] = blank[x] + sq;
        }

        const float inf = float.MaxValue / 4;
        // cur[x] = cena, ak predosle znaky koncia pred stlpcom x
        var cur = new float[width + 1];
        for (var x = 0; x <= width; x++)
            cur[x] = x <= 30 ? blank[x] : inf;
        var next = new float[width + 1];
        var gapped = new float[width + 1];
        foreach (var (c, isSpaced) in chars)
        {
            var maxGap = isSpaced ? 40 : 1;
            for (var x = 0; x <= width; x++)
            {
                var best = inf;
                for (var gap = 0; gap <= maxGap && gap <= x; gap++)
                {
                    var v = cur[x - gap] + blank[x] - blank[x - gap];
                    if (v < best)
                        best = v;
                }

                gapped[x] = best;
            }

            Array.Fill(next, inf);
            foreach (var (w, costs) in Templates(line, c, cache))
                for (var x = w; x <= width; x++)
                {
                    var from = gapped[x - w];
                    if (from >= inf)
                        continue;
                    var v = from + costs[x - w];
                    if (v < next[x])
                        next[x] = v;
                }

            (cur, next) = (next, cur);
        }

        var total = inf;
        for (var x = 0; x <= width; x++)
            total = Math.Min(total, cur[x] + blank[width] - blank[x]);
        if (total >= inf)
            return double.MaxValue;
        // znak len s nahradnymi vzormi pisma sedi horsie - pri zhode ma prednost text so znamymi znakmi
        var substitutes = chars.Count(c => !_font.ByChar[c.Char].Any(g => g.Learned));
        return (total + SubstituteCost * substitutes) / chars.Count;
    }

    /// <summary>Znak, ktory ma vzory: sam, alebo pismeno bez diakritiky.</summary>
    private char? Resolve(char c)
    {
        if (_font.ByChar.ContainsKey(c))
            return c;
        var plain = Names.WithoutDiacritics(c.ToString());
        return plain.Length == 1 && _font.ByChar.ContainsKey(plain[0]) ? plain[0] : null;
    }

    /// <summary>Ceny vzorov znaku na vsetkych zaciatkoch (naucene, inak nahradne); vysledok sa pamata pre dalsie texty.</summary>
    private List<(int Width, float[] Costs)> Templates(NoteLine line, char c, Dictionary<char, List<(int Width, float[] Costs)>> cache)
    {
        if (cache.TryGetValue(c, out var list))
            return list;
        var glyphs = _font.ByChar[c];
        var use = glyphs.Any(g => g.Learned) ? glyphs.Where(g => g.Learned) : glyphs;
        list = use.Select(g => (g.Width, Costs(line, g))).Where(t => t.Item2.Length > 0).ToList();
        cache[c] = list;
        return list;
    }
}
