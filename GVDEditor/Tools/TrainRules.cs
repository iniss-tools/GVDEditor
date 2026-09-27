using System.Globalization;
using System.Text.RegularExpressions;
using GVDEditor.Entities;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.Tools;

/// <summary>
///     Grafikon, voci ktoremu sa kontroluje upravovany vlak.
/// </summary>
/// <param name="Trains">vsetky vlaky grafikonu</param>
/// <param name="Row">riadok upravovaneho vlaku v <paramref name="Trains" />; pri novom vlaku a kopii pocet vlakov</param>
/// <param name="AutoVariant">varianty prideluje GVDEditor sam</param>
/// <param name="DisableVariantCheck">varianta jedineho vlaku nemusi byt -1</param>
/// <param name="HomeStationId">stanica grafikonu - do trasy sa nepise; <see langword="null" />, ak nie je znama</param>
internal sealed record TrainContext(IReadOnlyList<Train> Trains, int Row, bool AutoVariant, bool DisableVariantCheck,
    string? HomeStationId = null);

/// <summary>
///     Kontrola vlaku upravovaneho v okne vlaku. Chyby brania ulozeniu, upozornenia nie.
/// </summary>
internal static partial class TrainRules
{
    /// <summary>
    ///     Pole vlaku, ku ktoremu sa chyba viaze.
    /// </summary>
    public enum Field
    {
        Number,
        Type,
        Operator,
        Arrival,
        Departure,
        Route,
        Track,
        LineArrival,
        LineDeparture,
        Validity,
        DateLimit,
        Variant,
        Dodatok,
        Radenie
    }

    /// <summary>
    ///     Chyba alebo upozornenie; <paramref name="Row" /> je poradie polozky zoznamu (dodatku, radenia), inak -1.
    /// </summary>
    public sealed record Problem(Field Field, string Message, bool IsWarning = false, int Row = -1);

    /// <summary>
    ///     Vsetky chyby a upozornenia vlaku v poradi poli okna (chyby maju prednost pred upozorneniami toho isteho pola).
    /// </summary>
    public static List<Problem> Check(TrainDraft draft, TrainContext context)
    {
        var problems = new List<Problem>();

        if (CheckNumber(draft.Number) is { } number)
            problems.Add(new Problem(Field.Number, number));
        if (draft.Type is null)
            problems.Add(new Problem(Field.Type, Resources.TrainRules_Typ));
        if (draft.Operator is null)
            problems.Add(new Problem(Field.Operator, Resources.TrainRules_Dopravca));

        DateTime? arrival = null, departure = null;
        if (draft.RouteFrom.Count != 0)
        {
            if (TryParseTime(draft.ArrivalText, out var time))
                arrival = time;
            else
                problems.Add(new Problem(Field.Arrival, Resources.FEditTrain_bSave_Click_Nesprávny_formát_času_príchodu));
        }

        if (draft.RouteTo.Count != 0)
        {
            if (TryParseTime(draft.DepartureText, out var time))
                departure = time;
            else
                problems.Add(new Problem(Field.Departure, Resources.FEditTrain_bSave_Click_Nesprávny_formát_času_odchodu));
        }

        if (draft.Routing is null)
            problems.Add(new Problem(Field.Route, Resources.FEditTrain_bSave_Click_Vlak_nemá_zadanú_žiadnu_stanicu));
        else if (!string.IsNullOrEmpty(context.HomeStationId) &&
                 draft.RouteFrom.Concat(draft.RouteTo).Any(station => station.ID == context.HomeStationId))
            problems.Add(new Problem(Field.Route, Resources.TrainRules_TrasaSToutoStanicou, true));

        // odchod skor ako prichod = vlak stoji v stanici cez polnoc (odchod je nasledujuci den)
        if (arrival is { } a && departure is { } d && d.TimeOfDay < a.TimeOfDay)
            problems.Add(new Problem(Field.Departure, Resources.TrainRules_CezPolnoc, true));

        if (draft.Track is null)
            problems.Add(new Problem(Field.Track, Resources.FEditTrain_bSave_Click_Nie_je_vybratá_koľaj));
        if (CheckLine(draft.LineArrival) is { } lineArrival)
            problems.Add(new Problem(Field.LineArrival, lineArrival));
        if (CheckLine(draft.LineDeparture) is { } lineDeparture)
            problems.Add(new Problem(Field.LineDeparture, lineDeparture));

        var validPeriod = draft.ValidFrom.Date <= draft.ValidTo.Date;
        if (!validPeriod)
            problems.Add(new Problem(Field.Validity, Resources.FEditTrain_bSave_Click_Začiatok_platnosti_musí_skôr_ako_koniec_platnosti));

        var limitError = validPeriod ? CheckDateLimit(draft.DateLimitText, draft.ValidFrom, draft.ValidTo) : null;
        if (limitError != null)
            problems.Add(new Problem(Field.DateLimit, limitError));

        var others = TrainVariants.Others(draft, context);
        if (CheckVariant(draft.Variant, others, context) is { } variant)
            problems.Add(variant);

        if (validPeriod && limitError == null)
            foreach (var (other, days) in TrainVariants.Overlaps(draft, others))
                problems.Add(new Problem(Field.DateLimit, string.Format(CultureInfo.CurrentCulture, Resources.TrainRules_Prekrytie,
                    Label(other), other.Variant, days), true));

        for (var i = 0; i < draft.Doplnky.Count; i++)
            if (draft.Doplnky[i].ChosenReports.All(chosen => chosen.Variants.Count == 0))
                problems.Add(new Problem(Field.Dodatok, string.Format(CultureInfo.CurrentCulture, Resources.TrainRules_DodatokBezHlaseni,
                    draft.Doplnky[i].Name), true, i));

        var radenia = draft.Radenia.Items;
        for (var i = 0; i < radenia.Count; i++)
            foreach (var (_, message) in RadenieRules.Check(radenia[i], radenia, i))
                problems.Add(new Problem(Field.Radenie, string.Format(CultureInfo.CurrentCulture, Resources.TrainRules_Radenie, i + 1, message),
                    Row: i));

        return problems;
    }

