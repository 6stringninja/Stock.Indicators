namespace Skender.Stock.Indicators;

public static partial class Indicator
{
    // remove recommended periods
    /// <inheritdoc cref="ReusableUtility.RemoveWarmupPeriods{T}(IEnumerable{T})"/>
    public static IEnumerable<ElderRayResult> RemoveWarmupPeriods(
        this IEnumerable<ElderRayResult> results)
    {
        int n = results
          .ToList()
          .FindIndex(x => x.BullPower != null) + 1;

        return results.Remove(n + 100);
    }
}
