namespace Skender.Stock.Indicators;

// AROON OSCILLATOR (SERIES)

public static partial class Indicator
{
    private static List<AroonResult> CalcAroon(
        this List<QuoteD> source,
        int lookbackPeriods)
    {
        // check parameter arguments
        Aroon.Validate(lookbackPeriods);

        // initialize
        int length = source.Count;
        List<AroonResult> results = new(length);

        // roll through source values
        for (int i = 0; i < length; i++)
        {
            QuoteD q = source[i];
            double? aroonUp = null;
            double? aroonDown = null;

            // add aroons
            if (i + 1 > lookbackPeriods)
            {
                double? lastHighPrice = 0;
                double? lastLowPrice = double.MaxValue;
                int lastHighIndex = 0;
                int lastLowIndex = 0;

                for (int p = i + 1 - lookbackPeriods - 1; p <= i; p++)
                {
                    QuoteD d = source[p];

                    if (d.High > lastHighPrice)
                    {
                        lastHighPrice = d.High;
                        lastHighIndex = p + 1;
                    }

                    if (d.Low < lastLowPrice)
                    {
                        lastLowPrice = d.Low;
                        lastLowIndex = p + 1;
                    }
                }

                aroonUp = 100d * (lookbackPeriods - (i + 1 - lastHighIndex)) / lookbackPeriods;
                aroonDown = 100d * (lookbackPeriods - (i + 1 - lastLowIndex)) / lookbackPeriods;
            }

            AroonResult r = new(
                Timestamp: q.Timestamp,
                AroonUp: aroonUp,
                AroonDown: aroonDown,
                Oscillator: aroonUp - aroonDown);

            results.Add(r);

        }

        return results;
    }
}