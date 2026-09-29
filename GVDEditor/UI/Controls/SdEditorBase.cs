using ExControls;
using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Zaklad editora vlastnosti: tabulka popis | ovladaci prvok, udalost <see cref="Changed" /> pri kazdej zmene.
/// </summary>
internal abstract class SdEditorBase : UserControl
{
    private readonly ToolTip _tips = new();

    /// <summary>Tabulka s riadkami editora.</summary>
    protected readonly TableLayoutPanel Table;

    /// <summary>Prebieha plnenie z modelu - zmeny sa nehlasia.</summary>
    protected bool Loading;

    /// <summary>Diagram upravovaneho prvku (pre prenos premenovaneho kluca do odkazov).</summary>
    private StateDgmDiagram? _diagram;

    /// <summary>Kluc, na ktory v diagrame ukazuju odkazy na prvok (z Bind, posuva sa po kazdom prenose).</summary>
    private string? _refKey;

    protected SdEditorBase()
    {
        AutoScroll = true;
        Table = new TableLayoutPanel
        {
            ColumnCount = 2,
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Padding = new Padding(6)
        };
        Table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        Controls.Add(Table);
    }

    /// <summary>Nastala zmena v modeli.</summary>
    public event EventHandler? Changed;

    /// <summary>Premenovanim prvku sa prepisali odkazy nan v inych prvkoch diagramu.</summary>
    public event EventHandler? ReferencesRenamed;

    /// <summary>Ohlasi zmenu (mimo plnenia).</summary>
    protected void RaiseChanged()
    {
        if (!Loading) Changed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Zapamata si diagram a kluc, na ktory ukazuju odkazy na naviazany prvok.</summary>
    protected void BindKey(StateDgmDiagram d, string key)
    {
        _diagram = d;
        _refKey = key;
    }

    /// <summary>
    /// Prenesie novy kluc prvku do odkazov (vola sa pri kazdej zmene kluca). Ked sa prenos odmietne - napr. medzikrok
    /// pisania sa zhoduje s klucom ineho prvku - odkazy ostanu na poslednom prenesenom kluci a presunu sa pri dalsej
    /// zmene; ak kluc ostane kolidujuci, duplicitu ohlasi kontrola diagramu.
    /// </summary>
    protected void RenameReferences(StateDgmElement element, string newKey)
    {
        if (_diagram == null || _refKey == null || !StateDgmRename.TryRename(_diagram, element, _refKey, out var changed)) return;
        _refKey = newKey;
        if (changed > 0) ReferencesRenamed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Koliesko mysi nad comboboxom / ciselnym polom posuva cely editor, nie hodnotu pola (hodnota sa meni len
    /// klavesnicou alebo klikom) - inak by sa pri rolovani panela nechtiac prepisovali hodnoty.
    /// </summary>
    private void HookWheel(Control c)
    {
        if (c is ComboBox or NumericUpDown)
            c.MouseWheel += (_, e) =>
            {
                if (c is ComboBox { DroppedDown: true }) return;
                if (e is HandledMouseEventArgs h) h.Handled = true;
                ScrollBy(-e.Delta);
            };
        foreach (Control child in c.Controls) HookWheel(child);
        c.ControlAdded += (_, e) =>
        {
            if (e.Control is { } added)
                HookWheel(added);
        };
    }

    /// <summary>Posunie obsah editora o dany pocet bodov.</summary>
    private void ScrollBy(int delta)
    {
        if (!VerticalScroll.Visible) return;
        var y = Math.Clamp(-AutoScrollPosition.Y + delta, VerticalScroll.Minimum, Math.Max(VerticalScroll.Minimum, VerticalScroll.Maximum - VerticalScroll.LargeChange + 1));
        AutoScrollPosition = new Point(-AutoScrollPosition.X, y);
    }

    /// <summary>Prida riadok popis + prvok; vrati popis.</summary>
    protected Label AddRow(string label, Control control, string? tip = null)
    {
        var l = new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 6, 6, 3) };
        var row = Table.RowCount++;
        Table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Table.Controls.Add(l, 0, row);
        control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        HookWheel(control);
        Table.Controls.Add(control, 1, row);
        if (tip != null)
        {
            _tips.SetToolTip(control, tip);
            _tips.SetToolTip(l, tip);
        }

        return l;
    }

    /// <summary>Prida prvok cez obe stlpce.</summary>
    protected void AddFull(Control control, int topMargin = 3)
    {
        var row = Table.RowCount++;
        Table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        control.Margin = new Padding(3, topMargin, 3, 3);
        control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        HookWheel(control);
        Table.Controls.Add(control, 0, row);
        Table.SetColumnSpan(control, 2);
    }

    /// <summary>Prida nadpis casti.</summary>
    protected Label AddHeader(string text)
    {
        var l = new Label { Text = text, AutoSize = true, Font = new Font(Font, FontStyle.Bold) };
        AddFull(l, 12);
        return l;
    }

    /// <summary>Prida informacny text (zalamovany).</summary>
    protected Label AddInfo(string text)
    {
        var l = new Label { Text = text, AutoSize = true, MaximumSize = new Size(360, 0), ForeColor = SystemColors.GrayText };
        AddFull(l);
        Table.SizeChanged += (_, _) => l.MaximumSize = new Size(Math.Max(120, Table.ClientSize.Width - 20), 0);
        return l;
    }

    /// <summary>Vytvori combo so zoznamom poloziek.</summary>
    protected static ExComboBox Combo(params SdEditorContext.Item[] items)
    {
        var cb = new ExComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220 };
        cb.Items.AddRange(items.Cast<object>().ToArray());
        return cb;
    }

    /// <summary>Vytvori textove pole.</summary>
    protected static ExTextBox TextField(string? hint = null) => new() { Width = 220, HintText = hint };

    /// <summary>Vytvori ciselne pole.</summary>
    protected static ExNumericUpDown Number(int min, int max, int step = 1) => new()
    {
        Minimum = min, Maximum = max, Increment = step, Width = 90, Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Right
    };

    /// <summary>Vytvori zaskrtavacie pole.</summary>
    protected static ExCheckBox Check(string text) => new() { Text = text, AutoSize = true, Anchor = AnchorStyles.Left };

    /// <summary>Zoznam prvkov riadka - zobrazi/skryje aj popis.</summary>
    protected void SetRowVisible(Control control, bool visible)
    {
        control.Visible = visible;
        var pos = Table.GetPositionFromControl(control);
        if (pos.Column == 1 && Table.GetControlFromPosition(0, pos.Row) is { } l) l.Visible = visible;
    }
}
