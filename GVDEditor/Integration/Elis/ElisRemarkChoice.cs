using GVDEditor.Domain.Entities;

namespace GVDEditor.Integration.Elis;

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
