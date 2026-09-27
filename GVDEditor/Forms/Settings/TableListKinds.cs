using System.Globalization;
using GVDEditor.Entities;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.Forms.Settings;

/// <summary>
///     Co zobrazuje a ako upravuje zoznam na strankach tabul (<see cref="TablesPage" />): polozky, stlpce, dialog
///     na upravu a kde sa polozka pouziva. Kazdy druh tabul (fyzicke, logicke, katalogove, texty, TabTab) ma vlastnu
///     podtriedu.
/// </summary>
internal abstract class TableListKind
{
    /// <summary>Popis stranky nad zoznamom.</summary>
    public abstract string Info { get; }

    /// <summary>Text tlacidla na pridanie.</summary>
    public abstract string AddText { get; }

    /// <summary>Text tlacidla na upravu.</summary>
    public virtual string EditText => Resources.TablesPage_Upravit;

    /// <summary>
    ///     Pridanie aj uprava idu cez jedno tlacidlo (editor TabTab) - pri vybranej polozke otvori ju, inak prazdny editor.
    /// </summary>
    public virtual bool SingleButton => false;

    /// <summary>Ci sa da polozka duplikovat.</summary>
    public virtual bool CanDuplicate => true;

    /// <summary>Nadpis stlpca s podrobnostou.</summary>
    public abstract string DetailHeader { get; }

    /// <summary>Ci zobrazit stlpec Kluc.</summary>
    public virtual bool ShowKey => true;

    /// <summary>Ci zobrazit stlpec Komentar.</summary>
    public virtual bool ShowComment => true;

    /// <summary>Ci pouzita polozka nejde odstranit (inak by odkaz na nu ostal prazdny).</summary>
    public virtual bool UsageBlocksDelete => true;

    /// <summary>Text, ked sa polozka nikde nepouziva.</summary>
    public virtual string NoUsage => Resources.TablesPage_Nepouziva;

    /// <summary>Polozky zoznamu v poradi.</summary>
    public abstract IEnumerable<object> Items { get; }

    public abstract string Name(object item);

    public virtual string Key(object item) => "";

    public abstract string Detail(object item);

    public virtual string Comment(object item) => "";

    /// <summary>Prida polozku (dialog); vrati novu polozku alebo <see langword="null" />.</summary>
    public abstract object? Add(IWin32Window owner);

    /// <summary>Prida kopiu polozky (dialog); vrati novu polozku alebo <see langword="null" />.</summary>
    public virtual object? Duplicate(IWin32Window owner, object item) => null;

    /// <summary>Upravi polozku (dialog); vrati upravenu polozku (moze byt nahradena novym objektom) alebo <see langword="null" />.</summary>
    public abstract object? Edit(IWin32Window owner, object item);

    /// <summary>Miesta, kde sa polozka pouziva.</summary>
    public abstract IReadOnlyList<string> Usage(object item);

    /// <summary>Odstrani polozku zo zoznamu.</summary>
    public abstract void Remove(object item);
}

/// <summary>
///     Fyzicke tabule - pouzivaju ich pozicie logickych tabul.
/// </summary>
internal sealed class PhysicalTablesKind : TableListKind
{
    public override string Info => Resources.TablesPage_Fyzicke_Info;
    public override string AddText => Resources.TablesPage_Nova_tabula;
    public override string DetailHeader => Resources.TablesPage_Stlpec_Katalog;
    public override IEnumerable<object> Items => GlobData.TablePhysicals;
    public override string Name(object item) => ((TablePhysical)item).Name;
    public override string Key(object item) => ((TablePhysical)item).Key;
    public override string Detail(object item) => ((TablePhysical)item).TableCatalog?.Name ?? "";
    public override string Comment(object item) => ((TablePhysical)item).Comment;

    public override object? Add(IWin32Window owner)
    {
        if (GlobData.TableCatalogs.Count == 0)
        {
            Utils.ShowError(Resources.FTablePhysical_NoCatalog);
            return null;
        }

        using var form = new FTablePhysical(new TablePhysical(), GlobData.TableCatalogs);
        if (form.ShowDialog(owner) != DialogResult.OK)
            return null;

        GlobData.TablePhysicals.Add(form.ThisTable);
        return form.ThisTable;
    }

    public override object? Duplicate(IWin32Window owner, object item)
    {
        using var form = new FTablePhysical((TablePhysical)item, GlobData.TableCatalogs, true);
        if (form.ShowDialog(owner) != DialogResult.OK)
            return null;

        GlobData.TablePhysicals.Add(form.ThisTable);
        return form.ThisTable;
    }

