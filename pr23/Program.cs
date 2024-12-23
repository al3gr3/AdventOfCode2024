var lines = File.ReadAllLines("TextFile1.txt").Select(x => x.Split('-').Order().ToArray());

First();

void First()
{
    var threes = lines.SelectMany(two =>
    {
        var thirds = lines.Where(p => p[0] == two[0] && lines.Any(ll => ll[0] == p[1] && ll[1] == two[1]))
            .Select(x => x[1]);
        return thirds.Select(x => string.Concat(new[] { two[0], x, two[1] }.Order()));
    }).ToList();
    Console.WriteLine(threes.Count(x => x[0] == 't' || x[2] == 't' || x[4] == 't'));
}
