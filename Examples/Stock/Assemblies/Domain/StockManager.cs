using Stock.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stock.Domain
{
    public class StockManager
    {
        public IStockQuery StockQueryProvider { get; set; }

        public Dictionary<string, ShareData> Shares { get; set; } = new();

        public Dictionary<string, IReadOnlyList<Candle>> GetHistoricalData(DateTime from, DateTime to, Period period = Period.Daily)
        {
            Services.Logger.Debug("-> StockManager::GetHistoricalData");
            var result = new Dictionary<string, IReadOnlyList<Candle>>();
            foreach (var d in Shares.Keys)
            {
                var shareData = StockQueryProvider.GetHistoricalData(d, from, to, period);
                result.Add(d, shareData);
            }
            Services.Logger.Debug("<- StockManager::GetHistoricalData");
            return result;
        }

        public Dictionary<string, Security> GetSecurities()
        {
            Services.Logger.Debug("-> StockManager::GetSecurities");
            var result = new Dictionary<string, Security>();
            var shareData = StockQueryProvider.GetSecurities(Shares.Keys.ToArray());
            foreach (var d in shareData)
            {
                result.Add(d.Symbol, d);
            }
            Services.Logger.Debug("<- StockManager::GetSecurities");
            return result;
        }
    }
}
