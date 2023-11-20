namespace Skender.Stock.Indicators;

// VOLATILITY SYSTEM/STOP (STREAMING)

public partial class VolatilityStop
{
    // TBD: constructor
    public VolatilityStop()
    {
        Initialize();
    }

    // TBD: PROPERTIES

    // STATIC METHODS

    // parameter validation
    internal static void Validate(
        int lookbackPeriods,
        double multiplier)
    {
        // check parameter arguments
        if (lookbackPeriods <= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(lookbackPeriods), lookbackPeriods,
                "Lookback periods must be greater than 1 for Volatility Stop.");
        }

        if (multiplier <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(multiplier), multiplier,
                "ATR Multiplier must be greater than 0 for Volatility Stop.");
        }
    }

    // TBD: increment  calculation
    internal static double Increment() => throw new NotImplementedException();

    // NON-STATIC METHODS

    // handle quote arrival
    public virtual void OnNext((DateTime Date, double Value) value)
    {
    }

    // TBD: initialize with existing quote cache
    private void Initialize() => throw new NotImplementedException();
}
