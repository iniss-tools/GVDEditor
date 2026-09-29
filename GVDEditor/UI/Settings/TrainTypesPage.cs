using System.Globalization;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranka Typy vlakov v okne Globalne nastavenia - zabudovane aj vlastne druhy (TrTypes.txt) v jednej tabulke
/// s upravou priamo v bunkach. Druh urcuje kategoriu a tym farbu vlaku v zozname INISSu; vlastnym typom
/// pridelí volne miesto (napr. R3) stranka sama. Zmeny idu rovno do <see cref="GlobData.TrainsTypes" />.
/// </summary>
public partial class TrainTypesPage : UserControl, ISettingsPage
{
    private const string BuiltinPrefix = "b:";
    private const string CustomPrefix = "c:";

    /// <summary>
    /// Vzhlad riadka vlaku v hlavnom zozname INISSu podla kategorie (farba textu a pozadia).
    /// </summary>
    private enum Look
    {
        Default,
        Blue,
        Bus,
        Red,
        BlueGray,
        GreenGray,
        Service
    }

    /// <summary>
    /// Polozka ponuky Druh - hodnota "b:Os" (zabudovany druh) alebo "c:R" (skupina vlastnych typov).
    /// </summary>
    private sealed record KindOption(string Value, string Text);

    private readonly GridPageSupport _grid;

    // vlaky, ktore typ pouzivaju, podla grafikonov (zistene pri otvoreni okna podla povodnej skratky)
    private readonly Dictionary<TrainType, List<(string Grafikon, int Count)>> _usage = new(ReferenceEqualityComparer.Instance);
    private bool _loading;
    private bool _refreshPending;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public TrainTypesPage()
    {
        InitializeComponent();
        dgv.AutoGenerateColumns = false;
        _grid = new GridPageSupport(dgv, lHint);
        pSlots.Resize += (_, _) => pSlots.Invalidate();
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
    /// Naplni tabulku typmi vlakov - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name="grafikony">vsetky grafikony instalacie (pre pocet vlakov kazdeho typu)</param>
    /// <param name="open">otvoreny grafikon - jeho vlaky sa beru z pamate</param>
    public void LoadData(IEnumerable<GVDDirectory> grafikony, GVDDirectory? open)
    {
        _grid.CaptureColors();
        pSlots.MinimumSize = pSlots.Size = SlotsSize(pSlots.Font);

        var counts = TrainTypeUsage.CountAll(grafikony, open, GlobData.Trains);
        foreach (var type in GlobData.TrainsTypes)
            _usage[type] = counts.TryGetValue(type.Key, out var list) ? list : [];

        _loading = true;
        dgv.Rows.Clear();
        foreach (var type in GlobData.TrainsTypes)
            dgv.Rows[dgv.Rows.Add()].Tag = type;
        _loading = false;

        RefreshRows();
    }

    private TrainType? CurrentType => dgv.CurrentRow?.Tag as TrainType;

    private int UsedBy(TrainType type) => _usage.TryGetValue(type, out var list) ? list.Sum(u => u.Count) : 0;

    /// <summary>
    /// Obnovi vsetky bunky podla typov - ponuka druhov zavisi od ostatnych riadkov (obsadene druhy a miesta).
    /// </summary>
    private void RefreshRows()
    {
        _refreshPending = false;
        _loading = true;
        try
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                var type = (TrainType)row.Tag!;
                var used = UsedBy(type);

                row.Cells[colKey.Index].Value = type.Key;
                var key = row.Cells[colKey.Index];
                key.ReadOnly = used > 0;
                key.ToolTipText = used > 0 ? Resources.TrainTypesPage_Zamknuta : "";
                key.Style.ForeColor = used > 0 ? SystemColors.GrayText : Color.Empty;

                row.Cells[colText.Index].Value = type.TextInTable;

                var kind = (DataGridViewComboBoxCell)row.Cells[colKind.Index];
                var options = KindOptions(type);
                kind.DataSource = options;
                kind.DisplayMember = nameof(KindOption.Text);
                kind.ValueMember = nameof(KindOption.Value);
                kind.Value = KindValue(type);

                row.Cells[colCategory.Index].Value = type.CategoryTrain;
                row.Cells[colTrains.Index].Value = used > 0 ? used.ToString(CultureInfo.CurrentCulture) : "–";
                ShowLook(row, type);
            }
        }
        finally
        {
            _loading = false;
        }

