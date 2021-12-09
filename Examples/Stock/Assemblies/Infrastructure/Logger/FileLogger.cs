using Stock.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using static Stock.Domain.Contracts.ILogService;

namespace Stock.Infrastructure.Logger
{
    public class FileLogger : ILogService
    {
        private readonly Type _owner;

        public FileLogger(Type owner)
        {
            _owner = owner;
        }

        private readonly Dictionary<LogType, string> _fileNameForLogType = new() {
            { LogType.Trace, "trace.log" },
            { LogType.Info, "info.log" },
            { LogType.Warning, "warn.log" },
            { LogType.Debug, "debug.log" },
            { LogType.Error, "error.log" },
        };

        public void Trace(string message)
        {
            // Should only need the Log method in this class but
            // I need to comply to ILogger, I'll keep them until
            // I come up with a solution, an ideal one would be 
            // I just call Log(message, type) from the HybridLogger

            // But thinking, is extra methods better? Encapsulation for LogType...

            LogToFile(message, LogType.Trace);
        }

        public void Warning(string message)
        {
            LogToFile(message, LogType.Warning);
        }

        public void Debug(string message)
        {
            LogToFile(message, LogType.Debug);
        }

        public void Info(string message)
        {
            LogToFile(message, LogType.Info);
        }

        public void Error(string message)
        {
            LogToFile(message, LogType.Error);
        }

        public void Exception(Exception e)
        {
            LogToFile(e.ToString(), LogType.Exception);
        }

        private void LogToFile(string message, LogType type)
        {
            using StreamWriter writer = new(Path.GetFullPath("logging/" + _fileNameForLogType[type]));
            writer.WriteLine($"Occurred at [{DateTime.Now:MM/dd HH:mm:ss}] in [{_owner.FullName}]: " + Environment.NewLine + message + Environment.NewLine);
        }
    }
}
