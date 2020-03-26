using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ProjectUniversal
{
    ///<sumary>
    /// The main entry point into the ProjectUniversal Framework library
    ///</sumary>
    public static class Framework
    {
        #region Private Members

        /// <summary>
        /// The dependency injection service provider
        /// (Similar to NInject, finds services we added and injects them)
        /// </summary>
        private static IServiceProvider ServiceProvider;      

        #endregion

        #region Public Properties

        /// <summary>
        /// The dependency injection service provider
        /// </summary>
        public static IServiceProvider Provider => ServiceProvider;

        /// <summary>
        /// Gets the configuration 
        /// </summary>
        public static IConfiguration Configuration => Provider.GetService<IConfiguration>();

        /// <summary>
        /// Gets the framework 
        /// </summary>
        public static FrameworkEnvironment Environment => Provider.GetService<FrameworkEnvironment>();

        /// <summary>
        /// Gets the default logger 
        /// </summary>
        public static ILogger Logger => Provider.GetService<ILogger>();

        /// <summary>
        /// Gets the frameworks exception handler
        /// </summary>
        public static IExceptionHandler ExceptionHandler => Provider.GetService<IExceptionHandler>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Should be called once a framework construction is finished and we want to build it
        /// and start our application
        /// </summary>
        /// <param name="frameworkConstruction">The construction</param>
        public static void Build(this FrameworkConstruction frameworkConstruction)
        {
            #region Service Provider          

            // Build the service provider
            ServiceProvider = frameworkConstruction.Services.BuildServiceProvider();

            #endregion

            // Log the startup complete
            Logger.LogCriticalSource($"ProjectUniversal started in { Environment.Configuration }...");                       
        }

        #endregion        
    }   
}
