using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stock.Domain;
using Stock.Infrastructure.YahooFinanceApi;

// https://github.com/zkavtaskin/Domain-Driven-Design-Example

namespace Stock
{
    public class StockManager
    {
        public Dictionary<string, ShareData> Shares { get; set; } = new();

        /// You should be able to query data from various markets including US, HK, TW
        /// The startTime & endTime here defaults to EST timezone
        public async Task<Dictionary<string,IReadOnlyList<Domain.Candle>>> GetHistoricalData(DateTime from, DateTime to, Domain.Period period = Domain.Period.Daily)
        {
            var result = new Dictionary<string, IReadOnlyList<Domain.Candle>>();
            foreach (var d in Shares.Keys)
            {
                var shareData = await Yahoo.GetHistoricalAsync(d, from, to, (Infrastructure.YahooFinanceApi.Period)period);
                //result.Add(d, shareData);
            }
            return result;
        }
    }
}
