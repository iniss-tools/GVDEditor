using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;

namespace GVDEditor.Domain.Editing;

/// <summary>
/// Varianty vlaku - vlaky s rovnakym cislom, nazvom a typom v jednom grafikone. INISS ich rozlisuje cislom
/// varianty, ktore je len identifikator: na hodnote nezalezi, v skupine musi byt jedinecne (inak riadok
/// Vlaky.txt dostane len prvy vlak). Cisla prideluje GVDEditor sam (<see cref="Normalize" />).
/// </summary>
internal static class TrainVariants
{
    /// <summary>
    /// Pridelí cisla variant: jediny vlak skupiny ma -1, platne cisla (od 1, v skupine jedinecne) ostavaju,
    /// ostatne vlaky dostanu najmensie volne cislo v poradi riadkov. Medzery sa nezhustuju - zmena cisla by
    /// zbytocne menila iny vlak.
    /// </summary>
    /// <returns>vlaky, ktorym sa cislo zmenilo</returns>
    public static List<Train> Normalize(IReadOnlyList<Train> trains)
    {
        var changed = new List<Train>();
        foreach (var group in trains.GroupBy(train => (train.Number, train.Name, train.Type)))
        {
            var members = group.ToList();
            if (members.Count == 1)
            {
                if (members[0].Variant != -1)
                {
                    members[0].Variant = -1;
                    changed.Add(members[0]);
                }

                continue;
            }

            var used = new HashSet<int>();
            var pending = members.Where(train => train.Variant < 1 || !used.Add(train.Variant)).ToList();
            var next = 1;
            foreach (var train in pending)
            {
                while (used.Contains(next))
                    next++;
                train.Variant = next;
                used.Add(next);
                changed.Add(train);
            }
        }

        return changed;
    }

    /// <summary>
    /// Vsetky vlaky skupiny <paramref name="train" /> (vratane neho) zoradene podla cisla varianty.
    /// </summary>
    public static List<Train> GroupOf(IReadOnlyList<Train> trains, Train train) =>
        [.. trains.Where(other => Train.IsSameVariant(other, train)).OrderBy(SortKey)];

    /// <summary>
    /// Poradie vo vypise skupiny: platne cisla varianty vzostupne, vlaky bez cisla na konci.
    /// </summary>
    public static int SortKey(Train train) => train.Variant >= 1 ? train.Variant : int.MaxValue;

    /// <summary>
    /// Ostatne vlaky s rovnakym cislom, nazvom a typom ako koncept (bez upravovaneho riadku). Ked sa pri
    /// zmene cisla, nazvu alebo typu menia aj ostatne varianty (<see cref="TrainDraft.RenameSiblings" />),
    /// patria sem aj povodne varianty.
    /// </summary>
    public static List<Train> Others(TrainDraft draft, TrainContext context)
    {
        var others = new List<Train>();
        for (var i = 0; i < context.Trains.Count; i++)
        {
            var train = context.Trains[i];
            if (i != context.Row && train.Number == draft.Number && train.Name == draft.Name && train.Type == draft.Type)
                others.Add(train);
        }

        if (draft.RenamesSiblings)
            foreach (var sibling in draft.Siblings)
                if (!others.Contains(sibling))
                    others.Add(sibling);

        return [.. others.OrderBy(SortKey)];
    }

    /// <summary>
    /// Poradie konceptu medzi variantami (od 1) a pocet vlakov skupiny vratane neho; novy vlak je posledny.
    /// </summary>
    public static (int Position, int Count) PositionOf(TrainDraft draft, IReadOnlyList<Train> others)
    {
        var position = draft.Variant >= 1 ? others.Count(other => SortKey(other) < draft.Variant) + 1 : others.Count + 1;
        return (position, others.Count + 1);
    }

    /// <summary>
    /// Poradie ostatnej varianty v skupine s konceptom (od 1).
    /// </summary>
    public static int PositionOf(Train other, TrainDraft draft, IReadOnlyList<Train> others)
    {
        var index = others.IndexOf(other) + 1;
        var (own, _) = PositionOf(draft, others);
        return index >= own ? index + 1 : index;
    }

    /// <summary>
    /// Varianty s rovnakym obdobim platnosti, ktorych datumove obmedzenie ma s konceptom spolocne dni;
    /// <c>Days</c> je obmedzenie spolocnych dni. Variant s necitatelnym obmedzenim sa preskoci.
    /// </summary>
    public static List<(Train Train, string Days)> Overlaps(TrainDraft draft, IEnumerable<Train> others)
    {
        var result = new List<(Train, string)>();
        if (draft.ValidTo.Date < draft.ValidFrom.Date)
            return result;

        var limit = new DateLimit(draft.ValidFrom.Date, draft.ValidTo.Date, true, true, false, false);
        foreach (var other in others)
        {
            if (other.ZaciatokPlatnosti.Date != draft.ValidFrom.Date || other.KoniecPlatnosti.Date != draft.ValidTo.Date)
                continue;

            try
            {
                var otherLimit = draft.LimitOf(other);
                if (limit.Overlap(otherLimit, draft.DateLimitText))
                    result.Add((other, limit.TextAnd(otherLimit, draft.DateLimitText)));
            }
            catch (DateLimit.ParseException)
            {
                // obmedzenie inej varianty sa neda precitat - prekrytie sa neda zistit
            }
        }

        return result;
    }

    /// <summary>
    /// Obmedzenie <paramref name="limit" /> bez dni obmedzenia <paramref name="removed" /> v obdobi konceptu;
    /// <see langword="null" />, ak sa obmedzenia nedaju precitat.
    /// </summary>
    public static string? Without(TrainDraft draft, string limit, string removed)
    {
        if (draft.ValidTo.Date < draft.ValidFrom.Date)
            return null;

        var dateLimit = new DateLimit(draft.ValidFrom.Date, draft.ValidTo.Date, true, true, false, false);
        try
        {
            return dateLimit.TextAnd(limit, dateLimit.TextNot(removed));
        }
        catch (DateLimit.ParseException)
        {
            return null;
        }
    }

    /// <summary>
    /// Navrh obmedzenia inej varianty bez dni, v ktore ide upravovany vlak; <see langword="null" />, ak sa
    /// obmedzenia nedaju precitat.
    /// </summary>
    public static string? WithoutCommonDays(TrainDraft draft, Train other) => Without(draft, draft.LimitOf(other), draft.DateLimitText);

    /// <summary>
    /// Spolocne dni s variantou <paramref name="other" /> prideli jednej strane: tomuto vlaku (druhej variante sa
    /// odoberu; zapise sa po ulozeni) alebo druhej variante (odoberu sa tomuto vlaku).
    /// </summary>
    /// <returns><see langword="false" />, ak sa obmedzenia nedaju precitat</returns>
    public static bool GiveCommonDays(TrainDraft draft, Train other, bool toThis)
    {
        if (toThis)
        {
            if (WithoutCommonDays(draft, other) is not { } otherLimit)
                return false;

            SetLimit(draft, other, otherLimit);
            return true;
        }

        if (Without(draft, draft.DateLimitText, draft.LimitOf(other)) is not { } ownLimit)
            return false;

        draft.DateLimitText = ownLimit;
        return true;
    }

    /// <summary>
    /// Zmenene obmedzenie inej varianty; rovnake ako vo vlaku sa uz nepamata.
    /// </summary>
    public static void SetLimit(TrainDraft draft, Train other, string limit)
    {
        if (limit == (other.DateLimitText ?? ""))
            draft.VariantLimits.Remove(other);
        else
            draft.VariantLimits[other] = limit;
    }
}
