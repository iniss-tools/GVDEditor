using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.Domain.Editing;

/// <summary>
/// Upravy stavoveho diagramu bez okna: nove polozky s jedinecnym klucom, odstranenie a presun v poradi.
/// Okno editora (FStateDgm) len vyberie polozku a obnovi zobrazenie.
/// </summary>
internal static class StateDgmEditing
{
    /// <summary>
    /// Kluc <paramref name="baseKey" />, alebo s cislom 2, 3… ak sa uz pouziva.
    /// </summary>
    public static string UniqueKey(IEnumerable<string> existing, string baseKey)
    {
        var set = existing.ToHashSet(StringComparer.Ordinal);
        if (!set.Contains(baseKey)) return baseKey;
        for (var i = 2;; i++)
            if (!set.Contains(baseKey + i))
                return baseKey + i;
    }

    /// <summary>
    /// Novy stav - odchodova tabula s kolajou a polohou (ako v predlohach INISSu).
    /// </summary>
    public static StateDgmState NewState(string key) => new()
    {
        Key = key,
        DoState = new StateDgmTableSet { OnDepartureTable = true, ShowTrack = true, ShowPosition = true }
    };

    /// <summary>
    /// Prida kategoriu so startovacim stavom.
    /// </summary>
    public static StateDgmCategory AddCategory(StateDgmDiagram diagram, string name)
    {
        var category = new StateDgmCategory { Key = UniqueKey(diagram.Categories.Select(x => x.Key), "#Kategorie"), Name = name };
        category.States.Add(NewState(StateDgmKeys.StartState));
        diagram.Categories.Add(category);
        return category;
    }

    /// <summary>
    /// Prida stav do kategorie za stav <paramref name="after" /> (inak na koniec). Prvy stav kategorie je startovaci.
    /// </summary>
    public static StateDgmState AddState(StateDgmCategory category, StateDgmState? after)
    {
        var state = NewState(UniqueKey(category.States.Select(x => x.Key), category.States.Count == 0 ? StateDgmKeys.StartState : "Stav"));
        var at = after != null ? category.States.IndexOf(after) + 1 : category.States.Count;
        category.States.Insert(at, state);
        return state;
    }

    /// <summary>
    /// Prida vzhlad tabule.
    /// </summary>
    public static StateDgmDesign AddDesign(StateDgmDiagram diagram)
    {
        var design = new StateDgmDesign { Key = UniqueKey(diagram.Designs.Select(x => x.Key), "Vzhlad"), Bitmaps = "0-0,1,2" };
        diagram.Designs.Add(design);
        return design;
    }

    /// <summary>
    /// Prida casovy bod do stavu, alebo (bez stavu) spolocny casovy bod diagramu medzi pravidelnym a predpokladanym odchodom.
    /// </summary>
    public static StateDgmTimePoint AddTimePoint(StateDgmDiagram diagram, StateDgmState? state)
    {
        var timePoint = new StateDgmTimePoint();
        if (state != null)
        {
            timePoint.Key = UniqueKey(state.TimePoints.Select(x => x.Key), StateDgmKeys.StartTime);
            state.TimePoints.Add(timePoint);
        }
        else
        {
            timePoint.Key = UniqueKey(diagram.TimePoints.Select(x => x.Key), "#Bod");
            timePoint.TimePointKey1 = StateDgmKeys.BuiltInTimePoints[1];
            timePoint.TimePointKey2 = StateDgmKeys.BuiltInTimePoints[3];
            diagram.TimePoints.Add(timePoint);
        }

        return timePoint;
    }

