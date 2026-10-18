using System.Globalization;
using System.Reflection;
using System.Text;

namespace Iniss.PlanRadenia;

/// <summary>
/// Znak cisla vozna ako matica tmavosti (0 = papier, 9 = cierna). Cislo stoji na streche vozna a spodok cislic
/// splyva s ciarou strechy, preto znak konci riadkom nad strechou - pri vsetkych cislach rovnako.
/// </summary>
internal sealed class Glyph(int x, int width, int height, byte[] ink)
{
    /// <summary>Tmavost bodu, od ktorej bod patri znaku.</summary>
    private const int InkThreshold = 4;

    /// <summary>Poloha na strane (lavy okraj) - na zoradenie cislic.</summary>
    public int X { get; } = x;

    public int Width { get; } = width;

    public int Height { get; } = height;

    /// <summary>Tmavost 0-9 po riadkoch.</summary>
    public byte[] Ink { get; } = ink;

    public int this[int x, int y] => x >= 0 && y >= 0 && x < Width && y < Height ? Ink[y * Width + x] : 0;

    /// <summary>Riadky tmavosti 0-9 oddelene lomkou (zapis vzorov).</summary>
    public string Pattern => string.Join('/', Enumerable.Range(0, Height)
        .Select(y => new string(Enumerable.Range(0, Width).Select(x => (char)('0' + this[x, y])).ToArray())));

    public static Glyph FromPattern(string pattern)
    {
        var rows = pattern.Split('/');
        var width = rows.Max(r => r.Length);
        var ink = new byte[width * rows.Length];
        for (var y = 0; y < rows.Length; y++)
            for (var x = 0; x < rows[y].Length; x++)
                ink[y * width + x] = (byte)(rows[y][x] - '0');
        return new Glyph(0, width, rows.Length, ink);
    }

    /// <summary>
    /// Horny okraj strechy vozna: prvy riadok s dlhou ciernou ciarou v sirke vozna.
    /// </summary>
    /// <returns>-1, ak sa strecha nenasla.</returns>
    public static int RoofTop(Raster page, Box body)
    {
        for (var y = Math.Max(0, body.Y - 10); y < body.Y + 8 && y < page.Height; y++)
        {
            var run = 0;
            for (var x = body.X; x < body.Right; x++)
            {
                run = page[x, y].IsBlack ? run + 1 : 0;
                if (run > body.Width / 2)
                    return y;
            }
        }

        return -1;
    }

    /// <summary>
    /// Cislice cisla v oblasti nad strechou, zlava doprava. Cislo je suvisla skupina tmavych stlpcov najblizsie
    /// k stredu vozna; cislice pisma maju rovnaku sirku, takze pocet cislic urcuje sirka cisla a hranice su
    /// v najsvetlejsom stlpci pri ocakavanej hranici (slabe tahy vyhladenej cislice ju inak rozdelia na kusy).
    /// </summary>
    public static List<Glyph> Extract(Raster page, Box area)
    {
        area = area.Clip(page.Width, page.Height);
        int w = area.Width, h = area.Height;
        if (w <= 0 || h <= 0)
            return [];

        var ink = new byte[w * h];
        for (var y = 0; y < h; y++)
            for (var x = 0; x < w; x++)
                ink[y * w + x] = Darkness(page[area.X + x, area.Y + y]);

        // dlhe vodorovne ciary (podvozok riadku nad pokracovanim, ciara tabulky) nie su cislice - najsirsia cislica
        // ma okolo 9 bodov
        for (var y = 0; y < h; y++)
            for (var x = 0; x < w;)
            {
                if (ink[y * w + x] < InkThreshold)
                {
                    x++;
                    continue;
                }

                var end = x;
                while (end < w && ink[y * w + end] >= InkThreshold)
                    end++;
                if (end - x > 12)
                    Array.Clear(ink, y * w + x, end - x);
                x = end;
            }

        var column = new int[w];
        for (var x = 0; x < w; x++)
            for (var y = 0; y < h; y++)
                column[x] += ink[y * w + x] >= InkThreshold ? ink[y * w + x] : 0;

        // skupiny stlpcov s medzerou aspon 7 bodov (jednotka je uzka a od susednej cislice dalej); cislo je
        // skupina najblizsie k stredu
        var groups = new List<(int Left, int Right)>();
        for (var x = 0; x < w;)
        {
            if (column[x] == 0)
            {
                x++;
                continue;
            }

            var end = x;
            var gap = 0;
            for (var i = x; i < w && gap < 7; i++)
            {
                if (column[i] > 0)
                {
                    end = i + 1;
                    gap = 0;
                }
                else
                    gap++;
            }

            groups.Add((x, end));
            x = end + 1;
        }

        if (groups.Count == 0)
            return [];

        var (left, right) = groups.MinBy(g => Math.Abs((g.Left + g.Right) / 2 - w / 2));
        var width = right - left;
        var count = width switch
        {
            <= 12 => 1,
            <= 23 => 2,
            <= 34 => 3,
            _ => 4
        };

        // hranice cislic: najsvetlejsi stlpec do 3 bodov od ocakavanej hranice
        var cuts = new List<int> { left };
        for (var k = 1; k < count; k++)
        {
            var expected = left + width * k / count;
            var best = expected;
            for (var x = Math.Max(cuts[^1] + 3, expected - 3); x <= Math.Min(right - 3, expected + 3); x++)
                if (column[x] < column[best])
                    best = x;
            cuts.Add(best);
        }

        cuts.Add(right);
        var glyphs = new List<Glyph>();
        for (var k = 0; k < count; k++)
            if (Cell(ink, w, h, cuts[k], cuts[k + 1], area.X) is { } glyph)
                glyphs.Add(glyph);
        return glyphs;
    }

