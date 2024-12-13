var lines = File.ReadAllLines("TextFile1.txt");
var taken = 0;
decimal result = 0;
while(taken < lines.Length)
{
    var equations = lines.Skip(taken).Take(4).ToArray();
    taken += 4;

    var (ax, ay) = Parse2Numbers(equations[0]);
    var (bx, by) = Parse2Numbers(equations[1]);
    var (px, py) = Parse2Numbers(equations[2]);
    px += 10000000000000;
    py += 10000000000000;

    var b = (decimal)(py * ax - ay * px) / (ax * by - ay * bx);
    var a = (px - bx * b) / ax;

    if (Math.Round(a) == a && Math.Round(b) == b)
        result += 3 * a + b;
}

Console.WriteLine(result);

(long ax, long ay) Parse2Numbers(string value)
{
    var splits = value.Split(new[] { "Button A: X+", "Button B: X+", ", Y+", "Prize: X=", ", Y=" }, StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse).ToArray();
    return (splits[0], splits[1]);
}