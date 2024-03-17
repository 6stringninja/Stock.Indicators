namespace Skender.Stock.Indicators;

[Serializable]
public sealed record class PmoResult : IReusableResult
{
    public DateTime Timestamp { get; set; }
    public double? Pmo { get; set; }
    public double? Signal { get; set; }

    double IReusableResult.Value => Pmo.Null2NaN();
}
