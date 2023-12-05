namespace Skender.Stock.Indicators;

public sealed class CciResult : ResultBase, IReusableResult
{
    public CciResult(DateTime date)
    {
        Date = date;
    }

    public double? Cci { get; set; }

    double IReusableResult.Value => Cci.Null2NaN();
}
