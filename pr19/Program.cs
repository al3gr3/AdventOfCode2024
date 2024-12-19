var lines = File.ReadAllLines("TextFile1.txt");
var towels = lines.First().Split(new[] { ", "}, StringSplitOptions.RemoveEmptyEntries);
var todos = lines.Skip(2).ToList();

var dict = new Dictionary<string, long>() { { "", 1 } };

Console.WriteLine(todos.Count(x => CountWays(x) > 0));
Console.WriteLine(todos.Sum(CountWays));

long CountWays(string arg) => towels.Where(x => arg.StartsWith(x)).Select(x => arg[x.Length..])
    .Sum(next => dict.ContainsKey(next)
        ? dict[next]
        : dict[next] = CountWays(next));