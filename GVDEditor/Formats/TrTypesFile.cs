using System.Globalization;
using System.Text.RegularExpressions;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Iniss.Tools;
using static GVDEditor.Formats.FormatCommon;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Iniss.Tools.ParseUtils;
using static ToolsCore.Iniss.Tools.PathUtils;

namespace GVDEditor.Formats;

/// <summary>
/// Druhy vlakov (TrTypes.txt).
/// </summary>
internal static class TrTypesFile
{
    /// <summary>
    /// Vrati kategorie vlakov.
    /// </summary>
    /// <returns>kategorie vlakov</returns>
    /// <param name="dataDir">priecinok DATA instalacie INISS</param>
    public static List<TrainType> Read(string dataDir)
    {
        var fileTrTypes = CombinePath(dataDir, FILE_TRTYPES)!;

        var typy = new List<TrainType>();

        // bez suboru INISS druhy vlakov z Export3A dalej prijima podla zabudovanej tabulky (len ich neponuka v dialogu);
        // GVDEditor preto ponukne celu zabudovanu tabulku, aby sa taky grafikon dal otvorit a upravovat
        if (!File.Exists(fileTrTypes))
        {
            LoadWarnings.Add(string.Format(CultureInfo.InvariantCulture, Properties.Resources.TxtParser_TrTypes_chyba_pouzite_zabudovane, fileTrTypes));
            return TrainType.GetDefaultValues();
        }

        using var trtypesF = new CsvFileReader(fileTrTypes);
        var riadok = 1;
        var row = new CsvRow();
        while (true)
        {
            var status = trtypesF.ReadRow(row);
            if (LineIsEmpty(status))
            {
                riadok++;
                continue;
            }

            if (LineIsEOF(status))
                break;

            try
            {
                TrainType typ;

                var s = row[0];

                if (Regex.IsMatch(s, "^X[1-9]$") || Regex.IsMatch(s, "^R[1-9]$") || Regex.IsMatch(s, "^Os[1-9]$") ||
                    Regex.IsMatch(s, "^Sl[1-9]$"))
                {
                    typ = new TrainType(s, row[1], row[2]);
                }
                else
                {
                    if (TrainType.Validate(s))
                    {
                        // pri dvoch poliach INISS pouzije ako text na tabuli druhy stlpec (kluc), nie prvy
                        var key = row.Count > 1 ? row[1] : s;
                        typ = new TrainType(s)
                        {
                            Key = key,
                            TextInTable = row.Count > 2 ? row[2] : key
                        };
                    }
                    else
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.TrTypes_Unknown, s));
                }

                typy.Add(typ);
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, FORMAT_EX, FILE_TRTYPES, riadok) + e.Message, e);
            }

            riadok++;
        }

        return typy;
    }

    /// <summary>
    /// Zapise do suboru kategorie vlakov.
    /// </summary>
    /// <param name="typy">kategorie vlakov</param>
    /// <param name="dataDir">priecinok DATA instalacie INISS</param>
    public static void Write(string dataDir, IEnumerable<TrainType> typy)
    {
        var fileTrTypes = CombinePath(dataDir, FILE_TRTYPES)!;

        using var trtypesF = new CsvFileWriter(fileTrTypes);
        foreach (var typ in typy)
        {
            var row = new CsvRow();
            if (typ.IsCustom)
            {
                row.Add(typ.CategoryTrain);
                row.Add(typ.Key);
                row.Add(typ.TextInTable);
            }
            else
            {
                row.Add(typ.CategoryTrain);
                if (typ.Key != typ.CategoryTrain || typ.Key != typ.TextInTable)
                {
                    row.Add(typ.Key);
                    row.Add(typ.TextInTable);
                }
                else if (typ.Key != typ.CategoryTrain) 
                    row.Add(typ.Key);
            }

            trtypesF.WriteRow(row);
        }
    }

    /// <summary>
    /// Zapise do suboru predvolene kategorie vlakov.
    /// </summary>
    /// <param name="dataDir">priecinok DATA instalacie INISS</param>
    public static void WriteDefaults(string dataDir)
    {
        var file = CombinePath(dataDir, FILE_TRTYPES)!;

        string[] types = { "Os", "Zr", "R", "Ex", "EC", "IC", "EN", "ER", "REX", "Bus", "SC" };

        using var trtypes = new CsvFileWriter(file);
        foreach (var typ in types)
        {
            var row = new CsvRow { typ };
            trtypes.WriteRow(row);
        }
    }
}
