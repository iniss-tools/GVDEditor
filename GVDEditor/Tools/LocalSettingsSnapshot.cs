namespace GVDEditor.Tools;

/// <summary>
///     Stav dat, ktore okno Lokalne nastavenia meni priamo v <see cref="GlobData" /> (dopravcovia, nastupistia, kolaje,
///     tabule, texty, pisma, TabTab, vlastne stanice a vlaky, ktorych sa zmeny tykaju). Tlacidlo Zrusit ho obnovi.
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
    ///     Zapamata aktualny stav dat lokalnych nastaveni.
    /// </summary>
    public static LocalSettingsSnapshot Capture()
    {
        (object List, Action Reset)?[] lists =
        [
            Item(GlobData.Operators), Item(GlobData.Platforms), Item(GlobData.Tracks), TrainsItem(GlobData.Trains),
            Item(GlobData.TablePhysicals), Item(GlobData.TableLogicals), Item(GlobData.TableCatalogs), Item(GlobData.TabTabs),
            Item(GlobData.TableTexts), Item(GlobData.TableFonts), Item(GlobData.CustomStations)
        ];
        var present = lists.OfType<(object List, Action Reset)>().ToList();

        var roots = present.Select(item => item.List).Append(GlobData.ModeTabsSections);
        var graph = ObjectGraphSnapshot.Capture(roots, IsEntity);
        return new LocalSettingsSnapshot(graph, present.Select(item => item.Reset).ToArray());
    }

    /// <summary>
    ///     Vrati data do stavu v case snimky a obnovi prvky na ne naviazane (napr. zoznam vlakov v hlavnom okne).
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
    ///     Okno vlaky nepridava ani nemaze, meni len ich odkazy (kolaj, dopravca). Staci preto prekreslit riadky -
    ///     ResetBindings by v hlavnom okne prestaval riadky tabulky vlakov a ta by grafikon oznacila ako zmeneny (*).
    /// </summary>
    private static (object List, Action Reset)? TrainsItem<T>(BindingList<T>? trains) =>
        trains == null ? null : (trains, () =>
        {
            for (var i = 0; i < trains.Count; i++)
                trains.ResetItem(i);
        });

    /// <summary>
    ///     Sleduju sa len entity GVDEditora; zvukova banka, obrazky a pod. sa oknom nemenia.
    /// </summary>
    private static bool IsEntity(Type type) =>
        type.Assembly == typeof(GlobData).Assembly && type.Namespace == "GVDEditor.Entities";
}
