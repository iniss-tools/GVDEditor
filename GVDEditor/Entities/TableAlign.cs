using ToolsCore.Tools;

namespace GVDEditor.Entities;

/// <summary>
///     Zarovnanie tabule
/// </summary>
public sealed class TableAlign : Enumeration<TableAlign>
{
    private TableAlign(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    ///     Konveruje identifikátor tabule ako <see cref="int" /> na <see cref="TableAlign" />
    /// </summary>
    /// <param name="s"></param>
    /// <returns><see cref="TableAlign" /> alebo <see langword="null" /> ak sa nenašla žiadna zhoda</returns>
    public static TableAlign Parse(int s)
    {
        return s switch
        {
            0 => Left,
            1 => Center,
            2 => Right,
            _ => null
        };
    }

    #region VALUES

    /// <summary>
    ///     Zarovnanie vľavo
    /// </summary>
    public static readonly TableAlign Left = new(0, "Vľavo");

    /// <summary>
    ///     Zarovnanie na stred
    /// </summary>
    public static readonly TableAlign Center = new(1, "V strede");

    /// <summary>
    ///     Zarovnanie vpravo
    /// </summary>
    public static readonly TableAlign Right = new(2, "Vpravo");

    #endregion
}