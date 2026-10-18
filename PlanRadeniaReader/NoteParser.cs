using System.Text.RegularExpressions;

namespace Iniss.PlanRadenia;

/// <summary>
/// Rozbor precitanej poznamky pod radenim:
/// <list type="bullet">
///   <item><c>• č. 21, č. 22 - ide do 15.IX.</c> (obmedzenie), <c>• č. 359, por. č. 1 - Humenné</c> (ciel vozni),</item>
///   <item><c>• Vozne č. 1, 2, 3 budú zaradené do 31.VIII. vo vlaku od ŽST Banská Bystrica.</c> (pridanie v stanici).</item>
/// </list>
/// </summary>
internal static partial class NoteParser
{
    /// <summary>Odkaz na vozen: cislo (č. 21) alebo poradie vozna bez cisla (por. č. 1).</summary>
    [GeneratedRegex(@"(?<por>por\.\s*)?č\.\s*(?<n>\d{1,4})")]
    private static partial Regex CarRef();

    /// <summary>
    /// Vozne pridane v stanici. Slova vety su v plane svetlejsie a citaju sa nespolahlivo - pevne su len cisla vozni,
    /// slovo s „raden“ (zaradené), „vo … od ŽST“ a nazov stanice.
    /// </summary>
    [GeneratedRegex(@"^Voz\S*\s*č\.\s*(?<cars>[\d,\s]*)\s*.*?\S*raden\S*\s*(?<rest>.*?)\s*vo\s*\S*\s*od\s*ŽS\S*(?:\s+[T7]\b)?\s*(?<station>.+?)\.?$")]
    private static partial Regex Joined();

    [GeneratedRegex(@"\d{1,4}")]
    private static partial Regex Number();

    public static PlanNote Parse(string text)
    {
        var note = new PlanNote { Text = text.Trim() };
        var body = note.Text.TrimStart('•', ' ');

        if (Joined().Match(body) is { Success: true } joined)
        {
            note.Kind = PlanNoteKind.Joined;
            foreach (Match number in Number().Matches(joined.Groups["cars"].Value))
                note.Cars.Add(number.Value);
            note.Rest = joined.Groups["rest"].Value;
            note.Station = joined.Groups["station"].Value.Trim();
            return note;
        }

        var dash = DashIndex(body);
        var refs = dash >= 0 ? body[..dash] : body;
        foreach (Match match in CarRef().Matches(refs))
            note.Cars.Add(match.Groups["por"].Success ? "#" + match.Groups["n"].Value : match.Groups["n"].Value);
        if (dash < 0)
            return note;

        var rest = body[(dash + 1)..].Trim();
        if (rest.Length == 0)
            return note;
        if (rest.StartsWith("ide", StringComparison.Ordinal) || rest.StartsWith("nejde", StringComparison.Ordinal) ||
            rest.StartsWith("okrem", StringComparison.Ordinal))
        {
            note.Kind = PlanNoteKind.Restriction;
            note.Rest = rest;
        }
        else
        {
            note.Kind = PlanNoteKind.Destination;
            note.Station = rest.TrimEnd('.').Trim();
        }

        return note;
    }

    /// <summary>Pomlcka medzi zoznamom vozni a textom - prva v poznamke (pomlcky v nazvoch stanic su az za nou).</summary>
    private static int DashIndex(string text) => text.IndexOf('-', StringComparison.Ordinal);
}
