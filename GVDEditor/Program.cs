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
        AppInit.Initialization(out GlobData.Config, out GlobData.Styles, out GlobData.UsingStyle);

        DateLimit.Loc = GlobData.Config.DateLimitLocate == AppLanguage.Czech ? DateLimit.Locale.Cz : DateLimit.Locale.Sk;

        AppInit.Run(GlobData.Config, () => MainForm = new FMain());
    }
}
