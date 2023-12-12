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
//var input = File.ReadAllText("sample.txt");

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
Console.WriteLine($"lowest 1: {lowest}");

var seedRanges = new List<(Int64 seedStart, Int64 seedLength)>();
for (var i = 0; i < seeds.Count; i += 2)
{
    seedRanges.Add((seeds[i], seeds[i + 1]));
}

/* Verify no ranges overlap */
foreach (var map in maps)
{
    var sourceOrder = map.Entries.OrderBy(x => x.SourceStart).ToArray();
    for (var i = 0; i < sourceOrder.Length - 1; i++)
    {
        if (sourceOrder[i].SourceStart + sourceOrder[i].Length - 1 >= sourceOrder[i + 1].SourceStart)
            throw new ArgumentException("Source Ranges overlap");
    }

    var destOrder = map.Entries.OrderBy(x => x.DestStart).ToArray();
    for (var i = 0; i < destOrder.Length - 1; i++)
    {
        if (destOrder[i].DestStart + destOrder[i].Length - 1 >= destOrder[i + 1].DestStart)
            throw new ArgumentException("Dest Ranges overlap");
    }

}
/* No overlapping ranges (whew) */

// Part Two
var final2 = new List<(Int64 seedStart, Int64 seedLength, Int64 location)>();
foreach (var seedRange in seedRanges)
{
    var lowestLocation = GetLowestMapping(seedRange.seedStart, seedRange.seedStart + seedRange.seedLength - 1, "seed");

    final2.Add((seedRange.seedStart, seedRange.seedLength, lowestLocation));
}

var lowest2 = final2.Select(x => x.location).Min();
Console.WriteLine($"lowest 2: {lowest2}");


Int64 GetLowestMapping(Int64 rangeStart, Int64 rangeEnd, string from)
{
    var chunks = new List<(Int64 sourceStart, Int64 sourceEnd, Int64 destStart, Int64 destEnd)>();

    var map = maps.First(x => x.From == from);
    var sourceOrder = map.Entries.OrderBy(x => x.SourceStart).ToArray();
    // Part One, split range into chunks that can be mapped
    foreach (var entry in sourceOrder)
    {
        // map entry is entirely before our range? skip it
        if (rangeStart > entry.SourceEnd)
            continue;   // move to next map entry

        // map entry is entirely after our range?
        if (rangeEnd < entry.SourceStart)
            break; // add 1:1 mapping

        // is there a chunk before this map entry?
        if (rangeStart < entry.SourceStart) {
            // Add chunk before map entry with 1:1 mapping
            chunks.Add((rangeStart, entry.SourceStart-1, rangeStart, entry.SourceStart - 1));

            rangeStart = entry.SourceStart;
            // rangeEnd is unchanged
            // fall out to map the rest of rangeStart-rangeEnd
        }

        if (rangeEnd > entry.SourceEnd)
        {
            // add chunk up to map entry end with dest mapping
            var newrangeStart = rangeStart;
            var newrangeEnd = entry.SourceEnd;
            var resultStart = entry.MapValue(newrangeStart)!.Value;
            var resultEnd = resultStart + (newrangeEnd - newrangeStart);
            chunks.Add((newrangeStart, newrangeEnd, resultStart, resultEnd));

            // continue looking for the rest
            rangeStart = entry.SourceEnd + 1;
            // rangeEnd is unchanged
            continue;   // move to next map entry
        }

        // add chunk for rangeStart-rangeEnd with dest mapping
        var resultStart2 = entry.MapValue(rangeStart)!.Value;
        var resultEnd2 = resultStart2 + (rangeEnd - rangeStart);
        chunks.Add((rangeStart, rangeEnd, resultStart2, resultEnd2));
        rangeStart = rangeEnd + 1;
        break; // skip any remaining map entries

    }

    if(rangeEnd - rangeStart >= 0) 
    {
        // add chunk for rangeStart-rangeEnd with 1:1 mapping
        chunks.Add((rangeStart, rangeEnd, rangeStart, rangeEnd));
    }

    if (map.To == "location")
    {
        // End recusion and return the lowest destination mapping of our chunks
        var lowestLocation = chunks.Select(x => x.destStart).Min();
        return lowestLocation;
    }

    // Part 2. for each chunk, recurse and return lowest result
    var results = new List<Int64>();
    foreach(var chunk in chunks)
    {
        results.Add(GetLowestMapping(chunk.destStart, chunk.destEnd, map.To));
    }

    var lowestresult = results.Min();
    return lowestresult;
}


class MapEntry
{
    public Int64 DestStart { get; set; }
    public Int64 SourceStart { get; set; }
    public Int64 Length { get; set; }
    public Int64 SourceEnd { get => SourceStart + Length - 1; }
    public Int64 DestEnd { get => DestStart + Length - 1; }

    public Int64? MapValue(Int64 value)
    {
        if (value >= SourceStart && value <= SourceEnd)
        {
            return value - (SourceStart - DestStart);
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