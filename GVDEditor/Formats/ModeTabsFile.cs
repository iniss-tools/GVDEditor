using GVDEditor.Domain.Entities;
using ToolsCore.Tools;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Tools.Utils;

namespace GVDEditor.Formats;

/// <summary>
/// Pisma tabul a ciselniky (ModeTabs.txt).
/// </summary>
internal static class ModeTabsFile
{
    /// <summary>
    /// Obsah ModeTabs.txt: pisma tabul, ich priecinok a ciselniky mimo sekcii MAIN a FONT tak, ako boli v subore.
    /// </summary>
    public sealed record Content(List<TableFont> Fonts, string FontDir, Dictionary<string, Dictionary<string, string>> Sections);

    /// <summary>
    /// Vrati typy fontov pre tabule
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    public static Content Read(string path)
    {
        var fileModeTabs = CombinePath(path, FILE_MODETABS)!;

        var fonts = new List<TableFont>();

        var modetabsF = new TxtPropsAreasFields(fileModeTabs);

        const string area = "FONT";

        // INISS sekciu [FONT] necita vobec, preto chybajuci COUNT nie je chyba
        var count = ParseIntOrDefault(modetabsF.Get(area, "COUNT", false));
        var fontDir = ParseStringOrDefault(modetabsF.Get(area, "PATH", false));

        for (var i = 0; i < count; i++)
        {
            var ti = i + 1;
            var pad = ti.PadZeros();
            var font = new TableFont
            {
                FontID = ParseIntOrDefault(modetabsF.Get(area, $"IDX_{pad}", false)),
                Name = modetabsF.Get(area, $"NAME_{pad}").ANSItoUTF(),
                Size = ParseIntOrDefault(modetabsF.Get(area, $"SIZE_{pad}", false)),
                Width = ParseIntOrDefault(modetabsF.Get(area, $"WIDTH_{pad}", false)),
                IsProportional = ParseIntOrDefault(modetabsF.Get(area, $"PROPORTIONAL_{pad}", false)).ToBool(),
                FileName = ParseStringOrDefault(modetabsF.Get(area, $"FILE_NAME_{pad}", false)).ANSItoUTF(),
                IsDia = ParseIntOrDefault(modetabsF.Get(area, $"IS_DIA_{pad}", false)).ToBool(),
                IsLower = ParseIntOrDefault(modetabsF.Get(area, $"IS_LOWER_{pad}", false)).ToBool(),
                IsUpper = ParseIntOrDefault(modetabsF.Get(area, $"IS_UPPER_{pad}", false)).ToBool(),
                IsNumber = ParseIntOrDefault(modetabsF.Get(area, $"IS_NUMBER_{pad}", false)).ToBool(),
                IsSpecChars = ParseIntOrDefault(modetabsF.Get(area, $"IS_SPEC_CHAR_{pad}", false)).ToBool(),
                IsSpecAssigment = ParseIntOrDefault(modetabsF.Get(area, $"IS_SPECIAL_ASSIGNMENT_{pad}", false)).ToBool()
            };

            var type = ParseStringOrDefault(modetabsF.Get(area, $"BOLD_FACE_{pad}", false)).ANSItoUTF();
            var parsedType = TableFontType.Parse(type);
            if (parsedType == null)
            {
                throw new FormatException($"Písmo tabule {font.Name} má neplatný typ (BOLD_FACE_{pad}): {type}.");
            }

            font.Type = parsedType;

            fonts.Add(font);
        }

        // ostatne sekcie (ciselniky) si odlozime tak, ako su v subore - INISS ma ich hodnoty zabudovane a subor mu
        // len dava mena, takze ich GVDEditor nesmie nahradzat vlastnym zoznamom
        var sections = new Dictionary<string, Dictionary<string, string>>();
        foreach (var otherArea in modetabsF.GetAreas())
        {
            if (string.IsNullOrWhiteSpace(otherArea) || otherArea is "MAIN" or "FONT")
                continue;

            sections[otherArea] = modetabsF.Get(otherArea)!
                .ToDictionary(pair => pair.Key, pair => pair.Value.ANSItoUTF());
        }

        return new Content(fonts, fontDir, sections);
    }

