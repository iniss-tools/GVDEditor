using System.Globalization;
using System.Reflection;
using GVDEditor.Forms;
using GVDEditor.Tools;
using GVDEditor.XML;
using ToolsCore;
using ToolsCore.Tools;

namespace GVDEditor.DocScreenshots;

/// <summary>
///     Generátor snímok okien GVDEditora do dokumentácie.
/// </summary>
/// <remarks>
///     Použitie: <c>GVDEditor.DocScreenshots [--out priečinok] [--work priečinok] [--only text] [--theme light|dark|both]</c>.
///     <list type="bullet">
///         <item><c>--out</c> – kam uložiť PNG; predvolene <c>iniss-tools-docs\static\img\gvdeditor</c> vedľa repozitára.</item>
///         <item><c>--work</c> – kde zostaviť ukážkovú inštaláciu INISS; predvolene <c>C:\INISS</c> (cesta je vidno
///         v titulku a nastaveniach). Existujúci priečinok bez značky <c>.docshots</c> sa nezmaže.</item>
///         <item><c>--timeout</c> – po koľkých minútach sa harness ukončí, ak ho zablokuje modálne okno (predvolene 5).</item>
///         <item><c>--only</c> – len snímky, ktorých cesta obsahuje daný text (napr. <c>uprava-vlaku</c>).</item>
///     </list>
///     Program beží pod vlastným menom, takže konfiguráciu (<c>%LocalAppData%\GVDEditor.DocScreenshots</c>)
///     aj register má oddelené od GVDEditora – pri každom spustení začína s predvolenými nastaveniami.
/// </remarks>
internal static class Program
{
    private const BindingFlags Any = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

    [STAThread]
    private static int Main(string[] args)
    {
        var options = Options.Parse(args);
        var log = new List<string>();

        // poistka: modálne okno (chyba, otázka) by harness zablokovalo navždy
        using var watchdog = new System.Threading.Timer(_ =>
        {
            Console.Error.WriteLine("Časový limit vypršal – pravdepodobne visí modálne okno. Posledné kroky:");
            foreach (var line in log.TakeLast(5)) Console.Error.WriteLine("  " + line);
            Console.Error.WriteLine("Otvorené okná: " + string.Join(" | ", WindowCapture.ProcessWindowTitles()));
            Environment.Exit(2);
        }, null, TimeSpan.FromMinutes(options.TimeoutMinutes), Timeout.InfiniteTimeSpan);

        try
        {
            InitApp();
            var gvdPath = DemoInstallation.Build(options.WorkDir, log);
            log.Add($"inštalácia: {options.WorkDir}");
            using (var screen = Graphics.FromHwnd(IntPtr.Zero))
                log.Add($"DPI: {screen.DpiX} (snímky majú rozmery podľa škálovania obrazovky, pre docs 96 = 100 %)");

            var shots = 0;
            foreach (var theme in options.Themes)
            {
                SetTheme(theme);
                shots += new Shots(options, theme, log).Run(gvdPath);
            }

            log.Add($"hotovo: {shots} snímok do {options.OutDir}");
            Flush(log);
            return 0;
        }
        catch (Exception e)
        {
            log.Add("CHYBA: " + (e is TargetInvocationException { InnerException: { } inner } ? inner : e));
            Flush(log);
            return 1;
        }
    }

    private static void Flush(List<string> log)
    {
        foreach (var line in log) Console.WriteLine(line);
    }

    /// <summary>
    ///     Rovnaká inicializácia ako GVDEditor.Program.Main, s čistou konfiguráciou a slovenčinou.
    /// </summary>
    private static void InitApp()
    {
        if (Directory.Exists(AppPaths.DataDir))
            Directory.Delete(AppPaths.DataDir, true);

        AppInit.Initialization(out GlobData.Config, out GlobData.Styles, out GlobData.UsingStyle);

        var culture = CultureInfo.CreateSpecificCulture("sk");
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        DateLimit.Loc = DateLimit.Locale.Sk;
    }

    private static void SetTheme(string theme)
    {
        var style = theme == "dark" ? GVDEditorStyle.DefaultDarkStyle : GVDEditorStyle.DefaultLightStyle;
        GlobData.UsingStyle = style;
        GlobSettings.UsingStyle = style;
        AppInit.MsgBoxStyleInit(style, GlobData.Config);
    }

    /// <summary>
    ///     Otvorí hlavné okno s ukážkovou inštaláciou rovnako ako Súbor → Nedávne.
    /// </summary>
    public static FMain OpenMain(string installDir)
    {
        var main = new FMain();
        typeof(GVDEditor.Program).GetProperty(nameof(GVDEditor.Program.MainForm), Any)!.SetValue(null, main);

        // FMain_Load by argumenty harnessu (--out …) bral ako cestu k projektu a registroval jump list
        main.Load -= (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), main, "FMain_Load");
        main.StartPosition = FormStartPosition.Manual;
        main.Location = new Point(40, 40);
        main.Show();
        Pump.Events();

        typeof(FMain).GetMethod("OpenRecentProject", Any)!.Invoke(main, [installDir]);
        if (!Pump.Until(() => GlobData.Trains.Count > 0 && Application.OpenForms.OfType<FWait>().All(f => !f.Visible)))
            throw new TimeoutException("Grafikon sa nenačítal.");

        return main;
    }

    internal sealed record Options(string OutDir, string WorkDir, string? Only, string[] Themes, int TimeoutMinutes)
    {
        public static Options Parse(string[] args)
        {
            string? Arg(string name) =>
                args.SkipWhile(a => a != name).Skip(1).FirstOrDefault();

            var theme = Arg("--theme") ?? "both";
            return new Options(
                Arg("--out") ?? DefaultOutDir(),
                Arg("--work") ?? @"C:\INISS",
                Arg("--only"),
                theme == "both" ? ["light", "dark"] : [theme],
                int.Parse(Arg("--timeout") ?? "5", CultureInfo.InvariantCulture));
        }

        private static string DefaultOutDir()
        {
            // hľadá iniss-tools-docs v niektorom nadradenom priečinku (D:\INISSTools\iniss-tools-docs)
            for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
            {
                var docs = Path.Combine(dir.FullName, "iniss-tools-docs");
                if (Directory.Exists(docs))
                    return Path.Combine(docs, "static", "img", "gvdeditor");
            }

            throw new DirectoryNotFoundException("Nenašiel sa priečinok iniss-tools-docs – zadaj --out.");
        }
    }
}
