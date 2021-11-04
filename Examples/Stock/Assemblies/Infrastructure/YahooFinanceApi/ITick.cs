using System;

namespace Stock.Infrastructure.YahooFinanceApi
{
    public interface ITick
    {
        DateTime DateTime { get; }
    }
}
