using GVDEditor.Config;
using GVDEditor.Domain.Calendar;
using GVDEditor.Integration;
using GVDEditor.UI.Main;
using ToolsCore.Tools;
using ToolsCore.XML;
using ToolsCore;

namespace GVDEditor;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // composition root (skladanie bez kontajnera): nastavenia programu, kontext editora a sluzby hlavneho okna;
        // okna a stranky ich dostavaju explicitne - ziadny staticky pristup k datam
        var context = new EditorContext(AppInit.Initialization<GVDEditorConfig, GVDEditorStyle>());
        var dialogs = new DialogService();
        using var iniss = new InissProcessService();

        // jazyk datumovych obmedzeni je okolite nastavenie (ako kultura) - meni sa len tu a po zmene nastaveni
        DateLimit.Loc = context.Config.DateLimitLocate == AppLanguage.Czech ? DateLimit.Locale.Cz : DateLimit.Locale.Sk;

        AppInit.Run(context.Config, () => new FMain(context, iniss, dialogs));
    }
}
