namespace GVDEditor.Entities;

/// <summary>
///     Text hlásenia založený obsluhou INISSu (položka <c>SLogTextConstReport</c> zo súboru RAWBANK\LogZvuk.usr):
///     výluka (<c>V</c>), odklon (<c>O</c>) alebo dodatok (<c>D</c>) s vlastným názvom, predlohou a dosadenými stanicami.
/// </summary>
/// <param name="Key">Kľúč položky, napr. <c>V1000001</c>. Písmeno určuje druh, číslo je kód zapisovaný do dátových súborov.</param>
/// <param name="Name">Názov zadaný obsluhou pri založení.</param>
/// <param name="Description">Popis (INISS ho necháva prázdny).</param>
/// <param name="Type">Druh položky: 7 = dodatok, 8 = odklon, 11 = výluka, 12 = výluka vlaku (dyn. def.).</param>
/// <param name="Template">Názov zabudovanej predlohy z LogZvuk.dat, do ktorej sa dosadzujú stanice.</param>
/// <param name="StationIds">Identifikátory dosadených staníc.</param>
public sealed record LogZvukText(string Key, string Name, string Description, int Type, string Template, List<string> StationIds)
{
    public const int TYPE_ADDITION = 7;
    public const int TYPE_DIVERSION = 8;
    public const int TYPE_LOCKOUT = 11;
    public const int TYPE_LOCKOUT_DYNAMIC = 12;

    /// <summary>
    ///     Druh položky podľa písmena kľúča: <c>V</c> výluka, <c>O</c> odklon, <c>D</c> dodatok.
    /// </summary>
    public char Kind => Key.Length > 0 ? char.ToUpperInvariant(Key[0]) : '?';

    /// <summary>
    ///     Číselný kód z kľúča (časť za písmenom) - hodnota, ktora sa zapisuje do Vyluka.TXT, Vyluky1.TXT alebo Odklony1.TXT.
    /// </summary>
    public int Code => Key.Length > 1 && int.TryParse(Key.AsSpan(1), out var code) ? code : 0;

    /// <summary>
    ///     Je to výluka (kľúč <c>V</c>).
    /// </summary>
    public bool IsLockout => Kind == 'V';

    /// <summary>
    ///     Názvy dosadených staníc (ak stanica nie je známa, jej identifikátor).
    /// </summary>
    public IEnumerable<string> StationNames => StationIds.Select(id => Station.GetFromID(id).Name);

    public override string ToString() => $"{Code} – {Name}";
}
