namespace Iniss.PlanRadenia.Data;

/// <summary>
/// Stanica planu radenia - jedna alebo viac stran s vlakmi.
/// </summary>
public sealed class PlanStation
{
    /// <summary>Nazov stanice v hlavicke strany.</summary>
    public string Name { get; set; } = "";

    /// <summary>Cisla stran (od 1).</summary>
    public List<int> Pages { get; set; } = [];

    public List<PlanTrain> Trains { get; set; } = [];
}
