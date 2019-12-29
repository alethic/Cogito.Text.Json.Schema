using System.Text;

namespace Cogito.Text.Json.Schema
{

    public class JsonSchemaWriterSettings
    {

        /// <summary>
        /// Gets the default reader instance.
        /// </summary>
        public static JsonSchemaWriterSettings Default { get; } = new JsonSchemaWriterSettings();

        /// <summary>
        /// Gets the encoding to use when writing data of unspecified origin.
        /// </summary>
        public Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

    }

}
