using GVDEditor.Domain.Entities;

namespace GVDEditor.Integration.PlanRadenia;

/// <summary>
/// Vstup prevodu, ktory nezavisi od vlaku.
/// </summary>
internal sealed class PlanRadeniaContext
{
    /// <summary>
    /// Zvuky banky po jazykoch grafikonu, zakladny jazyk prvy. Druh vozna sa urcuje podla zakladneho jazyka (kluce
    /// nahravok su vo vsetkych jazykoch rovnake); veta sa sklada v kazdom jazyku, ktoreho banka ma slova radenia.
    /// </summary>
    public required IReadOnlyList<RadenieBank> Banks { get; init; }

    /// <summary>Obdobie grafikonu - k nemu sa vztahuju dni poznamok aj radenia.</summary>
    public required DateOnly From { get; init; }

    public required DateOnly To { get; init; }

    /// <summary>Stanica grafikonu podla nazvu z planu radenia, alebo <see langword="null" />.</summary>
    public required Func<string, Station?> Station { get; init; }

    /// <summary>V ktorych hlaseniach sa radenie hlasi (pre vlak bez vlastneho radenia).</summary>
    public required List<ChosenReportType> Reports { get; init; }
}
