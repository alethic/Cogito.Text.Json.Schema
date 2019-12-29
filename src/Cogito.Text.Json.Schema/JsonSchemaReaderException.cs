using System;
using System.Runtime.Serialization;

namespace Cogito.Text.Json.Schema
{
    [Serializable]
    internal class JsonSchemaReaderException : Exception
    {
        public JsonSchemaReaderException()
        {
        }

        public JsonSchemaReaderException(string message) : base(message)
        {
        }

        public JsonSchemaReaderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JsonSchemaReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}