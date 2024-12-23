var lines = File.ReadAllLines("TextFile1.txt").Select(x => x.Split('-').Order().ToArray());
var dict = new Dictionary<string, List<string>>();

void Add(string key, string val)
{
    if (dict.ContainsKey(key))
        dict[key].Add(val);
    else
        dict[key] = new[] { val }.ToList();
}

foreach (var line in lines)
{
    Add(line[0], line[1]);
    Add(line[1], line[0]);
}
foreach (var key in dict.Keys)
    dict[key] = dict[key].Order().ToList();

var CBC = 0;
var maxStr = "";
clique(dict.Keys.ToList(), 0);
Console.WriteLine($"{maxStr} {CBC}");
Test(maxStr);

void clique(List<string> V, int depth)
{
    if (V.Count == 0)
    {
        if (depth > CBC)
            CBC = depth;
        return;
    }

    var i = 0;
    while (i < V.Count)
    {
        if (depth + V.Count - i <= CBC)
            return;
        
        var Nvi = dict[V[i]]; // neighborhood of Vi
        var nextV = Nvi.Where(vj => V.IndexOf(vj) > i).ToList();
        var before = CBC;
        clique(nextV, depth + 1);
        if (CBC > before && depth == 0)
        {
            var t = Nvi.ToList();
            t.Add(V[i]);
            maxStr = $"{string.Join(',', t.OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase))}";
        }
        i++;
    }
}

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

void Test(string maxStr)
{
    //Console.WriteLine(string.Join(',', new[] { "z", "v" }.OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase)));

    var cliq = maxStr.Split(',');
    foreach (var s in cliq)
    {
        var inter = dict[s].Intersect(cliq).ToList();
        inter.Add(s);

        Console.WriteLine($"{s}: {string.Join(',', inter.OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase))}");
    }
}