var text = File.ReadAllLines("TextFile1.txt");
var isField = true;
var lines = new List<string>();
var moves = "";

foreach (var line in text)
{
    if (string.IsNullOrEmpty(line))
    {
        isField = false;
        continue;
    }

    if (isField)
        lines.Add(line);
    else
        moves += line;
}

static void Print(List<string> ls)
{
    foreach (var line in ls)
        Console.WriteLine(line);
}

static Point FindFish(List<string> ls)
{
    for (var i = 0; i < ls.Count; i++)
        if (ls[i].Contains('@'))
            return new Point { X = ls[i].IndexOf('@'), Y = i };
    throw new Exception();
}

static int Count(List<string> ls)
{
    var result = 0;
    for (var y = 0; y < ls.Count(); y++)
        for (var x = 0; x < ls.First().Length; x++)
            if ("[O".Contains(ls[y][x]))
                result += 100 * y + x;
    return result;
}

var dict = new Dictionary<char, string>
{
    { '#', "##" },
    { 'O', "[]" },
    { '.', ".." },
    { '@', "@." },
};

lines = lines.Select(l => string.Concat(l.Select(c => dict[c]))).ToList();

var fish = FindFish(lines);
Print(lines);
foreach (var move in moves)
{
    //Console.WriteLine(move);
    var dir = FindDirection(move);
    var n = fish.AddClone(dir);
    if (lines[n.Y][n.X] == '.')
    {
        SetChar(n, '@');
        SetChar(fish, '.');
        fish = n;
    }
    else if ("[]".Contains(lines[n.Y][n.X]))
    {
        if ("<>".Contains(move))
        {
            var m = n.Clone();
            while ("[]".Contains(lines[m.Y][m.X]))
                m = m.AddClone(dir);
            if (lines[m.Y][m.X] == '.')
            {
                var s = lines[m.Y];
                if (move == '<')
                    lines[m.Y] = s[..m.X] + s[(m.X + 1)..(n.X + 2)] + '.' + s[(n.X + 2)..]; 
                else
                    lines[m.Y] = s[..(n.X - 1)] + '.' + s[(n.X - 1)..(m.X)]  + s[(m.X + 1)..]; 

                fish = n;
            }
        }
        else
        {
            // vertical movement
            var toMove = new[] { fish.Clone() }.ToList();

            var canMove = true;
            var i = 0;
            while (i < toMove.Count) 
            {
                var cur = toMove[i];
                n = cur.AddClone(dir);
                if (lines[n.Y][n.X] == '#')
                {
                    canMove = false;
                    break;
                }
                else if (lines[n.Y][n.X] == '[')
                {
                    toMove.Add(n);
                    toMove.Add(n.AddClone(FindDirection('>')));
                }
                else if (lines[n.Y][n.X] == ']')
                {
                    toMove.Add(n);
                    toMove.Add(n.AddClone(FindDirection('<')));
                }
                i++;
            }

            if (canMove)
            {
                var oldLines = lines.ToList();
                toMove.ForEach(x => SetChar(x, '.'));
                SetChar(toMove.First().AddClone(dir), '@');
                toMove.Skip(1).ToList().ForEach(x => SetChar(x.AddClone(dir), oldLines[x.Y][x.X]));
                fish.Add(dir);
            }
        }
    }
    //Print(lines);
}


Console.WriteLine(Count(lines));

void SetChar(Point p, char c) => lines[p.Y] = lines[p.Y][..p.X] + c + lines[p.Y][(p.X + 1)..];

Point FindDirection(char c) => Point.OrtoDirections["^>v<".IndexOf(c)];

void First()
{
    var fish = FindFish(lines);

    foreach (var move in moves)
    {
        var n = fish.AddClone(FindDirection(move));
        if (lines[n.Y][n.X] == '.')
        {
            SetChar(n, '@');
            SetChar(fish, '.');
            fish = n;
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
                fish = n;

                SetChar(m, 'O');
            }
        }
    }

    Console.WriteLine(Count(lines));
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