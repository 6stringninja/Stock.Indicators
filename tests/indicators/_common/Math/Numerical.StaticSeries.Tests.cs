namespace Utilities;

[TestClass]
public class Numericals : TestBase
{
    private readonly double[] _closePrice = LongishQuotes
        .Select(x => (double)x.Close)
        .ToArray();

    private readonly double[] _x = [1, 2, 3, 4, 5];
    private readonly double[] _y = [0, 0, 0, 0];

    [TestMethod]
    public void StdDev()
    {
        double sd = _closePrice.StdDev();

        Assert.AreEqual(633.932098287, Math.Round(sd, 9));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException), "Null parameter.")]
    public void StdDevNull() => Numerical.StdDev(null);

    [TestMethod]
    public void Slope()
    {
        double s = Numerical.Slope(_x, _x);

        Assert.AreEqual(1d, s);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException), "Null X parameter.")]
    public void SlopeXnull() => Numerical.Slope(null, _x);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException), "Null Y parameter.")]
    public void SlopeYnull() => Numerical.Slope(_x, null);

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "X and Y different lengths.")]
    public void SlopeMismatch() => Numerical.Slope(_x, _y);

    [TestMethod]
    public void RoundDownDate()
    {
        TimeSpan interval = PeriodSize.OneHour.ToTimeSpan();
        DateTime evDate = DateTime.Parse("2020-12-15 09:35:45", englishCulture);

        DateTime rnDate = evDate.RoundDown(interval);
        DateTime exDate = DateTime.Parse("2020-12-15 09:00:00", englishCulture);

        Assert.AreEqual(exDate, rnDate);
    }

    [TestMethod]
    public void ToTimeSpan()
    {
        Assert.AreEqual(PeriodSize.OneMinute.ToTimeSpan(), TimeSpan.FromMinutes(1));
        Assert.AreEqual(PeriodSize.TwoMinutes.ToTimeSpan(), TimeSpan.FromMinutes(2));
        Assert.AreEqual(PeriodSize.ThreeMinutes.ToTimeSpan(), TimeSpan.FromMinutes(3));
        Assert.AreEqual(PeriodSize.FiveMinutes.ToTimeSpan(), TimeSpan.FromMinutes(5));
        Assert.AreEqual(PeriodSize.FifteenMinutes.ToTimeSpan(), TimeSpan.FromHours(0.25));
        Assert.AreEqual(PeriodSize.ThirtyMinutes.ToTimeSpan(), TimeSpan.FromHours(0.5));
        Assert.AreEqual(PeriodSize.OneHour.ToTimeSpan(), TimeSpan.FromMinutes(60));
        Assert.AreEqual(PeriodSize.TwoHours.ToTimeSpan(), TimeSpan.FromHours(2));
        Assert.AreEqual(PeriodSize.FourHours.ToTimeSpan(), TimeSpan.FromHours(4));
        Assert.AreEqual(PeriodSize.Day.ToTimeSpan(), TimeSpan.FromHours(24));
        Assert.AreEqual(PeriodSize.Week.ToTimeSpan(), TimeSpan.FromDays(7));

        Assert.AreEqual(PeriodSize.Month.ToTimeSpan(), TimeSpan.Zero);
    }
}