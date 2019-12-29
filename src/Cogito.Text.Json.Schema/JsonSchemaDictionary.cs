using System;
using System.Collections.Generic;

using Cogito.Text.Json.Schema.Internal;

namespace Cogito.Text.Json.Schema
{

    class JsonSchemaDictionary : DictionaryBase<string, JsonSchema>
    {

        readonly JsonSchema parentSchema;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parentSchema"></param>
        /// <param name="dictionary"></param>
        public JsonSchemaDictionary(JsonSchema parentSchema, IDictionary<string, JsonSchema> dictionary) :
            base(dictionary)
        {
            this.parentSchema = parentSchema;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parentSchema"></param>
        public JsonSchemaDictionary(JsonSchema parentSchema) :
            this(parentSchema, new Dictionary<string, JsonSchema>(StringComparer.Ordinal))
        {

        }

    }

}
