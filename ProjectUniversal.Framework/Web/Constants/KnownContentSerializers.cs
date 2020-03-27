using System;

namespace ProjectUniversal
{
    ///<sumary>
    /// Known content types that can be serialized and sent to a receiver
    ///</sumary>
    public enum KnownContentSerializers
    {
        /// <summary>
        /// The data should be serialized to JSON
        /// </summary>
        Json = 1,

        /// <summary>
        /// The data should be serialized to XML
        /// </summary>
        Xml
    }
}
