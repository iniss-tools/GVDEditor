using GVDEditor.Config;
using ToolsCore.XML;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Nahlad farieb druhov vlakov na stranke farebnej schemy: riadky tabulky vlakov s druhom a cislom vlaku.
/// Klik na riadok vyberie druh.
/// </summary>
internal sealed class TrainTypeColorPreview : Control, IColorPreview
{
    // druh vlaku a cislo ako v tabulke vlakov (data INISS, nie text rozhrania)
    private static readonly (string Type, string Number)[] Rows = [("Os", "3305"), ("R", "605"), ("X", "1871"), ("Sl", "99")];

    private readonly List<(Rectangle Bounds, string Id)> _areas = [];
    private GVDEditorStyle? _style;
    private Font? _bold;

    public TrainTypeColorPreview()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
    }

    /// <inheritdoc />
    public Control Control => this;

    /// <inheritdoc />
    public event EventHandler<string>? KeyClicked;

    /// <inheritdoc />
    public void ShowScheme(Style style)
    {
        _style = style as GVDEditorStyle;
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        _areas.Clear();
        if (_style is null)
            return;

        var box = _style.ControlsColorScheme.Box;
        var g = e.Graphics;
        using (var back = new SolidBrush(box.BackColor))
            g.FillRectangle(back, ClientRectangle);

        var row = Font.Height + LogicalToDeviceUnits(8);
        var typeWidth = LogicalToDeviceUnits(60);
        var schemes = _style.TrainTypeColumnScheme;
        ColorSetting[] settings = [schemes.Os, schemes.R, schemes.X, schemes.Sl];
        using var grid = new Pen(_style.ControlsColorScheme.Border.ForeColor);
        for (var i = 0; i < Rows.Length; i++)
        {
            var bounds = new Rectangle(0, i * row, Width, row);
            var type = new Rectangle(0, bounds.Y, typeWidth, row);
            var setting = settings[i];
            if (setting.BackColor.A > 0)
            {
                using var back = new SolidBrush(setting.BackColor);
                g.FillRectangle(back, type);
            }

            var font = setting.Bold ? _bold ??= new Font(Font, FontStyle.Bold) : Font;
            const TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
            TextRenderer.DrawText(g, Rows[i].Type, font, type, setting.ForeColor, flags);
            TextRenderer.DrawText(g, Rows[i].Number, Font, new Rectangle(typeWidth, bounds.Y, typeWidth, row), box.ForeColor, flags);
            g.DrawLine(grid, 0, bounds.Bottom - 1, Width, bounds.Bottom - 1);
            _areas.Add((bounds, "TrainType." + Rows[i].Type));
        }

        g.DrawLine(grid, typeWidth, 0, typeWidth, Rows.Length * row);
    }

    /// <inheritdoc />
    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        var area = _areas.FirstOrDefault(a => a.Bounds.Contains(e.Location));
        if (area.Id is not null)
            KeyClicked?.Invoke(this, area.Id);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _bold?.Dispose();
        base.Dispose(disposing);
    }
}
