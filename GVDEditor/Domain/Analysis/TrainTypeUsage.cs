using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Domain.Analysis;

/// <summary>
/// Zisti, ktore grafikony pouzivaju typ vlaku. Vlak sa na typ odkazuje klucom (stlpec 4 v Export3A.TXT) -
/// po odstraneni alebo premenovani pouzivaneho typu by sa grafikon nedal otvorit.
/// </summary>
internal static class TrainTypeUsage
{
    /// <summary>
    /// Grafikony, ktorych vlaky pouzivaju typ s klucom <paramref name="key" />.
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
                : UsesKey(PathUtils.CombinePath(gvd.Dir.FullPath, GvdFileConsts.FILE_EXPORT3A)!, key);

            if (used) result.Add(gvd.PeriodFormatted);
        }

        return result;
    }

    /// <summary>
    /// Pocet vlakov kazdeho typu po grafikonoch - jeden prechod cez vsetky grafikony.
    /// </summary>
    /// <param name="grafikony">vsetky grafikony instalacie</param>
    /// <param name="open">otvoreny grafikon - jeho vlaky sa beru z pamate (mozu byt neulozene)</param>
    /// <param name="openTrains">vlaky otvoreneho grafikonu</param>
    /// <returns>skratka typu -> zoznam (grafikon, pocet vlakov)</returns>
    public static Dictionary<string, List<(string Grafikon, int Count)>> CountAll(IEnumerable<GVDDirectory> grafikony,
        GVDDirectory? open, IEnumerable<Train> openTrains)
    {
        var result = new Dictionary<string, List<(string, int)>>();
        foreach (var gvd in grafikony)
        {
            var keys = ReferenceEquals(gvd, open)
                ? openTrains.Select(t => t.Type?.Key).OfType<string>()
                : ReadKeys(PathUtils.CombinePath(gvd.Dir.FullPath, GvdFileConsts.FILE_EXPORT3A)!);

            foreach (var group in keys.GroupBy(k => k))
            {
                if (!result.TryGetValue(group.Key, out var list))
                    result[group.Key] = list = [];
                list.Add((gvd.PeriodFormatted, group.Count()));
            }
        }

        return result;
    }

    private static List<string> ReadKeys(string export3A)
    {
        var keys = new List<string>();
        if (!File.Exists(export3A))
            return keys;

        using var reader = new CsvFileReader(export3A);
        while (true)
        {
            var row = new CsvRow();
            var status = reader.ReadRow(row);
            if (status == ReadStartChar.Eof)
                return keys;

            if (status == ReadStartChar.NonEmpty && row.Count > 3)
                keys.Add(row[3]);
        }
    }

    /// <summary>
    /// Ci niektory vlak v <c>Export3A.TXT</c> ma typ s klucom <paramref name="key" />.
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
