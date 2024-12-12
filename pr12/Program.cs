var lines = File.ReadAllLines("TextFile1.txt");

var result = 0;
for (int x = 0; x < lines[0].Length; x++)
    for (int y = 0; y < lines.Length; y++)
        result += Flood2(new Point { X = x, Y = y });

Console.WriteLine(result);

int Flood(Point start)
{
    var fences = 0;
    var area = 0;
    var queue = new Queue<Point>();
    queue.Enqueue(start);
    while (queue.Any())
    {
        var next = queue.Dequeue();
        var c = lines[next.Y][next.X];
        if (ToLower(c) == c)
            continue;
        foreach (var nextdir in Point.OrtoDirections.Select(next.AddClone))
        {
            if (InBounds(nextdir) && lines[nextdir.Y][nextdir.X] == c)
                queue.Enqueue(nextdir);
            
            if (!InBounds(nextdir) || !new[] { c, ToLower(c)}.Contains(lines[nextdir.Y][nextdir.X]))
                fences++;
        }
        lines[next.Y] = lines[next.Y][..next.X] + ToLower(c) + lines[next.Y][(next.X + 1)..];
        area++;
    }
    return fences * area;
}

int Flood2(Point start)
{
    var corners = 0;
    var area = 0;
    var queue = new Queue<Point>();
    queue.Enqueue(start);
    while (queue.Any())
    {
        var next = queue.Dequeue();
        var c = lines[next.Y][next.X];
        if (ToLower(c) == c)
            continue;
        foreach (var nextdir in Point.OrtoDirections.Select(next.AddClone))
        {
            if (InBounds(nextdir) && lines[nextdir.Y][nextdir.X] == c)
                queue.Enqueue(nextdir);
        }
        foreach(var diagIndex in new[] { 1, 3, 5, 7 })
        {
            var diag = next.AddClone(Point.AllDirections[diagIndex]);
            if (!InBounds(diag) || !new[] { c, ToLower(c) }.Contains(lines[diag.Y][diag.X]))
            {
                // outer
                var others = new[] { 1, -1 }
                    .Select(x => (x + diagIndex + 8) % 8)
                    .Select(i => next.AddClone(Point.AllDirections[i]))
                    .Count(p => !InBounds(p) || !new[] { c, ToLower(c) }.Contains(lines[p.Y][p.X]));

                // inner
                var others2 = new[] { 1, -1 }
                    .Select(x => (x + diagIndex + 8) % 8)
                    .Select(i => next.AddClone(Point.AllDirections[i]))
                    .Count(p => InBounds(p) && new[] { c, ToLower(c) }.Contains(lines[p.Y][p.X]));
                if (others == 2 || others2 == 2)
                    corners++;
            }

            /*
            AAAAAA
            AAABBA
            AAABBA
            ABBAAA
            ABBAAA
            AAAAAA
            */
            if (InBounds(diag) && new[] { c, ToLower(c) }.Contains(lines[diag.Y][diag.X]))
            {
                var others2 = new[] { 1, -1 }
                    .Select(x => (x + diagIndex + 8) % 8)
                    .Select(i => next.AddClone(Point.AllDirections[i]))
                    .Count(p => InBounds(p) && !new[] { c, ToLower(c) }.Contains(lines[p.Y][p.X]));
                if (others2 == 2)
                    corners++;
            }
        }
        lines[next.Y] = lines[next.Y][..next.X] + ToLower(c) + lines[next.Y][(next.X + 1)..];
        area++;
    }
    return corners * area;
}

char ToLower(char c) => c.ToString().ToLower().First();

bool InBounds(Point p) => 0 <= p.X && p.X < lines[0].Length && 0 <= p.Y && p.Y < lines.Length;

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