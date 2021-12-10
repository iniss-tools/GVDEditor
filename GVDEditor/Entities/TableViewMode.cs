using ToolsCore.Tools;

namespace GVDEditor.Entities;

/// <summary>
///     Definuje mód zobrazenia udaju na tabuli.
/// </summary>
public sealed class TableViewMode : Enumeration<TableViewMode>
{
    private TableViewMode(string key, string name) : base(key, name)
    {
    }

    /// <summary>
    ///     Odkaz na seba, pre potreby DataSource.
    /// </summary>
    public TableViewMode This => this;

    /// <summary>
    ///     Skonvertuje mód zobrazenia podla kluca na objekt.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public new static TableViewMode Parse(string s)
    {
        return s switch
        {
            "LVM_Nothing" => Nothing,
            "LVM_Vlak" => Vlak,
            "LVM_VlakZpozdenyPrijezd" => VlakZmeskanyPrichod,
            "LVM_VlakZpozdenyOdjezd" => VlakZmeskanyOdchod,
            "LVM_VlakZpozdeny" => VlakZmeskany,
            "LVM_Text" => Text,
            _ => null
        };
    }

    #region VALUES

#pragma warning disable 1591
    public static readonly TableViewMode Nothing = new("LVM_Nothing", "Prázdna tabuľa");
    public static readonly TableViewMode Vlak = new("LVM_Vlak", "Vlak bez meškania");
    public static readonly TableViewMode VlakZmeskanyPrichod = new("LVM_VlakZpozdenyPrijezd", "Na príchode meškajúci vlak");
    public static readonly TableViewMode VlakZmeskanyOdchod = new("LVM_VlakZpozdenyOdjezd", "Na odchode meškajúci vlak");
    public static readonly TableViewMode VlakZmeskany = new("LVM_VlakZpozdeny", "Na príchode i odchode meškajúci vlak");
    public static readonly TableViewMode Text = new("LVM_Text", "Text");
#pragma warning restore 1591

    #endregion
}