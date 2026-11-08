using System.Globalization;
using GVDEditor.Properties;
using GVDEditor.UI.Settings;

namespace GVDEditor.Domain.Analysis.Problems;

/// <summary>
/// Polozka lokalnych nastaveni (tabula, text, kolaj…), ktoru by stranka nastaveni neulozila, alebo s nedostatkami,
/// ktore INISS znesie.
/// </summary>
internal sealed class SettingsItemProblems : IProblem
{
    private readonly string _kind;
    private readonly string? _key;
    private readonly LocalSettingsPage _page;
    private readonly object _item;
    private readonly AnalysisScope _scope;
    private readonly Func<(List<string> Errors, List<string> Warnings)> _check;
    private readonly (List<string> Errors, List<string> Warnings) _found;
    private readonly ProblemType _errorType;

    private SettingsItemProblems(string kind, string? key, LocalSettingsPage page, object item, AnalysisScope scope,
        Func<(List<string>, List<string>)> check, (List<string>, List<string>) found, ProblemType errorType)
    {
        _errorType = errorType;
        _kind = kind;
        _key = key;
        _page = page;
        _item = item;
        _scope = scope;
        _check = check;
        _found = found;
    }

    /// <summary>
    /// Problem polozky, alebo <see langword="null" />, ak je v poriadku.
    /// </summary>
    /// <param name="errorType">zavaznost chyb - <see cref="ProblemType.Error" />, ak ich INISS neprijme, inak
    /// <see cref="ProblemType.Warning" /> (stranka ich neulozi, no INISS bezi)</param>
    public static SettingsItemProblems? Create(string kind, string? key, LocalSettingsPage page, object item, AnalysisScope scope,
        Func<(List<string> Errors, List<string> Warnings)> check, ProblemType errorType = ProblemType.Error)
    {
        var found = check();
        return found.Errors.Count == 0 && found.Warnings.Count == 0
            ? null
            : new SettingsItemProblems(kind, key, page, item, scope, check, found, errorType);
    }

    public string Text
    {
        get
        {
            // texty stranky su odrazky zoznamu - v jednom riadku analyzy bez nich
            var all = _found.Errors.Concat(_found.Warnings).Select(m => m.TrimStart('•', ' ')).ToList();
            var key = string.IsNullOrWhiteSpace(_key) ? Resources.Analyzer_NoKey : _key;
            var text = string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_SettingsItem, _kind, key, all[0]);
            return all.Count > 1 ? text + " " + string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_More, all.Count - 1) : text;
        }
    }

    public string Solution => _found.Errors.Count > 0 ? Resources.Analyzer_SettingsItem_Fix : Resources.Analyzer_SettingsItemWarning_Fix;

    public ProblemType ProblemType => _found.Errors.Count > 0 ? _errorType : ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        _scope.Host.ShowLocalSettings(_page, select: _item);
        var (errors, warnings) = _check();
        return errors.Count == 0 && warnings.Count == 0 ? FixResult.Done : FixResult.NotSolved;
    }
}
