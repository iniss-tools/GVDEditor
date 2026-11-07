using System.Globalization;
using System.Text;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.TabTab;

namespace GVDEditor.Integration;

/// <summary>
/// Predloha, ktora tabulku TabTab pouziva ako prvu tabulku stlpca - urcuje vyhodnotenie pravidiel ako v INISSe 3.39.
/// </summary>
/// <param name="CatalogKey">kluc predlohy INISS2</param>
/// <param name="Site">miesto vyhodnotenia (prichodova tabula, ina)</param>
/// <param name="MergeByParts">tabula strieda casti textu - <c>#MERGE</c> spaja po castiach</param>
internal sealed record Iniss2TabTabUse(string CatalogKey, ExprEvalSite Site, bool MergeByParts);

/// <summary>
/// Prevod pravidiel tabulky TabTab INISS 3.39, ktore slovnik INISS2 nevyjadri (<c>#VYLUKA</c>, <c>#ODKLON</c>, <c>#POZODJ_</c>,
/// <c>#SWITCH</c>, <c>#MERGE</c>, <c>#MERGE2</c>), na vyraz VB.NET tabulky. Server vola vyraz s textom stlpca <c>sOrig</c>
/// (text vlaku z TTVlaky alebo vlastna hodnota stlpca) a jeho vysledok prekoduje slovnikom podla <c>trn</c> - rovnako ako
/// INISS 3.39 sklada text zdrojmi v pevnom poradi a nakoniec ho prekoduje jednoduchymi pravidlami. Text s <c>@</c> caka na dalsi
/// zdroj, posledny je <c>sOrig</c>; <c>%meno%</c> je prekodovany text ineho stlpca tej istej predlohy. Ked tabulku pouzivaju
/// predlohy s inym miestom vyhodnotenia alebo striedanim casti, vyraz sa rozvetvi podla kluca predlohy.
/// </summary>
/// <param name="trainTypeKeys">kluce druhov vlakov z TrTypes s indexom zabudovaneho druhu INISSu</param>
/// <param name="trackKey">kluc kolaje INISS2 podla nazvu kolaje 3.39</param>
internal sealed class Iniss2TabTabTranspiler(IReadOnlyDictionary<string, int> trainTypeKeys, Func<string, string?> trackKey)
{
    private const string Indent = "    ";
    private readonly List<Iniss2ExprProblem> _problems = [];

    /// <summary>
    /// Co sa nedalo preniest presne (problemy prekladu podmienok).
    /// </summary>
    public IReadOnlyList<Iniss2ExprProblem> Problems => _problems;

    /// <summary>
    /// Ci tabulka ma pravidla, ktore patria do vyrazu.
    /// </summary>
    public static bool HasRules(TabTabSection section)
    {
        ArgumentNullException.ThrowIfNull(section);
        return section.Rules.Any(rule => rule.Event is not (TabTabEventKind.None or TabTabEventKind.Unknown));
    }

    /// <summary>
    /// Mena stlpcov, na ktore odkazuje <c>%meno%</c> v pravidlach <c>#SWITCH</c> a <c>#MERGE</c> - predloha ich musi mat, inak
    /// sa text nedosadi.
    /// </summary>
    public static IEnumerable<string> PeerNames(TabTabSection section)
    {
        ArgumentNullException.ThrowIfNull(section);
        return section.Rules.Where(rule => rule.Event is TabTabEventKind.Switch or TabTabEventKind.Merge or TabTabEventKind.Merge2)
            .SelectMany(rule => rule.Items.Where(item => !item.IsCondition).Select(item => Decode(item).Text))
            .Where(IsPeer)
            .Select(text => text[1..^1]);
    }

