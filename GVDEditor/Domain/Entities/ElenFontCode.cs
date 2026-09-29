using System.Globalization;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Entities;

/// <summary>
/// Vyznam cisla pisma pri tabuliach s protokolom ELEN (ELEN, ELENOLD, ELEN10, ELEN16, ELEN16Kam, ELEKON).
/// INISS posle tabuli dolny bajt cisla a pri ELEN10/ELEN16 s bitom 0x8000 aj horny bajt; podla tych istych
/// bitov pocita sirku textu pri zarovnani. Ostatni vyrobcovia cislo pisma takto nepouzivaju.
/// </summary>
/// <param name="Id">Cislo pisma; zaporne = pismo stlpca.</param>
internal readonly record struct ElenFontCode(int Id)
{
    private const int ExtendedFlag = 0x8000;

    /// <summary>
    /// Farba: 0 bez farby, 1 cervena, 2 zelena, 3 zlta.
    /// </summary>
    public int Color => Id < 0 ? 0 : Id & 0x03;

    /// <summary>
    /// Pismo blika.
    /// </summary>
    public bool Blinks => Id >= 0 && (Id & 0x04) != 0;

    /// <summary>
    /// Medzera a cislice sa nahradia vysokymi cislicami pisma.
    /// </summary>
    public bool TallDigits => Id >= 0 && (Id & 0x08) != 0;

    /// <summary>
    /// Rez: 0 neproporcionalne (6 px), 1 tenke, 2 tucne, 3 len cislice.
    /// </summary>
    public int Face => Id < 0 ? 0 : (Id >> 4) & 0x03;

    /// <summary>
    /// Cislo rozsireneho pisma ELEN10/ELEN16 (bity 8-11 pri bite 0x8000); 0 = ziadne, rozhoduje <see cref="Face" />.
    /// </summary>
    public int ExtendedFont => Id >= 0 && (Id & ExtendedFlag) != 0 ? (Id >> 8) & 0x0F : 0;

    /// <summary>
    /// Bity cisla, ktore nie su farba, blikanie, vysoke cislice, rez ani rozsirene pismo (napr. 0x40, ktory
    /// v datach byva vzdy). Pri skladani cisla v <see cref="Compose" /> sa ponechaju, aby sa cislo nezmenilo.
    /// </summary>
    public int KeptBits => Id < 0 ? DefaultKeptBits : Id & ~(0x3F | (ExtendedFont > 0 ? ExtendedFlag | 0x0F00 : 0));

    /// <summary>
    /// Bity noveho pisma - bit 0x40 ako v datach INISSu.
    /// </summary>
    public const int DefaultKeptBits = 0x40;

    /// <summary>
    /// Najvacsie cislo rozsireneho pisma, ktore vyrobca pozna (0 = rozsirene pisma nema).
    /// Bez vyrobcu (zoznam pisiem) sa pripusta ELEN16.
    /// </summary>
    public static int MaxExtendedFont(TableManufacturer? manufacturer)
    {
        if (manufacturer == null || manufacturer == TableManufacturer.Elen16 || manufacturer == TableManufacturer.Elen16Kam)
            return 9;
        return manufacturer == TableManufacturer.Elen10 ? 4 : 0;
    }

    /// <summary>
    /// Zlozi cislo pisma z jeho casti - opak vlastnosti tejto struktury.
    /// </summary>
    /// <param name="keptBits">ostatne bity (<see cref="KeptBits" />)</param>
    /// <param name="face">rez 0-3</param>
    /// <param name="color">farba 0-3</param>
    /// <param name="blinks">pismo blika</param>
    /// <param name="tallDigits">vysoke cislice</param>
    /// <param name="extendedFont">rozsirene pismo 1-15; 0 = ziadne</param>
    public static ElenFontCode Compose(int keptBits, int face, int color, bool blinks, bool tallDigits, int extendedFont)
    {
        var id = keptBits | (color & 0x03) | (blinks ? 0x04 : 0) | (tallDigits ? 0x08 : 0) | ((face & 0x03) << 4);
        if (extendedFont > 0)
            id |= ExtendedFlag | ((extendedFont & 0x0F) << 8);
        return new ElenFontCode(id);
    }

    /// <summary>
    /// Nazov pisma podla vzhladu, napr. "Tucne cervene blikajuce".
    /// </summary>
    public string SuggestedName()
    {
        if (Id < 0)
            return Resources.ElenFont_PismoStlpca;

        var parts = new List<string>
        {
            ExtendedFont > 0
                ? string.Format(CultureInfo.CurrentCulture, Resources.ElenFont_Rozsirene, ExtendedFont)
                : FaceName(Face)
        };
        switch (Color)
        {
            case 1: parts.Add(Resources.ElenFont_Cervene); break;
            case 2: parts.Add(Resources.ElenFont_Zelene); break;
            case 3: parts.Add(Resources.ElenFont_Zlte); break;
        }

        if (Blinks)
            parts.Add(Resources.ElenFont_Nazov_Blikajuce);
        if (TallDigits)
            parts.Add(Resources.ElenFont_Nazov_VysokeCislice);

        var name = string.Join(" ", parts);
        return char.ToUpper(name[0], CultureInfo.CurrentCulture) + name[1..];
    }

    private static string FaceName(int face) => face switch
    {
        1 => Resources.ElenFont_Tenke,
        2 => Resources.ElenFont_Tucne,
        3 => Resources.ElenFont_LenCislice,
        _ => Resources.ElenFont_Neproporcionalne
    };

    /// <summary>
    /// Cislo ma nastavene bity nad dolnym bajtom, ktore sa bez bitu 0x8000 tabuli neposielaju.
    /// </summary>
    public bool HasIgnoredHighBits => Id > 0xFF && (Id & ExtendedFlag) == 0;

    /// <summary>
    /// Ci vyrobca pouziva cislo pisma podla tohto kodovania. Bez vyrobcu (zoznam pisiem) sa predpoklada ELEN.
    /// </summary>
    public static bool AppliesTo(TableManufacturer? manufacturer) =>
        manufacturer == null || manufacturer == TableManufacturer.Elen || manufacturer == TableManufacturer.Elenold ||
        manufacturer == TableManufacturer.Elen10 || manufacturer == TableManufacturer.Elen16 ||
        manufacturer == TableManufacturer.Elen16Kam || manufacturer == TableManufacturer.Elekon;

    /// <summary>
    /// Typ pisma, ktory zodpoveda rezu.
    /// </summary>
    public TableFontType SuggestedType => Face switch
    {
        2 => TableFontType.Bold,
        3 => TableFontType.Special,
        _ => TableFontType.None
    };

    /// <summary>
    /// Neproporcionalny je len rez 0.
    /// </summary>
    public bool SuggestedProportional => Face != 0;

    /// <summary>
    /// Typicka sirka znaku rezu v bodoch podla tabuliek sirok v INISSe.
    /// </summary>
    public int SuggestedWidth => Face switch
    {
        1 => 5,
        2 => 7,
        _ => 6
    };

    /// <summary>
    /// Popis pre obsluhu, napr. "tucne, cervene, blika".
    /// </summary>
    public string Describe()
    {
        if (Id < 0)
            return Resources.ElenFont_PismoStlpca;

        var parts = new List<string>
        {
            ExtendedFont > 0
                ? string.Format(CultureInfo.CurrentCulture, Resources.ElenFont_Rozsirene, ExtendedFont)
                : FaceName(Face)
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
