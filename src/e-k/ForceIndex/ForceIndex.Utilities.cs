namespace Skender.Stock.Indicators;

public static partial class Indicator
{
    // remove recommended periods
    /// <inheritdoc cref="ReusableUtility.RemoveWarmupPeriods{T}(IEnumerable{T})"/>
    public static IEnumerable<ForceIndexResult> RemoveWarmupPeriods(
        this IEnumerable<ForceIndexResult> results)
    {
        int n = results
            .ToList()
            .FindIndex(x => x.ForceIndex != null);

        return results.Remove(n + 100);
    }
}
