using System;
using System.Collections.Generic;
using System.Threading;
using DictionaryDesign.Interfaces;

namespace DictionaryDesign.Implementations
{
    public class CustomDictionary<TKey, TValue> : IMyDictionary<TKey, TValue>
    {
        private const int InitialCapacity = 16;
        private LinkedList<KeyValuePair<TKey, TValue>>[] buckets;
        private int count;

        public CustomDictionary()
        {
            buckets = new LinkedList<KeyValuePair<TKey, TValue>>[InitialCapacity];
            count = 0;
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            int hash = Math.Abs(key!.GetHashCode()) % buckets.Length;
            if (buckets[hash] == null)
            {
                buckets[hash] = new LinkedList<KeyValuePair<TKey, TValue>>();
            }

            foreach (var kvp in buckets[hash]!)
            {
                if (kvp.Key!.Equals(key))
                {
                    throw new ArgumentException("An item with the same key has already been added.");
                }
            }

            buckets[hash].AddLast(new KeyValuePair<TKey, TValue>(key, value));
            count++;
        }

        public bool Remove(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            int hash = Math.Abs(key!.GetHashCode()) % buckets.Length;
            if (buckets[hash] != null)
            {
                var node = buckets[hash]!.First;
                while (node != null)
                {
                    if (node.Value.Key!.Equals(key))
                    {
                        buckets[hash]!.Remove(node);
                        count--;
                        return true;
                    }
                    node = node.Next;
                }
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            int hash = Math.Abs(key!.GetHashCode()) % buckets.Length;
            if (buckets[hash] != null)
            {
                foreach (var kvp in buckets[hash]!)
                {
                    if (kvp.Key!.Equals(key))
                    {
                        value = kvp.Value;
                        return true;
                    }
                }
            }
            value = default!;
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            TValue value;
            return TryGetValue(key, out value);
        }

        public int Count => count;

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (TryGetValue(key, out value))
                {
                    return value;
                }
                throw new KeyNotFoundException();
            }
            set
            {
                int hash = Math.Abs(key!.GetHashCode()) & % buckets.Length;
                if (buckets[hash] != null)
                {
                    var node = buckets[hash]!.First;
                    while (node != null)
                    {
                        if (node.Value.Key!.Equals(key))
                        {
                            node.Value = new KeyValuePair<TKey, TValue>(key, value);
                            return;
                        }
                        node = node.Next;
                    }
                }
                Add(key, value);
            }
        }

        public void Clear()
        {
            buckets = new LinkedList<KeyValuePair<TKey, TValue>>[InitialCapacity];
            count = 0;
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                foreach (var bucket in buckets)
                {
                    if (bucket != null)
                    {
                        foreach (var kvp in bucket)
                        {
                            yield return kvp.Key;
                        }
                    }
                }
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (var bucket in buckets)
                {
                    if (bucket != null)
                    {
                        foreach (var kvp in bucket)
                        {
                            yield return kvp.Value;
                        }
                    }
                }
            }
        }
    }
}