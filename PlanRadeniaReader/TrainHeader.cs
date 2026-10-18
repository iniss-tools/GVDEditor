using System.Globalization;
using System.Text.RegularExpressions;

namespace Iniss.PlanRadenia;

/// <summary>
/// Udaje vlaku z laveho stlpca a hlavicky strany: druh a cislo, nazov, casy a vychodiskova a cielova stanica.
/// </summary>
internal static partial class TrainHeader
{
    /// <summary>Prvy riadok: druh a cislo vlaku, pripadne aj nazov (R 830 URPÍN).</summary>
    [GeneratedRegex(@"^\s*(?<type>[A-Za-z]{1,4})\s*(?<number>\d{2,5})\b\s*(?<rest>.*)$")]
    private static partial Regex TypeNumber();

    /// <summary>
    /// Casy: 6:58 - 6:59, OCR casto vynecha dvojbodku (6 58-6 59, 653-655) alebo ju precita ako bodku.
    /// </summary>
    [GeneratedRegex(@"(?<h1>\d{1,2})\s*[:.,]?\s*(?<m1>\d{2})(?:\s*[-–]\s*(?<h2>\d{1,2})\s*[:.,]?\s*(?<m2>\d{2}))?")]
    private static partial Regex Times();

    [GeneratedRegex(@"(?<d>\d{1,2})\s*\.\s*(?<m>\d{1,2})\s*\.\s*(?<y>\d{4})")]
    private static partial Regex Date();

    /// <summary>
    /// Vyplni vlak z riadkov OCR laveho stlpca.
    /// </summary>
    /// <returns><see langword="false" />, ak prvy riadok nema druh a cislo vlaku.</returns>
    public static bool Fill(PlanTrain train, IReadOnlyList<TextLine> lines)
    {
        var sorted = lines.OrderBy(line => line.Box.Y).ThenBy(line => line.Box.X).ToList();
        var first = sorted.FindIndex(line => TypeNumber().IsMatch(line.Text));
        if (first < 0)
            return false;

        var match = TypeNumber().Match(sorted[first].Text);
        train.Type = match.Groups["type"].Value;
        train.Number = match.Groups["number"].Value;

        var name = new List<string>();
        if (match.Groups["rest"].Value.Trim() is { Length: > 0 } rest)
            name.Add(rest);

        var stations = new List<string>();
        var timesFound = false;
        foreach (var line in sorted.Skip(first + 1))
        {
            var text = line.Text.Trim();
            if (!timesFound && Times().Match(text) is { Success: true } times && times.Value.Length * 2 >= text.Replace(" ", "").Length)
            {
                timesFound = true;
                train.Times = FormatTimes(times);
                continue;
            }

            if (timesFound)
                stations.Add(text);
            else
                name.Add(text);
        }

        train.Name = string.Join(" ", name).Trim();
        SplitStations(train, stations);
        return true;
    }

    private static string FormatTimes(Match match)
    {
        var text = $"{int.Parse(match.Groups["h1"].Value, CultureInfo.InvariantCulture)}:{match.Groups["m1"].Value}";
        if (match.Groups["h2"].Success)
            text += $" - {int.Parse(match.Groups["h2"].Value, CultureInfo.InvariantCulture)}:{match.Groups["m2"].Value}";
        return text;
    }

    /// <summary>
    /// Vychodiskova a cielova stanica - cielova zacina pomlckou, obe sa mozu zalomit na viac riadkov.
    /// </summary>
    private static void SplitStations(PlanTrain train, List<string> lines)
    {
        var to = lines.FindIndex(line => line.StartsWith('-') || line.StartsWith('–') || line.StartsWith('•'));
        if (to < 0)
        {
            train.From = string.Join(" ", lines);
            return;
        }

        train.From = string.Join(" ", lines.Take(to));
        train.To = string.Join(" ", lines.Skip(to)).TrimStart('-', '–', '•', ' ');
    }

    /// <summary>
    /// Platnost planu z hlavicky strany (Plati od: 1. 9. 2026 do: 12. 12. 2026).
    /// </summary>
    public static (string From, string To) Validity(IEnumerable<TextLine> lines)
    {
        var dates = lines.SelectMany(line => Date().Matches(line.Text))
            .Select(m => $"{m.Groups["y"].Value}-{int.Parse(m.Groups["m"].Value, CultureInfo.InvariantCulture):00}-{int.Parse(m.Groups["d"].Value, CultureInfo.InvariantCulture):00}")
            .ToList();
        return (dates.ElementAtOrDefault(0) ?? "", dates.ElementAtOrDefault(1) ?? "");
    }
}
