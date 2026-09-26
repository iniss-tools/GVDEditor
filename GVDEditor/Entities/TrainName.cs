using ToolsCore.Tools;

namespace GVDEditor.Entities;

/// <summary>
///     Nazov vlaku zo zvukovej banky (zvuk skupiny s klucom V8).
/// </summary>
/// <remarks>
///     Do grafikonu sa zapisuje kluc zvuku - INISS podla neho hlada nahravku; v rozhrani sa zobrazuje nazov zvuku.
/// </remarks>
/// <param name="Key">Kluc zvuku.</param>
/// <param name="Name">Nazov zvuku.</param>
public sealed record TrainName(string Key, string Name)
{
    /// <inheritdoc />
    public override string ToString() => Name;

    /// <summary>
    ///     Vrati text, ktory sa ma zobrazit pre nazov vlaku zapisany v grafikone (kluc zvuku -> nazov zvuku).
    /// </summary>
    /// <param name="names">nazvy vlakov zo zvukovej banky.</param>
    /// <param name="stored">nazov vlaku z grafikonu.</param>
    /// <returns>nazov zvuku, alebo povodny text, ak taky kluc v banke nie je.</returns>
    public static string ToDisplay(IEnumerable<TrainName> names, string stored) =>
        names.FirstOrDefault(n => n.Key.EqualsIgnoreCase(stored))?.Name ?? stored;

    /// <summary>
    ///     Vrati hodnotu, ktora sa zapise do grafikonu pre text zadany v rozhrani.
    /// </summary>
    /// <remarks>
    ///     Najprv sa hlada podla nazvu (ten je v zozname vidno), potom podla kluca; iny text sa zapise tak, ako je.
    /// </remarks>
    /// <param name="names">nazvy vlakov zo zvukovej banky.</param>
    /// <param name="text">text zo zoznamu nazvov vlakov.</param>
    /// <returns>kluc zvuku, alebo povodny text, ak nazov v banke nie je.</returns>
    public static string ToStored(IEnumerable<TrainName> names, string text)
    {
        var list = names as IList<TrainName> ?? names.ToList();
        return (list.FirstOrDefault(n => n.Name.EqualsIgnoreCase(text)) ?? list.FirstOrDefault(n => n.Key.EqualsIgnoreCase(text)))?.Key ?? text;
    }
}
