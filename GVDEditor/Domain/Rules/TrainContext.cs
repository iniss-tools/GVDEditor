using GVDEditor.Domain.Entities;

namespace GVDEditor.Domain.Rules;

/// <summary>
/// Grafikon, voci ktoremu sa kontroluje upravovany vlak.
/// </summary>
/// <param name="Trains">vsetky vlaky grafikonu</param>
/// <param name="Row">riadok upravovaneho vlaku v <paramref name="Trains" />; pri novom vlaku a kopii pocet vlakov</param>
/// <param name="HomeStationId">stanica grafikonu - do trasy sa nepise; <see langword="null" />, ak nie je znama</param>
internal sealed record TrainContext(IReadOnlyList<Train> Trains, int Row, string? HomeStationId = null);
