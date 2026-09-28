using ExControls;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using ToolsCore.Entities;
using ToolsCore.StateDgm;
using ToolsCore.Tools;
using ToolsCore.XML;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Tools.Utils;
using static GVDEditor.Formats.FormatCommon;

namespace GVDEditor.Formats;

/// <summary>
/// Mapovanie stanic ELIS na stanice INISS (ElisMap.txt).
/// </summary>
internal static class ElisMapFile
{
    /// <summary>
    /// Hodnota v ELISMAP.TXT, ktora znamena "tuto stanicu z ELIS vynechat".
    /// </summary>
    public const string ELIS_MAP_SKIP = "-";

    /// <summary>
    /// Nacita priradenie nazvov stanic z programu ELIS k staniciam grafikonu z ELISMAP.TXT.
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <returns>
    /// Slovnik nazov z ELIS -> ID stanice, alebo <see cref="ELIS_MAP_SKIP" /> ak sa ma stanica vynechat.
    /// Ak subor neexistuje, vrati prazdny slovnik.
    /// </returns>
    public static Dictionary<string, string> Read(string path)
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var fileMap = CombinePath(path, FILE_ELISMAP)!;
        if (!File.Exists(fileMap))
            return map;

        using var mapF = new CsvFileReader(fileMap);
        var riadok = 1;
        var row = new CsvRow();
        while (true)
        {
            var status = mapF.ReadRow(row);
            if (LineIsEmpty(status))
            {
                riadok++;
                continue;
            }

            if (LineIsEOF(status))
                break;

            try
            {
                if (row.Count >= 2)
                    map[row[0].ANSItoUTF()] = row[1].Trim();
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, FORMAT_EX, FILE_ELISMAP, riadok) + e.Message, e);
            }

            riadok++;
        }

        return map;
    }

    /// <summary>
    /// Zapise priradenie nazvov stanic z programu ELIS do ELISMAP.TXT.
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="map">nazov z ELIS -> ID stanice alebo <see cref="ELIS_MAP_SKIP" /></param>
    /// <param name="gvd">informacie o grafikone</param>
    /// <param name="language">jazyk komentarov v hlavicke suboru</param>
    public static void Write(string path, Dictionary<string, string> map, GVDInfo gvd, AppLanguage language)
    {
        var fileMap = CombinePath(path, FILE_ELISMAP)!;

        using var mapF = new CsvFileWriter(fileMap);

        var comments = FormatCommon.GenerateComment(path, FILE_ELISMAP, gvd, language);
        foreach (var comment in comments) mapF.WriteComment(comment);

        // abecedne len kvoli citatelnosti - vzdy slovenske poradie, aby subor nezavisel od jazyka systemu
        foreach (var pair in map.OrderBy(p => p.Key, StringComparer.Create(CultureInfo.GetCultureInfo("sk-SK"), false)))
        {
            var row = new CsvRow(2)
            {
                pair.Key.UTFtoANSI().Quote(),
                pair.Value
            };

            mapF.WriteRow(row);
        }
    }
}
