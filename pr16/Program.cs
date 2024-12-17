var lines = File.ReadAllLines("TextFile1.txt");
var height = lines.Length;
var width = lines.First().Length;

var prev = new Dictionary<string, List<Position>>();

var dist = Enumerable.Range(1, height).Select(x => Enumerable.Range(1, width).Select(c => new[] { int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue }).ToArray()).ToArray();
var queue = Enumerable.Range(0, height).SelectMany(y => Enumerable.Range(0, width)
    .SelectMany(x => new[] { 0, 1, 2, 3}.Select(d => new Position { Dir = d, P = new Point { X = x, Y = y } }))).ToList();

Dijkstra();

var result = dist[1][width - 2].Min();
Console.WriteLine(result);

var back = new Queue<Position>();
var all = new List<Position>();

new[] { 0, 1, 2, 3 }.Where(x => dist[1][width - 2][x] == result).ToList().ForEach(d => back.Enqueue(new Position { P = new Point { X = width - 2, Y = 1 }, Dir = d }));

while (back.Any())
{
    var b = back.Dequeue();
    all.Add(b);
    if (prev.ContainsKey(b.Represent))
        foreach (var p in prev[b.Represent])
            if (!all.Any(z => z.IsEqual(p)))
                back.Enqueue(p);
}
Console.WriteLine(all.GroupBy(p => $"{p.P.X}|{p.P.Y}").Count());

void Dijkstra()
{
    dist[height - 2][1][1] = 0;

    while (queue.Any())
    {
        var u = queue.Aggregate(queue.First(), (min, n) => dist[min.P.Y][min.P.X][min.Dir] > dist[n.P.Y][n.P.X][n.Dir] ? n : min);
        queue.Remove(u);
        if (dist[u.P.Y][u.P.X][u.Dir] == int.MaxValue)
            continue;

        var newPoses = new[]
        {
            new Position { P = u.P.AddClone(Point.OrtoDirections[u.Dir]), Dir = u.Dir },
            new Position { P = u.P.Clone(), Dir = (u.Dir + -1 + 4) % 4 },
            new Position { P = u.P.Clone(), Dir = (u.Dir +  1 + 4) % 4 },
        };

        for (int i = 0; i < newPoses.Length; i++)
        {
            var newPos = newPoses[i];
            if (lines[newPos.P.Y][newPos.P.X] != '#')
            {
                var v = queue.FirstOrDefault(x => x.P.X == newPos.P.X && x.P.Y == newPos.P.Y && x.Dir == newPos.Dir);
                if (v != null)
                {
                    var alt = dist[u.P.Y][u.P.X][u.Dir] + (i == 0 ? 1 : 1000);

                    if (alt == dist[v.P.Y][v.P.X][v.Dir])
                    {
                        var key = v.Represent;
                        if (prev.ContainsKey(key))
                            prev[key].Add(u);
                        else
                            prev[key] = new[] { u }.ToList();
                    }
                    else if (alt < dist[v.P.Y][v.P.X][v.Dir])
                    {
                        dist[v.P.Y][v.P.X][v.Dir] = alt;
                        prev[v.Represent] = new[] { u }.ToList();
                    }
                }
            }
        }
    }
}

class Position
{
    internal Point P;
    internal int Dir;

    internal string Represent => $"{P.X}|{P.Y}|{Dir}";

    internal bool IsEqual(Position p) => this.Dir == p.Dir && P.IsEqual(p.P);
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