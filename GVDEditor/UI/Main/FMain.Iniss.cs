using System.Globalization;
using GVDEditor.Config;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Integration;
using GVDEditor.Properties;
using GVDEditor.UI.InissSettings;
using GVDEditor.UI.Settings;
using ToolsCore.Tools;

namespace GVDEditor.UI.Main;

internal partial class FMain
{
    // text tlacidla Spustit bez otvorenej instalacie (z navrhu)
    private string? _runButtonText;

    private void Iniss_StateChanged(object? sender, EventArgs e)
    {
        UpdateRunMenus();
        UpdateCommandStates();
    }

    /// <summary>
    /// INISS skoncil - ak hned po starte z dovodu, ktory pouzivatel inak nevidi, oznami ho.
    /// </summary>
    private void Iniss_Exited(object? sender, InissExitedEventArgs e)
    {
        var text = e.ExitCode switch
        {
            -4 => Resources.Run_Exit_AlreadyRunning,
            -3 => Resources.Run_Exit_Mutex,
            66 => Resources.Run_Exit_Unregistered,
            _ => null
        };
        if (text is not null && !IsDisposed)
            _dialogs.ShowWarning(string.Format(CultureInfo.CurrentCulture, text, e.Instance.Configuration.Name));
    }

    /// <summary>
    /// Okno Nastavenia INISSu pre vybranu konfiguraciu spustania, pripadne na sekcii <paramref name="section" />.
    /// </summary>
    public void ShowInissSettings(string? section)
    {
        using var f = new FInissSettings(_ctx, _iniss, _dialogs, _ctx.RunConfigurations.Selected);
        if (section is not null) f.SelectSection(section);
        f.ShowDialog(this);
        // klon vetvy mohol pridat konfiguraciu spustania
        FillRunMenus();
    }

    /// <summary>
    /// Okno Konfiguracie spustania; po ulozeni obnovi ponuky.
    /// </summary>
    private void ShowRunConfigurations()
    {
        if (!HasInstallation)
            return;

        using var f = new FRunConfigurations(_ctx, _iniss, _dialogs);
        if (f.ShowDialog(this) != DialogResult.OK)
            return;

        _ctx.RunConfigurations.Replace(f.Result);
        _ctx.RunConfigurations.SelectedId = f.SelectedId;
        SaveRunConfigurations(true);
        FillRunMenus();
        if (f.RunAfterSave)
            RunSelected();
    }

    /// <summary>
    /// Nacita konfiguracie spustania otvorenej instalacie.
    /// </summary>
    private void LoadRunConfigurations()
    {
        try
        {
            _ctx.RunConfigurations = RunConfigurationStore.Load(_ctx.Config, _ctx.Workspace.INISSDir, _ctx.Workspace.INISSExeFiles);
            // predvolene konfiguracie: vybrat prvu, ktorej INISS uz ma nastavenia v registri
            if (_ctx.RunConfigurations.IsDefault)
            {
                var branches = InissRegistry.AppNames();
                _ctx.RunConfigurations.SelectedId = _ctx.RunConfigurations.Items
                    .FirstOrDefault(c => branches.Contains(RunConfigurations.AppName(c), StringComparer.OrdinalIgnoreCase))?.Id;
            }
        }
        catch (RunConfigurationLoadException e)
        {
            _ctx.RunConfigurations = e.Partial;
            _dialogs.ShowWarning(string.Format(CultureInfo.CurrentCulture, Resources.Run_LoadSharedFailed, e.Message));
        }
    }

    /// <summary>
    /// Zapise konfiguracie spustania (<paramref name="shared" /> = aj zdielane v datach instalacie) a config.xml.
    /// </summary>
    private void SaveRunConfigurations(bool shared)
    {
        try
        {
            if (shared)
                RunConfigurationStore.Save(_ctx.Config, _ctx.RunConfigurations);
            else
                RunConfigurationStore.SaveSelection(_ctx.Config, _ctx.RunConfigurations);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            _dialogs.ShowError(string.Format(CultureInfo.CurrentCulture, Resources.Run_SaveSharedFailed, e.Message));
        }

        SettingsWindow.SaveConfig(_ctx.Config);
    }

