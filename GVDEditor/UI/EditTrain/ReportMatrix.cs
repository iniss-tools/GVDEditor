using ExControls;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using ToolsCore;

namespace GVDEditor.UI.EditTrain;

/// <summary>
/// Tabulka Kedy hlasit: riadok pre kazdy typ hlasenia, stlpec pre kazdu variantu (dlhe, kratke). Zaskrtnutie
/// meni priamo zoznam vybranych hlaseni dodatku alebo radenia.
/// </summary>
public sealed class ReportMatrix : UserControl
{
    private readonly DataGridView _grid;
    private List<ChosenReportType>? _chosen;
    private IReadOnlyList<ReportType> _types = [];
    private IReadOnlyList<ReportVariant> _variants = [];
    private bool _loading;

    /// <summary>
    /// Vytvori prazdnu tabulku; typy a varianty nastavi <see cref="Bind" />.
    /// </summary>
    public ReportMatrix()
    {
        _grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            AllowUserToResizeColumns = false,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
            RowHeadersVisible = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.CellSelect,
            EditMode = DataGridViewEditMode.EditOnEnter,
            TabIndex = 0
        };
        _grid.CurrentCellDirtyStateChanged += Grid_CurrentCellDirtyStateChanged;
        _grid.CellValueChanged += Grid_CellValueChanged;
        Controls.Add(_grid);
    }

    /// <summary>
    /// Pouzivatel zaskrtol alebo odskrtol hlasenie.
    /// </summary>
    public event EventHandler? Changed;

    /// <summary>
    /// Zobrazi hlasenia <paramref name="types" /> vo variantach <paramref name="variants" /> a zaskrtne vybrane
    /// v <paramref name="chosen" />; <see langword="null" /> = nic nie je vybrane na upravu (tabulka je nedostupna).
    /// </summary>
    internal void Bind(List<ChosenReportType>? chosen, IReadOnlyList<ReportType> types, IReadOnlyList<ReportVariant> variants)
    {
        _loading = true;
        _chosen = chosen;
        if (!ReferenceEquals(types, _types) || !ReferenceEquals(variants, _variants) || _grid.ColumnCount == 0)
        {
            _types = types;
            _variants = variants;
            BuildColumns();
        }

        for (var i = 0; i < _types.Count; i++)
            for (var j = 0; j < _variants.Count; j++)
                _grid.Rows[i].Cells[j + 1].Value = chosen != null && ReportChoices.IsChosen(chosen, _types[i], _variants[j]);

        _grid.Enabled = chosen != null;
        _loading = false;
    }

    private void BuildColumns()
    {
        _grid.Rows.Clear();
        _grid.Columns.Clear();
        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "",
            ReadOnly = true,
            SortMode = DataGridViewColumnSortMode.NotSortable,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            MinimumWidth = 100
        });

        var style = GlobSettings.UsingStyle;
        var scheme = style.ControlsColorScheme;
        foreach (var variant in _variants)
        {
            var column = new DataGridViewExCheckBoxColumn
            {
                HeaderText = variant.Name,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader,
                MinimumWidth = 60,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DefaultStyle = style.ControlsDefaultStyle
            };
            if (!style.ControlsDefaultStyle)
            {
                column.BorderColor = scheme.Border.ForeColor;
                column.MarkColor = scheme.Mark.ForeColor;
                column.SquareBackColor = scheme.Panel.BackColor;
                column.HighlightColor = scheme.Highlight.BackColor;
            }

            _grid.Columns.Add(column);
        }

        foreach (var type in _types)
            _grid.Rows.Add(type.Name);
    }

    // zaskrtnutie sa zapise hned, nie az pri opusteni bunky
    private void Grid_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (_grid.IsCurrentCellDirty)
            _grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void Grid_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_loading || _chosen == null || e.RowIndex < 0 || e.ColumnIndex < 1)
            return;

        ReportChoices.Set(_chosen, _types[e.RowIndex], _variants[e.ColumnIndex - 1],
            _grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value is true, _variants);
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
