namespace Iniss.PlanRadenia;

/// <summary>
/// Piktogramy vozni s popismi z legendy ZSSK (napr. „klimatizovaný vozeň“). Piktogram vo voze sa porovnava
/// s katalogom; legenda na spodku kazdej strany sluzi na jeho doplnenie (--dump-icons).
/// </summary>
internal sealed class Legend
{
    /// <summary>Velkost mriezky, na ktoru sa piktogramy pred porovnanim zmensia (v legende su vacsie nez vo voznoch).</summary>
    private const int Grid = 12;

    /// <summary>Najvacsi priemerny rozdiel farby (0-255), pri ktorom sa piktogram este povazuje za zhodny.</summary>
    private const double MaxDifference = 32;

    /// <summary>Piktogramy legendy s popisom.</summary>
    public List<(string Label, float[] Icon)> Entries { get; } = [];

    /// <summary>
    /// Katalog piktogramov vlozeny do zostavy (Icons.txt): popis z legendy ZSSK a vzorka piktogramu. Zostaveny
    /// z legiend vsetkych stran - OCR drobneho textu legendy je na citanie popisov za behu prilis nespolahlive.
    /// </summary>
    public static Legend LoadCatalog()
    {
        var legend = new Legend();
        using var stream = typeof(Legend).Assembly.GetManifestResourceStream("Iniss.PlanRadenia.Icons.txt");
        if (stream is null)
            return legend;

        using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
        while (reader.ReadLine() is { } line)
        {
            if (line.Length == 0 || line[0] == '#' || line.Split('	') is not [var label, var values])
                continue;
            legend.Entries.Add((label, values.Split(',').Select(v => float.Parse(v, System.Globalization.CultureInfo.InvariantCulture)).ToArray()));
        }

        return legend;
    }

    /// <summary>
    /// Piktogramy vo voze: rovnako velke stvorce v rade v strede karoserie (rad je zarovnany na stred, prvy byva
    /// stvorec triedy). Velkost zavisi od ich poctu (21 bodov, pri mnohych az 13). Najprv sa v celej karoserii
    /// najde stvorec s najlepsou zhodou (velkost, poloha), od neho rad pokracuje doprava aj dolava s medzerou
    /// 0-3 body, kym su dalsie stvorce podobne piktogramom katalogu.
    /// </summary>
    public List<string> Match(Raster page, Box body) => Cells(page, body).Select(c => c.Label).ToList();

    /// <summary>Najdene stvorce piktogramov zlava doprava s popisom a zhodou.</summary>
    public List<(Box Box, string Label, double Difference)> Cells(Raster page, Box body)
    {
        var cells = new List<(Box Box, string Label, double Difference)>();
        if (Entries.Count == 0 || body.Width < 30)
            return cells;

        var image = new SummedImage(page, body.Inflate(4, 4).Clip(page.Width, page.Height));
        var center = body.Y + body.Height / 2;
        (Box Box, int Entry, double Difference) best = (default, -1, double.MaxValue);
        // hrube hladanie (velkost po 2, poloha po 3 bodoch), potom spresnenie okolo najlepsieho
        for (var size = 11; size <= 23; size += 2)
            for (var y = center - size / 2 - 2; y <= center - size / 2 + 1; y++)
                for (var x = body.X + 2; x + size <= body.Right - 2; x += 3)
                {
                    var candidate = BestAt(image, new Box(x, y, size, size));
                    if (candidate.Difference < best.Difference)
                        best = candidate;
                }

        if (best.Entry < 0)
            return cells;

        var start = best.Box;
        for (var size = start.Width - 1; size <= start.Width + 1; size++)
            for (var dy = -1; dy <= 1; dy++)
                for (var dx = -2; dx <= 2; dx++)
                {
                    var candidate = BestAt(image, new Box(start.X + dx, start.Y + dy, size, size));
                    if (candidate.Difference < best.Difference)
                        best = candidate;
                }

        if (best.Difference > MaxDifference)
            return cells;

        cells.Add((best.Box, Entries[best.Entry].Label, best.Difference));
        Extend(image, body, best.Box, 1, cells);
        Extend(image, body, best.Box, -1, cells);
        return cells.OrderBy(c => c.Box.X).ToList();
    }

