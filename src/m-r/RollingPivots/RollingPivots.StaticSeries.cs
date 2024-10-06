namespace Skender.Stock.Indicators;

// ROLLING PIVOT POINTS (SERIES)

public static partial class Indicator
{
    private static List<RollingPivotsResult> CalcRollingPivots<TQuote>(
        this List<TQuote> quotesList,
        int windowPeriods,
        int offsetPeriods,
        PivotPointType pointType)
        where TQuote : IQuote
    {
        // check parameter arguments
        RollingPivots.Validate(windowPeriods, offsetPeriods);

        // initialize
        int length = quotesList.Count;
        List<RollingPivotsResult> results = new(length);

        // roll through source values
        for (int i = 0; i < length; i++)
        {
            TQuote q = quotesList[i];

            RollingPivotsResult r;

            if (i >= windowPeriods + offsetPeriods)
            {
                // window values
                int s = i - windowPeriods - offsetPeriods;
                TQuote hi = quotesList[s];

                decimal windowHigh = hi.High;
                decimal windowLow = hi.Low;
                decimal windowClose = quotesList[i - offsetPeriods - 1].Close;

                for (int p = s; p <= i - offsetPeriods - 1; p++)
                {
                    TQuote d = quotesList[p];
                    windowHigh = d.High > windowHigh ? d.High : windowHigh;
                    windowLow = d.Low < windowLow ? d.Low : windowLow;
                }

                // pivot points
                WindowPoint wp = GetPivotPoint(
                        pointType, q.Open, windowHigh, windowLow, windowClose);

                r = new() {

                    Timestamp = q.Timestamp,

                    // pivot point
                    PP = wp.PP,

                    // support
                    S1 = wp.S1,
                    S2 = wp.S2,
                    S3 = wp.S3,
                    S4 = wp.S4,

                    // resistance
                    R1 = wp.R1,
                    R2 = wp.R2,
                    R3 = wp.R3,
                    R4 = wp.R4
                };
            }
            else
            {
                r = new() { Timestamp = q.Timestamp };
            }

            results.Add(r);
        }

        return results;
    }
}