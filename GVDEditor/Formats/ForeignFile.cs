using System.Globalization;
using GVDEditor.Domain.Entities;
using ToolsCore.Entities;
using ToolsCore.Tools;
using static GVDEditor.Formats.FormatCommon;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Tools.Utils;

namespace GVDEditor.Formats;

/// <summary>
/// Dalsie jazyky hlasenia vlakov (Foreign.txt).
/// </summary>
internal static class ForeignFile
{
    /// <summary>
    /// Nacita Foreign.txt (nepovinny). Pole s klucom jazyka zapne ten jazyk, cislo 1 vsetky jazyky stanice.
    /// </summary>
    /// <param name="file">cesta k suboru</param>
    /// <param name="trains">vlaky v poradi podla ID</param>
    /// <param name="languages">jazyky stanice</param>
    public static void Read(string file, IList<Train> trains, IList<FyzLanguage> languages)
    {
        try
        {
            ReadRows(file, FILE_FOREIGN, (row, _) =>
            {
                var train = trains[int.Parse(row[0], CultureInfo.InvariantCulture) - 1];

                // INISS spracuje vsetky polia za indexom - vlak moze mat viac jazykov naraz
                for (var k = 1; k < row.Count; k++)
                {
                    var field = row[k];
                    if (string.IsNullOrWhiteSpace(field))
                        continue;

                    if (!IsInt(field))
                    {
                        var language = FyzLanguage.GetLanguageFromKey(languages, field);
                        if (language != null && !train.Languages.Contains(language))
                            train.Languages.Add(language);
                    }
                    else if (int.Parse(field, CultureInfo.InvariantCulture) == 1)
                    {
                        foreach (var language in languages)
                            if (!train.Languages.Contains(language))
                                train.Languages.Add(language);
                    }
                }
            });
        }
        catch (FileNotFoundException)
        {
            // grafikon bez dalsich jazykov
        }
    }

    /// <summary>
    /// Zapise Foreign.txt: -1 = len hlavny jazyk, 1 = vsetky jazyky stanice, inak kluce dalsich jazykov.
    /// </summary>
    public static void Write(string file, IEnumerable<Train> trains, IList<FyzLanguage> languages, IEnumerable<string> comments) =>
        WriteRows(file, comments, trains.Select((train, index) =>
        {
            var row = new CsvRow { Export3File.Id(index) };
            if (train.Languages.Count == 0)
                row.Add("-1");
            else if (train.Languages.Count == languages.Count)
                row.Add("1");
            else
                row.AddRange(train.Languages.Where(language => !language.IsBasic).Select(language => language.Key.Quote()));
            return row;
        }));
}
