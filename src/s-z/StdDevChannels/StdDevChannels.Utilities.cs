namespace Skender.Stock.Indicators;

public static partial class Indicator
{
    // CONDENSE (REMOVE null results)
    /// <inheritdoc cref="ReusableUtility.Condense{T}(IEnumerable{T})"/>
    public static IEnumerable<StdDevChannelsResult> Condense(
        this IEnumerable<StdDevChannelsResult> results)
    {
        List<StdDevChannelsResult> resultsList = results
            .ToList();

        resultsList
            .RemoveAll(match: x =>
               x.UpperChannel is null
            && x.LowerChannel is null
            && x.Centerline is null
            && !x.BreakPoint);

        return resultsList.ToSortedList();
    }

    // remove recommended periods
    /// <inheritdoc cref="ReusableUtility.RemoveWarmupPeriods{T}(IEnumerable{T})"/>
    public static IEnumerable<StdDevChannelsResult> RemoveWarmupPeriods(
        this IEnumerable<StdDevChannelsResult> results)
    {
        int removePeriods = results
            .ToList()
            .FindIndex(x => x.UpperChannel != null || x.LowerChannel != null);

        return results.Remove(removePeriods);
    }
}
