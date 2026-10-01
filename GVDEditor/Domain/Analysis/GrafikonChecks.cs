using System.Globalization;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Formats;
using GVDEditor.Properties;
using GVDEditor.TabTabEditor;
using GVDEditor.UI.Settings;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.Domain.Analysis;

/// <summary>
/// Kontroly celeho grafikonu, ktore inak robia az okna pri uprave (vlak, tabule, stavovy diagram). Udaje zo suborov,
/// z importu alebo zo starsich verzii GVDEditora nimi nepresli - analyza ich najde vsetky naraz.
/// </summary>
internal static class GrafikonChecks
{
    /// <summary>
    /// Stavovy diagram grafikonu: chybajuci, nenacitatelny, alebo s chybami a upozorneniami validatora.
    /// </summary>
    public static IEnumerable<IProblem> StateDgm(GVDDirectory gvd, AnalysisScope scope)
    {
        var dir = gvd.Dir.FullPath;
        if (!File.Exists(StateDgmFile.PathOf(dir)))
            return [new StateDgmMissing(dir, scope)];

        var diagnostics = ValidateStateDgm(dir, scope, out var readError);
        if (readError is not null)
            return [new StateDgmProblems(dir, [], readError, scope)];

        return diagnostics.Count == 0 ? [] : [new StateDgmProblems(dir, diagnostics, null, scope)];
    }

    /// <summary>
    /// Chyby a upozornenia stavoveho diagramu (bez informacii), pri chybe citania <paramref name="readError" />.
    /// </summary>
    internal static List<StateDgmDiagnostic> ValidateStateDgm(string dir, AnalysisScope scope, out string? readError)
    {
        readError = null;
        StateDgmDiagram? diagram;
        try
        {
            diagram = StateDgmFile.Read(dir);
        }
        catch (Exception e) when (e is StateDgmParseException or IOException or UnauthorizedAccessException)
        {
            readError = e.Message;
            return [];
        }

        if (diagram is null)
            return [];

        return StateDgmValidator.Validate(diagram, new StateDgmValidationOptions
        {
            ReportKeys = scope.Document.ReportTypes.Count > 0 ? scope.Document.ReportTypes.Select(r => r.Key).ToList() : null,
            Symbols = new GvdExprSymbols(scope.Workspace, scope.Document),
            ReportInfos = false
        }).Where(d => d.Severity != ExprSeverity.Info).ToList();
    }

    /// <summary>
    /// Vlaky, ktore by okno vlaku neulozilo alebo pri nich upozornuje (rovnake pravidla ako pri ulozeni vlaku).
    /// </summary>
    public static IEnumerable<IProblem> Trains(GVDDirectory gvd, AnalysisScope scope)
    {
        var trains = scope.Document.Trains;
        for (var i = 0; i < trains.Count; i++)
        {
            var problems = CheckTrain(trains, i, gvd.GVD.ThisStation.ID);
            if (problems.Count > 0)
                yield return new TrainProblems(trains[i], problems, gvd.GVD.ThisStation.ID, scope);
        }
    }

    /// <summary>
    /// Pravidla okna vlaku pre vlak na pozicii <paramref name="index" />. Upozornenie na pobyt cez polnoc sa vynechava
    /// - je to bezny vlak, nie problem. Prekrytie variant sa hlasi len pri prvom vlaku dvojice, nie pri oboch.
    /// </summary>
    internal static List<TrainRules.Problem> CheckTrain(IReadOnlyList<Train> trains, int index, string? homeStationId)
    {
        var draft = TrainDraft.From(trains[index]);
        var context = new TrainContext(trains, index, homeStationId);
        var problems = TrainRules.Check(draft, context)
            .Where(p => !(p.IsWarning && p.Field is TrainRules.Field.Departure or TrainRules.Field.DateLimit))
            .ToList();

        if (problems.Any(p => p.Field == TrainRules.Field.DateLimit))
            return problems;

        var others = TrainVariants.Others(draft, context);
        foreach (var (other, days) in TrainVariants.Overlaps(draft, others))
            if (IndexOf(trains, other) > index)
                problems.Add(new TrainRules.Problem(TrainRules.Field.DateLimit, string.Format(CultureInfo.CurrentCulture,
                    Resources.TrainRules_Prekrytie, TrainRules.Label(other),
                    $"{TrainVariants.PositionOf(other, draft, others)}/{others.Count + 1}", days), true));

        return problems;
    }

