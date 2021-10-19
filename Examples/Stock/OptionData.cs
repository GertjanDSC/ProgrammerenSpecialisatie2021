using System;

namespace MarketData.Models
{
    public class OptionData
    {
        public string ContractSymbol { get; set; }
        public decimal Strike { get; set; }
        public string Currency { get; set; }
        public decimal LastPrice { get; set; }
        public decimal Change { get; set; }
        public int Volume { get; set; }
        public int OpenInterest { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public string ContractSize { get; set; }
        public string Expiration { get; set; }
        public string LastTradeDate { get; set; }
        public double ImpliedVol { get; set; }
        public Boolean InTheMoneny { get; set; }
    }
}
