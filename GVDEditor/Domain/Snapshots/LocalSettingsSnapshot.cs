using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
namespace GVDEditor.Domain.Snapshots;

/// <summary>
/// Stav dat, ktore okno Lokalne nastavenia meni priamo v otvorenom grafikone (dopravcovia, nastupistia, kolaje,
/// tabule, texty, pisma, TabTab, vlastne stanice a vlaky, ktorych sa zmeny tykaju). Tlacidlo Zrusit ho obnovi.
/// </summary>
internal sealed class LocalSettingsSnapshot
{
    private readonly ObjectGraphSnapshot _graph;
    private readonly Action[] _resetBindings;

    private LocalSettingsSnapshot(ObjectGraphSnapshot graph, Action[] resetBindings)
    {
        _graph = graph;
        _resetBindings = resetBindings;
    }

    /// <summary>
    /// Zapamata aktualny stav dat lokalnych nastaveni.
    /// </summary>
    public static LocalSettingsSnapshot Capture(GrafikonDocument document)
    {
        (object List, Action Reset)?[] lists =
        [
            Item(document.Operators), Item(document.Platforms), Item(document.Tracks), TrainsItem(document.Trains),
            Item(document.TablePhysicals), Item(document.TableLogicals), Item(document.TableCatalogs), Item(document.TabTabs),
            Item(document.TableTexts), Item(document.TableFonts), Item(document.CustomStations)
        ];
        var present = lists.OfType<(object List, Action Reset)>().ToList();

        var roots = present.Select(item => item.List).Append(document.ModeTabsSections);
        var graph = ObjectGraphSnapshot.Capture(roots, IsEntity);
        return new LocalSettingsSnapshot(graph, present.Select(item => item.Reset).ToArray());
    }

    /// <summary>
    /// Vrati data do stavu v case snimky a obnovi prvky na ne naviazane (napr. zoznam vlakov v hlavnom okne).
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
    /// Okno vlaky nepridava ani nemaze, meni len ich odkazy (kolaj, dopravca). Staci preto prekreslit riadky -
    /// ResetBindings by v hlavnom okne prestaval riadky tabulky vlakov a ta by grafikon oznacila ako zmeneny (*).
    /// </summary>
    private static (object List, Action Reset)? TrainsItem<T>(BindingList<T>? trains) =>
        trains == null ? null : (trains, () =>
        {
            for (var i = 0; i < trains.Count; i++)
                trains.ResetItem(i);
        });

    /// <summary>
    /// Sleduju sa len entity GVDEditora a grafikonu a tabul z ToolsCore.Iniss; zvukova banka, obrazky a pod. sa oknom nemenia.
    /// </summary>
    private static bool IsEntity(Type type) =>
        (type.Assembly == typeof(Train).Assembly && type.Namespace == typeof(Train).Namespace) || IsInissEntity(type);

    /// <summary>Entity grafikonu a tabul presunute do ToolsCore.Iniss (stanica, tabule).</summary>
    internal static bool IsInissEntity(Type type) =>
        type.Assembly == typeof(Station).Assembly && type.Namespace is { } ns && (ns == typeof(Station).Namespace || ns == typeof(TablePhysical).Namespace);
}
