using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using GVDEditor.Services;
using GVDEditor.UI.Dialogs;
using GVDEditor.UI.Settings;
using GVDEditor.UI.StateDgm;
using GVDEditor.UI.TabTab;
using Microsoft.VisualBasic.FileIO;
using ToolsCore.Forms;
using ToolsCore.Iniss.Tools;
using ToolsCore.Tools;
using ToolsCore.XML;

namespace GVDEditor.UI.Main;

internal partial class FMain
{
    private void ShowAppSettings(string? page = null)
    {
        var old = _ctx.Config;
        var form = new FAppSettings(_ctx);
        if (page != null) form.PreselectMenuItem(page);
        if (form.ShowDialog() != DialogResult.OK)
            return;

        UpdateMainUI();

        // tieto nastavenia sa inak nacitaju len pri starte programu
        DateLimit.Loc = _ctx.Config.DateLimitLocate == AppLanguage.Czech ? DateLimit.Locale.Cz : DateLimit.Locale.Sk;
        Log.DoAppLogs = _ctx.Config.LoggingInfo;
        Log.DoErrorLogs = _ctx.Config.LoggingError;

        if (old.Language != _ctx.Config.Language || old.ClassicGUI != _ctx.Config.ClassicGUI)
            _dialogs.ShowInfo(Resources.FMain_Nastavenia_po_restarte);
    }

    private void ShowInfoApp()
    {
        var form = new FAboutApp(Resources.AboutAppDescription, Resources.gvd);
        form.ShowDialog(this);
    }

    /// <inheritdoc />
    public void EditTabTab(TableTabTab tabTab)
    {
        using var form = new FTabTab(_ctx, tabTab);
        form.ShowDialog(this);
    }

    /// <returns><see langword="true" />, ak pouzivatel nastavenia ulozil.</returns>
    public bool ShowLocalSettings(LocalSettingsPage page = LocalSettingsPage.Grafikon,
        LocalSettingsAction action = LocalSettingsAction.None, object? select = null)
    {
        var dir = (GVDDirectory)tscbObdobie.ComboBox.SelectedItem!;
        // FLocalSettings meni dir.GVD priamo, povodne hodnoty treba zapamatat vopred
        var oldStation = dir.GVD.ThisStation.Name;
        var oldPeriod = dir.Period;
        var wasSaved = DataSaved;
        // okno sa chvilu zostavuje - kurzor ukaze, ze klik zabral (po zobrazeni okna sa vrati sam)
        Cursor.Current = Cursors.WaitCursor;
        var svform = new FLocalSettings(_ctx, dir, page, action, select);
        var result = svform.ShowDialog();
        if (result != DialogResult.OK)
        {
            // Zrusit/krizik vratil vsetky data - obnova zoznamov nesmie grafikon oznacit ako zmeneny
            DataSaved = wasSaved;
            return false;
        }

        RefreshStationAndPeriod(dir, oldStation, oldPeriod);

        _ctx.Document.TableFontDir = svform.FontDir;
        DataSaved = false;
        _ctx.Document.Trains.ResetBindings();
        return true;
    }

    /// <summary>
    /// Po zmene stanice alebo obdobia platnosti grafikonu v lokalnych nastaveniach aktualizuje comboboxy
    /// Stanica a Obdobie tak, aby grafikon <paramref name="dir" /> ostal vybraty.
    /// </summary>
    private void RefreshStationAndPeriod(GVDDirectory dir, string oldStation, string oldPeriod)
    {
        var newStation = dir.GVD.ThisStation.Name;
        if (newStation == oldStation && dir.Period == oldPeriod) return;

        // zmena zdrojov comboboxov by cez SelectedIndexChanged znovu nacitala grafikon zo suborov
        // a zahodila neulozene zmeny (vratane tych z lokalnych nastaveni)
        WithoutSelectionEvents(() =>
        {
            if (newStation != oldStation)
            {
                GVDSelectionLists.RenameStation(_stations, _gvdDirs, oldStation, newStation);

                _periods.Clear();
                foreach (var gvdDir in GVDSelectionLists.PeriodsOf(_gvdDirs, newStation)) _periods.Add(gvdDir);

                tscbStanica.ComboBox.SelectedItem = newStation;
            }
            else
            {
                _periods.ResetBindings();
            }

            tscbObdobie.ComboBox.SelectedItem = dir;
        });
    }

