using GVDEditor.Forms;
using GVDEditor.Tools;
using ToolsCore;
using ToolsCore.XML;
using ToolsCore.Tools;

namespace GVDEditor;

internal static class Program
{
    public static FMain MainForm { get; private set; }

    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
        GlobSettings.LinkUpdater = "http://iniss.6f.sk/gvdeditor-updater/update.txt";
        AppInit.Initialization(out GlobData.Config, out GlobData.Styles, out GlobData.UsingStyle);

        if (args.Length == 1 && (args[0] == "-unlock" || args[0] == "/unlock")) 
            GlobData.PrivateFeatures = true;
        DateLimit.Loc = GlobData.Config.DateLimitLocate == AppLanguage.Czech ? DateLimit.Locale.CZ : DateLimit.Locale.SK;

        MainForm = new FMain();
        Application.Run(MainForm);

        Log.Info("Program sa ukončuje\r\n");
    }
}