    /// <summary>
    /// Zapise sekciu ciselnika do ModeTabs.TXT: ak bola v povodnom subore, zapise ju nezmenenu, inak z predvolenych
    /// hodnot GVDEditora.
    /// </summary>
    /// <param name="modetabsF">Zapisovany subor.</param>
    /// <param name="area">Nazov sekcie.</param>
    /// <param name="writeDefaults">Zapis predvolenych hodnot, ak sekcia v povodnom subore nebola.</param>
    private static void WriteModeTabsSection(TxtPropsAreasFields modetabsF, string area,
        Dictionary<string, Dictionary<string, string>> sections, Action writeDefaults)
    {
        if (sections.TryGetValue(area, out var fields) && fields.Count > 0)
        {
            foreach (var pair in fields)
            {
                // COUNT a IDX_nnn su cisla, KEY_nnn a NAME_nnn retazce v uvodzovkach
                var isNumber = pair.Key == "COUNT" || pair.Key.StartsWith("IDX_", StringComparison.Ordinal);
                modetabsF.Set(area, pair.Key, pair.Value, isNumber ? WriteType.WriteNumber : WriteType.WriteStringANSI);
            }

            return;
        }

        writeDefaults();
    }

    /// <summary>
    /// Zapise mody tabuli, typy tabuli, typy obsahov a typy fontov pre tabule do suboru
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="fonts">fonty pre tabule</param>
    /// <param name="fontdir">priecinok v ktorom sa nachadzaju binarne subory s fontami</param>
    /// <param name="sections">ciselniky z povodneho suboru (<see cref="Content.Sections" />)</param>
    public static void Write(string path, IList<TableFont> fonts, string fontdir, Dictionary<string, Dictionary<string, string>> sections)
    {
        var fileModeTabs = CombinePath(path, FILE_MODETABS)!;

        var modetabsF = new TxtPropsAreasFields(fileModeTabs, true);

        const string areaMode = "VIEW_MODE";
        const string areatType = "VIEW_TYPE";
        const string areaFillSection = "FILL_SECTION";
        const string areaManufacturer = "MANUFACTURER";
        const string areaFont = "FONT";
        const string areaAlign = "ALIGN";

        modetabsF.Set("MAIN", "BREAK_CHAR", "#", WriteType.WriteStringANSI);

        WriteModeTabsSection(modetabsF, areaMode, sections, () =>
        {
            var modes = TableViewMode.GetValues();
            modetabsF.Set(areaMode, "COUNT", modes.Count);
            for (var i = 0; i < modes.Count; i++)
            {
                var ti = i + 1;
                modetabsF.Set(areaMode, $"KEY_{ti.PadZeros()}", modes[i].Key, WriteType.WriteStringANSI);
                modetabsF.Set(areaMode, $"NAME_{ti.PadZeros()}", modes[i].Name, WriteType.WriteStringANSI);
            }
        });

        WriteModeTabsSection(modetabsF, areatType, sections, () =>
        {
            var views = TableViewType.GetValues();
            modetabsF.Set(areatType, "COUNT", views.Count);
            for (var i = 0; i < views.Count; i++)
            {
                var ti = i + 1;
                modetabsF.Set(areatType, $"KEY_{ti.PadZeros()}", views[i].Key, WriteType.WriteStringANSI);
                modetabsF.Set(areatType, $"NAME_{ti.PadZeros()}", views[i].Name, WriteType.WriteStringANSI);
            }
        });

        WriteModeTabsSection(modetabsF, areaFillSection, sections, () =>
        {
            var sections = TableFillSection.GetValues();
            modetabsF.Set(areaFillSection, "COUNT", sections.Count);
            for (var i = 0; i < sections.Count; i++)
            {
                var ti = i + 1;
                modetabsF.Set(areaFillSection, $"IDX_{ti.PadZeros()}", sections[i].Id);
                modetabsF.Set(areaFillSection, $"NAME_{ti.PadZeros()}", sections[i].Name, WriteType.WriteStringANSI);
            }
        });

        WriteModeTabsSection(modetabsF, areaManufacturer, sections, () =>
        {
            // do noveho suboru idu len vyrobcovia, ktorych pozna INISS
            var manufactures = TableManufacturer.GetValues().Where(manufacturer => manufacturer.IsKnownToIniss).ToList();
            modetabsF.Set(areaManufacturer, "COUNT", manufactures.Count);
            for (var i = 0; i < manufactures.Count; i++)
            {
                var ti = i + 1;
                modetabsF.Set(areaManufacturer, $"KEY_{ti.PadZeros()}", manufactures[i].Name, WriteType.WriteStringANSI);
                modetabsF.Set(areaManufacturer, $"NAME_{ti.PadZeros()}", manufactures[i].Description, WriteType.WriteStringANSI);
            }
        });

        modetabsF.Set(areaFont, "COUNT", fonts.Count);
        if (!string.IsNullOrEmpty(fontdir)) 
            modetabsF.Set(areaFont, "PATH", fontdir, WriteType.WriteStringANSI);
        for (var i = 0; i < fonts.Count; i++)
        {
            var font = fonts[i];
            var ti = i + 1;

            modetabsF.Set(areaFont, $"IDX_{ti.PadZeros()}", font.FontID);
            modetabsF.Set(areaFont, $"NAME_{ti.PadZeros()}", font.Name, WriteType.WriteStringANSI);
            if (!string.IsNullOrEmpty(font.FileName))
                modetabsF.Set(areaFont, $"FILE_NAME_{ti.PadZeros()}", font.FileName, WriteType.WriteStringANSI);

            if (font.Type != null) 
                modetabsF.Set(areaFont, $"BOLD_FACE_{ti.PadZeros()}", font.Type.Key, WriteType.WriteStringANSI);

            if (font.Size != 0) 
                modetabsF.Set(areaFont, $"SIZE_{ti.PadZeros()}", font.Size);

            if (font.Width != 0) 
                modetabsF.Set(areaFont, $"WIDTH_{ti.PadZeros()}", font.Width);

            modetabsF.Set(areaFont, $"PROPORTIONAL_{ti.PadZeros()}", font.IsProportional.ToNumber());
            modetabsF.Set(areaFont, $"IS_DIA_{ti.PadZeros()}", font.IsDia.ToNumber());
            modetabsF.Set(areaFont, $"IS_LOWER_{ti.PadZeros()}", font.IsLower.ToNumber());
            modetabsF.Set(areaFont, $"IS_UPPER_{ti.PadZeros()}", font.IsUpper.ToNumber());
            modetabsF.Set(areaFont, $"IS_NUMBER_{ti.PadZeros()}", font.IsNumber.ToNumber());
            modetabsF.Set(areaFont, $"IS_SPEC_CHAR_{ti.PadZeros()}", font.IsSpecChars.ToNumber());
            modetabsF.Set(areaFont, $"IS_SPECIAL_ASSIGNMENT_{ti.PadZeros()}", font.IsSpecAssigment.ToNumber());
        }

        WriteModeTabsSection(modetabsF, areaAlign, sections, () =>
        {
            var aligns = TableAlign.GetValues();
            modetabsF.Set(areaAlign, "COUNT", aligns.Count);
            for (var i = 0; i < aligns.Count; i++)
            {
                var ti = i + 1;
                modetabsF.Set(areaAlign, $"IDX_{ti.PadZeros()}", aligns[i].Id);
                modetabsF.Set(areaAlign, $"NAME_{ti.PadZeros()}", aligns[i].Name, WriteType.WriteStringANSI);
            }
        });

        // pripadne dalsie sekcie, ktore GVDEditor nepozna, prejdu nezmenene
        foreach (var otherArea in sections.Keys)
            if (otherArea is not (areaMode or areatType or areaFillSection or areaManufacturer or areaAlign))
                WriteModeTabsSection(modetabsF, otherArea, sections, () => { });

        modetabsF.Save();
    }
}
