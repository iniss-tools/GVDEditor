using ExControls;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using ToolsCore.Entities;
using ToolsCore.StateDgm;
using ToolsCore.Tools;
using ToolsCore.XML;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Tools.Utils;
using static GVDEditor.Formats.FormatCommon;

namespace GVDEditor.Formats;

/// <summary>
/// Stavovy diagram grafikonu (StateDgm.txt) a jeho vyrovnavacia pamat StateDgm.dat.
/// </summary>
internal static class StateDgmFile
{
    /// <summary>
    /// Zapise predlohu stavoveho diagramu do priecinka grafikonu.
    /// </summary>
    /// <param name="path">cesta do priecinka grafikonu</param>
    /// <param name="dataDir">priecinok DATA instalacie INISS (vyrovnavacia pamat StateDgm.dat)</param>
    /// <param name="template">predloha</param>
    public static void WriteTemplate(string path, string dataDir, StateDgmTemplate template = StateDgmTemplate.Slovak)
    {
        File.WriteAllText(CombinePath(path, FILE_STATEDGM)!, TemplateText(template), Encodings.Win1250);
        DeleteCache(dataDir);
    }

    /// <summary>
    /// Text predlohy stavoveho diagramu (na nahlad alebo na porovnanie).
    /// </summary>
    public static string TemplateText(StateDgmTemplate template) => template switch
    {
        StateDgmTemplate.Czech => Resources.statedgmCZ,
        StateDgmTemplate.SlovakIltis => Resources.statedgmILTIS,
        _ => Resources.statedgmSK
    };

    /// <summary>
    /// Cesta k suboru StateDgm.txt v priecinku grafikonu (INISS meno suboru nerozlisuje velkostou pismen,
    /// preto sa pouzije existujuci subor, ak tam je).
    /// </summary>
    public static string PathOf(string dir)
    {
        var existing = Directory.Exists(dir)
            ? Directory.EnumerateFiles(dir).FirstOrDefault(f => string.Equals(Path.GetFileName(f), FILE_STATEDGM, StringComparison.OrdinalIgnoreCase))
            : null;
        return existing ?? CombinePath(dir, FILE_STATEDGM)!;
    }

    /// <summary>
    /// Nacita stavovy diagram grafikonu; null, ak subor neexistuje. Chyby syntaxe vyhadzuje
    /// <see cref="StateDgmParseException" />.
    /// </summary>
    public static StateDgmDiagram? Read(string dir)
    {
        var file = PathOf(dir);
        return File.Exists(file) ? StateDgmDiagram.Load(file) : null;
    }

    /// <summary>
    /// Zapise stavovy diagram do priecinka grafikonu a zmaze vyrovnavaciu pamat StateDgm.dat.
    /// </summary>
    public static void Write(string dir, string dataDir, StateDgmDiagram diagram)
    {
        diagram.Save(PathOf(dir));
        DeleteCache(dataDir);
    }

    /// <summary>
    /// Zapise text stavoveho diagramu tak, ako je (aj s chybou syntaxe), a zmaze vyrovnavaciu pamat StateDgm.dat.
    /// </summary>
    public static void WriteText(string dir, string dataDir, string text)
    {
        File.WriteAllText(PathOf(dir), text, Encodings.Win1250);
        DeleteCache(dataDir);
    }

    /// <summary>
    /// Zmaze StateDgm.dat v koreni datoveho adresara. INISS textove diagramy cita znova, len ked .dat chyba
    /// alebo je starsi nez niektory StateDgm.txt - po kopii suborov so starym casom by inak dalej pouzival
    /// stary diagram.
    /// </summary>
    public static void DeleteCache(string dataDir)
    {
        if (string.IsNullOrEmpty(dataDir) || !Directory.Exists(dataDir)) return;
        foreach (var f in Directory.EnumerateFiles(dataDir).Where(f => string.Equals(Path.GetFileName(f), FILE_STATEDGM_DAT, StringComparison.OrdinalIgnoreCase)))
            File.Delete(f);
    }
}
