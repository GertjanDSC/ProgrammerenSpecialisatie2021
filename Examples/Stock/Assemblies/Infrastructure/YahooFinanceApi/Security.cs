using System.Collections.Generic;

namespace Stock.Infrastructure.YahooFinanceApi
{
    public class Security
    {
        public IReadOnlyDictionary<string, dynamic> Fields { get; private set; }

        // ctor
        internal Security(IReadOnlyDictionary<string, dynamic> fields) => Fields = fields;

        public dynamic this[string fieldName] => Fields[fieldName];
        public dynamic this[Field field] => Fields[field.ToString()];

        // Security.cs: This list was generated automatically. These names and types have been defined by Yahoo.
        public double Ask => this["Ask"];
        public long AskSize => this["AskSize"];
        public long AverageDailyVolume10Day => this["AverageDailyVolume10Day"];
        public long AverageDailyVolume3Month => this["AverageDailyVolume3Month"];
        public double Bid => this["Bid"];
        public long BidSize => this["BidSize"];
        public double BookValue => this["BookValue"];
        public string Currency => this["Currency"];
        public long DividendDate => this["DividendDate"];
        public long EarningsTimestamp => this["EarningsTimestamp"];
        public long EarningsTimestampEnd => this["EarningsTimestampEnd"];
        public long EarningsTimestampStart => this["EarningsTimestampStart"];
        public double EpsForward => this["EpsForward"];
        public double EpsTrailingTwelveMonths => this["EpsTrailingTwelveMonths"];
        public string Exchange => this["Exchange"];
        public long ExchangeDataDelayedBy => this["ExchangeDataDelayedBy"];
        public string ExchangeTimezoneName => this["ExchangeTimezoneName"];
        public string ExchangeTimezoneShortName => this["ExchangeTimezoneShortName"];
        public double FiftyDayAverage => this["FiftyDayAverage"];
        public double FiftyDayAverageChange => this["FiftyDayAverageChange"];
        public double FiftyDayAverageChangePercent => this["FiftyDayAverageChangePercent"];
        public double FiftyTwoWeekHigh => this["FiftyTwoWeekHigh"];
        public double FiftyTwoWeekHighChange => this["FiftyTwoWeekHighChange"];
        public double FiftyTwoWeekHighChangePercent => this["FiftyTwoWeekHighChangePercent"];
        public double FiftyTwoWeekLow => this["FiftyTwoWeekLow"];
        public double FiftyTwoWeekLowChange => this["FiftyTwoWeekLowChange"];
        public double FiftyTwoWeekLowChangePercent => this["FiftyTwoWeekLowChangePercent"];
        public string FinancialCurrency => this["FinancialCurrency"];
        public double ForwardPE => this["ForwardPE"];
        public string FullExchangeName => this["FullExchangeName"];
        public long GmtOffSetMilliseconds => this["GmtOffSetMilliseconds"];
        public string Language => this["Language"];
        public string LongName => this["LongName"];
        public string Market => this["Market"];
        public long MarketCap => this["MarketCap"];
        public string MarketState => this["MarketState"];
        public string MessageBoardId => this["MessageBoardId"];
        public long PriceHint => this["PriceHint"];
        public double PriceToBook => this["PriceToBook"];
        public string QuoteSourceName => this["QuoteSourceName"];
        public string QuoteType => this["QuoteType"];
        public double RegularMarketChange => this["RegularMarketChange"];
        public double RegularMarketChangePercent => this["RegularMarketChangePercent"];
        public double RegularMarketDayHigh => this["RegularMarketDayHigh"];
        public double RegularMarketDayLow => this["RegularMarketDayLow"];
        public double RegularMarketOpen => this["RegularMarketOpen"];
        public double RegularMarketPreviousClose => this["RegularMarketPreviousClose"];
        public double RegularMarketPrice => this["RegularMarketPrice"];
        public long RegularMarketTime => this["RegularMarketTime"];
        public long RegularMarketVolume => this["RegularMarketVolume"];
        public double PostMarketChange => this["PostMarketChange"];
        public double PostMarketChangePercent => this["PostMarketChangePercent"];
        public double PostMarketPrice => this["PostMarketPrice"];
        public long PostMarketTime => this["PostMarketTime"];
        public long SharesOutstanding => this["SharesOutstanding"];
        public string ShortName => this["ShortName"];
        public long SourceInterval => this["SourceInterval"];
        public string Symbol => this["Symbol"];
        public bool Tradeable => this["Tradeable"];
        public double TrailingAnnualDividendRate => this["TrailingAnnualDividendRate"];
        public double TrailingAnnualDividendYield => this["TrailingAnnualDividendYield"];
        public double TrailingPE => this["TrailingPE"];
        public double TwoHundredDayAverage => this["TwoHundredDayAverage"];
        public double TwoHundredDayAverageChange => this["TwoHundredDayAverageChange"];
        public double TwoHundredDayAverageChangePercent => this["TwoHundredDayAverageChangePercent"];
    }
}
