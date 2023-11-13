namespace Skender.Stock.Indicators;

// EXPONENTIAL MOVING AVERAGE (COMMON)

/// <summary>See the <see href = "https://dotnet.stockindicators.dev/indicators/Ema/">
///  Stock Indicators for .NET online guide</see> for more information.</summary>
public partial class Ema : ChainProvider
{
    // parameter validation
    internal static void Validate(
        int lookbackPeriods)
    {
        // check parameter arguments
        if (lookbackPeriods <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lookbackPeriods), lookbackPeriods,
                "Lookback periods must be greater than 0 for EMA.");
        }
    }

    // increment calculations
    /// <include file='./info.xml' path='info/type[@name="increment-k"]/*' />
    ///
    public static double Increment(
        double k,
        double lastEma,
        double newPrice)
        => lastEma + (k * (newPrice - lastEma));

    /// <include file='./info.xml' path='info/type[@name="increment-lookback"]/*' />
    ///
    public static double Increment(
        int lookbackPeriods,
        double lastEma,
        double newPrice)
    {
        double k = 2d / (lookbackPeriods + 1);
        return Increment(k, lastEma, newPrice);
    }

    /// <include file='./info.xml' path='info/type[@name="increment-quote"]/*' />
    ///
    public EmaResult Increment<TQuote>(
        TQuote quote)
        where TQuote : IQuote
    {
        QuoteD q = quote.ToQuoteD();
        return Increment((q.Date, q.Close));
    }

    internal EmaResult Increment((DateTime Date, double Value) tp)
    {
        // initialize
        EmaResult r = new(tp.Date);
        int i = ProtectedResults.Count;

        // initialization periods
        if (i <= LookbackPeriods - 1)
        {
            SumValue += tp.Value;

            // set first value
            if (i == LookbackPeriods - 1)
            {
                r.Ema = (SumValue / LookbackPeriods).NaN2Null();
                SumValue = double.NaN;
            }

            ProtectedResults.Add(r);
            SendToChain(r);
            return r;
        }

        // check against last entry
        EmaResult last = ProtectedResults[i - 1];

        // add new
        if (r.Date > last.Date)
        {
            double lastEma = (last.Ema == null) ? double.NaN : (double)last.Ema;
            double ema = Increment(K, lastEma, tp.Value);

            r.Ema = ema.NaN2Null();

            ProtectedResults.Add(r);
            SendToChain(r);
            return r;
        }

        // update last
        else if (r.Date == last.Date)
        {
            // get prior last EMA
            EmaResult prior = ProtectedResults[i - 2];

            double priorEma = (prior.Ema == null) ? double.NaN : (double)prior.Ema;
            last.Ema = Increment(K, priorEma, tp.Value);

            SendToChain(last);
            return r;
        }

        // late arrival
        else
        {
            // heal
            throw new NotImplementedException();
        }
    }
}
