using ToolsCore.Tools;

namespace GVDEditor.Entities;

/// <summary>
///     Urcuje obsah sekcie na katalogovej tabuli.
/// </summary>
public sealed class TableFillSection : Enumeration<TableFillSection>
{
    private TableFillSection(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    ///     Odkaz na seba, pre potreby DataSource.
    /// </summary>
    public TableFillSection This => this;

    /// <summary>
    ///     Konvertuje ID typu obsahu sekcie na objekt.
    /// </summary>
    /// <param name="id">Cislo zo sekcie [FILL_SECTION] v ModeTabs.TXT (TYPE_ITEMS_IDX v TKatalog.TXT).</param>
    /// <returns>Objekt alebo <see langword="null" />, ak INISS take cislo nepozna.</returns>
    public static TableFillSection? Parse(int id) => GetValues().FirstOrDefault(section => section.Id == id);

    #region VALUES

#pragma warning disable 1591
    // Cisla su zabudovane v INISSe (ArgsPanel_*); poradie a nazvy zodpovedaju zoznamu v INISS 3.39.
    public static readonly TableFillSection NotDefined = new(0, "Nedefinované");
    public static readonly TableFillSection Free = new(1, "Prázdny text");
    public static readonly TableFillSection VychadzajucaStanica = new(2, "Názov vychádzajúcej stanice");
    public static readonly TableFillSection StaniceZoSmeru = new(3, "Stanice zo smeru");
    public static readonly TableFillSection StaniceDoSmeruNastupiste = new(4, "Stanice do smeru (pre nástupištnú tabuľu)");
    public static readonly TableFillSection StaniceDoSmeru = new(5, "Stanice do smeru");
    public static readonly TableFillSection CielovaStanicaNastupiste = new(6, "Názov cieľovej stanice (nástupište)");
    public static readonly TableFillSection CielovaStanicaPodchod = new(7, "Názov cieľovej stanice (podchod)");
    public static readonly TableFillSection CielovaStanica = new(8, "Názov cieľovej stanice");
    public static readonly TableFillSection CasOdchodu = new(9, "Čas odchodu");
    public static readonly TableFillSection CasPrichodu = new(10, "Čas príchodu");
    public static readonly TableFillSection MeskaniePrichod = new(11, "Meškanie na príchode");
    public static readonly TableFillSection MeskanieOdchod = new(12, "Meškanie na odchode");
    public static readonly TableFillSection MeskaniePrichodPopis = new(13, "Meškanie na príchode (vo formáte Mešká X min.)");
    public static readonly TableFillSection MeskanieOdchodPopis = new(14, "Meškanie na odchode (vo formáte Mešká X min.)");
    public static readonly TableFillSection TypVlaku = new(15, "Typ vlaku (R, Os,...)");
    public static readonly TableFillSection CisloVlaku = new(16, "Číslo vlaku (R-121)");
    public static readonly TableFillSection TypCisloVlaku = new(17, "Typ a číslo vlaku");
    public static readonly TableFillSection NazovVlaku = new(18, "Názov vlaku (Košičan)");
    public static readonly TableFillSection KolajPrichod = new(19, "Text koľaje na príchode (z Pozice_A)");
    public static readonly TableFillSection KolajOdchod = new(20, "Text koľaje na odchode (z Pozice_A)");
    public static readonly TableFillSection NastupistePrichod = new(21, "Označenie nástupišťa na príchode (z Pozice_A)");
    public static readonly TableFillSection NastupisteOdchod = new(22, "Označenie nástupišťa na odchode (z Pozice_A)");
    public static readonly TableFillSection VlakStojiVStanici = new(23, "Vlak stojí v stanici");
    public static readonly TableFillSection TextLine1 = new(24, "Text 1. riadok");
    public static readonly TableFillSection TextLine2 = new(25, "Text 2. riadok");
    public static readonly TableFillSection TypNazovOrCislo = new(26, "Typ vlaku a názov alebo číslo vlaku");
    public static readonly TableFillSection TypCisloVlaku6Chars = new(27, "Typ a číslo vlaku na 6 znakov");
    public static readonly TableFillSection HexTypVlaku = new(28, "Hex. typ vlaku (8 znakov)");
    public static readonly TableFillSection TypMedzeraCisloVlaku = new(29, "Typ, medzera a číslo vlaku");
    public static readonly TableFillSection NastupisteKolajPrichod = new(30, "Nástupište a koľaj na príchode (text z Pozice_A)");
    public static readonly TableFillSection NastupisteKolajOdchod = new(31, "Nástupište a koľaj na odchode (text z Pozice_A)");
    public static readonly TableFillSection KolajAltPrichod = new(32, "Alternatívny text koľaje na príchode (z Pozice_A)");
    public static readonly TableFillSection KolajAltOdchod = new(33, "Alternatívny text koľaje na odchode (z Pozice_A)");
    public static readonly TableFillSection Dopravca = new(34, "Dopravca");
    public static readonly TableFillSection CisloVlaku35 = new(35, "Číslo vlaku (variant 35)");
    public static readonly TableFillSection LinkaOdchod = new(36, "Linka na odchode");
    public static readonly TableFillSection LinkaPrichod = new(37, "Linka na príchode");
    public static readonly TableFillSection CisloVlaku38 = new(38, "Číslo vlaku (variant 38)");
#pragma warning restore 1591

    #endregion
}