    private void ShowGlobalSettings(GlobalSettingsPage page = GlobalSettingsPage.Grafikony)
    {
        // okno sa chvilu zostavuje - kurzor ukaze, ze klik zabral (po zobrazeni okna sa vrati sam)
        Cursor.Current = Cursors.WaitCursor;
        var gf = new FGlobalSettings(_ctx, _gvdDirs.ToList(), page, _grafikonLoaded ? _previousSelectedGVD : null);
        if (gf.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            GrafikonService.SaveGlobalSettings(_ctx.Workspace, gf.Grafikony.Select(gvd => gvd.Dir).ToList(), _ctx.Document);
        }
        catch (InvalidOperationException e)
        {
            _dialogs.ShowError(e.Message);
            return;
        }
        finally
        {
            _ctx.Document.Trains.ResetBindings();
        }

        if (gf.RemovedGVDs.Count != 0)
            RemoveGrafikony(gf.RemovedGVDs);

        // grafikon, ktory sa predtym nenacital (napr. pre chybajuci typ vlaku), skusit nacitat znova
        if (!_grafikonLoaded && tscbObdobie.ComboBox.SelectedItem is GVDDirectory dir && _gvdDirs.Contains(dir))
        {
            tscbObdobie.ComboBox.SelectedItem = null;
            tscbObdobie.ComboBox.SelectedItem = dir;
        }

        UpdateCommandStates();
    }

    /// <summary>
    /// Presunie odstranene grafikony do kosa a prisposobi im vyber stanice a obdobia.
    /// DirList.TXT uz je zapisany bez nich.
    /// </summary>
    private void RemoveGrafikony(IReadOnlyCollection<GVDDirectory> removed)
    {
        var currentRemoved = _previousSelectedGVD is not null && removed.Contains(_previousSelectedGVD);

        foreach (var gvd in removed)
        {
            _gvdDirs.Remove(gvd);

            try
            {
                FileSystem.DeleteDirectory(gvd.Dir.FullPath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
            }
            catch (Exception e)
            {
                _dialogs.ShowError(e.Message);
            }
        }

        if (currentRemoved)
        {
            // otvoreny grafikon uz neexistuje - jeho vlaky nesmu ostat v zozname a zmeny v nom sa nemaju ukladat
            DataSaved = true;
            _previousSelectedGVD = null;
            CloseGrafikon();

            if (InitializeDataList())
                InitializeGUI();
            return;
        }

        // otvoreny grafikon ostava - len zo zoznamov zmiznu odstranene obdobia a stanice bez grafikonu
        WithoutSelectionEvents(() =>
        {
            foreach (var gvd in removed)
                _periods.Remove(gvd);

            foreach (var station in _stations.Where(s => _gvdDirs.All(d => d.GVD.ThisStation.Name != s)).ToList())
                _stations.Remove(station);

            tscbStanica.ComboBox.SelectedItem = _previousSelectedGVD?.GVD.ThisStation.Name;
            tscbObdobie.ComboBox.SelectedItem = _previousSelectedGVD;
        });
    }

    /// <summary>
    /// Otvori editor stavoveho diagramu aktualneho grafikonu.
    /// </summary>
    private void ShowStateDgm()
    {
        if (tscbObdobie.ComboBox.SelectedItem is not GVDDirectory dir) return;
        using var f = new FStateDgm(_ctx, dir);
        f.ShowDialog(this);
    }

    private void ShowDatObm()
    {
        var gvd = (tscbObdobie.ComboBox?.SelectedItem as GVDDirectory)?.GVD;
        using var fobm = new FDatObm(gvd?.StartValidTimeTable.ToDateTime(), gvd?.EndValidTimeTable.ToDateTime());
        fobm.ShowDialog(this);
    }
}
