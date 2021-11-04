using System;

namespace Stock.Infrastructure.YahooFinanceApi
{
    public sealed class DividendTick : ITick
    {
        public DateTime DateTime { get; internal set; }

        public decimal Dividend { get; internal set; }
    }
}