    /// <summary>
    /// Tabule, texty, nastupistia, kolaje, dopravcovia a pisma, ktore by stranka lokalnych nastaveni neulozila,
    /// a nedostatky zostav a stlpcov, ktore INISS znesie, no zapise do logu.
    /// </summary>
    public static IEnumerable<IProblem> Settings(AnalysisScope scope)
    {
        var doc = scope.Document;

        foreach (var table in doc.TablePhysicals.ToList())
            if (SettingsItemProblems.Create(Resources.Analyzer_ItemPhysical, table.Key, LocalSettingsPage.FyzickeTabule, table, scope,
                    () => CheckPhysical(doc, table)) is { } problem)
                yield return problem;

        foreach (var table in doc.TableLogicals.ToList())
            if (SettingsItemProblems.Create(Resources.Analyzer_ItemLogical, table.Key, LocalSettingsPage.LogickeTabule, table, scope,
                    () => CheckLogical(doc, table)) is { } problem)
                yield return problem;

        foreach (var table in doc.TableCatalogs.ToList())
            if (SettingsItemProblems.Create(Resources.Analyzer_ItemCatalog, table.Key, LocalSettingsPage.KatalogoveTabule, table, scope,
                    () => CheckCatalog(doc, table)) is { } problem)
                yield return problem;

        foreach (var text in doc.TableTexts.ToList())
            if (SettingsItemProblems.Create(Resources.Analyzer_ItemText, text.Key, LocalSettingsPage.Texty, text, scope,
                    () => CheckText(doc, text)) is { } problem)
                yield return problem;

        // nastupiste a kolaj "nedefinovane" nie su v zozname stranky - nekontroluju sa (rovnako ako na stranke)
        foreach (var platform in doc.Platforms.Where(p => p != Platform.None).ToList())
            if (SettingsItemProblems.Create(Resources.Analyzer_ItemPlatform, platform.Key, LocalSettingsPage.Nastupistia, platform, scope,
                    () =>
                    {
                        var platforms = doc.Platforms.Where(p => p != Platform.None).ToList();
                        var i = Index(platforms, platform);
                        return One(i >= 0 ? PlatformTrackRules.CheckPlatform(platforms, i)?.Message : null);
                    }, ProblemType.Warning)
                is { } problem)
                yield return problem;

        foreach (var track in doc.Tracks.Where(t => t != Track.None).ToList())
            if (SettingsItemProblems.Create(Resources.Analyzer_ItemTrack, track.Key, LocalSettingsPage.Kolaje, track, scope,
                    () =>
                    {
                        var tracks = doc.Tracks.Where(t => t != Track.None).ToList();
                        var i = Index(tracks, track);
                        return One(i >= 0 ? PlatformTrackRules.CheckTrack(tracks, i)?.Message : null);
                    }, ProblemType.Warning)
                is { } problem)
                yield return problem;

        // dopravca "ziadny" nie je v zozname stranky Dopravcovia - nekontroluje sa
        foreach (var oper in doc.Operators.Where(o => o != Operator.None).ToList())
            if (SettingsItemProblems.Create(Resources.Analyzer_ItemOperator, oper.Name, LocalSettingsPage.Dopravcovia, oper, scope,
                    () =>
                    {
                        var operators = doc.Operators.Where(o => o != Operator.None).ToList();
                        var i = Index(operators, oper);
                        return One(i >= 0 ? OperatorRules.CheckName(operators.Select(o => o.Name ?? "").ToList(), i) : null);
                    }, ProblemType.Warning)
                is { } problem)
                yield return problem;

        foreach (var font in doc.TableFonts.ToList())
            if (SettingsItemProblems.Create(Resources.Analyzer_ItemFont, font.Name, LocalSettingsPage.Pisma, font, scope,
                    () => One(Index(doc.TableFonts, font) is var i and >= 0
                        ? FontRules.CheckName(font.Name ?? "") ?? FontRules.CheckId(doc.TableFonts, i)
                        : null), ProblemType.Warning)
                is { } problem)
                yield return problem;
    }

