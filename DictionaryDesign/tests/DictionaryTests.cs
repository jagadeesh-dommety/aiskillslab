using System;
using System.Collections.Generic;
using NUnit.Framework;
using DictionaryDesign.Implementations;

namespace DictionaryDesign.Tests
{
    [TestFixture]
    public class DictionaryTests
    {
        [Test]
        public void Add_IncreasesCount()
        {
            var dictionary = new CustomDictionary<string, int>();

            dictionary.Add("one", 1);
            dictionary.Add("two", 2);

            Assert.That(dictionary.Count, Is.EqualTo(2));
            Assert.That(dictionary.ContainsKey("one"), Is.True);
            Assert.That(dictionary.ContainsKey("two"), Is.True);
        }

        [Test]
        public void Add_DuplicateKey_ThrowsArgumentException()
        {
            var dictionary = new CustomDictionary<string, int>();
            dictionary.Add("one", 1);

            Assert.Throws<ArgumentException>(() => dictionary.Add("one", 10));
        }

        [Test]
        public void Add_NullKey_ThrowsArgumentNullException()
        {
            var dictionary = new CustomDictionary<string, int>();

            Assert.Throws<ArgumentNullException>(() => dictionary.Add(null!, 1));
        }

        [Test]
        public void TryGetValue_ReturnsValueForExistingKey()
        {
            var dictionary = new CustomDictionary<string, string>();
            dictionary.Add("hello", "world");

            var result = dictionary.TryGetValue("hello", out var value);

            Assert.That(result, Is.True);
            Assert.That(value, Is.EqualTo("world"));
        }

        [Test]
        public void TryGetValue_ReturnsFalseForMissingKey()
        {
            var dictionary = new CustomDictionary<string, string>();

            var result = dictionary.TryGetValue("missing", out var value);

            Assert.That(result, Is.False);
            Assert.That(value, Is.EqualTo(default(string)));
        }

        [Test]
        public void TryGetValue_NullKey_ThrowsArgumentNullException()
        {
            var dictionary = new CustomDictionary<string, int>();

            Assert.Throws<ArgumentNullException>(() => dictionary.TryGetValue(null!, out _));
        }

        [Test]
        public void ContainsKey_ReturnsTrueForExistingKey()
        {
            var dictionary = new CustomDictionary<string, int>();
            dictionary.Add("key", 42);

            Assert.That(dictionary.ContainsKey("key"), Is.True);
        }

        [Test]
        public void ContainsKey_NullKey_ThrowsArgumentNullException()
        {
            var dictionary = new CustomDictionary<string, int>();

            Assert.Throws<ArgumentNullException>(() => dictionary.ContainsKey(null!));
        }

        [Test]
        public void Indexer_GetReturnsValue()
        {
            var dictionary = new CustomDictionary<string, string>();
            dictionary.Add("first", "value");

            Assert.That(dictionary["first"], Is.EqualTo("value"));
        }

        [Test]
        public void Indexer_GetMissingKey_ThrowsKeyNotFoundException()
        {
            var dictionary = new CustomDictionary<string, string>();

            Assert.Throws<KeyNotFoundException>(() => { var _ = dictionary["missing"]; });
        }

        [Test]
        public void Indexer_SetUpdatesExistingKey()
        {
            var dictionary = new CustomDictionary<string, string>();
            dictionary.Add("key", "initial");

            dictionary["key"] = "updated";

            Assert.That(dictionary["key"], Is.EqualTo("updated"));
            Assert.That(dictionary.Count, Is.EqualTo(1));
        }

        [Test]
        public void Indexer_SetAddsNewKey()
        {
            var dictionary = new CustomDictionary<string, int>();

            dictionary["newKey"] = 100;

            Assert.That(dictionary.ContainsKey("newKey"), Is.True);
            Assert.That(dictionary["newKey"], Is.EqualTo(100));
            Assert.That(dictionary.Count, Is.EqualTo(1));
        }

        [Test]
        public void Remove_RemovesExistingKeyAndReturnsTrue()
        {
            var dictionary = new CustomDictionary<string, int>();
            dictionary.Add("a", 1);

            var removed = dictionary.Remove("a");

            Assert.That(removed, Is.True);
            Assert.That(dictionary.ContainsKey("a"), Is.False);
            Assert.That(dictionary.Count, Is.EqualTo(0));
        }

        [Test]
        public void Remove_NonExistingKey_ReturnsFalse()
        {
            var dictionary = new CustomDictionary<string, int>();

            var removed = dictionary.Remove("missing");

            Assert.That(removed, Is.False);
            Assert.That(dictionary.Count, Is.EqualTo(0));
        }

        [Test]
        public void Remove_NullKey_ThrowsArgumentNullException()
        {
            var dictionary = new CustomDictionary<string, int>();

            Assert.Throws<ArgumentNullException>(() => dictionary.Remove(null!));
        }

        [Test]
        public void Clear_EmptiesDictionary()
        {
            var dictionary = new CustomDictionary<string, int>();
            dictionary.Add("one", 1);
            dictionary.Add("two", 2);

            dictionary.Clear();

            Assert.That(dictionary.Count, Is.EqualTo(0));
            Assert.That(dictionary.ContainsKey("one"), Is.False);
            Assert.That(dictionary.ContainsKey("two"), Is.False);
        }

        [Test]
        public void Keys_And_Values_ReturnExistingItems()
        {
            var dictionary = new CustomDictionary<string, int>();
            dictionary.Add("alpha", 1);
            dictionary.Add("beta", 2);

            var keys = new HashSet<string>(dictionary.Keys);
            var values = new HashSet<int>(dictionary.Values);

            Assert.That(keys, Is.EquivalentTo(new[] { "alpha", "beta" }));
            Assert.That(values, Is.EquivalentTo(new[] { 1, 2 }));
        }
    }
}
