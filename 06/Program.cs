// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");




//var input = new List<(long time, long distance)>
//{
//    (7,9),
//    (15,40),
//    (30,200),
//};

var input1 = new List<(long time, long distance)>
{
    (40,215),
    (70,1051),
    (98,2147),
    (79,1005),
};

var input2 = new List<(long time, long distance)>
{
    (40709879,215105121471005),
};

long waysToBeat(long time, long recordDistance)
{
    long ways = 0;
    for (long buttonSecs = 0; buttonSecs <= time; buttonSecs++)
    {
        long remaining = time - buttonSecs;
        long speed = buttonSecs;
        long distance = remaining * speed;

        if (distance > recordDistance)
            ways++;
    }
    return ways;
}

long total = 1;
foreach(var race in input1)
{
    var ways = waysToBeat(race.time, race.distance);
    total *= ways;
}

Console.WriteLine($"Result: {total}");

total = 1;
foreach (var race in input2)
{
    var ways = waysToBeat(race.time, race.distance);
    total *= ways;
}

Console.WriteLine($"Result2: {total}");