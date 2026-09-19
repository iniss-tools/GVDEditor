using ExControls;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using JetBrains.Annotations;
using ToolsCore.Expressions;
using ToolsCore.TabTab;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Nahlad vysledku stlpcov katalogovych tabul pre vybrany vlak a simulovany prevadzkovy stav -
///     ukaze, co by INISS poslal na tabulu po uplatneni pravidiel TabTab (vratane neulozenych uprav v editore).
/// </summary>
public partial class FTabTabPreview : Form
{
    private readonly Func<string, string?> _sectionText;
    private readonly string? _currentSection;
    private readonly int _homeStationId;
    private readonly Dictionary<string, TabTabSection?> _sections = new(StringComparer.Ordinal);
    private readonly BindingList<ResultRow> _rows = [];
    // ReSharper disable once MemberInitializerValueIgnored
    private readonly bool _loading = true;

    /// <summary>
    ///     Vytvori nahlad.
    /// </summary>
    /// <param name="sectionText">Text sekcie TabTab podla mena (aktualny text z editora); <see langword="null"/>, ak sekcia nie je.</param>
    /// <param name="currentSection">Meno sekcie otvorenej v editore - predvolene sa zobrazia len stlpce, ktore ju pouzivaju.</param>
    /// <param name="homeStationId">ID stanice grafikonu (pre <c>ZAJMSTANICE</c>, <c>MISTNI</c>).</param>
    internal FTabTabPreview(Func<string, string?> sectionText, string? currentSection, int homeStationId)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();
        if (GlobData.UsingStyle.DarkTitleBar) ExTools.SetImmersiveDarkMode(Handle, true);

        _sectionText = sectionText;
        _currentSection = currentSection;
        _homeStationId = homeStationId;

        chkOnlySection.Enabled = currentSection is not null;
        chkOnlySection.Checked = currentSection is not null;
        chkOnlySection.Text = string.Format(Resources.FTabTabPreview_Len_sekcia, currentSection ?? "");

        cbTrain.DisplayMember = nameof(TrainItem.Text);
        foreach (var t in GlobData.Trains.OrderBy(t => t.Arrival ?? t.Departure))
            cbTrain.Items.Add(new TrainItem(t));
        if (cbTrain.Items.Count > 0) cbTrain.SelectedIndex = 0;

