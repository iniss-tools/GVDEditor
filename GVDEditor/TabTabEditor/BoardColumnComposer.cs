using GVDEditor.Domain.Documents;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.TabTab;

namespace GVDEditor.TabTabEditor;

/// <summary>
/// Sklada text stlpca katalogovej tabule pre vlak tak ako INISS (<see cref="TabTabComposer" />): miesto vyhodnotenia podla
/// tabule, text z TTexts.txt a <c>%stlpec%</c> z inych stlpcov. Spolocne pre nahlad editora TabTab a server MCP.
/// </summary>
/// <param name="document">grafikon (TTexts, katalogove tabule)</param>
/// <param name="section">sekcia TabTab podla mena; <see langword="null" />, ak nie je</param>
internal sealed class BoardColumnComposer(GrafikonDocument document, Func<string, TabTabSection?> section)
{
    /// <summary>Najvacsia hlbka <c>%stlpec%</c> - stlpce sa mozu odkazovat dookola.</summary>
    private const int MaxDepth = 3;

    /// <summary>Text stlpca <paramref name="item" /> tabule <paramref name="catalog" /> pre vlak a stav <paramref name="train" />.</summary>
    public TabTabComposeResult Compose(TableCatalog catalog, TableItem item, GvdTrainContext train) => Compose(catalog, item, train, 0);

    /// <summary>INISS berie tabulu ako prichodovu, ked jej prvy stlpec je typu 1 (cas prichodu).</summary>
    public static ExprEvalSite SiteOf(TableCatalog catalog) =>
        catalog.Items.FirstOrDefault() is { } first && first.FillSection == TableFillSection.CasPrichodu ? ExprEvalSite.ArrivalTable : ExprEvalSite.Default;

    private TabTabComposeResult Compose(TableCatalog catalog, TableItem item, GvdTrainContext train, int depth)
    {
        TabTabValue? texts = null;
        foreach (var text in document.TableTexts)
        {
            if (!text.Realizations.Any(r => r.Table == catalog && r.Item == item)) continue;
            if (text.Trains.FirstOrDefault(t => t.Train == train.Train) is { } own)
            {
                texts = new TabTabValue(own.Text, own.FontID);
                break;
            }
        }

        return TabTabComposer.Compose(new TabTabColumnInput
        {
            Train = train,
            Site = SiteOf(catalog),
            OwnValue = train.OwnValue(item),
            TextsValue = texts,
            DivType = item.DivType.Id,
            TypeItemsIdx = item.FillSection.Id,
            Tab1 = item.Tab1 == TableTabTab.Empty ? null : section(item.Tab1.Key),
            Tab2 = item.Tab2 == TableTabTab.Empty ? null : section(item.Tab2.Key),
            ColumnValue = depth < MaxDepth
                ? name =>
                {
                    var other = catalog.Items.FirstOrDefault(i =>
                        string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase) || string.Equals(i.Key, name, StringComparison.OrdinalIgnoreCase));
                    return other is null || other == item ? null : Compose(catalog, other, train, depth + 1).Value;
                }
                : null
        });
    }
}
