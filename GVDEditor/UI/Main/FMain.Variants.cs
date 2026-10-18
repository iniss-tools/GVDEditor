using System.Globalization;
using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using GVDEditor.UI.EditTrain;
using ToolsCore;
using ToolsCore.Tools;
using ToolsCore.Icons;

namespace GVDEditor.UI.Main;

internal partial class FMain
{
    // prehlad variant pre zoznam vlakov - postavi sa znova az pri kresleni po zmene zoznamu
    private VariantIndex? _variantIndex;

    // riadok, ktoreho varianty su v zozname zvyraznene
    private int _variantRow = -1;

    private VariantIndex Variants => _variantIndex ??= VariantIndex.Build(_ctx.Document.Trains);

    private void InvalidateVariants()
    {
        _variantIndex = null;
        dgvTrains.Invalidate();
    }

    /// <summary>
    /// Prideli cisla variant (<see cref="TrainVariants.Normalize" />) a obnovi zoznam, ak sa niektore zmenilo.
    /// </summary>
    private void NormalizeVariants()
    {
        if (TrainVariants.Normalize(_ctx.Document.Trains).Count != 0)
            _ctx.Document.Trains.ResetBindings();
        InvalidateVariants();
        FitNumberColumn();
    }

    /// <summary>
    /// Rozsiri stlpec Cislo, aby sa zmestilo poradie varianty aj s ikonou upozornenia (napr. „4327  1/2“ a ikona).
    /// </summary>
    private void FitNumberColumn()
    {
        var column = cisloDataGridViewTextBoxColumn;
        if (dgvTrains.DataSource == null || column.AutoSizeMode != DataGridViewAutoSizeColumnMode.None)
            return;

        var preferred = column.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
        // ikona upozornenia je v okraji bunky z formatovania - sirka podla obsahu ho nezapocita
        if (_ctx.Document.Trains.Any(t => Variants.Of(t) is { Count: > 1, Overlaps.Count: > 0 }))
            preferred += OverlapIconSpace;
        if (preferred > column.Width)
            column.Width = preferred;
    }

    // okraj ikony upozornenia na prekrytie dni variant v bunke cisla vlaku
    private const int OverlapIconMargin = 4;

    /// <summary>
    /// Miesto vpravo v bunke cisla vlaku pre ikonu upozornenia (okraj bunky, preto s nim pocita aj sirka stlpca).
    /// </summary>
    private int OverlapIconSpace => dgvTrains.LogicalToDeviceUnits(IconImages.Size + 2 * OverlapIconMargin);

