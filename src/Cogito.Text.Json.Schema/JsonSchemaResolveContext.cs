using System;

namespace Cogito.Text.Json.Schema
{

    /// <summary>
    /// Describes the schema ID and its context when resolving a schema.
    /// </summary>
    public class JsonSchemaResolveContext
    {

        /// <summary>
        /// The referenced schema ID resolved using parent scopes.
        /// </summary>
        public Uri ResolvedSchemaId { get; set; }

        /// <summary>
        /// The base URI of the schema being read that is resolving the reference.
        /// </summary>
        public Uri ResolverBaseUri { get; set; }

        /// <summary>
        /// The referenced schema ID.
        /// </summary>
        public Uri SchemaId { get; set; }

    }

}
