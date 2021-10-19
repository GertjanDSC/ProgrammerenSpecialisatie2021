using System;

namespace MarketData.Models
{
    public class YahooStockData
    {
        public string Ticker { get; set; }
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double CloseAdj { get; set; }
        public double Volume { get; set; }
    }
}
