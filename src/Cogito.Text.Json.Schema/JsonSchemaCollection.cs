using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cogito.Text.Json.Schema
{

    class JsonSchemaCollection : Collection<JsonSchema>
    {

        readonly JsonSchema parentSchema;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parentSchema"></param>
        public JsonSchemaCollection(JsonSchema parentSchema) :
            base(new List<JsonSchema>())
        {
            this.parentSchema = parentSchema;
        }

    }

}