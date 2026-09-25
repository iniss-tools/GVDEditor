using ToolsCore.Expressions;
using ToolsCore.TabTab;

namespace GVDEditor.Tools;

/// <summary>
///     Formatovanie textu sekcie TabTab (tlacidlo Formatovat v editore TabTab).
///     Meni len to, co INISS pri citani aj tak ignoruje: velkost pismen funkcii a konstant a medzery okolo
///     <c>&amp;&amp;</c> / <c>||</c> v podmienkach pravidiel <c>#SWITCH</c>, <c>#MERGE</c>, <c>#MERGE2</c> a medzery
///     okolo ich <c>=</c>. Texty pre tabulu (retazce, jednoduche pravidla, komentare) ostavaju nedotknute.
/// </summary>
internal static class TabTabFormatter
{
    private static readonly Dictionary<string, string> ConstantNames =
        ExprConstants.Common.ToDictionary(c => c.Name, c => c.Name, StringComparer.OrdinalIgnoreCase);

    private readonly record struct Edit(int Start, int Length, string Text);

    /// <summary>
    ///     Naformatuje text sekcie.
    /// </summary>
    /// <param name="text">Text sekcie.</param>
    /// <returns>Naformatovany text (rovnaky, ak nie je co menit).</returns>
    public static string Format(string text)
    {
        var edits = new List<Edit>();

        foreach (var line in TabTabSectionParser.Parse(text).Lines)
        {
            if (line.Kind != TabTabLineKind.Rule
                || line.Event is not (TabTabEventKind.Switch or TabTabEventKind.Merge or TabTabEventKind.Merge2)
                || line.Items.Count == 0)
                continue;

            foreach (var item in line.Items.Where(i => i.IsCondition))
                FormatCondition(text, item, edits);

            // medzery okolo '=' medzi poslednou polozkou a udalostou - polozky aj udalost sa orezavaju
            var last = line.Items[^1];
            var gapStart = last.Span.End;
            var gapEnd = line.RightSpan.Start;
            if (last.Span.Length > 0 && gapStart < gapEnd && gapEnd <= text.Length)
            {
                var gap = text[gapStart..gapEnd];
                if (gap.Trim(' ', '\t') == "=" && gap != " = ")
                    edits.Add(new Edit(gapStart, gap.Length, " = "));
            }
        }

        if (edits.Count == 0)
            return text;

        var sb = new StringBuilder(text);
        // na rovnakej pozicii najprv nahradenie slova, potom vlozenie medzery pred neho
        foreach (var edit in edits.OrderByDescending(e => e.Start).ThenByDescending(e => e.Length))
            sb.Remove(edit.Start, edit.Length).Insert(edit.Start, edit.Text);
        return sb.ToString();
    }

    /// <summary>
    ///     Upravi jednu podmienku: mena funkcii a konstant na tvar z tabulky INISSu, jedna medzera okolo spojok.
    ///     Obsah retazcov a literalov <c>#…#</c> sa preskakuje.
    /// </summary>
    private static void FormatCondition(string text, TabTabItem item, List<Edit> edits)
    {
        var span = item.Span;
        if (span.Length == 0 || span.End > text.Length)
            return;

        // polozka rozdelena na viac riadkov (pokracovanie '\') alebo s escapovanym znakom - radsej nechat tak
        var s = text.Substring(span.Start, span.Length);
        if (s != item.Text || s.Contains('\\'))
            return;

        var i = 0;
        while (i < s.Length)
        {
            var c = s[i];
            if (c is '"' or '#')
            {
                var close = s.IndexOf(c, i + 1);
                if (close < 0)
                    return; // neukonceny retazec - zvysok nechat, ako je
                i = close + 1;
            }
            else if (char.IsLetter(c) || c == '_')
            {
                var start = i;
                while (i < s.Length && (char.IsLetterOrDigit(s[i]) || s[i] == '_')) i++;
                var word = s[start..i];
                var canonical = ExprFunctions.Find(word)?.Name ?? ConstantNames.GetValueOrDefault(word);
                if (canonical is not null && canonical != word)
                    edits.Add(new Edit(span.Start + start, word.Length, canonical));
            }
            else if (c is '&' or '|' && i + 1 < s.Length && s[i + 1] == c)
            {
                // medzery pred spojkou a za nou zjednotit na jednu
                var before = i;
                while (before > 0 && s[before - 1] is ' ' or '\t') before--;
                var after = i + 2;
                while (after < s.Length && s[after] is ' ' or '\t') after++;

                if (before > 0 && s[before..i] != " ")
                    edits.Add(new Edit(span.Start + before, i - before, " "));
                if (after < s.Length && s[(i + 2)..after] != " ")
                    edits.Add(new Edit(span.Start + i + 2, after - i - 2, " "));
                i += 2;
            }
            else
            {
                i++;
            }
        }
    }
}
