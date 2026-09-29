using GVDEditor.Domain.Entities;
using System.Globalization;
using ToolsCore.Tools;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Tools.Utils;

namespace GVDEditor.Formats;

/// <summary>
/// Hlavicka grafikonu (Grafikon.txt).
/// </summary>
internal static class InfoGvdFile
{
    /// <summary>
    /// Vrati informacie o grafikone.
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <returns>informacie o grafikone</returns>
    public static GVDInfo Read(string path)
    {
        var fileGrafikon = CombinePath(path, FILE_GRAFIKON)!;

        var gvd = new GVDInfo();

        var config = new TxtProps(fileGrafikon);

        gvd.Category = int.Parse(config.Get("CATEGORY", "1"), CultureInfo.InvariantCulture);
        gvd.Subcat = int.Parse(config.Get("SUBCAT", "0"), CultureInfo.InvariantCulture);
        var id = int.Parse(config.Get("IDSTATION"), CultureInfo.InvariantCulture);
        var name = config.Get("NAMESTATION");
        gvd.ThisStation = new Station(id.ToString(CultureInfo.InvariantCulture), name);
        gvd.TrainCount = int.Parse(config.Get("TRAINCOUNT", "0"), CultureInfo.InvariantCulture);
        gvd.StartValidTimeTable = ParseDateOnlyAlts(config.Get("START_VALID_TIMETABLE"));
        gvd.EndValidTimeTable = ParseDateOnlyAlts(config.Get("END_VALID_TIMETABLE"));
        gvd.StartValidData = ParseDateOnlyAlts(config.Get("START_VALID_DATA"));
        gvd.EndValidData = ParseDateOnlyAlts(config.Get("END_VALID_DATA"));
        gvd.CreateData = ParseDateOnlyAlts(config.Get("CREATE_DATA"));
        gvd.TTIndex = int.Parse(config.Get("TT_INDEX", "0"), CultureInfo.InvariantCulture);
        gvd.VLIndex = int.Parse(config.Get("VL_INDEX", "-1"), CultureInfo.InvariantCulture);
        gvd.STIndex = int.Parse(config.Get("ST_INDEX", "0"), CultureInfo.InvariantCulture);
        var isRegionText = config.Get("IS_REGION_TEXT", "1");
        gvd.IsRegionText = isRegionText == "1";
        gvd.OnlyCityVLIndex = int.Parse(config.Get("ONLY_CITY_VL_INDEX", "-999"), CultureInfo.InvariantCulture);

        return gvd;
    }

    /// <summary>
    /// Zapise informacie o grafikone do suboru.
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="gvd">informacie o grafikone</param>
    public static void Write(string path, GVDInfo gvd)
    {
        var fileGrafikon = CombinePath(path, FILE_GRAFIKON)!;

        var config = new TxtProps(fileGrafikon, true);

        config.Set("CATEGORY", gvd.Category);
        config.Set("SUBCAT", gvd.Subcat);
        config.Set("IDSTATION", gvd.ThisStation.ID);
        config.Set("NAMESTATION", gvd.ThisStation.Name);
        config.Set("TRAINCOUNT", gvd.TrainCount);
        config.Set("START_VALID_TIMETABLE", gvd.StartValidTimeTable.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));
        config.Set("END_VALID_TIMETABLE", gvd.EndValidTimeTable.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));
        config.Set("START_VALID_DATA", gvd.StartValidData.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));
        config.Set("END_VALID_DATA", gvd.EndValidData.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));
        config.Set("CREATE_DATA", gvd.CreateData.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));
        config.Set("TT_INDEX", gvd.TTIndex);
        config.Set("VL_INDEX", gvd.VLIndex);
        config.Set("ST_INDEX", gvd.STIndex);
        var isRegionText = gvd.IsRegionText ? 1 : 0;
        config.Set("IS_REGION_TEXT", isRegionText);
        config.Set("ONLY_CITY_VL_INDEX", gvd.OnlyCityVLIndex);
        config.Save();
    }
}
