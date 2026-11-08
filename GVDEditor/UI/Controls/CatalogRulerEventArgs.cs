namespace GVDEditor.UI.Controls;

/// <summary>
/// Udalost kliknutia na stlpec pravitka - poradie stlpca v tabuli.
/// </summary>
public sealed class CatalogRulerEventArgs(int column) : EventArgs
{
    /// <summary>
    /// Poradie stlpca v katalogovej tabuli.
    /// </summary>
    public int Column { get; } = column;
}
