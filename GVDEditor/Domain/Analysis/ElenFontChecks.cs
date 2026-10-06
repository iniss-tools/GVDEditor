using System.Globalization;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using GVDEditor.UI.Settings;
using ToolsCore.Iniss.Elen;
using ToolsCore.Iniss.TabTab;

namespace GVDEditor.Domain.Analysis;

/// <summary>
/// Miesto, odkial INISS posle tabuli cislo pisma.
/// </summary>
internal enum ElenFontSource
{
    /// <summary>Zoznam pisiem - INISS ich ponuka pri vkladani informacneho textu.</summary>
    FontList,

    /// <summary>Stlpec katalogovej tabule.</summary>
    Column,

    /// <summary>Text vlaku na tabuli.</summary>
    TrainText,

    /// <summary>Lava strana pravidla TabTab (koncove <c>{n}</c>).</summary>
    TabTab
}

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

/// <summary>
/// Cisla pisiem, ktore INISS posle tabuliam s protokolom ELEN (ELEN, ELENOLD, ELEN10, ELEN16, ELEN16Kam, ELEKON)
/// ako <c>ESC n</c> s dolnym bajtom cisla. Cisla pre ELEN maju vzdy bit 0x40, takze <c>n</c> nikdy nie je
/// riadiaci znak; cislo bez neho (napr. 4) posle <c>1B 04</c>, tabula spravu ukonci predcasne, nesedi sucet
/// a INISS ju opakuje. Kontroluje sa zoznam pisiem, stlpce, texty vlakov a pravidla TabTab, ktore sa
/// tabuliam ELEN mozu poslat.
/// </summary>
internal static class ElenFontChecks
{
    /// <summary>
    /// Problemy grafikonu z <paramref name="scope" />.
    /// </summary>
    public static IEnumerable<IProblem> Problems(AnalysisScope scope) =>
        Find(scope.Document).Select(f => new ElenFontProblem(f, scope));

    /// <summary>
    /// Ci vyrobca pouziva protokol ELEN.
    /// </summary>
    public static bool IsElen(TableManufacturer? manufacturer) => manufacturer is not null && ElenFontCode.AppliesTo(manufacturer);

    /// <summary>
    /// Zle cisla pisiem grafikonu. Ak nema ziadnu fyzicku tabulu ELEN, INISS ich tabuli ELEN nikdy neposle.
    /// </summary>
    public static List<ElenFontFinding> Find(GrafikonDocument doc)
    {
        // tabule ELEN, ktorym INISS nieco posiela - katalogova tabula bez fyzickej je len predloha
        var catalogs = doc.TablePhysicals.Select(p => p.TableCatalog).Where(c => c is not null && IsElen(c.Manufacturer)).ToHashSet();
        if (catalogs.Count == 0)
            return [];

        var findings = new List<ElenFontFinding>();

        foreach (var font in doc.TableFonts)
            if (IsBad(font.FontID))
                findings.Add(new ElenFontFinding(ElenFontSource.FontList, font, font.Name, null, font.FontID, 1));

        foreach (var catalog in doc.TableCatalogs.Where(catalogs.Contains))
        {
            var columns = catalog.Items.Where(item => IsBad(item.FontIdx)).ToList();
            if (columns.Count > 0)
                findings.Add(new ElenFontFinding(ElenFontSource.Column, catalog, catalog.Key, ColumnLabel(columns[0], catalog),
                    columns[0].FontIdx, columns.Count));
        }

        foreach (var text in doc.TableTexts.Where(t => t.Realizations.Any(r => catalogs.Contains(r.Table))))
        {
            // -1 = pismo stlpca, to sa kontroluje pri stlpci
            var trains = text.Trains.Where(train => IsBad(train.FontID)).ToList();
            if (trains.Count > 0)
                findings.Add(new ElenFontFinding(ElenFontSource.TrainText, text, text.Key, TrainLabel(trains[0]), trains[0].FontID,
                    trains.Count));
        }

        var sections = catalogs.SelectMany(c => c.Items).SelectMany(UsedTabTabs).ToHashSet();
        foreach (var tab in doc.TabTabs.Where(sections.Contains))
        {
            var fonts = SentFonts(tab.Text).Where(f => IsBad(f.Font)).ToList();
            if (fonts.Count > 0)
                findings.Add(new ElenFontFinding(ElenFontSource.TabTab, tab, tab.Key,
                    (fonts[0].LineIndex + 1).ToString(CultureInfo.CurrentCulture), fonts[0].Font, fonts.Count));
        }

        return findings;
    }