    internal static (List<string> Errors, List<string> Warnings) CheckPhysical(Domain.Documents.GrafikonDocument doc, TablePhysical table)
    {
        var index = Index(doc.TablePhysicals, table);
        return index < 0 ? ([], []) : (TablePhysicalRules.Check(doc.TablePhysicals, index, doc.TableCatalogs).Select(p => p.Message).ToList(), []);
    }

    internal static (List<string> Errors, List<string> Warnings) CheckLogical(Domain.Documents.GrafikonDocument doc, TableLogical table)
    {
        var index = Index(doc.TableLogicals, table);
        if (index < 0)
            return ([], []);

        // zostava sa da skontrolovat, len ak sa zaznamy daju zapisat ako riadky zostavy (rovnako ako na stranke)
        var segments = TableLogicalLayout.FromRecords(table.Records);
        var layout = TableLogicalLayout.IsExpressible(table.Records, segments) ? segments : null;
        var errors = TableLogicalRules.Check(doc.TableLogicals, index, layout).Select(p => p.Message).ToList();
        var warnings = layout is null ? [] : TableLogicalRules.Warnings(layout).Select(w => w.Message).ToList();
        return (errors, warnings);
    }

    internal static (List<string> Errors, List<string> Warnings) CheckCatalog(Domain.Documents.GrafikonDocument doc, TableCatalog table)
    {
        var index = Index(doc.TableCatalogs, table);
        return index < 0
            ? ([], [])
            : (TableCatalogRules.Check(doc.TableCatalogs, index).Select(p => p.Message).ToList(),
                TableCatalogRules.Warnings(table).Select(w => w.Message).ToList());
    }

    internal static (List<string> Errors, List<string> Warnings) CheckText(Domain.Documents.GrafikonDocument doc, TableText text)
    {
        var index = Index(doc.TableTexts, text);
        return index < 0 ? ([], []) : (TableTextRules.Check(doc.TableTexts, index, doc.TableCatalogs).Select(p => p.Message).ToList(), []);
    }

    /// <summary>
    /// Stanice, ktore sa v trasach vlakov hlasia, no zvukova banka pre ne nema nahravku - INISS ich nema cim ohlasit.
    /// </summary>
    public static IEnumerable<IProblem> StationRecordings(AnalysisScope scope)
    {
        foreach (var (station, trains) in StationsWithoutRecording(scope.Document.Trains, scope.Workspace.Stations ?? []))
            yield return new StationWithoutRecording(station, trains, scope);
    }

    /// <summary>
    /// Hlasene stanice tras bez nahravky v banke (skupina R1 hlavneho jazyka) s vlakmi, ktore ich hlasia.
    /// </summary>
    internal static List<(Station Station, List<Train> Trains)> StationsWithoutRecording(IEnumerable<Train> trains, IEnumerable<Station> bank)
    {
        var recorded = bank.Select(s => s.ID).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var result = new List<(Station, List<Train>)>();
        var byId = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var train in trains)
            foreach (var station in train.StaniceZoSmeru.Concat(train.StaniceDoSmeru))
            {
                if (!(station.IsInShortReport || station.IsInLongReport) || recorded.Contains(station.ID))
                    continue;

                if (!byId.TryGetValue(station.ID, out var index))
                {
                    byId[station.ID] = index = result.Count;
                    result.Add((new Station(station.ID, station.Name), []));
                }

                if (!result[index].Item2.Contains(train))
                    result[index].Item2.Add(train);
            }

