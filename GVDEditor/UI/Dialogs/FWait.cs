using ToolsCore.Tools;

namespace GVDEditor.UI.Dialogs;

/// <summary>
/// Okno oznamujuce pouzivatelovi nacitavanie dat.
/// </summary>
internal partial class FWait : Form
{
    /// <summary>
    /// Kontext editora - nastavenia programu, instalacia INISS a otvoreny grafikon.
    /// </summary>
    private readonly EditorContext _ctx;

    /// <summary>
    /// Vytvori novy formular typu <see cref="FWait"/>.
    /// </summary>
    public FWait(EditorContext context, string? text = null)
    {
        _ctx = context;
        InitializeComponent();
        this.ApplyThemeAndFonts();

        if (text != null) 
            lText.Text = text;
    }

    private void FWait_Paint(object sender, PaintEventArgs e)
    {
        using var pen = new Pen(_ctx.UsingStyle.ControlsColorScheme.Highlight.BackColor);
        var rec = e.ClipRectangle;
        rec.Inflate(-1,-1);
        e.Graphics.DrawRectangle(pen, rec);
    }
}