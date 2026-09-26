using ExControls;

namespace GVDEditor.Entities;

/// <summary>
///     Zoznam vlakov grafikonu pre tabulku hlavneho okna.
/// </summary>
/// <remarks>
///     Nazov vlaku je v grafikone kluc zvuku, tabulka vsak zobrazuje nazov zvuku - podla neho sa aj triedi.
/// </remarks>
public sealed class TrainBindingList : ExBindingList<Train>
{
    /// <summary>Vytvori prazdny zoznam vlakov.</summary>
    public TrainBindingList()
    {
    }

    /// <summary>Vytvori zoznam nad danymi vlakmi.</summary>
    public TrainBindingList(IList<Train> list) : base(list)
    {
    }

    /// <summary>Nazvy vlakov zo zvukovej banky; predvolene <see cref="GlobData.TrainNames" />.</summary>
    public IEnumerable<TrainName>? TrainNames { get; set; }

    /// <inheritdoc />
    protected override int OnComparison(object left, object right)
    {
        var names = TrainNames ?? GlobData.TrainNames;
        if (SortPropertyCore?.Name == nameof(Train.Name) && names != null && left is string l && right is string r)
            return StringComparer.CurrentCulture.Compare(TrainName.ToDisplay(names, l), TrainName.ToDisplay(names, r));

        return base.OnComparison(left, right);
    }
}
