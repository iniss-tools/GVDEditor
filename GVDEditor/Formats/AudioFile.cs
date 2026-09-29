using GVDEditor.Domain.Entities;
using System.Globalization;
using ToolsCore.Iniss.Tools;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Iniss.Tools.ParseUtils;
using static ToolsCore.Iniss.Tools.PathUtils;
using static GVDEditor.Formats.FormatCommon;

namespace GVDEditor.Formats;

/// <summary>
/// Zvukove okruhy stanice (Audio.txt).
/// </summary>
internal static class AudioFile
{
    /// <summary>
    /// Vrati informacie o audio linkach zo suboru.
    /// </summary>
    /// <returns>audio linky</returns>
    /// <param name="dataDir">priecinok DATA instalacie INISS</param>
    /// <param name="stations">stanice zvukovej banky</param>
    /// <param name="trailer">riadky od prveho riadka zacinajuceho '/' - INISS ich ako okruhy necita, zapisu sa spat bez zmeny</param>
    public static List<Audio> Read(string dataDir, IEnumerable<Station> stations, out List<string> trailer)
    {
        var fileAudio = CombinePath(dataDir, FILE_AUDIO)!;

        var audios = new List<Audio>();
        trailer = [];

        using var audioF = new CsvFileReader(fileAudio);
        var riadok = 1;
        var row = new CsvRow();
        while (true)
        {
            var status = audioF.ReadRow(row);

            // INISS okruhy cita len po prvy riadok zacinajuci '/' - zvysok suboru sa nesmie stat okruhmi
            if (status == ReadStartChar.Slash)
            {
                trailer.Add(row.LineText!);
                while (audioF.ReadLine() is { } line)
                    trailer.Add(line);
                break;
            }

            if (LineIsEmpty(status))
            {
                riadok++;
                continue;
            }

            if (LineIsEOF(status))
                break;

            try
            {
                var audio = new Audio
                {
                    Station = Station.GetFromID(row[0], stations, []),
                    Name = row[1],
                    ShortName = row[2],
                    QueueName = row[3],
                    Mixer = row.ElementAtOrDefaultStr(4),
                    SoundCard = row.ElementAtOrDefaultStr(5),
                    InputLine = row.ElementAtOrDefaultStr(6),
                    AmplifierPort = row.ElementAtOrDefaultStr(7),
                    ExchangeParameter = row.ElementAtOrDefaultStr(8),
                    Node = row.ElementAtOrDefaultStr(9)
                };

                audios.Add(audio);
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, FORMAT_EX, FILE_AUDIO, riadok) + e.Message, e);
            }

            riadok++;
        }

        return audios;
    }

    /// <summary>
    /// Zapise informacie o audio linkach do suboru.
    /// </summary>
    /// <param name="audios">audio linky</param>
    /// <param name="dataDir">priecinok DATA instalacie INISS</param>
    /// <param name="trailer">riadky za okruhmi z <see cref="Read" /></param>
    public static void Write(string dataDir, IEnumerable<Audio> audios, IEnumerable<string> trailer)
    {
        WriteFile(CombinePath(dataDir, FILE_AUDIO)!, audios, trailer);
    }

    /// <summary>
    /// Zapise audio linky a za ne riadky <paramref name="trailer" /> do suboru <paramref name="fileAudio" />.
    /// </summary>
    private static void WriteFile(string fileAudio, IEnumerable<Audio> audios, IEnumerable<string> trailer)
    {
        using var audioF = new CsvFileWriter(fileAudio);
        foreach (var a in audios)
        {
            var row = new CsvRow();

            row.Insert(0, a.Station.ID);
            row.Insert(1, a.Name);
            row.Insert(2, a.ShortName);
            row.Insert(3, a.QueueName);
            row.Insert(4, ParseStringOrDefault(a.Mixer));
            row.Insert(5, ParseStringOrDefault(a.SoundCard));

            // stlpce 7-10 INISS pouziva na mixer, spinanie zosilnovaca a uzol; zapisu sa, len ak su vyplnene,
            // aby sa bezny riadok s piatimi poliami nepredlzoval
            var tail = new[]
            {
                ParseStringOrDefault(a.InputLine), ParseStringOrDefault(a.AmplifierPort),
                ParseStringOrDefault(a.ExchangeParameter), ParseStringOrDefault(a.Node)
            };
            var last = Array.FindLastIndex(tail, s => !string.IsNullOrEmpty(s));
            for (var i = 0; i <= last; i++)
                row.Insert(6 + i, tail[i]);

            audioF.WriteRow(row);
        }

        foreach (var line in trailer)
            audioF.WriteLine(line);
    }
}
