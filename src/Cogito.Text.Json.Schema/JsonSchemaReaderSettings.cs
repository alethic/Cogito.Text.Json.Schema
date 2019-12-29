namespace Cogito.Text.Json.Schema
{

    public class JsonSchemaReaderSettings
    {

        public static JsonSchemaReaderSettings Default { get; } = new JsonSchemaReaderSettings();

        public JsonSchemaResolver Resolver { get; set; }

    }

}
