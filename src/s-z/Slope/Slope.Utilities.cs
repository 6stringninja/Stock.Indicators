namespace Skender.Stock.Indicators;

public static partial class Indicator
{
    // remove recommended periods
    /// <inheritdoc cref="ReusableUtility.RemoveWarmupPeriods{T}(IEnumerable{T})"/>
    public static IEnumerable<SlopeResult> RemoveWarmupPeriods(
        this IEnumerable<SlopeResult> results)
    {
        int removePeriods = results
            .ToList()
            .FindIndex(x => x.Slope != null);

        return results.Remove(removePeriods);
    }
}
