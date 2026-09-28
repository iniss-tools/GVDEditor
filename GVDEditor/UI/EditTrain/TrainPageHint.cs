using System.Globalization;
using GVDEditor.Domain.Rules;
using GVDEditor.UI.Settings;
using GVDEditor.Properties;

namespace GVDEditor.UI.EditTrain;

/// <summary>
///     Text chyb a upozorneni pod poliami stranky okna vlaku.
/// </summary>
internal static class TrainPageHint
{
    /// <summary>
    ///     Vypise chyby (pred upozorneniami) do <paramref name="hint" />; chyby cervenou, samotne upozornenia oranzovou.
    /// </summary>
    /// <param name="hint">popis pod poliami stranky</param>
    /// <param name="problems">chyby a upozornenia stranky</param>
    /// <param name="normal">povodna farba popisu</param>
    public static void Show(Label hint, IReadOnlyCollection<TrainRules.Problem> problems, Color normal)
    {
        hint.Text = string.Join(Environment.NewLine, problems.OrderBy(problem => problem.IsWarning).Select(problem =>
            problem.IsWarning ? string.Format(CultureInfo.CurrentCulture, Resources.TrainPageHint_Upozornenie, problem.Message) : problem.Message));
        hint.ForeColor = problems.Count == 0 ? normal
            : problems.Any(problem => !problem.IsWarning) ? SettingsWindow.ProblemColor(hint)
            : WarningColor(hint);
    }

    /// <summary>
    ///     Farba upozornenia citatelna na pozadi prvku (svetla aj tmava tema).
    /// </summary>
    public static Color WarningColor(Control control) =>
        control.BackColor.GetBrightness() < 0.5f ? Color.FromArgb(255, 190, 90) : Color.FromArgb(166, 86, 0);
}
