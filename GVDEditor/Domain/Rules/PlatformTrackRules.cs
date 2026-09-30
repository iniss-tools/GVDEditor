using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Rules;

/// <summary>
/// Pravidla nastupist a kolaji stanice (Pozice_A.txt): povinne polia a jedinecne oznacenie - vlaky sa na kolaj
/// odkazuju jej oznacenim.
/// </summary>
internal static class PlatformTrackRules
{
    /// <summary>
    /// Pole nastupista alebo kolaje, ktoreho sa chyba tyka.
    /// </summary>
    public enum Field
    {
        Key,
        Name,
        FullName,
        TrackName,
        SoundName
    }

    /// <summary>
    /// Prva chyba nastupista na pozicii <paramref name="index" />, alebo <see langword="null" />.
    /// </summary>
    public static (Field Field, string Message)? CheckPlatform(IReadOnlyList<Platform> platforms, int index)
    {
        var platform = platforms[index];
        var key = platform.Key.Trim();
        if (key.Length == 0)
            return (Field.Key, Resources.PlatformRules_Oznacenie);
        for (var i = 0; i < platforms.Count; i++)
            if (i != index && platforms[i].Key.Trim() == key)
                return (Field.Key, Resources.FLocalSettings_Zadaný_kľúč_nástupišťa_už_existuje);
        if (string.IsNullOrWhiteSpace(platform.FullName))
            return (Field.FullName, Resources.PlatformRules_Nazov);
        if (string.IsNullOrWhiteSpace(platform.SoundName))
            return (Field.SoundName, Resources.PlatformRules_Zvuk);
        return null;
    }

    /// <summary>
    /// Prva chyba kolaje na pozicii <paramref name="index" />, alebo <see langword="null" />.
    /// </summary>
    public static (Field Field, string Message)? CheckTrack(IReadOnlyList<Track> tracks, int index)
    {
        var track = tracks[index];
        var key = (track.Key ?? "").Trim();
        if (key.Length == 0)
            return (Field.Key, Resources.TrackRules_Oznacenie);
        for (var i = 0; i < tracks.Count; i++)
            if (i != index && (tracks[i].Key ?? "").Trim() == key)
                return (Field.Key, Resources.FLocalSettings_Zadaný_kľúč_koľaje_už_existuje);
        if (string.IsNullOrWhiteSpace(track.Name))
            return (Field.Name, Resources.TrackRules_Kratky_nazov);
        if (string.IsNullOrWhiteSpace(track.FullName))
            return (Field.FullName, Resources.TrackRules_Nazov);
        if (string.IsNullOrWhiteSpace(track.TrackName))
            return (Field.TrackName, Resources.TrackRules_Text);
        if (string.IsNullOrWhiteSpace(track.SoundName))
            return (Field.SoundName, Resources.TrackRules_Zvuk);
        return null;
    }

    /// <summary>
    /// Oznacenie noveho nastupista alebo kolaje - o jedno vyssie cislo nez najvyssie ciselne oznacenie.
    /// </summary>
    public static string SuggestKey(IEnumerable<string?> keys)
    {
        var max = 0;
        foreach (var key in keys)
            if (int.TryParse(key, NumberStyles.None, CultureInfo.InvariantCulture, out var number) && number > max)
                max = number;
        return (max + 1).ToString(CultureInfo.InvariantCulture);
    }
}
