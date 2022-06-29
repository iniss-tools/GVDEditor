using GVDEditor.Tools;
using ToolsCore.Entities;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GVDEditor.Entities;

/// <summary>
///     Trieda reprezentujuca vlak.
/// </summary>
public sealed record Train
{
    private NumberVariant _numberVariant;
    private Routing _routing;

    /// <summary>
    ///     Identifikator vlaku.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    ///     Cislo vlaku.
    /// </summary>
    public string Number
    {
        get => _numberVariant.Number;
        set => _numberVariant.Number = value;
    }

    /// <summary>
    ///     Cislo a varianta vlaku.
    /// </summary>
    public NumberVariant NumberVariant
    {
        get => _numberVariant;
        set
        {
            _numberVariant = value;
            _numberVariant.Number = Number;
            _numberVariant.Variant = Variant;
        }
    }

    /// <summary>
    ///     Typ vlaku.
    /// </summary>
    public TrainType Type { get; set; }

    /// <summary>
    ///     Nazov vlaku.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Smerovanie vlaku.
    /// </summary>
    public Routing Routing
    {
        get => _routing;
        set
        {
            _routing = value;
            RoutingImage = _routing.Image;
        }
    }

    /// <summary>
    ///     Smerovanie vlaku vo forme obrazku.
    /// </summary>
    public Bitmap RoutingImage { get; set; }

    /// <summary>
    ///     Stanice zo smeru.
    /// </summary>
    public List<Station> StaniceZoSmeru { get; } = new();

    /// <summary>
    ///     Stanice do smeru.
    /// </summary>
    public List<Station> StaniceDoSmeru { get; } = new();

    /// <summary>
    ///     Prichod vlaku do stanice (nullable).
    /// </summary>
    public DateTime? Arrival { get; set; }

    /// <summary>
    ///     Odchod vlaku zo stanice (nullable).
    /// </summary>
    public DateTime? Departure { get; set; }

    /// <summary>
    ///     Vychodzia stanica.
    /// </summary>
    public Station StartingStation { get; set; }

    /// <summary>
    ///     Konecna stanica.
    /// </summary>
    public Station EndingStation { get; set; }

    /// <summary>
    ///     Kolaj na stanici v ktorej stoji vlak.
    /// </summary>
    public Track Track { get; set; }

    /// <summary>
    ///     Dopravca vlaku.
    /// </summary>
    public Operator Operator { get; set; }

    /// <summary>
    ///     Datumove obmedzenie v textovej forme.
    /// </summary>
    public string DateLimitText { get; set; }

    /// <summary>
    ///     Jazykove mutacie hlasenia vlaku.
    /// </summary>
    public List<FyzLanguage> Languages { get; set; } = new();

    /// <summary>
    ///     Ci ma vlak priznak medzistatny.
    /// </summary>
    public bool IsMedzistatny { get; set; }

    /// <summary>
    ///     Ci ma vlak priznak dialkovy.
    /// </summary>
    public bool IsDialkovy { get; set; }

    /// <summary>
    ///     Ci ma vlak priznak mimoriadny.
    /// </summary>
    public bool IsMimoriadny { get; set; }

    /// <summary>
    ///     Ci ma vlak priznak miestenkovy.
    /// </summary>
    public bool IsMiestenkovy { get; set; }

    /// <summary>
    ///     Ci ma vlak priznak lozkovy.
    /// </summary>
    public bool IsIbaLozkovy { get; set; }

    /// <summary>
    ///     Ci ma vlak priznak nizkopodlazny.
    /// </summary>
    public bool IsNizkopodlazny { get; set; }

    /// <summary>
    ///     Zaciatok platnosti datumoveho obmedzenia.
    /// </summary>
    public DateTime ZaciatokPlatnosti { get; set; }

    /// <summary>
    ///     Koniec platnosti datumoveho obmedzenia.
    /// </summary>
    public DateTime KoniecPlatnosti { get; set; }

    /// <summary>
    ///     Linka na prichode.
    /// </summary>
    public string LineArrival { get; set; }

    /// <summary>
    ///     Linka na odchode.
    /// </summary>
    public string LineDeparture { get; set; }

    /// <summary>
    ///     Dodatkove hlasenia vlaku.
    /// </summary>
    public List<Dodatok> Doplnky { get; set; } = new();

    /// <summary>
    ///     Varianta vlaku.
    /// </summary>
    public int Variant
    {
        get => _numberVariant.Variant;
        set => _numberVariant.Variant = value;
    }

    /// <summary>
    ///     Radenie vlaku.
    /// </summary>
    public List<Radenie> Radenia { get; } = new();

    /// <summary>
    ///     Vrati prvy vyskyt vlaku v zozname so specifikovanymi vlastnostami.
    /// </summary>
    /// <param name="trains">zoznam vsetkych vlakov</param>
    /// <param name="trainNum">cislo vlaku</param>
    /// <param name="trainName">nazov vlaku</param>
    /// <param name="trainType">typ vlaku</param>
    /// <param name="variant">variant vlaku</param>
    /// <returns></returns>
    public static Train GetTrain(IEnumerable<Train> trains, string trainNum, string trainName, TrainType trainType, int variant) 
        => trains.FirstOrDefault(train =>
            train.Number == trainNum && train.Name == trainName && train.Type == trainType && train.Variant == variant);

    /// <summary>
    ///     Vráti všetky názvy vlakov zo zvukovej banky (priečinok V8).
    /// </summary>
    /// <returns></returns>
    public static List<string> GetTrainNames() => GlobData.Sounds.Where(soundE => soundE.Group.Name == "V8").Select(soundE => soundE.Name).ToList();

    /// <summary>
    ///     Vrati vlak zo zoznamu variant, ktory je hlavna varianta vlaku (ma najdlhsiu trasu poctom stanic).
    /// </summary>
    /// <param name="allvariants">Varianty vlaku.</param>
    /// <returns>hlavna varianta vlaku.</returns>
    public static void ReorderVariants(List<Train> allvariants)
    {
        switch (allvariants.Count)
        {
            case 0:
                return;
            case 1:
                allvariants[0].Variant = -1;
                return;
        }

        //P1: reordering
        allvariants.Sort((t1, t2) =>
        {
            var stcount1 = t1.StaniceZoSmeru.Count + t1.StaniceDoSmeru.Count;
            var stcount2 = t2.StaniceZoSmeru.Count + t2.StaniceDoSmeru.Count;
            return stcount1.CompareTo(stcount2);
        });

        //P2: changing datelimits
        //rovnaky limit pre vsetky varianty
        var limit = new DateLimit(allvariants[0].ZaciatokPlatnosti, allvariants[0].KoniecPlatnosti, insertMarks: false);
        for (var i = 0; i < allvariants.Count; i++) //prechadzam kazdym variantom vlaku
        {
            allvariants[i].Variant = i + 1; // nastavenie vlastnosti Variant kazdemu vlaku (zacina 1, lebo variant 0 nie je)
            if (i + 1 != allvariants.Count) //ak nie je tento variant posledny, urobi sa op. XOR medzi tymto a nasledujucim variantom
                allvariants[i].DateLimitText = limit.TextXor(allvariants[i].DateLimitText, allvariants[i + 1].DateLimitText);
        }
    }

    /// <summary>
    ///     Zisti, ci sa vlaky zhoduju v cisle, nazve a type.
    /// </summary>
    /// <param name="train1"></param>
    /// <param name="train2"></param>
    /// <returns></returns>
    public static bool IsSameVariant(Train train1, Train train2) 
        => train1.Number == train2.Number && train1.Name == train2.Name && train1.Type == train2.Type;
}