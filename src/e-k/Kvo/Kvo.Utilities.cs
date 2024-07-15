namespace Skender.Stock.Indicators;

public static partial class Indicator
{
    // remove recommended periods
    /// <inheritdoc cref="ReusableUtility.RemoveWarmupPeriods{T}(IEnumerable{T})"/>
    public static IEnumerable<KvoResult> RemoveWarmupPeriods(
        this IEnumerable<KvoResult> results)
    {
        int l = results
            .ToList()
            .FindIndex(x => x.Oscillator != null) - 1;

        return results.Remove(l + 150);
    }
}
