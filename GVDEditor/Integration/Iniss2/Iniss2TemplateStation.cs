using ToolsCore.Iniss2.Grafikon;

namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Stanica cielovej instalacie INISS2, ktora moze byt sablonou hlaseni - ma stavovy diagram aj hlasenia.
/// </summary>
/// <param name="Group">grafikon</param>
/// <param name="Directory">priecinok stanice</param>
/// <param name="Path">cela cesta k priecinku stanice</param>
/// <param name="Reports">hlasenia stanice</param>
internal sealed record Iniss2TemplateStation(string Group, string Directory, string Path, IReadOnlyList<ReportKey> Reports)
{
    /// <inheritdoc />
    public override string ToString() => $"{Group} / {Directory}";
}
