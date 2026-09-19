using GVDEditor.Entities;
using ToolsCore.Expressions;
using ToolsCore.TabTab;

namespace GVDEditor.Tools;

/// <summary>
///     Prevadzkovy stav vlaku, ktory grafikon nepozna a nahlad si ho musi nasimulovat: meskania,
///     stav, vyluky, odklon a "teraz".
/// </summary>
internal sealed record TrainRuntime
{
    public int ArrivalDelayMinutes { get; init; }
    public int DepartureDelayMinutes { get; init; }
    public bool IsStanding { get; init; }
    public bool IsDispatched { get; init; }
    public bool LockoutArrival { get; init; }
    public bool LockoutDeparture { get; init; }
    public bool IsDeflected { get; init; }
    public DateTime Now { get; init; } = DateTime.Now;
}

/// <summary>
///     <see cref="IExprTrainContext"/> nad vlakom GVDEditora: udaje z grafikonu + simulovany prevadzkovy stav.
/// </summary>
internal sealed class GvdTrainContext : IExprTrainContext
{
    private readonly Train _train;
    private readonly TrainRuntime _runtime;
    private readonly int _homeStationId;

    public GvdTrainContext(Train train, TrainRuntime runtime, int homeStationId)
    {
        _train = train;
        _runtime = runtime;
        _homeStationId = homeStationId;
    }

    public Train Train => _train;

    public string TrainNumber => _train.Number;

    public int Position => _train.Routing == Routing.Vychadzajuci ? 1 : _train.Routing == Routing.Konciaci ? 3 : 2;

    public int TrainTypeIndex => Math.Max(0, ExprTrainTypes.IndexOf(_train.Type.CategoryTrain));

    /// <summary>Priznaky ako v INISSe: bity Prizn_* z pismen Export3A + vyluky zo simulacie.</summary>
    public uint Flags
    {
        get
        {
            uint f = 0;
            if (_runtime.LockoutArrival) f |= 0x100;
            if (_runtime.LockoutDeparture) f |= 0x200;
            if (_train.IsMedzistatny) f |= 0x1000;
            if (_train.IsPriznakO) f |= 0x2000;
            if (_train.IsDialkovy) f |= 0x4000;
            if (_train.IsMimoriadny) f |= 0x8000;
            if (_train.IsMiestenkovy) f |= 0x10000;
            if (_train.IsIbaLozkovy) f |= 0x20000;
            if (_train.IsNizkopodlazny) f |= 0x40000;
            if (_train.IsPrestupovy) f |= 0x80000;
            return f;
        }
    }

    public uint State => (_runtime.IsDispatched ? 0x20u : 0) | (_runtime.IsStanding ? 0x80u : 0);

    public bool IsDeflected => _runtime.IsDeflected;

    public int ArrivalDelaySeconds => _runtime.ArrivalDelayMinutes * 60;

    public int DepartureDelaySeconds => _runtime.DepartureDelayMinutes * 60;

    public int StayTimeSeconds => PlannedStayTimeSeconds + Math.Max(0, DepartureDelaySeconds - ArrivalDelaySeconds);

    public int PlannedStayTimeSeconds =>
        _train.Arrival is { } a && _train.Departure is { } d ? (int)Math.Max(0, (d.TimeOfDay - a.TimeOfDay).TotalSeconds) : 0;

    public int HomeStationId => _homeStationId;

    public int BaseStationId => StationId(_train.StartingStation);

    public int EndStationId => StationId(_train.EndingStation);

    public bool IsLocal => _train.StartingStation is not null && StationId(_train.StartingStation) == _homeStationId
                           || _train.EndingStation is not null && StationId(_train.EndingStation) == _homeStationId;

    public bool IsFromStation(int stationId) => _train.StaniceZoSmeru.Any(s => StationId(s) == stationId);

    public bool IsToStation(int stationId) => _train.StaniceDoSmeru.Any(s => StationId(s) == stationId);

    public string ArrivalTrack => _train.Arrival is null || _train.Track == Track.None ? "" : _train.Track.Name;

    public string DepartureTrack
    {
        get
        {
            if (_train.Departure is null) return "";
            var t = _train.TrackDeparture ?? _train.Track;
            return t == Track.None ? "" : t.Name;
        }
    }

    public DateOnly ArrivalDate => DateOnly.FromDateTime(_train.Arrival ?? _runtime.Now);

    public DateOnly DepartureDate => DateOnly.FromDateTime(_train.Departure ?? _runtime.Now);

    public TimeOnly ArrivalTime => TimeOnly.FromDateTime(_train.Arrival ?? _train.Departure ?? _runtime.Now);

    public TimeOnly DepartureTime => TimeOnly.FromDateTime(_train.Departure ?? _train.Arrival ?? _runtime.Now);

