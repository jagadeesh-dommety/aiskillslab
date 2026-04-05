// See https://aka.ms/new-console-template for more information

using DictionaryDesign.Implementations;

Console.WriteLine("Custom Dictionary Demo");

var dict = new CustomDictionary<string, int>();

// Add some key-value pairs
dict.Add("apple", 1);
dict.Add("banana", 2);
dict.Add("cherry", 3);

Console.WriteLine($"Count: {dict.Count}");

// Access values
Console.WriteLine($"Apple: {dict["apple"]}");

// Check contains
Console.WriteLine($"Contains 'banana': {dict.ContainsKey("banana")}");

// Try get value
int value;
if (dict.TryGetValue("cherry", out value))
{
    Console.WriteLine($"Cherry value: {value}");
}

// Remove
dict.Remove("banana");
Console.WriteLine($"After removing banana, count: {dict.Count}");

// Iterate keys
Console.WriteLine("Keys:");
foreach (var key in dict.Keys)
{
    Console.WriteLine(key);
}

// Iterate values
Console.WriteLine("Values:");
foreach (var val in dict.Values)
{
    Console.WriteLine(val);
}

// Clear
dict.Clear();
Console.WriteLine($"After clear, count: {dict.Count}");
