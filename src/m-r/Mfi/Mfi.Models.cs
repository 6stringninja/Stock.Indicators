namespace Skender.Stock.Indicators;

[Serializable]
public sealed record class MfiResult : IReusableResult
{
    public DateTime Timestamp { get; set; }
    public double? Mfi { get; set; }

    double IReusableResult.Value => Mfi.Null2NaN();
}