    /// <summary>
    /// Vyraz tabulky pre predlohy, ktore ju pouzivaju; prazdny, ked tabulka pravidla do vyrazu nema alebo ju ziadna predloha
    /// nepouziva ako prvu tabulku.
    /// </summary>
    public string Expression(TabTabSection section, IReadOnlyList<Iniss2TabTabUse> uses)
    {
        ArgumentNullException.ThrowIfNull(section);
        ArgumentNullException.ThrowIfNull(uses);
        if (!HasRules(section) || uses.Count == 0)
            return "";

        var rules = section.Rules.ToList();
        var merges = rules.Any(rule => rule.Event is TabTabEventKind.Merge or TabTabEventKind.Merge2);
        var peers = rules.Any(rule => rule.Event is TabTabEventKind.Switch or TabTabEventKind.Merge or TabTabEventKind.Merge2
                                      && rule.Items.Any(item => !item.IsCondition && IsPeer(Decode(item).Text)));

        // telo pre kazde miesto vyhodnotenia a striedanie casti; rovnake tela sa zlucia
        var bodies = uses.GroupBy(use => (use.Site, MergeByParts: use.MergeByParts && merges))
            .Select(group => (Body: Body(rules, group.Key.Site, group.Key.MergeByParts), Keys: group.Select(use => use.CatalogKey).ToList()))
            .GroupBy(item => item.Body, StringComparer.Ordinal)
            .Select(group => (Body: group.Key, Keys: group.SelectMany(item => item.Keys).Distinct().Order(StringComparer.Ordinal).ToList()))
            .OrderBy(item => item.Keys.Count)
            .ToList();

        var sb = new StringBuilder();
        sb.AppendLine("' prevod pravidiel INISS 3.39: text s @ caka na dalsi zdroj, posledny je povodny text sOrig");
        sb.AppendLine("If VlakS Is Nothing Then Return sOrig");
        sb.AppendLine("Dim p As TabString = Nothing");
        sb.AppendLine("Dim r As TabString = Nothing");
        sb.AppendLine("Dim v As TabString");
        sb.AppendLine("Dim accept = Function(t As TabString) As Boolean");
        sb.AppendLine(Indent + "If p IsNot Nothing Then t = New TabString(p.sString.Replace(\"@\", t.sString), If(p.iFont <> -1, p.iFont, t.iFont))");
        sb.AppendLine(Indent + "If t.sString.Contains(\"@\") Then");
        sb.AppendLine(Indent + Indent + "p = t");
        sb.AppendLine(Indent + Indent + "Return False");
        sb.AppendLine(Indent + "End If");
        sb.AppendLine(Indent + "r = t");
        sb.AppendLine(Indent + "Return True");
        sb.AppendLine("End Function");
        if (merges)
            AppendMerge(sb);
        if (peers)
            AppendPeer(sb);

        if (bodies.Count == 1)
        {
            sb.Append(bodies[0].Body);
        }
        else
        {
            sb.AppendLine("Select Case oDiv.oTable.sKey");
            for (var i = 0; i < bodies.Count; i++)
            {
                sb.AppendLine(i == bodies.Count - 1 ? Indent + "Case Else" : Indent + "Case " + string.Join(", ", bodies[i].Keys.Select(Text)));
                foreach (var line in bodies[i].Body.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                    sb.AppendLine(Indent + Indent + line.TrimEnd('\r'));
            }

            sb.AppendLine("End Select");
        }

        sb.Append("If p Is Nothing Then Return sOrig").Append('\n');
        sb.Append("Return New TabString(p.sString.Replace(\"@\", If(sOrig Is Nothing, \"\", sOrig.sString)), If(p.iFont <> -1, p.iFont, If(sOrig Is Nothing, -1, sOrig.iFont)))");
        return sb.ToString().Replace("\r\n", "\n", StringComparison.Ordinal);
    }

    // zdroje v poradi INISSu: #VYLUKA, #ODKLON, #POZODJ_ kolaje odchodu, #SWITCH, #MERGE, #MERGE2
    private string Body(List<TabTabLine> rules, ExprEvalSite site, bool mergeByParts)
    {
        var conditions = new Iniss2ExpressionTranspiler(trainTypeKeys, trackKey, site);
        var sb = new StringBuilder();

        // rovnaka udalost viackrat: plati posledne pravidlo (INISS ich drzi v mape podla textu)
        var lockout = rules.LastOrDefault(rule => rule.Event == TabTabEventKind.Vyluka);
        if (lockout is not null)
        {
            var condition = site switch
            {
                ExprEvalSite.ArrivalTable => ".VylukaPrij",
                ExprEvalSite.DepartureTable => ".VylukaOdj",
                _ => "(.VylukaPrij Or .VylukaOdj)"
            };
            sb.AppendLine(CultureInfo.InvariantCulture, $"If {condition} AndAlso accept({Value(lockout.Left)}) Then Return r");
        }

        var deflection = rules.LastOrDefault(rule => rule.Event == TabTabEventKind.Odklon);
        if (deflection is not null)
            sb.AppendLine(CultureInfo.InvariantCulture, $"If .JeOdklonen AndAlso accept({Value(deflection.Left)}) Then Return r");

        foreach (var rule in rules.Where(rule => rule.Event == TabTabEventKind.PozOdj && !string.IsNullOrEmpty(rule.PozOdjTrack))
                     .GroupBy(rule => rule.PozOdjTrack!, StringComparer.Ordinal).Select(group => group.Last()))
        {
            // kolaj, ktora v grafikone nie je, vlak mat nemoze
            if (trackKey(rule.PozOdjTrack!) is { } key)
                sb.AppendLine(CultureInfo.InvariantCulture, $"If .KlicPoziceOdjSkut = {Text(key)} AndAlso accept({Value(rule.Left)}) Then Return r");
        }

        foreach (var rule in rules.Where(rule => rule.Event == TabTabEventKind.Switch))
        {
            sb.AppendLine("' " + Comment(rule));
            var items = rule.Items;
            var first = true;
            for (var i = 0; i + 1 < items.Count; i += 2)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"{(first ? "If" : "ElseIf")} {Condition(conditions, items[i])} Then");
                sb.AppendLine(CultureInfo.InvariantCulture, $"{Indent}v = {Item(items[i + 1])}");
                first = false;
            }

            if (first)
                continue;

            sb.AppendLine("Else");
            sb.AppendLine(Indent + "v = Nothing");
            sb.AppendLine("End If");
            sb.AppendLine("If v IsNot Nothing AndAlso accept(v) Then Return r");
        }

