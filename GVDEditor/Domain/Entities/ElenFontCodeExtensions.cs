using System.Globalization;
using GVDEditor.Properties;
using ToolsCore.Iniss.Elen;

namespace GVDEditor.Domain.Entities;

/// <summary>
/// Cislo pisma ELEN (<see cref="ElenFontCode" /> z ToolsCore.Iniss) vo vztahu k vyrobcom tabul, typom pisma
/// a popisom pre obsluhu GVDEditora.
/// </summary>
internal static class ElenFontCodeExtensions
{
    extension(ElenFontCode)
    {
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
        /// Ci vyrobca pouziva cislo pisma podla tohto kodovania. Bez vyrobcu (zoznam pisiem) sa predpoklada ELEN.
        /// </summary>
        public static bool AppliesTo(TableManufacturer? manufacturer) =>
            manufacturer == null || manufacturer == TableManufacturer.Elen || manufacturer == TableManufacturer.Elenold ||
            manufacturer == TableManufacturer.Elen10 || manufacturer == TableManufacturer.Elen16 ||
            manufacturer == TableManufacturer.Elen16Kam || manufacturer == TableManufacturer.Elekon;
    }

    extension(ElenFontCode code)
    {
        /// <summary>
        /// Typ pisma, ktory zodpoveda rezu.
        /// </summary>
        public TableFontType SuggestedType => code.Face switch
        {
            2 => TableFontType.Bold,
            3 => TableFontType.Special,
            _ => TableFontType.None
        };

        /// <summary>
        /// Neproporcionalny je len rez 0.
        /// </summary>
        public bool SuggestedProportional => code.Face != 0;

        /// <summary>
        /// Typicka sirka znaku rezu v bodoch podla tabuliek sirok v INISSe.
        /// </summary>
        public int SuggestedWidth => code.Face switch
        {
            1 => 5,
            2 => 7,
            _ => 6
        };

        /// <summary>
        /// Nazov pisma podla vzhladu, napr. "Tucne cervene blikajuce".
        /// </summary>
        public string SuggestedName()
        {
            if (code.Id < 0)
                return Resources.ElenFont_PismoStlpca;

            var parts = new List<string> { FaceOrExtended(code) };
            AddColor(parts, code.Color);
            if (code.Blinks)
                parts.Add(Resources.ElenFont_Nazov_Blikajuce);
            if (code.TallDigits)
                parts.Add(Resources.ElenFont_Nazov_VysokeCislice);

            var name = string.Join(" ", parts);
            return char.ToUpper(name[0], CultureInfo.CurrentCulture) + name[1..];
        }

        /// <summary>
        /// Popis pre obsluhu, napr. "tucne, cervene, blika".
        /// </summary>
        public string Describe()
        {
            if (code.Id < 0)
                return Resources.ElenFont_PismoStlpca;

            var parts = new List<string> { FaceOrExtended(code) };
            AddColor(parts, code.Color);
            if (code.Blinks)
                parts.Add(Resources.ElenFont_Blika);
            if (code.TallDigits)
                parts.Add(Resources.ElenFont_VysokeCislice);
            if (code.HasIgnoredHighBits)
                parts.Add(Resources.ElenFont_HornyBajt);

            return string.Join(", ", parts);
        }
    }

    private static string FaceOrExtended(ElenFontCode code) =>
        code.ExtendedFont > 0
            ? string.Format(CultureInfo.CurrentCulture, Resources.ElenFont_Rozsirene, code.ExtendedFont)
            : code.Face switch
            {
                1 => Resources.ElenFont_Tenke,
                2 => Resources.ElenFont_Tucne,
                3 => Resources.ElenFont_LenCislice,
                _ => Resources.ElenFont_Neproporcionalne
            };

    private static void AddColor(List<string> parts, int color)
    {
        switch (color)
        {
            case 1: parts.Add(Resources.ElenFont_Cervene); break;
            case 2: parts.Add(Resources.ElenFont_Zelene); break;
            case 3: parts.Add(Resources.ElenFont_Zlte); break;
        }
    }
}
