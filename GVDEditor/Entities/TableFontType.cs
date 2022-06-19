using ToolsCore.Tools;

namespace GVDEditor.Entities;

/// <summary>
///     Typ písma pre tabule.
/// </summary>
public sealed class TableFontType : Enumeration<TableFontType>
{
    private TableFontType(string key, string name) : base(key, name)
    {
    }

    /// <summary>
    ///     This.
    /// </summary>
    public TableFontType This => this;

    /// <summary>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public new static TableFontType Parse(string type)
    {
        return type.ToLower() switch
        {
            "" => None,
            "tenký" => None,
            "tučný" => Bold,
            "šikmý" => Italics,
            "šikmý a tučný" => BoldItalics,
            "špeciálny" => Special,
            "speciální" => Special,
            _ => null
        };
    }

    #region VALUES

    /// <summary>
    ///     Obyčajné písmo.
    /// </summary>
    public static readonly TableFontType None = new("", "Normálne písmo");

    /// <summary>
    ///     Tučné písmo.
    /// </summary>
    public static readonly TableFontType Bold = new("Tučný", "Tučné písmo");

    /// <summary>
    ///     Šikmé písmo (kurzíva).
    /// </summary>
    public static readonly TableFontType Italics = new("Šikmý", "Šikmé písmo");

    /// <summary>
    ///     Tučné a šikmé písmo.
    /// </summary>
    public static readonly TableFontType BoldItalics = new("Šikmý a tučný", "Šikmé a tučné písmo");

    /// <summary>
    ///     Špeciálne písmo.
    /// </summary>
    public static readonly TableFontType Special = new("Špeciálny", "Špeciálne písmo");

    #endregion
}