namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Co sa pri exporte do INISS2 nedalo preniest presne alebo co zastavilo export.
/// </summary>
internal enum Iniss2ExportIssue
{
    /// <summary>Stanica nema ciselne oznacenie; v trase vlaku chyba.</summary>
    StationWithoutNumber,

    /// <summary>Trasa vlaku neobsahuje stanicu grafikonu, ostane len vychodzia a konecna stanica.</summary>
    RouteWithoutStation,

    /// <summary>Datumova poznamka sa neda rozvinut; vlak v INISS2 nejde ziaden den.</summary>
    BadValidity,

    /// <summary>Obdobie vlaku ma viac ako 400 dni; server poznamku nerozvinie.</summary>
    PeriodTooLong,

    /// <summary>Vlak ma priznak D, server s nim nesparuje spravy GTN.</summary>
    LongDistanceFlag,

    /// <summary>Varianty vlaku idu v rovnaky den; server vezme jednu, v spolocne dni ostava ta s menej dnami.</summary>
    VariantOverlap,

    /// <summary>Vlaky s rovnakym cislom maju v jeden den rozne kolaje; server pozna pre cislo a den len jeden zaznam.</summary>
    SharedNumberTracks,

    /// <summary>Radenie ma cislo vlaku, ktory v stanici nejde; server ho zahodi.</summary>
    CompositionWithoutTrain,

    /// <summary>Radenie neplati v ziadny den vlaku, nikdy by sa neprehralo.</summary>
    CompositionOutsidePeriod,

    /// <summary>Vyluka alebo odklon obsluhy neplati v ziadny den vlaku v stanici.</summary>
    NoticeOutsidePeriod,

    /// <summary>V cielovej instalacii nie je LogText.vb, kluce hlaseni sa nedali overit.</summary>
    LogTextMissing,

    /// <summary>LogText nema hlasenie, na ktore kluc ukazuje; dodatok, vyluka alebo odklon sa neprehra.</summary>
    LogTextFunctionMissing,

    /// <summary>Vlak s rovnakym cislom v inej stanici sa nespojil - trasy si odporuju alebo sa dni nestretli.</summary>
    TrainNotMerged,

    /// <summary>Vlak ide v stanici v den mimo obdobia, ktore server stanici urci; v ten den neplati koliaj ani dodatky.</summary>
    StationDayOutsidePeriod,

    /// <summary>Stanica je medzi exportovanymi grafikonmi dvakrat.</summary>
    StationTwice,

    /// <summary>Druh tabule INISS2 nepozna; tabula sa neprevedie.</summary>
    TableUnsupported,

    /// <summary>Logicka tabula nema druh zobrazenia, ktory INISS2 pozna; jej cast neukazuje vlaky.</summary>
    TableViewUnknown,

    /// <summary>Obsah udaja tabule INISS2 nepozna; udaj ostane prazdny.</summary>
    ContentUnsupported,

    /// <summary>Udaj tabule sa prekryva s inym v tom istom riadku; server by zapis odmietol, udaj sa vynecha.</summary>
    ColumnOverlap,

    /// <summary>Tabulka TabTab ma pravidla, ktore sa nedaju previest; su ako komentar vo vyraze a treba ich prepisat vo VB.NET.</summary>
    TabTabNotConverted,

    /// <summary>Udaj odkazuje na tabulku TabTab, ktora v grafikone nie je; udaj sa neprekoduje.</summary>
    TabTabMissing,

    /// <summary>Tabulka TabTab stanice ma rovnaky nazov ako spolocna tabulka v cielovej instalacii; server sa nespusti.</summary>
    TabTabGlobalDuplicate,

    /// <summary>Vlaky s rovnakym cislom maju v jeden den rozne texty; server pozna pre cislo a den len jeden.</summary>
    TableTextConflict,

    /// <summary>Tabula so zariadenim nema linku; server vezme linku stanice alebo prvu s vhodnym protokolom, inak sa nespusti.</summary>
    TableWithoutLine,

    /// <summary>Linka INISS 3.39 nema v INISS2 obdobu (druh tabul alebo port); jej tabule ostanu bez linky.</summary>
    LineUnsupported,

    /// <summary>Predloha ELEN nema banku pisem; server sa bez nej nespusti.</summary>
    FontBankMissing,

    /// <summary>Stanica nema stavovy diagram ani sablonu; server sa bez neho nespusti.</summary>
    StateDiagramMissing,

    /// <summary>StateDgm.txt sa neda nacitat; stanica dostane diagram sablony.</summary>
    StateDiagramUnreadable,

    /// <summary>Vyraz stavoveho diagramu sa nedal prelozit; jeho cast sa vynecha.</summary>
    StateExpressionNotConverted,

    /// <summary>Cast stavoveho diagramu (akcia, starter, casovy bod) INISS2 nevie vyjadrit; vynecha sa.</summary>
    StatePartNotConverted,

    /// <summary>Akcie, ktore vedu do stavu, maju rozne hlasenia; uzol INISS2 ma jedno.</summary>
    StateReportConflict,

    /// <summary>Typ hlasenia 3.39 nema hlasenie v cieli; stav ostane bez hlasenia.</summary>
    StateReportMissing,

    /// <summary>LogText cielovej instalacie nema funkciu dodatku; export ju doplni s nahravkou z banky.</summary>
    LogTextFunctionAdded,

    /// <summary>Server INISS2 na tomto pocitaci bezi; subory by pri dalsej uprave prepisal.</summary>
    ServerRunning,

    /// <summary>Grafikon v cielovej instalacii uz ma vlaky.</summary>
    GroupExists
}
