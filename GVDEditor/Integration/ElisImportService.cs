using System.Globalization;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using GVDEditor.Properties;
using Iniss.Elis;
using Iniss.PlanRadenia;
using ToolsCore.Iniss.Entities;
using ToolsCore.XML;

namespace GVDEditor.Integration;

/// <summary>
/// Co sa pri importe z ELIS stane s vlakmi, ktore grafikon uz ma.
/// </summary>
internal enum ElisImportMode
{
    /// <summary>Naimportovane vlaky sa pridaju k povodnym.</summary>
    Add,

    /// <summary>
    /// Povodne vlaky sa nahradia naimportovanymi, ale rovnaky vlak si ponecha udaje, ktore ELIS nepozna (kolaje,
    /// doplnky, jazyky...). Povodne vlaky, ktore v ELIS nie su, sa odstrania.
    /// </summary>
    Update,

    /// <summary>Povodne vlaky sa odstrania bez nahrady.</summary>
    Replace
}

/// <summary>
/// Volby importu vlakov z ELIS zadane pouzivatelom.
/// </summary>
internal sealed class ElisImportOptions
{
    /// <summary>
    /// Predvolene umiestnenie aplikacie Cestovne poriadky.
    /// </summary>
    public const string DefaultElisDirectory = @"C:\Program Files (x86)\Cestovné poriadky";

    public string AppDirectory { get; init; } = null!;
    public string RegistrationNumber { get; init; } = null!;
    public bool OmitPassingTrains { get; init; }
    public bool ReorderTrains { get; init; }

    /// <summary>
    /// Co sa stane s povodnymi vlakmi grafikonu.
    /// </summary>
    public ElisImportMode Mode { get; init; }

    /// <summary>
    /// Ci sa maju naimportovanym vlakom vygenerovat texty na tabuliach.
    /// </summary>
    public bool GenerateTableTexts { get; init; }

    /// <summary>
    /// Grafikon, z ktoreho vlakov sa prevezme kratke hlasenie a hlasenia doplnkov (otvoreny alebo ine obdobie
    /// stanice); <see langword="null" /> = nepreberat.
    /// </summary>
    public GVDDirectory? LearnFrom { get; init; }

    /// <summary>Plan radenia vlakov ZSSK (PDF), z ktoreho sa vlakom navrhne radenie; <see langword="null" /> = bez radenia.</summary>
    public string? PlanPdf { get; init; }
}

/// <summary>
/// Medzivysledok importu z ELIS - nacitane data a stanice, ktore treba priradit rucne.
/// </summary>
internal sealed class ElisImport
{
    public required ElisBridgeClient Client { get; init; }
    public required ElisResult Data { get; init; }
    public required List<string> Unresolved { get; init; }
    public required string GVDPath { get; init; }
    public required GVDInfo GVDInfo { get; init; }

    /// <summary>
    /// Co sa stane s povodnymi vlakmi. Nesie sa spolu s vysledkom - stav okolo asynchronneho behu sa lahko strati
    /// a chyba by sa prejavila az tichym nenahradenim vlakov.
    /// </summary>
    public required ElisImportMode Mode { get; init; }

    /// <summary>
    /// Ci sa maju naimportovanym vlakom vygenerovat texty na tabuliach.
    /// </summary>
    public required bool GenerateTableTexts { get; init; }

    /// <summary>
    /// Vlaky, z ktorych sa preberie kratke hlasenie a hlasenia doplnkov; prazdne = nepreberat.
    /// </summary>
    public required IReadOnlyList<Train> SourceTrains { get; init; }

    /// <summary>
    /// Volba kratkeho hlasenia pre stanice z predchadzajuceho importu (ID stanice -> hlasi sa).
    /// </summary>
    public required Dictionary<string, bool> SavedShort { get; init; }

    /// <summary>
    /// Priradenie poznamok vlakov z predchadzajuceho importu.
    /// </summary>
    public required Dictionary<string, ElisRemarkAction> SavedRemarks { get; init; }

    /// <summary>Vlaky stanice v plane radenia; <see langword="null" />, ak sa plan necital alebo sa necital uspesne.</summary>
    public PlanResult? Plan { get; init; }

    /// <summary>Preco sa plan radenia nepodarilo precitat (sprava pre pouzivatela), inak <see langword="null" />.</summary>
    public string? PlanError { get; init; }

