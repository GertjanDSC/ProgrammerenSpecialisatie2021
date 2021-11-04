using Stock.Domain.Interfaces;
using System;

// https://github.com/zkavtaskin/Domain-Driven-Design-Example

namespace Stock.Domain
{

    public class Candle : ITick
    {
        public DateTime DateTime { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public long Volume { get; set; }

        public decimal AdjustedClose { get; set; }
    }
}
