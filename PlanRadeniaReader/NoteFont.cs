using System.Globalization;
using System.Reflection;
using System.Text;

namespace Iniss.PlanRadenia;

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
