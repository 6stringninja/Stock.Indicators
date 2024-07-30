namespace Skender.Stock.Indicators;

// HEIKIN-ASHI (SERIES)

public static partial class Indicator
{
    private static List<HeikinAshiResult> CalcHeikinAshi<TQuote>(
        this List<TQuote> quotesList)
        where TQuote : IQuote
    {
        // initialize
        int length = quotesList.Count;
        List<HeikinAshiResult> results = new(length);

        decimal prevOpen = decimal.MinValue;
        decimal prevClose = decimal.MinValue;

        if (length > 0)
        {
            TQuote q = quotesList[0];
            prevOpen = q.Open;
            prevClose = q.Close;
        }

        // roll through source values
        for (int i = 0; i < length; i++)
        {
            TQuote q = quotesList[i];

            // close
            decimal close = (q.Open + q.High + q.Low + q.Close) / 4;

            // open
            decimal open = (prevOpen + prevClose) / 2;

            // high
            decimal[] arrH = [q.High, open, close];
            decimal high = arrH.Max();

            // low
            decimal[] arrL = [q.Low, open, close];
            decimal low = arrL.Min();

            results.Add(new HeikinAshiResult(
                Timestamp: q.Timestamp,
                Open: open,
                High: high,
                Low: low,
                Close: close,
                Volume: q.Volume));

            // save for next iteration
            prevOpen = open;
            prevClose = close;
        }

        return results;
    }
}