        return result;
    }

    private static int IndexOf(IReadOnlyList<Train> trains, Train train)
    {
        for (var i = 0; i < trains.Count; i++)
            if (ReferenceEquals(trains[i], train))
                return i;
        return -1;
    }

    private static int Index<T>(IList<T> items, T item) where T : class
    {
        for (var i = 0; i < items.Count; i++)
            if (ReferenceEquals(items[i], item))
                return i;
        return -1;
    }

    private static (List<string> Errors, List<string> Warnings) One(string? error) => (error is null ? [] : [error], []);
}

/// <summary>
/// Grafikon nema stavovy diagram - INISS jeho vlaky nedokaze obsluhovat.
/// </summary>
internal sealed class StateDgmMissing(string dir, AnalysisScope scope) : IProblem
{
    public string Text => Resources.Analyzer_StateDgmMissing;

    public string Solution => Resources.Analyzer_StateDgmMissing_Fix;

    public ProblemType ProblemType => ProblemType.Error;

    public FixType FixType => FixType.Manual;

    // diagram sa zapise hned v editore, nie pri ulozeni grafikonu
    public bool ChangesGrafikon => false;

    public FixResult FixProblem()
    {
        scope.Host.ShowLocalSettings(LocalSettingsPage.StavovyDiagram);
        return File.Exists(StateDgmFile.PathOf(dir)) ? FixResult.Done : FixResult.NotSolved;
    }
}

/// <summary>
/// Stavovy diagram sa neda nacitat, alebo ma chyby ci upozornenia validatora.
/// </summary>
internal sealed class StateDgmProblems(string dir, List<StateDgmDiagnostic> diagnostics, string? readError, AnalysisScope scope) : IProblem
{
    public string Text
    {
        get
        {
            if (readError is not null)
                return string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_StateDgmRead, readError);

            var errors = diagnostics.Count(d => d.IsError);
            var first = diagnostics.OrderByDescending(d => d.IsError).First();
            var where = string.IsNullOrEmpty(first.Path) ? first.Message : $"{first.Path}: {first.Message}";
            return string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_StateDgmProblems, errors, diagnostics.Count - errors, where);
        }
    }

    public string Solution => Resources.Analyzer_StateDgmProblems_Fix;

    public ProblemType ProblemType => readError is not null || diagnostics.Any(d => d.IsError) ? ProblemType.Error : ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    // diagram sa zapise hned v editore, nie pri ulozeni grafikonu
    public bool ChangesGrafikon => false;

    public FixResult FixProblem()
    {
        scope.Host.ShowLocalSettings(LocalSettingsPage.StavovyDiagram, LocalSettingsAction.OpenStateDgmEditor);
        var again = GrafikonChecks.ValidateStateDgm(dir, scope, out var error);
        return error is null && again.Count == 0 ? FixResult.Done : FixResult.NotSolved;
    }
}

/// <summary>
/// Vlak s chybami alebo upozorneniami okna vlaku.
/// </summary>
internal sealed class TrainProblems(Train train, List<TrainRules.Problem> problems, string? homeStationId, AnalysisScope scope) : IProblem
{
    public string Text
    {
        get
        {
            var first = problems.OrderBy(p => p.IsWarning).First();
            var text = string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_Train, TrainRules.Label(train), first.Message);
            return problems.Count > 1 ? text + " " + string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_More, problems.Count - 1) : text;
        }
    }

    public string Solution => Resources.Analyzer_Train_Fix;

    public ProblemType ProblemType => problems.Any(p => !p.IsWarning) ? ProblemType.Error : ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        if (!scope.Host.EditTrain(train))
            return FixResult.NotSolved;

        var index = scope.Document.Trains.IndexOf(train);
        return index < 0 || GrafikonChecks.CheckTrain(scope.Document.Trains, index, homeStationId).Count == 0 ? FixResult.Done : FixResult.NotSolved;
    }
}

