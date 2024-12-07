var lines = File.ReadAllLines("TextFile1.txt");

Console.WriteLine(lines.Sum(x => Do(x, First)));
Console.WriteLine(lines.Sum(x => Do(x, Second)));

long[] First(long r, long t) => new[] { r + t, r * t };
long[] Second(long r, long t) => new[] { r + t, r * t, long.Parse($"{r}{t}") };

long Do(string x, Func<long, long, long[]> f)
{
    var splits = x.Split(new[] { ':', ' '}, StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x));
    var total = splits.First();

    var results = new[] { splits.Skip(1).First() }.ToList();

    foreach(var t in splits.Skip(2))
        results = results.SelectMany(r => f(r, t)).ToList();

    return results.Any(r => r == total) ? total : 0;    
}