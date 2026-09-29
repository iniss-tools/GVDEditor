using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using ToolsCore.Entities;

namespace GVDEditor.Domain.Editing;

/// <summary>
/// Udaje vlaku upravovane v okne vlaku. Okno meni len koncept - vlak (a radenia v grafikone) sa zmenia az
/// pri <see cref="ApplyTo" /> a <see cref="RadeniaEditing.Commit" />, takze Zrusit nic nezanecha.
/// </summary>
/// <remarks>
/// Trasa a dodatky su kopie - stlpce Dlhe/Kratke ani Kedy hlasit nesmu menit vlak pred ulozenim. Kopia vlaku
/// z nich dostane vlastne objekty a nezdiela ich so zdrojovym vlakom.
/// </remarks>
internal sealed class TrainDraft
{
    public string Number { get; set; } = "";

    public TrainType? Type { get; set; }

    /// <summary>
    /// Nazov vlaku tak, ako je v grafikone (kluc zvuku alebo volny text).
    /// </summary>
    public string Name { get; set; } = "";

    public Operator? Operator { get; set; }

    /// <summary>
    /// Cas prichodu v tvare HH:mm; pri vlaku bez trasy zo smeru sa neberie do uvahy.
    /// </summary>
    public string ArrivalText { get; set; } = "";

    /// <summary>
    /// Cas odchodu v tvare HH:mm; pri vlaku bez trasy do smeru sa neberie do uvahy.
    /// </summary>
    public string DepartureText { get; set; } = "";

    public Track? Track { get; set; }

    /// <summary>
    /// Kolaj odchodu tak, ako je v ponuke - rovnaka kolaj ako pri prichode sa pri ulozeni nezapise.
    /// </summary>
    public Track? TrackDeparture { get; set; }

    public string LineArrival { get; set; } = "";

    public string LineDeparture { get; set; } = "";

    public string DateLimitText { get; set; } = "";

    public DateOnly ValidFrom { get; set; }

    public DateOnly ValidTo { get; set; }

    /// <summary>
    /// Cislo varianty vlaku pri otvoreni okna - len na urcenie poradia medzi variantmi; cisla prideluje
    /// <see cref="TrainVariants.Normalize" />.
    /// </summary>
    public int Variant { get; set; } = -1;

    public int LockoutNumber { get; set; }

    public bool IsMedzistatny { get; set; }

    public bool IsMimoriadny { get; set; }

    public bool IsMiestenkovy { get; set; }

    public bool IsDialkovy { get; set; }

    public bool IsNizkopodlazny { get; set; }

    public bool IsPrestupovy { get; set; }

    public bool IsMotorovy { get; set; }

    public bool IsIbaLozkovy { get; set; }

    /// <summary>
    /// Priznak O z Export3A.txt - okno ho neukazuje, ale nesmie sa stratit (ani pri kopii vlaku).
    /// </summary>
    public bool IsPriznakO { get; set; }

    /// <summary>
    /// Stanice pred touto stanicou v poradi jazdy (prva je vychodzia stanica).
    /// </summary>
    public List<Station> RouteFrom { get; } = [];

    /// <summary>
    /// Stanice za touto stanicou v poradi jazdy (posledna je cielova stanica).
    /// </summary>
    public List<Station> RouteTo { get; } = [];

    /// <summary>
    /// Dalsie jazyky hlasenia (bez zakladneho).
    /// </summary>
    public List<FyzLanguage> Languages { get; } = [];

    public List<Dodatok> Doplnky { get; } = [];

    /// <summary>
    /// Kopie radeni vlaku; do grafikonu sa zapisu cez <see cref="RadeniaEditing.Commit" />.
    /// </summary>
    public RadeniaEditing Radenia { get; } = new();

    /// <summary>
    /// Ostatne varianty upravovaneho vlaku v case otvorenia okna (pri novom vlaku a kopii prazdne).
    /// </summary>
    public List<Train> Siblings { get; } = [];

    /// <summary>
    /// Zmena cisla, nazvu alebo typu sa prenesie aj na <see cref="Siblings" /> - vlak so vsetkymi variantmi
    /// ostane jednou skupinou. Bez nej vlak zo skupiny odide.
    /// </summary>
    public bool RenameSiblings { get; set; } = true;