    /// <summary>
    /// Cisla pisiem, ktore pravidla sekcie TabTab posielaju tabuli: koncove <c>{n}</c> lavej strany jednoduchych
    /// pravidiel a <c>#VYLUKA</c>, <c>#ODKLON</c>, <c>#POZODJ_</c>, pri <c>#SWITCH</c>/<c>#MERGE</c>/<c>#MERGE2</c>
    /// textov poloziek. Prava strana sa len porovnava, <c>{@}</c> (-1) je predvolene pismo.
    /// </summary>
    internal static IEnumerable<(int LineIndex, int Font)> SentFonts(string? text)
    {
        if (string.IsNullOrEmpty(text))
            yield break;

        foreach (var rule in TabTabSection.Parse(text).Rules)
            switch (rule.Event)
            {
                case TabTabEventKind.None or TabTabEventKind.Vyluka or TabTabEventKind.Odklon or TabTabEventKind.PozOdj:
                    if (TabTabText.Decode(rule.Left).Font is { } font)
                        yield return (rule.LineIndex, font);
                    break;

                case TabTabEventKind.Switch or TabTabEventKind.Merge or TabTabEventKind.Merge2:
                    foreach (var item in rule.Items.Where(i => !i.IsCondition && !i.IsSeparator))
                        if ((item.Decoded ?? TabTabText.Decode(item.Text)).Font is { } itemFont)
                            yield return (rule.LineIndex, itemFont);
                    break;
            }
    }

    private static bool IsBad(int fontId) => new ElenFontCode(fontId).SendsControlChar;

    /// <summary>
    /// Sekcie TabTab, ktore stlpec pouzije: TAB1 pri kazdom sposobe plnenia okrem volneho, TAB2 len pri case.
    /// </summary>
    private static IEnumerable<TableTabTab> UsedTabTabs(TableItem item)
    {
        if (item.DivType != TableDivType.Free && item.Tab1 is { } tab1 && tab1 != TableTabTab.Empty)
            yield return tab1;
        if (item.DivType == TableDivType.TableTime && item.Tab2 is { } tab2 && tab2 != TableTabTab.Empty)
            yield return tab2;
    }

    private static string ColumnLabel(TableItem item, TableCatalog catalog) =>
        string.IsNullOrWhiteSpace(item.Name)
            ? (catalog.Items.IndexOf(item) + 1).ToString(CultureInfo.CurrentCulture)
            : $"„{item.Name}“";

    private static string TrainLabel(TableTrain train) => train.Train is null ? "" : TrainRules.Label(train.Train);
}

/// <summary>
/// Cislo pisma, ktore by tabula ELEN dostala ako riadiaci znak - opravi sa na stranke alebo v editore polozky.
/// </summary>
internal sealed class ElenFontProblem(ElenFontFinding finding, AnalysisScope scope) : IProblem
{
    public string Text
    {
        get
        {
            var where = finding.Source switch
            {
                ElenFontSource.FontList => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_ElenFont_Font, finding.Name),
                ElenFontSource.Column => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_ElenFont_Column, finding.Name, finding.Detail),
                ElenFontSource.TrainText => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_ElenFont_Text, finding.Name, finding.Detail),
                _ => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_ElenFont_TabTab, finding.Name, finding.Detail)
            };
            var text = string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_ElenFont, where, finding.FontId,
                (finding.FontId & 0xFF).ToString("X2", CultureInfo.InvariantCulture));
            return finding.Count > 1 ? text + " " + string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_More, finding.Count - 1) : text;
        }
    }

    public string Solution => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_ElenFont_Fix, finding.FontId,
        new ElenFontCode(finding.FontId).WithKeptBit.Id);

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        switch (finding.Item)
        {
            case TableFont font:
                scope.Host.ShowLocalSettings(LocalSettingsPage.Pisma, select: font);
                break;
            case TableCatalog catalog:
                scope.Host.ShowLocalSettings(LocalSettingsPage.KatalogoveTabule, select: catalog);
                break;
            case TableText text:
                scope.Host.ShowLocalSettings(LocalSettingsPage.Texty, select: text);
                break;
            case TableTabTab tab:
                scope.Host.EditTabTab(tab);
                break;
        }

        return ElenFontChecks.Find(scope.Document).Any(f => ReferenceEquals(f.Item, finding.Item)) ? FixResult.NotSolved : FixResult.Done;
    }
}
