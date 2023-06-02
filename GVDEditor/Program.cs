using GVDEditor.Forms;
using GVDEditor.Tools;
using ToolsCore;
using ToolsCore.XML;
using ToolsCore.Tools;
using ToolsCore.Forms;

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
        DateLimit.Loc = GlobData.Config.DateLimitLocate == AppLanguage.Czech ? DateLimit.Locale.Cz : DateLimit.Locale.Sk;

        MainForm = new FMain();

        if (GlobData.Config.DebugModeGUI != DebugMode.AppCrash)
        {
            try
            {
                Application.Run(MainForm);
            }
            catch (Exception exception)
            {
                Log.Exception(exception);
                FError.ShowError(GlobData.Config.DebugModeGUI == DebugMode.OnlyMessage ? exception.Message : exception.ToString());
            }
        }
        else
        {
            Application.Run(MainForm);
        }

        Log.Info("Program sa ukončuje\r\n");
    }
}