    /// <summary>Nazvy stanic pre poznamky planu a stanice grafikonu, ku ktorym patria.</summary>
    public Dictionary<string, Station> PlanStations { get; init; } = [];
}

/// <summary>
/// Odkial je navrh kratkeho hlasenia pre stanicu.
/// </summary>
internal enum ElisShortSource
{
    /// <summary>Bez navrhu.</summary>
    None,

    /// <summary>Volba z predchadzajuceho importu.</summary>
    Saved,

    /// <summary>Ako ju hlasia vlaky grafikonu.</summary>
    Grafikon,

    /// <summary>Podla druhu stanice v ELIS (uzlova alebo koncova stanica).</summary>
    ElisType
}

/// <summary>
/// Stanica, ktora sa v naimportovanych vlakoch hlasi medzi koncovymi - obsluha vyberie, ci aj v kratkom hlaseni.
/// </summary>
internal sealed class ElisStationChoice
{
    public required Station Station { get; init; }

    /// <summary>Pocet naimportovanych vlakov, ktore ju hlasia.</summary>
    public required int Trains { get; init; }

    /// <summary>Ako ju hlasia vlaky grafikonu, z ktoreho sa preberia.</summary>
    public ShortReportVote Vote { get; init; }

    /// <summary>Druh stanice v ELIS (<see cref="ElisStop.StationType" />).</summary>
    public int ElisType { get; init; }

    public ElisShortSource Source { get; init; }

    /// <summary>Ci sa hlasi v kratkom hlaseni.</summary>
    public bool Short { get; set; }
}

/// <summary>
/// Poznamka vlakov z ELIS - obsluha vyberie, co sa s vlakmi s touto poznamkou urobi.
/// </summary>
internal sealed class ElisRemarkChoice
{
    public required string Text { get; init; }

    /// <summary>Pocet naimportovanych vlakov s poznamkou.</summary>
    public required int Trains { get; init; }

    /// <summary>Ci je volba z predchadzajuceho importu.</summary>
    public bool Saved { get; init; }

    public ElisRemarkAction Action { get; set; } = ElisRemarkAction.Nothing;
}

/// <summary>
/// Volby, ktore obsluha pred dokoncenim importu skontroluje: kratke hlasenie a poznamky vlakov.
/// </summary>
internal sealed class ElisReview
{
    public required List<ElisStationChoice> Stations { get; init; }
    public required List<ElisRemarkChoice> Remarks { get; init; }

    /// <summary>Naimportovany vlak -> ten isty vlak v grafikone, z ktoreho sa preberia.</summary>
    public required Dictionary<Train, Train> Same { get; init; }

    /// <summary>Ci si vlaky, ktore v grafikone su, ponechaju svoje kratke hlasenie.</summary>
    public bool KeepTrainShort { get; set; } = true;

    /// <summary>Navrhy radenia vlakov z planu radenia.</summary>
    public List<RadenieProposal> Radenia { get; init; } = [];

    /// <summary>Preco sa plan radenia nepodarilo precitat, inak <see langword="null" />.</summary>
    public string? PlanError { get; init; }

    /// <summary>Ci je co kontrolovat.</summary>
    public bool IsEmpty => Stations.Count == 0 && Remarks.Count == 0 && Radenia.Count == 0 && PlanError is null;
}

/// <summary>
/// Import vlakov z programu ELIS (Cestovne poriadky): nacitanie dat na pozadi, priradenie stanic a prevod na vlaky.
/// </summary>
internal static class ElisImportService
{
    /// <summary>Skupina zvukov s doplnkami hlasenia.</summary>
    private const string AdditionGroup = "DODATKY";

