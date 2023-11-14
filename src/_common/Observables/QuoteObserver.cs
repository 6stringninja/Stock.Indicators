using System.Diagnostics.CodeAnalysis;

namespace Skender.Stock.Indicators;

// OBSERVER of QUOTES (BOILERPLATE)
public abstract class QuoteObserver : IObserver<Quote>
{
    // fields
    private IDisposable? unsubscriber;

    // properties
    internal QuoteProvider? QuoteSupplier { get; set; }

    // methods
    public virtual void Subscribe()
        => unsubscriber = QuoteSupplier != null
            ? QuoteSupplier.Subscribe(this)
            : throw new ArgumentNullException(nameof(QuoteSupplier));

    public virtual void OnCompleted() => Unsubscribe();

    public virtual void OnError(Exception error) => throw error;

    [ExcludeFromCodeCoverage]
    public virtual void OnNext(Quote value)
    {
        // » handle new quote with override in observer
    }

    public virtual void Unsubscribe() => unsubscriber?.Dispose();
}
