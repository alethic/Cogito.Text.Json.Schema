using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cogito.Text.Json.Schema.Internal
{

    class ValuesCollection : ICollection<JsonSchema>, IEnumerable<JsonSchema>, IEnumerable
    {

        readonly ICollection<PatternSchema> inner;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="inner"></param>
        public ValuesCollection(ICollection<PatternSchema> inner)
        {
            this.inner = inner;
        }

        public int Count => inner.Count;

        public bool IsReadOnly => inner.IsReadOnly;

        public void Add(JsonSchema item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(JsonSchema item)
        {
            return inner.Any(p => p.Schema == item);
        }

        public void CopyTo(JsonSchema[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<JsonSchema> GetEnumerator()
        {
            ValuesCollection valuesCollections = null;
            foreach (var patternSchema in valuesCollections.inner)
                yield return patternSchema.Schema;
        }

        public bool Remove(JsonSchema item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}