    /// <summary>Pokracuje radom piktogramov od stvorca doprava (+1) alebo dolava (-1).</summary>
    private void Extend(SummedImage image, Box body, Box from, int direction, List<(Box, string, double)> cells)
    {
        var previous = from;
        while (true)
        {
            (Box Box, int Entry, double Difference) next = (default, -1, double.MaxValue);
            for (var size = previous.Width - 1; size <= previous.Width + 1; size++)
                for (var gap = 0; gap <= 3; gap++)
                    for (var dy = -1; dy <= 1; dy++)
                    {
                        var x = direction > 0 ? previous.Right + gap : previous.X - gap - size;
                        var box = new Box(x, previous.Y + dy, size, size);
                        if (box.X < body.X + 1 || box.Right > body.Right - 1)
                            continue;
                        var candidate = BestAt(image, box);
                        if (candidate.Difference < next.Difference)
                            next = candidate;
                    }

            if (next.Entry < 0 || next.Difference > MaxDifference)
                return;
            cells.Add((next.Box, Entries[next.Entry].Label, next.Difference));
            previous = next.Box;
        }
    }

    /// <summary>Najpodobnejsi piktogram katalogu v oblasti; prazdna karoseria nema ziadny.</summary>
    private (Box Box, int Entry, double Difference) BestAt(SummedImage image, Box box)
    {
        if (!image.Contains(box))
            return (box, -1, double.MaxValue);

        var sample = image.Sample(box, Grid);
        var best = (box, -1, double.MaxValue);
        for (var i = 0; i < Entries.Count; i++)
        {
            var difference = Difference(sample, Entries[i].Icon, best.Item3);
            if (difference < best.Item3)
                best = (box, i, difference);
        }

        return best;
    }

    private static double Luma(Rgb p) => 0.3 * p.R + 0.59 * p.G + 0.11 * p.B;

    /// <summary>Plne stvorce v legende (piktogramy); pismena textu su mensie.</summary>
    public static List<Box> IconBoxes(Raster page, Box area)
    {
        int w = area.Width, h = area.Height;
        var seen = new bool[w * h];
        var boxes = new List<Box>();
        var stack = new Stack<int>();
        for (var start = 0; start < w * h; start++)
        {
            if (seen[start] || page[area.X + start % w, area.Y + start / w].IsWhite)
                continue;

            int left = w, right = 0, top = h, bottom = 0;
            stack.Push(start);
            seen[start] = true;
            while (stack.Count > 0)
            {
                var p = stack.Pop();
                int px = p % w, py = p / w;
                left = Math.Min(left, px);
                right = Math.Max(right, px);
                top = Math.Min(top, py);
                bottom = Math.Max(bottom, py);
                foreach (var n in new[] { p - 1, p + 1, p - w, p + w })
                {
                    if (n < 0 || n >= w * h || seen[n] || (n % w == 0 && p % w == w - 1) || (p % w == 0 && n % w == w - 1))
                        continue;
                    if (page[area.X + n % w, area.Y + n / w].IsWhite)
                        continue;
                    seen[n] = true;
                    stack.Push(n);
                }
            }

            int bw = right - left + 1, bh = bottom - top + 1;
            if (bw is >= 16 and <= 34 && bh is >= 16 and <= 34 && Math.Abs(bw - bh) <= 8)
                boxes.Add(new Box(area.X + left, area.Y + top, bw, bh));
        }

        return boxes.OrderBy(b => b.Y / 10).ThenBy(b => b.X).ToList();
    }