        ApplyFilter();
        Check();
    }

    private void ShowLook(DataGridViewRow row, TrainType type)
    {
        var (fore, back) = Colors(LookOf(type.CategoryTrain));
        var cell = row.Cells[colLook.Index];
        cell.Value = type.Key;
        cell.Style.ForeColor = cell.Style.SelectionForeColor = fore;
        cell.Style.BackColor = cell.Style.SelectionBackColor = back;
    }

    // TrainType porovnava podla hodnot - riadok patri konkretnemu objektu
    private static int IndexOf(TrainType type)
    {
        for (var i = 0; i < GlobData.TrainsTypes.Count; i++)
            if (ReferenceEquals(GlobData.TrainsTypes[i], type))
                return i;
        return -1;
    }

    private static string KindValue(TrainType type) =>
        TrainTypeRules.GroupOf(type.CategoryTrain) is { } group ? CustomPrefix + group : BuiltinPrefix + type.CategoryTrain;

    /// <summary>
    /// Ponuka druhov pre typ: skupiny vlastnych typov s volnym miestom a zabudovane druhy, ktore nema iny typ.
    /// </summary>
    private static List<KindOption> KindOptions(TrainType type)
    {
        var options = new List<KindOption>();
        var ownGroup = TrainTypeRules.GroupOf(type.CategoryTrain);
        foreach (var group in TrainTypeRules.CustomGroups)
        {
            var free = TrainTypeRules.SlotsPerGroup - TrainTypeRules.CountInGroup(GlobData.TrainsTypes, group, type);
            if (free > 0 || group == ownGroup)
                options.Add(new KindOption(CustomPrefix + group, string.Format(CultureInfo.CurrentCulture,
                    Resources.TrainTypesPage_Vlastny, GroupName(group), group, Math.Max(0, free))));
        }

        var taken = GlobData.TrainsTypes.Where(t => !ReferenceEquals(t, type)).Select(t => t.CategoryTrain).ToHashSet();
        foreach (var builtin in TrainType.GetDefaultValues().Select(t => t.CategoryTrain))
            if (!taken.Contains(builtin) || builtin == type.CategoryTrain)
                options.Add(new KindOption(BuiltinPrefix + builtin,
                    string.Format(CultureInfo.CurrentCulture, Resources.TrainTypesPage_Zabudovany, builtin)));

        // kategoria zo suboru, ktoru ponuka nepozna (napr. R10), sa musi dat zobrazit
        var value = KindValue(type);
        if (options.All(o => o.Value != value))
            options.Add(new KindOption(value, type.CategoryTrain));
        return options;
    }

    private static string GroupName(string group) => group switch
    {
        "R" => Resources.TrainTypesPage_Skupina_R,
        "X" => Resources.TrainTypesPage_Skupina_X,
        "Sl" => Resources.TrainTypesPage_Skupina_Sl,
        _ => Resources.TrainTypesPage_Skupina_Os
    };

    /// <summary>
    /// Vzhlad v zozname INISSu podla zabudovanej tabulky druhov (vlastne typy preberaju vzhlad svojej skupiny).
    /// </summary>
    private static Look LookOf(string category) => TrainTypeRules.GroupOf(category) switch
    {
        "Os" => Look.Default,
        "R" => Look.Red,
        "X" => Look.GreenGray,
        "Sl" => Look.Service,
        _ => category switch
        {
            "Sp" or "Zr" => Look.Blue,
            "Bus" => Look.Bus,
            "R" or "Ex" or "REX" or "ER" => Look.Red,
            "EC" or "IC" => Look.BlueGray,
            "SC" or "ICE" or "EN" or "NZ" or "TGV" => Look.GreenGray,
            "Nákl" or "Sl" or "Rn" or "Rp" => Look.Service,
            _ => Look.Default
        }
    };

    private static (Color Fore, Color Back) Colors(Look look) => look switch
    {
        Look.Blue => (Color.Blue, Color.White),
        Look.Bus => (Color.Black, Color.LightYellow),
        Look.Red => (Color.Red, Color.White),
        Look.BlueGray => (Color.Blue, Color.LightGray),
        Look.GreenGray => (Color.Green, Color.LightGray),
        Look.Service => (Color.Black, Color.LightGreen),
        _ => (Color.Black, Color.White)
    };

    private static string LookText(Look look) => look switch
    {
        Look.Blue => Resources.TrainTypesPage_Vzhlad_Blue,
        Look.Bus => Resources.TrainTypesPage_Vzhlad_Bus,
        Look.Red => Resources.TrainTypesPage_Vzhlad_Red,
        Look.BlueGray => Resources.TrainTypesPage_Vzhlad_BlueGray,
        Look.GreenGray => Resources.TrainTypesPage_Vzhlad_GreenGray,
        Look.Service => Resources.TrainTypesPage_Vzhlad_Service,
        _ => Resources.TrainTypesPage_Vzhlad_Def
    };

    private void Check()
    {
        _grid.BeginCheck();
        var types = GlobData.TrainsTypes.ToList();
        foreach (DataGridViewRow row in dgv.Rows)
        {
            var type = (TrainType)row.Tag!;
            var index = IndexOf(type);
            _grid.Report(row.Cells[colKey.Index], TrainTypeRules.CheckKey(types, index));
            _grid.Report(row.Cells[colText.Index], TrainTypeRules.CheckText(type.TextInTable));
            _grid.Report(row.Cells[colKind.Index], TrainTypeRules.CheckCategory(types, index));
        }

        _grid.Defer(UpdateSelection);
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateSelection()
    {
        var type = CurrentType;
        bDelete.Enabled = type is not null && UsedBy(type) == 0;
        _grid.ShowHint(null);
        lDetail.Text = type is null ? "" : Detail(type);
        pSlots.Invalidate();
    }

    private string Detail(TrainType type)
    {
        var lines = new List<string>();
        var name = type.Key.Length > 0 ? type.Key : "–";
        lines.Add(TrainTypeRules.GroupOf(type.CategoryTrain) is { } group
            ? string.Format(CultureInfo.CurrentCulture, Resources.TrainTypesPage_Detail_Vlastny, name, type.CategoryTrain, GroupName(group))
            : string.Format(CultureInfo.CurrentCulture, Resources.TrainTypesPage_Detail_Zabudovany, name, type.CategoryTrain));
        lines.Add(string.Format(CultureInfo.CurrentCulture, Resources.TrainTypesPage_Detail_Vzhlad, LookText(LookOf(type.CategoryTrain))));

        var usage = _usage.TryGetValue(type, out var list) ? list : [];
        if (usage.Count == 0)
            lines.Add(Resources.TrainTypesPage_Nepouziva);
        else
        {
            lines.Add(string.Format(CultureInfo.CurrentCulture, Resources.TrainTypesPage_Pouzivaju,
                string.Join(", ", usage.Select(u => $"{u.Grafikon} ({u.Count})"))));
            lines.Add(Resources.TrainTypesPage_Zamknuta);
        }

        return string.Join(Environment.NewLine, lines);
    }

    /// <summary>
    /// Obsadenost skupin vlastnych typov: nazov skupiny, devat policok a pocet. Kresli sa, aby boli policka
    /// pod sebou zarovnane (v texte s proporcionalnym pismom by nesedeli).
    /// </summary>
    private void pSlots_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        var font = pSlots.Font;
        var fore = lDetail.ForeColor;
        // velkost plochy podla pisma, ktorym sa naozaj kresli (okno ho po nacitani este skaluje)
        var required = SlotsSize(font);
        if (pSlots.Size != required)
        {
            pSlots.MinimumSize = pSlots.Size = required;
            return;
        }

        g.Clear(pSlots.BackColor);
        var line = font.Height + 4;
        var box = Math.Max(6, font.Height - 5);
        var gap = Math.Max(2, box / 4);
        var labelWidth = TrainTypeRules.CustomGroups.Max(group => TextRenderer.MeasureText(GroupRange(group), font).Width) + 8;
        var flags = TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter;

        TextRenderer.DrawText(g, Resources.TrainTypesPage_Sloty, font, new Rectangle(0, 0, pSlots.Width, line), fore, flags);
        using var fill = new SolidBrush(GlobData.UsingStyle.ControlsColorScheme.Highlight.BackColor);
        using var border = new Pen(Color.FromArgb(140, fore));
        var y = line;
        foreach (var group in TrainTypeRules.CustomGroups)
        {
            var used = Math.Min(TrainTypeRules.SlotsPerGroup, TrainTypeRules.CountInGroup(GlobData.TrainsTypes, group));
            TextRenderer.DrawText(g, GroupRange(group), font, new Rectangle(0, y, labelWidth, line), fore, flags);
            var x = labelWidth;
            for (var i = 0; i < TrainTypeRules.SlotsPerGroup; i++, x += box + gap)
            {
                var r = new Rectangle(x, y + (line - box) / 2, box, box);
                if (i < used)
                    g.FillRectangle(fill, r);
                g.DrawRectangle(border, r);
            }

            TextRenderer.DrawText(g, string.Format(CultureInfo.CurrentCulture, "{0}/{1}", used, TrainTypeRules.SlotsPerGroup),
                font, new Rectangle(x + 4, y, pSlots.Width - x - 4, line), fore, flags | TextFormatFlags.NoClipping);
            y += line;
        }
    }

    private static string GroupRange(string group) => $"{group}1–{group}9";

    /// <summary>
    /// Velkost plochy s obsadenostou pre pismo <paramref name="font" />.
    /// </summary>
    private static Size SlotsSize(Font font)
    {
        var line = font.Height + 4;
        var box = Math.Max(6, font.Height - 5);
        var labelWidth = TrainTypeRules.CustomGroups.Max(group => TextRenderer.MeasureText(GroupRange(group), font).Width) + 8;
        var width = Math.Max(TextRenderer.MeasureText(Resources.TrainTypesPage_Sloty, font).Width,
            labelWidth + TrainTypeRules.SlotsPerGroup * (box + Math.Max(2, box / 4)) + TextRenderer.MeasureText("9/9", font).Width + 12);
        return new Size(width + 4, line * (TrainTypeRules.CustomGroups.Length + 1) + 4);
    }

    /// <summary>
    /// Obnovenie riadkov az po skonceni udalosti tabulky - zmena ponuky v udalosti by bola vnorena.
    /// </summary>
    private void RequestRefresh()
    {
        if (_refreshPending)
            return;

        _refreshPending = true;
        BeginInvoke(RefreshRows);
    }

    private static void Changed(TrainType type)
    {
        var index = IndexOf(type);
        if (index >= 0)
            GlobData.TrainsTypes.ResetItem(index);
    }

    private void dgv_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_loading || e.RowIndex < 0)
            return;

        var row = dgv.Rows[e.RowIndex];
        var type = (TrainType)row.Tag!;
        var value = row.Cells[e.ColumnIndex].Value as string ?? "";

        if (e.ColumnIndex == colKey.Index)
        {
            // text na tabuli ide so skratkou, kym ho pouzivatel nezmeni
            var oldKey = type.Key;
            type.Key = value.Trim();
            if (type.TextInTable.Length == 0 || type.TextInTable == oldKey)
            {
                type.TextInTable = type.Key;
                _loading = true;
                row.Cells[colText.Index].Value = type.TextInTable;
                _loading = false;
            }

            ShowLook(row, type);
            Check();
        }
        else if (e.ColumnIndex == colText.Index)
        {
            type.TextInTable = value.Trim();
            Check();
        }
        else if (e.ColumnIndex == colKind.Index && value != KindValue(type))
        {
            SetKind(type, value);
            RequestRefresh();
        }
        else
            return;

        Changed(type);
    }

    /// <summary>
    /// Zmena druhu. Zabudovany druh predvyplni skratku a text (ak ich pouzivatel nezmenil), vlastny typ dostane
    /// volne miesto v skupine.
    /// </summary>
    private void SetKind(TrainType type, string value)
    {
        var oldCategory = type.CategoryTrain;
        var oldKey = type.Key;
        if (value.StartsWith(BuiltinPrefix, StringComparison.Ordinal))
        {
            var category = value[BuiltinPrefix.Length..];
            type.CategoryTrain = category;
            if (UsedBy(type) == 0 && (oldKey.Length == 0 || oldKey == oldCategory))
                type.Key = category;
            if (type.TextInTable.Length == 0 || type.TextInTable == oldKey || type.TextInTable == oldCategory)
                type.TextInTable = type.Key;
        }
        else
        {
            // docasne za poslednym miestom - precislovanie ho zaradi podla poradia v zozname
            type.CategoryTrain = value[CustomPrefix.Length..] + "99";
        }

        TrainTypeRules.Renumber(GlobData.TrainsTypes);
    }

    private void bAdd_Click(object? sender, EventArgs e)
    {
        var group = TrainTypeRules.CustomGroups.FirstOrDefault(g =>
            TrainTypeRules.CountInGroup(GlobData.TrainsTypes, g) < TrainTypeRules.SlotsPerGroup);
        if (group is null)
        {
            ToolsCore.Tools.Utils.ShowError(Resources.FGlobalSettings_Maximálny_počet_typov_vlakov_tohto_druhu_je_9);
            return;
        }

        var type = new TrainType(group + "99", "", "");
        GlobData.TrainsTypes.Add(type);
        TrainTypeRules.Renumber(GlobData.TrainsTypes);
        _usage[type] = [];

        tbFilter.Text = "";
        dgv.Rows[dgv.Rows.Add()].Tag = type;
        RefreshRows();
        _grid.Edit(dgv.Rows.Count - 1, colKey);
    }

    private void bDelete_Click(object? sender, EventArgs e)
    {
        if (CurrentType is not { } type || UsedBy(type) > 0)
            return;

        GlobData.TrainsTypes.RemoveAt(IndexOf(type));
        _usage.Remove(type);
        TrainTypeRules.Renumber(GlobData.TrainsTypes);
        dgv.Rows.RemoveAt(dgv.CurrentRow!.Index);
        RefreshRows();
    }

    private void tbFilter_TextChanged(object? sender, EventArgs e) => ApplyFilter();

    private void ApplyFilter()
    {
        var filter = tbFilter.Text.Trim();
        foreach (DataGridViewRow row in dgv.Rows)
        {
            var type = (TrainType)row.Tag!;
            var visible = filter.Length == 0 ||
                          type.Key.Contains(filter, StringComparison.CurrentCultureIgnoreCase) ||
                          type.TextInTable.Contains(filter, StringComparison.CurrentCultureIgnoreCase);
            if (!visible && dgv.CurrentRow == row)
                dgv.CurrentCell = null;
            row.Visible = visible;
        }
    }

    private void dgv_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        // vyber druhu sa prejavi hned, nie az po opusteni bunky
        if (dgv.IsCurrentCellDirty && dgv.CurrentCell is DataGridViewComboBoxCell)
            dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void dgv_DataError(object? sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgv_CurrentCellChanged(object? sender, EventArgs e) => _grid.Defer(UpdateSelection);

    private void dgv_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && !dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
            dgv.BeginEdit(true);
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e) =>
        GridPageSupport.HandleKeys(e, () => bAdd_Click(this, EventArgs.Empty), () => bDelete_Click(this, EventArgs.Empty));
}
