using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using GVDEditor.UI.Settings;
using ToolsCore.Iniss.Elen;

namespace GVDEditor.Domain.Analysis.Problems;

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
