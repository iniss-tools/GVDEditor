namespace Iniss.PlanRadenia;

/// <summary>
/// Rozlozenie strany planu radenia: hlavicka s nazvom stanice, tabulka s riadkami vlakov (oddelenymi svetlosivymi
/// ciarami) a lavym stlpcom s udajmi vlaku. Legenda piktogramov je pod tabulkou.
/// </summary>
internal sealed class PageLayout
{
    /// <summary>Vodorovne deliace ciary tabulky (y stredu ciary), zhora nadol.</summary>
    public required List<int> Rules { get; init; }

    /// <summary>Zvisla ciara medzi stlpcom vlaku a radenim.</summary>
    public required int Column { get; init; }

    /// <summary>Pravy okraj tabulky.</summary>
    public required int Right { get; init; }

    /// <summary>Lavy okraj tabulky.</summary>
    public required int Left { get; init; }

    /// <summary>Spodok obsahu tabulky - nad legendou.</summary>
    public required int Bottom { get; init; }

    /// <summary>Riadky vlakov pod hlavickou tabulky (Druh a cislo vlaku / Radenie vlaku).</summary>
    public IEnumerable<Box> TrainRows()
    {
        // ciary: pod nadpisom strany, pod hlavickou tabulky a medzi vlakmi; pas hlavicky tabulky nema druh a cislo
        // vlaku, vynecha ho az citanie laveho stlpca
        for (var i = 0; i < Rules.Count; i++)
        {
            var top = Rules[i] + 2;
            var bottom = i + 1 < Rules.Count ? Rules[i + 1] - 2 : Bottom;
            if (bottom - top > 20)
                yield return Box.FromEdges(Left, top, Right, bottom);
        }
    }

    /// <summary>
    /// Najde ciary tabulky.
    /// </summary>
    /// <returns><see langword="null" />, ak strana nema tabulku planu radenia.</returns>
    public static PageLayout? Find(Raster page)
    {
        var rules = HorizontalRules(page);
        if (rules.Count < 2)
            return null;

        // strana s jednym vlakom ma pod hlavickou tabulky uz len vlak bez dalsej ciary
        var bottom = LegendTop(page, rules[^1]);
        // zvisla ciara zacina az pod hlavickou tabulky
        var column = VerticalRule(page, rules.Count > 1 ? rules[1] : rules[0], Math.Max(bottom, rules[^1]));
        if (column < 0)
            return null;

        var (left, right) = RuleExtent(page, rules[0]);
        return new PageLayout
        {
            Rules = rules,
            Column = column,
            Left = left,
            Right = right,
            Bottom = bottom
        };
    }

    /// <summary>
    /// Riadky, kde vacsinu sirky strany tvori svetlosiva ciara bez odtiena - deliace ciary. Strechy a podvozky
    /// vozni su cierne a farebne, do ciar sa nezaratavaju.
    /// </summary>
    private static List<int> HorizontalRules(Raster page)
    {
        int from = page.Width * 4 / 100, to = page.Width * 96 / 100;
        var candidates = new List<int>();
        for (var y = page.Height / 20; y < page.Height * 97 / 100; y++)
        {
            int rule = 0, total = 0;
            for (var x = from; x < to; x += 3, total++)
                if (page[x, y].IsRule)
                    rule++;
            // skutocna ciara ide cez celu sirku a nad aj pod nou je papier; svetly podvozok dlheho vlaku nie
            if (rule > total * 95 / 100 && MostlyWhite(page, y - 4, from, to) && MostlyWhite(page, y + 4, from, to))
                candidates.Add(y);
        }

        // ciara byva hruba 1-3 body - zo susednych riadkov jeden
        var rules = new List<int>();
        for (var i = 0; i < candidates.Count;)
        {
            var j = i;
            while (j + 1 < candidates.Count && candidates[j + 1] - candidates[j] <= 2)
                j++;
            rules.Add((candidates[i] + candidates[j]) / 2);
            i = j + 1;
        }

        return rules;
    }

    /// <summary>Zvisla ciara v lavej tretine strany medzi prvou a poslednou vodorovnou ciarou.</summary>
    private static int VerticalRule(Raster page, int top, int bottom)
    {
        int best = -1, bestCount = 0;
        for (var x = page.Width * 3 / 100; x < page.Width * 35 / 100; x++)
        {
            int rule = 0, total = 0;
            for (var y = top + 3; y < bottom - 3; y += 2, total++)
                if (page[x, y].IsRule)
                    rule++;
            if (total > 0 && rule > total * 70 / 100 && rule > bestCount)
            {
                best = x;
                bestCount = rule;
            }
        }

        return best;
    }

    private static (int Left, int Right) RuleExtent(Raster page, int y)
    {
        int left = 0, right = page.Width - 1;
        while (left < page.Width / 2 && !page[left, y].IsRule)
            left++;
        while (right > page.Width / 2 && !page[right, y].IsRule)
            right--;
        return (left, right);
    }

    /// <summary>
    /// Spodok obsahu tabulky: pod poslednou ciarou moze byt este posledny vlak a pod nim po medzere legenda na spodku
    /// strany. Hlada sa sirsia biela medzera nad legendou; ak nad nou nic nie je, tabulka konci poslednou ciarou.
    /// </summary>
    private static int LegendTop(Raster page, int lastRule)
    {
        var bottom = page.Height - 1;
        // spodok poslednej neprazdnej casti (legenda)
        while (bottom > lastRule && RowIsBlank(page, bottom))
            bottom--;
        // hore cez legendu az po medzeru aspon 40 bodov
        var y = bottom;
        while (y > lastRule)
        {
            if (!RowIsBlank(page, y))
            {
                y--;
                continue;
            }

            var gapEnd = y;
            while (y > lastRule && RowIsBlank(page, y))
                y--;
            if (gapEnd - y >= 40)
                return y + 1;
        }

        return lastRule;
    }

    /// <summary>Horny okraj legendy piktogramov na spodku strany.</summary>
    public static Box Legend(Raster page, int contentBottom)
    {
        var top = contentBottom;
        while (top < page.Height - 1 && RowIsBlank(page, top))
            top++;
        return Box.FromEdges(0, top, page.Width, page.Height);
    }

    private static bool MostlyWhite(Raster page, int y, int from, int to)
    {
        if (y < 0 || y >= page.Height)
            return true;
        int white = 0, total = 0;
        for (var x = from; x < to; x += 3, total++)
            if (page[x, y].IsWhite)
                white++;
        return white > total / 2;
    }

    private static bool RowIsBlank(Raster page, int y)
    {
        for (var x = page.Width * 2 / 100; x < page.Width * 98 / 100; x += 2)
            if (!page[x, y].IsWhite)
                return false;
        return true;
    }
}
