using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace ProjectUniversal
{
    ///<sumary>
    /// Extension methods for a <see cref="ILogger"/>
    ///</sumary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Logs a critical message, including the source of the log
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="message">The message</param>
        /// <param name="origin">The caller's member function name</param>
        /// <param name="filePath">The caller's source code file path</param>
        /// <param name="lineNumber">The line number of the caller</param>
        /// <param name="args">The additional arguments</param>
        public static void LogCriticalSource(
            this ILogger logger, 
            string message,
            EventId eventId = new EventId(),
            Exception exception = null,
            [CallerMemberName] string origin = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0,
            params object[] args)
        {
            logger.Log<object[]>(LogLevel.Critical, eventId, args.Prepend(origin, filePath, lineNumber, message), exception, LoggerSourceFormatter.Format);            
        }       
    }

    /// TODO: Implement the other wrappers of the methods from <see cref="LoggerExtensions"/>
}
