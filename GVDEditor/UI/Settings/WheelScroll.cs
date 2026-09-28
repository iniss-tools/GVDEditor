namespace GVDEditor.UI.Settings;

/// <summary>
///     Koliesko mysi v oknach nastaveni: posuva stranku, nie hodnotu pola, nad ktorym sa kurzor pri posuvani ocitne.
///     <list type="bullet">
///         <item>Rozbalovaci zoznam kolieskom hodnotu nemeni nikdy (rozbaleny zoznam sa posuva ako doteraz).</item>
///         <item>Ciselne pole meni hodnotu kolieskom, len ked ma fokus (pouzivatel don klikol).</item>
///         <item>Tabulka a zoznam sa posuvaju samy, kym mozu; na zaciatku alebo konci posuva koliesko stranku.</item>
///     </list>
///     Posuva sa najblizsi rodic s posuvnikom (panel s udajmi polozky).
/// </summary>
internal static class WheelScroll
{
    /// <summary>
    ///     Pripoji spravanie kolieska ku vsetkym prvkom v <paramref name="root" /> (okno so vsetkymi strankami).
    /// </summary>
    public static void Attach(Control root)
    {
        foreach (Control control in root.Controls)
        {
            switch (control)
            {
                case ComboBox or UpDownBase or DataGridView or ListBox:
                    control.MouseWheel += OnMouseWheel;
                    break;
            }

            Attach(control);
        }
    }

    private static void OnMouseWheel(object? sender, MouseEventArgs e)
    {
        if (sender is not Control control || e is not HandledMouseEventArgs handled || handled.Handled)
            return;

        var up = e.Delta > 0;
        var ownScroll = control switch
        {
            ComboBox combo => combo.DroppedDown,
            UpDownBase upDown => upDown.ContainsFocus,
            DataGridView grid => CanScroll(grid, up),
            ListBox list => CanScroll(list, up),
            _ => true
        };
        if (ownScroll)
            return;

        var panel = ScrollParent(control);
        // bez posuvaneho rodica sa neposuva nic, ale zoznam a pole aj tak hodnotu nezmenia
        if (panel is null && control is DataGridView or ListBox)
            return;

        handled.Handled = true;
        if (panel is not null)
            ScrollBy(panel, e.Delta);
    }

    private static bool CanScroll(DataGridView grid, bool up)
    {
        if (grid.RowCount == 0 || grid.FirstDisplayedScrollingRowIndex < 0)
            return false;

        var first = grid.FirstDisplayedScrollingRowIndex;
        if (up)
            return first > 0;

        return first + grid.DisplayedRowCount(false) < grid.Rows.GetRowCount(DataGridViewElementStates.Visible);
    }

    private static bool CanScroll(ListBox list, bool up)
    {
        if (list.Items.Count == 0 || list.ItemHeight <= 0)
            return false;

        if (up)
            return list.TopIndex > 0;

        var visible = Math.Max(1, list.ClientSize.Height / list.ItemHeight);
        return list.TopIndex + visible < list.Items.Count;
    }

    private static ScrollableControl? ScrollParent(Control control)
    {
        for (var parent = control.Parent; parent is not null; parent = parent.Parent)
            if (parent is ScrollableControl { AutoScroll: true, VerticalScroll.Visible: true } scrollable)
                return scrollable;
        return null;
    }

    private static void ScrollBy(ScrollableControl panel, int delta)
    {
        // rovnako ako koliesko nad prazdnym miestom panela: pocet riadkov podla nastavenia Windows
        var lines = SystemInformation.MouseWheelScrollLines;
        var pixels = lines < 0
            ? panel.ClientSize.Height * Math.Sign(delta)
            : delta * lines * panel.Font.Height / SystemInformation.MouseWheelScrollDelta;

        var position = panel.AutoScrollPosition;
        panel.AutoScrollPosition = new Point(-position.X, Math.Max(0, -position.Y - pixels));
    }
}
