namespace Skender.Stock.Indicators;

[Serializable]
public sealed record class MaEnvelopeResult : IResult
{
    public DateTime Timestamp { get; set; }
    public double? Centerline { get; set; }
    public double? UpperEnvelope { get; set; }
    public double? LowerEnvelope { get; set; }
}
