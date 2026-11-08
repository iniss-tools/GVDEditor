using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using ToolsCore.Iniss.Entities;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss2.Grafikon;

namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Udaje instalacie INISS 3.39 mimo grafikonu, ktore export potrebuje.
/// </summary>
/// <param name="Languages">jazyky instalacie; vlak so vsetkymi sa hlasi vo vsetkych jazykoch banky</param>
/// <param name="UserTexts">texty zalozene obsluhou (LogZvuk.usr)</param>
/// <param name="Lockouts">vyluky obsluhy (Vyluky1.txt)</param>
/// <param name="Diversions">odklony obsluhy (Odklony1.txt)</param>
internal sealed record Iniss2ExportSource(
    IReadOnlyList<FyzLanguage> Languages,
    IReadOnlyList<LogZvukText> UserTexts,
    IReadOnlyList<TrainNotice> Lockouts,
    IReadOnlyList<TrainNotice> Diversions)
{
    /// <summary>Linky tabul zo zistenia INISSu 3.39, pozri <see cref="Iniss2Export.TableLines" />.</summary>
    public IReadOnlyList<Iniss2TableLine> TableLines { get; init; } = [];

    /// <summary>Banky pisem ELEN: kluc katalogovej tabule 3.39 → subor .bin.</summary>
    public IReadOnlyDictionary<string, string> FontBanks { get; init; } = new Dictionary<string, string>();

    /// <summary>
    /// Stanica cielovej instalacie, z ktorej sa prevezmu hlasenia ReportKeys.xml a statedgm.xml pre stanice bez StateDgm.txt;
    /// prazdne = hlasenia z typov 3.39.
    /// </summary>
    public string TemplateStation { get; init; } = "";

    /// <summary>Hlasenia sablony; null = hlasenia z typov hlaseni 3.39.</summary>
    public IReadOnlyList<ReportKey>? ReportTemplate { get; init; }

    /// <summary>
    /// Pismeno hlasenia INISS2 pre typ hlasenia 3.39 podla kluca typu; typ, ktory tu nie je, ma svoj znak, prazdne pismeno = dodatky
    /// a radenie sa pri nom neprehraju.
    /// </summary>
    public IReadOnlyDictionary<string, string> ReportLetters { get; init; } = new Dictionary<string, string>();

    /// <summary>Kluce druhov vlakov z TrTypes s indexom zabudovaneho druhu INISSu - pre preklad podmienok TabTab.</summary>
    public IReadOnlyDictionary<string, int> TrainTypeKeys { get; init; } = new Dictionary<string, int>();

    /// <summary>
    /// Udaje nacitanej instalacie; vyluky a odklony obsluhy sa citaju zo suborov.
    /// </summary>
    /// <param name="workspace">instalacia</param>
    public static Iniss2ExportSource FromWorkspace(InissWorkspace workspace)
    {
        ArgumentNullException.ThrowIfNull(workspace);

        var keys = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var type in workspace.TrainsTypes ?? [])
        {
            var index = ExprTrainTypes.IndexOf(type.CategoryTrain);
            if (index >= 0 && !string.IsNullOrEmpty(type.Key))
                keys.TryAdd(type.Key, index);
        }

        return new Iniss2ExportSource(workspace.Languages, workspace.LogZvukTexts,
            Formats.TrainNoticesFile.Read(workspace.DataDir, Formats.TrainNoticesFile.FileLockouts),
            Formats.TrainNoticesFile.Read(workspace.DataDir, Formats.TrainNoticesFile.FileDiversions)) { TrainTypeKeys = keys };
    }
}
