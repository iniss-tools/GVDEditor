using ExControls;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Integration;
using GVDEditor.Properties;
using GVDEditor.UI.Analysis;
using ToolsCore.Tools;

namespace GVDEditor.UI.Main;

// ukotvitelne panely (ExDockManager): tabulka vlakov v dokumente, analyza grafikonu dole
internal partial class FMain
{
    // kluc panela v ulozenom rozlozeni (config.xml) - nemenit
    private const string AnalysisPanelKey = "Analysis";

    private readonly ExDockManager _dock = AppDock.Create();
    private readonly ExDockPanel _analysisPanel = new(AnalysisPanelKey, Resources.Panel_Analysis);
    private readonly AnalysisView _analysisView;
    private string _defaultLayout = "";

    /// <summary>
    /// Presunie tabulku vlakov z navrhu okna do dokumentu a prida panel analyzy; rozlozenie z nastaveni sa obnovi az po
    /// teme a pismach.
    /// </summary>
    private void InitDocking()
    {
        SuspendLayout();
        var index = Controls.GetChildIndex(panelMain);
        Controls.Remove(panelMain);
        panelMain.Dock = DockStyle.Fill;
        _dock.Document.Controls.Add(panelMain);
        _analysisView.Dock = DockStyle.Fill;
        _analysisPanel.Controls.Add(_analysisView);

        _dock.AddPanel(_analysisPanel, ExDockSide.Bottom);
        _dock.SetZoneSize(ExDockSide.Bottom, 180);
        Controls.Add(_dock);
        // na miesto tabulky - pred panelmi ukotvenymi k okraju, aby vyplnil zvysok okna
        Controls.SetChildIndex(_dock, index);
        ResumeLayout(true);

        _defaultLayout = _dock.SaveLayout();
        GridColumns.Attach(_analysisView.Grid, "Problems", () => _ctx.Config, () => UI.Settings.SettingsWindow.SaveConfig(_ctx.Config));
    }

    private AnalysisView CreateAnalysisView() => new(progress =>
    {
        if (!_grafikonLoaded || tscbObdobie.SelectedItem is not GVDDirectory gvd)
            return null;

        var config = _ctx.RunConfigurations.Selected;
        var scope = new AnalysisScope(_ctx.Document, _ctx.Workspace, this)
        {
            InissLoader = () => InissEnvironment.AnalysisView(config, _ctx.Workspace)
        };
        return Task.Run(() => Analyzer.FindProblems(gvd, scope, progress));
    }, AnalysisFixed);

    // prikaz Analyzovat grafikon: zobrazi panel a spusti analyzu
    private void ShowAnalyzeGVD()
    {
        _dock.ShowPanel(_analysisPanel);
        _ = _analysisView.AnalyzeAsync();
    }

    // opravy menia grafikon v pamati - bez oznacenia by sa pri zatvoreni bez otazky stratili
    private void AnalysisFixed(IProblem problem)
    {
        if (!problem.ChangesGrafikon)
            return;
        MarkChanged(Resources.History_Analysis);
        _ctx.Document.Trains.ResetBindings();
    }

    private void ResetLayout() => _dock.LoadLayout(_defaultLayout);

    // rozlozenie panelov sa uklada pri zatvoreni okna spolu s ostatnymi nastaveniami
    private void SaveDockLayout()
    {
        _ctx.Config.DockLayout = _dock.SaveLayout();
        UI.Settings.SettingsWindow.SaveConfig(_ctx.Config);
    }
}
