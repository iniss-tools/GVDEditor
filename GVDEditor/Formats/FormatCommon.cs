using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Iniss.Tools;
using ToolsCore.XML;
using static ToolsCore.Iniss.Tools.ParseUtils;
using static ToolsCore.Iniss.Tools.PathUtils;

namespace GVDEditor.Formats;

/// <summary>
/// Spolocne pomocne funkcie formatov INISS (hlavicka suborov, chybove hlasky).
/// </summary>
internal static class FormatCommon
{
    internal static string FormatEx => Resources.FormatCommon_Error;
    internal static string FormatExArea => Resources.FormatCommon_UndefinedField;

    /// <summary>
    /// Precita CSV subor INISS po riadkoch. Prazdne riadky a komentare preskoci; chybu riadka obali
    /// <see cref="FormatException" /> s nazvom suboru a cislom riadka.
    /// </summary>
    /// <param name="file">cesta k suboru</param>
    /// <param name="fileName">nazov suboru do chybovej hlasky</param>
    /// <param name="readRow">spracovanie riadka (riadok, cislo riadka od 1)</param>
    internal static void ReadRows(string file, string fileName, Action<CsvRow, int> readRow)
    {
        using var reader = new CsvFileReader(file);
        var rowNumber = 1;
        var row = new CsvRow();
        while (true)
        {
            var status = reader.ReadRow(row);
            if (LineIsEmpty(status))
            {
                rowNumber++;
                continue;
            }

            if (LineIsEof(status))
                break;

            try
            {
                readRow(row, rowNumber);
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, FormatEx, fileName, rowNumber) + e.Message, e);
            }

            rowNumber++;
        }
    }

    /// <summary>
    /// Zapise CSV subor INISS: najprv komentare hlavicky, potom riadky.
    /// </summary>
    /// <param name="file">cesta k suboru</param>
    /// <param name="comments">komentare na zaciatok suboru</param>
    /// <param name="rows">riadky suboru</param>
    internal static void WriteRows(string file, IEnumerable<string> comments, IEnumerable<CsvRow> rows)
    {
        using var writer = new CsvFileWriter(file);
        foreach (var comment in comments)
            writer.WriteComment(comment);
        foreach (var row in rows)
            writer.WriteRow(row);
    }

    /// <summary>
    /// Vygeneruje komentar ako hlavicku k niektorym suborom.
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="filename">nazov suboru</param>
    /// <param name="gvd">informacie o grafikone</param>
    /// <param name="lang">jazyk generovania komentara</param>
    /// <param name="toAnsi">true (default) ak ma pouyit ANSI kodovanie, false pre pouzitie UTF-8</param>
    /// <returns></returns>
    internal static IEnumerable<string> GenerateComment(string path, string filename, GVDInfo gvd, AppLanguage lang, bool toAnsi = true)
    {
        var rows = new List<string>();

        switch (lang)
        {
            case AppLanguage.Slovak:
                rows.Add(CombinePath(path, filename)!);
                rows.Add(
                    string.Create(CultureInfo.InvariantCulture, $"Vygenerované programom {Application.ProductName}, verzia {Application.ProductVersion} dňa {DateTime.Today:dd.MM.yyyy} v {DateTime.Now:HH:mm}"));
                rows.Add($"Názov stanice: {gvd.ThisStation.Name} ({gvd.ThisStation.ID})");
                rows.Add(string.Create(CultureInfo.InvariantCulture, $"Platnosť grafikonu: {gvd.StartValidTimeTable:dd.MM.yyyy} - {gvd.EndValidTimeTable:dd.MM.yyyy}"));
                rows.Add(string.Create(CultureInfo.InvariantCulture, $"Platnosť dát: {gvd.StartValidData:dd.MM.yyyy} - {gvd.EndValidData:dd.MM.yyyy}"));
                rows.Add(string.Create(CultureInfo.InvariantCulture, $"Dáta vytvorené: {gvd.CreateData:dd.MM.yyyy}"));
                rows.Add("=============================================================================================");
                break;
            case AppLanguage.Czech:
                rows.Add(CombinePath(path, filename)!);
                rows.Add(
                    string.Create(CultureInfo.InvariantCulture, $"Vygenerované programem {Application.ProductName}, verze {Application.ProductVersion} dne {DateTime.Today:dd.MM.yyyy} v {DateTime.Now:HH:mm}"));
                rows.Add($"Název stanice: {gvd.ThisStation.Name} ({gvd.ThisStation.ID})");
                rows.Add(string.Create(CultureInfo.InvariantCulture, $"Platnost grafikonu: {gvd.StartValidTimeTable:dd.MM.yyyy} - {gvd.EndValidTimeTable:dd.MM.yyyy}"));
                rows.Add(string.Create(CultureInfo.InvariantCulture, $"Platnost dat: {gvd.StartValidData:dd.MM.yyyy} - {gvd.EndValidData:dd.MM.yyyy}"));
                rows.Add(string.Create(CultureInfo.InvariantCulture, $"Data vytvořené: {gvd.CreateData:dd.MM.yyyy}"));
                rows.Add("=============================================================================================");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(lang), lang, null);
        }


        if (toAnsi)
            for (var i = 0; i < rows.Count; i++)
                rows[i] = rows[i].UTFtoANSI();

        return rows;
    }
}
