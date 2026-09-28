using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Entities;

namespace GVDEditor.Domain.Editing;

/// <summary>
///     Dni, v ktore idu jednotlive varianty vlaku, v spolocnom rozsahu ich obdobi platnosti - pre pruh kalendara na
///     stranke Platnost. Den, v ktory ide viac variant naraz, je prekrytie.
/// </summary>
internal sealed class VariantCalendar
{
    private VariantCalendar(DateTime from, DateTime to, List<Row> rows)
    {
        From = from;
        To = to;
        Rows = rows;
    }

    /// <summary>
    ///     Riadok pruhu - jedna varianta.
    /// </summary>
    /// <param name="Train">varianta; <see langword="null" /> = upravovany vlak</param>
    /// <param name="Position">poradie varianty v skupine (od 1)</param>
    /// <param name="Runs">den od <see cref="From" /> - vlak ide</param>
    /// <param name="Invalid">obmedzenie sa neda precitat - riadok nema ziadny den</param>
    public sealed record Row(Train? Train, int Position, bool[] Runs, bool Invalid);

    /// <summary>
    ///     Prvy den pruhu.
    /// </summary>
    public DateTime From { get; }

    /// <summary>
    ///     Posledny den pruhu.
    /// </summary>
    public DateTime To { get; }

    /// <summary>
    ///     Riadky v poradi variant.
    /// </summary>
    public IReadOnlyList<Row> Rows { get; }

    /// <summary>
    ///     Pocet dni pruhu.
    /// </summary>
    public int Days => (To - From).Days + 1;

    /// <summary>
    ///     V den <paramref name="day" /> ide viac variant naraz.
    /// </summary>
    public bool IsOverlap(int day) => Rows.Count(row => row.Runs[day]) > 1;

    /// <summary>
    ///     Pocet dni, v ktore ide viac variant naraz.
    /// </summary>
    public int OverlapDays
    {
        get
        {
            var count = 0;
            for (var day = 0; day < Days; day++)
                if (IsOverlap(day))
                    count++;
            return count;
        }
    }

    /// <summary>
    ///     Pruh pre koncept a jeho ostatne varianty (<paramref name="others" /> zoradene podla poradia); zmenene
    ///     obmedzenia inych variant sa beru z konceptu.
    /// </summary>
    public static VariantCalendar Build(TrainDraft draft, IReadOnlyList<Train> others)
    {
        var periods = new List<(Train? Train, DateTime From, DateTime To, string Limit)>
        {
            (null, draft.ValidFrom.Date, draft.ValidTo.Date, draft.DateLimitText)
        };
        periods.AddRange(others.Select(other =>
            ((Train?)other, other.ZaciatokPlatnosti.Date, other.KoniecPlatnosti.Date, draft.LimitOf(other))));

        var valid = periods.Where(p => p.From <= p.To).ToList();
        var from = valid.Count == 0 ? draft.ValidFrom.Date : valid.Min(p => p.From);
        var to = valid.Count == 0 ? from : valid.Max(p => p.To);
        var days = (to - from).Days + 1;

        var (ownPosition, _) = TrainVariants.PositionOf(draft, others);
        var rows = new List<Row>();
        foreach (var (train, start, end, limit) in periods)
        {
            var runs = new bool[days];
            var invalid = start > end;
            if (!invalid)
                try
                {
                    var bits = new DateLimit(start, end, insertMarks: false).TextToBitArray(limit ?? "");
                    var offset = (start - from).Days;
                    for (var i = 0; i < bits.Length && offset + i < days; i++)
                        runs[offset + i] = bits[i];
                }
                catch (DateLimit.ParseException)
                {
                    invalid = true;
                }

            var position = train == null ? ownPosition : TrainVariants.PositionOf(train, draft, others);
            rows.Add(new Row(train, position, runs, invalid));
        }

        return new VariantCalendar(from, to, [.. rows.OrderBy(row => row.Position)]);
    }
}
