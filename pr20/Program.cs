var lines = File.ReadAllLines("TextFile1.txt");
var height = lines.Length;
var width = lines.First().Length;

var dist = Enumerable.Range(1, height).Select(x => Enumerable.Range(1, width).Select(c => int.MaxValue).ToArray()).ToArray();
var queue = Enumerable.Range(0, height).SelectMany(y => Enumerable.Range(0, width).Select(x => new Point { X = x, Y = y })).ToList();

var start = Find('S');
var end = Find('E');

Dijkstra();

Console.WriteLine("Dijkstra done");

FindCheats(2);
FindCheats(20);

bool InBounds(Point p) => 0 <= p.X && p.X < lines[0].Length && 0 <= p.Y && p.Y < lines.Length;

Point Find(char c)
{
    for (var i = 0; i < lines.Length; i++)
        if (lines[i].Contains(c))
            return new Point { X = lines[i].IndexOf(c), Y = i };
    throw new Exception();
}

void Dijkstra()
{
    dist[start.Y][start.X] = 0;

    while (queue.Any())
    {
        var u = queue.Aggregate(queue.First(), (min, n) => dist[min.Y][min.X] > dist[n.Y][n.X] ? n : min);
        queue.Remove(u);
        if (dist[u.Y][u.X] == int.MaxValue)
            break;

        var newPoses = Point.OrtoDirections.Select(x => u.AddClone(x)).Where(x => lines[x.Y][x.X] != '#').ToArray();

        foreach (var newPos in newPoses)
        {
            var v = queue.FirstOrDefault(x => x.IsEqual(newPos));
            if (v != null)
            {
                var alt = dist[u.Y][u.X] + 1;

                if (alt < dist[v.Y][v.X])
                    dist[v.Y][v.X] = alt;
            }
        }
    }
}

void FindCheats(int pico)
{
    var wins = new List<int>();
    for (var y = 0; y < height; y++)
        for (var x = 0; x < height; x++)
        {
            var u = new Point { X = x, Y = y };
            if (dist[u.Y][u.X] == int.MaxValue)
                continue;

            for (var yy = 0; yy < height; yy++)
                for (var xx = 0; xx < height; xx++)
                {
                    var newPos = new Point { X = xx, Y = yy };
                    if (dist[newPos.Y][newPos.X] == int.MaxValue)
                        continue;
                    var distance = newPos.Distance(u);
                    if (distance <= pico)
                    {
                        var diff = Math.Abs(dist[u.Y][u.X] - dist[newPos.Y][newPos.X]);
                        if (diff > distance)
                            wins.Add(diff - distance);
                    }
                }
        }

    //wins.Where(x => x >= 50).GroupBy(x => x).OrderBy(grp => grp.Key).ToList().ForEach(grp => Console.WriteLine($"There are {grp.Count() / 2} cheats that save {grp.Key} picoseconds"));
    Console.WriteLine(wins.Count(x => x >= 100) / 2);
}

class Point
{
    internal int X;
    internal int Y;

    internal static Point[] OrtoDirections => new[]
    {
        new Point { Y = -1, X = 0 },
        new Point { Y = 0, X = 1 },
        new Point { Y = 1, X = 0 },
        new Point { Y = 0, X = -1 },
    };

    internal static Point[] AllDirections => new[]
    {
        new Point { Y = 1, X = 0 },
        new Point { Y = 1, X = 1 },
        new Point { Y = 0, X = 1 },
        new Point { Y = -1, X = 1 },
        new Point { Y = -1, X = 0 },
        new Point { Y = -1, X = -1 },
        new Point { Y = 0, X = -1 },
        new Point { Y = 1, X = -1 },
    };

    internal Point Add(Point point)
    {
        this.X += point.X;
        this.Y += point.Y;
        return this;
    }

    internal bool IsEqual(Point p) => this.X == p.X && this.Y == p.Y;

    internal Point AddClone(Point point) => this.Clone().Add(point);

    internal Point Clone() => new Point { X = this.X, Y = this.Y };

    internal Point Multiply(int i)
    {
        this.X *= i;
        this.Y *= i;
        return this;
    }

    internal Point MultiplyClone(int i) => this.Clone().Multiply(i);

    internal int Distance(Point u)
    {
        var diff = this.AddClone(u.MultiplyClone(-1));
        return Math.Abs(diff.X) + Math.Abs(diff.Y);
    }
}