using ToolsCore.Iniss2.Tables;

namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Linka fyzickej tabule INISS 3.39 v tvare INISS2.
/// </summary>
/// <param name="Grafikon">priecinok grafikonu v DirList.TXT</param>
/// <param name="TableKey">kluc fyzickej tabule</param>
/// <param name="Line">linka</param>
internal sealed record Iniss2TableLine(string Grafikon, string TableKey, CommLineDefinition Line);
