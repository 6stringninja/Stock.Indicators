namespace Skender.Stock.Indicators;

// DOUBLE EXPONENTIAL MOVING AVERAGE - DEMA (API)
public static partial class Indicator
{
    // SERIES, from TQuote
    /// <include file='./info.xml' path='info/*' />
    ///
    public static IEnumerable<DemaResult> GetDema<TQuote>(
        this IEnumerable<TQuote> quotes,
        int lookbackPeriods)
        where TQuote : IQuote => quotes
            .ToTuple(CandlePart.Close)
            .CalcDema(lookbackPeriods);

    // SERIES, from CHAIN
    public static IEnumerable<DemaResult> GetDema(
        this IEnumerable<IReusableResult> results,
        int lookbackPeriods) => results
            .ToTupleResult()
            .CalcDema(lookbackPeriods);

    // SERIES, from TUPLE
    public static IEnumerable<DemaResult> GetDema(
        this IEnumerable<(DateTime, double)> priceTuples,
        int lookbackPeriods) => priceTuples
            .ToSortedList()
            .CalcDema(lookbackPeriods);
}
