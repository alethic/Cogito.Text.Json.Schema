using System.Text;

namespace Cogito.Text.Json.Schema
{

    public class JsonSchemaReaderSettings
    {

        /// <summary>
        /// Gets the default reader instance.
        /// </summary>
        public static JsonSchemaReaderSettings Default { get; } = new JsonSchemaReaderSettings();

        /// <summary>
        /// Gets or sets the resolver to be used to retrieve related resources.
        /// </summary>
        public JsonSchemaResolver Resolver { get; set; }

        /// <summary>
        /// Gets the encoding to use when parsing data of unknown origin.
        /// </summary>
        public Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

    }

}
