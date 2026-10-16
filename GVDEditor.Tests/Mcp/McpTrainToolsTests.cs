using GVDEditor.Domain.Entities;
using GVDEditor.Mcp;
using ModelContextProtocol;

namespace GVDEditor.Tests.Mcp;

/// <summary>Nastroje MCP na zmeny vlakov a sekcii TabTab: kontroly ako v oknach a navrat pri chybe.</summary>
public sealed partial class McpToolsTests
{
    [TestMethod]
    public async Task Vlak_ZmenaNovyAKopia()
    {
        var editor = Editor(out var context);
        context.Document.LocalLanguages.Add(context.Workspace.Languages[1]);

        var changed = await TrainEditTools.SetTrain(editor, id: 1, departure: "10:20:00", track: "2");
        var created = await TrainEditTools.SetTrain(editor, number: "3005", type: "os", @operator: "ZSSK", departure: "14:15", name: "Tatran",
            stationsAfter: [new RouteStopInput("Brezová Dolina", Short: true)]);
        var copy = await TrainEditTools.SetTrain(editor, copyFrom: 2, validity: "jazdí v 7");
        var details = await TrainEditTools.SetTrain(editor, id: 3, languages: ["en"], flags: ["lowFloor", "transfer"]);
        var ambiguous = await Assert.ThrowsExactlyAsync<McpException>(() => TrainEditTools.SetTrain(editor, id: 1, stationsAfter: [new RouteStopInput("Žilina")]));
        var invalid = await Assert.ThrowsExactlyAsync<McpException>(() => TrainEditTools.SetTrain(editor, id: 1, number: "30 01"));

        Assert.AreEqual((new TimeOnly(10, 20), "2"), (changed.Train!.Train.Departure, changed.Train.Train.Track));
        Assert.IsNull(changed.Train.TrackDeparture, "odchod ide s prichodom");
        Assert.AreEqual((4, "V8_01", -1), (created.Train!.Train.Id, created.Train.Train.Name, created.Train.Train.Variant));
        Assert.AreEqual(new RouteStop("Brezová Dolina", "2", true, true), created.Train.StationsAfter.Single());
        Assert.AreSame(Routing.Vychadzajuci, context.Document.Trains[3].Routing);
        StringAssert.EndsWith(created.Change.UndoStep, "(agent)");
        Assert.AreEqual((5, "801", "jazdí v 7"), (copy.Train!.Train.Id, copy.Train.Train.Number, copy.Train.Train.Validity));
        CollectionAssert.AreEquivalent(new[] { 1, 2 }, context.Document.Trains.Where(t => t.Number == "801").Select(t => t.Variant).ToArray());
        CollectionAssert.AreEqual(new[] { "EN" }, details.Train!.Languages.ToArray());
        CollectionAssert.AreEqual(new[] { "lowFloor", "transfer" }, details.Train.Flags.ToArray());
        StringAssert.StartsWith(ambiguous.Message, "station.ambiguous:");
        StringAssert.Contains(ambiguous.Message, "3, 4");
        StringAssert.StartsWith(invalid.Message, "train.invalid:");
        Assert.AreEqual("3001", context.Document.Trains[0].Number, "chyba vratila vlak");
    }

    [TestMethod]
    public async Task Vlak_PremenovanieVariantAZmazanie()
    {
        var editor = Editor(out var context);
        await TrainEditTools.SetTrain(editor, copyFrom: 2, validity: "jazdí v 7");

        await TrainEditTools.SetTrain(editor, id: 2, number: "803");
        var numbers = context.Document.Trains.Select(t => t.Number).ToArray();
        await TrainEditTools.SetTrain(editor, id: 4, number: "805", renameVariants: false);
        var deleted = await TrainEditTools.DeleteTrain(editor, 1);
        var missing = await Assert.ThrowsExactlyAsync<McpException>(() => TrainEditTools.DeleteTrain(editor, 9));

        CollectionAssert.AreEqual(new[] { "3001", "803", "3003", "803" }, numbers, "varianty idu s vlakom");
        CollectionAssert.AreEqual(new[] { "803", "3003", "805" }, context.Document.Trains.Select(t => t.Number).ToArray());
        CollectionAssert.AreEqual(new[] { 1, 2, 3 }, context.Document.Trains.Select(t => t.ID).ToArray());
        Assert.IsTrue(context.Document.Trains.All(t => t.Variant == -1), "vlaky bez dalsej varianty");
        Assert.IsTrue(deleted.Change.Changed);
        Assert.IsEmpty(context.Document.TableTexts[0].Trains, "text zmazaneho vlaku");
        StringAssert.StartsWith(missing.Message, "train.notFound:");
    }

    [TestMethod]
    public async Task TabTab_ZmenaNovaPremenovanieAMazanie()
    {
        var editor = Editor(out var context);
        var druh = context.Document.TabTabs[1];

        var changed = await TabTabEditTools.SetTabTab(editor, "odchody", text: "R=R\nOs=Os");
        var created = await TabTabEditTools.SetTabTab(editor, "NOVA", text: "R=R");
        var error = await Assert.ThrowsExactlyAsync<McpException>(() => TabTabEditTools.SetTabTab(editor, "NOVA", text: "Typ(Typ_R), \"a\" = #MERGE"));
        var encoding = await Assert.ThrowsExactlyAsync<McpException>(() => TabTabEditTools.SetTabTab(editor, "NOVA", text: "中=R"));
        var name = await Assert.ThrowsExactlyAsync<McpException>(() => TabTabEditTools.SetTabTab(editor, "Žiadny", text: "R=R"));
        var renamed = await TabTabEditTools.SetTabTab(editor, "DRUH", newKey: "DRUH2");
        var used = await Assert.ThrowsExactlyAsync<McpException>(() => TabTabEditTools.DeleteTabTab(editor, "druh2"));
        var deleted = await TabTabEditTools.DeleteTabTab(editor, "nova");

        Assert.AreEqual("R=R\r\nOs=Os", changed.TabTab!.Text);
        StringAssert.EndsWith(created.Change.UndoStep, "(agent)");
        StringAssert.StartsWith(error.Message, "tabtab.invalid:");
        StringAssert.StartsWith(encoding.Message, "tabtab.encoding:");
        StringAssert.StartsWith(name.Message, "tabtab.invalidName:");
        Assert.AreEqual("DRUH2", renamed.TabTab!.Key);
        Assert.AreSame(druh, context.Document.TableCatalogs[0].Items[0].Tab1, "stlpec pouziva tu istu sekciu");
        StringAssert.Contains(used.Message, "K16");
        Assert.IsTrue(deleted.Change.Changed);
        CollectionAssert.AreEqual(new[] { "ODCHODY", "DRUH2" }, context.Document.TabTabs.Select(t => t.Key).ToArray());
    }
}
