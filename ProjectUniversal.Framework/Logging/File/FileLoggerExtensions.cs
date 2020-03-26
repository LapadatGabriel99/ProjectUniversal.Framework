using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ProjectUniversal
{
    ///<sumary>
    /// Extension methods for the file logger
    ///</sumary>
    public static class FileLoggerExtensions
    {
        /// <summary>
        /// Add a new file logger to the specific path
        /// </summary>
        /// <param name="builder">The log builder to add to</param>
        /// <param name="path">The path where to write to</param>
        /// <param name="configuration">The file logger configuration</param>
        /// <returns></returns>
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string path, FileLoggerConfiguration configuration = null)
        {
            if (configuration == null)
                configuration = new FileLoggerConfiguration();

            // Add file log provider to builder
            builder.AddProvider(new FileLoggerProvider(path, configuration));

            return builder;
        }

        /// <summary>
        /// Injects a file logger into the framework construction
        /// </summary>
        /// <param name="frameworkConstruction">The construction</param>
        /// <param name="logPath">The path to the file to log to</param>
        /// <returns></returns>
        public static FrameworkConstruction UseFileLogger(this FrameworkConstruction frameworkConstruction, string logPath = "log.txt")
        {
            // Make use of AddLogging extension
            frameworkConstruction.Services.AddLogging(options =>
            {
                // Add file logger
                options.AddFile(logPath);
            });

            // Chain the construction
            return frameworkConstruction;
        }
    }
}
