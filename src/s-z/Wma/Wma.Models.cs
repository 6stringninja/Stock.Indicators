namespace Skender.Stock.Indicators;

public sealed class WmaResult : ResultBase, IReusableResult
{
    public WmaResult(DateTime date)
    {
        Date = date;
    }

    public double? Wma { get; set; }

    double IReusableResult.Value => Wma.Null2NaN();
}
