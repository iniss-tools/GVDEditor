using System.Globalization;
using GVDEditor.Tools;

namespace GVDEditor.Controls;

/// <summary>
///     Pruh kalendara variant vlaku: pre kazdu variantu riadok dni obdobia, dni jazdy farebne a dni, v ktore ide
///     viac variant naraz, cervene. Nad riadkami su mesiace. Bublina ukaze datum a varianty, ktore v den idu.
/// </summary>
internal sealed class VariantCalendarStrip : Control
{
    private const int HeaderHeight = 18;
    private const int RowHeight = 16;
    private const int LabelWidth = 44;

    private readonly ToolTip _toolTip = new();
    private VariantCalendar? _calendar;
    private int _hoverDay = -1;

    /// <summary>
    ///     Vytvori prazdny pruh.
    /// </summary>
    public VariantCalendarStrip()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                 ControlStyles.UserPaint, true);
        Height = HeaderHeight + RowHeight + 2;
    }

    /// <summary>
    ///     Text popisu riadku (napr. „1/3“) podla riadku kalendara.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Func<VariantCalendar.Row, string> RowLabel { get; set; } = row => $"{row.Position}";

    /// <summary>
    ///     Text bubliny pre den: datum a popisy variant, ktore v nom idu.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Func<DateTime, IReadOnlyList<VariantCalendar.Row>, string> DayToolTip { get; set; } =
        (date, _) => date.ToString("d", CultureInfo.CurrentCulture);

    /// <summary>
    ///     Zobrazi kalendar a prisposobi vysku poctu variant.
    /// </summary>
    public void SetCalendar(VariantCalendar? calendar)
    {
        _calendar = calendar;
        Height = HeaderHeight + RowHeight * Math.Max(1, calendar?.Rows.Count ?? 1) + 2;
        Invalidate();
    }

    private bool Dark => BackColor.GetBrightness() < 0.5f;

    private RectangleF DaysArea => new(LabelWidth, HeaderHeight, Math.Max(1, Width - LabelWidth - 2), Height - HeaderHeight - 2);

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(BackColor);
        if (_calendar is not { Days: > 0 } calendar)
            return;

        var area = DaysArea;
        var dayWidth = area.Width / calendar.Days;
        var grid = Dark ? Color.FromArgb(70, 70, 70) : Color.FromArgb(215, 215, 215);
        var empty = Dark ? Color.FromArgb(45, 45, 45) : Color.FromArgb(242, 242, 242);
        var own = Dark ? Color.FromArgb(70, 150, 230) : Color.FromArgb(0, 110, 200);
        var other = Dark ? Color.FromArgb(120, 140, 160) : Color.FromArgb(130, 150, 175);
        var overlap = Dark ? Color.FromArgb(240, 90, 90) : Color.FromArgb(200, 30, 45);

        using var gridPen = new Pen(grid);
        using var textBrush = new SolidBrush(ForeColor);
        using var emptyBrush = new SolidBrush(empty);
        using var ownBrush = new SolidBrush(own);
        using var otherBrush = new SolidBrush(other);
        using var overlapBrush = new SolidBrush(overlap);
        var format = new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.None };

        // mesiace
        for (var day = 0; day < calendar.Days; day++)
        {
            var date = calendar.From.AddDays(day);
            if (day != 0 && date.Day != 1)
                continue;

            var x = area.Left + day * dayWidth;
            g.DrawLine(gridPen, x, 0, x, Height);
            var monthName = date.ToString(dayWidth * 31 >= 40 ? "MMM" : "%M", CultureInfo.CurrentCulture);
            g.DrawString(monthName, Font, textBrush, new RectangleF(x + 2, 0, dayWidth * 31, HeaderHeight), format);
        }

        for (var r = 0; r < calendar.Rows.Count; r++)
        {
            var row = calendar.Rows[r];
            var top = area.Top + r * RowHeight;
            using var labelFont = row.Train == null ? new Font(Font, FontStyle.Bold) : null;
            g.DrawString(RowLabel(row), labelFont ?? Font, textBrush, new RectangleF(2, top, LabelWidth - 4, RowHeight), format);
            g.FillRectangle(emptyBrush, area.Left, top + 2, area.Width, RowHeight - 4);

            for (var day = 0; day < calendar.Days; day++)
            {
                if (!row.Runs[day])
                    continue;

                var brush = calendar.IsOverlap(day) ? overlapBrush : row.Train == null ? ownBrush : otherBrush;
                g.FillRectangle(brush, area.Left + day * dayWidth, top + 2, Math.Max(1f, dayWidth), RowHeight - 4);
            }
        }

        if (_hoverDay >= 0 && _hoverDay < calendar.Days)
        {
            var x = area.Left + _hoverDay * dayWidth;
            g.DrawRectangle(Pens.Gray, x, area.Top, Math.Max(1f, dayWidth), area.Height - 1);
        }
    }

    /// <inheritdoc />
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (_calendar is not { Days: > 0 } calendar)
            return;

        var area = DaysArea;
        var day = e.X < area.Left ? -1 : (int)((e.X - area.Left) / (area.Width / calendar.Days));
        if (day >= calendar.Days)
            day = -1;
        if (day == _hoverDay)
            return;

        _hoverDay = day;
        Invalidate();
        if (day < 0)
        {
            _toolTip.SetToolTip(this, null);
            return;
        }

        var running = calendar.Rows.Where(row => row.Runs[day]).ToList();
        _toolTip.SetToolTip(this, DayToolTip(calendar.From.AddDays(day), running));
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hoverDay = -1;
        _toolTip.SetToolTip(this, null);
        Invalidate();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _toolTip.Dispose();
        base.Dispose(disposing);
    }
}
