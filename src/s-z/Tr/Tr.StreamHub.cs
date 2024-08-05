namespace Skender.Stock.Indicators;

// TRUE RANGE (STREAM HUB)

public class TrHub<TIn>
    : ChainProvider<TIn, TrResult>
    where TIn : IQuote
{
    #region constructors

    internal TrHub(IQuoteProvider<TIn> provider)
        : base(provider)
    {
        Reinitialize();
    }
    #endregion

    // METHODS

    protected override (TrResult result, int index)
        ToCandidate(TIn item, int? indexHint)
    {
        int i = indexHint ?? ProviderCache.GetIndex(item, true);

        // skip first period
        if (i == 0)
        {
            return (new TrResult(item.Timestamp, null), i);
        }

        TIn prev = ProviderCache[i - 1];

        // candidate result
        TrResult r = new(
            item.Timestamp,
            Tr.Increment(
                (double)item.High,
                (double)item.Low,
                (double)prev.Close));

        return (r, i);
    }

    public override string ToString() => "TRUE RANGE";
}
