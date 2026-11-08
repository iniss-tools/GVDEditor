using JetBrains.Annotations;

namespace Iniss.Elis.Data;

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
