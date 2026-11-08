namespace GVDEditor.Integration.PlanRadenia;

/// <summary>
/// Co treba pri navrhu radenia skontrolovat.
/// </summary>
internal enum PlanRadeniaIssueKind
{
    /// <summary>Zvuk chyba v zvukovej banke (skupina/kluc v <see cref="PlanRadeniaIssue.Text" />).</summary>
    MissingSound,

    /// <summary>Druh vozna sa nepodarilo urcit (cislo vozna).</summary>
    UnknownKind,

    /// <summary>Banka nema bistro vozen - hlasi sa ako restauracny.</summary>
    BistroAsRestaurant,

    /// <summary>Banka nema vozen prvej a druhej triedy - hlasi sa ako vozen druhej triedy.</summary>
    MixedAsSecond,

    /// <summary>Cielova stanica vozni z poznamky nie je medzi stanicami grafikonu (nazov z planu).</summary>
    UnknownStation,

    /// <summary>Dni z poznamky sa nepodarilo spracovat (text poznamky).</summary>
    BadNote,

    /// <summary>Poznamka sa precitala nespolahlivo (text poznamky).</summary>
    UncertainNote,

    /// <summary>Vozne sa do vlaku pridavaju v inej stanici (nazov stanice).</summary>
    JoinedCars,

    /// <summary>Na konci prveho riadku radenia nie je rusen - vozne su zoradene zlava.</summary>
    NoLocomotive,

    /// <summary>Cislo vozna v poznamke opravene (povodne a opravene cislo oddelene medzerou).</summary>
    CarFixed,

    /// <summary>Vozen z poznamky v radeni nie je (cislo vozna).</summary>
    CarMissing,

    /// <summary>V dalsom jazyku chyba zvuk - radenie sa v nom vynecha (jazyk a chybajuce zvuky).</summary>
    LanguageMissing,

    /// <summary>Banka dalsieho jazyka nema slova radenia - radenie sa v nom vynecha (jazyk).</summary>
    LanguageNoWords
}
