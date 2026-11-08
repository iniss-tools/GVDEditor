using System.Numerics;

namespace Iniss.PlanRadenia;

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
