using System.Globalization;
using GVDEditor.Entities;
using GVDEditor.Properties;

namespace GVDEditor.Tools;

/// <summary>
///     Kontrola radenia vlaku (zalozka Radenie v okne vlaku).
/// </summary>
internal static class RadenieRules
{
    /// <summary>
    ///     Pole radenia, ku ktoremu sa chyba viaze.
    /// </summary>
    public enum Field
    {
        Validity,
        DateLimit,
        Sounds
    }

    /// <summary>
    ///     Vsetky chyby radenia <paramref name="radenie" /> voci radeniam vlaku. Radenie bez obdobia platnosti
    ///     (<see cref="Radenie.HasValidity" />) plati cely grafikon - obdobie ani prekrytie sa pri nom nekontroluju.
    /// </summary>
    /// <param name="radenie">kontrolovane radenie (hodnoty z okna)</param>
    /// <param name="radenia">radenia vlaku</param>
    /// <param name="index">poradie radenia v <paramref name="radenia" /> (porovna sa s radeniami pred nim); -1 pri novom</param>
    public static List<(Field Field, string Message)> Check(Radenie radenie, IReadOnlyList<Radenie> radenia, int index)
    {
        var problems = new List<(Field, string)>();

        if (radenie.HasValidity)
        {
            if (radenie.KonPlatnosti.Date <= radenie.ZacPlatnosti.Date)
            {
                problems.Add((Field.Validity, Resources.FEditTrain_Začiatok_platnosti_radenia_je_neskôr_ako_jeho_koniec));
            }
            else if (CheckDateLimit(radenie) is { } limit)
            {
                problems.Add((Field.DateLimit, limit));
            }
            else if (FindOverlap(radenie, radenia, index) is { } days)
            {
                problems.Add((Field.DateLimit, string.Format(CultureInfo.CurrentCulture,
                    Resources.FEditTrain_Zadané_dátumové_obmedzenie_radenia_sa_prekrýva_s_iným_v_období, days)));
            }
        }

        if (radenie.Sounds is not { Count: > 0 })
            problems.Add((Field.Sounds, Resources.FEditTrain_Nebolo_zadané_radenie_vlaku));

        return problems;
    }

    private static string? CheckDateLimit(Radenie radenie)
    {
        try
        {
            new DateLimit(radenie.ZacPlatnosti.Date, radenie.KonPlatnosti.Date).TextToBitArray(radenie.DatObm ?? "");
            return null;
        }
        catch (Exception ex)
        {
            return Resources.FEditTrain_DateRem_radenia_obsahuje_chybu + ex.Message;
        }
    }

    /// <summary>
    ///     Spolocne dni s inym radenim s rovnakym obdobim a cielovou stanicou - INISS by nevedel, ktore pouzit.
    /// </summary>
    private static string? FindOverlap(Radenie radenie, IReadOnlyList<Radenie> radenia, int index)
    {
        for (var i = 0; i < radenia.Count; i++)
        {
            var other = radenia[i];
            // dvojica sa hlasi len pri neskorsom radeni - pridane radenie neoznaci chybou radenie, ktore uz bolo v poriadku
            if (i == index || (index >= 0 && i > index) || other.ZacPlatnosti.Date != radenie.ZacPlatnosti.Date ||
                other.KonPlatnosti.Date != radenie.KonPlatnosti.Date || other.DestStation?.ID != radenie.DestStation?.ID)
                continue;

            var limit = new DateLimit(other.ZacPlatnosti.Date, other.KonPlatnosti.Date, insertMarks: false);
            try
            {
                if (limit.Overlap(other.DatObm ?? "", radenie.DatObm ?? ""))
                    return limit.TextAnd(other.DatObm ?? "", radenie.DatObm ?? "");
            }
            catch (DateLimit.ParseException)
            {
                // obmedzenie ineho radenia sa neda precitat - prekrytie sa neda zistit
            }
        }

        return null;
    }
}
