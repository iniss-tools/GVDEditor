using System.Globalization;
using System.Reflection;
using System.Text;

namespace Iniss.PlanRadenia;

/// <summary>
/// Vzor znaku pisma poznamok: tmavost bodov bunky znaku (<see cref="NoteFont.Rows" /> riadkov x <see cref="Width" />
/// stlpcov, 0-1) po riadkoch. Bunka siaha od zaciatku znaku po zaciatok dalsieho.
/// </summary>
/// <param name="Learned">
/// Vzor je nauceny zo skutocnych poznamok; nahradny (vykresleny pismom) sluzi len na porovnanie s nazvami stanic.
/// </param>
internal sealed record NoteGlyph(char Char, int Width, bool Learned, float[] Ink);

/// <summary>
/// Pismo poznamok pod radenim. Text poznamok ma v plane okolo 9 bodov na vysku a rovnaky znak vyzera inak podla
/// toho, o aky zlomok bodu je posunuty - preto ma znak viac vzorov (podla sirky bunky a polohy znaku v nej).
/// </summary>
internal sealed class NoteFont
{
    /// <summary>Riadky obrazka riadku poznamky (nad uplatnou ciarou <see cref="Top" />, zvysok pod nou).</summary>
    public const int Rows = 14;

    /// <summary>Riadky nad uplatnou ciarou vratane nej.</summary>
    public const int Top = 10;

    private NoteFont(float[] profile, List<NoteGlyph> glyphs)
    {
        Profile = profile;
        Glyphs = glyphs;
        ByChar = glyphs.GroupBy(g => g.Char).ToDictionary(g => g.Key, g => g.ToList());
    }

    /// <summary>Priemerny zvisly profil riadku (podiel tmavosti v riadkoch) - podla neho sa riadok zarovna zvisle.</summary>
    public float[] Profile { get; }

    public IReadOnlyList<NoteGlyph> Glyphs { get; }

    public IReadOnlyDictionary<char, List<NoteGlyph>> ByChar { get; }

    public static NoteFont Load()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Iniss.PlanRadenia.NoteGlyphs.txt")
                           ?? throw new InvalidOperationException("NoteGlyphs");
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var profile = Array.Empty<float>();
        var glyphs = new List<NoteGlyph>();
        while (reader.ReadLine() is { } line)
        {
            if (line.Length == 0 || line[0] == '#')
                continue;
            var parts = line.Split('\t');
            if (parts[0] == "profil")
            {
                profile = parts[1].Split(' ').Select(v => float.Parse(v, CultureInfo.InvariantCulture)).ToArray();
                continue;
            }

            var width = int.Parse(parts[1], CultureInfo.InvariantCulture);
            var ink = parts[3].Split(' ').Select(v => int.Parse(v, CultureInfo.InvariantCulture) / 255f).ToArray();
            glyphs.Add(new NoteGlyph(parts[0][0], width, parts[2] == "r", ink));
        }

        return new NoteFont(profile, glyphs);
    }
}

/// <summary>
/// Riadok poznamky pripraveny na citanie: tmavost bodov (0-1) v <see cref="NoteFont.Rows" /> riadkoch od uplatnej
/// ciary, zvisle posunuty o zlomok bodu tak, aby sedel na profil pisma.
/// </summary>
internal sealed class NoteLine
{
    /// <summary>Okraj vlavo a vpravo od textu.</summary>
    private const int Pad = 4;

    private NoteLine(float[] ink, int width, int x, Box band)
    {
        Ink = ink;
        Width = width;
        X = x;
        Band = band;
    }

    /// <summary>Tmavost po riadkoch (Rows x Width).</summary>
    public float[] Ink { get; }

    public int Width { get; }

    /// <summary>Poloha prveho stlpca na strane.</summary>
    public int X { get; }

    /// <summary>Pas riadku na strane.</summary>
    public Box Band { get; }

    public float this[int x, int y] => Ink[y * Width + x];

    /// <summary>Tmavost bodu strany 0-255 (jas podla vzorca ITU-R 601, ako pri uceni vzorov).</summary>
    public static int Darkness(Rgb p) => 255 - ((p.R * 19595 + p.G * 38470 + p.B * 7471 + 0x8000) >> 16);

