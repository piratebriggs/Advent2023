// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;


Console.WriteLine("Hello, World!");


var input = File.ReadAllText("input.txt");
//var input = File.ReadAllText("sample.txt");

int total1 = 0;
int total2 = 0;
foreach (var line in input.Split("\r\n")){
    var gameMatch = Regex.Match(line,"^Game (\\d+):");
    var cubesMatch = Regex.Matches(line,"\\s(\\d+)\\s(blue|green|red|)");

//    Console.WriteLine(line);
    var game = int.Parse(gameMatch.Groups[1].Value);
    Console.Write(game);
    int maxRed = 0;
    int maxGreen = 0;
    int maxBlue = 0;
    bool possible = true;
    foreach (Match cubes in cubesMatch)
    {
        var num = int.Parse( cubes.Groups[1].Value);
        var colour = cubes.Groups[2].Value;
        switch (colour)
        {
            case "red":
                if(num > 12)
                {
                    possible = false;
                }
                if(num>maxRed) maxRed = num;
                break;
            case "green":
                if (num > 13)
                {
                    possible = false;
                }
                if (num > maxGreen) maxGreen = num;
                break;
            case "blue":
                if (num > 14)
                {
                    possible = false;
                }
                if (num > maxBlue) maxBlue = num;
                break;
        }
    }
    if (possible)
    {
        Console.Write("impossible");
        total1 += game;
    }
    total2 += maxRed * maxGreen * maxBlue;

    Console.WriteLine();
}
Console.WriteLine($"Total1: {total1}");
Console.WriteLine($"Total2: {total2}");
