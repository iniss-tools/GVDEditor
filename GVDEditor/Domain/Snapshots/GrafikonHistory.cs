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

/// <summary>
/// Kroky spat a znova nad otvorenym grafikonom: po kazdej dokoncenej uprave sa zapamata stav grafikonu
/// (<see cref="Commit" />), Spat a Znova sa medzi zapamatanymi stavmi len posuvaju. Najviac <see cref="Capacity" />
/// krokov; nova uprava po kroku spat zahodi kroky znova.
/// </summary>
internal sealed class GrafikonHistory
{
    /// <summary>Kolko krokov spat sa pamata.</summary>
    public const int Capacity = 30;

    private readonly Func<GrafikonState> _capture;
    private readonly List<(string Label, GrafikonState State)> _states = [];
    private int _position;
    private int _savedPosition;

    /// <summary>Zacne historiu so sucasnym stavom; <paramref name="saved" /> = sucasny stav je ulozeny.</summary>
    public GrafikonHistory(Func<GrafikonState> capture, bool saved)
    {
        _capture = capture;
        _states.Add(("", capture()));
        _savedPosition = saved ? 0 : -1;
    }

    public bool CanUndo => _position > 0;

    public bool CanRedo => _position < _states.Count - 1;

    /// <summary>Popis kroku, ktory vrati Spat; null = nie je.</summary>
    public string? UndoLabel => CanUndo ? _states[_position].Label : null;

    /// <summary>Popis kroku, ktory zopakuje Znova; null = nie je.</summary>
    public string? RedoLabel => CanRedo ? _states[_position + 1].Label : null;

    /// <summary>Popisy krokov spat od posledneho.</summary>
    public IReadOnlyList<string> UndoLabels => Enumerable.Range(1, _position).Select(i => _states[_position + 1 - i].Label).ToList();

    /// <summary>Popisy krokov znova od najblizsieho.</summary>
    public IReadOnlyList<string> RedoLabels => _states.Skip(_position + 1).Select(s => s.Label).ToList();

    /// <summary>Grafikon je v stave, v akom sa naposledy ulozil.</summary>
    public bool IsSaved => _position == _savedPosition;

    /// <summary>Kolko krokov spat sa pamata teraz.</summary>
    public int Count => _position;

    /// <summary>
    /// Zapamata stav po dokoncenej uprave <paramref name="label" />. Ak sa grafikon v skutocnosti nezmenil, krok nevznikne.
    /// </summary>
    /// <returns><see langword="true" />, ak vznikol krok.</returns>
    public bool Commit(string label)
    {
        var state = _capture();
        if (state.SameAs(_states[_position].State))
            return false;

        // nova uprava po kroku spat - kroky znova (a s nimi aj ulozeny stav, ak bol medzi nimi) zaniknu
        _states.RemoveRange(_position + 1, _states.Count - _position - 1);
        if (_savedPosition > _position)
            _savedPosition = -1;

        _states.Add((label, state));
        _position++;
        if (_states.Count > Capacity + 1)
        {
            _states.RemoveAt(0);
            _position--;
            _savedPosition = _savedPosition > 0 ? _savedPosition - 1 : -1;
        }

        return true;
    }

    /// <summary>Vrati <paramref name="steps" /> krokov spat.</summary>
    public void Undo(int steps = 1)
    {
        if (steps < 1 || steps > _position)
            throw new ArgumentOutOfRangeException(nameof(steps));
        _position -= steps;
        _states[_position].State.Restore();
    }

    /// <summary>Zopakuje <paramref name="steps" /> krokov znova.</summary>
    public void Redo(int steps = 1)
    {
        if (steps < 1 || _position + steps > _states.Count - 1)
            throw new ArgumentOutOfRangeException(nameof(steps));
        _position += steps;
        _states[_position].State.Restore();
    }

    /// <summary>Sucasny stav sa ulozil.</summary>
    public void MarkSaved() => _savedPosition = _position;
}
