// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");


async Task<string> DownloadString(string url)
{
    HttpClient httpClient = new HttpClient();
    string content = await httpClient.GetStringAsync(url);
    return content;
}

var numbers = new Dictionary<string,int> {
    {"zero",0},
    {"one",1},
    {"two",2},
    {"three",3},
    {"four",4},
    {"five",5},
    {"six",6},
    {"seven",7},
    {"eight",8},
    {"nine",9},
};

string DeHumanize(string number)
{
    foreach(var kv in numbers) 
        number = number.Replace(kv.Key, kv.Value.ToString());
    return number;
}

// var input = File.ReadAllText("..\\..\\..\\sample.txt");
var input = File.ReadAllText("input.txt");


int total1 = 0;
int total2 = 0;
foreach(var line in input.Split("\r\n")){


    var match1 = Regex.Match(line,"^[^\\d]*(\\d)");
    var match2 = Regex.Match(line,"(\\d)[^\\d]*$");

    Console.Write(line);
    Console.Write(" - ");

    Console.Write(match1.Groups[1].Value);
    Console.Write(match2.Groups[1].Value);
    Console.Write(" : ");
    var text = match1.Groups[1].Value + match2.Groups[1].Value;
    if(text != string.Empty)
        total1 += int.Parse(text);

    var matches = Regex.Matches(line,"(?=(\\d|one|two|three|four|five|six|seven|eight|nine))");
    var first = DeHumanize(matches.First().Groups[1].Value);
    var last = DeHumanize(matches.Last().Groups[1].Value);
    Console.Write(first);
    Console.WriteLine(last);

    total2 += int.Parse(first+last);

}
Console.WriteLine(total1);
Console.WriteLine(total2);