    /// <summary>
    /// Prva faza importu na pozadi - nacita data z ELIS, vlaky grafikonu, z ktoreho sa preberie kratke hlasenie,
    /// a zisti, ktore stanice sa nepodarilo priradit. Pouzivatela sa tu nic nepyta; nepriradene stanice doriesi
    /// <see cref="SaveStationMap" /> a prevod <see cref="Convert" />.
    /// </summary>
    /// <param name="options">Volby importu.</param>
    /// <param name="gvdDir">Grafikon, do ktoreho sa importuje.</param>
    /// <param name="types">Typy vlakov instalacie.</param>
    /// <param name="operators">Dopravcovia grafikonu.</param>
    /// <param name="track">Kolaj pre vlaky bez kolaje.</param>
    /// <param name="existingTrains">Vlaky otvoreneho grafikonu.</param>
    /// <param name="stations">Stanice zvukovej banky a grafikonu.</param>
    /// <param name="workspace">Instalacia INISS - na nacitanie ineho obdobia stanice.</param>
    public static Task<ElisImport> LoadAsync(ElisImportOptions options, GVDDirectory gvdDir, IReadOnlyList<TrainType> types,
        IReadOnlyList<Operator> operators, Track track, IReadOnlyList<Train> existingTrains, StationDirectory stations, InissWorkspace workspace)
    {
        // zoznamy sa skopiruju este na UI vlakne - pocas importu ich okno moze menit
        var typeList = types.ToList();
        var operatorList = operators.ToList();
        var current = existingTrains.ToList();
        var gvdPath = gvdDir.Dir.FullPath;
        var gvd = gvdDir.GVD;
        var learnFrom = options.LearnFrom;
        var learnPath = learnFrom?.Dir.FullPath;
        var learnIsOther = learnPath != null && !PathsEqual(learnPath, gvdPath);

        var planPdf = options.PlanPdf;
        var stationName = gvd.ThisStation.Name;
        var candidates = string.IsNullOrEmpty(planPdf) ? [] : PlanStations.Candidates(stations.All);

        return Task.Run(async () =>
        {
            var source = learnFrom is null ? [] : learnIsOther ? LoadTrains(learnFrom, workspace) : current;

            // nove obdobie stanice este nema vlastne volby - prevezmu sa z grafikonu, z ktoreho sa uci
            var stationMap = ElisMapFile.Read(gvdPath);
            var savedShort = ElisChoicesFile.ReadShort(gvdPath);
            var savedRemarks = ElisChoicesFile.ReadRemarks(gvdPath);
            if (learnIsOther)
            {
                foreach (var (name, id) in ElisMapFile.Read(learnPath!))
                    stationMap.TryAdd(name, id);
                if (savedShort.Count == 0)
                    savedShort = ElisChoicesFile.ReadShort(learnPath!);
                if (savedRemarks.Count == 0)
                    savedRemarks = ElisChoicesFile.ReadRemarks(learnPath!);
            }

            var client = new ElisBridgeClient(typeList, operatorList, gvd, track, stations)
            {
                AppDirectory = options.AppDirectory,
                RegistrationNumber = options.RegistrationNumber,
                // cisla variant pridelene importom nesmu kolidovat s vlakmi, ktore v grafikone ostanu
                DefinedTrains = options.Mode == ElisImportMode.Add ? current : [],
                OmitPassingTrains = options.OmitPassingTrains,
                ReorderTrains = options.ReorderTrains,
                StationMap = stationMap
            };

            var data = client.LoadData();
            var (plan, planError) = string.IsNullOrEmpty(planPdf) ? (null, null) : await ReadPlanAsync(planPdf, stationName, candidates).ConfigureAwait(false);
            return new ElisImport
            {
                Client = client,
                Data = data,
                Unresolved = client.FindUnresolvedStations(data),
                GVDPath = gvdPath,
                GVDInfo = gvd,
                Mode = options.Mode,
                GenerateTableTexts = options.GenerateTableTexts,
                SourceTrains = source,
                SavedShort = savedShort,
                SavedRemarks = savedRemarks,
                Plan = plan,
                PlanError = planError,
                PlanStations = candidates
            };
        });
    }

    /// <summary>
    /// Precita strany stanice v plane radenia (stanica podla nazvu v hlavicke strany).
    /// </summary>
    /// <returns>Plan, alebo sprava, preco sa ho nepodarilo precitat.</returns>
    private static async Task<(PlanResult? Plan, string? Error)> ReadPlanAsync(string pdf, string stationName, Dictionary<string, Station> candidates)
    {
        try
        {
            var pages = (await PlanRadeniaReaderClient.ListStationsAsync(pdf, CancellationToken.None).ConfigureAwait(false))
                .Where(p => PlanStations.Same(p.Station, stationName)).Select(p => p.Page).ToList();
            if (pages.Count == 0)
                return (null, string.Format(CultureInfo.CurrentCulture, Resources.PlanRadenia_StationNotInPlan, stationName));
            return (await PlanRadeniaReaderClient.ReadAsync(pdf, pages, candidates.Keys, null, CancellationToken.None).ConfigureAwait(false), null);
        }
        catch (Exception e) when (e is InvalidOperationException or IOException or InvalidDataException or System.Xml.XmlException)
        {
            return (null, e.Message);
        }
    }

