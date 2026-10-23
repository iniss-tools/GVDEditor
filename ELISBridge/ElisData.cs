using System.Text;
using System.Xml;
using System.Xml.Serialization;
using JetBrains.Annotations;

namespace Iniss.Elis;

/// <summary>
/// Vysledok jedneho vycitania dat z programu ELIS (Cestovne poriadky, INPROP).
/// Tento subor je zdielany medzi projektom ELISBridge (x86 host nad TT.dll)
/// a GVDEditorom, ktory ho linkuje - preto nesmie zavisiet na niecom z GVDEditora.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class ElisResult
{
    /// <summary>Priecinok s datami (.tt subormi), z ktoreho sa citalo.</summary>
    public string DataPath { get; set; } = null!;

    /// <summary>Nazov stanice tak, ako ho pozna ELIS.</summary>
    public string StationName { get; set; } = null!;

    /// <summary>Cislo stanice (SR70) podla ELIS; 0, ak ho nema.</summary>
    public int StationCode { get; set; }

    /// <summary>Zaciatok platnosti cestovneho poriadku (yyyy-MM-dd).</summary>
    public string ValidFrom { get; set; } = null!;

    /// <summary>Koniec platnosti cestovneho poriadku (yyyy-MM-dd).</summary>
    public string ValidTo { get; set; } = null!;

    /// <summary>Pocet dni platnosti - dlzka retazca <see cref="ElisTrain.RunsBits" />.</summary>
    public int TotalDays { get; set; }

    /// <summary>Vlaky prechadzajuce zadanou stanicou.</summary>
    public List<ElisTrain> Trains { get; set; } = [];

    /// <summary>Zaciatok platnosti ako <see cref="DateTime" />.</summary>
    public DateTime ValidFromDate => DateTime.ParseExact(ValidFrom, "yyyy-MM-dd", null);

    /// <summary>Koniec platnosti ako <see cref="DateTime" />.</summary>
    public DateTime ValidToDate => DateTime.ParseExact(ValidTo, "yyyy-MM-dd", null);

    /// <summary>Zapise vysledok do suboru ako XML v kodovani UTF-8.</summary>
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(ElisResult));
        using var writer = new StreamWriter(path, false, new UTF8Encoding(false));
        serializer.Serialize(writer, this);
    }

    /// <summary>Nacita vysledok zo suboru zapisaneho metodou <see cref="Save" />.</summary>
    public static ElisResult Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ElisResult));
        using var reader = new StreamReader(path, Encoding.UTF8);
        return (ElisResult)serializer.Deserialize(XmlReader.Create(reader))!;
    }
}

/// <summary>
/// Jeden vlak tak, ako ho vracia TT.dll - bez naviazania na entity GVDEditora.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class ElisTrain
{
    /// <summary>Hodnota <see cref="ArrivalMinutes" />/<see cref="DepartureMinutes" />, ked cas nie je uvedeny.</summary>
    public const int NoTime = -1;

    /// <summary>Typ (kategoria) vlaku, napr. "EC", "Os".</summary>
    public string Type { get; set; } = null!;

    /// <summary>Cislo vlaku.</summary>
    public string Number { get; set; } = null!;

    /// <summary>Nazov vlaku (moze byt prazdny).</summary>
    public string Name { get; set; } = null!;

    /// <summary>Prichod do stanice v minutach od polnoci, alebo <see cref="NoTime" />.</summary>
    public int ArrivalMinutes { get; set; } = NoTime;

    /// <summary>Odchod zo stanice v minutach od polnoci, alebo <see cref="NoTime" />.</summary>
    public int DepartureMinutes { get; set; } = NoTime;

    /// <summary>Stanice pred domovskou stanicou, v poradi jazdy.</summary>
    public List<ElisStop> StationsBefore { get; set; } = [];

    /// <summary>Stanice za domovskou stanicou, v poradi jazdy.</summary>
    public List<ElisStop> StationsAfter { get; set; } = [];

    /// <summary>Nazov dopravcu (pole ON), alebo prazdne.</summary>
    public string OperatorName { get; set; } = null!;

    /// <summary>Cislo dopravcu (pole ONo) - stabilnejsi kluc nez nazov.</summary>
    public string OperatorNumber { get; set; } = null!;

    /// <summary>
    /// Linka integrovaneho dopravneho systemu, na ktorej vlak do stanice PRICHADZA
    /// (napr. "R2"). Prazdne, ak stanica do ziadneho IDS nepatri alebo vlak tam linku nema.
    /// </summary>
    public string LineArrival { get; set; } = "";

    /// <summary>Linka IDS, na ktorej vlak zo stanice ODCHADZA. Pozri <see cref="LineArrival" />.</summary>
    public string LineDeparture { get; set; } = "";

    /// <summary>Nazov dopravneho systemu liniek vyssie, napr. "IDS BK". Prazdne, ak linka nie je.</summary>
    public string LineSystem { get; set; } = "";

    /// <summary>
    /// Traťové číslo (podla knizneho cestovneho poriadku), po ktorom vlak do stanice
    /// prichadza - napr. "190". Na rozdiel od <see cref="LineArrival" /> ho maju vsetky vlaky.
    /// </summary>
    public string RailLineArrival { get; set; } = "";

    /// <summary>Traťové číslo, po ktorom vlak zo stanice odchadza. Pozri <see cref="RailLineArrival" />.</summary>
    public string RailLineDeparture { get; set; } = "";

    /// <summary>
    /// Datumove obmedzenie ako retazec '0'/'1' dlzky <see cref="ElisResult.TotalDays" />,
    /// kde index 0 zodpoveda <see cref="ElisResult.ValidFrom" />.
    /// </summary>
    public string RunsBits { get; set; } = null!;

    /// <summary>
    /// Vseobecne poznamky vlaku tak, ako ich zobrazuje ELIS, vratane useku na konci
    /// (napr. <c>náhradná autobusová doprava (Púchov-&gt;Vsetín)</c>). Bez prazdnych poznamok a bez
    /// liniek IDS - tie su v <see cref="LineArrival" />/<see cref="LineDeparture" />.
    /// </summary>
    public List<string> Remarks { get; set; } = [];
}

