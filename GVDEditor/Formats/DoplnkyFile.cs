using System.Globalization;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using ToolsCore.Tools;
using static GVDEditor.Formats.FormatCommon;
using static GVDEditor.Formats.GvdFileConsts;

namespace GVDEditor.Formats;

/// <summary>
/// Doplnkove hlasenia vlakov (Doplnky.txt): kod doplnku a mapa typov hlaseni, pri ktorych zaznie.
/// </summary>
internal static class DoplnkyFile
{
    /// <summary>
    /// Nacita Doplnky.txt. Kod doplnku bez nahravky v skupine DODATKY zvukovej banky sa preskoci s varovanim.
    /// </summary>
    /// <param name="file">cesta k suboru (nepovinny)</param>
    /// <param name="trains">vlaky v poradi podla ID</param>
    /// <param name="context">zvuky banky, typy a varianty hlaseni grafikonu</param>
    public static void Read(string file, IList<Train> trains, GrafikonContext context)
    {
        if (!File.Exists(file))
            return;

        ReadRows(file, FILE_DOPLNKY, (row, rowNumber) =>
        {
            var id = int.Parse(row[0], CultureInfo.InvariantCulture);
            var train = trains[id - 1];

            var count = int.Parse(row[1], CultureInfo.InvariantCulture);
            if (count == -1)
                return;

            for (var i = 0; i < count; i++)
            {
                var code = row[i * 2 + 2];
                var sound = context.Workspace.Sounds.FirstOrDefault(snd =>
                    snd.Group.Key.EqualsIgnoreCase("DODATKY") && Dodatok.CodeFromKey(snd.Key).EqualsIgnoreCase(code));

                if (sound == null)
                {
                    // kod bez nahravky v banke: nezahadzujeme ho ticho, ale aspon zalogujeme
                    LoadWarnings.Add($"{FILE_DOPLNKY}, riadok {rowNumber}: doplnok {code} vlaku {train.Number} nemá zvuk v skupine DODATKY; pri uložení sa stratí.");
                    continue;
                }

                try
                {
                    train.Doplnky.Add(Dodatok.NumsToDodatok(sound, row[i * 2 + 3], context.Document.ReportTypes,
                        context.Document.ReportVariants, train.Routing));
                }
                catch (Exception e)
                {
                    throw new FormatException($"Doplnok vlaku [{id},{i}]: " + e.Message, e);
                }
            }
        });
    }

    /// <summary>
    /// Zapise Doplnky.txt. Mapa doplnku obsahuje len typy hlaseni platne pre smerovanie vlaku.
    /// </summary>
    public static void Write(string file, IEnumerable<Train> trains, GrafikonDocument document, IEnumerable<string> comments) =>
        WriteRows(file, comments, trains.Select((train, index) =>
        {
            var row = new CsvRow { Export3File.Id(index) };
            if (train.Doplnky.Count == 0)
            {
                row.Add("-1");
                return row;
            }

            row.Add(train.Doplnky.Count.ToString(CultureInfo.InvariantCulture));
            var reportTypes = train.Routing == Routing.Prechadzajuci ? document.ReportTypesP
                : train.Routing == Routing.Konciaci ? document.ReportTypesK
                : document.ReportTypesV;

            foreach (var dodatok in train.Doplnky)
                row.AddRange([dodatok.Sound.Name.Replace("D", ""), Dodatok.DodatokToNums(dodatok, reportTypes, document.ReportVariants)]);
            return row;
        }));
}
