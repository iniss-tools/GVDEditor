using GVDEditor.Entities;

namespace GVDEditor.Tools;

/// <summary>
///     Radenia vlaku upravovane v okne vlaku (zalozka Radenie). Okno pracuje s kopiami, aby sa uprava dala zrusit;
///     az <see cref="Commit" /> zapise zmeny do grafikonu.
/// </summary>
/// <remarks>
///     Radenie patri cislu vlaku: ten isty objekt <see cref="Radenie" /> je v <c>GlobData.Radenia</c> aj
///     v <see cref="Train.Radenia" /> vsetkych vlakov s tymto cislom - preto sa povodne objekty nenahradzaju, ale
///     prepisuju sa ich vlastnosti.
/// </remarks>
internal sealed class RadeniaEditing
{
    // kopia -> povodny objekt; nove radenia (Pridat) v mape nie su
    private readonly Dictionary<Radenie, Radenie> _originals = new(ReferenceEqualityComparer.Instance);

    // vsetky povodne radenia, ktore okno kedy zobrazilo - tie, ktore v okne uz nie su, sa pri ulozeni odstrania
    private readonly List<Radenie> _loaded = [];

    // radenia vlaku, s ktorym sa okno otvorilo (pri kopii radenia zdrojoveho vlaku)
    private List<Radenie> _own = [];

    /// <summary>
    ///     Radenia zobrazene v okne (kopie povodnych a nove).
    /// </summary>
    public BindingList<Radenie> Items { get; } = [];

    /// <summary>
    ///     Zobrazi kopie radeni vlaku, s ktorym sa okno otvorilo; <see cref="RestoreOwn" /> sa k nim vie vratit.
    /// </summary>
    /// <param name="radenia">radenia vlaku (povodne objekty)</param>
    public void LoadOwn(IEnumerable<Radenie> radenia)
    {
        _own = [.. radenia];
        Load(_own);
    }

    /// <summary>
    ///     Po prevzati radeni ineho vlaku vrati radenia vlaku, s ktorym sa okno otvorilo - cislo sa uz s tym vlakom
    ///     nezhoduje. Ak okno zobrazuje vlastne radenia, upravy v nich ostanu.
    /// </summary>
    public void RestoreOwn()
    {
        if (!Shows(_own))
            Load(_own);
    }

    /// <summary>
    ///     Nahradi zobrazene radenia kopiami radeni vlaku - pri prevzati radeni vlaku s rovnakym cislom.
    /// </summary>
    /// <param name="radenia">radenia vlaku (povodne objekty)</param>
    public void Load(IEnumerable<Radenie> radenia)
    {
        Items.Clear();
        _originals.Clear();

        foreach (var original in radenia)
        {
            var copy = Clone(original);
            _originals[copy] = original;
            Items.Add(copy);

            if (!_loaded.Contains(original))
                _loaded.Add(original);
        }
    }

    /// <summary>
    ///     Ci okno zobrazuje (kopie) prave tieto radenia - vtedy ich netreba preberat znova a zahodit upravy.
    /// </summary>
    /// <param name="radenia">radenia ineho vlaku</param>
    public bool Shows(IEnumerable<Radenie> radenia)
    {
        var shown = _originals.Values.ToHashSet(ReferenceEqualityComparer.Instance);
        var other = radenia.ToHashSet(ReferenceEqualityComparer.Instance);
        return shown.SetEquals(other);
    }

    /// <summary>
    ///     Zapise upravy do grafikonu a zosuladi <see cref="Train.Radenia" /> vlakov s <paramref name="globalRadenia" />.
    /// </summary>
    /// <remarks>
    ///     Povodne radenie sa prepise, len ak po ulozeni nepatri inemu vlaku - napr. pri kopii vlaku s inym cislom
    ///     alebo pri zmene cisla jednej varianty zostava vlaku s povodnym cislom a ukladany vlak dostane novy objekt.
    /// </remarks>
    /// <param name="train">ukladany vlak (uz s novym cislom); nemusi byt v <paramref name="trains" /></param>
    /// <param name="globalRadenia">vsetky radenia grafikonu (<c>GlobData.Radenia</c>)</param>
    /// <param name="trains">vlaky grafikonu</param>
    public void Commit(Train train, List<Radenie> globalRadenia, IEnumerable<Train> trains)
    {
        var number = train.Number;
        var all = trains.Where(t => !ReferenceEquals(t, train)).ToList();
        var others = all.Select(t => t.Number).ToHashSet();
        all.Add(train);

        bool KeptByOthers(Radenie r) => r.CisloVlaku != number && others.Contains(r.CisloVlaku);

        var numbers = _loaded.Select(r => r.CisloVlaku).Append(number).ToHashSet();

        var result = new List<Radenie>();
        foreach (var item in Items)
        {
            var rad = item;
            if (_originals.TryGetValue(item, out var original) && !KeptByOthers(original))
            {
                CopyTo(item, original);
                rad = original;
            }
            else if (original != null)
            {
                // povodne radenie ostava inemu vlaku - ukladany vlak dostane vlastny objekt
                rad = Clone(item);
            }

            rad.CisloVlaku = number;
            result.Add(rad);
            if (!globalRadenia.Contains(rad))
                globalRadenia.Add(rad);
        }

        foreach (var r in _loaded)
            if (!result.Contains(r) && !KeptByOthers(r))
                globalRadenia.Remove(r);

        // rovnako ako pri nacitani grafikonu (TxtParser.ReadTrains)
        foreach (var vlak in all.Where(t => numbers.Contains(t.Number)))
        {
            vlak.Radenia.Clear();
            vlak.Radenia.AddRange(globalRadenia.Where(r => r.CisloVlaku == vlak.Number));
        }
    }

    private static Radenie Clone(Radenie source)
    {
        var copy = new Radenie();
        CopyTo(source, copy);
        return copy;
    }

    private static void CopyTo(Radenie source, Radenie target)
    {
        target.ZacPlatnosti = source.ZacPlatnosti;
        target.KonPlatnosti = source.KonPlatnosti;
        target.DatObm = source.DatObm;
        target.Text = source.Text;
        target.DestStation = source.DestStation;
        target.CisloVlaku = source.CisloVlaku;
        target.Sounds = [.. source.Sounds];
        target.ChosenReports = source.ChosenReports
            .Select(chosen => new ChosenReportType { Type = chosen.Type, Variants = [.. chosen.Variants] }).ToList();
    }
}
