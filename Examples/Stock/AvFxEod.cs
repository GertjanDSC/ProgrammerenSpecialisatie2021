using System;

namespace MarketData.Models
{
    public class AvFxEod
    {
        public string Ticker { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
    }
}
