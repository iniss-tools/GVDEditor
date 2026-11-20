using GVDEditor.Config;
using GVDEditor.UI.Settings;
using Serilog;
using ToolsCore.Iniss.Elen;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Subor pisiem ELEN pre nahlad pisma tabule (<see cref="LedPreview" />). Cesta je v nastaveniach programu; subor sa
/// nacita raz a znova az po zmene na disku. INISS 3.39 subor nepotrebuje - pisma ma panel, preto ho vyberie obsluha.
/// </summary>
/// <param name="config">nastavenia programu</param>
/// <param name="fontsDirectory">priecinok, kde dialog zacne (DATA\FONTS instalacie); null = posledny pouzity</param>
/// <param name="save">zapise nastavenia programu</param>
internal sealed class ElenPreviewFonts(GVDEditorConfig config, string? fontsDirectory, Action save)
{
    // priecinok bank pisiem instalacie INISS 3.39 (ako v exporte do INISS2)
    private const string FontsFolder = "FONTS";

    private string? _loadedPath;
    private DateTime _loadedStamp;
    private ElenFontFile? _file;

    /// <summary>
    /// Subor pisiem z nastaveni programu; dialog vyberu zacne v DATA\FONTS zvolenej instalacie.
    /// </summary>
    public static ElenPreviewFonts For(EditorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var dataDir = context.Workspace.DataDir;
        var fonts = string.IsNullOrEmpty(dataDir) ? null : System.IO.Path.Combine(dataDir, FontsFolder);
        return new ElenPreviewFonts(context.Config, fonts is not null && Directory.Exists(fonts) ? fonts : null,
            () => SettingsWindow.SaveConfig(context.Config));
    }

    /// <summary>Cesta k suboru .bin; prazdna = nahlad pismom Windows.</summary>
    public string Path => config.ElenPreviewFonts;

    /// <summary>Priecinok, v ktorom zacne dialog vyberu suboru.</summary>
    public string? FontsDirectory => fontsDirectory;

    /// <summary>Dovod, preco sa subor nedal nacitat; null = nacitany alebo ziadny.</summary>
    public string? Error { get; private set; }

    /// <summary>
    /// Nacitany subor pisiem; null = ziadny alebo sa neda nacitat (dovod v <see cref="Error" />).
    /// </summary>
    public ElenFontFile? File
    {
        get
        {
            var path = Path;
            if (path.Length == 0)
            {
                Error = null;
                return null;
            }

            var stamp = System.IO.File.Exists(path) ? System.IO.File.GetLastWriteTimeUtc(path) : DateTime.MinValue;
            if (_loadedPath == path && _loadedStamp == stamp)
                return _file;

            _loadedPath = path;
            _loadedStamp = stamp;
            try
            {
                _file = ElenFontFile.Load(path);
                Error = null;
            }
            catch (Exception e) when (e is IOException or UnauthorizedAccessException or FormatException)
            {
                Log.Warning(e, "Subor pisiem ELEN {Path} pre nahlad sa nepodarilo nacitat", path);
                _file = null;
                Error = e.Message;
            }

            return _file;
        }
    }

    /// <summary>
    /// Nastavi subor pisiem (prazdna cesta = pismo Windows) a zapise nastavenia programu.
    /// </summary>
    public void Set(string path)
    {
        config.ElenPreviewFonts = path;
        _loadedPath = null;
        save();
    }
}