    /// <summary>
    ///     Typ, cislo a nazov vlaku, napr. „Ex 521 Lipovan“.
    /// </summary>
    public static string Label(Train train) =>
        string.Join(" ", new[] { train.Type?.ToString(), train.Number, train.Name }.Where(part => !string.IsNullOrEmpty(part)));

    /// <summary>
    ///     Chyba cisla vlaku - prazdne alebo so znakom, ktory by rozbil riadok grafikonu.
    /// </summary>
    public static string? CheckNumber(string? number)
    {
        if (string.IsNullOrEmpty(number))
            return Resources.FEditTrain_bSave_Click_Zadajte_číslo_vlaku;

        return number.IndexOfAny([';', ' ', '\t']) >= 0 ? Resources.TrainRules_CisloZnaky : null;
    }

    /// <summary>
    ///     Chyba linky - prazdna je v poriadku (vlak bez linky), inak najviac 20 pismen bez diakritiky a cislic.
    /// </summary>
    public static string? CheckLine(string? line) =>
        string.IsNullOrEmpty(line) || LinePattern().IsMatch(line) ? null : Resources.TrainRules_Linka;

    /// <summary>
    ///     Chyba datumoveho obmedzenia v obdobi platnosti; <see langword="null" />, ak sa da precitat.
    /// </summary>
    public static string? CheckDateLimit(string? text, DateTime from, DateTime to)
    {
        if (to.Date < from.Date)
            return Resources.FEditTrain_bSave_Click_Začiatok_platnosti_musí_skôr_ako_koniec_platnosti;

        try
        {
            new DateLimit(from.Date, to.Date).TextToBitArray(text ?? "");
            return null;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    ///     Chyba alebo upozornenie varianty podla ostatnych vlakov s rovnakym cislom, nazvom a typom.
    ///     Pri automatickej sprave variant ich prideluje GVDEditor, takze sa nekontroluju.
    /// </summary>
    public static Problem? CheckVariant(int variant, IReadOnlyCollection<Train> others, TrainContext context)
    {
        if (context.AutoVariant)
            return null;

        if (others.Count == 0)
            return variant != -1 && !context.DisableVariantCheck
                ? new Problem(Field.Variant, Resources.FEditTrain_Tento_vlak_nemá_iné_varianty_a_preto_mu_bude_varianta_nastavená_na_hodnotu_Minus_1, true)
                : null;

        if (variant == -1)
            return new Problem(Field.Variant, Resources.FEditTrain_bSave_Click_Varianta_tohto_vlaku_nemôže_byť_Minus_1);

        return others.Any(other => other.Variant == variant)
            ? new Problem(Field.Variant, Resources.FEditTrain_bSave_Click_Vybraná_varianta_vlaku_sa_už_používa_pri_inom_vlaku)
            : null;
    }

    /// <summary>
    ///     Cas v tvare HH:mm; prazdne pole nie je cas (<see cref="Utils.ParseTime" /> by vratil polnoc).
    /// </summary>
    public static bool TryParseTime(string? text, out DateTime time)
    {
        time = default;
        return !string.IsNullOrWhiteSpace(text) && Utils.TryParseTime(text.Trim(), out time);
    }

    /// <summary>
    ///     Smerovanie podla vyplnenych casti trasy; <see langword="null" />, ak vlak nema ziadnu stanicu.
    /// </summary>
    public static Routing? RoutingOf(bool hasFrom, bool hasTo) => (hasFrom, hasTo) switch
    {
        (true, true) => Routing.Prechadzajuci,
        (true, false) => Routing.Konciaci,
        (false, true) => Routing.Vychadzajuci,
        _ => null
    };

    /// <summary>
    ///     Typy hlaseni, ktore INISS pri vlaku s danym smerovanim pouziva (v poradi <paramref name="all" />);
    ///     vlak bez trasy nema ziadne.
    /// </summary>
    public static List<ReportType> ReportTypesFor(Routing? routing, IEnumerable<ReportType> all) =>
        all.Where(type => routing == Routing.Vychadzajuci ? type.BaseTrain
            : routing == Routing.Prechadzajuci ? type.PassThrough
            : routing == Routing.Konciaci && type.TerminateTrain).ToList();

    /// <summary>
    ///     Po zmene smerovania odstrani z dodatkov hlasenia, ktore vlak uz nema.
    /// </summary>
    public static void PruneReports(IEnumerable<Dodatok> doplnky, IReadOnlyCollection<ReportType> allowed)
    {
        foreach (var dodatok in doplnky)
            dodatok.ChosenReports.RemoveAll(chosen => !allowed.Contains(chosen.Type));
    }

    [GeneratedRegex("^[a-zA-Z0-9]{1,20}$")]
    private static partial Regex LinePattern();
}

/// <summary>
///     Varianty vlaku - vlaky s rovnakym cislom, nazvom a typom v jednom grafikone.
/// </summary>
internal static class TrainVariants
{
    /// <summary>
    ///     Ostatne vlaky s rovnakym cislom, nazvom a typom ako koncept (bez upravovaneho riadku).
    /// </summary>
    public static List<Train> Others(TrainDraft draft, TrainContext context)
    {
        var others = new List<Train>();
        for (var i = 0; i < context.Trains.Count; i++)
        {
            var train = context.Trains[i];
            if (i != context.Row && train.Number == draft.Number && train.Name == draft.Name && train.Type == draft.Type)
                others.Add(train);
        }

        return others;
    }

    /// <summary>
    ///     Varianty s rovnakym obdobim platnosti, ktorych datumove obmedzenie ma s konceptom spolocne dni;
    ///     <c>Days</c> je obmedzenie spolocnych dni. Variant s necitatelnym obmedzenim sa preskoci.
    /// </summary>
    public static List<(Train Train, string Days)> Overlaps(TrainDraft draft, IEnumerable<Train> others)
    {
        var result = new List<(Train, string)>();
        if (draft.ValidTo.Date < draft.ValidFrom.Date)
            return result;

        var limit = new DateLimit(draft.ValidFrom.Date, draft.ValidTo.Date, true, true, false, false);
        foreach (var other in others)
        {
            if (other.ZaciatokPlatnosti.Date != draft.ValidFrom.Date || other.KoniecPlatnosti.Date != draft.ValidTo.Date)
                continue;

            try
            {
                var otherLimit = draft.LimitOf(other);
                if (limit.Overlap(otherLimit, draft.DateLimitText))
                    result.Add((other, limit.TextAnd(otherLimit, draft.DateLimitText)));
            }
            catch (DateLimit.ParseException)
            {
                // obmedzenie inej varianty sa neda precitat - prekrytie sa neda zistit
            }
        }

        return result;
    }

    /// <summary>
    ///     Navrh obmedzenia inej varianty bez dni, v ktore ide upravovany vlak; <see langword="null" />, ak sa
    ///     obmedzenia nedaju precitat.
    /// </summary>
    public static string? WithoutCommonDays(TrainDraft draft, Train other)
    {
        if (draft.ValidTo.Date < draft.ValidFrom.Date)
            return null;

        var limit = new DateLimit(draft.ValidFrom.Date, draft.ValidTo.Date, true, true, false, false);
        try
        {
            return limit.TextAnd(draft.LimitOf(other), limit.TextNot(draft.DateLimitText));
        }
        catch (DateLimit.ParseException)
        {
            return null;
        }
    }

    /// <summary>
    ///     Nastavi variantu ulozeneho vlaku. Bez automatickej spravy dostane jediny vlak -1 (ak to nie je vypnute),
    ///     inak zvolenu variantu. Pri automatickej sprave dostane novy vlak a jeho varianty poradie 1, 2, …
    ///     a jediny vlak -1; upraveny vlak variantu nemeni.
    /// </summary>
    /// <param name="train">ukladany vlak</param>
    /// <param name="variant">varianta zvolena v okne</param>
    /// <param name="others">ostatne varianty (<see cref="Others" />)</param>
    /// <param name="isNew">novy vlak alebo kopia</param>
    /// <param name="context">nastavenia variant</param>
    public static void Assign(Train train, int variant, IReadOnlyList<Train> others, bool isNew, TrainContext context)
    {
        if (!context.AutoVariant)
        {
            train.Variant = others.Count == 0 && variant != -1 && !context.DisableVariantCheck ? -1 : variant;
            return;
        }

        if (!isNew)
            return;

        for (var i = 0; i < others.Count; i++)
            others[i].Variant = i + 1;

        train.Variant = others.Count == 0 ? -1 : others.Count + 1;
    }
}