    /// <summary>
    /// Doplni do ponuk Spustit konfiguracie spustania otvorenej instalacie.
    /// </summary>
    private void FillRunMenus()
    {
        // konfiguracie z predtym otvorenej instalacie - nechat len polozky pred oddelovacom
        RemoveItemsAfter(tssbStartINISS.DropDownItems, toolStripSeparator8);
        RemoveItemsAfter(tsmiRun.DropDownItems, toolStripSeparator14);

        foreach (var config in _ctx.RunConfigurations.Items)
        {
            tssbStartINISS.DropDownItems.Add(RunItem(config));
            tsmiRun.DropDownItems.Add(RunItem(config));
        }

        foreach (ToolStripItem item in tssbStartINISS.DropDownItems) item.ForeColor = _ctx.UsingStyle.ControlsColorScheme.Button.ForeColor;
        foreach (ToolStripItem item in tsmiRun.DropDownItems) item.ForeColor = _ctx.UsingStyle.ControlsColorScheme.Button.ForeColor;
        UpdateRunMenus();
    }

    private ToolStripMenuItem RunItem(RunConfiguration config)
    {
        var id = config.Id;
        var item = new ToolStripMenuItem(config.Name) { Tag = id, ImageScaling = ToolStripItemImageScaling.SizeToFit };
        item.Click += (_, _) =>
        {
            // vyber podla identifikatora - konfiguracia sa mohla medzitym nahradit
            if (_ctx.RunConfigurations.Find(id) is not { } current)
                return;
            SelectRunConfiguration(id);
            RunConfiguration(current);
        };
        return item;
    }

    /// <summary>
    /// Vyznaci vybranu konfiguraciu a beziace INISSy v ponukach, nazov vybranej na tlacidle Spustit.
    /// </summary>
    private void UpdateRunMenus()
    {
        _runButtonText ??= tssbStartINISS.Text;
        var selected = HasInstallation ? _ctx.RunConfigurations.Selected : null;
        tssbStartINISS.Text = selected?.Name ?? _runButtonText;
        tssbStartINISS.ToolTipText = selected is null ? _runButtonText : string.Format(CultureInfo.CurrentCulture, Resources.Run_ButtonToolTip, selected.Name);

        foreach (var items in new[] { tssbStartINISS.DropDownItems, tsmiRun.DropDownItems })
        foreach (var item in items.OfType<ToolStripMenuItem>())
        {
            if (item.Tag is not string id || _ctx.RunConfigurations.Find(id) is not { } config)
                continue;

            var count = _iniss.Instances.Count(i => i.Configuration.Id == id);
            item.Checked = id == selected?.Id;
            item.Text = count > 1 ? string.Format(CultureInfo.CurrentCulture, Resources.Run_ItemInstances, config.Name, count) : config.Name;
            // beziaca konfiguracia ma ikonu spustenia (zaskrtnutie vybranej sa zobrazi vedla nej)
            item.Image = count > 0 ? tssbStartINISS.Image : null;
            item.ToolTipText = RunConfigurations.CommandLine(config);
        }
    }

    private static void RemoveItemsAfter(ToolStripItemCollection items, ToolStripItem separator)
    {
        var index = items.IndexOf(separator);
        while (items.Count > index + 1)
        {
            var item = items[items.Count - 1];
            items.Remove(item);
            item.Dispose();
        }
    }

    /// <summary>
    /// Vyberie konfiguraciu (zapamata sa pre instalaciu na tomto pocitaci).
    /// </summary>
    private void SelectRunConfiguration(string id)
    {
        if (_ctx.RunConfigurations.SelectedId == id && !_ctx.RunConfigurations.IsDefault)
            return;

        _ctx.RunConfigurations.SelectedId = id;
        SaveRunConfigurations(false);
        UpdateRunMenus();
    }

    /// <summary>
    /// Spusti vybranu konfiguraciu; bez konfiguracie otvori okno Konfiguracie spustania.
    /// </summary>
    private void RunSelected()
    {
        if (_ctx.RunConfigurations.Selected is { } config)
            RunConfiguration(config);
        else
            ShowRunConfigurations();
    }