        dgvResult.DataSource = _rows;
        _loading = false;
        Recompute();
    }

    /// <summary>
    ///     Zneplatni nacitane sekcie (po zmene textu v editore) a prepocita.
    /// </summary>
    public void RefreshPreview()
    {
        _sections.Clear();
        Recompute();
    }

    private sealed record TrainItem(Train Train)
    {
        public string Text => $"{Train.Type.Key} {Train.Number}{(string.IsNullOrEmpty(Train.Name) ? "" : " " + Train.Name)}  "
                              + $"{Train.Arrival?.ToString("HH:mm") ?? "–"} / {Train.Departure?.ToString("HH:mm") ?? "–"}  {Train.Routing.Symbol}";
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    private sealed class ResultRow
    {
        public string Table { get; init; } = "";
        public string Column { get; init; } = "";
        public string Kind { get; init; } = "";
        public int DivType { get; init; }
        public string Tab1 { get; init; } = "";
        public string Tab2 { get; init; } = "";
        public string Own { get; init; } = "";
        public string Result { get; init; } = "";
        public string Font { get; init; } = "";
        public TabTabComposeResult? Compose { get; init; }
    }

    private void Input_Changed(object sender, EventArgs e)
    {
        if (!_loading) Recompute();
    }

    private TabTabSection? Section(string? name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        if (_sections.TryGetValue(name, out var s)) return s;
        var text = _sectionText(name);
        s = text is null ? null : TabTabSection.Parse(text);
        _sections[name] = s;
        return s;
    }

    private void Recompute()
    {
        var selected = dgvResult.CurrentRow?.Index ?? 0;
        _rows.RaiseListChangedEvents = false;
        _rows.Clear();

        if (cbTrain.SelectedItem is TrainItem ti)
        {
            var runtime = new TrainRuntime
            {
                ArrivalDelayMinutes = (int)nudDelayArr.Value,
                DepartureDelayMinutes = (int)nudDelayDep.Value,
                IsStanding = chkStanding.Checked,
                IsDispatched = chkDispatched.Checked,
                LockoutArrival = chkLockArr.Checked,
                LockoutDeparture = chkLockDep.Checked,
                IsDeflected = chkDeflected.Checked
            };
            var ctx = new GvdTrainContext(ti.Train, runtime, _homeStationId);

            foreach (var catalog in GlobData.TableCatalogs)
            {
                var usesSection = catalog.Items.Any(i => UsesCurrent(i));
                if (chkOnlySection.Checked && !usesSection) continue;

                foreach (var item in catalog.Items)
                {
                    if (chkOnlySection.Checked && !UsesCurrent(item)) continue;
                    var r = ComposeColumn(catalog, item, ctx, 0);
                    _rows.Add(new ResultRow
                    {
                        Table = catalog.Name,
                        Column = item.Name,
                        Kind = item.FillSection.Name,
                        DivType = item.DivType.Id,
                        Tab1 = item.Tab1 == TableTabTab.Empty ? "" : item.Tab1.Key,
                        Tab2 = item.Tab2 == TableTabTab.Empty ? "" : item.Tab2.Key,
                        Own = ctx.OwnValue(item).Text,
                        Result = r.Value.Text,
                        Font = FontText(r.Value.Font),
                        Compose = r
                    });
                }
            }
        }

        _rows.RaiseListChangedEvents = true;
        _rows.ResetBindings();
        if (_rows.Count > 0)
        {
            dgvResult.ClearSelection();
            var idx = Math.Min(selected, _rows.Count - 1);
            dgvResult.Rows[idx].Selected = true;
            dgvResult.CurrentCell = dgvResult.Rows[idx].Cells[cResult.Index];
        }
        ShowSteps();
    }

    private bool UsesCurrent(TableItem item) =>
        _currentSection is not null && (item.Tab1?.Key == _currentSection || item.Tab2?.Key == _currentSection);

    private TabTabComposeResult ComposeColumn(TableCatalog catalog, TableItem item, GvdTrainContext ctx, int depth)
    {
        var first = catalog.Items.FirstOrDefault();
        // INISS berie tabulu ako prichodovu, ked jej prvy stlpec je typu 1 (cas prichodu)
        var site = first is not null && first.FillSection == TableFillSection.CasPrichodu ? ExprEvalSite.ArrivalTable : ExprEvalSite.Default;

        TabTabValue? ttexts = null;
        foreach (var tt in GlobData.TableTexts)
        {
            if (!tt.Realizations.Any(r => r.Table == catalog && r.Item == item)) continue;
            var train = tt.Trains.FirstOrDefault(t => t.Train == ctx.Train);
            if (train is not null)
            {
                ttexts = new TabTabValue(train.Text, train.FontID);
                break;
            }
        }

        return TabTabComposer.Compose(new TabTabColumnInput
        {
            Train = ctx,
            Site = site,
            OwnValue = ctx.OwnValue(item),
            TTextsValue = ttexts,
            DivType = item.DivType.Id,
            TypeItemsIdx = item.FillSection.Id,
            Tab1 = item.Tab1 == TableTabTab.Empty ? null : Section(item.Tab1.Key),
            Tab2 = item.Tab2 == TableTabTab.Empty ? null : Section(item.Tab2.Key),
            ColumnValue = depth < 3
                ? name =>
                {
                    var other = catalog.Items.FirstOrDefault(i =>
                        string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase) || string.Equals(i.Key, name, StringComparison.OrdinalIgnoreCase));
                    return other is null || other == item ? null : ComposeColumn(catalog, other, ctx, depth + 1).Value;
                }
                : null
        });
    }

    private static string FontText(int? font) => font switch
    {
        null => "",
        TabTabText.DefaultFont => "{@}",
        _ => font.Value.ToString()
    };

    private void dgvResult_SelectionChanged(object sender, EventArgs e) => ShowSteps();

    private void ShowSteps()
    {
        if (dgvResult.SelectedRows.Count == 0 || dgvResult.SelectedRows[0].DataBoundItem is not ResultRow { Compose: { } c })
        {
            tbSteps.Text = "";
            return;
        }

        var sb = new StringBuilder();
        foreach (var s in c.Steps)
        {
            sb.Append(s.Source.PadRight(22)).Append("→ \"").Append(s.Value.Text).Append('"');
            if (s.Value.Font is not null) sb.Append(" {").Append(FontText(s.Value.Font)).Append('}');
            if (s.Note.Length > 0) sb.Append("   ").Append(s.Note);
            sb.AppendLine();
        }
        if (c.Error is not null)
            sb.AppendLine().Append(Resources.FTabTabPreview_Chyba_vyhodnotenia).Append(' ').Append(c.Error);
        tbSteps.Text = sb.ToString();
    }
}
