var stones = File.ReadAllLines("TextFile1.txt").First().Split(' ');

var dict = new Dictionary<string, long>();
Console.WriteLine(f(stones, 0));

foreach (var step in Enumerable.Range(1, 25))
    stones = stones.SelectMany(Blink).ToArray();
Console.WriteLine(stones.Length);

long f(IEnumerable<string> sts, int level)
{
    if (level == 75)
        return sts.Count();

    return sts.GroupBy(x => x).Sum(grp =>
    {
        var key = $"{level}|{grp.Key}";
        return grp.Count() * (dict.ContainsKey(key) ? dict[key] : (dict[key] = f(Blink(grp.Key), level + 1)));
    });
}

IEnumerable<string> Blink(string x)
{
    if (x == "0")
        return new[] { "1" };
    if (x.Length % 2 == 0)
        return new[] { x[..(x.Length / 2)], long.Parse(x[(x.Length / 2)..]).ToString() };

    return new[] { $"{long.Parse(x) * 2024}" };
}