using System.Globalization;

namespace GVDEditor.Controls;

/// <summary>
///     Udalost kliknutia na stlpec pravitka - poradie stlpca v tabuli.
/// </summary>
public sealed class CatalogRulerEventArgs(int column) : EventArgs
{
    /// <summary>
    ///     Poradie stlpca v katalogovej tabuli.
    /// </summary>
    public int Column { get; } = column;
}

/// <summary>
///     Pravitko katalogovej tabule: stlpce ako obdlzniky podla bodov (START–END) na svojich riadkoch. Stlpce, ktore sa
///     na riadku prekryvaju (napr. alternativy pre rozne rezimy), idu pod seba. Vybrany stlpec je zvyrazneny, chybny
///     cerveny; hranica tabule (napr. 512 bodov pri ELEN) je zvisla ciara.
/// </summary>
public sealed class CatalogRuler : Control
{
    private const int Gutter = 26;
    private const int ScaleHeight = 16;

    private readonly ToolTip _tip = new();
    private List<Column> _columns = [];
    private readonly List<(int Column, Rectangle Bounds)> _hits = [];
    private int _selected = -1;
    private int? _limit;
    private int _lanesHeight;
    private int _tipColumn = -1;

    /// <summary>
    ///     Stlpec tabule tak, ako ho pravitko kresli.
    /// </summary>
    public sealed record Column(string Name, int Line, int Start, int End, bool Problem);

    /// <summary>
    ///     Vytvori pravitko.
    /// </summary>
    public CatalogRuler()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint |
                 ControlStyles.ResizeRedraw, true);
        Cursor = Cursors.Hand;
    }

    /// <summary>
    ///     Pouzivatel klikol na stlpec.
    /// </summary>
    public event EventHandler<CatalogRulerEventArgs>? ColumnClicked;

    /// <summary>
    ///     Nastavi stlpce, vybrany stlpec a hranicu tabule v bodoch (<see langword="null" /> = bez hranice).
    /// </summary>
    public void SetColumns(IReadOnlyList<Column> columns, int selected, int? limit)
    {
        _columns = columns.ToList();
        _selected = selected;
        _limit = limit;
        Height = PreferredHeight();
        Invalidate();
    }

    /// <summary>
    ///     Zmeni len vybrany stlpec.
    /// </summary>
    public void Select(int selected)
    {
        _selected = selected;
        Invalidate();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _tip.Dispose();
        base.Dispose(disposing);
    }

    private int LaneHeight => Font.Height + 6;

    private int Lines => _columns.Count == 0 ? 1 : _columns.Max(c => Math.Max(0, c.Line)) + 1;

    private int PreferredHeight()
    {
        var lanes = 0;
        for (var line = 0; line < Lines; line++)
            lanes += Math.Max(1, Lanes(line).Count);
        return ScaleHeight + lanes * LaneHeight + 4;
    }

    // stlpce riadku rozdelene do pasov tak, aby sa v pase neprekryvali
    private List<List<int>> Lanes(int line)
    {
        var lanes = new List<List<int>>();
        var indexes = _columns.Select((column, i) => (column, i)).Where(c => c.column.Line == line)
            .OrderBy(c => c.column.Start).ThenBy(c => c.column.End).Select(c => c.i);
        foreach (var i in indexes)
        {
            var lane = lanes.FirstOrDefault(l => l.All(j => _columns[j].End <= _columns[i].Start || _columns[i].End <= _columns[j].Start));
            if (lane is null)
                lanes.Add(lane = []);
            lane.Add(i);
        }

        return lanes;
    }

    private int Span()
    {
        var max = Math.Max(_limit ?? 0, _columns.Count == 0 ? 0 : _columns.Max(c => c.End));
        max = Math.Max(max, 64);
        return (max + 31) / 32 * 32;
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        var back = Parent?.BackColor ?? BackColor;
        g.Clear(back);
        _hits.Clear();

        var fore = ForeColor;
        var grid = Blend(fore, back, 0.25);
        var span = Span();
        var width = Math.Max(1, ClientSize.Width - Gutter - 2);
        int X(int point) => Gutter + (int)Math.Round((double)point * width / span);

        // mierka po 64 bodoch
        using var gridPen = new Pen(grid);
        var step = span > 640 ? 128 : 64;
        for (var p = 0; p <= span; p += step)
        {
            var x = X(p);
            g.DrawLine(gridPen, x, ScaleHeight - 4, x, Height - 2);
            TextRenderer.DrawText(g, p.ToString(CultureInfo.InvariantCulture), Font, new Point(x + 2, 0), grid, TextFormatFlags.NoPadding);
        }

        var y = ScaleHeight;
        var laneHeight = LaneHeight;
        for (var line = 0; line < Lines; line++)
        {
            var lanes = Lanes(line);
            var lineHeight = Math.Max(1, lanes.Count) * laneHeight;
            TextRenderer.DrawText(g, (line + 1).ToString(CultureInfo.InvariantCulture), Font,
                new Rectangle(0, y, Gutter - 4, laneHeight), grid, TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
            g.DrawLine(gridPen, Gutter, y + lineHeight, ClientSize.Width - 1, y + lineHeight);

            for (var l = 0; l < lanes.Count; l++)
                foreach (var i in lanes[l])
                {
                    var c = _columns[i];
                    var rect = new Rectangle(X(c.Start), y + l * laneHeight + 2, Math.Max(3, X(c.End) - X(c.Start) - 1), laneHeight - 4);
                    var selected = i == _selected;
                    var fill = c.Problem ? Color.FromArgb(200, 60, 60)
                        : selected ? SystemColors.Highlight
                        : Blend(SystemColors.Highlight, back, 0.35);
                    var text = c.Problem || selected ? Color.White : fore;
                    using (var brush = new SolidBrush(fill))
                        g.FillRectangle(brush, rect);
                    using (var pen = new Pen(selected ? fore : Blend(SystemColors.Highlight, back, 0.8)))
                        g.DrawRectangle(pen, rect);
                    TextRenderer.DrawText(g, c.Name, Font, rect, text,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis |
                        TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
                    _hits.Add((i, rect));
                }

            y += lineHeight;
        }

        _lanesHeight = y;

        // hranica tabule
        if (_limit is { } limit)
        {
            using var pen = new Pen(Color.FromArgb(200, 60, 60), 2);
            g.DrawLine(pen, X(limit), ScaleHeight - 4, X(limit), _lanesHeight);
        }
    }

    private static Color Blend(Color a, Color b, double amount) =>
        Color.FromArgb((int)(a.R * amount + b.R * (1 - amount)), (int)(a.G * amount + b.G * (1 - amount)),
            (int)(a.B * amount + b.B * (1 - amount)));

    private int HitTest(Point point)
    {
        // pri prekryve vyhra posledny nakresleny (spodnejsi pas)
        for (var i = _hits.Count - 1; i >= 0; i--)
            if (_hits[i].Bounds.Contains(point))
                return _hits[i].Column;
        return -1;
    }

    /// <inheritdoc />
    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        var column = HitTest(e.Location);
        if (column >= 0)
            ColumnClicked?.Invoke(this, new CatalogRulerEventArgs(column));
    }

    /// <inheritdoc />
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        var column = HitTest(e.Location);
        if (column == _tipColumn)
            return;

        _tipColumn = column;
        if (column < 0)
        {
            _tip.Hide(this);
            return;
        }

        var c = _columns[column];
        _tip.Show($"{c.Name}: {c.Start} – {c.End}", this, e.X + 12, e.Y + 16);
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _tipColumn = -1;
        _tip.Hide(this);
    }
}
