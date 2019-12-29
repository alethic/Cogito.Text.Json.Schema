using System;

namespace Cogito.Text.Json.Schema
{

    /// <summary>
    /// Describes a schema reference.
    /// </summary>
    public class JsonSchemaReference
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="subschemaId"></param>
        public JsonSchemaReference(Uri baseUri, Uri subschemaId)
        {
            BaseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
            SubschemaId = subschemaId;
        }

        /// <summary>
        /// The base URI for the referenced schema.
        /// </summary>
        public Uri BaseUri { get; }

        /// <summary>
        /// The subschema ID for the referenced schema.
        /// </summary>
        public Uri SubschemaId { get; }

    }

}
