using System.Xml.Serialization;
using JetBrains.Annotations;

namespace Iniss.Elis.Data;

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
