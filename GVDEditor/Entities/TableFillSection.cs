using ToolsCore.Tools;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Urcuje obsah sekcie na katalogovej tabuli
    /// </summary>
    public sealed class TableFillSection : Enumeration<TableFillSection>
    {
        private TableFillSection(int id, string name) : base(id, name)
        {
        }

        /// <summary>
        ///     Odkaz na seba, pre potreby DataSource
        /// </summary>
        public TableFillSection This => this;

        /// <summary>
        ///     Konvertuje ID typu obsahu sekcie na objekt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TableFillSection Parse(int id)
        {
            return id switch
            {
                0 => NotDefined,
                1 => Free,
                2 => VychadzajucaStanica,
                3 => StaniceZoSmeru,
                4 => StaniceDoSmeru,
                5 => StaniceDoSmeruNastupiste,
                6 => CielovaStanicaNastupiste,
                7 => CielovaStanicaPodchod,
                8 => CielovaStanica,
                9 => CasOdchodu,
                10 => CasPrichodu,
                11 => MeskaniePrichod,
                12 => MeskanieOdchod,
                13 => MeskaniePrichodPopis,
                14 => MeskanieOdchodPopis,
                15 => TypVlaku,
                16 => CisloVlaku,
                17 => TypCisloVlaku,
                18 => NazovVlaku,
                19 => NastupistePrichod,
                20 => NastupisteOdchod,
                21 => KolajPrichod,
                22 => KolajOdchod,
                23 => VlakStojiVStanici,
                24 => TextLine1,
                25 => TextLine2,
                26 => TypNazovOrCislo,
                27 => TypCisloVlaku6Chars,
                28 => HexTypVlaku,
                29 => TypMedzeraCisloVlaku,
                34 => Dopravca,
                36 => LinkaOdchod,
                37 => LinkaPrichod,
                _ => null
            };
        }

        #region VALUES

#pragma warning disable 1591
        public static readonly TableFillSection NotDefined = new(0, "Nedefinované");
        public static readonly TableFillSection Free = new(1, "Prázdny text");
        public static readonly TableFillSection VychadzajucaStanica = new(2, "Názov vychadzajúcej stanice");
        public static readonly TableFillSection StaniceZoSmeru = new(3, "Stanice zo smeru");
        public static readonly TableFillSection StaniceDoSmeru = new(4, "Stanice do smeru");
        public static readonly TableFillSection StaniceDoSmeruNastupiste = new(5, "Stanice do smeru (pre nástupištnú tabuľu)");
        public static readonly TableFillSection CielovaStanicaNastupiste = new(6, "Názov cieľovej stanice (nástupište)");
        public static readonly TableFillSection CielovaStanicaPodchod = new(7, "Názov cieľovej stanice (podchod)");
        public static readonly TableFillSection CielovaStanica = new(8, "Názov cieľovej stanice");
        public static readonly TableFillSection CasOdchodu = new(9, "Čas príchodu");
        public static readonly TableFillSection CasPrichodu = new(10, "Čas odchodu");
        public static readonly TableFillSection MeskaniePrichod = new(11, "Meškanie na príchode");
        public static readonly TableFillSection MeskanieOdchod = new(12, "Meškanie na odchode");
        public static readonly TableFillSection MeskaniePrichodPopis = new(13, "Meškanie na príchode (vo formáte Mešká X min.)");
        public static readonly TableFillSection MeskanieOdchodPopis = new(14, "Meškanie na odchode (vo formáte Mešká X min.)");
        public static readonly TableFillSection TypVlaku = new(15, "Typ vlaku (R, Os,...)");
        public static readonly TableFillSection CisloVlaku = new(16, "Číslo vlaku (R-121)");
        public static readonly TableFillSection TypCisloVlaku = new(17, "Typ a číslo vlaku");
        public static readonly TableFillSection NazovVlaku = new(18, "Názov vlaku (Košičan)");
        public static readonly TableFillSection NastupistePrichod = new(19, "Číslo nástupišťa na príchode");
        public static readonly TableFillSection NastupisteOdchod = new(20, "Číslo nástupišťa na odchode");
        public static readonly TableFillSection KolajPrichod = new(21, "Číslo koľaje na príchode");
        public static readonly TableFillSection KolajOdchod = new(22, "Číslo koľaje na odchode");
        public static readonly TableFillSection VlakStojiVStanici = new(23, "Vlak stojí v stanici");
        public static readonly TableFillSection TextLine1 = new(24, "Text 1. riadok");
        public static readonly TableFillSection TextLine2 = new(25, "Text 2. riadok");
        public static readonly TableFillSection TypNazovOrCislo = new(26, "Typ vlaku a názov alebo číslo vlaku");
        public static readonly TableFillSection TypCisloVlaku6Chars = new(27, "Typ a číslo vlaku na 6 znakov");
        public static readonly TableFillSection HexTypVlaku = new(28, "Hex. typ vlaku (8 znakov)");
        public static readonly TableFillSection TypMedzeraCisloVlaku = new(29, "Číslo koľaje na odchode");
        public static readonly TableFillSection Dopravca = new(34, "Dopravca");
        public static readonly TableFillSection LinkaOdchod = new(36, "Linka na odchode");
        public static readonly TableFillSection LinkaPrichod = new(37, "Linka na príchode");
#pragma warning restore 1591

        #endregion
    }
}