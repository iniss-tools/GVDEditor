using GVDEditor.Domain.Entities;
using GVDEditor.Mcp;
using ModelContextProtocol;

namespace GVDEditor.Tests.Mcp;

/// <summary>Nastroje MCP nad tabulami a pismami: detail, zmeny s kontrolou a mazanie len nepouzitych.</summary>
public sealed partial class McpToolsTests
{
    [TestMethod]
    public async Task Tabule_DetailAMoznosti()
    {
        var editor = Editor();

        var catalog = await TableTools.GetTable(editor, "Catalog", "k16");
        var physical = await TableTools.GetTable(editor, "physical", "T1");
        var font = await TableTools.GetTable(editor, "font", "113");
        var options = TableTools.GetTableOptions();

        Assert.AreEqual("catalog", catalog.Kind);
        CollectionAssert.AreEqual(new[] { "Druh", "Ciel" }, catalog.Catalog!.Columns.Select(c => c.Key).ToArray());
        CollectionAssert.AreEqual(new[] { "CIEL" }, catalog.Catalog.Columns[1].Texts.ToArray());
        Assert.AreEqual(("DRUH", 3), (catalog.Catalog.Columns[0].Tab1, catalog.Catalog.Columns[0].DivType));
        CollectionAssert.Contains(catalog.UsedBy.ToArray(), "physical board T1");
        Assert.AreEqual("K16", physical.Physical!.Catalog);
        Assert.AreEqual("Piktogramy", font.Font!.Name);
        CollectionAssert.Contains(font.UsedBy.ToArray(), "TabTab section DRUH ({113})");
        Assert.HasCount(6, options.ViewModes);
        Assert.IsTrue(options.Manufacturers.Any(m => m.Value == "ELEN16"));
        var kind = await Assert.ThrowsExactlyAsync<McpException>(() => TableTools.GetTable(editor, "tabula", "K16"));
        StringAssert.StartsWith(kind.Message, "table.invalidValue:");
        var missing = await Assert.ThrowsExactlyAsync<McpException>(() => TableTools.GetTable(editor, "logical", "L1"));
        StringAssert.StartsWith(missing.Message, "table.notFound:");
    }

    [TestMethod]
    public async Task Pismo_NoveCisloAMazanieLenNepouziteho()
    {
        var editor = Editor(out var context);

        var created = await TableEditTools.SetFont(editor, 81, name: "Tenké červené", size: 9);
        var renumbered = await TableEditTools.SetFont(editor, 113, newId: 114);
        var duplicate = await Assert.ThrowsExactlyAsync<McpException>(() => TableEditTools.SetFont(editor, 81, newId: 114));
        var used = await Assert.ThrowsExactlyAsync<McpException>(() => TableEditTools.DeleteTable(editor, "font", "114"));
        var deleted = await TableEditTools.DeleteTable(editor, "font", "81");

        Assert.AreEqual(("Tenké červené", 9, true), (created.Table!.Font!.Name, created.Table.Font.Size, created.Table.Font.Uppercase));
        StringAssert.EndsWith(created.Change.UndoStep, "(agent)");
        Assert.AreEqual(114, context.Document.TableTexts[0].Trains[0].FontID, "text vlaku ide s pismom");
        StringAssert.Contains(renumbered.Notes.Single(), "DRUH");
        StringAssert.StartsWith(duplicate.Message, "table.invalid:");
        StringAssert.StartsWith(used.Message, "table.inUse:");
        Assert.IsTrue(deleted.Change.Changed);
        CollectionAssert.AreEqual(new[] { 114 }, context.Document.TableFonts.Select(f => f.FontID).ToArray());
    }

