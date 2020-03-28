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
            else
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

        /// <summary>
        /// Posts a web request to an URL and returns of the expected data type
        /// </summary>
        /// <param name="url">The URL to post to</param>
        /// <param name="content">The content to post</param>
        /// <param name="sendType">The format to serialize the content into</param>
        /// <param name="returnType">The expected type of content to be returned from the server</param>
        /// <returns></returns>
        public static async Task<WebRequestResult<TResponse>> PostAsync<TResponse>(
            string url,
            object content = null,
            KnownContentSerializers sendType = KnownContentSerializers.Json,
            KnownContentSerializers returnType = KnownContentSerializers.Json)
        {
            // Make standard Post call first
            var serverResponse = await PostAsync(url, content, sendType, returnType);

            // Create a result
            var result = serverResponse.CreateWebRequestResult<TResponse>();

            // If the response status code isn't equal to 200...
            if(result.StatusCode != HttpStatusCode.OK)
            {
                // Call failed
                // TODO: Localize string
                result.ErrorMessage = $"Server returned unsuccessful status code, { result.StatusCode } { result.StatusDescription }";

                // Done
                return result;
            }

            // If we have no content to deserialize
            if(result.RawServerResponse.IsNullOrEmpty())
            {
                // Done
                return result;
            }

            try
            {
                // If the server response type was not the expected type...
                if(!serverResponse.ContentType.ToLower().Contains(returnType.ToMimeString().ToLower()))
                {
                    // Failed due to unexpected return type
                    result.ErrorMessage = $"Server did not return data in expected type.Expected { returnType.ToMimeString() }, receiver { serverResponse.ContentType }";

                    // Done
                    return result;
                }

                // Deserialize to Json?
                if (returnType == KnownContentSerializers.Json)
                {
                    // Deserialize json string
                    result.ServerResponse = JsonConvert.DeserializeObject<TResponse>(result.RawServerResponse);
                }
                // Deserialize to Xml?
                else if(returnType == KnownContentSerializers.Xml)
                {
                    // Create xml serializer
                    var xmlSerializer = new XmlSerializer(typeof(TResponse));

                    // Create a memory stream for the raw string data
                    using(var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result.RawServerResponse)))
                    {
                        // Deserialize XML string
                        result.ServerResponse = (TResponse)xmlSerializer.Deserialize(memoryStream);                        
                    }
                }
                // Currently unknown
                else
                {
                    result.ErrorMessage = "Unknown return type, cannot deserialize server response to the expected type";

                    // Done
                    return result;
                }
            }
            catch(Exception ex)
            {
                // If deserialize failed
                result.ErrorMessage = "Failed to deserialize server response to the expected type";

                // Done
                return result;
            }

            // Deserialize raw response

            return result;
        }
    }
}