    /// <summary>
    /// Ikona upozornenia v bunke cisla vlaku, ktorej dni sa prekryvaju s inou variantou.
    /// </summary>
    private void dgvTrains_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.Handled || e.Graphics is null || e.RowIndex < 0 || e.RowIndex >= _ctx.Document.Trains.Count || e.ColumnIndex != cisloDataGridViewTextBoxColumn.Index
            || Variants.Of(_ctx.Document.Trains[e.RowIndex]) is not { Count: > 1, Overlaps.Count: > 0 })
            return;

        e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
        var size = dgvTrains.LogicalToDeviceUnits(IconImages.Size);
        var selected = (e.State & DataGridViewElementStates.Selected) != 0;
        var back = e.CellStyle is null ? dgvTrains.BackgroundColor : selected ? e.CellStyle.SelectionBackColor : e.CellStyle.BackColor;
        if (AppIcons.Get("status-warning", null, size, AppIcons.IsDarkBackground(back)) is { } icon)
        {
            var x = e.CellBounds.Right - dgvTrains.LogicalToDeviceUnits(OverlapIconMargin) - size;
            e.Graphics.DrawImage(icon, new Rectangle(x, e.CellBounds.Top + (e.CellBounds.Height - size) / 2, size, size));
        }

        e.Handled = true;
    }

    private Train? CurrentTrain =>
        dgvTrains.CurrentRow is { Index: var index } && index >= 0 && index < _ctx.Document.Trains.Count ? _ctx.Document.Trains[index] : null;

    /// <summary>
    /// Kontextove menu zoznamu vlakov s prikazmi pre varianty.
    /// </summary>
    private void CreateVariantMenu()
    {
        var add = new ToolStripMenuItem(Resources.FMain_Variant_Pridat);
        var show = new ToolStripMenuItem(Resources.FMain_Variant_Varianty);
        var reorder = new ToolStripMenuItem(Resources.FMain_Variant_Usporiadat);
        var menu = new ContextMenuStrip();
        menu.Items.AddRange([add, show, reorder]);
        menu.Opening += (_, e) =>
        {
            // tema sa mohla zmenit v nastaveniach programu
            FormUtils.ChangeColorContextMenu(GlobSettings.UsingStyle, menu);
            var train = CurrentTrain;
            e.Cancel = train == null || !HasGrafikon;
            reorder.Enabled = train != null && Variants.Of(train).Count > 1;
        };
        add.Click += (_, _) =>
        {
            // kopia s rovnakym cislom, nazvom a typom je dalsou variantou - dni sa rozdelia na stranke Platnost
            if (CurrentTrain is { } train)
                ShowEditTrain(train, _ctx.Document.Trains.Count, true, EditTrainPage.Platnost);
        };
        show.Click += (_, _) =>
        {
            if (CurrentTrain is { } train)
                ShowEditTrain(train, _ctx.Document.Trains.IndexOf(train), false, EditTrainPage.Platnost);
        };
        reorder.Click += (_, _) => ReorderVariants();
        dgvTrains.ContextMenuStrip = menu;
        dgvTrains.CellMouseDown += (_, e) =>
        {
            // pravy klik vyberie riadok, na ktory sa menu vztahuje
            if (e.Button != MouseButtons.Right || e.RowIndex < 0)
                return;

            dgvTrains.ClearSelection();
            dgvTrains.CurrentCell = dgvTrains.Rows[e.RowIndex].Cells[Math.Max(0, e.ColumnIndex)];
            dgvTrains.Rows[e.RowIndex].Selected = true;
        };
        dgvTrains.CurrentCellChanged += (_, _) =>
        {
            var row = dgvTrains.CurrentRow?.Index ?? -1;
            if (row == _variantRow)
                return;

            _variantRow = row;
            dgvTrains.Invalidate();
        };
        dgvTrains.CellToolTipTextNeeded += (_, e) =>
        {
            if (e.ColumnIndex == cisloDataGridViewTextBoxColumn.Index && e.RowIndex >= 0 && e.RowIndex < _ctx.Document.Trains.Count)
                e.ToolTipText = VariantToolTip(_ctx.Document.Trains[e.RowIndex]);
        };
    }

    /// <summary>
    /// Bublina cisla vlaku s variantmi: zoznam variant s trasou a dnami, prekrytie s inymi variantmi.
    /// </summary>
    private string VariantToolTip(Train train)
    {
        var info = Variants.Of(train);
        if (info.Count < 2)
            return "";

        var home = _previousSelectedGVD?.GVD.ThisStation?.Name ?? "";
        var lines = new List<string> { string.Format(CultureInfo.CurrentCulture, Resources.FMain_Variant_Zoznam, TrainRules.Label(train)) };
        for (var i = 0; i < info.Group.Count; i++)
        {
            var member = info.Group[i];
            lines.Add($"{i + 1}/{info.Count}  {member.StartingStation?.Name ?? home} → {member.EndingStation?.Name ?? home}  " +
                      $"{member.DateLimitText}");
        }

        foreach (var (other, days) in info.Overlaps)
            lines.Add(string.Format(CultureInfo.CurrentCulture, Resources.FMain_Variant_Prekrytie,
                $"{Variants.Of(other).Position}/{info.Count}", days));

        return string.Join(Environment.NewLine, lines);
    }

    /// <summary>
    /// Zoradi varianty vybraneho vlaku podla dlzky trasy a dni, v ktore by islo viac variant naraz, necha len
    /// variante s najdlhsou trasou.
    /// </summary>
    private void ReorderVariants()
    {
        if (CurrentTrain is not { } train)
            return;

        var group = Variants.Of(train).Group.ToList();
        if (group.Count < 2 ||
            _dialogs.ShowQuestion(string.Format(CultureInfo.CurrentCulture, Resources.FMain_Variant_UsporiadatOtazka, TrainRules.Label(train)))
            != DialogResult.Yes)
            return;

        try
        {
            Train.ReorderVariants(group);
        }
        catch (DateLimit.ParseException exception)
        {
            _dialogs.ShowError(exception.Message);
            return;
        }

        _ctx.Document.Trains.ResetBindings();
        InvalidateVariants();
        MarkChanged(Resources.History_Variants);
    }

    private int CountSelTrainVariants(Train train) => Variants.Of(train).Count;
}
