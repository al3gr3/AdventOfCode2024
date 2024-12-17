var lines = File.ReadAllLines("TextFile1.txt");
var height = lines.Length;
var width = lines.First().Length;

//var prev = Enumerable.Range(1, height).Select(x => new Point[width]).ToArray();
var dist = Enumerable.Range(1, height).Select(x => Enumerable.Range(1, width).Select(c => new[] { int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue }).ToArray()).ToArray();
var queue = Enumerable.Range(0, height).SelectMany(y => Enumerable.Range(0, width)
    .SelectMany(x => new[] { 0, 1, 2, 3}.Select(d => new Position { Dir = d, P = new Point { X = x, Y = y } }))).ToList();

First();

void First()
{
    dist[height - 2][1][1] = 0;

    while (queue.Any())
    {
        var u = queue.Aggregate(queue.First(), (min, n) => dist[min.P.Y][min.P.X][min.Dir] > dist[n.P.Y][n.P.X][n.Dir] ? n : min);
        queue.Remove(u);
        if (dist[u.P.Y][u.P.X][u.Dir] == int.MaxValue)
            continue;

        var newPos = new Position { P = u.P.AddClone(Point.OrtoDirections[u.Dir]), Dir = u.Dir };

        if (lines[newPos.P.Y][newPos.P.X] != '#')
        {
            var v = queue.FirstOrDefault(x => x.P.X == newPos.P.X && x.P.Y == newPos.P.Y && x.Dir == newPos.Dir);
            if (v != null)
            {
                var alt = dist[u.P.Y][u.P.X][u.Dir] + 1;
                if (alt < dist[v.P.Y][v.P.X][v.Dir])
                {
                    dist[v.P.Y][v.P.X][v.Dir] = alt;
                    //prev[v.Y][v.X] = u.Clone();
                }
            }
        }

        new[] { -1, 1 }.Select(lr => (u.Dir + lr + 4) % 4 ).ToList().ForEach(directionIndex =>
        {
            var newPos = new Position { P = u.P.Clone(), Dir = directionIndex };

            var v = queue.FirstOrDefault(x => x.P.X == newPos.P.X && x.P.Y == newPos.P.Y && x.Dir == newPos.Dir);
            if (v != null)
            {
                var alt = dist[u.P.Y][u.P.X][u.Dir] + 1000;
                if (alt < dist[v.P.Y][v.P.X][v.Dir])
                {
                    dist[v.P.Y][v.P.X][v.Dir] = alt;
                    //prev[v.Y][v.X] = u.Clone();
                }
            }
        });
    }
    Console.WriteLine(dist[2][width - 2].Min() + 1); // 109506 too high
}

class Position
{
    internal Point P;
    internal int Dir;
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