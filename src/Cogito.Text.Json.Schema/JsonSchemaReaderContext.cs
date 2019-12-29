using System;

namespace Cogito.Text.Json.Schema
{

    class JsonSchemaReaderContext
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="root"></param>
        public JsonSchemaReaderContext(JsonSchemaReader reader, JsonSchema root)
        {
            Reader = reader ?? throw new ArgumentNullException(nameof(reader));
            Root = root ?? throw new ArgumentNullException(nameof(root));
        }

        /// <summary>
        /// Gets the current reader instance.
        /// </summary>
        public JsonSchemaReader Reader { get; }

        /// <summary>
        /// Gets the root schema initially being read.
        /// </summary>
        public JsonSchema Root { get; }

    }

}
