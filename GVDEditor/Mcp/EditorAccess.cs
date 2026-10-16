using System.Globalization;
using GVDEditor.Domain.Snapshots;
using GVDEditor.Properties;
using ModelContextProtocol;

namespace GVDEditor.Mcp;

/// <summary>
/// Pristup nastrojov MCP k otvorenemu grafikonu: kazde citanie aj zmena bezi na vlakne hlavneho okna, kde dokument
/// meni aj pouzivatel, takze nastroj nikdy nevidi polovicnu upravu. Bez okna (<paramref name="owner" /> null, testy)
/// pristupuje priamo; bez <paramref name="host" /> sa zmena nekontroluje voci oknu ani nezapamata ako krok spat.
/// </summary>
internal sealed class EditorAccess(EditorContext context, Control? owner, IAgentHost? host = null)
{
    /// <summary>Popis parametra <c>dryRun</c> nastrojov so zmenou.</summary>
    public const string DryRunDescription = "Only report the result; the timetable stays as it is.";

    /// <summary>Vysledok <paramref name="read" /> nad kontextom editora z vlakna okna.</summary>
    public Task<T> ReadAsync<T>(Func<EditorContext, T> read)
    {
        if (owner is null)
            return Task.FromResult(read(context));
        var result = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        if (owner.IsDisposed || !owner.IsHandleCreated)
        {
            result.SetException(new McpException("editor.closed: GVDEditor is closing."));
            return result.Task;
        }

        owner.BeginInvoke(() =>
        {
            try
            {
                result.SetResult(read(context));
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
        });
        return result.Task;
    }

    /// <summary>
    /// Zmena otvoreneho grafikonu agentom: jedno volanie = jeden krok spat „<paramref name="label" /> (agent)“
    /// v GVDEditore, grafikon sa neuklada. Pri chybe (<see cref="McpException" /> s kodom) a pri skuske nanecisto
    /// (<paramref name="dryRun" />) sa grafikon vrati do stavu pred zmenou - vysledok je rovnaky ako pri skutocnej zmene.
    /// </summary>
    public Task<(T Result, ChangeInfo Change)> WriteAsync<T>(string label, bool dryRun, Func<EditorContext, T> write) =>
        WriteAsync(_ => label, dryRun, write);

    /// <inheritdoc cref="WriteAsync{T}(string, bool, Func{EditorContext, T})" />
    /// <param name="label">popis kroku podla grafikonu pred zmenou (napr. nova alebo upravena tabula)</param>
    public Task<(T Result, ChangeInfo Change)> WriteAsync<T>(Func<EditorContext, string> label, bool dryRun, Func<EditorContext, T> write) =>
        ReadAsync(_ =>
        {
            var document = McpGuards.Open(context);
            var step = string.Format(CultureInfo.CurrentCulture, Resources.History_Agent, label(context));
            T result = default!;
            var changed = false;

            string? Apply()
            {
                var before = GrafikonState.Capture(document, context.Directory);
                try
                {
                    result = write(context);
                }
                catch
                {
                    before.Restore();
                    throw;
                }

                changed = !before.SameAs(GrafikonState.Capture(document, context.Directory));
                if (!changed || !dryRun)
                    return changed ? step : null;
                before.Restore();
                return null;
            }

            if (host is null)
                Apply();
            else
                host.Change(Apply);
            return (result, new ChangeInfo(dryRun, changed, changed && !dryRun ? step : null));
        });
}

/// <summary>Hlavne okno GVDEditora pri zmene grafikonu agentom.</summary>
internal interface IAgentHost
{
    /// <summary>
    /// Vykona zmenu <paramref name="change" /> na vlakne okna: ak sa grafikon teraz menit neda (otvorene okno, rozpisana
    /// bunka, nacitavanie), hodi <see cref="McpException" /> s kodom <c>editor.busy</c>. Pocas zmeny potlaci udalosti
    /// okna, potom obnovi zobrazenie a zapamata krok spat s popisom, ktory zmena vrati (null = grafikon sa nezmenil).
    /// </summary>
    void Change(Func<string?> change);
}
