using GVDEditor.Domain.Entities;

namespace GVDEditor.Domain.Editing;

/// <summary>
/// Pocet vlakov, v ktorych sa stanica hlasi medzi koncovymi stanicami, a kolko z nich ju ma v kratkom hlaseni.
/// </summary>
/// <param name="Trains">Pocet vlakov, ktore stanicou idu a hlasia ju v dlhom hlaseni.</param>
/// <param name="Short">Pocet z nich, ktore ju hlasia aj v kratkom hlaseni.</param>
public readonly record struct ShortReportVote(int Trains, int Short)
{
    /// <summary>
    /// Ci ju vlaky grafikonu prevazne hlasia v kratkom hlaseni (aspon polovica).
    /// </summary>
    public bool IsShort => Trains > 0 && Short * 2 >= Trains;
}

/// <summary>
/// Kratke hlasenie importovanych vlakov podla vlakov grafikonu: rovnaky vlak prevezme svoje stanice, ostatne vlaky
/// volbu pre stanicu. Koncove stanice (vychodiskova a cielova) su v kratkom hlaseni vzdy.
/// </summary>
public static class ShortReportLearning
{
    /// <summary>
    /// Stanice trasy medzi vychodiskovou a cielovou, ktore sa hlasia v dlhom hlaseni - o tych sa rozhoduje.
    /// </summary>
    public static IEnumerable<Station> InnerStations(Train train)
    {
        var first = Origin(train);
        var last = Destination(train);
        return train.StaniceZoSmeru.Concat(train.StaniceDoSmeru)
            .Where(station => station.IsInLongReport && !ReferenceEquals(station, first) && !ReferenceEquals(station, last));
    }

    /// <summary>
    /// Vychodiskova stanica - prva hlasena zo smeru; <see langword="null" />, ak vlak v stanici grafikonu zacina alebo
    /// zo smeru ziadnu hlasenu stanicu nema (len hranicne body a prejazdy).
    /// </summary>
    public static Station? Origin(Train train) => train.StaniceZoSmeru.FirstOrDefault(station => station.IsInLongReport);

    /// <summary>
    /// Cielova stanica - posledna hlasena do smeru; <see langword="null" />, ak vlak v stanici grafikonu konci alebo
    /// do smeru ziadnu hlasenu stanicu nema.
    /// </summary>
    public static Station? Destination(Train train) => train.StaniceDoSmeru.LastOrDefault(station => station.IsInLongReport);

    /// <summary>
    /// Spocita pre kazdu stanicu (podla ID), ako ju vlaky hlasia medzi koncovymi stanicami.
    /// </summary>
    public static Dictionary<string, ShortReportVote> Votes(IEnumerable<Train> trains)
    {
        var votes = new Dictionary<string, ShortReportVote>();
        foreach (var train in trains)
        foreach (var station in InnerStations(train).DistinctBy(station => station.ID))
        {
            votes.TryGetValue(station.ID, out var vote);
            votes[station.ID] = new ShortReportVote(vote.Trains + 1, vote.Short + (station.IsInShortReport ? 1 : 0));
        }

        return votes;
    }

    /// <summary>
    /// Nastavi kratke hlasenie vlaku. Koncove stanice su v nom vzdy, stanice, ktore sa nehlasia, nikdy. Ostatne podla
    /// rovnakeho vlaku grafikonu, ak ju ma medzi koncovymi stanicami, inak podla volby pre stanicu (bez volby nie).
    /// </summary>
    /// <param name="train">Importovany vlak.</param>
    /// <param name="stations">Volba pre stanicu podla jej ID.</param>
    /// <param name="same">Ten isty vlak v grafikone, alebo <see langword="null" />.</param>
    public static void Apply(Train train, IReadOnlyDictionary<string, bool> stations, Train? same)
    {
        // stanica je record s menitelnymi priznakmi - porovnava sa instancia
        var inner = new HashSet<Station>(InnerStations(train), ReferenceEqualityComparer.Instance);
        var known = same is null
            ? new Dictionary<string, Station>()
            : InnerStations(same).DistinctBy(station => station.ID).ToDictionary(station => station.ID);

        foreach (var station in train.StaniceZoSmeru.Concat(train.StaniceDoSmeru))
        {
            if (!inner.Contains(station))
                station.IsInShortReport = station.IsInLongReport;
            else if (known.TryGetValue(station.ID, out var old))
                station.IsInShortReport = old.IsInShortReport;
            else
                station.IsInShortReport = stations.TryGetValue(station.ID, out var chosen) && chosen;
        }

        // koncova stanica je vzdy hlasena - hranicny bod alebo prejazd nie je v kratkom hlaseni nikdy
        if (Origin(train) is { } origin) origin.IsInShortReport = true;
        if (Destination(train) is { } destination) destination.IsInShortReport = true;
    }
}
