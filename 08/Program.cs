// See https://aka.ms/new-console-template for more information

using System.Linq;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");



var input = File.ReadAllText("input.txt");
//var input = File.ReadAllText("sample2.txt");

var treeData = new List<TreeEntry>();
string instructions = "";
foreach (var line in input.Split("\r\n"))
{
    if (string.IsNullOrEmpty(line)) continue;

    if (!line.Contains('='))
    {
        instructions = line;
        continue;
    }

    var match = Regex.Match(line, "^(\\w{3}) = \\((\\w{3}), (\\w{3})\\)$");

    treeData.Add(new TreeEntry(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value));

}

/* Part 1 */
int instructionCounter = 0;
long stepCounter = 0;
var currentNode = treeData.First(x => x.Id == "AAA");
while (true)
{
    var instruction = instructions[instructionCounter++];
    if (instructionCounter >= instructions.Length) instructionCounter = 0;

    var nextId = (instruction == 'L') ? currentNode.Left : currentNode.Right;

    currentNode = treeData.First(x => x.Id == nextId);
    stepCounter++;

    if (currentNode.Id == "ZZZ")
        break;
}

Console.WriteLine($"Steps: {stepCounter}");

/* Part 2 */
instructionCounter = 0;
var currentNodes = treeData.FindAll(x => x.Id.EndsWith('A'));
stepCounter = 0;
var nodeSteps = new long?[currentNodes.Count];
while (true)
{
    var instruction = instructions[instructionCounter++];
    if (instructionCounter >= instructions.Length) instructionCounter = 0;

    for (var i = 0; i < currentNodes.Count; i++)
    {
        var node = currentNodes[i];

        if (node.Id.EndsWith('Z'))
        {
            if (!nodeSteps[i].HasValue)
            {
                nodeSteps[i] = stepCounter;
            }
            continue;
        }

        var nextId = (instruction == 'L') ? node.Left : node.Right;
        currentNodes[i] = treeData.First(x => x.Id == nextId);
    }

    stepCounter++;

    // Finished if all nodes have ID that ends in Z
    if (!nodeSteps.AsEnumerable().Any(x=>!x.HasValue))
        break;
}

static long LCM(long[] numbers)
{
    return numbers.Aggregate(lcm);
}
static long lcm(long a, long b)
{
    return Math.Abs(a * b) / GCD(a, b);
}
static long GCD(long a, long b)
{
    return b == 0 ? a : GCD(b, a % b);
}

var total2 = LCM(nodeSteps.Select(x=>x.Value).ToArray());

Console.WriteLine($"Steps2: {total2}");


class TreeEntry
{
    public TreeEntry(string id, string left, string right)
    {
        Id = id;
        Left = left;
        Right = right;
    }
    public string Id { get; set; }
    public string Left { get; set; }
    public string Right { get; set; }
}
