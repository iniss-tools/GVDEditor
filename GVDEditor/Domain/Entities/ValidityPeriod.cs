namespace GVDEditor.Domain.Entities;

/// <summary>
/// Obdobie platnosti (vratane oboch dni).
/// </summary>
/// <param name="From">prvy den platnosti</param>
/// <param name="To">posledny den platnosti</param>
public readonly record struct ValidityPeriod(DateOnly From, DateOnly To)
{
    /// <summary>
    /// Ci obdobie nie je obratene (koniec pred zaciatkom).
    /// </summary>
    public bool IsValid => From <= To;
}
