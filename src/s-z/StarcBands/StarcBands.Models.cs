namespace Skender.Stock.Indicators;

[Serializable]
public sealed record class StarcBandsResult : IResult
{
    public DateTime Timestamp { get; set; }
    public double? UpperBand { get; set; }
    public double? Centerline { get; set; }
    public double? LowerBand { get; set; }
}
