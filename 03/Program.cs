// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;



Console.WriteLine("Hello, World!");


var input = File.ReadAllText("input.txt");
// var input = File.ReadAllText("sample.txt");

char[][] schematic = input.Split("\r\n").Select(x => x.ToArray()).ToArray();

bool IsSymbol(int linePos, int charPos, bool gearsOnly)
{
    if (linePos < 0 || linePos >= schematic.Length)
        return false;

    if (charPos < 0 || charPos >= schematic[linePos].Length)
        return false;

    var ch = schematic[linePos][charPos];
    if (gearsOnly)
    {
        return ch == '*';
    }
    else
    {
        return !Char.IsDigit(ch) && ch != '.';
    }
}

var gears = new List<Gear>();

void ProcessGearNum(int linePos, int charPos, int num)
{
    Console.Write("gear?");
    var existingGear = gears.FirstOrDefault(x => x.linePos == linePos && x.charPos == charPos);
    if (existingGear != null)
    {
        existingGear.numbers.Add(num);
    }
    else
    {
        var gear = new Gear(linePos, charPos);
        gear.numbers.Add(num);
        gears.Add(gear);
    }
}

bool CheckAdjacent(int linePos, int charPos, int num)
{
    bool result = false;
    for (int i = -1; i <= 1; i++)
    {
        for (int j = -1; j <= 1; j++)
        {
            if (IsSymbol(linePos + i, charPos + j, true))
                ProcessGearNum(linePos + i, charPos + j, num);

            if (IsSymbol(linePos + i, charPos + j, false))
                result = true;
        }
    }

    return result;
}


int total1 = 0;
int total2 = 0;
int linePos = 0;
foreach (var line in input.Split("\r\n"))
{

    var numsMatch = Regex.Matches(line, "(\\d+)");

    Console.WriteLine(line);
    foreach (Match nums in numsMatch)
    {
        var num = int.Parse(nums.Groups[1].Value);
        Console.Write(num);

        for (var i = 0; i < nums.Length; i++)
        {
            var charPos = nums.Index + i;
            if (CheckAdjacent(linePos, charPos, num))
            {
                Console.Write("part");
                total1 += num;
                break;
            }
        }

        Console.Write(",");
    }

    linePos += 1;
    Console.WriteLine();
}
Console.WriteLine($"Total1: {total1}");


foreach(var gear in gears)
{
    if(gear.numbers.Count == 2)
    {
        var num1 = gear.numbers.First();
        var num2 = gear.numbers.Last();

        Console.WriteLine($"Gear: {num1},{num2}");
        total2 += num1*num2;
    }
}


Console.WriteLine($"Total2: {total2}");

class Gear
{
    public Gear(int linePos, int charPos)
    {
        this.linePos = linePos;
        this.charPos = charPos;
        numbers = new HashSet<int>();
    }
    public int linePos { get; }
    public int charPos { get; }
    public HashSet<int> numbers;
};
