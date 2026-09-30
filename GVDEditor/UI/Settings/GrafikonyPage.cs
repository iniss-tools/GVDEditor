using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranka Grafikony v okne Globalne nastavenia - grafikony v datovom priecinku s portami a farbou upravovanymi
/// priamo v tabulke. Porty a farba idu rovno do <see cref="DirList" /> grafikonu (Zrusit okna ich vrati),
/// odstraneny grafikon sa len zapamata - jeho priecinok sa presunie do Kosa az po OK.
/// </summary>
public partial class GrafikonyPage : UserControl, ISettingsPage
{
    private readonly GridPageSupport _grid;

    // text portu, ktory sa do grafikonu nezapisal, lebo nie je platny - ostava v bunke s chybou
    private readonly Dictionary<(GVDDirectory Grafikon, int Column), string> _invalid = [];
    private IList<GVDDirectory> _grafikony = [];
    private List<GVDDirectory> _removed = [];
    private bool _loading;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public GrafikonyPage()
    {
        InitializeComponent();
        dgv.AutoGenerateColumns = false;
        _grid = new GridPageSupport(dgv, lHint);
    }

    /// <inheritdoc />
    public event EventHandler? ProblemsChanged;

    /// <inheritdoc />
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? FirstProblem => _grid.FirstProblem;

    /// <inheritdoc />
    public void FocusFirstProblem() => _grid.FocusFirstProblem();

    /// <summary>
    /// Naplni tabulku grafikonmi - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name="grafikony">grafikony v priecinku; odstraneny grafikon sa z neho vyberie</param>
    /// <param name="removed">sem sa pridaju odstranene grafikony</param>
    public void LoadData(IList<GVDDirectory> grafikony, List<GVDDirectory> removed)
    {
        _grafikony = grafikony;
        _removed = removed;
        _grid.CaptureColors();
        Fill(null);
    }

    private GVDDirectory? Current => dgv.CurrentRow?.Tag as GVDDirectory;

    private void Fill(GVDDirectory? select)
    {
        _loading = true;
        dgv.Rows.Clear();
        foreach (var grafikon in _grafikony)
        {
            var gvd = grafikon.GVD;
            var dir = grafikon.Dir;
            var index = dgv.Rows.Add(gvd.ThisStation.Name,
                $"{gvd.StartValidTimeTable:dd.MM.yyyy} – {gvd.EndValidTimeTable:dd.MM.yyyy}",
                PortText(grafikon, colTablePort, dir.TablePort), PortText(grafikon, colReportPort, dir.ReportPort), null!);
            var row = dgv.Rows[index];
            row.Tag = grafikon;

            // grafikon priamo v DATA sa do zoznamu nezapisuje - porty a farba by sa stratili
            if (dir.IsDataRoot)
                foreach (var column in new[] { colTablePort, colReportPort, colColor })
                {
                    row.Cells[column.Index].ReadOnly = true;
                    GridPageSupport.MarkLocked(row.Cells[column.Index]);
                }
        }
        _loading = false;

        var selected = dgv.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => ReferenceEquals(r.Tag, select));
        if (selected is not null)
            dgv.CurrentCell = selected.Cells[colStation.Index];

