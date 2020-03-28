using System;
using System.IO;
using System.Text;

namespace ProjectUniversal
{
    ///<sumary>
    /// A wrapper class for a <see cref="StringWriter"/> to override
    /// the encoding property so that it defaults to Utf8 instead of Utf16
    ///</sumary>
    public class Utf8StringWriter : StringWriter
    {        
        #region Public Properties

        /// <summary>
        /// Override the Encoding property of the base <see cref="StringWriter"/>
        /// to make it return a Utf8 encoding
        /// </summary>
        public override Encoding Encoding => System.Text.Encoding.UTF8;

        #endregion
    }
}
