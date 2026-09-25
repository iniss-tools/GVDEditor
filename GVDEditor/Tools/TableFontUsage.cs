using System.Globalization;
using System.Text.RegularExpressions;
using GVDEditor.Entities;

namespace GVDEditor.Tools;

/// <summary>
///     Kde sa v grafikone používa číslo písma tabule. Katalógové tabule a texty vlakov majú číslo písma
///     vo vlastnom poli, TabTab ho má v texte pravidiel ako <c>{n}</c>.
/// </summary>
/// <param name="CatalogColumns">Stĺpce katalógových tabúľ s týmto písmom (TYPE_ITEMS_FONT_IDX).</param>
/// <param name="TrainTexts">Texty vlakov s týmto písmom (TRAIN_nnn_IDX_FONT v TTexts.txt).</param>
/// <param name="TabTabSections">Sekcie TabTab, v ktorých sa vyskytuje kód <c>{n}</c> s týmto číslom.</param>
internal sealed record TableFontUsage(int CatalogColumns, int TrainTexts, IReadOnlyList<string> TabTabSections)
{
    private static readonly Regex CodeInBraces = new(@"\{(\d+)\}", RegexOptions.Compiled);

    public bool IsUsed => CatalogColumns > 0 || TrainTexts > 0 || TabTabSections.Count > 0;

    /// <summary>
    ///     Či sa dá použitie zmeniť automaticky - stĺpce a texty áno, TabTab nie.
    /// </summary>
    public bool HasReplaceable => CatalogColumns > 0 || TrainTexts > 0;

    public static TableFontUsage Find(int fontId, IEnumerable<TableCatalog> catalogs, IEnumerable<TableText> texts,
        IEnumerable<TableTabTab> tabTabs)
    {
        var columns = catalogs.Sum(catalog => catalog.Items.Count(item => item.FontIDX == fontId));
        var trainTexts = texts.Sum(text => text.Trains.Count(train => train.FontID == fontId));

        // {n} v TabTab môže byť kód písma aj kód znaku - preto sa len hlási, nemení sa
        var sections = tabTabs
            .Where(tab => tab != TableTabTab.Empty && ContainsCode(tab.Text, fontId))
            .Select(tab => tab.Key)
            .ToList();

        return new TableFontUsage(columns, trainTexts, sections);
    }

    /// <summary>
    ///     Zmení číslo písma v stĺpcoch katalógových tabúľ a v textoch vlakov. TabTab sa nemení.
    /// </summary>
    /// <returns>Počet zmenených miest.</returns>
    public static int Replace(int oldId, int newId, IEnumerable<TableCatalog> catalogs, IEnumerable<TableText> texts)
    {
        var changed = 0;

        foreach (var item in catalogs.SelectMany(catalog => catalog.Items).Where(item => item.FontIDX == oldId))
        {
            item.FontIDX = newId;
            changed++;
        }

        foreach (var train in texts.SelectMany(text => text.Trains).Where(train => train.FontID == oldId))
        {
            train.FontID = newId;
            changed++;
        }

        return changed;
    }

    private static bool ContainsCode(string? text, int code) =>
        !string.IsNullOrEmpty(text) && CodeInBraces.Matches(text).Any(match =>
            int.TryParse(match.Groups[1].Value, NumberStyles.None, CultureInfo.InvariantCulture, out var value) && value == code);
}
