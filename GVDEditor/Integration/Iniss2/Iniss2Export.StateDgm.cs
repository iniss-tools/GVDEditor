using ToolsCore.Iniss2.Grafikon;

namespace GVDEditor.Integration.Iniss2;

internal static partial class Iniss2Export
{
    private sealed partial class StationBuilder
    {
        // kluc kolaje INISS2 podla nazvu kolaje 3.39 - pre podmienky na kolaj
        private Dictionary<string, string> TrackKeys() =>
            document.Tracks.GroupBy(track => track.Name, StringComparer.Ordinal)
                .ToDictionary(group => group.Key, group => group.First().Key, StringComparer.Ordinal);

        // stavovy diagram grafikonu v tvare INISS2; hlasenie stavu je nazov hlasenia v ReportKeys.xml stanice
        private StateDiagram? ToStateDiagram()
        {
            if (stateDgm is null)
                return null;

            var tracks = TrackKeys();
            var converter = new Iniss2StateDgmConverter(new Iniss2ExpressionTranspiler(source.TrainTypeKeys, name => tracks.GetValueOrDefault(name)),
                ReportName);
            var diagram = converter.Convert(stateDgm);
            state.Warnings.AddRange(converter.Warnings.Select(warning => warning with { Detail = $"{gvd.ThisStation.Name}: {warning.Detail}" }));
            return diagram;
        }

        // bez sablony je nazov hlasenia kluc typu 3.39, so sablonou hlasenie sablony s pismenom priradenym typu
        private string? ReportName(string key)
        {
            var type = document.ReportTypes.FirstOrDefault(type => type.Key == key);
            if (type is null)
                return null;

            if (source.ReportTemplate is not { } template)
                return type.Key;

            var letter = source.ReportLetters.TryGetValue(type.Key, out var mapped) ? mapped : type.Char;
            return letter.Length == 0 ? null : template.FirstOrDefault(report => string.Equals(report.Key, letter, StringComparison.OrdinalIgnoreCase))?.Name;
        }
    }
}
