using System.Globalization;
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
/// Nastupistia a kolaje (Pozice_A.txt).
/// </summary>
internal static class TracksFile
{
    /// <summary>
    /// Nainicializuje informacie o nastupistiach a kolajach nachadzajucich sa na stanici
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="logicals">logicke tabule grafikonu - kolaje sa na ne odkazuju</param>
    /// <param name="warnings">zberac varovani pri nacitani</param>
    public static List<Track> Read(string path, IEnumerable<TableLogical> logicals, LoadWarnings warnings)
    {
        var file = CombinePath(path, FILE_POZICE_A)!;

        var tracks = new List<Track>();

        using var poziceAF = new CsvFileReader(file);
        var riadok = 1;
        var row = new CsvRow();
        while (true)
        {
            var status = poziceAF.ReadRow(row);
            if (LineIsEmpty(status))
            {
                riadok++;
                continue;
            }

            if (LineIsEOF(status))
                break;

            try
            {
                var track = new Track
                {
                    Key = row[0],
                    Name = row[1],
                    FullName = row[2].ANSItoUTF(),
                    TrackName = row[4],
                    SoundName = row[6],
                    Platform = new Platform(row[5], row[3].ANSItoUTF(), row[7])
                };

                var tableCount = ParseIntOrDefault(row[8]);
                for (var i = 0; i < tableCount; i++)
                    foreach (var table in logicals)
                        if (table.Key == row[i + 9])
                        {
                            track.Tables.Add(table);
                            // za klucmi tabul nasleduje rovnaky pocet priorit
                            track.TablePriorities[table.Key] = ParseIntOrDefault(row.ElementAtOrDefault(9 + tableCount + i));
                            break;
                        }

                // dve nepovinne textove polia za prioritami (FILL_SECTION 30-33)
                track.PlatformTrackText = row.ElementAtOrDefaultStr(9 + 2 * tableCount).ANSItoUTF();
                track.AltTrackText = row.ElementAtOrDefaultStr(10 + 2 * tableCount).ANSItoUTF();

                tracks.Add(track);
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, FORMAT_EX, FILE_POZICE_A, riadok) + e.Message, e);
            }

            riadok++;
        }

        // riadok s klucom N (neznama kolaj) zastupuje Track.None - v zozname je vzdy prvy a zapisuje sa naspat
        tracks.RemoveAll(track => track.Key == Track.None.Key);
        tracks.Insert(0, Track.None);

        ShareTrackPlatforms(tracks, warnings);

        return tracks;
    }

    /// <summary>
    /// Kazdy riadok Pozice_A.txt nesie vlastnu kopiu udajov nastupista. Kolaje s rovnakym klucom nastupista musia
    /// zdielat jednu instanciu <see cref="Platform" />, inak by sa uprava nastupista (Lokalne nastavenia) prejavila
    /// len na jednej z nich. Ked sa udaje medzi riadkami lisia, pouzije sa najcastejsia varianta (pri zhode prva)
    /// a do <see cref="LoadWarnings" /> sa zapise upozornenie. Nastupiste s klucom N je vzdy <see cref="Platform.None" />.
    /// </summary>
    /// <param name="tracks">nacitane kolaje</param>
    /// <param name="warnings">zberac varovani pri nacitani</param>
    private static void ShareTrackPlatforms(List<Track> tracks, LoadWarnings warnings)
    {
        foreach (var group in tracks.GroupBy(track => track.Platform.Key, StringComparer.Ordinal))
        {
            // GroupBy zachovava poradie prveho vyskytu a OrderByDescending je stabilne
            var variants = group.GroupBy(track => (track.Platform.FullName, track.Platform.SoundName))
                .OrderByDescending(variant => variant.Count()).ToList();
            var shared = group.Key == Platform.None.Key ? Platform.None : variants[0].First().Platform;

            if (variants.Any(variant => variant.Key != (shared.FullName, shared.SoundName)))
            {
                var descriptions = variants.Select(variant =>
                    $"„{variant.Key.FullName}“/{variant.Key.SoundName} ({string.Join(", ", variant.Select(track => track.Key))})");
                warnings.Add(string.Format(CultureInfo.CurrentCulture, Resources.TracksFile_PlatformDiffers, FILE_POZICE_A, group.Key, string.Join("; ", descriptions)) +
                                 string.Format(CultureInfo.CurrentCulture, Resources.TracksFile_PlatformShared, shared.FullName, shared.SoundName));
            }

            foreach (var track in group)
                track.Platform = shared;
        }
    }

    /// <summary>
    /// Zapise informacie o nastupistiach a kolajach nachadzajucich sa na stanici/zastavke
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="tracks">kolaje</param>
    public static void Write(string path, IEnumerable<Track> tracks)
    {
        var file = CombinePath(path, FILE_POZICE_A)!;

        using var poziceAF = new CsvFileWriter(file);

        var allTracks = tracks.ToList();
        if (!allTracks.Contains(Track.None))
            allTracks.Insert(0, Track.None);

        foreach (var track in allTracks)
        {
            var tabcount = track.Tables.Count;
            var row = new CsvRow(9 + 2 * tabcount)
            {
                track.Key.Quote().UTFtoANSI(),
                track.Name.Quote().UTFtoANSI(),
                track.FullName.Quote().UTFtoANSI(),
                track.Platform.FullName.Quote().UTFtoANSI(),
                track.TrackName.Quote().UTFtoANSI(),
                track.Platform.Key.Quote().UTFtoANSI(),
                track.SoundName.Quote().UTFtoANSI(),
                track.Platform.SoundName.Quote().UTFtoANSI(),
                track.Tables.Count.ToString(CultureInfo.InvariantCulture)
            };

            // najprv vsetky kluce tabul, az potom vsetky priority
            for (var i = 0; i < tabcount; i++)
                row.Add(track.Tables[i].Key.Quote().UTFtoANSI());
            for (var i = 0; i < tabcount; i++)
                row.Add(track.TablePriorities.TryGetValue(track.Tables[i].Key, out var priority) ? priority.ToString(CultureInfo.InvariantCulture) : "0");

            // nepovinne texty na konci sa zapisu, len ak je aspon jeden vyplneny
            if (!string.IsNullOrEmpty(track.PlatformTrackText) || !string.IsNullOrEmpty(track.AltTrackText))
            {
                row.Add(track.PlatformTrackText.Quote().UTFtoANSI());
                row.Add(track.AltTrackText.Quote().UTFtoANSI());
            }

            poziceAF.WriteRow(row);
        }
    }
}
