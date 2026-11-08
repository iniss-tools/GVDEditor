using System.Globalization;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Rules;

/// <summary>
/// Kontrola fyzickej tabule tak, ako ju INISS nacita.
/// </summary>
internal static class TablePhysicalRules
{
    /// <summary>
    /// Pole fyzickej tabule, ku ktoremu sa chyba viaze.
    /// </summary>
    public enum Field
    {
        Name,
        Key,
        Catalog,
        Id
    }

    /// <summary>
    /// Vsetky chyby tabule na pozicii <paramref name="index" />.
    /// </summary>
    /// <param name="tables">fyzicke tabule</param>
    /// <param name="index">kontrolovana tabula</param>
    /// <param name="catalogs">katalogove tabule, ktore este existuju</param>
    public static List<(Field Field, string Message)> Check(IReadOnlyList<TablePhysical> tables, int index,
        IReadOnlyCollection<TableCatalog> catalogs)
    {
        var table = tables[index];
        var problems = new List<(Field, string)>();

        if (TableRules.CheckName(table.Name) is { } name)
            problems.Add((Field.Name, name));
        if (TableRules.CheckKey(tables.Select(t => t.Key).ToList(), index) is { } key)
            problems.Add((Field.Key, key));

        if (table.TableCatalog is not { } catalog || !catalogs.Contains(catalog))
            problems.Add((Field.Catalog, Resources.TablePhysicalRules_Katalog));
        else if (CheckId(table.ID, catalog.Manufacturer) is { } id)
            problems.Add((Field.Id, id));

        return problems;
    }

    /// <summary>
    /// Adresa tabule mimo rozsahu vyrobcu - INISS taku tabulu nezalozi. -1 (bez adresy) je vzdy v poriadku.
    /// </summary>
    public static string? CheckId(int id, TableManufacturer? manufacturer)
    {
        if (manufacturer is null || !manufacturer.IsKnownToIniss || manufacturer.IsAddressValid(id))
            return null;

        return string.Format(CultureInfo.CurrentCulture, Resources.TablePhysicalRules_Adresa, id, manufacturer.Name,
            manufacturer.MinAddress, manufacturer.MaxAddress);
    }
}
