using GVDEditor.Entities;

namespace GVDEditor.Tools;

/// <summary>
///     Upravy ciselnika kolaji a nastupist (Lokalne nastavenia), ktore musia ostat v sulade s vlakmi - Pozice.txt
///     odkazuje na kolaje klucom a Pozice_A.txt uklada nastupistia len spolu s kolajami.
/// </summary>
internal static class TrackEditing
{
    /// <summary>
    ///     Kolko vlakov pouziva kolaj ako kolaj prichodu a kolko ako (odlisnu) kolaj odchodu.
    /// </summary>
    /// <param name="track">kolaj</param>
    /// <param name="trains">vlaky grafikonu</param>
    public static (int Arrival, int Departure) CountUsage(Track track, IEnumerable<Train> trains)
    {
        int arrival = 0, departure = 0;
        foreach (var train in trains)
        {
            if (train.Track?.EqualsKeys(track) == true)
                arrival++;
            if (train.TrackDeparture?.EqualsKeys(track) == true)
                departure++;
        }

        return (arrival, departure);
    }

    /// <summary>
    ///     Odstrani kolaj zo zoznamu. Vlaky, ktore na nej stali, presunie na <see cref="Track.None" />; vlakom, ktore z
    ///     nej odchadzali, zrusi kolaj odchodu (odchadzaju z kolaje prichodu). Inak by Pozice.txt odkazoval na
    ///     neexistujucu kolaj a grafikon by sa uz nenacital.
    /// </summary>
    /// <param name="track">odstranovana kolaj</param>
    /// <param name="tracks">zoznam kolaji</param>
    /// <param name="trains">vlaky grafikonu</param>
    public static void Remove(Track track, IList<Track> tracks, IEnumerable<Train> trains)
    {
        foreach (var train in trains)
        {
            if (train.Track?.EqualsKeys(track) == true)
                train.Track = Track.None;

            // kolaj odchodu zhodna s kolajou prichodu sa v Pozice.txt nezapisuje
            if (train.TrackDeparture != null &&
                (train.TrackDeparture.EqualsKeys(track) || (train.Track != null && train.TrackDeparture.EqualsKeys(train.Track))))
                train.TrackDeparture = null;
        }

        tracks.Remove(track);
    }

    /// <summary>
    ///     Nastupistia, na ktorych nelezi ziadna kolaj. Pozice_A.txt nema riadky nastupist, takze sa take nastupiste
    ///     nezapise a po opatovnom otvoreni grafikonu zmizne.
    /// </summary>
    /// <param name="platforms">zoznam nastupist</param>
    /// <param name="tracks">zoznam kolaji</param>
    public static List<Platform> PlatformsWithoutTracks(IEnumerable<Platform> platforms, IEnumerable<Track> tracks)
    {
        var used = tracks.Select(track => track.Platform).Where(platform => platform != null).ToList();
        return platforms.Where(platform => !used.Any(u => u.EqualsKeys(platform))).ToList();
    }
}
