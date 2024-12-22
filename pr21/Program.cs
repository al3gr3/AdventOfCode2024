var lines = File.ReadAllLines("TextFile1.txt");
var numeric = new[]
{
    "789",
    "456",
    "123",
    " 0A"
};
var directional = new[]
{
    " ^A",
    "<v>",
};
var dictPermutate = new Dictionary<string, string[]>
{
    { "", new [] { "" } }
};

var result = 0L;

var dict = new Dictionary<string, long>();
foreach (var line in lines)
{
    var next = Dial(line, numeric);
    var min = next.Min(n => ShortestLength(n, 25, 1));
    Console.WriteLine($"{line} {min}");
    result += (min * int.Parse(line[..3]));
}

Console.WriteLine(result);

long ShortestLength(string code, int maxDepth, int curDepth)
{
    var key = $"{code}|{curDepth}";
    if (dict.ContainsKey(key))
        return dict[key];

    var res = 0L;

    var pos = Find('A', directional);
    foreach (var c in code)
    {
        if (curDepth == maxDepth)
            res += FindMoves(directional, pos, c).First().Length;
        else
        {
            var permutations = FindMoves(directional, pos, c);
            res += permutations.Min(a => ShortestLength(a, maxDepth, curDepth + 1));
        }
        pos = Find(c, directional);
    }

    return dict[key] = res;
}

List<string> Dial(string line, string[] dial)
{
    var wave = new List<string>();
    wave.Add("");
    var pos = Find('A', dial);
    foreach (var c in line)
    {
        var permutations = FindMoves(dial, pos, c);

        wave = wave.SelectMany(x => permutations.Select(ppp => x + ppp)).Distinct().ToList();

        pos = Find(c, dial);
    }
    return wave;
}

string[] FindMoves(string[] dial, Point pos, char c)
{
    var nextPos = Find(c, dial);
    var diff = nextPos.AddClone(pos.MultiplyClone(-1));
    var add = "";
    if (diff.X < 0)
        add += Repeat(-1 * diff.X, '<');
    if (diff.X > 0)
        add += Repeat(diff.X, '>');
    if (diff.Y < 0)
        add += Repeat(-1 * diff.Y, '^');
    if (diff.Y > 0)
        add += Repeat(diff.Y, 'v');

    var permutations = Permutate(add).Where(x => IsCheck(dial, pos, x)).Select(x => x + 'A').ToArray();
    return permutations;
}

static bool IsCheck(string[] dial, Point pos, string add)
{
    var checkPos = pos.Clone();
    foreach (var ch in add)
    {
        checkPos.Add(Point.OrdoDirection(ch));
        if (dial[checkPos.Y][checkPos.X] == ' ')
            return false;
    }
    return true;
}

string[] Permutate(string add) => dictPermutate.ContainsKey(add)
    ? dictPermutate[add]
    : dictPermutate[add] = Enumerable.Range(0, add.Length)
        .SelectMany(i => Permutate(add[..i] + add[(i + 1)..]).Select(x => add[i] + x))
        .Distinct().ToArray();

string Repeat(int amount, char c) => string.Concat(Enumerable.Range(0, amount).Select(x => c));

static Point Find(char c, string[] ls)
{
    for (var i = 0; i < ls.Length; i++)
        if (ls[i].Contains(c))
            return new Point { X = ls[i].IndexOf(c), Y = i };
    throw new Exception();
}

public class Point
{
    internal int X;
    internal int Y;

    internal static Point OrdoDirection(char c) => OrtoDirections["^>v<".IndexOf(c)];

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

    internal int Distance(Point u)
    {
        var diff = this.AddClone(u.MultiplyClone(-1));
        return Math.Abs(diff.X) + Math.Abs(diff.Y);
    }
}