    // cislo, nazov a typ vlaku pri otvoreni okna
    private (string Number, string Name, TrainType? Type) _originalKey;

    /// <summary>
    /// Cislo, nazov alebo typ sa zmenili oproti vlaku pri otvoreni okna.
    /// </summary>
    public bool KeyChanged => Number != _originalKey.Number || Name != _originalKey.Name || Type != _originalKey.Type;

    /// <summary>
    /// Pri ulozeni sa zmena cisla, nazvu alebo typu prenesie aj na ostatne varianty.
    /// </summary>
    public bool RenamesSiblings => RenameSiblings && KeyChanged && Siblings.Count != 0;

    /// <summary>
    /// Datumove obmedzenia inych variant zmenene v okne (kvoli prekrytiu); do vlakov sa zapisu az
    /// <see cref="ApplyVariantLimits" />.
    /// </summary>
    public Dictionary<Train, string> VariantLimits { get; } = new(ReferenceEqualityComparer.Instance);

    /// <summary>
    /// Datumove obmedzenie inej varianty - zmenene v okne alebo zo vlaku.
    /// </summary>
    public string LimitOf(Train other) => VariantLimits.TryGetValue(other, out var limit) ? limit : other.DateLimitText ?? "";

    /// <summary>
    /// Zapamata si ostatne varianty upravovaneho vlaku (vlaky s rovnakym cislom, nazvom a typom okrem riadku
    /// <paramref name="row" />) - pri zmene cisla, nazvu alebo typu sa mozu zmenit s nim.
    /// </summary>
    public void LoadSiblings(IReadOnlyList<Train> trains, int row)
    {
        Siblings.Clear();
        for (var i = 0; i < trains.Count; i++)
            if (i != row && trains[i].Number == _originalKey.Number && trains[i].Name == _originalKey.Name &&
                trains[i].Type == _originalKey.Type)
                Siblings.Add(trains[i]);
    }

    /// <summary>
    /// Prenesie nove cislo, nazov a typ na ostatne varianty (<see cref="RenamesSiblings" />).
    /// </summary>
    public void ApplyToSiblings()
    {
        if (!RenamesSiblings)
            return;

        foreach (var sibling in Siblings)
        {
            sibling.Number = Number;
            sibling.Name = Name;
            sibling.Type = Type!;
        }
    }

    /// <summary>
    /// Zapise zmenene datumove obmedzenia inych variant do ich vlakov.
    /// </summary>
    public void ApplyVariantLimits()
    {
        foreach (var (train, limit) in VariantLimits)
            train.DateLimitText = limit;
    }

    /// <summary>
    /// Smerovanie podla vyplnenych casti trasy; <see langword="null" />, ak vlak nema ziadnu stanicu.
    /// </summary>
    public Routing? Routing => TrainRules.RoutingOf(RouteFrom.Count != 0, RouteTo.Count != 0);

    /// <summary>
    /// Koncept noveho vlaku - platnost ma obdobie grafikonu.
    /// </summary>
    public static TrainDraft New(DateOnly gvdStart, DateOnly gvdEnd) => new() { ValidFrom = gvdStart, ValidTo = gvdEnd };