    /// <summary>
    /// Spusti konfiguraciu <paramref name="config" />. Ak uz bezi, podla konfiguracie sa opyta, restartuje ju alebo
    /// spusti dalsiu instanciu.
    /// </summary>
    private async void RunConfiguration(RunConfiguration config)
    {
        if (!HasInstallation)
            return;

        var running = _iniss.Instances.Where(i => i.Configuration.Id == config.Id && !i.IsRestarting).ToList();
        if (running.Count > 0)
        {
            var mode = config.WhenRunning;
            if (mode == WhenAlreadyRunning.Ask)
            {
                var answer = _dialogs.ShowQuestion(string.Format(CultureInfo.CurrentCulture, Resources.Run_AlreadyRunning, config.Name), MessageBoxButtons.YesNoCancel);
                if (answer == DialogResult.Cancel)
                    return;
                mode = answer == DialogResult.Yes ? WhenAlreadyRunning.Restart : WhenAlreadyRunning.NewInstance;
            }

            if (mode == WhenAlreadyRunning.Restart)
            {
                await RestartAsync(running[^1], config);
                return;
            }
        }

        if (!await PrepareLaunchAsync(config, false))
            return;

        try
        {
            _iniss.Start(InissLaunch.Create(config, _ctx.Workspace.INISSDir));
        }
        catch (InvalidOperationException e)
        {
            _dialogs.ShowError(e.Message);
        }
    }

    /// <summary>
    /// Pred spustenim: kontrola konfiguracie (chyby zastavia, varovania sa opytaju), ulozenie zmien a analyza
    /// grafikonu podla konfiguracie.
    /// </summary>
    /// <param name="config">spustana konfiguracia</param>
    /// <param name="restart">restart - beziaci INISS sa pred spustenim ukonci, jeho zamok jedinej instancie nevadi</param>
    /// <returns><see langword="false" />, ak sa spustenie zrusilo.</returns>
    private async Task<bool> PrepareLaunchAsync(RunConfiguration config, bool restart)
    {
        var checks = RunConfigurationChecks.Check(config, InissEnvironment.Create(_ctx.Workspace, _ctx.RunConfigurations.Items, !restart));
        var errors = checks.Where(c => c.Severity == RunCheckSeverity.Error).Select(c => "• " + c.Text).ToList();
        if (errors.Count > 0)
        {
            _dialogs.ShowError(string.Format(CultureInfo.CurrentCulture, Resources.Run_CannotStart, config.Name, string.Join(Environment.NewLine, errors)));
            return false;
        }

        var warnings = checks.Where(c => c.Severity == RunCheckSeverity.Warning).Select(c => "• " + c.Text).ToList();
        if (warnings.Count > 0 &&
            _dialogs.ShowQuestion(string.Format(CultureInfo.CurrentCulture, Resources.Run_WarningsQuestion, config.Name, string.Join(Environment.NewLine, warnings))) != DialogResult.Yes)
            return false;

        if (!SaveBeforeLaunch(config))
            return false;

        return !config.AnalyzeBefore || await AnalyzeBeforeLaunchAsync(config);
    }

    /// <summary>
    /// INISS cita data grafikonu pri starte - neulozene zmeny by v nom chybali.
    /// </summary>
    /// <returns><see langword="false" />, ak pouzivatel spustenie zrusil alebo sa grafikon nepodarilo ulozit.</returns>
    private bool SaveBeforeLaunch(RunConfiguration config)
    {
        if (DataSaved || !HasInstallation)
            return true;

        return config.SaveBefore switch
        {
            SaveBeforeRun.Always => DoSave(),
            SaveBeforeRun.Never => true,
            _ => _dialogs.ShowQuestion(Resources.FMain_Ulozit_pred_spustenim_INISS, MessageBoxButtons.YesNoCancel) switch
            {
                DialogResult.Yes => DoSave(),
                DialogResult.No => true,
                _ => false
            }
        };
    }

    /// <summary>
    /// Analyza otvoreneho grafikonu pred spustenim; pri chybach sa opyta, ci aj tak spustit (inak otvori analyzu).
    /// </summary>
    private async Task<bool> AnalyzeBeforeLaunchAsync(RunConfiguration config)
    {
        if (!HasGrafikon || tscbObdobie.SelectedItem is not GVDDirectory gvd)
            return true;

        List<IProblem> problems;
        UseWaitCursor = true;
        try
        {
            var scope = new AnalysisScope(_ctx.Document, _ctx.Workspace, this)
            {
                InissLoader = () => InissEnvironment.AnalysisView(config, _ctx.Workspace)
            };
            problems = await Task.Run(() => Analyzer.FindProblems(gvd, scope));
        }
        finally
        {
            UseWaitCursor = false;
        }

        var errors = problems.Count(p => p.ProblemType == ProblemType.Error);
        if (errors == 0)
            return true;

        var answer = _dialogs.ShowQuestion(string.Format(CultureInfo.CurrentCulture, Resources.Run_AnalysisErrors, errors), MessageBoxButtons.YesNoCancel);
        if (answer == DialogResult.No)
            ShowAnalyzeGVD();
        return answer == DialogResult.Yes;
    }

