using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using GVDEditor.TabTabEditor;
using GVDEditor.UI.StateDgm;
using GVDEditor.Properties;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranka Stavovy diagram v okne Lokalne nastavenia - stav suboru stavoveho diagramu a otvorenie jeho editora.
/// </summary>
public partial class StateDgmPage : UserControl
{
    /// <summary>
    /// Kontext editora - nastavi ho <c>LoadData</c>.
    /// </summary>
    private EditorContext _ctx = null!;

    private GVDDirectory _dir = null!;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public StateDgmPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Zobrazi stav stavoveho diagramu grafikonu.
    /// </summary>
    /// <param name=\"context\">kontext editora</param>
    internal void LoadData(EditorContext context, GVDDirectory dir)
    {
        _ctx = context;
        _dir = dir;
        RefreshStatus();
    }

    /// <summary>
    /// Otvori editor stavoveho diagramu a po jeho zatvoreni obnovi stav.
    /// </summary>
    public void OpenEditor()
    {
        using var form = new FStateDgm(_ctx, _dir);
        form.ShowDialog(FindForm());
        RefreshStatus();
    }

    private void bOpen_Click(object? sender, EventArgs e) => OpenEditor();

    private void RefreshStatus()
    {
        try
        {
            var d = StateDgmFile.Read(_dir.Dir.FullPath);
            if (d == null)
            {
                lStatus.Text = Resources.FLocalSettings_SD_Chyba_Nie;
                return;
            }

            var diags = StateDgmValidator.Validate(d, new StateDgmValidationOptions
            {
                ReportKeys = _ctx.Document.ReportTypes?.Count > 0 ? _ctx.Document.ReportTypes.Select(r => r.Key).ToList() : null,
                Symbols = new GvdExprSymbols(_ctx.Workspace, _ctx.Document)
            });
            var errors = diags.Count(x => x.IsError);
            var warnings = diags.Count(x => x.Severity == ExprSeverity.Warning);
            var check = diags.Count == 0
                ? Resources.FStateDgm_BezProblemov
                : string.Format(CultureInfo.CurrentCulture, Resources.FStateDgm_PocetProblemov, errors, warnings, diags.Count - errors - warnings);
            lStatus.Text = string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_SD_Stav,
                               Path.GetFileName(StateDgmFile.PathOf(_dir.Dir.FullPath)), d.Categories.Count, d.Categories.Sum(c => c.States.Count))
                           + Environment.NewLine + string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_SD_Problemy, check);
        }
        catch (StateDgmParseException e)
        {
            lStatus.Text = string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_SD_Chyba, $"({e.Line + 1}) {e.Message}");
        }
    }
}
