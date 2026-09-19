using ExControls;
using ToolsCore;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog s editorom prvku diagramu (akcia, starter) a tlacidlami OK / Zrusit.
/// </summary>
internal sealed class FStateDgmItem : Form
{
    public FStateDgmItem(string title, Control editor, int width, int height)
    {
        Text = title;
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition = FormStartPosition.CenterParent;
        MinimizeBox = false;
        MaximizeBox = false;
        ShowIcon = false;
        ShowInTaskbar = false;
        ClientSize = new Size(width, height);
        MinimumSize = new Size(400, 300);

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(6) };
        var ok = new ExButton { Text = GlobalResources.Global_OK, DialogResult = DialogResult.OK, Width = 90 };
        var cancel = new ExButton { Text = GlobalResources.Global_Cancel, DialogResult = DialogResult.Cancel, Width = 90 };
        buttons.Controls.Add(cancel);
        buttons.Controls.Add(ok);
        editor.Dock = DockStyle.Fill;
        Controls.Add(editor);
        Controls.Add(buttons);
        AcceptButton = ok;
        CancelButton = cancel;
        this.ApplyThemeAndFonts();
        editor.Font = Font;
    }
}