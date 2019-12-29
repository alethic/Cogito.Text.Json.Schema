using System;

namespace Cogito.Text.Json.Schema
{

    /// <summary>
    /// Describes an exception that occurred while reading JSON schema.
    /// </summary>
    public class JsonSchemaReaderException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public JsonSchemaReaderException()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public JsonSchemaReaderException(string message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public JsonSchemaReaderException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}