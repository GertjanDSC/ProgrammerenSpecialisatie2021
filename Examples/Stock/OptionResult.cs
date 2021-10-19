using System.Collections.Generic;

namespace MarketData.Models
{
    public class OptionResult
    {
        public List<string> Expirations { get; set; }
        public YahooStockData StockData { get; set; }
        public List<OptionData> CallOptions { get; set; }
        public List<OptionData> PutOptions { get; set; }
    }   
}
