namespace Skender.Stock.Indicators;

// QUOTE PROVIDER

public class QuoteProvider<TQuote>
    : SeriesCache<TQuote>, IQuoteProvider<TQuote>
    where TQuote : IQuote, new()
{
    // fields
    private readonly List<IObserver<(Act, TQuote)>> observers;

    // constructor
    public QuoteProvider()
    {
        observers = [];

        Initialize();
    }

    // PROPERTIES

    public IEnumerable<TQuote> Quotes => Cache;

    // METHODS

    // string label
    public override string ToString()
        => $"Quote Provider ({Cache.Count} items)";

    // add one
    public Act Add(TQuote quote)
    {
        try
        {
            Act act = CacheWithAnalysis(quote);

            NotifyObservers((act, quote));

            return act;
        }
        catch (OverflowException ox)
        {
            EndTransmission();

            string msg = "A repeated Quote update exceeded the 100 attempt threshold. "
                        + "Check and remove circular chains or check your Quote provider."
                        + "Provider terminated.";

            throw new OverflowException(msg, ox);
        }
    }

    // add many
    public void Add(IEnumerable<TQuote> quotes)
    {
        List<TQuote> added = quotes
            .ToSortedList();

        for (int i = 0; i < added.Count; i++)
        {
            Add(added[i]);
        }
    }

    // delete one
    public Act Delete(TQuote quote)
    {
        try
        {
            Act act = PurgeWithAnalysis(quote);
            NotifyObservers((act, quote));
            return act;
        }
        catch (OverflowException ox)
        {
            EndTransmission();

            string msg = "A repeated Quote delete exceeded the 100 attempt threshold. "
                        + "Check and remove circular chains or check your Quote provider."
                        + "Provider terminated.";

            throw new OverflowException(msg, ox);
        }

    }

    // re/initialize is graceful erase only for quote provider
    public void Initialize() => ClearCache();

    // subscribe observer
    public IDisposable Subscribe(IObserver<(Act, TQuote)> observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }

        return new Unsubscriber(observers, observer);
    }

    // unsubscribe all observers
    public override void EndTransmission()
    {
        foreach (IObserver<(Act, TQuote)> obs in observers.ToArray())
        {
            if (observers.Contains(obs))
            {
                obs.OnCompleted();
            }
        }

        observers.Clear();
    }

    // delete cache, gracefully
    internal override void ClearCache(int fromIndex, int toIndex)
    {
        // delete and deliver instruction,
        // in reverse order to prevent recompositions
        for (int i = Cache.Count - 1; i > 0; i--)
        {
            TQuote q = Cache[i];
            Act act = CacheResultPerAction(Act.Delete, q);
            NotifyObservers((act, q));
        }

        // note: there is no auto-rebuild option since the
        // quote provider is a top level external entry point.
        // The using system will need to handle resupply with Add().
    }

    internal override void RebuildCache(DateTime fromDate, int offset)
        => throw new InvalidOperationException();

    internal override void RebuildCache(int fromIndex, int offset)
        => throw new InvalidOperationException();

    // notify observers
    private void NotifyObservers((Act act, TQuote quote) quoteMessage)
    {
        // do not propogate "do nothing" acts
        if (quoteMessage.act == Act.DoNothing)
        {
            return;
        }

        // send to subscribers
        List<IObserver<(Act, TQuote)>> obsList = [.. observers];

        for (int i = 0; i < obsList.Count; i++)
        {
            IObserver<(Act, TQuote)> obs = obsList[i];
            obs.OnNext(quoteMessage);
        }
    }

    // unsubscriber
    private class Unsubscriber(
        List<IObserver<(Act, TQuote)>> observers,
        IObserver<(Act, TQuote)> observer) : IDisposable
    {
        // can't mutate and iterate on same list, make copy
        private readonly List<IObserver<(Act, TQuote)>> observers = observers;
        private readonly IObserver<(Act, TQuote)> observer = observer;

        // remove single observer
        public void Dispose()
        {
            if (observer != null && observers.Contains(observer))
            {
                observers.Remove(observer);
            }
        }
    }
}
