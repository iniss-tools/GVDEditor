using System.Globalization;
using GVDEditor.Domain.Entities;
using Serilog;
using ToolsCore.Iniss.Tools;
using static GVDEditor.Formats.FormatCommon;
using static ToolsCore.Iniss.Tools.ParseUtils;

namespace GVDEditor.Formats;

/// <summary>
/// Vyluky a odklony zadane obsluhou INISS (Vyluky1.txt a Odklony1.txt v DATA) - oba subory maju rovnaky tvar. GVDEditor ich
/// len cita pre export do INISS2, spravuje ich INISS.
/// </summary>
internal static class TrainNoticesFile
{
    /// <summary>Vyluky vlakov.</summary>
    public const string FileLockouts = "VYLUKY1.txt";

    /// <summary>Odklony vlakov.</summary>
    public const string FileDiversions = "ODKLONY1.txt";

    /// <summary>
    /// Nacita riadky v poradi suboru. Chybny riadok INISS preskoci, rovnako aj tu; chybajuci subor je prazdny zoznam.
    /// </summary>
    /// <param name="dataDir">priecinok DATA</param>
    /// <param name="fileName">nazov suboru</param>
    public static List<TrainNotice> Read(string dataDir, string fileName)
    {
        var file = PathUtils.CombinePath(dataDir, fileName)!;
        var notices = new List<TrainNotice>();
        if (!File.Exists(file))
            return notices;

        ReadRows(file, fileName, (row, rowNumber) =>
        {
            try
            {
                var station = string.IsNullOrWhiteSpace(row.ElementAtOrDefault(1)) ? 0 : int.Parse(row[1].Trim(), CultureInfo.InvariantCulture);
                var from = ParseDateOnlyAlts(row[3].Trim());
                var to = ParseDateOnlyAlts(row[4].Trim());
                if (from > to)
                {
                    Log.Warning("Riadok {Row} suboru {File} ma obdobie od {From} po {To}, preskocil sa", rowNumber, file, from, to);
                    return;
                }

                notices.Add(new TrainNotice(row[0].Trim(), station, int.Parse(row[2].Trim(), CultureInfo.InvariantCulture), from, to,
                    row.ElementAtOrDefault(5)?.Trim() ?? ""));
            }
            catch (Exception e) when (e is FormatException or OverflowException or ArgumentOutOfRangeException)
            {
                Log.Warning(e, "Riadok {Row} suboru {File} sa preskocil", rowNumber, file);
            }
        });

        return notices;
    }
}
