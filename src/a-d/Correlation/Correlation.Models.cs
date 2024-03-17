namespace Skender.Stock.Indicators;

[Serializable]
public sealed record class CorrResult : IReusableResult
{
    public DateTime Timestamp { get; set; }
    public double? VarianceA { get; set; }
    public double? VarianceB { get; set; }
    public double? Covariance { get; set; }
    public double? Correlation { get; set; }
    public double? RSquared { get; set; }

    double IReusableResult.Value => Correlation.Null2NaN();
}