    public override object? Edit(IWin32Window owner, object item)
    {
        using var form = new FTablePhysical((TablePhysical)item, GlobData.TableCatalogs);
        if (form.ShowDialog(owner) != DialogResult.OK)
            return null;

        GlobData.TablePhysicals.ResetBindings();
        return item;
    }

    public override IReadOnlyList<string> Usage(object item)
    {
        var usage = new List<string>();
        foreach (var logical in GlobData.TableLogicals)
        foreach (var record in logical.Records)
        foreach (TablePosition position in record)
            if (ReferenceEquals(position.Table, item))
                usage.Add(string.Format(CultureInfo.CurrentCulture, Resources.TablesPage_Pouzitie_Logicka, logical.Name, position.Position));
        return usage;
    }

    public override void Remove(object item) => GlobData.TablePhysicals.Remove((TablePhysical)item);
}

/// <summary>
///     Logicke tabule - priradene su kolajam.
/// </summary>
internal sealed class LogicalTablesKind(Station station) : TableListKind
{
    public override string Info => Resources.TablesPage_Logicke_Info;
    public override string AddText => Resources.TablesPage_Nova_tabula;
    public override string DetailHeader => Resources.TablesPage_Stlpec_Rezim;
    public override IEnumerable<object> Items => GlobData.TableLogicals;
    public override string Name(object item) => ((TableLogical)item).Name;
    public override string Key(object item) => ((TableLogical)item).Key;
    public override string Detail(object item) => ((TableLogical)item).ViewType?.Name ?? "";
    public override string Comment(object item) => ((TableLogical)item).Comment;

    public override object? Add(IWin32Window owner)
    {
        using var form = new FTableLogical(new TableLogical(), GlobData.TablePhysicals, thisStation: station);
        if (form.ShowDialog(owner) != DialogResult.OK)
            return null;

        GlobData.TableLogicals.Add(form.ThisTable);
        return form.ThisTable;
    }

    public override object? Duplicate(IWin32Window owner, object item)
    {
        using var form = new FTableLogical((TableLogical)item, GlobData.TablePhysicals, true, station);
        if (form.ShowDialog(owner) != DialogResult.OK)
            return null;

        GlobData.TableLogicals.Add(form.ThisTable);
        return form.ThisTable;
    }

    public override object? Edit(IWin32Window owner, object item)
    {
        using var form = new FTableLogical((TableLogical)item, GlobData.TablePhysicals, thisStation: station);
        if (form.ShowDialog(owner) != DialogResult.OK)
            return null;

        GlobData.TableLogicals.ResetBindings();
        return item;
    }

    public override IReadOnlyList<string> Usage(object item) =>
        GlobData.Tracks.Where(track => track.Tables.Any(t => ReferenceEquals(t, item)))
            .Select(track => string.Format(CultureInfo.CurrentCulture, Resources.TablesPage_Pouzitie_Kolaj, track.Name))
            .ToList();

    public override void Remove(object item) => GlobData.TableLogicals.Remove((TableLogical)item);
}

/// <summary>
///     Katalogove tabule - predlohy fyzickych tabul a realizacii textov.
/// </summary>
internal sealed class CatalogTablesKind : TableListKind
{
    public override string Info => Resources.TablesPage_Katalogove_Info;
    public override string AddText => Resources.TablesPage_Nova_tabula;
    public override string DetailHeader => Resources.TablesPage_Stlpec_Vyrobca;
    public override IEnumerable<object> Items => GlobData.TableCatalogs;
    public override string Name(object item) => ((TableCatalog)item).Name;
    public override string Key(object item) => ((TableCatalog)item).Key;
    public override string Detail(object item) => ((TableCatalog)item).Manufacturer?.Name ?? "";
    public override string Comment(object item) => ((TableCatalog)item).Comment;

    public override object? Add(IWin32Window owner)
    {
        using var form = new FTableCatalog(new TableCatalog(), GlobData.TabTabs.ToList());
        if (form.ShowDialog(owner) != DialogResult.OK)
            return null;

        GlobData.TableCatalogs.Add(form.ThisTable);
        return form.ThisTable;
    }

    public override object? Duplicate(IWin32Window owner, object item)
    {
        using var form = new FTableCatalog((TableCatalog)item, GlobData.TabTabs, true);
        if (form.ShowDialog(owner) != DialogResult.OK)
            return null;

        GlobData.TableCatalogs.Add(form.ThisTable);
        return form.ThisTable;
    }

    public override object? Edit(IWin32Window owner, object item)
    {
        using var form = new FTableCatalog((TableCatalog)item, GlobData.TabTabs);
        if (form.ShowDialog(owner) != DialogResult.OK)
            return null;

        GlobData.TableCatalogs.ResetBindings();
        return item;
    }

