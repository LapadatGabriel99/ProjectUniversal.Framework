using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

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
        /// Gets the configuration for the framework environment
        /// </summary>
        public static IConfiguration Configuration => Provider.GetService<IConfiguration>();

        /// <summary>
        /// Gets the framework environment of this class
        /// </summary>
        public static FrameworkEnvironment Environment => Provider.GetService<FrameworkEnvironment>();

        /// <summary>
        /// Gets the default logger for the framework
        /// </summary>
        public static ILogger Logger => Provider.GetService<ILogger>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Should be called at the very start of your application to configure and setup
        /// the ProjectUniversal
        /// </summary>
        /// <param name="configure">The action to add custom configuration to the configuration builder</param>
        /// <param name="servicesInjection">The action to inject services into the service collection</param>
        public static void Startup(Action<IConfigurationBuilder> configure = null, Action<IServiceCollection, IConfiguration> servicesInjection = null)
        {
            #region Initialize

            // Create a new list of dependencies
            var services = new ServiceCollection();

            #endregion

            #region Environment

            // Create environment details
            var environment = new FrameworkEnvironment();

            // Inject environment into services
            services.AddSingleton(environment);

            #endregion

            #region Configuration

            // Create our configuration sources
            var configurationBuilder = new ConfigurationBuilder()
                // Add environment variables( from our personal machine )
                .AddEnvironmentVariables()
                // Set base path for JSON files as the startup location of the application
                .SetBasePath(Directory.GetCurrentDirectory())
                // Add application settings JSON files
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ environment.Configuration }.json", optional: true, reloadOnChange: true);

            // Let custom configuration happened
            configure?.Invoke(configurationBuilder);

            // Inject configuration into services
            var configuration = configurationBuilder.Build();
            services.AddSingleton<IConfiguration>(configuration);

            #endregion

            #region Logging

            // Add logging as default
            services.AddLogging(options =>
            {
                // Setup loggers from configuration
                options.AddConfiguration(configuration.GetSection("Logging"));

                // Add console logger
                options.AddConsole();

                // Add debug logger
                options.AddDebug();

                // Add file logger
                options.AddFile("newlog.txt");                
            });

            // Add default logger
            services.AddDefaultLogger();

            #endregion

            #region Custom Services and Building           

            // Allow custom service injection
            servicesInjection?.Invoke(services, configuration);

            // Build the service provider
            ServiceProvider = services.BuildServiceProvider();

            #endregion

            // Log the startup complete
            Logger.LogCriticalSource($"ProjectUniversal started in { environment.Configuration }...");                       
        }

        #endregion        
    }   
}
