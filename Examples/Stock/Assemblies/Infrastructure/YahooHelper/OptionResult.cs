using Stock.Infrastructure.YahooHelper;
using System.Collections.Generic;

namespace Stock.YahooHelper
{
    public class OptionResult
    {
        public List<string> Expirations { get; set; }
        public YahooStockData StockData { get; set; }
        public List<OptionData> CallOptions { get; set; }
        public List<OptionData> PutOptions { get; set; }
    }
}
