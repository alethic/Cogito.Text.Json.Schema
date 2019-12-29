using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cogito.Text.Json.Schema
{

    class JsonSchemaConverter : JsonConverter<JsonSchema>
    {

        readonly JsonSchemaReader schemaReader = new JsonSchemaReader();

        public override JsonSchema Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return schemaReader.Read(JsonDocument.ParseValue(ref reader));
        }

        public override void Write(Utf8JsonWriter writer, JsonSchema value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

    }

}
