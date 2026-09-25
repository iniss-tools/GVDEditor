using GVDEditor.Entities;

namespace GVDEditor.Tools;

/// <summary>
///     Generovanie textov na tabuliach (TTexts.txt) z grafikonu a pracovne kopie pre okno FTableText. Spolocne pre
///     tlacidlo Vygenerovat v okne aj automaticke generovanie pri ukladani grafikonu.
/// </summary>
public static class TableTextGenerating
{
    /// <summary>
    ///     Vrati, ci sa texty daju vygenerovat pre stlpec s danym obsahom.
    /// </summary>
    public static bool IsSupported(TableFillSection? fillSection) =>
        fillSection == TableFillSection.CielovaStanica
        || fillSection == TableFillSection.CielovaStanicaNastupiste
        || fillSection == TableFillSection.CielovaStanicaPodchod
        || fillSection == TableFillSection.VychadzajucaStanica
        || fillSection == TableFillSection.StaniceDoSmeru
        || fillSection == TableFillSection.StaniceDoSmeruNastupiste
        || fillSection == TableFillSection.StaniceZoSmeru;

    /// <summary>
    ///     Vrati realizaciu, podla ktorej sa texty generuju pri ukladani – prvu, ktorej stlpec generovanie podporuje;
    ///     <see langword="null"/>, ak taka nie je.
    /// </summary>
    public static TableTextRealization? FindGeneratingRealization(TableText tableText) =>
        tableText.Realizations.FirstOrDefault(r => IsSupported(r.Item?.FillSection));

    /// <summary>
    ///     Vygeneruje text jedneho vlaku pre stlpec s danym obsahom. Zalomenie riadka je <c>#</c>, text neobsahuje
    ///     konce riadkov ani tabulatory.
    /// </summary>
    /// <param name="train">Vlak.</param>
    /// <param name="fillSection">Obsah stlpca; musi byt podporovany (<see cref="IsSupported"/>).</param>
    /// <param name="thisStation">Stanica grafikonu.</param>
    /// <exception cref="ArgumentException">Obsah stlpca generovanie nepodporuje.</exception>
    public static string GenerateText(Train train, TableFillSection fillSection, Station thisStation)
    {
        var goesOn = train.Routing == Routing.Prechadzajuci || train.Routing == Routing.Vychadzajuci;
        var comesFrom = train.Routing == Routing.Prechadzajuci || train.Routing == Routing.Konciaci;

        if (fillSection == TableFillSection.CielovaStanica || fillSection == TableFillSection.CielovaStanicaNastupiste ||
            fillSection == TableFillSection.CielovaStanicaPodchod)
            return goesOn ? train.StaniceDoSmeru.LastOrDefault()?.Name ?? "" : thisStation.Name;

        if (fillSection == TableFillSection.VychadzajucaStanica)
            return comesFrom ? train.StaniceZoSmeru.FirstOrDefault()?.Name ?? "" : thisStation.Name;

        if (fillSection == TableFillSection.StaniceDoSmeru || fillSection == TableFillSection.StaniceDoSmeruNastupiste)
        {
            if (!goesOn) return "";

            //posledna stanica v kratsom hlaseni je spravidla cielova – tu sa vynecha
            var stations = train.StaniceDoSmeru.Where(s => s.IsInShortReport).ToList();
            if (stations.Count != 0) stations.RemoveAt(stations.Count - 1);
            return JoinStations(stations);
        }

        if (fillSection == TableFillSection.StaniceZoSmeru)
        {
            if (!comesFrom) return "";

            //prva stanica v kratsom hlaseni je spravidla vychodiskova – tu sa vynecha
            return JoinStations(train.StaniceZoSmeru.Where(s => s.IsInShortReport).Skip(1));
        }

        throw new ArgumentException($"Obsah stĺpca \"{fillSection.Name}\" generovanie textov nepodporuje.", nameof(fillSection));
    }

    /// <summary>
    ///     Vygeneruje texty pre vsetky vlaky v poradi grafikonu (pismo -1 = predvolene).
    /// </summary>
    /// <inheritdoc cref="GenerateText" path="/exception"/>
    public static List<TableTrain> Generate(IEnumerable<Train> trains, TableFillSection fillSection, Station thisStation) =>
        trains.Select(t => new TableTrain { Train = t, Text = GenerateText(t, fillSection, thisStation), FontID = -1 }).ToList();

    /// <summary>
    ///     Znova vygeneruje texty vlakov vsetkych typov textov (automaticke generovanie pri ukladani). Typ textu sa
    ///     generuje podla <see cref="FindGeneratingRealization"/>; typ bez takej realizacie sa nemeni.
    /// </summary>
    /// <returns>Pocet typov textov, ktorych texty sa vygenerovali.</returns>
    public static int RegenerateAll(IEnumerable<TableText> tableTexts, IReadOnlyCollection<Train> trains, Station thisStation)
    {
        var count = 0;
        foreach (var tableText in tableTexts)
        {
            var realization = FindGeneratingRealization(tableText);
            if (realization == null) continue;

            tableText.Trains = Generate(trains, realization.Item.FillSection, thisStation);
            count++;
        }

        return count;
    }

    /// <summary>
    ///     Vrati vlaky grafikonu (v jeho poradi), ktore v zozname textov vlakov este nie su.
    /// </summary>
    public static List<Train> TrainsWithoutText(IEnumerable<Train> trains, IEnumerable<TableTrain> tableTrains)
    {
        //Train je record – vlaky sa porovnavaju ako instancie, nie podla hodnot
        var withText = new HashSet<Train>(tableTrains.Select(t => t.Train), ReferenceEqualityComparer.Instance);
        return trains.Where(t => !withText.Contains(t)).ToList();
    }

    /// <summary>
    ///     Vytvori text pre jeden vlak – vygenerovany podla obsahu stlpca, ak ho generovanie podporuje, inak prazdny.
    /// </summary>
    public static TableTrain CreateFor(Train train, TableFillSection? fillSection, Station thisStation) => new()
    {
        Train = train,
        Text = fillSection != null && IsSupported(fillSection) ? GenerateText(train, fillSection, thisStation) : "",
        FontID = -1
    };

    /// <summary>
    ///     Kopia textu vlaku (vlak je zdielany).
    /// </summary>
    public static TableTrain Clone(TableTrain tableTrain) =>
        new() { Train = tableTrain.Train, Text = tableTrain.Text, FontID = tableTrain.FontID };

    /// <summary>
    ///     Kopia realizacie (tabula a stlpec su zdielane).
    /// </summary>
    public static TableTextRealization Clone(TableTextRealization realization) =>
        new() { Table = realization.Table, Item = realization.Item };

    private static string JoinStations(IEnumerable<Station> stations) => string.Join("#", stations.Select(s => s.Name));
}