    public string OperatorName => _train.Operator?.Name ?? "";

    public string ArrivalLine => _train.LineArrival ?? "";

    public string DepartureLine => _train.LineDeparture ?? "";

    public string TrainName => _train.Name ?? "";

    private static int StationId(Station? s) => s is not null && int.TryParse(s.ID, out var id) ? id : 0;

    /// <summary>
    ///     Hodnota, ktoru stlpec pocita sam (TYPE_ITEMS_IDX) - priblizna napodobenina INISSu pre nahlad.
    /// </summary>
    public TabTabValue OwnValue(TableItem item)
    {
        var s = item.FillSection;
        var t = _train;
        string text;

        if (s == TableFillSection.VychadzajucaStanica) text = t.StartingStation?.Name ?? "";
        else if (s == TableFillSection.StaniceZoSmeru) text = string.Join(", ", t.StaniceZoSmeru.Select(x => x.Name));
        else if (s == TableFillSection.StaniceDoSmeru || s == TableFillSection.StaniceDoSmeruNastupiste) text = string.Join(", ", t.StaniceDoSmeru.Select(x => x.Name));
        else if (s == TableFillSection.CielovaStanica || s == TableFillSection.CielovaStanicaNastupiste || s == TableFillSection.CielovaStanicaPodchod) text = t.EndingStation?.Name ?? "";
        else if (s == TableFillSection.CasOdchodu) text = t.Departure?.ToString("HH:mm") ?? "";
        else if (s == TableFillSection.CasPrichodu) text = t.Arrival?.ToString("HH:mm") ?? "";
        else if (s == TableFillSection.MeskaniePrichod) text = _runtime.ArrivalDelayMinutes > 0 ? _runtime.ArrivalDelayMinutes.ToString() : "";
        else if (s == TableFillSection.MeskanieOdchod) text = _runtime.DepartureDelayMinutes > 0 ? _runtime.DepartureDelayMinutes.ToString() : "";
        else if (s == TableFillSection.MeskaniePrichodPopis) text = _runtime.ArrivalDelayMinutes > 0 ? $"Mešká {_runtime.ArrivalDelayMinutes} min." : "";
        else if (s == TableFillSection.MeskanieOdchodPopis) text = _runtime.DepartureDelayMinutes > 0 ? $"Mešká {_runtime.DepartureDelayMinutes} min." : "";
        else if (s == TableFillSection.TypVlaku || s == TableFillSection.HexTypVlaku) text = t.Type.TextInTable;
        else if (s == TableFillSection.CisloVlaku || s == TableFillSection.CisloVlaku35 || s == TableFillSection.CisloVlaku38) text = t.Number;
        else if (s == TableFillSection.TypCisloVlaku || s == TableFillSection.TypCisloVlaku6Chars) text = t.Type.TextInTable + t.Number;
        else if (s == TableFillSection.TypMedzeraCisloVlaku) text = $"{t.Type.TextInTable} {t.Number}";
        else if (s == TableFillSection.NazovVlaku) text = t.Name ?? "";
        else if (s == TableFillSection.TypNazovOrCislo) text = string.IsNullOrEmpty(t.Name) ? $"{t.Type.TextInTable} {t.Number}" : $"{t.Type.TextInTable} {t.Name}";
        else if (s == TableFillSection.NastupistePrichod) text = t.Arrival is null ? "" : t.Track.Platform.Key;
        else if (s == TableFillSection.NastupisteOdchod) text = t.Departure is null ? "" : (t.TrackDeparture ?? t.Track).Platform.Key;
        else if (s == TableFillSection.KolajPrichod) text = ArrivalTrack;
        else if (s == TableFillSection.KolajOdchod) text = DepartureTrack;
        else if (s == TableFillSection.NastupisteKolajPrichod) text = t.Arrival is null ? "" : t.Track.PlatformTrackText;
        else if (s == TableFillSection.NastupisteKolajOdchod) text = t.Departure is null ? "" : (t.TrackDeparture ?? t.Track).PlatformTrackText;
        else if (s == TableFillSection.KolajAltPrichod) text = t.Arrival is null ? "" : t.Track.AltTrackText;
        else if (s == TableFillSection.KolajAltOdchod) text = t.Departure is null ? "" : (t.TrackDeparture ?? t.Track).AltTrackText;
        else if (s == TableFillSection.VlakStojiVStanici) text = _runtime.IsStanding ? "1" : "";
        else if (s == TableFillSection.Dopravca) text = OperatorName;
        else if (s == TableFillSection.LinkaOdchod) text = DepartureLine;
        else if (s == TableFillSection.LinkaPrichod) text = ArrivalLine;
        else text = "";

        return new TabTabValue(text, item.FontIDX);
    }
}
