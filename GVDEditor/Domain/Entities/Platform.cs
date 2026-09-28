namespace GVDEditor.Domain.Entities;

/// <summary>
/// Nástupište na stanici.
/// </summary>
/// <param name="key">kľúč nástupišťa</param>
/// <param name="fullName">celý názov nástupišťa</param>
/// <param name="soundName">názov zvuku nástupišťa (bez prípony)</param>
/// <remarks>
/// Entita s identitou - koľaje s rovnakým kľúčom nástupišťa zdieľajú jednu inštanciu, porovnáva sa referenciou.
/// </remarks>
public sealed class Platform(string key, string fullName, string soundName)
{
    /// <summary>
    /// Predvolené nástupište.
    /// </summary>
    public static readonly Platform None = new("N", "Nedefinované", "");

    /// <summary>
    /// Kľúč nástupišťa.
    /// </summary>
    public string Key { get; set; } = key;

    /// <summary>
    /// Celý názov nástupišťa.
    /// </summary>
    public string FullName { get; set; } = fullName;

    /// <summary>
    /// Názov zvuku nástupišťa (bez prípony).
    /// </summary>
    public string SoundName { get; set; } = soundName;

    /// <summary>
    /// This
    /// </summary>
    public Platform This => this;

    /// <summary>
    /// Porovná nástupišťa (porovná iba ich kľúče).
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool EqualsKeys(Platform other) => other.Key == Key;

    /// <inheritdoc />
    public override string ToString() => Key;
}