namespace GVDEditor.Entities;

/// <summary>
///     Definuje spravanie obsahu sekcie katalogovej tabule.
/// </summary>
public sealed record TableTabTab
{
    /// <summary>
    ///     Predvolený TabTab - žiadny.
    /// </summary>
    public static readonly TableTabTab Empty = new() { Key = "Žiadny", Text = "" };

    /// <summary>
    ///     Kluc TabTab.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    ///     Obsah TabTab ako text.
    /// </summary>
    public string Text { get; set; }

    /// <inheritdoc />
    public override string ToString() => Key;
}