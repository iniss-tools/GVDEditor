namespace GVDEditor.Entities;

/// <summary>
///     Nástupište na stanici.
/// </summary>
/// <param name="Key">kľúč nástupišťa</param>
/// <param name="FullName">celý názov nástupišťa</param>
/// <param name="SoundName">názov zvuku nástupišťa (bez prípony)</param>
public sealed record Platform(string Key, string FullName, string SoundName)
{
    /// <summary>
    ///     Predvolené nástupište.
    /// </summary>
    public static readonly Platform None = new("N", "Nedefinované", "");

    /// <summary>
    ///     Kľúč nástupišťa.
    /// </summary>
    public string Key { get; set; } = Key;

    /// <summary>
    ///     Celý názov nástupišťa.
    /// </summary>
    public string FullName { get; set; } = FullName;

    /// <summary>
    ///     Názov zvuku nástupišťa (bez prípony).
    /// </summary>
    public string SoundName { get; set; } = SoundName;

    /// <summary>
    ///     This
    /// </summary>
    public Platform This => this;

    /// <summary>
    ///     Porovná nástupišťa (porovná iba ich kľúče).
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool EqualsKeys(Platform other) => other.Key == Key;

    /// <inheritdoc />
    public override string ToString() => Key;
}