using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using ToolsCore.Iniss.Entities;

namespace GVDEditor.Domain.Snapshots;

/// <summary>
/// Stav dat, ktore okno Globalne nastavenia meni priamo v instalacii INISS (jazyky, meskania, typy vlakov,
/// audio linky). Tlacidlo Zrusit ho obnovi.
/// </summary>
internal sealed class GlobalSettingsSnapshot
{
    private readonly ObjectGraphSnapshot _graph;
    private readonly Action[] _resetBindings;

    private GlobalSettingsSnapshot(ObjectGraphSnapshot graph, Action[] resetBindings)
    {
        _graph = graph;
        _resetBindings = resetBindings;
    }

    /// <summary>
    /// Zapamata aktualny stav dat globalnych nastaveni.
    /// </summary>
    public static GlobalSettingsSnapshot Capture(InissWorkspace workspace)
    {
        (object List, Action Reset)?[] lists =
        [
            Item(workspace.Languages), Item(workspace.Delays), Item(workspace.TrainsTypes), Item(workspace.Audios)
        ];
        var present = lists.OfType<(object List, Action Reset)>().ToList();

        var graph = ObjectGraphSnapshot.Capture(present.Select(item => item.List), IsEntity);
        return new GlobalSettingsSnapshot(graph, present.Select(item => item.Reset).ToArray());
    }

    /// <summary>
    /// Vrati data do stavu v case snimky a obnovi prvky na ne naviazane.
    /// </summary>
    public void Restore()
    {
        _graph.Restore();
        foreach (var reset in _resetBindings)
            reset();
    }

    private static (object List, Action Reset)? Item<T>(BindingList<T>? list) =>
        list == null ? null : (list, list.ResetBindings);

    /// <summary>
    /// Sleduju sa entity GVDEditora a jazyk; skupiny zvukov jazyka (zvukova banka) sa oknom nemenia.
    /// </summary>
    private static bool IsEntity(Type type) =>
        type == typeof(FyzLanguage) ||
        (type.Assembly == typeof(Train).Assembly && type.Namespace == typeof(Train).Namespace);
}
