var text = File.ReadAllLines("TextFile1.txt");
var isField = true;
var lines = new List<string>();
var moves = "";
Point fish = new Point();
foreach(var line in text)
{
    if (string.IsNullOrEmpty(line))
    {
        isField = false;
        continue;
    }

    if (isField)
    {
        lines.Add(line);
        if (line.Contains('@'))
            fish = new Point { X = line.IndexOf('@'), Y = lines.Count() - 1 };

    }
    else
        moves += line;
}

foreach (var move in moves)
{
    var n = fish.AddClone(FindDirection(move));
    if (lines[n.Y][n.X] == '.')
    {
        SetChar(n, '@');
        SetChar(fish, '.');
        fish = n;
    }
    else if (lines[n.Y][n.X] == '.')
    {

    }
    else if (lines[n.Y][n.X] == 'O')
    {
        var m = n.Clone();
        while ('O' == lines[m.Y][m.X])
            m = m.AddClone(FindDirection(move));
        if (lines[m.Y][m.X] == '.')
        {
            SetChar(n, '@');
            SetChar(fish, '.');
            SetChar(m, 'O');
            fish = n;
        }
    }
}

foreach(var line in lines)
    Console.WriteLine(line);

var result = 0;
for (var y = 0; y < lines.Count(); y++)
    for (var x = 0; x < lines.First().Length; x++)
        if (lines[y][x] == 'O')
            result += 100 * y + x;

Console.WriteLine(result);

void SetChar(Point p, char c) => lines[p.Y] = lines[p.Y][..p.X] + c + lines[p.Y][(p.X + 1)..];

Point FindDirection (char c) => Point.OrtoDirections["^>v<".IndexOf(c)];

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