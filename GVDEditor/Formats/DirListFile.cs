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
/// Zoznam grafikonov instalacie (DirList.txt).
/// </summary>
internal static class DirListFile
{
    /// <summary>
    /// Vrati zoznam pouzivanych priecinkov s GVD.
    /// </summary>
    /// <param name="dataDir">priecinok DATA instalacie INISS</param>
    /// <returns>priecinky s GVD</returns>
    public static List<DirList> Read(string dataDir)
    {
        var fileDirList = CombinePath(dataDir, FILE_DIRLIST)!;

        var dirs = new List<DirList>();

        // bez DirList.TXT berie INISS ako jediny GVD samotny priecinok DATA (starsi zapis s jednym grafikonom)
        if (!File.Exists(fileDirList))
        {
            if (File.Exists(CombinePath(dataDir, FILE_GRAFIKON)))
                dirs.Add(new DirList { DirName = "", FullPath = dataDir });
            return dirs;
        }

        using var dirlistF = new CsvFileReader(fileDirList);
        var riadok = 1;
        var row = new CsvRow();
        while (true)
        {
            var status = dirlistF.ReadRow(row);
            if (LineIsEmpty(status))
            {
                riadok++;
                continue;
            }

            if (LineIsEOF(status))
                break;

            try
            {
                var dirList = new DirList();
                var dirName = row[0];
                dirList.DirName = dirName;
                dirList.FullPath = dataDir + Path.DirectorySeparatorChar + dirName;

                dirList.TablePort = ParseIntOrNull(row.ElementAtOrDefault(1));
                dirList.ReportPort = ParseIntOrNull(row.ElementAtOrDefault(2));
                dirList.Flags = row.ElementAtOrDefault(3);
                dirList.BackColor = TryParseHex(row.ElementAtOrDefault(4));

                dirs.Add(dirList);
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, FORMAT_EX, FILE_DIRLIST, riadok) + e.Message, e);
            }

            riadok++;
        }

        return dirs;
    }

    /// <summary>
    /// Zapise zoznam pouzivanych priecinkoch s GVD.
    /// </summary>
    /// <param name="dataDir">priecinok DATA instalacie INISS</param>
    /// <param name="dirs">priecinky s GVD</param>
    public static void Write(string dataDir, IEnumerable<DirList> dirs)
    {
        WriteFile(CombinePath(dataDir, FILE_DIRLIST)!, dirs);
    }

    /// <summary>
    /// Zapise zoznam priecinkov s GVD do daneho suboru. Subor nezalozi ani neprepise, ked by v nom neostal
    /// ziadny riadok a zaroven existuje grafikon priamo v DATA alebo subor este neexistuje.
    /// </summary>
    /// <param name="fileDirList">cesta k DirList.TXT</param>
    /// <param name="dirs">priecinky s GVD</param>
    /// <returns>true, ak sa subor zapisal</returns>
    internal static bool WriteFile(string fileDirList, IEnumerable<DirList> dirs)
    {
        var all = dirs.ToList();
        // grafikon priamo v DATA sa v DirList.TXT zapisat neda - INISS ho vidi len vtedy, ked sa subor neda otvorit.
        // GVDEditor taky grafikon pri otvoreni ponuka presunut do vlastneho priecinka
        var toWrite = all.Where(d => !d.IsDataRoot).ToList();
        var hasDataRoot = toWrite.Count != all.Count;

        if (toWrite.Count == 0 && (hasDataRoot || !File.Exists(fileDirList)))
            return false;

        if (hasDataRoot)
            Log.Warning("DirList.TXT: grafikon priamo v priečinku DATA sa do zoznamu nezapisuje – INISS ho po zápise DirList.TXT prestane vidieť.");

        using var dirlistF = new CsvFileWriter(fileDirList);
        foreach (var dir in toWrite)
        {
            var row = new CsvRow();
            row.Insert(0, dir.DirName);
            if (dir.TablePort.HasValue && dir.TablePort != 0)
                row.Insert(1, dir.TablePort.Value.ToString(CultureInfo.InvariantCulture));
            else
                row.Insert(1, "");

            if (dir.ReportPort.HasValue && dir.ReportPort != 0)
                row.Insert(2, dir.ReportPort.Value.ToString(CultureInfo.InvariantCulture));
            else
                row.Insert(2, "");

            row.Insert(3, dir.Flags ?? "");
            row.Insert(4, dir.BackColor.HasValue ? dir.BackColor.Value.ToHex() : "");

            dirlistF.WriteRow(row);
        }

        return true;
    }
}
