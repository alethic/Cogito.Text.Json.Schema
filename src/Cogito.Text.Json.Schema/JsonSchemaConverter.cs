using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cogito.Text.Json.Schema
{

    /// <summary>
    /// Provides a converter for parsing JSON data into a <see cref="JsonSchema"/>.
    /// </summary>
    class JsonSchemaConverter : JsonConverter<JsonSchema>
    {

        static readonly JsonSchemaReader rdr = new JsonSchemaReader();
        static readonly JsonSchemaWriter wrt = new JsonSchemaWriter();

        public override JsonSchema Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return rdr.Read(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, JsonSchema value, JsonSerializerOptions options)
        {
            wrt.Write(ref writer, value);
        }

    }

}
