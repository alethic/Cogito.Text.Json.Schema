using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Cogito.Text.Json.Schema.Internal
{

    abstract class DictionaryBase<TKey, TValue> :
        IDictionary<TKey, TValue>,
        ICollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>>,
        IEnumerable,
        IDictionary,
        ICollection
    {

        readonly IDictionary<TKey, TValue> dictionary;

        public int Count => dictionary.Count;

        protected IDictionary<TKey, TValue> Dictionary => dictionary;

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get => dictionary[key];
            set => SetItem(key, value);
        }

        public ICollection<TKey> Keys => dictionary.Keys;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        object IDictionary.this[object key]
        {
            get => IsCompatibleKey(key) && TryGetValue((TKey)key, out var tValue) ? tValue : (object)null;
            set
            {
                VerifyKey(key);
                VerifyValueType(value);
                SetItem((TKey)key, (TValue)value);
            }
        }

        ICollection IDictionary.Keys => dictionary.Keys.ToList();

        ICollection IDictionary.Values => dictionary.Values.ToList<TValue>();

        public ICollection<TValue> Values => dictionary.Values;

        protected DictionaryBase()
        {
            dictionary = new Dictionary<TKey, TValue>();
        }

        protected DictionaryBase(IDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = dictionary;
        }

        protected DictionaryBase(IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        protected DictionaryBase(int capacity)
        {
            dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        protected DictionaryBase(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            this.dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        protected DictionaryBase(int capacity, IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        public void Add(TKey key, TValue value) => AddItem(key, value);

        protected virtual void AddItem(TKey key, TValue value) => dictionary.Add(key, value);

        public void Clear() => ClearItems();

        protected virtual void ClearItems() => dictionary.Clear();

        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

        static bool IsCompatibleKey(object key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            return key is TKey;
        }

        public bool Remove(TKey key) => RemoveItem(key);

        protected virtual bool RemoveItem(TKey key) => dictionary.Remove(key);

        protected virtual void SetItem(TKey key, TValue value) => dictionary[key] = value;

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair) => AddItem(keyValuePair.Key, keyValuePair.Value);

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair) => dictionary.Contains(keyValuePair);

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => dictionary.CopyTo(array, arrayIndex);

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            if (TryGetValue(keyValuePair.Key, out var tValue) && EqualityComparer<TValue>.Default.Equals(tValue, keyValuePair.Value))
            {
                RemoveItem(keyValuePair.Key);
                return true;
            }

            return false;
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            dictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
        }

        void IDictionary.Add(object key, object value)
        {
            VerifyKey(key);
            VerifyValueType(value);
            AddItem((TKey)key, (TValue)value);
        }

        bool IDictionary.Contains(object key)
        {
            return IsCompatibleKey(key) ? dictionary.ContainsKey((TKey)key) : false;
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new DictionaryEnumerator<TKey, TValue>(dictionary.GetEnumerator());
        }

        void IDictionary.Remove(object key)
        {
            VerifyKey(key);
            Remove((TKey)key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)this).GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        static void VerifyKey(object key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (!(key is TKey))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Keys is of type {0}.", typeof(TKey)), "key");
        }

        static void VerifyValueType(object value)
        {
            if (!(value is TValue) && (value != null || typeof(TValue).GetTypeInfo().IsValueType))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Value is of type {0}.", typeof(TValue)), "value");
        }

        struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : IDictionaryEnumerator, IEnumerator
        {

            readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> enumerator;

            public object Current
            {
                get
                {
                    var current = enumerator.Current;
                    object key = current.Key;
                    current = enumerator.Current;
                    return new DictionaryEntry(key, current.Value);
                }
            }

            public DictionaryEntry Entry => (DictionaryEntry)Current;

            public object Key => Entry.Key;

            public object Value => Entry.Value;

            public DictionaryEnumerator(IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> enumerator)
            {
                this.enumerator = enumerator;
            }

            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            public void Reset()
            {
                enumerator.Reset();
            }

        }

    }

}