/// <summary>
/// Zastavka na trase vlaku.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class ElisStop
{
    /// <summary>Stanica v zahranici.</summary>
    public const int TypeForeign = 0x0002;

    /// <summary>Stanica v hlavnom meste (aj vsetky Bratislava-*, nie je to dolezitost).</summary>
    public const int TypeCapital = 0x0080;

    /// <summary>Stanica v krajskom meste.</summary>
    public const int TypeRegionalSeat = 0x0100;

    /// <summary>Stanica v okresnom meste.</summary>
    public const int TypeDistrictSeat = 0x0200;

    /// <summary>Uzlova alebo koncova stanica.</summary>
    public const int TypeJunction = 0x0800;

    /// <summary>Hranicny bod (Gr., št.hr.) - nie je to zastavka.</summary>
    public const int TypeBorderPoint = 0x2000;

    /// <summary>Vlak tu nezastavuje pre vystup - len pre nastup (TT.dll ho nezaradi do prichodov).</summary>
    public const int FlagNoAlighting = 0x0800;

    /// <summary>Vlak tu nezastavuje pre nastup - len pre vystup (TT.dll ho nezaradi do odchodov).</summary>
    public const int FlagNoBoarding = 0x1000;

    /// <summary>Prejazd bez vymeny cestujucich - oba priznaky naraz.</summary>
    public const int FlagPassing = FlagNoAlighting | FlagNoBoarding;

    /// <summary>
    /// Cislo stanice (SR70), ktore je zaroven ID stanice v zvukovej banke INISS - jednoznacny
    /// kluc na parovanie. 0, ak ho ELIS pre danu zastavku nema.
    /// </summary>
    public int Code { get; set; }

    /// <summary>Nazov tak, ako ho pise ELIS (skratky "n.", druhy jazyk v zatvorke).</summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Druh stanice z TT.dll (<c>TTStType</c>) - bitove priznaky <c>Type*</c>. Vyznam bitov je odvodeny z dat,
    /// TT.dll ho nedokumentuje.
    /// </summary>
    public int StationType { get; set; }

    /// <summary>
    /// Priznaky zastavky v trase vlaku (horne bity slova trasy z <c>TTTrainRouteExt</c>) - <c>Flag*</c>.
    /// </summary>
    public int RouteFlags { get; set; }

    /// <summary>Ci je to hranicny bod.</summary>
    [XmlIgnore]
    public bool IsBorderPoint => (StationType & TypeBorderPoint) != 0;

    /// <summary>Ci vlak bodom len prechadza bez vymeny cestujucich.</summary>
    [XmlIgnore]
    public bool IsPassing => (RouteFlags & FlagPassing) == FlagPassing;

    /// <summary>Ci sa zastavka hlasi - hranicne body a prejazdy nie.</summary>
    [XmlIgnore]
    public bool IsAnnounced => !IsBorderPoint && !IsPassing;

    /// <summary>Ci je stanica v zahranici alebo na hranici - vlak, ktory nou ide, je medzistatny.</summary>
    [XmlIgnore]
    public bool IsAbroad => (StationType & (TypeForeign | TypeBorderPoint)) != 0;

    /// <inheritdoc />
    public override string ToString() => Name;
}