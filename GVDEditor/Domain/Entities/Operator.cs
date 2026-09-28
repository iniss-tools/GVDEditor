namespace GVDEditor.Domain.Entities;

/// <summary>
///     Dopravca vlaku (operátor).
/// </summary>
/// <param name="id">identifikátor dopravcu</param>
/// <param name="name">názov dopravcu</param>
/// <remarks>Entita s identitou - vlaky sa odkazujú na inštanciu zo zoznamu dopravcov, porovnáva sa referenciou.</remarks>
public sealed class Operator(int id, string name)
{
    /// <summary>
    ///     Predvolený dopravca.
    /// </summary>
    public static readonly Operator None = new(-1, "Žiadny");

    /// <summary>
    ///     Identifikátor dopravcu.
    /// </summary>
    public int Id { get; } = id;

    /// <summary>
    ///     Názov dopravcu.
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    ///     This.
    /// </summary>
    public Operator This => this;

    /// <summary>
    ///     Vráti dopravcu zo zadaného listu podľa identifikátora dopravcu.
    /// </summary>
    /// <param name="operators">list dopravcov</param>
    /// <param name="id">identifikátor dopravcu</param>
    /// <returns>dopravcu, ak ho nenájde, vráti <see langword="null" /></returns>
    public static Operator? GetFromID(IEnumerable<Operator> operators, int id) => 
        operators.FirstOrDefault(dopravca => dopravca.Id == id);

    /// <summary>
    ///     Vráti dopravcu zo zadaného listu podľa názvu dopravcu.
    /// </summary>
    /// <param name="operators">list dopravcov</param>
    /// <param name="name">názov dopravcu</param>
    /// <returns>dopravcu, ak ho nenájde, vráti <see langword="null" /></returns>
    public static Operator? GetFromName(IEnumerable<Operator> operators, string name) => 
        operators.FirstOrDefault(dopravca => dopravca.Name == name);

    /// <inheritdoc />
    public override string ToString() => Name;
}