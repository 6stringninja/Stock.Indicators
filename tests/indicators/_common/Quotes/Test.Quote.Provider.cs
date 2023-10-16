using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skender.Stock.Indicators;

namespace Tests.Common;

[TestClass]
public class QuoteProviderTests : TestBase
{
    [TestMethod]
    public void Standard()
    {
        List<Quote> quotesList = quotes
            .ToSortedList();

        int length = quotes.Count();

        // add base quotes
        QuoteProvider provider = new();
        provider.Add(quotesList.Take(200));

        // emulate incremental quotes
        for (int i = 200; i < length; i++)
        {
            Quote q = quotesList[i];
            provider.Add(q);
        }

        // assert same as original
        for (int i = 0; i < length; i++)
        {
            Quote o = quotesList[i];
            Quote q = provider.ProtectedQuotes[i];

            Assert.AreEqual(o, q);
        }

        // confirm public interface
        Assert.AreEqual(provider.ProtectedQuotes.Count, provider.Quotes.Count());

        // close observations
        provider.EndTransmission();
    }

    [TestMethod]
    public void LateArrival()
    {
        List<Quote> quotesList = quotes
            .ToSortedList();

        int length = quotes.Count();

        // add base quotes
        QuoteProvider provider = new();
        provider.Add(quotesList.Take(200));

        // emulate incremental quotes
        for (int i = 200; i < length; i++)
        {
            if (i == 100)
            {
                continue;
            }

            Quote q = quotesList[i];
            provider.Add(q);
        }

        Quote late = quotesList[100];
        provider.Add(late);

        // assert same as original
        for (int i = 0; i < length; i++)
        {
            Quote o = quotesList[i];
            Quote q = provider.ProtectedQuotes[i];

            Assert.AreEqual(o, q);
        }

        // close observations
        provider.EndTransmission();
    }

    [TestMethod]
    public void Exceptions()
    {
        // null quote added
        QuoteProvider provider = new();

        // overflow
        Assert.ThrowsException<OverflowException>(() =>
        {
            Quote quote = new()
            {
                Date = DateTime.Now
            };

            for (int i = 0; i <= 101; i++)
            {
                provider.Add(quote);
            }
        });

        // null quote
        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            Quote quote = null;
            provider.Add(quote);
        });

        // close observations
        provider.EndTransmission();
    }
}
