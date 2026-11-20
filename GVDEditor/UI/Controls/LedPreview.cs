using System.Drawing.Text;
using ToolsCore.Forms.Elen;
using ToolsCore.Iniss.Elen;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Nahlad ukazkoveho textu na bodovej tabuli ELEN podla cisla pisma - farba, rez, blikanie a vysoke cislice.
/// So suborom pisiem ELEN (<see cref="Fonts" />) kresli text pismom, ktore cislo vyberie, presne ako panel; bez neho
/// alebo ked pismo v subore nie je, priblizne pismom Windows. Tabula ma 16 riadkov bodov.
/// </summary>
public sealed class LedPreview : ElenBoardView
{
    private const int Rows = 16;

    private ElenFontCode _code = new(ElenFontCode.DefaultKeptBits | 0x10);
    private ElenFontFile? _fonts;
    private Encoding _encoding = Encodings.Win1250;

    /// <summary>
    /// Vytvori nahlad.
    /// </summary>
    public LedPreview()
    {
        BoardRows = Rows;
        UpdateRaster();
    }

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
            UpdateRaster();
        }
    }

    /// <summary>
    /// Subor pisiem ELEN, ktorym sa text kresli; null = pismo Windows.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ElenFontFile? Fonts
    {
        get => _fonts;
        set
        {
            _fonts = value;
            UpdateRaster();
        }
    }

    /// <summary>
    /// Kodovanie textu tabule (kod Kamenickych alebo Windows-1250 podla vyrobcu).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Encoding TextEncoding
    {
        get => _encoding;
        set
        {
            _encoding = value;
            UpdateRaster();
        }
    }

    /// <summary>
    /// Ci sa text kresli pismom zo suboru (inak pismom Windows).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool UsesFontFile => _fonts is not null && ElenText.FontFor(_fonts, _code) is not null;

    /// <summary>Ukazkovy text podla rezu pisma.</summary>
    private string SampleText => _code.ExtendedFont > 0 ? "IC 511 Tatran" : _code.Face == 3 ? "612  14:35" : "R 612 Košice";

    private void UpdateRaster()
    {
        DotColor = _code.Color;
        Blinks = _code.Blinks;
        Raster = _fonts is not null && ElenText.FontFor(_fonts, _code) is { } font
            ? ElenText.Render(font, _encoding.GetBytes(SampleText), _code.TallDigits)
            : RenderWindowsText();
    }

    /// <summary>
    /// Ukazkovy text vykresleny ciernobielo pismom Windows po bodoch tabule (bez suboru pisiem).
    /// </summary>
    private ElenTextRaster RenderWindowsText()
    {
        const int width = 160;
        using var bitmap = new Bitmap(width, Rows);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.Black);
            g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

            var (family, style, size) = _code.ExtendedFont > 0
                ? ("Georgia", FontStyle.Bold, 12f)
                : _code.Face switch
                {
                    1 => ("Arial", FontStyle.Regular, 11f),
                    2 => ("Arial", FontStyle.Bold, 11f),
                    3 => ("Consolas", FontStyle.Regular, 12f),
                    _ => ("Consolas", FontStyle.Regular, 11f)
                };

            using var font = new Font(family, size, style, GraphicsUnit.Pixel);
            using var tall = new Font(family, size * 1.35f, style, GraphicsUnit.Pixel);
            using var format = StringFormat.GenericTypographic;
            format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            var x = 0f;
            foreach (var ch in SampleText)
            {
                // vysoke cislice nahradia medzeru a cislice znakmi cez celu vysku tabule
                var isTall = _code.TallDigits && (char.IsDigit(ch) || ch == ' ');
                var f = isTall ? tall : font;
                var s = ch.ToString();
                g.DrawString(s, f, Brushes.White, x, isTall ? -1 : 2, format);
                x += g.MeasureString(s, f, PointF.Empty, format).Width + (_code.Face is 0 or 3 ? 0 : 0.6f);
            }
        }

        var bits = new bool[width * Rows];
        for (var y = 0; y < Rows; y++)
        {
            for (var x = 0; x < width; x++)
                bits[y * width + x] = bitmap.GetPixel(x, y).R > 127;
        }

        return new ElenTextRaster(width, Rows, bits, []);
    }
}
