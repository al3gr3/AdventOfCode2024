var lines = File.ReadAllLines("TextFile1.txt");
var robots = lines.Select(ParseRobot).ToList();

var dim = new Point { X = 101, Y = 103 };
var center = new Point { X = dim.X / 2, Y = dim.Y / 2 };

First();

Robot ParseRobot(string value)
{
    var splits = value.Split(new[] { "p=", ",", " v=" }, StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse).ToArray();
    return new Robot
    {
        Pos = new Point { X = splits[0], Y = splits[1] },
        Vector = new Point { X = splits[2], Y = splits[3] },
    };
}

void Second()
{
    var i = 0;
    while (true)
    {
        i++;
        foreach (var robot in robots)
        {
            robot.Pos.Add(dim.MultiplyClone(100000)).Add(robot.Vector);
            robot.Pos.X %= dim.X;
            robot.Pos.Y %= dim.Y;
        }
        Print(robots);
        Console.WriteLine(i);
        Thread.Sleep(30);
    }
}

void Print(List<Robot> robots)
{
    var m = Enumerable.Range(0, dim.Y).Select(y => Enumerable.Range(0, dim.X).Select(x => 0).ToArray()).ToArray();
    foreach (var robot in robots)
        m[robot.Pos.Y][robot.Pos.X]++;

    for (var y = 0; y < dim.Y; y++)
    {
        var s = "";
        for (var x = 0; x < dim.X; x++)
            s += m[y][x] > 0 ? '*' : ' ';
        Console.WriteLine(s);
    }
}

void First()
{
    foreach (var robot in robots)
    {
        robot.Pos.Add(dim.MultiplyClone(100000)).Add(robot.Vector.MultiplyClone(100));
        robot.Pos.X %= dim.X;
        robot.Pos.Y %= dim.Y;
    }

    var groups = robots.GroupBy(x => x.Quadrant(center)).Where(grp => new[] { 1, 3, 5, 7 }.Contains(grp.Key))
        .Select(grp => grp.Count());

    var result = groups.Aggregate(1, (s, n) => s * n);

    Console.WriteLine(result);
}

class Robot
{
    public Point Pos;
    public Point Vector;

    public int Quadrant(Point center)
    {
        var cmp = new Point
        {
            X = this.Pos.X.CompareTo(center.X),
            Y = this.Pos.Y.CompareTo(center.Y)
        };

        if (cmp.IsEqual(new Point { X = 0, Y = 0 }))
            return 0;

        for (int i = 0; i < Point.AllDirections.Length; i++)
            if (cmp.IsEqual(Point.AllDirections[i]))
                return i;
        throw new Exception();
    }
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