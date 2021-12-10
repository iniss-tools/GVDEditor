namespace GVDEditor.Entities;

/// <summary>
///     Reprezentuje typ vlaku.
/// </summary>
public sealed class TrainType
{
    /// <summary>
    ///     Konstruktor pre definovanie predvoleneho typu vlaku.
    /// </summary>
    /// <param name="key">Kluc typu vlaku.</param>
    public TrainType(string key)
    {
        Key = key;
        IsCustom = false;
        CategoryTrain = null;
        TextInTable = key;
    }

    /// <summary>
    ///     Konstruktor pre definovanie pouzivatelom definovaneho typu vlaku.
    /// </summary>
    /// <param name="categoryTrain"></param>
    /// <param name="key"></param>
    /// <param name="textInTable"></param>
    public TrainType(string categoryTrain, string key, string textInTable)
    {
        Key = key;
        IsCustom = true;
        CategoryTrain = categoryTrain;
        TextInTable = textInTable;
    }

    /// <summary>
    ///     Kluc typu vlaku.
    /// </summary>
    public string Key { get; }

    /// <summary>
    ///     Ci je pouzivatelom definovany.
    /// </summary>
    public bool IsCustom { get; }

    /// <summary>
    ///     Vrati text na tabuli, ktory sa ma zobrazovat na mieste typu vlaku.
    /// </summary>
    public string TextInTable { get; }

    /// <summary>
    ///     Kategoria typu vlaku.
    /// </summary>
    public string CategoryTrain { get; set; }

    /// <summary>
    ///     This.
    /// </summary>
    public TrainType This => this;

    /// <inheritdoc />
    public override string ToString() => Key;

    /// <summary>
    ///     Zisti, ci zadany retazec reprezentuje predvoleny typ vlaku.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool Validate(string type)
    {
        switch (type)
        {
            case "Os":
            case "MOs":
            case "Sp":
            case "R":
            case "Ex":
            case "EC":
            case "EN":
            case "IC":
            case "SC":
            case "Bus":
            case "Rn":
            case "Rp":
            case "Zr":
            case "REX":
            case "ER":
            case "Nákl":
            case "NZ":
            case "ICE":
            case "Sl":
            case "SPR":
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    ///     Vrati vsetky predvolene typy vlakov.
    /// </summary>
    /// <returns></returns>
    public static List<TrainType> GetDefaultValues()
    {
        var types = new List<TrainType>
        {
            new("Os"),
            new("R"),
            new("Zr"),
            new("IC"),
            new("EC"),
            new("ER"),
            new("EN"),
            new("Bus"),
            new("Ex"),
            new("REX"),
            new("Sp"),
            new("Rn"),
            new("Rp"),
            new("Nákl"),
            new("NZ"),
            new("ICE"),
            new("Sl"),
            new("SPR")
        };

        return types;
    }

    /// <summary>
    ///     Porovna typy vlakov
    /// </summary>
    /// <param name="t1"></param>
    /// <param name="t2"></param>
    /// <returns></returns>
    public static bool Equals(TrainType t1, TrainType t2)
    {
        if (t1 is null && t2 is null)
            return true;
        return t1 is not null && t2 is not null &&
               t1.Key == t2.Key && t1.IsCustom == t2.IsCustom && t1.CategoryTrain == t2.CategoryTrain && t1.TextInTable == t2.TextInTable;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is TrainType t1 && t1.Key == Key && t1.IsCustom == IsCustom &&
               t1.CategoryTrain == CategoryTrain && t1.TextInTable == TextInTable;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Key != null ? Key.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ IsCustom.GetHashCode();
            hashCode = (hashCode * 397) ^ (TextInTable != null ? TextInTable.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (CategoryTrain != null ? CategoryTrain.GetHashCode() : 0);
            return hashCode;
        }
    }

    /// <summary>
    ///     Returns a value that indicates whether the values of two <see cref="T:GVDEditor.Entities.TrainType" /> objects
    ///     are equal.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    ///     true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise,
    ///     false.
    /// </returns>
    public static bool operator ==(TrainType left, TrainType right)
    {
        return Equals(left, right);
    }

    /// <summary>
    ///     Returns a value that indicates whether two <see cref="T:GVDEditor.Entities.TrainType" /> objects have
    ///     different values.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(TrainType left, TrainType right)
    {
        return !Equals(left, right);
    }
}