namespace Skender.Stock.Indicators;

[Serializable]
public sealed record class ElderRayResult : IReusableResult
{
    public DateTime Timestamp { get; set; }
    public double? Ema { get; set; }
    public double? BullPower { get; set; }
    public double? BearPower { get; set; }

    double IReusableResult.Value => (BullPower + BearPower).Null2NaN();
}
