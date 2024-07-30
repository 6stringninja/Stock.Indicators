namespace StaticSeries;

[TestClass]
public class McGinleyDynamic : StaticSeriesTestBase
{
    [TestMethod]
    public override void Standard()
    {
        IReadOnlyList<DynamicResult> results = Quotes
            .GetDynamic(14);

        // assertions
        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(501, results.Count(x => x.Dynamic != null));

        // sample values
        DynamicResult r1 = results[1];
        Assert.AreEqual(212.9465, r1.Dynamic.Round(4));

        DynamicResult r25 = results[25];
        Assert.AreEqual(215.4801, r25.Dynamic.Round(4));

        DynamicResult r250 = results[250];
        Assert.AreEqual(256.0554, r250.Dynamic.Round(4));

        DynamicResult r501 = results[501];
        Assert.AreEqual(245.7356, r501.Dynamic.Round(4));
    }

    [TestMethod]
    public void UseReusable()
    {
        IReadOnlyList<DynamicResult> results = Quotes
            .Use(CandlePart.Close)
            .GetDynamic(20);

        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(501, results.Count(x => x.Dynamic != null));
        Assert.AreEqual(0, results.Count(x => x.Dynamic is double.NaN));
    }

    [TestMethod]
    public void Chainee()
    {
        IReadOnlyList<DynamicResult> results = Quotes
            .GetSma(10)
            .GetDynamic(14);

        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(492, results.Count(x => x.Dynamic != null));
    }

    [TestMethod]
    public void Chainor()
    {
        IReadOnlyList<SmaResult> results = Quotes
            .GetDynamic(14)
            .GetSma(10);

        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(492, results.Count(x => x.Sma != null));
    }

    [TestMethod]
    public override void BadData()
    {
        IReadOnlyList<DynamicResult> r = BadQuotes
            .GetDynamic(15);

        Assert.AreEqual(502, r.Count);
        Assert.AreEqual(0, r.Count(x => x.Dynamic is double.NaN));
    }

    [TestMethod]
    public override void NoQuotes()
    {
        IReadOnlyList<DynamicResult> r0 = Noquotes
            .GetDynamic(14);

        Assert.AreEqual(0, r0.Count);

        IReadOnlyList<DynamicResult> r1 = Onequote
            .GetDynamic(14);

        Assert.AreEqual(1, r1.Count);
    }

    [TestMethod]
    public void Exceptions()
    {
        // bad lookback period
        Assert.ThrowsException<ArgumentOutOfRangeException>(()
            => Quotes.GetDynamic(0));

        // bad k-factor
        Assert.ThrowsException<ArgumentOutOfRangeException>(()
            => Quotes.GetDynamic(14, 0));
    }
}