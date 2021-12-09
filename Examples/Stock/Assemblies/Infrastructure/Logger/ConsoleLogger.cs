using Stock.Domain.Contracts;
using System;
using System.Collections.Generic;
using static Stock.Domain.Contracts.ILogService;

namespace Stock.Infrastructure.Logger
{
    public class ConsoleLogger : ILogService
    {
        private readonly Type _owner;

        public ConsoleLogger(Type owner)
        {
            _owner = owner;
        }

        private readonly Dictionary<LogType, ConsoleColor> _colorsForLogType = new() {
            { LogType.Trace, ConsoleColor.White },
            { LogType.Info, ConsoleColor.Green },
            { LogType.Warning, ConsoleColor.Yellow },
            { LogType.Debug, ConsoleColor.Cyan },
            { LogType.Error, ConsoleColor.Red },
        };

        public void Trace(string message)
        {
            Log(message, LogType.Trace);
        }

        public void Warning(string message)
        {
            Log(message, LogType.Warning);
        }

        public void Debug(string message)
        {
            Log(message, LogType.Debug);
        }

        public void Info(string message)
        {
            Log(message, LogType.Info);
        }

        public void Error(string message)
        {
            Log(message, LogType.Error);
        }

        public void Exception(Exception e)
        {
            Log("An error occurred: " + Environment.NewLine + e, LogType.Error);
        }

        private void Log(string message, LogType type)
        {
            var oldColor = Console.ForegroundColor;
            var newColor = _colorsForLogType[type];

            Console.ForegroundColor = newColor;
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:MM/dd HH:mm:ss}] " + message);
            Console.ForegroundColor = oldColor;
        }
    }
}
