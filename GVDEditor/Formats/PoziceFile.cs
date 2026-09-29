using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Iniss.Tools;
using static GVDEditor.Formats.FormatCommon;
using static GVDEditor.Formats.GvdFileConsts;

namespace GVDEditor.Formats;

/// <summary>
/// Kolaje vlakov (Pozice.txt): kolaj prichodu a pripadne kolaj odchodu.
/// </summary>
internal static class PoziceFile
{
    /// <summary>
    /// Nacita Pozice.txt - priradi vlakom kolaj prichodu a pripadne kolaj odchodu.
    /// </summary>
    /// <param name="file">cesta k suboru Pozice.txt</param>
    /// <param name="trains">vlaky v poradi podla ID (riadok s ID n patri vlaku na indexe n - 1)</param>
    /// <param name="tracks">kolaje stanice (Pozice_A.txt)</param>
    /// <exception cref="FormatException">riadok odkazuje na neexistujucu kolaj alebo vlak</exception>
    public static void Read(string file, IList<Train> trains, IEnumerable<Track> tracks) =>
        ReadRows(file, FilePozice, (row, _) =>
        {
            var train = trains[int.Parse(row[0], CultureInfo.InvariantCulture) - 1];
            var track = Track.GetFromID(tracks, row[1]) ?? throw new FormatException(string.Format(CultureInfo.CurrentCulture, Resources.Pozice_TrackMissing, row[1]));
            train.Track = track;

            // nepovinne tretie pole: kolaj pri odchode, ak vlak v stanici prechadza na inu kolaj
            var departureKey = row.ElementAtOrDefault(2);
            if (!string.IsNullOrEmpty(departureKey))
            {
                var departure = Track.GetFromID(tracks, departureKey)
                                ?? throw new FormatException(string.Format(CultureInfo.CurrentCulture, Resources.Pozice_DepartureTrackMissing, departureKey));
                train.TrackDeparture = departure.EqualsKeys(track) ? null : departure;
            }
        });

    /// <summary>
    /// Zapise Pozice.txt - kolaj prichodu a (ak sa lisi) kolaj odchodu kazdeho vlaku.
    /// </summary>
    /// <param name="file">cesta k suboru Pozice.txt</param>
    /// <param name="trains">vlaky v poradi podla ID</param>
    /// <param name="comments">komentare na zaciatok suboru</param>
    public static void Write(string file, IEnumerable<Train> trains, IEnumerable<string> comments) =>
        WriteRows(file, comments, trains.Select((train, index) =>
        {
            var row = new CsvRow { Export3File.Id(index), train.Track.Key.Quote() };
            if (train.TrackDeparture != null && !train.TrackDeparture.EqualsKeys(train.Track))
                row.Add(train.TrackDeparture.Key.Quote());
            return row;
        }));
}
