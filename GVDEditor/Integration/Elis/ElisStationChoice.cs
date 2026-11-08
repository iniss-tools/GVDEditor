using GVDEditor.Domain.Editing;
using Iniss.Elis.Data;

namespace GVDEditor.Integration.Elis;

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
