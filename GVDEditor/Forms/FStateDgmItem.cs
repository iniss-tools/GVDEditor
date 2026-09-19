using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog s editorom prvku stavoveho diagramu (akcia, starter) a tlacidlami OK / Zrusit.
/// </summary>
public partial class FStateDgmItem : Form
{
    /// <summary>
    ///     Vytvori dialog s editorom.
    /// </summary>
    /// <param name="title">Titulok okna.</param>
    /// <param name="editor">Editor, ktory sa vlozi do dialogu (vyplni celu plochu nad tlacidlami).</param>
    /// <param name="width">Sirka klientskej casti.</param>
    /// <param name="height">Vyska klientskej casti.</param>
    internal FStateDgmItem(string title, Control editor, int width, int height)
    {
        InitializeComponent();
        Text = title;
        ClientSize = new Size(width, height);

        editor.Dock = DockStyle.Fill;
        pnlEditor.Controls.Add(editor);

        this.ApplyThemeAndFonts();
        editor.Font = Font;
    }
}