    /// <summary>
    /// Prida rucne priradene stanice a priradenie ulozi do grafikonu, aby sa nabuduce nepytalo znova.
    /// </summary>
    /// <returns>chyba zapisu (import moze pokracovat), inak <see langword="null" />.</returns>
    public static Exception? SaveStationMap(ElisImport import, IReadOnlyDictionary<string, string> resolved, AppLanguage language)
    {
        foreach (var (elis, station) in resolved)
            import.Client.StationMap[elis] = station;

        try
        {
            ElisMapFile.Write(import.GVDPath, import.Client.StationMap, import.GVDInfo, language);
            return null;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    /// <summary>
    /// Prevedie nacitane data na vlaky grafikonu.
    /// </summary>
    public static ElisConversion Convert(ElisImport import) => import.Client.Convert(import.Data);

    /// <summary>
    /// Pripravi volby na kontrolu: stanice hlasene medzi koncovymi a poznamky vlakov, s navrhom z predchadzajuceho
    /// importu, z vlakov grafikonu alebo podla druhu stanice v ELIS.
    /// </summary>
    /// <param name="import">Import.</param>
    /// <param name="conversion">Naimportovane vlaky.</param>
    /// <param name="document">Otvoreny grafikon - radenia (hlasenia, v ktorych sa radenie hlasi).</param>
    /// <param name="sounds">Zvuky zakladneho jazyka banky - z nich sa sklada hlasenie radenia.</param>
    public static ElisReview BuildReview(ElisImport import, ElisConversion conversion, GrafikonDocument document, IReadOnlyList<FyzSound> sounds)
    {
        // grafikon, ktory medzi koncovymi stanicami v kratkom hlaseni nehlasi nic (napr. z predchadzajuceho importu),
        // nie je vzor - prevzatie by len zopakovalo prazdne stlpce Smer a Zo smeru
        var votes = ShortReportLearning.Votes(import.SourceTrains);
        var learn = votes.Values.Any(vote => vote.Short > 0);
        if (!learn)
            votes.Clear();

        var same = new Dictionary<Train, Train>(ReferenceEqualityComparer.Instance);
        if (learn)
            foreach (var (original, train) in TrainMerging.Match(import.SourceTrains, conversion.Trains))
                same[train] = original;

        var stations = conversion.Trains
            .SelectMany(train => ShortReportLearning.InnerStations(train).DistinctBy(station => station.ID))
            .GroupBy(station => station.ID)
            .Select(group => StationChoice(new Station(group.Key, group.First().Name), group.Count(), import.SavedShort, votes,
                conversion.StationTypes.GetValueOrDefault(group.Key)))
            .OrderByDescending(choice => choice.Trains)
            .ThenBy(choice => choice.Station.Name, StringComparer.CurrentCulture)
            .ToList();

        var remarks = conversion.Remarks.Values
            .SelectMany(keys => keys)
            .GroupBy(key => key, StringComparer.Ordinal)
            .Select(group => import.SavedRemarks.TryGetValue(group.Key, out var saved)
                ? new ElisRemarkChoice { Text = group.Key, Trains = group.Count(), Saved = true, Action = saved }
                : new ElisRemarkChoice { Text = group.Key, Trains = group.Count(), Action = ElisRemarks.Suggest(group.Key) })
            .OrderByDescending(choice => choice.Trains)
            .ThenBy(choice => choice.Text, StringComparer.CurrentCulture)
            .ToList();

        return new ElisReview
        {
            Stations = stations,
            Remarks = remarks,
            Same = same,
            Radenia = ProposeRadenia(import, conversion, document, sounds),
            PlanError = import.PlanError
        };
    }

    /// <summary>
    /// Navrhy radenia naimportovanych vlakov z planu radenia - vlak planu k vlakom s rovnakym cislom.
    /// </summary>
    private static List<RadenieProposal> ProposeRadenia(ElisImport import, ElisConversion conversion, GrafikonDocument document, IReadOnlyList<FyzSound> sounds)
    {
        if (import.Plan is not { } plan)
            return [];

        var gvd = import.GVDInfo;
        var context = new PlanRadeniaContext
        {
            Sounds = sounds,
            From = gvd.StartValidTimeTable,
            To = gvd.EndValidTimeTable,
            Station = name => PlanStations.Resolve(name, import.PlanStations),
            // hlasenia, v ktorych sa radenie hlasi, ako ich ma vacsina radeni grafikonu
            Reports = document.Radenia.Select(r => r.ChosenReports).Where(r => r.Count > 0)
                .GroupBy(ReportsKey).OrderByDescending(g => g.Count()).Select(g => g.First()).FirstOrDefault() ?? []
        };
        var planFrom = DateOnly.TryParseExact(plan.ValidFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var from) ? from : context.From;
        var planTo = DateOnly.TryParseExact(plan.ValidTo, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var to) ? to : context.To;

        var result = new List<RadenieProposal>();
        foreach (var planTrain in plan.Stations.SelectMany(s => s.Trains).DistinctBy(p => p.Number))
        {
            var trains = conversion.Trains.Where(train => train.Number == planTrain.Number).ToList();
            if (trains.Count > 0)
                result.AddRange(PlanRadeniaConversion.Propose(planTrain, trains, planFrom, planTo, context));
        }

        return result;
    }

    private static string ReportsKey(List<ChosenReportType> reports) =>
        string.Join(";", reports.Select(r => r.Type.Key + ":" + string.Join(",", r.Variants.Select(v => v.Key))));

    /// <summary>
    /// Ulozi volby do grafikonu, aby ich dalsi import navrhol. Volby stanic a poznamok, ktore tento import nemal,
    /// ostanu.
    /// </summary>
    /// <returns>chyba zapisu (import moze pokracovat), inak <see langword="null" />.</returns>
    public static Exception? SaveChoices(ElisImport import, ElisReview review, StationDirectory stations, AppLanguage language)
    {
        var shortChoices = new Dictionary<string, bool>(import.SavedShort);
        foreach (var choice in review.Stations)
            shortChoices[choice.Station.ID] = choice.Short;

        var remarks = new Dictionary<string, ElisRemarkAction>(import.SavedRemarks, StringComparer.Ordinal);
        foreach (var choice in review.Remarks)
            remarks[choice.Text] = choice.Action;

        try
        {
            ElisChoicesFile.WriteShort(import.GVDPath, shortChoices, stations, import.GVDInfo, language);
            ElisChoicesFile.WriteRemarks(import.GVDPath, remarks, import.GVDInfo, language);
            return null;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    /// <summary>
    /// Pouzije volby na naimportovane vlaky: kratke hlasenie, priznaky, doplnky a vyluky z poznamok a radenia
    /// grafikonu podla cisla vlaku.
    /// </summary>
    /// <param name="import">Import.</param>
    /// <param name="conversion">Naimportovane vlaky.</param>
    /// <param name="review">Skontrolovane volby.</param>
    /// <param name="document">Otvoreny grafikon - typy hlaseni, ich varianty a radenia.</param>
    /// <param name="sounds">Zvuky zvukovej banky (doplnky su v skupine DODATKY).</param>
    public static void Apply(ElisImport import, ElisConversion conversion, ElisReview review, GrafikonDocument document, IEnumerable<FyzSound> sounds)
    {
        var choices = review.Stations.ToDictionary(choice => choice.Station.ID, choice => choice.Short);
        var actions = review.Remarks.ToDictionary(choice => choice.Text, choice => choice.Action, StringComparer.Ordinal);
        var additions = sounds.Where(sound => sound.Group.Key.Equals(AdditionGroup, StringComparison.OrdinalIgnoreCase)).ToList();

        Dodatok? CreateAddition(Train train, string code)
        {
            var sound = additions.FirstOrDefault(s => Dodatok.CodeFromKey(s.Key) == code);
            if (sound is null)
                return null;

            // hlasenia ako ma rovnaky doplnok vlak s rovnakym smerovanim, inak vo vsetkych hlaseniach smerovania
            var template = import.SourceTrains
                .Where(source => source.Routing == train.Routing)
                .SelectMany(source => source.Doplnky)
                .FirstOrDefault(dodatok => dodatok.Name == code);
            return new Dodatok
            {
                Sound = sound,
                Name = code,
                ChosenReports = template != null
                    ? TrainDraft.CopyDodatok(template).ChosenReports
                    : AllReports(train.Routing, document.ReportTypes, document.ReportVariants)
            };
        }

        // prevzate radenia z planu nahradia radenia vlaku s tym istym cislom v obdobi planu; mimo neho ostanu
        foreach (var group in review.Radenia.Where(p => p.Take).GroupBy(p => p.TrainNumber))
        {
            var period = group.First().Radenie.Validity ?? new ValidityPeriod(import.GVDInfo.StartValidTimeTable, import.GVDInfo.EndValidTimeTable);
            foreach (var radenie in document.Radenia.Where(r => r.CisloVlaku == group.Key).ToList())
                if (!RadeniaEditing.WithoutPeriod(radenie, period, import.GVDInfo.StartValidTimeTable, import.GVDInfo.EndValidTimeTable))
                    document.Radenia.Remove(radenie);

            foreach (var proposal in group)
            {
                if (proposal.Radenie.ChosenReports.Count == 0 && conversion.Trains.FirstOrDefault(t => t.Number == group.Key) is { } first)
                    proposal.Radenie.ChosenReports = AllReports(first.Routing, document.ReportTypes, document.ReportVariants);
                document.Radenia.Add(proposal.Radenie);
            }
        }

        foreach (var train in conversion.Trains)
        {
            var same = review.KeepTrainShort ? review.Same.GetValueOrDefault(train) : null;
            ShortReportLearning.Apply(train, choices, same);
            ElisRemarks.Apply(train, conversion.Remarks.GetValueOrDefault(train) ?? [], actions, CreateAddition);
            train.Radenia.AddRange(document.Radenia.Where(radenie => radenie.CisloVlaku == train.Number));
        }
    }

    private static ElisStationChoice StationChoice(Station station, int trains, Dictionary<string, bool> saved,
        Dictionary<string, ShortReportVote> votes, int elisType)
    {
        var vote = votes.GetValueOrDefault(station.ID);
        var (source, isShort) = saved.TryGetValue(station.ID, out var chosen) ? (ElisShortSource.Saved, chosen)
            : vote.Trains > 0 ? (ElisShortSource.Grafikon, vote.IsShort)
            : IsMajor(elisType) ? (ElisShortSource.ElisType, true)
            : (ElisShortSource.None, false);

        return new ElisStationChoice { Station = station, Trains = trains, Vote = vote, ElisType = elisType, Source = source, Short = isShort };
    }

    /// <summary>
    /// Ci ELIS stanicu povazuje za vyznamnu tak, ze ju navrhneme do kratkeho hlasenia: uzlova alebo koncova stanica.
    /// </summary>
    internal static bool IsMajor(int elisType) => (elisType & ElisStop.TypeJunction) != 0;

    /// <summary>
    /// Vsetky typy hlaseni platne pre smerovanie vlaku, kazdy so vsetkymi variantmi.
    /// </summary>
    private static List<ChosenReportType> AllReports(Routing routing, IEnumerable<ReportType> types, IReadOnlyCollection<ReportVariant> variants) =>
        types.Where(type => routing == Routing.Vychadzajuci ? type.BaseTrain
                : routing == Routing.Prechadzajuci ? type.PassThrough
                : type.TerminateTrain)
            .Select(type => new ChosenReportType { Type = type, Variants = [.. variants] })
            .ToList();

    /// <summary>
    /// Nacita vlaky ineho obdobia stanice, z ktorych sa preberie kratke hlasenie.
    /// </summary>
    /// <exception cref="InvalidOperationException">grafikon sa neda nacitat; sprava je urcena pouzivatelovi</exception>
    private static List<Train> LoadTrains(GVDDirectory dir, InissWorkspace workspace)
    {
        try
        {
            return GrafikonRepository.Load(dir.Dir.FullPath, dir.GVD, workspace, new LoadWarnings()).Trains.ToList();
        }
        catch (Exception e) when (e is FormatException or ArgumentException or IOException)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.ElisImport_LearnFromFailed, dir.Period, e.Message), e);
        }
    }

    private static bool PathsEqual(string a, string b) =>
        string.Equals(Path.GetFullPath(a).TrimEnd('\\'), Path.GetFullPath(b).TrimEnd('\\'), StringComparison.OrdinalIgnoreCase);
}
