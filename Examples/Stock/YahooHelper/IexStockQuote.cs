using System;

namespace Stock.YahooHelper
{
    public class IexStockQuote
    {
        public string Ticker { get; set; }
        public decimal Open { get; set; }
        public DateTime OpenTime { get; set; }
        public decimal Close { get; set; }
        public DateTime CloseTime { get; set; }
        public decimal LatestPrice { get; set; }
        public DateTime LatestTime { get; set; }
        public DateTime LatestUpdateTime { get; set; }
        public double LatestVolume { get; set; }
        public decimal DelayedPrice { get; set; }
        public DateTime DelayedPriceTime { get; set; }
        public decimal PreviousClose { get; set; }
        public decimal IexRealTimePrice { get; set; }
        public double IexRealTimeSize { get; set; }
        public DateTime IexLastUpdated { get; set; }
        public decimal IexBidPrice { get; set; }
        public decimal IexBidSize { get; set; }
        public decimal IexAskPrice { get; set; }
        public decimal IexAskSize { get; set; }
        public double Change { get; set; }
        public double ChangePercent { get; set; }
        public double MarketCap { get; set; }
        public double PeRatio { get; set; }
        public decimal Week52High { get; set; }
        public decimal Week52Low { get; set; }
        public double YtdChange { get; set; }
    }
}
