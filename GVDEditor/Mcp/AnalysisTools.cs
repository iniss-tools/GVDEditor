using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Integration;
using GVDEditor.UI.Settings;
using ModelContextProtocol.Server;

namespace GVDEditor.Mcp;

/// <summary>Analyza otvoreneho grafikonu ako v okne Analyza.</summary>
[McpServerToolType]
internal static class AnalysisTools
{
    [McpServerTool(Name = "analyze_grafikon", Title = "Analyze timetable", ReadOnly = true, Idempotent = true, OpenWorld = false, UseStructuredContent = true)]
    [Description("Checks the open timetable like the Analysis window of GVDEditor: validity, trains, boards, TabTab, state diagram, recordings and "
                 + "INISS settings of the selected run configuration. Returns the problems with severity and a suggested fix; fixing is done in GVDEditor. "
                 + "A TabTab problem shows only its first finding - get_tabtab lists all of them with line numbers.")]
    public static Task<AnalysisResult> AnalyzeGrafikon(EditorAccess editor) =>
        // na vlakne okna - pouzivatel medzitym grafikon nemeni (okno Analyza je modalne)
        editor.ReadAsync(context =>
        {
            McpGuards.Open(context);
            var config = context.RunConfigurations.Selected;
            var scope = new AnalysisScope(context.Document, context.Workspace, NoHost.Instance)
            {
                InissLoader = () => InissEnvironment.AnalysisView(config, context.Workspace)
            };
            var problems = Analyzer.FindProblems(context.Directory!, scope)
                .Select(p => new ProblemInfo(Severity(p.ProblemType), p.Text, p.Solution, Fix(p.FixType)))
                .OrderBy(p => p.Severity switch { "error" => 0, "warning" => 1, _ => 2 })
                .ToList();
            return new AnalysisResult(problems.Count(p => p.Severity == "error"), problems.Count(p => p.Severity == "warning"), problems);
        });

    private static string Severity(ProblemType type) => type switch
    {
        ProblemType.Error => "error",
        ProblemType.Warning => "warning",
        _ => "hint"
    };

    private static string Fix(FixType type) => type switch
    {
        FixType.Auto => "auto",
        FixType.SemiAuto => "semiAuto",
        _ => "manual"
    };

    /// <summary>Analyza bez okien - opravy cez MCP nie su, takze sa nikdy nevola.</summary>
    private sealed class NoHost : IAnalyzerHost
    {
        public static readonly NoHost Instance = new();

        public bool ShowLocalSettings(LocalSettingsPage page = LocalSettingsPage.Grafikon, LocalSettingsAction action = LocalSettingsAction.None,
            object? select = null) => false;

        public void EditTabTab(TableTabTab tabTab)
        {
        }

        public bool EditTrain(Train train) => false;

        public void ShowInissSettings(string? section)
        {
        }
    }
}