    /// <summary>
    /// Vykona <paramref name="action" /> nad beziacim INISSom: vybranej konfiguracie, jedinym beziacim, inak nad tym,
    /// ktory si pouzivatel vyberie z ponuky.
    /// </summary>
    /// <param name="source">prvok, ktorym bol prikaz vyvolany (ponuka sa ukaze pod nim)</param>
    /// <param name="action">co spravit</param>
    /// <param name="all">polozka Vsetky (ukoncenie)</param>
    private void WithInstance(ToolStripItem? source, Action<InissInstance> action, bool all)
    {
        var running = _iniss.Instances.Where(i => !i.IsRestarting).ToList();
        if (running.Count == 0)
            return;

        var own = running.Where(i => i.Configuration.Id == _ctx.RunConfigurations.SelectedId).ToList();
        if (own.Count == 1 || running.Count == 1)
        {
            action(own.Count == 1 ? own[0] : running[0]);
            return;
        }

        var menu = new ContextMenuStrip();
        foreach (var instance in running)
        {
            var text = string.Format(CultureInfo.CurrentCulture, Resources.Run_InstanceItem, instance.Configuration.Name, instance.Started);
            menu.Items.Add(text, null, (_, _) => action(instance));
        }

        if (all)
        {
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(Resources.Run_AllInstances, null, (_, _) => running.ForEach(action));
        }

        FormUtils.ChangeColorContextMenu(_ctx.UsingStyle, menu);
        menu.Closed += (_, _) => BeginInvoke(menu.Dispose);
        if (source is { Owner: ToolStrip owner } && owner == toolMenu)
            menu.Show(owner, new Point(source.Bounds.Left, source.Bounds.Bottom));
        else
            menu.Show(Cursor.Position);
    }

    private void ShutDownINISS(ToolStripItem? source) => WithInstance(source, _iniss.ShutDown, true);

    private void KillINISS(InissInstance instance)
    {
        try
        {
            _iniss.Kill(instance);
        }
        catch (InvalidOperationException e)
        {
            _dialogs.ShowError(e.Message);
        }
    }

    /// <summary>
    /// Nutene ukoncenie na priamy prikaz - s potvrdenim, predvolena skratka F10 sa lahko stlaci omylom.
    /// </summary>
    private void AskKillINISS(ToolStripItem? source)
    {
        if (!_iniss.IsRunning)
            return;

        WithInstance(source, instance =>
        {
            if (_dialogs.ShowQuestion(string.Format(CultureInfo.CurrentCulture, Resources.Run_KillQuestion, instance.Configuration.Name)) == DialogResult.Yes)
                KillINISS(instance);
        }, false);
    }

    private void RestartINISS(ToolStripItem? source) => WithInstance(source, instance => _ = RestartAsync(instance, null), false);

    /// <summary>
    /// Riadne ukonci INISS a spusti ho znova podla aktualnej konfiguracie. Ak sa INISS do casoveho limitu neukonci
    /// (napr. caka na potvrdenie), ponukne nutene ukoncenie.
    /// </summary>
    private async Task RestartAsync(InissInstance instance, RunConfiguration? config)
    {
        config ??= _ctx.RunConfigurations.Find(instance.Configuration.Id) ?? instance.Configuration;
        if (!await PrepareLaunchAsync(config, true))
            return;

        try
        {
            await _iniss.RestartAsync(instance, InissLaunch.Create(config, _ctx.Workspace.INISSDir),
                () => _dialogs.ShowQuestion(Resources.FMain_INISS_sa_neukoncil) == DialogResult.Yes);
        }
        catch (InvalidOperationException e)
        {
            _dialogs.ShowError(e.Message);
        }
    }
}
