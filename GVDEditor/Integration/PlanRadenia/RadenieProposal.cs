using Iniss.PlanRadenia.Data;
using GVDEditor.Domain.Entities;

namespace GVDEditor.Integration.PlanRadenia;

/// <summary>
/// Navrh radenia vlaku z planu radenia - jeden variant (dni, v ktore idu rovnake vozne).
/// </summary>
internal sealed class RadenieProposal
{
    public required string TrainNumber { get; init; }

    /// <summary>Vlak v plane radenia.</summary>
    public required PlanTrain Plan { get; init; }

    public required Radenie Radenie { get; init; }

    public List<PlanRadeniaIssue> Issues { get; } = [];

    /// <summary>Ci sa radenie prevezme do grafikonu.</summary>
    public bool Take { get; set; } = true;

    /// <summary>Ci v hlaseni chyba niektory zvuk banky.</summary>
    public bool HasMissingSounds => Issues.Any(i => i.Kind == PlanRadeniaIssueKind.MissingSound);
}
