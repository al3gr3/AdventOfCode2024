var directions = new[]
{
    new Point { Y = -1, X = 0 },
    new Point { Y = 0, X = 1 },
    new Point { Y = 1, X = 0 },
    new Point { Y = 0, X = -1 },
};

var lines = File.ReadAllLines("TextFile1.txt");

var result = 0;
for (int x = 0; x < lines[0].Length; x++)
    for (int y = 0; y < lines.Length; y++)
        result += Flood(new Point { X = x, Y = y });

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
        foreach (var dir in directions)
        {
            var nextdir = next.AddClone(dir);
            if (InBounds(nextdir))
            {
                if (lines[nextdir.Y][nextdir.X] == c)
                    queue.Enqueue(nextdir);
                else if (lines[nextdir.Y][nextdir.X] != ToLower(c))
                    fences++;
            }
            else
                fences++;
        }
        lines[next.Y] = lines[next.Y][..next.X] + ToLower(c) + lines[next.Y][(next.X + 1)..];
        area++;
    }
    return fences * area;
}

char ToLower(char c) => c.ToString().ToLower().First();

bool InBounds(Point p) => 0 <= p.X && p.X < lines[0].Length && 0 <= p.Y && p.Y < lines.Length;

class Point
{
    internal int X;
    internal int Y;

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