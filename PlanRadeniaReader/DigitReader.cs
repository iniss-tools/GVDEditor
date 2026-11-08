using System.Globalization;
using System.Reflection;
using System.Text;

namespace Iniss.PlanRadenia;

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
