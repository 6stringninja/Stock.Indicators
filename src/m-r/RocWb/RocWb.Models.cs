namespace Skender.Stock.Indicators;

[Serializable]
public sealed record class RocWbResult : IReusableResult
{
    public DateTime Timestamp { get; set; }
    public double? Roc { get; set; }
    public double? RocEma { get; set; }
    public double? UpperBand { get; set; }
    public double? LowerBand { get; set; }

    double IReusableResult.Value => Roc.Null2NaN();
}