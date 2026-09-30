using System.Globalization;
using GVDEditor.Domain.Entities;
using ToolsCore.Iniss.Tools;
using static GVDEditor.Formats.FormatCommon;
using static GVDEditor.Formats.GvdFileConsts;

namespace GVDEditor.Formats;

/// <summary>
/// Motorove vlaky (Mos.txt) - INISS im pri nacitani zmeni druh na MOs.
/// </summary>
internal static class MosFile
{
    /// <summary>
    /// Nacita Mos.txt (nepovinny) - indexy motorovych vlakov v lubovolnom poradi.
    /// </summary>
    public static void Read(string file, IList<Train> trains)
    {
        if (!File.Exists(file))
            return;

        ReadRows(file, FileMos, (row, _) =>
        {
            var id = int.Parse(row[0], CultureInfo.InvariantCulture);
            if (id < 1 || id > trains.Count)
                throw new FormatException($"Vlak s indexom {id} neexistuje.");

            trains[id - 1].IsMotorovy = true;
        });
    }

    /// <summary>
    /// Zapise Mos.txt - index kazdeho motoroveho vlaku.
    /// </summary>
    public static void Write(string file, IEnumerable<Train> trains, IEnumerable<string> comments) =>
        WriteRows(file, comments, trains.Select((train, index) => (train, index))
            .Where(item => item.train.IsMotorovy)
            .Select(item => new CsvRow { Export3File.Id(item.index) }));
}