    /// <summary>Cislica v stlpcoch od-do orezana na tmave body; spodok je vzdy riadok nad strechou.</summary>
    private static Glyph? Cell(byte[] ink, int w, int h, int from, int to, int offset)
    {
        int left = to, right = from - 1, top = h;
        for (var y = 0; y < h; y++)
            for (var x = from; x < to; x++)
                if (ink[y * w + x] >= InkThreshold)
                {
                    left = Math.Min(left, x);
                    right = Math.Max(right, x);
                    top = Math.Min(top, y);
                }

        if (right < left || h - top < 5)
            return null;

        int gw = right - left + 1, gh = h - top;
        var bits = new byte[gw * gh];
        for (var y = 0; y < gh; y++)
            for (var x = 0; x < gw; x++)
                bits[y * gw + x] = ink[(top + y) * w + left + x];
        return new Glyph(offset + left, gw, gh, bits);
    }

    /// <summary>Tmavost bodu 0-9: cierne cislice na papieri (farebne casti vozna su svetlejsie nez cierna).</summary>
    private static byte Darkness(Rgb p) => (byte)Math.Clamp((255 - p.Max) * 10 / 256, 0, 9);
}

/// <summary>
/// Cita cisla vozni porovnanim so vzormi cislic. Cislice su v plane radenia vzdy tym istym pismom a velkostou,
/// preto je porovnanie so vzormi spolahlivejsie nez vseobecne OCR, ktore samostatne cislice necita.
/// </summary>
internal sealed class DigitReader
{
    private readonly List<(char Digit, Glyph Glyph)> _templates;

    private DigitReader(List<(char, Glyph)> templates) => _templates = templates;

    /// <summary>Vzory zo suboru Digits.txt vlozeneho do zostavy (riadok: cislica medzera vzor).</summary>
    public static DigitReader Load()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Iniss.PlanRadenia.Digits.txt");
        var templates = new List<(char, Glyph)>();
        if (stream is not null)
        {
            using var reader = new StreamReader(stream, Encoding.UTF8);
            while (reader.ReadLine() is { } line)
            {
                line = line.Trim();
                if (line.Length < 3 || line[0] == '#')
                    continue;
                templates.Add((line[0], Glyph.FromPattern(line[2..].Trim())));
            }
        }

        return new DigitReader(templates);
    }

    public int TemplateCount => _templates.Count;

    /// <summary>
    /// Precita cislo zo znakov zoradenych zlava.
    /// </summary>
    /// <returns>Cislo a najhorsia zhoda cislice so vzorom (0-1).</returns>
    public (string Number, double Score) Read(IReadOnlyList<Glyph> glyphs)
    {
        if (_templates.Count == 0 || glyphs.Count == 0)
            return ("", 0);

        var text = new StringBuilder();
        var worst = 1.0;
        foreach (var glyph in glyphs)
        {
            var (digit, score) = Best(glyph);
            text.Append(digit);
            worst = Math.Min(worst, score);
        }

        return (text.ToString(), Math.Round(worst, 3));
    }

    /// <summary>Najpodobnejsi vzor a zhoda.</summary>
    public (char Digit, double Score) Best(Glyph glyph)
    {
        var best = ('?', 0.0);
        foreach (var (digit, template) in _templates)
        {
            if (Math.Abs(template.Width - glyph.Width) > 2 || Math.Abs(template.Height - glyph.Height) > 2)
                continue;

            var score = Compare(glyph, template);
            if (score > best.Item2)
                best = (digit, score);
        }

        return best;
    }

    /// <summary>
    /// Zhoda dvoch znakov 0-1: 1 - priemerny rozdiel tmavosti v bodoch, kde je aspon jeden znak, pri najlepsom
    /// posunuti o -1..1 bod. Znaky su zarovnane spodnym okrajom (riadok nad strechou).
    /// </summary>
    public static double Compare(Glyph a, Glyph b)
    {
        int w = Math.Max(a.Width, b.Width) + 1, h = Math.Max(a.Height, b.Height);
        var best = 0.0;
        for (var dx = -1; dx <= 1; dx++)
            for (var dy = -1; dy <= 1; dy++)
            {
                int diff = 0, ink = 0;
                for (var y = 0; y < h; y++)
                    for (var x = -1; x < w; x++)
                    {
                        // zarovnanie spodnych riadkov
                        int pa = a[x, a.Height - h + y], pb = b[x + dx, b.Height - h + y + dy];
                        if (pa == 0 && pb == 0)
                            continue;
                        ink += Math.Max(pa, pb);
                        diff += Math.Abs(pa - pb);
                    }

                if (ink > 0)
                    best = Math.Max(best, 1 - (double)diff / ink);
            }

        return best;
    }

    public override string ToString() => _templates.Count.ToString(CultureInfo.InvariantCulture);
}
