using System.Globalization;
using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using GVDEditor.UI.EditTrain;
using ToolsCore;
using ToolsCore.Tools;

namespace GVDEditor.UI.Main;

public partial class FMain
{
    // prehlad variant pre zoznam vlakov - postavi sa znova az pri kresleni po zmene zoznamu
    private VariantIndex? _variantIndex;

    // riadok, ktoreho varianty su v zozname zvyraznene
    private int _variantRow = -1;

    private VariantIndex Variants => _variantIndex ??= VariantIndex.Build(GlobData.Trains);

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
        if (TrainVariants.Normalize(GlobData.Trains).Count != 0)
            GlobData.Trains.ResetBindings();
        InvalidateVariants();
        FitNumberColumn();
    }

    /// <summary>
    /// Rozsiri stlpec Cislo, aby sa zmestilo poradie varianty aj s upozornenim (napr. „4327  1/2  ⚠“).
    /// </summary>
    private void FitNumberColumn()
    {
        var column = cisloDataGridViewTextBoxColumn;
        if (dgvTrains.DataSource == null || column.AutoSizeMode != DataGridViewAutoSizeColumnMode.None)
            return;

        var preferred = column.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
        if (preferred > column.Width)
            column.Width = preferred;
    }

    private Train? CurrentTrain =>
        dgvTrains.CurrentRow is { Index: var index } && index >= 0 && index < GlobData.Trains.Count ? GlobData.Trains[index] : null;

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
                ShowEditTrain(train, GlobData.Trains.Count, true, EditTrainPage.Platnost);
        };
        show.Click += (_, _) =>
        {
            if (CurrentTrain is { } train)
                ShowEditTrain(train, GlobData.Trains.IndexOf(train), false, EditTrainPage.Platnost);
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
            if (e.ColumnIndex == cisloDataGridViewTextBoxColumn.Index && e.RowIndex >= 0 && e.RowIndex < GlobData.Trains.Count)
                e.ToolTipText = VariantToolTip(GlobData.Trains[e.RowIndex]);
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
            Utils.ShowQuestion(string.Format(CultureInfo.CurrentCulture, Resources.FMain_Variant_UsporiadatOtazka, TrainRules.Label(train)))
            != DialogResult.Yes)
            return;

        try
        {
            Train.ReorderVariants(group);
        }
        catch (DateLimit.ParseException exception)
        {
            Utils.ShowError(exception.Message);
            return;
        }

        GlobData.Trains.ResetBindings();
        InvalidateVariants();
        DataSaved = false;
    }

    private int CountSelTrainVariants(Train train) => Variants.Of(train).Count;
}
