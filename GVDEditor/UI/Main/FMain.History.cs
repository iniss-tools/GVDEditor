using System.Globalization;
using GVDEditor.Domain.Snapshots;
using GVDEditor.Properties;

namespace GVDEditor.UI.Main;

/// <summary>
/// Kroky spat a znova: po kazdej dokoncenej uprave grafikonu (vsade, kde sa grafikon oznaci ako zmeneny) sa zapamata jeho
/// stav. Upravy jednej operacie (napr. viac udalosti tabulky) sa zlucia do jedneho kroku - stav sa zapamata az po
/// spracovani spravy, ktora operaciu vyvolala.
/// </summary>
internal partial class FMain
{
    private GrafikonHistory? _history;

    /// <summary>Popis dokoncovanej upravy; null = vseobecna uprava grafikonu.</summary>
    private string? _changeLabel;

    private bool _commitPending;

    /// <summary>Prave sa obnovuje stav z historie - zmeny tabulky nie su nova uprava.</summary>
    private bool _restoring;

    /// <summary>Zacne historiu otvoreneho grafikonu (po otvoreni, novom grafikone a zmene globalnych nastaveni).</summary>
    private void StartHistory(bool saved)
    {
        _commitPending = false;
        _changeLabel = null;
        _history = new GrafikonHistory(() => GrafikonState.Capture(_ctx.Document, _ctx.Directory), saved);
        UpdateHistoryUi();
    }

    /// <summary>Zavrety grafikon - nic sa neda vratit.</summary>
    private void ClearHistory()
    {
        _history = null;
        _commitPending = false;
        _changeLabel = null;
        UpdateHistoryUi();
    }

    /// <summary>Grafikon sa zmenil operaciou <paramref name="label" /> - jeden krok spat.</summary>
    private void MarkChanged(string label)
    {
        _changeLabel ??= label;
        DataSaved = false;
    }

    /// <summary>Zapamata stav po dokonceni prave spracovavanej operacie (vola ho oznacenie grafikonu ako zmeneneho).</summary>
    private void ScheduleCommit()
    {
        if (_history is null || _restoring || _commitPending || !IsHandleCreated)
            return;
        _commitPending = true;
        BeginInvoke(FlushCommit);
    }

    /// <summary>Zapamata cakajuci krok hned - pred ulozenim a pred krokom spat.</summary>
    private void FlushCommit()
    {
        if (!_commitPending)
            return;
        _commitPending = false;
        var label = _changeLabel ?? Resources.History_Change;
        _changeLabel = null;
        if (_history is null)
            return;

        // zmena, ktora grafikon v skutocnosti nezmenila (napr. prekreslenie tabulky) - ak je ulozeny stav, hviezdicka zmizne
        if (!_history.Commit(label) && _history.IsSaved)
            DataSaved = true;
        UpdateHistoryUi();
    }

    private void Undo() => Undo(1);

    private void Redo() => Redo(1);

    private void Undo(int steps) => MoveInHistory(history => history.Undo(steps));

    private void Redo(int steps) => MoveInHistory(history => history.Redo(steps));

    private void MoveInHistory(Action<GrafikonHistory> move)
    {
        // rozpisana bunka je tiez uprava - najprv sa zapamata
        if (dgvTrains.IsCurrentCellInEditMode)
            dgvTrains.EndEdit();
        FlushCommit();
        if (_history is null)
            return;

        var dir = _ctx.Directory;
        var oldStation = dir?.GVD.ThisStation.Name ?? "";
        var oldPeriod = dir?.Period ?? "";
        var row = dgvTrains.CurrentCell?.RowIndex ?? -1;
        var column = dgvTrains.CurrentCell?.ColumnIndex ?? -1;

        _restoring = true;
        var prechod = _prechod;
        _prechod = true;
        try
        {
            move(_history);
            _ctx.Document.Tracks.ResetBindings();
            _ctx.Document.Operators.ResetBindings();
            _ctx.Document.Trains.ResetBindings();
            InvalidateVariants();
            FitNumberColumn();
            DataSaved = _history.IsSaved;
        }
        finally
        {
            _prechod = prechod;
            _restoring = false;
        }

        if (dir != null)
            RefreshStationAndPeriod(dir, oldStation, oldPeriod);
        if (row >= 0 && row < dgvTrains.RowCount && column >= 0 && dgvTrains.Columns[column].Visible)
            dgvTrains.CurrentCell = dgvTrains.Rows[row].Cells[column];
        UpdateHistoryUi();
        UpdateCommandStates();
    }

    /// <summary>Popisy prikazov a tlacidiel Spat a Znova podla historie.</summary>
    private void UpdateHistoryUi()
    {
        var undo = _history?.UndoLabel;
        var redo = _history?.RedoLabel;
        tsmiUndo.Text = undo is null ? Resources.Cmd_Undo : string.Format(CultureInfo.CurrentCulture, Resources.History_UndoFormat, undo);
        tsmiRedo.Text = redo is null ? Resources.Cmd_Redo : string.Format(CultureInfo.CurrentCulture, Resources.History_RedoFormat, redo);
        tssbUndo.ToolTipText = tsmiUndo.Text + ShortcutHint(GvdCommands.Undo.Id);
        tssbRedo.ToolTipText = tsmiRedo.Text + ShortcutHint(GvdCommands.Redo.Id);
        _commands[GvdCommands.Undo.Id].UpdateState();
        _commands[GvdCommands.Redo.Id].UpdateState();
    }

    private string ShortcutHint(string id) =>
        _commands[id].ShortcutKeys is var keys and not Keys.None ? $" ({new KeysConverter().ConvertToString(null, CultureInfo.CurrentCulture, keys)})" : "";

    /// <summary>Rozbalovacia ponuka tlacidla Spat alebo Znova - zoznam krokov, klik vrati alebo zopakuje vsetky po vybrany.</summary>
    private static void FillHistoryMenu(ToolStripSplitButton button, IReadOnlyList<string> labels, Action<int> run)
    {
        button.DropDownItems.Clear();
        for (var i = 0; i < labels.Count; i++)
        {
            var steps = i + 1;
            button.DropDownItems.Add(labels[i], null, (_, _) => run(steps));
        }
    }

    private void tssbUndo_DropDownOpening(object? sender, EventArgs e)
    {
        FlushCommit();
        FillHistoryMenu(tssbUndo, _history?.UndoLabels ?? [], Undo);
    }

    private void tssbRedo_DropDownOpening(object? sender, EventArgs e) =>
        FillHistoryMenu(tssbRedo, _history?.RedoLabels ?? [], Redo);
}
