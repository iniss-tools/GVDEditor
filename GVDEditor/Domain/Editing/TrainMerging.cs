using System.Text.RegularExpressions;
using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Entities;

namespace GVDEditor.Domain.Editing;

/// <summary>
/// Aktualizacia vlakov grafikonu importovanymi vlakmi: parovanie podla druhu, cisla a nazvu a prenesenie udajov,
/// ktore import nepozna (kolaje, doplnky, jazyky, priznaky).
/// </summary>
public static partial class TrainMerging
{
    /// <summary>
    /// Sparuje povodne vlaky s importovanymi. Paruju sa vlaky s rovnakym druhom, cislom a nazvom; z viacerych variant
    /// najprv tie s najviac spolocnymi dnami premavania a rovnakymi casmi. Kazdy vlak je najviac v jednej dvojici.
    /// </summary>
    public static List<(Train Original, Train Imported)> Match(IEnumerable<Train> originals, IEnumerable<Train> imported)
    {
        var result = new List<(Train, Train)>();
        var byKey = originals.GroupBy(Key).ToDictionary(group => group.Key, group => group.ToList());

        foreach (var variants in imported.GroupBy(Key))
        {
            if (!byKey.TryGetValue(variants.Key, out var candidates))
                continue;

            var days = new Dictionary<Train, HashSet<DateOnly>>(ReferenceEqualityComparer.Instance);
            HashSet<DateOnly> DaysOf(Train train) => days.TryGetValue(train, out var set) ? set : days[train] = RunningDays(train);

            // OrderByDescending je stabilne - pri zhode ostane poradie grafikonu a importu
            var pairs = (from original in candidates
                    from train in variants
                    select (original, train, score: Score(original, train, DaysOf(original), DaysOf(train))))
                .OrderByDescending(pair => pair.score);

            var used = new HashSet<Train>(ReferenceEqualityComparer.Instance);
            foreach (var (original, train, _) in pairs)
                if (!used.Contains(original) && !used.Contains(train))
                {
                    used.Add(original);
                    used.Add(train);
                    result.Add((original, train));
                }
        }

        return result;
    }

    /// <summary>
    /// Prenesie na importovany vlak udaje povodneho vlaku, ktore import nepozna: kolaje, cislo varianty, jazyky,
    /// doplnky, vyluku, linky a dopravcu (ak ich import nema) a priznaky (zachova sa kazdy zapnuty).
    /// </summary>
    public static void Preserve(Train original, Train imported)
    {
        imported.Track = original.Track;
        imported.TrackDeparture = original.TrackDeparture;
        imported.Variant = original.Variant;
        imported.Languages = [.. original.Languages];

        imported.IsMedzistatny |= original.IsMedzistatny;
        imported.IsDialkovy |= original.IsDialkovy;
        imported.IsMimoriadny |= original.IsMimoriadny;
        imported.IsMiestenkovy |= original.IsMiestenkovy;
        imported.IsIbaLozkovy |= original.IsIbaLozkovy;
        imported.IsNizkopodlazny |= original.IsNizkopodlazny;
        imported.IsPrestupovy |= original.IsPrestupovy;
        imported.IsPriznakO |= original.IsPriznakO;
        imported.IsMotorovy |= original.IsMotorovy;

        if (imported.LockoutNumber == 0)
            imported.LockoutNumber = original.LockoutNumber;
        if (ReferenceEquals(imported.Operator, Operator.None))
            imported.Operator = original.Operator;
        if (string.IsNullOrEmpty(imported.LineArrival))
            imported.LineArrival = original.LineArrival;
        if (string.IsNullOrEmpty(imported.LineDeparture))
            imported.LineDeparture = original.LineDeparture;

        // povodne doplnky (s ich hlaseniami) najprv, z importu len tie, ktore vlak este nemal
        var doplnky = original.Doplnky.Select(TrainDraft.CopyDodatok).ToList();
        doplnky.AddRange(imported.Doplnky.Where(dodatok => doplnky.All(own => own.Name != dodatok.Name)));
        imported.Doplnky = doplnky;
    }

    /// <summary>
    /// Nazov vlaku bez cisel, pod ktorymi vlak ide na inom useku trasy - ELIS ich pripaja za lomku
    /// (<c>TATRAN /617</c>, <c>REGIOJET /1021</c>). Lomka s textom (nazov meniaci sa po trase) ostane.
    /// </summary>
    public static string BaseName(string? name) => OtherNumbers().Replace(name ?? "", "").Trim();

    // stare importy z ELIS mali v nazve aj cisla za lomkou - parovat sa maju s vlakmi bez nich
    private static (string Type, string Number, string Name) Key(Train train) => (train.Type.Key, train.Number, BaseName(train.Name));

    [GeneratedRegex(@"\s*/\s*\d+\b")]
    private static partial Regex OtherNumbers();

    private static int Score(Train original, Train imported, HashSet<DateOnly> originalDays, HashSet<DateOnly> importedDays)
    {
        var score = originalDays.Count(importedDays.Contains) * 4;
        if (original.Arrival == imported.Arrival) score += 2;
        if (original.Departure == imported.Departure) score += 2;
        return score;
    }

    /// <summary>
    /// Dni, v ktore vlak premava; prazdne, ak sa datumove obmedzenie neda spracovat.
    /// </summary>
    internal static HashSet<DateOnly> RunningDays(Train train)
    {
        try
        {
            var bits = new DateLimit(train.ZaciatokPlatnosti, train.KoniecPlatnosti).TextToBitArray(train.DateLimitText);
            var days = new HashSet<DateOnly>();
            for (var i = 0; i < bits.Length; i++)
                if (bits[i])
                    days.Add(train.ZaciatokPlatnosti.AddDays(i));
            return days;
        }
        catch (Exception e) when (e is DateLimit.ParseException or ArgumentException or FormatException)
        {
            return [];
        }
    }
}
