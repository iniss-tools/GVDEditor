using GVDEditor.Domain.Editing;
using ToolsCore.StateDgm;

namespace GVDEditor.Tests.Domain.Editing;

/// <summary>
/// Upravy stavoveho diagramu v editore - nove polozky, odstranenie, poradie.
/// </summary>
[TestClass]
public class StateDgmEditingTests
{
    private static StateDgmDiagram Diagram()
    {
        var d = new StateDgmDiagram();
        StateDgmEditing.AddCategory(d, "Osobné");
        return d;
    }

    [TestMethod]
    public void UniqueKey_ObsadenyKluc_PridaCislo()
    {
        Assert.AreEqual("Stav", StateDgmEditing.UniqueKey(["#Start"], "Stav"));
        Assert.AreEqual("Stav3", StateDgmEditing.UniqueKey(["Stav", "Stav2"], "Stav"));
    }

    [TestMethod]
    public void AddCategory_NovaKategoria_MaStartovaciStavAJedinecnyKluc()
    {
        var d = Diagram();
        var second = StateDgmEditing.AddCategory(d, "Nákladné");

        Assert.AreEqual("#Kategorie2", second.Key);
        Assert.AreEqual(StateDgmKeys.START_STATE, second.States.Single().Key);
        Assert.IsTrue(second.States[0].DoState.OnDepartureTable);
    }

    [TestMethod]
    public void AddState_ZaVybranyStav_VlozenyZaNim()
    {
        var category = Diagram().Categories[0];
        var start = category.States[0];
        var last = StateDgmEditing.AddState(category, null);

        var middle = StateDgmEditing.AddState(category, start);

        CollectionAssert.AreEqual(new[] { start, middle, last }, category.States);
        Assert.AreEqual("Stav2", middle.Key);
    }

    [TestMethod]
    public void AddTimePoint_BezStavu_SpolocnyBodMedziOdchodmi()
    {
        var d = Diagram();

        var common = StateDgmEditing.AddTimePoint(d, null);
        var own = StateDgmEditing.AddTimePoint(d, d.Categories[0].States[0]);

        Assert.AreEqual("#Bod", common.Key);
        Assert.AreEqual(StateDgmKeys.BuiltInTimePoints[1], common.TimePointKey1);
        Assert.AreEqual(StateDgmKeys.BuiltInTimePoints[3], common.TimePointKey2);
        Assert.AreSame(own, d.Categories[0].States[0].TimePoints.Single());
    }

    [TestMethod]
    public void Remove_VratiVlastnikaPolozky()
    {
        var d = Diagram();
        var category = d.Categories[0];
        var state = StateDgmEditing.AddState(category, null);
        var own = StateDgmEditing.AddTimePoint(d, state);
        var common = StateDgmEditing.AddTimePoint(d, null);
        var design = StateDgmEditing.AddDesign(d);

        Assert.AreSame(state, StateDgmEditing.Remove(d, own));
        Assert.IsNull(StateDgmEditing.Remove(d, common));
        Assert.AreSame(category, StateDgmEditing.Remove(d, state));
        Assert.IsNull(StateDgmEditing.Remove(d, design));
        Assert.IsNull(StateDgmEditing.Remove(d, category));
        Assert.IsEmpty(d.Categories);
        Assert.IsEmpty(d.TimePoints);
        Assert.IsEmpty(d.Designs);
    }

    [TestMethod]
    public void Move_NaKraji_NepohnePolozku()
    {
        var d = Diagram();
        var first = d.Categories[0];
        var second = StateDgmEditing.AddCategory(d, "Nákladné");

        Assert.IsFalse(StateDgmEditing.Move(d, first, -1, null));
        Assert.IsTrue(StateDgmEditing.Move(d, first, 1, null));
        CollectionAssert.AreEqual(new[] { second, first }, d.Categories);
    }

    [TestMethod]
    public void SwapEvents_VymeniPoradieAkciiAjTlacidiel()
    {
        var state = Diagram().Categories[0].States[0];
        var a = StateDgmEditing.NewEvent(state, null);
        state.Events.Add(a);
        var b = StateDgmEditing.NewEvent(state, "#Odchod");
        state.Events.Add(b);
        var ca = new StateDgmControl { CtrlId = 0, EventKey = a.Key };
        var cb = new StateDgmControl { CtrlId = 1, EventKey = b.Key };

        StateDgmEditing.SwapEvents(state, (a, ca), (b, cb));

        CollectionAssert.AreEqual(new[] { b, a }, state.Events);
        Assert.AreEqual(1, ca.CtrlId);
        Assert.AreEqual(0, cb.CtrlId);
        Assert.AreEqual("#GoToOdchod", b.Key);
    }

    [TestMethod]
    public void NewStarter_SpustaPrvuAkciuStavu()
    {
        var state = Diagram().Categories[0].States[0];
        state.Events.Add(StateDgmEditing.NewEvent(state, null));

        var starter = StateDgmEditing.NewStarter(state);

        Assert.AreEqual("#Akcia", starter.EventKey);
        Assert.AreEqual(-360, starter.TimeOffset);
        Assert.AreEqual(600, starter.TimeOffsetStep);
    }
}
