using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace ProjectUniversal
{
    ///<sumary>
    /// A logger that writes the logs to file
    ///</sumary>
    public class FileLogger : ILogger
    {
        #region Static Properties

        protected static ConcurrentDictionary<string, object> FileLocks = new ConcurrentDictionary<string, object>();

        #endregion

        #region Protected Members

        /// <summary>
        /// The category for this file logger
        /// </summary>
        protected string _categoryName;

        /// <summary>
        /// The file path to log to
        /// </summary>
        protected string _filePath;

        /// <summary>
        /// The configuration to use
        /// </summary>
        protected FileLoggerConfiguration _configuration;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="categoryName">The category for this file logger</param>
        /// <param name="filePath">The file path to log to</param>
        /// <param name="configuration">The configuration to use</param>
        public FileLogger(string categoryName, string filePath, FileLoggerConfiguration configuration)
        {
            // Get the absolute path
            filePath = Path.GetFullPath(filePath);

            // Set members
            _categoryName = categoryName;
            _filePath = filePath;
            _configuration = configuration;
        }

        #endregion

        #region FileLogger Implementation

        /// <summary>
        /// File loggers are not scoped so this is always null
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// Enabled if the log level is the same or greater than the configuration
        /// </summary>
        /// <param name="logLevel">The log level to check against</param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            // Enabled if the log level is greater or equal to what we want to log
            return logLevel >= _configuration.LogLevel;
        }

        /// <summary>
        /// Logs the message to file
        /// </summary>
        /// <typeparam name="TState">The type of the details of the message</typeparam>
        /// <param name="logLevel">The log level</param>
        /// <param name="eventId">The Id</param>
        /// <param name="state">The details of the message</param>
        /// <param name="exception">Any exception to log</param>
        /// <param name="formatter">The formatter for converting the state and exception to a message string</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //If we should not log
            if(!IsEnabled(logLevel))
            {
                // Return
                return;
            }

            // Get the current time
            var currentTime = DateTimeOffset.Now.ToString("yyyy-MM-dd hh:mm:ss");

            // Prefix the time log to the message if desired
            var timeLogString = _configuration.LogTime ? $"[{currentTime}] " : "";

            // Get the formatted message string
            var message = formatter(state, exception);

            // Write the message to the log file
            var output = timeLogString + message + Environment.NewLine;            

            //Normalize path
            //TODO: Make use of configuration base path
            var normalizedPath = _filePath.ToUpper();

            // Get the file lock based on the absolute path
            var fileLock = FileLocks.GetOrAdd(normalizedPath, path => new object());

            // Lock the file so that it is thread safe
            lock(fileLock)
            {
                // Write the message to the file
                File.AppendAllText(_filePath, output);
            }
        } 

        #endregion
    }
}
