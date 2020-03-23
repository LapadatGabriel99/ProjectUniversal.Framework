using Microsoft.Extensions.Logging;
using System;

namespace ProjectUniversal
{
    ///<sumary>
    /// The configuration for a <see cref="FileLogger"/>
    ///</sumary>
    public class FileLoggerConfiguration
    {
        #region Public Properties

        /// <summary>
        /// The level of log that should be processed
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Trace;

        /// <summary>
        /// Whether to log the time as part of the message
        /// </summary>
        public bool LogTime { get; set; } = true;       

        #endregion

        #region Constructors

        ///<sumary>
        ///Default Constructor
        ///</sumary>
        public FileLoggerConfiguration()
        {

        }

        #endregion
    }
}
