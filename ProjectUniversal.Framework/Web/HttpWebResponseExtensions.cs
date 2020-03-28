using System;
using System.IO;
using System.Net;

namespace ProjectUniversal
{
    ///<sumary>
    /// Extension methods for <see cref="HttpWebResponse"/>
    ///</sumary>
    public static class HttpWebResponseExtensions
    {
        /// <summary>
        /// Returns a <see cref="WebRequestResult{T}"/> pre-populated with the <see cref="HttpWebResponse"/> information
        /// </summary>
        /// <typeparam name="TResponse">The type of response to create</typeparam>
        /// <param name="webResponse">The web/server response</param>
        /// <returns></returns>
        public static WebRequestResult<TResponse> CreateWebRequestResult<TResponse>(this HttpWebResponse webResponse)
        {
            // Return a new web request result
            var result = new WebRequestResult<TResponse>
            {
                // Content type
                ContentType = webResponse.ContentType,

                // Headers
                Headers = webResponse.Headers,

                // Cookies
                Cookies = webResponse.Cookies,

                // Status code
                StatusCode = webResponse.StatusCode,

                // Status description
                StatusDescription = webResponse.StatusDescription
            };

            // If we got a successful response
            if(result.StatusCode == HttpStatusCode.OK)
            {
                // Open the response stream
                using(var responseStream = webResponse.GetResponseStream())
                {
                    // Get a stream reader
                    using(var streamReader = new StreamReader(responseStream))
                    {
                        // Read in the response body
                        result.RawServerResponse = streamReader.ReadToEnd();
                    }
                }
            }

            return result;
        }               
    }
}
