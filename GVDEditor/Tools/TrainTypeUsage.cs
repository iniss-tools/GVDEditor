using GVDEditor.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Tools;

/// <summary>
///     Zisti, ktore grafikony pouzivaju typ vlaku. Vlak sa na typ odkazuje klucom (stlpec 4 v Export3A.TXT) -
///     po odstraneni alebo premenovani pouzivaneho typu by sa grafikon nedal otvorit.
/// </summary>
internal static class TrainTypeUsage
{
    /// <summary>
    ///     Grafikony, ktorych vlaky pouzivaju typ s klucom <paramref name="key" />.
    /// </summary>
    /// <param name="key">kluc typu vlaku</param>
    /// <param name="grafikony">vsetky grafikony instalacie</param>
    /// <param name="open">otvoreny grafikon - jeho vlaky sa beru z pamate (mozu byt neulozene)</param>
    /// <param name="openTrains">vlaky otvoreneho grafikonu</param>
    public static List<string> Find(string key, IEnumerable<GVDDirectory> grafikony, GVDDirectory? open, IEnumerable<Train> openTrains)
    {
        var result = new List<string>();
        foreach (var gvd in grafikony)
        {
            var used = ReferenceEquals(gvd, open)
                ? openTrains.Any(t => t.Type?.Key == key)
                : UsesKey(Utils.CombinePath(gvd.Dir.FullPath, FileConsts.FILE_EXPORT3A)!, key);

            if (used) result.Add(gvd.PeriodFormatted);
        }

        return result;
    }

    /// <summary>
    ///     Ci niektory vlak v <c>Export3A.TXT</c> ma typ s klucom <paramref name="key" />.
    /// </summary>
    public static bool UsesKey(string export3A, string key)
    {
        if (!File.Exists(export3A))
            return false;

        using var reader = new CsvFileReader(export3A);
        while (true)
        {
            var row = new CsvRow();
            var status = reader.ReadRow(row);
            if (status == ReadStartChar.Eof)
                return false;

            if (status == ReadStartChar.NonEmpty && row.Count > 3 && row[3] == key)
                return true;
        }
    }
}
