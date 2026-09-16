using ToolsCore.Tools;

namespace GVDEditor.Tools;

/// <summary>
///     Zbiera varovania, ktore vzniknu pri nacitani dat (preskocene riadky, chybajuce nahravky, neznamy druh vlaku...).
///     Kazde varovanie sa zaroven zapise do logu; po nacitani grafikonu ich hlavne okno ukaze pouzivatelovi naraz.
/// </summary>
internal static class LoadWarnings
{
    private static readonly List<string> _items = new();
    private static readonly object _locker = new();

    /// <summary>
    ///     Varovania od posledneho volania <see cref="Clear" />.
    /// </summary>
    public static IReadOnlyList<string> Items
    {
        get
        {
            lock (_locker)
                return _items.ToList();
        }
    }

    /// <summary>
    ///     Prida varovanie a zapise ho do logu.
    /// </summary>
    /// <param name="message">Text varovania.</param>
    public static void Add(string message)
    {
        Log.Warning(message);
        lock (_locker)
            _items.Add(message);
    }

    /// <summary>
    ///     Vyprazdni zoznam - vola sa pred nacitanim dalsieho grafikonu.
    /// </summary>
    public static void Clear()
    {
        lock (_locker)
            _items.Clear();
    }

    /// <summary>
    ///     Ak sa pri nacitani nieco preskocilo, ukaze suhrn (najviac <paramref name="maxLines" /> riadkov) a zoznam vyprazdni.
    /// </summary>
    /// <param name="maxLines">Kolko varovani vypisat do okna; zvysok je v logu.</param>
    public static void ShowSummary(int maxLines = 12)
    {
        List<string> items;
        lock (_locker)
        {
            if (_items.Count == 0)
                return;

            items = _items.ToList();
            _items.Clear();
        }

        var sb = new StringBuilder();
        sb.AppendLine(string.Format(Properties.Resources.LoadWarnings_Header, items.Count));
        sb.AppendLine();
        foreach (var item in items.Take(maxLines))
            sb.AppendLine("• " + item);

        if (items.Count > maxLines)
            sb.AppendLine(string.Format(Properties.Resources.LoadWarnings_More, items.Count - maxLines));

        sb.AppendLine();
        sb.Append(Properties.Resources.LoadWarnings_Footer);

        Utils.ShowWarning(sb.ToString());
    }
}
