// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;



Console.WriteLine("Hello, World!");


var input = File.ReadAllText("input.txt");
//var input = File.ReadAllText("sample.txt");


int total1 = 0;
int total2 = 0;
var cardMatches = new List<int>();
foreach (var line in input.Split("\r\n"))
{

    var lineMatch = Regex.Match(line, "^Card\\s+(\\d+):(\\s+\\d+)+\\s\\|(\\s+\\d+)+$");

    Console.WriteLine(line);
    var card = lineMatch.Groups[1].Value;
    var winning = lineMatch.Groups[2].Captures.Select(x => int.Parse(x.Value)).ToList();
    var have = lineMatch.Groups[3].Captures.Select(x => int.Parse(x.Value)).ToList();

    var matches = have.Intersect(winning).ToList();
    if (matches.Count > 0)
    {
        var points = (int)Math.Pow(2, matches.Count - 1);
        Console.WriteLine($"Points: {points}");
        total1 += points;
    }

    cardMatches.Add(matches.Count);

}
var cards = cardMatches.Select(matches => (matches, copies: 1)).ToArray();

for (var i = 0; i < cards.Length; i++)
{
    for (int j = 0; j < cards[i].matches; j++)
    {
        cards[i+j+1].copies+= cards[i].copies;
    }
}
total2 = cards.Select(x=>x.copies).Sum();
Console.WriteLine($"Total1: {total1}");
Console.WriteLine($"Total2: {total2}");
