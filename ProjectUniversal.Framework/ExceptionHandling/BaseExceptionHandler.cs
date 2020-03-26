using System;

namespace ProjectUniversal
{
    ///<sumary>
    /// Handles all exceptions, simply logging them to the logger
    ///</sumary>
    public class BaseExceptionHandler : IExceptionHandler
    {
        #region Constructors

        ///<sumary>
        ///Default Constructor
        ///</sumary>
        public BaseExceptionHandler()
        {

        }

        #endregion

        #region IExceptionHandler Methods

        /// <summary>
        /// Logs the given error
        /// </summary>
        /// <param name="exception">The exception</param>
        public void HandleError(Exception exception)
        {
            // Log it
            // TODO: Localization of string
            Framework.Logger.LogCriticalSource("Unhandled exception occurred", exception: exception);
        }

        #endregion
    }
}
