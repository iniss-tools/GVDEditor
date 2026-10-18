namespace Iniss.PlanRadenia;

/// <summary>
/// Farby karoserie vozidla. Farba urcuje triedu a druh vozna: zelena 2. trieda, zlta 1. trieda, cervena jedalen,
/// tmavomodra lozkovy, svetlomodra lezadlovy, siva ostatne vozne. Rusen je biely s cervenym napisom ZSSK.
/// </summary>
internal static class BodyColors
{
    // farby namerane v plane radenia (200 dpi, pasy pod strechou a nad podvozkom)
    private static readonly (string Name, Rgb Color)[] Palette =
    [
        ("green", new Rgb(120, 168, 48)),
        ("yellow", new Rgb(240, 216, 48)),
        ("red", new Rgb(205, 48, 24)),
        ("navy", new Rgb(48, 72, 140)),
        ("lightblue", new Rgb(130, 180, 216)),
        ("gray", new Rgb(165, 150, 140)),
        ("white", new Rgb(250, 250, 250))
    ];

    /// <summary>Rusen: biela karoseria s oranzovocervenym napisom ZSSK.</summary>
    public static bool IsLocomotive(Raster page, Box body)
    {
        int orange = 0, white = 0, total = 0;
        for (var y = body.Y; y < body.Bottom; y++)
            for (var x = body.X; x < body.Right; x++, total++)
            {
                var p = page[x, y];
                if (p.R > 180 && p.G is > 60 and < 150 && p.B < 100)
                    orange++;
                else if (p.IsWhite)
                    white++;
            }

        return orange > 15 && white > total / 5;
    }

    /// <summary>
    /// Podiely farieb karoserie z bodov tesne pod obrysom strechy a nad spodnym obrysom - piktogramy a stvorec
    /// triedy su vo vnutri karoserie a k obrysu nesiahaju.
    /// </summary>
    public static List<PlanColor> Read(Raster page, Box body)
    {
        var counts = new Dictionary<string, int>();
        var total = 0;
        for (var x = body.X + 6; x < body.Right - 6; x++)
        {
            foreach (var y in EdgeFill(page, x, body.Y, 1, body.CenterY).Concat(EdgeFill(page, x, body.Bottom - 1, -1, body.CenterY)))
            {
                var p = page[x, y];
                var name = Palette.MinBy(c => c.Color.Distance(p)).Name;
                counts[name] = counts.GetValueOrDefault(name) + 1;
                total++;
            }
        }

        if (total == 0)
            return [];
        return counts.Where(c => c.Value * 100 >= total * 35)
            .OrderByDescending(c => c.Value)
            .Select(c => new PlanColor { Name = c.Key, Share = Math.Round((double)c.Value / total, 2) })
            .ToList();
    }

    /// <summary>
    /// Dva body vyplne za obrysom v smere od okraja k stredu: papier a obrys sa preskocia a prvy bod za obrysom
    /// tiez (vyhladzovanie prechodu do vyplne).
    /// </summary>
    private static IEnumerable<int> EdgeFill(Raster page, int x, int from, int step, int limit)
    {
        var y = from;
        bool Inside() => (y - limit) * step < 0;
        for (var i = 0; i < 8 && Inside() && page[x, y].IsWhite; i++)
            y += step;
        var outline = 0;
        for (; outline < 8 && Inside() && IsOutline(page[x, y]); outline++)
            y += step;
        if (outline == 0)
            yield break;
        y += step;
        for (var i = 0; i < 2 && Inside(); i++, y += step)
            if (!IsOutline(page[x, y]) && !page[x, y].IsWhite)
                yield return y;
    }

    /// <summary>Obrys a tien karoserie: tmave body bez odtiena (tmavomodra karoseria odtien ma).</summary>
    private static bool IsOutline(Rgb p) => p.Max < 120 && p.Max - p.Min < 40;
}
