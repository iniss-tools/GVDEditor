using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.TabTab;

namespace GVDEditor.TabTabEditor;

/// <summary>
/// Symboly nacitaneho grafikonu pre kontrolu vyrazov a TabTab: druhy vlakov z TrTypes.txt, stanice,
/// kolaje a dopravcovia.
/// </summary>
internal sealed class GvdExprSymbols : IExprSymbolProvider
{
    private readonly Dictionary<string, int> _trainTypeKeys;
    private readonly HashSet<int> _stations;
    private readonly HashSet<string> _tracks;
    private readonly HashSet<string> _operators;
    private readonly IEnumerable<TableCatalog> _catalogs;

    /// <summary>
    /// Zostavi symboly z instalacie INISS a grafikonu.
    /// </summary>
    public GvdExprSymbols(InissWorkspace workspace, GrafikonDocument document)
    {
        _trainTypeKeys = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var t in workspace.TrainsTypes ?? [])
        {
            var idx = ExprTrainTypes.IndexOf(t.CategoryTrain);
            if (idx >= 0 && !string.IsNullOrEmpty(t.Key))
                _trainTypeKeys.TryAdd(t.Key, idx);
        }

        _stations = [];
        foreach (var s in (workspace.Stations ?? []).Concat(document.CustomStations ?? []))
            if (int.TryParse(s.ID, out var id))
                _stations.Add(id);

        _tracks = new HashSet<string>((document.Tracks ?? []).Select(t => t.Name), StringComparer.Ordinal);
        _operators = new HashSet<string>((document.Operators ?? []).Select(o => o.Name), StringComparer.Ordinal);
        _catalogs = document.TableCatalogs ?? [];
    }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, int>? TrainTypeKeys => _trainTypeKeys;

    /// <inheritdoc />
    public bool? StationExists(int id) => _stations.Count == 0 ? null : _stations.Contains(id);

    /// <inheritdoc />
    public bool? TrackExists(string name) => _tracks.Count == 0 ? null : _tracks.Contains(name);

    /// <inheritdoc />
    public bool? OperatorExists(string name) =>
        _operators.Count == 0 ? null : _operators.Contains(name) || _operators.Any(o => ExprEvaluator.CzechEquals(o, name));

    /// <summary>
    /// Mena a kluce stlpcov katalogovych tabul, ktore danu sekciu TabTab pouzivaju (pre <c>%meno%</c>).
    /// <see langword="null"/>, ak sekciu nepouziva ziadna tabula - kontrola sa vtedy nerobi.
    /// </summary>
    public IReadOnlyCollection<string>? ColumnNamesFor(TableTabTab tab)
    {
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var used = false;
        foreach (var catalog in _catalogs)
        {
            if (!catalog.Items.Any(i => i.Tab1 == tab || i.Tab2 == tab)) continue;
            used = true;
            foreach (var item in catalog.Items)
            {
                if (!string.IsNullOrEmpty(item.Name)) names.Add(item.Name);
                if (!string.IsNullOrEmpty(item.Key)) names.Add(item.Key);
            }
        }
        return used ? names : null;
    }

    /// <summary>
    /// Nastavenia kontroly sekcie TabTab pre danu sekciu.
    /// </summary>
    public TabTabValidationOptions OptionsFor(TableTabTab tab) => new()
    {
        Symbols = this,
        ColumnNames = ColumnNamesFor(tab),
        ReportContextDependent = false
    };
}
