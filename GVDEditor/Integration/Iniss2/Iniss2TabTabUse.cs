using ToolsCore.Iniss.Expressions;

namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Predloha, ktora tabulku TabTab pouziva ako prvu tabulku stlpca - urcuje vyhodnotenie pravidiel ako v INISSe 3.39.
/// </summary>
/// <param name="CatalogKey">kluc predlohy INISS2</param>
/// <param name="Site">miesto vyhodnotenia (prichodova tabula, ina)</param>
/// <param name="MergeByParts">tabula strieda casti textu - <c>#MERGE</c> spaja po castiach</param>
internal sealed record Iniss2TabTabUse(string CatalogKey, ExprEvalSite Site, bool MergeByParts);
