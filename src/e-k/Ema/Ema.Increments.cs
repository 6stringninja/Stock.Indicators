namespace Skender.Stock.Indicators;

// EXPONENTIAL MOVING AVERAGE (INCREMENTING LIST)

/// <summary>
/// Exponential Moving Average (EMA)
/// from incremental reusable values.
/// </summary>
public class EmaInc : List<EmaResult>, IEma, IIncrementQuote, IIncrementReusable
{
    private readonly List<double> _buffer;

    public EmaInc(int lookbackPeriods)
    {
        Ema.Validate(lookbackPeriods);
        LookbackPeriods = lookbackPeriods;
        K = 2d / (lookbackPeriods + 1);

        _buffer = new(lookbackPeriods);
    }

    public int LookbackPeriods { get; init; }
    public double K { get; init; }

    public void Add(DateTime timestamp, double value)
    {
        // update buffer
        _buffer.Add(value);

        if (_buffer.Count > LookbackPeriods)
        {
            _buffer.RemoveAt(0);
        }

        // add nulls for incalculable periods
        if (Count < LookbackPeriods - 1)
        {
            base.Add(new EmaResult(timestamp));
            return;
        }

        // re/initialize
        if (this[^1].Ema is null)
        {
            double sum = 0;
            for (int i = 0; i < LookbackPeriods; i++)
            {
                sum += _buffer[i];
            }

            base.Add(new EmaResult(
                timestamp,
                sum / LookbackPeriods));

            return;
        }

        // calculate EMA normally
        base.Add(new EmaResult(
            timestamp,
            Ema.Increment(K, this[^1].Ema, value)));
    }

    public void Add(IReusable value)
        => Add(value.Timestamp, value.Value);

    public void Add(IReadOnlyList<IReusable> values)
    {
        for (int i = 0; i < values.Count; i++)
        {
            Add(values[i].Timestamp, values[i].Value);
        }
    }

    public void Add(IQuote quote)
        => Add(quote.Timestamp, quote.Value);

    public void Add(IReadOnlyList<IQuote> quotes)
    {
        for (int i = 0; i < quotes.Count; i++)
        {
            Add(quotes[i]);
        }
    }
}

/// <summary>
/// Exponential Moving Average (EMA)
/// from incremental primatives, without date context.
/// </summary>
/// <inheritdoc cref="IIncrementPrimitive"/>
public class EmaIncPrimitive : List<double?>, IEma, IIncrementPrimitive
{
    private readonly List<double> _buffer;

    public EmaIncPrimitive(int lookbackPeriods)
    {
        Ema.Validate(lookbackPeriods);
        LookbackPeriods = lookbackPeriods;
        K = 2d / (lookbackPeriods + 1);

        _buffer = new(lookbackPeriods);
    }

    public int LookbackPeriods { get; init; }
    public double K { get; init; }

    public void Add(double value)
    {
        // update buffer
        _buffer.Add(value);

        if (_buffer.Count > LookbackPeriods)
        {
            _buffer.RemoveAt(0);
        }

        // add nulls for incalculable periods
        if (Count < LookbackPeriods - 1)
        {
            base.Add(null);
            return;
        }

        // re/initialize
        if (this[^1] is null)
        {
            double sum = 0;
            for (int i = 0; i < LookbackPeriods; i++)
            {
                sum += _buffer[i];
            }

            base.Add(sum / LookbackPeriods);
            return;
        }

        // calculate EMA normally
        base.Add(Ema.Increment(K, this[^1], value));
    }

    public void Add(double[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            Add(values[i]);
        }
    }
}