        Check();
    }

    private string PortText(GVDDirectory grafikon, DataGridViewColumn column, int? port) =>
        _invalid.TryGetValue((grafikon, column.Index), out var text) ? text : port?.ToString(CultureInfo.InvariantCulture) ?? "";

    private void Check()
    {
        _grid.BeginCheck();
        foreach (DataGridViewRow row in dgv.Rows)
            foreach (var column in new[] { colTablePort, colReportPort })
                if (_invalid.TryGetValue(((GVDDirectory)row.Tag!, column.Index), out var text))
                    _grid.Report(row.Cells[column.Index], DirListRules.ParsePort(text, out _));

        _grid.Defer(UpdateSelection);
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateSelection()
    {
        var grafikon = Current;
        var editable = grafikon is not null && !grafikon.Dir.IsDataRoot;
        bColor.Enabled = editable;
        bNoColor.Enabled = editable && grafikon!.Dir.BackColor is not null;
        bDelete.Enabled = editable;
        bOpenDir.Enabled = grafikon is not null;

        _grid.ShowHint(grafikon is null ? null
            : grafikon.Dir.IsDataRoot ? Resources.GrafikonyPage_DataRoot
            : string.Format(CultureInfo.CurrentCulture, Resources.GrafikonyPage_Priecinok, grafikon.Dir.FullPath));
    }

    private void dgv_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_loading || e.RowIndex < 0 || (e.ColumnIndex != colTablePort.Index && e.ColumnIndex != colReportPort.Index))
            return;

        var cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
        var grafikon = (GVDDirectory)dgv.Rows[e.RowIndex].Tag!;
        var text = cell.Value as string ?? "";

        if (DirListRules.ParsePort(text, out var port) is null)
        {
            _invalid.Remove((grafikon, e.ColumnIndex));
            if (e.ColumnIndex == colTablePort.Index)
                grafikon.Dir.TablePort = port;
            else
                grafikon.Dir.ReportPort = port;

            // 0 aj " 5" sa zobrazia tak, ako sa zapisu
            _loading = true;
            cell.Value = port?.ToString(CultureInfo.InvariantCulture) ?? "";
            _loading = false;
        }
        else
        {
            _invalid[(grafikon, e.ColumnIndex)] = text;
        }

        Check();
    }

    private void dgv_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != colColor.Index || e.Graphics is null ||
            dgv.Rows[e.RowIndex].Tag is not GVDDirectory grafikon)
            return;

        e.PaintBackground(e.CellBounds, true);

        var color = grafikon.Dir.BackColor;
        var selected = (e.State & DataGridViewElementStates.Selected) != 0;
        var fore = grafikon.Dir.IsDataRoot ? SystemColors.GrayText
            : selected ? e.CellStyle!.SelectionForeColor : e.CellStyle!.ForeColor;

        var bounds = e.CellBounds;
        var textLeft = bounds.X + 4;
        if (color is { } c)
        {
            var swatch = new Rectangle(bounds.X + 5, bounds.Y + (bounds.Height - 12) / 2, 18, 12);
            using (var brush = new SolidBrush(c))
                e.Graphics.FillRectangle(brush, swatch);
            using (var pen = new Pen(fore))
                e.Graphics.DrawRectangle(pen, swatch);
            textLeft = swatch.Right + 6;
        }

        var text = color is { } value ? $"#{value.R:X2}{value.G:X2}{value.B:X2}" : Resources.GrafikonyPage_Z_palety;
        var textBounds = new Rectangle(textLeft, bounds.Y, bounds.Right - textLeft - 2, bounds.Height);
        TextRenderer.DrawText(e.Graphics, text, e.CellStyle?.Font, textBounds, fore,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        e.Handled = true;
    }

    private void bColor_Click(object? sender, EventArgs e)
    {
        if (Current is not { } grafikon || grafikon.Dir.IsDataRoot)
            return;

        using var dialog = new ColorDialog();
        dialog.FullOpen = true;
        dialog.Color = grafikon.Dir.BackColor ?? Color.White;
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        grafikon.Dir.BackColor = Color.FromArgb(dialog.Color.R, dialog.Color.G, dialog.Color.B);
        ColorChanged();
    }

    private void bNoColor_Click(object? sender, EventArgs e)
    {
        if (Current is not { } grafikon || grafikon.Dir.IsDataRoot)
            return;

        grafikon.Dir.BackColor = null;
        ColorChanged();
    }

    private void ColorChanged()
    {
        if (dgv.CurrentRow is { } row)
            dgv.InvalidateCell(row.Cells[colColor.Index]);
        _grid.Defer(UpdateSelection);
    }

    private void bOpenDir_Click(object? sender, EventArgs e)
    {
        if (Current is { } grafikon)
            Utils.OpenShell(grafikon.Dir.FullPath);
    }

    private void bDelete_Click(object? sender, EventArgs e)
    {
        // grafikon priamo v DATA by sa odstranil s celym datovym priecinkom
        if (Current is not { } grafikon || grafikon.Dir.IsDataRoot)
            return;

        var question = string.Format(CultureInfo.CurrentCulture, Resources.GrafikonyPage_Odstranit,
            grafikon.GVD.ThisStation.Name, grafikon.Period, grafikon.Dir.DirName);
        if (Utils.ShowQuestion(question) != DialogResult.Yes)
            return;

        var index = _grafikony.IndexOf(grafikon);
        _removed.Add(grafikon);
        _grafikony.RemoveAt(index);
        _invalid.Remove((grafikon, colTablePort.Index));
        _invalid.Remove((grafikon, colReportPort.Index));

        Fill(_grafikony.Count == 0 ? null : _grafikony[Math.Min(index, _grafikony.Count - 1)]);
    }

    private void dgv_CurrentCellChanged(object? sender, EventArgs e) => _grid.Defer(UpdateSelection);

    private void dgv_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
            return;

        if (e.ColumnIndex == colColor.Index)
            bColor_Click(this, EventArgs.Empty);
        else if (!dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
            dgv.BeginEdit(true);
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e)
    {
        if (dgv.CurrentCell is not { } cell)
            return;

        // Delete vymaze port; grafikon sa odstranuje len tlacidlom (presunie priecinok do Kosa)
        if (e.KeyCode == Keys.Delete && !cell.ReadOnly && (cell.ColumnIndex == colTablePort.Index || cell.ColumnIndex == colReportPort.Index))
        {
            cell.Value = "";
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Enter && cell.ColumnIndex == colColor.Index)
        {
            bColor_Click(this, EventArgs.Empty);
            e.Handled = true;
        }
    }
}