    /// <summary>
    /// Odstrani kategoriu, stav, vzhlad alebo casovy bod.
    /// </summary>
    /// <returns>vlastnik odstranenej polozky (kategoria stavu, stav casoveho bodu), inak <see langword="null" />.</returns>
    public static object? Remove(StateDgmDiagram diagram, object item)
    {
        switch (item)
        {
            case StateDgmCategory category:
                diagram.Categories.Remove(category);
                return null;
            case StateDgmState state:
                var owner = diagram.Categories.First(x => x.States.Contains(state));
                owner.States.Remove(state);
                return owner;
            case StateDgmDesign design:
                diagram.Designs.Remove(design);
                return null;
            case StateDgmTimePoint timePoint:
                if (diagram.TimePoints.Remove(timePoint))
                    return null;
                return diagram.Categories.SelectMany(x => x.States).FirstOrDefault(s => s.TimePoints.Remove(timePoint));
            default:
                throw new ArgumentException("Neznáma položka diagramu.", nameof(item));
        }
    }

    /// <summary>
    /// Posunie polozku o <paramref name="delta" /> miest v jej zozname. Casovy bod stavu sa hlada v stave
    /// <paramref name="state" />.
    /// </summary>
    /// <returns><see langword="false" />, ak sa polozka neda posunut (je na kraji).</returns>
    public static bool Move(StateDgmDiagram diagram, object item, int delta, StateDgmState? state) => item switch
    {
        StateDgmCategory c => MoveIn(diagram.Categories, c, delta),
        StateDgmState s => MoveIn(diagram.Categories.First(x => x.States.Contains(s)).States, s, delta),
        StateDgmDesign d => MoveIn(diagram.Designs, d, delta),
        StateDgmTimePoint t => diagram.TimePoints.Contains(t) ? MoveIn(diagram.TimePoints, t, delta) : state != null && MoveIn(state.TimePoints, t, delta),
        _ => false
    };

    /// <summary>
    /// Vymeni polozku so susednou v zozname.
    /// </summary>
    public static bool MoveIn<T>(List<T> list, T item, int delta)
    {
        var i = list.IndexOf(item);
        var j = i + delta;
        if (i < 0 || j < 0 || j >= list.Count) return false;
        (list[i], list[j]) = (list[j], list[i]);
        return true;
    }

    /// <summary>
    /// Nova akcia stavu (este nepridana); pri prechode do stavu <paramref name="nextState" /> dostane kluc #GoTo….
    /// </summary>
    public static StateDgmEvent NewEvent(StateDgmState state, string? nextState) => new()
    {
        Key = UniqueKey(state.Events.Select(x => x.Key), nextState != null ? "#GoTo" + nextState.TrimStart('#') : "#Akcia"),
        Class = "SDEventUniPos",
        NextState = nextState
    };

    /// <summary>
    /// Vymeni poradie dvoch akcii stavu - tlacidla si vymenia CtrlID (urcuje ich poradie), akcie miesto v zozname.
    /// </summary>
    public static void SwapEvents(StateDgmState state, (StateDgmEvent? Event, StateDgmControl? Control) a,
        (StateDgmEvent? Event, StateDgmControl? Control) b)
    {
        if (a.Control != null && b.Control != null)
            (a.Control.CtrlId, b.Control.CtrlId) = (b.Control.CtrlId, a.Control.CtrlId);
        if (a.Event != null && b.Event != null)
        {
            var i = state.Events.IndexOf(a.Event);
            var j = state.Events.IndexOf(b.Event);
            (state.Events[i], state.Events[j]) = (state.Events[j], state.Events[i]);
        }
    }

    /// <summary>
    /// Novy starter stavu (este nepridany) - spusta prvu akciu stavu od 6 minut pred
    /// pravidelnym odchodom kazdych 10 minut, naposledy 5 minut pred predpokladanym odchodom.
    /// </summary>
    public static StateDgmStarter NewStarter(StateDgmState state) => new()
    {
        Key = UniqueKey(state.Starters.Select(x => x.Key), "Starter"),
        EventKey = state.Events.FirstOrDefault()?.Key ?? "",
        TimePointKey = StateDgmKeys.BuiltInTimePoints[1],
        TimeOffset = -360,
        TimeOffsetStep = 600,
        TimePointKeyLast = StateDgmKeys.BuiltInTimePoints[3],
        TimeOffsetLast = -300
    };
}
