namespace DataAccess
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;

    namespace EFLogging
    {
        public class EfToNpgsqlLoggerProvider : ILoggerProvider
        {
            public ILogger CreateLogger(string categoryName)
            {
                return new MyLogger();
            }

            public void Dispose()
            { }

            private class MyLogger : ILogger
            {
                public bool IsEnabled(LogLevel logLevel)
                {
                    return true;
                }

                public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
                {
                    const string fileName = @"efcore-mipt-log.txt";
                    if (!File.Exists(fileName))
                        File.Create(fileName);
                    
                    File.AppendAllText(fileName, formatter(state, exception));
                    Console.WriteLine(formatter(state, exception));
                }

                public IDisposable BeginScope<TState>(TState state)
                {
                    return null;
                }
            } 
        }
    }

}