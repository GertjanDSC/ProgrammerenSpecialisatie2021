using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Stock.Domain;
using Stock.Infrastructure.YahooFinanceApi;

// https://github.com/zkavtaskin/Domain-Driven-Design-Example

namespace Stock
{

    internal class Program
    {
        private static Dictionary<string, ShareData> _shares = new()
        {
            // Afkortingen te zoeken via: https://finance.yahoo.com/quote/ABI.BR/?p=ABI.BR
            { "ABI.BR", new ShareData { Count = 160, Value = 150 }}, // dit wil zeggen: ik kocht 160 aandelen, oorspronkelijk tegen een waarde van 150 euro
            { "ALC", new ShareData { Count = 40, Value = 0 }},
            { "AAPL", new ShareData { Count = 280, Value = 0 }},
            { "MTS.MC", new ShareData { Count = 333, Value = 0 }},
            { "ASML.AS", new ShareData { Count = 345, Value = 0 }},
            { "BNB.BR", new ShareData { Count = 14, Value = 0 }},
            { "CFEB.BR", new ShareData { Count = 60, Value = 0 }},
            { "COLR.BR", new ShareData { Count = 500, Value = 0 }},
            { "FLUX.BR", new ShareData { Count = 500, Value = 0 }},
            { "GBLB.BR", new ShareData { Count = 110, Value = 0 }},
            { "INTC", new ShareData { Count = 800, Value = 0 }},
            { "KBC.BR", new ShareData { Count = 100, Value = 0 }},
            { "MSFT", new ShareData { Count = 150, Value = 0 }},
            { "NOVN.SW", new ShareData { Count = 200, Value = 0 }},
            { "OSPN", new ShareData { Count = 500, Value = 0 }},
            { "PROX.BR", new ShareData { Count = 250, Value = 0 }},
            { "REPYY", new ShareData { Count = 333, Value = 0 }},
            { "SIP.BR", new ShareData { Count = 240, Value = 0 }},
            { "SOLB.BR", new ShareData { Count = 137, Value = 0 }},
            { "TNET.BR", new ShareData { Count = 500, Value = 0 }},
            { "UCB.BR", new ShareData { Count = 500, Value = 0 }}
        };

        private async static void Execute(object sender, EventArgs e)
        {
            var keys = _shares.Keys.ToArray();
            var securities = await Yahoo
                .Symbols(keys) // LINQ: ToArray()
                .Fields(Stock.Infrastructure.YahooFinanceApi.Field.Symbol, Stock.Infrastructure.YahooFinanceApi.Field.RegularMarketOpen, Stock.Infrastructure.YahooFinanceApi.Field.RegularMarketPrice, Stock.Infrastructure.YahooFinanceApi.Field.RegularMarketTime, Stock.Infrastructure.YahooFinanceApi.Field.Currency, Stock.Infrastructure.YahooFinanceApi.Field.LongName)
                .QueryAsync();

            var total = (decimal)0.0;
            foreach (var tick in securities)
            {
                if (_shares.ContainsKey(tick.Key))
                {
                    var v = _shares[tick.Key].Count * ConvertCurrency(tick.Value.Currency, (decimal)tick.Value.RegularMarketPrice);
                    total += v;
                    var u = _shares[tick.Key].Count * ConvertCurrency(tick.Value.Currency, _shares[tick.Key].Value);
                    System.Diagnostics.Debug.WriteLine(tick.Key + ": " + u + " -> " + v + " (delta: " + (v - u) + ")");
                }
            }
            // You should be able to query data from various markets including US, HK, TW
            // The startTime & endTime here defaults to EST timezone
            var history = await Yahoo.GetHistoricalAsync("KBC.BR", new DateTime(2021, 1, 1), new DateTime(2021, 6, 30), Infrastructure.YahooFinanceApi.Period.Daily);

            foreach (var candle in history)
            {
                System.Console.WriteLine($"DateTime: {candle.DateTime}, Open: {candle.Open}, High: {candle.High}, Low: {candle.Low}, Close: {candle.Close}, Volume: {candle.Volume}, AdjustedClose: {candle.AdjustedClose}");
            }
        }

        private static decimal ConvertCurrency(string currency, decimal regularMarketPrice)
        {
            // dummy implementation:
            return regularMarketPrice;
        }

        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Execute(sender, e);
        }

        private static Timer _timer;

        static void Main(string[] args)
        {
            _timer = new Timer(5000); // Timer loopt af elke 5 seconden - 5000 milliseconden
            // To add the elapsed event handler:
            // ... Type "_timer.Elapsed += " and press tab twice.            
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true; // Activeert de timer bij het starten
            _timer.Start();

            // Om de applicatie niet meteen te laten stoppen en de timer te onderbreken, organiseren
            // we een eindeloze loop die telkens 1 seconde pauseert: "busy form of waiting"
            while(true)
            {
                System.Threading.Thread.Sleep(10000); // de timer blijft doorlopen
                System.Console.WriteLine("Waiting...");
            }
        }
    }
}
