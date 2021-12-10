namespace GVDEditor.Entities;

/// <summary>
///     Informácie o grafikone
/// </summary>
public sealed record GVDInfo
{
    /// <summary>
    ///     Kategória
    /// </summary>
    public int Category { get; set; }

    /// <summary>
    ///     Podkategória
    /// </summary>
    public int Subcat { get; set; }

    /// <summary>
    ///     Reprezentuje stanicu, pre ktorú bol grafikon robený
    /// </summary>
    public Station ThisStation { get; set; }

    /// <summary>
    ///     Počet vlakov
    /// </summary>
    public int TrainCount { get; set; }

    /// <summary>
    ///     Začiatok platnosti rozvrhu vlakov
    /// </summary>
    public DateTime StartValidTimeTable { get; set; }

    /// <summary>
    ///     Koniec platnosti rozvrhu vlakov
    /// </summary>
    public DateTime EndValidTimeTable { get; set; }

    /// <summary>
    ///     Začiatok platnosti dát
    /// </summary>
    public DateTime StartValidData { get; set; }

    /// <summary>
    ///     Koniec platnosti dát
    /// </summary>
    public DateTime EndValidData { get; set; }

    /// <summary>
    ///     Dátum vytvorenia/úpravy grafikonu
    /// </summary>
    public DateTime CreateData { get; set; }

    /// <summary>
    ///     TTIndex
    /// </summary>
    public int TTIndex { get; set; }

    /// <summary>
    ///     VLIndex
    /// </summary>
    public int VLIndex { get; set; }

    /// <summary>
    ///     STIndex
    /// </summary>
    public int STIndex { get; set; }

    /// <summary>
    ///     IsRegionText
    /// </summary>
    public bool IsRegionText { get; set; }

    /// <summary>
    ///     OnlyCityVLIndex
    /// </summary>
    public int OnlyCityVLIndex { get; set; }
}