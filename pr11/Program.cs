var stones = File.ReadAllLines("TextFile1.txt").First().Split(' ');
var dict = new Dictionary<string, long>();
Console.WriteLine(f(stones, 0, 25));
Console.WriteLine(f(stones, 0, 75));

long f(IEnumerable<string> sts, int level, int maxLevel) => level == maxLevel
    ? sts.Count()
    : sts.Sum(st =>
    {
        var key = $"{maxLevel - level}|{st}";
        return dict.ContainsKey(key) ? dict[key] : (dict[key] = f(Blink(st), level + 1, maxLevel));
    });

IEnumerable<string> Blink(string x)
{
    if (x == "0")
        return new[] { "1" };
    if (x.Length % 2 == 0)
        return new[] { x[..(x.Length / 2)], long.Parse(x[(x.Length / 2)..]).ToString() };

    return new[] { $"{long.Parse(x) * 2024}" };
}