using GVDEditor.Config;
using GVDEditor.Domain.Calendar;
using GVDEditor.UI.Main;
using ToolsCore;
using ToolsCore.XML;

namespace GVDEditor;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // composition root - kontext editora dostavaju okna explicitne
        var context = new EditorContext(AppInit.Initialization<GVDEditorConfig, GVDEditorStyle>());

        DateLimit.Loc = context.Config.DateLimitLocate == AppLanguage.Czech ? DateLimit.Locale.Cz : DateLimit.Locale.Sk;

        AppInit.Run(context.Config, () => new FMain(context));
    }
}
