var lines = File.ReadAllLines("TextFile1.txt");
var obstacles = lines.Select(line =>
{
    var splits = line.Split(',').Select(int.Parse).ToArray();
    return new Point { X = splits[0], Y = splits[1] };
}).ToArray();

var height = 71;
var width = 71;

var lb = 1024;
var rb = lines.Length;

while (lb < rb)
{
    var n = (lb + rb) / 2;
    if (IsReachableDijkstra(n))
        lb = n;
    else
        rb = n - 1;

    Console.WriteLine($"{lb} {rb}");
}
Console.WriteLine(lines[lb]);

bool InBounds(Point p) => 0 <= p.X && p.X < width && 0 <= p.Y && p.Y < height;

bool IsReachableDijkstra(int amount)
{
    var dist = Enumerable.Range(1, height).Select(x => Enumerable.Range(1, width).Select(c => int.MaxValue).ToArray()).ToArray();
    var queue = Enumerable.Range(0, height).SelectMany(y => Enumerable.Range(0, width).Select(x => new Point { X = x, Y = y })).ToList();

    dist[0][0] = 0;

    while (queue.Any())
    {
        var u = queue.Aggregate(queue.First(), (min, n) => dist[min.Y][min.X] > dist[n.Y][n.X] ? n : min);
        queue.Remove(u);
        if (dist[u.Y][u.X] == int.MaxValue)
            continue;

        var newPoses = Point.OrtoDirections.Select(x => u.AddClone(x)).Where(InBounds).ToArray();

        foreach (var newPos in newPoses)
        {
            if (!obstacles.Take(amount).Any(x => x.IsEqual(newPos)))
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

    var result = dist[height - 1][width - 1];

    return result < int.MaxValue;
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
}