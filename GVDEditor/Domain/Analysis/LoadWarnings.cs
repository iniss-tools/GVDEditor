using System.Globalization;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Domain.Analysis;

/// <summary>
/// Varovania pri nacitani instalacie a grafikonu, ktore nezastavia nacitanie (preskoceny riadok, chybajuca
/// nahravka...). Kazde nacitanie dostane zberac explicitne; zbiera sa aj z vlakna na pozadi. Varovanie sa aj zaloguje.
/// </summary>
internal sealed class LoadWarnings
{
    private readonly List<string> _items = new();
    private readonly object _locker = new();

    /// <summary>
    /// Zozbierane varovania (kopia).
    /// </summary>
    public IReadOnlyList<string> Items
    {
        get
        {
            lock (_locker)
                return _items.ToList();
        }
    }

    /// <summary>
    /// Prida varovanie a zaloguje ho.
    /// </summary>
    public void Add(string message)
    {
        Log.Warning(message);
        lock (_locker)
            _items.Add(message);
    }

    /// <summary>
    /// Vyprazdni zoznam.
    /// </summary>
    public void Clear()
    {
        lock (_locker)
            _items.Clear();
    }

    /// <summary>
    /// Suhrn varovani pre pouzivatela (najviac <paramref name="maxLines" /> riadkov) - zoznam sa pritom vyprazdni.
    /// </summary>
    /// <returns><see langword="null" />, ak ziadne varovanie nie je.</returns>
    public string? TakeSummary(int maxLines = 12)
    {
        List<string> items;
        lock (_locker)
        {
            if (_items.Count == 0)
                return null;

            items = _items.ToList();
            _items.Clear();
        }

        var sb = new StringBuilder();
        sb.AppendLine(string.Format(CultureInfo.CurrentCulture, Properties.Resources.LoadWarnings_Header, items.Count));
        sb.AppendLine();
        foreach (var item in items.Take(maxLines))
            sb.AppendLine("• " + item);

        if (items.Count > maxLines)
            sb.AppendLine(string.Format(CultureInfo.CurrentCulture, Properties.Resources.LoadWarnings_More, items.Count - maxLines));

        sb.AppendLine();
        sb.Append(Properties.Resources.LoadWarnings_Footer);
        return sb.ToString();
    }
}
