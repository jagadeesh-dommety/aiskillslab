using System.Collections.Generic;

namespace DictionaryDesign.Interfaces
{
    public interface IMyDictionary<TKey, TValue>
    {
        void Add(TKey key, TValue value);
        bool Remove(TKey key);
        bool TryGetValue(TKey key, out TValue value);
        bool ContainsKey(TKey key);
        int Count { get; }
        TValue this[TKey key] { get; set; }
        void Clear();
        IEnumerable<TKey> Keys { get; }
        IEnumerable<TValue> Values { get; }
    }
}