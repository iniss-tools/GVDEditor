// ReSharper disable UnusedMemberInSuper.Global
namespace GVDEditor.Entities;

/// <summary>
///     Rozhranie pre všetky triedy tabuľovitého typu s klucom a nazvom.
/// </summary>
public interface ITable
{
    /// <summary>
    ///     Kluc tabule.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    ///     Nazov tabule.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Typ tabule ako retazec.
    /// </summary>
    public string TypeName { get; }
}