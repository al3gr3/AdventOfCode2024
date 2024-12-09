var lines = File.ReadAllLines("TextFile1.txt");

var antennas = new List<Point>();
for (int x = 0; x < lines[0].Length; x++)
    for (int y = 0; y < lines.Length; y++)
        if (lines[y][x] != '.')
            antennas.Add(new Point { X = x, Y = y, C = lines[y][x] });

First();

bool InBounds(Point p) => 0 <= p.X && p.X < lines[0].Length && 0 <= p.Y && p.Y < lines.Length;

void First()
{
    var antinodes = Enumerable.Range(0, lines.Length).Select(y => Enumerable.Range(0, lines[0].Length).Select(x => false).ToArray()).ToArray();

    antennas.GroupBy(p => p.C).ToList().ForEach(grp =>
    {
        foreach (var left in grp)
            foreach (var right in grp)
                if (!left.IsEqual(right))
                {
                    var vector = left.MultiplyClone(-1).AddClone(right);
                    var rr = right.AddClone(vector);
                    var ll = left.AddClone(vector.MultiplyClone(-1));

                    if (InBounds(rr))
                        antinodes[rr.Y][rr.X] = true;

                    if (InBounds(ll))
                        antinodes[ll.Y][ll.X] = true;
                }
    });

    Console.WriteLine(antinodes.Sum(y => y.Count(c => c)));
}

class Point
{
    internal char C;
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