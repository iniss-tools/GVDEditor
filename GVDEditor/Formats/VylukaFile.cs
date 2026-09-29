using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Iniss.Tools;
using static GVDEditor.Formats.FormatCommon;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Iniss.Tools.ParseUtils;

namespace GVDEditor.Formats;

/// <summary>
/// Vyluky priradene vlakom v grafikone (Vyluka.txt).
/// </summary>
internal static class VylukaFile
{
    /// <summary>
    /// Nacita Vyluka.txt (nepovinny); riadok pre kazdy vlak: index,0 alebo index,1,&lt;cislo vyluky&gt;.
    /// </summary>
    public static void Read(string file, IList<Train> trains)
    {
        if (!File.Exists(file))
            return;

        ReadRows(file, FileVyluka, (row, _) =>
        {
            var train = trains[int.Parse(row[0], CultureInfo.InvariantCulture) - 1];
            var count = ParseIntOrDefault(row.ElementAtOrDefault(1));
            if (count > 1)
                throw new FormatException(Resources.Vyluka_OnlyOne);

            train.LockoutNumber = count == 1 ? int.Parse(row[2], CultureInfo.InvariantCulture) : 0;
        });
    }

    /// <summary>
    /// Zapise Vyluka.txt.
    /// </summary>
    public static void Write(string file, IEnumerable<Train> trains, IEnumerable<string> comments) =>
        WriteRows(file, comments, trains.Select((train, index) => train.LockoutNumber == 0
            ? new CsvRow { Export3File.Id(index), "0" }
            : new CsvRow { Export3File.Id(index), "1", train.LockoutNumber.ToString(CultureInfo.InvariantCulture) }));
}
