using GVDEditor.Domain.Entities;

namespace GVDEditor.Domain.Analysis;

/// <summary>
/// Polozka grafikonu s cislom pisma, ktore by tabula ELEN dostala ako riadiaci znak.
/// </summary>
/// <param name="Source">odkial sa pismo posiela</param>
/// <param name="Item">polozka, ktora sa opravuje (<see cref="TableFont" />, <see cref="TableCatalog" />,
/// <see cref="TableText" />, <see cref="TableTabTab" />)</param>
/// <param name="Name">nazov alebo kluc polozky</param>
/// <param name="Detail">prve zle miesto polozky - stlpec, vlak, cislo riadka TabTab; pri pisme zo zoznamu null</param>
/// <param name="FontId">cislo pisma na prvom zlom mieste</param>
/// <param name="Count">pocet zlych miest polozky</param>
internal sealed record ElenFontFinding(ElenFontSource Source, object Item, string Name, string? Detail, int FontId, int Count);
