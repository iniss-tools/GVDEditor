using GVDEditor.Domain.Entities;
using System.Globalization;
using ToolsCore.Iniss.Tools;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Iniss.Tools.ParseUtils;
using static ToolsCore.Iniss.Tools.PathUtils;
using static GVDEditor.Formats.FormatCommon;

namespace GVDEditor.Formats;

/// <summary>
/// Dopravcovia (Vlastnik.txt).
/// </summary>
internal static class OperatorsFile
{
    /// <summary>
    /// Nainicializuje informacie o dopravcoch
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    public static List<Operator> Read(string path)
    {
        var file = CombinePath(path, FILE_VLASTNIK)!;

        var operators = new List<Operator> { Operator.None };

        try
        {
            using var vlastnikF = new CsvFileReader(file);
            var riadok = 1;
            var row = new CsvRow();
            while (true)
            {
                var status = vlastnikF.ReadRow(row);
                if (LineIsEmpty(status))
                {
                    riadok++;
                    continue;
                }

                if (LineIsEOF(status))
                    break;

                try
                {
                    var id = int.Parse(row[0], CultureInfo.InvariantCulture);
                    var nazov = row[1].ANSItoUTF();
                    // rovnaky riadok dvakrat (aj riadok s predvolenym dopravcom) sa nacita len raz
                    if (!operators.Any(o => o.Id == id && o.Name == nazov))
                        operators.Add(new Operator(id, nazov));
                }
                catch (Exception e)
                {
                    throw new FormatException(string.Format(CultureInfo.InvariantCulture, FORMAT_EX, FILE_VLASTNIK, riadok) + e.Message, e);
                }

                riadok++;
            }
        }
        catch (FileNotFoundException)
        {
            //ignored
        }

        return operators;
    }

    /// <summary>
    /// Zapise informacie o dopravcoch
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="operators">dopravcovia</param>
    public static void Write(string path, IEnumerable<Operator> operators)
    {
        var file = CombinePath(path, FILE_VLASTNIK)!;

        using var vlastnikF = new CsvFileWriter(file);
        foreach (var operatorV in operators)
            if (operatorV != Operator.None)
            {
                var row = new CsvRow(2)
                {
                    operatorV.Id.ToString(CultureInfo.InvariantCulture),
                    operatorV.Name.Quote().UTFtoANSI()
                };

                vlastnikF.WriteRow(row);
            }
    }
}
