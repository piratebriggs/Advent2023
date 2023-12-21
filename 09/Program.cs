// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");



var input = File.ReadAllText("input.txt");
//var input = File.ReadAllText("sample.txt");

long total1 = 0;
long total2 = 0;
foreach (var line in input.Split("\r\n"))
{

    var values = line.Split(' ').Select(x => int.Parse(x)).ToList();
    // Step 1, calculate histories
    var current = values;
    var history = new List<List<int>>
    {
        current
    };
    while (true)
    {
        var result = DoIt(current);
        if (!result.Any(x => x != 0))
            break;
        history.Add(result);
        current = result;
    }
    // Part 1 Calculate next value in sequence
    int difference = 0;
    for (var i = history.Count - 1; i >= 0; i--)
    {
        var historyItem = history[i];
        var lastVal = historyItem[historyItem.Count - 1];

        var newVal = lastVal + difference;
        historyItem.Add(newVal);

        difference = newVal;
    }

    total1 += history[0][history[0].Count-1];

    // Part 2 Caclulate previous value in sequence
    difference = 0;
    for (var i = history.Count - 1; i >= 0; i--)
    {
        var historyItem = history[i];
        var firstValue = historyItem[0];

        var newVal = firstValue - difference;
        historyItem.Insert(0,newVal);

        difference = newVal;
    }

    total2 += history[0][0];

}
Console.WriteLine($"Total1: {total1}");
Console.WriteLine($"Total2: {total2}");

List<int> DoIt(List<int> input)
{
    var result = new List<int>();
    for (var i = 1; i < input.Count; i++)
    {
        result.Add(input[i] - input[i - 1]);
    }
    return result;
}