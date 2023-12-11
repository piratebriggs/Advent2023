// See https://aka.ms/new-console-template for more information
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

var test = new MapEntry
{
    DestStart = 50,
    SourceStart = 98,
    Length = 2,
};

Assert.AreEqual(null, test.MapValue(97));
Assert.AreEqual(50, test.MapValue(98));
Assert.AreEqual(51, test.MapValue(99));
Assert.AreEqual(null, test.MapValue(100));


var input = File.ReadAllText("input.txt");
// var input = File.ReadAllText("sample.txt");

/* Read ALL THE THINGS */

var seeds = new List<Int64>();
var maps = new List<Map>();
Map? currentMap = null;
foreach (var line in input.Split("\r\n"))
{
    if (line.StartsWith("seeds:"))
    {
        var seedsMatch = Regex.Match(line, "^seeds:(\\s+\\d+)+$");
        seeds = seedsMatch.Groups[1].Captures.Select(x => Int64.Parse(x.Value)).ToList();
        continue;
    }

    if (line.EndsWith("map:"))
    {
        var mapMatch = Regex.Match(line, "^(\\w+)-to-(\\w+) map:$");
        var map = new Map(mapMatch.Groups[1].Value, mapMatch.Groups[2].Value);
        maps.Add(map);
        currentMap = map;
        continue;
    }

    if (!string.IsNullOrEmpty(line))
    {
        var entryMatch = Regex.Match(line, "^(\\d+)\\s+(\\d+)\\s+(\\d+)$");
        var map = new MapEntry()
        {
            DestStart = Int64.Parse(entryMatch.Groups[1].Value),
            SourceStart = Int64.Parse(entryMatch.Groups[2].Value),
            Length = Int64.Parse(entryMatch.Groups[3].Value),
        };
        currentMap!.Entries.Add(map);
    }
}

/* Do the things */

var final = new List<(Int64 seed, Int64 location)>();
foreach (var seed in seeds)
{
    var from = "seed";
    var result = seed;

    while (true)
    {
        var map = maps.First(x => x.From == from);
        result = map.MapValue(result);
        if (map.To == "location")
            break;

        from = map.To;
    }

    final.Add((seed, result));
}

var lowest = final.Select(x => x.location).Min();
Console.WriteLine($"lowest: {lowest}");

class MapEntry
{
    public Int64 DestStart { get; set; }
    public Int64 SourceStart { get; set; }
    public Int64 Length { get; set; }

    public Int64? MapValue(Int64 sourceVal)
    {
        if (sourceVal >= SourceStart && sourceVal < SourceStart + Length)
        {
            return sourceVal - (SourceStart - DestStart);
        }
        return null;
    }
}

class Map
{
    public Map(string from, string to)
    {
        this.From = from;
        this.To = to;
        this.Entries = new List<MapEntry>();
    }
    public string From { get; set; }
    public string To { get; set; }
    public List<MapEntry> Entries { get; set; }

    public Int64 MapValue(Int64 sourceVal)
    {
        Int64? result = null;
        foreach (var entry in Entries)
        {
            result = entry.MapValue(sourceVal);
            if (result != null)
                break;
        }
        return result ?? sourceVal;
    }

}