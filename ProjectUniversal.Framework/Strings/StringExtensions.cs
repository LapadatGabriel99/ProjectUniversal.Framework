using System;

namespace ProjectUniversal
{
    ///<sumary>
    /// Extension methods for <see cref="string"/>
    ///</sumary>
    public static class StringExtensions
    {
        /// <summary>
        /// An extension method that determines if the given string is null or empty
        /// Returns true if so
        /// </summary>
        /// <param name="content">The string to be checked</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string content)
        {
            // Return the result
            return string.IsNullOrEmpty(content) ? true : false;
        }

        /// <summary>
        /// An extension method that determines if the given string is null or white space
        /// Returns true if so
        /// </summary>
        /// <param name="content">The string to be checked</param>
        /// <returns></returns>
        public static bool IsNullOrWhitespace(this string content)
        {
            // Return the result
            return string.IsNullOrWhiteSpace(content) ? true : false;
        }
    }
}