    /// <summary>
    /// Koncept s udajmi vlaku (aj pri kopii - ta sa lisi az tym, do ktoreho vlaku sa koncept zapise).
    /// </summary>
    public static TrainDraft From(Train train)
    {
        var draft = new TrainDraft
        {
            Number = train.Number ?? "",
            Type = train.Type,
            Name = train.Name ?? "",
            Operator = train.Operator,
            ArrivalText = train.Arrival?.ToString("HH:mm", CultureInfo.InvariantCulture) ?? "",
            DepartureText = train.Departure?.ToString("HH:mm", CultureInfo.InvariantCulture) ?? "",
            Track = train.Track,
            TrackDeparture = train.TrackDeparture ?? train.Track,
            LineArrival = train.LineArrival ?? "",
            LineDeparture = train.LineDeparture ?? "",
            DateLimitText = train.DateLimitText ?? "",
            ValidFrom = train.ZaciatokPlatnosti,
            ValidTo = train.KoniecPlatnosti,
            Variant = train.Variant,
            LockoutNumber = train.LockoutNumber,
            IsMedzistatny = train.IsMedzistatny,
            IsMimoriadny = train.IsMimoriadny,
            IsMiestenkovy = train.IsMiestenkovy,
            IsDialkovy = train.IsDialkovy,
            IsNizkopodlazny = train.IsNizkopodlazny,
            IsPrestupovy = train.IsPrestupovy,
            IsMotorovy = train.IsMotorovy,
            IsIbaLozkovy = train.IsIbaLozkovy,
            IsPriznakO = train.IsPriznakO
        };

        draft.RouteFrom.AddRange(Station.CopyRoute(train.StaniceZoSmeru));
        draft.RouteTo.AddRange(Station.CopyRoute(train.StaniceDoSmeru));
        draft.Languages.AddRange(train.Languages.Where(language => !language.IsBasic));
        draft.Doplnky.AddRange(train.Doplnky.Select(CopyDodatok));
        draft.Radenia.LoadOwn(train.Radenia);
        draft._originalKey = (draft.Number, draft.Name, draft.Type);
        return draft;
    }

    /// <summary>
    /// Kopia dodatku s vlastnym zoznamom vybranych hlaseni.
    /// </summary>
    public static Dodatok CopyDodatok(Dodatok dodatok) => new()
    {
        Sound = dodatok.Sound,
        Name = dodatok.Name,
        ChosenReports = dodatok.ChosenReports
            .Select(chosen => new ChosenReportType { Type = chosen.Type, Variants = [.. chosen.Variants] }).ToList()
    };

    /// <summary>
    /// Zapise koncept do vlaku okrem cisla varianty (prideli ho <see cref="TrainVariants.Normalize" />) a radeni.
    /// Koncept musi byt bez chyb podla <see cref="TrainRules.Check" />.
    /// </summary>
    /// <exception cref="InvalidOperationException">koncept ma chybu, ktora sa neda zapisat</exception>
    public void ApplyTo(Train train)
    {
        var routing = Routing ?? throw new InvalidOperationException("Vlak nemá trasu.");
        var track = Track ?? throw new InvalidOperationException("Vlak nemá koľaj.");

        train.Number = Number;
        train.Type = Type ?? throw new InvalidOperationException("Vlak nemá typ.");
        train.Name = Name;
        train.Operator = Operator ?? throw new InvalidOperationException("Vlak nemá dopravcu.");

        train.Arrival = RouteFrom.Count != 0 && TrainRules.TryParseTime(ArrivalText, out var arrival) ? arrival : null;
        train.Departure = RouteTo.Count != 0 && TrainRules.TryParseTime(DepartureText, out var departure) ? departure : null;
        train.Routing = routing;
        train.StartingStation = RouteFrom.FirstOrDefault();
        train.EndingStation = RouteTo.LastOrDefault();

        train.Track = track;
        train.TrackDeparture = TrackDeparture != null && !TrackDeparture.EqualsKeys(track) ? TrackDeparture : null;
        train.LineArrival = LineArrival;
        train.LineDeparture = LineDeparture;

        train.DateLimitText = DateLimitText;
        train.ZaciatokPlatnosti = ValidFrom;
        train.KoniecPlatnosti = ValidTo;

        train.IsMedzistatny = IsMedzistatny;
        train.IsMimoriadny = IsMimoriadny;
        train.IsMiestenkovy = IsMiestenkovy;
        train.IsDialkovy = IsDialkovy;
        train.IsNizkopodlazny = IsNizkopodlazny;
        train.IsPrestupovy = IsPrestupovy;
        train.IsMotorovy = IsMotorovy;
        train.IsIbaLozkovy = IsIbaLozkovy;
        train.IsPriznakO = IsPriznakO;
        train.LockoutNumber = LockoutNumber;

        train.StaniceZoSmeru.Clear();
        train.StaniceZoSmeru.AddRange(RouteFrom);
        train.StaniceDoSmeru.Clear();
        train.StaniceDoSmeru.AddRange(RouteTo);

        // vlastne zoznamy - koncept sa po ulozeni uz nemeni, ale kopia vlaku nesmie zdielat zoznam so zdrojom
        train.Languages = [.. Languages];
        train.Doplnky = [.. Doplnky];
    }
}
