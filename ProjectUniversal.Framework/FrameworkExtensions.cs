using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ProjectUniversal
{
    ///<sumary>
    /// Extension methods for the ProjectUniversal Framework
    ///</sumary>
    public static class FrameworkExtensions
    {
        /// <summary>
        /// Adds a default logger so that we can get a non-generic ILogger
        /// that will have the category name of "ProjectUniversal"
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultLogger(this IServiceCollection services)
        {
            // Add a default logger
            // The logger will be used and then immediately destroyed
            services.AddTransient(provider => provider.GetService<ILoggerFactory>().CreateLogger("ProjectUniversal"));

            // Return the services
            return services;
        }
    }
}
