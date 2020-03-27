using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace ProjectUniversal
{
    ///<sumary>
    /// Provides http calls for providing and receiving information from a http server
    ///</sumary>
    public static class WebRequests
    {
        /// <summary>
        /// Posts a web request to and URL and returns the raw http web request
        /// </summary>
        /// <param name="url">The URL to post to</param>
        /// <param name="content">The content to post</param>
        /// <param name="sendType">The format to serialize the content into</param>
        /// <param name="returnType">The expected type of content to be returned from the server</param>
        /// <returns></returns>
        public static async Task<HttpWebResponse> PostAsync(
            string url, 
            object content = null, 
            KnownContentSerializers sendType = KnownContentSerializers.Json, 
            KnownContentSerializers returnType = KnownContentSerializers.Json)
        {
            #region Setup
            
            // Create the web request
            var request = WebRequest.CreateHttp(url);

            // Make it a POST request method
            request.Method = HttpMethod.Post.ToString();

            // Set the appropriate return type
            request.Accept = returnType.ToMimeString();

            // Set content type
            request.ContentType = sendType.ToMimeString();

            #endregion

            #region Write Content

            // Set content length
            if (content == null)
            {
                request.ContentLength = 0;
            }
            // Otherwise...
            {
                // Create content to write
                var contentString = string.Empty;

                // Serialize to Json?
                if(sendType == KnownContentSerializers.Json)
                {
                    // Serialize content to Json string
                    contentString = JsonConvert.SerializeObject(content);                    
                }
                // Serialize to Xml?
                else if(sendType == KnownContentSerializers.Xml)
                {
                    // Create xml serializer
                    var xmlSerializer = new XmlSerializer(content.GetType());

                    // Create a string writer to receive the serialized string
                    using(var stringWriter = new StringWriter())
                    {                                                
                        // Serialize the object to a string
                        xmlSerializer.Serialize(stringWriter, content);

                        // Extract the string from the writer
                        contentString = stringWriter.ToString();
                    }
                }
                // Currently unknown
                else
                {
                    //TODO: Throw error once we have ProjectUniversal Framework exception types
                }

                // Get the request stream(body stream) async...
                using(var requestStream = await request.GetRequestStreamAsync())
                {
                    // Create a stream writer to write to the body stream...
                    using(var streamWriter = new StreamWriter(requestStream))
                    {
                        // Write content to Http body stream
                        await streamWriter.WriteAsync(contentString);
                    }
                }
            }

            #endregion

            // Return the raw server response
            return await request.GetResponseAsync() as HttpWebResponse;
        }
    }
}
