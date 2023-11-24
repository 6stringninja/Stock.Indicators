namespace Skender.Stock.Indicators;

[Serializable]
public sealed class TrixResult : ResultBase, IReusableResult
{
    public TrixResult(DateTime date)
    {
        Date = date;
    }

    public double? Ema3 { get; set; }
    public double? Trix { get; set; }

    [Obsolete("Use a chained `results.GetSma(smaPeriods)` to generate a moving average signal.", false)]
    public double? Signal { get; set; }

    double IReusableResult.Value => Trix.Null2NaN();
}