    [TestMethod]
    public async Task KatalogovaTabula_StlpceSPremenovanimAPoradim()
    {
        var editor = Editor(out var context);
        var ciel = context.Document.TableCatalogs[0].Items[1];
        CatalogColumnInput[] columns =
        [
            new("Druh", "Druh", 0, 48, 0, Content: 15, DivType: 3, Tab1: "druh"),
            new("Ciel2", "Cieľová", 48, 200, 113, Content: 8, PreviousKey: "Ciel")
        ];

        var renamed = await TableEditTools.SetCatalogTable(editor, "K16", columns: columns,
            columnOrder: [new ColumnOrderInput("Tabule_Odjezdova", ["Druh", "Ciel2"], [new ViewModeInput("LVM_Nothing", [])])]);
        var removed = await Assert.ThrowsExactlyAsync<McpException>(() => TableEditTools.SetCatalogTable(editor, "K16", columns: [columns[0]]));
        var unknown = await Assert.ThrowsExactlyAsync<McpException>(() =>
            TableEditTools.SetCatalogTable(editor, "K16", columnOrder: [new ColumnOrderInput("Tabule_Odjezdova", ["Neznamy"])]));
        var copy = await TableEditTools.SetCatalogTable(editor, "K20", copyFrom: "K16", name: "ELEN 20", manufacturer: "elen16", maxRecords: 3);

        Assert.AreSame(ciel, context.Document.TableCatalogs[0].Items[1], "texty ukazuju na ten isty stlpec");
        Assert.AreEqual(("Ciel2", 113), (ciel.Key, ciel.FontIdx));
        var order = renamed.Table!.Catalog!.ColumnOrder.Single();
        Assert.HasCount(6, order.Modes);
        Assert.IsEmpty(order.Modes.Single(m => m.Mode == "LVM_Nothing").Columns);
        CollectionAssert.AreEqual(new[] { "Druh", "Ciel2" }, order.Modes.Single(m => m.Mode == "LVM_Vlak").Columns.ToArray());
        StringAssert.Contains(removed.Message, "CIEL");
        StringAssert.StartsWith(unknown.Message, "table.invalid:");
        Assert.HasCount(2, context.Document.TableCatalogs[0].Items, "chyba vratila stlpce");
        Assert.AreEqual(("ELEN 20", "ELEN16", 3), (copy.Table!.Catalog!.Name, copy.Table.Catalog.Manufacturer, copy.Table.Catalog.Rows.Count));
        Assert.AreNotSame(context.Document.TableCatalogs[0].Items[0], context.Document.TableCatalogs[1].Items[0], "kopia ma vlastne stlpce");
    }

    [TestMethod]
    public async Task FyzickaALogickaTabula_ZostavaKolajeAPouzitie()
    {
        var editor = Editor(out var context);

        var physical = await TableEditTools.SetPhysicalTable(editor, "T2", catalog: "K16", address: 5, line: 2);
        var logical = await TableEditTools.SetLogicalTable(editor, "L1", name: "Odchody", records: 3, layout: [new LayoutRowInput("T1", 1, 3)]);
        var longer = await TableEditTools.SetLogicalTable(editor, "L1", records: 4);
        var invalid = await Assert.ThrowsExactlyAsync<McpException>(() =>
            TableEditTools.SetLogicalTable(editor, "L1", layout: [new LayoutRowInput("T2", 1, 6)]));
        // vlastna kolaj - kolaje grafikonu z testu su zdielane
        var platform = new Track("5", "5", "Koľaj 5", Platform.None, "", "5");
        context.Document.Tracks.Add(platform);
        var track = await TableEditTools.SetTrackTables(editor, "5", ["l1"]);
        platform.TablePriorities["L1"] = 2;
        await TableEditTools.SetLogicalTable(editor, "L1", newKey: "ODCH");
        var logicalUsed = await Assert.ThrowsExactlyAsync<McpException>(() => TableEditTools.DeleteTable(editor, "logical", "ODCH"));
        var physicalUsed = await Assert.ThrowsExactlyAsync<McpException>(() => TableEditTools.DeleteTable(editor, "physical", "T1"));
        var catalogUsed = await Assert.ThrowsExactlyAsync<McpException>(() => TableEditTools.DeleteTable(editor, "catalog", "K16"));
        await TableEditTools.DeleteTable(editor, "physical", "T2");

        Assert.AreEqual((5, 2, 0), (physical.Table!.Physical!.Address, physical.Table.Physical.Line, physical.Table.Physical.Records));
        Assert.AreEqual(new LayoutRowInfo("T1", 1, 3, 1, "Tabule_Odjezdova"), logical.Table!.Logical!.Layout!.Single());
        Assert.AreEqual(4, longer.Table!.Logical!.Layout!.Single().LastRecord, "riadok konciaci poslednym zaznamom sa predlzi");
        StringAssert.StartsWith(invalid.Message, "table.invalid:");
        CollectionAssert.AreEqual(new[] { "L1" }, track.Track.Tables.ToArray());
        Assert.AreEqual(2, platform.TablePriorities["ODCH"], "priorita ide s klucom");
        StringAssert.Contains(logicalUsed.Message, "track 5");
        StringAssert.Contains(physicalUsed.Message, "logical board ODCH");
        StringAssert.Contains(catalogUsed.Message, "board text CIEL");
        Assert.HasCount(1, context.Document.TablePhysicals);
    }
}
