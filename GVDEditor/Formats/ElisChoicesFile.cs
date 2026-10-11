using System.Globalization;
using GVDEditor.Domain.Entities;
using ToolsCore.Iniss.Tools;
using ToolsCore.XML;
using static ToolsCore.Iniss.Grafikon.GvdFileConsts;
using static ToolsCore.Iniss.Tools.ParseUtils;
using static ToolsCore.Iniss.Tools.PathUtils;
using static GVDEditor.Formats.FormatCommon;

namespace GVDEditor.Formats;

/// <summary>
/// Volby importu z ELIS ulozene v grafikone, aby sa pri dalsom importe nepytalo znova: stanice kratkeho hlasenia
/// (ElisKrat.txt) a priradenie poznamok vlakov (ElisPozn.txt). INISS tieto subory nepozna.
/// </summary>
internal static class ElisChoicesFile
{
    private const string FlagReservation = "MIESTENKOVY";
    private const string FlagLowFloor = "NIZKOPODLAZNY";
    private const string FlagSleeperOnly = "LOZKOVY";

    /// <summary>
    /// Nacita volbu kratkeho hlasenia pre stanice: ID stanice -> ci sa hlasi v kratkom hlaseni. Bez suboru prazdna.
    /// </summary>
    /// <param name="path">priecinok grafikonu</param>
    public static Dictionary<string, bool> ReadShort(string path)
    {
        var result = new Dictionary<string, bool>();
        ReadRows(path, FileElisShort, row =>
        {
            if (row.Count >= 2)
                result[row[0].Trim()] = row[1].Trim() == "1";
        });
        return result;
    }

    /// <summary>
    /// Zapise volbu kratkeho hlasenia pre stanice. Nazov stanice v tretom stlpci je len pre citatela.
    /// </summary>
    /// <param name="path">priecinok grafikonu</param>
    /// <param name="choices">ID stanice -> ci sa hlasi v kratkom hlaseni</param>
    /// <param name="stations">stanice grafikonu (nazvy)</param>
    /// <param name="gvd">hlavicka grafikonu</param>
    /// <param name="language">jazyk komentarov v hlavicke suboru</param>
    public static void WriteShort(string path, IReadOnlyDictionary<string, bool> choices, StationDirectory stations, GVDInfo gvd, AppLanguage language) =>
        WriteRows(path, FileElisShort, gvd, language, choices
            .OrderBy(pair => pair.Key, StringComparer.Ordinal)
            .Select(pair => new CsvRow(3) { pair.Key, pair.Value ? "1" : "0", Text(stations.FromID(pair.Key).Name) }));

    /// <summary>
    /// Nacita priradenie poznamok vlakov: text poznamky (bez useku) -> co sa s vlakom urobi. Bez suboru prazdne.
    /// </summary>
    /// <param name="path">priecinok grafikonu</param>
    public static Dictionary<string, ElisRemarkAction> ReadRemarks(string path)
    {
        var result = new Dictionary<string, ElisRemarkAction>(StringComparer.Ordinal);
        ReadRows(path, FileElisRemarks, row =>
        {
            if (row.Count < 4)
                return;

            var flag = row[1].Trim() switch
            {
                FlagReservation => ElisTrainFlag.Reservation,
                FlagLowFloor => ElisTrainFlag.LowFloor,
                FlagSleeperOnly => ElisTrainFlag.SleeperOnly,
                _ => ElisTrainFlag.None
            };
            result[row[0].AnsiToUTF()] = new ElisRemarkAction(flag, row[2].Trim(), int.Parse(row[3].Trim(), CultureInfo.InvariantCulture));
        });
        return result;
    }

    /// <summary>
    /// Zapise priradenie poznamok vlakov - aj poznamky bez ucinku, aby sa nabuduce nenavrhovali znova.
    /// </summary>
    /// <param name="path">priecinok grafikonu</param>
    /// <param name="remarks">text poznamky -> co sa s vlakom urobi</param>
    /// <param name="gvd">hlavicka grafikonu</param>
    /// <param name="language">jazyk komentarov v hlavicke suboru</param>
    public static void WriteRemarks(string path, IReadOnlyDictionary<string, ElisRemarkAction> remarks, GVDInfo gvd, AppLanguage language) =>
        WriteRows(path, FileElisRemarks, gvd, language, remarks
            .OrderBy(pair => pair.Key, StringComparer.Create(CultureInfo.GetCultureInfo("sk-SK"), false))
            .Select(pair => new CsvRow(4)
            {
                Text(pair.Key),
                pair.Value.Flag switch
                {
                    ElisTrainFlag.Reservation => FlagReservation,
                    ElisTrainFlag.LowFloor => FlagLowFloor,
                    ElisTrainFlag.SleeperOnly => FlagSleeperOnly,
                    _ => ""
                },
                pair.Value.Addition,
                pair.Value.Lockout.ToString(CultureInfo.InvariantCulture)
            }));

    /// <summary>
    /// Text v uvodzovkach - uvodzovky v nom zdvojene, ako ich cita <see cref="CsvFileReader" />.
    /// </summary>
    private static string Text(string text) => text.UTFtoANSI().Replace("\"", "\"\"", StringComparison.Ordinal).Quote();

    private static void ReadRows(string path, string fileName, Action<CsvRow> read)
    {
        var file = CombinePath(path, fileName)!;
        if (!File.Exists(file))
            return;

        using var reader = new CsvFileReader(file);
        var line = 1;
        var row = new CsvRow();
        while (true)
        {
            var status = reader.ReadRow(row);
            if (LineIsEmpty(status))
            {
                line++;
                continue;
            }

            if (LineIsEof(status))
                break;

            try
            {
                read(row);
            }
            catch (Exception e) when (e is FormatException or OverflowException)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, FormatEx, fileName, line) + e.Message, e);
            }

            line++;
        }
    }

    private static void WriteRows(string path, string fileName, GVDInfo gvd, AppLanguage language, IEnumerable<CsvRow> rows)
    {
        using var writer = new CsvFileWriter(CombinePath(path, fileName)!);
        foreach (var comment in GenerateComment(path, fileName, gvd, language))
            writer.WriteComment(comment);
        foreach (var row in rows)
            writer.WriteRow(row);
    }
}