    /// <summary>Priemerne farby oblasti v mriezke <see cref="Grid" /> x <see cref="Grid" /> (R, G, B za sebou).</summary>
    public static float[] Sample(Raster page, Box box)
    {
        var result = new float[Grid * Grid * 3];
        for (var gy = 0; gy < Grid; gy++)
            for (var gx = 0; gx < Grid; gx++)
            {
                int x0 = box.X + gx * box.Width / Grid, x1 = Math.Max(x0 + 1, box.X + (gx + 1) * box.Width / Grid);
                int y0 = box.Y + gy * box.Height / Grid, y1 = Math.Max(y0 + 1, box.Y + (gy + 1) * box.Height / Grid);
                float r = 0, g = 0, b = 0;
                var n = 0;
                for (var y = y0; y < y1; y++)
                    for (var x = x0; x < x1; x++, n++)
                    {
                        var p = page[x, y];
                        r += p.R;
                        g += p.G;
                        b += p.B;
                    }

                var i = (gy * Grid + gx) * 3;
                result[i] = r / n;
                result[i + 1] = g / n;
                result[i + 2] = b / n;
            }

        return result;
    }

    /// <summary>Priemerny rozdiel zloziek; ak uz presiahne <paramref name="limit" />, dalej sa nepocita.</summary>
    private static double Difference(float[] a, float[] b, double limit = double.MaxValue)
    {
        double sum = 0;
        var stop = limit * a.Length;
        for (var i = 0; i < a.Length; i++)
        {
            sum += Math.Abs(a[i] - b[i]);
            if (sum > stop)
                return double.MaxValue;
        }

        return sum / a.Length;
    }
}

/// <summary>
/// Integralny obraz oblasti strany (sucty zloziek farieb) - priemer farieb lubovolneho obdlznika styrmi pristupmi.
/// </summary>
internal sealed class SummedImage
{
    private readonly Box _area;
    private readonly long[] _sums;

    public SummedImage(Raster page, Box area)
    {
        _area = area;
        int w = area.Width + 1, h = area.Height + 1;
        _sums = new long[w * h * 3];
        for (var y = 1; y < h; y++)
            for (var x = 1; x < w; x++)
            {
                var p = page[area.X + x - 1, area.Y + y - 1];
                for (var c = 0; c < 3; c++)
                {
                    long value = c == 0 ? p.R : c == 1 ? p.G : p.B;
                    _sums[(y * w + x) * 3 + c] = value + _sums[((y - 1) * w + x) * 3 + c] + _sums[(y * w + x - 1) * 3 + c]
                                                 - _sums[((y - 1) * w + x - 1) * 3 + c];
                }
            }
    }

    public bool Contains(Box box) => box.X >= _area.X && box.Y >= _area.Y && box.Right <= _area.Right && box.Bottom <= _area.Bottom;

    /// <summary>Priemerne farby obdlznika v mriezke grid x grid (R, G, B za sebou).</summary>
    public float[] Sample(Box box, int grid)
    {
        var result = new float[grid * grid * 3];
        var w = _area.Width + 1;
        for (var gy = 0; gy < grid; gy++)
            for (var gx = 0; gx < grid; gx++)
            {
                int x0 = box.X - _area.X + gx * box.Width / grid, x1 = Math.Max(x0 + 1, box.X - _area.X + (gx + 1) * box.Width / grid);
                int y0 = box.Y - _area.Y + gy * box.Height / grid, y1 = Math.Max(y0 + 1, box.Y - _area.Y + (gy + 1) * box.Height / grid);
                var n = (float)((x1 - x0) * (y1 - y0));
                for (var c = 0; c < 3; c++)
                {
                    var sum = _sums[(y1 * w + x1) * 3 + c] - _sums[(y0 * w + x1) * 3 + c] - _sums[(y1 * w + x0) * 3 + c] + _sums[(y0 * w + x0) * 3 + c];
                    result[(gy * grid + gx) * 3 + c] = sum / n;
                }
            }

        return result;
    }
}