        foreach (var kind in new[] { TabTabEventKind.Merge, TabTabEventKind.Merge2 })
        foreach (var rule in rules.Where(rule => rule.Event == kind))
        {
            sb.AppendLine("' " + Comment(rule));
            sb.AppendLine("v = Nothing");
            var parts = kind == TabTabEventKind.Merge && mergeByParts ? "True" : "False";
            var items = rule.Items;
            for (var i = 0; i + 2 < items.Count; i += 3)
                sb.AppendLine(CultureInfo.InvariantCulture, $"If {Condition(conditions, items[i])} Then v = merge(v, {Text(Decode(items[i + 1]).Text)}, {Item(items[i + 2])}, {parts})");
            sb.AppendLine("If v IsNot Nothing AndAlso accept(v) Then Return r");
        }

        _problems.AddRange(conditions.Problems.Where(problem => problem.Issue != Iniss2ExprIssue.ParseError));
        return sb.ToString().Replace("\r\n", "\n", StringComparison.Ordinal);
    }

    // polozka, ktora sa neda prelozit, sa v INISSe preskoci - podmienka neplati
    private static string Condition(Iniss2ExpressionTranspiler conditions, TabTabItem item) => conditions.Condition(item.Text) ?? "False";

    // text polozky s pismom; %meno% je text ineho stlpca predlohy
    private static string Item(TabTabItem item)
    {
        var decoded = Decode(item);
        var font = Font(decoded.Font);
        return IsPeer(decoded.Text) ? $"peer({Text(decoded.Text[1..^1])}, {font})" : $"New TabString({Text(decoded.Text)}, {font})";
    }

    private static string Value(string side)
    {
        var decoded = TabTabText.Decode(side);
        return $"New TabString({Text(decoded.Text)}, {Font(decoded.Font)})";
    }

    private static TabTabText Decode(TabTabItem item) => item.Decoded ?? TabTabText.Decode(item.Text);

    private static bool IsPeer(string text) => text.Length > 2 && text[0] == '%' && text[^1] == '%';

    private static string Font(int? font) => (font ?? -1).ToString(CultureInfo.InvariantCulture);

    // povodne pravidlo na jeden riadok (polozky boli na viacerych riadkoch spojenych \)
    private static string Comment(TabTabLine rule) =>
        string.Join(" ", (rule.Left.Trim() + " = " + rule.Right.Trim()).Split('\n').Select(line => line.Trim().TrimEnd('\\').Trim()).Where(line => line.Length > 0));

    // retazec VB.NET; riadiace znaky (nesmu byt v XML ani v literali) cez ChrW
    private static string Text(string value)
    {
        var parts = new List<string>();
        var current = new StringBuilder();
        foreach (var ch in value)
        {
            if (char.IsControl(ch))
            {
                if (current.Length > 0)
                    parts.Add("\"" + current.Replace("\"", "\"\"") + "\"");
                current.Clear();
                parts.Add("ChrW(" + ((int)ch).ToString(CultureInfo.InvariantCulture) + ")");
            }
            else
            {
                current.Append(ch);
            }
        }

        if (current.Length > 0 || parts.Count == 0)
            parts.Add("\"" + current.Replace("\"", "\"\"") + "\"");
        return parts.Count == 1 ? parts[0] : "(" + string.Join(" & ", parts) + ")";
    }

    // #MERGE: texty splnenych podmienok za sebou s oddelovacom, prazdny sa preskoci; pismo je prve zadane; po castiach (#) sa
    // kratsi text opakuje
    private static void AppendMerge(StringBuilder sb)
    {
        sb.AppendLine("Dim merge = Function(acc As TabString, sep As String, t As TabString, byParts As Boolean) As TabString");
        sb.AppendLine(Indent + "If String.IsNullOrEmpty(t.sString) Then Return acc");
        sb.AppendLine(Indent + "If acc Is Nothing Then Return t");
        sb.AppendLine(Indent + "Dim f = If(acc.iFont <> -1, acc.iFont, t.iFont)");
        sb.AppendLine(Indent + "If byParts AndAlso (acc.sString.Contains(\"#\") OrElse t.sString.Contains(\"#\")) Then");
        sb.AppendLine(Indent + Indent + "Dim a = acc.sString.Split(\"#\"c)");
        sb.AppendLine(Indent + Indent + "Dim b = t.sString.Split(\"#\"c)");
        sb.AppendLine(Indent + Indent + "Dim n = Math.Max(a.Length, b.Length)");
        sb.AppendLine(Indent + Indent + "Dim joined(n - 1) As String");
        sb.AppendLine(Indent + Indent + "For k = 0 To n - 1");
        sb.AppendLine(Indent + Indent + Indent + "joined(k) = a(k Mod a.Length) & sep & b(k Mod b.Length)");
        sb.AppendLine(Indent + Indent + "Next");
        sb.AppendLine(Indent + Indent + "Return New TabString(String.Join(\"#\", joined), f)");
        sb.AppendLine(Indent + "End If");
        sb.AppendLine(Indent + "Return New TabString(acc.sString & sep & t.sString, f)");
        sb.AppendLine("End Function");
    }

    // %meno%: stlpec predlohy podla mena alebo kluca bez ohladu na velkost pismen, jeho text prekodovany ako na tabuli;
    // stlpec, ktory nie je, alebo ten isty stlpec necha text tak, ako je
    private static void AppendPeer(StringBuilder sb)
    {
        sb.AppendLine("Dim peer = Function(name As String, font As Integer) As TabString");
        sb.AppendLine(Indent + "Dim d As Catalog.Table.Division = Nothing");
        sb.AppendLine(Indent + "If oDiv IsNot Nothing AndAlso oDiv.oTable IsNot Nothing AndAlso oDiv.oTable.aoDivision IsNot Nothing Then");
        sb.AppendLine(Indent + Indent + "For Each x In oDiv.oTable.aoDivision");
        sb.AppendLine(Indent + Indent + Indent + "If String.Equals(x.sName, name, StringComparison.OrdinalIgnoreCase) OrElse String.Equals(x.sKey, name, StringComparison.OrdinalIgnoreCase) Then");
        sb.AppendLine(Indent + Indent + Indent + Indent + "d = x");
        sb.AppendLine(Indent + Indent + Indent + Indent + "Exit For");
        sb.AppendLine(Indent + Indent + Indent + "End If");
        sb.AppendLine(Indent + Indent + "Next");
        sb.AppendLine(Indent + "End If");
        sb.AppendLine(Indent + "If d Is Nothing OrElse d Is oDiv Then Return New TabString(\"%\" & name & \"%\", font)");
        sb.AppendLine(Indent + "Dim o = TabSupport.GetTextForTrain(VlakS, d, iTableType)");
        sb.AppendLine(Indent + "If o Is Nothing Then o = New TabString(\"\", -1)");
        sb.AppendLine(Indent + "If d.oTabTab1 Is Nothing OrElse Not d.oTabTab1.bViewValues Then");
        sb.AppendLine(Indent + Indent + "Dim c As TabString = Nothing");
        sb.AppendLine(Indent + Indent + "d.Str2Code(o, c)");
        sb.AppendLine(Indent + Indent + "o = If(c, New TabString(\"\", o.iFont))");
        sb.AppendLine(Indent + "End If");
        sb.AppendLine(Indent + "Return New TabString(o.sString, If(font <> -1, font, o.iFont))");
        sb.AppendLine("End Function");
    }
}
