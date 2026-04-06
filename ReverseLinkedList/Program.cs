using ReverseLinkedList;

var list = new ReversibleLinkedList<int>();

// Read numbers from million.txt
var content = File.ReadAllText("/workspaces/aiskillslab/ReverseLinkedList/million.txt");
var numbers = content.Split(',', StringSplitOptions.RemoveEmptyEntries);

foreach (var numStr in numbers)
{
    if (int.TryParse(numStr.Trim(), out var number))
    {
        list.AddLast(number);
    }
}

Console.WriteLine($"Loaded {list.Count} numbers from file.");
Console.WriteLine("Original list (first 20):");
Console.WriteLine(string.Join(" -> ", list.AsEnumerable().Take(20)));

list.Reverse();

Console.WriteLine("Reversed list (first 20):");
Console.WriteLine(string.Join(" -> ", list.AsEnumerable().Take(20)));

