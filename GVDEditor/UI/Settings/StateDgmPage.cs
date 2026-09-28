using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using GVDEditor.TabTabEditor;
using GVDEditor.UI.StateDgm;
using GVDEditor.Properties;
using ToolsCore.StateDgm;

namespace GVDEditor.UI.Settings;

/// <summary>
///     Stranka Stavovy diagram v okne Lokalne nastavenia - stav suboru stavoveho diagramu a otvorenie jeho editora.
/// </summary>
public partial class StateDgmPage : UserControl
{
    private GVDDirectory _dir = null!;

    /// <summary>
    ///     Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public StateDgmPage()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Zobrazi stav stavoveho diagramu grafikonu.
    /// </summary>
    public void LoadData(GVDDirectory dir)
    {
        _dir = dir;
        RefreshStatus();
    }

    /// <summary>
    ///     Otvori editor stavoveho diagramu a po jeho zatvoreni obnovi stav.
    /// </summary>
    public void OpenEditor()
    {
        using var form = new FStateDgm(_dir);
        form.ShowDialog(FindForm());
        RefreshStatus();
    }

    private void bOpen_Click(object? sender, EventArgs e) => OpenEditor();

    private void RefreshStatus()
    {
        try
        {
            var d = TxtParser.ReadStateDgm(_dir.Dir.FullPath);
            if (d == null)
            {
                lStatus.Text = Resources.FLocalSettings_SD_Chyba_Nie;
                return;
            }

            var diags = StateDgmValidator.Validate(d, new StateDgmValidationOptions
            {
                ReportKeys = GlobData.ReportTypes?.Count > 0 ? GlobData.ReportTypes.Select(r => r.Key).ToList() : null,
                Symbols = new GvdExprSymbols()
            });
            var errors = diags.Count(x => x.IsError);
            var warnings = diags.Count(x => x.Severity == ToolsCore.Expressions.ExprSeverity.Warning);
            var check = diags.Count == 0
                ? Resources.FStateDgm_BezProblemov
                : string.Format(CultureInfo.CurrentCulture, Resources.FStateDgm_PocetProblemov, errors, warnings, diags.Count - errors - warnings);
            lStatus.Text = string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_SD_Stav,
                               Path.GetFileName(TxtParser.StateDgmPath(_dir.Dir.FullPath)), d.Categories.Count, d.Categories.Sum(c => c.States.Count))
                           + Environment.NewLine + string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_SD_Problemy, check);
        }
        catch (StateDgmParseException e)
        {
            lStatus.Text = string.Format(CultureInfo.CurrentCulture, Resources.FLocalSettings_SD_Chyba, $"({e.Line + 1}) {e.Message}");
        }
    }
}
