namespace Skender.Stock.Indicators;

// USE / QUOTE CONVERTER (STREAM HUB)

#region hub interface
public interface IQuotePartHub
{
    CandlePart CandlePartSelection { get; }

    // TODO: consider renaming to IBarPartHub, with IQuote to IBar
}
#endregion

public class QuotePartHub<TQuote> : QuoteObserver<TQuote, QuotePart>,
    IReusableHub<TQuote, QuotePart>, IQuotePartHub
    where TQuote : IQuote
{
    #region constructors

    internal QuotePartHub(
        IQuoteProvider<TQuote> provider,
        CandlePart candlePart)
        : base(provider)
    {
        CandlePartSelection = candlePart;

        Reinitialize();
    }
    #endregion

    public CandlePart CandlePartSelection { get; }

    // METHODS

    internal override void Add(Act act, TQuote newIn, int? index)
    {
        // candidate result
        QuotePart r
            = newIn.ToQuotePart(CandlePartSelection);

        // save and send
        Motify(act, r, index);
    }

    public override string ToString()
        => $"QUOTE-PART({CandlePartSelection.ToString().ToUpperInvariant()})";
}