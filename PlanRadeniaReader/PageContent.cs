using Iniss.PlanRadenia.Data;

namespace Iniss.PlanRadenia;

/// <summary>
/// Precitana strana planu radenia.
/// </summary>
internal sealed class PageContent
{
    public required PageHeader Header { get; init; }

    public required PageLayout Layout { get; init; }

    public List<PlanTrain> Trains { get; } = [];

    /// <summary>Riadky tabulky bez druhu a cisla vlaku (hlavicka tabulky alebo neprecitany vlak).</summary>
    public List<(Box Row, string Text)> Skipped { get; } = [];
}
