using System.Net;
using System.Text.Json;
using GVDEditor.Config;
using GVDEditor.Integration;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using TableSimulator.Contracts.Station;

namespace GVDEditor.Mcp;

/// <summary>Nastroje MCP nad simulatorom tabul (TableSimulator).</summary>
[McpServerToolType]
internal static class SimulatorTools
{
    /// <summary>Kde sa v GVDEditore linky presmeruju a nastavi simulator.</summary>
    private const string RedirectWindow = "GVDEditor > INISS settings > Redirect boards to the simulator";

    [McpServerTool(Name = "prepare_simulator", Title = "Prepare board simulator", ReadOnly = false, Destructive = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Creates the lines and boards of the station in the board simulator (TableSimulator) like 'Prepare in the simulator' in GVDEditor: "
                 + "lines of the selected run configuration with the boards INISS assigns to them, their faces, virtual boards from the XML export and "
                 + "the INISS log folder. Existing boards stay. The INISS .INI does not change - INISS sends to the simulator only after its lines are "
                 + "redirected (" + RedirectWindow + "); notes list the lines that are not. Run it with dryRun first to see the lines.")]
    public static async Task<SimulatorPrepareResult> PrepareSimulator(
        EditorAccess editor,
        [Description("Line numbers to prepare; null = the lines the .INI next to the INISS program redirects to the simulator, or all active lines "
                     + "when none is redirected.")]
        IReadOnlyList<int>? lines = null,
        [Description("Only report the lines and boards; nothing is sent to the simulator.")] bool dryRun = false)
    {
        // nastavenia na vlakne okna, nacitanie instalacie a register mimo neho
        var (config, installationDir, grafikons, circuits, settings) = await editor.ReadAsync(context =>
        {
            var run = context.RunConfigurations.Selected
                      ?? throw new McpException("iniss.noConfiguration: No run configuration is selected in GVDEditor; the user selects it on the toolbar.");
            if (run.Program.Length == 0 || context.Workspace is not { INISSDir.Length: > 0 } workspace)
                throw new McpException($"iniss.noProgram: The run configuration '{run.Name}' has no INISS program or no installation is open.");
            var simulator = context.Config.TableSimulator;
            return (run, workspace.INISSDir, (IReadOnlyList<DirList>)workspace.GVDDirs.ToList(),
                workspace.Audios.Select(a => (a.Name, (string?)a.AmplifierPort, (string?)a.ExchangeParameter)).ToList(),
                new TableSimulatorSettings { WebUrl = simulator.WebUrl, BasePort = simulator.BasePort, ProtectedApiKey = simulator.ProtectedApiKey });
        }).ConfigureAwait(false);

        var url = TableSimulatorClient.ParseUrl(settings.WebUrl)
                  ?? throw new McpException($"simulator.url: The simulator address '{settings.WebUrl}' is not valid; the user sets it in {RedirectWindow}.");
        var plan = await Task.Run(() => SimulatorPreparation.Plan(config, installationDir, grafikons, circuits, settings, lines?.ToList())).ConfigureAwait(false);
        if (plan.Missing.Count > 0)
            throw new McpException($"simulator.lineNotFound: Lines {string.Join(", ", plan.Missing)} are not active lines of the run configuration '{config.Name}'.");
        if (plan.Lines.Count == 0)
            throw new McpException($"simulator.noLines: The run configuration '{config.Name}' has no active lines with a number.");

        List<string> notes = [];
        if (plan.Lines.Where(l => !l.Redirected).Select(l => l.Line.Line!.Value).ToList() is { Count: > 0 } notRedirected)
            notes.Add($"Lines {string.Join(", ", notRedirected)} are not redirected to the simulator in the .INI next to the INISS program - INISS keeps "
                      + $"sending them to the real boards. The user redirects them in {RedirectWindow} with the first port {settings.BasePort}.");

        var lineInfos = plan.Lines.Select(l => new SimulatorLineInfo(l.Line.Line!.Value, l.Line.Section, l.Line.Class, l.Port,
            plan.Request.Boards.Count(b => b.Line == l.Line.Line), l.Redirected)).ToList();
        if (dryRun)
            return new SimulatorPrepareResult(true, url.ToString(), config.Name, lineInfos, plan.Request.XmlTables.Count, plan.Request.LogFolder, null, notes);

        StationImportedDto imported;
        try
        {
            imported = await TableSimulatorClient.ImportAsync(url, settings.ApiKey, plan.Request, CancellationToken.None).ConfigureAwait(false);
        }
        catch (SimulatorHttpsRequiredException e)
        {
            throw new McpException($"simulator.httpsRequired: The simulator accepts only HTTPS from the network; the user sets its address to {e.HttpsUrl} in {RedirectWindow}.");
        }
        catch (HttpRequestException e) when (TableSimulatorClient.IsUntrustedCertificate(e))
        {
            throw new McpException($"simulator.untrustedCertificate: This computer does not trust the certificate of the simulator; the user installs its "
                                   + $"certification authority from {new Uri(url, "api/https/certificate")}.");
        }
        catch (HttpRequestException e) when (e.StatusCode is HttpStatusCode.Unauthorized)
        {
            throw new McpException($"simulator.unauthorized: The simulator requires an API key; the user sets it in {RedirectWindow}.");
        }
        catch (HttpRequestException e) when (e.StatusCode is HttpStatusCode.Forbidden)
        {
            throw new McpException("simulator.forbidden: The API key of GVDEditor may not change lines and boards in the simulator (permission simulator.edit).");
        }
        catch (Exception e) when (e is HttpRequestException or JsonException or TaskCanceledException)
        {
            throw new McpException($"simulator.offline: The simulator at {url} does not answer or refused the request: {e.Message}");
        }

        return new SimulatorPrepareResult(false, url.ToString(), config.Name, lineInfos, plan.Request.XmlTables.Count, plan.Request.LogFolder,
            new SimulatorImportInfo(imported.LinesAdded, imported.LinesUpdated, imported.BoardsAdded, imported.BoardsKept, imported.VirtualBoardsAdded,
                imported.VirtualBoardsKept, imported.UnsupportedLines, imported.SkippedBoards, imported.LogFolder switch
                {
                    StationLogFolder.Added => "added",
                    StationLogFolder.Watched => "watched",
                    StationLogFolder.Missing => "missing",
                    StationLogFolder.Full => "full",
                    _ => "none"
                }),
            notes);
    }
}
