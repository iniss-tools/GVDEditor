using System.Globalization;
using ToolsCore.Iniss.Tools;
using static ToolsCore.Iniss.Grafikon.GvdFileConsts;
using static ToolsCore.Iniss.Tools.ParseUtils;
using static ToolsCore.Iniss.Tools.PathUtils;
using static GVDEditor.Formats.FormatCommon;

namespace GVDEditor.Formats;

/// <summary>
/// Hlasene meskania (Zpozdeni.txt) a vyrovnavacia pamat Zpozdeni.dat.
/// </summary>
internal static class ZpozdeniFile
{
    /// <summary>
    /// Vrati mozne casy meskani.
    /// </summary>
    /// <returns>meskania</returns>
    /// <param name="dataDir">priecinok DATA instalacie INISS</param>
    public static List<string> Read(string dataDir)
    {
        var file = CombinePath(dataDir, FileZpozdeni)!;

        var meskania = new List<string>();

        using var zpozdeniF = new CsvFileReader(file);
        var riadok = 1;
        var row = new CsvRow();
        while (true)
        {
            var status = zpozdeniF.ReadRow(row);
            if (LineIsEmpty(status))
            {
                riadok++;
                continue;
            }

            if (LineIsEof(status))
                break;

            try
            {
                meskania.Add(row[0]);
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, FormatEx, FileZpozdeni, riadok) + e.Message, e);
            }

            riadok++;
        }

        return meskania;
    }

    /// <summary>
    /// Zmaze vyrovnavaciu pamat Zpozdeni.DAT. INISS textovy Zpozdeni.TXT cita, len ked .DAT chyba alebo je spusteny
    /// s parametrom /Import - bez zmazania by dalej pouzival stary zoznam.
    /// </summary>
    private static void DeleteCache(string dataDir)
    {
        var cache = CombinePath(dataDir, FileZpozdeniDat)!;
        if (File.Exists(cache))
            File.Delete(cache);
    }

    /// <summary>
    /// Zapise predvolene casy meskani.
    /// </summary>
    /// <param name="dataDir">priecinok DATA instalacie INISS</param>
    public static void WriteDefault(string dataDir)
    {
        var file = CombinePath(dataDir, FileZpozdeni)!;
        DeleteCache(dataDir);

        using var zpozdeniF = new CsvFileWriter(file);
        for (var i = 5; i <= 480; i += 5)
        {
            var row = new CsvRow();
            row.Insert(0, i.ToString(CultureInfo.InvariantCulture));

            zpozdeniF.WriteRow(row);
        }
    }

    /// <summary>
    /// Zapise mozne casy meskani do suboru.
    /// </summary>
    /// <param name="meskania">meskania</param>
    /// <param name="dataDir">priecinok DATA instalacie INISS</param>
    public static void Write(string dataDir, IEnumerable<string> meskania)
    {
        var file = CombinePath(dataDir, FileZpozdeni)!;
        DeleteCache(dataDir);

        using var zpozdeniF = new CsvFileWriter(file);
        foreach (var meskanie in meskania)
        {
            var row = new CsvRow();
            row.Insert(0, meskanie.ToString(CultureInfo.InvariantCulture));

            zpozdeniF.WriteRow(row);
        }
    }
}
