using System;
using System.IO;
using System.Text.Json;

namespace Cogito.Text.Json.Schema
{

    public class JsonSchemaWriter
    {

        readonly JsonSchemaWriterSettings settings;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="settings"></param>
        public JsonSchemaWriter(JsonSchemaWriterSettings settings = null)
        {
            this.settings = settings ?? JsonSchemaWriterSettings.Default;
        }

        public void Write(ref Utf8JsonWriter writer, JsonSchema value)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            throw new NotImplementedException();
        }

        public void Write(TextWriter writer, JsonSchema value)
        {
            throw new NotImplementedException();
        }

        public void Write(Stream stream, JsonSchema value)
        {
            throw new NotImplementedException();
        }

    }

}
