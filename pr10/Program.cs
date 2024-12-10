var directions = new[]
{
    new Point { Y = -1, X = 0 },
    new Point { Y = 0, X = 1 },
    new Point { Y = 1, X = 0 },
    new Point { Y = 0, X = -1 },
};

var lines = File.ReadAllLines("TextFile1.txt");

var zeroes = new List<Point>();
for (int x = 0; x < lines[0].Length; x++)
    for (int y = 0; y < lines.Length; y++)
        if (lines[y][x] == '0')
            zeroes.Add(new Point { X = x, Y = y });

Console.WriteLine(zeroes.Select(z => Walk(new[] { z })).Sum(wave => wave.Select(p => $"{p.X}|{p.Y}").Distinct().Count()));
Console.WriteLine(Walk(zeroes.ToArray()).Length);

Point[] Walk(Point[] wave)
{
    foreach (var step in Enumerable.Range(1, 9))
        wave = wave.SelectMany(p => directions.Select(dir => p.AddClone(dir)).Where(next => InBounds(next) && $"{lines[next.Y][next.X]}" == $"{step}")).ToArray();
    return wave;
}

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