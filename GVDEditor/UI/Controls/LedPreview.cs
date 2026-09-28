using System.Drawing.Drawing2D;
using System.Drawing.Text;
using GVDEditor.Domain.Entities;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Nahlad ukazkoveho textu na bodovej tabuli ELEN podla cisla pisma - farba, rez, blikanie a vysoke cislice.
/// Tabula ma 16 riadkov bodov; velkost bodu sa riadi vyskou prvku.
/// </summary>
public sealed class LedPreview : Control
{
    private const int Rows = 16;

    private static readonly Color Board = Color.FromArgb(10, 11, 12);
    private static readonly Color Off = Color.FromArgb(28, 31, 34);

    // farby 0 bez farby (predvolena farba tabule), 1 cervena, 2 zelena, 3 zlta
    private static readonly Color[] Lit =
        [Color.FromArgb(241, 236, 226), Color.FromArgb(255, 74, 61), Color.FromArgb(63, 224, 123), Color.FromArgb(255, 210, 58)];

    private readonly System.Windows.Forms.Timer _blink = new() { Interval = 550 };
    private ElenFontCode _code = new(ElenFontCode.DefaultKeptBits | 0x10);
    private bool _blinkOff;

    /// <summary>
    /// Vytvori nahlad.
    /// </summary>
    public LedPreview()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint |
                 ControlStyles.ResizeRedraw, true);
        _blink.Tick += (_, _) =>
        {
            _blinkOff = !_blinkOff;
            Invalidate();
        };
    }

    /// <summary>
    /// Farba svietiaceho bodu pre farbu pisma ELEN (0 bez farby, 1 cervena, 2 zelena, 3 zlta).
    /// </summary>
    public static Color LitColor(int color) => Lit[Math.Clamp(color, 0, Lit.Length - 1)];

    /// <summary>
    /// Zobrazovane cislo pisma.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int FontId
    {
        get => _code.Id;
        set
        {
            _code = new ElenFontCode(value);
            UpdateBlink();
            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        UpdateBlink();
    }

    private void UpdateBlink()
    {
        var blink = _code.Blinks && Visible && !DesignMode;
        _blink.Enabled = blink;
        if (!blink)
            _blinkOff = false;
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Board);

        var pitch = Math.Max(2, (ClientSize.Height - 4) / Rows);
        var cols = Math.Max(1, (ClientSize.Width - 4) / pitch);
        var left = (ClientSize.Width - cols * pitch) / 2;
        var top = (ClientSize.Height - Rows * pitch) / 2;

        using var bitmap = RenderText(cols);
        using var on = new SolidBrush(_blinkOff ? Off : Lit[_code.Color]);
        using var off = new SolidBrush(Off);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var size = pitch * 0.74f;
        var inset = (pitch - size) / 2;
        for (var y = 0; y < Rows; y++)
            for (var x = 0; x < cols; x++)
            {
                var lit = bitmap.GetPixel(x, y).R > 127;
                g.FillEllipse(lit ? on : off, left + x * pitch + inset, top + y * pitch + inset, size, size);
            }
    }

    /// <summary>
    /// Ukazkovy text vykresleny ciernobielo po bodoch tabule.
    /// </summary>
    private Bitmap RenderText(int cols)
    {
        var bitmap = new Bitmap(cols, Rows);
        using var g = Graphics.FromImage(bitmap);
        g.Clear(Color.Black);
        g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

        var (text, family, style, size) = _code.ExtendedFont > 0
            ? ("IC 511 Tatran", "Georgia", FontStyle.Bold, 12f)
            : _code.Face switch
            {
                1 => ("R 612 Košice", "Arial", FontStyle.Regular, 11f),
                2 => ("R 612 Košice", "Arial", FontStyle.Bold, 11f),
                3 => ("612  14:35", "Consolas", FontStyle.Regular, 12f),
                _ => ("R 612 Košice", "Consolas", FontStyle.Regular, 11f)
            };

        using var font = new Font(family, size, style, GraphicsUnit.Pixel);
        using var tall = new Font(family, size * 1.35f, style, GraphicsUnit.Pixel);
        using var format = StringFormat.GenericTypographic;
        format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

        var x = 1f;
        foreach (var ch in text)
        {
            // vysoke cislice nahradia medzeru a cislice znakmi cez celu vysku tabule
            var isTall = _code.TallDigits && (char.IsDigit(ch) || ch == ' ');
            var f = isTall ? tall : font;
            var s = ch.ToString();
            g.DrawString(s, f, Brushes.White, x, isTall ? -1 : 2, format);
            x += g.MeasureString(s, f, PointF.Empty, format).Width + (_code.Face == 0 || _code.Face == 3 ? 0 : 0.6f);
        }

        return bitmap;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _blink.Dispose();
        base.Dispose(disposing);
    }
}
