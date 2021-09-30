using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PlatformService.Models
{
    public class LoggerProvider : ILoggerProvider
    {
        LoggerOptions loggerOptions;

        public LoggerProvider(IOptionsMonitor<LoggerOptions> options)
        {
            loggerOptions = options.CurrentValue;
        }
        public ILogger CreateLogger(string categoryName)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}