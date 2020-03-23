using Microsoft.Extensions.Logging;
using System;

namespace ProjectUniversal
{
    ///<sumary>
    /// Extension methods for the file logger
    ///</sumary>
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string path, FileLoggerConfiguration configuration = null)
        {
            if (configuration == null)
                configuration = new FileLoggerConfiguration();

            // Add file log provider to builder
            builder.AddProvider(new FileLoggerProvider(path, configuration));

            return builder;
        }
    }
}