    public override IReadOnlyList<string> Usage(object item)
    {
        var usage = GlobData.TablePhysicals.Where(p => ReferenceEquals(p.TableCatalog, item))
            .Select(p => string.Format(CultureInfo.CurrentCulture, Resources.TablesPage_Pouzitie_Fyzicka, p.Name))
            .ToList();
        foreach (var text in GlobData.TableTexts)
        foreach (var realization in text.Realizations)
            if (ReferenceEquals(realization.Table, item))
                usage.Add(string.Format(CultureInfo.CurrentCulture, Resources.TablesPage_Pouzitie_Text, text.Name,
                    realization.Item?.Name));
        return usage;
    }

    public override void Remove(object item) => GlobData.TableCatalogs.Remove((TableCatalog)item);
}

/// <summary>
///     Texty na tabuliach - typy textov s realizaciami a textami vlakov.
/// </summary>
internal sealed class TableTextsKind(GVDInfo gvd) : TableListKind
{
    public override string Info => Resources.TablesPage_Texty_Info;
    public override string AddText => Resources.TablesPage_Novy_text;
    public override bool CanDuplicate => false;
    public override string DetailHeader => Resources.TablesPage_Stlpec_Vlakov;
    public override bool UsageBlocksDelete => false;
    public override string NoUsage => Resources.TablesPage_Texty_Bez_realizacie;
    public override IEnumerable<object> Items => GlobData.TableTexts;
    public override string Name(object item) => ((TableText)item).Name;
    public override string Key(object item) => ((TableText)item).Key;
    public override string Detail(object item) => ((TableText)item).Trains.Count.ToString(CultureInfo.CurrentCulture);
    public override string Comment(object item) => ((TableText)item).Comment;

    public override object? Add(IWin32Window owner)
    {
        using var form = new FTableText(new TableText(), GlobData.TableCatalogs, gvd, -1);
        if (form.ShowDialog(owner) != DialogResult.OK)
            return null;

        GlobData.TableTexts.Add(form.ThisTableText);
        return form.ThisTableText;
    }

    public override object? Edit(IWin32Window owner, object item)
    {
        var index = GlobData.TableTexts.IndexOf((TableText)item);
        using var form = new FTableText((TableText)item, GlobData.TableCatalogs, gvd, index);
        if (form.ShowDialog(owner) != DialogResult.OK)
            return null;

        // dialog vracia novy objekt textu - nahradi povodny na tom istom mieste
        GlobData.TableTexts.RemoveAt(index);
        GlobData.TableTexts.Insert(index, form.ThisTableText);
        return form.ThisTableText;
    }

    public override IReadOnlyList<string> Usage(object item) =>
        ((TableText)item).Realizations
        .Select(r => string.Format(CultureInfo.CurrentCulture, Resources.TablesPage_Realizacia, r.Table?.Name, r.Item?.Name))
        .ToList();

    public override void Remove(object item) => GlobData.TableTexts.Remove((TableText)item);
}

/// <summary>
///     Sekcie TabTab - upravuju sa v editore TabTab, pouzivaju ich stlpce katalogovych tabul.
/// </summary>
internal sealed class TabTabKind(Station station) : TableListKind
{
    public override string Info => Resources.TablesPage_TabTab_Info;
    public override string AddText => Resources.TablesPage_TabTab_Editor;
    public override bool SingleButton => true;
    public override bool CanDuplicate => false;
    public override string DetailHeader => Resources.TablesPage_Stlpec_Riadkov;
    public override bool ShowKey => false;
    public override bool ShowComment => false;
    public override IEnumerable<object> Items => GlobData.TabTabs.Where(tab => tab != TableTabTab.Empty);
    public override string Name(object item) => ((TableTabTab)item).Key;

    public override string Detail(object item) =>
        (((TableTabTab)item).Text ?? "").Split('\n').Count(line => line.Trim().Length > 0).ToString(CultureInfo.CurrentCulture);

    public override object? Add(IWin32Window owner)
    {
        using var form = new FTabTab(null, station);
        form.ShowDialog(owner);
        return null;
    }

    public override object? Edit(IWin32Window owner, object item)
    {
        using var form = new FTabTab((TableTabTab)item, station);
        form.ShowDialog(owner);
        return item;
    }

    public override IReadOnlyList<string> Usage(object item) =>
        Tools.TabTabSections.FindUsage((TableTabTab)item, GlobData.TableCatalogs);

    public override void Remove(object item) => GlobData.TabTabs.Remove((TableTabTab)item);
}
