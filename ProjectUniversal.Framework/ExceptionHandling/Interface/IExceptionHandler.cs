using System;

namespace ProjectUniversal
{
    ///<sumary>
    /// Handles exceptions when they are caught and passed to the exception handler
    ///</sumary>
    public interface IExceptionHandler
    {
        /// <summary>
        /// Handles the given exception
        /// </summary>
        /// <param name="exception">The exception to handle</param>
        void HandleError(Exception exception);
    }
}
