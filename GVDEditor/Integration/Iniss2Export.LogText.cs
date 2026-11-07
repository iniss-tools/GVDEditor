using System.Text.RegularExpressions;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Integration;

/// <summary>
/// Funkcia dodatku, ktoru export doplni do skriptu hlaseni cielovej instalacie.
/// </summary>
/// <param name="Name">nazov funkcie - kluc dodatku <c>D&lt;kod&gt;</c></param>
/// <param name="Description">popis pre obsluhu - text nahravky z banky INISS 3.39</param>
/// <param name="Sound">nahravka v tvare <c>skupina/zvuk</c>; jazyk doplni server podla hlaseneho jazyka</param>
internal sealed record LogTextAddition(string Name, string Description, string Sound);

internal static partial class Iniss2Export
{
    /// <summary>
    /// Doplni do LogText.vb funkcie dodatkov, ktore v nom nie su. Server preklada jediny subor, preto idu dovnutra triedy
    /// <c>LogText</c> pred jej koniec; funkcia s rovnakym nazvom sa nemeni.
    /// </summary>
    /// <param name="file">LogText.vb cielovej instalacie</param>
    /// <param name="additions">funkcie dodatkov</param>
    /// <returns>nazvy doplnenych funkcii</returns>
    public static List<string> AddAdditionFunctions(string file, IEnumerable<LogTextAddition> additions)
    {
        var text = File.ReadAllText(file, Encodings.Win1250);
        var existing = LogTextFunctions(text.Split('\n'));
        var missing = additions.Where(addition => !existing.ContainsKey(addition.Name)).DistinctBy(addition => addition.Name).ToList();
        if (missing.Count == 0)
            return [];

        File.WriteAllText(file, InsertIntoLogText(text, missing), Encodings.Win1250);
        return [.. missing.Select(addition => addition.Name)];
    }

    /// <summary>
    /// Text skriptu s funkciami vlozenymi pred koniec triedy <c>LogText</c>; konce riadkov podla suboru.
    /// </summary>
    /// <exception cref="FormatException">skript nema triedu LogText</exception>
    internal static string InsertIntoLogText(string text, IReadOnlyList<LogTextAddition> additions)
    {
        var newLine = text.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
        var lines = text.Split(newLine).ToList();
        var end = LogTextClassEnd(lines) ?? throw new FormatException("LogText.vb nema triedu LogText.");

        var block = new List<string> { "", "    ' Dodatky doplnene exportom z GVDEditora (INISS 3.39)" };
        foreach (var addition in additions)
        {
            block.Add($"    <Dodatek(\"{addition.Description.Replace("\"", "\"\"", StringComparison.Ordinal)}\")> _");
            block.Add($"    Public Sub {addition.Name}()");
            block.Add($"        Zvuk(\"{addition.Sound}\")");
            block.Add($"    End Sub ' {addition.Name}");
            block.Add("");
        }

        lines.InsertRange(end, block);
        return string.Join(newLine, lines);
    }

    // riadok End Class, ktory uzatvara triedu LogText; vnorene triedy sa preskocia, komentare sa ignoruju
    private static int? LogTextClassEnd(List<string> lines)
    {
        var start = lines.FindIndex(line => LogTextClassPattern().IsMatch(line));
        if (start < 0)
            return null;

        var depth = 1;
        for (var i = start + 1; i < lines.Count; i++)
        {
            var line = lines[i].TrimStart();
            if (line.StartsWith('\''))
                continue;

            if (EndClassPattern().IsMatch(line) && --depth == 0)
                return i;
            if (ClassPattern().IsMatch(line))
                depth++;
        }

        return null;
    }

    [GeneratedRegex(@"^\s*(?:(?:Public|Partial|Friend|NotInheritable)\s+)*Class\s+LogText\b", RegexOptions.IgnoreCase)]
    private static partial Regex LogTextClassPattern();

    [GeneratedRegex(@"^(?:(?:Public|Private|Friend|Protected|Partial|Shared|NotInheritable|MustInherit)\s+)*Class\s+\w", RegexOptions.IgnoreCase)]
    private static partial Regex ClassPattern();

    [GeneratedRegex(@"^End\s+Class\b", RegexOptions.IgnoreCase)]
    private static partial Regex EndClassPattern();
}
