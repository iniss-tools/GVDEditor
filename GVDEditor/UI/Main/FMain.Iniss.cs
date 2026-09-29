using GVDEditor.Properties;
using ToolsCore.Iniss.Tools;
using ToolsCore.Tools;

namespace GVDEditor.UI.Main;

public partial class FMain
{
    /// <summary>
    /// Doplni do ponuk Spustit programy (*.exe) otvorenej instalacie.
    /// </summary>
    private void FillInissPrograms()
    {
        // programy z predtym otvorenej instalacie - nechat len polozky pred oddelovacom
        RemoveItemsAfter(tssbStartINISS.DropDownItems, toolStripSeparator8);
        RemoveItemsAfter(tsmiRun.DropDownItems, toolStripSeparator14);

        foreach (var file in GlobData.INISSExeFiles)
        {
            ToolStripItem item1 = new ToolStripMenuItem(file);
            item1.Click += InissStartItemOnClick;
            tssbStartINISS.DropDownItems.Add(item1);

            ToolStripItem item2 = new ToolStripMenuItem(file);
            item2.Click += InissStartItemOnClick;
            tsmiRun.DropDownItems.Add(item2);
        }

        foreach (ToolStripItem item in tssbStartINISS.DropDownItems) item.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Button.ForeColor;

        foreach (ToolStripItem item in tsmiRun.DropDownItems) item.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Button.ForeColor;
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

    private void InissStartItemOnClick(object? sender, EventArgs e)
    {
        if (sender is ToolStripItem tsmi && !_iniss.IsRestarting)
            StartINISS(PathUtils.CombinePath(GlobData.INISSDir, tsmi.Text!)!, tsmiRun);
    }

    /// <summary>
    /// Spusti program <paramref name="path" /> (null = naposledy spusteny, inak rozbali ponuku
    /// <paramref name="dropDown" />). Ak INISS uz bezi, ponukne jeho nutene ukoncenie.
    /// </summary>
    private void StartINISS(string? path, ToolStripDropDownItem dropDown)
    {
        if (_iniss.IsRunning)
        {
            if (Utils.ShowQuestion(Resources.FMain_InissStartItemOnClick) == DialogResult.Yes)
                KillINISS();
            return;
        }

        path ??= _iniss.LastStartPath;
        if (path == null)
        {
            dropDown.ShowDropDown();
            return;
        }

        if (!ConfirmSaveBeforeINISS())
            return;

        try
        {
            _iniss.Start(path, GlobData.Config.StartupINISSConfig);
        }
        catch (InvalidOperationException e)
        {
            Utils.ShowError(e.Message);
        }
    }

    /// <summary>
    /// INISS cita data grafikonu pri starte - neulozene zmeny by v nom chybali.
    /// </summary>
    /// <returns><see langword="false" />, ak pouzivatel spustenie zrusil alebo sa grafikon nepodarilo ulozit.</returns>
    private bool ConfirmSaveBeforeINISS()
    {
        if (DataSaved || !HasInstallation)
            return true;

        return Utils.ShowQuestion(Resources.FMain_Ulozit_pred_spustenim_INISS, MessageBoxButtons.YesNoCancel) switch
        {
            DialogResult.Yes => DoSave(),
            DialogResult.No => true,
            _ => false
        };
    }

    private void KillINISS()
    {
        try
        {
            _iniss.Kill();
        }
        catch (InvalidOperationException e)
        {
            Utils.ShowError(e.Message);
        }
    }

    /// <summary>
    /// Nutene ukoncenie na priamy prikaz - s potvrdenim, predvolena skratka F10 sa lahko stlaci omylom.
    /// </summary>
    private void AskKillINISS()
    {
        if (_iniss.IsRunning && Utils.ShowQuestion(Resources.FMain_Vynutit_ukoncenie_INISS) == DialogResult.Yes)
            KillINISS();
    }

    /// <summary>
    /// Riadne ukonci INISS a spusti ho znova. Ak sa INISS do casoveho limitu neukonci (napr. caka na potvrdenie),
    /// ponukne nutene ukoncenie.
    /// </summary>
    private async void RestartINISS()
    {
        if (!ConfirmSaveBeforeINISS())
            return;

        try
        {
            await _iniss.RestartAsync(GlobData.Config.StartupINISSConfig,
                () => Utils.ShowQuestion(Resources.FMain_INISS_sa_neukoncil) == DialogResult.Yes);
        }
        catch (InvalidOperationException e)
        {
            Utils.ShowError(e.Message);
        }
    }
}
