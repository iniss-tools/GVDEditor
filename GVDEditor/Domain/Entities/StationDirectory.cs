using System.Globalization;
using GVDEditor.Properties;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Domain.Entities;

/// <summary>
/// Stanice, ktore pozna grafikon: zo zvukovej banky (skupina R1) a definovane v grafikone (Stanice.txt).
/// Vyhladavanie vracia nove instancie, aby sa priznaky hlasenia v trase vlaku nemenili v zdroji.
/// </summary>
/// <param name="stations">stanice zo zvukovej banky</param>
/// <param name="customStations">stanice definovane v grafikone</param>
public sealed class StationDirectory(IEnumerable<Station> stations, IEnumerable<Station> customStations)
{
    /// <summary>
    /// Prazdny zoznam (bez zvolenej instalacie).
    /// </summary>
    public static StationDirectory Empty { get; } = new([], []);

    /// <summary>
    /// Vsetky stanice - najprv zo zvukovej banky, potom z grafikonu.
    /// </summary>
    public IEnumerable<Station> All => stations.Concat(customStations);

    /// <summary>
    /// Stanica podla ID; ak neexistuje, stanica s nazvom rovnym ID.
    /// </summary>
    public Station FromID(string? id) => Station.GetFromID(id, stations, customStations);

    /// <summary>
    /// Stanica podla nazvu - bez ohladu na velkost pismen, diakritiku, bodky a pomlcky.
    /// </summary>
    /// <returns><see langword="null" />, ak sa nenasla.</returns>
    public Station? FromName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return Station.None;

        var wanted = Normalize(name);
        var found = All.FirstOrDefault(station => Normalize(station.Name) == wanted);
        return found is null ? null : new Station(found.ID, found.Name);
    }

    /// <summary>
    /// Stanice z retazca ID oddelenych ciarkou.
    /// </summary>
    public List<Station> FromIDList(string ids) =>
        ids.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(FromID).ToList();

    /// <summary>
    /// Stanice z retazca nazvov oddelenych ciarkou; nazvy mozu obsahovat medzery (Velka Ves).
    /// </summary>
    /// <exception cref="ArgumentException">ak stanica neexistuje</exception>
    public List<Station> FromNameList(string names) =>
        names.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(name => FromName(name) ?? throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Station_NotFound, name)))
            .ToList();

    private static string Normalize(string name) =>
        StringUtils.RemoveDiacritics(name.Replace(".", "").Replace("-", "").ToLowerInvariant());
}
