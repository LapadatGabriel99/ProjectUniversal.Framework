using System;

namespace ProjectUniversal
{
    ///<sumary>
    /// Details about the current system environment
    ///</sumary>
    public class FrameworkEnvironment
    {
        #region Public Properties

        /// <summary>
        /// True if we are in development environment
        /// Naturally we want to presume we start in debug mode
        /// </summary>
        public bool IsDevelopment { get; set; } = true;

        /// <summary>
        /// The configuration of the environment
        /// Either Development or Production
        /// </summary>
        public string Configuration => IsDevelopment ? "Development" : "Production";

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public FrameworkEnvironment()
        {
            // If we are in release set development flag to false
            #if RELEASE
            
            IsDevelopment = false;

            #endif
        }

        #endregion
    }
}
