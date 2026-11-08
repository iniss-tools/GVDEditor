using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;

namespace GVDEditor.Domain.Snapshots;

/// <summary>
/// Stav otvoreneho grafikonu na kroky spat - vsetko, co uklada Ctrl+S: dokument (vlaky, tabule, TabTab, texty,
/// pisma, jazyky, radenie...) a hlavicka grafikonu. Odbery udalosti sa nezapamataju, takze obnova nerozbije
/// vazby okna (tabulka vlakov).
/// </summary>
internal sealed class GrafikonState
{
    private readonly ObjectGraphSnapshot _graph;

    private GrafikonState(ObjectGraphSnapshot graph) => _graph = graph;

    /// <summary>Zapamata stav dokumentu <paramref name="document" /> a hlavicky <paramref name="directory" />.</summary>
    public static GrafikonState Capture(GrafikonDocument document, GVDDirectory? directory) =>
        new(ObjectGraphSnapshot.Capture([document, directory], IsTracked, includeDelegates: false));

    /// <summary>Vrati dokument a hlavicku do zapamataneho stavu (tie iste objekty - odkazy medzi nimi ostanu platne).</summary>
    public void Restore() => _graph.Restore();

    /// <summary>Rovnaky stav - zmena, ktora nic nezmenila, nie je krok.</summary>
    public bool SameAs(GrafikonState other) => _graph.SameAs(other._graph);

    private static bool IsTracked(Type type) => type == typeof(GrafikonDocument) || LocalSettingsSnapshot.IsEntity(type);
}
