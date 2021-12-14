using Serilog;
using Serilog.Formatting.Compact;
//using MongoDB.Driver;
using Stock.Domain.Contracts;
using System;

namespace Stock.Infrastructure.Logger
{
    public class SeriLogger : ILogService
    {
        public SeriLogger()
        {
            
            // JSON file:

            Log.Logger = new LoggerConfiguration().
                            MinimumLevel.Debug().
                            Enrich.WithProperty("Application", "Stock").
                            Enrich.WithThreadId().
                            Enrich.WithMemoryUsage().
                            Enrich.WithAssemblyName().
                            // Adds time to stock file name:
                            WriteTo.File(new CompactJsonFormatter(), @"logs\stock.json", rollingInterval: RollingInterval.Hour).
                            WriteTo.Debug().
                            CreateLogger();
            /*            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "Stock")
                .Enrich.WithAssemblyName()
                .Enrich.WithCorrelationId()
                .Enrich.WithMemoryUsage()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .WriteTo.Conditional(_ => true, wt => wt.Console())
                .WriteTo.MongoDBBson(cfg =>
                {
                    var mongoDbInstance = new MongoClient(
                        "mongodb://127.0.0.1:27017/")
                    .GetDatabase("logs");
                    // sink will use the IMongoDatabase instance provided
                    cfg.SetMongoDatabase(mongoDbInstance);
                })
                .CreateLogger();

            // See: https://marketplace.visualstudio.com/items?itemName=gluca.log-view-l4n
            // use VSCode, View > Command Palette > Inspect SeriLog events (after opening a trust window)
            // mongodb://127.0.0.1/logs (as url)
            // logs (for collection)    
            */
        }

        ~SeriLogger()
        {
            Log.CloseAndFlush();
        }

        public void Debug(string message)
        {
            Log.Debug(message);            
        }

        public void Error(string message)
        {
            Log.Error(message);
        }

        public void Exception(Exception e)
        {
            Log.Fatal(e.Message);
        }

        public void Info(string message)
        {
            Log.Information(message);
        }

        public void Trace(string message)
        {
            Log.Verbose(message);
        }

        public void Warning(string message)
        {
            Log.Warning(message);
        }        
    }
}
