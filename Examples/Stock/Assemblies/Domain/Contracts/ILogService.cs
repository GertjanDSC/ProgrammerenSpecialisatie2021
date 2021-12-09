using System;

namespace Stock.Domain.Contracts
{
    public interface ILogService
    {
        enum LogType { Exception, Error, Warning, Info, Debug, Trace };

        void Exception(Exception e);
        void Error(string message);
        void Warning(string message);
        void Info(string message);
        void Debug(string message);
        void Trace(string message);
    }
}
