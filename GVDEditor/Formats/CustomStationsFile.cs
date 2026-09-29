using GVDEditor.Domain.Entities;
using System.Globalization;
using ToolsCore.Tools;
using ToolsCore.XML;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Tools.Utils;
using static GVDEditor.Formats.FormatCommon;

namespace GVDEditor.Formats;

/// <summary>
/// Stanice definovane v grafikone (Stanice.txt).
/// </summary>
internal static class CustomStationsFile
{
    /// <summary>
    /// Nainicializuje vsetky pouzivatelom-definovane stanice, ktore sa nenachadzaju v zvukovej banke
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="gvd">informacie o grafikone</param>
    /// <returns>vlastne stanice</returns>
    /// <param name="bankStations">stanice zo zvukovej banky</param>
    public static List<Station> Read(string path, GVDInfo gvd, IEnumerable<Station> bankStations)
    {
        var fileStanice = CombinePath(path, FILE_STANICE)!;

        var stanice = new List<Station>();

        using var staniceF = new CsvFileReader(fileStanice);
        var riadok = 1;
        var row = new CsvRow();
        while (true)
        {
            var status = staniceF.ReadRow(row);
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
                var name = row[1].ANSItoUTF();
                var stanica = new Station(id.ToString(CultureInfo.InvariantCulture), name);

                if (!Station.ContainsName(bankStations, stanica.Name))
                {
                    if (!stanica.EqualsName(gvd.ThisStation.Name))
                    {
                        stanica.IsCustom = true;
                        stanice.Add(stanica);
                    }
                    else
                        gvd.ThisStation.IsCustom = true;
                }
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, FORMAT_EX, FILE_STANICE, riadok) + e.Message, e);
            }

            riadok++;
        }

        return stanice;
    }

    /// <summary>
    /// Zapise vsetky pouzivatelom definovane stanice do STANICE.TXT
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="cstations">zoznam definovanych stanic</param>
    /// <param name="gvd">informacie o grafikone</param>
    /// <param name="language">jazyk komentarov v hlavicke suboru</param>
    public static void Write(string path, IEnumerable<Station> cstations, GVDInfo gvd, AppLanguage language)
    {
        var fileStations = CombinePath(path, FILE_STANICE)!;

        using var staniceF = new CsvFileWriter(fileStations);

        var comments = FormatCommon.GenerateComment(path, FILE_STANICE, gvd, language);
        foreach (var comment in comments) staniceF.WriteComment(comment);

        foreach (var cs in cstations)
        {
            var row = new CsvRow(2)
            {
                cs.ID,
                cs.Name.UTFtoANSI().Quote()
            };

            staniceF.WriteRow(row);
        }
    }
}
