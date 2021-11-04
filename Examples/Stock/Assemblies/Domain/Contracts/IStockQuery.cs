using System;
using System.Collections.Generic;

// https://github.com/zkavtaskin/Domain-Driven-Design-Example

namespace Stock.Domain.Contracts
{
    public interface IStockQuery
    {
        /// You should be able to query data from various markets including US, HK, TW
        /// The startTime & endTime here defaults to EST timezone
        IReadOnlyList<Candle> GetHistoricalData(string tick, DateTime from, DateTime to, Period period = Period.Daily);
        IReadOnlyList<Security> GetSecurities(string[] shares);
    }
}
