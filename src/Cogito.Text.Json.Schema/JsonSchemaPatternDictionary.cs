using System;
using System.Collections;
using System.Collections.Generic;

using Cogito.Text.Json.Schema.Internal;

namespace Cogito.Text.Json.Schema
{

    class JSchemaPatternDictionary :
        IDictionary<string, JsonSchema>,
        ICollection<KeyValuePair<string, JsonSchema>>,
        IEnumerable<KeyValuePair<string, JsonSchema>>,
        IEnumerable
    {

        readonly Dictionary<string, PatternSchema> inner;
        ValuesCollection _values;

        public JSchemaPatternDictionary()
        {
            inner = new Dictionary<string, PatternSchema>(StringComparer.Ordinal);
        }

        public JsonSchema this[string key]
        {
            get => inner[key].Schema;
            set => inner[key] = new PatternSchema(key, value);
        }

        public int Count => inner.Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, PatternSchema>>)inner).IsReadOnly;

        public ICollection<string> Keys => inner.Keys;

        public ICollection<JsonSchema> Values
        {
            get
            {
                if (_values == null)
                    _values = new ValuesCollection(inner.Values);

                return _values;
            }
        }

        public void Add(KeyValuePair<string, JsonSchema> item) => inner.Add(item.Key, new PatternSchema(item.Key, item.Value));

        public void Add(string key, JsonSchema value) => inner.Add(key, new PatternSchema(key, value));

        public void Clear() => inner.Clear();

        public bool Contains(KeyValuePair<string, JsonSchema> item)
        {
            return inner.TryGetValue(item.Key, out var patternSchema) ? patternSchema.Schema == item.Value : false;
        }

        public bool ContainsKey(string key) => inner.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, JsonSchema>[] array, int arrayIndex)
        {
            var keyValuePairArray = new KeyValuePair<string, PatternSchema>[array.Length];
            ((ICollection<KeyValuePair<string, PatternSchema>>)inner).CopyTo(keyValuePairArray, arrayIndex);
            for (var i = 0; i < keyValuePairArray.Length; i++)
            {
                var keyValuePair = keyValuePairArray[i];
                array[i] = new KeyValuePair<string, JsonSchema>(keyValuePair.Key, keyValuePair.Value.Schema);
            }
        }

        public IEnumerator<KeyValuePair<string, JsonSchema>> GetEnumerator()
        {
            JSchemaPatternDictionary jSchemaPatternDictionaries = null;
            foreach (var keyValuePair in jSchemaPatternDictionaries.inner)
            {
                yield return new KeyValuePair<string, JsonSchema>(keyValuePair.Key, keyValuePair.Value.Schema);
            }
        }

        public IEnumerable<PatternSchema> GetPatternSchemas() => inner.Values;

        public bool Remove(KeyValuePair<string, JsonSchema> item)
        {
            return inner.TryGetValue(item.Key, out var patternSchema) && patternSchema.Schema == item.Value ? inner.Remove(item.Key) : false;
        }

        public bool Remove(string key)
        {
            return inner.Remove(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool TryGetValue(string key, out JsonSchema value)
        {
            if (inner.TryGetValue(key, out var patternSchema))
            {
                value = patternSchema.Schema;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

    }

}
