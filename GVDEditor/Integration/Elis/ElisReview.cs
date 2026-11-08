using GVDEditor.Domain.Entities;
using GVDEditor.Integration.PlanRadenia;

namespace GVDEditor.Integration.Elis;

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
