namespace GVDEditor.Domain.Editing;

/// <summary>
/// Jeden riadok zostavy logickej tabule: suvisly rozsah zaznamov logickej tabule, ktory ide na jednu fyzicku
/// tabulu od urceneho riadku s jednym typom zobrazenia. Cisla su od 1 ako v okne (zaznam 1 = <c>nnn</c> 001,
/// riadok 1 = <c>POSITION</c> 0).
/// </summary>
public sealed class TableLogicalSegment
{
    /// <summary>
    /// Fyzicka tabula, na ktoru sa zaznamy posielaju.
    /// </summary>
    public TablePhysical Table { get; set; } = null!;

    /// <summary>
    /// Prvy zaznam logickej tabule v rozsahu (od 1).
    /// </summary>
    public int FirstRecord { get; set; }

    /// <summary>
    /// Posledny zaznam logickej tabule v rozsahu (od 1).
    /// </summary>
    public int LastRecord { get; set; }

    /// <summary>
    /// Riadok fyzickej tabule, na ktory ide <see cref="FirstRecord" /> (od 1); dalsie zaznamy idu na nasledujuce riadky.
    /// </summary>
    public int StartRow { get; set; }

    /// <summary>
    /// Typ zobrazenia umiestneni (<c>TYPE_VIEW_KEY_nnn_mmm</c>).
    /// </summary>
    public TableViewType TypeView { get; set; } = null!;

    /// <summary>
    /// Posledny riadok fyzickej tabule, na ktory rozsah siaha (od 1).
    /// </summary>
    public int EndRow => StartRow + LastRecord - FirstRecord;

    /// <summary>
    /// Kopia riadku.
    /// </summary>
    public TableLogicalSegment Clone() => (TableLogicalSegment)MemberwiseClone();
}
