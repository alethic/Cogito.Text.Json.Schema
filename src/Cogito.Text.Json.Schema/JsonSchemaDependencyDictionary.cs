using System;
using System.Collections.Generic;

using Cogito.Text.Json.Schema.Internal;

namespace Cogito.Text.Json.Schema
{

    class JsonSchemaDependencyDictionary : DictionaryBase<string, object>
    {

        readonly JsonSchema parentSchema;
        int count;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parentSchema"></param>
        public JsonSchemaDependencyDictionary(JsonSchema parentSchema) :
            base(StringComparer.Ordinal)
        {
            this.parentSchema = parentSchema;
        }

        public bool HasSchemas => count > 0;

        protected override void AddItem(string key, object value)
        {
            base.AddItem(key, value);

            if (value is JsonSchema)
                count++;
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            count = 0;
        }

        public Dictionary<string, object> GetInnerDictionary()
        {
            return (Dictionary<string, object>)Dictionary;
        }

        protected override bool RemoveItem(string key)
        {
            if (!TryGetValue(key, out var obj))
                return false;

            base.RemoveItem(key);

            if (obj is JsonSchema)
                count--;

            return true;
        }

        protected override void SetItem(string key, object value)
        {
            if (TryGetValue(key, out var obj) && obj is JsonSchema schema)
            {
                if (schema == value)
                    return;

                count--;
            }

            base.SetItem(key, value);

            if (value is JsonSchema)
                count++;
        }

    }

}
