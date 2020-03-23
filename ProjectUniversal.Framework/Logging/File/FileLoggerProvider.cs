using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace ProjectUniversal
{
    ///<sumary>
    /// Provides the ability to log to file
    ///</sumary>
    public class FileLoggerProvider : ILoggerProvider
    {
        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="path">The path to log to</param>
        /// <param name="configuration">The configuration to use</param>
        public FileLoggerProvider(string path, FileLoggerConfiguration configuration)
        {
            // Set the configuration
            _configuration = configuration;

            // Set the path
            _filePath = path;
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// The path to write to
        /// </summary>
        protected string _filePath;

        /// <summary>
        /// The configuration to be used when creating file loggers
        /// </summary>
        protected readonly FileLoggerConfiguration _configuration;

        /// <summary>
        /// Keeps track of the logger already created
        /// </summary>
        protected readonly ConcurrentDictionary<string, FileLogger> _loggers = new ConcurrentDictionary<string, FileLogger>();

        #endregion

        #region ILoggerProvider Implementation

        /// <summary>
        /// Creates a file logger based on the category name
        /// </summary>
        /// <param name="categoryName">The category name of this logger</param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            // Get or create the logger for this category
            return _loggers.GetOrAdd(categoryName, name => new FileLogger(name, _filePath, _configuration));
        }

        /// <summary>
        /// Disposes the list of loggers
        /// </summary>
        public void Dispose()
        {
            _loggers.Clear();
        } 

        #endregion
    }
}
