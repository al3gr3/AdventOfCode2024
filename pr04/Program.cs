var lines = File.ReadAllLines("TextFile1.txt");

Console.WriteLine(First());
Console.WriteLine(Second());

int Second()
{
    var result = 0;
    for (var x = 0; x < lines[0].Length; x++)
        for (var y = 0; y < lines.Length; y++)
            if (lines[y][x] == 'A')
                result += XShapedWords(new Point { X = x, Y = y }).All(w => w == "MAS" || w == "SAM") ? 1 : 0;
    return result;
}

IEnumerable<string> XShapedWords(Point start)
{
    var directions = new[]
    {
        new Point { Y = 1, X = 1 },
        new Point { Y = 1, X = -1 },
    }.ToList();

    foreach (var dir in directions)
        yield return FindLetter(start.AddClone(dir.MultiplyClone(-1))) + FindLetter(start) + FindLetter(start.AddClone(dir));
}

IEnumerable<string> WordsStartingWithX(Point start)
{
    var directions = new[]
    {
        new Point { Y = 1, X = 0 },
        new Point { Y = 0, X = -1 },
        new Point { Y = 0, X = 1 },
        new Point { Y = -1, X = 0 },

        new Point { Y = 1, X = 1 },
        new Point { Y = 1, X = -1 },
        new Point { Y = -1, X = 1 },
        new Point { Y = -1, X = -1 },
    }.ToList();

    foreach (var dir in directions)
    {
        var t = start.Clone();
        var s = "";

        for (var i = 0; i < 4; i++)
        {
            s += FindLetter(t);
            t.Add(dir);
        }
        yield return s;
    }
}

string FindLetter(Point t) => 0 <= t.X && t.X < lines[0].Length && 0 <= t.Y && t.Y < lines.Length ? $"{lines[t.Y][t.X]}" : "";

int First()
{
    var result = 0;
    for (var x = 0; x < lines[0].Length; x++)
        for (var y = 0; y < lines.Length; y++)
            if (lines[y][x] == 'X')
                result += WordsStartingWithX(new Point { X = x, Y = y }).Count(w => w == "XMAS");
    return result;
}

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