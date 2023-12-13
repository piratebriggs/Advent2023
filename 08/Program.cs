﻿// See https://aka.ms/new-console-template for more information

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");



var input = File.ReadAllText("input.txt");
//var input = File.ReadAllText("sample.txt");

var treeData = new List<TreeEntry>();
string instructions = "";
foreach (var line in input.Split("\r\n"))
{
    if(string.IsNullOrEmpty(line)) continue;

    if(!line.Contains('='))
    {
        instructions = line;
        continue;
    }

    var match = Regex.Match(line, "^(\\w{3}) = \\((\\w{3}), (\\w{3})\\)$");

    treeData.Add(new TreeEntry(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value));

}

int instructionCounter = 0;
var currentNode = treeData.First(x=>x.Id == "AAA");
int stepCounter = 0;
while (true)
{
    var instruction = instructions[instructionCounter++];
    if(instructionCounter >= instructions.Length) instructionCounter = 0;

    var nextId = (instruction == 'L') ? currentNode.Left : currentNode.Right;

    currentNode = treeData.First(x => x.Id == nextId);
    stepCounter++;

    if (currentNode.Id == "ZZZ")
        break;
}

Console.WriteLine($"Steps: {stepCounter}");

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