    /// <summary>
    /// Pripravi riadok z pasu strany: uplatna ciara je spodok riadkov s vela tmavymi bodmi (cislice a male pismena),
    /// zvisly posun o zlomok bodu sa hlada porovnanim profilu riadku s profilom pisma.
    /// </summary>
    /// <returns><see langword="null" />, ak pas nema text.</returns>
    public static NoteLine? Create(Raster page, Box band, NoteFont font)
    {
        // vodorovny rozsah textu
        int left = int.MaxValue, right = -1;
        for (var x = band.X; x < band.Right; x++)
            for (var y = band.Y; y < band.Bottom; y++)
                if (Darkness(page[x, y]) > 20)
                {
                    left = Math.Min(left, x);
                    right = Math.Max(right, x);
                    break;
                }

        if (right < 0)
            return null;

        int x0 = Math.Max(0, left - Pad), x1 = Math.Min(page.Width, right + 1 + Pad);
        var counts = new int[band.Height];
        for (var y = band.Y; y < band.Bottom; y++)
            for (var x = x0; x < x1; x++)
                if (Darkness(page[x, y]) > 90)
                    counts[y - band.Y]++;
        var max = counts.Max();
        if (max == 0)
            return null;
        var baseline = band.Y + Array.FindLastIndex(counts, c => c >= 0.35 * max);

        // zvisly posun -1,5 az 1,5 bodu po osminach
        var best = (Difference: double.MaxValue, Shift: 0.0);
        for (var k = -12; k <= 12; k++)
        {
            var shift = k / 8.0;
            var profile = new double[NoteFont.Rows];
            for (var r = 0; r < NoteFont.Rows; r++)
                for (var x = left; x <= right; x++)
                    profile[r] += Sample(page, x, baseline - NoteFont.Top + 1 + shift + r);
            var sum = profile.Sum();
            if (sum <= 0)
                continue;
            var difference = 0.0;
            for (var r = 0; r < NoteFont.Rows; r++)
                difference += Math.Pow(profile[r] / sum - font.Profile[r], 2);
            if (difference < best.Difference)
                best = (difference, shift);
        }

        var width = x1 - x0;
        var ink = new float[NoteFont.Rows * width];
        for (var r = 0; r < NoteFont.Rows; r++)
            for (var x = 0; x < width; x++)
                ink[r * width + x] = (float)(Sample(page, x0 + x, baseline - NoteFont.Top + 1 + best.Shift + r) / 255);
        return new NoteLine(ink, width, x0, band);
    }

    /// <summary>Tmavost v neceloselnom riadku (linearna interpolacia medzi riadkami).</summary>
    private static double Sample(Raster page, int x, double y)
    {
        var y0 = (int)Math.Floor(y);
        var t = y - y0;
        double a = y0 >= 0 && y0 < page.Height ? Darkness(page[x, y0]) : 0;
        double b = y0 + 1 >= 0 && y0 + 1 < page.Height ? Darkness(page[x, y0 + 1]) : 0;
        return a * (1 - t) + b * t;
    }

    /// <summary>Riadok vodorovne zmeneny v pomere <paramref name="factor" /> (linearna interpolacia stlpcov).</summary>
    public NoteLine Stretch(double factor) => Resample(factor, 1);

    /// <summary>
    /// Riadok zmeneny vodorovne v pomere <paramref name="sx" /> a zvisle v pomere <paramref name="sy" /> okolo
    /// uplatnej ciary (bilinearna interpolacia) - vacsie pismo sa zmensi na velkost vzorov.
    /// </summary>
    public NoteLine Resample(double sx, double sy)
    {
        var width = Math.Max(1, (int)Math.Round(Width * sx));
        var ink = new float[NoteFont.Rows * width];
        const int baseline = NoteFont.Top - 1;
        for (var r = 0; r < NoteFont.Rows; r++)
        {
            var sourceRow = baseline + (r - baseline) / sy;
            var r0 = (int)Math.Floor(sourceRow);
            var ty = (float)(sourceRow - r0);
            for (var x = 0; x < width; x++)
            {
                var source = (x + 0.5) / sx - 0.5;
                var x0 = (int)Math.Floor(source);
                var tx = (float)(source - x0);
                int a = Math.Clamp(x0, 0, Width - 1), b = Math.Clamp(x0 + 1, 0, Width - 1);
                float Row(int row) => row < 0 || row >= NoteFont.Rows ? 0 : Ink[row * Width + a] * (1 - tx) + Ink[row * Width + b] * tx;
                ink[r * width + x] = Row(r0) * (1 - ty) + Row(r0 + 1) * ty;
            }
        }

        return new NoteLine(ink, width, X, Band);
    }

    /// <summary>Cast riadku (stlpce od - do) - na porovnanie s nazvom stanice.</summary>
    public NoteLine Slice(int from, int to)
    {
        from = Math.Max(0, from);
        to = Math.Min(Width, to);
        var width = Math.Max(0, to - from);
        var ink = new float[NoteFont.Rows * width];
        for (var r = 0; r < NoteFont.Rows; r++)
            Array.Copy(Ink, r * Width + from, ink, r * width, width);
        return new NoteLine(ink, width, X + from, Band);
    }
}
