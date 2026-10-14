using ModelContextProtocol;

namespace GVDEditor.Mcp;

/// <summary>
/// Pristup nastrojov MCP k otvorenemu grafikonu: kazde citanie bezi na vlakne hlavneho okna, kde dokument meni aj
/// pouzivatel, takze nastroj nikdy nevidi polovicnu upravu. Bez okna (<paramref name="owner" /> null, testy) cita priamo.
/// </summary>
internal sealed class EditorAccess(EditorContext context, Control? owner)
{
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
}
