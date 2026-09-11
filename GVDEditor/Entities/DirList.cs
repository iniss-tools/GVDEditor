namespace GVDEditor.Entities;

/// <summary>
///     Trida obsahujuca informácie o priečinku s grafikonom.
/// </summary>
public sealed class DirList
{
    /// <summary>
    ///     Vrati alebo nastavi názov priečinka.
    /// </summary>
    public string DirName { get; set; } = null!;

    /// <summary>
    ///     Vrati alebo nastavi celá cestu k priečinku s grafikonom.
    /// </summary>
    public string FullPath { get; set; } = null!;

    /// <summary>
    ///     Vrati alebo nastavi port pre vzdialené ovládanie tabúľ.
    /// </summary>
    public int? TablePort { get; set; }

    /// <summary>
    ///     Vrati alebo nastavi port pre zabezpečenie vzdialeného hlásenia.
    /// </summary>
    public int? ReportPort { get; set; }

    /// <summary>
    ///     Vrati alebo nastavi príznaky priečinka (4. stĺpec súboru <c>DirList.TXT</c>).
    /// </summary>
    /// <remarks>
    ///     INISS v poli hľadá jednotlivé znaky bez ohľadu na poradie a veľkosť písmen:
    ///     <c>Z</c> (šírenie externej správy na nasledujúce stanice trasy), <c>O</c> (ktorý
    ///     koľajový údaj správa prepisuje), <c>K</c> (do grafikonu sa smú zakladať vlaky
    ///     z externých správ), <c>M</c> (ako <c>K</c>, navyše sa z priečinka nenačíta
    ///     <c>Categori.TXT</c>) a číslicu <c>1</c>–<c>9</c> (položka je aktívna len pri
    ///     spustení INISSu s prepínačom <c>/N</c>).
    ///     GVDEditor obsah poľa nijako neinterpretuje, iba ho zachováva, aby sa pri uložení
    ///     nestratil.
    /// </remarks>
    public string? Flags { get; set; }

    /// <summary>
    ///     Vrati alebo nastavi farbu zobrazujúcu v INISS ako farba pozadia vlaku
    ///     na pracovnej ploche ako odlíšenie od vlakov iných staníc.
    /// </summary>
    public Color? BackColor { get; set; }
}