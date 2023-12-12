// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");




//var input = new List<(int time, int distance)>
//{
//    (7,9),
//    (15,40),
//    (30,200),
//};

var input = new List<(int time, int distance)>
{
    (40,215),
    (70,1051),
    (98,2147),
    (79,1005),
};

int waysToBeat(int time, int recordDistance)
{
    int ways = 0;
    for (int buttonSecs = 0; buttonSecs <= time; buttonSecs++)
    {
        var remaining = time - buttonSecs;
        var speed = buttonSecs;
        var distance = remaining * speed;

        if (distance > recordDistance)
            ways++;
    }
    return ways;
}

int total = 1;
foreach(var race in input)
{
    var ways = waysToBeat(race.time, race.distance);
    total *= ways;
}

Console.WriteLine($"Result: {total}");