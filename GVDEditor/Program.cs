using GVDEditor.Config;
using GVDEditor.Domain.Calendar;
using GVDEditor.UI.Main;
using ToolsCore;
using ToolsCore.XML;

namespace GVDEditor;

internal static class Program
{
    public static FMain MainForm { get; private set; } = null!;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        GlobData.Session = AppInit.Initialization<GVDEditorConfig, GVDEditorStyle>();

        DateLimit.Loc = GlobData.Config.DateLimitLocate == AppLanguage.Czech ? DateLimit.Locale.Cz : DateLimit.Locale.Sk;

        AppInit.Run(GlobData.Config, () => MainForm = new FMain());
    }
}
