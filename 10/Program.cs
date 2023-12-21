// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");


 var input = File.ReadAllText("input.txt");
//var input = File.ReadAllText("sample2.txt");

// Read map
var mapLines = new List<Cell[]>();
foreach (var line in input.Split("\r\n"))
{
    mapLines.Add(line.Select(x => new Cell { tile = x }).ToArray());
}

Cell[][] map = mapLines.ToArray();

CellPos FindStart()
{
    for (var y = 0; y < map.Length; y++)
    {
        for (var x = 0; x < map[y].Length; x++)
        {
            if (map[y][x].tile == 'S')
                return new CellPos(x, y);
        }
    }
    throw new DataMisalignedException("No start tile");
}

char getTile(CellPos pos)
{
    if (pos.y < 0 || pos.y > map.Length) return '.';
    var row = map[pos.y];
    if (pos.x < 0 || pos.x > row.Length) return '.';

    return map[pos.y][pos.x].tile;
}

bool isPipe(CellPos pos)
{
    var tile = getTile(pos);
    return (tile == '-' || tile == '|');
}

List<CellPos> ExitsFromStart(CellPos pos)
{
    var result = new List<CellPos>();
    var north = pos with { y = pos.y - 1 };
    if(Exits(north).Contains(pos)) 
        result.Add(north);
    var south = pos with { y = pos.y + 1 };
    if (Exits(south).Contains(pos))
        result.Add(south);
    var west = pos with { x = pos.x - 1 };
    if (Exits(west).Contains(pos))
        result.Add(west);
    var east = pos with { x = pos.x + 1 };
    if (Exits(east).Contains(pos))
        result.Add(east);
    return result;
}

List<CellPos> Exits(CellPos pos)
{
    var tile = getTile(pos);

    var result = new List<CellPos>();
    switch (tile)
    {
        case 'S':
            return ExitsFromStart(pos);
        case 'L':
            result.Add(pos with { y = pos.y - 1 });
            result.Add(pos with { x = pos.x + 1 });
            break;
        case 'J':
            result.Add(pos with { y = pos.y - 1 });
            result.Add(pos with { x = pos.x - 1 });
            break;
        case '7':
            result.Add(pos with { y = pos.y + 1 });
            result.Add(pos with { x = pos.x - 1 });
            break;
        case 'F':
            result.Add(pos with { y = pos.y + 1 });
            result.Add(pos with { x = pos.x + 1 });
            break;
        case '|':
            result.Add(pos with { y = pos.y - 1 });
            result.Add(pos with { y = pos.y + 1 });
            break;
        case '-':
            result.Add(pos with { x = pos.x - 1 });
            result.Add(pos with { x = pos.x + 1 });
            break;
        case '.':
            break;
        default:
            throw new DataMisalignedException("Wut?");
    }

    return result;
}

/// Perform Depth first search of map from start.
/// A Breadth first search would probably be more efficient
void Traverse(CellPos start, int distance)
{
    // Have we visited this cell already?
    if (map[start.y][start.x].distance.HasValue)
    {
        // if we've found a shorter way here, stop
        if (map[start.y][start.x].distance <= distance)
            return;
    }

    // Store distance 
    map[start.y][start.x].distance = distance;
    // visit each exit in turn
    foreach (var pos in Exits(start))
    {
        Traverse(pos, distance + 1);
    }
}

var start = FindStart();
Traverse(start, 0);

int maxDistance = 0;
foreach(var line in map)
{
    foreach(var cell in line)
    {
        if (cell.distance.HasValue)
            Console.Write(cell.distance);
        else Console.Write('.');
        if (cell.distance.HasValue && cell.distance.Value > maxDistance)
            maxDistance = cell.distance.Value;
    }
    Console.WriteLine();
}

Console.WriteLine($"Max distance: {maxDistance}");


record struct CellPos(int x, int y);

class Cell
{
    public char tile;
    public int? distance;
}
