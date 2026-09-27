using GVDEditor.Entities;

namespace GVDEditor.Tools;

/// <summary>
///     Prehlad variant vlakov pre zoznam vlakov v hlavnom okne: poradie vlaku v skupine (1/2), jeho ostatne
///     varianty a prekrytie dni s nimi. Stavia sa raz pre cely zoznam, zoznam ho pri kresleni len cita.
/// </summary>
internal sealed class VariantIndex
{
    private readonly Dictionary<Train, Info> _items = new(ReferenceEqualityComparer.Instance);

    /// <summary>
    ///     Varianty jedneho vlaku.
    /// </summary>
    /// <param name="Position">poradie vlaku v skupine (od 1)</param>
    /// <param name="Group">vsetky vlaky skupiny vratane neho v poradi variant</param>
    /// <param name="Overlaps">ine varianty s rovnakym obdobim platnosti a spolocnymi dnami (spolocne dni slovom)</param>
    public sealed record Info(int Position, IReadOnlyList<Train> Group, IReadOnlyList<(Train Train, string Days)> Overlaps)
    {
        /// <summary>
        ///     Pocet vlakov skupiny.
        /// </summary>
        public int Count => Group.Count;
    }

    private VariantIndex()
    {
    }

    /// <summary>
    ///     Varianty vlaku; vlak bez variant ma skupinu len so sebou.
    /// </summary>
    public Info Of(Train train) => _items.TryGetValue(train, out var info) ? info : new Info(1, [train], []);

    /// <summary>
    ///     Vlaky <paramref name="a" /> a <paramref name="b" /> su rozne varianty toho isteho vlaku.
    /// </summary>
    public bool AreSiblings(Train a, Train b) => !ReferenceEquals(a, b) && Of(a).Group.Contains(b);

    /// <summary>
    ///     Prehlad variant vsetkych vlakov zoznamu.
    /// </summary>
    public static VariantIndex Build(IReadOnlyList<Train> trains)
    {
        var index = new VariantIndex();
        foreach (var group in trains.GroupBy(train => (train.Number, train.Name, train.Type)))
        {
            var members = group.OrderBy(TrainVariants.SortKey).ToList();
            if (members.Count == 1)
                continue;

            for (var i = 0; i < members.Count; i++)
            {
                var overlaps = new List<(Train, string)>();
                foreach (var other in members)
                    if (!ReferenceEquals(other, members[i]) && CommonDays(members[i], other) is { } days)
                        overlaps.Add((other, days));

                index._items[members[i]] = new Info(i + 1, members, overlaps);
            }
        }

        return index;
    }

    /// <summary>
    ///     Spolocne dni dvoch vlakov s rovnakym obdobim platnosti; <see langword="null" />, ak nejdu v ziadny spolocny
    ///     den, maju ine obdobie alebo sa obmedzenie neda precitat.
    /// </summary>
    public static string? CommonDays(Train a, Train b)
    {
        if (a.ZaciatokPlatnosti.Date != b.ZaciatokPlatnosti.Date || a.KoniecPlatnosti.Date != b.KoniecPlatnosti.Date ||
            a.KoniecPlatnosti.Date < a.ZaciatokPlatnosti.Date)
            return null;

        var limit = new DateLimit(a.ZaciatokPlatnosti.Date, a.KoniecPlatnosti.Date, true, true, false, false);
        try
        {
            return limit.Overlap(a.DateLimitText ?? "", b.DateLimitText ?? "")
                ? limit.TextAnd(a.DateLimitText ?? "", b.DateLimitText ?? "")
                : null;
        }
        catch (DateLimit.ParseException)
        {
            return null;
        }
    }
}
