using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using ToolsCore.XML;

namespace GVDEditor.Formats;

/// <summary>
/// Zavislosti citania a zapisu suborov grafikonu: instalacia INISS (banka, jazyky stanice, druhy vlakov),
/// dokument grafikonu, do ktoreho sa cita alebo z ktoreho sa zapisuje, a jazyk hlaviciek suborov.
/// </summary>
/// <param name="Workspace">zvolena instalacia INISS</param>
/// <param name="Document">grafikon - pri citani ten, ktory sa prave naplna</param>
/// <param name="CommentLanguage">jazyk komentarov v hlavicke zapisanych suborov</param>
internal sealed record GrafikonContext(InissWorkspace Workspace, GrafikonDocument Document, AppLanguage CommentLanguage)
{
    /// <summary>
    /// Kontext otvoreneho grafikonu (cez fasadu <see cref="GlobData" />) - pre okna, ktore zatial pracuju s GlobData.
    /// Bez nacitanej konfiguracie (testy) su hlavicky suborov po slovensky.
    /// </summary>
    public static GrafikonContext Current =>
        new(GlobData.Workspace, GlobData.Document, GlobData.Session?.Config.Language ?? AppLanguage.Slovak);

    /// <summary>
    /// Stanica podla identifikatora - zo zvukovej banky alebo zo stanic grafikonu; neznama stanica ma za nazov
    /// svoj identifikator.
    /// </summary>
    public Station StationFromID(string? id) => Station.GetFromID(id, Workspace.Stations, Document.CustomStations);
}
