using System.Text.RegularExpressions;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Integration;

/// <summary>
/// Nazvy stanic v plane radenia ZSSK a stanice grafikonu. Plan pise nazvy inak nez zvukova banka - cele slova
/// namiesto skratiek (Bratislava hlavná stanica / Bratislava hl.st.) a zahranicne stanice bez diakritiky
/// (Krakow Glowny / Kraków Główny).
/// </summary>
internal static partial class PlanStations
{
    /// <summary>Skratky v nazvoch stanic a ich plne tvary.</summary>
    private static readonly (string Short, string Full)[] Abbreviations =
    [
        ("hl.st.", "hlavná stanica"), ("hl. st.", "hlavná stanica"), ("os.st.", "osobná stanica"), ("zast.", "zastávka"),
        ("hl.n.", "hlavní nádraží"), ("Gł.", "Główny"), ("Gl.", "Glowny")
    ];

    [GeneratedRegex(@"[\s.\-,]+")]
    private static partial Regex Separators();

    /// <summary>
    /// Nazvy stanic pre citanie poznamok pod radenim (nazov, plne tvary skratiek a nazov bez diakritiky) a stanica,
    /// ku ktorej patria.
    /// </summary>
    public static Dictionary<string, Station> Candidates(IEnumerable<Station> stations)
    {
        var result = new Dictionary<string, Station>(StringComparer.Ordinal);
        foreach (var station in stations)
        {
            if (string.IsNullOrWhiteSpace(station.Name) || station.ID.Length == 0)
                continue;
            var names = new List<string> { station.Name.Trim() };
            foreach (var (abbreviation, full) in Abbreviations)
                if (names[0].Contains(abbreviation, StringComparison.Ordinal))
                    names.Add(names[0].Replace(abbreviation, " " + full, StringComparison.Ordinal).Replace("  ", " ", StringComparison.Ordinal).Trim());
            foreach (var name in names.ToList())
                names.Add(Plain(name));
            foreach (var name in names)
                result.TryAdd(name, station);
        }

        return result;
    }

    /// <summary>
    /// Ci ide o tu istu stanicu: rovnaky pocet slov, prve rovnake a kazde dalsie rovnake alebo skratkou druheho
    /// (bez ohladu na diakritiku a velkost pismen).
    /// </summary>
    public static bool Same(string a, string b)
    {
        var x = Words(a);
        var y = Words(b);
        if (x.Length == 0 || x.Length != y.Length || x[0] != y[0])
            return false;
        for (var i = 1; i < x.Length; i++)
            if (!x[i].StartsWith(y[i], StringComparison.Ordinal) && !y[i].StartsWith(x[i], StringComparison.Ordinal))
                return false;
        return true;
    }

    /// <summary>Stanica grafikonu k nazvu z planu radenia, alebo <see langword="null" />.</summary>
    public static Station? Resolve(string name, Dictionary<string, Station> candidates)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;
        if (candidates.TryGetValue(name.Trim(), out var station))
            return station;
        return candidates.FirstOrDefault(c => Same(c.Key, name)).Value;
    }

    private static string[] Words(string name) =>
        Separators().Split(Plain(name.ToLowerInvariant())).Where(w => w.Length > 0).ToArray();

    /// <summary>Text bez diakritiky; ł a Ł nemaju rozklad na pismeno a znamienko - nahradzaju sa zvlast.</summary>
    private static string Plain(string text) => StringUtils.RemoveDiacritics(text.Replace('ł', 'l').Replace('Ł', 'L'));
}