/// <summary>
/// Polozka lokalnych nastaveni (tabula, text, kolaj…), ktoru by stranka nastaveni neulozila, alebo s nedostatkami,
/// ktore INISS znesie.
/// </summary>
internal sealed class SettingsItemProblems : IProblem
{
    private readonly string _kind;
    private readonly string? _key;
    private readonly LocalSettingsPage _page;
    private readonly object _item;
    private readonly AnalysisScope _scope;
    private readonly Func<(List<string> Errors, List<string> Warnings)> _check;
    private readonly (List<string> Errors, List<string> Warnings) _found;
    private readonly ProblemType _errorType;

    private SettingsItemProblems(string kind, string? key, LocalSettingsPage page, object item, AnalysisScope scope,
        Func<(List<string>, List<string>)> check, (List<string>, List<string>) found, ProblemType errorType)
    {
        _errorType = errorType;
        _kind = kind;
        _key = key;
        _page = page;
        _item = item;
        _scope = scope;
        _check = check;
        _found = found;
    }

    /// <summary>
    /// Problem polozky, alebo <see langword="null" />, ak je v poriadku.
    /// </summary>
    /// <param name="errorType">zavaznost chyb - <see cref="ProblemType.Error" />, ak ich INISS neprijme, inak
    /// <see cref="ProblemType.Warning" /> (stranka ich neulozi, no INISS bezi)</param>
    public static SettingsItemProblems? Create(string kind, string? key, LocalSettingsPage page, object item, AnalysisScope scope,
        Func<(List<string> Errors, List<string> Warnings)> check, ProblemType errorType = ProblemType.Error)
    {
        var found = check();
        return found.Errors.Count == 0 && found.Warnings.Count == 0
            ? null
            : new SettingsItemProblems(kind, key, page, item, scope, check, found, errorType);
    }

    public string Text
    {
        get
        {
            // texty stranky su odrazky zoznamu - v jednom riadku analyzy bez nich
            var all = _found.Errors.Concat(_found.Warnings).Select(m => m.TrimStart('•', ' ')).ToList();
            var key = string.IsNullOrWhiteSpace(_key) ? Resources.Analyzer_NoKey : _key;
            var text = string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_SettingsItem, _kind, key, all[0]);
            return all.Count > 1 ? text + " " + string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_More, all.Count - 1) : text;
        }
    }

    public string Solution => _found.Errors.Count > 0 ? Resources.Analyzer_SettingsItem_Fix : Resources.Analyzer_SettingsItemWarning_Fix;

    public ProblemType ProblemType => _found.Errors.Count > 0 ? _errorType : ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        _scope.Host.ShowLocalSettings(_page, select: _item);
        var (errors, warnings) = _check();
        return errors.Count == 0 && warnings.Count == 0 ? FixResult.Done : FixResult.NotSolved;
    }
}

/// <summary>
/// Stanica, ktora sa v trasach hlasi, no zvukova banka pre nu nema nahravku.
/// </summary>
internal sealed class StationWithoutRecording(Station station, List<Train> trains, AnalysisScope scope) : IProblem
{
    public string Text => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_StationNoRecording, station.Name, station.ID,
        trains.Count, TrainRules.Label(trains[0]));

    public string Solution => Resources.Analyzer_StationNoRecording_Fix;

    // pohranicne body a cudzie stanice byvaju v trase zamerne - grafikon funguje, len stanicu INISS neohlasi
    public ProblemType ProblemType => ProblemType.Hint;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        // stanica sa z hlaseni vynecha v trase vlaku; nahravka sa doplna v RawBankEditore
        var train = trains.FirstOrDefault(t => scope.Document.Trains.Contains(t));
        if (train is null || !scope.Host.EditTrain(train))
            return FixResult.NotSolved;

        return GrafikonChecks.StationsWithoutRecording(scope.Document.Trains, scope.Workspace.Stations ?? [])
            .Any(s => string.Equals(s.Station.ID, station.ID, StringComparison.OrdinalIgnoreCase))
            ? FixResult.NotSolved
            : FixResult.Done;
    }
}
