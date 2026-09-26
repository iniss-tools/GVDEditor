using System.Globalization;
using GVDEditor.Entities;
using GVDEditor.Properties;

namespace GVDEditor.Tools;

/// <summary>
///     Vyznam cisla pisma pri tabuliach s protokolom ELEN (ELEN, ELENOLD, ELEN10, ELEN16, ELEN16Kam, ELEKON).
///     INISS posle tabuli dolny bajt cisla a pri ELEN10/ELEN16 s bitom 0x8000 aj horny bajt; podla tych istych
///     bitov pocita sirku textu pri zarovnani. Ostatni vyrobcovia cislo pisma takto nepouzivaju.
/// </summary>
/// <param name="Id">Cislo pisma; zaporne = pismo stlpca.</param>
internal readonly record struct ElenFontCode(int Id)
{
    private const int ExtendedFlag = 0x8000;

    /// <summary>
    ///     Farba: 0 bez farby, 1 cervena, 2 zelena, 3 zlta.
    /// </summary>
    public int Color => Id < 0 ? 0 : Id & 0x03;

    /// <summary>
    ///     Pismo blika.
    /// </summary>
    public bool Blinks => Id >= 0 && (Id & 0x04) != 0;

    /// <summary>
    ///     Medzera a cislice sa nahradia vysokymi cislicami pisma.
    /// </summary>
    public bool TallDigits => Id >= 0 && (Id & 0x08) != 0;

    /// <summary>
    ///     Rez: 0 neproporcionalne (6 px), 1 tenke, 2 tucne, 3 len cislice.
    /// </summary>
    public int Face => Id < 0 ? 0 : (Id >> 4) & 0x03;

    /// <summary>
    ///     Cislo rozsireneho pisma ELEN10/ELEN16 (bity 8-11 pri bite 0x8000); 0 = ziadne, rozhoduje <see cref="Face" />.
    /// </summary>
    public int ExtendedFont => Id >= 0 && (Id & ExtendedFlag) != 0 ? (Id >> 8) & 0x0F : 0;

    /// <summary>
    ///     Cislo ma nastavene bity nad dolnym bajtom, ktore sa bez bitu 0x8000 tabuli neposielaju.
    /// </summary>
    public bool HasIgnoredHighBits => Id > 0xFF && (Id & ExtendedFlag) == 0;

    /// <summary>
    ///     Ci vyrobca pouziva cislo pisma podla tohto kodovania. Bez vyrobcu (zoznam pisiem) sa predpoklada ELEN.
    /// </summary>
    public static bool AppliesTo(TableManufacturer? manufacturer) =>
        manufacturer == null || manufacturer == TableManufacturer.ELEN || manufacturer == TableManufacturer.ELENOLD ||
        manufacturer == TableManufacturer.ELEN10 || manufacturer == TableManufacturer.ELEN16 ||
        manufacturer == TableManufacturer.ELEN16Kam || manufacturer == TableManufacturer.ELEKON;

    /// <summary>
    ///     Typ pisma, ktory zodpoveda rezu.
    /// </summary>
    public TableFontType SuggestedType => Face switch
    {
        2 => TableFontType.Bold,
        3 => TableFontType.Special,
        _ => TableFontType.None
    };

    /// <summary>
    ///     Neproporcionalny je len rez 0.
    /// </summary>
    public bool SuggestedProportional => Face != 0;

    /// <summary>
    ///     Typicka sirka znaku rezu v bodoch podla tabuliek sirok v INISSe.
    /// </summary>
    public int SuggestedWidth => Face switch
    {
        1 => 5,
        2 => 7,
        _ => 6
    };

    /// <summary>
    ///     Popis pre obsluhu, napr. "tucne, cervene, blika".
    /// </summary>
    public string Describe()
    {
        if (Id < 0)
            return Resources.ElenFont_PismoStlpca;

        var parts = new List<string>
        {
            ExtendedFont > 0
                ? string.Format(CultureInfo.CurrentCulture, Resources.ElenFont_Rozsirene, ExtendedFont)
                : Face switch
                {
                    1 => Resources.ElenFont_Tenke,
                    2 => Resources.ElenFont_Tucne,
                    3 => Resources.ElenFont_LenCislice,
                    _ => Resources.ElenFont_Neproporcionalne
                }
        };

        switch (Color)
        {
            case 1: parts.Add(Resources.ElenFont_Cervene); break;
            case 2: parts.Add(Resources.ElenFont_Zelene); break;
            case 3: parts.Add(Resources.ElenFont_Zlte); break;
        }

        if (Blinks)
            parts.Add(Resources.ElenFont_Blika);
        if (TallDigits)
            parts.Add(Resources.ElenFont_VysokeCislice);
        if (HasIgnoredHighBits)
            parts.Add(Resources.ElenFont_HornyBajt);

        return string.Join(", ", parts);
    }
}
