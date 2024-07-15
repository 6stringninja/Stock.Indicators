namespace Skender.Stock.Indicators;

public static partial class Indicator
{
    // CONDENSE (REMOVE null results)
    /// <inheritdoc cref="ReusableUtility.Condense{T}(IEnumerable{T})"/>
    public static IEnumerable<IchimokuResult> Condense(
        this IEnumerable<IchimokuResult> results)
    {
        List<IchimokuResult> resultsList = results
            .ToList();

        resultsList
            .RemoveAll(match: x =>
                   x.TenkanSen is null
                && x.KijunSen is null
                && x.SenkouSpanA is null
                && x.SenkouSpanB is null
                && x.ChikouSpan is null);

        return resultsList.ToSortedList();
    }
}
