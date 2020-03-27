using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace ProjectUniversal
{
    ///<sumary>
    /// Extension methods for the ProjectUniversal Framework
    ///</sumary>
    public static class FrameworkExtensions
    {
        #region Configure
        
        /// <summary>
        /// Configures a framework construction
        /// </summary>
        /// <param name="frameworkConstruction">The construction to configure</param>
        /// <param name="configure">The custom configuration action</param>
        /// <returns></returns>
        public static FrameworkConstruction Configure(this FrameworkConstruction frameworkConstruction, Action<IConfigurationBuilder> configure = null)
        {
            // Create our configuration source
            var configurationBuilder = new ConfigurationBuilder()
                // Add environment variables
                .AddEnvironmentVariables()
                // Set base path for JSON files as the startup location of the application
                .SetBasePath(Directory.GetCurrentDirectory())
                // Add application settings JSON files
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ frameworkConstruction.Environment }.json", optional: true, reloadOnChange: true);

            // Apply custom configuration, if there is one
            configure?.Invoke(configurationBuilder);

            // Inject configuration into Services
            var configuration = configurationBuilder.Build();
            frameworkConstruction.Services.AddSingleton<IConfiguration>(configuration);

            // Set the framework construction configuration
            frameworkConstruction.Configuration = configuration;

            // Chain the construction
            return frameworkConstruction;
        }

        #endregion

        #region Logging

        /// <summary>
        /// Injects the default logger into the framework construction        
        /// </summary>
        /// <param name="frameworkConstruction">The construction</param>
        /// <returns></returns>
        public static FrameworkConstruction AddDefaultLogger(this FrameworkConstruction frameworkConstruction)
        {
            // Add default logging
            frameworkConstruction.Services.AddLogging(options =>
            {
                // Setup loggers from configuration
                options.AddConfiguration(frameworkConstruction.Configuration.GetSection("Logging"));

                // Add console logger
                options.AddConsole();

                // Add debug logger
                options.AddDebug();
            });

            // Adds a default logger so that we can get a non-generic ILogger
            // that will have the category name of "ProjectUniversal"
            frameworkConstruction.Services.AddTransient(provider => provider.GetService<ILoggerFactory>().CreateLogger("ProjectUniversal"));

            // Return the services
            return frameworkConstruction;
        } 

        #endregion

        #region ExceptionHandler

        /// <summary>
        /// Injects the default exception handler into the framework construction
        /// </summary>
        /// <param name="frameworkConstruction">The construction</param>
        /// <returns></returns>
        public static FrameworkConstruction AddDefaultExceptionHandler(this FrameworkConstruction frameworkConstruction)
        {
            // Bind a static instance of the BaseExceptionHandler
            frameworkConstruction.Services.AddSingleton<IExceptionHandler>(new BaseExceptionHandler());

            // Chain the construction
            return frameworkConstruction;
        }

        #endregion

        #region Default Services

        /// <summary>
        /// Injects all of the default services used by this framework for a quicker and cleaner setup
        /// </summary>
        /// <param name="frameworkConstruction">The construction</param>
        /// <returns></returns>
        public static FrameworkConstruction UseDefaultServices(this FrameworkConstruction frameworkConstruction)
        {
            #region Exception Handling

            // Add the exception handling
            frameworkConstruction.AddDefaultExceptionHandler();

            #endregion

            #region Logging

            // Add default logger
            frameworkConstruction.AddDefaultLogger();

            #endregion

            // Chain the construction
            return frameworkConstruction;
        } 
        #endregion
    }
}
